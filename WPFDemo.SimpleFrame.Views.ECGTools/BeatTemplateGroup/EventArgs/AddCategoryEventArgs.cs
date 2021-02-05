using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public class AddCategoryEventArgs : EventArgs
    {
        public AddCategoryEventArgs(string targetBeatTemplateGroupItemViewId, string originItemViewId)
        {
            TargetBeatTemplateGroupItemViewId = targetBeatTemplateGroupItemViewId;
            OriginItemViewId = originItemViewId;
        }

        public string TargetBeatTemplateGroupItemViewId { get; set; }
        public string OriginItemViewId { get; set; }

        
    }
}
