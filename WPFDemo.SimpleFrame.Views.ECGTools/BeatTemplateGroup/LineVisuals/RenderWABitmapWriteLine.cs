using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
	public sealed class RenderWABitmapWriteLine : RenderWABitmapWriteBase
	{
		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<PixelPointArrayEx>), typeof(RenderWABitmapWriteLine), new PropertyMetadata(OnItemsSourceChanged));

		public static readonly DependencyProperty RenderingPensProperty = DependencyProperty.Register("RenderingPens", typeof(Dictionary<int, RenderPen>), typeof(RenderWABitmapWriteLine), new FrameworkPropertyMetadata(OnRenderingPensChanged));

		public static readonly DependencyProperty LineModeProperty = DependencyProperty.Register("LineMode", typeof(EnumRenderingLineMode), typeof(RenderWABitmapWriteLine), new PropertyMetadata(OnLineModeChanged));

		private readonly IAntiAlgo _antiAlgo;

		public ObservableCollection<PixelPointArrayEx> ItemsSource
		{
			get
			{
				return (ObservableCollection<PixelPointArrayEx>)GetValue(ItemsSourceProperty);
			}
			set
			{
				SetValue(ItemsSourceProperty, value);
			}
		}

		public Dictionary<int, RenderPen> RenderingPens
		{
			get
			{
				return (Dictionary<int, RenderPen>)GetValue(RenderingPensProperty);
			}
			set
			{
				SetValue(RenderingPensProperty, value);
			}
		}

		public EnumRenderingLineMode LineMode
		{
			get
			{
				return (EnumRenderingLineMode)GetValue(LineModeProperty);
			}
			set
			{
				SetValue(LineModeProperty, value);
			}
		}

		protected override PixelFormat ARGBFormat => PixelFormats.Pbgra32;

		private static void OnItemsSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			RenderWABitmapWriteLine obj2 = (RenderWABitmapWriteLine)obj;
			obj2.ClearAndNewMap(obj2.RenderArea);
		}

		private static void OnRenderingPensChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RenderWABitmapWriteLine obj = (RenderWABitmapWriteLine)d;
			obj.ClearAndNewMap(obj.RenderArea);
		}

		private static void OnLineModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RenderWABitmapWriteLine obj = (RenderWABitmapWriteLine)d;
			obj.ClearAndNewMap(obj.RenderArea);
		}

		public RenderWABitmapWriteLine()
			: base(new PixelAlgo())
		{
			_antiAlgo = new AntiAlgo();
		}

		protected override bool ClearAndNewMapRender(int[] backBuffer, byte stride)
		{
			if (ItemsSource == null || RenderingPens == null || LineMode == EnumRenderingLineMode.None)
			{
				return false;
			}
			foreach (PixelPointArrayEx item in ItemsSource)
			{
				for (int i = 0; i < item.Points.Count - 1; i++)
				{
					switch (LineMode)
					{
						case EnumRenderingLineMode.None:
							throw new Exception("RenderWABitmapWriteLineClearAndNewMapRender");
						case EnumRenderingLineMode.Bresenham:
							_antiAlgo.DrawLineBresenham(backBuffer, base.RenderArea.Width, base.RenderArea.Height, item.Points[i].X, item.Points[i].Y, item.Points[i + 1].X, item.Points[i + 1].Y, RenderingPens[item.Type].Brush);
							break;
						case EnumRenderingLineMode.DDA:
							_antiAlgo.DrawLineDDA(backBuffer, base.RenderArea.Width, base.RenderArea.Height, item.Points[i].X, item.Points[i].Y, item.Points[i + 1].X, item.Points[i + 1].Y, RenderingPens[item.Type].Brush);
							break;
						case EnumRenderingLineMode.OptimizedDDA:
							_antiAlgo.DrawOptimizedDDA(backBuffer, base.RenderArea.Width, base.RenderArea.Height, item.Points[i].X, item.Points[i].Y, item.Points[i + 1].X, item.Points[i + 1].Y, RenderingPens[item.Type].Brush);
							break;
						case EnumRenderingLineMode.OptimizedGuptaSproullAntiAliased:
							_antiAlgo.DrawLineAa(backBuffer, base.RenderArea.Width, base.RenderArea.Height, item.Points[i].X, item.Points[i].Y, item.Points[i + 1].X, item.Points[i + 1].Y, RenderingPens[item.Type].Brush);
							break;
						default:
							throw new Exception("RenderWABitmapWriteLineClearAndNewMapRender");
					}
				}
			}
			return true;
		}

		protected override bool AppendRender(int[] backBuffer, byte stride)
		{
			throw new NotImplementedException();
		}
	}
}
