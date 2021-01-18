using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public static class ConfigSource
    {
        public static Dictionary<string, string> ConfigDic = new Dictionary<string, string>();

        static ConfigSource()
        {
            ConfigDic.Add("ColumnCount", "6");
            ConfigDic.Add("RowCount", "5");
        }
    }
}
