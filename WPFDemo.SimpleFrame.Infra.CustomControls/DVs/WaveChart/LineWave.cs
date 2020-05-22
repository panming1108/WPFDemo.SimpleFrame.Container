using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DVs.WaveChart
{
    [TemplatePart(Name = PART_CANVAS, Type = typeof(Canvas))]
    public class LineWave : Control
    {
        private const string PART_CANVAS = "PART_Canvas";
        private Canvas _canvas;
        private DataPoint _currentPoint;

        public Dictionary<string, double> ItemsSource
        {
            get { return (Dictionary<string, double>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(Dictionary<string, double>), typeof(LineWave));

        public WaveIntervalConverter WaveIntervalConverter
        {
            get { return (WaveIntervalConverter)GetValue(WaveIntervalConverterProperty); }
            set { SetValue(WaveIntervalConverterProperty, value); }
        }

        public static readonly DependencyProperty WaveIntervalConverterProperty =
            DependencyProperty.Register(nameof(WaveIntervalConverter), typeof(WaveIntervalConverter), typeof(LineWave), new PropertyMetadata(OnReDrawing));

        public bool IsDrawPoint
        {
            get { return (bool)GetValue(IsDrawPointProperty); }
            set { SetValue(IsDrawPointProperty, value); }
        }

        public static readonly DependencyProperty IsDrawPointProperty =
            DependencyProperty.Register(nameof(IsDrawPoint), typeof(bool), typeof(LineWave));

        public LineModeEnum LineMode
        {
            get { return (LineModeEnum)GetValue(LineModeProperty); }
            set { SetValue(LineModeProperty, value); }
        }

        public static readonly DependencyProperty LineModeProperty =
            DependencyProperty.Register(nameof(LineMode), typeof(LineModeEnum), typeof(LineWave), new PropertyMetadata(OnReDrawing));

        public double WaveStrokeThickness
        {
            get { return (double)GetValue(WaveStrokeThicknessProperty); }
            set { SetValue(WaveStrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty WaveStrokeThicknessProperty =
            DependencyProperty.Register(nameof(WaveStrokeThickness), typeof(double), typeof(LineWave), new PropertyMetadata(OnReDrawing));

        public Brush WaveStrokeBrush
        {
            get { return (Brush)GetValue(WaveStrokeBrushProperty); }
            set { SetValue(WaveStrokeBrushProperty, value); }
        }

        public static readonly DependencyProperty WaveStrokeBrushProperty =
            DependencyProperty.Register(nameof(WaveStrokeBrush), typeof(Brush), typeof(LineWave), new PropertyMetadata(OnReDrawing));

        public double PointRadius
        {
            get { return (double)GetValue(PointRadiusProperty); }
            set { SetValue(PointRadiusProperty, value); }
        }

        public static readonly DependencyProperty PointRadiusProperty =
            DependencyProperty.Register(nameof(PointRadius), typeof(double), typeof(LineWave), new PropertyMetadata(OnReDrawing));

        public Brush PointFillBrush
        {
            get { return (Brush)GetValue(PointFillBrushProperty); }
            set { SetValue(PointFillBrushProperty, value); }
        }

        public static readonly DependencyProperty PointFillBrushProperty =
            DependencyProperty.Register(nameof(PointFillBrush), typeof(Brush), typeof(LineWave), new PropertyMetadata(OnReDrawing));

        public bool IsWaveFill
        {
            get { return (bool)GetValue(IsWaveFillProperty); }
            set { SetValue(IsWaveFillProperty, value); }
        }

        public static readonly DependencyProperty IsWaveFillProperty =
            DependencyProperty.Register(nameof(IsWaveFill), typeof(bool), typeof(LineWave), new PropertyMetadata(OnReDrawing));

        public Brush WaveFillBrush
        {
            get { return (Brush)GetValue(WaveFillBrushProperty); }
            set { SetValue(WaveFillBrushProperty, value); }
        }

        public static readonly DependencyProperty WaveFillBrushProperty =
            DependencyProperty.Register(nameof(WaveFillBrush), typeof(Brush), typeof(LineWave), new PropertyMetadata(OnReDrawing));

        public LineWave()
        {
            MouseMove += LineWave_MouseMove;
            Unloaded += LineWave_Unloaded;
        }

        private void LineWave_MouseMove(object sender, MouseEventArgs e)
        {
            var control = sender as LineWave;
            if(control == null)
            {
                return;
            }
            if(control.WaveIntervalConverter == null)
            {
                return;
            }
            if(control.ItemsSource == null)
            {
                return;
            }
            if(control._canvas == null)
            {
                return;
            }
            var allPoints = control.WaveIntervalConverter.GenerateCurvePoints(control.ItemsSource);
            var currentPoint = e.GetPosition(control);
            var minDistance = allPoints.Select(x => Math.Abs(x.X - currentPoint.X)).Min();
            var selectedPoints = allPoints.Where(x => Math.Abs(x.X - currentPoint.X) == minDistance);
            var point = selectedPoints.First();
            if (_currentPoint == null)
            {
                _currentPoint = new DataPoint(PointRadius * 4, PointRadius * 4, point.X, point.Y)
                {
                    Background = Brushes.Red,
                    //XValue = "111",
                    //YValue = 20
                };
            }
            else
            {
                _currentPoint.X = point.X;
                _currentPoint.Y = point.Y;
                //_currentPoint.XValue = "22";
                //_currentPoint.YValue = 33;               
            }
            if (!_canvas.Children.Contains(_currentPoint))
            {
                _canvas.Children.Add(_currentPoint);
            }
        }

        private void LineWave_Unloaded(object sender, RoutedEventArgs e)
        {
            MouseMove -= LineWave_MouseMove;
            Unloaded -= LineWave_Unloaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _canvas = GetTemplateChild(PART_CANVAS) as Canvas;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (ItemsSource == null)
            {
                return;
            }
            if (ItemsSource.Count() == 0)
            {
                return;
            }
            if (WaveIntervalConverter == null)
            {
                return;
            }
            Pen pen = new Pen(WaveStrokeBrush, WaveStrokeThickness);
            DrawLineWave(drawingContext, pen);
            if(IsDrawPoint)
            {
                DrawPoint();
            }
        }

        private void DrawPoint()
        {
            if(_canvas == null)
            {
                return;
            }
            _canvas.Children.Clear();
            foreach (var item in ItemsSource)
            {
                Point point = WaveIntervalConverter.DataConverter2Point(item);
                AddDataPoint(point, item.Key, item.Value);
            }
        }

        private void AddDataPoint(Point point, string xValue, double yValue)
        {
            if(_canvas == null)
            {
                return;
            }
            DataPoint dataPoint = new DataPoint(PointRadius * 2, PointRadius * 2, point.X, point.Y)
            {
                Background = PointFillBrush,
                XValue = xValue,
                YValue = yValue
            };
            _canvas.Children.Add(dataPoint);
        }

        private void DrawLineWave(DrawingContext drawingContext, Pen pen)
        {
            //var points = WaveIntervalConverter.GenerateCurvePoints(ItemsSource);
            //for (int i = 0; i < points.Count - 1; i++)
            //{
            //    drawingContext.DrawLine(pen, points[i], points[i + 1]);
            //}

            PathGeometry pathGeometry = WaveIntervalConverter.CaculateCurveGeometry(ItemsSource, LineMode, IsWaveFill);
            drawingContext.DrawGeometry(WaveFillBrush, pen, pathGeometry);
        }

        private static void OnReDrawing(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d != null)
            {
                LineWave lineWave = d as LineWave;
                lineWave.InvalidateVisual();
            }
        }
    }
}
