using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.IViewModels.Navis
{
    public interface IMenuViewModel
    {
        List<TreeViewNode> MenuSource { get; set; }
    }
}
