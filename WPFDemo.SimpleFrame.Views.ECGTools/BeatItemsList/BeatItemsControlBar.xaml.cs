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
    /// BeatItemsControlBar.xaml 的交互逻辑
    /// </summary>
    public partial class BeatItemsControlBar : UserControl
    {
        public event EventHandler<LeadSelectionChangedEventArgs> LeadSelectionChanged;
        public event EventHandler<EventArgs> SelectedAll;
        public event EventHandler<EventArgs> SelectedReverse;
        public event EventHandler<BoolEventArgs> StrechChanged;
        public event EventHandler<SortEventArgs> SortChanged;
        public event EventHandler<PrevCurrentNextEventArgs> PrevCurrentNextChanged;
        public IList LeadSelectedItems => PART_LeadSwitch.SelectedItems;

        public IEnumerable LeadSource
        {
            get { return (IEnumerable)GetValue(LeadSourceProperty); }
            set { SetValue(LeadSourceProperty, value); }
        }
        public int TotalCount
        {
            get { return (int)GetValue(TotalCountProperty); }
            set { SetValue(TotalCountProperty, value); }
        }
        public int SelectedCount
        {
            get { return (int)GetValue(SelectedCountProperty); }
            set { SetValue(SelectedCountProperty, value); }
        }

        public static readonly DependencyProperty SelectedCountProperty =
            DependencyProperty.Register(nameof(SelectedCount), typeof(int), typeof(BeatItemsControlBar));
        public static readonly DependencyProperty TotalCountProperty =
            DependencyProperty.Register(nameof(TotalCount), typeof(int), typeof(BeatItemsControlBar));
        public static readonly DependencyProperty LeadSourceProperty =
            DependencyProperty.Register(nameof(LeadSource), typeof(IEnumerable), typeof(BeatItemsControlBar));

        public BeatItemsControlBar()
        {
            InitializeComponent();
        }

        private void PART_LeadListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LeadSelectionChanged?.Invoke(this, new LeadSelectionChangedEventArgs(PART_LeadSwitch.SelectedItems));
        }

        private void PART_SelectAllBtn_Click(object sender, RoutedEventArgs e)
        {
            SelectedAll?.Invoke(this, EventArgs.Empty);
        }

        private void PART_SelectReverseBtn_Click(object sender, RoutedEventArgs e)
        {
            SelectedReverse?.Invoke(this, EventArgs.Empty);
        }

        private void PART_StrechBtn_Checked(object sender, RoutedEventArgs e)
        {
            StrechChanged?.Invoke(this, new BoolEventArgs(true));
        }

        private void PART_StrechBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            StrechChanged?.Invoke(this, new BoolEventArgs(false));
        }
        private bool _isAsc;
        private void PART_SortBtn_Click(object sender, RoutedEventArgs e)
        {
            _isAsc = !_isAsc;
            SortChanged?.Invoke(this, new SortEventArgs(new SortArgs() { SortFieldName = "Interval", IsAsc = _isAsc }));
        }

        private void PART_Prev_Checked(object sender, RoutedEventArgs e)
        {
            PrevCurrentNextChanged?.Invoke(this, new PrevCurrentNextEventArgs(PrevCurrentNextEnum.Prev));
        }

        private void PART_Current_Checked(object sender, RoutedEventArgs e)
        {
            PrevCurrentNextChanged?.Invoke(this, new PrevCurrentNextEventArgs(PrevCurrentNextEnum.Current));
        }

        private void PART_Next_Checked(object sender, RoutedEventArgs e)
        {
            PrevCurrentNextChanged?.Invoke(this, new PrevCurrentNextEventArgs(PrevCurrentNextEnum.Next));
        }
    }
}
