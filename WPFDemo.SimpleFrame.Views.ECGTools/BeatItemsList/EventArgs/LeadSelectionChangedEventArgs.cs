using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class LeadSelectionChangedEventArgs : EventArgs
    {
        public IList LeadSelectionChangedItems { get; set; }

        public LeadSelectionChangedEventArgs(IList leadSelectionChangedItems)
        {
            LeadSelectionChangedItems = leadSelectionChangedItems;
        }
    }
}
