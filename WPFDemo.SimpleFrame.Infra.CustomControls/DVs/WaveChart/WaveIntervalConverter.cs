﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DVs.WaveChart
{
    public class WaveIntervalConverter
    {
        public double YAxisInterval { get; private set; }
        public double XAxisInterval { get; private set; }
        public double YAxisIntervalValue { get; private set; }
        public Dictionary<string, double> YAxisIntervalMapping { get; private set; }
        public Dictionary<string, double> XAxisIntervalMapping { get; private set; }

        private double _width;
        private double _height;
        private double _offset;
        private double _yMin;
        private int _ordinateCount;
        private Dictionary<string, double> _itemsSource;

        private double _rt = 0.3;

        public WaveIntervalConverter(Dictionary<string, double> itemsSource, int ordinateCount, double width, double height, double offset, double yMin = 0)
        {
            _ordinateCount = ordinateCount;
            _width = width;
            _height = height;
            _offset = offset;
            _yMin = yMin;
            _itemsSource = itemsSource;
            YAxisIntervalMapping = CaculateYAxisIntervalMapping(itemsSource.Values.Max());
            XAxisIntervalMapping = CaculateXAxisIntervalMapping(itemsSource.Keys);
        }

        private Dictionary<string, double> CaculateYAxisIntervalMapping(double max)
        {
            Dictionary<string, double> datas = new Dictionary<string, double>();
            datas.Add(_yMin.ToString(), _height);
            if (max == _yMin)
            {
                YAxisInterval = _height / 2;
                YAxisIntervalValue = max;
                datas.Add(max.ToString(), _height - YAxisInterval);
            }
            else
            {
                YAxisInterval = (_height - _offset) / _ordinateCount;
                YAxisIntervalValue = Math.Round((max - _yMin) / (_ordinateCount), 2);
                for (int i = 0; i < _ordinateCount; i++)
                {
                    datas.Add((_yMin + ((i + 1) * YAxisIntervalValue)).ToString(), _height - (i + 1) * YAxisInterval);
                }
            }
            return datas;
        }

        private Dictionary<string, double> CaculateXAxisIntervalMapping(IEnumerable<string> abscissaAxisSource)
        {
            Dictionary<string, double> datas = new Dictionary<string, double>();
            XAxisInterval = (_width - _offset) / abscissaAxisSource.Count();

            int i = 0;
            foreach (var item in abscissaAxisSource)
            {
                datas.Add(item, (i + 1) * XAxisInterval);
                i++;
            }
            return datas;
        }

        public PathGeometry CaculateCurveGeometry(Dictionary<string, double> itemsSource, LineModeEnum lineMode, bool isFill)
        {
            PathFigureCollection pathFigures = new PathFigureCollection();
            PathSegmentCollection pathSegments = GeneratePathSegmentsByItemsSource(lineMode);
            PathFigure pathFigure;
            if (isFill)
            {
                double avg = GetItemsSourceValueAvgYAxis(itemsSource);
                Point startPoint = new Point(0, avg);
                Point firstPoint = DataConverter2Point(itemsSource.First());
                PathSegment firstLine = new LineSegment(startPoint, false);
                PathSegment secondLine = GetBezierSegment(startPoint, startPoint, firstPoint, DataConverter2Point(itemsSource.ElementAt(1)));
                PathSegment finalLine = new LineSegment(new Point(DataConverter2Point(itemsSource.Last()).X, _height), false);
                pathSegments.Insert(0, firstLine);
                pathSegments.Insert(1, secondLine);
                pathSegments.Add(finalLine);
                pathFigure = new PathFigure(new Point(0, _height), pathSegments, false);
                pathFigure.IsFilled = true;
            }
            else
            {
                pathFigure = new PathFigure(DataConverter2Point(itemsSource.First()), pathSegments, false);
                pathFigure.IsFilled = false;
            }
            pathFigures.Add(pathFigure);
            PathGeometry pathGeometry = new PathGeometry(pathFigures);
            return pathGeometry;
        }

        private PathSegmentCollection GeneratePathSegmentsByItemsSource(LineModeEnum lineMode)
        {
            PathSegmentCollection pathSegments;
            if(lineMode == LineModeEnum.StraightLine)
            {
                pathSegments = new PathSegmentCollection();
                foreach (var item in _itemsSource)
                {
                    PathSegment pathSegment = new LineSegment(DataConverter2Point(item), true);
                    pathSegments.Add(pathSegment);
                }
            }
            else
            {
                pathSegments = GenerateCurvePathSegment(_itemsSource);
            }
            return pathSegments;
        }

        private PathSegmentCollection GenerateCurvePathSegment(Dictionary<string, double> itemsSource)
        {
            int count = itemsSource.Count - 1;
            PathSegmentCollection pathSegments = new PathSegmentCollection();
            double ave = GetItemsSourceValueAvgYAxis(itemsSource);
            for (int i = 0; i < count; i++)
            {
                Point a, b, c, d;
                b = DataConverter2Point(itemsSource.ElementAt(i));
                c = DataConverter2Point(itemsSource.ElementAt(i + 1));
                if (i == 0)
                {
                    a = new Point(b.X * 2 - c.X, ave);
                    d = DataConverter2Point(itemsSource.ElementAt(2));
                }
                else if(i == count - 1)
                {
                    a = DataConverter2Point(itemsSource.ElementAt(i - 1));                  
                    d = new Point(c.X * 2 - b.X, ave);
                }
                else
                {
                    a = DataConverter2Point(itemsSource.ElementAt(i - 1));
                    d = DataConverter2Point(itemsSource.ElementAt(i + 2));
                }

                BezierSegment bezierSegment = GetBezierSegment(a,b,c,d);
                
                pathSegments.Add(bezierSegment);
            }
            return pathSegments;
        }

        private BezierSegment GetBezierSegment(Point prev, Point current, Point next, Point nextNext, bool isStroked = true)
        {
            var tuple1 = GetControlPoint(_rt, prev, current, next);
            var tuple2 = GetControlPoint(_rt, current, next, nextNext);

            var centerPoint = new Point((current.X + next.X) / 2, (current.Y + next.Y) / 2);

            Point point1 = tuple1.Item2;
            Point point2 = tuple2.Item1;

            if (point1.X > centerPoint.X)
            {
                point1.X = centerPoint.X;
            }

            if (point2.X < centerPoint.X)
            {
                point2.X = centerPoint.X;
            }

            return new BezierSegment(point1, point2, next, isStroked);
        }

        private Tuple<Point, Point> GetControlPoint(double rt, Point pointPrev, Point pointCur, Point pointNext)
        {
            var a = pointPrev;
            var b = pointCur;
            var c = pointNext;
            var v1 = new Vector(a.X - b.X, a.Y - b.Y);
            var v2 = new Vector(c.X - b.X, c.Y - b.Y);
            var v1Len = v1.Length();
            var v2Len = v2.Length();
            var t1 = v1.Normalize();
            var t2 = v2.Normalize();
            var t3 = t1.Add(t2);
            var centerV = t3.Normalize();
            if(double.IsNaN(centerV.X) && double.IsNaN(centerV.Y))
            {
                var p1 = new Point((a.X + b.X) / 2, (a.Y + b.Y) / 2);
                var p2 = new Point((c.X + b.X) / 2, (c.Y + b.Y) / 2);
                return new Tuple<Point, Point>(p1, p2);
            }
            var ncp1 = new Vector(centerV.Y, centerV.X * -1);
            var ncp2 = new Vector(centerV.Y * -1, centerV.X);
            Vector vectorB = new Vector(b.X, b.Y);
            if (ncp1.Angle(v1) < 90)
            {
                var p1 = ncp1.Multiply(v1Len * rt).Add(vectorB);
                var p2 = ncp2.Multiply(v2Len * rt).Add(vectorB);
                return new Tuple<Point, Point>(p1.ToPoint(), p2.ToPoint());
            }
            else
            {
                var p1 = ncp1.Multiply(v2Len * rt).Add(vectorB);
                var p2 = ncp2.Multiply(v1Len * rt).Add(vectorB);
                return new Tuple<Point, Point>(p2.ToPoint(), p1.ToPoint());
            }
        }

        public double GetItemsSourceValueAvgYAxis(Dictionary<string, double> itemsSource)
        {
            return _height - (itemsSource.Values.Average() / YAxisIntervalValue) * YAxisInterval;
        }

        public Point DataConverter2Point(KeyValuePair<string, double> data)
        {
            Point point = new Point
            {
                X = XAxisIntervalMapping[data.Key],
                Y = _height - (data.Value / YAxisIntervalValue) * YAxisInterval
            };
            return point;
        }
    }
}
