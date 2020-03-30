using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DVs.WaveChart
{
    [TemplatePart(Name = PART_CANVAS, Type = typeof(Canvas))]
    public class LineWave : Control
    {
        private const string PART_CANVAS = "PART_Canvas";
        private Canvas _canvas;

        public Dictionary<string, double> ItemsSource
        {
            get { return (Dictionary<string, double>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(Dictionary<string, double>), typeof(LineWave));

        public WaveIntervalConverter WaveIntervalConverter
        {
            get { return (WaveIntervalConverter)GetValue(WaveIntervalConverterProperty); }
            set { SetValue(WaveIntervalConverterProperty, value); }
        }

        public static readonly DependencyProperty WaveIntervalConverterProperty =
            DependencyProperty.Register(nameof(WaveIntervalConverter), typeof(WaveIntervalConverter), typeof(LineWave), new PropertyMetadata(OnWaveIntervalConverterChanged));

        public bool IsDrawPoint
        {
            get { return (bool)GetValue(IsDrawPointProperty); }
            set { SetValue(IsDrawPointProperty, value); }
        }

        public static readonly DependencyProperty IsDrawPointProperty =
            DependencyProperty.Register(nameof(IsDrawPoint), typeof(bool), typeof(LineWave));

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
            Pen pen = new Pen(Brushes.Black, 1);
            DrawLineWave(drawingContext, pen);
            if(IsDrawPoint)
            {
                DrawPoint(drawingContext, pen);
            }
        }

        private void DrawPoint(DrawingContext drawingContext, Pen pen)
        {
            if(_canvas == null)
            {
                return;
            }
            foreach (var item in ItemsSource)
            {
                Point point = WaveIntervalConverter.DataConverter2Point(item);
                DataPoint dataPoint = new DataPoint(4,4, point.X,point.Y)
                {
                    Width = 4,
                    Height = 4,
                    X = point.X,
                    Y = point.Y,
                    Background = pen.Brush
                };
                _canvas.Children.Add(dataPoint);
            }
        }

        private void DrawLineWave(DrawingContext drawingContext, Pen pen)
        {
            drawingContext.DrawGeometry(Brushes.Black, pen, WaveIntervalConverter.CaculateCurveGeometry());
        }

        private static void OnWaveIntervalConverterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d != null)
            {
                LineWave lineWave = d as LineWave;
                lineWave.InvalidateVisual();
            }
        }
    }
}
