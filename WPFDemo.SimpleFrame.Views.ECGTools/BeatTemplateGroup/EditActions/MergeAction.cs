using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public class MergeAction : IWorkAction
    {
        private Point _mouseDownPoint;
        private BeatTemplateItemView _startItemView;
        private BeatTemplateGroupItemView _endBeatTemplateGroupItemView;
        private readonly BeatTemplateGroupView _groupView;
        private readonly ISelectMaskPaint _selectMaskPaint;

        public BeatTemplateItemView _currentMoveItemView;

        public event EventHandler<AddCategoryEventArgs> CategoryAdded;
        public event EventHandler<MergeTemplateEventArgs> TemplateMerged;

        public MergeAction(BeatTemplateGroupView groupView, ISelectMaskPaint selectMaskPaint)
        {
            _groupView = groupView;
            _selectMaskPaint = selectMaskPaint;
        }

        public void Click()
        {
            
        }

        public bool Draging(Point currentPoint)
        {
            if (_startItemView == null || _mouseDownPoint == currentPoint)
            {
                return false;
            }

            RenderDragItemShadow(_startItemView, currentPoint);

            if (_currentMoveItemView != null)
            {
                _currentMoveItemView.IsPrepareMerge = true;
            }
            else
            {
                foreach (var item in _groupView.GroupItems)
                {
                    var groupItemView = item as BeatTemplateGroupItemView;
                    var result = groupItemView.IsBeatTemplateGroupItemHeader(currentPoint, out _endBeatTemplateGroupItemView);
                    if (result)
                    {
                        break;
                    }
                }
            }
            return true;
        }

        public void DragOver()
        {
            RenderDragItemShadow(null, new Point());

            if (_currentMoveItemView != null)
            {
                _currentMoveItemView.IsPrepareMerge = false;
                OnMergeBeatTemplate(_currentMoveItemView, _startItemView);
            }
            else
            {
                if (_endBeatTemplateGroupItemView == null)
                {
                    return;
                }
                OnAddCategory(_endBeatTemplateGroupItemView, _startItemView);
                _endBeatTemplateGroupItemView = null;
            }
        }

        private void OnAddCategory(BeatTemplateGroupItemView endBeatTemplateGroupItemView, BeatTemplateItemView startItemView)
        {
            if (startItemView.GroupItemView == endBeatTemplateGroupItemView)
            {
                return;
            }
            CategoryAdded?.Invoke(this, new AddCategoryEventArgs(endBeatTemplateGroupItemView.Id, startItemView.Id));
        }

        private void OnMergeBeatTemplate(BeatTemplateItemView currentMoveItemView, BeatTemplateItemView startItemView)
        {
            if (currentMoveItemView == startItemView)
            {
                return;
            }
            TemplateMerged?.Invoke(this, new MergeTemplateEventArgs(currentMoveItemView.Id, startItemView.Id));
        }

        public void MouseDown(Point currentPoint)
        {
            _mouseDownPoint = currentPoint;
            _startItemView = null;
            foreach (var item in _groupView.GroupItems)
            {
                var groupItemView = item as BeatTemplateGroupItemView;
                var result = groupItemView.IsBeatTemplateItemView(currentPoint, out _startItemView);
                if (result)
                {
                    break;
                }
            }
        }

        private void RenderDragItemShadow(BeatTemplateItemView startItemView, Point currentPoint)
        {
            _selectMaskPaint.DrawingHandler(drawingContext =>
            {
                if(startItemView == null)
                {
                    return;
                }
                drawingContext.PushOpacity(0.6);
                Rect rect = new Rect(currentPoint.X - startItemView.ActualWidth / 4, currentPoint.Y - startItemView.ActualHeight / 4, startItemView.ActualWidth / 2, startItemView.ActualHeight / 2);
                drawingContext.DrawRectangle(new VisualBrush(startItemView), new Pen(Brushes.Transparent, 0), rect);
            });
        }
    }
}
