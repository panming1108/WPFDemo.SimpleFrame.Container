using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.UXs.DesktopAlert
{
    public interface IPopupNotifyBox
    {
        UIElement PlacementTarget { get; set; }
        void Show(string title, string content);
    }
}
