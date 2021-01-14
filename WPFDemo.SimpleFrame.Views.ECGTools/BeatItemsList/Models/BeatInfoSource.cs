using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class BeatInfoSource
    {
        public static string[] beatTypes = new string[] { "N", "S", "V" };
        public static IList<BeatInfo> AllBeatInfos { get; set; }
        static BeatInfoSource()
        {
            AllBeatInfos = GetAllBeatInfos();
        }

        public static IList<BeatInfo> GetAllBeatInfos(int count = 200000)
        {
            Random random = new Random();
            var results = new List<BeatInfo>();
            for (int i = 0; i < count; i++)
            {
                BeatInfo beatInfo = new BeatInfo()
                {
                    BeatType = beatTypes[i % 3],
                    Position = i,
                    R = i,
                    Interval = random.Next(0, 2000),
                    Data = GetECGData(random)
                };
                results.Add(beatInfo);
            }
            return results;
        }
        
        public static void ChangedBeatInfo(List<BeatInfo> beatInfos, IList beatInfoRs, string type)
        {
            foreach (var item in beatInfoRs)
            {
                var beatInfo = (BeatInfo)item;
                beatInfos[beatInfos.IndexOf(beatInfo)].BeatType = type;
            }
        }

        public static void DeleteBeatInfos(List<BeatInfo> beatInfos, IList beatInfoRs)
        {            
            foreach (var item in beatInfoRs)
            {
                var beatInfo = (BeatInfo)item;
                beatInfos.Remove(beatInfo);
            }
        }

        public static double[] GetECGData(Random random)
        {
            double[] data = new double[200];
            for (int i = 0; i < 200; i++)
            {
                data[i] = random.NextDouble() * 60 + 30;
            }
            return data;
        }
    }
}
