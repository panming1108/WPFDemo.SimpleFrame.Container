using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class SelectActionFactory
    {
        private readonly ISelectMaskPaint _selectMaskPaint;
        private readonly ISelectItemsContainer _selectItemsContainer;
        public SelectActionFactory(ISelectItemsContainer selectItemsContainer, ISelectMaskPaint selectMaskPaint)
        {
            _selectItemsContainer = selectItemsContainer;
            _selectMaskPaint = selectMaskPaint;
        }

        public BaseSelectAction GetSelectActionInstance(SelectActionEnum selectAction, Point lastMouseDownPoint)
        {
            switch (selectAction)
            {
                case SelectActionEnum.None:
                    return new CommonSelectAction(_selectItemsContainer, _selectMaskPaint);
                case SelectActionEnum.Ctrl:
                    return new CtrlSelectAction(_selectItemsContainer, _selectMaskPaint);
                case SelectActionEnum.Shift:
                    return new ShiftSelectAction(_selectItemsContainer, _selectMaskPaint, lastMouseDownPoint);
                default:
                    return new CommonSelectAction(_selectItemsContainer, _selectMaskPaint);
            }
        }
    }
}
