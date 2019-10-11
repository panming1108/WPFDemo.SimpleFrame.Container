using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.IViewModels.DVs
{
    public interface IChartViewViewModel
    {
        Dictionary<DateTime, double> SmallChartData { get; set; }

        Dictionary<DateTime, double> SmallAverageData { get; set; }

        ICommand SmallChartDataChangeCommand { get; set; }

        Dictionary<DateTime, double> BigChartData { get; set; }

        Dictionary<DateTime, double> BigAverageData { get; set; }

        ICommand BigChartDataChangeCommand { get; set; }


        Dictionary<DateTime, double> FixChartData { get; set; }

        Dictionary<DateTime, double> FixAverageData { get; set; }

    }
}
