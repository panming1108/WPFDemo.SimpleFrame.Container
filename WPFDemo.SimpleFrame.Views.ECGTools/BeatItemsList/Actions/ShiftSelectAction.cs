using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class ShiftSelectAction : BaseSelectAction
    {
        private Dictionary<string, bool> _itemsStatusDicWhenMouseDown = new Dictionary<string, bool>();
        private readonly Point _lastMouseDownPoint;
        public override SelectActionEnum SelectActionMode => SelectActionEnum.Shift;
        public ShiftSelectAction(ISelectItemsContainer selectItemsContainer, ISelectMaskPaint selectMaskPaint, Point lastMouseDownPoint) : base(selectItemsContainer, selectMaskPaint)
        {
            _lastMouseDownPoint = lastMouseDownPoint;
        }

        protected override void OnMouseDown(Point currentPoint)
        {
            base.OnMouseDown(currentPoint);
            var currentItemView = GetItemsByMouseUpPosition(MouseDownPoint).SingleOrDefault();
            var lastItemView = GetItemsByMouseUpPosition(_lastMouseDownPoint).SingleOrDefault();
            if (lastItemView == null)
            {
                return;
            }
            if (currentItemView == null)
            {
                return;
            }
            var lastIndex = SelectItemsContainer.Items.IndexOf(lastItemView);
            var currentIndex = SelectItemsContainer.Items.IndexOf(currentItemView);
            var startIndex = Math.Min(lastIndex, currentIndex);
            var endIndex = Math.Max(lastIndex, currentIndex);
            SelectItemsContainer.SelectedItemsCollection.TryClearItems();
            for (int i = startIndex; i <= endIndex; i++)
            {
                ((ISelectItem)SelectItemsContainer.Items[i]).IsSelected = true;
            }
            _itemsStatusDicWhenMouseDown.Clear();
            foreach (var item in SelectItemsContainer.Items)
            {
                var itemView = item as ISelectItem;
                _itemsStatusDicWhenMouseDown.Add(item.GetHashCode().ToString(), itemView.IsSelected);
            }
        }

        protected override void SetItemsSelectStatus()
        {
            SelectItemsContainer.SelectedItemsCollection.TryClearItems();
            foreach (var item in SelectItemsContainer.Items)
            {
                var itemView = item as ISelectItem;
                var oldSelectStatus = _itemsStatusDicWhenMouseDown[item.GetHashCode().ToString()];
                itemView.IsSelected = oldSelectStatus;
            }
            foreach (var item in ActionSelectItems)
            {
                item.IsSelected = true;
            }
        }
    }
}
