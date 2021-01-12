using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class BoolEventArgs : EventArgs
    {
        public bool Boolean { get; set; }
        public BoolEventArgs(bool boolean)
        {

            Boolean = boolean;
        }
    }
}
