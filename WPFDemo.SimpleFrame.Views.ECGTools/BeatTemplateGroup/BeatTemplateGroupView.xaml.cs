using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Windows.Threading;
using WPFDemo.SimpleFrame.Infra.Helper;
using WPFDemo.SimpleFrame.Infra.Messager;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    /// <summary>
    /// BeatTemplateGroupView.xaml 的交互逻辑
    /// </summary>
    public partial class BeatTemplateGroupView : UserControl
    {
        private bool _isMouseDown;
        private Point _mouseDownPoint;
        private readonly DispatcherTimer _dispatcherTimer;
        public UIElementCollection GroupItems => PART_GroupItemsControl.Children;
        private readonly SelectedItemsCollection _selectedItemsCollection;
        public SelectedItemsCollection SelectedItemsCollection => _selectedItemsCollection;
        private readonly SelectActionFactory _selectActionFactory;
        private BaseSelectAction _currentSelectAction;
        private readonly MergeAction _mergeAction;
        private IEnumerable<Control> _menuItems;
        private IBeatTemplateAction _beatTemplateAction;
        private readonly List<string> _allItemsIdList = new List<string>();
        private readonly BeatTemplateGroupContextMenuHelper _beatTemplateGroupContextMenuHelper;
        public List<string> AllItemsIdList => _allItemsIdList;
        public bool IsAtrialPattern { get; set; }
        public bool IsEditMode
        {
            get { return (bool)GetValue(IsEditModeProperty); }
            set { SetValue(IsEditModeProperty, value); }
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

        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register(nameof(ItemWidth), typeof(double), typeof(BeatTemplateGroupView), new PropertyMetadata(OnItemSizeChanged));
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register(nameof(ItemHeight), typeof(double), typeof(BeatTemplateGroupView), new PropertyMetadata(OnItemSizeChanged));
        public static readonly DependencyProperty IsEditModeProperty =
            DependencyProperty.Register(nameof(IsEditMode), typeof(bool), typeof(BeatTemplateGroupView));

        public BeatTemplateGroupView()
        {
            _dispatcherTimer = new DispatcherTimer(DispatcherPriority.Send)
            {
                Interval = TimeSpan.FromMilliseconds(20)
            };
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            _dispatcherTimer.Start();
            _beatTemplateAction = new BeatTemplateAction();
            _beatTemplateGroupContextMenuHelper = new BeatTemplateGroupContextMenuHelper(OnUpdateBeatHandler, OnDeleteBeatHandler, OnMergeBeatHandler, OnUnConfuseHandler, OnSignAFHandler, OnCancelSignAfOrAFHandler);
            InitializeComponent();
            _menuItems = _beatTemplateGroupContextMenuHelper.GetMenuItems();
            _selectedItemsCollection = new SelectedItemsCollection(this);
            _mergeAction = new MergeAction(this, PART_GroupSelectMask);
            _selectActionFactory = new SelectActionFactory(this, PART_GroupSelectMask);
            _mergeAction.CategoryAdded += MergeAction_CategoryAdded;
            _mergeAction.TemplateMerged += MergeAction_TemplateMerged;
            Loaded += BeatTemplateGroupView_Loaded;
            Unloaded += BeatTemplateGroupView_Unloaded;
            MouseLeftButtonUp += BeatTemplateGroupView_MouseLeftButtonUp;
            MouseRightButtonUp += BeatTemplateGroupView_MouseRightButtonUp;
            KeyUp += BeatTemplateGroupView_KeyUp;
            MessagerInstance.GetMessager().Register<IList>(this, "LeadChanged", OnLeadChanged);
        }

        private void BeatTemplateGroupView_KeyUp(object sender, KeyEventArgs e)
        {
            if(SelectedItemsCollection.SelectedItems.Count <= 0)
            {
                return;
            }
            var typeList = EnumHelper.GetSelectList(typeof(BeatTypeEnum)).Select(x => x.Name).ToList();
            if (typeList.Contains(e.Key.ToString()))
            {
                _beatTemplateAction.ChangeBeatInfo(e.Key.ToString(), SelectedItemsCollection.SelectedItems);
                InitGroupView();
            }          
            if(e.Key == Key.D)
            {
                SelectedItemsCollection.TryClearItems();
                InitGroupView();
            }
        }

        private void OnCancelSignAfOrAFHandler(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("取消标记房颤/房扑");
            e.Handled = true;
        }

        private void OnSignAFHandler(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if(menuItem.InputGestureText == "(F)")
            {
                Console.WriteLine("标记为房颤");
            }
            else
            {
                Console.WriteLine("标记为房扑");
            }
            e.Handled = true;
        }

        private void OnUnConfuseHandler(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem.Header.ToString() == "叠加反混淆")
            {
                Console.WriteLine("叠加反混淆");
            }
            else
            {
                Console.WriteLine("P波反混淆");
            }
            e.Handled = true;
        }

        private void OnMergeBeatHandler(object sender, RoutedEventArgs e)
        {
            if (SelectedItemsCollection.SelectedItems.Count < 2)
            {
                return;
            }
            var targetId = SelectedItemsCollection.SelectedItems.First();
            _beatTemplateAction.MergeBeatTemplate(SelectedItemsCollection.SelectedItems.Except(new string[] { targetId }).ToList(), targetId);
            InitGroupView();
            e.Handled = true;
        }

        private void OnDeleteBeatHandler(object sender, RoutedEventArgs e)
        {
            _beatTemplateAction.ChangeBeatInfo(Key.D.ToString(), SelectedItemsCollection.SelectedItems);
            SelectedItemsCollection.TryClearItems();
            InitGroupView();
            e.Handled = true;
        }

        private void OnUpdateBeatHandler(object sender, RoutedEventArgs e)
        {
            _beatTemplateAction.ChangeBeatInfo(Key.N.ToString(), SelectedItemsCollection.SelectedItems);
            InitGroupView();
            e.Handled = true;
        }

        private static void OnItemSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var groupView = d as BeatTemplateGroupView;
            foreach (var groupItem in groupView.GroupItems)
            {
                var groupItemView = groupItem as BeatTemplateGroupItemView;
                foreach (var item in groupItemView.Items)
                {
                    var itemView = item as BeatTemplateItemView;
                    itemView.InvalidateVisual();
                }
            }
        }

        private async Task OnLeadChanged(IList leadSource)
        {
            if (leadSource.Count >= 3)
            {
                ItemHeight = 213;
                ItemWidth = 142;
            }
            else
            {
                ItemHeight = 132;
                ItemWidth = 142;
            }
            await TaskEx.FromResult(0);
        }

        private void BeatTemplateGroupView_Loaded(object sender, RoutedEventArgs e)
        {
            IsAtrialPattern = false;//是事件
            ItemWidth = 142;
            ItemHeight = 132;
            SelectedItemsCollection.SelectedItems.Add(_beatTemplateAction.GetReferIdByBeatTemplateIndex(1));
            InitGroupView();
        }

        private void BeatTemplateGroupView_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var currentPoint = e.GetPosition(this);
            _currentSelectAction = _selectActionFactory.GetSelectActionInstance(SelectActionEnum.None);
            var itemView = _currentSelectAction.GetItemsByMouseUpPosition(currentPoint).SingleOrDefault();
            if (itemView == null)
            {
                ContextMenu = null;
                return;
            }
            if (_menuItems != null && _menuItems.Count() > 0)
            {
                ContextMenu = new ContextMenu() { ItemsSource = _menuItems };
            }
            if (SelectedItemsCollection.SelectedItems.Count <= 0 || !SelectedItemsCollection.SelectedItems.Contains(itemView.Id))
            {
                //没选中，则右击选中
                _currentSelectAction.MouseDown(currentPoint);
                _currentSelectAction.Click();
                OnClickItem(itemView);
                _isMouseDown = false;
            }
        }

        private void BeatTemplateGroupView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = true;
            IsEditMode = false;
            var currentPoint = Mouse.GetPosition(this);
            _mouseDownPoint = currentPoint;

            _mergeAction.MouseDown(_mouseDownPoint);

            SetCurrentSelectActionMode();
            _currentSelectAction.MouseDown(currentPoint);
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            var currentPoint = Mouse.GetPosition(this);
            if (!_isMouseDown)
            {
                return;
            }
            IsEditMode = _mergeAction.Draging(currentPoint);
            if (IsEditMode)
            {
                return;
            }
            _currentSelectAction.Draging(Mouse.GetPosition(this));
        }

        private void BeatTemplateGroupView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsEditMode)
            {
                _mergeAction.DragOver();
            }
            else
            {
                if (_currentSelectAction.MouseDownPoint == e.GetPosition(this))
                {
                    _currentSelectAction.Click();
                    var itemView = _currentSelectAction.ActionSelectItems.SingleOrDefault();
                    if (itemView != null)
                    {
                        OnClickItem(itemView);
                    }
                }
                else
                {
                    _currentSelectAction.DragOver();
                    OnDragOver();
                }
            }
            _isMouseDown = false;
            IsEditMode = false;
        }

        private void OnDragOver()
        {
            foreach (var item in SelectedItemsCollection.SelectedItems)
            {
                GetItemViewById(item).IsChecked = true;
            }
            _beatTemplateAction.UpdateIsCheckedStatus(SelectedItemsCollection.SelectedItems);
            MessagerInstance.GetMessager().Send("SetBeatDetailItemsSource", SelectedItemsCollection.SelectedItems.ToList());
        }

        private void OnClickItem(BeatTemplateItemView itemView)
        {
            itemView.IsChecked = true;
            _beatTemplateAction.UpdateIsCheckedStatus(SelectedItemsCollection.SelectedItems);
            MessagerInstance.GetMessager().Send("SetBeatDetailItemsSource", SelectedItemsCollection.SelectedItems.ToList());
        }

        public void OnGroupItemsSelectAll()
        {
            foreach (var item in SelectedItemsCollection.SelectedItems)
            {
                GetItemViewById(item).IsChecked = true;
            }
            _beatTemplateAction.UpdateIsCheckedStatus(SelectedItemsCollection.SelectedItems);
            MessagerInstance.GetMessager().Send("SetBeatDetailItemsSource", SelectedItemsCollection.SelectedItems.ToList());
        }

        private void MergeAction_TemplateMerged(object sender, MergeTemplateEventArgs e)
        {
            _beatTemplateAction.MergeBeatTemplate(new List<string>() { e.OriginBeatTemplateItemViewId }, e.TargetBeatTemplateItemViewId);
            InitGroupView();
        }

        private void MergeAction_CategoryAdded(object sender, AddCategoryEventArgs e)
        {
            _beatTemplateAction.AddBeatTemplate(e.OriginItemViewId, e.TargetBeatTemplateGroupItemViewId);
            InitGroupView();
        }

        public void InitGroupView()
        {
            AllItemsIdList.Clear();
            var groupItemsAtrialSource = _beatTemplateAction.GetAtrialBeatTemplate().GroupBy(x => x.BeatType).ToList();
            var groupItemsEventSource = _beatTemplateAction.GetEventBeatTemplate();
            var parentTemplate = _beatTemplateAction.GetParentBeatTemplate();
            PART_GroupItemsControl.Children.Clear();
            foreach (var groupItem in parentTemplate)
            {
                BeatTemplateGroupItemView groupItemView;
                var formSource = groupItemsAtrialSource.Single(x => x.Key == groupItem.BeatType).ToList();
                if (groupItem.BeatType == (int)BeatTypeEnum.S)
                {
                    groupItemView = new SBeatTemplateGroupItemView(groupItem.Id, formSource, groupItemsEventSource, this);
                }
                else
                {
                    groupItemView = new BeatTemplateGroupItemView(groupItem.Id, formSource.ToList(), this);
                }
                groupItemView.SetGroupItemItemsSource(SelectedItemsCollection.SelectedItems);
                groupItemView.CategoryNameEn = groupItem.CategoryNameEn;
                groupItemView.Percent = groupItem.Percent;
                groupItemView.Count = groupItem.Count;
                groupItemView.CategoryName = groupItem.CategoryName;
                PART_GroupItemsControl.Children.Add(groupItemView);
            }
            ReSetSelectedItems();
            MessagerInstance.GetMessager().Send("SetBeatDetailItemsSource", SelectedItemsCollection.SelectedItems.ToList());
        }

        private void ReSetSelectedItems()
        {
            var notExistIds = SelectedItemsCollection.SelectedItems.Where(x => !AllItemsIdList.Contains(x)).ToList();
            foreach (var id in notExistIds)
            {
                SelectedItemsCollection.TryRemoveItem(id);
            }
        }

        internal void SetCurrentMoveBeatTemplateItemView(BeatTemplateItemView itemView)
        {
            _mergeAction._currentMoveItemView = itemView;
        }

        internal void ResetIsAtrialPattern(bool isAtrialPattern, UIElementCollection oldItemViews)
        {
            IsAtrialPattern = isAtrialPattern;
            foreach (var item in oldItemViews)
            {
                var itemView = item as BeatTemplateItemView;
                SelectedItemsCollection.TryRemoveItem(itemView.Id);
            }
        }

        private void SetCurrentSelectActionMode()
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                _currentSelectAction = _selectActionFactory.GetSelectActionInstance(SelectActionEnum.Ctrl);
            }
            else
            {
                _currentSelectAction = _selectActionFactory.GetSelectActionInstance(SelectActionEnum.None);
            }
        }

        public BeatTemplateItemView GetItemViewById(string id)
        {
            foreach (var groupItem in GroupItems)
            {
                var groupItemView = groupItem as BeatTemplateGroupItemView;
                foreach (var item in groupItemView.Items)
                {
                    var itemView = item as BeatTemplateItemView;
                    if (itemView.Id == id)
                    {
                        return itemView;
                    }
                }
            }
            return null;
        }

        private void BeatTemplateGroupView_Unloaded(object sender, RoutedEventArgs e)
        {
            _dispatcherTimer.Tick -= DispatcherTimer_Tick;
            Loaded -= BeatTemplateGroupView_Loaded;
            Unloaded -= BeatTemplateGroupView_Unloaded;
            MouseLeftButtonUp -= BeatTemplateGroupView_MouseLeftButtonUp;
            MouseRightButtonUp -= BeatTemplateGroupView_MouseRightButtonUp;
            KeyUp -= BeatTemplateGroupView_KeyUp;
            _mergeAction.CategoryAdded -= MergeAction_CategoryAdded;
            _mergeAction.TemplateMerged -= MergeAction_TemplateMerged;
            MessagerInstance.GetMessager().Unregister<IList>(this, "LeadChanged", OnLeadChanged);
        }
    }
}
