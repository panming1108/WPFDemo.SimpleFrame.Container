using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public class BeatInfoCache
    {
        public static List<BeatInfo> GetBeats()
        {
            string[] data = new string[] { "U", "N", "Af" };
            List<BeatInfo> beats = new List<BeatInfo>();
            for (double i = 50; i < 2000; i += 100)
            {
                beats.Add(new BeatInfo()
                {
                    BeatType = data[(int)i % 3],
                    Position = i,
                    Interval = 100,
                });
            }
            return beats;
        }
    }

    public class BeatInfo
    {
        public string BeatType { get; set; }
        public double Position { get; set; }
        public double Interval { get; set; }
    }
}
