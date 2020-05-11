using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs.ListBox
{
    public class ListBoxAttachProperty
    {
        #region Command
        public static ICommand GetSelectionChangedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(SelectionChangedCommandProperty);
        }

        public static void SetSelectionChangedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(SelectionChangedCommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for SelectionChangedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionChangedCommandProperty =
            DependencyProperty.RegisterAttached("SelectionChangedCommand", typeof(ICommand), typeof(ListBoxAttachProperty), new PropertyMetadata(OnCommandChanged));
        #endregion

        #region CommandParameter
        public static object GetCommandParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(CommandParameterProperty, value);
        }

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(ListBoxAttachProperty));
        #endregion

        #region SelectedItems
        [AttachedPropertyBrowsableForType(typeof(System.Windows.Controls.ListBox))]
        public static IList GetSelectedItems(DependencyObject obj)
        {
            return (IList)obj.GetValue(SelectedItemsProperty);
        }

        public static void SetSelectedItems(DependencyObject obj, IList value)
        {
            obj.SetValue(SelectedItemsProperty, value);
        }

        // Using a DependencyProperty as the backing store for SelectedItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(ListBoxAttachProperty), new PropertyMetadata(OnSelectedItemsChanged));
        #endregion

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listBox = (System.Windows.Controls.ListBox)d;
            if (listBox != null)
            {
                foreach (var item in e.NewValue as IList)
                {
                    listBox.SelectedItems.Add(item);
                }
            }
        }

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (Selector)d;

            element.AddHandler(Selector.SelectionChangedEvent, new RoutedEventHandler(OnHandler));
            element.AddHandler(Selector.UnloadedEvent, new RoutedEventHandler(OnUnloadedHandler));
        }

        private static void OnUnloadedHandler(object sender, RoutedEventArgs e)
        {
            var element = (System.Windows.Controls.ListBox)sender;
            element.RemoveHandler(Selector.SelectionChangedEvent, new RoutedEventHandler(OnHandler));
            element.RemoveHandler(Selector.UnloadedEvent, new RoutedEventHandler(OnUnloadedHandler));
        }

        private static void OnHandler(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;

            var command = GetSelectionChangedCommand(element);

            var commandParameter = GetCommandParameter(element);

            command?.Execute(commandParameter);
        }
    }
}
