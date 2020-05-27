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

        private BezierAlgorithm _bezierAlgorithm;

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
            _bezierAlgorithm = new BezierAlgorithm(3, XAxisInterval == 0 ? 1 : 1 / XAxisInterval);
        }

        private Dictionary<string, double> CaculateYAxisIntervalMapping(double max)
        {
            Dictionary<string, double> datas = new Dictionary<string, double>();
            if (max == _yMin)
            {
                datas.Add("0", _height);
                YAxisInterval = _height / 2;
                YAxisIntervalValue = max;
                if(max != 0)
                {
                    datas.Add(max.ToString(), _height - YAxisInterval);
                }
            }
            else
            {
                datas.Add(_yMin.ToString(), _height);
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
            PathSegmentCollection pathSegments = GeneratePathSegmentsByItemsSource(itemsSource, lineMode);
            PathFigure pathFigure;
            if (isFill)
            {
                double avg = GetItemsSourceValueAvgYAxis(itemsSource);
                Point startPoint = new Point(0, avg);
                Point firstPoint = DataConverter2Point(itemsSource.First());
                PathSegment firstLine = new LineSegment(startPoint, false);
                PathSegment secondLine = _bezierAlgorithm.GetBezierSegment(startPoint, startPoint, firstPoint, DataConverter2Point(itemsSource.ElementAt(1)));
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

        private PathSegmentCollection GeneratePathSegmentsByItemsSource(Dictionary<string, double> itemsSource, LineModeEnum lineMode)
        {
            PathSegmentCollection pathSegments;
            if(lineMode == LineModeEnum.StraightLine)
            {
                pathSegments = new PathSegmentCollection();
                foreach (var item in itemsSource)
                {
                    PathSegment pathSegment = new LineSegment(DataConverter2Point(item), true);
                    pathSegments.Add(pathSegment);
                }
            }
            else
            {
                pathSegments = GenerateCurvePathSegment(itemsSource);
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

                BezierSegment bezierSegment = _bezierAlgorithm.GetBezierSegment(a, b, c, d);
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

                points.AddRange(_bezierAlgorithm.GetThirdLevelBezierPoints(a, b, c, d));
            }
            return points;
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
