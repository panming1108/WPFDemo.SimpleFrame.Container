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
        private double _originX;
        private double _contextMenuX;

        private GeometryDrawing _rectDrawing;
        private GeometryDrawing _startLineDrawing;
        private GeometryDrawing _endLineDrawing;

        private readonly DrawingCollection _rectDrawings = new DrawingCollection();
        private readonly DrawingCollection _backgroundDrawings = new DrawingCollection();

        private readonly string[] _rectContextMenu = new string[] { "正常", "房颤", "房早", "删除心搏" };
        private MenuItem _setFlagMenuItem;
        private MenuItem _endFlagMenuItem;
        private MenuItem _clearFlagMenuItem;

        //是否右键标记创建
        private bool _isFlag;
        private readonly bool _canDrag;

        public int DragPriority { get; set; }
        public int MouseUpPriority { get; set; }

        public event EventHandler StartDragArea;
        public event EventHandler<PositionEventArgs> DragAreaMouseUp;

        public DragAreaAction(bool canDrag, double leftOffset, double topOffset) : base(leftOffset, topOffset)
        {
            _canDrag = canDrag;
        }

        public override void Dispose()
        {
            _setFlagMenuItem.Click -= SetStartFlag_Click;
            _endFlagMenuItem.Click -= EndFlagMenuItem_Click;
            _clearFlagMenuItem.Click -= ClearFlagMenuItem_Click;
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
            _isFlag = false;
            StartDragArea?.Invoke(this, new EventArgs());
            DrawingDragArea(_originX, currentPoint.X);
        }

        public override void DrawingMouseUp(Point currentPoint)
        {
            double resultX = currentPoint.X;
            if (_rectDrawing != null && _rectDrawing.Bounds.Contains(currentPoint))
            {
                resultX = _startLineDrawing.Bounds.Left;
            }
            DragAreaMouseUp?.Invoke(this, new PositionEventArgs(resultX));
            if(!_isFlag)
            {
                ResetMask();
            }
            DrawingDragAreaMask();
        }

        public override void DrawingMouseWheel(double offset)
        {
            base.DrawingMouseWheel(offset);
            if(_rectDrawing == null && _startLineDrawing == null && _endLineDrawing == null)
            {
                return;
            }
            if(_endLineDrawing == null)
            {
                _rectDrawings.Clear();
                _startLineDrawing = DrawingLine(_startLineDrawing.Bounds.Left + offset);
                _rectDrawings.Add(_startLineDrawing);
                DrawingDragAreaMask();
            }
            else
            {
                DrawingDragArea(_startLineDrawing.Bounds.Left + offset, _endLineDrawing.Bounds.Left + offset);
            }
        }

        public override void DrawingMouseDownWheel(double offset, Point currentPoint)
        {
            base.DrawingMouseDownWheel(offset, currentPoint);
            _originX += offset;
            DrawingDrag(new Point(currentPoint.X - offset, currentPoint.Y));
        }

        private void DrawingDragArea(double start, double end)
        {
            _rectDrawings.Clear();
            Point leftTopPoint = new Point(Math.Min(start, end), TopOffset);
            Point rightBottomPoint = new Point(Math.Max(start, end), TopOffset + Height);
            Rect rect = new Rect(leftTopPoint, rightBottomPoint);

            _rectDrawing = DrawingRect(rect);
            _startLineDrawing = DrawingLine(start);
            _endLineDrawing = DrawingLine(end);

            _rectDrawings.Add(_rectDrawing);
            _rectDrawings.Add(_startLineDrawing);
            _rectDrawings.Add(_endLineDrawing);
            DrawingDragAreaMask();
        }

        private GeometryDrawing DrawingRect(Rect rect)
        {
            Brush rectBrush = (Brush)_brushConverter.ConvertFromString("#6021ADFF");
            Pen rectPen = new Pen(Brushes.Transparent, 1);
            RectangleGeometry rectangleGeometry = new RectangleGeometry(rect);
            return new GeometryDrawing(rectBrush, rectPen, rectangleGeometry);
        }

        private GeometryDrawing DrawingLine(double position)
        {
            Brush lineBrush = Brushes.Red;
            Pen linePen = new Pen(lineBrush, 1);
            LineGeometry lineGeometry = new LineGeometry(new Point(position, TopOffset), new Point(position, TopOffset + Height));
            return new GeometryDrawing(lineBrush, linePen, lineGeometry);
        }

        protected override IEnumerable SetContextMenuItems(Point currentPoint)
        {
            _contextMenuX = currentPoint.X;
            if(_startLineDrawing == null)
            {
                return new MenuItem[] { _setFlagMenuItem };
            }
            else if(_rectDrawing != null && _rectDrawing.Bounds.Contains(currentPoint))
            {
                return _rectContextMenu;
            }
            else
            {
                return new MenuItem[] { _setFlagMenuItem, _endFlagMenuItem, _clearFlagMenuItem };
            }
        }

        public override void PrepareMask(Point current)
        {
            _originX = current.X;
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

        public override void InitMask()
        {
            base.InitMask();
            _setFlagMenuItem = new MenuItem
            {
                Header = "标记开始位置"
            };
            _setFlagMenuItem.Click += SetStartFlag_Click;

            _endFlagMenuItem = new MenuItem
            {
                Header = "标记结束位置"
            };
            _endFlagMenuItem.Click += EndFlagMenuItem_Click;

            _clearFlagMenuItem = new MenuItem
            {
                Header = "取消标记位置"
            };
            _clearFlagMenuItem.Click += ClearFlagMenuItem_Click;
        }

        private void ClearFlagMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ResetMask();
            DrawingDragAreaMask();
        }

        private void EndFlagMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _isFlag = true;
            DrawingDragArea(_startLineDrawing.Bounds.Left, _contextMenuX);
        }

        private void SetStartFlag_Click(object sender, RoutedEventArgs e)
        {
            _isFlag = true;
            if(_rectDrawing == null && _endLineDrawing == null)
            {
                _rectDrawings.Clear();
                _startLineDrawing = DrawingLine(_contextMenuX);
                _rectDrawings.Add(_startLineDrawing);
                DrawingDragAreaMask();
            }
            else
            {
                DrawingDragArea(_contextMenuX, _endLineDrawing.Bounds.Left);
            }
        }

        public override void ResetMask()
        {
            _rectDrawings.Clear();
            _rectDrawing = null;
            _startLineDrawing = null;
            _endLineDrawing = null;
            _originX = default;
        }
    }
}
