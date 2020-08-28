using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using WPFDemo.SimpleFrame.Infra.Messager;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    /// <summary>
    /// DiagCompleteECG.xaml 的交互逻辑
    /// </summary>
    public partial class DiagCompleteECG : UserControl
    {
        private Point _originPoint;
        private DispatcherTimer _dispatcherTimer;
        private readonly DragAreaAction _dragArea;
        private readonly EquiDistanceAction _equiDistance = new EquiDistanceAction(0, 30);
        private readonly BoxLineMeterAction _boxLineMeter = new BoxLineMeterAction(0, 30);
        private readonly BeatMarkAction _beatMark;
        private readonly AFAreaAction _aFArea = new AFAreaAction(0, 30);
        private bool _isMouseDown;
        private double _currentPosition;
        private readonly MaskActionCollection _maskList;
        public DiagCompleteECG()
        {
            InitializeComponent();

            _dispatcherTimer = new DispatcherTimer(DispatcherPriority.Send)
            {
                Interval = TimeSpan.FromMilliseconds(20)
            };
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            _dispatcherTimer.Start();

            Loaded += DiagCompleteECG_Loaded;
            Unloaded += DiagCompleteECG_Unloaded;
            MouseLeftButtonDown += DiagCompleteECG_MouseLeftButtonDown;
            MouseLeftButtonUp += DiagCompleteECG_MouseLeftButtonUp;
            MouseRightButtonDown += DiagCompleteECG_MouseRightButtonDown;
            MouseDoubleClick += DiagCompleteECG_MouseDoubleClick;
            MouseWheel += DiagCompleteECG_MouseWheel;
            KeyUp += DiagCompleteECG_KeyUp;

            RegisterMessages();

            _dragArea = new DragAreaAction(true, 0, 30)
            {
                DragPriority = 0,
                MouseUpPriority = 0,
            };
            _beatMark = new BeatMarkAction(false, 0, 0)
            {
                BeatInfos = BeatInfoCache.GetBeats()
            };

            _boxLineMeter.DragPriority = 1;
            _equiDistance.MouseUpPriority = 1;

            _maskList = new MaskActionCollection();
            _maskList.Add(_dragArea);
            _maskList.Add(_beatMark);
            _maskList.Add(_aFArea);
        }

        private void DiagCompleteECG_KeyUp(object sender, KeyEventArgs e)
        {
            Console.WriteLine("鼠标按下:" + e.Key);
        }

        private void RegisterMessages()
        {
            MessagerInstance.GetMessager().Register<Tuple<double, double>>(this, MaskMessageKeyEnum.DragAFMaskOver, OnDragAFMaskOver);
            MessagerInstance.GetMessager().Register<double>(this, MaskMessageKeyEnum.RenderAFMask, OnRenderAFMask);
            MessagerInstance.GetMessager().Register<double>(this, MaskMessageKeyEnum.ChangedMaskPosition, OnMaskPositionChanged);

            MessagerInstance.GetMessager().Register<string>(this, MaskMessageKeyEnum.StartDragArea, OnStartDragArea);
            MessagerInstance.GetMessager().Register<double>(this, MaskMessageKeyEnum.DragAreaMouseUp, OnDragAreaMouseUp);

            MessagerInstance.GetMessager().Register<double>(this, MaskMessageKeyEnum.SetStartFlag, OnSetStartFlag);
            MessagerInstance.GetMessager().Register<double>(this, MaskMessageKeyEnum.SetEndFlag, OnSetEndFlag);
            MessagerInstance.GetMessager().Register<string>(this, MaskMessageKeyEnum.ClearFlag, OnClearFlag);
        }

        private void UnRegisterMessages()
        {
            MessagerInstance.GetMessager().Unregister<Tuple<double, double>>(this, MaskMessageKeyEnum.DragAFMaskOver, OnDragAFMaskOver);
            MessagerInstance.GetMessager().Unregister<double>(this, MaskMessageKeyEnum.RenderAFMask, OnRenderAFMask);
            MessagerInstance.GetMessager().Unregister<double>(this, MaskMessageKeyEnum.ChangedMaskPosition, OnMaskPositionChanged);

            MessagerInstance.GetMessager().Unregister<string>(this, MaskMessageKeyEnum.StartDragArea, OnStartDragArea);
            MessagerInstance.GetMessager().Unregister<double>(this, MaskMessageKeyEnum.DragAreaMouseUp, OnDragAreaMouseUp);

            MessagerInstance.GetMessager().Unregister<double>(this, MaskMessageKeyEnum.SetStartFlag, OnSetStartFlag);
            MessagerInstance.GetMessager().Unregister<double>(this, MaskMessageKeyEnum.SetEndFlag, OnSetEndFlag);
            MessagerInstance.GetMessager().Unregister<string>(this, MaskMessageKeyEnum.ClearFlag, OnClearFlag);
        }

        private Task OnMaskPositionChanged(double position)
        {
            var offset = ActualWidth / 2 - position;
            var newPosition = _currentPosition - offset;
            if (newPosition < 0)
            {
                newPosition = 0;
            }
            var delta = _currentPosition - newPosition;
            _currentPosition = newPosition;
            if (delta != 0)
            {
                foreach (var item in _maskList.Masks)
                {
                    item.DrawingMouseWheel(delta);
                }
                RenderMaskPaint();
            }
            return TaskEx.FromResult(0);
        }

        private Task OnDragAFMaskOver(Tuple<double, double> afArea)
        {
            _aFArea.OnDragAFMaskOver(BeatMarkHelper.GetNearBeat(afArea.Item1 + _currentPosition) - _currentPosition, BeatMarkHelper.GetNearBeat(afArea.Item2 + _currentPosition) - _currentPosition);
            return TaskEx.FromResult(0);
        }

        private Task OnRenderAFMask(double beat)
        {
            var afArea = BeatMarkHelper.GetAfArea(beat);
            if (afArea.Item1 != afArea.Item2)
            {
                _aFArea.OnRenderAFMask(afArea.Item1 - _currentPosition, afArea.Item2 - _currentPosition);
                _dragArea.OnClearFlag();
            }
            RenderMaskPaint();
            return TaskEx.FromResult(0);
        }

        private Task OnDragAreaMouseUp(double resultX)
        {
            _beatMark.OnDragAreaMouseUp(resultX);
            RenderMaskPaint();
            return TaskEx.FromResult(0);
        }

        private Task OnStartDragArea(string arg)
        {
            _beatMark.OnStartDragArea();
            _aFArea.OnClearAfArea();
            RenderMaskPaint();
            return TaskEx.FromResult(0);
        }

        private Task OnSetStartFlag(double contextMenuX)
        {
            _dragArea.OnSetStartFlag(contextMenuX);
            RenderMaskPaint();
            return TaskEx.FromResult(0);
        }

        private Task OnSetEndFlag(double contextMenuX)
        {
            _dragArea.OnSetEndFlag(contextMenuX);
            RenderMaskPaint();
            return TaskEx.FromResult(0);
        }

        private Task OnClearFlag(string arg)
        {
            _dragArea.OnClearFlag();
            RenderMaskPaint();
            return TaskEx.FromResult(0);
        }

        private void DiagCompleteECG_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var newPosition = _currentPosition - e.Delta;
            if(newPosition < 0)
            {
                newPosition = 0;
            }
            var delta = _currentPosition - newPosition;
            _currentPosition = newPosition;
            if (delta != 0)
            {
                foreach (var item in _maskList.Masks)
                {
                    if(_isMouseDown && item == _maskList.MouseOverMask)
                    {
                        //鼠标按住后滚动
                        item.DrawingMouseDownWheel(delta, Mouse.GetPosition(this));
                    }
                    else
                    {
                        //全部滚动
                        item.DrawingMouseWheel(delta);
                    }
                }
                RenderMaskPaint();
            }
        }

        private void DiagCompleteECG_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _maskList.MouseOverMask?.DrawingMouseDoubleClick(e.GetPosition(this));
        }

        private void DiagCompleteECG_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var currentPoint = e.GetPosition(this);
            _maskList.MouseOverMask?.DrawingMouseRightButtonDown(currentPoint);
            var itemsSource = _maskList.MouseOverMask?.ContextMenuItems;
            if(itemsSource != null)
            {
                ContextMenu = new ContextMenu() { ItemsSource = itemsSource };
            }
            else
            {
                ContextMenu = null;
            }
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
                MouseMoveHandler(currentPoint);
            }
            else
            {
                MouseDragHandler(currentPoint);
            }
        }

        private void MouseDragHandler(Point currentPoint)
        {
            _maskList.DragMask?.DrawingDrag(currentPoint);
            RenderMaskPaint();
        }

        private void MouseMoveHandler(Point currentPoint)
        {
            _maskList.SetMouseOverMask(currentPoint);
            foreach (var item in _maskList.Masks)
            {
                item.DrawingMouseOver(currentPoint);
            }
            Cursor = _maskList.MouseOverMask?.Cursor;
            RenderMaskPaint();
        }

        private void DiagCompleteECG_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = false;
            ReleaseMouseCapture();
            var currentPoint = e.GetPosition(this);
            if (currentPoint == _originPoint)
            {
                MouseLeftButtonUpHandler(currentPoint);               
            }
            else
            {
                DragOverHandler(currentPoint);
            }
        }

        private void DragOverHandler(Point currentPoint)
        {
            _maskList.DragMask?.DrawingDragOver(currentPoint);
            RenderMaskPaint();
        }

        private void MouseLeftButtonUpHandler(Point currentPoint)
        {
            _maskList.MouseUpMask?.DrawingMouseUp(currentPoint);
            RenderMaskPaint();
        }

        private void DiagCompleteECG_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _originPoint = e.GetPosition(this);
            _isMouseDown = true;
            CaptureMouse();
            _maskList.SetScreenDragMask();
            _maskList.SetScreenMouseUpMask();
            _maskList.DragMask?.PrepareMask(_originPoint);
            _maskList.MouseUpMask?.PrepareMask(_originPoint);
        }

        private void DiagCompleteECG_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void DiagCompleteECG_Unloaded(object sender, RoutedEventArgs e)
        {
            UnRegisterMessages();
            Loaded -= DiagCompleteECG_Loaded;
            Unloaded -= DiagCompleteECG_Unloaded;
            MouseLeftButtonDown -= DiagCompleteECG_MouseLeftButtonDown;
            MouseLeftButtonUp -= DiagCompleteECG_MouseLeftButtonUp;
            MouseRightButtonDown -= DiagCompleteECG_MouseRightButtonDown;
            MouseDoubleClick -= DiagCompleteECG_MouseDoubleClick;
            MouseWheel -= DiagCompleteECG_MouseWheel;
            KeyUp -= DiagCompleteECG_KeyUp;
            _dispatcherTimer.Stop();
            _dispatcherTimer.IsEnabled = false;
            _dispatcherTimer = null;
            _maskList.Dispose();
        }        

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            _dragArea.RenderMaskSize(PART_ECG.ActualHeight, PART_ECG.ActualWidth);
            _equiDistance.RenderMaskSize(PART_ECG.ActualHeight, PART_ECG.ActualWidth);
            _boxLineMeter.RenderMaskSize(PART_ECG.ActualHeight, PART_ECG.ActualWidth);
            _beatMark.RenderMaskSize(ActualHeight, ActualWidth);
            _aFArea.RenderMaskSize(PART_ECG.ActualHeight, PART_ECG.ActualWidth);
            RenderMaskPaint();
        }

        private void RenderMaskPaint()
        {
            PART_Paint.DrawingHandler((drawingContext) => 
            {
                foreach (var item in _maskList.Masks)
                {
                    foreach (var drawing in item.DrawingChildren)
                    {
                        drawingContext.DrawDrawing(drawing);
                    }
                    foreach (var text in item.DrawingTexts)
                    {
                        drawingContext.DrawText(text.Text, text.Position);
                    }
                }
            });
        }

        #region 工具开关
        private void PART_Equi_Checked(object sender, RoutedEventArgs e)
        {
            _maskList.Add(_equiDistance);
            RenderMaskPaint();
        }

        private void PART_Equi_Unchecked(object sender, RoutedEventArgs e)
        {
            _maskList.Remove(_equiDistance);
            RenderMaskPaint();
        }

        private void PART_Box_Checked(object sender, RoutedEventArgs e)
        {
            _maskList.Add(_boxLineMeter);
            RenderMaskPaint();
        }

        private void PART_Box_Unchecked(object sender, RoutedEventArgs e)
        {
            _maskList.Remove(_boxLineMeter);
            RenderMaskPaint();
        }

        private void PART_Changed_Checked(object sender, RoutedEventArgs e)
        {
            _beatMark.RenderMaskSize(ActualHeight, ActualWidth / 2);
        }

        private void PART_Changed_Unchecked(object sender, RoutedEventArgs e)
        {
            _beatMark.RenderMaskSize(ActualHeight, ActualWidth);
        }
        #endregion
    }
}
