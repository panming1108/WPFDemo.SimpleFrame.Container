using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class PrevCurrentNextEventArgs : EventArgs
    {
        public PrevCurrentNextEnum PrevCurrentNext { get; set; }

        public PrevCurrentNextEventArgs(PrevCurrentNextEnum prevCurrentNext)
        {
            PrevCurrentNext = prevCurrentNext;
        }
    }
}
