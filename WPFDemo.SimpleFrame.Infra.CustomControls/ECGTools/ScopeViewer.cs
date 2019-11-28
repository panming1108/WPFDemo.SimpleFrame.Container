using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.ECGTools
{
    [TemplatePart(Name = "PART_Canvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_GridToMove", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_VisualBrush", Type = typeof(VisualBrush))]
    public class ScopeViewer : ContentControl
    {
        private Point m_lastPoint;

        private Canvas _canvas;
        private Grid _grid;
        private VisualBrush _visualBrush;

        #region DependencyProperty
        public Visibility ScopeVisibility
        {
            get { return (Visibility)GetValue(ScopeVisibilityProperty); }
            set { SetValue(ScopeVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScopeVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScopeVisibilityProperty =
            DependencyProperty.Register("ScopeVisibility", typeof(Visibility), typeof(ScopeViewer), new PropertyMetadata(Visibility.Collapsed));

        public double ScopeHeight
        {
            get { return (double)GetValue(ScopeHeightProperty); }
            set { SetValue(ScopeHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScopeHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScopeHeightProperty =
            DependencyProperty.Register("ScopeHeight", typeof(double), typeof(ScopeViewer));

        public double ScopeWidth
        {
            get { return (double)GetValue(ScopeWidthProperty); }
            set { SetValue(ScopeWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScopeWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScopeWidthProperty =
            DependencyProperty.Register("ScopeWidth", typeof(double), typeof(ScopeViewer));

        public Thickness ScopeMargin
        {
            get { return (Thickness)GetValue(ScopeMarginProperty); }
            set { SetValue(ScopeMarginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScopeMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScopeMarginProperty =
            DependencyProperty.Register("ScopeMargin", typeof(Thickness), typeof(ScopeViewer));

        public double ScopeRate
        {
            get { return (double)GetValue(ScopeRateProperty); }
            set { SetValue(ScopeRateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScopeRate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScopeRateProperty =
            DependencyProperty.Register("ScopeRate", typeof(double), typeof(ScopeViewer), new PropertyMetadata(2.0, ScopeRatePropertyChangedCallback));
        #endregion

        public ScopeViewer()
        {
            Unloaded += ScopeViewer_Unloaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _grid = GetTemplateChild("PART_GridToMove") as Grid;
            _visualBrush = GetTemplateChild("PART_VisualBrush") as VisualBrush;
            if (_canvas != null)
            {
                _canvas.MouseWheel -= Canvas_MouseWheel;
                _canvas.MouseMove -= Canvas_MouseMove;
                _canvas.MouseRightButtonUp -= Canvas_MouseRightButtonUp;
            }
            _canvas = GetTemplateChild("PART_Canvas") as Canvas;
            if (_canvas != null)
            {
                _canvas.MouseWheel += Canvas_MouseWheel;
                _canvas.MouseMove += Canvas_MouseMove;
                _canvas.MouseRightButtonUp += Canvas_MouseRightButtonUp;
            }
        }

        #region Canvas事件
        private void Canvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ScopeVisibility = Visibility.Collapsed;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_canvas != null)
            {
                m_lastPoint = e.GetPosition(_canvas);
                DoScope();
            }
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta == 0)
            {
                return;
            }
            bool bUp = e.Delta > 0;
            if (bUp)
            {
                double newScope = ScopeRate + 1.0;
                ScopeRate = Math.Min(newScope, 10.0);

            }
            else
            {
                double newScope = ScopeRate - 1.0;
                ScopeRate = Math.Max(newScope, 1.0);
            }
        }
        #endregion

        private static void ScopeRatePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ScopeViewer).UpdateRate();
        }

        private void UpdateRate()
        {
            DoScope();
        }

        private void DoScope()
        {
            if (_canvas == null || _grid == null || _visualBrush == null)
            {
                return;
            }
            double scopeWidth = ScopeWidth, scopeHeight = ScopeHeight;
            double borderWidth = ScopeMargin.Left;
            double maxX = _canvas.ActualWidth - scopeWidth - borderWidth * 2;
            double maxY = _canvas.ActualHeight - scopeHeight - borderWidth * 2;
            double toX = m_lastPoint.X - 0.5 * scopeWidth - borderWidth;
            double toY = m_lastPoint.Y - 0.5 * scopeHeight - borderWidth;
            if (toX < 0)
            {
                toX = 0;
            }
            if (toX > maxX)
            {
                toX = maxX;
            }
            if (toY < 0)
            {
                toY = 0;
            }
            if (toY > maxY)
            {
                toY = maxY;
            }
            Canvas.SetTop(_grid, toY);
            Canvas.SetLeft(_grid, toX);

            //计算viewbox区域
            //以鼠标为基准

            double scopeTimes = ScopeRate;
            double rate = 1.0 / scopeTimes;

            double vbWidth = scopeWidth * rate;
            double vbHeight = scopeHeight * rate;
            double vbCenterXMin = vbWidth * 0.5;
            double vbCenterYMin = vbHeight * 0.5;
            double vbCenterXMax = _canvas.ActualWidth - vbWidth * 0.5;
            double vbCenterYMax = _canvas.ActualHeight - vbHeight * 0.5;
            double vbCenterX = m_lastPoint.X;
            double vbCenterY = m_lastPoint.Y;
            if (vbCenterX < vbCenterXMin)
            {
                vbCenterX = vbCenterXMin;
            }
            if (vbCenterX > vbCenterXMax)
            {
                vbCenterX = vbCenterXMax;
            }

            if (vbCenterY < vbCenterYMin)
            {
                vbCenterY = vbCenterYMin;
            }
            if (vbCenterY > vbCenterYMax)
            {
                vbCenterY = vbCenterYMax;
            }

            double vbX = vbCenterX - vbWidth * 0.5;
            double vbY = vbCenterY - vbHeight * 0.5;
            //double vbY = toY + (1.0 - rate) * scope.Height * 0.5;

            Rect vbRect = new Rect(vbX, vbY, vbWidth, vbHeight);
            if (Content is Visual)
            {
                _visualBrush.Visual = Content as Visual;
                _visualBrush.Viewbox = vbRect;
            }
        }

        private void ScopeViewer_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_canvas != null)
            {
                _canvas.MouseWheel -= Canvas_MouseWheel;
                _canvas.MouseMove -= Canvas_MouseMove;
                _canvas.MouseRightButtonUp -= Canvas_MouseRightButtonUp;
            }
        }
    }
}
