using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class ShiftSelectAction : BaseSelectAction
    {
        public override SelectActionEnum SelectActionMode => SelectActionEnum.Shift;
        public ShiftSelectAction(ISelectItemsContainer selectItemsContainer, ISelectMaskPaint selectMaskPaint) : base(selectItemsContainer, selectMaskPaint)
        {
        }

        protected override void SetItemsSelectStatus()
        {
            
        }
    }
}
