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
    public class DragAreaAction : MaskActionBase
    {
        private Point _originPoint;
        private double _rectStartX;

        private Rect _originRect;

        public DragAreaAction(double leftOffset, double topOffset) : base(leftOffset, topOffset)
        {
            
        }

        public override void DrawingDrag(Point currentPoint)
        {
            DrawingCollection drawings = new DrawingCollection();

            _rectStartX = _originPoint.X;
            var left = Math.Min(currentPoint.X, _rectStartX);
            var right = Math.Max(currentPoint.X, _rectStartX);
            Point leftTopPoint = new Point(left, TopOffset);
            Point rightBottomPoint = new Point(right, TopOffset + Height);
            _originRect = new Rect(leftTopPoint, rightBottomPoint);

            BrushConverter brushConverter = new BrushConverter();
            Brush lineBrush = Brushes.Red;
            Brush rectBrush = (Brush)brushConverter.ConvertFromString("#6021ADFF");
            Pen rectPen = new Pen(Brushes.Transparent, 1);
            Pen linePen = new Pen(lineBrush, 1);

            RectangleGeometry rectangleGeometry = new RectangleGeometry(_originRect);
            LineGeometry lineGeometry1 = new LineGeometry(leftTopPoint, new Point(left, TopOffset + Height));
            LineGeometry lineGeometry2 = new LineGeometry(new Point(right, TopOffset), rightBottomPoint);

            GeometryDrawing rectangleDrawing = new GeometryDrawing(rectBrush, rectPen, rectangleGeometry);
            GeometryDrawing lineDrawing1 = new GeometryDrawing(lineBrush, linePen, lineGeometry1);
            GeometryDrawing lineDrawing2 = new GeometryDrawing(lineBrush, linePen, lineGeometry2);

            drawings.Add(rectangleDrawing);
            drawings.Add(lineDrawing1);
            drawings.Add(lineDrawing2);
            DrawingChildren = drawings;
        }

        public override void DrawingMouseUp(Point currentPoint)
        {
            DrawingCollection drawings = new DrawingCollection();
            LineGeometry lineGeometry;
            if (_originRect.Contains(currentPoint))
            {
                lineGeometry = new LineGeometry(new Point(_rectStartX, TopOffset), new Point(_rectStartX, TopOffset + Height));
            }
            else
            {
                lineGeometry = new LineGeometry(new Point(currentPoint.X, TopOffset), new Point(currentPoint.X, TopOffset + Height));
            }
            GeometryDrawing lineDrawing = new GeometryDrawing(Brushes.Orange, new Pen(Brushes.Orange, 1), lineGeometry);
            drawings.Add(lineDrawing);
            _originRect = default;
            _originPoint = default;
            DrawingChildren = drawings;
        }

        public ContextMenu GetDragContextMenu(Point currentPoint)
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

        public override void PrepareMask(Point current)
        {
            _originPoint = current;
        }

        public override void ResetMask()
        {
            _originRect = default;
            _originPoint = default;
        }
    }
}
