using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFDemo.SimpleFrame.Infra.CustomControls.UXs.BusyIndicator;
using WPFDemo.SimpleFrame.Infra.CustomControls.UXs.DesktopAlert;
using WPFDemo.SimpleFrame.Infra.CustomControls.UXs.Dialog;
using WPFDemo.SimpleFrame.Infra.Enums;
using WPFDemo.SimpleFrame.Infra.Ioc;
using WPFDemo.SimpleFrame.Infra.Messager;
using WPFDemo.SimpleFrame.Infra.Models;
using WPFDemo.SimpleFrame.IViews.CustomDialogs;

namespace WPFDemo.SimpleFrame.Container.NaviPages
{
    /// <summary>
    /// UXsPage.xaml 的交互逻辑
    /// </summary>
    public partial class UXsPage : Page
    {
        public UXsPage()
        {
            InitializeComponent();
        }

        private void EMCButton_Click(object sender, RoutedEventArgs e)
        {
            var confirmDialog = IocManagerInstance.ResolveType<IConfirmDialog>();
            Debug.WriteLine(confirmDialog.ShowDialog("确认", "asdfhkashkdfjhkaj", null));
        }

        private async void EMCButton_Click_1(object sender, RoutedEventArgs e)
        {
            MessagerInstance.GetMessager().Send(MessagerKeyEnum.IsBusy, BusyStateEnum.IsBusy);
            await Task.Factory.StartNew(() => { Thread.Sleep(5000); });
            MessagerInstance.GetMessager().Send(MessagerKeyEnum.IsBusy, BusyStateEnum.NotBusy);
        }

        private void EMCButton_Click_2(object sender, RoutedEventArgs e)
        {
            MessagerInstance.GetMessager().Send(MessagerKeyEnum.PopupNotifyBox, new PopupNotifyObject("通知", "阿阿斯顿发斯蒂芬"));
        }
    }
}
