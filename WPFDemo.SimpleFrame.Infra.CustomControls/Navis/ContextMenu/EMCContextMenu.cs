using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFDemo.SimpleFrame.Infra.CustomControls.Navis.Menu;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.Navis.ContextMenu
{
    public class EMCContextMenu : System.Windows.Controls.ContextMenu
    {
        public event EventHandler<KeyEventArgs> InputGestureKeyDown;

        private object _lastMenuItem;
        private Dictionary<string, List<EMCMenuItem>> _groupList = new Dictionary<string, List<EMCMenuItem>>();

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
            DependencyProperty.Register("ItemCommand", typeof(ICommand), typeof(EMCContextMenu));

        public ICommand ItemMouseOverCommand
        {
            get { return (ICommand)GetValue(ItemMouseOverCommandProperty); }
            set { SetValue(ItemMouseOverCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemMouseOverCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemMouseOverCommandProperty =
            DependencyProperty.Register("ItemMouseOverCommand", typeof(ICommand), typeof(EMCContextMenu));

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            InputGestureKeyDown?.Invoke(this, e);
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var menuItem = element as EMCMenuItem;
            if(!string.IsNullOrEmpty(menuItem.GroupName))
            {
                if(!_groupList.Keys.Contains(menuItem.GroupName))
                { 
                    _groupList.Add(menuItem.GroupName, new List<EMCMenuItem>());
                }
                _groupList[menuItem.GroupName].Add(menuItem);
            }

            if(item == _lastMenuItem)
            {
                foreach (var group in _groupList)
                {
                    group.Value.Last().IsGroupEnd = true;
                }

                menuItem.IsGroupEnd = false;
            }
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            foreach (var item in newValue)
            {
                _lastMenuItem = item;
            }
        }
    }
}
