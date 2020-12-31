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
        private List<UploadInfo> _uploadInfos;
        private DateTime _startTime;
        private DateTime _endTime;
        private List<string> _marqueeSource;
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

        public List<string> MarqueeSource
        {
            get => _marqueeSource;
            set
            {
                _marqueeSource = value;
                OnPropertyChanged(() => MarqueeSource);
            }
        }

        public List<UploadInfo> UploadInfos
        {
            get => _uploadInfos;
            set
            {
                _uploadInfos = value;
                OnPropertyChanged(() => UploadInfos);
            }
        }

        public DateTime StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                OnPropertyChanged(() => StartTime);
            }
        }

        public DateTime EndTime
        {
            get => _endTime;
            set
            {
                _endTime = value;
                OnPropertyChanged(() => EndTime);
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
            _uploadInfos = new List<UploadInfo>();
            _marqueeSource = new List<string>();
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
            MarqueeSource = new List<string>() 
            { 
                "这是滚动数据一！！",
                "这是滚动数据二！！",
                "这是滚动数据三！！",
                "这是滚动数据四！！",
                "这是滚动数据五！！",
                "这是滚动数据六！！",
            };
            StartTime = new DateTime(2020, 12, 30, 0, 0, 0);
            EndTime = new DateTime(2020, 12, 31, 0, 0, 0);
            UploadInfos = new List<UploadInfo>()
            {
                new UploadInfo(){ StartTime = new DateTime(2020, 12, 30, 0, 0, 0), EndTime = new DateTime(2020, 12, 30, 12, 0, 0), IsUploaded = true },
                new UploadInfo(){ StartTime = new DateTime(2020, 12, 30, 12, 0, 0), EndTime = new DateTime(2020, 12, 30, 18, 0, 0), IsUploaded = false },
                new UploadInfo(){ StartTime = new DateTime(2020, 12, 30, 18, 0, 0), EndTime = new DateTime(2020, 12, 30, 21, 0, 0), IsUploaded = true },
                new UploadInfo(){ StartTime = new DateTime(2020, 12, 30, 21, 0, 0), EndTime = new DateTime(2020, 12, 31, 0, 0, 0), IsUploaded = false },
            };
            await TaskEx.FromResult(0);
        }

        protected async override Task UnLoaded()
        {
            await TaskEx.FromResult(0);
            _timer.Dispose();
        }
    }

    public class UploadInfo
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsUploaded { get; set; }
    }
}
