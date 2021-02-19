using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Views.ECGTools.ContextMenuHelper
{
    public class InsertBeatContextMenuSource : IDisposable
    {
        private readonly List<MenuItem> _singleLevelMenuItems = new List<MenuItem>();
        private readonly List<MenuItem> _doubleLevelMenuItems = new List<MenuItem>();
        private readonly RoutedEventHandler _routedEventHandler;

        protected List<MenuItem> SingleLevelMenuItems => _singleLevelMenuItems;
        protected List<MenuItem> DoubleLevelMenuItems => _doubleLevelMenuItems;
        protected RoutedEventHandler RoutedEventHandler => _routedEventHandler;

        public InsertBeatContextMenuSource(RoutedEventHandler routedEventHandler)
        {
            _routedEventHandler = routedEventHandler;
            InitSingleLevelMenuItems();
            InitDoubleLevelMenuItems();
        }

        private void InitDoubleLevelMenuItems()
        {
            var wItem1 = MenuItemHelper.GetMenuItem("窦性", "N(W)", _routedEventHandler);
            var wItem2 = MenuItemHelper.GetMenuItem("房性", "S(W)", _routedEventHandler);
            var wItem = MenuItemHelper.GetMenuItem("预激波", "W", new MenuItem[] { wItem1, wItem2 });
            DoubleLevelMenuItems.Add(wItem);

            var bItem1 = MenuItemHelper.GetMenuItem("窦性", "N(B)", _routedEventHandler);
            var bItem2 = MenuItemHelper.GetMenuItem("房性", "S(B)", _routedEventHandler);
            var bItem = MenuItemHelper.GetMenuItem("束支传导阻滞", "B", new MenuItem[] { bItem1, bItem2 });
            DoubleLevelMenuItems.Add(bItem);

            var pItem1 = MenuItemHelper.GetMenuItem("心房起搏", "F1", _routedEventHandler);
            var pItem2 = MenuItemHelper.GetMenuItem("心室起搏", "F2", _routedEventHandler);
            var pItem3 = MenuItemHelper.GetMenuItem("双腔起搏", "F3", _routedEventHandler);
            var pItem = MenuItemHelper.GetMenuItem("起搏", "P", new MenuItem[] { pItem1, pItem2, pItem3 });
            DoubleLevelMenuItems.Add(pItem);
        }

        private void InitSingleLevelMenuItems()
        {
            SingleLevelMenuItems.Add(MenuItemHelper.GetMenuItem("正常", "N", _routedEventHandler));
            SingleLevelMenuItems.Add(MenuItemHelper.GetMenuItem("房早", "S", _routedEventHandler));
            SingleLevelMenuItems.Add(MenuItemHelper.GetMenuItem("房早未下传", "O", _routedEventHandler));
            SingleLevelMenuItems.Add(MenuItemHelper.GetMenuItem("房早伴室内差异性传导", "H", _routedEventHandler));
            SingleLevelMenuItems.Add(MenuItemHelper.GetMenuItem("房颤/房扑伴室内差异性传导", string.Empty, _routedEventHandler));
            SingleLevelMenuItems.Add(MenuItemHelper.GetMenuItem("交界性早搏", "J", _routedEventHandler));
            SingleLevelMenuItems.Add(MenuItemHelper.GetMenuItem("室早", "V", _routedEventHandler));
            SingleLevelMenuItems.Add(MenuItemHelper.GetMenuItem("房性逸搏", "M", _routedEventHandler));
            SingleLevelMenuItems.Add(MenuItemHelper.GetMenuItem("交界性逸搏", "G", _routedEventHandler));
            SingleLevelMenuItems.Add(MenuItemHelper.GetMenuItem("室性逸搏", "E", _routedEventHandler));
        }

        public virtual IEnumerable<Control> GetAllMenuItems()
        {
            var result = new List<Control>();
            result.AddRange(SingleLevelMenuItems);
            result.AddRange(DoubleLevelMenuItems);
            return result;
        }

        public virtual void Dispose()
        {
            foreach (var item in SingleLevelMenuItems)
            {
                if(item is MenuItem)
                {
                    MenuItemHelper.RemoveClickEventHander(item, _routedEventHandler);
                }
            }
            SingleLevelMenuItems.Clear();
            foreach (var item in DoubleLevelMenuItems)
            {
                if (item is MenuItem)
                {
                    MenuItemHelper.RemoveClickEventHander(item, _routedEventHandler);
                }
            }
            DoubleLevelMenuItems.Clear();
        }
    }
}
