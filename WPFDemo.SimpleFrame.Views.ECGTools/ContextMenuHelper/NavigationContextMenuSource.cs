using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Views.ECGTools.ContextMenuHelper
{
    public class NavigationContextMenuSource : IDisposable
    {
        private readonly MenuItem _jumpToFullView;
        public MenuItem JumpToFullView => _jumpToFullView;
        private readonly RoutedEventHandler _jumpToFullViewHandler;
        private readonly MenuItem _jumpToDiagView;
        public MenuItem JumpToDiagView => _jumpToDiagView;
        private readonly RoutedEventHandler _jumpToDiagViewHandler;

        public NavigationContextMenuSource()
        {
            _jumpToFullViewHandler = new RoutedEventHandler(JumpToFullView_Click);
            _jumpToDiagViewHandler = new RoutedEventHandler(JumpToDiagView_Click);

            _jumpToFullView = MenuItemHelper.GetMenuItem("跳转到预览图", string.Empty, JumpToFullView_Click);
            _jumpToDiagView = MenuItemHelper.GetMenuItem("跳转到诊断图", string.Empty, JumpToDiagView_Click);
        }

        public IEnumerable<Control> GetAllMenuItems()
        {
            return new Control[] { _jumpToFullView, _jumpToDiagView };
        }

        private void JumpToDiagView_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("跳转到诊断图");
        }

        private void JumpToFullView_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("跳转到预览图");
        }

        public void Dispose()
        {
            MenuItemHelper.RemoveClickEventHander(_jumpToFullView, _jumpToFullViewHandler);
            MenuItemHelper.RemoveClickEventHander(_jumpToFullView, _jumpToDiagViewHandler);
        }
    }
}
