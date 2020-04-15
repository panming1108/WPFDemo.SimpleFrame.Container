using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.IViewModels.DVs
{
    public interface IChartViewViewModel
    {
        ICommand DataChangeCommand { get; set; }
    }
}
