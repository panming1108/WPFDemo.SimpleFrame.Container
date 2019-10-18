using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.IViewModels.LayOut
{
    public interface ITreeViewViewModel
    {
        List<TreeViewNode> IconNodes { get; set; }
        List<TreeViewNode> ImageNodes { get; set; }
    }
}
