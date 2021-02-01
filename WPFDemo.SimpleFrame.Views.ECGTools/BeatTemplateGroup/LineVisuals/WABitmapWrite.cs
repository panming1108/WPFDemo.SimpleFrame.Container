using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
	internal sealed class WABitmapWrite : WABitmapBase
	{
		private int[] _pixels;

		private IMemory _memory;

		public WABitmapWrite(short width, short height, double dpiX, double dpiY, PixelFormat pixelFormat, IMemory memory)
			: base(width, height, dpiX, dpiY, pixelFormat)
		{
			_memory = memory;
			Copy();
		}

		private void Copy()
		{
			int num = _writeableBitmap.PixelWidth * _writeableBitmap.PixelHeight;
			_pixels = new int[num];
			Memory.Copy(_writeableBitmap.BackBuffer, _pixels, 0, num);
		}

		[TargetedPatchingOptOut("Candidate for inlining across NGen boundaries for performance reasons")]
		public void Clear()
		{
			int[] pixels = new int[_writeableBitmap.PixelWidth * _writeableBitmap.PixelHeight];
			_writeableBitmap.WritePixels(new Int32Rect(0, 0, _writeableBitmap.PixelWidth, _writeableBitmap.PixelHeight), pixels, _writeableBitmap.BackBufferStride, 0);
			_pixels = pixels;
		}

		public void CreateNewMap(PixelRect pixelRect, Func<int[], byte, bool> render)
		{
			if (_mode != EnumReadWriteMode.ReadWrite)
			{
				throw new Exception();
			}
			if (pixelRect.X + pixelRect.Width > _writeableBitmap.PixelWidth || pixelRect.Y + pixelRect.Height > _writeableBitmap.PixelHeight)
			{
				throw new Exception("pixelRect out");
			}
			int[] array = new int[pixelRect.Width * pixelRect.Height];
			if (render(array, _stride))
			{
				int stride = _stride * pixelRect.Width;
				_ = pixelRect.Height;
				_writeableBitmap.WritePixels(new Int32Rect(pixelRect.X, pixelRect.Y, pixelRect.Width, pixelRect.Height), array, stride, 0);
			}
		}

		public void Append(PixelRect pixelRect, Func<int[], byte, bool> render)
		{
			if (_mode != EnumReadWriteMode.ReadWrite)
			{
				throw new Exception();
			}
			if (pixelRect.X + pixelRect.Width > _writeableBitmap.PixelWidth || pixelRect.Y + pixelRect.Height > _writeableBitmap.PixelHeight)
			{
				throw new Exception("pixelRect out");
			}
			int[] array = _memory.Merge(_stride, _writeableBitmap.PixelWidth, _writeableBitmap.PixelHeight, pixelRect, _writeableBitmap.BackBuffer);
			if (render(array, _stride))
			{
				int stride = _stride * pixelRect.Width;
				_ = pixelRect.Height;
				_writeableBitmap.WritePixels(new Int32Rect(pixelRect.X, pixelRect.Y, pixelRect.Width, pixelRect.Height), array, stride, 0);
			}
		}
	}
}
