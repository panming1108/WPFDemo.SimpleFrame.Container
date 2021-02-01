using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;
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
        private Dictionary<int, List<BeatInfo>> _allBeatInfoDic;
        public Dictionary<int, List<BeatInfo>> AllBeatInfoDic => _allBeatInfoDic;

        public static BeatInfoSource BeatSource;
        public static Dictionary<int, ParentBeatTemplate> ParentBeatTemplateDic;
        public static List<BeatTemplate> BeatTemplates;
        private Dictionary<string, BeatTemplate> OriginBeatTemplates = new Dictionary<string, BeatTemplate>();
        private Dictionary<string, BeatTemplate> OriginEventBeatTemplates = new Dictionary<string, BeatTemplate>();
        public static Dictionary<int, RenderPen> RenderPens;

        public BeatInfoSource(int count)
        {
            _count = count;
            _allBeatInfos = GetAllBeatInfos();
            _allBeatInfoDic = GetAllBeatInfoDic();
            ParentBeatTemplateDic = GetParentBeatTemplates();
            BeatTemplates = GetBeatTemplates();
            SetBeatReferId();
            OriginBeatTemplates = BeatTemplates.ToDictionary(k => GetDicKey(k.BeatType, k.SubType));
            AddEventTemplates();
            OriginEventBeatTemplates = BeatTemplates.Where(x => x.IsEvent).ToDictionary(k => k.Id);
            RenderPens = GetRenderPens();
        }

        private Dictionary<int, RenderPen> GetRenderPens()
        {
            var result = new Dictionary<int, RenderPen>();
            var beatTypeSource = EnumHelper.GetSelectList(typeof(BeatTypeEnum));
            foreach (var item in beatTypeSource)
            {
                result.Add(int.Parse(item.Id), new RenderPen(1, ConvertColor(Colors.Black)));
            }
            return result;
        }

        private int ConvertColor(Color color)
        {
            int num = color.A + 1;
            return (color.A << 24) | ((byte)(color.R * num >> 8) << 16) | ((byte)(color.G * num >> 8) << 8) | (byte)(color.B * num >> 8);
        }

        private void SetBeatReferId()
        {
            var dic = BeatTemplates.ToDictionary(k => GetDicKey(k.BeatType, k.SubType), v => v.Id);
            foreach (var item in _allBeatInfos)
            {
                item.BeatReferId = dic[GetDicKey(item.BeatType, item.SubType)];
            }
        }

        private string GetDicKey(int beatType, int subType)
        {
            return beatType + "_" + subType;
        }

        private List<BeatTemplate> GetBeatTemplates()
        {
            return _allBeatInfos.GroupBy(x => new { x.BeatType, x.SubType }).Select(w =>
            new BeatTemplate()
            {
                Id = OriginBeatTemplates.ContainsKey(GetDicKey(w.Key.BeatType, w.Key.SubType)) ? OriginBeatTemplates[GetDicKey(w.Key.BeatType, w.Key.SubType)].Id : Guid.NewGuid().ToString(),
                CategoryEn = ((BeatTypeEnum)w.Key.BeatType).ToString(),
                CategoryName = ((BeatTypeEnum)w.Key.BeatType).ToString(),
                DataCount = w.Count(),
                IsAdd = OriginBeatTemplates.Count > 0 ? !OriginBeatTemplates.ContainsKey(GetDicKey(w.Key.BeatType, w.Key.SubType)) : false,
                IsChecked = OriginBeatTemplates.ContainsKey(GetDicKey(w.Key.BeatType, w.Key.SubType)) ? OriginBeatTemplates[GetDicKey(w.Key.BeatType, w.Key.SubType)].IsChecked : false,
                BeatType = w.Key.BeatType,
                SubType = w.Key.SubType,
                Percent = w.Count() * 1d / _allBeatInfos.Count,
                ParentID = ParentBeatTemplateDic[w.Key.BeatType].Id,
                ParentCategoryEn = ParentBeatTemplateDic[w.Key.BeatType].CategoryNameEn,
                ParentCategoryName = ParentBeatTemplateDic[w.Key.BeatType].CategoryName,
                ParentCount = ParentBeatTemplateDic[w.Key.BeatType].Count,
                WaveList = new ObservableCollection<PixelPointArrayEx>(w.Select(s => new PixelPointArrayEx((int)BeatTypeEnum.S, GetPixelPointArrayEx(s.Data))).ToList()),
            }).OrderBy(a => a.BeatType).ToList();
        }

        private Dictionary<int, ParentBeatTemplate> GetParentBeatTemplates()
        {
            return _allBeatInfos.GroupBy(x => new { x.BeatType }).Select(w =>
            new ParentBeatTemplate()
            {
                Id = Guid.NewGuid().ToString(),
                CategoryNameEn = ((BeatTypeEnum)w.Key.BeatType).ToString(),
                CategoryName = ((BeatTypeEnum)w.Key.BeatType).GetDescription(),
                Count = w.Count(),
                BeatType = w.Key.BeatType,
                Percent = w.Count() * 1d / _allBeatInfos.Count,
            }).OrderBy(a => a.BeatType).ToDictionary(k => k.BeatType);
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

        private int _maxSubType = 10;
        public List<BeatInfo> GetAllBeatInfos()
        {
            var count = EnumHelper.GetSelectList(typeof(BeatTypeEnum)).Count;
            var results = new List<BeatInfo>();
            for (int i = 0; i < _count; i++)
            {
                var beatType = random.Next(0, count);
                BeatInfo beatInfo = new BeatInfo()
                {
                    Position = i,
                    BeatType = beatType,
                    R = i,
                    Interval = random.Next(0, _count),
                    Data = GetECGData(random),
                    SubType = random.Next(0, _maxSubType),
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
                AllBeatInfoDic[item].First().BeatType = (int)type;
            }
            ResetSource();
        }

        public void DeleteBeatInfos(List<int> beatInfoRs)
        {
            foreach (var item in beatInfoRs)
            {
                _allBeatInfos.Remove(AllBeatInfoDic[item].First());
            }
            ResetSource();
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

        public void MergeBeats(string originBeatReferId, string targetBeatReferId)
        {
            var beatTemplate = BeatTemplates.Single(x => x.Id == targetBeatReferId);
            beatTemplate.IsChecked = true;
            List<BeatInfo> needChangeSource;
            var eventCount = EnumHelper.GetSelectList(typeof(EventCodeEnum)).Count;
            if(originBeatReferId.Length <= 3)
            {
                //房性事件合并
                needChangeSource = _allBeatInfos.Where(x => x.BeatType == (int)BeatTypeEnum.S && (x.Interval % eventCount).ToString() == originBeatReferId).ToList();
            }
            else
            {
                needChangeSource = _allBeatInfos.Where(x => x.BeatReferId == originBeatReferId).ToList();
            }
            if(targetBeatReferId.Length <= 3)
            {
                //事件合并
                needChangeSource.ForEach(t => { t.BeatType = beatTemplate.BeatType; t.Interval = eventCount * random.Next(0, _count) + int.Parse(targetBeatReferId); });
            }
            else
            {
                needChangeSource.ForEach(t => { t.BeatType = beatTemplate.BeatType; t.SubType = beatTemplate.SubType; });
            }
            ResetSource();
        }

        public void AddCategory(string originBeatReferId, string targetParentId)
        {
            var beatTemplate = BeatTemplates.FirstOrDefault(x => x.ParentID == targetParentId);
            List<BeatInfo> needChangeSource;
            if (originBeatReferId.Length <= 3)
            {
                var eventCount = EnumHelper.GetSelectList(typeof(EventCodeEnum)).Count;
                //房性事件合并
                needChangeSource = _allBeatInfos.Where(x => x.BeatType == (int)BeatTypeEnum.S && (x.Interval % eventCount).ToString() == originBeatReferId).ToList();
            }
            else
            {
                needChangeSource = _allBeatInfos.Where(x => x.BeatReferId == originBeatReferId).ToList();
            }
            needChangeSource.ForEach(t => { t.BeatType = beatTemplate.BeatType; });
            ResetSource();
        }

        private void ResetSource()
        {
            _allBeatInfoDic = GetAllBeatInfoDic();
            ParentBeatTemplateDic = GetParentBeatTemplates();
            BeatTemplates = GetBeatTemplates();
            SetBeatReferId();
            AddEventTemplates();
        }

        private void AddEventTemplates()
        {
            var eventCount = EnumHelper.GetSelectList(typeof(EventCodeEnum)).Count;
            var eventSource = _allBeatInfos.Where(x => x.BeatType == (int)BeatTypeEnum.S).GroupBy(x => x.Interval % eventCount).Select(x => 
            new BeatTemplate() 
            {
                Id = x.Key.ToString(),
                IsEvent = true,
                CategoryEn = x.Key.ToString(),
                CategoryName = ((EventCodeEnum)x.Key).GetDescription(),
                DataCount = x.Count(),
                BeatType = (int)BeatTypeEnum.S,
                IsAdd = OriginEventBeatTemplates.Count > 0 ? !OriginEventBeatTemplates.ContainsKey(x.Key.ToString()) : false,
                IsChecked = OriginEventBeatTemplates.ContainsKey(x.Key.ToString()) ? OriginEventBeatTemplates[x.Key.ToString()].IsChecked : false,
                Percent = x.Count() * 1d / _allBeatInfos.Count,
                ParentID = ParentBeatTemplateDic[(int)BeatTypeEnum.S].Id,
                ParentCategoryEn = ParentBeatTemplateDic[(int)BeatTypeEnum.S].CategoryNameEn,
                ParentCategoryName = ParentBeatTemplateDic[(int)BeatTypeEnum.S].CategoryName,
                ParentCount = ParentBeatTemplateDic[(int)BeatTypeEnum.S].Count,
                WaveList = new ObservableCollection<PixelPointArrayEx>(x.Select(s => new PixelPointArrayEx((int)BeatTypeEnum.S, GetPixelPointArrayEx(s.Data))).ToList()),
            }).OrderBy(a => a.CategoryEn).ToList();
            BeatTemplates.AddRange(eventSource);
        }

        private List<PixelPoint> GetPixelPointArrayEx(double[] data)
        {
            var result = new List<PixelPoint>();
            for (int i = 0; i < data.Count(); i++)
            {
                PixelPoint pixelPoint = new PixelPoint(i, (int)data[i]);
                result.Add(pixelPoint);
            }
            return result;
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
