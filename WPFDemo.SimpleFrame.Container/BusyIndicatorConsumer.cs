using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFDemo.SimpleFrame.Infra.CustomControls.UXs.BusyIndicator;
using WPFDemo.SimpleFrame.Infra.Enums;
using WPFDemo.SimpleFrame.Infra.Messager;
using WPFDemo.SimpleFrame.Infra.MVVM;

namespace WPFDemo.SimpleFrame.Container
{
    public class BusyIndicatorConsumer : IBusyIndicatorConsumer
    {
        private readonly IEMCBusyIndicator _busyIndicator;

        public BusyIndicatorConsumer(IEMCBusyIndicator busyIndicator)
        {
            _busyIndicator = busyIndicator;
            MessagerInstance.GetMessager().Register<BusyStateEnum>(this, MessagerKeyEnum.IsBusy, OnBusy);
        }

        private async Task OnBusy(BusyStateEnum arg)
        {
            switch (arg)
            {
                case BusyStateEnum.None:
                    throw new Exception("Busy BusyStatus.None");
                case BusyStateEnum.IsBusy:
                    _busyIndicator.Show();
                    break;
                case BusyStateEnum.NotBusy:
                    _busyIndicator.Hide();
                    break;
                default:
                    break;
            }
            await TaskEx.FromResult(0);
        }

        public void Init(Window owner)
        {
            _busyIndicator.WindowOwner = owner;
        }

        public void Dispose()
        {
            MessagerInstance.GetMessager().Unregister<BusyStateEnum>(this, MessagerKeyEnum.IsBusy, OnBusy);
        }
    }
}
