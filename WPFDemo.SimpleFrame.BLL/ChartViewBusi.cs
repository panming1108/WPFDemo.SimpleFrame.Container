using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo.SimpleFrame.IBLL;

namespace WPFDemo.SimpleFrame.BLL
{
    public class ChartViewBusi : IChartViewBusi
    {

        public Task<Dictionary<string, double>> GetChartDatas()
        {
            return Task.Factory.StartNew(
                () =>
                {
                    Dictionary<string, double> datas = new Dictionary<string, double>();
                    Random random = new Random();
                    for (int i = 1; i <= 30; i++)
                    {
                        double data = random.Next(1, 12) * 10000;
                        datas.Add(i.ToString(), data);
                    }
                    return datas;
                });
        }
    }
}
