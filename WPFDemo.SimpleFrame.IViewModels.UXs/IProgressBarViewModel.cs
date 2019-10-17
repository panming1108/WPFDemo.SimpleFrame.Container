using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.IViewModels.UXs
{
    public interface IProgressBarViewModel
    {
        double Percent { get; set; }

        double Total { get; set; }

        double CurrentValue { get; set; }

        ICommand StartCommand { get; set; }

        ICommand ResetCommand { get; set; }
    }
}
