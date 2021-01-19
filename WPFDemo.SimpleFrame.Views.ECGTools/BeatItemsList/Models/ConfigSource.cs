using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
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
