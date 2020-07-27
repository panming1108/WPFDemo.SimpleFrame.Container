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

        public EquiDistanceAction(double minInterval)
        {
            _minInterval = minInterval;
        }

        public void DrawingMouseUpAllLines(Point mainPoint, double height, double width)
        {
            if(!CanReSetDrawingChildren)
            {
                return;
            }
            _firstPoint = mainPoint.X;
            DrawingChildren = DrawingAllLines(height, width);
        }

        public void DrawingMouseMoveAllLines(Point currentPoint, double height, double width)
        {
            if (!CanReSetDrawingChildren)
            {
                return;
            }
            if(currentPoint.X >= _firstPoint - 10 && currentPoint.X <= _firstPoint + 10)
            {
                _firstPoint = currentPoint.X;
                DrawingChildren = DrawingAllLines(height, width);
            }
        }

        private DrawingCollection DrawingAllLines(double height, double width)
        {
            DrawingCollection drawings = new DrawingCollection();
            LineGeometry mainLineGeometry = new LineGeometry(new Point(_firstPoint, 0), new Point(_firstPoint, height));
            GeometryDrawing mainLineDrawing = new GeometryDrawing(_mainPen.Brush, _mainPen, mainLineGeometry);
            drawings.Add(mainLineDrawing);
            var total = width / _minInterval + 1;
            //往前画
            for (int i = 1; i < total; i++)
            {
                LineGeometry otherLineGeometry = new LineGeometry(new Point(_firstPoint - i * _interval, 0), new Point(_firstPoint - i * _interval, height));
                GeometryDrawing otherLineDrawing = new GeometryDrawing(_otherPen.Brush, _otherPen, otherLineGeometry);
                drawings.Add(otherLineDrawing);
            }
            //往后画
            for (int i = 1; i < total; i++)
            {
                LineGeometry otherLineGeometry = new LineGeometry(new Point(_firstPoint + i * _interval, 0), new Point(_firstPoint + i * _interval, height));
                GeometryDrawing otherLineDrawing = new GeometryDrawing(_otherPen.Brush, _otherPen, otherLineGeometry);
                drawings.Add(otherLineDrawing);
            }
            return drawings;
        }
    }
}
