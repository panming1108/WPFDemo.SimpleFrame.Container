using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public class EquiDistanceAction : MaskActionBase
    {
        private double _minInterval;
        private double _firstPoint;
        private double _interval = 100;
        private Pen _mainPen = new Pen(Brushes.Red, 1);
        private Pen _otherPen = new Pen(Brushes.Blue, 1);
        private EquiStatusEnum _equiStatus;
        private double _currentMultiple;
        private double _lastPointX;

        public EquiDistanceAction(double minInterval)
        {
            _minInterval = minInterval;
        }

        public void DrawingMouseUpAllLines(Point mainPoint)
        {
            _firstPoint = mainPoint.X;
            DrawingAllLines(Height, Width);
        }

        public void DrawingMouseMoveAllLines(Point currentPoint)
        {
            switch (_equiStatus)
            {
                case EquiStatusEnum.MainLine:
                    _firstPoint = currentPoint.X;
                    
                    break;
                case EquiStatusEnum.OtherLine:
                    _interval += (currentPoint.X - _lastPointX) / _currentMultiple;
                    if (_interval < _minInterval)
                    {
                        _interval = _minInterval;
                    }
                    _lastPointX = currentPoint.X;
                    break;
                default:
                    break;
            }
            DrawingAllLines(Height, Width);           
        }

        private void SetEquiStatus(Point currentPoint)
        {
            if(currentPoint.X >= _firstPoint - 10 && currentPoint.X <= _firstPoint + 10)
            {
                _equiStatus = EquiStatusEnum.MainLine;
            }
            else
            {
                double multiple = Math.Round((currentPoint.X - _firstPoint) / _interval, MidpointRounding.ToEven);
                double currentLineX = _firstPoint + multiple * _interval;
                if(currentPoint.X >= currentLineX - 10 && currentPoint.X <= currentLineX + 10)
                {
                    _equiStatus = EquiStatusEnum.OtherLine;
                    _currentMultiple = multiple;
                }
                else
                {
                    _equiStatus = EquiStatusEnum.None;
                }
            }
        }

        public Cursor GetMouseOverCursor(Point currentPoint)
        {
            SetEquiStatus(currentPoint);
            Cursor cursor = Cursors.Arrow;
            switch (_equiStatus)
            {
                case EquiStatusEnum.MainLine:
                    cursor = Cursors.SizeAll;
                    break;
                case EquiStatusEnum.OtherLine:
                    cursor = Cursors.SizeWE;
                    break;
            }
            return cursor;
        }

        private void DrawingAllLines(double height, double width)
        {
            if(_firstPoint <= 0 || _firstPoint >= width)
            {
                return;
            }
            DrawingCollection drawings = new DrawingCollection();
            LineGeometry mainLineGeometry = new LineGeometry(new Point(_firstPoint, 0), new Point(_firstPoint, height));
            GeometryDrawing mainLineDrawing = new GeometryDrawing(_mainPen.Brush, _mainPen, mainLineGeometry);
            drawings.Add(mainLineDrawing);
            //往前画
            for (double i = _firstPoint - _interval; i >= 0; i -= _interval)
            {
                if(i > width)
                {
                    continue;
                }
                LineGeometry otherLineGeometry = new LineGeometry(new Point(i, 0), new Point(i, height));
                GeometryDrawing otherLineDrawing = new GeometryDrawing(_otherPen.Brush, _otherPen, otherLineGeometry);
                drawings.Add(otherLineDrawing);
            }
            //往后画
            for (double i = _firstPoint + _interval; i < width; i += _interval)
            {
                if (i < 0)
                {
                    continue;
                }
                LineGeometry otherLineGeometry = new LineGeometry(new Point(i, 0), new Point(i, height));
                GeometryDrawing otherLineDrawing = new GeometryDrawing(_otherPen.Brush, _otherPen, otherLineGeometry);
                drawings.Add(otherLineDrawing);
            }
            DrawingChildren = drawings;
        }

        public override void ResetMask()
        {
            DrawingChildren.Clear();
            _interval = 100;
            _equiStatus = EquiStatusEnum.None;
            _currentMultiple = 0;
        }

        public override void PrepareMask(Point current, double height, double width)
        {
            _lastPointX = current.X;
            Height = height;
            Width = width;
        }
    }

    public enum EquiStatusEnum
    {
        None = 0,
        MainLine = 1,
        OtherLine = 2,
    }
}
