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
        private readonly FanCollection _fans;
        public FanCollection Fans => _fans;
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

        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register(nameof(StartAngle), typeof(double), typeof(Ring), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty RingThicknessProperty =
            DependencyProperty.Register(nameof(RingThickness), typeof(double), typeof(Ring), new PropertyMetadata(10.0));

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register(nameof(Radius), typeof(double), typeof(Ring), new FrameworkPropertyMetadata(50.0, OnRadiusChanged));

        private static void OnRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Ring ring = d as Ring;
            ring.Width = (double)e.NewValue * 2;
            ring.Height = (double)e.NewValue * 2;
            ring.Fans.UpdateFanRadius();
        }

        public Ring()
        {
            _fans = new FanCollection(this);
        }

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
            Fan fan = element as Fan;
            fan.ParentRing = this;
            Fans.Add(fan);
        }
    }
}
