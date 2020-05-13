using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.Editors
{
    [TemplatePart(Name = "PART_ListBox", Type = typeof(ListBox))]
    [TemplatePart(Name = "PART_SelectAllBtn", Type = typeof(Button))]
    public class MultiComboBox : ComboBox
    {
        private ListBox PART_ListBox;
        private Button PART_SelectAllBtn;

        public IList InitSelectedItems
        {
            get { return (IList)GetValue(InitSelectedItemsProperty); }
            set { SetValue(InitSelectedItemsProperty, value); }
        }

        public static readonly DependencyProperty InitSelectedItemsProperty =
            DependencyProperty.Register(nameof(InitSelectedItems), typeof(IList), typeof(MultiComboBox), new PropertyMetadata(OnSelectedChanged));

        public ICommand SelectionChangedCommand
        {
            get { return (ICommand)GetValue(SelectionChangedCommandProperty); }
            set { SetValue(SelectionChangedCommandProperty, value); }
        }

        public static readonly DependencyProperty SelectionChangedCommandProperty =
            DependencyProperty.Register(nameof(SelectionChangedCommand), typeof(ICommand), typeof(MultiComboBox));


        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register(nameof(Watermark), typeof(string), typeof(MultiComboBox));

        public Visibility SelectAllBtnVisiblity
        {
            get { return (Visibility)GetValue(SelectAllBtnVisiblityProperty); }
            set { SetValue(SelectAllBtnVisiblityProperty, value); }
        }

        public static readonly DependencyProperty SelectAllBtnVisiblityProperty =
            DependencyProperty.Register(nameof(SelectAllBtnVisiblity), typeof(Visibility), typeof(MultiComboBox));

        public bool? IsCheckedAll
        {
            get { return (bool?)GetValue(IsCheckedAllProperty); }
            set { SetValue(IsCheckedAllProperty, value); }
        }

        public static readonly DependencyProperty IsCheckedAllProperty =
            DependencyProperty.Register(nameof(IsCheckedAll), typeof(bool?), typeof(MultiComboBox), new PropertyMetadata(false, OnIsCheckAllChanged));

        public MultiComboBox()
        {
            Unloaded += MultiComboBox_Unloaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UnLoadControls();
            PART_ListBox = GetTemplateChild("PART_ListBox") as ListBox;
            PART_SelectAllBtn = GetTemplateChild("PART_SelectAllBtn") as Button;
            if (PART_ListBox != null)
            {
                PART_ListBox.SelectionChanged += PART_ListBox_SelectionChanged;
            }
            if(PART_SelectAllBtn != null)
            {
                PART_SelectAllBtn.Click += PART_SelectAllBtn_Click;
            }
        }

        private void PART_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var control = sender as ListBox;
            StringBuilder sb = new StringBuilder();
            if(control.SelectedItems.Count == 0)
            {
                IsCheckedAll = false;
                Text = string.Empty;
            }
            else if(control.SelectedItems.Count == control.Items.Count)
            {
                IsCheckedAll = true;
                Text = "全部";
            }
            else
            {
                IsCheckedAll = null;
                foreach (var item in control.SelectedItems)
                {
                    Type type = item.GetType(); 
                    if (!string.IsNullOrEmpty(DisplayMemberPath))
                    {
                        var display = type.GetProperty(DisplayMemberPath);
                        if (display != null)
                        {
                            var result = display.GetValue(item, null) ?? "null";
                            sb.Append(result.ToString()).Append("、");
                        }
                    }
                    else
                    {
                        sb.Append(item.ToString()).Append("、");
                    }
                }
                Text = sb.ToString().TrimEnd('、');
            }
            SelectionChangedCommand?.Execute(control.SelectedItems);
        }

        private static void OnSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MultiComboBox;
            if (control != null)
            {
                foreach (var item in (IList)e.NewValue)
                {
                    control.PART_ListBox.SelectedItems.Add(item);
                }
            }
        }

        private static void OnIsCheckAllChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MultiComboBox;
            var isCheckedAll = e.NewValue as bool?;
            if(isCheckedAll.HasValue)
            {
                if(isCheckedAll.Value)
                {
                    control.PART_ListBox.SelectAll();
                }
                else
                {
                    control.PART_ListBox.UnselectAll();
                }
            }
        }

        private void PART_SelectAllBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PART_ListBox == null)
            {
                return;
            }
            if (PART_ListBox.SelectedItems.Count == PART_ListBox.Items.Count)
            {
                PART_ListBox.UnselectAll();
            }
            else
            {
                PART_ListBox.SelectAll();
            }
        }

        private void UnLoadControls()
        {
            if (PART_ListBox != null)
            {
                PART_ListBox.SelectionChanged -= PART_ListBox_SelectionChanged;
            }
            if (PART_SelectAllBtn != null)
            {
                PART_SelectAllBtn.Click += PART_SelectAllBtn_Click;
            }
        }
        private void MultiComboBox_Unloaded(object sender, RoutedEventArgs e)
        {
            UnLoadControls();
            Unloaded -= MultiComboBox_Unloaded;
        }
    }
}
