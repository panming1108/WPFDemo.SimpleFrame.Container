using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using WPFDemo.SimpleFrame.Infra.Messager;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public class AFAreaAction : MaskActionBase
    {
        private GeometryDrawing _afRectDrawing = new GeometryDrawing();
        private GeometryDrawing _startLineDrawing = new GeometryDrawing();
        private GeometryDrawing _endLineDrawing = new GeometryDrawing();
        private GeometryDrawing _leftEllipseDrawing = new GeometryDrawing();
        private GeometryDrawing _rightEllipseDrawing = new GeometryDrawing();
        private GeometryDrawing _toolTipRectDrawing = new GeometryDrawing();
        private MaskText _toolTipContent = new MaskText();

        private AFAreaStatusEnum _aFAreaStatus;

        private readonly CultureInfo _culture = CultureInfo.GetCultureInfo("en-us");

        private MenuItem _rectMenuItem;

        public AFAreaAction(MaskPaint maskPaint, double leftOffset, double topOffset) : base(maskPaint, leftOffset, topOffset)
        {
            
        }

        public void OnRenderAFMask(double start, double end)
        {
            if (DrawingChildren.Count > 0)
            {
                DrawingChildren.Clear();
                return;
            }
            DrawingAFMask(start, end);
        }

        public void OnDragAFMaskOver(double start, double end)
        {
            DrawingAFMask(start, end);
        }

        public void OnClearAfArea()
        {
            DrawingChildren.Clear();
            RenderMaskPaint();
        }

        public override void DrawingMouseWheel(double offset)
        {
            base.DrawingMouseWheel(offset);
            if (DrawingChildren.Count <= 0 && DrawingTexts.Count <= 0)
            {
                return;
            }
            DrawingAFMask(_startLineDrawing.Bounds.Left + _startLineDrawing.Bounds.Width / 2 + offset, _endLineDrawing.Bounds.Left + _endLineDrawing.Bounds.Width / 2 + offset);
        }

        public override void DrawingMouseDownWheel(double offset, Point currentPoint)
        {
            base.DrawingMouseDownWheel(offset, currentPoint);
            switch (_aFAreaStatus)
            {
                case AFAreaStatusEnum.StartLine:
                    DrawingAFMask(currentPoint.X, _endLineDrawing.Bounds.Left + _endLineDrawing.Bounds.Width / 2 + offset);
                    break;
                case AFAreaStatusEnum.EndLine:
                    DrawingAFMask(_startLineDrawing.Bounds.Left + _startLineDrawing.Bounds.Width / 2 + offset, currentPoint.X);
                    break;
            }
        }

        public override void DrawingMouseUp(Point currentPoint)
        {
            base.DrawingMouseUp(currentPoint);
            if(_aFAreaStatus == AFAreaStatusEnum.LeftCircle)
            {
                MessagerInstance.GetMessager().Send(MaskMessageKeyEnum.ChangedMaskPosition, _afRectDrawing.Bounds.Left);
            }
            else if(_aFAreaStatus == AFAreaStatusEnum.RightCircle)
            {
                MessagerInstance.GetMessager().Send(MaskMessageKeyEnum.ChangedMaskPosition, _afRectDrawing.Bounds.Right);
            }
        }

        private void DrawingAFMask(double start, double end)
        {
            var left = Math.Min(start, end);
            var right = Math.Max(start, end);
            DrawingChildren.Clear();
            DrawingTexts.Clear();

            Rect afRect = new Rect(left, TopOffset, right - left, Height);

            _afRectDrawing = DrawingRect(afRect, new Pen(Brushes.Transparent, 0), (Brush)_brushConverter.ConvertFromString("#80007200"));
            _startLineDrawing = DrawingLine(start);
            _endLineDrawing = DrawingLine(end);

            DrawingChildren.Add(_afRectDrawing);
            DrawingChildren.Add(_startLineDrawing);
            DrawingChildren.Add(_endLineDrawing);

            if(afRect.Left >= LeftOffset && afRect.Left <= LeftOffset + Width)
            {
                _leftEllipseDrawing = null;
            }
            else
            {
                DrawingLeftCircleButton();
            }

            if(afRect.Right >= LeftOffset && afRect.Right <= LeftOffset + Width)
            {
                _rightEllipseDrawing = null;
            }
            else
            {
                DrawingRightCircleButton();
            }

            RenderMaskPaint();
        }

        public override void DrawingMouseOver(Point currentPoint)
        {
            base.DrawingMouseOver(currentPoint);
            DrawingTexts.Clear();
            if (DrawingChildren.Contains(_toolTipRectDrawing))
            {
                DrawingChildren.Remove(_toolTipRectDrawing);
            }
            if (_aFAreaStatus != AFAreaStatusEnum.LeftCircle && _aFAreaStatus != AFAreaStatusEnum.RightCircle)
            {
                _toolTipRectDrawing = null;
                return;
            }
            if(_toolTipRectDrawing == null || _toolTipRectDrawing.Bounds == default)
            {
                var toolTipRect = new Rect(currentPoint.X, currentPoint.Y, 122, 27);
                var toolTipContent = _aFAreaStatus == AFAreaStatusEnum.LeftCircle ? "定位到房颤开始位置" : "定位到房颤结束位置";
                _toolTipRectDrawing = DrawingRect(toolTipRect, new Pen((Brush)_brushConverter.ConvertFromString("#767676"), 1), (Brush)_brushConverter.ConvertFromString("#F1F2F7"));

                Typeface internalFace = new Typeface(new FontFamily("微软雅黑"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
                FormattedText text = new FormattedText(toolTipContent, _culture, FlowDirection.LeftToRight, internalFace, 12d, (Brush)_brushConverter.ConvertFromString("#575757"));
                double textXOffset = toolTipRect.X + (toolTipRect.Width - text.Width) / 2;
                double textYOffset = toolTipRect.Y + (toolTipRect.Height - text.Height) / 2;
                Point textPosition = new Point(textXOffset, textYOffset);
                _toolTipContent.Text = text;
                _toolTipContent.Position = textPosition;
            }
            DrawingChildren.Add(_toolTipRectDrawing);
            DrawingTexts.Add(_toolTipContent);

            RenderMaskPaint();
        }

        private void DrawingLeftCircleButton()
        {
            Pen circlePen = new Pen(Brushes.Black, 2);
            Rect leftCircleRect = new Rect(LeftOffset + (Width / 2) - 15 - 40, TopOffset + 35, 40, 40);
            EllipseGeometry leftEllipseGeometry = new EllipseGeometry(leftCircleRect);
            _leftEllipseDrawing = new GeometryDrawing(Brushes.Transparent, circlePen, leftEllipseGeometry);

            LineGeometry leftLineGeometry1 = new LineGeometry(new Point(leftCircleRect.X + 7, leftCircleRect.Y + 20), new Point(leftCircleRect.X + 33, leftCircleRect.Y + 20));
            LineGeometry leftLineGeometry2 = new LineGeometry(new Point(leftCircleRect.X + 7, leftCircleRect.Y + 20), new Point(leftCircleRect.X + 20, leftCircleRect.Y + 10));
            LineGeometry leftLineGeometry3 = new LineGeometry(new Point(leftCircleRect.X + 7, leftCircleRect.Y + 20), new Point(leftCircleRect.X + 20, leftCircleRect.Y + 30));
            GeometryDrawing leftLineDrawing1 = new GeometryDrawing(Brushes.Black, circlePen, leftLineGeometry1);
            GeometryDrawing leftLineDrawing2 = new GeometryDrawing(Brushes.Black, circlePen, leftLineGeometry2);
            GeometryDrawing leftLineDrawing3 = new GeometryDrawing(Brushes.Black, circlePen, leftLineGeometry3);

            DrawingChildren.Add(_leftEllipseDrawing);
            DrawingChildren.Add(leftLineDrawing1);
            DrawingChildren.Add(leftLineDrawing2);
            DrawingChildren.Add(leftLineDrawing3);
        }

        private void DrawingRightCircleButton()
        {
            Pen circlePen = new Pen(Brushes.Black, 2);
            Rect rightCircleRect = new Rect(LeftOffset + (Width / 2) + 15, TopOffset + 35, 40, 40);
            EllipseGeometry rightEllipseGeometry = new EllipseGeometry(rightCircleRect);
            _rightEllipseDrawing = new GeometryDrawing(Brushes.Transparent, circlePen, rightEllipseGeometry);

            LineGeometry rightLineGeometry1 = new LineGeometry(new Point(rightCircleRect.X + 7, rightCircleRect.Y + 20), new Point(rightCircleRect.X + 33, rightCircleRect.Y + 20));
            LineGeometry rightLineGeometry2 = new LineGeometry(new Point(rightCircleRect.X + 20, rightCircleRect.Y + 10), new Point(rightCircleRect.X + 33, rightCircleRect.Y + 20));
            LineGeometry rightLineGeometry3 = new LineGeometry(new Point(rightCircleRect.X + 20, rightCircleRect.Y + 30), new Point(rightCircleRect.X + 33, rightCircleRect.Y + 20));
            GeometryDrawing rightLineDrawing1 = new GeometryDrawing(Brushes.Black, circlePen, rightLineGeometry1);
            GeometryDrawing rightLineDrawing2 = new GeometryDrawing(Brushes.Black, circlePen, rightLineGeometry2);
            GeometryDrawing rightLineDrawing3 = new GeometryDrawing(Brushes.Black, circlePen, rightLineGeometry3);

            DrawingChildren.Add(_rightEllipseDrawing);
            DrawingChildren.Add(rightLineDrawing1);
            DrawingChildren.Add(rightLineDrawing2);
            DrawingChildren.Add(rightLineDrawing3);
        }

        private GeometryDrawing DrawingRect(Rect rect, Pen pen, Brush fill)
        {
            RectangleGeometry rectangleGeometry = new RectangleGeometry(rect);
            return new GeometryDrawing(fill, pen, rectangleGeometry);
        }

        private GeometryDrawing DrawingLine(double position)
        {
            Brush lineBrush = (Brush)_brushConverter.ConvertFromString("#007200");
            Pen linePen = new Pen(lineBrush, 4);
            LineGeometry lineGeometry = new LineGeometry(new Point(position, TopOffset), new Point(position, TopOffset + Height));
            return new GeometryDrawing(lineBrush, linePen, lineGeometry);
        }

        public override void Dispose()
        {
            _rectMenuItem.Click -= RectMenuItem_Click;
        }

        public override void DrawingDrag(Point currentPoint)
        {
            if(currentPoint.X < LeftOffset || currentPoint.X > LeftOffset + Width)
            {
                return;
            }
            switch (_aFAreaStatus)
            {
                case AFAreaStatusEnum.StartLine:
                    DrawingAFMask(currentPoint.X, _endLineDrawing.Bounds.Left + _endLineDrawing.Bounds.Width / 2);
                    break;
                case AFAreaStatusEnum.EndLine:
                    DrawingAFMask(_startLineDrawing.Bounds.Left + _startLineDrawing.Bounds.Width / 2, currentPoint.X);
                    break;
                default:
                    break;
            }
        }

        public override void DrawingDragOver(Point currentPoint)
        {
            MessagerInstance.GetMessager().Send(MaskMessageKeyEnum.DragAFMaskOver, new Tuple<double, double>(_startLineDrawing.Bounds.Left + _startLineDrawing.Bounds.Width, _endLineDrawing.Bounds.Left + _endLineDrawing.Bounds.Width));
            DrawingMouseUp(currentPoint);
        }

        public override void PrepareMask(Point current)
        {
            
        }

        public override void InitMask()
        {
            base.InitMask();
            _rectMenuItem = new MenuItem()
            {
                Header = "取消本阵房颤",
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,                
            };
            _rectMenuItem.Click += RectMenuItem_Click;
        }

        private void RectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OnClearAfArea();
        }

        protected override Cursor SetMouseOverCursor(Point currentPoint)
        {
            SetAfAreaStatus(currentPoint);
            Cursor cursor = Cursors.Arrow;
            switch (_aFAreaStatus)
            {
                case AFAreaStatusEnum.StartLine:
                case AFAreaStatusEnum.EndLine:
                    cursor = Cursors.SizeWE;
                    break;
                case AFAreaStatusEnum.LeftCircle:
                case AFAreaStatusEnum.RightCircle:
                    cursor = Cursors.Hand;
                    break;
            }
            return cursor;
        }

        private void SetAfAreaStatus(Point current)
        {
            if(_startLineDrawing.Bounds.Contains(current))
            {
                _aFAreaStatus = AFAreaStatusEnum.StartLine;
            }
            else if(_endLineDrawing.Bounds.Contains(current))
            {
                _aFAreaStatus = AFAreaStatusEnum.EndLine;
            }
            else if(_leftEllipseDrawing != null && _leftEllipseDrawing.Bounds.Contains(current))
            {
                _aFAreaStatus = AFAreaStatusEnum.LeftCircle;
            }
            else if (_rightEllipseDrawing != null && _rightEllipseDrawing.Bounds.Contains(current))
            {
                _aFAreaStatus = AFAreaStatusEnum.RightCircle;
            }
            else
            {
                _aFAreaStatus = AFAreaStatusEnum.Rect;
            }
        }

        protected override IEnumerable SetContextMenuItems(Point currentPoint)
        {
            return new MenuItem[] { _rectMenuItem };
        }

        private enum AFAreaStatusEnum
        {
            None,
            StartLine,
            EndLine,
            LeftCircle,
            RightCircle,
            Rect,
        }
    }
}
