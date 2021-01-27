using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private SelectActionFactory _selectActionFactory;
        private BaseSelectAction _currentSelectAction;
        private MergeAction _mergeAction;

        public bool IsEditMode
        {
            get { return (bool)GetValue(IsEditModeProperty); }
            set { SetValue(IsEditModeProperty, value); }
        }

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
        }

        private void BeatTemplateGroupView_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateData();
        }

        private void BeatTemplateGroupView_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            
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

        private void MergeAction_TemplateMerged(object sender, MergeTemplateEventArgs e)
        {
            MessagerInstance.GetMessager().Send("PopupNotifyBox", new PopupNotifyObject("通知", "合并"));
        }

        private void MergeAction_CategoryAdded(object sender, AddCategoryEventArgs e)
        {
            MessagerInstance.GetMessager().Send("PopupNotifyBox", new PopupNotifyObject("通知", "新增分类"));
        }

        public void GenerateData()
        {
            var groupCount = _random.Next(5, 8);
            var itemCount = _random.Next(5, 8);
            PART_GroupItemsControl.Items.Clear();
            for (int i = 0; i < groupCount; i++)
            {
                BeatTemplateGroupItemView groupItemView = new BeatTemplateGroupItemView(this);
                var source = new List<BeatTemplate>();
                for (int j = 0; j < itemCount; j++)
                {
                    BeatTemplate beatTemplate = new BeatTemplate() { CategoryName = ((BeatTypeEnum)(j % 5)).ToString() };
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
        }
    }
}
