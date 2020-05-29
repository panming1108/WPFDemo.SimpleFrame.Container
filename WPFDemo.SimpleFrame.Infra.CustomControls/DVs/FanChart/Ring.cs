using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DVs.FanChart
{
    public class Ring : ItemsControl
    {
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(Ring), new FrameworkPropertyMetadata(10.0));

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register(nameof(Radius), typeof(double), typeof(Ring), new FrameworkPropertyMetadata(50.0));

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is Fan;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new Fan();
        }
    }
}
