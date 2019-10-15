using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.MVVM.VOnly
{
    public class FELifeAttachCommand
    {
        #region LoadedCommand

        public static readonly DependencyProperty LoadedCommandProperty =
            DependencyProperty.RegisterAttached("LoadedCommand",
                typeof(ICommand), typeof(FELifeAttachCommand)
                , new FrameworkPropertyMetadata(LoadedCommandChanged));

        private static void LoadedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;

            element.AddHandler(FrameworkElement.LoadedEvent, new RoutedEventHandler(element_LoadedHandler));
            element.AddHandler(FrameworkElement.UnloadedEvent, new RoutedEventHandler(element_Unload_Unload));
        }

        private static void element_Unload_Unload(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;

            element.RemoveHandler(FrameworkElement.LoadedEvent, new RoutedEventHandler(element_LoadedHandler));
            element.RemoveHandler(FrameworkElement.UnloadedEvent, new RoutedEventHandler(element_Unload_Unload));
        }

        private static void element_LoadedHandler(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;

            var command = GetLoadedCommand(element);

            command.Execute(null);
        }

        public static void SetLoadedCommand(FrameworkElement element, ICommand value)
        {
            element.SetValue(LoadedCommandProperty, value);
        }

        public static ICommand GetLoadedCommand(FrameworkElement element)
        {
            return (ICommand)element.GetValue(LoadedCommandProperty);
        }

        #endregion LoadedCommand


        #region UnloadedCommand

        public static readonly DependencyProperty UnLoadedCommandProperty =
            DependencyProperty.RegisterAttached("UnLoadedCommand",
                typeof(ICommand), typeof(FELifeAttachCommand)
                , new FrameworkPropertyMetadata(UnLoadedCommandChanged));

        private static void UnLoadedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;

            element.AddHandler(FrameworkElement.UnloadedEvent, new RoutedEventHandler(element_UnLoadedHandler));
        }

        private static void element_UnLoadedHandler(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;

            var command = GetUnLoadedCommand(element);

            command.Execute(null);

            element.RemoveHandler(FrameworkElement.UnloadedEvent, new RoutedEventHandler(element_UnLoadedHandler));
        }

        public static void SetUnLoadedCommand(FrameworkElement element, ICommand value)
        {
            element.SetValue(UnLoadedCommandProperty, value);
        }

        public static ICommand GetUnLoadedCommand(FrameworkElement element)
        {
            return (ICommand)element.GetValue(UnLoadedCommandProperty);
        }

        #endregion UnloadedCommand
    }
}
