using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs
{
    [TemplatePart(Name = "PART_DisplayListBox", Type = typeof(System.Windows.Controls.ListBox))]
    [TemplatePart(Name = "PART_UnDisplayListBox", Type = typeof(System.Windows.Controls.ListBox))]
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    public class ExpandRadioGroup : Control
    {
        private List<RowData> _tempDic;
        private System.Windows.Controls.ListBox PART_DisplayListBox;
        private System.Windows.Controls.ListBox PART_UnDisplayListBox;
        private Popup PART_Popup;
        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public List<RowData> DisplayItemsSource
        {
            get { return (List<RowData>)GetValue(DisplayItemsSourceProperty); }
            set { SetValue(DisplayItemsSourceProperty, value); }
        }
        public int DisplayCount
        {
            get { return (int)GetValue(DisplayCountProperty); }
            set { SetValue(DisplayCountProperty, value); }
        }

        public List<RowData> UnDisplayItemsSource
        {
            get { return (List<RowData>)GetValue(UnDisplayItemsSourceProperty); }
            set { SetValue(UnDisplayItemsSourceProperty, value); }
        }

        public ICommand SelectedItemChangedCommand
        {
            get { return (ICommand)GetValue(SelectedItemChangedCommandProperty); }
            set { SetValue(SelectedItemChangedCommandProperty, value); }
        }

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }


        public double DisplayListBoxWidth
        {
            get { return (double)GetValue(DisplayListBoxWidthProperty); }
            set { SetValue(DisplayListBoxWidthProperty, value); }
        }

        public static readonly DependencyProperty DisplayListBoxWidthProperty =
            DependencyProperty.Register(nameof(DisplayListBoxWidth), typeof(double), typeof(ExpandRadioGroup));

        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(nameof(DisplayMemberPath), typeof(string), typeof(ExpandRadioGroup));

        public static readonly DependencyProperty SelectedItemChangedCommandProperty =
            DependencyProperty.Register(nameof(SelectedItemChangedCommand), typeof(ICommand), typeof(ExpandRadioGroup));

        public static readonly DependencyProperty UnDisplayItemsSourceProperty =
            DependencyProperty.Register(nameof(UnDisplayItemsSource), typeof(List<RowData>), typeof(ExpandRadioGroup));

        public static readonly DependencyProperty DisplayCountProperty =
            DependencyProperty.Register(nameof(DisplayCount), typeof(int), typeof(ExpandRadioGroup));

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IList), typeof(ExpandRadioGroup), new PropertyMetadata(OnItemsSourceChanged));

        public static readonly DependencyProperty DisplayItemsSourceProperty =
            DependencyProperty.Register(nameof(DisplayItemsSource), typeof(List<RowData>), typeof(ExpandRadioGroup));

        public ExpandRadioGroup()
        {
            DisplayItemsSource = new List<RowData>();
            UnDisplayItemsSource = new List<RowData>();
            Unloaded += ExpandRadioGroup_Unloaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (PART_DisplayListBox != null)
            {
                PART_DisplayListBox.SelectionChanged -= PART_DisplayListBox_SelectionChanged;
            }
            if (PART_UnDisplayListBox != null)
            {
                PART_UnDisplayListBox.SelectionChanged -= PART_UnDisplayListBox_SelectionChanged;
            }
            PART_DisplayListBox = GetTemplateChild("PART_DisplayListBox") as System.Windows.Controls.ListBox;
            if(PART_DisplayListBox != null)
            {
                PART_DisplayListBox.SelectionChanged += PART_DisplayListBox_SelectionChanged;
            }
            PART_UnDisplayListBox = GetTemplateChild("PART_UnDisplayListBox") as System.Windows.Controls.ListBox;
            if (PART_UnDisplayListBox != null)
            {
                PART_UnDisplayListBox.SelectionChanged += PART_UnDisplayListBox_SelectionChanged;
            }
            PART_Popup = GetTemplateChild("PART_Popup") as Popup;
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ExpandRadioGroup;
            control.HandleItemsSource((IList)e.NewValue);
        }

        private void HandleItemsSource(IList itemsSource)
        {
            if(DisplayCount == 0)
            {
                return;
            }
            var row = itemsSource.Count % DisplayCount == 0 ? itemsSource.Count / DisplayCount : (itemsSource.Count / DisplayCount) + 1;
            if (_tempDic == null)
            {
                _tempDic = new List<RowData>();
            }
            else
            {
                _tempDic.Clear();
            }
            for (int i = 0; i < row; i++)
            {
                _tempDic.AddRange(GetRangeItems(i, itemsSource));
            }
            SetItemsSource(0);
        }

        private void SetItemsSource(int row)
        {
            DisplayItemsSource = _tempDic.Where(x => x.Row == row).ToList();
            UnDisplayItemsSource = _tempDic.Where(x => x.Row != row).ToList();
        }

        private List<RowData> GetRangeItems(int row, IList itemsSource)
        {
            List<RowData> result  = new List<RowData>();
            for (int i = row * DisplayCount; i < (row + 1) * DisplayCount; i++)
            {
                if(i >= itemsSource.Count)
                {
                    break;
                }
                var rowData = new RowData(row, itemsSource[i]);
                if(i == 0)
                {
                    rowData.IsSelected = true;
                }
                result.Add(rowData);
            }
            return result;
        }

        /// <summary>
        /// 折叠ListBox事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PART_UnDisplayListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var unDisplayListBox = sender as System.Windows.Controls.ListBox;
            if(PART_Popup == null)
            {
                return;
            }
            if(unDisplayListBox.SelectedItem == null)
            {
                return;
            }
            var rowData = unDisplayListBox.SelectedItem as RowData;
            foreach (var item in _tempDic)
            {
                item.IsSelected = false;
            }
            rowData.IsSelected = true;
            SetItemsSource(rowData.Row);
            PART_Popup.IsOpen = false;
        }

        /// <summary>
        /// 显示ListBox事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PART_DisplayListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var displayListBox = sender as System.Windows.Controls.ListBox;
            if (displayListBox.SelectedItem == null)
            {
                return;
            }
            var rowData = displayListBox.SelectedItem as RowData;
            SelectedItemChangedCommand?.Execute(rowData.Data);
        }

        private void ExpandRadioGroup_Unloaded(object sender, RoutedEventArgs e)
        {
            if (PART_DisplayListBox != null)
            {
                PART_DisplayListBox.SelectionChanged -= PART_DisplayListBox_SelectionChanged;
            }
            if (PART_UnDisplayListBox != null)
            {
                PART_UnDisplayListBox.SelectionChanged -= PART_UnDisplayListBox_SelectionChanged;
            }
            Unloaded -= ExpandRadioGroup_Unloaded;
        }
    }
}
