using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DVs.WaveChart
{
    /// <summary>
    /// 直方图
    /// </summary>
    [TemplatePart(Name = PART_CANVAS, Type = typeof(Canvas))]
    public class Histogram : Control
    {
        private const string PART_CANVAS = "PART_Canvas";
        private Canvas _canvas;

        public Dictionary<string, double> ItemsSource
        {
            get { return (Dictionary<string, double>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(Dictionary<string, double>), typeof(Histogram), new PropertyMetadata(OnReDrawing));

        public WaveIntervalConverter WaveIntervalConverter
        {
            get { return (WaveIntervalConverter)GetValue(WaveIntervalConverterProperty); }
            set { SetValue(WaveIntervalConverterProperty, value); }
        }

        public static readonly DependencyProperty WaveIntervalConverterProperty =
            DependencyProperty.Register(nameof(WaveIntervalConverter), typeof(WaveIntervalConverter), typeof(Histogram), new PropertyMetadata(OnReDrawing));

        public double RectBorderThickness
        {
            get { return (double)GetValue(RectBorderThicknessProperty); }
            set { SetValue(RectBorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty RectBorderThicknessProperty =
            DependencyProperty.Register(nameof(RectBorderThickness), typeof(double), typeof(Histogram), new PropertyMetadata(1.0));

        public Brush RectBorderBrush
        {
            get { return (Brush)GetValue(RectBorderBrushProperty); }
            set { SetValue(RectBorderBrushProperty, value); }
        }

        public static readonly DependencyProperty RectBorderBrushProperty =
            DependencyProperty.Register(nameof(RectBorderBrush), typeof(Brush), typeof(Histogram), new PropertyMetadata(OnReDrawing));

        public Brush RectFillBrush
        {
            get { return (Brush)GetValue(RectFillBrushProperty); }
            set { SetValue(RectFillBrushProperty, value); }
        }

        public static readonly DependencyProperty RectFillBrushProperty =
            DependencyProperty.Register(nameof(RectFillBrush), typeof(Brush), typeof(Histogram), new PropertyMetadata(OnReDrawing));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _canvas = GetTemplateChild(PART_CANVAS) as Canvas;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (ItemsSource == null)
            {
                return;
            }
            if (ItemsSource.Count() == 0)
            {
                return;
            }
            if (WaveIntervalConverter == null)
            {
                return;
            }
            DrawRects();
        }

        private void DrawRects()
        {
            if(_canvas == null)
            {
                return;
            }
            _canvas.Children.Clear();
            foreach (var item in ItemsSource)
            {
                Point point = WaveIntervalConverter.DataConverter2Point(item);
                var rectWidth = WaveIntervalConverter.XAxisInterval;
                DataRect rect = new DataRect(rectWidth, ActualHeight - point.Y, point.X, point.Y)
                {
                    Background = RectFillBrush,
                    BorderBrush = RectBorderBrush,
                    BorderThickness = new Thickness(RectBorderThickness),
                    XValue = item.Key,
                    YValue = item.Value
                };
                _canvas.Children.Add(rect);
            }
        }

        private static void OnReDrawing(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d != null)
            {
                Histogram histogram = d as Histogram;
                histogram.InvalidateVisual();
            }
        }
    }
}
