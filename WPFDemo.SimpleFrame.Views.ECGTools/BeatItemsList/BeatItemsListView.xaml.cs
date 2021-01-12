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

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    /// <summary>
    /// BeatItemsListView.xaml 的交互逻辑
    /// </summary>
    public partial class BeatItemsListView : UserControl, ISelectItemsContainer
    {
        private readonly SelectedItemsCollection _selectedItemsCollection;
        public bool IsCtrlKeyDown { get; set; }
        public SelectedItemsCollection SelectedItemsCollection => _selectedItemsCollection;
        public ItemCollection Items => PART_ItemsControl.Items;


        public event EventHandler<ItemsControlSelectionChangedEventArgs> ItemsControlSelectionChanged;
        public BeatItemsListView()
        {
            _selectedItemsCollection = new SelectedItemsCollection(this);
            InitializeComponent();
            MouseLeftButtonUp += BeatItemsListView_MouseLeftButtonUp;
            Unloaded += BeatItemsListView_Unloaded;
        }
        private void BeatItemsListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var selectItem = GetItemByMouseUpPosition(Mouse.GetPosition(this));
            if(selectItem == null)
            {
                return;
            }
            if (IsCtrlKeyDown)
            {
                selectItem.IsSelected = !selectItem.IsSelected;
            }
            else
            {
                _selectedItemsCollection.TryClearItems();
                selectItem.IsSelected = true;
            }
            ItemsControlSelectionChanged?.Invoke(this, new ItemsControlSelectionChangedEventArgs(new List<ISelectItem>() { selectItem }, IsCtrlKeyDown));
        }

        private void BeatItemsListView_Unloaded(object sender, RoutedEventArgs e)
        {
            MouseLeftButtonUp -= BeatItemsListView_MouseLeftButtonUp;
            Unloaded -= BeatItemsListView_Unloaded;
        }

        private ISelectItem GetItemByMouseUpPosition(Point point)
        {
            foreach (var item in Items)
            {
                var itemView = item as BeatItemView;
                var topLeft = itemView.TranslatePoint(new Point(0, 0), this);
                var itemBounds = new Rect(topLeft.X, topLeft.Y, itemView.ActualWidth, itemView.ActualHeight);
                if(itemBounds.Contains(point))
                {
                    return itemView;
                }
            }
            return null;
        }
    }
}
