using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs.ListBox
{
    public class LazyFlushListBox : System.Windows.Controls.ListBox
    {
        private ScrollViewer _scrollViewer;

        public LazyFlushListBox()
        {
            Unloaded += LazyFlushListBox_Unloaded;
        }

        public string IsFlushMemberPath
        {
            get { return (string)GetValue(IsFlushMemberPathProperty); }
            set { SetValue(IsFlushMemberPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsFlushMemberPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFlushMemberPathProperty =
            DependencyProperty.Register("IsFlushMemberPath", typeof(string), typeof(LazyFlushListBox));

        public ICommand LazyLoadCommand
        {
            get { return (ICommand)GetValue(LazyLoadCommandProperty); }
            set { SetValue(LazyLoadCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LazyLoadCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LazyLoadCommandProperty =
            DependencyProperty.Register("LazyLoadCommand", typeof(ICommand), typeof(LazyFlushListBox));

        public int StartLazyLoadCount
        {
            get { return (int)GetValue(StartLazyLoadCountProperty); }
            set { SetValue(StartLazyLoadCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartLazyLoadCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartLazyLoadCountProperty =
            DependencyProperty.Register("StartLazyLoadCount", typeof(int), typeof(LazyFlushListBox), new PropertyMetadata(20));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollChanged -= ScrollViewer_ScrollChanged;
            }
            _scrollViewer = GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            double bottomOffset = e.ExtentHeight - e.VerticalOffset - e.ViewportHeight;
            if (e.VerticalOffset > 0 && bottomOffset == 0)
            {
                if (Items.Count >= StartLazyLoadCount)
                {
                    Console.WriteLine("开始懒加载");
                    LazyLoadCommand?.Execute(new object());
                }
            }
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is LazyFlushListBoxItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new LazyFlushListBoxItem();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            Type type = item.GetType();
            if (string.IsNullOrEmpty(IsFlushMemberPath))
            {
                return;
            }
            var isFLush = type.GetProperty(IsFlushMemberPath);
            if (isFLush == null)
            {
                return;
            }
            if (!(bool)isFLush.GetValue(item, null))
            {
                return;
            }
            var listItem = element as LazyFlushListBoxItem;
            if (listItem != null)
            {
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.Duration = TimeSpan.FromMilliseconds(500);
                doubleAnimation.From = 1;
                doubleAnimation.To = 0;
                doubleAnimation.RepeatBehavior = new RepeatBehavior(TimeSpan.FromSeconds(10));
                doubleAnimation.AutoReverse = true;
                listItem.BeginAnimation(OpacityProperty, doubleAnimation);
            }
        }

        private void LazyFlushListBox_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollChanged -= ScrollViewer_ScrollChanged;
            }
            Unloaded -= LazyFlushListBox_Unloaded;
        }
    }
}
