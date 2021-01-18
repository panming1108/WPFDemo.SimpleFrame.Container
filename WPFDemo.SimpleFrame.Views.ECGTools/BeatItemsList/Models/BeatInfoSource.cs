using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class BeatInfoSource
    {
        public static Random random = new Random();
        public static string[] beatTypes = new string[] { "N", "S", "V" };
        public static Dictionary<int, BeatInfo> AllBeatInfos { get; set; }
        static BeatInfoSource()
        {
            AllBeatInfos = GetAllBeatInfos();
        }

        public static Dictionary<int, BeatInfo> GetAllBeatInfos(int count = 200000)
        {          
            var results = new Dictionary<int, BeatInfo>();
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
                results.Add(i, beatInfo);
            }
            return results;
        }

        public static List<int> GenerateItemsSource(int count)
        {
            var results = new List<int>();
            for (int i = 0; i < count; i++)
            {
                results.Add(random.Next(0, count));
            }
            return results;
        }
        
        public static void ChangedBeatInfo(List<int> beatInfoRs, string type)
        {
            foreach (var item in beatInfoRs)
            {
                AllBeatInfos[item].BeatType = type;
            }
        }

        public static void DeleteBeatInfos(List<int> beatInfoRs)
        {            
            foreach (var item in beatInfoRs)
            {
                AllBeatInfos.Remove(item);
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
