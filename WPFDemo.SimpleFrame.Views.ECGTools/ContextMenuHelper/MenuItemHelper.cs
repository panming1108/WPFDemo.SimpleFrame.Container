using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Views.ECGTools.ContextMenuHelper
{
    public class MenuItemHelper
    {
        public static MenuItem GetMenuItem(string header, string InputGestureText)
        {
            return GetMenuItem(header, InputGestureText, null, null, null);
        }

        public static MenuItem GetMenuItem(string header, string InputGestureText, RoutedEventHandler routedEventHandler)
        {
            return GetMenuItem(header, InputGestureText, routedEventHandler, null, null);
        }

        public static MenuItem GetMenuItem(string header, string InputGestureText, RoutedEventHandler routedEventHandler, object tag)
        {
            return GetMenuItem(header, InputGestureText, routedEventHandler, tag, null);
        }

        public static MenuItem GetMenuItem(string header, string InputGestureText, IEnumerable itemsSource)
        {
            return GetMenuItem(header, InputGestureText, null, null, itemsSource);
        }

        public static MenuItem GetMenuItem(string header, string InputGestureText, RoutedEventHandler routedEventHandler, object tag, IEnumerable itemsSource)
        {
            MenuItem menuItem = new MenuItem() 
            { 
                Header = header, 
                InputGestureText = string.IsNullOrEmpty(InputGestureText) ? string.Empty : "(" + InputGestureText + ")", 
                HorizontalContentAlignment = HorizontalAlignment.Center, 
                VerticalContentAlignment = VerticalAlignment.Center, 
                Tag = tag,
                ItemsSource = itemsSource
            };
            if(routedEventHandler != null)
            {
                menuItem.AddHandler(MenuItem.ClickEvent, routedEventHandler);
            }
            return menuItem;
        }

        private static void ClearMultiLevelMenuItems(IEnumerable multiLevelMenuItems, RoutedEventHandler routedEventHandler)
        {
            foreach (var item in multiLevelMenuItems)
            {
                if (!(item is MenuItem))
                {
                    continue;
                }
                var menuItem = item as MenuItem;
                menuItem.RemoveHandler(MenuItem.ClickEvent, routedEventHandler);
                if (menuItem.Items.Count > 0)
                {
                    ClearMultiLevelMenuItems(menuItem.Items, routedEventHandler);
                }
            }
        }

        public static void RemoveClickEventHander(MenuItem menuItem, RoutedEventHandler routedEventHandler)
        {
            if(routedEventHandler == null)
            {
                return;
            }
            if(menuItem.Items.Count <= 0)
            {
                menuItem.RemoveHandler(MenuItem.ClickEvent, routedEventHandler);
            }
            else
            {
                ClearMultiLevelMenuItems(menuItem.Items, routedEventHandler);
            }
        }
    }
}
