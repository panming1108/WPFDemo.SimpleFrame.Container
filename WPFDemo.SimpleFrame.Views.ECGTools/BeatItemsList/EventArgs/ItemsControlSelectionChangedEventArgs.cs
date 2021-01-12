using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class ItemsControlSelectionChangedEventArgs : EventArgs
    {
        public bool IsCtrlKeyDown { get; set; }
        public IList SelectedItems { get; set; }

        public ItemsControlSelectionChangedEventArgs(IList selectedItems, bool isCtrlKeyDown)
        {
            SelectedItems = selectedItems;
            IsCtrlKeyDown = isCtrlKeyDown;
        }
    }
}
