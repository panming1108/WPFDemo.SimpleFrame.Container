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
        private readonly DrawingVisual _drawingVisual = new DrawingVisual();
        public MaskPaint()
        {
            AddVisualChild(_drawingVisual);
        }

        public void DrawingHandler(Action<DrawingContext> drawingAction)
        {
            var drawingContext = _drawingVisual.RenderOpen();
            drawingAction(drawingContext);
            drawingContext.Close();
        }

        public DrawingCollection GetDrawings()
        {
            return _drawingVisual.Drawing.Children;
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
