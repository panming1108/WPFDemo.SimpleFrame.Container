using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools.ContextMenuHelper
{
    public class MenuItemHelper
    {
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
                ItemsSource = itemsSource,
            };
            if(routedEventHandler != null)
            {
                menuItem.AddHandler(MenuItem.ClickEvent, routedEventHandler);
                if (itemsSource != null)
                {
                    menuItem.AddHandler(MenuItem.PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(OnMultiLevelMenuItemHeaderClick));
                }
            }
            return menuItem;
        }

        private static void OnMultiLevelMenuItemHeaderClick(object sender, MouseButtonEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var source = e.OriginalSource;

            RoutedEventArgs routedEventArgs = new RoutedEventArgs(MenuItem.ClickEvent, sender);
            switch (source)
            {
                case TextBlock textBlock when textBlock.DataContext != menuItem.Header:
                    return;

                case TextBlock textBlock:
                    menuItem.RaiseEvent(routedEventArgs);
                    break;

                case MenuItem oriMenuItem:
                    oriMenuItem.RaiseEvent(routedEventArgs);
                    break;

                default:
                    var panel = source as FrameworkElement;
                    var menu = panel.TemplatedParent as MenuItem;

                    if (menu.Header == menuItem.Header)
                    {
                        menuItem.RaiseEvent(routedEventArgs);
                    }

                    break;
            }
            var contextMenu = FindVisualParent<ContextMenu>(menuItem);
            if(contextMenu != null)
            {
                contextMenu.IsOpen = false;
            }
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
            menuItem.RemoveHandler(MenuItem.ClickEvent, routedEventHandler);
            menuItem.RemoveHandler(MenuItem.PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(OnMultiLevelMenuItemHeaderClick));
            if (menuItem.Items.Count > 0)
            {
                ClearMultiLevelMenuItems(menuItem.Items, routedEventHandler);
            }
        }

        public static T FindVisualParent<T>(DependencyObject d) where T : DependencyObject
        {
            if (d == null)
            {
                throw new Exception();
            }

            while (d != null)
            {
                var result = d as T;

                if (result != null)
                {
                    return result;
                }

                d = GetParent(d);
            }

            return null;
        }

        private static DependencyObject GetParent(DependencyObject element)
        {
            DependencyObject parent = null;
            try
            {
                //// fix for bug 188967.
                parent = VisualTreeHelper.GetParent(element);
            }
            catch (InvalidOperationException)
            {
                parent = null;
            }
            if (parent == null)
            {
                var frameworkElement = element as FrameworkElement;
                if (frameworkElement != null)
                    parent = frameworkElement.Parent;

                var frameworkContentElement = element as FrameworkContentElement;
                if (frameworkContentElement != null)
                    parent = frameworkContentElement.Parent;
            }
            return parent;
        }
    }
}
