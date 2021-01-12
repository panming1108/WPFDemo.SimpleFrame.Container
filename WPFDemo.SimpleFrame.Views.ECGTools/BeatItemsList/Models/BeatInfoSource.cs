using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class BeatInfoSource
    {
        public static string[] beatTypes = new string[] { "N", "S", "V" };
        public static List<BeatInfo> AllBeatInfos { get; set; }
        static BeatInfoSource()
        {
            AllBeatInfos = GetAllBeatInfos();
        }

        public static List<BeatInfo> GetAllBeatInfos()
        {
            Random random = new Random();
            var results = new List<BeatInfo>();
            for (int i = 0; i < 200000; i++)
            {
                BeatInfo beatInfo = new BeatInfo()
                {
                    BeatType = beatTypes[i % 3],
                    Position = i,
                    Interval = random.Next(0, 2000)
                };
                results.Add(beatInfo);
            }
            return results;
        }

        public static List<BeatInfo> GetPagerBeatInfo(int pageIndex, int pageSize)
        {
            var totalPage = AllBeatInfos.Count % pageSize == 0 ? AllBeatInfos.Count / pageSize : (AllBeatInfos.Count / pageSize) + 1;
            var pageNo = pageIndex;
            if(pageIndex > totalPage)
            {
                pageNo = totalPage;
            }
            return AllBeatInfos.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
