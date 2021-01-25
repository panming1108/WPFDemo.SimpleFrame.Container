using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public static class ConfigSource
    {
        public static double ItemSmallHeight => 122;
        public static double ItemBigHeight => 150;
        public static double ItemBigWidth => 240;
        public static double ItemSmallWidth => 122;
        public static bool RSortAsc => true;
        public static bool IntervalSortAsc => true;
        public static SortEnum DefaultSort => SortEnum.IntervalSort;
    }
}
