using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class ClickSelectionChangedEventArgs : EventArgs
    {
        public object SelectedItem { get; set; }

        public ClickSelectionChangedEventArgs(object selectedItem)
        {
            SelectedItem = selectedItem;
        }
    }
}
