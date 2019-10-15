using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFDemo.SimpleFrame.IBLL
{
    public interface IChartViewBusi
    {
        Task<Dictionary<DateTime, double>> GetBigChartDatas();
        Task<Dictionary<DateTime, double>> GetBigAverageDatas();

        Task<Dictionary<DateTime, double>> GetSmallChartDatas();
        Task<Dictionary<DateTime, double>> GetSmallAverageDatas();

        Task<Dictionary<DateTime, double>> GetFixChartDatas();
        Task<Dictionary<DateTime, double>> GetFixAverageDatas();
    }
}
