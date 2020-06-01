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
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public double RingThickness
        {
            get { return (double)GetValue(RingThicknessProperty); }
            set { SetValue(RingThicknessProperty, value); }
        }


        public Point CircleCenter
        {
            get { return (Point)GetValue(CircleCenterProperty); }
            set { SetValue(CircleCenterProperty, value); }
        }

        public static readonly DependencyProperty CircleCenterProperty =
            DependencyProperty.Register(nameof(CircleCenter), typeof(Point), typeof(Ring));

        public static readonly DependencyProperty RingThicknessProperty =
            DependencyProperty.Register(nameof(RingThickness), typeof(double), typeof(Ring), new PropertyMetadata(10.0));

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

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
        }
    }
}
