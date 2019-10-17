using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using WPFDemo.SimpleFrame.Infra.Enums;
using WPFDemo.SimpleFrame.Infra.Messager;
using WPFDemo.SimpleFrame.Infra.Models;
using WPFDemo.SimpleFrame.Infra.MVVM.VMOnly;
using WPFDemo.SimpleFrame.IViewModels.UXs;

namespace WPFDemo.SimpleFrame.ViewModels.UXs
{
    public class ProgressBarViewModel : BaseViewModel, IProgressBarViewModel
    {
        private double _percent;
        private double _total;
        private double _currentValue;
        private System.Timers.Timer _timer;
        public double Percent
        {
            get => _percent;
            set
            {
                _percent = value;
                OnPropertyChanged(() => Percent);
            }
        }
        public double Total
        {
            get => _total;
            set
            {
                _total = value;
                OnPropertyChanged(() => Total);
            }
        }
        public double CurrentValue
        {
            get => _currentValue;
            set
            {
                _currentValue = value;
                OnPropertyChanged(() => CurrentValue);
            }
        }
        
        public ICommand StartCommand { get; set; }
        public ICommand ResetCommand { get; set; }

        public ProgressBarViewModel()
        {
            InitFields();
            StartCommand = new AsyncDelegateCommand(OnStart);
            ResetCommand = new AsyncDelegateCommand(OnReset);
        }

        private void InitFields()
        {
            _timer = new System.Timers.Timer(10);
            _timer.Elapsed += OnTimerChanged;
            CurrentValue = 0;
            Total = 1000;
            Percent = 0;
        }

        private void OnTimerChanged(object sender, ElapsedEventArgs e)
        {
            if(Total != 0)
            {
                if(CurrentValue >= Total)
                {                    
                    _timer.Stop();
                    MessagerInstance.GetMessager().Send(MessagerKeyEnum.PopupNotifyBox, new PopupNotifyObject("通知","结束！"));
                }
                else
                {
                    CurrentValue += 1;
                    Percent = Math.Round(CurrentValue / Total, 2) * 100;
                }
            }
        }
        private async Task OnReset()
        {
            await TaskEx.FromResult(0);
            InitFields();
        }

        private async Task OnStart()
        {
            await TaskEx.FromResult(0);       
            _timer.Start();
        }

        protected async override Task Loaded()
        {
            await TaskEx.FromResult(0);
        }

        protected async override Task UnLoaded()
        {
            await TaskEx.FromResult(0);
            _timer.Dispose();
        }
    }
}
