using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFDemo.SimpleFrame.Infra.Messager;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class BeatDetailAction : IBeatDetailAction, IDisposable
    {
        private SortArgs _currentSortArgs;
        private readonly List<int> _itemsSource = new List<int>();
        private readonly List<int> _originItemsSource = new List<int>();
        private readonly List<int> _selectedItems = new List<int>();

        public BeatDetailAction()
        {
            MessagerInstance.GetMessager().Register<List<int>>(this, "SetBeatDetailItemsSource", OnSetBeatDetailItemsSource);
        }

        private async Task OnSetBeatDetailItemsSource(List<int> source)
        {
            foreach (var item in source)
            {
                _originItemsSource.Add(item);
            }
            await TaskEx.FromResult(0);
        }

        public void ChangeBeatInfo(string key)
        {
            if (_selectedItems.Count <= 0)
            {
                return;
            }
            if (key == Key.N.ToString() || key == Key.S.ToString() || key == Key.V.ToString())
            {
                BeatInfoSource.BeatSource.ChangedBeatInfo(_selectedItems, (BeatTypeEnum)Enum.Parse(typeof(BeatTypeEnum), key));
                MessagerInstance.GetMessager().Send(MessagerKeyEnum.UpdateBeat, string.Empty);
            }
            else if (key == Key.D.ToString())
            {
                foreach (var item in _selectedItems)
                {
                    _originItemsSource.Remove(item);
                    _itemsSource.Remove(item);
                }
                _selectedItems.Clear();
                BeatInfoSource.BeatSource.DeleteBeatInfos(_selectedItems);
                MessagerInstance.GetMessager().Send(MessagerKeyEnum.DeleteBeat, string.Empty);
            }
        }

        public int GetCurrentItemPageNo(int r, int pageSize)
        {
            var index = 0;
            for (int i = 0; i < _itemsSource.Count(); i++)
            {
                if (_itemsSource[i] == r)
                {
                    index = i;
                    break;
                }
            }
            return (index + 1) % pageSize == 0 ? (index + 1) / pageSize : (index + 1) / pageSize + 1;
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
            var rSource = _itemsSource.Skip((pageNo - 1) * pageSize).Take(pageSize);
            foreach (var item in rSource)
            {
                result.Add(BeatInfoSource.BeatSource.AllBeatInfoDic[item].First());
            }
            return new ItemsPager() { PageNo = tempPageNo, PageSize = pageSize, TotalCount = _itemsSource.Count(), Source = result };
        }

        public int GetTotalPage(int pageSize)
        {
            if (_itemsSource.Count() <= 0)
            {
                return 1;
            }
            return _itemsSource.Count() % pageSize == 0 ? _itemsSource.Count() / pageSize : (_itemsSource.Count() / pageSize) + 1;
        }

        public void PrevCurrentNextChanged(int prevCurrentNext)
        {
            _itemsSource.Clear();
            foreach (var item in _originItemsSource)
            {
                if (BeatInfoSource.BeatSource.AllBeatInfoDic.TryGetValue(item, out List<BeatInfo> beatList))
                {
                    var beat = beatList[(int)prevCurrentNext];
                    if (beat != null)
                    {
                        _itemsSource.Add(beat.R);
                    }
                }
            }
            SortItemsSource(_currentSortArgs);
        }

        public int ResetSelectedItemsWithOtherSelectItems(IList selectedItems, IList unSelectedItems)
        {
            foreach (var item in selectedItems)
            {
                var itemView = item as ISelectItem;
                var beatInfo = (BeatInfo)itemView.DataContext;
                if (!_selectedItems.Contains(beatInfo.R))
                {
                    _selectedItems.Add(beatInfo.R);
                }
            }
            foreach (var item in unSelectedItems)
            {
                var itemView = item as ISelectItem;
                var beatInfo = (BeatInfo)itemView.DataContext;
                if (_selectedItems.Contains(beatInfo.R))
                {
                    _selectedItems.Remove(beatInfo.R);
                }
            }
            return _selectedItems.Count;
        }

        public int ResetSelectItemsWithOnlyDisplaySelectedItems(IList selectedItems)
        {
            _selectedItems.Clear();
            foreach (var item in selectedItems)
            {
                var itemView = item as ISelectItem;
                var beatInfo = (BeatInfo)itemView.DataContext;
                _selectedItems.Add(beatInfo.R);
            }
            return _selectedItems.Count;
        }

        public int SelectAll()
        {
            _selectedItems.Clear();
            foreach (var item in _itemsSource)
            {
                _selectedItems.Add(item);
            }
            return _selectedItems.Count;
        }

        public int SelectReverse()
        {
            int[] reverseSelectedItems = _itemsSource.Except(_selectedItems).ToArray();
            _selectedItems.Clear();
            foreach (int item in reverseSelectedItems)
            {
                _selectedItems.Add(item);
            }
            return _selectedItems.Count;
        }

        public void SortItemsSource(SortArgs sortArgs)
        {
            _currentSortArgs = sortArgs;
            if(_itemsSource.Count <= 0)
            {
                return;
            }
            var result = BeatInfoSource.BeatSource.SortItemsSource(_itemsSource, sortArgs);
            _itemsSource.Clear();
            foreach (var item in result)
            {
                _itemsSource.Add((int)item);
            }
        }

        public bool SelectedItemsContainsR(int r)
        {
            return _selectedItems.Contains(r);
        }

        public int GetSelectedItemsCount()
        {
            return _selectedItems.Count;
        }

        public void Dispose()
        {
            MessagerInstance.GetMessager().Unregister<List<int>>(this, "SetBeatDetailItemsSource", OnSetBeatDetailItemsSource);
        }
    }
}
