using Newtonsoft.Json.Linq;
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
using WPFDemo.SimpleFrame.Infra.Helper;
using WPFDemo.SimpleFrame.Infra.Ioc;

namespace WPFDemo.SimpleFrame.Container
{
    /// <summary>
    /// HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            DataContext = IocManagerInstance.ResolveType<IHomePageViewModel>();
            //string text = "{\"data\":{\"name\":\"PC端对接诊断中心\",\"code\":\"pc_diag_01\",\"contact\":null,\"phone\":null,\"orgId\":3000000000002220,\"passportId\":4573859613029888,\"dispatchTarget\":1,\"id\":1000000000000126,\"remark\":\"\",\"createUser\":0,\"createUserName\":\"\",\"createTime\":\"2019-09-09T11:53:03.797\",\"updateUser\":0,\"updateUserName\":\"\",\"updateTime\":\"2019-09-09T11:53:03.797\",\"status\":0,\"rowVersion\":4573859170547201},\"code\":0,\"msg\":\"\",\"serverTime\":637093519665998191}";
            //var jobj = JObject.Parse(text);

            ////创建TreeView的数据源

            //treeView.ItemsSource = jobj.Children().Select(c => JsonHeaderLogic.FromJToken(c));
            LogHelper.Info("Test Log");
        }
    }
}
