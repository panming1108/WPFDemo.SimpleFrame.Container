using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.Schedules.Calendar
{
    public class CalendarWithHourAndMin : System.Windows.Controls.Calendar
    {
        public TextBox TextBox_Hour { get; set; }
        public TextBox TextBox_Min { get; set; }
        public TextBox TextBox_Second { get; set; }
        public TextBox TextBox_Year { get; set; }
        public TextBox TextBox_Month { get; set; }
        public TextBox TextBox_Day { get; set; }
        public Button BtnEnsure { get; set; }
        public Button BtnEmpty { get; set; }
        public Button BtnToDay { get; set; }
        public CalendarItem Calendar { get; set; }

        public Action ClickCallBack;
        static CalendarWithHourAndMin()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarWithHourAndMin), new FrameworkPropertyMetadata(typeof(CalendarWithHourAndMin)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            BtnEnsure = GetTemplateChild("PART_EnsureBtn") as Button;
            BtnEmpty = GetTemplateChild("PART_EmptyBtn") as Button;
            BtnToDay = GetTemplateChild("PART_ToDay") as Button;

            Calendar = GetTemplateChild("PART_CalendarItem") as CalendarItem;

            TextBox_Hour = GetTemplateChild("PART_Hour") as TextBox;
            TextBox_Min = GetTemplateChild("PART_Min") as TextBox;
            TextBox_Second = GetTemplateChild("PART_Second") as TextBox;
            TextBox_Year = GetTemplateChild("PART_Year") as TextBox;
            TextBox_Month = GetTemplateChild("PART_Month") as TextBox;
            TextBox_Day = GetTemplateChild("PART_Day") as TextBox;

            SelectedDate = DateTime.Now;
            if (SelectedDate != null)
            {
                TextBox_Hour.Text = $"{SelectedDate.Value.Hour:D2}";
                TextBox_Min.Text = $"{SelectedDate.Value.Minute:D2}";
                TextBox_Second.Text = $"{SelectedDate.Value.Second:D2}";
                TextBox_Year.Text = $"{DisplayDate.Year:D4}";
                TextBox_Month.Text = $"{DisplayDate.Month:D2}";
                TextBox_Day.Text = $"{DisplayDate.Day:D2}";
            }
            this.Loaded += CalendarWithHourAndMin_loaded;
            this.Unloaded += CalendarWithHourAndMin_Unloaded;
        }

        private void CalendarWithHourAndMin_loaded(object sender, RoutedEventArgs e)
        {
            BtnEnsure.Click += BtnEnsure_Click;
            BtnEmpty.Click += BtnEmpty_Click;
            BtnToDay.Click += BtnToDay_Click;
            TextBox_Hour.PreviewTextInput += Content_Change;
            TextBox_Min.PreviewTextInput += Content_Change;
            TextBox_Second.PreviewTextInput += Content_Change;
            TextBox_Year.PreviewTextInput += Content_Change;
            TextBox_Month.PreviewTextInput += Content_Change;
            TextBox_Day.PreviewTextInput += Content_Change;
            Calendar.PreviewMouseUp += Calendar_Click;
            this.Unloaded += CalendarWithHourAndMin_Unloaded;
        }

        private void BtnToDay_Click(object sender, RoutedEventArgs e)
        {
            SelectedDate = DateTime.Now;
            DisplayDate = DateTime.Now;
        }

        private void CalendarWithHourAndMin_Unloaded(object sender, RoutedEventArgs e)
        {
            BtnEnsure.Click -= BtnEnsure_Click;
            BtnEmpty.Click -= BtnEmpty_Click;
            BtnToDay.Click -= BtnToDay_Click;
            TextBox_Hour.PreviewTextInput -= Content_Change;
            TextBox_Min.PreviewTextInput -= Content_Change;
            TextBox_Second.PreviewTextInput -= Content_Change;
            TextBox_Year.PreviewTextInput -= Content_Change;
            TextBox_Month.PreviewTextInput -= Content_Change;
            TextBox_Day.PreviewTextInput -= Content_Change;
            Calendar.PreviewMouseUp -= Calendar_Click;
            this.Unloaded -= CalendarWithHourAndMin_Unloaded;
        }

        private void Calendar_Click(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.Captured is CalendarItem)
            {
                Mouse.Capture(null);
            }
        }

        private void Content_Change(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void BtnEmpty_Click(object sender, RoutedEventArgs e)
        {
            TextBox_Hour.Text = string.Empty;
            TextBox_Min.Text = string.Empty;
            TextBox_Second.Text = string.Empty;
        }

        private void BtnEnsure_Click(object sender, RoutedEventArgs e)
        {
            int year;
            int month;
            int day;
            int hour;
            int min;
            int sec;
            if (!int.TryParse(TextBox_Year.Text, out int result_year))
            {
                year = DateTime.Now.Year;
            }
            else
            {
                year = result_year;
            }

            if (!int.TryParse(TextBox_Month.Text, out int result_month))
            {
                month = 1;
            }
            else
            {
                month = result_month;
            }
            if (!int.TryParse(TextBox_Day.Text, out int result_day))
            {
                day = 1;
            }
            else
            {
                day = result_day;
            }
            if (!int.TryParse(TextBox_Hour.Text, out int result))
            {
                hour = 0;
            }
            else
            {
                hour = result;
            }
            if (!int.TryParse(TextBox_Min.Text, out int result1))
            {
                min = 0;
            }
            else
            {
                min = result1;
            }
            if (!int.TryParse(TextBox_Second.Text, out int result2))
            {
                sec = 0;
            }
            else
            {
                sec = result2;
            }
            if (hour >= 24 || hour < 0)
            {
                TextBox_Hour.Text = "00";
                hour = 0;
            }
            if (min >= 60 || min < 0)
            {
                TextBox_Min.Text = "00";
                min = 0;
            }
            if (sec >= 60 || sec < 0)
            {
                TextBox_Second.Text = "00";
                sec = 0;
            }
            if(year < 1800)
            {
                TextBox_Year.Text = DateTime.Now.Year.ToString();
                year = DateTime.Now.Year;
            }
            if (month <= 0 || month > 12)
            {
                TextBox_Month.Text = DateTime.Now.Month.ToString();
                month = DateTime.Now.Month;
            }
            if (day > 0)
            {
                if(month == 2)
                {
                    if(DateTime.IsLeapYear(year))
                    {
                        if(day > 29)
                        {
                            TextBox_Day.Text = DateTime.Now.Day.ToString();
                            day = DateTime.Now.Day;
                        }
                    }
                    else
                    {
                        if(day > 28)
                        {
                            TextBox_Day.Text = DateTime.Now.Day.ToString();
                            day = DateTime.Now.Day;
                        }
                    }
                }
                else if(month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
                {
                    if(day > 31)
                    {
                        TextBox_Day.Text = DateTime.Now.Day.ToString();
                        day = DateTime.Now.Day;
                    }
                }
                else
                {
                    if (day > 30)
                    {
                        TextBox_Day.Text = DateTime.Now.Day.ToString();
                        day = DateTime.Now.Day;
                    }
                }
            }
            else
            {
                TextBox_Day.Text = DateTime.Now.Day.ToString();
                day = DateTime.Now.Day;
            }
            if (!SelectedDate.HasValue)
            {
                SelectedDate = DateTime.Now;
            }
            else
            {
                //SelectedDate = new DateTime(SelectedDate.Value.Year, SelectedDate.Value.Month, SelectedDate.Value.Day, hour, min, sec);
                SelectedDate = new DateTime(year, month, day, hour, min, sec);
            }
            ClickCallBack.Invoke();
        }

        protected override void OnDisplayDateChanged(CalendarDateChangedEventArgs e)
        {
            base.OnDisplayDateChanged(e);
            if(TextBox_Year == null || TextBox_Month == null || TextBox_Day == null)
            {
                return;
            }
            if(e.AddedDate.HasValue)
            {
                TextBox_Year.Text = $"{e.AddedDate.Value.Year:D4}";
                TextBox_Month.Text = $"{e.AddedDate.Value.Month:D2}";
                TextBox_Day.Text = $"{e.AddedDate.Value.Day:D2}";
            }
        }

        protected override void OnSelectedDatesChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectedDatesChanged(e);
            if (TextBox_Year == null || TextBox_Month == null || TextBox_Day == null)
            {
                return;
            }
            if(e.AddedItems != null && e.AddedItems.Count > 0)
            {
                TextBox_Year.Text = $"{DateTime.Parse(e.AddedItems[0].ToString()).Year:D4}";
                TextBox_Month.Text = $"{DateTime.Parse(e.AddedItems[0].ToString()).Month:D2}";
                TextBox_Day.Text = $"{DateTime.Parse(e.AddedItems[0].ToString()).Day:D2}";
            }
        }
    }
}
