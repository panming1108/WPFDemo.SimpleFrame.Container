using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DVs.FanChart
{
    public class FanCollection : ObservableCollection<Fan>
    {
        private Ring _ownerRing;
        public FanCollection(Ring ring)
        {
            _ownerRing = ring;            
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    UpdateFanAngle();
                    UpdateFanRadius();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    UpdateFanAngle();
                    UpdateFanRadius();
                    break;
                case NotifyCollectionChangedAction.Replace:
                    UpdateFanAngle();
                    UpdateFanRadius();
                    break;
                default:
                    break;
            }          
        }

        internal void UpdateFanAngle()
        {
            using (IEnumerator<Fan> enumerator = GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Fan current = enumerator.Current;
                    if (IndexOf(current) == 0)
                    {
                        current.StartAngle = _ownerRing.StartAngle;
                    }
                    else
                    {
                        current.StartAngle = this[IndexOf(current) - 1].EndAngle;
                    }
                    current.InvalidateVisual();
                }
            }
        }

        internal void UpdateFanRadius()
        {
            var radius = _ownerRing.Radius;
            using (IEnumerator<Fan> enumerator = GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Fan current = enumerator.Current;
                    current.Radius = radius;
                    current.InvalidateVisual();
                }
            }
        }
    }
}
