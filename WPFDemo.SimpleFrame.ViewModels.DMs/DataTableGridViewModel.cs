using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.IViewModels.DMs;

namespace WPFDemo.SimpleFrame.ViewModels.DMs
{
    public class DataTableGridViewModel : BasicDataGridViewModel, IDataTableGridViewModel
    {
        public DataTableGridViewModel(IDataGridBusi dataGridBusi) : base(dataGridBusi)
        {
        }
    }
}
