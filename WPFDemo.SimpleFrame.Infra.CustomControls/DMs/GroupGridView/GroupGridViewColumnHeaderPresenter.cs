using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs
{
    public class GroupGridViewColumnHeaderPresenter : ItemsControl
    {
        public double ColumnHeaderHeight
        {
            get { return (double)GetValue(ColumnHeaderHeightProperty); }
            set { SetValue(ColumnHeaderHeightProperty, value); }
        }

        public static readonly DependencyProperty ColumnHeaderHeightProperty =
            DependencyProperty.Register(nameof(ColumnHeaderHeight), typeof(double), typeof(GroupGridViewColumnHeaderPresenter));

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new GroupGridViewColumnHeader();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is GroupGridViewColumnHeader;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            GroupGridViewColumnHeader columnHeader = element as GroupGridViewColumnHeader;
            columnHeader.Height = ColumnHeaderHeight;
        }
    }
}
