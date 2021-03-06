﻿using System;
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
using WPFDemo.SimpleFrame.Views.ECGTools.ContextMenuHelper;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    /// <summary>
    /// BeatItemListView.xaml 的交互逻辑
    /// </summary>
    public partial class BeatItemListViewContainer : UserControl, IBeatItemListViewContainer
    {
        private bool _isLoadControl;
        private readonly BeatInfoSource _beatInfoSource;
        private readonly BeatDetailViewContextMenuHelpler _beatDetailViewContextMenuHelpler;
        private string[] LeadSource => new string[] { "I", "II", "III", "aVR", "aVL", "aVF", "V1", "V2", "V3", "V4", "V5", "V6" };

        public int SelectedCount
        {
            get { return (int)GetValue(SelectedCountProperty); }
            set { SetValue(SelectedCountProperty, value); }
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

        private IBeatDetailAction _beatDetailAction;
        public IBeatDetailAction BeatDetailAction => _beatDetailAction;

        public static readonly DependencyProperty ItemSmallHeightProperty =
            DependencyProperty.Register(nameof(ItemSmallHeight), typeof(double), typeof(BeatItemListViewContainer), new PropertyMetadata(ConfigSource.ItemSmallHeight));
        public static readonly DependencyProperty ItemBigHeightProperty =
            DependencyProperty.Register(nameof(ItemBigHeight), typeof(double), typeof(BeatItemListViewContainer), new PropertyMetadata(ConfigSource.ItemBigHeight));
        public static readonly DependencyProperty ItemSmallWidthProperty =
            DependencyProperty.Register(nameof(ItemSmallWidth), typeof(double), typeof(BeatItemListViewContainer), new PropertyMetadata(ConfigSource.ItemSmallWidth));
        public static readonly DependencyProperty ItemBigWidthProperty =
            DependencyProperty.Register(nameof(ItemBigWidth), typeof(double), typeof(BeatItemListViewContainer), new PropertyMetadata(ConfigSource.ItemBigWidth));
        public static readonly DependencyProperty SelectedCountProperty =
            DependencyProperty.Register(nameof(SelectedCount), typeof(int), typeof(BeatItemListViewContainer));

        private bool _isNeedToMove;

        public BeatItemListViewContainer()
        {
            _beatInfoSource = BeatInfoSource.BeatSource;
            _beatDetailAction = new BeatDetailAction();
            _beatDetailViewContextMenuHelpler = new BeatDetailViewContextMenuHelpler(OnUpdateBeatHandler, OnDeleteBeatHandler, OnInsertBeatToPrevHandler, OnInsertBeatToNextHandler, OnUnConfuseHandler, OnSetLongestRRHandler, OnSignAFHandler);
            InitializeComponent();
            InitControl();
            InitContextMenuItems();
            MouseWheel += BeatItemListViewContainer_MouseWheel;
            KeyDown += BeatItemListViewContainer_KeyDown;
            KeyUp += BeatItemListViewContainer_KeyUp;
            Unloaded += BeatItemListViewContainer_Unloaded;
            MessagerInstance.GetMessager().Register<string>(this, MessagerKeyEnum.UpdateBeat, OnBeatChanged);
            MessagerInstance.GetMessager().Register<string>(this, MessagerKeyEnum.DeleteBeat, OnBeatDeleted);
            MessagerInstance.GetMessager().Register<string>(this, "FreshItemsControl", OnFreshItemsControl);
        }

        private async Task OnBeatDeleted(string arg)
        {
            FreshPage(PART_ScrollBar.PageNo, false, 0);
            await TaskEx.FromResult(0);
        }

        private async Task OnBeatChanged(string arg)
        {
            if(PART_ScrollBar.PageNo == PART_ScrollBar.TotalPage && PART_ItemsControl.CurrentMoveIndex == PART_ItemsControl.Items.Count - 1)
            {
                FreshPage(PART_ScrollBar.PageNo, false, 0);
            }
            else
            {
                FreshPage(PART_ScrollBar.PageNo, _isNeedToMove, PART_ItemsControl.CurrentMoveIndex + 1);
            }
            await TaskEx.FromResult(0);
        }

        #region 键盘事件
        private void BeatItemListViewContainer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.N || e.Key == Key.S || e.Key == Key.V)
            {
                _isNeedToMove = true;
            }
            _beatDetailAction.ChangeBeatInfo(e.Key.ToString());
            _isNeedToMove = false;
            e.Handled = true;
        }

        private void BeatItemListViewContainer_KeyDown(object sender, KeyEventArgs e)
        {            
            if (e.Key == Key.Up)
            {
                ItemsControlMoveToIndex(PART_ItemsControl.CurrentMoveIndex - PART_ItemsControl.ColumnCount);
            }
            else if(e.Key == Key.Left)
            {
                ItemsControlMoveToIndex(PART_ItemsControl.CurrentMoveIndex - 1);
            }
            else if (e.Key == Key.Down)
            {
                ItemsControlMoveToIndex(PART_ItemsControl.CurrentMoveIndex + PART_ItemsControl.ColumnCount);
            }
            else if(e.Key == Key.Right)
            {
                ItemsControlMoveToIndex(PART_ItemsControl.CurrentMoveIndex + 1);
            }
            e.Handled = true;
        }
        #endregion

        private void PART_ItemsControl_ItemsControlSelectionChanged(object sender, ItemsControlSelectionChangedEventArgs e)
        {
            switch (e.SelectActionMode)
            {
                case SelectActionEnum.None:
                    SelectedCount = _beatDetailAction.ResetSelectItemsWithOnlyDisplaySelectedItems(e.SelectedItems);
                    break;
                case SelectActionEnum.Ctrl:
                    SelectedCount = _beatDetailAction.ResetSelectedItemsWithOtherSelectItems(e.SelectedItems, e.UnSelectedItems);
                    break;
                case SelectActionEnum.Shift:
                    SelectedCount = _beatDetailAction.ResetSelectItemsWithOnlyDisplaySelectedItems(e.SelectedItems);
                    break;
                default:
                    break;
            }
        }

        #region ControlBar，ScrollBar事件
        private void PART_ItemsControlBar_LeadSelectionChanged(object sender, LeadSelectionChangedEventArgs e)
        {
            if(_isLoadControl)
            {
                return;
            }
            MessagerInstance.GetMessager().Send("LeadChanged", PART_ItemsControlBar.LeadSelectedItems);
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

        private void PART_ItemsControlBar_PrevCurrentNextChanged(object sender, PrevCurrentNextEventArgs e)
        {
            _beatDetailAction.PrevCurrentNextChanged((int)e.PrevCurrentNext);
            if (_isLoadControl)
            {
                return;
            }
            SelectAllItems();
        }

        private void PART_ItemsControlBar_StrechChanged(object sender, BoolEventArgs e)
        {
            if (_isLoadControl)
            {
                return;
            }
            ChangePageSize();
        }
        private void PART_ItemsControlBar_SortChanged(object sender, SortEventArgs e)
        {
            if (_isLoadControl)
            {
                return;
            }
            _beatDetailAction.SortItemsSource(e.SortArgs);
            SelectAllItems();
        }

        private void PART_ScrollBar_PageNoChanged(object sender, PageNoChangedEventArgs e)
        {
            if (_isLoadControl)
            {
                return;
            }
            InitItemsControl(PART_ScrollBar.PageNo, PART_ScrollBar.PageSize);
        }

        #endregion
        private void ChangePageSize()
        {
            var pageSize = GetPageSize(PART_ItemsControlBar.IsStrech, PART_ItemsControlBar.LeadSelectedItems.Count);
            if (pageSize == PART_ScrollBar.PageSize)
            {
                return;
            }
            _isLoadControl = true;
            PART_ScrollBar.PageSize = pageSize;
            _isLoadControl = false;
            if (PART_ItemsControl.Items.Count <= 0)
            {
                FreshPage(1, false, 0);
            }
            else
            {
                ISelectItem itemView;
                if (PART_ItemsControl.SelectedItemsCollection.SelectedItems.Count > 0)
                {
                    itemView = PART_ItemsControl.SelectedItemsCollection.SelectedItems[0];
                }
                else
                {
                    itemView = PART_ItemsControl.Items[0] as ISelectItem;
                }
                var beatInfo = itemView.DataContext as BeatInfo;
                var pageNo = _beatDetailAction.GetCurrentItemPageNo(beatInfo.R, PART_ScrollBar.PageSize);
                FreshPage(pageNo, false, 0);
            }
        }

        private void ItemsControlMoveToIndex(int index)
        {
            if(PART_ItemsControl.CanMoveToIndex(index))
            {
                PART_ItemsControl.MoveToIndex(index);
            }
            else
            {
                int newPageNo;
                if(index < 0)
                {
                    if (PART_ScrollBar.PageNo <= 1)
                    {
                        return;
                    }
                    index = PART_ItemsControl.Items.Count + index;
                    //向前翻一页
                    newPageNo = PART_ScrollBar.PageNo - 1;
                }
                else
                {
                    if (PART_ScrollBar.PageNo >= PART_ScrollBar.TotalPage)
                    {
                        return;
                    }
                    index = index - PART_ItemsControl.Items.Count;
                    //向后翻一页
                    newPageNo = PART_ScrollBar.PageNo + 1;
                }
                FreshPage(newPageNo, true, index);
            }
        }

        private void SelectAllItems()
        {
            _beatDetailAction.SelectAll();
            InitItemsControl(PART_ScrollBar.PageNo, PART_ScrollBar.PageSize);
            PART_ItemsControl.CurrentMoveIndex = 0;
        }

        private void SelectReverseItems()
        {
            _beatDetailAction.SelectReverse();
            InitItemsControl(PART_ScrollBar.PageNo, PART_ScrollBar.PageSize);
            PART_ItemsControl.CurrentMoveIndex = 0;
        }

        /// <summary>
        /// 刷新页面
        /// </summary>
        /// <param name="pageNo">要刷新的页码</param>
        /// <param name="isNeedToMove">是否需要移动</param>
        /// <param name="moveIndex">要移动的序号，当需要移动时才参数才有效</param>
        private void FreshPage(int pageNo, bool isNeedToMove, int moveIndex)
        {
            if (!isNeedToMove || PART_ItemsControl.CanMoveToIndex(moveIndex))
            {
                //不需要移动或者可以移动到序号，直接刷新
                InitItemsControl(pageNo, PART_ScrollBar.PageSize);
            }
            if (PART_ScrollBar.TotalCount > 0 && isNeedToMove)
            {
                ItemsControlMoveToIndex(moveIndex);
            }
        }

        private void InitItemsControl(int pageNo, int pageSize)
        {
            if(pageNo != PART_ScrollBar.PageNo)
            {
                PART_ScrollBar.PageNo = pageNo;
            }
            else
            {
                var pageCount = _beatDetailAction.GetTotalPage(pageSize);
                if(pageNo > pageCount)
                {
                    PART_ScrollBar.PageNo = pageCount;
                }
                else
                {
                    var pagerSource = _beatDetailAction.GetPagerSource(pageNo, pageSize);
                    SelectedCount = _beatDetailAction.GetSelectedItemsCount();
                    PART_ScrollBar.TotalCount = pagerSource.TotalCount;
                    PART_ScrollBar.PageNo = pagerSource.PageNo;
                    PART_ItemsControl.ClearItemsSource();
                    foreach (var item in pagerSource.Source)
                    {
                        var beatInfo = item as BeatInfo;
                        BeatItemView itemView = new BeatItemView(PART_ItemsControl)
                        {
                            DataContext = item,
                            IsSelected = _beatDetailAction.SelectedItemsContainsR(beatInfo.R),
                        };
                        PART_ItemsControl.Items.Add(itemView);
                    }
                }
            }
        }

        private void InitControl()
        {
            _isLoadControl = true;
            PART_ItemsControlBar.PART_IntervalSort.IsChecked = true;
            PART_ItemsControlBar.LeadSource = LeadSource;
            PART_ItemsControlBar.LeadSelectedItems.Add(LeadSource[0]);
            PART_ItemsControlBar.LeadSelectedItems.Add(LeadSource[3]);
            PART_ItemsControlBar.PART_StrechBtn.IsChecked = false;
            PART_ItemsControlBar.PART_Current.IsChecked = true;
            PART_ScrollBar.PageNo = 1;
            PART_ScrollBar.PageSize = GetPageSize(PART_ItemsControlBar.IsStrech, PART_ItemsControlBar.LeadSelectedItems.Count);
            _isLoadControl = false;
        }

        private void BeatItemListViewContainer_Unloaded(object sender, RoutedEventArgs e)
        {
            ((IDisposable)_beatDetailAction).Dispose();
            KeyDown -= BeatItemListViewContainer_KeyDown;
            KeyUp -= BeatItemListViewContainer_KeyUp;
            MouseWheel -= BeatItemListViewContainer_MouseWheel;
            Unloaded -= BeatItemListViewContainer_Unloaded;
            _beatDetailViewContextMenuHelpler.Dispose();
            MessagerInstance.GetMessager().Unregister<string>(this, MessagerKeyEnum.UpdateBeat, OnBeatChanged);
            MessagerInstance.GetMessager().Unregister<string>(this, MessagerKeyEnum.DeleteBeat, OnBeatDeleted);
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
            var firstItem = PART_ItemsControl.Items[0];
            var itemView = firstItem as ISelectItem;
            MessagerInstance.GetMessager().Send("WaveDotChanged", ((BeatInfo)itemView.DataContext).R);
        }

        private int GetPageSize(bool isStrech, int leadCount)
        {
            var itemWidth = isStrech ? ItemBigWidth : ItemSmallWidth;
            var itemHeight = leadCount >= 3 ? ItemBigHeight : ItemSmallHeight;
            PART_ItemsControl.ItemWidth = itemWidth;
            PART_ItemsControl.ItemHeight = itemHeight;
            return PART_ItemsControl.ColumnCount * PART_ItemsControl.RowCount;
        }

        private void OnUpdateBeatHandler(object sender, RoutedEventArgs e)
        {
            _beatDetailAction.ChangeBeatInfo(Key.N.ToString());
            _isNeedToMove = true;
            MessagerInstance.GetMessager().Send(MessagerKeyEnum.UpdateBeat, string.Empty);
            _isNeedToMove = false;
        }

        private void OnInsertBeatToPrevHandler(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            Console.WriteLine("往前插入心搏" + menuItem.Header.ToString());
        }

        private void OnInsertBeatToNextHandler(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            Console.WriteLine("往后插入心搏" + menuItem.Header.ToString());
        }

        private void OnUnConfuseHandler(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if(menuItem.Header.ToString() == "叠加反混淆")
            {
                Console.WriteLine("叠加反混淆");
            }
            else
            {
                Console.WriteLine("P波反混淆");
            }
        }

        private void OnSetLongestRRHandler(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("设置最长RR间期");
        }

        private void OnSignAFHandler(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if(menuItem.InputGestureText.ToString() == "(F)")
            {
                Console.WriteLine("标记为房颤");
            }
            else
            {
                Console.WriteLine("标记为房扑");
            }
        }

        private void OnDeleteBeatHandler(object sender, RoutedEventArgs e)
        {
            _beatDetailAction.ChangeBeatInfo(Key.D.ToString());
            MessagerInstance.GetMessager().Send(MessagerKeyEnum.DeleteBeat, string.Empty);
        }

        private void InitContextMenuItems()
        {
            PART_ItemsControl.SingleSelectContextMenuItems = _beatDetailViewContextMenuHelpler.GetSingleSelectMenuItems();
            PART_ItemsControl.BatchSelectContextMenuItems = _beatDetailViewContextMenuHelpler.GetBatchSelectMenuItems();
        }

        private async Task OnFreshItemsControl(string arg)
        {
            _isLoadControl = true;
            PART_ItemsControlBar.PART_StrechBtn.IsChecked = false;
            PART_ItemsControlBar.PART_Current.IsChecked = true;
            PART_ScrollBar.PageNo = 1;
            PART_ScrollBar.PageSize = GetPageSize(PART_ItemsControlBar.IsStrech, PART_ItemsControlBar.LeadSelectedItems.Count);
            _isLoadControl = false;

            _beatDetailAction.SortItemsSource(PART_ItemsControlBar.SortArgs);
            _beatDetailAction.PrevCurrentNextChanged((int)PrevCurrentNextEnum.Current);
            SelectAllItems();
            await TaskEx.FromResult(0);
        }
    }
}
