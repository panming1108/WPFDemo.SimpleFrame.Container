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
    /// BeatItemView.xaml 的交互逻辑
    /// </summary>
    public partial class BeatItemView : UserControl, ISelectItem
    {
        private readonly BrushConverter _brushConverter;
        private readonly Brush _hoverBorderBrush;
        private readonly Brush _selectedBorderBrush;
        private readonly Brush _commonBorderBrush;
        private readonly Pen _pen;
        private bool _isSelected;
        private readonly ISelectItemsContainer _container;
        public ISelectItemsContainer Container => _container;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if(_isSelected != value)
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
                Container.SelectedItemsCollection.TryAddItem(this);
                PART_Border.BorderBrush = _selectedBorderBrush;
            }
            else
            {
                Container.SelectedItemsCollection.TryRemoveItem(this);
                PART_Border.BorderBrush = _commonBorderBrush;
            }
        }

        public BeatItemView(ISelectItemsContainer itemsContainer)
        {
            _container = itemsContainer;
            _brushConverter = new BrushConverter();
            _hoverBorderBrush = (Brush)_brushConverter.ConvertFromString("#00AAFF");
            _commonBorderBrush = (Brush)_brushConverter.ConvertFromString("#DBE0E3");
            _selectedBorderBrush = (Brush)_brushConverter.ConvertFromString("#00AAFF");
            _pen = new Pen(Brushes.Black, 1);
            InitializeComponent();
            MouseEnter += BeatItemView_MouseEnter;
            MouseLeave += BeatItemView_MouseLeave;
            Unloaded += BeatItemView_Unloaded;
        }

        private void BeatItemView_Unloaded(object sender, RoutedEventArgs e)
        {
            MouseEnter -= BeatItemView_MouseEnter;
            MouseLeave -= BeatItemView_MouseLeave;
            Unloaded -= BeatItemView_Unloaded;
        }

        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeatItemView_MouseLeave(object sender, MouseEventArgs e)
        {
            if(IsSelected)
            {
                PART_Border.BorderBrush = _selectedBorderBrush;
            }
            else
            {
                PART_Border.BorderBrush = _commonBorderBrush;
            }
        }

        /// <summary>
        /// 鼠标悬浮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeatItemView_MouseEnter(object sender, MouseEventArgs e)
        {
            PART_Border.BorderBrush = _hoverBorderBrush;            
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var beatInfo = (BeatInfo)DataContext;
            PART_BeatType.Text = beatInfo.BeatType;
            PART_Order.Text = beatInfo.Interval.ToString();
            Width = _container.ItemWidth;
            Height = _container.ItemHeight;
            DrawECG(beatInfo);
        }

        private void DrawECG(BeatInfo beatInfo)
        {
            PART_Canvas.DrawingHandler((d) => 
            {
                for (int i = 0; i < beatInfo.Data.Count() - 1; i++)
                {
                    var point1 = new Point(i, beatInfo.Data[i]);
                    var point2 = new Point(i + 1, beatInfo.Data[i + 1]);
                    d.DrawLine(_pen, point1, point2);
                }
            });
        }
    }
}
