using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public interface IBeatDetailAction
    {
        //排序
        void SortItemsSource(SortArgs sortArgs);
        //切换前一个，当前，下一个
        void PrevCurrentNextChaned(int prevCurrentNext);
        //改变心搏
        void ChangeBeatInfo(string key);
        //获取分页显示项
        ItemsPager GetPagerSource(int pageNo, int pageSize);
        //获取总页数
        int GetTotalPage(int pageSize);
        //获取某一项所在页数
        int GetCurrentItemPageNo(int r, int pageSize);


        int GetSelectedItemsCount();
        //全选
        int SelectAll();
        //全选
        int SelectReverse();
        //重新选中 选中项
        int ResetSelectItemsWithOnlyDisplaySelectedItems(IList selectedItems);
        int ResetSelectedItemsWithOtherSelectItems(IList selectedItems, IList unSelectedItems);
        bool SelectedItemsContainsR(int r);
    }
}
