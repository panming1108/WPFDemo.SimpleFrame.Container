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

        public ICommand DataChangeCommand { get; set; }

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
            DataChangeCommand = new AsyncDelegateCommand(OnChartDataChanged);
        }

        private async Task OnChartDataChanged()
        {
            WaveSource = await _chartViewBusi.GetChartDatas();
        }

        protected async override Task Loaded()
        {
            await OnChartDataChanged();
        }

        protected async override Task UnLoaded()
        {
            await TaskEx.FromResult(0);
        }
    }
}
