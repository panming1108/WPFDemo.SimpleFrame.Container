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
    public class EquiDistanceAction : IMaskAction
    {
        private double _minInterval;
        private double _firstPoint;
        private double _lastValue = 100;
        private Pen _mainPen = new Pen(Brushes.Red, 1);
        private Pen _otherPen = new Pen(Brushes.Blue, 1);

        public bool IsDisplay { get; set; }
        public int Priority { get; set; }

        public EquiDistanceAction(double minInterval)
        {
            _minInterval = minInterval;
        }

        public DrawingCollection DrawingAllLines(Point mainPoint, double height, double width)
        {
            DrawingCollection drawings = new DrawingCollection();
            _firstPoint = mainPoint.X;
            LineGeometry mainLineGeometry = new LineGeometry(new Point(_firstPoint, 0), new Point(_firstPoint, height));
            GeometryDrawing mainLineDrawing = new GeometryDrawing(_mainPen.Brush, _mainPen, mainLineGeometry);
            drawings.Add(mainLineDrawing);
            var total = width / _minInterval + 1;
            //往前画
            for (int i = 1; i < total; i++)
            {
                LineGeometry otherLineGeometry = new LineGeometry(new Point(_firstPoint - i * _lastValue, 0), new Point(_firstPoint - i * _lastValue, height));
                GeometryDrawing otherLineDrawing = new GeometryDrawing(_otherPen.Brush, _otherPen, otherLineGeometry);
                drawings.Add(otherLineDrawing);
            }
            //往后画
            for (int i = 1; i < total; i++)
            {
                LineGeometry otherLineGeometry = new LineGeometry(new Point(_firstPoint + i * _lastValue, 0), new Point(_firstPoint + i * _lastValue, height));
                GeometryDrawing otherLineDrawing = new GeometryDrawing(_otherPen.Brush, _otherPen, otherLineGeometry);
                drawings.Add(otherLineDrawing);
            }
            return drawings;
        }

        public void CheckAndExecuteAction(bool isReDraw, DrawingContext drawingContext, DrawingCollection drawings, Action drawingAction)
        {
            if (!IsDisplay)
            {
                return;
            }
            if (isReDraw)
            {
                drawingAction();
            }
            else
            {
                foreach (var item in drawings)
                {
                    drawingContext.DrawDrawing(item);
                }
            }
        }
    }
}
