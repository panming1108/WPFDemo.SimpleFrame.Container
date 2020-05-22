using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DVs.WaveChart
{
    public class DataPoint : Control
    {
        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register(nameof(X), typeof(double), typeof(DataPoint), new PropertyMetadata(OnPositionChanged));

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register(nameof(Y), typeof(double), typeof(DataPoint), new PropertyMetadata(OnPositionChanged));

        public string ToolTipText
        {
            get { return (string)GetValue(ToolTipTextProperty); }
            set { SetValue(ToolTipTextProperty, value); }
        }

        public static readonly DependencyProperty ToolTipTextProperty =
            DependencyProperty.Register(nameof(ToolTipText), typeof(string), typeof(DataPoint));

        public DataPoint()
        {

        }

        public DataPoint(double width, double height, double x, double y)
        {
            Width = width;
            Height = height;
            X = x;
            Y = y;
        }

        private static void OnPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d != null)
            {
                DataPoint point = d as DataPoint;
                if(e.Property == XProperty)
                {
                    point.SetValue(Canvas.LeftProperty, (double)e.NewValue - (point.Width / 2));
                }
                else if(e.Property == YProperty)
                {
                    point.SetValue(Canvas.TopProperty, (double)e.NewValue - (point.Height / 2));
                }
            }
        }
    }
}
