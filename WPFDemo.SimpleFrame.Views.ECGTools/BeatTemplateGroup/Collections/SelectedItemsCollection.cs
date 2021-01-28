using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public class SelectedItemsCollection : IDisposable
    {
        public ObservableCollection<string> SelectedItems { get; }

        public BeatTemplateGroupView Container { get; }

        public SelectedItemsCollection(BeatTemplateGroupView container)
        {
            Container = container;
            SelectedItems = new ObservableCollection<string>();
            SelectedItems.CollectionChanged += SelectedItems_CollectionChanged;
        }

        private void SelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        var id = item as string;
                        var itemView = Container.GetItemViewById(id);
                        if (itemView != null)
                        {
                            itemView.IsSelected = true;
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        var id = item as string;
                        var itemView = Container.GetItemViewById(id);
                        if (itemView != null)
                        {
                            itemView.IsSelected = false;
                        }
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

        public bool TryAddItem(string id)
        {
            if (SelectedItems.Contains(id))
            {
                return false;
            }
            else
            {
                SelectedItems.Add(id);
                return true;
            }
        }

        public bool TryRemoveItem(string id)
        {
            if (SelectedItems.Contains(id))
            {
                SelectedItems.Remove(id);
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
            SelectedItems.Clear();
        }

        public void Dispose()
        {
            SelectedItems.CollectionChanged -= SelectedItems_CollectionChanged;
            SelectedItems.Clear();
        }
    }
}
