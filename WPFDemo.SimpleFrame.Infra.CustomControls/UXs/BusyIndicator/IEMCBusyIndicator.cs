using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.UXs.BusyIndicator
{
    public interface IEMCBusyIndicator
    {
        Window WindowOwner { get; set; }
        void Show();
        void Hide();
    }
}
