using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.Navis.Menu
{
    public class MenuItemAttachProperty
    {
        [AttachedPropertyBrowsableForType(typeof(MenuItem))]
        public static bool GetIsImageIcon(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsImageIconProperty);
        }

        public static void SetIsImageIcon(DependencyObject obj, bool value)
        {
            obj.SetValue(IsImageIconProperty, value);
        }
        
        // Using a DependencyProperty as the backing store for IsImageIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsImageIconProperty =
            DependencyProperty.RegisterAttached("IsImageIcon", typeof(bool), typeof(MenuItemAttachProperty));
    }
}
