using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
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

        public static Dictionary<int, Color> PenColors => new Dictionary<int, Color>() 
        { 
            { (int)BeatTypeEnum.N, Colors.Black } ,
            { (int)BeatTypeEnum.Q, Colors.Gray } ,
            { (int)BeatTypeEnum.S, Colors.Blue } ,
            { (int)BeatTypeEnum.V, Colors.Red } ,
            { (int)BeatTypeEnum.X, Colors.Green } ,
        };
    }
}
