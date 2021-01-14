using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class PageNoChangedEventArgs : EventArgs
    {
        public PageNoChangedEventArgs(int newPageNo, int oldPageNo)
        {
            NewPageNo = newPageNo;
            OldPageNo = oldPageNo;
        }

        public int NewPageNo { get; set; }
        public int OldPageNo { get; set; }


    }
}
