using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.Editors.RadioButton
{
    public class RadioButtonGroup : ListBox
    {
        public Style RadioButtonStyle
        {
            get { return (Style)GetValue(RadioButtonStyleProperty); }
            set { SetValue(RadioButtonStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RadioButtonStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadioButtonStyleProperty =
            DependencyProperty.Register("RadioButtonStyle", typeof(Style), typeof(RadioButtonGroup));


        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(RadioButtonGroup));

        public IList NotSelectedItems
        {
            get { return (IList)GetValue(NotSelectedItemsProperty); }
            set { SetValue(NotSelectedItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NotSelectedItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotSelectedItemsProperty =
            DependencyProperty.Register("NotSelectedItems", typeof(IList), typeof(RadioButtonGroup));

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            Type type = ItemsSource.GetType();
            IList list = (IList)Activator.CreateInstance(type, true);
            foreach (var item in ItemsSource)
            {
                if(item != SelectedItem)
                {
                    list.Add(item);
                }
            }
            NotSelectedItems = list;
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(RadioButtonGroup));




    }
}
