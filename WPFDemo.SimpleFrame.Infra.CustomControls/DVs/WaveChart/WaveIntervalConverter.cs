using System;
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
                YAxisIntervalValue = Math.Round((max - _yMin) / _ordinateCount, 2);
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
            for (int i = 0; i < count; i++)
            {
                Point a, b, c, d;
                a = DataConverter2Point(itemsSource.ElementAt(i - 1 < 0 ? 0 : i - 1));
                b = DataConverter2Point(itemsSource.ElementAt(i));
                c = DataConverter2Point(itemsSource.ElementAt(i + 1));
                d = DataConverter2Point(itemsSource.ElementAt(i + 2 > count - 1 ? count : i + 2));

                BezierSegment bezierSegment = GetBezierSegment(a, b, c, d);
                pathSegments.Add(bezierSegment);
            }
            return pathSegments;
        }

        public List<Point> GenerateCurvePoints(Dictionary<string, double> itemsSource)
        {
            int count = itemsSource.Count - 1;
            List<Point> points = new List<Point>();
            for (int i = 0; i < count; i++)
            {
                Point a, b, c, d;
                a = DataConverter2Point(itemsSource.ElementAt(i - 1 < 0 ? 0 : i - 1));
                b = DataConverter2Point(itemsSource.ElementAt(i));
                c = DataConverter2Point(itemsSource.ElementAt(i + 1));
                d = DataConverter2Point(itemsSource.ElementAt(i + 2 > count - 1 ? count : i + 2));

                points.AddRange(GetBezierPoints(a, b, c, d));
            }
            return points;
        }

        private BezierSegment GetBezierSegment(Point prev, Point current, Point next, Point nextNext, bool isStroked = true)
        {
            var tuple1 = GetControlPoint(_rt, prev, current, next);
            var tuple2 = GetControlPoint(_rt, current, next, nextNext);

            return new BezierSegment(tuple1.Item2, tuple2.Item1, next, isStroked);
        }

        /// <summary>
        /// 根据三次贝塞尔曲线公式，获取一段Bezier曲线上的所有点
        /// </summary>
        /// <param name="prev">前一个点</param>
        /// <param name="current">当前点</param>
        /// <param name="next">下一个点</param>
        /// <param name="nextNext">下下一个点</param>
        /// <returns></returns>
        private List<Point> GetBezierPoints(Point prev, Point current, Point next, Point nextNext)
        {
            Point control1 = GetControlPoint(_rt, prev, current, next).Item2;
            Point control2 = GetControlPoint(_rt, current, next, nextNext).Item1;

            List<Point> points = new List<Point>();
            var step = XAxisInterval == 0 ? 1 : 1 / XAxisInterval;

            for (double t = 0; t <= 1; t += step)
            {
                double p01X = (1 - t) * current.X + t * control1.X;
                double p11X = (1 - t) * control1.X + t * control2.X;
                double p21X = (1 - t) * control2.X + t * next.X;
                double p02X = (1 - t) * p01X + t * p11X;
                double p12X = (1 - t) * p11X + t * p21X;
                double p03X = (1 - t) * p02X + t * p12X;

                double p01Y = (1 - t) * current.Y + t * control1.Y;
                double p11Y = (1 - t) * control1.Y + t * control2.Y;
                double p21Y = (1 - t) * control2.Y + t * next.Y;
                double p02Y = (1 - t) * p01Y + t * p11Y;
                double p12Y = (1 - t) * p11Y + t * p21Y;
                double p03Y = (1 - t) * p02Y + t * p12Y;

                points.Add(new Point(p03X, p03Y));
            }

            return points;
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
            Vector item1, item2;
            if (ncp1.Angle(v1) < 90)
            {
                item1 = ncp1.Multiply(v1Len * rt).Add(vectorB);
                item2 = ncp2.Multiply(v2Len * rt).Add(vectorB);
            }
            else
            {
                item1 = ncp2.Multiply(v1Len * rt).Add(vectorB);
                item2 = ncp1.Multiply(v2Len * rt).Add(vectorB);
            }

            Point item1Center = new Point((pointPrev.X + pointCur.X) / 2, (pointPrev.Y + pointCur.Y) / 2);
            Point item2Center = new Point((pointCur.X + pointNext.X) / 2, (pointCur.Y + pointNext.Y) / 2);

            if (item1.X > pointCur.X)
            {
                item1.X = pointCur.X;
            }
            if (item2.X < pointCur.X)
            {
                item2.X = pointCur.X;
            }

            if (item1.X < item1Center.X)
            {
                item1.X = item1Center.X;
            }
            if (item2.X > item2Center.X)
            {
                item2.X = item2Center.X;
            }

            return new Tuple<Point, Point>(item1.ToPoint(), item2.ToPoint());
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

        public KeyValuePair<string, double> PointConverter2Data(Point point)
        {
            string key = "";
            double value = 1;
            return new KeyValuePair<string, double>(key, value);
        }
    }
}
