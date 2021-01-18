using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class BeatInfoSource
    {
        public Random random = new Random();
        public string[] beatTypes = new string[] { "N", "S", "V" };
        public Dictionary<int, BeatInfo> AllBeatInfos { get; set; }

        public BeatInfoSource()
        {
            AllBeatInfos = GetAllBeatInfos();
        }

        public Dictionary<int, BeatInfo> GetAllBeatInfos(int count = 840000)
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

        public List<int> GenerateItemsSource(int count)
        {
            var results = new List<int>();
            for (int i = 0; i < count; i++)
            {
                results.Add(random.Next(0, count));
            }
            return results;
        }
        
        public void ChangedBeatInfo(IList beatInfoRs, string type)
        {
            foreach (var item in beatInfoRs)
            {
                AllBeatInfos[(int)item].BeatType = type;
            }
        }

        public void DeleteBeatInfos(IList beatInfoRs)
        {            
            foreach (var item in beatInfoRs)
            {
                AllBeatInfos.Remove((int)item);
            }
        }

        public double[] GetECGData(Random random)
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
