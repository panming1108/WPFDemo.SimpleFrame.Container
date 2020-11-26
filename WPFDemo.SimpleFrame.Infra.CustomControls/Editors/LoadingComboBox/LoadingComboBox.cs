using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.Editors
{
    [TemplatePart(Name = "PART_ListBox", Type = typeof(ListBox))]
    [TemplatePart(Name = "PART_ScrollViewer", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "PART_SearchBtn", Type = typeof(Button))]
    [TemplatePart(Name = "PART_ClearBtn", Type = typeof(Button))]
    [TemplatePart(Name = "PART_SearchTextBox", Type = typeof(TextBox))]
    public class LoadingComboBox : ContentControl
    {
        #region Fields
        private ListBox PART_ListBox;
        private ScrollViewer PART_ScrollViewer;
        private Button PART_SearchBtn;
        private Button PART_ClearBtn;
        private TextBox PART_SearchTextBox;
        #endregion

        #region DependenceProperties
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        //加载数据命令，调接口
        public ICommand LoadDataCommand
        {
            get { return (ICommand)GetValue(LoadDataCommandProperty); }
            set { SetValue(LoadDataCommandProperty, value); }
        }
        //查询内容
        public string SearchContent
        {
            get { return (string)GetValue(SearchContentProperty); }
            set { SetValue(SearchContentProperty, value); }
        }
        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }
        //是否正在加载数据
        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }
        //查询命令
        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }
        //复选框下拉按钮样式
        public Style ToggleBtnStyle
        {
            get { return (Style)GetValue(ToggleBtnStyleProperty); }
            set { SetValue(ToggleBtnStyleProperty, value); }
        }
        //查询按钮样式
        public Style SearchBtnStyle
        {
            get { return (Style)GetValue(SearchBtnStyleProperty); }
            set { SetValue(SearchBtnStyleProperty, value); }
        }
        //下拉列表项样式
        public Style ListBoxItemStyle
        {
            get { return (Style)GetValue(ListBoxItemStyleProperty); }
            set { SetValue(ListBoxItemStyleProperty, value); }
        }
        //清空按钮样式
        public Style ClearBtnStyle
        {
            get { return (Style)GetValue(ClearBtnStyleProperty); }
            set { SetValue(ClearBtnStyleProperty, value); }
        }

        public static readonly DependencyProperty ClearBtnStyleProperty =
            DependencyProperty.Register(nameof(ClearBtnStyle), typeof(Style), typeof(LoadingComboBox));
        public static readonly DependencyProperty ListBoxItemStyleProperty =
            DependencyProperty.Register(nameof(ListBoxItemStyle), typeof(Style), typeof(LoadingComboBox));
        public static readonly DependencyProperty SearchBtnStyleProperty =
            DependencyProperty.Register(nameof(SearchBtnStyle), typeof(Style), typeof(LoadingComboBox));
        public static readonly DependencyProperty ToggleBtnStyleProperty =
            DependencyProperty.Register(nameof(ToggleBtnStyle), typeof(Style), typeof(LoadingComboBox));
        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register(nameof(SearchCommand), typeof(ICommand), typeof(LoadingComboBox));
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(LoadingComboBox));
        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register(nameof(IsDropDownOpen), typeof(bool), typeof(LoadingComboBox));
        public static readonly DependencyProperty SearchContentProperty =
            DependencyProperty.Register(nameof(SearchContent), typeof(string), typeof(LoadingComboBox));
        public static readonly DependencyProperty LoadDataCommandProperty =
            DependencyProperty.Register(nameof(LoadDataCommand), typeof(ICommand), typeof(LoadingComboBox));
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(LoadingComboBox));
        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(nameof(DisplayMemberPath), typeof(string), typeof(LoadingComboBox));
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(LoadingComboBox));
        #endregion

        public LoadingComboBox()
        {
            Unloaded += LoadingComboBox_Unloaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UnLoadControl();
            PART_ListBox = GetTemplateChild("PART_ListBox") as ListBox;            
            PART_ScrollViewer = GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
            PART_SearchBtn = GetTemplateChild("PART_SearchBtn") as Button;
            PART_SearchTextBox = GetTemplateChild("PART_SearchTextBox") as TextBox;
            PART_ClearBtn = GetTemplateChild("PART_ClearBtn") as Button;
            if (PART_ListBox != null)
            {
                PART_ListBox.SelectionChanged += PART_ListBox_SelectionChanged;
                PART_ListBox.PreviewMouseWheel += PART_ListBox_PreviewMouseWheel;
            }
            if (PART_ScrollViewer != null)
            {
                PART_ScrollViewer.ScrollChanged += PART_ScrollViewer_ScrollChanged;
            }
            if (PART_SearchBtn != null)
            {
                PART_SearchBtn.Click += PART_SearchBtn_Click;
            }
            if (PART_SearchTextBox != null)
            {
                PART_SearchTextBox.PreviewKeyDown += PART_SearchTextBox_PreviewKeyDown;
            }
            if (PART_ClearBtn != null)
            {
                PART_ClearBtn.Click += PART_ClearBtn_Click;
            }
        }

        /// <summary>
        /// 重置按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PART_ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchContent = string.Empty;
            ExecuteSearchCommand();
        }

        /// <summary>
        /// 查询文本框回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PART_SearchTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                ExecuteSearchCommand();
            }
        }

        #region 滚动条
        /// <summary>
        /// ListBox滚到事件由外部注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PART_ListBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var listBox = sender as ListBox;
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = MouseWheelEvent,
                Source = sender
            };
            listBox.RaiseEvent(eventArg);
        }

        /// <summary>
        /// 滚动条滚动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PART_ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            double bottomOffset = e.ExtentHeight - e.VerticalOffset - e.ViewportHeight;
            if (e.VerticalOffset > 0 && bottomOffset == 0)
            {
                LoadDataCommand?.Execute(SearchContent);
            }
        }
        #endregion

        /// <summary>
        /// 查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PART_SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSearchCommand();
        }

        /// <summary>
        /// 执行查询命令
        /// </summary>
        private void ExecuteSearchCommand()
        {
            PART_ScrollViewer.ScrollToTop();
            SearchCommand?.Execute(SearchContent);
        }

        /// <summary>
        /// ListBox选中改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PART_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if(e.AddedItems.Count > 0 && listBox.SelectedItem != null)
            {
                if(DisplayMemberPath != null)
                {
                    TextBlock textBlock = new TextBlock
                    {
                        DataContext = listBox.SelectedItem,
                        TextTrimming = TextTrimming.CharacterEllipsis,
                    };
                    textBlock.SetBinding(TextBlock.TextProperty, new Binding(DisplayMemberPath));
                    Binding toolTipBinding = new Binding
                    {
                        RelativeSource = new RelativeSource() { Mode = RelativeSourceMode.Self },
                        Path = new PropertyPath(nameof(textBlock.Text))
                    };
                    textBlock.SetBinding(ToolTipProperty, toolTipBinding);
                    Content = textBlock;
                }
                else
                {
                    Content = listBox.SelectedItem;
                }
                IsDropDownOpen = false;
            }
        }

        /// <summary>
        /// Unload控件
        /// </summary>
        private void UnLoadControl()
        {
            if (PART_ListBox != null)
            {
                PART_ListBox.SelectionChanged -= PART_ListBox_SelectionChanged;
                PART_ListBox.PreviewMouseWheel -= PART_ListBox_PreviewMouseWheel;
            }
            if (PART_ScrollViewer != null)
            {
                PART_ScrollViewer.ScrollChanged -= PART_ScrollViewer_ScrollChanged;
            }
            if (PART_SearchBtn != null)
            {
                PART_SearchBtn.Click -= PART_SearchBtn_Click;
            }
            if (PART_SearchTextBox != null)
            {
                PART_SearchTextBox.PreviewKeyDown -= PART_SearchTextBox_PreviewKeyDown;
            }
            if (PART_ClearBtn != null)
            {
                PART_ClearBtn.Click -= PART_ClearBtn_Click;
            }
        }

        /// <summary>
        /// Unloaded事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadingComboBox_Unloaded(object sender, RoutedEventArgs e)
        {
            UnLoadControl();
            Unloaded -= LoadingComboBox_Unloaded;
        }
    }
}
