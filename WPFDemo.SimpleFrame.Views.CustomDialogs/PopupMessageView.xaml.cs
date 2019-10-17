using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using WPFDemo.SimpleFrame.Infra.CustomControls.UXs.DesktopAlert;
using WPFDemo.SimpleFrame.IViews.CustomDialogs;

namespace WPFDemo.SimpleFrame.Views.CustomDialogs
{
    /// <summary>
    /// PopupMessageView.xaml 的交互逻辑
    /// </summary>
    public partial class PopupMessageView : EMCPopupNotifyBox, IPopupMessageView
    {
        public PopupMessageView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.IsOpen = false;
        }
    }
}
