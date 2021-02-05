using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public class MergeTemplateEventArgs : EventArgs
    {
        public MergeTemplateEventArgs(string targetBeatTemplateItemViewId, string originBeatTemplateItemViewId)
        {
            TargetBeatTemplateItemViewId = targetBeatTemplateItemViewId;
            OriginBeatTemplateItemViewId = originBeatTemplateItemViewId;
        }

        public string TargetBeatTemplateItemViewId { get; set; }
        public string OriginBeatTemplateItemViewId { get; set; }


    }
}
