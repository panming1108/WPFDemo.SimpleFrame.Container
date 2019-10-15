using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.IBLL
{
    public interface IDataGridBusi
    {
        Task<List<DataGridModel>> GetDataGridSource();
    }
}
