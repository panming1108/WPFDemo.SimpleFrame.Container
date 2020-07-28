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
    public class BoxLineMeterAction : MaskActionBase
    {
        private BoxLineMeterStatusEnum _boxLineMeterStatus;
        private Point _lastPoint;
        private double _vTextRectWidth = 75;
        private double _tTextRectWidth = 150;
        private double _textRectHeight = 20;

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
        public override void DrawingDrag(Point currentPoint)
        {           
            switch (_boxLineMeterStatus)
            {
                case BoxLineMeterStatusEnum.VText:
                case BoxLineMeterStatusEnum.TText:
                    DragTextRect(currentPoint);
                    break;
                case BoxLineMeterStatusEnum.LeftTopThumb:
                    break;
                case BoxLineMeterStatusEnum.LeftCenterThumb:
                    break;
                case BoxLineMeterStatusEnum.LeftBottomThumb:
                    break;
                case BoxLineMeterStatusEnum.RightTopThumb:
                    break;
                case BoxLineMeterStatusEnum.RightCenterThumb:
                    break;
                case BoxLineMeterStatusEnum.RightBottomThumb:
                    break;
                case BoxLineMeterStatusEnum.CenterTopThumb:
                    break;
                case BoxLineMeterStatusEnum.CenterBottomThumb:
                    break;
            }
            _lastPoint = currentPoint;
        }

        private void DragTextRect(Point currentPoint)
        {
            var xOffset = currentPoint.X - _lastPoint.X;
            var yOffset = currentPoint.Y - _lastPoint.Y;
            _originRect.X += xOffset;
            _originRect.Y += yOffset;
            if (_originRect.TopLeft.X <= _vTextRectWidth)
            {
                _originRect.X = _vTextRectWidth;
            }
            if (_originRect.TopLeft.X >= Width - _originRect.Width)
            {
                _originRect.X = Width - _originRect.Width;
            }
            if (_originRect.TopLeft.Y <= _textRectHeight)
            {
                _originRect.Y = _textRectHeight;
            }
            if (_originRect.TopLeft.Y >= Height - _originRect.Height)
            {
                _originRect.Y = Height - _originRect.Height;
            }
            DrawingRect(_originRect.TopLeft, _originRect.Height, _originRect.Width);
        }

        public override void DrawingMouseUp(Point currentPoint)
        {
            
        }

        public void DrawingMouseMove(Point currentPoint)
        {
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
            DrawingCollection drawings = new DrawingCollection();
            List<MaskText> maskTexts = new List<MaskText>();
            string brushString = isMeasuring ? "#0081E4" : "#00000D";
            BrushConverter brushConverter = new BrushConverter();

            #region DrawRect
            Rect rect = new Rect(leftTopPoint.X, leftTopPoint.Y, width, height);
            _originRect = rect;
            Brush penBrush = (Brush)brushConverter.ConvertFromString(brushString);
            Pen rectPen = new Pen(penBrush, 2);
            RectangleGeometry rectangleGeometry = new RectangleGeometry(rect);
            GeometryDrawing rectangleDrawing = new GeometryDrawing(Brushes.Transparent, rectPen, rectangleGeometry);
            drawings.Add(rectangleDrawing);
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
                drawings.Add(leftTopThumbDrawing);
                drawings.Add(leftCenterThumbDrawing);
                drawings.Add(leftBottomThumbDrawing);
                drawings.Add(centerTopThumbDrawing);
                drawings.Add(centerBottomThumbDrawing);
                drawings.Add(rightTopThumbDrawing);
                drawings.Add(rightCenterThumbDrawing);
                drawings.Add(rightBottomThumbDrawing);
                #endregion
            }
            #endregion

            #region DrawText
            Rect vTextRect = new Rect(leftTopPoint.X - _vTextRectWidth, leftTopPoint.Y + _textRectHeight, _vTextRectWidth, _textRectHeight);
            Rect tTextRect = new Rect(leftTopPoint.X, leftTopPoint.Y - _textRectHeight, _tTextRectWidth, _textRectHeight);
            _originVTextRect = vTextRect;
            _originTTextRect = tTextRect;
            Brush textRectBrush = (Brush)brushConverter.ConvertFromString("#CC0F4983");
            Pen textRectPen = new Pen(textRectBrush, 1);
            RectangleGeometry vTextGeometry = new RectangleGeometry(vTextRect, 5, 5);
            RectangleGeometry tTextGeometry = new RectangleGeometry(tTextRect, 5, 5);
            GeometryDrawing vTextDrawing = new GeometryDrawing(textRectBrush, textRectPen, vTextGeometry);
            GeometryDrawing tTextDrawing = new GeometryDrawing(textRectBrush, textRectPen, tTextGeometry);
            drawings.Add(vTextDrawing);
            drawings.Add(tTextDrawing);

            FormattedText vText = new FormattedText(GetVText(), _culture, FlowDirection.LeftToRight, _typeface, _emSize, Brushes.White);
            FormattedText tText = new FormattedText(GetTText(), _culture, FlowDirection.LeftToRight, _typeface, _emSize, Brushes.White);
            var fontTextOffset = (_textRectHeight - _emSize) / 2;
            Point vTextPosition = new Point(vTextRect.Left + fontTextOffset, vTextRect.Top + fontTextOffset);
            Point tTextPosition = new Point(tTextRect.Left + fontTextOffset, tTextRect.Top + fontTextOffset);
            maskTexts.Add(new MaskText() { Text = vText, Position = vTextPosition });
            maskTexts.Add(new MaskText() { Text = tText, Position = tTextPosition });
            #endregion
            DrawingTexts = maskTexts;
            DrawingChildren = drawings;
        }

        private string GetTText()
        {
            return _originRect.Width + "ms (" + _originRect.Height + "bpm)";
        }

        private string GetVText()
        {
            return _originRect.Height + "uV";
        }

        public override void PrepareMask(Point current, double height, double width)
        {
            Height = height;
            Width = width;
            _lastPoint = current;
        }

        public override void ResetMask()
        {
            
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

        public override Cursor GetMouseOverCursor(Point point)
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
