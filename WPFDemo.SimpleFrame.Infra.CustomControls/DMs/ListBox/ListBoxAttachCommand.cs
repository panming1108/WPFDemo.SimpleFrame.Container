using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs.ListBox
{
    public class ListBoxAttachCommand
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
            DependencyProperty.RegisterAttached("SelectionChangedCommand", typeof(ICommand), typeof(ListBoxAttachCommand), new PropertyMetadata(OnCommandChanged));
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
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(ListBoxAttachCommand));
        #endregion


        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (System.Windows.Controls.ListBox)d;

            element.AddHandler(System.Windows.Controls.ListBox.SelectionChangedEvent, new RoutedEventHandler(OnHandler));
            element.AddHandler(System.Windows.Controls.ListBox.UnloadedEvent, new RoutedEventHandler(OnUnloadedHandler));
        }

        private static void OnUnloadedHandler(object sender, RoutedEventArgs e)
        {
            var element = (System.Windows.Controls.ListBox)sender;
            element.RemoveHandler(System.Windows.Controls.ListBox.SelectionChangedEvent, new RoutedEventHandler(OnHandler));
            element.RemoveHandler(System.Windows.Controls.ListBox.UnloadedEvent, new RoutedEventHandler(OnUnloadedHandler));
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
