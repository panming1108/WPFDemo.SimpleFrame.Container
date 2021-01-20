using System;
using System.Collections;
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

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    /// <summary>
    /// BeatItemsListView.xaml 的交互逻辑
    /// </summary>
    public partial class BeatItemsListView : UserControl, ISelectItemsContainer
    {
        private bool _isMouseDown;
        private Point _mouseDownPoint;
        private Point _lastMouseDownPoint;
        private readonly DispatcherTimer _dispatcherTimer;
        private readonly SelectedItemsCollection _selectedItemsCollection;
        private BaseSelectAction _currentSelectAction;
        private readonly SelectActionFactory _selectActionFactory;
        public SelectedItemsCollection SelectedItemsCollection => _selectedItemsCollection;
        public ItemCollection Items => PART_ItemsControl.Items;
        public int ColumnCount => Convert.ToInt32(ActualWidth / ItemWidth);
        public int RowCount => Convert.ToInt32(ActualHeight / ItemHeight);
        public int CurrentMoveIndex { get; set; }

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
        public IEnumerable<MenuItem> SingleSelectContextMenuItems
        {
            get { return (IEnumerable<MenuItem>)GetValue(SingleSelectContextMenuItemsProperty); }
            set { SetValue(SingleSelectContextMenuItemsProperty, value); }
        }
        public IEnumerable<MenuItem> BatchSelectContextMenuItems
        {
            get { return (IEnumerable<MenuItem>)GetValue(BatchSelectContextMenuItemsProperty); }
            set { SetValue(BatchSelectContextMenuItemsProperty, value); }
        }

        public static readonly DependencyProperty BatchSelectContextMenuItemsProperty =
            DependencyProperty.Register(nameof(BatchSelectContextMenuItems), typeof(IEnumerable<MenuItem>), typeof(BeatItemsListView));
        public static readonly DependencyProperty SingleSelectContextMenuItemsProperty =
            DependencyProperty.Register(nameof(SingleSelectContextMenuItems), typeof(IEnumerable<MenuItem>), typeof(BeatItemsListView));
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register(nameof(ItemWidth), typeof(double), typeof(BeatItemsListView));
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register(nameof(ItemHeight), typeof(double), typeof(BeatItemsListView));

        public event EventHandler<ItemsControlSelectionChangedEventArgs> ItemsControlSelectionChanged;

        public BeatItemsListView()
        {
            _dispatcherTimer = new DispatcherTimer(DispatcherPriority.Send)
            {
                Interval = TimeSpan.FromMilliseconds(20)
            };
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            _dispatcherTimer.Start();
            _selectedItemsCollection = new SelectedItemsCollection(this);
            InitializeComponent();
            _selectActionFactory = new SelectActionFactory(this, PART_SelectMask);
            MouseLeftButtonDown += BeatItemsListView_MouseLeftButtonDown;
            MouseLeftButtonUp += BeatItemsListView_MouseLeftButtonUp;
            MouseRightButtonUp += BeatItemsListView_MouseRightButtonUp;
            Unloaded += BeatItemsListView_Unloaded;
        }

        private void BeatItemsListView_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var currentPoint = e.GetPosition(this);
            _currentSelectAction = _selectActionFactory.GetSelectActionInstance(SelectActionEnum.None, currentPoint);
            var itemView = _currentSelectAction.GetItemsByMouseUpPosition(currentPoint).SingleOrDefault();
            if(itemView == null)
            {
                return;
            }
            if (SelectedItemsCollection.SelectedItems.Count > 1 && SelectedItemsCollection.SelectedItems.Contains(itemView))
            {
                if(BatchSelectContextMenuItems != null && BatchSelectContextMenuItems.Count() > 0)
                {
                    ContextMenu = new ContextMenu() { ItemsSource = BatchSelectContextMenuItems };
                }
            }
            else
            {
                _currentSelectAction.MouseDown(currentPoint);
                _currentSelectAction.Click();
                CurrentMoveIndex = Items.IndexOf(SelectedItemsCollection.SelectedItems.Last());
                _isMouseDown = false;
                OnItemsControlSelectionChanged(_currentSelectAction.SelectActionMode);
                if(SingleSelectContextMenuItems != null && SingleSelectContextMenuItems.Count() > 0)
                {
                    ContextMenu = new ContextMenu() { ItemsSource = SingleSelectContextMenuItems };
                }
            }
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            //鼠标是否移动了
            if (_isMouseDown)
            {
                _currentSelectAction.Draging(Mouse.GetPosition(this));
            }
        }

        private void BeatItemsListView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = true;
            SetSelectActionMode();
            var currentPoint = e.GetPosition(this);
            _mouseDownPoint = currentPoint;
            if (e.ClickCount == 2)
            {
                var itemView = _currentSelectAction.GetItemsByMouseUpPosition(currentPoint).SingleOrDefault();
                if (itemView != null)
                {
                    OnDoubleClick(itemView);
                }
            }
            _currentSelectAction.MouseDown(currentPoint);
        }

        private void BeatItemsListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(_currentSelectAction.MouseDownPoint == e.GetPosition(this))
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
            }
            if(_isMouseDown)
            {
                CurrentMoveIndex = Items.IndexOf(SelectedItemsCollection.SelectedItems.Last());
            }
            _isMouseDown = false;
            if(!(_currentSelectAction is ShiftSelectAction))
            {
                _lastMouseDownPoint = _mouseDownPoint;
            }
            OnItemsControlSelectionChanged(_currentSelectAction.SelectActionMode);
        }

        private void OnDoubleClick(ISelectItem itemView)
        {
            //弹窗诊断图弹窗
            Console.WriteLine("双击");
        }

        private void OnClickItem(ISelectItem itemView)
        {
            //发送消息，定位诊断图
            Console.WriteLine("单击");
        }

        public void OnItemsControlSelectionChanged(SelectActionEnum selectActionEnum)
        {
            ItemsControlSelectionChanged?.Invoke(this, new ItemsControlSelectionChangedEventArgs(SelectedItemsCollection.SelectedItems, SelectedItemsCollection.UnSelectedItems, selectActionEnum));
        }

        private void BeatItemsListView_Unloaded(object sender, RoutedEventArgs e)
        {
            _dispatcherTimer.Stop();
            MouseLeftButtonDown -= BeatItemsListView_MouseLeftButtonDown;
            MouseLeftButtonUp -= BeatItemsListView_MouseLeftButtonUp;
            Unloaded -= BeatItemsListView_Unloaded;
        }

        public void SetSelectActionMode()
        {
            if(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                _currentSelectAction = _selectActionFactory.GetSelectActionInstance(SelectActionEnum.Ctrl, new Point()); 
            }
            else if(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {                
                _currentSelectAction = _selectActionFactory.GetSelectActionInstance(SelectActionEnum.Shift, _lastMouseDownPoint);
            }
            else
            {
                _currentSelectAction = _selectActionFactory.GetSelectActionInstance(SelectActionEnum.None, new Point());
            }
        }

        public void OnCurrentMoveIndexChanged()
        {
            _selectedItemsCollection.TryClearItems();
            var itemView = Items[CurrentMoveIndex] as ISelectItem;
            itemView.IsSelected = true;
            OnItemsControlSelectionChanged(SelectActionEnum.None);
        }

        public bool CanMoveToIndex(int index)
        {
            return index >= 0 && index <= Items.Count - 1;
        }

        public void MoveToIndex(int index)
        {
            if(!CanMoveToIndex(index))
            {
                return;
            }
            CurrentMoveIndex = index;
            OnCurrentMoveIndexChanged();
        }

        public void ClearItemsSource()
        {
            Items.Clear();
            _selectedItemsCollection.TryClearItems();
        }
    }
}
