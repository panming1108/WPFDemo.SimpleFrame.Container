using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.Editors
{
    public class CheckMultiComboBox : ContentControl
    {
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(CheckMultiComboBox));

        public IList InitSelectedItems
        {
            get { return (IList)GetValue(InitSelectedItemsProperty); }
            set { SetValue(InitSelectedItemsProperty, value); }
        }

        public static readonly DependencyProperty InitSelectedItemsProperty =
            DependencyProperty.Register(nameof(InitSelectedItems), typeof(IList), typeof(CheckMultiComboBox));

        public ICommand SelectionChangedCommand
        {
            get { return (ICommand)GetValue(SelectionChangedCommandProperty); }
            set { SetValue(SelectionChangedCommandProperty, value); }
        }

        public static readonly DependencyProperty SelectionChangedCommandProperty =
            DependencyProperty.Register(nameof(SelectionChangedCommand), typeof(ICommand), typeof(CheckMultiComboBox));

        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register(nameof(Watermark), typeof(string), typeof(CheckMultiComboBox));

        public Visibility SelectAllBtnVisiblity
        {
            get { return (Visibility)GetValue(SelectAllBtnVisiblityProperty); }
            set { SetValue(SelectAllBtnVisiblityProperty, value); }
        }

        public static readonly DependencyProperty SelectAllBtnVisiblityProperty =
            DependencyProperty.Register(nameof(SelectAllBtnVisiblity), typeof(Visibility), typeof(CheckMultiComboBox));

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(nameof(DisplayMemberPath), typeof(string), typeof(CheckMultiComboBox));
    }

    public class NewOrderCheckBox : System.Windows.Controls.CheckBox
    {
        protected override void OnToggle()
        {
            if (IsChecked == true)
            {
                IsChecked = false;
            }
            else if (IsChecked == false)
            {
                IsChecked = this.IsThreeState ? null : (bool?)true;
            }
            else
            {
                IsChecked = true;
            }
        }
    }
}
