using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.IViewModels.LayOut
{
    public interface ILayOutViewModel
    {
        ICommand ShowMVVMWindowCommand { get; set; }
    }
}
