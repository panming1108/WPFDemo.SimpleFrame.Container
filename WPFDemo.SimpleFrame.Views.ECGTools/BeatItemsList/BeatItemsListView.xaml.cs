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
        private readonly DispatcherTimer _dispatcherTimer;
        private readonly SelectedItemsCollection _selectedItemsCollection;
        private BaseSelectAction _currentSelectAction;
        private readonly SelectActionFactory _selectActionFactory;
        public SelectedItemsCollection SelectedItemsCollection => _selectedItemsCollection;
        public ItemCollection Items => PART_ItemsControl.Items;

        public int CurrentMoveIndex { get; set; }

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
            Unloaded += BeatItemsListView_Unloaded;
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
            _currentSelectAction.MouseDown(e.GetPosition(this));
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
            _isMouseDown = false;
            if(_currentSelectAction.ActionSelectItems != null && _currentSelectAction.ActionSelectItems.Count > 0)
            {
                OnItemsControlSelectionChanged(new ItemsControlSelectionChangedEventArgs(_currentSelectAction.ActionSelectItems, _currentSelectAction.SelectActionMode));
            }
        }

        private void OnClickItem(ISelectItem itemView)
        {
            if (_currentSelectAction.SelectActionMode == SelectActionEnum.None)
            {
                CurrentMoveIndex = Items.IndexOf(itemView);
            }
            //发送消息，定位诊断图
        }

        public void OnItemsControlSelectionChanged(ItemsControlSelectionChangedEventArgs e)
        {
            ItemsControlSelectionChanged?.Invoke(this, e);
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
                _currentSelectAction = _selectActionFactory.GetSelectActionInstance(SelectActionEnum.Ctrl); 
            }
            else if(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                _currentSelectAction = _selectActionFactory.GetSelectActionInstance(SelectActionEnum.Shift);
            }
            else
            {
                _currentSelectAction = _selectActionFactory.GetSelectActionInstance(SelectActionEnum.None);
            }
        }

        public void OnCurrentMoveIndexChanged()
        {
            _selectedItemsCollection.TryClearItems();
            var itemView = Items[CurrentMoveIndex] as ISelectItem;
            itemView.IsSelected = true;
            OnItemsControlSelectionChanged(new ItemsControlSelectionChangedEventArgs(new List<ISelectItem>() { itemView }, SelectActionEnum.None));
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
