using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    /// <summary>
    /// BeatItemsListScrollBar.xaml 的交互逻辑
    /// </summary>
    public partial class BeatItemsListScrollBar : UserControl
    {
        public event EventHandler<PageNoChangedEventArgs> PageNoChanged;
        public int PageNo
        {
            get { return (int)GetValue(PageNoProperty); }
            set { SetValue(PageNoProperty, value); }
        }
        public int TotalCount
        {
            get { return (int)GetValue(TotalCountProperty); }
            set { SetValue(TotalCountProperty, value); }
        }
        public int TotalPage
        {
            get { return (int)GetValue(TotalPageProperty); }
            set { SetValue(TotalPageProperty, value); }
        }
        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register(nameof(PageSize), typeof(int), typeof(BeatItemsListScrollBar), new PropertyMetadata(30, OnPageSizeChanged));
        public static readonly DependencyProperty TotalPageProperty =
            DependencyProperty.Register(nameof(TotalPage), typeof(int), typeof(BeatItemsListScrollBar));
        public static readonly DependencyProperty TotalCountProperty =
            DependencyProperty.Register(nameof(TotalCount), typeof(int), typeof(BeatItemsListScrollBar), new PropertyMetadata(0, OnTotalCountChanged));
        public static readonly DependencyProperty PageNoProperty =
            DependencyProperty.Register(nameof(PageNo), typeof(int), typeof(BeatItemsListScrollBar), new PropertyMetadata(1, OnPageNoChanged));

        private static void OnPageNoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as BeatItemsListScrollBar;
            control.OnPageNoChanged(new PageNoChangedEventArgs((int)e.NewValue, (int)e.OldValue));
        }

        private static void OnPageSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as BeatItemsListScrollBar;
            control.ResetTotalPage();
        }

        private static void OnTotalCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as BeatItemsListScrollBar;
            control.ResetTotalPage();
        }

        private void ResetTotalPage()
        {
            if(TotalCount <= 0)
            {
                TotalPage = 1;
            }
            else
            {
                TotalPage = TotalCount % PageSize == 0 ? TotalCount / PageSize : (TotalCount / PageSize) + 1;
            }
            if(PageNo > TotalPage)
            {
                PageNo = TotalPage;
            }
        }

        public BeatItemsListScrollBar()
        {
            InitializeComponent();
        }

        private void OnPageNoChanged(PageNoChangedEventArgs pageNoChangedEventArgs)
        {
            PageNoChanged?.Invoke(this, pageNoChangedEventArgs);
        }
    }
}
