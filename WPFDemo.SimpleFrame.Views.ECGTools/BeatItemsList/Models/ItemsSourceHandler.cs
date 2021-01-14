using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class ItemsSourceHandler<T, T1>
    {
        private IBeatItemListViewContainer<T, T1> _beatItemListViewContainer;
        private readonly ObservableCollection<T> _selectedItems;
        public ObservableCollection<T> SelectedItems => _selectedItems;
        private Dictionary<T,T1> ItemsSource => _beatItemListViewContainer.ItemsSource;


        public ItemsSourceHandler(IBeatItemListViewContainer<T, T1> beatItemListViewContainer)
        {
            _beatItemListViewContainer = beatItemListViewContainer;
            _selectedItems = new ObservableCollection<T>();
        }

        public void OnItemsControlSelectionChanged(ItemsControlSelectionChangedEventArgs e)
        {
            switch (e.SelectActionMode)
            {
                case SelectActionEnum.None:
                    SelectedItems.Clear();
                    foreach (var item in e.SelectedItems)
                    {
                        var itemView = item as ISelectItem;
                        var beatInfoR = (T)itemView.DataContext;
                        SelectedItems.Add(beatInfoR);
                    }
                    break;
                case SelectActionEnum.Ctrl:
                    foreach (var item in e.SelectedItems)
                    {
                        var itemView = item as ISelectItem;
                        var beatInfoR = (T)itemView.DataContext;
                        if (SelectedItems.Contains(beatInfoR))
                        {
                            SelectedItems.Remove(beatInfoR);
                        }
                        else
                        {
                            SelectedItems.Add(beatInfoR);
                        }
                    }
                    break;
                case SelectActionEnum.Shift:
                    break;
                default:
                    break;
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
            return new ItemsPager<T>() { PageNo = tempPageNo, PageSize = pageSize, TotalCount = ItemsSource.Count, Source = ItemsSource.Keys.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList() };
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
