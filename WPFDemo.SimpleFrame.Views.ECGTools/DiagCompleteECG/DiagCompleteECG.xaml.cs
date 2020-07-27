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
        private Point _originPoint;
        private DispatcherTimer _dispatcherTimer;
        private readonly DragAreaAction _dragArea = new DragAreaAction();
        private readonly EquiDistanceAction _equiDistance = new EquiDistanceAction(20);
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
            MouseRightButtonDown += DiagCompleteECG_MouseRightButtonDown;

            _dragArea.IsDisplay = true;
            _dragArea.IsReDraw = true;
        }

        private void DiagCompleteECG_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContextMenu = _dragArea.RectHitTest(e.GetPosition(this));
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            var currentPoint = Mouse.GetPosition(this);
            if (currentPoint != _originPoint)
            {
                _dragArea.DrawingArea(_originPoint, currentPoint, ActualHeight);
                _equiDistance.DrawingMouseMoveAllLines(currentPoint, ActualHeight, ActualWidth);
                RenderMaskPaint();
            }
        }

        private void DiagCompleteECG_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _dispatcherTimer.Stop();
            var currentPoint = e.GetPosition(this);
            if (currentPoint == _originPoint)
            {
                _dragArea.DrawingSingleLine(currentPoint, ActualHeight);
                _equiDistance.DrawingMouseUpAllLines(currentPoint, ActualHeight, ActualWidth);
                RenderMaskPaint();
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
            MouseRightButtonDown -= DiagCompleteECG_MouseRightButtonDown;
            _dispatcherTimer.Stop();
            _dispatcherTimer.IsEnabled = false;
            _dispatcherTimer = null;
        }

        private void PART_Equi_Checked(object sender, RoutedEventArgs e)
        {
            _dragArea.IsReDraw = false;
            _equiDistance.IsDisplay = true;
            _equiDistance.IsReDraw = true;
            RenderMaskPaint();
        }

        private void PART_Equi_Unchecked(object sender, RoutedEventArgs e)
        {
            _dragArea.IsReDraw = true;
            _equiDistance.IsDisplay = false;
            RenderMaskPaint();
        }

        private void PART_Box_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void PART_Box_Unchecked(object sender, RoutedEventArgs e)
        {
            
        }

        private void RenderMaskPaint()
        {
            PART_Paint.DrawingHandler((drawingContext) => 
            {
                foreach (var item in _dragArea.DrawingChildren)
                {
                    drawingContext.DrawDrawing(item);
                }
                foreach (var item in _equiDistance.DrawingChildren)
                {
                    drawingContext.DrawDrawing(item);
                }
            });
        }
    }
}
