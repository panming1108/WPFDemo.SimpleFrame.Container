using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo.SimpleFrame.Container.IViewModels;
using WPFDemo.SimpleFrame.Infra.Enums;
using WPFDemo.SimpleFrame.Infra.Messager;
using WPFDemo.SimpleFrame.Infra.Models;
using WPFDemo.SimpleFrame.Infra.MVVM.VMOnly;

namespace WPFDemo.SimpleFrame.Container.ViewModels
{
    public class SchedulesViewModel : BaseViewModel, ISchedulesPageViewModel
    {
        private DateTime _testDateTime;
        public DateTime TestDateTime
        {
            get => _testDateTime;
            set
            {
                _testDateTime = value;
                OnDateTimeChanged(_testDateTime);
                OnPropertyChanged(() => TestDateTime);
            }
        }

        private void OnDateTimeChanged(DateTime testDateTime)
        {
            MessagerInstance.GetMessager().Send(MessagerKeyEnum.PopupNotifyBox, new PopupNotifyObject("通知", testDateTime.ToString()));
        }

        public SchedulesViewModel()
        {
            //_testDateTime = new DateTime(1997, 11, 8, 15, 23, 36);
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
