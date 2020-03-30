using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DVs.WaveChart
{
    public class WaveAxis : Control
    {
        private readonly CultureInfo _culture;
        private readonly Typeface _typeface;
        private readonly double _emSize;

        public Dictionary<string, double> ItemsSource
        {
            get { return (Dictionary<string, double>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(Dictionary<string, double>), typeof(WaveAxis));

        public WaveIntervalConverter WaveIntervalConverter
        {
            get { return (WaveIntervalConverter)GetValue(WaveIntervalConverterProperty); }
            set { SetValue(WaveIntervalConverterProperty, value); }
        }

        public static readonly DependencyProperty WaveIntervalConverterProperty =
            DependencyProperty.Register(nameof(WaveIntervalConverter), typeof(WaveIntervalConverter), typeof(WaveAxis), new PropertyMetadata(OnWaveIntervalConverterChanged));

        public bool IsDrawOrdinateAxis
        {
            get { return (bool)GetValue(IsDrawOrdinateAxisProperty); }
            set { SetValue(IsDrawOrdinateAxisProperty, value); }
        }

        public static readonly DependencyProperty IsDrawOrdinateAxisProperty =
            DependencyProperty.Register(nameof(IsDrawOrdinateAxis), typeof(bool), typeof(WaveAxis));

        public bool IsDrawAbscissaAxis
        {
            get { return (bool)GetValue(IsDrawAbscissaAxisProperty); }
            set { SetValue(IsDrawAbscissaAxisProperty, value); }
        }

        public static readonly DependencyProperty IsDrawAbscissaAxisProperty =
            DependencyProperty.Register(nameof(IsDrawAbscissaAxis), typeof(bool), typeof(WaveAxis));

        public WaveAxis()
        {
            _culture = CultureInfo.GetCultureInfo("en-us");
            _typeface = new Typeface("Klavika");
            _emSize = 10d;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if(ItemsSource == null)
            {
                return;
            }
            if(ItemsSource.Count() == 0)
            {
                return;
            }
            if(WaveIntervalConverter == null)
            {
                return;
            }
            Pen pen = new Pen(Brushes.Black, 1);
            drawingContext.DrawLine(pen, new Point(0, 0), new Point(0, ActualHeight));
            drawingContext.DrawLine(pen, new Point(0, 0), new Point(-5, 10));
            drawingContext.DrawLine(pen, new Point(0, 0), new Point(5, 10));
            drawingContext.DrawLine(pen, new Point(0, ActualHeight), new Point(ActualWidth, ActualHeight));
            drawingContext.DrawLine(pen, new Point(ActualWidth, ActualHeight), new Point(ActualWidth - 10, ActualHeight - 5));
            drawingContext.DrawLine(pen, new Point(ActualWidth, ActualHeight), new Point(ActualWidth - 10, ActualHeight + 5));
            if(IsDrawAbscissaAxis)
            {
                //画横坐标
                DrawAbscissaAxis(drawingContext, pen);
            }
            if(IsDrawOrdinateAxis)
            {
                //画纵坐标
                DrawOrdinateAxis(drawingContext, pen);
            }
        }

        /// <summary>
        /// 画纵坐标
        /// </summary>
        /// <param name="drawingContext"></param>
        private void DrawOrdinateAxis(DrawingContext drawingContext, Pen pen)
        {
            var yMapping = WaveIntervalConverter.YAxisIntervalMapping;
            foreach (var item in yMapping)
            {
                drawingContext.DrawLine(pen, new Point(0, item.Value), new Point(5, item.Value));
                var text = new FormattedText(item.Key, _culture, FlowDirection.LeftToRight, _typeface, _emSize, pen.Brush);
                drawingContext.DrawText(text, new Point(-5 - text.Width, item.Value - text.Height / 2));
            }
        }

        /// <summary>
        /// 画横坐标
        /// </summary>
        /// <param name="drawingContext"></param>
        private void DrawAbscissaAxis(DrawingContext drawingContext, Pen pen)
        {
            var xMapping = WaveIntervalConverter.XAxisIntervalMapping;
            foreach (var item in xMapping)
            {
                drawingContext.DrawLine(pen, new Point(item.Value, ActualHeight), new Point(item.Value, ActualHeight - 5));
                var text = new FormattedText(item.Key, _culture, FlowDirection.LeftToRight, _typeface, _emSize, pen.Brush);
                drawingContext.DrawText(text, new Point(item.Value - text.Width / 2, ActualHeight + 5));
            }
        }

        private static void OnWaveIntervalConverterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d != null)
            {
                WaveAxis waveAxis = d as WaveAxis;
                waveAxis.InvalidateVisual();
            }
        }
    }
}
