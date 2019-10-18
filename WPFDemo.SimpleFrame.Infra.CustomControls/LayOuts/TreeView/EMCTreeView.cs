using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.LayOuts.TreeView
{
    public class EMCTreeView : System.Windows.Controls.TreeView
    {
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
            SelectedTreeViewItem = e.NewValue;
        }
    }
}
