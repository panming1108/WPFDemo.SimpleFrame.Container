using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class ItemsSourceHandler<T>
    {
        private IBeatItemListViewContainer<T> _beatItemListViewContainer;

        public ObservableCollection<T> SelectedItems { get; }
        private List<T> ItemsSource => _beatItemListViewContainer.ItemsSource;


        public ItemsSourceHandler(IBeatItemListViewContainer<T> beatItemListViewContainer)
        {
            _beatItemListViewContainer = beatItemListViewContainer;
            SelectedItems = new ObservableCollection<T>();
        }

        public void OnItemsControlSelectionChanged(ItemsControlSelectionChangedEventArgs e)
        {
            switch (e.SelectActionMode)
            {
                case SelectActionEnum.None:
                    ResetSelectItemsWithOnlyDisplaySelectedItems(e.SelectedItems);
                    break;
                case SelectActionEnum.Ctrl:
                    ResetSelectedItemsWithOtherSelectItems(e.SelectedItems, e.UnSelectedItems);
                    break;
                case SelectActionEnum.Shift:
                    break;
                default:
                    break;
            }
        }

        private void ResetSelectItemsWithOnlyDisplaySelectedItems(IList selectedItems)
        {
            SelectedItems.Clear();
            foreach (var item in selectedItems)
            {
                var itemView = item as ISelectItem;
                var beatInfoR = (T)itemView.DataContext;
                SelectedItems.Add(beatInfoR);
            }
        }

        private void ResetSelectedItemsWithOtherSelectItems(IList selectedItems, IList unSelectedItems)
        {
            foreach (var item in selectedItems)
            {
                var itemView = item as ISelectItem;
                var beatInfoR = (T)itemView.DataContext;
                if (!SelectedItems.Contains(beatInfoR))
                {
                    SelectedItems.Add(beatInfoR);
                }
            }
            foreach (var item in unSelectedItems)
            {
                var itemView = item as ISelectItem;
                var beatInfoR = (T)itemView.DataContext;
                if (SelectedItems.Contains(beatInfoR))
                {
                    SelectedItems.Remove(beatInfoR);
                }
            }
        }

        public ItemsPager<T> GetPagerSource(int pageNo, int pageSize)
        {
            var totalPage = GetTotalPage(pageSize);
            var tempPageNo = pageNo;
            if (pageNo > totalPage)
            {
                tempPageNo = totalPage;
            }
            return new ItemsPager<T>() { PageNo = tempPageNo, PageSize = pageSize, TotalCount = ItemsSource.Count, Source = ItemsSource.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList() };
        }

        public int GetTotalPage(int pageSize)
        {
            if(ItemsSource.Count <= 0)
            {
                return 1;
            }
            return ItemsSource.Count % pageSize == 0 ? ItemsSource.Count / pageSize : (ItemsSource.Count / pageSize) + 1;
        }
    }
}
