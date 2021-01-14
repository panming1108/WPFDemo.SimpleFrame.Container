using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public interface ISelectItemsContainer : IMoveSelectItemAction
    {
        ItemCollection Items { get; }
        SelectedItemsCollection SelectedItemsCollection { get; }
        void ClearItemsSource();
        void OnItemsControlSelectionChanged(SelectActionEnum selectActionEnum);
    }
}
