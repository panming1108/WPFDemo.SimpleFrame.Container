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
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public Point CircleCenter
        {
            get { return (Point)GetValue(CircleCenterProperty); }
            set { SetValue(CircleCenterProperty, value); }
        }

        public double FanThickness
        {
            get { return (double)GetValue(FanThicknessProperty); }
            set { SetValue(FanThicknessProperty, value); }
        }

        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register(nameof(EndAngle), typeof(double), typeof(Fan), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register(nameof(StartAngle), typeof(double), typeof(Fan), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty FanThicknessProperty =
            DependencyProperty.Register(nameof(FanThickness), typeof(double), typeof(Fan), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty CircleCenterProperty =
            DependencyProperty.Register(nameof(CircleCenter), typeof(Point), typeof(Fan), new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register(nameof(Radius), typeof(double), typeof(Fan), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        protected override Geometry DefiningGeometry
        {
            get
            {
                var startAngle = StartAngle * 2 * Math.PI / 360;
                var endAngle = EndAngle * 2 * Math.PI / 360;
                var inRadius = Radius - FanThickness;
                bool isLargeArc = endAngle - startAngle >= PolarCoordinate.ANGLE180;
                var startOutPolar = new PolarCoordinate(Radius, startAngle, CircleCenter);
                var endOutPolar = new PolarCoordinate(Radius, endAngle, CircleCenter);
                var startInPolar = new PolarCoordinate(inRadius, startAngle, CircleCenter);
                var endInPolar = new PolarCoordinate(inRadius, endAngle, CircleCenter);
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
