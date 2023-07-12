using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using WPFDemo.SimpleFrame.Infra.Application;
using WPFDemo.SimpleFrame.Infra.Helper;
using WPFDemo.SimpleFrame.Infra.Ioc;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.Container
{
    public class Startup : AppBase
    {
        public Startup()
        {
            DispatcherUnhandledException += Startup_DispatcherUnhandledException;
        }

        private void Startup_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var str = "系统异常\r\n" + e.Exception.Message + "\r\n" + e.Exception.StackTrace + "\r\n" + e.Exception.InnerException;
            LogHelper.Error(str);
            MessageBox.Show(str);
        }

        protected override void App_Startup(object sender, StartupEventArgs e)
        {
            string[] lists = new string[]
            {
                "/WPFDemo.SimpleFrame.Infra.ControlsThemes;component/ScrollViewerStyle.xaml",
                "/WPFDemo.SimpleFrame.Infra.ControlsThemes;component/LayOutStyle.xaml",
                "/WPFDemo.SimpleFrame.Infra.ControlsThemes;component/UXsStyle.xaml"
            };
            LoadResources(lists);
            Globalization.Init(LangueageEnum.English);
            Application.Current.MainWindow = new MainWindow();
            Application.Current.MainWindow.ShowDialog();
        }
    }
}
