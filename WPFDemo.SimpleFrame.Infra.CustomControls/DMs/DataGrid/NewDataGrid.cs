using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFDemo.SimpleFrame.Infra.CustomControls.DMs.DataPager;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs.DataGrid
{
    public class NewDataGrid : System.Windows.Controls.DataGrid
    {
        private const string PART_DataPager = "PART_DataPager";
        private bool _isDisplayIndexColumn = false;
        private bool _isHandleCurrentPageNo = false;
        private bool _isHandleCurrentPageSize = false;
        private static int _currentPageNo = -1;
        private static int _currentPageSize = -1;

        private EMCDataPager _dataPager;

        // Using a DependencyProperty as the backing store for IsDisplayIndexColumn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDisplayIndexColumnProperty =
            DependencyProperty.Register("IsDisplayIndexColumn", typeof(bool), typeof(NewDataGrid), new PropertyMetadata(false));

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
            DependencyProperty.Register("PageNo", typeof(int), typeof(NewDataGrid), new PropertyMetadata(1, OnPageNoChanged));

        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(NewDataGrid), new PropertyMetadata(10, OnPageSizeChanged));

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

        public bool IsOpenCopyButton
        {
            get { return (bool)GetValue(IsOpenCopyButtonProperty); }
            set { SetValue(IsOpenCopyButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOpenCopyButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenCopyButtonProperty =
            DependencyProperty.Register("IsOpenCopyButton", typeof(bool), typeof(NewDataGrid), new PropertyMetadata(false));

        public Style CopyButtonStyle
        {
            get { return (Style)GetValue(CopyButtonStyleProperty); }
            set { SetValue(CopyButtonStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CopyButtonStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CopyButtonStyleProperty =
            DependencyProperty.Register("CopyButtonStyle", typeof(Style), typeof(NewDataGrid));

        public ICommand MouseDoubleClickCommand
        {
            get { return (ICommand)GetValue(MouseDoubleClickCommandProperty); }
            set { SetValue(MouseDoubleClickCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MouseDoubleClickCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseDoubleClickCommandProperty =
            DependencyProperty.Register("MouseDoubleClickCommand", typeof(ICommand), typeof(NewDataGrid));

        public DataGridLengthUnitType AutoGenerateColumnWidthType
        {
            get { return (DataGridLengthUnitType)GetValue(AutoGenerateColumnWidthTypeProperty); }
            set { SetValue(AutoGenerateColumnWidthTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AutoGenerateColumnWidthType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutoGenerateColumnWidthTypeProperty =
            DependencyProperty.Register("AutoGenerateColumnWidthType", typeof(DataGridLengthUnitType), typeof(NewDataGrid));

        public ContextMenu RowContextMenu
        {
            get { return (ContextMenu)GetValue(RowContextMenuProperty); }
            set { SetValue(RowContextMenuProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RowContentMenu.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowContextMenuProperty =
            DependencyProperty.Register("RowContextMenu", typeof(ContextMenu), typeof(NewDataGrid));

        public NewDataGrid()
        {

        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            if (oldValue != null)
            {
                if(!_isHandleCurrentPageSize)
                {
                    if (_currentPageSize != -1 && _currentPageSize != PageSize)
                    {
                        _isHandleCurrentPageNo = true;
                        PageSize = _currentPageSize;
                    }
                }
                if(!_isHandleCurrentPageNo)
                {
                    if (_currentPageNo != -1 && _currentPageNo != PageNo)
                    {
                        _isHandleCurrentPageSize = true;
                        PageNo = _currentPageNo;
                    }
                }
                _isHandleCurrentPageNo = false;
                _isHandleCurrentPageSize = false;
            }
        }

        //public override void OnApplyTemplate()
        //{
        //    base.OnApplyTemplate();
        //    if(_dataPager != null)
        //    {
        //        _dataPager.PageNoChanged -= DataPager_PageNoChanged;
        //        _dataPager.PageSizeChanged -= DataPager_PageSizeChanged;
        //    }
        //    _dataPager = GetTemplateChild(PART_DataPager) as EMCDataPager;
        //    if(_dataPager != null)
        //    {
        //        _dataPager.PageNoChanged += DataPager_PageNoChanged;
        //        _dataPager.PageSizeChanged += DataPager_PageSizeChanged;
        //    }
        //}

        private void DataPager_PageSizeChanged(object sender, PageSizeChangedEventArgs e)
        {
            PageSize = e.NewPageSize;
        }

        private void DataPager_PageNoChanged(object sender, PageNoChangedEventArgs e)
        {
            PageNo = e.NewPageIndex;
        }

        private static void OnPageNoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var dataGrid = d as NewDataGrid;
            //if(dataGrid != null)
            //{
            //    if(dataGrid._dataPager != null)
            //    {
            //        dataGrid._dataPager.PageNo = (int)e.NewValue;
            //    }
            //}
            _currentPageNo = (int)e.NewValue;
        }

        private static void OnPageSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var dataGrid = d as NewDataGrid;
            //if (dataGrid != null)
            //{
            //    if (dataGrid._dataPager != null)
            //    {
            //        dataGrid._dataPager.PageSize = (int)e.NewValue;
            //    }
            //}
            _currentPageSize = (int)e.NewValue;
        }

        protected override void OnLoadingRow(DataGridRowEventArgs e)
        {
            base.OnLoadingRow(e);
            InitIndexNum(e);
            e.Row.MouseDoubleClick -= RaiseMouseDoubleClick;
            e.Row.MouseDoubleClick += RaiseMouseDoubleClick;
            if (RowContextMenu != null)
            {
                e.Row.ContextMenu = RowContextMenu;
            }
        }

        private void RaiseMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = sender as DataGridRow;
            MouseDoubleClickCommand?.Execute(row.DataContext);
        }

        private void InitIndexNum(DataGridRowEventArgs e)
        {
            if (!_isDisplayIndexColumn)
            {
                foreach (var column in Columns)
                {
                    if (column is EMCDataGridIndexColumn)
                    {
                        _isDisplayIndexColumn = true;
                        break;
                    }
                }
            }
            if (_isDisplayIndexColumn)
            {
                if (IsUseDataPager)
                {
                    e.Row.Header = ((PageNo - 1) * PageSize) + e.Row.GetIndex() + 1;
                }
                else
                {
                    e.Row.Header = e.Row.GetIndex() + 1;
                }
            }
        }

        protected override void OnAutoGeneratingColumn(DataGridAutoGeneratingColumnEventArgs e)
        {
            if (AutoGenerateColumns)
            {
                e.Column.Width = new DataGridLength(1, AutoGenerateColumnWidthType);
            }
            base.OnAutoGeneratingColumn(e);
        }
    }
}
