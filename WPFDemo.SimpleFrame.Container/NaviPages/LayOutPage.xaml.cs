using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFDemo.SimpleFrame.Infra.CustomControls.LayOuts.Window;

namespace WPFDemo.SimpleFrame.Container.NaviPages
{
    /// <summary>
    /// LayOutPage.xaml 的交互逻辑
    /// </summary>
    public partial class LayOutPage : Page
    {
        public LayOutPage()
        {
            InitializeComponent();
        }

        private void EMCButton_Click(object sender, RoutedEventArgs e)
        {
            EMCWindow eMCWindow = new EMCWindow();
            eMCWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            eMCWindow.ShowDialog();
        }

        private void EMCButton_Click_1(object sender, RoutedEventArgs e)
        {
            TestEMCWindow eMCWindow = new TestEMCWindow();
            eMCWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            eMCWindow.ShowDialog();
        }
    }
}
