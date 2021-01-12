using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class SelectedItemsCollection
    {
        private readonly ISelectItemsContainer _container;
        private readonly ObservableCollection<ISelectItem> _selectedItems;
        public ObservableCollection<ISelectItem> SelectedItems => _selectedItems;

        public ISelectItemsContainer Container => _container;

        public SelectedItemsCollection(ISelectItemsContainer container)
        {
            _container = container;
            _selectedItems = new ObservableCollection<ISelectItem>();
            _selectedItems.CollectionChanged += SelectedItems_CollectionChanged;
        }

        private void SelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        var itemView = item as ISelectItem;
                        itemView.IsSelected = true;
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        var itemView = item as ISelectItem;
                        itemView.IsSelected = false;
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        public bool TryAddItem(ISelectItem selectItem)
        {
            if (_selectedItems.Contains(selectItem))
            {
                return false;
            }
            else
            {
                _selectedItems.Add(selectItem);
                return true;
            }
        }

        public bool TryRemoveItem(ISelectItem selectItem)
        {
            if (_selectedItems.Contains(selectItem))
            {
                _selectedItems.Remove(selectItem);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void TryClearItems()
        {
            var count = _selectedItems.Count;
            for (int i = count; i > 0; i--)
            {
                _selectedItems.RemoveAt(i - 1);
            }
        }
    }
}
