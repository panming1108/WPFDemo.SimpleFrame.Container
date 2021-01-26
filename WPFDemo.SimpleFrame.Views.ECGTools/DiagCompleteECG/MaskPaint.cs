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
        private readonly List<DrawingVisual> _visualCollection = new List<DrawingVisual>();

        public void AddDrawingVisual(DrawingVisual drawingVisual)
        {
            AddVisualChild(drawingVisual);
            _visualCollection.Add(drawingVisual);
        }

        public void RemoveDrawingVisual(DrawingVisual drawingVisual)
        {
            RemoveVisualChild(drawingVisual);
            _visualCollection.Remove(drawingVisual);
        }

        public void DrawingHandler(DrawingVisual drawingVisual, Action<DrawingContext> drawingAction)
        {
            var drawingContext = drawingVisual.RenderOpen();
            drawingAction(drawingContext);
            drawingContext.Close();
        }

        protected override int VisualChildrenCount
        {
            get { return _visualCollection.Count; }
        }

        protected override Visual GetVisualChild(int index)
        {
            return _visualCollection[index];
        }
    }
}
