using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public class SelectActionFactory
    {
        private readonly ISelectMaskPaint _selectMaskPaint;
        private readonly BeatTemplateGroupView _groupItemsContainer;
        public SelectActionFactory(BeatTemplateGroupView groupItemsContainer, ISelectMaskPaint selectMaskPaint)
        {
            _groupItemsContainer = groupItemsContainer;
            _selectMaskPaint = selectMaskPaint;
        }

        public BaseSelectAction GetSelectActionInstance(SelectActionEnum selectAction)
        {
            switch (selectAction)
            {
                case SelectActionEnum.None:
                    return new CommonSelectAction(_groupItemsContainer, _selectMaskPaint);
                case SelectActionEnum.Ctrl:
                    return new CtrlSelectAction(_groupItemsContainer, _selectMaskPaint);
                default:
                    return new CommonSelectAction(_groupItemsContainer, _selectMaskPaint);
            }
        }
    }
}
