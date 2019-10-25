using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.Editors.CheckBox
{
    public class CheckBoxGroup : ListBox
    {
        public Style CheckBoxStyle
        {
            get { return (Style)GetValue(CheckBoxStyleProperty); }
            set { SetValue(CheckBoxStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CheckBoxStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CheckBoxStyleProperty =
            DependencyProperty.Register("CheckBoxStyle", typeof(Style), typeof(CheckBoxGroup));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(CheckBoxGroup));

        public IList NotSelectedItems
        {
            get { return (IList)GetValue(NotSelectedItemsProperty); }
            set { SetValue(NotSelectedItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NotSelectedItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotSelectedItemsProperty =
            DependencyProperty.Register("NotSelectedItems", typeof(IList), typeof(CheckBoxGroup));

        public IList HaveSelectedItems
        {
            get { return (IList)GetValue(HaveSelectedItemsProperty); }
            set { SetValue(HaveSelectedItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HaveSelectedItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HaveSelectedItemsProperty =
            DependencyProperty.Register("HaveSelectedItems", typeof(IList), typeof(CheckBoxGroup));

        public int MinimunSelected
        {
            get { return (int)GetValue(MinimunSelectedProperty); }
            set { SetValue(MinimunSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinimunSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimunSelectedProperty =
            DependencyProperty.Register("MinimunSelected", typeof(int), typeof(CheckBoxGroup), new PropertyMetadata(0));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(CheckBoxGroup));

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            Type type = ItemsSource.GetType();
            IList list1 = (IList)Activator.CreateInstance(type, true);
            IList list2 = (IList)Activator.CreateInstance(type, true);
            foreach (var item in ItemsSource)
            {
                if (!SelectedItems.Contains(item))
                {
                    list1.Add(item);
                }
            }
            foreach (var item in SelectedItems)
            {
                list2.Add(item);
            }
            HaveSelectedItems = list2;
            NotSelectedItems = list1;
        }
    }
}
