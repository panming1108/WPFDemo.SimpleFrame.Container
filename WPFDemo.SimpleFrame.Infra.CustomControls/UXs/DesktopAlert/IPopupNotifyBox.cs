using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.UXs.DesktopAlert
{
    public interface IPopupNotifyBox
    {
        void Show(string title, string content);
    }
}
