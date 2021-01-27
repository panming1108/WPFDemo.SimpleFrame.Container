using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public class MergeTemplateEventArgs : EventArgs
    {
        public MergeTemplateEventArgs(BeatTemplateItemView targetBeatTemplateItemView, BeatTemplateItemView originBeatTemplateItemView)
        {
            TargetBeatTemplateItemView = targetBeatTemplateItemView;
            OriginBeatTemplateItemView = originBeatTemplateItemView;
        }

        public BeatTemplateItemView TargetBeatTemplateItemView { get; set; }
        public BeatTemplateItemView OriginBeatTemplateItemView { get; set; }


    }
}
