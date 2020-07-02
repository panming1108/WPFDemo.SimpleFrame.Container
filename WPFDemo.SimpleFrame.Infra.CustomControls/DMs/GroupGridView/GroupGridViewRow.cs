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
            DependencyProperty.Register(nameof(IsExpanded), typeof(bool), typeof(GroupGridViewRow));

        public int GroupGridViewAlternationIndex
        {
            get { return (int)GetValue(GroupGridViewAlternationIndexProperty); }
            private set { SetValue(GroupGridViewAlternationIndexProperty, value); }
        }

        public static readonly DependencyProperty GroupGridViewAlternationIndexProperty =
            DependencyProperty.Register(nameof(GroupGridViewAlternationIndex), typeof(int), typeof(GroupGridViewRow));

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new GroupGridViewRow();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is GroupGridViewRow;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            GroupGridViewRow row = element as GroupGridViewRow;
            row.ItemsSourceDisplayMemberPath = ItemsSourceDisplayMemberPath;
            if (!string.IsNullOrEmpty(ItemsSourceDisplayMemberPath))
            {
                row.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(ItemsSourceDisplayMemberPath));
            }
        }
    }
}
