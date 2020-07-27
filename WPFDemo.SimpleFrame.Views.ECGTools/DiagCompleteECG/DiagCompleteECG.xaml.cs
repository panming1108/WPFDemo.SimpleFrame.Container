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
        private bool _isMouseDown;
        private MaskActionCollection _maskList;
        private MaskActionBase _currentUsingMask;
        public DiagCompleteECG()
        {
            InitializeComponent();

            _dispatcherTimer = new DispatcherTimer(DispatcherPriority.Send);
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(20);
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            _dispatcherTimer.Start();

            Loaded += DiagCompleteECG_Loaded;
            Unloaded += DiagCompleteECG_Unloaded;
            MouseLeftButtonDown += DiagCompleteECG_MouseLeftButtonDown;
            MouseLeftButtonUp += DiagCompleteECG_MouseLeftButtonUp;
            MouseRightButtonDown += DiagCompleteECG_MouseRightButtonDown;

            _maskList = new MaskActionCollection(this)
            {
                _dragArea
            };
        }

        private void DiagCompleteECG_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetContextMenu(e);           
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            var currentPoint = Mouse.GetPosition(this);
            //鼠标是否移动了
            if(currentPoint == _originPoint)
            {
                return;
            }
            if (!_isMouseDown)
            {
                SetCursor(currentPoint);               
            }
            else
            {
                if (_currentUsingMask is DragAreaAction)
                {
                    _dragArea.DrawingArea(currentPoint);
                }
                if (_currentUsingMask is EquiDistanceAction)
                {
                    _equiDistance.DrawingMouseMoveAllLines(currentPoint);
                }
                RenderMaskPaint();
            }
        }

        private void DiagCompleteECG_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = false;
            ReleaseMouseCapture();
            var currentPoint = e.GetPosition(this);
            if (currentPoint == _originPoint)
            {               
                if (_currentUsingMask is DragAreaAction)
                {
                    _dragArea.DrawingSingleLine(currentPoint);
                }
                RenderMaskPaint();              
            }
        }

        private void DiagCompleteECG_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _originPoint = e.GetPosition(this);
            _isMouseDown = true;
            CaptureMouse();
            _currentUsingMask = _maskList.GetCurrentMask(_originPoint);
            _currentUsingMask.PrepareMask(_originPoint, ActualHeight, ActualWidth);
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

        private void SetCursor(Point point)
        {
            if(_maskList.Contains(_equiDistance))
            {
                Cursor = _equiDistance.GetMouseOverCursor(point);
            }
            else
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void SetContextMenu(MouseButtonEventArgs e)
        {
            ContextMenu = _dragArea.GetDragContextMenu(e.GetPosition(this));
        }

        private void PART_Equi_Checked(object sender, RoutedEventArgs e)
        {
            _maskList.Add(_equiDistance);
            _equiDistance.PrepareMask(new Point(ActualWidth / 2, 0), ActualHeight, ActualWidth);
            _equiDistance.DrawingMouseUpAllLines(new Point(ActualWidth / 2, 0));
            RenderMaskPaint();
        }

        private void PART_Equi_Unchecked(object sender, RoutedEventArgs e)
        {
            _maskList.Remove(_equiDistance);
            _equiDistance.ResetMask();
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
                foreach (var item in _maskList)
                {
                    foreach (var drawing in item.DrawingChildren)
                    {
                        drawingContext.DrawDrawing(drawing);
                    }
                }
            });
        }
    }
}
