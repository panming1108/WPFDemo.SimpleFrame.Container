using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WPFDemo.SimpleFrame.Infra.Messager;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public class AFAreaAction : MaskActionBase
    {
        private GeometryDrawing _afRectDrawing;
        private GeometryDrawing _startLineDrawing;
        private GeometryDrawing _endLineDrawing;

        private AFAreaStatusEnum _aFAreaStatus;
        private Tuple<double, double> _originAfAreaTuple;
        private double _lastPoint;

        private bool _isAfChanging;

        public AFAreaAction(double leftOffset, double topOffset) : base(leftOffset, topOffset)
        {
            MessagerInstance.GetMessager().Register<Tuple<double, double>>(this, MaskMessageKeyEnum.RenderAFMask, OnRenderAFMask);
            MessagerInstance.GetMessager().Register<string>(this, MaskMessageKeyEnum.StartDragArea, OnClearAfArea);
        }

        private Task OnRenderAFMask(Tuple<double, double> afArea)
        {
            if (DrawingChildren.Count > 0 && !_isAfChanging)
            {
                DrawingChildren.Clear();
                return TaskEx.FromResult(0);
            }
            DrawingAFMask(afArea);
            return TaskEx.FromResult(0);
        }

        private Task OnClearAfArea(string arg)
        {
            DrawingChildren.Clear();
            return TaskEx.FromResult(0);
        }

        private void DrawingAFMask(Tuple<double, double> afArea)
        {
            _originAfAreaTuple = afArea;

            var left = Math.Min(afArea.Item1, afArea.Item2);
            var right = Math.Max(afArea.Item1, afArea.Item2);
            DrawingChildren.Clear();

            Rect afRect = new Rect(left, TopOffset, right - left, Height);
            RectangleGeometry afRectGeometry = new RectangleGeometry(afRect);
            Brush afRectFill = (Brush)_brushConverter.ConvertFromString("#80007200");
            _afRectDrawing = new GeometryDrawing(afRectFill, new Pen(Brushes.Transparent, 0), afRectGeometry);

            Brush afPenStroke = (Brush)_brushConverter.ConvertFromString("#007200");
            LineGeometry startLineGeometry = new LineGeometry(new Point(left, TopOffset), new Point(left, TopOffset + Height));
            _startLineDrawing = new GeometryDrawing(afPenStroke, new Pen(afPenStroke, 4), startLineGeometry);
            LineGeometry endLineGeometry = new LineGeometry(new Point(right, TopOffset), new Point(right, TopOffset + Height));
            _endLineDrawing = new GeometryDrawing(afPenStroke, new Pen(afPenStroke, 4), endLineGeometry);

            DrawingChildren.Add(_afRectDrawing);
            DrawingChildren.Add(_startLineDrawing);
            DrawingChildren.Add(_endLineDrawing);
            _isAfChanging = false;
        }

        public override void Dispose()
        {
            MessagerInstance.GetMessager().Unregister<Tuple<double, double>>(this, MaskMessageKeyEnum.RenderAFMask, OnRenderAFMask);
            MessagerInstance.GetMessager().Unregister<string>(this, MaskMessageKeyEnum.StartDragArea, OnClearAfArea);
        }

        public override void DrawingDrag(Point currentPoint)
        {
            if(currentPoint.X < LeftOffset || currentPoint.X > LeftOffset + Width)
            {
                _lastPoint = currentPoint.X;
                return;
            }
            Tuple<double, double> result;
            var xOffSet = currentPoint.X - _lastPoint;
            double start;
            double end;
            switch (_aFAreaStatus)
            {
                case AFAreaStatusEnum.StartLine:
                    start = _originAfAreaTuple.Item1 + xOffSet;
                    end = _originAfAreaTuple.Item2;
                    result = new Tuple<double, double>(start, end);
                    DrawingAFMask(result);
                    _lastPoint = currentPoint.X;
                    break;
                case AFAreaStatusEnum.EndLine:
                    start = _originAfAreaTuple.Item1;
                    end = _originAfAreaTuple.Item2 + xOffSet;
                    result = new Tuple<double, double>(start, end);
                    DrawingAFMask(result);
                    _lastPoint = currentPoint.X;
                    break;
                default:
                    break;
            }
        }

        public override void DrawingMouseUp(Point currentPoint)
        {
            _isAfChanging = true;
            var beat = BeatMarkHelper.GetNearBeat(currentPoint.X);
            double start, end;
            if(_aFAreaStatus == AFAreaStatusEnum.StartLine)
            {
                start = Math.Min(_originAfAreaTuple.Item2, beat);
                end = Math.Max(_originAfAreaTuple.Item2, beat);
                MessagerInstance.GetMessager().Send(MaskMessageKeyEnum.RenderAFMask, new Tuple<double, double>(start, end));
            }
            else if(_aFAreaStatus == AFAreaStatusEnum.EndLine)
            {
                start = Math.Min(_originAfAreaTuple.Item1, beat);
                end = Math.Max(_originAfAreaTuple.Item1, beat);
                MessagerInstance.GetMessager().Send(MaskMessageKeyEnum.RenderAFMask, new Tuple<double, double>(start, end));
            }          
        }

        public override void PrepareMask(Point current)
        {
            _lastPoint = current.X;
        }

        public override Cursor GetMouseOverCursor(Point currentPoint)
        {
            SetAfAreaStatus(currentPoint);
            Cursor cursor = Cursors.Arrow;
            switch (_aFAreaStatus)
            {
                case AFAreaStatusEnum.StartLine:
                case AFAreaStatusEnum.EndLine:
                    cursor = Cursors.SizeWE;
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
            else
            {
                _aFAreaStatus = AFAreaStatusEnum.Rect;
            }
        }

        private enum AFAreaStatusEnum
        {
            None,
            StartLine,
            EndLine,
            Rect,
        }
    }
}
