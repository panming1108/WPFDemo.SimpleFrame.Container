using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public class MaskPaint : FrameworkElement
    {
        public MaskDrawings DragAreaCollection { get; set; }
        public MaskDrawings EquiCollection { get; set; }

        private readonly DrawingVisual _drawingVisual = new DrawingVisual();
        public MaskPaint()
        {
            AddVisualChild(_drawingVisual);
            DragAreaCollection = new MaskDrawings();
            EquiCollection = new MaskDrawings();
        }

        public void DrawingHandler()
        {
            var drawingContext = _drawingVisual.RenderOpen();
            foreach (var item in DragAreaCollection.DrawingCollection)
            {
                drawingContext.DrawDrawing(item);
            }
            foreach (var item in EquiCollection.DrawingCollection)
            {
                drawingContext.DrawDrawing(item);
            }
            drawingContext.Close();
        }

        public DrawingCollection GetDrawings()
        {
            return _drawingVisual.Drawing?.Children;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        protected override Visual GetVisualChild(int index)
        {
            return _drawingVisual;
        }
    }
}
