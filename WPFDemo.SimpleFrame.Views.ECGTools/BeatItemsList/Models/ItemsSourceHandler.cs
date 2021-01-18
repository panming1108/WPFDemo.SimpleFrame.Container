using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class ItemsSourceHandler
    {
        private readonly IBeatItemListViewContainer _beatItemListViewContainer;
        private int[] _itemsSource;
        private KeyValuePair<int, BeatInfo>[] _itemsRealSource;
        public List<int> SelectedItems { get; }
        
        public int[] ItemsSource 
        {
            get => _itemsSource;
            set
            {
                _itemsSource = null;
                _itemsSource = value;
                SelectedItems.Clear();
                _itemsRealSource = null;
                _itemsRealSource = new KeyValuePair<int, BeatInfo>[_itemsSource.Count()];
                for (int i = 0; i < _itemsSource.Count(); i++)
                {
                    var key = _itemsSource[i];
                    _itemsRealSource[i] = new KeyValuePair<int, BeatInfo>(key, BeatInfoSource.AllBeatInfos[key]);
                }
                _beatItemListViewContainer.SelectedCount = SelectedItems.Count;
            }
        }

        public ItemsSourceHandler(IBeatItemListViewContainer beatItemListViewContainer)
        {
            _beatItemListViewContainer = beatItemListViewContainer;
            SelectedItems = new List<int>();
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
            _beatItemListViewContainer.SelectedCount = SelectedItems.Count;
        }

        private void ResetSelectItemsWithOnlyDisplaySelectedItems(IList selectedItems)
        {
            SelectedItems.Clear();
            foreach (var item in selectedItems)
            {
                var itemView = item as ISelectItem;
                var beatInfo = (BeatInfo)itemView.DataContext;
                SelectedItems.Add(beatInfo.R);
            }
        }

        private void ResetSelectedItemsWithOtherSelectItems(IList selectedItems, IList unSelectedItems)
        {
            foreach (var item in selectedItems)
            {
                var itemView = item as ISelectItem;
                var beatInfo = (BeatInfo)itemView.DataContext;
                if (!SelectedItems.Contains(beatInfo.R))
                {
                    SelectedItems.Add(beatInfo.R);
                }
            }
            foreach (var item in unSelectedItems)
            {
                var itemView = item as ISelectItem;
                var beatInfo = (BeatInfo)itemView.DataContext;
                if (SelectedItems.Contains(beatInfo.R))
                {
                    SelectedItems.Remove(beatInfo.R);
                }
            }
        }

        public ItemsPager GetPagerSource(int pageNo, int pageSize)
        {
            var totalPage = GetTotalPage(pageSize);
            var tempPageNo = pageNo;
            if (pageNo > totalPage)
            {
                tempPageNo = totalPage;
            }
            var result = new List<BeatInfo>();
            var rSource = ItemsSource.Skip((pageNo - 1) * pageSize).Take(pageSize);
            foreach (var item in rSource)
            {
                result.Add(BeatInfoSource.AllBeatInfos[item]);
            }
            return new ItemsPager() { PageNo = tempPageNo, PageSize = pageSize, TotalCount = ItemsSource.Count(), Source = result };
        }

        public int GetTotalPage(int pageSize)
        {
            if(ItemsSource.Count() <= 0)
            {
                return 1;
            }
            return ItemsSource.Count() % pageSize == 0 ? ItemsSource.Count() / pageSize : (ItemsSource.Count() / pageSize) + 1;
        }

        public int GetCurrentItemPageNo(int item, int pageSize)
        {
            var index = 0;
            for (int i = 0; i < ItemsSource.Count(); i++)
            {
                if(ItemsSource[i] == item)
                {
                    index = i;
                    break;
                }
            }
            return (index + 1) % pageSize == 0 ? (index + 1) / pageSize : (index + 1) / pageSize + 1;
        }

        public int[] SortItemsSource(bool isAsc)
        {
            if(isAsc)
            {
                return _itemsRealSource.OrderBy(x => x.Value.Interval).Select(x => x.Key).ToArray();
            }
            else
            {
                return _itemsRealSource.OrderByDescending(x => x.Value.Interval).Select(x => x.Key).ToArray();
            }
        }
    }
}
