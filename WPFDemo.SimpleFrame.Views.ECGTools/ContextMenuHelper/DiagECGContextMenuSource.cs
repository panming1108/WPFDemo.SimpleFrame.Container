using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Views.ECGTools.ContextMenuHelper
{
    public class DiagECGContextMenuSource : IDisposable
    {
        private readonly MenuItem _addTypical;
        public MenuItem AddTypical => _addTypical;
        private readonly RoutedEventHandler _addTypicalRoutedEventHandler;

        private readonly MenuItem _setFastestHR;
        public MenuItem SetFastestHR => _setFastestHR;
        private readonly RoutedEventHandler _setFastestHRRoutedEventHandler;

        private readonly MenuItem _setSlowestHR;
        public MenuItem SetSlowestHR => _setSlowestHR;
        private readonly RoutedEventHandler _setSlowestHRRoutedEventHandler;

        private readonly MenuItem _markStartFlag;
        public MenuItem MarkStartFlag => _markStartFlag;
        private readonly RoutedEventHandler _markStartFlagRoutedEventHandler;

        private readonly MenuItem _markEndFlag;
        public MenuItem MarkEndFlag => _markEndFlag;
        private readonly RoutedEventHandler _markEndFlagRoutedEventHandler;

        private readonly MenuItem _cancelMarkFlag;
        public MenuItem CancelMarkFlag => _cancelMarkFlag;
        private readonly RoutedEventHandler _cancelMarkFlagRoutedEventHandler;

        public DiagECGContextMenuSource(RoutedEventHandler addTypicalRoutedEventHandler, RoutedEventHandler setFastestHRRoutedEventHandler, RoutedEventHandler setSlowestHRRoutedEventHandler, RoutedEventHandler markStartFlagRoutedEventHandler, RoutedEventHandler markEndFlagRoutedEventHandler, RoutedEventHandler cancelMarkFlagRoutedEventHandler)
        {
            _addTypicalRoutedEventHandler = addTypicalRoutedEventHandler;
            _setFastestHRRoutedEventHandler = setFastestHRRoutedEventHandler;
            _setSlowestHRRoutedEventHandler = setSlowestHRRoutedEventHandler;
            _markStartFlagRoutedEventHandler = markStartFlagRoutedEventHandler;
            _markEndFlagRoutedEventHandler = markEndFlagRoutedEventHandler;
            _cancelMarkFlagRoutedEventHandler = cancelMarkFlagRoutedEventHandler;

            _addTypical = MenuItemHelper.GetMenuItem("添加典型图", string.Empty, _addTypicalRoutedEventHandler);
            _setFastestHR = MenuItemHelper.GetMenuItem("设置最快心率", string.Empty, _setFastestHRRoutedEventHandler);
            _setSlowestHR = MenuItemHelper.GetMenuItem("设置最慢心率", string.Empty, _setSlowestHRRoutedEventHandler);
            _markStartFlag = MenuItemHelper.GetMenuItem("标记开始位置", string.Empty, _markStartFlagRoutedEventHandler);
            _markEndFlag = MenuItemHelper.GetMenuItem("标记结束位置", string.Empty, _markEndFlagRoutedEventHandler);
            _cancelMarkFlag = MenuItemHelper.GetMenuItem("取消标记位置", string.Empty, _cancelMarkFlagRoutedEventHandler);
        }

        public void Dispose()
        {
            MenuItemHelper.RemoveClickEventHander(_addTypical, _addTypicalRoutedEventHandler);
            MenuItemHelper.RemoveClickEventHander(_setFastestHR, _setFastestHRRoutedEventHandler);
            MenuItemHelper.RemoveClickEventHander(_setSlowestHR, _setSlowestHRRoutedEventHandler);
            MenuItemHelper.RemoveClickEventHander(_markStartFlag, _markStartFlagRoutedEventHandler);
            MenuItemHelper.RemoveClickEventHander(_markEndFlag, _markEndFlagRoutedEventHandler);
            MenuItemHelper.RemoveClickEventHander(_cancelMarkFlag, _cancelMarkFlagRoutedEventHandler);
        }
    }
}
