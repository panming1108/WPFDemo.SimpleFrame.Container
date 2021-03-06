﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class CtrlSelectAction : BaseSelectAction
    {
        private Dictionary<string, bool> _itemsStatusDicWhenMouseDown = new Dictionary<string, bool>();
        public override SelectActionEnum SelectActionMode => SelectActionEnum.Ctrl;
        public CtrlSelectAction(ISelectItemsContainer selectItemsContainer, ISelectMaskPaint selectMaskPaint) : base(selectItemsContainer, selectMaskPaint)
        {
        }

        protected override void OnMouseDown(Point currentPoint)
        {
            base.OnMouseDown(currentPoint);
            _itemsStatusDicWhenMouseDown.Clear();
            foreach (var item in SelectItemsContainer.Items)
            {
                var itemView = item as ISelectItem;
                _itemsStatusDicWhenMouseDown.Add(item.GetHashCode().ToString(), itemView.IsSelected);
            }
        }

        protected override void SetItemsSelectStatus()
        {
            foreach (var item in SelectItemsContainer.Items)
            {
                var itemView = item as ISelectItem;
                var oldSelectStatus = _itemsStatusDicWhenMouseDown[item.GetHashCode().ToString()];
                //如果是框选的则取反，否则还是之前的
                if (ActionSelectItems.Contains(item))
                {
                    itemView.IsSelected = !oldSelectStatus;
                }
                else
                {
                    itemView.IsSelected = oldSelectStatus;
                }
            }
        }
    }
}
