using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public class BoxLineMeterAction : MaskActionBase, IScreenDragAction
    {
        private BoxLineMeterStatusEnum _boxLineMeterStatus;
        private Point _lastPoint;
        private readonly double _vTextRectWidth = 75;
        private readonly double _tTextRectWidth = 150;
        private readonly double _textRectHeight = 20;

        private Rect _originRect;
        private Rect _originVTextRect;
        private Rect _originTTextRect;
        private Rect _originLeftTopRect;
        private Rect _originLeftCenterRect;
        private Rect _originLeftBottomRect;
        private Rect _originCenterTopRect;
        private Rect _originCenterBottomRect;
        private Rect _originRightTopRect;
        private Rect _originRightCenterRect;
        private Rect _originRightBottomRect;

        private readonly CultureInfo _culture = CultureInfo.GetCultureInfo("en-us");
        private readonly Typeface _typeface = new Typeface("Klavika");
        private readonly double _emSize = 15d;

        public int DragPriority { get; set; }

        public BoxLineMeterAction(MaskPaint maskPaint, double leftOffset, double topOffset) : base(maskPaint, leftOffset, topOffset)
        {
        }

        public override void DrawingDrag(Point currentPoint)
        {
            bool isMeasuring = true;
            switch (_boxLineMeterStatus)
            {
                case BoxLineMeterStatusEnum.VText:
                case BoxLineMeterStatusEnum.TText:
                    DragTextRect(currentPoint);
                    isMeasuring = false;
                    break;
                case BoxLineMeterStatusEnum.LeftTopThumb:
                    DragLeftTopPoint(currentPoint);
                    break;
                case BoxLineMeterStatusEnum.LeftCenterThumb:
                    DragLeftCenterPoint(currentPoint);
                    break;
                case BoxLineMeterStatusEnum.LeftBottomThumb:
                    DragLeftBottomPoint(currentPoint);
                    break;
                case BoxLineMeterStatusEnum.RightTopThumb:
                    DragRightTopPoint(currentPoint);
                    break;
                case BoxLineMeterStatusEnum.RightCenterThumb:
                    DragRightCenterPoint(currentPoint);
                    break;
                case BoxLineMeterStatusEnum.RightBottomThumb:
                    DragRightBottomPoint(currentPoint);
                    break;
                case BoxLineMeterStatusEnum.CenterTopThumb:
                    DragCenterTopPoint(currentPoint);
                    break;
                case BoxLineMeterStatusEnum.CenterBottomThumb:
                    DragCenterBottomPoint(currentPoint);
                    break;
                default:
                    DragRender(currentPoint);
                    isMeasuring = false;
                    break;
            }
            DrawingRect(_originRect.TopLeft, _originRect.Height, _originRect.Width, isMeasuring);

        }

        private void DragRender(Point currentPoint)
        {
            var xOffset = currentPoint.X - _lastPoint.X;
            var yOffset = currentPoint.Y - _lastPoint.Y;

            if (currentPoint.Y < _textRectHeight)
            {
                currentPoint.Y = _textRectHeight;
            }
            if (currentPoint.X < _vTextRectWidth)
            {
                currentPoint.X = _vTextRectWidth;
            }
            _originRect.Height = Math.Abs(yOffset);
            _originRect.Width = Math.Abs(xOffset);
            _originRect.X = Math.Min(_lastPoint.X, currentPoint.X);
            _originRect.Y = Math.Min(_lastPoint.Y, currentPoint.Y);
        }

        private void DragCenterBottomPoint(Point currentPoint)
        {
            var yOffset = currentPoint.Y - _lastPoint.Y;
            
            var height = _originRect.Height + yOffset;

            if (height >= 1 && height <= Height + TopOffset - _originRect.Y)
            {
                _originRect.Height = height;
                _lastPoint.Y = currentPoint.Y;
            }           
        }

        private void DragCenterTopPoint(Point currentPoint)
        {
            var yOffset = currentPoint.Y - _lastPoint.Y;
            
            var leftUpPointY = _originRect.Y + yOffset;
            var height = _originRect.Height - yOffset;

            if (height >= 1 && leftUpPointY >= _textRectHeight + TopOffset)
            {
                _originRect.Y = leftUpPointY;
                _originRect.Height = height;
                _lastPoint.Y = currentPoint.Y;
            }
        }

        private void DragRightBottomPoint(Point currentPoint)
        {
            var xOffset = currentPoint.X - _lastPoint.X;
            var yOffset = currentPoint.Y - _lastPoint.Y;

            var width = _originRect.Width + xOffset;
            var height = _originRect.Height + yOffset;

            if (height >= 1 && height <= Height + TopOffset - _originRect.Y)
            {
                _originRect.Height = height;
                _lastPoint.Y = currentPoint.Y;
            }
            if (width >= 1 && width <= Width + LeftOffset - _originRect.X)
            {
                _originRect.Width = width;
                _lastPoint.X = currentPoint.X;
            }
        }

        private void DragRightCenterPoint(Point currentPoint)
        {
            var xOffset = currentPoint.X - _lastPoint.X;
            
            var width = _originRect.Width + xOffset;

            if (width >= 1 && width <= Width + LeftOffset - _originRect.X)
            {
                _originRect.Width = width;
                _lastPoint.X = currentPoint.X;
            }
        }

        private void DragRightTopPoint(Point currentPoint)
        {
            var xOffset = currentPoint.X - _lastPoint.X;
            var yOffset = currentPoint.Y - _lastPoint.Y;
            
            var leftUpPointY = _originRect.Y + yOffset;
            var height = _originRect.Height - yOffset;
            var width = _originRect.Width + xOffset;

            if (height >= 1 && leftUpPointY >= _textRectHeight + TopOffset)
            {
                _originRect.Height = height;
                _originRect.Y = leftUpPointY;
                _lastPoint.Y = currentPoint.Y;
            }
            if (width >= 1 && width <= Width + LeftOffset - _originRect.X)
            {
                _originRect.Width = width;
                _lastPoint.X = currentPoint.X;
            }
        }

        private void DragLeftBottomPoint(Point currentPoint)
        {
            var xOffset = currentPoint.X - _lastPoint.X;
            var yOffset = currentPoint.Y - _lastPoint.Y;

            var leftUpPointX = _originRect.X + xOffset;
            var height = _originRect.Height + yOffset;
            var width = _originRect.Width - xOffset;

            if (height >= 1 && height <= Height + TopOffset - _originRect.Y)
            {
                _originRect.Height = height;
                _lastPoint.Y = currentPoint.Y;
            }
            if (width >= 1 && leftUpPointX >= _vTextRectWidth + LeftOffset)
            {
                _originRect.Width = width;
                _originRect.X = leftUpPointX;
                _lastPoint.X = currentPoint.X;
            }
        }

        private void DragLeftCenterPoint(Point currentPoint)
        {
            var xOffset = currentPoint.X - _lastPoint.X;

            var leftUpPointX = _originRect.X + xOffset;
            var width = _originRect.Width - xOffset;

            if (width >= 1 && leftUpPointX >= _vTextRectWidth + LeftOffset)
            {
                _originRect.X = leftUpPointX;
                _originRect.Width = width;
                _lastPoint.X = currentPoint.X;
            }
        }

        private void DragLeftTopPoint(Point currentPoint)
        {
            var xOffset = currentPoint.X - _lastPoint.X;
            var yOffset = currentPoint.Y - _lastPoint.Y;

            var leftUpPointY = _originRect.Y + yOffset;
            var leftUpPointX = _originRect.X + xOffset;
            var height = _originRect.Height - yOffset;
            var width = _originRect.Width - xOffset;

            if (height >= 1 && leftUpPointY >= _textRectHeight + TopOffset)
            {
                _originRect.Height = height;
                _originRect.Y = leftUpPointY;
                _lastPoint.Y = currentPoint.Y;
            }
            if (width >= 1 && leftUpPointX >= _vTextRectWidth + LeftOffset)
            {
                _originRect.Width = width;
                _originRect.X = leftUpPointX;
                _lastPoint.X = currentPoint.X;
            }
        }

        private void DragTextRect(Point currentPoint)
        {
            var xOffset = currentPoint.X - _lastPoint.X;
            var yOffset = currentPoint.Y - _lastPoint.Y;
            var newLeftTopX = _originRect.X + xOffset;
            var newLeftTopY = _originRect.Y + yOffset;
            if (newLeftTopX >= _vTextRectWidth + LeftOffset && newLeftTopX <= Width + LeftOffset - _originRect.Width)
            {
                _originRect.X = newLeftTopX;
                _lastPoint.X = currentPoint.X;
            }
            if(newLeftTopY >= _textRectHeight + TopOffset && newLeftTopY <= Height + TopOffset - _originRect.Height)
            {
                _originRect.Y = newLeftTopY;
                _lastPoint.Y = currentPoint.Y;
            }
        }

        public override void DrawingMouseOver(Point currentPoint)
        {
            base.DrawingMouseOver(currentPoint);
            if(_originRect.Width == 0 && _originRect.Height == 0)
            {
                return;
            }
            Rect controlRect = new Rect(_originRect.X - 2, _originRect.Y - 2, _originRect.Width + 4, _originRect.Height + 4);
            if(controlRect.Contains(currentPoint))
            {
                DrawingRect(_originRect.TopLeft, _originRect.Height, _originRect.Width, true);
            }
            else
            {
                DrawingRect(_originRect.TopLeft, _originRect.Height, _originRect.Width, false);
            }
        }

        public void DrawingRect(Point leftTopPoint, double height, double width, bool isMeasuring = false)
        {
            DrawingChildren.Clear();
            DrawingTexts.Clear();
            string brushString = isMeasuring ? "#0081E4" : "#00000D";

            #region DrawRect
            Rect rect = new Rect(leftTopPoint.X, leftTopPoint.Y, width, height);
            _originRect = rect;
            Brush penBrush = (Brush)_brushConverter.ConvertFromString(brushString);
            Pen rectPen = new Pen(penBrush, 2);
            RectangleGeometry rectangleGeometry = new RectangleGeometry(rect);
            GeometryDrawing rectangleDrawing = new GeometryDrawing(Brushes.Transparent, rectPen, rectangleGeometry);
            DrawingChildren.Add(rectangleDrawing);
            #endregion

            #region DrawThumb
            if(isMeasuring)
            {
                var thumbSideLength = 4;
                Brush thumbBackground = Brushes.Black;
                Pen thumbPen = new Pen(thumbBackground, 1);
                #region 计算ThumbRect
                Rect leftTopRect = new Rect(leftTopPoint.X - thumbSideLength / 2, leftTopPoint.Y - thumbSideLength / 2, thumbSideLength, thumbSideLength);
                Rect leftCenterRect = new Rect(leftTopPoint.X - thumbSideLength / 2, leftTopPoint.Y + height / 2 - thumbSideLength / 2, thumbSideLength, thumbSideLength);
                Rect leftBottomRect = new Rect(leftTopPoint.X - thumbSideLength / 2, leftTopPoint.Y + height - thumbSideLength / 2, thumbSideLength, thumbSideLength);
                Rect centerTopRect = new Rect(leftTopPoint.X + width / 2 - thumbSideLength / 2, leftTopPoint.Y - thumbSideLength / 2, thumbSideLength, thumbSideLength);
                Rect centerBottomRect = new Rect(leftTopPoint.X + width / 2 - thumbSideLength / 2, leftTopPoint.Y + height - thumbSideLength / 2, thumbSideLength, thumbSideLength);
                Rect rightTopRect = new Rect(leftTopPoint.X + width - thumbSideLength / 2, leftTopPoint.Y - thumbSideLength / 2, thumbSideLength, thumbSideLength);
                Rect rightCenterRect = new Rect(leftTopPoint.X + width - thumbSideLength / 2, leftTopPoint.Y + height / 2 - thumbSideLength / 2, thumbSideLength, thumbSideLength);
                Rect rightBottomRect = new Rect(leftTopPoint.X + width - thumbSideLength / 2, leftTopPoint.Y + height - thumbSideLength / 2, thumbSideLength, thumbSideLength);
                _originLeftTopRect = leftTopRect;
                _originLeftCenterRect = leftCenterRect;
                _originLeftBottomRect = leftBottomRect;
                _originRightTopRect = rightTopRect;
                _originRightCenterRect = rightCenterRect;
                _originRightBottomRect = rightBottomRect;
                _originCenterTopRect = centerTopRect;
                _originCenterBottomRect = centerBottomRect;
                #endregion
                #region 创建Geometry
                RectangleGeometry leftTopThumbGeometry = new RectangleGeometry(leftTopRect);
                RectangleGeometry leftCenterThumbGeometry = new RectangleGeometry(leftCenterRect);
                RectangleGeometry leftBottomThumbGeometry = new RectangleGeometry(leftBottomRect);
                RectangleGeometry centerTopThumbGeometry = new RectangleGeometry(centerTopRect);
                RectangleGeometry centerBottomThumbGeometry = new RectangleGeometry(centerBottomRect);
                RectangleGeometry rightTopThumbGeometry = new RectangleGeometry(rightTopRect);
                RectangleGeometry rightCenterThumbGeometry = new RectangleGeometry(rightCenterRect);
                RectangleGeometry rightBottomThumbGeometry = new RectangleGeometry(rightBottomRect);
                #endregion
                #region 创建GeometryDrawing
                GeometryDrawing leftTopThumbDrawing = new GeometryDrawing(thumbBackground, thumbPen, leftTopThumbGeometry);
                GeometryDrawing leftCenterThumbDrawing = new GeometryDrawing(thumbBackground, thumbPen, leftCenterThumbGeometry);
                GeometryDrawing leftBottomThumbDrawing = new GeometryDrawing(thumbBackground, thumbPen, leftBottomThumbGeometry);
                GeometryDrawing centerTopThumbDrawing = new GeometryDrawing(thumbBackground, thumbPen, centerTopThumbGeometry);
                GeometryDrawing centerBottomThumbDrawing = new GeometryDrawing(thumbBackground, thumbPen, centerBottomThumbGeometry);
                GeometryDrawing rightTopThumbDrawing = new GeometryDrawing(thumbBackground, thumbPen, rightTopThumbGeometry);
                GeometryDrawing rightCenterThumbDrawing = new GeometryDrawing(thumbBackground, thumbPen, rightCenterThumbGeometry);
                GeometryDrawing rightBottomThumbDrawing = new GeometryDrawing(thumbBackground, thumbPen, rightBottomThumbGeometry);
                #endregion
                #region AddThumbDrawings
                DrawingChildren.Add(leftTopThumbDrawing);
                DrawingChildren.Add(leftCenterThumbDrawing);
                DrawingChildren.Add(leftBottomThumbDrawing);
                DrawingChildren.Add(centerTopThumbDrawing);
                DrawingChildren.Add(centerBottomThumbDrawing);
                DrawingChildren.Add(rightTopThumbDrawing);
                DrawingChildren.Add(rightCenterThumbDrawing);
                DrawingChildren.Add(rightBottomThumbDrawing);
                #endregion
            }
            #endregion

            #region DrawText
            Rect vTextRect = new Rect(leftTopPoint.X - _vTextRectWidth, leftTopPoint.Y + _textRectHeight, _vTextRectWidth, _textRectHeight);
            Rect tTextRect = new Rect(leftTopPoint.X, leftTopPoint.Y - _textRectHeight, _tTextRectWidth, _textRectHeight);
            _originVTextRect = vTextRect;
            _originTTextRect = tTextRect;
            Brush textRectBrush = (Brush)_brushConverter.ConvertFromString("#CC0F4983");
            Pen textRectPen = new Pen(textRectBrush, 1);
            RectangleGeometry vTextGeometry = new RectangleGeometry(vTextRect, 5, 5);
            RectangleGeometry tTextGeometry = new RectangleGeometry(tTextRect, 5, 5);
            GeometryDrawing vTextDrawing = new GeometryDrawing(textRectBrush, textRectPen, vTextGeometry);
            GeometryDrawing tTextDrawing = new GeometryDrawing(textRectBrush, textRectPen, tTextGeometry);
            DrawingChildren.Add(vTextDrawing);
            DrawingChildren.Add(tTextDrawing);

            FormattedText vText = new FormattedText(GetVText(), _culture, FlowDirection.LeftToRight, _typeface, _emSize, Brushes.White);
            FormattedText tText = new FormattedText(GetTText(), _culture, FlowDirection.LeftToRight, _typeface, _emSize, Brushes.White);
            var fontTextOffset = (_textRectHeight - _emSize) / 2;
            Point vTextPosition = new Point(vTextRect.Left + fontTextOffset, vTextRect.Top + fontTextOffset);
            Point tTextPosition = new Point(tTextRect.Left + fontTextOffset, tTextRect.Top + fontTextOffset);
            DrawingTexts.Add(new MaskText() { Text = vText, Position = vTextPosition });
            DrawingTexts.Add(new MaskText() { Text = tText, Position = tTextPosition });
            #endregion

            RenderMaskPaint();
        }

        private string GetTText()
        {
            return _originRect.Width + "ms (" + _originRect.Height + "bpm)";
        }

        private string GetVText()
        {
            return _originRect.Height + "uV";
        }

        public override void PrepareMask(Point current)
        {
            _lastPoint = current;
        }

        private void SetBoxLineMeterStatus(Point currentPoint)
        {
            if(_originRect.Contains(currentPoint))
            {
                _boxLineMeterStatus = BoxLineMeterStatusEnum.InnerRect;
            }
            else if(_originTTextRect.Contains(currentPoint))
            {
                _boxLineMeterStatus = BoxLineMeterStatusEnum.TText;
            }
            else if(_originVTextRect.Contains(currentPoint))
            {
                _boxLineMeterStatus = BoxLineMeterStatusEnum.VText;
            }
            else if(_originLeftTopRect.Contains(currentPoint))
            {
                _boxLineMeterStatus = BoxLineMeterStatusEnum.LeftTopThumb;
            }
            else if (_originLeftCenterRect.Contains(currentPoint))
            {
                _boxLineMeterStatus = BoxLineMeterStatusEnum.LeftCenterThumb;
            }
            else if (_originLeftBottomRect.Contains(currentPoint))
            {
                _boxLineMeterStatus = BoxLineMeterStatusEnum.LeftBottomThumb;
            }
            else if (_originRightTopRect.Contains(currentPoint))
            {
                _boxLineMeterStatus = BoxLineMeterStatusEnum.RightTopThumb;
            }
            else if (_originRightCenterRect.Contains(currentPoint))
            {
                _boxLineMeterStatus = BoxLineMeterStatusEnum.RightCenterThumb;
            }
            else if (_originRightBottomRect.Contains(currentPoint))
            {
                _boxLineMeterStatus = BoxLineMeterStatusEnum.RightBottomThumb;
            }
            else if (_originCenterTopRect.Contains(currentPoint))
            {
                _boxLineMeterStatus = BoxLineMeterStatusEnum.CenterTopThumb;
            }
            else if (_originCenterBottomRect.Contains(currentPoint))
            {
                _boxLineMeterStatus = BoxLineMeterStatusEnum.CenterBottomThumb;
            }
            else
            {
                _boxLineMeterStatus = BoxLineMeterStatusEnum.None;
            }
        }

        protected override Cursor SetMouseOverCursor(Point point)
        {
            SetBoxLineMeterStatus(point);
            Cursor result = Cursors.Arrow;
            switch (_boxLineMeterStatus)
            {
                case BoxLineMeterStatusEnum.VText:
                case BoxLineMeterStatusEnum.TText:
                    result = Cursors.SizeAll;
                    break;
                case BoxLineMeterStatusEnum.LeftTopThumb:
                    result = Cursors.SizeNWSE;
                    break;
                case BoxLineMeterStatusEnum.LeftCenterThumb:
                    result = Cursors.SizeWE;
                    break;
                case BoxLineMeterStatusEnum.LeftBottomThumb:
                    result = Cursors.SizeNESW;
                    break;
                case BoxLineMeterStatusEnum.RightTopThumb:
                    result = Cursors.SizeNESW;
                    break;
                case BoxLineMeterStatusEnum.RightCenterThumb:
                    result = Cursors.SizeWE;
                    break;
                case BoxLineMeterStatusEnum.RightBottomThumb:
                    result = Cursors.SizeNWSE;
                    break;
                case BoxLineMeterStatusEnum.CenterTopThumb:
                    result = Cursors.SizeNS;
                    break;
                case BoxLineMeterStatusEnum.CenterBottomThumb:
                    result = Cursors.SizeNS;
                    break;
            }
            return result;
        }

        public override void Dispose()
        {
            
        }

        public enum BoxLineMeterStatusEnum
        {
            None,
            VText,
            TText,
            LeftTopThumb,
            LeftCenterThumb,
            LeftBottomThumb,
            RightTopThumb,
            RightCenterThumb,
            RightBottomThumb,
            CenterTopThumb,
            CenterBottomThumb,
            InnerRect,
        }
    }
}
