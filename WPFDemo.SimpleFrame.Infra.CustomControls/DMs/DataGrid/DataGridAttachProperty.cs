using System;
using System.Collections;
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

        #region IsCopyEnable
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
        #endregion

        #region IconPosition
        public static IconPosition GetIconPosition(DependencyObject obj)
        {
            return (IconPosition)obj.GetValue(IconPositionProperty);
        }

        public static void SetIconPosition(DependencyObject obj, IconPosition value)
        {
            obj.SetValue(IconPositionProperty, value);
        }

        // Using a DependencyProperty as the backing store for IconPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconPositionProperty =
            DependencyProperty.RegisterAttached("IconPosition", typeof(IconPosition), typeof(DataGridAttachProperty));
        #endregion

        #region IconsSource
        public static IEnumerable GetIconsSource(DependencyObject obj)
        {
            return (IEnumerable)obj.GetValue(IconsSourceProperty);
        }

        public static void SetIconsSource(DependencyObject obj, IEnumerable value)
        {
            obj.SetValue(IconsSourceProperty, value);
        }

        // Using a DependencyProperty as the backing store for IconsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconsSourceProperty =
            DependencyProperty.RegisterAttached("IconsSource", typeof(IEnumerable), typeof(DataGridAttachProperty));
        #endregion



        public static IMultiValueConverter GetValueConverter(DependencyObject obj)
        {
            return (IMultiValueConverter)obj.GetValue(ValueConverterProperty);
        }

        public static void SetValueConverter(DependencyObject obj, IMultiValueConverter value)
        {
            obj.SetValue(ValueConverterProperty, value);
        }

        // Using a DependencyProperty as the backing store for ValueConverter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueConverterProperty =
            DependencyProperty.RegisterAttached("ValueConverter", typeof(IMultiValueConverter), typeof(DataGridAttachProperty), new PropertyMetadata(OnValueConverterChanged));

        private static void OnValueConverterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var valueConverter = e.NewValue as IMultiValueConverter;
            DataGridIconsConverter.ValueConverter = valueConverter;
        }

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

    public enum IconPosition
    {
        Front,
        Behind
    }
}
