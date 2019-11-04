using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs.DataGrid
{
    public class IconsItemsControl : ItemsControl
    {
        public string IconSourceMemberPath
        {
            get { return (string)GetValue(IconSourceMemberPathProperty); }
            set { SetValue(IconSourceMemberPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconSourceMemberPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconSourceMemberPathProperty =
            DependencyProperty.Register("IconSourceMemberPath", typeof(string), typeof(IconsItemsControl));

        public string IconToolTipMemberPath
        {
            get { return (string)GetValue(IconToolTipMemberPathProperty); }
            set { SetValue(IconToolTipMemberPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconToolTipMemberPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconToolTipMemberPathProperty =
            DependencyProperty.Register("IconToolTipMemberPath", typeof(string), typeof(IconsItemsControl));

        public string IconCommandMemberPath
        {
            get { return (string)GetValue(IconCommandMemberPathProperty); }
            set { SetValue(IconCommandMemberPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconCommandMemberPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconCommandMemberPathProperty =
            DependencyProperty.Register("IconCommandMemberPath", typeof(string), typeof(IconsItemsControl));

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is IconButton;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new IconButton();
        }
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            var icon = element as IconButton;
            SetIconProperties(icon, item);
            SetIconVisibilityConverter(icon);
            base.PrepareContainerForItemOverride(element, item);
        }

        private void SetIconProperties(IconButton icon, object item)
        {
            if(string.IsNullOrEmpty(IconSourceMemberPath))
            {
                return;
            }
            var sourceValue = item.GetType().GetProperty(IconSourceMemberPath);
            if (sourceValue == null)
            {
                return;
            }
            string source = sourceValue.GetValue(item, null) as string;
            icon.Source = new BitmapImage(new Uri(source, UriKind.RelativeOrAbsolute));
            if(!string.IsNullOrEmpty(IconToolTipMemberPath))
            {
                var tooltipValue = item.GetType().GetProperty(IconToolTipMemberPath);
                if (tooltipValue != null)
                {
                    string toolTip = tooltipValue.GetValue(item, null) as string;
                    icon.ToolTip = toolTip;
                }
            }
            if (!string.IsNullOrEmpty(IconCommandMemberPath))
            {
                var commandValue = item.GetType().GetProperty(IconCommandMemberPath);
                if (commandValue != null)
                {
                    ICommand command = commandValue.GetValue(item, null) as ICommand;
                    icon.Command = command;
                    icon.CommandParameter = this.DataContext;
                }
            }
        }

        private void SetIconVisibilityConverter(IconButton icon)
        {
            MultiBinding multiBinding = new MultiBinding();
            Binding binding1 = new Binding();
            binding1.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(IconsItemsControl), 1);
            binding1.Path = new PropertyPath("DataContext");
            Binding binding2 = new Binding();
            multiBinding.Bindings.Add(binding1);
            multiBinding.Bindings.Add(binding2);
            multiBinding.Converter = new DataGridIconsConverter();
            icon.SetBinding(VisibilityProperty, multiBinding);
        }
    }
}
