using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public interface IBeatTemplateAction
    {
        //合并模板
        bool MergeBeatTemplate(IList<string> originReferId, string targetReferId);

        //新增模板
        bool AddBeatTemplate(string originReferId, string targetParentReferId);

        //修改或删除心搏
        void ChangeBeatInfo(string key, IList<string> referIds);

        //更新IsChecked状态
        void UpdateIsCheckedStatus(IList<string> referIds);

        //获取形态模板
        List<BeatTemplate> GetAtrialBeatTemplate();
        //获取事件模板
        List<BeatTemplate> GetEventBeatTemplate();
        //获取父类型分类
        List<ParentBeatTemplate> GetParentBeatTemplate();
        //根据分类序号获取ReferId
        string GetReferIdByBeatTemplateIndex(int count);
    }
}
