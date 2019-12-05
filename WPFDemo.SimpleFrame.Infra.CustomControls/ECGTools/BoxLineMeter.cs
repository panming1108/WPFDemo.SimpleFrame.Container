using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.ECGTools
{
    [TemplatePart(Name = PART_Canvas, Type = typeof(Canvas))]
    [TemplatePart(Name = PART_Rectangle, Type = typeof(Rectangle))]

    [TemplatePart(Name = PART_TimeBorder, Type = typeof(Border))]
    [TemplatePart(Name = PART_VoltageBorder, Type = typeof(Border))]
    [TemplatePart(Name = PART_TimeThumb, Type = typeof(Thumb))]
    [TemplatePart(Name = PART_VoltageThumb, Type = typeof(Thumb))]

    [TemplatePart(Name = PART_LeftUpThumb, Type = typeof(Thumb))]
    [TemplatePart(Name = PART_CenterUpThumb, Type = typeof(Thumb))]
    [TemplatePart(Name = PART_RightUpThumb, Type = typeof(Thumb))]
    [TemplatePart(Name = PART_LeftCenterThumb, Type = typeof(Thumb))]
    [TemplatePart(Name = PART_RightCenterThumb, Type = typeof(Thumb))]
    [TemplatePart(Name = PART_LeftDownThumb, Type = typeof(Thumb))]
    [TemplatePart(Name = PART_CenterDownThumb, Type = typeof(Thumb))]
    [TemplatePart(Name = PART_RightDownThumb, Type = typeof(Thumb))]
    public class BoxLineMeter : ContentControl
    {
        #region PART Control Name
        protected const string PART_Canvas = "BoxLineMeterCanvas";
        protected const string PART_Rectangle = "PART_Rectangle";

        protected const string PART_TimeBorder = "PART_TimeBorder";
        protected const string PART_VoltageBorder = "PART_VoltageBorder";
        protected const string PART_TimeThumb = "PART_TimeThumb";
        protected const string PART_VoltageThumb = "PART_VoltageThumb";

        protected const string PART_LeftUpThumb = "PART_LeftUpThumb";
        protected const string PART_CenterUpThumb = "PART_CenterUpThumb";
        protected const string PART_RightUpThumb = "PART_RightUpThumb";
        protected const string PART_LeftCenterThumb = "PART_LeftCenterThumb";
        protected const string PART_RightCenterThumb = "PART_RightCenterThumb";
        protected const string PART_LeftDownThumb = "PART_LeftDownThumb";
        protected const string PART_CenterDownThumb = "PART_CenterDownThumb";
        protected const string PART_RightDownThumb = "PART_RightDownThumb";
        #endregion

        #region PART Control
        protected Canvas _canvas;
        protected Rectangle _rectangle;

        protected Border _timeBorder;
        protected Border _voltageBorder;
        protected Thumb _timeThumb;
        protected Thumb _voltageThumb;

        protected Thumb _leftUpThumb;
        protected Thumb _centerUpThumb;
        protected Thumb _rightUpThumb;
        protected Thumb _leftCenterThumb;
        protected Thumb _rightCenterThumb;
        protected Thumb _leftDownThumb;
        protected Thumb _centerDownThumb;
        protected Thumb _rightDownThumb;
        #endregion

        #region fields
        private bool _isUseThumb;
        private bool _isCompassesOn;//判断是否开启
        private Point _startPoint;
        private Point _endPoint;
        private Point _rectangleLeftUpPoint;
        private Point _dragStartPoint;
        private double _offsetX;
        private double _offsetY;
        #endregion

        #region DependencyProperty
        public double RectangleHeight
        {
            get { return (double)GetValue(RectangleHeightProperty); }
            set { SetValue(RectangleHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RectangleHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RectangleHeightProperty =
            DependencyProperty.Register("RectangleHeight", typeof(double), typeof(BoxLineMeter), new PropertyMetadata(0.0));

        public double RectangleWidth
        {
            get { return (double)GetValue(RectangleWidthProperty); }
            set { SetValue(RectangleWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RectangleWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RectangleWidthProperty =
            DependencyProperty.Register("RectangleWidth", typeof(double), typeof(BoxLineMeter), new PropertyMetadata(0.0));

        public Brush TextBorderBackground
        {
            get { return (Brush)GetValue(TextBorderBackgroundProperty); }
            set { SetValue(TextBorderBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextBorderBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextBorderBackgroundProperty =
            DependencyProperty.Register("TextBorderBackground", typeof(Brush), typeof(BoxLineMeter));

        public string TimeInterval
        {
            get { return (string)GetValue(TimeIntervalProperty); }
            set { SetValue(TimeIntervalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TimeInterval.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeIntervalProperty =
            DependencyProperty.Register("TimeInterval", typeof(string), typeof(BoxLineMeter), new PropertyMetadata(string.Empty));

        public string HeartRate
        {
            get { return (string)GetValue(HeartRateProperty); }
            set { SetValue(HeartRateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeartRate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeartRateProperty =
            DependencyProperty.Register("HeartRate", typeof(string), typeof(BoxLineMeter), new PropertyMetadata(string.Empty));

        public string Voltage
        {
            get { return (string)GetValue(VoltageProperty); }
            set { SetValue(VoltageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Voltage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VoltageProperty =
            DependencyProperty.Register("Voltage", typeof(string), typeof(BoxLineMeter), new PropertyMetadata(string.Empty));

        public Visibility OtherControlsVisiblty
        {
            get { return (Visibility)GetValue(OtherControlsVisibltyProperty); }
            set { SetValue(OtherControlsVisibltyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OtherControlsVisiblty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OtherControlsVisibltyProperty =
            DependencyProperty.Register("OtherControlsVisiblty", typeof(Visibility), typeof(BoxLineMeter), new PropertyMetadata(Visibility.Collapsed));

        public Visibility ThumbVisiblity
        {
            get { return (Visibility)GetValue(ThumbVisiblityProperty); }
            set { SetValue(ThumbVisiblityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ThumbVisiblity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbVisiblityProperty =
            DependencyProperty.Register("ThumbVisiblity", typeof(Visibility), typeof(BoxLineMeter), new PropertyMetadata(Visibility.Collapsed));

        public Brush MeasuringLineBrush
        {
            get { return (Brush)GetValue(MeasuringLineBrushProperty); }
            set { SetValue(MeasuringLineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MeasuringLineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MeasuringLineBrushProperty =
            DependencyProperty.Register("MeasuringLineBrush", typeof(Brush), typeof(BoxLineMeter));

        public Brush MeasuredLineBrush
        {
            get { return (Brush)GetValue(MeasuredLineBrushProperty); }
            set { SetValue(MeasuredLineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MeasuredLineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MeasuredLineBrushProperty =
            DependencyProperty.Register("MeasuredLineBrush", typeof(Brush), typeof(BoxLineMeter));

        public Brush LineBrush
        {
            get { return (Brush)GetValue(LineBrushProperty); }
            set { SetValue(LineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LineBrushProperty =
            DependencyProperty.Register("LineBrush", typeof(Brush), typeof(BoxLineMeter));
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UnLoadedPartControls();
            LoadedPartControls();
        }
        private void LoadedPartControls()
        {
            LoadCanvas();

            LoadTimeBorder();
            LoadVoltageBorder();
            LoadTimeThumb();
            LoadVoltageThumb();

            LoadTimeThumb();
            LoadVoltageThumb();
            LoadRectangle();
            LoadLeftUpThumb();
            LoadCenterUpThumb();
            LoadRightUpThumb();
            LoadLeftCenterThumb();
            LoadRightCenterThumb();
            LoadLeftDownThumb();
            LoadCenterDownThumb();
            LoadRightDownThumb();
        }

        private void UnLoadedPartControls()
        {
            UnLoadCanvas();

            UnLoadTimeBorder();
            UnLoadVoltageBorder();
            UnLoadTimeThumb();
            UnLoadVoltageThumb();

            UnLoadRectangle();
            UnLoadLeftUpThumb();
            UnLoadCenterUpThumb();
            UnLoadRightUpThumb();
            UnLoadLeftCenterThumb();
            UnLoadRightCenterThumb();
            UnLoadLeftDownThumb();
            UnLoadCenterDownThumb();
            UnLoadRightDownThumb();
        }

        #region LoadControls 注册事件
        private void LoadRightDownThumb()
        {
            _rightDownThumb = GetTemplateChild(PART_RightDownThumb) as Thumb;
            if (_rightDownThumb != null)
            {
                _rightDownThumb.MouseEnter += Thumbs_OnMouseEnter;
                _rightDownThumb.MouseLeave += Thumbs_OnMouseLeave;
                _rightDownThumb.DragDelta += RightDownThumb_OnDragDelta;
            }
        }

        private void LoadCenterDownThumb()
        {
            _centerDownThumb = GetTemplateChild(PART_CenterDownThumb) as Thumb;
            if (_centerDownThumb != null)
            {
                _centerDownThumb.MouseEnter += Thumbs_OnMouseEnter;
                _centerDownThumb.MouseLeave += Thumbs_OnMouseLeave;
                _centerDownThumb.DragDelta += CenterDownThumb_OnDragDelta;
            }
        }

        private void LoadLeftDownThumb()
        {
            _leftDownThumb = GetTemplateChild(PART_LeftDownThumb) as Thumb;
            if(_leftDownThumb != null)
            {
                _leftDownThumb.MouseEnter += Thumbs_OnMouseEnter;
                _leftDownThumb.MouseLeave += Thumbs_OnMouseLeave;
                _leftDownThumb.DragDelta += LeftDownThumb_OnDragDelta;
            }
        }

        private void LoadRightCenterThumb()
        {
            _rightCenterThumb = GetTemplateChild(PART_RightCenterThumb) as Thumb;
            if(_rightCenterThumb != null)
            {
                _rightCenterThumb.MouseEnter += Thumbs_OnMouseEnter;
                _rightCenterThumb.MouseLeave += Thumbs_OnMouseLeave;
                _rightCenterThumb.DragDelta += RightCenterThumb_OnDragDelta;
            }
        }

        private void LoadLeftCenterThumb()
        {
            _leftCenterThumb = GetTemplateChild(PART_LeftCenterThumb) as Thumb;
            if(_leftCenterThumb != null)
            {
                _leftCenterThumb.MouseEnter += Thumbs_OnMouseEnter;
                _leftCenterThumb.MouseLeave += Thumbs_OnMouseLeave;
                _leftCenterThumb.DragDelta += LeftCenterThumb_OnDragDelta;
            }
        }

        private void LoadRightUpThumb()
        {
            _rightUpThumb = GetTemplateChild(PART_RightUpThumb) as Thumb;
            if(_rightUpThumb != null)
            {
                _rightUpThumb.MouseEnter += Thumbs_OnMouseEnter;
                _rightUpThumb.MouseLeave += Thumbs_OnMouseLeave;
                _rightUpThumb.DragDelta += RightUpThumb_OnDragDelta;
            }
        }

        private void LoadCenterUpThumb()
        {
            _centerUpThumb = GetTemplateChild(PART_CenterUpThumb) as Thumb;
            if(_centerUpThumb != null)
            {
                _centerUpThumb.MouseEnter += Thumbs_OnMouseEnter;
                _centerUpThumb.MouseLeave += Thumbs_OnMouseLeave;
                _centerUpThumb.DragDelta += CenterUpThumb_OnDragDelta;
            }
        }

        private void LoadLeftUpThumb()
        {
            _leftUpThumb = GetTemplateChild(PART_LeftUpThumb) as Thumb;
            if(_leftUpThumb != null)
            {
                _leftUpThumb.MouseEnter += Thumbs_OnMouseEnter;
                _leftUpThumb.MouseLeave += Thumbs_OnMouseLeave;
                _leftUpThumb.DragDelta += LeftUpThumb_OnDragDelta;
            }
        }

        private void LoadRectangle()
        {
            _rectangle = GetTemplateChild(PART_Rectangle) as Rectangle;
            if (_rectangle != null)
            {

            }
        }

        private void LoadVoltageBorder()
        {
            _voltageBorder = GetTemplateChild(PART_VoltageBorder) as Border;
        }

        private void LoadTimeBorder()
        {
            _timeBorder = GetTemplateChild(PART_TimeBorder) as Border;
        }

        private void LoadVoltageThumb()
        {
            _voltageThumb = GetTemplateChild(PART_VoltageThumb) as Thumb;
            if(_voltageThumb != null)
            {
                _voltageThumb.MouseEnter += Thumbs_OnMouseEnter;
                _voltageThumb.MouseLeave += Thumbs_OnMouseLeave;
                _voltageThumb.DragDelta += TextThumb_DragDelta;
            }
        }

        private void LoadTimeThumb()
        {
            _timeThumb = GetTemplateChild(PART_TimeThumb) as Thumb;
            if (_timeThumb != null)
            {
                _timeThumb.MouseEnter += Thumbs_OnMouseEnter;
                _timeThumb.MouseLeave += Thumbs_OnMouseLeave;
                _timeThumb.DragDelta += TextThumb_DragDelta;
            }
        }

        private void LoadCanvas()
        {
            _canvas = GetTemplateChild(PART_Canvas) as Canvas;
            if (_canvas != null)
            {
                _canvas.MouseMove += BoxLineMeterCanvas_OnMouseMove;
                _canvas.MouseLeftButtonDown += BoxLineMeterCanvas_OnMouseLeftButtonDown;
            }
        }
        #endregion

        #region UnLoadControls 注销事件
        private void UnLoadRightDownThumb()
        {
            if (_rightDownThumb != null)
            {
                _rightDownThumb.MouseEnter -= Thumbs_OnMouseEnter;
                _rightDownThumb.MouseLeave -= Thumbs_OnMouseLeave;
                _rightDownThumb.DragDelta -= RightDownThumb_OnDragDelta;
            }
        }

        private void UnLoadCenterDownThumb()
        {
            if (_centerDownThumb != null)
            {
                _centerDownThumb.MouseEnter -= Thumbs_OnMouseEnter;
                _centerDownThumb.MouseLeave -= Thumbs_OnMouseLeave;
                _centerDownThumb.DragDelta -= CenterDownThumb_OnDragDelta;
            }
        }

        private void UnLoadLeftDownThumb()
        {
            if (_leftDownThumb != null)
            {
                _leftDownThumb.MouseEnter -= Thumbs_OnMouseEnter;
                _leftDownThumb.MouseLeave -= Thumbs_OnMouseLeave;
                _leftDownThumb.DragDelta -= LeftDownThumb_OnDragDelta;
            }
        }

        private void UnLoadRightCenterThumb()
        {
            if (_rightCenterThumb != null)
            {
                _rightCenterThumb.MouseEnter -= Thumbs_OnMouseEnter;
                _rightCenterThumb.MouseLeave -= Thumbs_OnMouseLeave;
                _rightCenterThumb.DragDelta -= RightCenterThumb_OnDragDelta;
            }
        }

        private void UnLoadLeftCenterThumb()
        {
            if (_leftCenterThumb != null)
            {
                _leftCenterThumb.MouseEnter -= Thumbs_OnMouseEnter;
                _leftCenterThumb.MouseLeave -= Thumbs_OnMouseLeave;
                _leftCenterThumb.DragDelta -= LeftCenterThumb_OnDragDelta;
            }
        }

        private void UnLoadRightUpThumb()
        {
            if (_rightUpThumb != null)
            {
                _rightUpThumb.MouseEnter -= Thumbs_OnMouseEnter;
                _rightUpThumb.MouseLeave -= Thumbs_OnMouseLeave;
                _rightUpThumb.DragDelta -= RightUpThumb_OnDragDelta;
            }
        }

        private void UnLoadCenterUpThumb()
        {
            if (_centerUpThumb != null)
            {
                _centerUpThumb.MouseEnter -= Thumbs_OnMouseEnter;
                _centerUpThumb.MouseLeave -= Thumbs_OnMouseLeave;
                _centerUpThumb.DragDelta -= CenterUpThumb_OnDragDelta;
            }
        }

        private void UnLoadLeftUpThumb()
        {
            if (_leftUpThumb != null)
            {
                _leftUpThumb.MouseEnter -= Thumbs_OnMouseEnter;
                _leftUpThumb.MouseLeave -= Thumbs_OnMouseLeave;
                _leftUpThumb.DragDelta -= LeftUpThumb_OnDragDelta;
            }
        }

        private void UnLoadRectangle()
        {
            if (_rectangle != null)
            {
                
            }
        }

        private void UnLoadVoltageBorder()
        {
            if (_voltageBorder != null)
            {

            }
        }

        private void UnLoadTimeBorder()
        {
            if (_timeBorder != null)
            {

            }
        }

        private void UnLoadVoltageThumb()
        {
            if(_voltageThumb != null)
            {
                _voltageThumb.MouseEnter -= Thumbs_OnMouseEnter;
                _voltageThumb.MouseLeave -= Thumbs_OnMouseLeave;
                _voltageThumb.DragDelta -= TextThumb_DragDelta;
            }
        }

        private void UnLoadTimeThumb()
        {
            if (_timeThumb != null)
            {
                _timeThumb.MouseEnter -= Thumbs_OnMouseEnter;
                _timeThumb.MouseLeave -= Thumbs_OnMouseLeave;
                _timeThumb.DragDelta -= TextThumb_DragDelta;
            }
        }

        private void UnLoadCanvas()
        {
            if (_canvas != null)
            {
                _canvas.MouseMove -= BoxLineMeterCanvas_OnMouseMove;
                _canvas.MouseLeftButtonDown -= BoxLineMeterCanvas_OnMouseLeftButtonDown;
            }
        }
        #endregion

        public BoxLineMeter()
        {
            Unloaded += BoxLineMeter_Unloaded;
            _isCompassesOn = true;
        }

        #region Canvas事件
        private void BoxLineMeterCanvas_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(_isCompassesOn && !_isUseThumb)
            {
                _startPoint = e.GetPosition((FrameworkElement)sender);
            }
            e.Handled = true;
        }

        private void BoxLineMeterCanvas_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !_isUseThumb)
            {
                OtherControlsVisiblty = Visibility.Visible;
                LineBrush = MeasuringLineBrush;
                _endPoint = e.GetPosition((FrameworkElement)sender);
                if(_endPoint.Y < _timeBorder.Height)
                {
                    _endPoint.Y = _timeBorder.Height;
                }
                if(_endPoint.X < _voltageBorder.Width)
                {
                    _endPoint.X = _voltageBorder.Width;
                }
                RectangleHeight = Math.Abs(_endPoint.Y - _startPoint.Y);
                RectangleWidth = Math.Abs(_endPoint.X - _startPoint.X);
                _rectangleLeftUpPoint = new Point(Math.Min(_startPoint.X, _endPoint.X), Math.Min(_startPoint.Y, _endPoint.Y));
                SetControlsPosition(_rectangleLeftUpPoint);     
            }
            else
            {
                var point = e.GetPosition((FrameworkElement)sender);
                Rect rect = new Rect(_rectangleLeftUpPoint.X, _rectangleLeftUpPoint.Y, RectangleWidth, RectangleHeight);
                if(rect.Contains(point))
                {
                    LineBrush = MeasuringLineBrush;
                    ThumbVisiblity = Visibility.Visible;
                }
                else
                {
                    LineBrush = MeasuredLineBrush;
                    ThumbVisiblity = Visibility.Collapsed;
                }
            }
            e.Handled = true;
        }
        #endregion

        #region 整体拖动事件
        private void TextThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            _rectangleLeftUpPoint.X += e.HorizontalChange;
            _rectangleLeftUpPoint.Y += e.VerticalChange;
            if(_rectangleLeftUpPoint.X <= _voltageBorder.Width)
            {
                _rectangleLeftUpPoint.X = _voltageBorder.Width;
            }
            if(_rectangleLeftUpPoint.X >= _canvas.ActualWidth - RectangleWidth)
            {
                _rectangleLeftUpPoint.X = _canvas.ActualWidth - RectangleWidth;
            }
            if(_rectangleLeftUpPoint.Y <= _timeBorder.Height)
            {
                _rectangleLeftUpPoint.Y = _timeBorder.Height;
            }
            if(_rectangleLeftUpPoint.Y >= _canvas.ActualHeight - RectangleHeight)
            {
                _rectangleLeftUpPoint.Y = _canvas.ActualHeight - RectangleHeight;
            }
            SetControlsPosition(_rectangleLeftUpPoint);
            e.Handled = true;
        }
        #endregion

        #region Thumb拖动事件
        private void RightCenterThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            var width = RectangleWidth + e.HorizontalChange;
            if(width < 1)
            {
                RectangleWidth = 1;
            }
            else if(width > _canvas.ActualWidth - _rectangleLeftUpPoint.X)
            {
                RectangleWidth = _canvas.ActualWidth - _rectangleLeftUpPoint.X;
            }
            else
            {
                RectangleWidth = width;
            }
            SetControlsPosition(_rectangleLeftUpPoint);
        }
        private void CenterDownThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            var height = RectangleHeight + e.VerticalChange;
            if(height < 1)
            {
                RectangleHeight = 1;
            }
            else if(height > _canvas.ActualHeight - _rectangleLeftUpPoint.Y)
            {
                RectangleHeight = _canvas.ActualHeight - _rectangleLeftUpPoint.Y;
            }
            else
            {
                RectangleHeight = height;
            }
            SetControlsPosition(_rectangleLeftUpPoint);
        }
        private void RightDownThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            var width = RectangleWidth + e.HorizontalChange;
            var height = RectangleHeight + e.VerticalChange;
            if (height < 1)
            {
                RectangleHeight = 1;
            }
            else if (height > _canvas.ActualHeight - _rectangleLeftUpPoint.Y)
            {
                RectangleHeight = _canvas.ActualHeight - _rectangleLeftUpPoint.Y;
            }
            else
            {
                RectangleHeight = height;
            }
            if (width < 1)
            {
                RectangleWidth = 1;
            }
            else if (width > _canvas.ActualWidth - _rectangleLeftUpPoint.X)
            {
                RectangleWidth = _canvas.ActualWidth - _rectangleLeftUpPoint.X;
            }
            else
            {
                RectangleWidth = width;
            }
            SetControlsPosition(_rectangleLeftUpPoint);
        }
        private void LeftCenterThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            var leftUpPointX = _rectangleLeftUpPoint.X + e.HorizontalChange;
            var width = RectangleWidth - e.HorizontalChange;

            if(leftUpPointX < _voltageBorder.Width)
            {
                _rectangleLeftUpPoint.X = _voltageBorder.Width;
                SetControlsPosition(_rectangleLeftUpPoint);
                return;
            }
            if(width < 1)
            {
                RectangleWidth = 1;
                SetControlsPosition(_rectangleLeftUpPoint);
                return;
            }

            _rectangleLeftUpPoint.X = leftUpPointX;
            RectangleWidth = width;
            SetControlsPosition(_rectangleLeftUpPoint);
        }
        private void CenterUpThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            var leftUpPointY = _rectangleLeftUpPoint.Y + e.VerticalChange;
            var height = RectangleHeight - e.VerticalChange;

            if (leftUpPointY < _timeBorder.Height)
            {
                _rectangleLeftUpPoint.Y = _timeBorder.Height;
                SetControlsPosition(_rectangleLeftUpPoint);
                return;
            }
            if (height < 1)
            {
                RectangleHeight = 1;
                SetControlsPosition(_rectangleLeftUpPoint);
                return;
            }

            _rectangleLeftUpPoint.Y = leftUpPointY;
            RectangleHeight = height;
            SetControlsPosition(_rectangleLeftUpPoint);
        }
        private void LeftDownThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            var leftUpPointX = _rectangleLeftUpPoint.X + e.HorizontalChange;
            var height = RectangleHeight + e.VerticalChange;
            var width = RectangleWidth - e.HorizontalChange;

            if (height < 1)
            {
                RectangleHeight = 1;
                SetControlsPosition(_rectangleLeftUpPoint);
                return;
            }
            else if(height > _canvas.ActualHeight - _rectangleLeftUpPoint.Y)
            {
                RectangleHeight = _canvas.ActualHeight - _rectangleLeftUpPoint.Y;
                SetControlsPosition(_rectangleLeftUpPoint);
                return;
            }
            if (leftUpPointX < _voltageBorder.Width)
            {
                _rectangleLeftUpPoint.X = _voltageBorder.Width;
                SetControlsPosition(_rectangleLeftUpPoint);
                return;
            }
            if (width < 1)
            {
                RectangleWidth = 1;
                SetControlsPosition(_rectangleLeftUpPoint);
                return;
            }
            _rectangleLeftUpPoint.X = leftUpPointX;
            RectangleWidth = width;
            RectangleHeight = height;
            SetControlsPosition(_rectangleLeftUpPoint);
        }
        private void RightUpThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            var leftUpPointY = _rectangleLeftUpPoint.Y + e.VerticalChange;
            var height = RectangleHeight - e.VerticalChange;
            var width = RectangleWidth + e.HorizontalChange;

            if (height < 1)
            {
                RectangleHeight = 1;
                SetControlsPosition(_rectangleLeftUpPoint);
                return;
            }
            if (leftUpPointY < _timeBorder.Height)
            {
                _rectangleLeftUpPoint.Y = _timeBorder.Height;
                SetControlsPosition(_rectangleLeftUpPoint);
                return;
            }
            if (width < 1)
            {
                RectangleWidth = 1;
                SetControlsPosition(_rectangleLeftUpPoint);
                return;
            }
            else if(width > _canvas.ActualWidth - _rectangleLeftUpPoint.X)
            {
                RectangleWidth = _canvas.ActualWidth - _rectangleLeftUpPoint.X;
                SetControlsPosition(_rectangleLeftUpPoint);
                return;
            }
            _rectangleLeftUpPoint.Y = leftUpPointY;
            RectangleWidth = width;
            RectangleHeight = height;
            SetControlsPosition(_rectangleLeftUpPoint);
        }
        private void LeftUpThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            var leftUpPointY = _rectangleLeftUpPoint.Y + e.VerticalChange;
            var leftUpPointX = _rectangleLeftUpPoint.X + e.HorizontalChange;
            var height = RectangleHeight - e.VerticalChange;
            var width = RectangleWidth - e.HorizontalChange;

            if (height < 1)
            {
                RectangleHeight = 1;
                SetControlsPosition(_rectangleLeftUpPoint);
                return;
            }
            if (leftUpPointY < _timeBorder.Height)
            {
                _rectangleLeftUpPoint.Y = _timeBorder.Height;
                SetControlsPosition(_rectangleLeftUpPoint);
                return;
            }
            if (width < 1)
            {
                RectangleWidth = 1;
                SetControlsPosition(_rectangleLeftUpPoint);
                return;
            }
            if (leftUpPointX < _voltageBorder.Width)
            {
                _rectangleLeftUpPoint.X = _voltageBorder.Width;
                SetControlsPosition(_rectangleLeftUpPoint);
                return;
            }
            _rectangleLeftUpPoint.Y = leftUpPointY;
            _rectangleLeftUpPoint.X = leftUpPointX;
            RectangleWidth = width;
            RectangleHeight = height;
            SetControlsPosition(_rectangleLeftUpPoint);
        }
        #endregion

        #region 其他控件鼠标进入离开事件
        private void Thumbs_OnMouseLeave(object sender, MouseEventArgs e)
        {
            _isUseThumb = false;
        }

        private void Thumbs_OnMouseEnter(object sender, MouseEventArgs e)
        {
            _isUseThumb = true;
        }
        #endregion

        private void SetControlsPosition(Point leftUpPoint)
        {
            double rectLeftUpPointX = leftUpPoint.X;
            double rectLeftUpPointY = leftUpPoint.Y;

            double halfThumbHeight = _leftUpThumb.Height / 2;
            double halfThumbWidth = _leftUpThumb.Width / 2;

            double halfRectangleHeight = RectangleHeight / 2;
            double halfRectangleWidth = RectangleWidth / 2;

            Canvas.SetTop(_rectangle, rectLeftUpPointY);
            Canvas.SetLeft(_rectangle, rectLeftUpPointX);

            Canvas.SetTop(_timeBorder, rectLeftUpPointY - _timeBorder.Height);
            Canvas.SetLeft(_timeBorder, rectLeftUpPointX);
            Canvas.SetTop(_timeThumb, rectLeftUpPointY - _timeBorder.Height);
            Canvas.SetLeft(_timeThumb, rectLeftUpPointX);

            Canvas.SetTop(_voltageBorder, rectLeftUpPointY + _voltageBorder.Height);
            Canvas.SetLeft(_voltageBorder, rectLeftUpPointX - _voltageBorder.Width);
            Canvas.SetTop(_voltageThumb, rectLeftUpPointY + _voltageBorder.Height);
            Canvas.SetLeft(_voltageThumb, rectLeftUpPointX - _voltageBorder.Width);

            Canvas.SetTop(_leftUpThumb, rectLeftUpPointY - halfThumbHeight);
            Canvas.SetLeft(_leftUpThumb, rectLeftUpPointX - halfThumbWidth);

            Canvas.SetTop(_centerUpThumb, rectLeftUpPointY - halfThumbHeight);
            Canvas.SetLeft(_centerUpThumb, rectLeftUpPointX + halfRectangleWidth - halfThumbWidth);

            Canvas.SetTop(_rightUpThumb, rectLeftUpPointY - halfThumbHeight);
            Canvas.SetLeft(_rightUpThumb, rectLeftUpPointX + RectangleWidth - halfThumbWidth);

            Canvas.SetTop(_leftCenterThumb, rectLeftUpPointY + halfRectangleHeight - halfThumbHeight);
            Canvas.SetLeft(_leftCenterThumb, rectLeftUpPointX - halfThumbWidth);

            Canvas.SetTop(_rightCenterThumb, rectLeftUpPointY + halfRectangleHeight - halfThumbHeight);
            Canvas.SetLeft(_rightCenterThumb, rectLeftUpPointX + RectangleWidth - halfThumbWidth);

            Canvas.SetTop(_leftDownThumb, rectLeftUpPointY + RectangleHeight - halfThumbHeight);
            Canvas.SetLeft(_leftDownThumb, rectLeftUpPointX - halfThumbWidth);

            Canvas.SetTop(_centerDownThumb, rectLeftUpPointY + RectangleHeight - halfThumbHeight);
            Canvas.SetLeft(_centerDownThumb, rectLeftUpPointX + halfRectangleWidth - halfThumbWidth);

            Canvas.SetTop(_rightDownThumb, rectLeftUpPointY + RectangleHeight - halfThumbHeight);
            Canvas.SetLeft(_rightDownThumb, rectLeftUpPointX + RectangleWidth - halfThumbWidth);

            SetText(RectangleWidth + "", RectangleHeight + "", HeartRate);
        }

        private void SetText(string timeInterval, string voltage, string heartRate)
        {
            TimeInterval = timeInterval + "";
            Voltage = voltage + "";
            HeartRate = heartRate + "";
        }

        private void BoxLineMeter_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //注销消息
            UnLoadedPartControls();
            Unloaded -= BoxLineMeter_Unloaded;
        }
    }
}
