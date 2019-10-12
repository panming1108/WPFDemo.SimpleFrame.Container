using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using WPFDemo.SimpleFrame.Infra.CustomControls.UXs.Dialog;
using WPFDemo.SimpleFrame.Infra.Ioc;

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
    }
}
