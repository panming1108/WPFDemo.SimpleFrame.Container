using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WPFDemo.SimpleFrame.Infra.Ioc;
using WPFDemo.SimpleFrame.IViewModels.ECGTools;
using WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    /// <summary>
    /// BeatItemListView.xaml 的交互逻辑
    /// </summary>
    public partial class BeatItemListViewContainer : UserControl, ISelectItemsContainer
    {
        private string[] LeadSource => new string[] { "I", "II", "III", "aVR", "aVL", "aVF", "V1", "V2", "V3", "V4", "V5", "V6" };
        private readonly SelectedItemsCollection _selectedItemsCollection;
        public SelectedItemsCollection SelectedItemsCollection => _selectedItemsCollection;
        private bool _isCtrlKeyDown = false;

        public BeatItemListViewContainer()
        {
            _selectedItemsCollection = new SelectedItemsCollection(this);
            InitializeComponent();
            InitItemsControl();
            InitItemsControlBar();
            KeyDown += BeatItemListViewContainer_KeyDown;
            KeyUp += BeatItemListViewContainer_KeyUp;
            Loaded += BeatItemListView_Loaded;
            Unloaded += BeatItemListViewContainer_Unloaded;
        }

        private void BeatItemListViewContainer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                _isCtrlKeyDown = false;
            }
        }

        private void BeatItemListViewContainer_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                _isCtrlKeyDown = true;
            }
        }

        private void InitItemsControl()
        {
            var source = GenerateSource(30);
            PART_ItemsControlBar.TotalCount = source.Count;
            foreach (var item in source)
            {
                ISelectItem itemView = new BeatItemView(this)
                {
                    DataContext = item,
                    IsSelected = true,
                };
                PART_ItemsControl.Items.Add(itemView);
            }
        }

        private void InitItemsControlBar()
        {
            PART_ItemsControlBar.LeadSource = LeadSource;
            PART_ItemsControlBar.LeadSelectedItems.Add(LeadSource[0]);
            PART_ItemsControlBar.LeadSelectedItems.Add(LeadSource[3]);
        }

        private void BeatItemListView_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void PART_ItemsControl_ClickSelectionChanged(object sender, ClickSelectionChangedEventArgs e)
        {
            var selectItem = e.SelectedItem as ISelectItem;
            if (_isCtrlKeyDown)
            {
                selectItem.IsSelected = !selectItem.IsSelected;
            }
            else
            {
                _selectedItemsCollection.TryClearItems();
                selectItem.IsSelected = true;
            }
        }

        private void PART_ItemsControlBar_LeadSelectionChanged(object sender, LeadSelectionChangedEventArgs e)
        {

        }

        private void PART_ItemsControlBar_SelectedAll(object sender, EventArgs e)
        {

        }

        private void PART_ItemsControlBar_SelectedReverse(object sender, EventArgs e)
        {

        }

        private void PART_ItemsControlBar_StrechChanged(object sender, BoolEventArgs e)
        {

        }

        private void PART_ItemsControlBar_SortChanged(object sender, SortEventArgs e)
        {

        }

        private void BeatItemListViewContainer_Unloaded(object sender, RoutedEventArgs e)
        {
            KeyDown -= BeatItemListViewContainer_KeyDown;
            KeyUp -= BeatItemListViewContainer_KeyUp;
            Loaded -= BeatItemListView_Loaded;
            Unloaded -= BeatItemListViewContainer_Unloaded;
        }

        private List<BeatInfo> GenerateSource(int count)
        {
            string[] beatTypes = new string[] { "N", "S", "V" };
            Random random = new Random();
            var results = new List<BeatInfo>();
            for (int i = 0; i < count; i++)
            {
                BeatInfo beatInfo = new BeatInfo()
                {
                    BeatType = beatTypes[i % 3],
                    Position = i * 10000,
                    Interval = random.Next(0, count)
                };
                results.Add(beatInfo);
            }
            return results;
        }
    }
}
