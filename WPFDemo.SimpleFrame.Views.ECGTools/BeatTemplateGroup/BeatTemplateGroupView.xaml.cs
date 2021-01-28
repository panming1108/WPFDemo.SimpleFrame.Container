﻿using System;
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
using WPFDemo.SimpleFrame.Infra.Messager;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    /// <summary>
    /// BeatTemplateGroupView.xaml 的交互逻辑
    /// </summary>
    public partial class BeatTemplateGroupView : UserControl
    {
        private Random _random = new Random();
        private bool _isMouseDown;
        private Point _mouseDownPoint;
        private readonly DispatcherTimer _dispatcherTimer;
        public ItemCollection GroupItems => PART_GroupItemsControl.Items;
        private readonly SelectedItemsCollection _selectedItemsCollection;
        public SelectedItemsCollection SelectedItemsCollection => _selectedItemsCollection;
        private readonly SelectActionFactory _selectActionFactory;
        private BaseSelectAction _currentSelectAction;
        private readonly MergeAction _mergeAction;
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
            _mergeAction = new MergeAction(this);
            _selectedItemsCollection = new SelectedItemsCollection(this);
            _selectActionFactory = new SelectActionFactory(this, PART_GroupSelectMask);
            _mergeAction.CategoryAdded += MergeAction_CategoryAdded;
            _mergeAction.TemplateMerged += MergeAction_TemplateMerged;
            Loaded += BeatTemplateGroupView_Loaded;
            Unloaded += BeatTemplateGroupView_Unloaded;
            MouseLeftButtonDown += BeatTemplateGroupView_MouseLeftButtonDown;
            MouseLeftButtonUp += BeatTemplateGroupView_MouseLeftButtonUp;
            MouseRightButtonUp += BeatTemplateGroupView_MouseRightButtonUp;
            MessagerInstance.GetMessager().Register<IList>(this, "LeadChanged", OnLeadChanged);
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
            if(leadSource.Count >= 3)
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
            InitGroupView();
        }

        private void BeatTemplateGroupView_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //右击菜单
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
            if(!_isMouseDown)
            {
                return;
            }
            IsEditMode = _mergeAction.Draging(currentPoint);
            if(IsEditMode)
            {
                return;
            }
            _currentSelectAction.Draging(Mouse.GetPosition(this));
        }

        private void BeatTemplateGroupView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(IsEditMode)
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
            MessagerInstance.GetMessager().Send("PopupNotifyBox", new PopupNotifyObject("通知", "框选结束：" + SelectedItemsCollection.SelectedItems.Count));
        }

        private void OnClickItem(BeatTemplateItemView itemView)
        {            
            MessagerInstance.GetMessager().Send("PopupNotifyBox", new PopupNotifyObject("通知", "点击"));
        }

        public void OnGroupItemsSelectAll()
        {
            MessagerInstance.GetMessager().Send("PopupNotifyBox", new PopupNotifyObject("通知", "全选结束：" + SelectedItemsCollection.SelectedItems.Count));
        }

        private void MergeAction_TemplateMerged(object sender, MergeTemplateEventArgs e)
        {
            MessagerInstance.GetMessager().Send("PopupNotifyBox", new PopupNotifyObject("通知", "合并"));
            InitGroupView();
        }

        private void MergeAction_CategoryAdded(object sender, AddCategoryEventArgs e)
        {
            MessagerInstance.GetMessager().Send("PopupNotifyBox", new PopupNotifyObject("通知", "新增分类"));
            InitGroupView();
        }

        public void InitGroupView()
        {
            var itemCount = _random.Next(5, 8);
            PART_GroupItemsControl.Items.Clear();
            for (int i = 0; i < 5; i++)
            {
                var categoryNameEn = ((BeatTypeEnum)(i % 5)).ToString();
                BeatTemplateGroupItemView groupItemView;
                if (categoryNameEn == BeatTypeEnum.S.ToString())
                {
                    groupItemView = new SBeatTemplateGroupItemView(this)
                    {
                        CategoryNameEn = categoryNameEn
                    };
                }
                else
                {
                    groupItemView = new BeatTemplateGroupItemView(this)
                    {
                        CategoryNameEn = categoryNameEn
                    };
                }
                var source = new List<BeatTemplate>();
                for (int j = 0; j < itemCount; j++)
                {
                    BeatTemplate beatTemplate = new BeatTemplate() { Id = Guid.NewGuid().ToString(), CategoryName = ((BeatTypeEnum)(j % 5)).ToString() };
                    source.Add(beatTemplate);
                }
                groupItemView.SetGroupItemItemsSource(source);
                PART_GroupItemsControl.Items.Add(groupItemView);
            }
        }

        internal void SetCurrentMoveBeatTemplateItemView(BeatTemplateItemView itemView)
        {
            _mergeAction._currentMoveItemView = itemView;
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

        private void BeatTemplateGroupView_Unloaded(object sender, RoutedEventArgs e)
        {
            _dispatcherTimer.Tick -= DispatcherTimer_Tick;
            Loaded -= BeatTemplateGroupView_Loaded;
            Unloaded -= BeatTemplateGroupView_Unloaded;
            MouseLeftButtonDown -= BeatTemplateGroupView_MouseLeftButtonDown;
            MouseLeftButtonUp -= BeatTemplateGroupView_MouseLeftButtonUp;
            MouseRightButtonUp -= BeatTemplateGroupView_MouseRightButtonUp;
            _mergeAction.CategoryAdded -= MergeAction_CategoryAdded;
            _mergeAction.TemplateMerged -= MergeAction_TemplateMerged;
            MessagerInstance.GetMessager().Unregister<IList>(this, "LeadChanged", OnLeadChanged);
        }
    }
}
