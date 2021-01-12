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
        private Rect _selectMaskRect;
        private Point _mouseDownPoint;
        private Brush _maskFillBrush;
        private Brush _maskStrokeBrush;
        private double _maskStrokeThickness;
        private Pen _maskStrokePen;
        private readonly BrushConverter _brushConverter = new BrushConverter();
        private readonly ISelectItemsContainer _container;
        public ISelectItemsContainer Container => _container;
        public bool IsCtrlKeyDown { get; set; }
        public DragSelectAction(ISelectItemsContainer container)
        {
            _container = container;
            InitBrush();
            RegisterMouseEvent();
        }

        private void InitBrush()
        {
            _maskFillBrush = (Brush)_brushConverter.ConvertFromString("#C099C9EF");
            _maskStrokeBrush = (Brush)_brushConverter.ConvertFromString("#CCE4F7");
            _maskStrokeThickness = 1;
            _maskStrokePen = new Pen(_maskStrokeBrush, _maskStrokeThickness);
        }

        private void RegisterMouseEvent()
        {
            var container = _container as UIElement;
            container.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnMouseLeftButtonDown));
            container.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(OnMouseLeftButtonUp));
            container.AddHandler(UIElement.MouseMoveEvent, new MouseEventHandler(OnMouseMove));
        }
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _mouseDownPoint = e.GetPosition(sender as FrameworkElement);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }
            //鼠标按下拖动
            var currentPoint = e.GetPosition(sender as FrameworkElement);
            _selectMaskRect = new Rect(_mouseDownPoint, currentPoint);
            RectangleGeometry rectangleGeometry = new RectangleGeometry(_selectMaskRect);        
            GeometryDrawing rectDrawing = new GeometryDrawing(_maskFillBrush, _maskStrokePen, rectangleGeometry);
            RenderDragSelect(rectDrawing);
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RenderDragSelect(null);
            var selectItem = GetItemByMouseUpPosition(Mouse.GetPosition((UIElement)_container));
            if (selectItem == null)
            {
                return;
            }
            if (IsCtrlKeyDown)
            {
                selectItem.IsSelected = !selectItem.IsSelected;
            }
            else
            {
                _container.SelectedItemsCollection.TryClearItems();
                selectItem.IsSelected = true;
            }
            _container.OnItemsControlSelectionChanged(new ItemsControlSelectionChangedEventArgs(new List<ISelectItem>() { selectItem }, IsCtrlKeyDown));
        }

        private void RenderDragSelect(GeometryDrawing rectDrawing)
        {
            var control = _container as IDragSelect;
            control.RenderDragSelectMask(rectDrawing);
        }

        private ISelectItem GetItemByMouseUpPosition(Point point)
        {
            foreach (var item in _container.Items)
            {
                var itemView = item as FrameworkElement;
                var topLeft = itemView.TranslatePoint(new Point(0, 0), (UIElement)_container);
                var itemBounds = new Rect(topLeft.X, topLeft.Y, itemView.ActualWidth, itemView.ActualHeight);
                if (itemBounds.Contains(point))
                {
                    return (ISelectItem)itemView;
                }
            }
            return null;
        }

        private void UnRegisterMouseEvent()
        {
            var container = _container as UIElement;
            container.RemoveHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnMouseLeftButtonDown));
            container.RemoveHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(OnMouseLeftButtonUp));
            container.RemoveHandler(UIElement.MouseMoveEvent, new MouseEventHandler(OnMouseMove));
        }

        public void Dispose()
        {
            UnRegisterMouseEvent();
        }
    }
}
