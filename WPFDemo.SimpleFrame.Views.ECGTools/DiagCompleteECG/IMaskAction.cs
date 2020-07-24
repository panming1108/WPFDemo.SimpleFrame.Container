using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public interface IMaskAction
    {
        bool IsDisplay { get; set; }
        int Priority { get; set; }

        void CheckAndExecuteAction(bool isReDraw, DrawingContext drawingContext, DrawingCollection drawings, Action drawingAction); 
    }
}
