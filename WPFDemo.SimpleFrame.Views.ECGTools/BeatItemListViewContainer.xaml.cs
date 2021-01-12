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
    public partial class BeatItemListViewContainer : UserControl
    {
        private string[] LeadSource => new string[] { "I", "II", "III", "aVR", "aVL", "aVF", "V1", "V2", "V3", "V4", "V5", "V6" };

        private readonly ObservableCollection<BeatInfo> _selectedItems = new ObservableCollection<BeatInfo>();
        public ObservableCollection<BeatInfo> SelectedItems => _selectedItems;

        public BeatItemListViewContainer()
        {
            InitializeComponent();
            InitItemsControl();
            InitItemsControlBar();
            KeyDown += BeatItemListViewContainer_KeyDown;
            KeyUp += BeatItemListViewContainer_KeyUp;
            Unloaded += BeatItemListViewContainer_Unloaded;
        }

        private void BeatItemListViewContainer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                PART_ItemsControl.DragSelectAction.IsCtrlKeyDown = false;
            }
        }

        private void BeatItemListViewContainer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                PART_ItemsControl.DragSelectAction.IsCtrlKeyDown = true;
            }
        }

        private void PART_ItemsControl_ItemsControlSelectionChanged(object sender, ItemsControlSelectionChangedEventArgs e)
        {
            if(e.IsCtrlKeyDown)
            {
                foreach (var item in e.SelectedItems)
                {
                    var itemView = item as ISelectItem;
                    var beatInfo = (BeatInfo)itemView.DataContext;
                    if(SelectedItems.Contains(beatInfo))
                    {
                        SelectedItems.Remove(beatInfo);
                    }
                    else
                    {
                        SelectedItems.Add(beatInfo);
                    }
                }
            }
            else
            {
                SelectedItems.Clear();
                foreach (var item in e.SelectedItems)
                {
                    var itemView = item as ISelectItem;
                    var beatInfo = (BeatInfo)itemView.DataContext;
                    SelectedItems.Add(beatInfo);
                }
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

        private void InitItemsControl()
        {
            PART_ItemsControlBar.TotalCount = BeatInfoSource.AllBeatInfos.Count;
            //默认全选
            foreach (var item in BeatInfoSource.AllBeatInfos)
            {
                _selectedItems.Add(item);
            }
            var source = BeatInfoSource.GetPagerBeatInfo(1, 30);
            foreach (var item in source)
            {
                ISelectItem itemView = new BeatItemView(PART_ItemsControl)
                {
                    DataContext = item,
                    IsSelected = _selectedItems.Contains(item),
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

        private void BeatItemListViewContainer_Unloaded(object sender, RoutedEventArgs e)
        {
            KeyDown -= BeatItemListViewContainer_KeyDown;
            KeyUp -= BeatItemListViewContainer_KeyUp;
            Unloaded -= BeatItemListViewContainer_Unloaded;
        }
    }
}
