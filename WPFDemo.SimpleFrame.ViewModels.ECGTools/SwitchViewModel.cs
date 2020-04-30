using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WPFDemo.SimpleFrame.Infra.MVVM.VMOnly;
using WPFDemo.SimpleFrame.IViewModels.ECGTools;

namespace WPFDemo.SimpleFrame.ViewModels.ECGTools
{
    public class SwitchViewModel : BaseViewModel, ISwitchViewModel
    {
        private Timer _timer;

        private HRObject _hRObject;
        public HRObject HRObject
        {
            get => _hRObject;
            set
            {
                _hRObject = value;
                OnPropertyChanged(() => HRObject);
            }
        }

        public SwitchViewModel()
        {
            _hRObject = new HRObject();
            _timer = new Timer(100);
            _timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            HRObject.HR = new Random().Next(0,300);
        }

        protected override async Task Loaded()
        {
            _timer.Start();
            await TaskEx.FromResult(0);
        }

        protected override async Task UnLoaded()
        {
            _timer.Stop();
            _timer.Dispose();
            await TaskEx.FromResult(0);
        }
    }

    public class HRObject : INotifyPropertyChanged
    {
        private int _hR;
        public int HR
        {
            get => _hR;
            set
            {
                _hR = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HR"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
