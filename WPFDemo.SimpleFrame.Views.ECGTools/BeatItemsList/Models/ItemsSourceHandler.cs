using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class ItemsSourceHandler : IDisposable
    {
        private readonly BeatInfoSource _beatInfoSource;
        private readonly IBeatItemListViewContainer _beatItemListViewContainer;
        private readonly List<int> _itemsSource;
        private readonly List<int> _originItemsSource;
        private readonly List<BeatInfo> _itemsDataSource;
        public List<int> SelectedItems { get; }

        public List<int> ItemsSource => _itemsSource;
        public List<int> OriginItemsSource => _originItemsSource.ToList();

        public ItemsSourceHandler(IBeatItemListViewContainer beatItemListViewContainer, BeatInfoSource beatInfoSource)
        {
            _beatInfoSource = beatInfoSource;
            _beatItemListViewContainer = beatItemListViewContainer;
            _itemsSource = new List<int>();
            _originItemsSource = new List<int>();
            _itemsDataSource = new List<BeatInfo>();
            SelectedItems = new List<int>();
        }

        public void SetItemsSource(IEnumerable itemsSource)
        {
            _itemsSource.Clear();
            _itemsDataSource.Clear();
            SelectedItems.Clear();
            foreach (var item in itemsSource)
            {
                int key = (int)item;
                _itemsSource.Add(key);
                _itemsDataSource.Add(_beatInfoSource.AllBeatInfoDic[key].First());
            }
            _beatItemListViewContainer.SelectedCount = SelectedItems.Count;
        }

        public void SetOriginItemsSource(IEnumerable itemsSource)
        {
            _originItemsSource.Clear();
            foreach (var item in itemsSource)
            {
                int key = (int)item;
                _originItemsSource.Add(key);
            }
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
                result.Add(_beatInfoSource.AllBeatInfoDic[item].First());
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

        public List<int> SortItemsSource(bool isAsc)
        {
            if(isAsc)
            {
                return _itemsDataSource.OrderBy(x => x.Interval).Select(x => x.R).ToList();
            }
            else
            {
                return _itemsDataSource.OrderByDescending(x => x.Interval).Select(x => x.R).ToList();
            }
        }

        public void ChangeBeatInfo(string type)
        {
            if(SelectedItems.Count <= 0)
            {
                return;
            }
            _beatInfoSource.ChangedBeatInfo(SelectedItems, type);
        }

        public void DeleteBeatInfo()
        {
            if (SelectedItems.Count <= 0)
            {
                return;
            }
            _beatInfoSource.DeleteBeatInfos(SelectedItems);
            foreach (var item in SelectedItems)
            {
                ItemsSource.Remove(item);
            }
        }

        public void Dispose()
        {
            ItemsSource.Clear();
            SelectedItems.Clear();
            _itemsDataSource.Clear();
            GC.Collect();
        }
    }
}
