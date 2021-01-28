using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using WPFDemo.SimpleFrame.Infra.Messager;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public class SBeatTemplateGroupItemView : BeatTemplateGroupItemView
    {
        public SBeatTemplateGroupItemView(BeatTemplateGroupView groupView) : base(groupView)
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
            var btn = sender as RadioButton;
            MessagerInstance.GetMessager().Send("PopupNotifyBox", new PopupNotifyObject("通知", "切换成" + btn.Name));
        }

        private void SBeatTemplateGroupItemView_Unloaded(object sender, RoutedEventArgs e)
        {
            PART_FormRadioBtn.Checked -= PART_FormEventRadioBtn_Checked;
            PART_EventRadioBtn.Checked -= PART_FormEventRadioBtn_Checked;
            Unloaded -= SBeatTemplateGroupItemView_Unloaded;
        }
    }
}
