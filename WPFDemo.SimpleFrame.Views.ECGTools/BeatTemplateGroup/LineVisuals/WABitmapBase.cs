using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
	internal abstract class WABitmapBase
	{
		protected readonly WriteableBitmap _writeableBitmap;

		protected readonly EnumReadWriteMode _mode;

		protected readonly byte _stride;

		public BitmapSource ImageSource => _writeableBitmap;

		public WABitmapBase(short width, short height, double dpiX, double dpiY, PixelFormat pixelFormat)
			: this(new WriteableBitmap(width, height, dpiX, dpiY, pixelFormat, null))
		{
		}

		public WABitmapBase(WriteableBitmap writeableBitmap)
			: this(writeableBitmap, EnumReadWriteMode.ReadWrite)
		{
		}

		public WABitmapBase(WriteableBitmap writeableBitmap, EnumReadWriteMode mode)
		{
			_mode = mode;
			_writeableBitmap = writeableBitmap;
			_stride = (byte)(_writeableBitmap.Format.BitsPerPixel >> 3);
		}
	}
}
