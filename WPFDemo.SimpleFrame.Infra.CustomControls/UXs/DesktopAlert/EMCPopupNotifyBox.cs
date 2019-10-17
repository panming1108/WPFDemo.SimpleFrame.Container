using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.UXs.DesktopAlert
{
    public class EMCPopupNotifyBox : Popup, IPopupNotifyBox
    {
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

        public virtual void Show(string title, string content)
        {
            Title = title;
            Info = content;

            VerticalOffset = SystemParameters.WorkArea.Height - Height;
            HorizontalOffset = SystemParameters.WorkArea.Width - Width;

            this.IsOpen = true;
        }
    }
}
