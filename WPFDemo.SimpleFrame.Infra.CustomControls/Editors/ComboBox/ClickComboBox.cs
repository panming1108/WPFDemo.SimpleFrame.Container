﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.Editors
{
    [TemplatePart(Name = "PART_SelectedItem", Type = typeof(Button))]
    public class ClickComboBox : ComboBox
    {
        private Button PART_SelectedItem;

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(ClickComboBox));

        public string CategoryName
        {
            get { return (string)GetValue(CategoryNameProperty); }
            set { SetValue(CategoryNameProperty, value); }
        }

        public static readonly DependencyProperty CategoryNameProperty =
            DependencyProperty.Register(nameof(CategoryName), typeof(string), typeof(ClickComboBox));



        public string DisplayText
        {
            get { return (string)GetValue(DisplayTextProperty); }
            set { SetValue(DisplayTextProperty, value); }
        }

        public static readonly DependencyProperty DisplayTextProperty =
            DependencyProperty.Register(nameof(DisplayText), typeof(string), typeof(ClickComboBox));



        public ClickComboBox()
        {
            PreviewMouseLeftButtonDown += ClickComboBox_MouseLeftButtonDown;
            Unloaded += ClickComboBox_Unloaded;
        }

        private void ClickComboBox_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IsChecked = true;
        }

        private void ClickComboBox_Unloaded(object sender, RoutedEventArgs e)
        {
            if (PART_SelectedItem != null)
            {
                PART_SelectedItem.Click -= PART_SelectedItem_Click;
            }
            PreviewMouseLeftButtonDown -= ClickComboBox_MouseLeftButtonDown;
            Unloaded -= ClickComboBox_Unloaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (PART_SelectedItem != null)
            {
                PART_SelectedItem.Click -= PART_SelectedItem_Click;
            }
            PART_SelectedItem = GetTemplateChild("PART_SelectedItem") as Button;
            if(PART_SelectedItem != null)
            {
                PART_SelectedItem.Click += PART_SelectedItem_Click;
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {           
            base.OnSelectionChanged(e);
            var item = SelectedItem;
            if(item == null)
            {
                return;
            }
            Type type = item.GetType();
            if (!string.IsNullOrEmpty(DisplayMemberPath))
            {
                var display = type.GetProperty(DisplayMemberPath);
                if (display != null)
                {
                    var result = display.GetValue(item, null) ?? "null";
                    DisplayText = result.ToString() == "全部" ? CategoryName : result.ToString();
                }
            }
        }

        private void PART_SelectedItem_Click(object sender, RoutedEventArgs e)
        {
            SelectedIndex = 0;
        }
    }
}
