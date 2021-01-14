using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class CommonSelectAction : BaseSelectAction
    {
        public override SelectActionEnum SelectActionMode => SelectActionEnum.None;
        public CommonSelectAction(ISelectItemsContainer selectItemsContainer, ISelectMaskPaint selectMaskPaint) : base(selectItemsContainer, selectMaskPaint)
        {

        }

        protected override void SetItemsSelectStatus()
        {
            SelectItemsContainer.SelectedItemsCollection.TryClearItems();
            foreach (var item in ActionSelectItems)
            {
                item.IsSelected = true;
            }
        }
    }
}
