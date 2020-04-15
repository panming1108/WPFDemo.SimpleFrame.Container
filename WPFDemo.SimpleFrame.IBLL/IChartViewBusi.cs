using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFDemo.SimpleFrame.IBLL
{
    public interface IChartViewBusi
    {
        Task<Dictionary<string, double>> GetChartDatas();
    }
}
