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
            for (int i = 0; i < 200; i++)
            {
                var beatType = data[(int)i % 3];
                if( i >= 7 && i < 11)
                {
                    beatType = data[2];
                }
                beats.Add(new BeatInfo()
                {
                    BeatTypeName = beatType,
                    Position = 50 + i * 100,
                    Interval = 100,
                });
            }
            return beats;
        }
    }

    public class BeatInfo
    {
        public string BeatTypeName { get; set; }
        private int _beatType;
        public int BeatType 
        {
            get => _beatType;
            set
            {
                _beatType = value;
                BeatTypeName = ((BeatTypeEnum)_beatType).ToString();
            }
        }
        public int SubType { get; set; }
        public double Position { get; set; }
        public double Interval { get; set; }
        public int R { get; set; }
        public string BeatReferId { get; set; }
        public double[] Data { get; set; }
    }
}
