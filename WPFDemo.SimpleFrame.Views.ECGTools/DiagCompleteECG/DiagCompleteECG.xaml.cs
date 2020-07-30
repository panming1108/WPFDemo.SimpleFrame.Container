﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly DragAreaAction _dragArea = new DragAreaAction(0, 30);
        private readonly EquiDistanceAction _equiDistance = new EquiDistanceAction(20, 0, 30);
        private readonly BoxLineMeterAction _boxLineMeter = new BoxLineMeterAction(0, 30);
        private readonly BeatMarkAction _beatMark = new BeatMarkAction(true, 0, 0);
        private readonly AFAreaAction _aFArea = new AFAreaAction(0, 30);
        private bool _isMouseDown;
        private readonly MaskActionCollection _maskList;
        private MaskActionBase _currentUsingMask;
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

            _maskList = new MaskActionCollection();
            _maskList.Add(_dragArea);
            _maskList.Add(_beatMark);
            _maskList.Add(_aFArea);
        }

        private void DiagCompleteECG_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(_maskList.Contains(_beatMark))
            {
                _beatMark.DrawingDoubleClick();
            }
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
                _currentUsingMask = _maskList.GetCurrentMask(currentPoint);
                Cursor = _currentUsingMask?.GetMouseOverCursor(currentPoint);
                if(_maskList.Contains(_beatMark))
                {
                    _beatMark.DrawingMouseMove(currentPoint);
                    RenderMaskPaint();
                }
                if(_maskList.Contains(_boxLineMeter))
                {
                    _boxLineMeter.DrawingMouseMove(currentPoint);
                    RenderMaskPaint();
                }
            }
            else
            {
                _currentUsingMask?.DrawingDrag(currentPoint);
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
                _currentUsingMask?.DrawingMouseUp(currentPoint);
                RenderMaskPaint();
            }
            else
            {
                if(_maskList.Contains(_aFArea))
                {
                    _aFArea.DrawingMouseUp(currentPoint);
                }
            }
        }

        private void DiagCompleteECG_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _originPoint = e.GetPosition(this);
            _isMouseDown = true;
            CaptureMouse();
            _currentUsingMask?.PrepareMask(_originPoint);
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
            _maskList.Dispose();
        }

        private void SetContextMenu(MouseButtonEventArgs e)
        {
            ContextMenu = _dragArea.GetDragContextMenu(e.GetPosition(this));
        }

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

        private void PART_Changed_Checked(object sender, RoutedEventArgs e)
        {
            _beatMark.RenderMaskSize(ActualHeight, ActualWidth / 2);
        }

        private void PART_Changed_Unchecked(object sender, RoutedEventArgs e)
        {
            _beatMark.RenderMaskSize(ActualHeight, ActualWidth);
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
    }
}
