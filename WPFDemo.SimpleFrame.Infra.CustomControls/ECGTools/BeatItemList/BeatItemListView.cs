using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.ECGTools
{
    [TemplatePart(Name = "PART_BeatItemListBox", Type = typeof(BeatItemListBox))]
    public class BeatItemListView : Control
    {
        private BeatItemListBox PART_BeatItemListBox;
        public int Total
        {
            get { return (int)GetValue(TotalProperty); }
            set { SetValue(TotalProperty, value); }
        }
        public bool IsWide
        {
            get { return (bool)GetValue(IsWideProperty); }
            set { SetValue(IsWideProperty, value); }
        }
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(BeatItemListView));
        public static readonly DependencyProperty IsWideProperty =
            DependencyProperty.Register(nameof(IsWide), typeof(bool), typeof(BeatItemListView));
        public static readonly DependencyProperty TotalProperty =
            DependencyProperty.Register(nameof(Total), typeof(int), typeof(BeatItemListView));
        static BeatItemListView()
        {
            CommandManager.RegisterClassCommandBinding(typeof(BeatItemListView), new CommandBinding(BeatItemListViewCommands.SelectedAllCommand, SelectedAllExecuted, CanSelectedAllExecute));
            CommandManager.RegisterClassCommandBinding(typeof(BeatItemListView), new CommandBinding(BeatItemListViewCommands.ReverseSelectedCommand, ReverseSelectedExecuted, CanReverseSelectedExecute));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_BeatItemListBox = GetTemplateChild("PART_BeatItemListBox") as BeatItemListBox;
        }
        private static void SelectedAllExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var control = sender as BeatItemListView;
            if(control != null)
            {
                control.PART_BeatItemListBox.SelectAll();
            }
        }
        private static void ReverseSelectedExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var control = sender as BeatItemListView;
            if (control != null)
            {
                control.PART_BeatItemListBox.ReverseSelect();
            }
        }

        private static void CanReverseSelectedExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private static void CanSelectedAllExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
