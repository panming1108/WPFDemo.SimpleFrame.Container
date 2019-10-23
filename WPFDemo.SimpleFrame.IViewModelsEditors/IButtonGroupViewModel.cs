using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.IViewModelsEditors
{
    public interface IButtonGroupViewModel
    {
        List<string> RadioButtonsSource { get; set; }
        List<string> CheckBoxsSource { get; set; }
    }
}
