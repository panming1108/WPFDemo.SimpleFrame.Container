using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.LayOuts.Menu
{
    public class EMCNaviMenu : System.Windows.Controls.Menu
    {
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(EMCNaviMenu), new PropertyMetadata(null));



        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(EMCNaviMenu), new PropertyMetadata(Orientation.Horizontal));



        public EMCNaviMenu()
        {
            MenuCommands.SelectedCommand = new DelegateCommand<object>(OnSelected);
        }

        private void OnSelected(object obj)
        {
            if(obj != null)
            {
                SelectedItem = obj;
            }
        }
    }
}
