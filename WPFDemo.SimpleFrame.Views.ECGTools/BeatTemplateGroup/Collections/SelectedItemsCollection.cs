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
        public ObservableCollection<BeatTemplateItemView> SelectedItems { get; }

        public ObservableCollection<BeatTemplateItemView> UnSelectedItems => GetUnSelectedItems();

        public BeatTemplateGroupView Container { get; }

        public SelectedItemsCollection(BeatTemplateGroupView container)
        {
            Container = container;
            SelectedItems = new ObservableCollection<BeatTemplateItemView>();
            SelectedItems.CollectionChanged += SelectedItems_CollectionChanged;
        }

        private void SelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        var itemView = item as BeatTemplateItemView;
                        itemView.IsSelected = true;
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        var itemView = item as BeatTemplateItemView;
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

        public bool TryAddItem(BeatTemplateItemView selectItem)
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

        public bool TryRemoveItem(BeatTemplateItemView selectItem)
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
            SelectedItems.Clear();
        }

        private ObservableCollection<BeatTemplateItemView> GetUnSelectedItems()
        {
            ObservableCollection<BeatTemplateItemView> result = new ObservableCollection<BeatTemplateItemView>();
            foreach (var groupItem in Container.GroupItems)
            {
                var groupItemView = groupItem as BeatTemplateGroupItemView;
                foreach (var item in groupItemView.Items)
                {
                    var itemView = item as BeatTemplateItemView;
                    if (!SelectedItems.Contains(itemView))
                    {
                        result.Add(itemView);
                    }
                }
            }
            return result;
        }

        public void Dispose()
        {
            SelectedItems.CollectionChanged -= SelectedItems_CollectionChanged;
            SelectedItems.Clear();
        }
    }
}
