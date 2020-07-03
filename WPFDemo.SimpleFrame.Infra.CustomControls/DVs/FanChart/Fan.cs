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
        internal Ring ParentRing { get; set; }
        internal double StartAngle { get; set; }
        internal double EndAngle => StartAngle + Angle;
        internal double Radius { get; set; }
        public double FanThickness
        {
            get { return (double)GetValue(FanThicknessProperty); }
            set { SetValue(FanThicknessProperty, value); }
        }

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register(nameof(Angle), typeof(double), typeof(Fan), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnAngleChanged));

        public static readonly DependencyProperty FanThicknessProperty =
            DependencyProperty.Register(nameof(FanThickness), typeof(double), typeof(Fan), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

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
                var startAngle = StartAngle * 2 * Math.PI / 360;
                var endAngle = EndAngle * 2 * Math.PI / 360;
                var inRadius = Radius - FanThickness;
                bool isLargeArc = endAngle - startAngle >= PolarCoordinate.ANGLE180;
                Point circleCenter = new Point(Radius, Radius);
                var startOutPolar = new PolarCoordinate(Radius, startAngle, circleCenter);
                var endOutPolar = new PolarCoordinate(Radius, endAngle, circleCenter);
                var startInPolar = new PolarCoordinate(inRadius, startAngle, circleCenter);
                var endInPolar = new PolarCoordinate(inRadius, endAngle, circleCenter);
                PathSegmentCollection pathSegments = new PathSegmentCollection
                {
                    new ArcSegment(endOutPolar.Convert2XYPoint(), new Size(Radius, Radius), 0, isLargeArc, SweepDirection.Clockwise, true),
                    new LineSegment(endInPolar.Convert2XYPoint(), true),
                    new ArcSegment(startInPolar.Convert2XYPoint(), new Size(inRadius, inRadius), 0, isLargeArc, SweepDirection.Counterclockwise, true)
                };
                PathFigureCollection pathFigures = new PathFigureCollection()
                {
                    new PathFigure(startOutPolar.Convert2XYPoint(), pathSegments, true)
                };
                return new PathGeometry(pathFigures);
            }
        }
    }
}
