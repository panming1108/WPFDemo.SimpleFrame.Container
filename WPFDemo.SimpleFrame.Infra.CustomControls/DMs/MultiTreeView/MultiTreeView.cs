using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs
{
    public class MultiTreeView : ItemsControl
    {
        public Style ToggleBtnStyle
        {
            get { return (Style)GetValue(ToggleBtnStyleProperty); }
            set { SetValue(ToggleBtnStyleProperty, value); }
        }

        public string HeaderDisplayMemberPath
        {
            get { return (string)GetValue(HeaderDisplayMemberPathProperty); }
            set { SetValue(HeaderDisplayMemberPathProperty, value); }
        }

        public string ChildrenDisplayMemberPath
        {
            get { return (string)GetValue(ChildrenDisplayMemberPathProperty); }
            set { SetValue(ChildrenDisplayMemberPathProperty, value); }
        }

        public ICommand SelectionChangedCommand
        {
            get { return (ICommand)GetValue(SelectionChangedCommandProperty); }
            set { SetValue(SelectionChangedCommandProperty, value); }
        }

        public IList SelectedItems
        {
            get { return (IList)GetValue(SelectedItemsProperty); }
            private set { SetValue(SelectedItemsProperty, value); }
        }

        public string IsCheckedDisplayMemberPath
        {
            get { return (string)GetValue(IsCheckedDisplayMemberPathProperty); }
            set { SetValue(IsCheckedDisplayMemberPathProperty, value); }
        }

        public Style CheckBoxStyle
        {
            get { return (Style)GetValue(CheckBoxStyleProperty); }
            set { SetValue(CheckBoxStyleProperty, value); }
        }

        public static readonly DependencyProperty CheckBoxStyleProperty =
            DependencyProperty.Register(nameof(CheckBoxStyle), typeof(Style), typeof(MultiTreeView));

        public static readonly DependencyProperty IsCheckedDisplayMemberPathProperty =
            DependencyProperty.Register(nameof(IsCheckedDisplayMemberPath), typeof(string), typeof(MultiTreeView));

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register(nameof(SelectedItems), typeof(IList), typeof(MultiTreeView));

        public static readonly DependencyProperty SelectionChangedCommandProperty =
            DependencyProperty.Register(nameof(SelectionChangedCommand), typeof(ICommand), typeof(MultiTreeView));

        public static readonly DependencyProperty ChildrenDisplayMemberPathProperty =
            DependencyProperty.Register(nameof(ChildrenDisplayMemberPath), typeof(string), typeof(MultiTreeView));

        public static readonly DependencyProperty HeaderDisplayMemberPathProperty =
            DependencyProperty.Register(nameof(HeaderDisplayMemberPath), typeof(string), typeof(MultiTreeView));

        public static readonly DependencyProperty ToggleBtnStyleProperty =
            DependencyProperty.Register(nameof(ToggleBtnStyle), typeof(Style), typeof(MultiTreeView));

        public MultiTreeView()
        {
            SelectedItems = new List<object>();
        }

        #region 重写Items
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MultiTreeViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is MultiTreeViewItem;
        }
        #endregion

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var treeViewItem = element as MultiTreeViewItem;
            SetItemsProperties(treeViewItem);                       
        }

        internal void SetItemsProperties(MultiTreeViewItem treeViewItem)
        {
            treeViewItem.HeaderDisplayMemberPath = HeaderDisplayMemberPath;
            treeViewItem.ChildrenDisplayMemberPath = ChildrenDisplayMemberPath;
            treeViewItem.IsCheckedDisplayMemberPath = IsCheckedDisplayMemberPath;
            treeViewItem.CheckBoxStyle = CheckBoxStyle;
            treeViewItem.ToggleBtnStyle = ToggleBtnStyle;
            treeViewItem.Padding = new Thickness(20, 0, 0, 0);
            SetItemsBindings(treeViewItem);
        }

        private void SetItemsBindings(MultiTreeViewItem treeViewItem)
        {
            if (!string.IsNullOrEmpty(HeaderDisplayMemberPath))
            {
                Binding binding = new Binding(HeaderDisplayMemberPath);
                treeViewItem.SetBinding(MultiTreeViewItem.HeaderProperty, binding);
            }
            if (!string.IsNullOrEmpty(ChildrenDisplayMemberPath))
            {
                Binding binding = new Binding(ChildrenDisplayMemberPath);
                treeViewItem.SetBinding(MultiTreeViewItem.ItemsSourceProperty, binding);
            }
            if (!string.IsNullOrEmpty(IsCheckedDisplayMemberPath))
            {
                Binding binding = new Binding(IsCheckedDisplayMemberPath);
                treeViewItem.SetBinding(MultiTreeViewItem.IsCheckedProperty, binding);
            }
        }

        internal void SetSelectedItems(object selectedItem)
        {
            SelectedItems.Add(selectedItem);
            SelectionChangedCommand?.Execute(SelectedItems);
        }

        internal void RemoveSelectedItems(object selectedItem)
        {
            SelectedItems.Remove(selectedItem);
            SelectionChangedCommand?.Execute(SelectedItems);
        }
    }
}
