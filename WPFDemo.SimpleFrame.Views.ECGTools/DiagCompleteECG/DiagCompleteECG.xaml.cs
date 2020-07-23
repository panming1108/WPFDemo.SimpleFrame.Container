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
using WPFDemo.SimpleFrame.Infra.CustomControls.ECGTools;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    /// <summary>
    /// DiagCompleteECG.xaml 的交互逻辑
    /// </summary>
    public partial class DiagCompleteECG : UserControl
    {
        private readonly DragArea _dragArea = new DragArea();
        private Point _originPoint;
        private DispatcherTimer _dispatcherTimer;
        private bool _isUseDragArea;
        public DiagCompleteECG()
        {
            InitializeComponent();

            _dispatcherTimer = new DispatcherTimer(DispatcherPriority.Send);
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(20);
            _dispatcherTimer.Tick += DispatcherTimer_Tick;

            Loaded += DiagCompleteECG_Loaded;
            Unloaded += DiagCompleteECG_Unloaded;
            MouseLeftButtonDown += DiagCompleteECG_MouseLeftButtonDown;
            MouseLeftButtonUp += DiagCompleteECG_MouseLeftButtonUp;

            _isUseDragArea = true;
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if(Mouse.GetPosition(this) != _originPoint)
            {
                if(_isUseDragArea)
                {
                    PART_Canvas.Children.Clear();
                    BrushConverter brushConverter = new BrushConverter();
                    Brush lineBrush = Brushes.Red;
                    Brush rectBrush = (Brush)brushConverter.ConvertFromString("#6021ADFF");
                    Point leftTopPoint = new Point(Math.Min(Mouse.GetPosition(this).X, _originPoint.X), 0);
                    Point rightBottomPoint = new Point(Math.Max(Mouse.GetPosition(this).X, _originPoint.X), ActualHeight);
                    PART_Canvas.Children.Add(_dragArea.RenderDragAreaRect(leftTopPoint, rightBottomPoint, rectBrush));
                    PART_Canvas.Children.Add(_dragArea.RenderLine(new Point(leftTopPoint.X, 0), new Point(leftTopPoint.X, ActualHeight), lineBrush, 1));
                    PART_Canvas.Children.Add(_dragArea.RenderLine(new Point(rightBottomPoint.X, 0), new Point(rightBottomPoint.X, ActualHeight), lineBrush, 1));
                }
            }
        }

        private void DiagCompleteECG_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _dispatcherTimer.Stop();
            if (Mouse.GetPosition(this) == _originPoint)
            {
                if(_isUseDragArea)
                {
                    PART_Canvas.Children.Clear();
                    PART_Canvas.Children.Add(_dragArea.RenderLine(new Point(_originPoint.X, 0), new Point(_originPoint.X, ActualHeight), Brushes.Orange, 1));
                }
            }
        }

        private void DiagCompleteECG_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _originPoint = e.GetPosition(this);
            _dispatcherTimer.Start();
        }

        private void DiagCompleteECG_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void DiagCompleteECG_Unloaded(object sender, RoutedEventArgs e)
        {
            Loaded -= DiagCompleteECG_Loaded;
            Unloaded -= DiagCompleteECG_Unloaded;
            MouseLeftButtonDown -= DiagCompleteECG_MouseLeftButtonDown;
            _dispatcherTimer.Stop();
            _dispatcherTimer.IsEnabled = false;
            _dispatcherTimer = null;
        }

        private void PART_Equi_Checked(object sender, RoutedEventArgs e)
        {
            _isUseDragArea = false;
            PART_Canvas.Children.Clear();
            EquiDistanceMeasure equiDistanceMeasure = new EquiDistanceMeasure()
            {
                Width = PART_Canvas.ActualWidth,
                Height = PART_Canvas.ActualHeight
            };
            Grid grid = new Grid();
            grid.Children.Add(equiDistanceMeasure);
            Canvas.SetLeft(grid, 0);
            Canvas.SetTop(grid, 0);
            PART_Canvas.Children.Add(grid);
        }

        private void PART_Equi_Unchecked(object sender, RoutedEventArgs e)
        {
            _isUseDragArea = true;
            PART_Canvas.Children.Clear();
        }

        private void PART_Box_Checked(object sender, RoutedEventArgs e)
        {
            _isUseDragArea = false;
            PART_Canvas.Children.Clear();
            BoxLineMeter boxLineMeter = new BoxLineMeter()
            {
                Width = PART_Canvas.ActualWidth,
                Height = PART_Canvas.ActualHeight
            };
            Grid grid = new Grid();
            grid.Children.Add(boxLineMeter);
            Canvas.SetLeft(grid, 0);
            Canvas.SetTop(grid, 0);
            PART_Canvas.Children.Add(grid);
        }

        private void PART_Box_Unchecked(object sender, RoutedEventArgs e)
        {
            _isUseDragArea = true;
            PART_Canvas.Children.Clear();
        }
    }
}
