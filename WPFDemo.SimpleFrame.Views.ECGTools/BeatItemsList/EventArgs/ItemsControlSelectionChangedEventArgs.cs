using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class ItemsControlSelectionChangedEventArgs : EventArgs
    {
        public SelectActionEnum SelectActionMode { get; set; }
        public IList SelectedItems { get; set; }

        public ItemsControlSelectionChangedEventArgs(IList selectedItems, SelectActionEnum selectActionMode)
        {
            SelectedItems = selectedItems;
            SelectActionMode = selectActionMode;
        }
    }
}
