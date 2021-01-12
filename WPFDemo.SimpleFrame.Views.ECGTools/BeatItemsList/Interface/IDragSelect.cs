using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public interface IDragSelect
    {
        DragSelectAction DragSelectAction { get; }
        void RenderDragSelectMask(GeometryDrawing geometryDrawing);
    }
}
