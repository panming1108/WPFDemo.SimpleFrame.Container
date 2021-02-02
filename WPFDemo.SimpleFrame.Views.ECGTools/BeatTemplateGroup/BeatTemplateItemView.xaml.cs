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

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    /// <summary>
    /// BeatTemplateItemView.xaml 的交互逻辑
    /// </summary>
    public partial class BeatTemplateItemView : UserControl
    {
        public string Id { get; }
        private readonly BeatTemplateGroupItemView _groupItemView;
        public BeatTemplateGroupItemView GroupItemView => _groupItemView;
        public ICommand RenderSizeCommand { get; set; }
        public bool IsPrepareMerge
        {
            get { return (bool)GetValue(IsPrepareMergeProperty); }
            set { SetValue(IsPrepareMergeProperty, value); }
        }
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }
        public bool IsAdded
        {
            get { return (bool)GetValue(IsAddedProperty); }
            set { SetValue(IsAddedProperty, value); }
        }

        public static readonly DependencyProperty IsAddedProperty =
            DependencyProperty.Register(nameof(IsAdded), typeof(bool), typeof(BeatTemplateItemView));
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(BeatTemplateItemView));
        public static readonly DependencyProperty IsPrepareMergeProperty =
            DependencyProperty.Register(nameof(IsPrepareMerge), typeof(bool), typeof(BeatTemplateItemView));

        private readonly BrushConverter _brushConverter;
        private readonly Brush _hoverBorderBrush;
        private readonly Brush _selectedBorderBrush;
        private readonly Brush _commonBorderBrush;
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnSelectedChanged(value);
                }
            }
        }

        private void OnSelectedChanged(bool newSelectedValue)
        {
            if (newSelectedValue)
            {
                GroupItemView.GroupView.SelectedItemsCollection.TryAddItem(Id);
                PART_Border.BorderBrush = _selectedBorderBrush;
            }
            else
            {
                GroupItemView.GroupView.SelectedItemsCollection.TryRemoveItem(Id);
                PART_Border.BorderBrush = _commonBorderBrush;
            }
        }
        public BeatTemplateItemView(string id, BeatTemplateGroupItemView groupItemView)
        {
            Id = id;
            _groupItemView = groupItemView;
            _brushConverter = new BrushConverter();
            _hoverBorderBrush = (Brush)_brushConverter.ConvertFromString("#00AAFF");
            _commonBorderBrush = (Brush)_brushConverter.ConvertFromString("#AEBFCC");
            _selectedBorderBrush = (Brush)_brushConverter.ConvertFromString("#00AAFF");
            RenderSizeCommand = new RenderSizeCommand(OnRenderSizeChanged);
            InitializeComponent();
            PART_RenderLine.RenderingSizeCommand = RenderSizeCommand;
            PART_RenderLine.RenderingPens = BeatInfoSource.RenderPens;
            MouseEnter += BeatTemplateItemView_MouseEnter;
            MouseLeave += BeatTemplateItemView_MouseLeave;
            Unloaded += BeatTemplateItemView_Unloaded;
        }

        private void BeatTemplateItemView_Unloaded(object sender, RoutedEventArgs e)
        {
            MouseEnter -= BeatTemplateItemView_MouseEnter;
            MouseLeave -= BeatTemplateItemView_MouseLeave;
            Unloaded -= BeatTemplateItemView_Unloaded;
        }

        private void BeatTemplateItemView_MouseLeave(object sender, MouseEventArgs e)
        {
            IsPrepareMerge = false;
            GroupItemView.GroupView.SetCurrentMoveBeatTemplateItemView(null);
            if (IsSelected)
            {
                PART_Border.BorderBrush = _selectedBorderBrush;
            }
            else
            {
                PART_Border.BorderBrush = _commonBorderBrush;
            }
        }

        private void BeatTemplateItemView_MouseEnter(object sender, MouseEventArgs e)
        {
            PART_Border.BorderBrush = _hoverBorderBrush;
            GroupItemView.GroupView.SetCurrentMoveBeatTemplateItemView(this);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var data = (BeatTemplate)DataContext;
            PART_TypeName.Text = data.CategoryName;
            PART_Count.Text = data.DataCount.ToString();
            PART_Percent.Text = data.Percent.ToString("p");
            Height = GroupItemView.GroupView.ItemHeight;
            Width = GroupItemView.GroupView.ItemWidth;
        }

        private void OnRenderSizeChanged(PixelRect obj)
        {
            var data = (BeatTemplate)DataContext;
            //PART_RenderLine.ItemsSource = data.WaveList;
        }

        //protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        //{
        //    base.OnRenderSizeChanged(sizeInfo);
        //    var data = (BeatTemplate)DataContext;
        //    int width = (int)sizeInfo.NewSize.Width;
        //    int height = (int)sizeInfo.NewSize.Height - 24;
        //    WriteableBitmap bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Pbgra32, null);
        //    Int32Rect rect = new Int32Rect(0, 0, width, height);
        //    int[] pixels = new int[width * height * bitmap.Format.BitsPerPixel / 8];
        //    int color = ConvertColor(Colors.Black);
        //    foreach (var waveData in data.WaveList)
        //    {
        //        for (int i = 0; i < waveData.Count() - 1; i++)
        //        {
        //            var point1 = new Point(i, waveData[i]);
        //            var point2 = new Point(i + 1, waveData[i + 1]);
        //            DrawLine(pixels, width, height, (int)point1.X, (int)point1.Y, (int)point2.X, (int)point2.Y, color);
        //        }
        //    }
        //    int stride = bitmap.PixelWidth * bitmap.Format.BitsPerPixel / 8;
        //    bitmap.WritePixels(rect, pixels, stride, 0);
        //    PART_Image.Source = bitmap;
        //}

        //private int ConvertColor(Color color)
        //{
        //    int num = color.A + 1;
        //    return (color.A << 24) | ((byte)(color.R * num >> 8) << 16) | ((byte)(color.G * num >> 8) << 8) | (byte)(color.B * num >> 8);
        //}

        //private void DrawLine(int[] pixels, int pixelWidth, int pixelHeight, int x1, int y1, int x2, int y2, int color)
        //{
        //    int num = x2 - x1;
        //    int num2 = y2 - y1;
        //    int num3 = (num2 < 0) ? (-num2) : num2;
        //    int num4 = (num < 0) ? (-num) : num;
        //    if (num4 > num3)
        //    {
        //        if (num < 0)
        //        {
        //            int num5 = x1;
        //            x1 = x2;
        //            x2 = num5;
        //            int num6 = y1;
        //            y1 = y2;
        //            y2 = num6;
        //        }
        //        int num7 = (num2 << 8) / num;
        //        int num8 = y1 << 8;
        //        int num9 = y2 << 8;
        //        int num10 = pixelHeight << 8;
        //        if (y1 < y2)
        //        {
        //            if (y1 >= pixelHeight || y2 < 0)
        //            {
        //                return;
        //            }
        //            if (num8 < 0)
        //            {
        //                if (num7 == 0)
        //                {
        //                    return;
        //                }
        //                int num11 = num8;
        //                num8 = num7 - 1 + (num8 + 1) % num7;
        //                x1 += (num8 - num11) / num7;
        //            }
        //            if (num9 >= num10 && num7 != 0)
        //            {
        //                num9 = num10 - 1 - (num10 - 1 - num8) % num7;
        //                x2 = x1 + (num9 - num8) / num7;
        //            }
        //        }
        //        else
        //        {
        //            if (y2 >= pixelHeight || y1 < 0)
        //            {
        //                return;
        //            }
        //            if (num8 >= num10)
        //            {
        //                if (num7 == 0)
        //                {
        //                    return;
        //                }
        //                int num12 = num8;
        //                num8 = num10 - 1 + (num7 - (num10 - 1 - num12) % num7);
        //                x1 += (num8 - num12) / num7;
        //            }
        //            if (num9 < 0 && num7 != 0)
        //            {
        //                num9 = num8 % num7;
        //                x2 = x1 + (num9 - num8) / num7;
        //            }
        //        }
        //        if (x1 < 0)
        //        {
        //            num8 -= num7 * x1;
        //            x1 = 0;
        //        }
        //        if (x2 >= pixelWidth)
        //        {
        //            x2 = pixelWidth - 1;
        //        }
        //        int num13 = num8;
        //        int num14 = num13 >> 8;
        //        int num15 = num14;
        //        int num16 = x1 + num14 * pixelWidth;
        //        int num17 = (num7 < 0) ? (1 - pixelWidth) : (1 + pixelWidth);
        //        for (int i = x1; i <= x2; i++)
        //        {
        //            pixels[num16] = color;
        //            num13 += num7;
        //            num14 = num13 >> 8;
        //            if (num14 != num15)
        //            {
        //                num15 = num14;
        //                num16 += num17;
        //            }
        //            else
        //            {
        //                num16++;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (num3 == 0)
        //        {
        //            return;
        //        }
        //        if (num2 < 0)
        //        {
        //            int num18 = x1;
        //            x1 = x2;
        //            x2 = num18;
        //            int num19 = y1;
        //            y1 = y2;
        //            y2 = num19;
        //        }
        //        int num20 = x1 << 8;
        //        int num21 = x2 << 8;
        //        int num22 = pixelWidth << 8;
        //        int num23 = (num << 8) / num2;
        //        if (x1 < x2)
        //        {
        //            if (x1 >= pixelWidth || x2 < 0)
        //            {
        //                return;
        //            }
        //            if (num20 < 0)
        //            {
        //                if (num23 == 0)
        //                {
        //                    return;
        //                }
        //                int num24 = num20;
        //                num20 = num23 - 1 + (num20 + 1) % num23;
        //                y1 += (num20 - num24) / num23;
        //            }
        //            if (num21 >= num22 && num23 != 0)
        //            {
        //                num21 = num22 - 1 - (num22 - 1 - num20) % num23;
        //                y2 = y1 + (num21 - num20) / num23;
        //            }
        //        }
        //        else
        //        {
        //            if (x2 >= pixelWidth || x1 < 0)
        //            {
        //                return;
        //            }
        //            if (num20 >= num22)
        //            {
        //                if (num23 == 0)
        //                {
        //                    return;
        //                }
        //                int num25 = num20;
        //                num20 = num22 - 1 + (num23 - (num22 - 1 - num25) % num23);
        //                y1 += (num20 - num25) / num23;
        //            }
        //            if (num21 < 0 && num23 != 0)
        //            {
        //                num21 = num20 % num23;
        //                y2 = y1 + (num21 - num20) / num23;
        //            }
        //        }
        //        if (y1 < 0)
        //        {
        //            num20 -= num23 * y1;
        //            y1 = 0;
        //        }
        //        if (y2 >= pixelHeight)
        //        {
        //            y2 = pixelHeight - 1;
        //        }
        //        int num26 = num20 + (y1 * pixelWidth << 8);
        //        int num27 = (pixelWidth << 8) + num23;
        //        for (int j = y1; j <= y2; j++)
        //        {
        //            pixels[num26 >> 8] = color;
        //            num26 += num27;
        //        }
        //    }
        //}
    }
}
