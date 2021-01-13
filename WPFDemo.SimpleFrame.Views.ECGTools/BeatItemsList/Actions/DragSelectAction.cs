using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class DragSelectAction : IDisposable
    {
        private Rect _selectMaskRect = Rect.Empty;
        private Point _mouseDownPoint;
        public Point MouseDownPoint => _mouseDownPoint;
        private Brush _maskFillBrush;
        private Brush _maskStrokeBrush;
        private double _maskStrokeThickness;
        private Pen _maskStrokePen;
        private bool _isDrag;
        private readonly BrushConverter _brushConverter = new BrushConverter();
        private Dictionary<string, bool> _itemsStatusDicWhenMouseDown = new Dictionary<string, bool>();
        private List<ISelectItem> _actionSelectItems;
        private readonly ISelectItemsContainer _container;
        private bool _isCtrlKeyDown;
        public ISelectItemsContainer Container => _container;
        public bool IsCtrlKeyDown 
        {
            get => _isCtrlKeyDown;
            set
            {
                //拖动过程中按下ctrl不允许修改 即 没有在拖动 或者 松开ctrl可以修改
                if(!_isDrag || !value)
                {
                    _isCtrlKeyDown = value;
                }
            }
        }
        public DragSelectAction(ISelectItemsContainer container)
        {
            _container = container;
            InitBrush();
        }

        private void InitBrush()
        {
            _maskFillBrush = (Brush)_brushConverter.ConvertFromString("#C099C9EF");
            _maskStrokeBrush = (Brush)_brushConverter.ConvertFromString("#CCE4F7");
            _maskStrokeThickness = 1;
            _maskStrokePen = new Pen(_maskStrokeBrush, _maskStrokeThickness);
        }

        public void OnMouseLeftButtonDown(Point mouseDownPoint)
        {
            _mouseDownPoint = mouseDownPoint;
            _itemsStatusDicWhenMouseDown.Clear();
            foreach (var item in _container.Items)
            {
                var itemView = item as ISelectItem;
                _itemsStatusDicWhenMouseDown.Add(item.GetHashCode().ToString(), itemView.IsSelected);
            }
        }

        public void OnMouseMove(Point currentPoint)
        {
            _isDrag = true;
            //鼠标按下拖动
            _selectMaskRect = new Rect(_mouseDownPoint, currentPoint);           
            RenderDragSelect(_selectMaskRect);
            SetDragSelectItems();
        }

        public List<ISelectItem> OnDragOver()
        {
            _isDrag = false;
            RenderDragSelect(Rect.Empty);
            _selectMaskRect = Rect.Empty;
            return _actionSelectItems;
        }

        public List<ISelectItem> OnClickOver()
        {
            _isDrag = false;
            RenderDragSelect(Rect.Empty);
            SetClickSelectItems();
            _selectMaskRect = Rect.Empty;
            return _actionSelectItems;
        }

        private void SetDragSelectItems()
        {
            _actionSelectItems = GetItemsByDragArea(_selectMaskRect);
            SetItemsSelectStatus(isDrag: true);
        }

        private void SetClickSelectItems()
        {
            _actionSelectItems = GetItemsByMouseUpPosition(Mouse.GetPosition((UIElement)_container));
            SetItemsSelectStatus(isDrag: false);
        }

        private void SetItemsSelectStatus(bool isDrag)
        {
            if (_actionSelectItems.Count <= 0)
            {
                return;
            }
            if (IsCtrlKeyDown)
            {
                if(isDrag)
                {
                    foreach (var item in _container.Items)
                    {
                        var itemView = item as ISelectItem;
                        var oldSelectStatus = _itemsStatusDicWhenMouseDown[item.GetHashCode().ToString()];
                        //如果是框选的则取反，否则还是之前的
                        if (_actionSelectItems.Contains(item))
                        {
                            itemView.IsSelected = !oldSelectStatus;
                        }
                        else
                        {
                            itemView.IsSelected = oldSelectStatus;
                        }
                    }
                }
                else
                {
                    foreach (var item in _actionSelectItems)
                    {
                        item.IsSelected = !item.IsSelected;
                    }
                }
            }
            else
            {
                _container.SelectedItemsCollection.TryClearItems();
                foreach (var item in _actionSelectItems)
                {
                    item.IsSelected = true;
                }
            }
        }

        private void RenderDragSelect(Rect rect)
        {
            RectangleGeometry rectangleGeometry = new RectangleGeometry(rect);
            GeometryDrawing rectDrawing = new GeometryDrawing(_maskFillBrush, _maskStrokePen, rectangleGeometry);
            var control = _container as IDragSelect;
            control.RenderDragSelectMask(rectDrawing);
        }

        private List<ISelectItem> GetItemsByDragArea(Rect rect)
        {
            var result = new List<ISelectItem>();
            foreach (var item in _container.Items)
            {
                var itemBounds = GetItemBound(item);
                if (itemBounds.IntersectsWith(rect))
                {
                    result.Add((ISelectItem)item);
                }
            }
            return result;
        }

        private List<ISelectItem> GetItemsByMouseUpPosition(Point point)
        {
            var result = new List<ISelectItem>();
            foreach (var item in _container.Items)
            {
                var itemBounds = GetItemBound(item);
                if (itemBounds.Contains(point))
                {
                    result.Add((ISelectItem)item);
                    break;
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
                var topLeft = itemView.TranslatePoint(new Point(0, 0), (UIElement)_container);
                return new Rect(topLeft.X, topLeft.Y, itemView.ActualWidth, itemView.ActualHeight);
            }
        }

        public void Dispose()
        {

        }
    }
}
