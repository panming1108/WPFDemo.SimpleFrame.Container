using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.UXs.BusyIndicator
{
    public class EMCBusyIndicator : Dialog.DialogBase, IEMCBusyIndicator
    {
        public EMCBusyIndicator()
        {
            this.Style = FindResource("EMCBusyIndicatorStyle") as Style;
        }
    }
}
