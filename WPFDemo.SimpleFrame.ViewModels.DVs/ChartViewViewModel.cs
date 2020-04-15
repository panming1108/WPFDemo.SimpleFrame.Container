using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.Infra.MVVM.VMOnly;
using WPFDemo.SimpleFrame.IViewModels.DVs;

namespace WPFDemo.SimpleFrame.ViewModels.DVs
{
    public class ChartViewViewModel : BaseViewModel, IChartViewViewModel
    {
        private IChartViewBusi _chartViewBusi;
        private Dictionary<DateTime, double> _smallChartData;
        private Dictionary<DateTime, double> _smallAverageData;
        private Dictionary<DateTime, double> _bigChartData;
        private Dictionary<DateTime, double> _bigAverageData;
        private Dictionary<DateTime, double> _fixChartData;
        private Dictionary<DateTime, double> _fixAverageData;
        public Dictionary<DateTime, double> SmallChartData
        {
            get => _smallChartData;
            set
            {
                _smallChartData = value;
                OnPropertyChanged(() => SmallChartData);
            }
        }
        public Dictionary<DateTime, double> SmallAverageData
        {
            get => _smallAverageData;
            set
            {
                _smallAverageData = value;
                OnPropertyChanged(() => SmallAverageData);
            }
        }
        public Dictionary<DateTime, double> BigChartData
        {
            get => _bigChartData;
            set
            {
                _bigChartData = value;
                OnPropertyChanged(() => BigChartData);
            }
        }
        public Dictionary<DateTime, double> BigAverageData
        {
            get => _bigAverageData;
            set
            {
                _bigAverageData = value;
                OnPropertyChanged(() => BigAverageData);
            }
        }

        public Dictionary<DateTime, double> FixChartData
        {
            get => _fixChartData;
            set
            {
                _fixChartData = value;
                OnPropertyChanged(() => FixChartData);
            }
        }
        public Dictionary<DateTime, double> FixAverageData
        {
            get => _fixAverageData;
            set
            {
                _fixAverageData = value;
                OnPropertyChanged(() => FixAverageData);
            }
        }
        public ICommand SmallChartDataChangeCommand { get; set; }
        public ICommand BigChartDataChangeCommand { get; set; }

        private Dictionary<string, double> _waveSource;
        public Dictionary<string, double> WaveSource
        {
            get => _waveSource;
            set
            {
                _waveSource = value;
                OnPropertyChanged(() => WaveSource);
            }
        }

        public ChartViewViewModel(IChartViewBusi chartViewBusi)
        {
            _chartViewBusi = chartViewBusi;
            _waveSource = new Dictionary<string, double>();
            InitCommands();
        }

        private void InitCommands()
        {
            SmallChartDataChangeCommand = new AsyncDelegateCommand(OnSmallChartDataChanged);
            BigChartDataChangeCommand = new AsyncDelegateCommand(OnBigChartDataChanged);
        }

        private async Task OnFixChartDataChanged()
        {
            FixChartData = await _chartViewBusi.GetFixChartDatas();
            FixAverageData = await _chartViewBusi.GetFixAverageDatas();
        }

        private async Task OnBigChartDataChanged()
        {
            BigChartData = await _chartViewBusi.GetBigChartDatas();
            BigAverageData = await _chartViewBusi.GetBigAverageDatas();
        }

        private async Task OnSmallChartDataChanged()
        {
            SmallChartData = await _chartViewBusi.GetSmallChartDatas();
            SmallAverageData = await _chartViewBusi.GetSmallAverageDatas();
        }

        protected async override Task Loaded()
        {
            await OnSmallChartDataChanged();
            await OnFixChartDataChanged();
            await OnBigChartDataChanged();

            Dictionary<string, double> keyValuePairs = new Dictionary<string, double>();
            foreach (var item in BigChartData)
            {
                keyValuePairs.Add(item.Key.ToString("dd"), item.Value);
            }
            WaveSource = keyValuePairs;
        }

        protected async override Task UnLoaded()
        {
            await TaskEx.FromResult(0);
        }
    }
}
