﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using WPFDemo.SimpleFrame.Infra.CustomControls.Navis.ContextMenu;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.Navis.Menu
{
    public class EMCMenuItem : MenuItem
    {
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

        public ICommand MouseOverCommand
        {
            get { return (ICommand)GetValue(MouseOverCommandProperty); }
            set { SetValue(MouseOverCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MouseOverCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseOverCommandProperty =
            DependencyProperty.Register("MouseOverCommand", typeof(ICommand), typeof(EMCMenuItem));

        private KeyBinding keyCommand;

        public EMCMenuItem()
        {
            Loaded += EMCMenuItem_Loaded;
            Unloaded += EMCMenuItem_Unloaded;
        }

        private void RegisteKeyCommands()
        {
            if(keyCommand != null)
            {
                return;
            }
            if(!string.IsNullOrEmpty(InputGestureText) && Command != null)
            {
                keyCommand = new KeyBinding(Command, Key.Q, ModifierKeys.Control);
                keyCommand.CommandParameter = this.DataContext;
                this.InputBindings.Add(keyCommand);
            }
        }

        private void EMCMenuItem_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= EMCMenuItem_Loaded;
            this.Unloaded -= EMCMenuItem_Unloaded;
        }

        private void EMCMenuItem_Loaded(object sender, RoutedEventArgs e)
        {
            RegisteKeyCommands();
            AddHandler(FrameworkElement.MouseEnterEvent, new RoutedEventHandler(ItemMouseOverHandler));
            var binding =  GetBindingExpression(RootContextMenuProperty);
            var root = binding.DataItem as EMCContextMenu;
            root.InputGestureKeyDown += Root_InputGestureKeyDown;
        }

        private void Root_InputGestureKeyDown(object sender, KeyEventArgs e)
        {
            if(!string.IsNullOrEmpty(InputGestureText) && Command != null)
            {
                if(InputGestureText.Contains("+"))
                {
                    string[] data = InputGestureText.Split('+');
                    if(Enum.TryParse(data[0], out ModifierKeys modifier) && Enum.TryParse(data[1], out Key key))
                    {
                        if(Keyboard.Modifiers == modifier && e.KeyStates == Keyboard.GetKeyStates(key))
                        {
                            Command.Execute(CommandParameter);
                        }
                    }
                }
                else
                {
                    if(Enum.TryParse(InputGestureText, out Key key))
                    {
                        if (e.KeyStates == Keyboard.GetKeyStates(key))
                        {
                            Command.Execute(CommandParameter);
                        }
                    }
                }
            }
        }

        private void ItemMouseOverHandler(object sender, RoutedEventArgs e)
        {
            var item = sender as EMCMenuItem;

            var command = (ICommand)item.GetValue(MouseOverCommandProperty);

            command?.Execute(DataContext);
        }

        public object RootContextMenu
        {
            get { return (object)GetValue(RootContextMenuProperty); }
            set { SetValue(RootContextMenuProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RootContextMenu.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RootContextMenuProperty =
            DependencyProperty.Register("RootContextMenu", typeof(object), typeof(EMCMenuItem));

        public bool IsGroupEnd
        {
            get { return (bool)GetValue(IsGroupEndProperty); }
            set { SetValue(IsGroupEndProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsGroupEnd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsGroupEndProperty =
            DependencyProperty.Register("IsGroupEnd", typeof(bool), typeof(EMCMenuItem), new PropertyMetadata(false));



        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.Register("GroupName", typeof(string), typeof(EMCMenuItem));

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var menuItem = element as EMCMenuItem;
            if (!string.IsNullOrEmpty(menuItem.GroupName))
            {
                if (!_groupList.Keys.Contains(menuItem.GroupName))
                {
                    _groupList.Add(menuItem.GroupName, new List<EMCMenuItem>());
                }
                _groupList[menuItem.GroupName].Add(menuItem);
            }

            if (item == _lastMenuItem)
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