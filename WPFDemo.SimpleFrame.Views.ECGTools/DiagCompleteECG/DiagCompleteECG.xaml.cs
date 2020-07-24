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
        private IMaskAction _currentAction;
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
            PART_Paint.DragAreaCollection.IsDisplay = true;
            PART_Paint.DragAreaCollection.IsReDraw = true;
            _dragArea.IsDisplay = true;
            _dragArea.Priority = 0;

            //_equiDistance.Priority = 1;
            _currentAction = _dragArea;
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
                PART_Paint.DragAreaCollection.DrawingCollection = _dragArea.DrawingArea(_originPoint, currentPoint, ActualHeight);
                PART_Paint.DrawingHandler();
            }
        }

        private void DiagCompleteECG_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _dispatcherTimer.Stop();
            var currentPoint = e.GetPosition(this);
            if (currentPoint == _originPoint)
            {
                PART_Paint.DragAreaCollection.DrawingCollection = _dragArea.DrawingSingleLine(currentPoint, ActualHeight);
                PART_Paint.EquiCollection.DrawingCollection = _equiDistance.DrawingAllLines(currentPoint, ActualHeight, ActualWidth);
                PART_Paint.DrawingHandler();
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
            PART_Paint.DragAreaCollection.IsReDraw = false;
            PART_Paint.EquiCollection.IsDisplay = true;
            PART_Paint.EquiCollection.IsReDraw = true;
            PART_Paint.DrawingHandler();
        }

        private void PART_Equi_Unchecked(object sender, RoutedEventArgs e)
        {
            PART_Paint.DragAreaCollection.IsReDraw = true;
            PART_Paint.EquiCollection.IsDisplay = false;
            PART_Paint.DrawingHandler();
        }

        private void PART_Box_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void PART_Box_Unchecked(object sender, RoutedEventArgs e)
        {
            
        }

        private bool CompareActionPriority(IMaskAction compareAction)
        {
            return compareAction.Priority >= _currentAction.Priority;
        }
    }
}
