using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.IViewModels.DMs
{
    public interface IBasicDataGridViewModel
    {
        List<DataGridModel> DataGridSource { get; set; }
    }
}
