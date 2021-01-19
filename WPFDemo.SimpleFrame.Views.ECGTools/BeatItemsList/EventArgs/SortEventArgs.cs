using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class SortEventArgs : EventArgs
    {
        public SortArgs SortArgs { get; set; }

        public SortEventArgs(SortArgs sortArgs)
        {
            SortArgs = sortArgs;
        }
    }
}
