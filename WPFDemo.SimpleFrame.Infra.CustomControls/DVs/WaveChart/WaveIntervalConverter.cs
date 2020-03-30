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

        public Geometry CaculateCurveGeometry()
        {
            PathFigureCollection pathFigures = new PathFigureCollection();
            PathSegmentCollection pathSegments = GeneratePathSegmentsByItemsSource();
            PathFigure pathFigure = new PathFigure(DataConverter2Point(_itemsSource.First()), pathSegments, false);
            pathFigure.IsFilled = false;
            pathFigures.Add(pathFigure);
            PathGeometry pathGeometry = new PathGeometry(pathFigures);
            return pathGeometry;
        }

        private PathSegmentCollection GeneratePathSegmentsByItemsSource()
        {
            PathSegmentCollection pathSegments = new PathSegmentCollection();
            foreach (var item in _itemsSource)
            {
                LineSegment lineSegment = new LineSegment(DataConverter2Point(item), true);
                pathSegments.Add(lineSegment);
            }
            return pathSegments;
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
