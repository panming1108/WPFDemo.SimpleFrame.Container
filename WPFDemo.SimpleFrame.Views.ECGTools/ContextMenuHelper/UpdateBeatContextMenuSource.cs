using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Views.ECGTools.ContextMenuHelper
{
    public class UpdateBeatContextMenuSource : InsertBeatContextMenuSource
    {
        private readonly MenuItem _shishangxingMenuItem;
        private readonly MenuItem _xMenuItem;
        private readonly MenuItem _otherMenuItem;
        public UpdateBeatContextMenuSource(RoutedEventHandler routedEventHandler) : base(routedEventHandler)
        {
            _shishangxingMenuItem = MenuItemHelper.GetMenuItem("室上性(自动归类N/S/Af/AF)", string.Empty, RoutedEventHandler);
            _xMenuItem = MenuItemHelper.GetMenuItem("伪差", "X", RoutedEventHandler);

            var yiwenxinboItem = MenuItemHelper.GetMenuItem("疑问心搏", "Q", RoutedEventHandler);
            var rongheboItem = MenuItemHelper.GetMenuItem("融合波", string.Empty, RoutedEventHandler);
            var shichanItem = MenuItemHelper.GetMenuItem("室颤", string.Empty, RoutedEventHandler);
            var pItem = MenuItemHelper.GetMenuItem("P波", "Y", RoutedEventHandler);
            var tItem = MenuItemHelper.GetMenuItem("t波", "T", RoutedEventHandler);
            _otherMenuItem = MenuItemHelper.GetMenuItem("其他", string.Empty, new MenuItem[] { yiwenxinboItem, rongheboItem, shichanItem, pItem, tItem });
        }

        public override IEnumerable<Control> GetAllMenuItems()
        {
            var result = new List<Control>();
            result.Add(_shishangxingMenuItem);
            result.AddRange(SingleLevelMenuItems);
            result.Add(_xMenuItem);
            result.AddRange(DoubleLevelMenuItems);
            result.Add(_otherMenuItem);
            return result;
        }

        public override void Dispose()
        {
            base.Dispose();
            MenuItemHelper.RemoveClickEventHander(_shishangxingMenuItem, RoutedEventHandler);
            MenuItemHelper.RemoveClickEventHander(_xMenuItem, RoutedEventHandler);
            MenuItemHelper.RemoveClickEventHander(_otherMenuItem, RoutedEventHandler);
        }
    }
}
