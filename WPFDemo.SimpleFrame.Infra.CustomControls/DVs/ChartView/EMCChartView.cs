using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DVs.ChartView
{
    public class EMCChartView : ContentControl
    {
        #region field
        private Path _wavePath { get; set; }
        private Path _averagePath { get; set; }
        #endregion
        #region Property
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public DataTemplate DateRangeTemplate
        {
            get { return (DataTemplate)GetValue(DateRangeTemplateProperty); }
            set { SetValue(DateRangeTemplateProperty, value); }
        }

        public Brush ChartBackground
        {
            get { return (Brush)GetValue(ChartBackgroundProperty); }
            set { SetValue(ChartBackgroundProperty, value); }
        }

        public Dictionary<DateTime, double> ChartData
        {
            get { return (Dictionary<DateTime, double>)GetValue(ChartDataProperty); }
            set { SetValue(ChartDataProperty, value); }
        }

        public ChartViewModeEnum ViewMode
        {
            get { return (ChartViewModeEnum)GetValue(ViewModeProperty); }
            set { SetValue(ViewModeProperty, value); }
        }

        public double QuantityResult
        {
            get { return (double)GetValue(QuantityResultProperty); }
            set { SetValue(QuantityResultProperty, value); }
        }

        public double LineThickness
        {
            get { return (double)GetValue(LineThicknessProperty); }
            set { SetValue(LineThicknessProperty, value); }
        }

        public Brush LineBrush
        {
            get { return (Brush)GetValue(LineBrushProperty); }
            set { SetValue(LineBrushProperty, value); }
        }

        public PathGeometry DataPath
        {
            get { return (PathGeometry)GetValue(DataPathProperty); }
            set { SetValue(DataPathProperty, value); }
        }

        public PathGeometry AverageDataPath
        {
            get { return (PathGeometry)GetValue(AverageDataPathProperty); }
            set { SetValue(AverageDataPathProperty, value); }
        }

        public Brush AverageLineBrush
        {
            get { return (Brush)GetValue(AverageLineBrushProperty); }
            set { SetValue(AverageLineBrushProperty, value); }
        }

        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }

        public Dictionary<DateTime, double> AverageData
        {
            get { return (Dictionary<DateTime, double>)GetValue(AverageDataProperty); }
            set { SetValue(AverageDataProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public string TooltipContent
        {
            get { return (string)GetValue(TooltipContentProperty); }
            set { SetValue(TooltipContentProperty, value); }
        }

        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }

        public Dictionary<KeyValuePair<DateTime, double>, string> HandledChartData
        {
            get { return (Dictionary<KeyValuePair<DateTime, double>, string>)GetValue(HandledChartDataProperty); }
            set { SetValue(HandledChartDataProperty, value); }
        }
        #endregion

        #region DependencyProperty
        // Using a DependencyProperty as the backing store for HandledChartData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HandledChartDataProperty =
            DependencyProperty.Register("HandledChartData", typeof(Dictionary<KeyValuePair<DateTime, double>, string>), typeof(EMCChartView));

        // Using a DependencyProperty as the backing store for Unit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register("Unit", typeof(string), typeof(EMCChartView));

        // Using a DependencyProperty as the backing store for TooltipContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TooltipContentProperty =
            DependencyProperty.Register("TooltipContent", typeof(string), typeof(EMCChartView));

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(EMCChartView));

        // Using a DependencyProperty as the backing store for AverageData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AverageDataProperty =
            DependencyProperty.Register("AverageData", typeof(Dictionary<DateTime, double>), typeof(EMCChartView), new PropertyMetadata(OnAverageDataChange));

        // Using a DependencyProperty as the backing store for StrokeDashArray.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrokeDashArrayProperty =
            DependencyProperty.Register("StrokeDashArray", typeof(DoubleCollection), typeof(EMCChartView));

        // Using a DependencyProperty as the backing store for AverageLineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AverageLineBrushProperty =
            DependencyProperty.Register("AverageLineBrush", typeof(Brush), typeof(EMCChartView));

        // Using a DependencyProperty as the backing store for AverageDataPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AverageDataPathProperty =
            DependencyProperty.Register("AverageDataPath", typeof(PathGeometry), typeof(EMCChartView));

        // Using a DependencyProperty as the backing store for DataPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataPathProperty =
            DependencyProperty.Register("DataPath", typeof(PathGeometry), typeof(EMCChartView));

        // Using a DependencyProperty as the backing store for LineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LineBrushProperty =
            DependencyProperty.Register("LineBrush", typeof(Brush), typeof(EMCChartView));

        // Using a DependencyProperty as the backing store for LineThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LineThicknessProperty =
            DependencyProperty.Register("LineThickness", typeof(double), typeof(EMCChartView));

        // Using a DependencyProperty as the backing store for QuantityResult.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty QuantityResultProperty =
            DependencyProperty.Register("QuantityResult", typeof(double), typeof(EMCChartView));

        // Using a DependencyProperty as the backing store for ViewMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModeProperty =
            DependencyProperty.Register("ViewMode", typeof(ChartViewModeEnum), typeof(EMCChartView), new PropertyMetadata(OnModeChange));

        // Using a DependencyProperty as the backing store for ChartData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChartDataProperty =
            DependencyProperty.Register("ChartData", typeof(Dictionary<DateTime, double>), typeof(EMCChartView), new PropertyMetadata(OnDataChange));

        // Using a DependencyProperty as the backing store for ChartBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChartBackgroundProperty =
            DependencyProperty.Register("ChartBackground", typeof(Brush), typeof(EMCChartView));

        // Using a DependencyProperty as the backing store for DateRangeTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateRangeTemplateProperty =
            DependencyProperty.Register("DateRangeTemplate", typeof(DataTemplate), typeof(EMCChartView));

        // Using a DependencyProperty as the backing store for HeaderTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(EMCChartView));

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(EMCChartView));
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (_wavePath != null)
            {
                _wavePath.MouseMove -= OnPathMouseMove;
            }
            _wavePath = GetTemplateChild("WaveLine") as Path;
            if (_wavePath != null)
            {
                _wavePath.MouseMove += OnPathMouseMove;
            }
            if (_averagePath != null)
            {
                _averagePath.MouseMove -= OnAveragePathMouseMove;
            }
            _averagePath = GetTemplateChild("AverageLineChart") as Path;
            if (_averagePath != null)
            {
                _averagePath.MouseMove += OnAveragePathMouseMove;
            }
        }

        private void OnAveragePathMouseMove(object sender, MouseEventArgs e)
        {
            Path path = sender as Path;
            Point currentPoint = e.GetPosition(path);
            int count = (int)((currentPoint.X) / 20);
            try
            {
                var tipContent = AverageData.ElementAt(Math.Max(0, count - 1));
                string label = "数据:";
                if (Unit == "例")
                {
                    label = "例数：";
                }
                if (Unit == "分")
                {
                    label = "分钟：";
                }
                TooltipContent = "日期：" + tipContent.Key.ToString("yyyy-MM-dd") + "\r\n"
                                 + label + Convert.ToDouble($"{tipContent.Value:f3}") + Unit;
            }
            catch (Exception exception)
            {
                Console.WriteLine("OnAveragePathMouseMove" + count);
            }

        }

        private void OnPathMouseMove(object sender, MouseEventArgs e)
        {
            Path path = sender as Path;
            Point currentPoint = e.GetPosition(path);
            int count = (int)(currentPoint.X / 20);
            try
            {
                var tipContent = ChartData.ElementAt(Math.Max(0, count - 1));
                string label = "数据：";
                if (Unit == "例")
                {
                    label = "例数：";
                }
                if (Unit == "分")
                {
                    label = "分钟：";
                }
                TooltipContent = "日期：" + tipContent.Key.ToString("yyyy-MM-dd") + "\r\n"
                                 + label + Convert.ToDouble($"{tipContent.Value:f3}") + Unit;
            }
            catch (Exception exception)
            {
                Console.WriteLine("OnPathMouseMove" + count);

            }

        }

        private static void OnModeChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EMCChartView columnChart = d as EMCChartView;
            if (columnChart != null && columnChart.ChartData != null)
            {
                var newMode = (ChartViewModeEnum)e.NewValue;
                columnChart.ChangeQuantityResultByMode(newMode);
            }
        }

        private static void OnDataChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EMCChartView columnChart = d as EMCChartView;
            if (columnChart != null)
            {
                var newDatas = e.NewValue as Dictionary<DateTime, double>;
                columnChart.ChangeQuantityResultByData(newDatas.Values);
                columnChart.ChangeDataPath(newDatas.Values);
                columnChart.HandleChartData(newDatas);
            }
        }

        private static void OnAverageDataChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EMCChartView columnChart = d as EMCChartView;
            if (columnChart != null)
            {
                var newAverageDatas = e.NewValue as Dictionary<DateTime, double>;
                columnChart.ChangeAverageDataPath(newAverageDatas.Values);
            }
        }

        private void ChangeAverageDataPath(IEnumerable<double> newAverageDatas)
        {
            if (ViewMode == ChartViewModeEnum.AverageLineChart)
            {
                IEnumerable<double> allDatas = newAverageDatas.Concat(ChartData.Values);
                var handledDatas = HandleDatas(allDatas);
                var handledAverageDatas = handledDatas.Take(newAverageDatas.Count());
                AverageDataPath = GenerateAveragePathGeometry(handledAverageDatas);
                DataPath = GeneratePathGeometry(handledDatas.Skip(newAverageDatas.Count()));
            }
        }

        private void ChangeDataPath(IEnumerable<double> newDatas)
        {
            if (ViewMode != ChartViewModeEnum.ColumnChart)
            {
                IEnumerable<double> handledChartDatas;
                if (AverageData == null)
                {
                    handledChartDatas = HandleDatas(newDatas);
                }
                else
                {
                    IEnumerable<double> allDatas = newDatas.Concat(AverageData.Values);
                    var handledDatas = HandleDatas(allDatas);
                    handledChartDatas = handledDatas.Take(newDatas.Count());
                }
                DataPath = GeneratePathGeometry(handledChartDatas);
            }
        }

        private void ChangeQuantityResultByData(IEnumerable<double> newDatas)
        {

            QuantityResult = 0;
            foreach (var item in newDatas)
            {
                QuantityResult += item;
            }
            if (ViewMode == ChartViewModeEnum.AverageLineChart)
            {
                QuantityResult /= newDatas.Count();
            }
            QuantityResult = Convert.ToDouble($"{QuantityResult:f3}");
        }

        private void ChangeQuantityResultByMode(ChartViewModeEnum newMode)
        {
            if (newMode == ChartViewModeEnum.AverageLineChart)
            {
                QuantityResult = 0;
                foreach (var item in ChartData.Values)
                {
                    QuantityResult += item;
                }
                QuantityResult /= ChartData.Count();
                QuantityResult = Convert.ToDouble($"{QuantityResult:f3}");
            }
        }

        private PathGeometry GeneratePathGeometry(IEnumerable<double> datas)
        {
            var newDatas = datas;
            PathFigureCollection pathFigures = new PathFigureCollection();
            PathSegmentCollection pathSegments = new PathSegmentCollection();
            Point prePoint = new Point();
            if (newDatas.Count() == 1)
            {
                prePoint = new Point(0, newDatas.ElementAt(0));
                PathSegment PathSegment1 = new LineSegment(prePoint, false);
                PathSegment PathSegment2 = new LineSegment(new Point(20, newDatas.ElementAt(0)), true);
                PathSegment PathSegment3 = new LineSegment(new Point(20, 0), false);
                pathSegments.Add(PathSegment1);
                pathSegments.Add(PathSegment2);
                pathSegments.Add(PathSegment3);
            }
            else
            {

                for (int i = 0; i < newDatas.Count() - 1; i++)
                {
                    if (i == 0)
                    {
                        prePoint = new Point(0, newDatas.ElementAt(i));
                        PathSegment firstPathSegment = new LineSegment(prePoint, false);
                        pathSegments.Add(firstPathSegment);
                    }
                    Point currentPoint = new Point(20 * i + 10, newDatas.ElementAt(i));
                    Point nextPoint1 = new Point(20 * i + 30, newDatas.ElementAt(i + 1));
                    Point nextPoint2;
                    if (i == newDatas.Count() - 2)
                    {
                        nextPoint2 = new Point(20 * i + 40, newDatas.ElementAt(i + 1));
                    }
                    else
                    {
                        nextPoint2 = new Point(20 * i + 50, newDatas.ElementAt(i + 2));
                    }
                    PathSegment pathSegment = GetBezierSegment(prePoint, currentPoint, nextPoint1, nextPoint2);
                    pathSegments.Add(pathSegment);
                    if (i == newDatas.Count() - 2)
                    {
                        PathSegment lastPathSegment1 = new LineSegment(new Point(20 * i + 40, newDatas.ElementAt(i + 1)), true);
                        PathSegment lastPathSegment2 = new LineSegment(new Point(20 * i + 40, 0), false);
                        PathSegment lastPathSegment3 = new LineSegment(new Point(0, 0), false);
                        pathSegments.Add(lastPathSegment1);
                        pathSegments.Add(lastPathSegment2);
                        pathSegments.Add(lastPathSegment3);
                    }
                    prePoint = currentPoint;
                }
            }
            PathFigure pathFigure = new PathFigure(new Point(0, 0), pathSegments, false);
            pathFigures.Add(pathFigure);
            PathGeometry pathGeometry = new PathGeometry(pathFigures);
            return pathGeometry;
        }

        private PathGeometry GenerateAveragePathGeometry(IEnumerable<double> datas)
        {
            var newDatas = datas;
            PathFigureCollection pathFigures = new PathFigureCollection();
            PathSegmentCollection pathSegments = new PathSegmentCollection();
            double firstData = 0;
            Point prePoint = new Point();
            if (newDatas.Count() == 1)
            {
                prePoint = new Point(0, newDatas.ElementAt(0));
                PathSegment PathSegment1 = new LineSegment(prePoint, false);
                PathSegment PathSegment2 = new LineSegment(new Point(20, newDatas.ElementAt(0)), true);
                pathSegments.Add(PathSegment1);
                pathSegments.Add(PathSegment2);
            }
            else
            {
                for (int i = 0; i < newDatas.Count() - 1; i++)
                {
                    if (i == 0)
                    {
                        firstData = newDatas.ElementAt(i);
                        prePoint = new Point(0, firstData);
                    }
                    Point currentPoint = new Point(20 * i + 10, newDatas.ElementAt(i));
                    Point nextPoint1 = new Point(20 * i + 30, newDatas.ElementAt(i + 1));
                    Point nextPoint2;
                    if (i == newDatas.Count() - 2)
                    {
                        nextPoint2 = new Point(20 * i + 40, newDatas.ElementAt(i + 1));
                    }
                    else
                    {
                        nextPoint2 = new Point(20 * i + 50, newDatas.ElementAt(i + 2));
                    }
                    PathSegment pathSegment = GetBezierSegment(prePoint, currentPoint, nextPoint1, nextPoint2);
                    pathSegments.Add(pathSegment);
                    if (i == newDatas.Count() - 2)
                    {
                        PathSegment lastPathSegment = new LineSegment(new Point(20 * i + 40, newDatas.ElementAt(i + 1)), true);
                        pathSegments.Add(lastPathSegment);
                    }
                    prePoint = currentPoint;
                }
            }
            PathFigure pathFigure = new PathFigure(new Point(0, firstData), pathSegments, false);
            pathFigures.Add(pathFigure);
            PathGeometry pathGeometry = new PathGeometry(pathFigures);
            return pathGeometry;
        }

        private BezierSegment GetBezierSegment(Point prePoint, Point currentPoint, Point nextPoint1, Point nextPoint2)
        {
            double x0 = prePoint.X;
            double y0 = prePoint.Y;
            double x1 = currentPoint.X;
            double y1 = currentPoint.Y;
            double x2 = nextPoint1.X;
            double y2 = nextPoint1.Y;
            double x3 = nextPoint2.X;
            double y3 = nextPoint2.Y;
            double a = (x1 + x2) / 2;
            double b = (y1 + y2) / 2;
            double k1 = (y2 - y0) / (x2 - x0);
            double k2 = (y3 - y1) / (x3 - x1);
            double m1 = 0;
            double m2 = 0;
            double cp1XC = 0;
            double cp1YC = 0;
            double cp1M = 0;
            double cp2XC = 0;
            double cp2YC = 0;
            double cp2M = 0;
            double cp1X = 0;
            double cp1Y = 0;
            double cp2X = 0;
            double cp2Y = 0;

            if (k1 != 0)
            {
                m1 = -1 / k1;
                cp1XC = k1 * x1 - y1 - (m1 * a - b);
                cp1YC = k1 * b + a - (m1 * y1 + x1);
                cp1M = k1 - m1;
                cp1X = cp1XC / cp1M;
                cp1Y = cp1YC / cp1M;
            }
            else
            {
                cp1X = a;
                cp1Y = y1;
            }

            if (k2 != 0)
            {
                m2 = -1 / k2;
                cp2XC = k2 * x2 - y2 - (m2 * a - b);
                cp2YC = k2 * b + a - (m2 * y2 + x2);
                cp2M = k2 - m2;
                cp2X = cp2XC / cp2M;
                cp2Y = cp2YC / cp2M;
            }
            else
            {
                cp2X = a;
                cp2Y = y2;
            }

            cp1Y = cp1Y < 0 ? 0 : cp1Y;
            cp2Y = cp2Y < 0 ? 0 : cp2Y;

            Point controlPoint1 = new Point(cp1X, cp1Y);
            Point controlPoint2 = new Point(cp2X, cp2Y);

            return new BezierSegment(controlPoint1, controlPoint2, nextPoint1, true);
        }

        private IEnumerable<double> HandleDatas(IEnumerable<double> datas)
        {
            double multiple = datas.Max() / 1.5;
            var newDatas = new List<double>();
            foreach (var item in datas)
            {
                newDatas.Add(item / multiple);
            }
            return newDatas;
        }

        private void HandleChartData(Dictionary<DateTime, double> chartDatas)
        {
            var keyValuePairs = new Dictionary<KeyValuePair<DateTime, double>, string>();
            foreach (var item in chartDatas)
            {
                KeyValuePair<DateTime, double> keyValuePair = new KeyValuePair<DateTime, double>(item.Key, item.Value);
                if (item.Value < chartDatas.Values.Max() / 60)
                {
                    if (item.Value == 0)
                    {
                        keyValuePair = new KeyValuePair<DateTime, double>(item.Key, 0);
                    }
                    else
                    {
                        keyValuePair = new KeyValuePair<DateTime, double>(item.Key, chartDatas.Values.Max() / 60);
                    }
                }
                string tooiTip = "日期：" + item.Key.ToString("yyyy-MM") + "\r\n" + "例数：" + item.Value;
                keyValuePairs.Add(keyValuePair, tooiTip);
            }
            HandledChartData = keyValuePairs;
        }
    }
}
