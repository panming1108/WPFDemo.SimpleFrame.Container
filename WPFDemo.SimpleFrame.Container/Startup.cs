using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFDemo.SimpleFrame.Container
{
    public class Startup : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string[] lists = new string[]
            {
                "/WPFDemo.SimpleFrame.Infra.ControlsThemes;component/LayOutStyle.xaml"
            };
            LoadResources(lists);
            base.OnStartup(e);
            Application.Current.MainWindow = new MainWindow();
            Application.Current.MainWindow.ShowDialog();
        }

        private void LoadResources(string[] resourcesUris)
        {
            if(resourcesUris == null)
            {
                return;
            }
            Application.Current.Resources.MergedDictionaries.Clear();

            foreach (var resourcesUri in resourcesUris)
            {
                var resourceDictionary = new ResourceDictionary()
                {
                    Source = new Uri(resourcesUri, UriKind.RelativeOrAbsolute)
                };
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }
        }
    }
}
