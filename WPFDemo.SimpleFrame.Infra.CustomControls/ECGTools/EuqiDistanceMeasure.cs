using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WPFDemo.SimpleFrame.Infra.Helper;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.ECGTools
{
    public class EquiDistanceMeasure : ContentControl
    {
        protected const string PART_Canvas = "EquiDistanceMeasureCanvas";
        protected Canvas _canvas;

        private bool _isCompassesOnMove;//防止别的crtl会触发这里的移动事件，添加一个属性去判断，是否时开启等距后的移动
        private bool _isCompassesOn;//判断等距是否开启
        private double _lastValue;
        private double _firstPotint;

        public Brush FirstLineBrush
        {
            get { return (Brush)GetValue(FirstLineBrushProperty); }
            set { SetValue(FirstLineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FirstLineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FirstLineBrushProperty =
            DependencyProperty.Register("FirstLineBrush", typeof(Brush), typeof(EquiDistanceMeasure));

        public Brush OtherLineBrush
        {
            get { return (Brush)GetValue(OtherLineBrushProperty); }
            set { SetValue(OtherLineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OtherLineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OtherLineBrushProperty =
            DependencyProperty.Register("OtherLineBrush", typeof(Brush), typeof(EquiDistanceMeasure));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UnLoadedCanvas();
            _canvas = GetTemplateChild(PART_Canvas) as Canvas;
            if (_canvas != null)
            {
                _canvas.MouseMove += EquiDistanceMeasureCanvas_OnMouseMove;
                _canvas.MouseLeftButtonDown += EquiDistanceMeasureCanvas_OnMouseLeftButtonDown;
            }
        }

        public EquiDistanceMeasure()
        {
            //TODO 注册消息
            this.Unloaded += EquiDistanceMeasure_Unloaded;
            _isCompassesOn = true;
        }

        #region Canvas事件
        private void EquiDistanceMeasureCanvas_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_isCompassesOn)
            {
                _isCompassesOnMove = true;//在鼠标当前位置画一个线
                _firstPotint = e.GetPosition((FrameworkElement)sender).X;
                _canvas.Children.Clear();
                DrawLine(_firstPotint, FirstLineBrush);
                e.Handled = true;//截断下方的鼠标事件的触发
            }
        }

        private void EquiDistanceMeasureCanvas_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _isCompassesOnMove)
            {
                var currentp = e.GetPosition((FrameworkElement)sender).X;//计算鼠标的移动距离
                _lastValue = Math.Abs(currentp - _firstPotint);
                if (_lastValue < 20)
                {
                    return;//如果移动的距离太短就不画了
                }
                ReLoadEquiDistanceMeasure();
            }
            if (_canvas != null && _canvas.Children.Count == 1)
            {
                _canvas.Children.Clear();//如果当前只剩下一条线，没有进行其他操作。就把它移除
            }
        }
        #endregion

        #region 提取的公共方法
        /// <summary>
        /// 重绘并行分规
        /// </summary>
        private void ReLoadEquiDistanceMeasure()
        {
            _canvas.Children.Clear();//每次重画都是清空后重新找到开始点
            _canvas.Children.Add(new Line()
            {
                X1 = _firstPotint,
                Y1 = 0,
                X2 = _firstPotint,
                Y2 = _canvas.ActualHeight,
                Stroke = FirstLineBrush,
                StrokeThickness = 2
            });
            //往前画
            for (double i = _firstPotint - _lastValue; i > 0; i = i - _lastValue)
            {
                DrawLine(i, OtherLineBrush);
            }
            //往后画
            for (double i = _firstPotint + _lastValue; i < _canvas.ActualWidth; i = i + _lastValue)
            {
                if (i > 0)
                {
                    DrawLine(i, OtherLineBrush);
                }
            }
        }

        /// <summary>
        /// 画线
        /// </summary>
        /// <param name="firstPotint"></param>
        private void DrawLine(double spot, Brush brush)
        {
            _canvas.Children.Add(new Line()
            {
                X1 = spot,
                Y1 = 0,
                X2 = spot,
                Y2 = _canvas.ActualHeight,
                Stroke = brush,
                StrokeThickness = 2
            });
        }
        #endregion

        private void EquiDistanceMeasure_Unloaded(object sender, RoutedEventArgs e)
        {
            //TODO 注销消息

            UnLoadedCanvas();
            this.Unloaded -= EquiDistanceMeasure_Unloaded;
        }

        private void UnLoadedCanvas()
        {
            if (_canvas != null)
            {
                _canvas.MouseMove -= EquiDistanceMeasureCanvas_OnMouseMove;
                _canvas.MouseLeftButtonDown -= EquiDistanceMeasureCanvas_OnMouseLeftButtonDown;
            }
        }
    }
}
