using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.Schedules
{
    [TemplatePart(Name = "PART_Calendar", Type = typeof(System.Windows.Controls.Calendar))]
    [TemplatePart(Name = "PART_Empty", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Confirm", Type = typeof(Button))]
    public class TwiceDatePicker : Control
    {
        private System.Windows.Controls.Calendar PART_Calendar;
        private Button PART_Empty;
        private Button PART_Confirm;

        private bool _isSelectedDatesChanging;
        public ObservableCollection<DateTime?> SelectedDates { get; private set; }

        public DateTime? StartTime
        {
            get { return (DateTime?)GetValue(StartTimeProperty); }
            set { SetValue(StartTimeProperty, value); }
        }
        public DateTime? EndTime
        {
            get { return (DateTime?)GetValue(EndTimeProperty); }
            set { SetValue(EndTimeProperty, value); }
        }
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(TwiceDatePicker));
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(TwiceDatePicker));
        public static readonly DependencyProperty EndTimeProperty =
            DependencyProperty.Register(nameof(EndTime), typeof(DateTime?), typeof(TwiceDatePicker), new PropertyMetadata(OnEndTimeChanged));
        public static readonly DependencyProperty StartTimeProperty =
            DependencyProperty.Register(nameof(StartTime), typeof(DateTime?), typeof(TwiceDatePicker), new PropertyMetadata(OnStartTimeChanged));

        public TwiceDatePicker()
        {
            SelectedDates = new ObservableCollection<DateTime?>() { null, null };
            SelectedDates.CollectionChanged += SelectedDates_CollectionChanged;
            Unloaded += TwiceDatePicker_Unloaded;
        }

        private void SelectedDates_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _isSelectedDatesChanging = true;
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    PART_Calendar.SelectedDates.Clear();
                    foreach (var item in SelectedDates)
                    {
                        if (item != null)
                        {
                            PART_Calendar.SelectedDates.Add(item.Value);
                        }
                    }
                    StartTime = SelectedDates[0];
                    EndTime = SelectedDates[1];
                    break;
                default:
                    break;
            }
            _isSelectedDatesChanging = false;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UnloadedControls();
            PART_Calendar = GetTemplateChild("PART_Calendar") as System.Windows.Controls.Calendar;
            PART_Confirm = GetTemplateChild("PART_Confirm") as Button;
            PART_Empty = GetTemplateChild("PART_Empty") as Button;
            if(PART_Calendar != null)
            {
                PART_Calendar.SelectedDatesChanged += PART_Calendar_SelectedDatesChanged;
            }
            if(PART_Confirm != null)
            {
                PART_Confirm.Click += PART_Confirm_Click;
            }
            if(PART_Empty != null)
            {
                PART_Empty.Click += PART_Empty_Click;
            }
        }

        private void PART_Empty_Click(object sender, RoutedEventArgs e)
        {
            if(PART_Calendar != null)
            {
                SelectedDates[0] = null;
                SelectedDates[1] = null;
            }
        }

        private void PART_Confirm_Click(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        private void PART_Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if(_isSelectedDatesChanging)
            {
                return;
            }
            var newDate = (DateTime)e.AddedItems[0];
            if(SelectedDates[0] == null && SelectedDates[1] == null)
            {
                SelectedDates[0] = newDate;
            }
            else
            {
                if(SelectedDates[0] != null && newDate < SelectedDates[0])
                {
                    SelectedDates[1] = SelectedDates[0];
                }
                else
                {
                    SelectedDates[1] = newDate;
                }
            }
        }

        private static void OnEndTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TwiceDatePicker;
            control.SelectedDates[1] = (DateTime?)e.NewValue;
        }
        private static void OnStartTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TwiceDatePicker;
            control.SelectedDates[0] = (DateTime?)e.NewValue;
        }

        private void TwiceDatePicker_Unloaded(object sender, RoutedEventArgs e)
        {
            UnloadedControls();
            Unloaded -= TwiceDatePicker_Unloaded;
        }

        private void UnloadedControls()
        {
            if (PART_Calendar != null)
            {
                PART_Calendar.SelectedDatesChanged -= PART_Calendar_SelectedDatesChanged;
            }
            if (PART_Confirm != null)
            {
                PART_Confirm.Click -= PART_Confirm_Click;
            }
            if (PART_Empty != null)
            {
                PART_Empty.Click -= PART_Empty_Click;
            }
        }
    }
}
