using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFDemo.SimpleFrame.Infra.Helper;
using WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList;
using WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public class BeatInfoSource
    {
        private readonly int _count;
        public Random random = new Random();
        private readonly List<BeatInfo> _allBeatInfos;
        private readonly Dictionary<int, List<BeatInfo>> _allBeatInfoDic;
        public Dictionary<int, List<BeatInfo>> AllBeatInfoDic => _allBeatInfoDic;

        public static BeatInfoSource BeatSource;
        public static List<ParentBeatTemplate> ParentBeatTemplates;

        public BeatInfoSource(int count)
        {
            _count = count;
            _allBeatInfos = GetAllBeatInfos();
            _allBeatInfoDic = GetAllBeatInfoDic();
            ParentBeatTemplates = GetParentBeatTemplates();
        }

        private List<ParentBeatTemplate> GetParentBeatTemplates()
        {
            var result = new List<ParentBeatTemplate>();
            foreach (var item in EnumHelper.GetSelectList(typeof(BeatTypeEnum)))
            {
                var count = _allBeatInfos.Where(x => x.BeatType == item.Name).Count();
                ParentBeatTemplate parentBeatTemplate = new ParentBeatTemplate()
                {
                    Id = Guid.NewGuid().ToString(),
                    CategortEn = item.Name,
                    CategoryName = item.Description,
                    Count = count,
                    Percent = count * 100d / _allBeatInfos.Count
                };
                result.Add(parentBeatTemplate);
            }
            return result;
        }

        public void SetBeatSource()
        {
            BeatSource = this;
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
            var count = EnumHelper.GetSelectList(typeof(BeatTypeEnum)).Count;
            var results = new List<BeatInfo>();
            for (int i = 0; i < _count; i++)
            {
                BeatInfo beatInfo = new BeatInfo()
                {
                    BeatType = ((BeatTypeEnum)(i % count)).ToString(),
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
        
        public void ChangedBeatInfo(List<int> beatInfoRs, BeatTypeEnum type)
        {
            foreach (var item in beatInfoRs)
            {
                AllBeatInfoDic[item].First().BeatType = type.ToString();
            }
        }

        public void DeleteBeatInfos(List<int> beatInfoRs)
        {
            foreach (var item in beatInfoRs)
            {
                AllBeatInfoDic.Remove(item);
            }
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
