using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
	public abstract class RenderWABitmapWriteBase : RenderFrameworkElement
	{
		private WABitmapWrite _bmp;

		private IMemory _memory;

		protected override void RenderSizeChanged(PixelRect pixelRect)
		{
			base.Children.Clear();
			_bmp = null;
			RenderBoard renderBoard = new RenderBoard();
			base.Children.Add(renderBoard);
			_bmp = new WABitmapWrite((short)pixelRect.Width, (short)pixelRect.Height, 96.0, 96.0, ARGBFormat, _memory);
			renderBoard.Init(_bmp.ImageSource);
		}

		public RenderWABitmapWriteBase(IMemory memory)
		{
			_memory = memory;
			base.Unloaded += RenderWBBitmapBase_Unloaded;
		}

		private void RenderWBBitmapBase_Unloaded(object sender, RoutedEventArgs e)
		{
			base.Children.Clear();
			_bmp = null;
		}

		protected void ClearAndNewMap(PixelRect pixelRect)
		{
			if (!base.RenderArea.IsEmpty && _bmp != null)
			{
				_bmp.CreateNewMap(pixelRect, ClearAndNewMapRender);
			}
		}

		protected void Append(PixelRect pixelRect)
		{
			if (!base.RenderArea.IsEmpty && _bmp != null)
			{
				_bmp.Append(pixelRect, AppendRender);
			}
		}

		protected abstract bool ClearAndNewMapRender(int[] backBuffer, byte stride);

		protected abstract bool AppendRender(int[] backBuffer, byte stride);
	}
}
