using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public class ParentBeatTemplate
    {
        public string Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryNameEn { get; set; }
        public int BeatType { get; set; }
        public int Count { get; set; }
        public double Percent { get; set; }
    }
}
