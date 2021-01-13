using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public interface ISelectItemsContainer
    {
        ItemCollection Items { get; }
        SelectedItemsCollection SelectedItemsCollection { get; }
        int CurrentMoveIndex { get; set; }
        void ClearItemsSource();
        void OnItemsControlSelectionChanged(ItemsControlSelectionChangedEventArgs e);
        bool CanMoveToIndex(int index);
        void MoveToIndex(int index);
    }
}
