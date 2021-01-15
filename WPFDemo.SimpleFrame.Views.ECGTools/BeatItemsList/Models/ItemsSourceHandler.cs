﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class ItemsSourceHandler
    {
        private IBeatItemListViewContainer _beatItemListViewContainer;

        public List<int> SelectedItems { get; }
        private int[] ItemsSource => _beatItemListViewContainer.ItemsSource;


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
    }
}
