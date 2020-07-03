using NLog.LayoutRenderers.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs
{
    public class GroupGridViewRow : ItemsControl
    {
        internal GroupGridView ParentGridView;
        internal ItemsControl ParentItemControl;

        public int GroupGridViewRowIndex
        {
            get { return (int)GetValue(GroupGridViewRowIndexProperty); }
            internal set { SetValue(GroupGridViewRowIndexProperty, value); }
        }

        public static readonly DependencyProperty GroupGridViewRowIndexProperty =
            DependencyProperty.Register(nameof(GroupGridViewRowIndex), typeof(int), typeof(GroupGridViewRow));

        public int GroupGridViewRowAlternationIndex
        {
            get { return (int)GetValue(GroupGridViewRowAlternationIndexProperty); }
            internal set { SetValue(GroupGridViewRowAlternationIndexProperty, value); }
        }
        
        public static readonly DependencyProperty GroupGridViewRowAlternationIndexProperty =
            DependencyProperty.Register(nameof(GroupGridViewRowAlternationIndex), typeof(int), typeof(GroupGridViewRow));

        public string ItemsSourceDisplayMemberPath
        {
            get { return (string)GetValue(ItemsSourceDisplayMemberPathProperty); }
            set { SetValue(ItemsSourceDisplayMemberPathProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceDisplayMemberPathProperty =
            DependencyProperty.Register(nameof(ItemsSourceDisplayMemberPath), typeof(string), typeof(GroupGridViewRow));

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register(nameof(IsExpanded), typeof(bool), typeof(GroupGridViewRow), new PropertyMetadata(OnIsExpandedChanged));

        private static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var row = d as GroupGridViewRow;
            if((bool)e.NewValue)
            {
                for (int i = 0; i < row.Items.Count; i++)
                {
                    if (row.ItemContainerGenerator.ContainerFromIndex(i) is GroupGridViewRow itemRow)
                    {
                        row.ParentGridView.Rows.InsertRow(row.ParentGridView.Rows.IndexOf(row) + i + 1, itemRow);
                    }
                }
            }
            else
            {
                for (int i = 0; i < row.Items.Count; i++)
                {
                    if (row.ItemContainerGenerator.ContainerFromIndex(i) is GroupGridViewRow itemRow)
                    {
                        row.ParentGridView.Rows.RemoveRow(itemRow);
                    }
                }
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new GroupGridViewRow();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is GroupGridViewRow;
        }

        private int _count = 0;
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            GroupGridViewRow row = element as GroupGridViewRow;
            _count++;
            row.ParentItemControl = this;
            row.ParentGridView = ParentGridView;
            var index = ParentGridView.Rows.IndexOf(this) + _count;
            ParentGridView.Rows.InsertRow(index, row);
            row.ItemsSourceDisplayMemberPath = ItemsSourceDisplayMemberPath;
            if (!string.IsNullOrEmpty(ItemsSourceDisplayMemberPath))
            {
                row.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(ItemsSourceDisplayMemberPath));
            }
        }
    }
}
