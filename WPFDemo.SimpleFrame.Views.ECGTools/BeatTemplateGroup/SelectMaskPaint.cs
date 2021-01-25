using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public class SelectMaskPaint : FrameworkElement, ISelectMaskPaint
    {
        private readonly DrawingVisual _drawingVisual = new DrawingVisual();
        public SelectMaskPaint()
        {
            AddVisualChild(_drawingVisual);
            IsHitTestVisible = false;
        }

        public void DrawingHandler(Action<DrawingContext> drawingAction)
        {
            var drawingContext = _drawingVisual.RenderOpen();
            drawingAction(drawingContext);
            drawingContext.Close();
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
