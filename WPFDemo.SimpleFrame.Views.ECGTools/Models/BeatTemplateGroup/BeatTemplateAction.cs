using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WPFDemo.SimpleFrame.Infra.Helper;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public class BeatTemplateAction : IBeatTemplateAction
    {
        public bool AddBeatTemplate(string originReferId, string targetParentReferId)
        {
            BeatInfoSource.BeatSource.AddCategory(originReferId, targetParentReferId);
            return true;
        }

        public void ChangeBeatInfo(string key, IList<string> referIds)
        {
            var typeList = EnumHelper.GetSelectList(typeof(BeatTypeEnum)).Select(x => x.Name).ToList();
            if (typeList.Contains(key))
            {
                var beatType = (BeatTypeEnum)Enum.Parse(typeof(BeatTypeEnum), key);
                BeatInfoSource.BeatSource.ChangedBeatInfo(referIds, beatType);
            }
            if (key == Key.D.ToString())
            {
                BeatInfoSource.BeatSource.DeleteBeatInfos(referIds);
            }
        }

        public List<BeatTemplate> GetAtrialBeatTemplate()
        {
            return BeatInfoSource.BeatTemplates.Where(x => !x.IsEvent).ToList();
        }

        public List<BeatTemplate> GetEventBeatTemplate()
        {
            return BeatInfoSource.BeatTemplates.Where(x => x.IsEvent).ToList();
        }

        public List<ParentBeatTemplate> GetParentBeatTemplate()
        {
            return BeatInfoSource.ParentBeatTemplateDic.Values.ToList();
        }

        public string GetReferIdByBeatTemplateIndex(int count)
        {
            if(BeatInfoSource.BeatTemplates.Count <= count)
            {
                return string.Empty;
            }
            return BeatInfoSource.BeatTemplates[count].Id;
        }

        public bool MergeBeatTemplate(IList<string> originReferId, string targetReferId)
        {
            BeatInfoSource.BeatSource.MergeBeats(originReferId, targetReferId);
            return true;
        }

        public void UpdateIsCheckedStatus(IList<string> referIds)
        {
            BeatInfoSource.BeatTemplates.Where(x => referIds.Contains(x.Id)).ToList().ForEach(t => t.IsChecked = true);
        }
    }
}
