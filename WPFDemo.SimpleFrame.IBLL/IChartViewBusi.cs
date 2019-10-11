using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.IBLL
{
    public interface IChartViewBusi
    {
        Dictionary<DateTime, double> GetBigChartDatas();
        Dictionary<DateTime, double> GetBigAverageDatas();

        Dictionary<DateTime, double> GetSmallChartDatas();
        Dictionary<DateTime, double> GetSmallAverageDatas();

        Dictionary<DateTime, double> GetFixChartDatas();
        Dictionary<DateTime, double> GetFixAverageDatas();
    }
}
