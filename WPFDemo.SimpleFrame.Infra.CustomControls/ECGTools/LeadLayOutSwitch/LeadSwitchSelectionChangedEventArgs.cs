using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.ECGTools
{
    public class LeadSwitchSelectionChangedEventArgs : EventArgs
    {
        public IList SelectedItems { get; set; }

        public LeadSwitchSelectionChangedEventArgs(IList selectedItems)
        {
            SelectedItems = selectedItems;
        }
    }
}
