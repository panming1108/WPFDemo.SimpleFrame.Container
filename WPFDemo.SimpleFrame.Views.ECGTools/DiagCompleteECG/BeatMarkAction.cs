using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public class BeatMarkAction : MaskActionBase
    {
        private readonly bool _canClick;

        private readonly CultureInfo _culture = CultureInfo.GetCultureInfo("en-us");
        private readonly Typeface _typeface = new Typeface("Klavika");
        private readonly double _emSize = 15d;

        public BeatMarkAction(bool canClick, double leftOffset, double topOffset) : base(leftOffset, topOffset)
        {
            _canClick = canClick;
        }

        public override void DrawingDrag(Point currentPoint)
        {
            
        }

        public override void DrawingMouseUp(Point currentPoint)
        {
            
        }

        public override void PrepareMask(Point current)
        {
            
        }

        public override void ResetMask()
        {
            
        }
    }
}
