using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.Infra.MVVM;
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

        public ChartViewViewModel(IChartViewBusi chartViewBusi)
        {
            _chartViewBusi = chartViewBusi;
            InitCommands();
        }

        private void InitCommands()
        {
            SmallChartDataChangeCommand = new DelegateCommand(OnSmallChartDataChanged);
            BigChartDataChangeCommand = new DelegateCommand(OnBigChartDataChanged);
        }

        private void OnFixChartDataChanged()
        {
            FixChartData = _chartViewBusi.GetFixChartDatas();
            FixAverageData = _chartViewBusi.GetFixAverageDatas();
        }

        private void OnBigChartDataChanged()
        {
            BigChartData = _chartViewBusi.GetBigChartDatas();
            BigAverageData = _chartViewBusi.GetBigAverageDatas();
        }

        private void OnSmallChartDataChanged()
        {
            SmallChartData = _chartViewBusi.GetSmallChartDatas();
            SmallAverageData = _chartViewBusi.GetSmallAverageDatas();
        }

        protected override void Loaded()
        {
            OnSmallChartDataChanged();
            OnFixChartDataChanged();
            OnBigChartDataChanged();
        }

        protected override void UnLoaded()
        {
            
        }
    }
}
