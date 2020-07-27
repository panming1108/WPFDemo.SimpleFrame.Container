using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public abstract class MaskActionBase
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public DrawingCollection DrawingChildren { get; set; } = new DrawingCollection();

        public abstract void PrepareMask(Point current, double height, double width);
        public abstract void ResetMask();
    }
}
