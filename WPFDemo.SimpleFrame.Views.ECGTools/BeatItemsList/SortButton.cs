using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class SortButton : RadioButton
    {
        public event EventHandler<SortEventArgs> SortArgsChanged;
        public SortEnum SortType
        {
            get { return (SortEnum)GetValue(SortTypeProperty); }
            set { SetValue(SortTypeProperty, value); }
        }
        public bool IsAsc
        {
            get { return (bool)GetValue(IsAscProperty); }
            private set { SetValue(IsAscProperty, value); }
        }
        public bool DefaultAsc
        {
            get { return (bool)GetValue(DefaultAscProperty); }
            set { SetValue(DefaultAscProperty, value); }
        }

        public static readonly DependencyProperty DefaultAscProperty =
            DependencyProperty.Register(nameof(DefaultAsc), typeof(bool), typeof(SortButton));
        public static readonly DependencyProperty IsAscProperty =
            DependencyProperty.Register(nameof(IsAsc), typeof(bool), typeof(SortButton), new PropertyMetadata(OnSortAscChanged));
        public static readonly DependencyProperty SortTypeProperty =
            DependencyProperty.Register(nameof(SortType), typeof(SortEnum), typeof(SortButton));

        private static void OnSortAscChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SortButton;
            if (control.IsChecked == true)
            {
                control.OnSortArgsChanged();
            }
        }

        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);
            if (DefaultAsc == IsAsc)
            {
                OnSortArgsChanged();
            }
            else
            {
                IsAsc = DefaultAsc;
            }
        }

        protected override void OnUnchecked(RoutedEventArgs e)
        {
            base.OnUnchecked(e);
            IsAsc = DefaultAsc;
        }

        protected override void OnClick()
        {
            if(IsChecked != true)
            {
                base.OnClick();
            }
            else
            {
                IsAsc = !IsAsc;
            }
        }

        private void OnSortArgsChanged()
        {
            SortArgsChanged?.Invoke(this, new SortEventArgs(new SortArgs(SortType, IsAsc)));
        }
    }
}
