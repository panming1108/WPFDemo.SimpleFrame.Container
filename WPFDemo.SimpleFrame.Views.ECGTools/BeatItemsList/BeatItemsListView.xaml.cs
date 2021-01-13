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

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    /// <summary>
    /// BeatItemsListView.xaml 的交互逻辑
    /// </summary>
    public partial class BeatItemsListView : UserControl, ISelectItemsContainer, IDragSelect
    {
        private readonly SelectedItemsCollection _selectedItemsCollection;
        private readonly DragSelectAction _dragSelectAction;
        public SelectedItemsCollection SelectedItemsCollection => _selectedItemsCollection;
        public ItemCollection Items => PART_ItemsControl.Items;

        public int CurrentMoveIndex { get; set; }

        public event EventHandler<ItemsControlSelectionChangedEventArgs> ItemsControlSelectionChanged;

        public BeatItemsListView()
        {
            _selectedItemsCollection = new SelectedItemsCollection(this);
            _dragSelectAction = new DragSelectAction(this);
            InitializeComponent();
            MouseLeftButtonDown += BeatItemsListView_MouseLeftButtonDown;
            MouseLeftButtonUp += BeatItemsListView_MouseLeftButtonUp;
            MouseMove += BeatItemsListView_MouseMove;
            Unloaded += BeatItemsListView_Unloaded;
        }

        private void BeatItemsListView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dragSelectAction.OnMouseLeftButtonDown(e.GetPosition(this));
        }

        private void BeatItemsListView_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }
            _dragSelectAction.OnMouseMove(e.GetPosition(this));
        }

        private void BeatItemsListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            List<ISelectItem> selectItems;
            if(_dragSelectAction.MouseDownPoint == e.GetPosition(this))
            {
                selectItems = _dragSelectAction.OnClickOver();
                if(!_dragSelectAction.IsCtrlKeyDown)
                {
                    var itemView = selectItems.SingleOrDefault();
                    if(itemView != null)
                    {
                        MoveToIndex(Items.IndexOf(itemView));
                    }
                }
            }
            else
            {
                selectItems = _dragSelectAction.OnDragOver();
            }
            OnItemsControlSelectionChanged(new ItemsControlSelectionChangedEventArgs(selectItems, _dragSelectAction.IsCtrlKeyDown));
        }

        public void OnItemsControlSelectionChanged(ItemsControlSelectionChangedEventArgs e)
        {
            ItemsControlSelectionChanged?.Invoke(this, e);
        }

        public void RenderDragSelectMask(GeometryDrawing geometryDrawing)
        {
            PART_SelectMask.DrawingHandler((drawingContext) =>
            {
                drawingContext.DrawDrawing(geometryDrawing);
            });
        }

        private void BeatItemsListView_Unloaded(object sender, RoutedEventArgs e)
        {
            _dragSelectAction.Dispose();
            MouseLeftButtonDown -= BeatItemsListView_MouseLeftButtonDown;
            MouseLeftButtonUp -= BeatItemsListView_MouseLeftButtonUp;
            MouseMove -= BeatItemsListView_MouseMove;
            Unloaded -= BeatItemsListView_Unloaded;
        }

        public void SetCtrlKeyStatus(bool isKeyDown)
        {
            _dragSelectAction.IsCtrlKeyDown = isKeyDown;
        }

        private void OnCurrentMoveIndexChanged()
        {
            _selectedItemsCollection.TryClearItems();
            var itemView = Items[CurrentMoveIndex] as ISelectItem;
            itemView.IsSelected = true;
            OnItemsControlSelectionChanged(new ItemsControlSelectionChangedEventArgs(new List<ISelectItem>() { itemView }, false));
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
