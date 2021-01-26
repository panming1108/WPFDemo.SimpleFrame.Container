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
using System.Windows.Threading;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    /// <summary>
    /// BeatTemplateGroupView.xaml 的交互逻辑
    /// </summary>
    public partial class BeatTemplateGroupView : UserControl
    {
        private Random _random = new Random();
        private bool _isMouseDown;
        private Point _mouseDownPoint;
        private readonly DispatcherTimer _dispatcherTimer;
        private BeatTemplateItemView _startItemView;
        private BeatTemplateItemView _currentMoveItemView;
        private BeatTemplateGroupItemView _endBeatTemplateGroupItemView;

        public bool IsEditMode
        {
            get { return (bool)GetValue(IsEditModeProperty); }
            set { SetValue(IsEditModeProperty, value); }
        }

        public static readonly DependencyProperty IsEditModeProperty =
            DependencyProperty.Register(nameof(IsEditMode), typeof(bool), typeof(BeatTemplateGroupView));

        public BeatTemplateGroupView()
        {
            _dispatcherTimer = new DispatcherTimer(DispatcherPriority.Send)
            {
                Interval = TimeSpan.FromMilliseconds(20)
            };
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            _dispatcherTimer.Start();
            InitializeComponent();
            Loaded += BeatTemplateGroupView_Loaded;
            Unloaded += BeatTemplateGroupView_Unloaded;
            MouseLeftButtonDown += BeatTemplateGroupView_MouseLeftButtonDown;
            MouseLeftButtonUp += BeatTemplateGroupView_MouseLeftButtonUp;
            MouseRightButtonUp += BeatTemplateGroupView_MouseRightButtonUp;
            KeyDown += BeatTemplateGroupView_KeyDown;
        }

        private void BeatTemplateGroupView_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateData();
        }

        private void BeatTemplateGroupView_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void BeatTemplateGroupView_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void BeatTemplateGroupView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = true;
            var currentPoint = Mouse.GetPosition(this);
            _mouseDownPoint = currentPoint;
            _startItemView = null;
            foreach (var item in PART_GroupItemsControl.Items)
            {
                var groupItemView = item as BeatTemplateGroupItemView;
                var result = groupItemView.IsBeatTemplateItemView(currentPoint, out _startItemView);
                if(result)
                {
                    break;
                }
            }

            if(_startItemView != null)
            {
                //拖动模板
                IsEditMode = true;
            }
            else
            {
                //框选
            }
        }

        private DragDropAdorner _tempAdorner;
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            var currentPoint = Mouse.GetPosition(this);
            if (_mouseDownPoint == currentPoint)
            {
                //鼠标没有移动
                return;
            }
            //鼠标移动了
            if(!_isMouseDown)
            {
                return;
            }
            //鼠标移动且左键按下 拖动
            if (IsEditMode)
            {
                var mAdornerLayer = AdornerLayer.GetAdornerLayer(this);
                if(_tempAdorner != null)
                {
                    mAdornerLayer.Remove(_tempAdorner);
                }
                _tempAdorner = new DragDropAdorner(_startItemView);
                mAdornerLayer.Add(_tempAdorner);

                if (_currentMoveItemView != null)
                {
                    _currentMoveItemView.IsPrepareMerge = true;
                }
                else
                {
                    foreach (var item in PART_GroupItemsControl.Items)
                    {
                        var groupItemView = item as BeatTemplateGroupItemView;
                        var result = groupItemView.IsBeatTemplateGroupItemHeader(currentPoint, out _endBeatTemplateGroupItemView);
                        if (result)
                        {
                            break;
                        }
                    }
                }
            }
            else
            {

            }
        }

        private void BeatTemplateGroupView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsEditMode = false;
            var mAdornerLayer = AdornerLayer.GetAdornerLayer(this);
            if (_tempAdorner != null)
            {
                mAdornerLayer.Remove(_tempAdorner);
                _tempAdorner = null;
            }
            if (_currentMoveItemView != null)
            {
                MergeBeatTemplate(_currentMoveItemView, _startItemView);
            }
            else
            {
                if (_endBeatTemplateGroupItemView == null)
                {
                    return;
                }
                AddCategory(_endBeatTemplateGroupItemView, _startItemView);
                _endBeatTemplateGroupItemView = null;
            }
        }

        private void MergeBeatTemplate(BeatTemplateItemView currentMoveItemView, BeatTemplateItemView startItemView)
        {
            Console.WriteLine("合并");
        }

        private void AddCategory(BeatTemplateGroupItemView endBeatTemplateGroupItemView, BeatTemplateItemView startItemView)
        {
            Console.WriteLine("新增分类");
        }

        public void GenerateData()
        {
            var groupCount = _random.Next(5, 8);
            var itemCount = _random.Next(5, 8);
            PART_GroupItemsControl.Items.Clear();
            for (int i = 0; i < groupCount; i++)
            {
                BeatTemplateGroupItemView groupItemView = new BeatTemplateGroupItemView(this);
                var source = new List<BeatTemplate>();
                for (int j = 0; j < itemCount; j++)
                {
                    BeatTemplate beatTemplate = new BeatTemplate() { CategoryName = ((BeatTypeEnum)(j % 5)).ToString() };
                    source.Add(beatTemplate);
                }
                groupItemView.SetGroupItemItemsSource(source);
                PART_GroupItemsControl.Items.Add(groupItemView);
            }
        }

        internal void SetCurrentMoveBeatTemplateItemView(BeatTemplateItemView itemView)
        {
            _currentMoveItemView = itemView;
        }

        private void BeatTemplateGroupView_Unloaded(object sender, RoutedEventArgs e)
        {
            _dispatcherTimer.Tick -= DispatcherTimer_Tick;
            Loaded -= BeatTemplateGroupView_Loaded;
            Unloaded -= BeatTemplateGroupView_Unloaded;
            MouseLeftButtonDown -= BeatTemplateGroupView_MouseLeftButtonDown;
            MouseLeftButtonUp -= BeatTemplateGroupView_MouseLeftButtonUp;
            MouseRightButtonUp -= BeatTemplateGroupView_MouseRightButtonUp;
            KeyDown -= BeatTemplateGroupView_KeyDown;
        }
    }
}
