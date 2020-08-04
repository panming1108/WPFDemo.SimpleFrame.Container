using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public class PositionEventArgs : EventArgs
    {
        public double Position { get; set; }
        public PositionEventArgs(double position)
        {
            Position = position;
        }
    }
}
