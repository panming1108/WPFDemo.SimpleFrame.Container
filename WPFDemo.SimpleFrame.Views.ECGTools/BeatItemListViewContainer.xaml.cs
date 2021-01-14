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
    public partial class BeatItemListViewContainer : UserControl, IBeatItemListViewContainer<int, BeatInfo>
    {
        private string[] LeadSource => new string[] { "I", "II", "III", "aVR", "aVL", "aVF", "V1", "V2", "V3", "V4", "V5", "V6" };

        public Dictionary<int, BeatInfo> ItemsSource => BeatInfoSource.AllBeatInfos;

        public ItemsSourceHandler<int, BeatInfo> ItemsSourceHandler { get; }
        public ObservableCollection<int> SelectedItems => ItemsSourceHandler.SelectedItems;

        public BeatItemListViewContainer()
        {
            ItemsSourceHandler = new ItemsSourceHandler<int, BeatInfo>(this);
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
            PART_ScrollBar.TotalCount = BeatInfoSource.AllBeatInfos.Count;
            var totalPage = ItemsSourceHandler.GetTotalPage(PART_ScrollBar.PageSize);
            if(PART_ScrollBar.PageNo > totalPage)
            {
                PART_ScrollBar.PageNo = totalPage;
            }
            else
            {
                FreshPage(PART_ScrollBar.PageNo, false);
            }
            await TaskEx.FromResult(0);
        }

        private async Task OnBeatChanged(string arg)
        {
            FreshPage(PART_ScrollBar.PageNo);
            await TaskEx.FromResult(0);
        }

        private void BeatItemListViewContainer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.N || e.Key == Key.S || e.Key == Key.V)
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
            if (e.Key == Key.Up || e.Key == Key.Left)
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
            ItemsSourceHandler.OnItemsControlSelectionChanged(e);
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
                if(PART_ScrollBar.PageNo + 1 > PART_ScrollBar.TotalPage)
                {
                    return;
                }
                FreshPage(PART_ScrollBar.PageNo + 1);
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
                if(PART_ScrollBar.PageNo - 1 < 1)
                {
                    return;
                }
                FreshPage(PART_ScrollBar.PageNo - 1);
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
            SelectedItems.Clear();
            //默认全选
            foreach (var item in BeatInfoSource.AllBeatInfos.Keys)
            {
                SelectedItems.Add(item);
            }
            var pagerSource = ItemsSourceHandler.GetPagerSource(PART_ScrollBar.PageNo, PART_ScrollBar.PageSize);
            InitItemsControl(pagerSource);
            PART_ItemsControl.CurrentMoveIndex = 0;
        }

        private void SelectReverseItems()
        {
            var reverseSelectedItems = BeatInfoSource.AllBeatInfos.Keys.Except(SelectedItems).ToList();
            SelectedItems.Clear();
            foreach (var item in reverseSelectedItems)
            {
                SelectedItems.Add(item);
            }
            var pagerSource = ItemsSourceHandler.GetPagerSource(PART_ScrollBar.PageNo, PART_ScrollBar.PageSize);
            InitItemsControl(pagerSource);
            PART_ItemsControl.CurrentMoveIndex = 0;
        }

        private void FreshPage(int pageNo, bool isMoveToNext = true)
        {
            //刷新界面
            if(pageNo != PART_ScrollBar.PageNo)
            {
                PART_ScrollBar.PageNo = pageNo;
            }
            else
            {
                var pagerSource = ItemsSourceHandler.GetPagerSource(PART_ScrollBar.PageNo, PART_ScrollBar.PageSize);
                InitItemsControl(pagerSource);
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

        private void InitItemsControl(ItemsPager<int> itemsPager)
        {
            PART_ScrollBar.TotalCount = itemsPager.TotalCount;
            PART_ScrollBar.PageNo = itemsPager.PageNo;
            PART_ItemsControl.ClearItemsSource();
            foreach (var item in itemsPager.Source)
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

        private void PART_ScrollBar_PageNoChanged(object sender, PageNoChangedEventArgs e)
        {
            var pagerSource = ItemsSourceHandler.GetPagerSource(PART_ScrollBar.PageNo, PART_ScrollBar.PageSize);
            InitItemsControl(pagerSource);
            //如果跳转页大于当前页，则选择第一个
            if (e.NewPageNo > e.OldPageNo)
            {
                PART_ItemsControl.MoveToIndex(0);
            }
            //如果跳转页小于当前页，则选择最后一个
            else if (e.NewPageNo < e.OldPageNo)
            {
                PART_ItemsControl.MoveToIndex(PART_ItemsControl.Items.Count - 1);
            }
        }
    }
}
