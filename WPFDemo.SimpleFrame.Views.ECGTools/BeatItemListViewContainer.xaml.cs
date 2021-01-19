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
    public partial class BeatItemListViewContainer : UserControl, IBeatItemListViewContainer
    {
        private readonly BeatInfoSource _beatInfoSource;
        private string[] LeadSource => new string[] { "I", "II", "III", "aVR", "aVL", "aVF", "V1", "V2", "V3", "V4", "V5", "V6" };
        private IList _selectedLead;

        public ItemsSourceHandler ItemsSourceHandler { get; }

        public int SelectedCount
        {
            get { return (int)GetValue(SelectedCountProperty); }
            set { SetValue(SelectedCountProperty, value); }
        }
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }
        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }
        public double ItemBigWidth
        {
            get { return (double)GetValue(ItemBigWidthProperty); }
            set { SetValue(ItemBigWidthProperty, value); }
        }
        public double ItemSmallWidth
        {
            get { return (double)GetValue(ItemSmallWidthProperty); }
            set { SetValue(ItemSmallWidthProperty, value); }
        }
        public double ItemBigHeight
        {
            get { return (double)GetValue(ItemBigHeightProperty); }
            set { SetValue(ItemBigHeightProperty, value); }
        }
        public double ItemSmallHeight
        {
            get { return (double)GetValue(ItemSmallHeightProperty); }
            set { SetValue(ItemSmallHeightProperty, value); }
        }

        public static readonly DependencyProperty ItemSmallHeightProperty =
            DependencyProperty.Register(nameof(ItemSmallHeight), typeof(double), typeof(BeatItemListViewContainer), new PropertyMetadata(ConfigSource.ItemSmallHeight));
        public static readonly DependencyProperty ItemBigHeightProperty =
            DependencyProperty.Register(nameof(ItemBigHeight), typeof(double), typeof(BeatItemListViewContainer), new PropertyMetadata(ConfigSource.ItemBigHeight));
        public static readonly DependencyProperty ItemSmallWidthProperty =
            DependencyProperty.Register(nameof(ItemSmallWidth), typeof(double), typeof(BeatItemListViewContainer), new PropertyMetadata(ConfigSource.ItemSmallWidth));
        public static readonly DependencyProperty ItemBigWidthProperty =
            DependencyProperty.Register(nameof(ItemBigWidth), typeof(double), typeof(BeatItemListViewContainer), new PropertyMetadata(ConfigSource.ItemBigWidth));
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register(nameof(ItemWidth), typeof(double), typeof(BeatItemListViewContainer));
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register(nameof(ItemHeight), typeof(double), typeof(BeatItemListViewContainer));
        public static readonly DependencyProperty SelectedCountProperty =
            DependencyProperty.Register(nameof(SelectedCount), typeof(int), typeof(BeatItemListViewContainer));

        private bool _isNeedToMove;

        public BeatItemListViewContainer()
        {
            _beatInfoSource = new BeatInfoSource(840000);
            InitializeComponent();
            ItemsSourceHandler = new ItemsSourceHandler(this, PART_ItemsControlBar, _beatInfoSource);
            InitItemsControlBar();
            MouseWheel += BeatItemListViewContainer_MouseWheel;
            KeyDown += BeatItemListViewContainer_KeyDown;
            KeyUp += BeatItemListViewContainer_KeyUp;
            Unloaded += BeatItemListViewContainer_Unloaded;
            MessagerInstance.GetMessager().Register<string>(this, MessagerKeyEnum.UpdateBeat, OnBeatChanged);
            MessagerInstance.GetMessager().Register<string>(this, MessagerKeyEnum.DeleteBeat, OnBeatDeleted);
        }

        private async Task OnBeatDeleted(string arg)
        {
            var newSource = _beatInfoSource.GetCurrentItemsSource(ItemsSourceHandler.ItemsSource);
            ItemsSourceHandler.SetItemsSource(newSource);
            if (PART_ItemsControlBar.PrevCurrentNextStatus == PrevCurrentNextEnum.Current)
            {
                ItemsSourceHandler.SetOriginItemsSource(newSource);
            }
            ItemsSourceHandler.SelectedItems.Clear();
            PART_ScrollBar.TotalCount = newSource.Count;
            var totalPage = ItemsSourceHandler.GetTotalPage(PART_ScrollBar.PageSize);
            FreshPage(PART_ScrollBar.PageNo > totalPage ? totalPage : PART_ScrollBar.PageNo, false, false);
            await TaskEx.FromResult(0);
        }

        private async Task OnBeatChanged(string arg)
        {
            FreshPage(PART_ScrollBar.PageNo, _isNeedToMove);
            await TaskEx.FromResult(0);
        }

        private void BeatItemListViewContainer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.N || e.Key == Key.S || e.Key == Key.V)
            {
                ItemsSourceHandler.ChangeBeatInfo(e.Key.ToString());
                _isNeedToMove = true;
                MessagerInstance.GetMessager().Send(MessagerKeyEnum.UpdateBeat, string.Empty);
                _isNeedToMove = false;
            }
            else if (e.Key == Key.D)
            {
                ItemsSourceHandler.DeleteBeatInfo();
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
            _selectedLead = e.LeadSelectionChangedItems;
            ChangePageSize();
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
            ChangePageSize();
        }

        private void ChangePageSize()
        {
            var pageSize = GetPageSize(PART_ItemsControlBar.IsStrech, _selectedLead.Count);
            if (pageSize == PART_ScrollBar.PageSize)
            {
                return;
            }
            PART_ScrollBar.PageSize = pageSize;
            if (PART_ItemsControl.Items.Count <= 0)
            {
                FreshPage(1, false, false);
            }
            else
            {
                ISelectItem itemView;
                if(PART_ItemsControl.SelectedItemsCollection.SelectedItems.Count > 0)
                {
                    itemView = PART_ItemsControl.SelectedItemsCollection.SelectedItems[0];
                }
                else
                {
                    itemView = PART_ItemsControl.Items[0] as ISelectItem;
                }
                var beatInfo = itemView.DataContext as BeatInfo;
                var pageNo = ItemsSourceHandler.GetCurrentItemPageNo(beatInfo.R, PART_ScrollBar.PageSize);
                FreshPage(pageNo, false, false);
            }
        }

        private void PART_ItemsControlBar_SortChanged(object sender, SortEventArgs e)
        {
            InitItemsSource(ItemsSourceHandler.ItemsSource.ToList());
        }

        private void ItemsControlMoveToNext()
        {
            if (PART_ItemsControl.CanMoveToIndex(PART_ItemsControl.CurrentMoveIndex + 1))
            {
                PART_ItemsControl.MoveToIndex(PART_ItemsControl.CurrentMoveIndex + 1);
            }
            else
            {
                if (PART_ScrollBar.PageNo + 1 > PART_ScrollBar.TotalPage)
                {
                    return;
                }
                FreshPage(PART_ScrollBar.PageNo + 1, true);
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
                if (PART_ScrollBar.PageNo - 1 < 1)
                {
                    return;
                }
                FreshPage(PART_ScrollBar.PageNo - 1, true);
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

        private void InitItemsSource(List<int> itemsSource)
        {
            ItemsSourceHandler.SetItemsSource(itemsSource);
            SelectAllItems();
        }

        private void SelectAllItems()
        {
            ItemsSourceHandler.SelectedItems.Clear();
            //默认全选
            foreach (var item in ItemsSourceHandler.ItemsSource)
            {
                ItemsSourceHandler.SelectedItems.Add(item);
            }
            InitItemsControl();
            PART_ItemsControl.CurrentMoveIndex = 0;
        }

        private void SelectReverseItems()
        {
            int[] reverseSelectedItems = ItemsSourceHandler.ItemsSource.Except(ItemsSourceHandler.SelectedItems).ToArray();
            ItemsSourceHandler.SelectedItems.Clear();
            foreach (int item in reverseSelectedItems)
            {
                ItemsSourceHandler.SelectedItems.Add(item);
            }
            InitItemsControl();
            PART_ItemsControl.CurrentMoveIndex = 0;
        }

        private void FreshPage(int pageNo, bool isNeedToMove, bool isMoveToNext = true)
        {
            //刷新界面
            if (pageNo != PART_ScrollBar.PageNo)
            {
                _isNeedToMove = isNeedToMove;
                PART_ScrollBar.PageNo = pageNo;
                _isNeedToMove = false;
            }
            else
            {
                InitItemsControl();
                if (PART_ScrollBar.TotalCount <= 0)
                {
                    return;
                }
                if (isNeedToMove)
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
        }

        private void InitItemsControl()
        {
            var pagerSource = ItemsSourceHandler.GetPagerSource(PART_ScrollBar.PageNo, PART_ScrollBar.PageSize);
            SelectedCount = ItemsSourceHandler.SelectedItems.Count;
            PART_ScrollBar.TotalCount = pagerSource.TotalCount;
            PART_ScrollBar.PageNo = pagerSource.PageNo;
            PART_ItemsControl.ClearItemsSource();
            foreach (var item in pagerSource.Source)
            {
                var beatInfo = item as BeatInfo;
                ISelectItem itemView = new BeatItemView(PART_ItemsControl)
                {
                    DataContext = item,
                    IsSelected = ItemsSourceHandler.SelectedItems.Contains(beatInfo.R),
                };
                PART_ItemsControl.Items.Add(itemView);
            }
            GC.Collect();
        }

        private void InitItemsControlBar()
        {
            PART_ItemsControlBar.LeadSource = LeadSource;
            PART_ItemsControlBar.LeadSelectedItems.Add(LeadSource[0]);
            PART_ItemsControlBar.LeadSelectedItems.Add(LeadSource[3]);
        }

        private void BeatItemListViewContainer_Unloaded(object sender, RoutedEventArgs e)
        {
            ItemsSourceHandler.Dispose();
            KeyDown -= BeatItemListViewContainer_KeyDown;
            KeyUp -= BeatItemListViewContainer_KeyUp;
            MouseWheel -= BeatItemListViewContainer_MouseWheel;
            Unloaded -= BeatItemListViewContainer_Unloaded;
            MessagerInstance.GetMessager().Unregister<string>(this, MessagerKeyEnum.UpdateBeat, OnBeatChanged);
            MessagerInstance.GetMessager().Unregister<string>(this, MessagerKeyEnum.DeleteBeat, OnBeatDeleted);
        }

        private void PART_ScrollBar_PageNoChanged(object sender, PageNoChangedEventArgs e)
        {
            InitItemsControl();
            if (_isNeedToMove)
            {
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

        private void PART_ItemsControlBar_PrevCurrentNextChanged(object sender, PrevCurrentNextEventArgs e)
        {
            switch (e.PrevCurrentNext)
            {
                case PrevCurrentNextEnum.Prev:
                    var prevList = _beatInfoSource.GetPrevItemsSource(ItemsSourceHandler.OriginItemsSource);
                    InitItemsSource(prevList);
                    break;
                case PrevCurrentNextEnum.Current:
                    InitItemsSource(ItemsSourceHandler.OriginItemsSource);
                    break;
                case PrevCurrentNextEnum.Next:
                    var nextList = _beatInfoSource.GetNextItemsSource(ItemsSourceHandler.OriginItemsSource);
                    InitItemsSource(nextList);
                    break;
                default:
                    break;
            }
        }

        private void BeatItemListViewContainer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta <= 0)
            {
                //加一页
                if (PART_ScrollBar.PageNo + 1 > PART_ScrollBar.TotalPage)
                {
                    return;
                }
                PART_ScrollBar.PageNo++;
            }
            else
            {
                //减一页
                if (PART_ScrollBar.PageNo - 1 < 1)
                {
                    return;
                }
                PART_ScrollBar.PageNo--;
            }
        }

        private int GetPageSize(bool isStrech, int leadCount)
        {
            ItemWidth = isStrech ? ItemBigWidth : ItemSmallWidth;
            ItemHeight = leadCount >= 3 ? ItemBigHeight : ItemSmallHeight;
            var columnCount = Convert.ToInt32(PART_ItemsControl.ActualWidth / ItemWidth);
            var rowCount = Convert.ToInt32(PART_ItemsControl.ActualHeight / ItemHeight);
            return columnCount * rowCount;
        }

        private void PART_ChangeSourceBtn_Click(object sender, RoutedEventArgs e)
        {
            PART_ScrollBar.PageSize = GetPageSize(PART_ItemsControlBar.IsStrech, _selectedLead.Count);
            PART_ItemsControlBar.PART_Current.IsChecked = false;
            ItemsSourceHandler.SetOriginItemsSource(_beatInfoSource.GenerateItemsSource(int.Parse(PART_ItemsCount.Text)));
            PART_ItemsControlBar.PART_Current.IsChecked = true;
        }

        private void PART_ChangeSourceType_Click(object sender, RoutedEventArgs e)
        {
            _beatInfoSource.ChangedBeatInfo(ItemsSourceHandler.SelectedItems, PART_Type.Text);
            MessagerInstance.GetMessager().Send(MessagerKeyEnum.UpdateBeat, string.Empty);
        }
    }
}
