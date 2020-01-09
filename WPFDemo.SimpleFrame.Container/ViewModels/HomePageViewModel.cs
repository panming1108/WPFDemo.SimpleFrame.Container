using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFDemo.SimpleFrame.Container.IViewModels;
using WPFDemo.SimpleFrame.Infra.MVVM.VMOnly;

namespace WPFDemo.SimpleFrame.Container.ViewModels
{
    public class HomePageViewModel : BaseViewModel, IHomePageViewModel
    {
        private string[] _angle = { "A", "B", "C", "E", "F", "G", "H" };
        private string[] _leng = { "B", "C", "D", "E", "F", "G", "H", "W", "X", "Y", "Z" };

        private string _angleText;
        public string AngleText
        {
            get => _angleText;
            set
            {
                _angleText = value;
                OnPropertyChanged(() => AngleText);
            }
        }

        private string _lengText;
        public string LengText
        {
            get => _lengText;
            set
            {
                _lengText = value;
                OnPropertyChanged(() => LengText);
            }
        }

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

        public ICommand RefreshCommand { get; set; }

        public HomePageViewModel()
        {
            RefreshCommand = new AsyncDelegateCommand(OnRefresh);
        }

        private async Task OnRefresh()
        {
            await TaskEx.FromResult(0);
            AngleText = GenerateText(_angle) + "C";
            LengText = GenerateText(_leng) + "D";
        }

        private string GenerateText(string[] data)
        {
            string[] copy = (string[])data.Clone();
            string[] newarr = new string[copy.Length];
            int k = 0;
            while (k < copy.Length)
            {
                int temp = new Random().Next(0, copy.Length);
                if (copy[temp] != "")
                {
                    newarr[k] = copy[temp];
                    k++;
                    copy[temp] = "";
                }
            }
            string result = "";
            for (int i = 0; i < newarr.Length; i++)
            {
                result += newarr[i];
            }
            return result;
        }

        protected async override Task Loaded()
        {
            string text = "{\"data\":{\"name\":\"PC端对接诊断中心\",\"code\":\"pc_diag_01\",\"contact\":null,\"phone\":null,\"orgId\":3000000000002220,\"passportId\":4573859613029888,\"dispatchTarget\":1,\"id\":1000000000000126,\"remark\":\"\",\"createUser\":0,\"createUserName\":\"\",\"createTime\":\"2019-09-09T11:53:03.797\",\"updateUser\":0,\"updateUserName\":\"\",\"updateTime\":\"2019-09-09T11:53:03.797\",\"status\":0,\"rowVersion\":4573859170547201},\"code\":0,\"msg\":\"\",\"serverTime\":637093519665998191}";
            var jobj = JObject.Parse(text);
            await OnRefresh();
            Source = jobj.Children().Select(c => JsonHeaderLogic.FromJToken(c));
            await TaskEx.FromResult(0);
        }

        protected async override Task UnLoaded()
        {
            await TaskEx.FromResult(0);
        }
    }
}
