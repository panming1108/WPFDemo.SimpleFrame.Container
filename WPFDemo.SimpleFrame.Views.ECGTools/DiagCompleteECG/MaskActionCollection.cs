using System;
using System.Collections.Generic;
using System.Windows;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public class MaskActionCollection : IDisposable
    {
        private List<MaskActionBase> _masks;
        public List<MaskActionBase> Masks => _masks;
        public MaskActionCollection()
        {
            _masks = new List<MaskActionBase>();
        }
        public void Dispose()
        {
            foreach (var item in _masks)
            {
                item.Dispose();
            }
            _masks.Clear();
        }

        public MaskActionBase GetCurrentMask(Point point)
        {
            MaskActionBase resultMask = null;
            foreach (var item in _masks)
            {
                foreach (var drawing in item.DrawingChildren)
                {
                    Rect rect = new Rect(drawing.Bounds.X - 2.5, drawing.Bounds.Y - 2.5, drawing.Bounds.Width + 5, drawing.Bounds.Height + 5);
                    if (rect.Contains(point))
                    {
                        resultMask = item;
                        break;
                    }
                }
            }
            return resultMask;
        }
        public void Add(MaskActionBase maskAction)
        {
            _masks.Add(maskAction);
            maskAction.InitMask();
        }
        public void Remove(MaskActionBase maskAction)
        {
            maskAction.ResetMask();
            _masks.Remove(maskAction);
        }

        public bool Contains(MaskActionBase maskAction)
        {
            return _masks.Contains(maskAction);
        }
    }
}
