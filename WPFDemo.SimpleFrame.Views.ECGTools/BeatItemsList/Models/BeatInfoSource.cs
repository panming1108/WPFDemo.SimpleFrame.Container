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
        public static Dictionary<int, BeatInfo> AllBeatInfos { get; set; }
        static BeatInfoSource()
        {
            AllBeatInfos = GetAllBeatInfos();
        }

        public static Dictionary<int, BeatInfo> GetAllBeatInfos()
        {
            Random random = new Random();
            var results = new Dictionary<int, BeatInfo>();
            for (int i = 0; i < 100000; i++)
            {
                BeatInfo beatInfo = new BeatInfo()
                {
                    BeatType = beatTypes[i % 3],
                    Position = i,
                    R = i,
                    Interval = random.Next(0, 2000)
                };
                results.Add(i, beatInfo);
            }
            return results;
        }

        public static Tuple<List<int>, int> GetPagerBeatInfo(int pageIndex, int pageSize)
        {
            var totalPage = AllBeatInfos.Count % pageSize == 0 ? AllBeatInfos.Count / pageSize : (AllBeatInfos.Count / pageSize) + 1;
            var pageNo = pageIndex;
            if(pageIndex > totalPage)
            {
                pageNo = totalPage;
            }
            return new Tuple<List<int>, int>(AllBeatInfos.Keys.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList(), pageNo);
        }
        
        public static void ChangedBeatInfo(IList beatInfoRs, string type)
        {
            foreach (var item in beatInfoRs)
            {
                var beatInfoR = (int)item; 
                AllBeatInfos[beatInfoR].BeatType = type;
            }
        }

        public static void DeleteBeatInfos(IList beatInfoRs)
        {            
            foreach (var item in beatInfoRs)
            {
                var beatInfoR = (int)item;
                AllBeatInfos.Remove(beatInfoR);
            }
        }
    }
}
