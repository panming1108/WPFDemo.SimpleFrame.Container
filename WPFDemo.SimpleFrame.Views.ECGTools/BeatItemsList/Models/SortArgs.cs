using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class SortArgs
    {
        public SortArgs(SortEnum sortType, bool isAsc)
        {
            SortType = sortType;
            IsAsc = isAsc;
        }

        public SortEnum SortType { get; set; }
        public bool IsAsc { get; set; }
    }
}
