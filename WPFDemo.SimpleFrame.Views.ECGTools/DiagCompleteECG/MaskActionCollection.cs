using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public class MaskActionCollection : ObservableCollection<MaskActionBase>
    {
        private FrameworkElement _ownerElement;
        public MaskActionCollection(FrameworkElement ownerElement)
        {
            _ownerElement = ownerElement;
        }

        public MaskActionBase GetCurrentMask(Point point)
        {
            MaskActionBase resultMask = null;
            using (IEnumerator<MaskActionBase> enumerator = GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    foreach (var drawing in enumerator.Current.DrawingChildren)
                    {
                        Rect rect = new Rect(drawing.Bounds.X - 2.5, drawing.Bounds.Y - 2.5, drawing.Bounds.Width + 5, drawing.Bounds.Height + 5);
                        if (rect.Contains(point))
                        {
                            resultMask = enumerator.Current;
                            break;
                        }
                    }
                }
            }
            return resultMask ?? this.First();
        }
    }
}
