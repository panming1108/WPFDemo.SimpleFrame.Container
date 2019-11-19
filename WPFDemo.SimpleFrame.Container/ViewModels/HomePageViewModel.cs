using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo.SimpleFrame.Container.IViewModels;
using WPFDemo.SimpleFrame.Infra.MVVM.VMOnly;

namespace WPFDemo.SimpleFrame.Container.ViewModels
{
    public class HomePageViewModel : BaseViewModel, IHomePageViewModel
    {
        private IEnumerable<JsonHeaderLogic> _source;
        public IEnumerable<JsonHeaderLogic> Source
        {
            get => _source;
            set
            {
                _source = value;
                OnPropertyChanged(() => Source);
            }
        }
        protected async override Task Loaded()
        {
            string text = "{\"data\":{\"name\":\"PC端对接诊断中心\",\"code\":\"pc_diag_01\",\"contact\":null,\"phone\":null,\"orgId\":3000000000002220,\"passportId\":4573859613029888,\"dispatchTarget\":1,\"id\":1000000000000126,\"remark\":\"\",\"createUser\":0,\"createUserName\":\"\",\"createTime\":\"2019-09-09T11:53:03.797\",\"updateUser\":0,\"updateUserName\":\"\",\"updateTime\":\"2019-09-09T11:53:03.797\",\"status\":0,\"rowVersion\":4573859170547201},\"code\":0,\"msg\":\"\",\"serverTime\":637093519665998191}";
            var jobj = JObject.Parse(text);

            Source = jobj.Children().Select(c => JsonHeaderLogic.FromJToken(c));
            await TaskEx.FromResult(0);
        }

        protected async override Task UnLoaded()
        {
            await TaskEx.FromResult(0);
        }
    }
}
