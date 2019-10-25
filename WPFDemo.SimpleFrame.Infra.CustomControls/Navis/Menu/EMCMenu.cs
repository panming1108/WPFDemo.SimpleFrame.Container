using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.Navis.Menu
{
    public class EMCMenu : System.Windows.Controls.Menu
    {
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is EMCMenuItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new EMCMenuItem();
        }

        public ICommand ItemCommand
        {
            get { return (ICommand)GetValue(ItemCommandProperty); }
            set { SetValue(ItemCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemCommandProperty =
            DependencyProperty.Register("ItemCommand", typeof(ICommand), typeof(EMCMenu));



        public ICommand ItemMouseOverCommand
        {
            get { return (ICommand)GetValue(ItemMouseOverCommandProperty); }
            set { SetValue(ItemMouseOverCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemMouseOverCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemMouseOverCommandProperty =
            DependencyProperty.Register("ItemMouseOverCommand", typeof(ICommand), typeof(EMCMenu));



    }
}
