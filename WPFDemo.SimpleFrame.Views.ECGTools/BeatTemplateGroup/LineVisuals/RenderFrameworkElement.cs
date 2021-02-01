using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
	public abstract class RenderFrameworkElement : Grid
	{
		public static readonly DependencyProperty RenderingSizeCommandProperty = DependencyProperty.Register("RenderingSizeCommand", typeof(ICommand), typeof(RenderFrameworkElement));

		private PixelRect _pixelRect = PixelRect.Empty;

		public ICommand RenderingSizeCommand
		{
			get
			{
				return (ICommand)GetValue(RenderingSizeCommandProperty);
			}
			set
			{
				SetValue(RenderingSizeCommandProperty, value);
			}
		}

		protected abstract PixelFormat ARGBFormat
		{
			get;
		}

		protected PixelRect RenderArea => _pixelRect;

		public RenderFrameworkElement()
		{
			base.IsHitTestVisible = false;
			base.UseLayoutRounding = false;
			base.SnapsToDevicePixels = false;
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			base.OnRenderSizeChanged(sizeInfo);
			if (!(sizeInfo.NewSize.Width <= 0.0) && !(sizeInfo.NewSize.Height <= 0.0) && RenderingSizeCommand != null)
			{
				_pixelRect = new PixelRect((int)sizeInfo.NewSize.Width, (int)sizeInfo.NewSize.Height);
				RenderSizeChanged(_pixelRect);
				RenderingSizeCommand.Execute(_pixelRect);
			}
		}

		protected abstract void RenderSizeChanged(PixelRect pixelRect);
	}
}
