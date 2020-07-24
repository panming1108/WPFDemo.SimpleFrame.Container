using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public class MaskDrawings
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
                    _drawingCollection?.Clear();
                }
            }
        }

        public bool IsReDraw { get; set; }

        private DrawingCollection _drawingCollection = new DrawingCollection();
        public DrawingCollection DrawingCollection 
        {
            get => _drawingCollection;
            set
            {
                if(IsDisplay && IsReDraw)
                {
                    _drawingCollection = value;
                }
            }
        }
    }
}
