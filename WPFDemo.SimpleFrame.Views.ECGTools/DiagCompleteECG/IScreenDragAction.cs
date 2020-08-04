using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public interface IScreenDragAction
    {
        int DragPriority { get; set; }
    }
}
