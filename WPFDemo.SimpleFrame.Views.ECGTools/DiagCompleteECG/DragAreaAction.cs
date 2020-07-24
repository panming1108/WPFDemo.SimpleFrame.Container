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
    public class DragAreaAction : IMaskAction
    {
        private Point _originPoint;

        private Rect _originRect;

        public bool IsDisplay { get; set; }
        public int Priority { get; set; }

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

        public DrawingCollection DrawingArea(Point startPoint, Point endPoint, double height)
        {
            DrawingCollection drawings = new DrawingCollection();

            _originPoint = startPoint;

            var left = Math.Min(endPoint.X, _originPoint.X);
            var right = Math.Max(endPoint.X, _originPoint.X);
            Point leftTopPoint = new Point(left, 0);
            Point rightBottomPoint = new Point(right, height);
            _originRect = new Rect(leftTopPoint, rightBottomPoint);

            BrushConverter brushConverter = new BrushConverter();
            Brush lineBrush = Brushes.Red;
            Brush rectBrush = (Brush)brushConverter.ConvertFromString("#6021ADFF");
            Pen rectPen = new Pen(Brushes.Transparent, 1);
            Pen linePen = new Pen(lineBrush, 1);

            RectangleGeometry rectangleGeometry = new RectangleGeometry(_originRect);
            LineGeometry lineGeometry1 = new LineGeometry(leftTopPoint, new Point(left, height));
            LineGeometry lineGeometry2 = new LineGeometry(new Point(right, 0), rightBottomPoint);

            GeometryDrawing rectangleDrawing = new GeometryDrawing(rectBrush, rectPen, rectangleGeometry);
            GeometryDrawing lineDrawing1 = new GeometryDrawing(lineBrush, linePen, lineGeometry1);
            GeometryDrawing lineDrawing2 = new GeometryDrawing(lineBrush, linePen, lineGeometry2);

            drawings.Add(rectangleDrawing);
            drawings.Add(lineDrawing1);
            drawings.Add(lineDrawing2);
            return drawings;
        }

        public DrawingCollection DrawingSingleLine(Point startPoint, double height)
        {
            DrawingCollection drawings = new DrawingCollection();
            LineGeometry lineGeometry;          
            if (_originRect.Contains(startPoint))
            {
                lineGeometry = new LineGeometry(new Point(_originPoint.X, 0), new Point(_originPoint.X, height));
            }
            else
            {
                lineGeometry = new LineGeometry(new Point(startPoint.X, 0), new Point(startPoint.X, height));
            }
            GeometryDrawing lineDrawing = new GeometryDrawing(Brushes.Orange, new Pen(Brushes.Orange, 1), lineGeometry);
            drawings.Add(lineDrawing);
            _originRect = default;
            _originPoint = default;
            return drawings;
        }

        public ContextMenu RectHitTest(Point currentPoint)
        {
            if(_originRect.Contains(currentPoint))
            {
                return new ContextMenu();
            }
            else
            {
                return null;
            }
        }
    }
}
