using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPFDemo.SimpleFrame.Infra.Messager;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public class BeatMarkAction : MaskActionBase
    {
        private double _mouseUpPointX;
        private readonly bool _canClick;
        private double _selectBeat;
        private double SelectBeat
        {
            get => _selectBeat;
            set
            {
                _selectBeat = value;
                DrawingSelectedBeatBar(_selectBeat);
                DrawingBeatMarkMask();
            }
        }
        private double _mouseOverBeat;
        private double MouseOverBeat
        {
            get => _mouseOverBeat;
            set
            {
                _mouseOverBeat = value;
                DrawingBeatMarkMask();
            }
        }
        private readonly double _beatRectHeight = 22;
        private readonly double _beatRectWidth = 30;

        private readonly CultureInfo _culture = CultureInfo.GetCultureInfo("en-us");

        private DrawingCollection _beatDrawings = new DrawingCollection();
        private DrawingCollection _lineDrawings = new DrawingCollection();

        private string[] _rectContextMenu = new string[] { "正常", "房颤", "房早", "删除心搏" };
        private string[] _lineContextMenu = new string[] { "添加典型图", "设置为最快心率", "设置为最慢心率", "标记开始位置" };

        public BeatMarkAction(bool canClick, double leftOffset, double topOffset) : base(leftOffset, topOffset)
        {
            _canClick = canClick;
            MessagerInstance.GetMessager().Register<string>(this, MaskMessageKeyEnum.StartDragArea, OnStartDragArea);
            MessagerInstance.GetMessager().Register<double>(this, MaskMessageKeyEnum.DragAreaMouseUp, OnDragAreaMouseUp);
        }

        private Task OnDragAreaMouseUp(double currentX)
        {
            _mouseUpPointX = currentX;
            SelectBeat = BeatMarkHelper.GetCurrentBeat(currentX);
            return TaskEx.FromResult(0);
        }

        private async Task OnStartDragArea(string arg)
        {
            _selectBeat = 0;
            _lineDrawings.Clear();
            DrawingBeatMarkMask();
            await TaskEx.FromResult(0);
        }

        public override void DrawingMouseOver(Point currentPoint)
        {
            if (currentPoint.Y < TopOffset || currentPoint.Y > TopOffset + _beatRectHeight)
            {
                MouseOverBeat = 0;
            }
            else
            {
                MouseOverBeat = BeatMarkHelper.GetCurrentBeat(currentPoint.X);
            }
        }
        public void DrawingBeatMarkMask()
        {
            DrawingBeatMark();
            DrawingCollection drawings = new DrawingCollection();
            foreach (var item in _beatDrawings)
            {
                drawings.Add(item);
            }
            foreach (var item in _lineDrawings)
            {
                drawings.Add(item);
            }
            DrawingChildren = drawings;
        }

        private void DrawingBeatMark()
        {
            _beatDrawings.Clear();
            DrawingTexts.Clear();
            for (int i = 0; i < BeatInfoCache.GetBeats().Count; i++)
            {
                var item = BeatInfoCache.GetBeats()[i];
                if(item.Position < LeftOffset || item.Position > Width + LeftOffset)
                {
                    continue;
                }
                #region 画心搏边框
                Rect beatRect = new Rect(LeftOffset + item.Position - _beatRectWidth / 2, TopOffset, _beatRectWidth, _beatRectHeight);
                Brush beatRectBackground = (Brush)_brushConverter.ConvertFromString("#EBFAFF");
                Brush beatRectStroke = (Brush)_brushConverter.ConvertFromString("#DBE0E3");
                if (_canClick && (item.Position == _selectBeat || item.Position == _mouseOverBeat))
                {
                    beatRectStroke = (Brush)_brushConverter.ConvertFromString("#5CBBF3");
                }
                Pen beatRectPen = new Pen(beatRectStroke, 2);
                RectangleGeometry beatGeometry = new RectangleGeometry(beatRect);
                GeometryDrawing beatRectDrawing = new GeometryDrawing(beatRectBackground, beatRectPen, beatGeometry);
                _beatDrawings.Add(beatRectDrawing);
                #endregion

                #region 画心搏类型文字
                Typeface beatTypeFace = new Typeface(new FontFamily("Klavika"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
                Brush beatTypeForground = (Brush)_brushConverter.ConvertFromString("#299EE3");
                FormattedText beatTypeText = new FormattedText(item.BeatType, _culture, FlowDirection.LeftToRight, beatTypeFace, 15d, beatTypeForground);
                double beatTypeTextXOffset = beatRect.X + (beatRect.Width - beatTypeText.Width) / 2;
                double beatTypeTextYOffset = beatRect.Y + (beatRect.Height - beatTypeText.Height) / 2;
                Point beatTypePosition = new Point(beatTypeTextXOffset, beatTypeTextYOffset);
                DrawingTexts.Add(new MaskText() { Text = beatTypeText, Position = beatTypePosition });
                #endregion

                if (i > 0)
                {
                    var prevItem = BeatInfoCache.GetBeats()[i - 1];
                    Typeface beatAndInternalFace = new Typeface(new FontFamily("Klavika"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
                    #region 画心率文字
                    FormattedText beatText = new FormattedText(item.Position.ToString(), _culture, FlowDirection.LeftToRight, beatAndInternalFace, 10d, Brushes.Black);
                    double beatTextXOffset = prevItem.Position + (item.Position - prevItem.Position - beatTypeText.Width) / 2;
                    double beatTextYOffset = beatRect.Y + beatRect.Width  / 2;
                    Point beatPosition = new Point(beatTextXOffset, beatTextYOffset);
                    DrawingTexts.Add(new MaskText() { Text = beatText, Position = beatPosition });
                    #endregion
                    #region 画间期文字
                    FormattedText beatIntervalText = new FormattedText(item.Interval.ToString(), _culture, FlowDirection.LeftToRight, beatAndInternalFace, 10d, Brushes.Black);
                    double beatIntervalTextXOffset = prevItem.Position + (item.Position - prevItem.Position - beatTypeText.Width) / 2 + (beatText.Width - beatIntervalText.Width) / 2;
                    double beatIntervalTextYOffset = beatRect.Y + beatRect.Width / 2 + beatText.Height + 5;
                    Point beatIntervalPosition = new Point(beatIntervalTextXOffset, beatIntervalTextYOffset);
                    DrawingTexts.Add(new MaskText() { Text = beatIntervalText, Position = beatIntervalPosition });
                    #endregion
                }
            }
        }

        private void DrawingSelectedBeatBar(double beat)
        {
            if (!_canClick)
            {
                return;
            }
            _lineDrawings.Clear();
            if (beat != 0)
            {
                //画诊断图选中条
                Rect beatRect = new Rect(LeftOffset + beat - _beatRectWidth / 2, TopOffset, _beatRectWidth, _beatRectHeight);
                Rect selectRect = new Rect(beatRect.X, beatRect.Y + beatRect.Height + 8, beatRect.Width, Height - beatRect.Height - 8);
                RectangleGeometry selectRectGeometry = new RectangleGeometry(selectRect);
                Brush selectRectBackground = (Brush)_brushConverter.ConvertFromString("#33B260FF");
                GeometryDrawing selectRectDrawing = new GeometryDrawing(selectRectBackground, new Pen(Brushes.Transparent, 0), selectRectGeometry);
                _lineDrawings.Add(selectRectDrawing);
            }
            else
            {
                //画黄线
                LineGeometry lineGeometry = new LineGeometry(new Point(_mouseUpPointX, TopOffset + _beatRectHeight + 8), new Point(_mouseUpPointX, Height + TopOffset));
                GeometryDrawing lineDrawing = new GeometryDrawing(Brushes.Orange, new Pen(Brushes.Orange, 1), lineGeometry);
                _lineDrawings.Add(lineDrawing);
            }
        }
        public override void DrawingMouseDoubleClick(Point currentPoint)
        {
            if(MouseOverBeat == 0)
            {
                return;
            }
            var afArea = BeatMarkHelper.GetAfArea(MouseOverBeat);
            if(afArea.Item1 == afArea.Item2)
            {
                return;
            }
            MessagerInstance.GetMessager().Send(MaskMessageKeyEnum.RenderAFMask, afArea);
        }

        public override void RenderMaskSize(double height, double width)
        {
            base.RenderMaskSize(height, width);
            _lineDrawings.Clear();
            _beatDrawings.Clear();
            DrawingBeatMarkMask();
        }

        public override void PrepareMask(Point current)
        {
            _mouseUpPointX = current.X;
            SelectBeat = BeatMarkHelper.GetCurrentBeat(current.X);
        }

        public override void ResetMask()
        {
            _selectBeat = 0;           
        }

        public override void DrawingMouseRightButtonDown(Point currentPoint)
        {
            base.DrawingMouseRightButtonDown(currentPoint);
            SelectBeat = BeatMarkHelper.GetCurrentBeat(currentPoint.X);
        }

        protected override IEnumerable SetContextMenuItems(Point currentPoint)
        {
            var beat = BeatMarkHelper.GetCurrentBeat(currentPoint.X);
            if(beat == 0)
            {
                return _lineContextMenu;
            }
            else
            {
                return _rectContextMenu;
            }
        }

        public override void Dispose()
        {
            MessagerInstance.GetMessager().Unregister<string>(this, MaskMessageKeyEnum.StartDragArea, OnStartDragArea);
            MessagerInstance.GetMessager().Unregister<double>(this, MaskMessageKeyEnum.DragAreaMouseUp, OnDragAreaMouseUp);
        }
    }
}
