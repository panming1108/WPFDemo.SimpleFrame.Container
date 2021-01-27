using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public abstract class BaseSelectAction : ISelectAction
    {
        private bool _isDrag;
        private Rect _selectMaskRect = Rect.Empty;
        private Point _mouseDownPoint;
        private Brush _maskFillBrush;
        private Brush _maskStrokeBrush;
        private double _maskStrokeThickness;
        private Pen _maskStrokePen;
        private ISelectMaskPaint _selectMaskPaint;
        private BeatTemplateGroupView _groupItemsContainer;
        public BeatTemplateGroupView GroupItemsContainer => _groupItemsContainer;
        public ISelectMaskPaint SelectMaskPaint => _selectMaskPaint;
        private List<BeatTemplateItemView> _actionSelectItems;
        private readonly BrushConverter _brushConverter = new BrushConverter();
        public List<BeatTemplateItemView> ActionSelectItems => _actionSelectItems;
        public Point MouseDownPoint => _mouseDownPoint;
        public Rect SelectMaskRect => _selectMaskRect;
        public bool IsDrag => _isDrag;

        public abstract SelectActionEnum SelectActionMode { get; }

        public BaseSelectAction(BeatTemplateGroupView selectItemsContainer, ISelectMaskPaint selectMaskPaint)
        {
            _groupItemsContainer = selectItemsContainer;
            _selectMaskPaint = selectMaskPaint;
            InitBrush();
        }

        private void InitBrush()
        {
            _maskFillBrush = (Brush)_brushConverter.ConvertFromString("#C099C9EF");
            _maskStrokeBrush = (Brush)_brushConverter.ConvertFromString("#CCE4F7");
            _maskStrokeThickness = 1;
            _maskStrokePen = new Pen(_maskStrokeBrush, _maskStrokeThickness);
        }

        public void Click()
        {
            OnClick();
        }

        public void MouseDown(Point currentPoint)
        {
            _mouseDownPoint = currentPoint;
            OnMouseDown(currentPoint);
        }

        public void Draging(Point currentPoint)
        {
            OnDraging(currentPoint);
        }

        public void DragOver()
        {
            OnDragOver();
        }

        protected virtual void OnDragOver()
        {
            SetDragSelectItems();
            _isDrag = false;
            var control = _groupItemsContainer as UIElement;
            control.ReleaseMouseCapture();
            RenderDragSelect(Rect.Empty);
            _selectMaskRect = Rect.Empty;
        }

        protected virtual void OnMouseDown(Point currentPoint)
        {

        }

        protected virtual void OnDraging(Point currentPoint)
        {
            if (_mouseDownPoint == currentPoint)
            {
                return;
            }
            _isDrag = true;
            _groupItemsContainer.CaptureMouse();
            //鼠标按下拖动
            if (currentPoint.X < 0)
            {
                currentPoint.X = 0;
            }
            if (currentPoint.X > _groupItemsContainer.ActualWidth)
            {
                currentPoint.X = _groupItemsContainer.ActualWidth;
            }
            if (currentPoint.Y < 0)
            {
                currentPoint.Y = 0;
            }
            if (currentPoint.Y > _groupItemsContainer.ActualHeight)
            {
                currentPoint.Y = _groupItemsContainer.ActualHeight;
            }
            _selectMaskRect = new Rect(_mouseDownPoint, currentPoint);
            RenderDragSelect(_selectMaskRect);
        }

        protected virtual void OnClick()
        {
            _isDrag = false;
            RenderDragSelect(Rect.Empty);
            SetClickSelectItems();
            _selectMaskRect = Rect.Empty;
        }

        private void SetDragSelectItems()
        {
            _actionSelectItems = GetItemsByDragArea(_selectMaskRect);
            if (_actionSelectItems.Count <= 0)
            {
                return;
            }
            SetItemsSelectStatus();
        }

        private void SetClickSelectItems()
        {
            _actionSelectItems = GetItemsByMouseUpPosition(Mouse.GetPosition(_groupItemsContainer));
            if (_actionSelectItems.Count <= 0)
            {
                return;
            }
            SetItemsSelectStatus();
        }

        protected abstract void SetItemsSelectStatus();

        public List<BeatTemplateItemView> GetItemsByDragArea(Rect rect)
        {
            var result = new List<BeatTemplateItemView>();
            foreach (var groupItem in _groupItemsContainer.GroupItems)
            {
                var groupItemView = groupItem as BeatTemplateGroupItemView;
                foreach (var item in groupItemView.Items)
                {
                    var itemBounds = GetItemBound(item);
                    if (itemBounds.IntersectsWith(rect))
                    {
                        result.Add((BeatTemplateItemView)item);
                    }
                }
            }
            return result;
        }

        public List<BeatTemplateItemView> GetItemsByMouseUpPosition(Point point)
        {
            var result = new List<BeatTemplateItemView>();
            foreach (var groupItem in _groupItemsContainer.GroupItems)
            {
                var groupItemView = groupItem as BeatTemplateGroupItemView;
                foreach (var item in groupItemView.Items)
                {
                    var itemBounds = GetItemBound(item);
                    if (itemBounds.Contains(point))
                    {
                        result.Add((BeatTemplateItemView)item);
                        break;
                    }
                }
            }
            return result;
        }

        private Rect GetItemBound(object item)
        {
            if (!(item is FrameworkElement itemView))
            {
                return Rect.Empty;
            }
            else
            {
                var topLeft = itemView.TranslatePoint(new Point(0, 0), _groupItemsContainer);
                return new Rect(topLeft.X, topLeft.Y, itemView.ActualWidth, itemView.ActualHeight);
            }
        }

        private void RenderDragSelect(Rect rect)
        {
            RectangleGeometry rectangleGeometry = new RectangleGeometry(rect);
            GeometryDrawing rectDrawing = new GeometryDrawing(_maskFillBrush, _maskStrokePen, rectangleGeometry);
            _selectMaskPaint.DrawingHandler((drawingContext) =>
            {
                drawingContext.DrawDrawing(rectDrawing);
            });
        }
    }
}
