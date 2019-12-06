using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using WPFDemo.SimpleFrame.Infra.Application;

namespace WPFDemo.SimpleFrame.Container
{
    public class Startup : AppBase
    {
        protected override void App_Startup(object sender, StartupEventArgs e)
        {
            string[] lists = new string[]
            {
                "/WPFDemo.SimpleFrame.Infra.ControlsThemes;component/ScrollViewerStyle.xaml",
                "/WPFDemo.SimpleFrame.Infra.ControlsThemes;component/LayOutStyle.xaml",
                "/WPFDemo.SimpleFrame.Infra.ControlsThemes;component/UXsStyle.xaml"
            };
            LoadResources(lists);
            Application.Current.MainWindow = new MainWindow();
            Application.Current.MainWindow.ShowDialog();
        }
    }
}
