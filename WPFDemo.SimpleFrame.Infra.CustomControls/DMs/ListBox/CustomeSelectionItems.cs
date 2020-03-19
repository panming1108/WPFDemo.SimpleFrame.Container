using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs.ListBox
{
    public class CustomeSelectionItems
    {
        private static bool _flag = false;
        public static bool GetCanSelectedItemsBinding(DependencyObject obj)
        {
            return (bool)obj.GetValue(CanSelectedItemsBindingProperty);
        }

        public static void SetCanSelectedItemsBinding(DependencyObject obj, bool value)
        {
            obj.SetValue(CanSelectedItemsBindingProperty, value);
        }

        public static readonly DependencyProperty CanSelectedItemsBindingProperty =
            DependencyProperty.RegisterAttached("CanSelectedItemsBinding", typeof(bool), typeof(CustomeSelectionItems), new PropertyMetadata(OnCanSelectedItemsBindingChanged));

        public static IList GetSelectedItems(DependencyObject obj)
        {
            return (IList)obj.GetValue(SelectedItemsProperty);
        }

        public static void SetSelectedItems(DependencyObject obj, IList value)
        {
            obj.SetValue(SelectedItemsProperty, value);
        }

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(CustomeSelectionItems), new PropertyMetadata(OnSelectedItemsChanged));

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listBox = d as System.Windows.Controls.ListBox;
            if(listBox != null)
            {
                if(!_flag)
                {
                    foreach (var item in e.NewValue as IList)
                    {
                        listBox.SelectedItems.Add(item);
                    }
                }
            }
        }

        private static void OnCanSelectedItemsBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listBox = d as System.Windows.Controls.ListBox;
            if(listBox != null)
            {
                listBox.SelectionChanged += ListBox_SelectionChanged;
            }
        }

        private static void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _flag = false;
            var listBox = sender as System.Windows.Controls.ListBox;
            if(listBox != null)
            {
                _flag = true;
                SetSelectedItems(listBox, listBox.SelectedItems);
            }
        }
    }
}
