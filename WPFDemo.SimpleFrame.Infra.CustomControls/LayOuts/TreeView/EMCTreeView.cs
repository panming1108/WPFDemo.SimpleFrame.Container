using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.LayOuts.TreeView
{
    public class EMCTreeView : System.Windows.Controls.TreeView
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new EMCTreeViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is EMCTreeViewItem;
        }
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(EMCTreeView), new PropertyMetadata(Orientation.Vertical));

        public object SelectedTreeViewItem
        {
            get { return (object)GetValue(SelectedTreeViewItemProperty); }
            set { SetValue(SelectedTreeViewItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTreeViewItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTreeViewItemProperty =
            DependencyProperty.Register("SelectedTreeViewItem", typeof(object), typeof(EMCTreeView));

        public EMCTreeView()
        {
            
        }

        protected override void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
        {
            base.OnSelectedItemChanged(e);
            if(e.NewValue != null)
            {
                SelectedTreeViewItem = e.NewValue;
            }
        }        
    }
}
