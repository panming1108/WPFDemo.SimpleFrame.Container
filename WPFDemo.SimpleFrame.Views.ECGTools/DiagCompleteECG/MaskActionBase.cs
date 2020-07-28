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
        public double LeftOffset { get; set; }
        public double TopOffset { get; set; }
        public DrawingCollection DrawingChildren { get; set; } = new DrawingCollection();
        public List<MaskText> DrawingTexts { get; set; } = new List<MaskText>();

        public MaskActionBase(double leftOffset, double topOffset)
        {
            LeftOffset = leftOffset;
            TopOffset = topOffset;
        }

        public abstract void PrepareMask(Point current);
        public abstract void ResetMask();
        public abstract void DrawingDrag(Point currentPoint);
        public abstract void DrawingMouseUp(Point currentPoint);

        public void RenderMaskSize(double height, double width)
        {
            Height = height;
            Width = width;
        }

        public virtual Cursor GetMouseOverCursor(Point currentPoint)
        {
            return Cursors.Arrow;
        }
    }
}
