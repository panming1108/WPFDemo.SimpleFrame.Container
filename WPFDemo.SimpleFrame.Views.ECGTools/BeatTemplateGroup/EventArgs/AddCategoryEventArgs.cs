using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public class AddCategoryEventArgs : EventArgs
    {
        public AddCategoryEventArgs(BeatTemplateGroupItemView targetBeatTemplateGroupItemView, BeatTemplateItemView originItemView)
        {
            TargetBeatTemplateGroupItemView = targetBeatTemplateGroupItemView;
            OriginItemView = originItemView;
        }

        public BeatTemplateGroupItemView TargetBeatTemplateGroupItemView { get; set; }
        public BeatTemplateItemView OriginItemView { get; set; }

        
    }
}
