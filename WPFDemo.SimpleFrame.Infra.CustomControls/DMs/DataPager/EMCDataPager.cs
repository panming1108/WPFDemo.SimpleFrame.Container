using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs.DataPager
{
    public class EMCDataPager : Control
    {
        /// <summary>
        /// 文本数据框的名称
        /// </summary>
        protected const string PARTNAME_TEXTBOX = "Part_TextBox";
        protected const string PART_ROOT = "PART_ROOT";
        protected TextBox _textBox;
        protected Border _border;

        private CommandBinding MoveToFirstPageCommand;
        private CommandBinding MoveToLastPageCommand;
        private CommandBinding MoveToNextPageCommand;
        private CommandBinding MoveToPageCommand;
        private CommandBinding MoveToPreviousPageCommand;

        private bool _isChangingPage;
        private bool _areHandlersSuspended;//是否处理程序被挂起

        public event EventHandler<PageNoChangingEventArgs> PageNoChanging;
        public event EventHandler<PageNoChangedEventArgs> PageNoChanged;
        public event EventHandler<PageSizeChangedEventArgs> PageSizeChanged;

        /// <summary>
        /// 触发页索引改变中事件
        /// </summary>
        protected virtual void OnPageNoChanging(PageNoChangingEventArgs args)
        {
            EventHandler<PageNoChangingEventArgs> pageNoChanging = PageNoChanging;
            if (pageNoChanging == null)
            {
                return;
            }
            pageNoChanging(this, args);
        }
        /// <summary>
        /// 触发页索引改变后事件
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnPageNoChanged(PageNoChangedEventArgs args)
        {
            EventHandler<PageNoChangedEventArgs> pageNoChanged = PageNoChanged;
            if (pageNoChanged == null)
            {
                return;
            }
            pageNoChanged(this, args);
        }
        private void OnPageNoChanged(int oldPageNo, int newPageNo)
        {
            PageNoChangedEventArgs args = new PageNoChangedEventArgs(oldPageNo, newPageNo);
            OnPageNoChanged(args);
        }

        protected virtual void OnPageSizeChanged(PageSizeChangedEventArgs args)
        {
            EventHandler<PageSizeChangedEventArgs> pageSizeChanged = PageSizeChanged;
            if(pageSizeChanged == null)
            {
                return;
            }
            pageSizeChanged(this, args);
        }

        private void OnPageSizeChanged(int oldPageSize, int newPageSize)
        {
            PageSizeChangedEventArgs args = new PageSizeChangedEventArgs(oldPageSize, newPageSize);
            OnPageSizeChanged(args);
        }


        #region Property
        /// <summary>
        /// 第一个省略号是否隐藏
        /// </summary>
        public Visibility FirstEllipsisVisibility
        {
            get { return (Visibility)GetValue(FirstEllipsisVisibilityProperty); }
            set { SetValue(FirstEllipsisVisibilityProperty, value); }
        }

        /// <summary>
        /// 最后一个省略号是否隐藏
        /// </summary>
        public Visibility LastEllipsisVisibility
        {
            get { return (Visibility)GetValue(LastEllipsisVisibilityProperty); }
            set { SetValue(LastEllipsisVisibilityProperty, value); }
        }

        /// <summary>
        /// 页码按钮样式
        /// </summary>
        public Style PageButtonStyle
        {
            get { return (Style)GetValue(PageButtonStyleProperty); }
            set { SetValue(PageButtonStyleProperty, value); }
        }

        /// <summary>
        /// 数据总条数
        /// </summary>
        public int ItemCount
        {
            get { return (int)GetValue(ItemCountProperty); }
            set { SetValue(ItemCountProperty, value); }
        }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageNo
        {
            get { return (int)GetValue(PageNoProperty); }
            set { SetValue(PageNoProperty, value); }
        }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        /// <summary>
        /// 页码总数
        /// </summary>
        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            set { SetValue(PageCountProperty, value); }
        }

        /// <summary>
        /// 页码按钮集合
        /// </summary>
        public IEnumerable<int> DisplayButtons
        {
            get { return (IEnumerable<int>)GetValue(DisplayButtonsProperty); }
            set { SetValue(DisplayButtonsProperty, value); }
        }

        /// <summary>
        /// 是否可以跳转到上一页
        /// </summary>
        public bool CanMoveToPrevious
        {
            get { return (bool)GetValue(CanMoveToPreviousProperty); }
            set { SetValue(CanMoveToPreviousProperty, value); }
        }

        /// <summary>
        /// 是否可以跳转到下一页
        /// </summary>
        public bool CanMoveToNext
        {
            get { return (bool)GetValue(CanMoveToNextProperty); }
            set { SetValue(CanMoveToNextProperty, value); }
        }

        /// <summary>
        /// 最大按钮数
        /// </summary>
        public int NumericButtonCount
        {
            get { return (int)GetValue(NumericButtonCountProperty); }
            set { SetValue(NumericButtonCountProperty, value); }
        }

        /// <summary>
        /// 是否隐藏跳转到最后一页
        /// </summary>
        public Visibility MoveToLastPageVisibility
        {
            get { return (Visibility)GetValue(MoveToLastPageVisibilityProperty); }
            set { SetValue(MoveToLastPageVisibilityProperty, value); }
        }



        public IEnumerable<int> PageSizeSource
        {
            get { return (IEnumerable<int>)GetValue(PageSizeSourceProperty); }
            set { SetValue(PageSizeSourceProperty, value); }
        }

        public Visibility PageSizeComboBoxVisibity
        {
            get { return (Visibility)GetValue(PageSizeComboBoxVisibityProperty); }
            set { SetValue(PageSizeComboBoxVisibityProperty, value); }
        }

        public bool CanMoveToFirstPage { get; private set; }
        public bool CanMoveToLastPage { get; private set; }
        public bool CanMoveToNextPage { get; private set; }
        public bool CanMoveToPreviousPage { get; private set; }
        #endregion

        #region DependencyProperty
        // Using a DependencyProperty as the backing store for PageSizeComboBoxVisibity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageSizeComboBoxVisibityProperty =
            DependencyProperty.Register("PageSizeComboBoxVisibity", typeof(Visibility), typeof(EMCDataPager));

        // Using a DependencyProperty as the backing store for PageSizeSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageSizeSourceProperty =
            DependencyProperty.Register("PageSizeSource", typeof(IEnumerable<int>), typeof(EMCDataPager), new PropertyMetadata(new int[3] { 10, 20, 50 }));

        // Using a DependencyProperty as the backing store for MoveToLastPageVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MoveToLastPageVisibilityProperty =
            DependencyProperty.Register("MoveToLastPageVisibility", typeof(Visibility), typeof(EMCDataPager));

        // Using a DependencyProperty as the backing store for NumericButtonCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumericButtonCountProperty =
            DependencyProperty.Register("NumericButtonCount", typeof(int), typeof(EMCDataPager), new PropertyMetadata(6));

        // Using a DependencyProperty as the backing store for CanMoveToNext.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanMoveToNextProperty =
            DependencyProperty.Register("CanMoveToNext", typeof(bool), typeof(EMCDataPager));

        // Using a DependencyProperty as the backing store for CanMoveToPrevious.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanMoveToPreviousProperty =
            DependencyProperty.Register("CanMoveToPrevious", typeof(bool), typeof(EMCDataPager));

        // Using a DependencyProperty as the backing store for DisplayButtons.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayButtonsProperty =
            DependencyProperty.Register("DisplayButtons", typeof(IEnumerable<int>), typeof(EMCDataPager));

        // Using a DependencyProperty as the backing store for PageCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageCountProperty =
            DependencyProperty.Register("PageCount", typeof(int), typeof(EMCDataPager));

        // Using a DependencyProperty as the backing store for PageSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize",
                typeof(int),
                typeof(EMCDataPager),
                new PropertyMetadata(10, new PropertyChangedCallback(OnPageSizePropertyChanged)));

        // Using a DependencyProperty as the backing store for PageNo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageNoProperty =
            DependencyProperty.Register("PageNo",
                typeof(int),
                typeof(EMCDataPager),
                new PropertyMetadata(1, new PropertyChangedCallback(OnPageNoPropertyChanged)), new ValidateValueCallback(IsValidDataPagerNo));

        // Using a DependencyProperty as the backing store for ItemCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemCountProperty =
            DependencyProperty.Register("ItemCount",
                typeof(int),
                typeof(EMCDataPager),
                new PropertyMetadata(-1, new PropertyChangedCallback(OnItemCountPropertyChanged)));

        // Using a DependencyProperty as the backing store for PageButtonStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageButtonStyleProperty =
            DependencyProperty.Register("PageButtonStyle", typeof(Style), typeof(EMCDataPager));

        // Using a DependencyProperty as the backing store for FirstEllipsisVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FirstEllipsisVisibilityProperty =
            DependencyProperty.Register("FirstEllipsisVisibility", typeof(Visibility), typeof(EMCDataPager));

        // Using a DependencyProperty as the backing store for LastEllipsisVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LastEllipsisVisibilityProperty =
            DependencyProperty.Register("LastEllipsisVisibility", typeof(Visibility), typeof(EMCDataPager));
        #endregion

        public EMCDataPager()
        {
            RegisterCommands();
        }

        /// <summary>
        /// 加载完模板
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (_textBox != null)
            {
                _textBox.KeyDown -= OnTextBoxKeyDown;
            }
            _textBox = GetTemplateChild(PARTNAME_TEXTBOX) as TextBox;
            if (_textBox != null)
            {
                _textBox.Text = string.Empty;
                _textBox.KeyDown += OnTextBoxKeyDown;
            }
            if (_border != null)
            {
                _border.Unloaded -= RootUnloaded;
            }
            _border = GetTemplateChild(PART_ROOT) as Border;
            if (_border != null)
            {
                _border.Unloaded += RootUnloaded;
            }
        }

        private void RootUnloaded(object sender, RoutedEventArgs e)
        {
            UnRegisterCommands();
        }

        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }
            TextBox textBox = (TextBox)sender;
            string text = textBox.Text;
            try
            {
                int pageNo;
                bool success = int.TryParse(text, out pageNo);
                if (success)
                {
                    if (pageNo > PageCount)
                    {
                        return;
                    }
                    PageNo = pageNo;
                }
                textBox.Text = PageNo.ToString();
            }
            catch (Exception)
            {
            }
        }

        #region Execute
        private bool MoveToFirstPage()
        {
            bool success = MoveToPage(1);
            if (success)
            {
                PageNo = 1;
            }
            return success;
        }

        private bool MoveToLastPage()
        {
            int last = PageCount;
            bool success = MoveToPage(last);
            if (success)
            {
                PageNo = last;
            }
            return success;
        }

        public bool MoveToNextPage()
        {
            int next = Math.Max(1, PageNo) + 1;
            bool success = MoveToPage(next);
            if (success)
            {
                PageNo = next;
            }
            return success;
        }

        private bool MoveToPage(int pageNo)
        {
            if(_isChangingPage)
            {
                throw new InvalidOperationException("Cannot change page during a page changing operation.");
            }
            if (pageNo > PageCount)
            {
                return false;
            }
            _isChangingPage = true;

            //页码改变中事件触发
            PageNoChangingEventArgs pageNoChangingEventArgs = new PageNoChangingEventArgs(PageNo, pageNo);
            OnPageNoChanging(pageNoChangingEventArgs);
            _isChangingPage = false;
            return !pageNoChangingEventArgs.Cancel;
        }

        private bool MoveToPreviousPage()
        {
            int previous = PageNo - 1;
            bool success = MoveToPage(previous);
            if (success)
            {
                PageNo = previous;
            }
            return success;
        }
        #endregion

        #region CommandExecute
        /// <summary>
        /// 跳转到上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMoveToPreviousPage(object sender, ExecutedRoutedEventArgs e)
        {
            EMCDataPager dataPager = sender as EMCDataPager;
            if (dataPager != null)
            {
                dataPager.MoveToPreviousPage();
            }
        }

        /// <summary>
        /// 跳转到选中页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMoveToPage(object sender, ExecutedRoutedEventArgs e)
        {
            EMCDataPager dataPager = sender as EMCDataPager;
            if (dataPager != null)
            {
                if (!(e.Parameter is int))
                {
                    throw new ArgumentException("The parameter of the MoveToPage command should be an integer.");
                }
                int pageNo = (int)e.Parameter;
                if (dataPager.MoveToPage(pageNo))
                {
                    dataPager.PageNo = pageNo;
                }
            }
        }

        /// <summary>
        /// 跳转到下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMoveToNextPage(object sender, ExecutedRoutedEventArgs e)
        {
            EMCDataPager dataPager = sender as EMCDataPager;
            if (dataPager != null)
            {
                dataPager.MoveToNextPage();
            }
        }

        /// <summary>
        /// 跳转到最后一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMoveToLastPage(object sender, ExecutedRoutedEventArgs e)
        {
            EMCDataPager dataPager = sender as EMCDataPager;
            if (dataPager != null)
            {
                dataPager.MoveToLastPage();
            }
        }

        /// <summary>
        /// 跳转到第一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMoveToFirstPage(object sender, ExecutedRoutedEventArgs e)
        {
            EMCDataPager dataPager = sender as EMCDataPager;
            if (dataPager != null)
            {
                dataPager.MoveToFirstPage();
            }
        }

        #endregion

        #region CanCommandExecute
        /// <summary>
        /// 是否可以跳转到上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanMoveToPreviousPageExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            EMCDataPager dataPager = sender as EMCDataPager;
            if (dataPager != null)
            {
                e.CanExecute = dataPager.CanMoveToPreviousPage;
            }
        }

        /// <summary>
        /// 是否可以跳转到选中页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanMoveToPageExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            EMCDataPager dataPager = sender as EMCDataPager;
            if (dataPager != null)
            {
                e.CanExecute = true;
            }
        }

        /// <summary>
        /// 是否可以跳转到下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanMoveToNextPageExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            EMCDataPager dataPager = sender as EMCDataPager;
            if (dataPager != null)
            {
                e.CanExecute = dataPager.CanMoveToNextPage;
            }
        }

        /// <summary>
        /// 是否可以跳转到最后一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanMoveToLastPageExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            EMCDataPager dataPager = sender as EMCDataPager;
            if (dataPager != null)
            {
                e.CanExecute = dataPager.CanMoveToLastPage = true;
            }
        }

        /// <summary>
        /// 是否可以跳转到第一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanMoveToFirstPageExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            EMCDataPager dataPager = sender as EMCDataPager;
            if (dataPager != null)
            {
                e.CanExecute = dataPager.CanMoveToFirstPage = true;
            }
        }
        #endregion

        private void ChangePageCount()
        {
            if (ItemCount == -1)
            {
                return;
            }
            PageCount = (ItemCount - 1) / Math.Max(1, PageSize) + 1;
            GenerateDisplayNumbers(PageNo);
            if (PageNo > PageCount)
            {
                PageNo = PageCount;
            }
            CanMoveToPreviousPage = PageNo > 1;
            CanMoveToNextPage = PageNo < PageCount;
        }

        private void GenerateDisplayNumbers(int pageNo)
        {
            int[] array;
            if (PageCount == 1)
            {
                FirstEllipsisVisibility = Visibility.Collapsed;
                LastEllipsisVisibility = Visibility.Collapsed;
                MoveToLastPageVisibility = Visibility.Collapsed;
                array = new int[0];
            }
            else if (PageCount <= NumericButtonCount)
            {
                FirstEllipsisVisibility = Visibility.Collapsed;
                LastEllipsisVisibility = Visibility.Collapsed;
                MoveToLastPageVisibility = Visibility.Visible;
                array = new int[PageCount - 2];
                for (int i = 0; i < PageCount - 2; i++)
                {
                    array[i] = i + 2;
                }
            }
            else
            {
                if (PageNo <= NumericButtonCount / 2)
                {
                    FirstEllipsisVisibility = Visibility.Collapsed;
                    LastEllipsisVisibility = Visibility.Visible;
                    MoveToLastPageVisibility = Visibility.Visible;
                    array = new int[NumericButtonCount - 3];
                    for (int i = 0; i < NumericButtonCount - 3; i++)
                    {
                        array[i] = i + 2;
                    }
                }
                else
                {
                    if (PageNo >= PageCount - (NumericButtonCount / 2))
                    {
                        FirstEllipsisVisibility = Visibility.Visible;
                        LastEllipsisVisibility = Visibility.Collapsed;
                        MoveToLastPageVisibility = Visibility.Visible;
                        array = new int[NumericButtonCount - 3];
                        for (int i = 0; i < NumericButtonCount - 3; i++)
                        {
                            array[i] = i + PageCount - NumericButtonCount + 3;
                        }
                    }
                    else
                    {
                        FirstEllipsisVisibility = Visibility.Visible;
                        LastEllipsisVisibility = Visibility.Visible;
                        MoveToLastPageVisibility = Visibility.Visible;
                        array = new int[NumericButtonCount - 4];
                        for (int i = 0; i < NumericButtonCount - 4; i++)
                        {
                            array[i] = i + PageNo + 1 - ((NumericButtonCount - 4) / 2);
                        }
                    }
                }
            }
            DisplayButtons = array;
        }

        private static void OnPageNoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EMCDataPager dataPager = (EMCDataPager)d;
            if(dataPager._areHandlersSuspended)
            {
                return;
            }
            int num = (int)e.OldValue;
            int num2 = (int)e.NewValue;
            dataPager.CheckPageNoChange(num, num2);
            if(!dataPager.MoveToPage(num2))
            {
                dataPager.SetValueNoCallback(PageNoProperty, num);
            }
            else
            {
                dataPager.GenerateDisplayNumbers(num2);
                dataPager.CanMoveToPreviousPage = num2 > 1;
                dataPager.CanMoveToNextPage = num2 < dataPager.PageCount;
                CommandManager.InvalidateRequerySuggested();
            }
            dataPager.OnPageNoChanged(num, num2);
            dataPager._textBox.Text = string.Empty;
        }

        private void CheckPageNoChange(int oldPageNo, int newPageNo)
        {
            if (PageSize == 0 && newPageNo != 1)
            {
                SetValueNoCallback(PageNoProperty, oldPageNo);
                throw new ArgumentOutOfRangeException("newPageNo", "PageNo can only be set to 1 when the PageSize is 0.");
            }
        }

        private void SetValueNoCallback(DependencyProperty property, int value)
        {
            _areHandlersSuspended = true;
            try
            {
                SetValue(property, value);
            }
            finally
            {
                _areHandlersSuspended = false;
            }
        }

        private static void OnPageSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EMCDataPager dataPager = (EMCDataPager)d;
            dataPager.ChangePageCount();
            if (dataPager._textBox == null)
            {
                return;
            }
            dataPager.OnPageSizeChanged((int)e.OldValue, (int)e.NewValue);
            dataPager._textBox.Text = string.Empty;
        }

        private static void OnItemCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EMCDataPager dataPager = (EMCDataPager)d;
            dataPager.ChangePageCount();
        }

        private static bool IsValidDataPagerNo(object value)
        {
            return (int)value >= 1;
        }


        #region RegisterCommand
        private void RegisterCommands()
        {
            RegisterCommand(MoveToFirstPageCommand, DataPagerCommands.MoveToFirstPage, OnMoveToFirstPage, CanMoveToFirstPageExecute);
            RegisterCommand(MoveToLastPageCommand, DataPagerCommands.MoveToLastPage, OnMoveToLastPage, CanMoveToLastPageExecute);
            RegisterCommand(MoveToNextPageCommand, DataPagerCommands.MoveToNextPage, OnMoveToNextPage, CanMoveToNextPageExecute);
            RegisterCommand(MoveToPageCommand, DataPagerCommands.MoveToPage, OnMoveToPage, CanMoveToPageExecute);
            RegisterCommand(MoveToPreviousPageCommand, DataPagerCommands.MoveToPreviousPage, OnMoveToPreviousPage, CanMoveToPreviousPageExecute);
        }

        private void UnRegisterCommands()
        {
            UnRegisterCommand(MoveToFirstPageCommand, OnMoveToFirstPage, CanMoveToFirstPageExecute);
            UnRegisterCommand(MoveToLastPageCommand, OnMoveToLastPage, CanMoveToLastPageExecute);
            UnRegisterCommand(MoveToNextPageCommand, OnMoveToNextPage, CanMoveToNextPageExecute);
            UnRegisterCommand(MoveToPageCommand, OnMoveToPage, CanMoveToPageExecute);
            UnRegisterCommand(MoveToPreviousPageCommand, OnMoveToPreviousPage, CanMoveToPreviousPageExecute);
        }

        private void RegisterCommand(CommandBinding bind, ICommand command, ExecutedRoutedEventHandler executed, CanExecuteRoutedEventHandler canExecute)
        {
            //CommandManager.RegisterClassCommandBinding(typeof(EMCDataPager), new CommandBinding(command, executed, canExecute));
            if (bind != null)
            {
                bind.Executed -= executed;
                bind.CanExecute -= canExecute;
                bind = null;
            }
            bind = new CommandBinding(command);
            bind.Executed += executed;
            bind.CanExecute += canExecute;
            this.CommandBindings.Add(bind);
        }

        private void UnRegisterCommand(CommandBinding bind, ExecutedRoutedEventHandler executed, CanExecuteRoutedEventHandler canExecute)
        {
            //CommandManager.RegisterClassCommandBinding(typeof(EMCDataPager), new CommandBinding(command, executed, canExecute));
            if (bind != null)
            {
                bind.Executed -= executed;
                bind.CanExecute -= canExecute;
                this.CommandBindings.Remove(bind);
                bind = null;
            }
        }
        #endregion
    }

    /// <summary>
    /// 页号改变中事件
    /// </summary>
    public class PageNoChangingEventArgs : CancelEventArgs
    {
        private int _oldPageIndex;
        private int _newPageIndex;

        /// <summary>
        /// 旧的页码
        /// </summary>
        public int OldPageIndex
        {
            get
            {
                return _oldPageIndex;
            }
        }
        /// <summary>
        /// 新的页码
        /// </summary>
        public int NewPageIndex
        {
            get
            {
                return _newPageIndex;
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public PageNoChangingEventArgs(int oldPageIndex, int newPageIndex)
        {
            _oldPageIndex = oldPageIndex;
            _newPageIndex = newPageIndex;
        }
    }

    /// <summary>
    /// 页码改变后事件
    /// </summary>
    public class PageNoChangedEventArgs : EventArgs
    {
        private readonly int _oldPageIndex;
        private readonly int _newPageIndex;
        /// <summary>
        /// 旧的页码
        /// </summary>
        public int OldPageIndex
        {
            get
            {
                return _oldPageIndex;
            }
        }
        /// <summary>
        /// 新的页码
        /// </summary>
        public int NewPageIndex
        {
            get
            {
                return _newPageIndex;
            }
        }
        /// <summary>
        /// 页码改变后事件
        /// </summary>
        public PageNoChangedEventArgs(int oldPageIndex, int newPageIndex)
        {
            _oldPageIndex = oldPageIndex;
            _newPageIndex = newPageIndex;
        }
    }

    /// <summary>
    /// 页大小改变后事件
    /// </summary>
    public class PageSizeChangedEventArgs : EventArgs
    {
        private readonly int _oldPageSize;
        private readonly int _newPageSize;
        /// <summary>
        /// 旧的页大小
        /// </summary>
        public int OldPageSize
        {
            get
            {
                return _oldPageSize;
            }
        }
        /// <summary>
        /// 新的页大小
        /// </summary>
        public int NewPageSize
        {
            get
            {
                return _newPageSize;
            }
        }
        /// <summary>
        /// 页码改变后事件
        /// </summary>
        public PageSizeChangedEventArgs(int oldPageSize, int newPageSize)
        {
            _oldPageSize = oldPageSize;
            _newPageSize = newPageSize;
        }
    }
}
