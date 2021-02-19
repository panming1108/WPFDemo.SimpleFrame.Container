using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Views.ECGTools.ContextMenuHelper
{
    public class AFContextMenuSource : IDisposable
    {
        private readonly MenuItem _signAf;
        public MenuItem SignAf => _signAf;
        private readonly MenuItem _signAF;
        public MenuItem SignAF => _signAF;
        private readonly RoutedEventHandler _signAFRoutedEventHandler;

        private readonly MenuItem _cancelSignAfOrAF;
        public MenuItem CancelSignAfOrAF => _cancelSignAfOrAF;
        private readonly RoutedEventHandler _cancelSignAfOrAFRoutedEventHandler;

        private readonly MenuItem _deleteAllAf;
        public MenuItem DeleteAllAf => _deleteAllAf;
        private readonly RoutedEventHandler _deleteAllAfHandler;
        private readonly MenuItem _deleteAllAF;
        public MenuItem DeleteAllAF => _deleteAllAF;
        private readonly RoutedEventHandler _deleteAllAFHandler;

        public AFContextMenuSource(RoutedEventHandler signAFRoutedEventHandler, RoutedEventHandler cancelSignAfOrAFRoutedEventHandler)
        {
            _signAFRoutedEventHandler = signAFRoutedEventHandler;
            _cancelSignAfOrAFRoutedEventHandler = cancelSignAfOrAFRoutedEventHandler;
            _deleteAllAfHandler = new RoutedEventHandler(DeleteAllAfHandler);
            _deleteAllAFHandler = new RoutedEventHandler(DeleteAllAFHandler);

            _signAf = MenuItemHelper.GetMenuItem("标记为房颤", "F", _signAFRoutedEventHandler);
            _signAF = MenuItemHelper.GetMenuItem("标记为房扑", "C", _signAFRoutedEventHandler);
            _cancelSignAfOrAF = MenuItemHelper.GetMenuItem("取消标记房颤/房扑", string.Empty, _cancelSignAfOrAFRoutedEventHandler);
            _deleteAllAf = MenuItemHelper.GetMenuItem("删除所有房颤", string.Empty, _deleteAllAfHandler);
            _deleteAllAF = MenuItemHelper.GetMenuItem("删除所有房扑", string.Empty, _deleteAllAFHandler);
        }

        private void DeleteAllAFHandler(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("删除所有房扑");
        }

        private void DeleteAllAfHandler(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("删除所有房颤");
        }

        public void Dispose()
        {
            MenuItemHelper.RemoveClickEventHander(_signAf, _signAFRoutedEventHandler);
            MenuItemHelper.RemoveClickEventHander(_signAF, _signAFRoutedEventHandler);
            MenuItemHelper.RemoveClickEventHander(_cancelSignAfOrAF, _cancelSignAfOrAFRoutedEventHandler);
            MenuItemHelper.RemoveClickEventHander(_deleteAllAf, _deleteAllAfHandler);
            MenuItemHelper.RemoveClickEventHander(_deleteAllAF, _deleteAllAFHandler);
        }
    }
}
