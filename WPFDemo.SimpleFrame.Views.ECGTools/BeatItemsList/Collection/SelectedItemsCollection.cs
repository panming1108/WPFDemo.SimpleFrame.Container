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
        public ObservableCollection<ISelectItem> SelectedItems { get; }

        public ObservableCollection<ISelectItem> UnSelectedItems => GetUnSelectedItems();

        public ISelectItemsContainer Container { get; }

        public SelectedItemsCollection(ISelectItemsContainer container)
        {
            Container = container;
            SelectedItems = new ObservableCollection<ISelectItem>();
            SelectedItems.CollectionChanged += SelectedItems_CollectionChanged;
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
            if (SelectedItems.Contains(selectItem))
            {
                return false;
            }
            else
            {
                SelectedItems.Add(selectItem);
                return true;
            }
        }

        public bool TryRemoveItem(ISelectItem selectItem)
        {
            if (SelectedItems.Contains(selectItem))
            {
                SelectedItems.Remove(selectItem);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void TryClearItems()
        {
            var count = SelectedItems.Count;
            for (int i = count; i > 0; i--)
            {
                SelectedItems.RemoveAt(i - 1);
            }
        }

        private ObservableCollection<ISelectItem> GetUnSelectedItems()
        {
            ObservableCollection<ISelectItem> result = new ObservableCollection<ISelectItem>();
            foreach (var item in Container.Items)
            {
                var itemView = item as ISelectItem;
                if(!SelectedItems.Contains(itemView))
                {
                    result.Add(itemView);
                }
            }
            return result;
        }
    }
}
