using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Views.ECGTools.ContextMenuHelper
{
    public class OperateContextMenuSource : IDisposable
    {
        private readonly RoutedEventHandler _insertBeatToPrevRoutedEventHandler;
        private readonly RoutedEventHandler _insertBeatToNextRoutedEventHandler;

        private readonly MenuItem _deleteBeat;
        public MenuItem DeleteBeat => _deleteBeat;
        private readonly RoutedEventHandler _deleteBeatRoutedEventHandler;

        private readonly MenuItem _insertBeatToPrev;
        public MenuItem InsertBeatToPrev => _insertBeatToPrev;

        private readonly MenuItem _insertBeatToNext;
        public MenuItem InsertBeatToNext => _insertBeatToNext;

        private readonly MenuItem _unConfuse;
        public MenuItem UnConfuse => _unConfuse;

        private readonly MenuItem _pWaveUnConfuse;
        public MenuItem PWaveUnConfuse => _pWaveUnConfuse;
        private readonly RoutedEventHandler _unConfuseRoutedEventHandler;

        private readonly MenuItem _setLongestRR;
        public MenuItem SetLongestRR => _setLongestRR;
        private readonly RoutedEventHandler _setLongestRRRoutedEventHandler;

        private readonly MenuItem _mergeBeat;
        public MenuItem MergeBeat => _mergeBeat;
        private readonly RoutedEventHandler _mergeBeatRoutedEventHandler;
        public OperateContextMenuSource(RoutedEventHandler deleteHandler, RoutedEventHandler insertBeatToPrevHandler, RoutedEventHandler insertBeatToNextHandler, RoutedEventHandler unConfuseHandler, RoutedEventHandler setLongestRRHandler, RoutedEventHandler mergeBeatHandler)
        {
            _deleteBeatRoutedEventHandler = deleteHandler;
            _insertBeatToPrevRoutedEventHandler = insertBeatToPrevHandler;
            _insertBeatToNextRoutedEventHandler = insertBeatToNextHandler;
            _unConfuseRoutedEventHandler = unConfuseHandler;
            _setLongestRRRoutedEventHandler = setLongestRRHandler;
            _mergeBeatRoutedEventHandler = mergeBeatHandler;

            _deleteBeat = MenuItemHelper.GetMenuItem("删除心搏", string.Empty, _deleteBeatRoutedEventHandler);

            _insertBeatToPrev = MenuItemHelper.GetMenuItem("往前插入心搏", string.Empty, new InsertBeatContextMenuSource(_insertBeatToPrevRoutedEventHandler).GetAllMenuItems());
            _insertBeatToNext = MenuItemHelper.GetMenuItem("往后插入心搏", string.Empty, new InsertBeatContextMenuSource(_insertBeatToNextRoutedEventHandler).GetAllMenuItems());

            _unConfuse = MenuItemHelper.GetMenuItem("叠加反混淆", string.Empty, _unConfuseRoutedEventHandler, "UnConfuse");
            _pWaveUnConfuse = MenuItemHelper.GetMenuItem("P波反混淆", string.Empty, _unConfuseRoutedEventHandler, "PWaveUnConfuse");
            _setLongestRR = MenuItemHelper.GetMenuItem("设置最长RR间期", string.Empty, _setLongestRRRoutedEventHandler);
            _mergeBeat = MenuItemHelper.GetMenuItem("合并", string.Empty, _mergeBeatRoutedEventHandler);
        }

        public void Dispose()
        {
            MenuItemHelper.RemoveClickEventHander(_deleteBeat, _deleteBeatRoutedEventHandler);
            MenuItemHelper.RemoveClickEventHander(_insertBeatToPrev, _insertBeatToPrevRoutedEventHandler);
            MenuItemHelper.RemoveClickEventHander(_insertBeatToNext, _insertBeatToNextRoutedEventHandler);
            MenuItemHelper.RemoveClickEventHander(_unConfuse, _unConfuseRoutedEventHandler);
            MenuItemHelper.RemoveClickEventHander(_pWaveUnConfuse, _unConfuseRoutedEventHandler);
            MenuItemHelper.RemoveClickEventHander(_setLongestRR, _setLongestRRRoutedEventHandler);
            MenuItemHelper.RemoveClickEventHander(_mergeBeat, _mergeBeatRoutedEventHandler);
        }
    }
}
