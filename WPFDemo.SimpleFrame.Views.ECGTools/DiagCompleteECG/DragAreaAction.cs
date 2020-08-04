using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WPFDemo.SimpleFrame.Infra.Messager;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public class DragAreaAction : MaskActionBase, IScreenDragAction, IScreenMouseUpAction
    {
        private Point _originPoint;
        private double _rectStartX;

        private Rect _originRect;

        private DrawingCollection _rectDrawings = new DrawingCollection();
        private DrawingCollection _backgroundDrawings = new DrawingCollection();

        private string[] _rectContextMenu = new string[] { "正常", "房颤", "房早", "删除心搏" };
        private string[] _outContextMenu = new string[] { "添加典型图", "设置为最快心率", "设置为最慢心率", "标记开始位置" };

        private bool _canDrag;

        public int DragPriority { get; set; }
        public int MouseUpPriority { get; set; }

        public DragAreaAction(bool canDrag, double leftOffset, double topOffset) : base(leftOffset, topOffset)
        {
            _canDrag = canDrag;
            MessagerInstance.GetMessager().Register<Tuple<double, double>>(this, MaskMessageKeyEnum.RenderAFMask, OnClearRectDrawing);
        }

        private Task OnClearRectDrawing(Tuple<double, double> arg)
        {
            _rectDrawings.Clear();
            DrawingDragAreaMask();
            return TaskEx.FromResult(0);
        }

        public override void Dispose()
        {
            MessagerInstance.GetMessager().Unregister<Tuple<double, double>>(this, MaskMessageKeyEnum.RenderAFMask, OnClearRectDrawing);
        }

        public override void DrawingDrag(Point currentPoint)
        {
            if(!_canDrag)
            {
                return;
            }
            if(currentPoint.Y < TopOffset || currentPoint.Y > TopOffset + Height)
            {
                return;
            }
            if(currentPoint.X < LeftOffset || currentPoint.X > LeftOffset + Width)
            {
                return;
            }
            _rectDrawings.Clear();

            _rectStartX = _originPoint.X;

            MessagerInstance.GetMessager().Send(MaskMessageKeyEnum.StartDragArea, string.Empty);

            var left = Math.Min(currentPoint.X, _rectStartX);
            var right = Math.Max(currentPoint.X, _rectStartX);
            Point leftTopPoint = new Point(left, TopOffset);
            Point rightBottomPoint = new Point(right, TopOffset + Height);
            _originRect = new Rect(leftTopPoint, rightBottomPoint);

            Brush lineBrush = Brushes.Red;
            Brush rectBrush = (Brush)_brushConverter.ConvertFromString("#6021ADFF");
            Pen rectPen = new Pen(Brushes.Transparent, 1);
            Pen linePen = new Pen(lineBrush, 1);

            RectangleGeometry rectangleGeometry = new RectangleGeometry(_originRect);
            LineGeometry lineGeometry1 = new LineGeometry(leftTopPoint, new Point(left, TopOffset + Height));
            LineGeometry lineGeometry2 = new LineGeometry(new Point(right, TopOffset), rightBottomPoint);

            GeometryDrawing rectangleDrawing = new GeometryDrawing(rectBrush, rectPen, rectangleGeometry);
            GeometryDrawing lineDrawing1 = new GeometryDrawing(lineBrush, linePen, lineGeometry1);
            GeometryDrawing lineDrawing2 = new GeometryDrawing(lineBrush, linePen, lineGeometry2);

            _rectDrawings.Add(rectangleDrawing);
            _rectDrawings.Add(lineDrawing1);
            _rectDrawings.Add(lineDrawing2);
            DrawingDragAreaMask();
        }

        public override void DrawingMouseUp(Point currentPoint)
        {
            _rectDrawings.Clear();
            double resultX = currentPoint.X;
            if (_originRect.Contains(currentPoint))
            {
                resultX = _rectStartX;
            }
            MessagerInstance.GetMessager().Send(MaskMessageKeyEnum.DragAreaMouseUp, resultX);
            _originRect = default;
            _originPoint = default;
            DrawingDragAreaMask();
        }

        protected override IEnumerable SetContextMenuItems(Point currentPoint)
        {
            if(_originRect.Contains(currentPoint))
            {
                return _rectContextMenu;
            }
            else
            {
                return _outContextMenu;
            }
        }

        public override void PrepareMask(Point current)
        {
            _originPoint = current;
        }

        public override void RenderMaskSize(double height, double width)
        {
            base.RenderMaskSize(height, width);
            _backgroundDrawings.Clear();
            _rectDrawings.Clear();
            Rect backRect = new Rect(LeftOffset, TopOffset, Width, Height);
            RectangleGeometry backRectGeometry = new RectangleGeometry(backRect);
            GeometryDrawing backRectDrawing = new GeometryDrawing(Brushes.Transparent, new Pen(Brushes.Transparent, 0), backRectGeometry);
            _backgroundDrawings.Add(backRectDrawing);
            DrawingDragAreaMask();
        }

        private void DrawingDragAreaMask()
        {
            DrawingChildren.Clear();
            foreach (var item in _backgroundDrawings)
            {
                DrawingChildren.Add(item);
            }
            foreach (var item in _rectDrawings)
            {
                DrawingChildren.Add(item);
            }
        }

        public override void ResetMask()
        {
            _originRect = default;
            _originPoint = default;
        }
    }
}
