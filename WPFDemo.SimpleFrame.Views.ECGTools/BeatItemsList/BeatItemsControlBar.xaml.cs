using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    public partial class BeatItemsControlBar : UserControl, IBeatItemsControlBar
    {
        private bool _isStrech;
        private PrevCurrentNextEnum _prevCurrentNextStatus;
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

        public bool IsStrech => _isStrech;

        public PrevCurrentNextEnum PrevCurrentNextStatus => _prevCurrentNextStatus;

        public SortArgs SortArgs { get; set; }

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

        private void SortButton_SortArgsChanged(object sender, SortEventArgs e)
        {
            SortArgs = e.SortArgs;
            SortChanged?.Invoke(this, e);
        }

        private void PART_PrevCurrentNext_Checked(object sender, RoutedEventArgs e)
        {
            var control = sender as RadioButton;
            var prevCurrentNextEnum = (PrevCurrentNextEnum)Enum.Parse(typeof(PrevCurrentNextEnum), control.Tag.ToString());
            _prevCurrentNextStatus = prevCurrentNextEnum;
            PrevCurrentNextChanged?.Invoke(this, new PrevCurrentNextEventArgs(_prevCurrentNextStatus));
        }

        private void PART_StrechBtn_CheckStatusSwitch(object sender, RoutedEventArgs e)
        {
            var control = sender as ToggleButton;
            _isStrech = control.IsChecked.Value;
            StrechChanged?.Invoke(this, new BoolEventArgs(_isStrech));
        }
    }
}
