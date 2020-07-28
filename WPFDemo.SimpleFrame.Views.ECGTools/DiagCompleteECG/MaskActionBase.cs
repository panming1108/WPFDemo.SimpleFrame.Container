using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public abstract class MaskActionBase
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public DrawingCollection DrawingChildren { get; set; } = new DrawingCollection();
        public List<MaskText> DrawingTexts { get; set; } = new List<MaskText>();

        public abstract void PrepareMask(Point current, double height, double width);
        public abstract void ResetMask();
        public abstract void DrawingDrag(Point currentPoint);
        public abstract void DrawingMouseUp(Point currentPoint);
        public virtual Cursor GetMouseOverCursor(Point currentPoint)
        {
            return Cursors.Arrow;
        }
    }
}
