﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public class BeatTemplate
    {
        public string Id { get; set; }
        public string ParentID { get; set; }

        public int ParentCount { get; set; }
        public string ParentCategoryEn { get; set; }

        public string ParentCategoryName { get; set; }

        public string CategoryEn { get; set; }

        public string CategoryName { get; set; }

        public int BeatType { get; set; }

        public int SubType { get; set; }

        public int DataCount { get; set; }

        public double Percent { get; set; }

        public bool IsAdd { get; set; }

        public bool IsChecked { get; set; }

        public bool IsEvent { get; set; }

        public ObservableCollection<PixelPointArrayEx> WaveList { get; set; }
    }
}
