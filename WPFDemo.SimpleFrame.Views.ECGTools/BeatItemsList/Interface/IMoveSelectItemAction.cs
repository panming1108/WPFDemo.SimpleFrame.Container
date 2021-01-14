using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public interface IMoveSelectItemAction
    {
        int CurrentMoveIndex { get; set; }
        bool CanMoveToIndex(int index);
        void MoveToIndex(int index);
        void OnCurrentMoveIndexChanged();
    }
}
