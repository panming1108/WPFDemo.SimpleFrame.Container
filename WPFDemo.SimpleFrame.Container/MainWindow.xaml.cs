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
using WPFDemo.SimpleFrame.Container.IViewModels;
using WPFDemo.SimpleFrame.Infra.Cache;
using WPFDemo.SimpleFrame.Infra.DialogConsumer.Interfaces;
using WPFDemo.SimpleFrame.Infra.Enums;
using WPFDemo.SimpleFrame.Infra.Ioc;
using WPFDemo.SimpleFrame.Infra.Messager;
using WPFDemo.SimpleFrame.Infra.MVVM;

namespace WPFDemo.SimpleFrame.Container
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBusyIndicatorConsumer _busyIndicatorConsumer;
        private IPopupNotifyBoxConsumer _popupNotifyBoxConsumer;
        public MainWindow()
        {
            InitializeComponent();
            Top = 0;
            Left = 0;
            this.Width = SystemParameters.WorkArea.Width;
            this.Height = SystemParameters.WorkArea.Height;
            CacheManagerInstance.Init(new CacheManager());
            MessagerInstance.Init(new Messager());

            _busyIndicatorConsumer = IocManagerInstance.ResolveType<IBusyIndicatorConsumer>();
            _popupNotifyBoxConsumer = IocManagerInstance.ResolveType<IPopupNotifyBoxConsumer>();
            MessagerInstance.GetMessager().Register<PageKeyEnum>(this, MessagerKeyEnum.MainPageNavi, Navi);
            DataContext = IocManagerInstance.ResolveType<IMainViewModel>();
        }

        private Uri _historyPage;
        private Uri _currentPage;

        private async Task Navi(PageKeyEnum naviKey)
        {
            var uri = Mapping(naviKey);
            if(frame.Source != uri)
            {
                _historyPage = _currentPage;
                _currentPage = uri;
                frame.Source = uri;
            }
            await TaskEx.FromResult(0);
        }

        private Uri Mapping(PageKeyEnum naviKey)
        {
            switch(naviKey)
            {
                case PageKeyEnum.HomePage:
                    return new Uri("HomePage.xaml", UriKind.RelativeOrAbsolute);
                case PageKeyEnum.NaviPage:
                    return new Uri("NaviPage.xaml", UriKind.RelativeOrAbsolute);
                case PageKeyEnum.TestPage:
                    return new Uri("TestPage.xaml", UriKind.RelativeOrAbsolute);
                default:
                    return new Uri("NaviPage.xaml", UriKind.RelativeOrAbsolute);
            }
        }

        private void Close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Min_btn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _busyIndicatorConsumer.Init(this);
            _popupNotifyBoxConsumer.Init(this);
            MessagerInstance.GetMessager().Send(MessagerKeyEnum.MainPageNavi, PageKeyEnum.NaviPage);
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            MessagerInstance.GetMessager().Unregister<PageKeyEnum>(this, MessagerKeyEnum.MainPageNavi, Navi);
            Loaded -= Window_Loaded;
            Unloaded -= Window_Unloaded;
        }
    }
}
