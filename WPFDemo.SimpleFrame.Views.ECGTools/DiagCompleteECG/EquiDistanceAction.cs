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
        private readonly double _minInterval = 20;
        private double _firstPoint;
        private double _interval = 100;
        private Pen _mainPen = new Pen(Brushes.Red, 1);
        private Pen _otherPen = new Pen(Brushes.Blue, 1);
        private EquiStatusEnum _equiStatus;
        private double _currentMultiple;
        private double _lastPointX;

        public EquiDistanceAction(double leftOffset, double topOffset) : base(leftOffset, topOffset)
        {
            
        }

        private void SetEquiStatus(Point currentPoint)
        {
            if(currentPoint.X >= _firstPoint - 2.5 && currentPoint.X <= _firstPoint + 2.5)
            {
                _equiStatus = EquiStatusEnum.MainLine;
            }
            else
            {
                double multiple = Math.Round((currentPoint.X - _firstPoint) / _interval, MidpointRounding.ToEven);
                double currentLineX = _firstPoint + multiple * _interval;
                if(currentPoint.X >= currentLineX - 2.5 && currentPoint.X <= currentLineX + 2.5)
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

        protected override Cursor SetMouseOverCursor(Point currentPoint)
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
            height += TopOffset;
            width += LeftOffset;           
            DrawingChildren.Clear();
            LineGeometry mainLineGeometry = new LineGeometry(new Point(_firstPoint, TopOffset), new Point(_firstPoint, height));
            GeometryDrawing mainLineDrawing = new GeometryDrawing(_mainPen.Brush, _mainPen, mainLineGeometry);
            DrawingChildren.Add(mainLineDrawing);
            //往前画
            for (double i = _firstPoint - _interval; i >= LeftOffset; i -= _interval)
            {
                if(i > width)
                {
                    continue;
                }
                LineGeometry otherLineGeometry = new LineGeometry(new Point(i, TopOffset), new Point(i, height));
                GeometryDrawing otherLineDrawing = new GeometryDrawing(_otherPen.Brush, _otherPen, otherLineGeometry);
                DrawingChildren.Add(otherLineDrawing);
            }
            //往后画
            for (double i = _firstPoint + _interval; i < width; i += _interval)
            {
                if (i < LeftOffset)
                {
                    continue;
                }
                LineGeometry otherLineGeometry = new LineGeometry(new Point(i, TopOffset), new Point(i, height));
                GeometryDrawing otherLineDrawing = new GeometryDrawing(_otherPen.Brush, _otherPen, otherLineGeometry);
                DrawingChildren.Add(otherLineDrawing);
            }
        }

        public override void ResetMask()
        {
            DrawingChildren.Clear();
            _interval = 100;
            _equiStatus = EquiStatusEnum.None;
            _currentMultiple = 0;
        }

        public override void InitMask()
        {
            base.InitMask();
            DrawingMouseUp(new Point(Width == 0 ? LeftOffset + 200 : LeftOffset + Width / 2, 0));
        }

        public override void PrepareMask(Point current)
        {
            _lastPointX = current.X;
        }

        public override void DrawingDrag(Point currentPoint)
        {
            switch (_equiStatus)
            {
                case EquiStatusEnum.MainLine:
                    if (currentPoint.X >= 0 && currentPoint.X <= Width)
                    {
                        _firstPoint = currentPoint.X;
                    }
                    break;
                case EquiStatusEnum.OtherLine:
                    var newInterval = _interval + (currentPoint.X - _lastPointX) / _currentMultiple;
                    if (newInterval >= _minInterval)
                    {
                        _interval = newInterval;
                        _lastPointX = currentPoint.X;
                    }
                    break;
                default:
                    break;
            }            
            DrawingAllLines(Height, Width);
        }

        public override void DrawingMouseWheel(double offset)
        {
            base.DrawingMouseWheel(offset);
            _firstPoint += offset;
            DrawingAllLines(Height, Width);
        }

        public override void DrawingMouseUp(Point currentPoint)
        {
            _firstPoint = currentPoint.X;
            DrawingAllLines(Height, Width);
        }

        public override void Dispose()
        {
            
        }

        public enum EquiStatusEnum
        {
            None = 0,
            MainLine = 1,
            OtherLine = 2,
        }
    }

    
}
