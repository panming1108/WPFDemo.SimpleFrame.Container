using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WPFDemo.SimpleFrame.Infra.CustomControls.Schedules.Calendar;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.Schedules.DateTimePicker
{
    public class EMCDateTimePicker : ContentControl
    {
        public string WaterMark
        {
            get { return (string)GetValue(WaterMarkProperty); }
            set { SetValue(WaterMarkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WaterMark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WaterMarkProperty =
            DependencyProperty.Register("WaterMark", typeof(string), typeof(EMCDateTimePicker));


        private CalendarWithHourAndMin Calendar { get; set; }
        private ToggleButton ToggleButton { get; set; }

        static EMCDateTimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EMCDateTimePicker), new FrameworkPropertyMetadata(typeof(EMCDateTimePicker)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Calendar = new CalendarWithHourAndMin();
            Calendar.ClickCallBack = new Action(
                () =>
                {
                    Content = Calendar.SelectedDate.Value;
                    ToggleButton.IsChecked = false;
                    Calendar.SelectedDate = DateTime.Now;
                });
            ToggleButton = GetTemplateChild("PART_DropDownToggle") as ToggleButton;
            Popup popup = GetTemplateChild("PART_Popup") as Popup;
            popup.Child = Calendar;
            ToggleButton.Click += ToggleButton_Click;
            this.Unloaded += DateTimePicker_Unloaded;
        }

        private void DateTimePicker_Unloaded(object sender, RoutedEventArgs e)
        {
            ToggleButton.Click -= ToggleButton_Click;
            this.Unloaded -= DateTimePicker_Unloaded;
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {

            Calendar.SelectedDate = DateTime.Now;
            Calendar.TextBox_Hour.Text = $"{Calendar.SelectedDate.Value.Hour:D2}";
            Calendar.TextBox_Min.Text = $"{Calendar.SelectedDate.Value.Minute:D2}";
            Calendar.TextBox_Second.Text = $"{Calendar.SelectedDate.Value.Second:D2}";
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            if (oldContent == newContent || Calendar == null)
            {
                return;
            }
            if (newContent is DateTime)
            {
                Calendar.SelectedDate = (DateTime)newContent;
                if (Calendar.TextBox_Hour != null &&
                    Calendar.TextBox_Min != null &&
                    Calendar.TextBox_Second != null)
                {
                    Calendar.TextBox_Hour.Text = $"{Calendar.SelectedDate.Value.Hour:D2}";
                    Calendar.TextBox_Min.Text = $"{Calendar.SelectedDate.Value.Minute:D2}";
                    Calendar.TextBox_Second.Text = $"{Calendar.SelectedDate.Value.Second:D2}";
                }
            }
        }
    }
}
