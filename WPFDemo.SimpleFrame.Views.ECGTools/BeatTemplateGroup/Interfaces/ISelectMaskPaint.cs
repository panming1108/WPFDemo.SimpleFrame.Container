using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public interface ISelectMaskPaint
    {
        void DrawingHandler(Action<DrawingContext> drawingAction);
    }
}
