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
    [TemplatePart(Name = "PART_HeaderGrid", Type = typeof(Grid))]
    public class MultiTreeViewItem : ItemsControl
    {
        private Grid PART_HeaderGrid;

        internal MultiTreeView ParentTreeView
        {
            get
            {
                for (ItemsControl itemsControl = ParentItemsControl; itemsControl != null; itemsControl = ItemsControlFromItemContainer(itemsControl))
                {
                    MultiTreeView treeView = itemsControl as MultiTreeView;
                    if (treeView != null)
                    {
                        return treeView;
                    }
                }
                return null;
            }
        }

        internal MultiTreeViewItem ParentTreeViewItem => ParentItemsControl as MultiTreeViewItem;

        internal ItemsControl ParentItemsControl => ItemsControlFromItemContainer(this);

        #region Properties
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }
        public Style ToggleBtnStyle
        {
            get { return (Style)GetValue(ToggleBtnStyleProperty); }
            set { SetValue(ToggleBtnStyleProperty, value); }
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public string ChildrenDisplayMemberPath
        {
            get { return (string)GetValue(ChildrenDisplayMemberPathProperty); }
            set { SetValue(ChildrenDisplayMemberPathProperty, value); }
        }

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public string HeaderDisplayMemberPath
        {
            get { return (string)GetValue(HeaderDisplayMemberPathProperty); }
            set { SetValue(HeaderDisplayMemberPathProperty, value); }
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
            DependencyProperty.Register(nameof(CheckBoxStyle), typeof(Style), typeof(MultiTreeViewItem));

        public static readonly DependencyProperty IsCheckedDisplayMemberPathProperty =
            DependencyProperty.Register(nameof(IsCheckedDisplayMemberPath), typeof(string), typeof(MultiTreeViewItem));

        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register(nameof(IsExpanded), typeof(bool), typeof(MultiTreeViewItem));

        public static readonly DependencyProperty ChildrenDisplayMemberPathProperty =
            DependencyProperty.Register(nameof(ChildrenDisplayMemberPath), typeof(string), typeof(MultiTreeViewItem));

        public static readonly DependencyProperty HeaderDisplayMemberPathProperty =
            DependencyProperty.Register(nameof(HeaderDisplayMemberPath), typeof(string), typeof(MultiTreeViewItem));

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(string), typeof(MultiTreeViewItem));

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(MultiTreeViewItem), new PropertyMetadata(OnIsCheckedChanged));

        public static readonly DependencyProperty ToggleBtnStyleProperty =
            DependencyProperty.Register(nameof(ToggleBtnStyle), typeof(Style), typeof(MultiTreeViewItem));
        #endregion

        public MultiTreeViewItem()
        {
            Unloaded += MultiTreeViewItem_Unloaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if(PART_HeaderGrid != null)
            {
                PART_HeaderGrid.MouseLeftButtonDown -= PART_HeaderGrid_MouseLeftButtonDown;
            }
            PART_HeaderGrid = GetTemplateChild("PART_HeaderGrid") as Grid;
            if (PART_HeaderGrid != null)
            {
                PART_HeaderGrid.MouseLeftButtonDown += PART_HeaderGrid_MouseLeftButtonDown;
            }
        }

        private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var treeItem = d as MultiTreeViewItem;
            if (treeItem == null)
            {
                return;
            }
            if (treeItem.HasItems)
            {
                return;
            }
            var tree = treeItem.ParentTreeView;
            if(tree == null)
            {
                throw new Exception("ParentTreeView is null");
            }
            if((bool)e.NewValue)
            {
                tree.SetSelectedItems(treeItem.DataContext);
            }
            else
            {
                tree.RemoveSelectedItems(treeItem.DataContext);
            }
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var treeViewItem = element as MultiTreeViewItem;
            ParentTreeView.SetItemsProperties(treeViewItem);
        }

        #region 事件
        private void PART_HeaderGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                IsExpanded = !IsExpanded;
            }
        }

        private void MultiTreeViewItem_Unloaded(object sender, RoutedEventArgs e)
        {
            if (PART_HeaderGrid != null)
            {
                PART_HeaderGrid.MouseLeftButtonDown -= PART_HeaderGrid_MouseLeftButtonDown;
            }
            Unloaded -= MultiTreeViewItem_Unloaded;
        }
        #endregion

        #region 重写Item项
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MultiTreeViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is MultiTreeViewItem;
        }
        #endregion
    }
}
