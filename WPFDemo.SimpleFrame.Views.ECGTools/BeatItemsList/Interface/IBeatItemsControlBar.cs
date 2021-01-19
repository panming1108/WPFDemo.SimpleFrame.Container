using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public interface IBeatItemsControlBar
    {
        bool IsStrech { get; }
        PrevCurrentNextEnum PrevCurrentNextStatus { get; }
        SortArgs SortArgs { get; set; }
    }
}
