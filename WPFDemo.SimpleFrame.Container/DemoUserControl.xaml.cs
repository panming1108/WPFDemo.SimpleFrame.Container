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
using WPFDemo.SimpleFrame.Container.IViewModels;
using WPFDemo.SimpleFrame.Infra.Ioc;

namespace WPFDemo.SimpleFrame.Container
{
    /// <summary>
    /// DemoUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class DemoUserControl : UserControl
    {
        public DemoUserControl()
        {
            InitializeComponent();
            DataContext = IocManagerInstance.ResolveType<IDemoViewModel>();
        }
    }
}
