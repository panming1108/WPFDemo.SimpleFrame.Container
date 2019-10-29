using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs.DataGrid
{
    public class NewDataGrid : System.Windows.Controls.DataGrid
    {
        public bool IsUseDataPager
        {
            get { return (bool)GetValue(IsUseDataPagerProperty); }
            set { SetValue(IsUseDataPagerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsUseDataPager.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsUseDataPagerProperty =
            DependencyProperty.Register("IsUseDataPager", typeof(bool), typeof(NewDataGrid), new PropertyMetadata(false));

        public int Total
        {
            get { return (int)GetValue(TotalProperty); }
            set { SetValue(TotalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Total.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalProperty =
            DependencyProperty.Register("Total", typeof(int), typeof(NewDataGrid));

        public Style PageButtonStyle
        {
            get { return (Style)GetValue(PageButtonStyleProperty); }
            set { SetValue(PageButtonStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageButtonStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageButtonStyleProperty =
            DependencyProperty.Register("PageButtonStyle", typeof(Style), typeof(NewDataGrid));

        public int PageNo
        {
            get { return (int)GetValue(PageNoProperty); }
            set { SetValue(PageNoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageNo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageNoProperty =
            DependencyProperty.Register("PageNo", typeof(int), typeof(NewDataGrid), new PropertyMetadata(1));

        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(NewDataGrid), new PropertyMetadata(10));

        public int NumericButtonCount
        {
            get { return (int)GetValue(NumericButtonCountProperty); }
            set { SetValue(NumericButtonCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NumericButtonCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumericButtonCountProperty =
            DependencyProperty.Register("NumericButtonCount", typeof(int), typeof(NewDataGrid), new PropertyMetadata(6));

        public IEnumerable<int> PageSizeSource
        {
            get { return (IEnumerable<int>)GetValue(PageSizeSourceProperty); }
            set { SetValue(PageSizeSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageSizeSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageSizeSourceProperty =
            DependencyProperty.Register("PageSizeSource", typeof(IEnumerable<int>), typeof(NewDataGrid), new PropertyMetadata(new int[3] { 10, 20, 50 }));

        public Visibility PageSizeComboBoxVisibity
        {
            get { return (Visibility)GetValue(PageSizeComboBoxVisibityProperty); }
            set { SetValue(PageSizeComboBoxVisibityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageSizeComboBoxVisibity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageSizeComboBoxVisibityProperty =
            DependencyProperty.Register("PageSizeComboBoxVisibity", typeof(Visibility), typeof(NewDataGrid));
    }
}
