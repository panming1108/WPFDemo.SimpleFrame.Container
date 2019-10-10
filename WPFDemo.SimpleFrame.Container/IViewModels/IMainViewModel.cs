using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Container.IViewModels
{
    public interface IMainViewModel
    {
        ICommand PageNaviCommand { get; set; }
    }
}
