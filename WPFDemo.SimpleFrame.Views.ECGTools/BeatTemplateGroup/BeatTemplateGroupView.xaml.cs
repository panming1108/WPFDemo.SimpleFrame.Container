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
        public ItemCollection GroupItems => PART_GroupItemsControl.Items;
        private readonly SelectedItemsCollection _selectedItemsCollection;
        public SelectedItemsCollection SelectedItemsCollection => _selectedItemsCollection;
        private readonly SelectActionFactory _selectActionFactory;
        private BaseSelectAction _currentSelectAction;
        private readonly MergeAction _mergeAction;
        private IEnumerable<MenuItem> _menuItems;
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
            InitializeComponent();
            InitContextMenuItems();
            _selectedItemsCollection = new SelectedItemsCollection(this);
            _mergeAction = new MergeAction(this, PART_GroupSelectMask);
            _selectActionFactory = new SelectActionFactory(this, PART_GroupSelectMask);
            _mergeAction.CategoryAdded += MergeAction_CategoryAdded;
            _mergeAction.TemplateMerged += MergeAction_TemplateMerged;
            Loaded += BeatTemplateGroupView_Loaded;
            Unloaded += BeatTemplateGroupView_Unloaded;
            MouseLeftButtonDown += BeatTemplateGroupView_MouseLeftButtonDown;
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
            if(typeList.Contains(e.Key.ToString()))
            {
                var beatType = (BeatTypeEnum)Enum.Parse(typeof(BeatTypeEnum), e.Key.ToString());
                BeatInfoSource.BeatSource.ChangedBeatInfo(SelectedItemsCollection.SelectedItems, beatType);
                InitGroupView();
            }
            if(e.Key == Key.D)
            {
                BeatInfoSource.BeatSource.DeleteBeatInfos(SelectedItemsCollection.SelectedItems);
                InitGroupView();
            }
        }

        private void InitContextMenuItems()
        {
            MenuItem updateMenuItem = new MenuItem()
            {
                Header = "合并",
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            updateMenuItem.Click += (s, e) =>
            {
                if(SelectedItemsCollection.SelectedItems.Count < 2)
                {
                    return;
                }
                var targetId = SelectedItemsCollection.SelectedItems.First();
                BeatInfoSource.BeatSource.MergeBeats(SelectedItemsCollection.SelectedItems.Except(new string[] { targetId }).ToList(), targetId);
                InitGroupView();
            };

            MenuItem deleteMenuItem = new MenuItem()
            {
                Header = "删除心搏",
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            deleteMenuItem.Click += (s, e) =>
            {
                BeatInfoSource.BeatSource.DeleteBeatInfos(SelectedItemsCollection.SelectedItems);
                InitGroupView();
            };

            MenuItem jumpMenuItem = new MenuItem()
            {
                Header = "跳转",
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            jumpMenuItem.Click += (s, e) => { Console.WriteLine("跳转"); };
            _menuItems = new MenuItem[] { jumpMenuItem, updateMenuItem, deleteMenuItem };
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
            SelectedItemsCollection.SelectedItems.Add(BeatInfoSource.BeatTemplates[1].Id);
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
            BeatInfoSource.BeatTemplates.Where(x => SelectedItemsCollection.SelectedItems.Contains(x.Id)).ToList().ForEach(t => t.IsChecked = true);
        }

        private void OnClickItem(BeatTemplateItemView itemView)
        {
            itemView.IsChecked = true;
            BeatInfoSource.BeatTemplates.Where(x => SelectedItemsCollection.SelectedItems.Contains(x.Id)).ToList().ForEach(t => t.IsChecked = true);
        }

        public void OnGroupItemsSelectAll()
        {
            foreach (var item in SelectedItemsCollection.SelectedItems)
            {
                GetItemViewById(item).IsChecked = true;
            }
            BeatInfoSource.BeatTemplates.Where(x => SelectedItemsCollection.SelectedItems.Contains(x.Id)).ToList().ForEach(t => t.IsChecked = true);
        }

        private void MergeAction_TemplateMerged(object sender, MergeTemplateEventArgs e)
        {
            BeatInfoSource.BeatSource.MergeBeats(new List<string>() { e.OriginBeatTemplateItemView.Id }, e.TargetBeatTemplateItemView.Id);
            InitGroupView();
        }

        private void MergeAction_CategoryAdded(object sender, AddCategoryEventArgs e)
        {
            BeatInfoSource.BeatSource.AddCategory(e.OriginItemView.Id, e.TargetBeatTemplateGroupItemView.Id);
            InitGroupView();
        }

        public void InitGroupView()
        {
            var groupItemsAtrialSource = BeatInfoSource.BeatTemplates.Where(x => !x.IsEvent).GroupBy(x => x.BeatType).ToList();
            var groupItemsEventSource = BeatInfoSource.BeatTemplates.Where(x => x.IsEvent).ToList();
            PART_GroupItemsControl.Items.Clear();
            foreach (var groupItem in BeatInfoSource.ParentBeatTemplateDic)
            {
                BeatTemplateGroupItemView groupItemView;
                var formSource = groupItemsAtrialSource.Single(x => x.Key == groupItem.Key).ToList();
                if (groupItem.Key == (int)BeatTypeEnum.S)
                {
                    groupItemView = new SBeatTemplateGroupItemView(groupItem.Value.Id, formSource, groupItemsEventSource, this);
                }
                else
                {
                    groupItemView = new BeatTemplateGroupItemView(groupItem.Value.Id, formSource.ToList(), this);
                }
                groupItemView.SetGroupItemItemsSource(SelectedItemsCollection.SelectedItems);
                groupItemView.CategoryNameEn = groupItem.Value.CategoryNameEn;
                groupItemView.Percent = groupItem.Value.Percent;
                groupItemView.Count = groupItem.Value.Count;
                groupItemView.CategoryName = groupItem.Value.CategoryName;
                PART_GroupItemsControl.Items.Add(groupItemView);
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
            MouseLeftButtonDown -= BeatTemplateGroupView_MouseLeftButtonDown;
            MouseLeftButtonUp -= BeatTemplateGroupView_MouseLeftButtonUp;
            MouseRightButtonUp -= BeatTemplateGroupView_MouseRightButtonUp;
            KeyUp -= BeatTemplateGroupView_KeyUp;
            _mergeAction.CategoryAdded -= MergeAction_CategoryAdded;
            _mergeAction.TemplateMerged -= MergeAction_TemplateMerged;
            MessagerInstance.GetMessager().Unregister<IList>(this, "LeadChanged", OnLeadChanged);
        }
    }
}
