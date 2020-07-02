using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs
{
    public class GroupGridViewColumnHeader : ContentControl
    {
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(object), typeof(GroupGridViewColumnHeader));
    }
}
