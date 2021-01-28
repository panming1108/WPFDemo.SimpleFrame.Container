using System;
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
        public SBeatTemplateGroupItemView(string id, BeatTemplateGroupView groupView) : base(id, groupView)
        {
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

        private void PART_FormEventRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            var isAtrialPattern = PART_FormRadioBtn.IsChecked == true;//形态被选中
            ResetSelectedItemsIds(isAtrialPattern);
            //TODO 对数据进行处理，房性事件形态切换，得到新的数据源
            var source = new List<BeatTemplate>();
            for (int j = 0; j < 3; j++)
            {
                BeatTemplate beatTemplate = new BeatTemplate() { Id = Guid.NewGuid().ToString(), CategoryName = ((BeatTypeEnum)(j % 5)).GetDescription() };
                source.Add(beatTemplate);
            }
            SetGroupItemItemsSource(source, isAtrialPattern ? _formSelectedItemsIds : _eventSelectedItemsIds);//重置显示
        }

        private void ResetSelectedItemsIds(bool isAtrialPattern)
        {
            GroupView.ResetIsAtrialPattern(isAtrialPattern, Items);
            List<string> tempIds = isAtrialPattern ? _eventSelectedItemsIds : _formSelectedItemsIds;
            tempIds.Clear();
            foreach (var item in Items)
            {
                var itemView = item as BeatTemplateItemView;
                tempIds.Add(itemView.Id);
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
