using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public interface IDragSelect
    {
        void SetCtrlKeyStatus(bool isKeyDown);
        void RenderDragSelectMask(GeometryDrawing geometryDrawing);
    }
}
