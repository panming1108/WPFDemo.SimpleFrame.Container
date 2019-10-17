using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using WPFDemo.SimpleFrame.Infra.Helper;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.UXs.DesktopAlert
{
    public class EMCPopupNotifyBox : Popup, IPopupNotifyBox
    {
        protected static List<IPopupNotifyBox> _notifyBoxs = new List<IPopupNotifyBox>();

        protected DispatcherTimer Timer { get; set; }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(EMCPopupNotifyBox), new PropertyMetadata(""));

        public string Info
        {
            get { return (string)GetValue(InfoProperty); }
            set { SetValue(InfoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Info.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InfoProperty =
            DependencyProperty.Register("Info", typeof(string), typeof(EMCPopupNotifyBox), new PropertyMetadata(""));

        public int Interval
        {
            get { return (int)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Interval.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(int), typeof(EMCPopupNotifyBox), new PropertyMetadata(3));

        public EMCPopupNotifyBox()
        {
            Placement = PlacementMode.Absolute;
            Closed += OnPopupClosed;
            Timer = new DispatcherTimer();
            Timer.Tick += OnTickChanged;
        }

        private void OnTickChanged(object sender, EventArgs e)
        {
            IsOpen = false;
        }

        public virtual void OnPopupClosed(object sender, EventArgs e)
        {
            Timer.Stop();
            lock (_notifyBoxs)
            {
                if (_notifyBoxs.Contains(this))
                {
                    _notifyBoxs.Remove(this);
                    SetPosition();
                }
            }
        }

        public virtual void Show(string title, string content)
        {
            DispatcherHelper.InvokeOnUIThread(
                () =>
                {
                    lock (_notifyBoxs)
                    {
                        if (_notifyBoxs.Contains(this))
                        {
                            _notifyBoxs.Remove(this);
                        }

                        Title = title;
                        Info = content;

                        _notifyBoxs.Add(this);
                        SetPosition();

                        this.IsOpen = true;

                        Timer.Interval = TimeSpan.FromSeconds(Interval);
                        Timer.Start();
                    }
                });
        }

        private void SetPosition()
        {
            for (int i = 0; i < _notifyBoxs.Count; i++)
            {
                _notifyBoxs[i].VerticalOffset = SystemParameters.WorkArea.Height - (Height * (i + 1));
                _notifyBoxs[i].HorizontalOffset = SystemParameters.WorkArea.Width - Width;
            }
        }
    }
}
