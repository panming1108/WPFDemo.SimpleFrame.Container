using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xaml;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs.DataGrid
{
    public class DataGridAttachProperty
    {
        public static RoutedUICommand CopyCommand { get; private set; }
        private static readonly CommandBinding CopyCommandBinding;

        static DataGridAttachProperty()
        {
            CopyCommand = new RoutedUICommand();
            CopyCommandBinding = new CommandBinding(CopyCommand);
            CopyCommandBinding.Executed += OnCopy;
        }

        public static bool GetIsCopyEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsCopyEnabledProperty);
        }

        public static void SetIsCopyEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsCopyEnabledProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsCopyEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCopyEnabledProperty =
            DependencyProperty.RegisterAttached("IsCopyEnabled", typeof(bool), typeof(DataGridAttachProperty), new PropertyMetadata(false, OnCopyEnabledChanged));

        private static void OnCopyEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = d as Button;
            if(e.OldValue != e.NewValue && button != null)
            {
                button.CommandBindings.Add(CopyCommandBinding);
            }
        }

        private static void OnCopy(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is TextBlock)
            {
                var textBlock = e.Parameter as TextBlock;
                Clipboard.SetText(textBlock.Text);
            }
            else if (e.Parameter is ContentPresenter)
            {
                var content = e.Parameter as ContentPresenter;
                Clipboard.SetText(content.Content.ToString());                       
            }
            else
            {
                Clipboard.SetText(e.Parameter.ToString());
            }
        }
    }
}
