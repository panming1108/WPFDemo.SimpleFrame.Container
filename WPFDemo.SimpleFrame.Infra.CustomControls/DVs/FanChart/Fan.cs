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
        public Point StartPoint
        {
            get { return (Point)GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }
        public SweepDirection SweepDirection
        {
            get { return (SweepDirection)GetValue(SweepDirectionProperty); }
            set { SetValue(SweepDirectionProperty, value); }
        }

        public Point EndPoint
        {
            get { return (Point)GetValue(EndPointProperty); }
            set { SetValue(EndPointProperty, value); }
        }

        public bool IsLargeArc
        {
            get { return (bool)GetValue(IsLargeArcProperty); }
            set { SetValue(IsLargeArcProperty, value); }
        }

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register(nameof(Radius), typeof(double), typeof(Fan), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty IsLargeArcProperty =
            DependencyProperty.Register(nameof(IsLargeArc), typeof(bool), typeof(Fan), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty EndPointProperty =
            DependencyProperty.Register(nameof(EndPoint), typeof(Point), typeof(Fan), new FrameworkPropertyMetadata(new Point(0,0), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register(nameof(StartPoint), typeof(Point), typeof(Fan), new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty SweepDirectionProperty =
            DependencyProperty.Register(nameof(SweepDirection), typeof(SweepDirection), typeof(Fan), new FrameworkPropertyMetadata(SweepDirection.Clockwise, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        protected override Geometry DefiningGeometry
        {
            get
            {
                PathSegmentCollection pathSegments = new PathSegmentCollection
                {
                    new ArcSegment(EndPoint, new Size(Radius, Radius), 0, IsLargeArc, SweepDirection, true)
                };
                PathFigureCollection pathFigures = new PathFigureCollection()
                {
                    new PathFigure(StartPoint, pathSegments, false)
                };
                return new PathGeometry(pathFigures);
            }
        }
    }
}
