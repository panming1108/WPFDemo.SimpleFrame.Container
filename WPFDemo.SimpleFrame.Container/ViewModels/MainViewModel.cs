using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFDemo.SimpleFrame.Container.IViewModels;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.Infra.Enums;
using WPFDemo.SimpleFrame.Infra.Helper;
using WPFDemo.SimpleFrame.Infra.Messager;
using WPFDemo.SimpleFrame.Infra.MVVM.VMOnly;

namespace WPFDemo.SimpleFrame.Container.ViewModels
{
    public class MainViewModel : BaseViewModel, IMainViewModel
    {
        public ICommand PageNaviCommand { get; set; }

        private List<EnumStructInfo> _pageNaviSource;
        public List<EnumStructInfo> PageNaviSource
        {
            get => _pageNaviSource;
            set
            {
                _pageNaviSource = value;
                OnPropertyChanged(() => PageNaviSource);
            }
        }

        public MainViewModel()
        {
            PageNaviSource = typeof(PageKeyEnum).GetEnumStructInfo();
            InitCommands();           
        }

        private void InitCommands()
        {
            PageNaviCommand = new DelegateCommand<string>(OnPageNavi);
        }

        private void OnPageNavi(string pageNaviId)
        {            
            MessagerInstance.GetMessager().Send(MessagerKeyEnum.MainPageNavi, (PageKeyEnum)int.Parse(pageNaviId));
        }

        protected async override Task Loaded()
        {
            await TaskEx.FromResult(0);
        }

        protected async override Task UnLoaded()
        {
            await TaskEx.FromResult(0);
        }

    }
}
