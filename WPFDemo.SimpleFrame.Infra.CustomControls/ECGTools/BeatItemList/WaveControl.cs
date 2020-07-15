using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.ECGTools
{
    public class WaveControl : Control
    {
        public Brush PenBrush
        {
            get { return (Brush)GetValue(PenBrushProperty); }
            set { SetValue(PenBrushProperty, value); }
        }
        public Point[] WaveData
        {
            get { return (Point[])GetValue(WaveDataProperty); }
            set { SetValue(WaveDataProperty, value); }
        }


        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(WaveControl), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty PenBrushProperty =
            DependencyProperty.Register(nameof(PenBrush), typeof(Brush), typeof(WaveControl), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty WaveDataProperty =
            DependencyProperty.Register(nameof(WaveData), typeof(Point[]), typeof(WaveControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if(WaveData == null)
            {
                return;
            }
            Pen pen = new Pen(PenBrush, StrokeThickness);
            for (int i = 0; i < WaveData.Count() - 1; i++)
            {
                drawingContext.DrawLine(pen, WaveData[i], WaveData[i + 1]);
            }
        }
    }
}
