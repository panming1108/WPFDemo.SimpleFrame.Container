using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class SortEventArgs : EventArgs
    {
        public string SortFieldName { get; set; }
        public bool IsAsc { get; set; }

        public SortEventArgs(string sortFieldName, bool isAsc)
        {
            SortFieldName = sortFieldName;
            IsAsc = isAsc;
        }
    }
}
