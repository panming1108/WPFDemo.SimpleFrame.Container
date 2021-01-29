using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using WPFDemo.SimpleFrame.Infra.Helper;
using WPFDemo.SimpleFrame.Infra.Messager;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public class SBeatTemplateGroupItemView : BeatTemplateGroupItemView
    {
        private readonly List<string> _formSelectedItemsIds = new List<string>();
        private readonly List<string> _eventSelectedItemsIds = new List<string>();
        private readonly IList _eventSource;
        public IList EventSource => _eventSource;
        public SBeatTemplateGroupItemView(string id, IList formSource, IList eventSource, BeatTemplateGroupView groupView) : base(id, formSource, groupView)
        {
            _eventSource = eventSource;
            PART_FormRadioBtn.Visibility = Visibility.Visible;
            PART_EventRadioBtn.Visibility = Visibility.Visible;
            if (GroupView.IsAtrialPattern)
            {
                PART_FormRadioBtn.IsChecked = true;
            }
            else
            {
                PART_EventRadioBtn.IsChecked = true;
            }
            PART_FormRadioBtn.Checked += PART_FormEventRadioBtn_Checked;
            PART_EventRadioBtn.Checked += PART_FormEventRadioBtn_Checked;
            Unloaded += SBeatTemplateGroupItemView_Unloaded;
        }

        public override void SetGroupItemItemsSource(IList<string> selectedIds)
        {
            List<string> tempIds = GroupView.IsAtrialPattern ? _formSelectedItemsIds : _eventSelectedItemsIds;
            tempIds.Clear();
            foreach (var item in selectedIds)
            {
                tempIds.Add(item);
            }
            SetGroupItemItemsSource(GroupView.IsAtrialPattern);
        }

        private void SetGroupItemItemsSource(bool isAtrialPattern)
        {
            if (isAtrialPattern)
            {
                SetGroupItemItemsSource(FormSource, _formSelectedItemsIds);//重置显示
            }
            else
            {
                SetGroupItemItemsSource(EventSource, _eventSelectedItemsIds);//重置显示
            }
        }

        private void PART_FormEventRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            var isAtrialPattern = PART_FormRadioBtn.IsChecked == true;//形态被选中
            ResetSelectedItemsIds(isAtrialPattern);
            GroupView.ResetIsAtrialPattern(isAtrialPattern, Items);
            SetGroupItemItemsSource(isAtrialPattern);
        }

        private void ResetSelectedItemsIds(bool isAtrialPattern)
        {
            List<string> tempIds = isAtrialPattern ? _eventSelectedItemsIds : _formSelectedItemsIds;
            tempIds.Clear();
            foreach (var item in Items)
            {
                var itemView = item as BeatTemplateItemView;
                if(itemView.IsSelected)
                {
                    tempIds.Add(itemView.Id);
                }
            }
        }

        private void SBeatTemplateGroupItemView_Unloaded(object sender, RoutedEventArgs e)
        {
            PART_FormRadioBtn.Checked -= PART_FormEventRadioBtn_Checked;
            PART_EventRadioBtn.Checked -= PART_FormEventRadioBtn_Checked;
            Unloaded -= SBeatTemplateGroupItemView_Unloaded;
        }
    }
}
