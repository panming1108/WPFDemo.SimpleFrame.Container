using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public class PointEventArgs : EventArgs
    {
        public PointEventArgs(Point point)
        {
            Point = point;
        }
        public Point Point { get; set; }
    }
}
