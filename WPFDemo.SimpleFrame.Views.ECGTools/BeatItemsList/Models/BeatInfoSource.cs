using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public class BeatInfoSource
    {
        private readonly int _count;
        public Random random = new Random();
        public string[] beatTypes = new string[] { "N", "S", "V" };
        private readonly List<BeatInfo> _allBeatInfos;
        private readonly Dictionary<int, List<BeatInfo>> _allBeatInfoDic;
        public Dictionary<int, List<BeatInfo>> AllBeatInfoDic => _allBeatInfoDic;

        public BeatInfoSource(int count)
        {
            _count = count;
            _allBeatInfos = GetAllBeatInfos();
            _allBeatInfoDic = GetAllBeatInfoDic();
        }

        private Dictionary<int, List<BeatInfo>> GetAllBeatInfoDic()
        {
            if (_allBeatInfos != null && !_allBeatInfos.Any())
                return null;

            var beatInfosDic = new Dictionary<int, List<BeatInfo>>();

            //第一个
            beatInfosDic[_allBeatInfos[0].R] = new List<BeatInfo>();
            beatInfosDic[_allBeatInfos[0].R].Add(_allBeatInfos[0]);//当前

            if (_allBeatInfos.Count > 1)
            {
                beatInfosDic[_allBeatInfos[0].R].Add(_allBeatInfos[1]);//下一个
            }
            beatInfosDic[_allBeatInfos[0].R].Add(null);//上一个

            for (var i = 1; i < _allBeatInfos.Count - 1; i++)
            {
                beatInfosDic[_allBeatInfos[i].R] = new List<BeatInfo>();
                beatInfosDic[_allBeatInfos[i].R].Add(_allBeatInfos[i]);//当前
                beatInfosDic[_allBeatInfos[i].R].Add(_allBeatInfos[i + 1]);//下一个
                beatInfosDic[_allBeatInfos[i].R].Add(_allBeatInfos[i - 1]);//上一个
            }

            //最后一个
            beatInfosDic[_allBeatInfos[_allBeatInfos.Count - 1].R] = new List<BeatInfo>();
            beatInfosDic[_allBeatInfos[_allBeatInfos.Count - 1].R].Add(_allBeatInfos[_allBeatInfos.Count - 1]);
            beatInfosDic[_allBeatInfos[_allBeatInfos.Count - 1].R].Add(null);

            if (_allBeatInfos.Count > 1)
            {
                beatInfosDic[_allBeatInfos[_allBeatInfos.Count - 1].R].Add(_allBeatInfos[_allBeatInfos.Count - 2]);
            }

            return beatInfosDic;
        }

        public List<BeatInfo> GetAllBeatInfos()
        {
            var results = new List<BeatInfo>();
            for (int i = 0; i < _count; i++)
            {
                BeatInfo beatInfo = new BeatInfo()
                {
                    BeatType = beatTypes[i % 3],
                    Position = i,
                    R = i,
                    Interval = random.Next(0, _count),
                    Data = GetECGData(random)
                };
                results.Add(beatInfo);
            }
            return results;
        }

        public List<int> GenerateItemsSource(int count)
        {
            var results = new List<int>();
            for (int i = 0; i < count; i++)
            {
                results.Add(i);
            }
            return results;
        }
        
        public void ChangedBeatInfo(List<int> beatInfoRs, string type)
        {
            foreach (var item in beatInfoRs)
            {
                AllBeatInfoDic[item].First().BeatType = type;
            }
        }

        public void DeleteBeatInfos(List<int> beatInfoRs)
        {
            foreach (var item in beatInfoRs)
            {
                AllBeatInfoDic.Remove(item);
            }
        }

        public List<int> GetPrevItemsSource(List<int> beatInfoRs)
        {
            var result = new List<int>();
            foreach (var item in beatInfoRs)
            {
                if (AllBeatInfoDic.TryGetValue(item, out List<BeatInfo> prevList))
                {
                    var prevBeat = prevList[2];
                    if (prevBeat != null)
                    {
                        result.Add(prevBeat.R);
                    }
                }
            }
            return result;
        }

        public List<int> GetCurrentItemsSource(List<int> beatInfoRs)
        {
            var result = new List<int>();
            foreach (var item in beatInfoRs)
            {
                if(AllBeatInfoDic.TryGetValue(item, out List<BeatInfo> currentList))
                {
                    var currentBeat = currentList[0];
                    if (currentBeat != null)
                    {
                        result.Add(currentBeat.R);
                    }
                }
            }
            return result;
        }

        public List<int> GetNextItemsSource(List<int> beatInfoRs)
        {
            var result = new List<int>();
            foreach (var item in beatInfoRs)
            {
                if (AllBeatInfoDic.TryGetValue(item, out List<BeatInfo> nextList))
                {
                    var nextBeat = nextList[1];
                    if (nextBeat != null)
                    {
                        result.Add(nextBeat.R);
                    }
                }
            }
            return result;
        }

        public IEnumerable SortItemsSource(List<int> itemRs, SortArgs sortArgs)
        {
            var list = new List<BeatInfo>();
            foreach (var item in itemRs)
            {
                if(AllBeatInfoDic.TryGetValue(item, out List<BeatInfo> beatInfos))
                {
                    list.Add(beatInfos[0]);
                }
            }
            switch (sortArgs.SortType)
            {
                case SortEnum.IntervalSort:
                    if (sortArgs.IsAsc)
                    {
                        return list.OrderBy(x => x.Interval).Select(w => w.R);
                    }
                    else
                    {
                        return list.OrderByDescending(x => x.Interval).Select(w => w.R);
                    }
                case SortEnum.RSort:
                default:
                    if (sortArgs.IsAsc)
                    {
                        return list.OrderBy(x => x.R).Select(w => w.R);
                    }
                    else
                    {
                        return list.OrderByDescending(x => x.R).Select(w => w.R);
                    }
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
