using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public class MaskActionCollection : IDisposable
    {
        private Dictionary<MaskActionBase, int> _screenDragMasksDic;
        private Dictionary<MaskActionBase, int> _screenMouseUpMasksDic;

        private List<MaskActionBase> _masks;
        public List<MaskActionBase> Masks => _masks;

        private MaskActionBase _mouseOverMask;
        public MaskActionBase MouseOverMask => _mouseOverMask;

        private MaskActionBase _dragMask;
        public MaskActionBase DragMask => _dragMask;

        private MaskActionBase _mouseUpMask;
        public MaskActionBase MouseUpMask => _mouseUpMask;

        public MaskActionCollection()
        {
            _masks = new List<MaskActionBase>();
            _screenDragMasksDic = new Dictionary<MaskActionBase, int>();
            _screenMouseUpMasksDic = new Dictionary<MaskActionBase, int>();
        }
        public void Dispose()
        {
            foreach (var item in _masks)
            {
                item.Dispose();
            }
            _masks.Clear();
        }

        public void SetMouseOverMask(Point point)
        {
            _mouseOverMask = null;
            foreach (var item in _masks)
            {
                foreach (var drawing in item.DrawingChildren)
                {
                    Rect rect = new Rect(drawing.Bounds.X - 2.5, drawing.Bounds.Y - 2.5, drawing.Bounds.Width + 5, drawing.Bounds.Height + 5);
                    if (rect.Contains(point))
                    {
                        _mouseOverMask = item;
                        return;
                    }
                }
            }
        }

        public void SetScreenDragMask()
        {
            _dragMask = _mouseOverMask;
            if(_dragMask is IScreenDragAction)
            {
                var tempDragMask = _screenDragMasksDic.Where(x => x.Value == _screenDragMasksDic.Max(w => w.Value)).LastOrDefault();
                _dragMask = tempDragMask.Key;
            }
        }

        public void SetScreenMouseUpMask()
        {
            _mouseUpMask = _mouseOverMask;
            if (_mouseUpMask is IScreenMouseUpAction)
            {
                var tempMouseUpMask = _screenMouseUpMasksDic.Where(x => x.Value == _screenMouseUpMasksDic.Max(w => w.Value)).LastOrDefault();
                _mouseUpMask = tempMouseUpMask.Key;
            }
        }

        public void Add(MaskActionBase maskAction)
        {
            _masks.Insert(0, maskAction);
            if(maskAction is IScreenDragAction)
            {
                _screenDragMasksDic.Add(maskAction, ((IScreenDragAction)maskAction).DragPriority);
            }
            if(maskAction is IScreenMouseUpAction)
            {
                _screenMouseUpMasksDic.Add(maskAction, ((IScreenMouseUpAction)maskAction).MouseUpPriority);
            }
            maskAction.InitMask();
        }
        public void Remove(MaskActionBase maskAction)
        {
            maskAction.ResetMask();
            _masks.Remove(maskAction);
            if (maskAction is IScreenDragAction)
            {
                _screenDragMasksDic.Remove(maskAction);
            }
            if (maskAction is IScreenMouseUpAction)
            {
                _screenMouseUpMasksDic.Remove(maskAction);
            }
        }

        public bool Contains(MaskActionBase maskAction)
        {
            return _masks.Contains(maskAction);
        }
    }
}
