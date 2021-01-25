using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public class BeatTemplate
    {
        public string TypeName { get; set; }
        public int Count { get; set; }
        public bool IsAdd { get; set; }
        public bool IsChecked { get; set; }
    }
}
