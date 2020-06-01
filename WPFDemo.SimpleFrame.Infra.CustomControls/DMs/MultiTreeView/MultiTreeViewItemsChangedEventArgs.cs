using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs
{
    public class MultiTreeViewItemsChangedEventArgs : EventArgs
    {
        public IList SelectedItems { get; set; }

        public MultiTreeViewItemsChangedEventArgs(IList selectedItems)
        {
            SelectedItems = selectedItems;
        }
    }
}
