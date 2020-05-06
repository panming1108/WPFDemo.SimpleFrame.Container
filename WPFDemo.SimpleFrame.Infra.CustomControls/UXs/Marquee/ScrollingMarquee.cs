using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.UXs.Marquee
{
    public class ScrollingMarquee : ContentControl
    {
        /// <summary>
        /// 文本数据框的名称
        /// </summary>
        protected const string PARTNAME_SCROLLITEMGrid1 = "Part_ScrollItemGrid1";
        protected const string PARTNAME_SCROLLITEMGrid2 = "Part_ScrollItemGrid2";
        protected const string PARTNAME_SCROLLITEMGrid3 = "Part_ScrollItemGrid3";
        protected const string PARTNAME_TEXTBOX1 = "Part_TextBoxt1";
        protected const string PARTNAME_TEXTBOX2 = "Part_TextBoxt2";
        protected const string PARTNAME_TEXTBOX3 = "Part_TextBoxt3";
        protected const string ROOT_BORDER = "Root_Border";
        protected const string PART_STACKPANEL = "Part_StackPanel";
        private TextBlock _textBox1;
        private TextBlock _textBox2;
        private TextBlock _textBox3;
        private Grid _scrollItemGrid1;
        private Grid _scrollItemGrid2;
        private Grid _scrollItemGrid3;
        private Border _border;
        private StackPanel _stackPanel;
        private DispatcherTimer _timer;
        private int _index;
        private Storyboard _storyboard;

        static ScrollingMarquee()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScrollingMarquee), new FrameworkPropertyMetadata(typeof(ScrollingMarquee)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (_border != null)
            {
                _border.Loaded -= OnLoaded;
                _border.MouseEnter -= Border_MouseEnter;
                _border.MouseLeave -= Border_MouseLeave;
                _border.Unloaded -= OnUnLoaded;
            }
            _scrollItemGrid1 = GetTemplateChild(PARTNAME_SCROLLITEMGrid1) as Grid;
            _scrollItemGrid2 = GetTemplateChild(PARTNAME_SCROLLITEMGrid2) as Grid;
            _scrollItemGrid3 = GetTemplateChild(PARTNAME_SCROLLITEMGrid3) as Grid;
            _textBox1 = GetTemplateChild(PARTNAME_TEXTBOX1) as TextBlock;
            _textBox2 = GetTemplateChild(PARTNAME_TEXTBOX2) as TextBlock;
            _textBox3 = GetTemplateChild(PARTNAME_TEXTBOX3) as TextBlock;
            _border = GetTemplateChild(ROOT_BORDER) as Border;
            _stackPanel = GetTemplateChild(PART_STACKPANEL) as StackPanel;
            if (_border != null)
            {
                _border.Loaded += OnLoaded;
                _border.MouseEnter += Border_MouseEnter;
                _border.MouseLeave += Border_MouseLeave;
                _border.Unloaded += OnUnLoaded;
            }
        }

        public int DisplayTimeSpan
        {
            get { return (int)GetValue(DisplayTimeSpanProperty); }
            set { SetValue(DisplayTimeSpanProperty, value); }
        }

        public static readonly DependencyProperty DisplayTimeSpanProperty =
            DependencyProperty.Register("DisplayTimeSpan", typeof(int), typeof(ScrollingMarquee), new PropertyMetadata(0));

        public List<string> ItemSource
        {
            get { return (List<string>)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemSourceProperty =
            DependencyProperty.Register("ItemSource", typeof(List<string>), typeof(ScrollingMarquee), new PropertyMetadata(null, OnItemSourceChanged));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ScrollingMarquee), new PropertyMetadata(Orientation.Horizontal));

        public bool CanPause
        {
            get { return (bool)GetValue(CanPauseProperty); }
            set { SetValue(CanPauseProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanPause.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanPauseProperty =
            DependencyProperty.Register("CanPause", typeof(bool), typeof(ScrollingMarquee));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ScrollingMarquee));

        private static void OnItemSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d != null)
            {
                var marquee = d as ScrollingMarquee;
                if (marquee._timer != null)
                {
                    marquee._timer.Stop();
                    marquee._timer = null;
                }
                marquee.OnApplyTemplate();
                marquee.Init();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private void Init()
        {
            if (_timer == null && _border != null)
            {
                if(Orientation == Orientation.Horizontal)
                {
                    _storyboard = (Storyboard)_border.FindResource("HorizontalStoryboard");
                }
                else
                {
                    _storyboard = (Storyboard)_border.FindResource("VerticalStoryboard");
                }
                if (ItemSource == null || ItemSource.Count == 0)
                {
                    return;
                }
                StartScrolling();
            }
        }

        private void StartScrolling()
        {
            //ItemSource.Reverse();

            _index = ItemSource.Count - 1;

            if(Orientation == Orientation.Horizontal)
            {
                _scrollItemGrid1.Width = ActualWidth;
                _scrollItemGrid2.Width = ActualWidth;
                _scrollItemGrid3.Width = ActualWidth;
                _stackPanel.Margin = new Thickness(ActualWidth * -1, 0, 0, 0);
            }
            else
            {
                _stackPanel.Margin = new Thickness(0, Height * -1, 0, 0);
                _stackPanel.RenderTransform = new TranslateTransform(0, Height);
            }
            ShowData();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(DisplayTimeSpan + 1);
            _timer.Tick += OnTimeChange;
            _timer.Start();
        }

        private void OnUnLoaded(object sender, RoutedEventArgs e)
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
            }
        }

        private void OnTimeChange(object sender, EventArgs e)
        {
            if (ItemSource.Count == 0)
            {
                return;
            }
            if (Orientation == Orientation.Vertical)
            {
                _stackPanel.RenderTransform = new TranslateTransform(0, 0);
            }
            else
            {
                _stackPanel.RenderTransform = new TranslateTransform(ActualWidth, 0);
            }

            _storyboard.Begin();

            _index--;
            if (_index < 0)
            {
                _index = ItemSource.Count - 1;
            }

            ShowData();
        }

        private void ShowData()
        {
            string data1 = GetData(_index, 0);
            string data2 = GetData(_index, 1);
            string data3 = GetData(_index, 2);

            if (data1 != null)
            {
                _textBox1.Text = data1;
            }

            if (data2 != null)
            {
                _textBox2.Text = data2;
            }

            if (data3 != null)
            {
                _textBox3.Text = data3;
            }
        }

        private string GetData(int index, int n)
        {
            if (ItemSource != null && ItemSource.Count > 0)
            {
                int i = index + n;
                if (i > ItemSource.Count - 1)
                {
                    i = i % ItemSource.Count;
                }
                return ItemSource[i];
            }
            return null;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            if(!CanPause)
            {
                return;
            }
            if (_storyboard != null && _timer != null)
            {
                _storyboard.Resume();
                _timer.Start();
            }
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!CanPause)
            {
                return;
            }
            if (_storyboard != null && _timer != null)
            {
                _storyboard.Pause();
                _timer.Stop();
            }
        }
    }
}
