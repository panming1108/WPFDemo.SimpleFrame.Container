using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public class CommonSelectAction : BaseSelectAction
    {
        public override SelectActionEnum SelectActionMode => SelectActionEnum.None;
        public CommonSelectAction(BeatTemplateGroupView groupItemsContainer, ISelectMaskPaint selectMaskPaint) : base(groupItemsContainer, selectMaskPaint)
        {

        }

        protected override void SetItemsSelectStatus()
        {
            GroupItemsContainer.SelectedItemsCollection.TryClearItems();
            foreach (var item in ActionSelectItems)
            {
                item.IsSelected = true;
            }
        }
    }
}
