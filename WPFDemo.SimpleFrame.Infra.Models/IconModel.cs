using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.Models
{
    public class IconModel
    {
        public string Source { get; set; }
        public string ToolTip { get; set; }
        public ICommand Command { get; set; }

        public IconModel()
        {

        }

        public IconModel(string source)
        {
            Source = source;
        }

        public IconModel(string source, string toolTip)
        {
            Source = source;
            ToolTip = toolTip;
        }

        public IconModel(string source, string toolTip, ICommand command)
        {
            Source = source;
            ToolTip = toolTip;
            Command = command;
        }
    }
}
