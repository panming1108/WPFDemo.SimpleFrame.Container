using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFDemo.SimpleFrame.Infra.Ioc;
using WPFDemo.SimpleFrame.Infra.Messager;
using WPFDemo.SimpleFrame.IViewModels.ECGTools;
using WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    /// <summary>
    /// BeatItemListView.xaml 的交互逻辑
    /// </summary>
    public partial class BeatItemListViewContainer : UserControl
    {
        private int _pageNo = 1;
        private int _pageSize = 30;
        private int TotalCount => BeatInfoSource.AllBeatInfos.Count;
        private int TotalPage => BeatInfoSource.AllBeatInfos.Count % _pageSize == 0 ? BeatInfoSource.AllBeatInfos.Count / _pageSize : (BeatInfoSource.AllBeatInfos.Count) / _pageSize + 1;
        private string[] LeadSource => new string[] { "I", "II", "III", "aVR", "aVL", "aVF", "V1", "V2", "V3", "V4", "V5", "V6" };

        private readonly ObservableCollection<int> _selectedItems = new ObservableCollection<int>();
        public ObservableCollection<int> SelectedItems => _selectedItems;

        public BeatItemListViewContainer()
        {
            InitializeComponent();
            InitItemsSource();
            InitItemsControlBar();
            KeyDown += BeatItemListViewContainer_KeyDown;
            KeyUp += BeatItemListViewContainer_KeyUp;
            Unloaded += BeatItemListViewContainer_Unloaded;
            MessagerInstance.GetMessager().Register<string>(this, MessagerKeyEnum.UpdateBeat, OnBeatChanged);
            MessagerInstance.GetMessager().Register<string>(this, MessagerKeyEnum.DeleteBeat, OnBeatDeleted);
        }

        private async Task OnBeatDeleted(string arg)
        {
            FreshPage(_pageNo, false);
            await TaskEx.FromResult(0);
        }

        private async Task OnBeatChanged(string arg)
        {
            FreshPage(_pageNo);
            await TaskEx.FromResult(0);
        }

        private void BeatItemListViewContainer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                PART_ItemsControl.SetCtrlKeyStatus(isKeyDown: false);
            }
            else if(e.Key == Key.N || e.Key == Key.S || e.Key == Key.V)
            {
                BeatInfoSource.ChangedBeatInfo(SelectedItems, e.Key.ToString());
                MessagerInstance.GetMessager().Send(MessagerKeyEnum.UpdateBeat, string.Empty);
            }
            else if(e.Key == Key.D)
            {
                BeatInfoSource.DeleteBeatInfos(SelectedItems);
                MessagerInstance.GetMessager().Send(MessagerKeyEnum.DeleteBeat, string.Empty);
            }
        }

        private void BeatItemListViewContainer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                PART_ItemsControl.SetCtrlKeyStatus(isKeyDown: true);
            }
            else if (e.Key == Key.Up || e.Key == Key.Left)
            {
                ItemsControlMoveToPrev();
            }
            else if (e.Key == Key.Down || e.Key == Key.Right)
            {
                ItemsControlMoveToNext();
            }
        }

        private void PART_ItemsControl_ItemsControlSelectionChanged(object sender, ItemsControlSelectionChangedEventArgs e)
        {
            if(e.IsCtrlKeyDown)
            {
                foreach (var item in e.SelectedItems)
                {
                    var itemView = item as ISelectItem;
                    var beatInfoR = (int)itemView.DataContext;
                    if(SelectedItems.Contains(beatInfoR))
                    {
                        SelectedItems.Remove(beatInfoR);
                    }
                    else
                    {
                        SelectedItems.Add(beatInfoR);
                    }
                }
            }
            else
            {
                SelectedItems.Clear();
                foreach (var item in e.SelectedItems)
                {
                    var itemView = item as ISelectItem;
                    var beatInfoR = (int)itemView.DataContext;
                    SelectedItems.Add(beatInfoR);
                }
            }
        }

        private void PART_ItemsControlBar_LeadSelectionChanged(object sender, LeadSelectionChangedEventArgs e)
        {

        }

        private void PART_ItemsControlBar_SelectedAll(object sender, EventArgs e)
        {
            SelectAllItems();
        }

        private void PART_ItemsControlBar_SelectedReverse(object sender, EventArgs e)
        {
            SelectReverseItems();
        }

        private void PART_ItemsControlBar_StrechChanged(object sender, BoolEventArgs e)
        {

        }

        private void PART_ItemsControlBar_SortChanged(object sender, SortEventArgs e)
        {

        }

        private void ItemsControlMoveToNext()
        {
            if (PART_ItemsControl.CanMoveToIndex(PART_ItemsControl.CurrentMoveIndex + 1))
            {
                PART_ItemsControl.MoveToIndex(PART_ItemsControl.CurrentMoveIndex + 1);
            }
            else
            {
                if(_pageNo + 1 > TotalPage)
                {
                    return;
                }
                FreshPage(_pageNo + 1);
            }
        }

        private void ItemsControlMoveToPrev()
        {
            if (PART_ItemsControl.CanMoveToIndex(PART_ItemsControl.CurrentMoveIndex - 1))
            {
                PART_ItemsControl.MoveToIndex(PART_ItemsControl.CurrentMoveIndex - 1);
            }
            else
            {
                if(_pageNo - 1 < 1)
                {
                    return;
                }
                FreshPage(_pageNo - 1);
            }
        }

        private void ItemsControlMoveToCurrentPageIndex(int index)
        {
            if (PART_ItemsControl.CanMoveToIndex(index))
            {
                PART_ItemsControl.MoveToIndex(index);
            }
            else
            {
                ItemsControlMoveToCurrentPageIndex(index - 1);
            }
        }

        private void InitItemsSource()
        {
            SelectAllItems();
        }

        private void SelectAllItems()
        {
            PART_ItemsControlBar.TotalCount = TotalCount;
            SelectedItems.Clear();
            //默认全选
            foreach (var item in BeatInfoSource.AllBeatInfos.Keys)
            {
                SelectedItems.Add(item);
            }
            var source = BeatInfoSource.GetPagerBeatInfo(_pageNo, _pageSize);
            _pageNo = source.Item2;
            PART_CurrentPage.Text = source.Item2.ToString();
            PART_TotalPage.Text = TotalPage.ToString();
            InitItemsControl(source.Item1);
            PART_ItemsControl.CurrentMoveIndex = 0;
        }

        private void SelectReverseItems()
        {
            PART_ItemsControlBar.TotalCount = TotalCount;
            var reverseSelectedItems = BeatInfoSource.AllBeatInfos.Keys.Except(SelectedItems).ToList();
            SelectedItems.Clear();
            foreach (var item in reverseSelectedItems)
            {
                SelectedItems.Add(item);
            }
            var source = BeatInfoSource.GetPagerBeatInfo(_pageNo, _pageSize);
            _pageNo = source.Item2;
            PART_CurrentPage.Text = source.Item2.ToString();
            PART_TotalPage.Text = TotalPage.ToString();
            InitItemsControl(source.Item1);
            PART_ItemsControl.CurrentMoveIndex = 0;
        }

        private void FreshPage(int pageNo, bool isMoveToNext = true)
        {
            //刷新界面
            if(pageNo < 1)
            {
                return;
            }
            PART_ItemsControlBar.TotalCount = TotalCount;
            var source = BeatInfoSource.GetPagerBeatInfo(pageNo, _pageSize);
            PART_TotalPage.Text = TotalPage.ToString();
            InitItemsControl(source.Item1);
            //如果跳转页大于当前页，则选择第一个
            if (source.Item2 > _pageNo)
            {
                PART_ItemsControl.MoveToIndex(0);
                _pageNo = source.Item2;
                PART_CurrentPage.Text = source.Item2.ToString();
            }
            //如果跳转页小于当前页，则选择最后一个
            else if (source.Item2 < _pageNo)
            {
                PART_ItemsControl.MoveToIndex(source.Item1.Count - 1);
                _pageNo = source.Item2;
                PART_CurrentPage.Text = source.Item2.ToString();
            }
            //如果跳转页等于当前页，则选择下一个
            else
            {
                if (isMoveToNext)
                {
                    ItemsControlMoveToNext();
                }
                else
                {
                    ItemsControlMoveToCurrentPageIndex(PART_ItemsControl.CurrentMoveIndex);
                }
            }
        }

        private void InitItemsControl(List<int> beatInfoRs)
        {
            PART_ItemsControl.ClearItemsSource();
            foreach (var item in beatInfoRs)
            {
                ISelectItem itemView = new BeatItemView(PART_ItemsControl)
                {
                    DataContext = item,
                    IsSelected = SelectedItems.Contains(item),
                };
                PART_ItemsControl.Items.Add(itemView);
            }
        }

        private void InitItemsControlBar()
        {
            PART_ItemsControlBar.LeadSource = LeadSource;
            PART_ItemsControlBar.LeadSelectedItems.Add(LeadSource[0]);
            PART_ItemsControlBar.LeadSelectedItems.Add(LeadSource[3]);
        }

        private void BeatItemListViewContainer_Unloaded(object sender, RoutedEventArgs e)
        {
            KeyDown -= BeatItemListViewContainer_KeyDown;
            KeyUp -= BeatItemListViewContainer_KeyUp;
            Unloaded -= BeatItemListViewContainer_Unloaded;
            MessagerInstance.GetMessager().Unregister<string>(this, MessagerKeyEnum.UpdateBeat, OnBeatChanged);
            MessagerInstance.GetMessager().Unregister<string>(this, MessagerKeyEnum.DeleteBeat, OnBeatDeleted);
        }
    }
}
