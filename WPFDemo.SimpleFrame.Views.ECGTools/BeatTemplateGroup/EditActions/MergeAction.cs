using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public class MergeAction : IWorkAction
    {
        private Point _mouseDownPoint;
        private BeatTemplateItemView _startItemView;
        private BeatTemplateGroupItemView _endBeatTemplateGroupItemView;
        private DragDropAdorner _tempAdorner;
        private readonly BeatTemplateGroupView _groupView;

        private AdornerLayer GroupViewAdornerLayer => AdornerLayer.GetAdornerLayer(_groupView);
        public BeatTemplateItemView _currentMoveItemView;

        public event EventHandler<AddCategoryEventArgs> CategoryAdded;
        public event EventHandler<MergeTemplateEventArgs> TemplateMerged;

        public MergeAction(BeatTemplateGroupView groupView)
        {
            _groupView = groupView;
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
            if (_tempAdorner != null)
            {
                GroupViewAdornerLayer.Remove(_tempAdorner);
            }
            _tempAdorner = new DragDropAdorner(_startItemView);
            GroupViewAdornerLayer.Add(_tempAdorner);

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
            if (_tempAdorner != null)
            {
                GroupViewAdornerLayer.Remove(_tempAdorner);
                _tempAdorner = null;
            }

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
            CategoryAdded?.Invoke(this, new AddCategoryEventArgs(endBeatTemplateGroupItemView, startItemView));
        }

        private void OnMergeBeatTemplate(BeatTemplateItemView currentMoveItemView, BeatTemplateItemView startItemView)
        {
            if (currentMoveItemView == startItemView)
            {
                return;
            }
            TemplateMerged?.Invoke(this, new MergeTemplateEventArgs(currentMoveItemView, startItemView));
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
    }
}
