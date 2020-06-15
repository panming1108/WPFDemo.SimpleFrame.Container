using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFDemo.SimpleFrame.Container.IViewModels;
using WPFDemo.SimpleFrame.Infra.Ioc;

namespace WPFDemo.SimpleFrame.Container
{
    /// <summary>
    /// DemoUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class DemoUserControl : System.Windows.Controls.UserControl
    {
        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(Keys bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        public const int KEYEVENTF_KEYUP = 2;

        public DemoUserControl()
        {
            InitializeComponent();
            DataContext = IocManagerInstance.ResolveType<IDemoViewModel>();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            keybd_event(Keys.LWin, 0, 0, 0);
            keybd_event(Keys.D, 0, 0, 0);
            keybd_event(Keys.LWin, 0, KEYEVENTF_KEYUP, 0);
        }
    }
}
