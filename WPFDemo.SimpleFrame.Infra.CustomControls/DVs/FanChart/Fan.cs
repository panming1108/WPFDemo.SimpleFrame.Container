using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DVs.FanChart
{
    public class Fan : Shape
    {
        public const double ANGLE180 = 2 * 2 * Math.PI / 4;

        internal Ring ParentRing { get; set; }
        internal double StartAngle { get; set; }
        internal double EndAngle => StartAngle + Angle;
        internal double Radius { get; set; }

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register(nameof(Angle), typeof(double), typeof(Fan), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnAngleChanged));

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register(nameof(Radius), typeof(double), typeof(Fan), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        private static void OnAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Fan fan = d as Fan;
            if(fan.ParentRing != null)
            {
                fan.ParentRing.Fans.UpdateFanAngle();
                fan.InvalidateVisual();
            }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                var angle = Angle;
                if (Angle == 360)
                {
                    angle = 359;
                }
                var startAngle = StartAngle * 2 * Math.PI / 360;
                var endAngle = (angle + StartAngle) * 2 * Math.PI / 360;
                var radius = Radius - (StrokeThickness / 2);
                bool isLargeArc = endAngle - startAngle >= ANGLE180;
                PathSegmentCollection pathSegments = new PathSegmentCollection
                {
                    new ArcSegment(Convert2XYPoint(radius, endAngle), new Size(radius, radius), 0, isLargeArc, SweepDirection.Clockwise, true)
                };
                PathFigureCollection pathFigures = new PathFigureCollection()
                {
                    new PathFigure(Convert2XYPoint(radius, startAngle), pathSegments, Angle == 360)
                };
                return new PathGeometry(pathFigures);
            }
        }

        private Point Convert2XYPoint(double radius, double angle)
        {
            return new Point(Radius + radius * Math.Cos(angle), Radius + radius * Math.Sin(angle));
        }
    }
}
