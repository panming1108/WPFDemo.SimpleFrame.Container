using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DVs.WaveChart
{
    public class DataRect : Control
    {
        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register(nameof(X), typeof(double), typeof(DataRect), new PropertyMetadata(OnPositionChanged));

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register(nameof(Y), typeof(double), typeof(DataRect), new PropertyMetadata(OnPositionChanged));

        public string XValue
        {
            get { return (string)GetValue(XValueProperty); }
            set { SetValue(XValueProperty, value); }
        }

        public static readonly DependencyProperty XValueProperty =
            DependencyProperty.Register(nameof(XValue), typeof(string), typeof(DataRect));

        public double YValue
        {
            get { return (double)GetValue(YValueProperty); }
            set { SetValue(YValueProperty, value); }
        }

        public static readonly DependencyProperty YValueProperty =
            DependencyProperty.Register(nameof(YValue), typeof(double), typeof(DataRect));



        public DataRect()
        {

        }

        public DataRect(double width, double height, double x, double y)
        {
            Width = width;
            Height = height;
            X = x;
            Y = y;
        }

        private static void OnPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d != null)
            {
                DataRect rect = d as DataRect;
                if (e.Property == XProperty)
                {
                    rect.SetValue(Canvas.LeftProperty, (double)e.NewValue - (rect.Width / 2));
                }
                else if (e.Property == YProperty)
                {
                    rect.SetValue(Canvas.TopProperty, (double)e.NewValue);
                }
            }
        }
    }
}
