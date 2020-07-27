using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public abstract class MaskActionBase
    {
        private bool _isDisplay;
        public bool IsDisplay 
        {
            get => _isDisplay;
            set
            {
                _isDisplay = value;
                if(!_isDisplay)
                {
                    IsReDraw = false;
                    _drawingChildren?.Clear();
                }
            }
        }

        public bool IsReDraw { get; set; }
        public bool CanReSetDrawingChildren => IsDisplay && IsReDraw;
        private DrawingCollection _drawingChildren = new DrawingCollection();
        public DrawingCollection DrawingChildren 
        {
            get => _drawingChildren;
            set
            {
                if(CanReSetDrawingChildren)
                {
                    _drawingChildren = value;
                }
            }
        }
    }
}
