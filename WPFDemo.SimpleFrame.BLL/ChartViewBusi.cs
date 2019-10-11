using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFDemo.SimpleFrame.IBLL;

namespace WPFDemo.SimpleFrame.BLL
{
    public class ChartViewBusi : IChartViewBusi
    {
        public Dictionary<DateTime, double> GetBigAverageDatas()
        {
            return GenerateBigData();
        }

        public Dictionary<DateTime, double> GetBigChartDatas()
        {
            return GenerateBigData();
        }

        public Dictionary<DateTime, double> GetFixAverageDatas()
        {
            Dictionary<DateTime, double> averageData = new Dictionary<DateTime, double>()
            {
                { System.DateTime.Now.AddDays(1) , 936.32 },
                { System.DateTime.Now.AddDays(2) , 3468.62 },
                { System.DateTime.Now.AddDays(3) , 0 },
                { System.DateTime.Now.AddDays(4) , 0 },
                { System.DateTime.Now.AddDays(5) , 0 },
                { System.DateTime.Now.AddDays(6) , 0 },
                { System.DateTime.Now.AddDays(7) , 0 },
                { System.DateTime.Now.AddDays(8) , 0 },
                { System.DateTime.Now.AddDays(9) , 0 },
                { System.DateTime.Now.AddDays(10) , 0 },
                { System.DateTime.Now.AddDays(11) , 0 },
                { System.DateTime.Now.AddDays(12) , 0 },
                { System.DateTime.Now.AddDays(13) , 0 },
                { System.DateTime.Now.AddDays(14) , 0 },
                { System.DateTime.Now.AddDays(15) , 0 },
                { System.DateTime.Now.AddDays(16) , 0 },
                { System.DateTime.Now.AddDays(17) , 0 },
                { System.DateTime.Now.AddDays(18) , 0 },
                { System.DateTime.Now.AddDays(19) , 0 },
                { System.DateTime.Now.AddDays(20) , 0 },
                { System.DateTime.Now.AddDays(21) , 0 },
                { System.DateTime.Now.AddDays(22) , 0 },
                { System.DateTime.Now.AddDays(23) , 0 },
                { System.DateTime.Now.AddDays(24) , 0 },
                { System.DateTime.Now.AddDays(25) , 0 },
                { System.DateTime.Now.AddDays(26) , 0 },
                { System.DateTime.Now.AddDays(27) , 0 },
                { System.DateTime.Now.AddDays(28) , 0 },
                { System.DateTime.Now.AddDays(29) , 0 },
                { System.DateTime.Now.AddDays(30) , 0 },
            };
            return averageData;
        }

        public Dictionary<DateTime, double> GetFixChartDatas()
        {
            Dictionary<DateTime, double> datas = new Dictionary<DateTime, double>()
            {
                { System.DateTime.Now.AddDays(1) , 524.37 },
                { System.DateTime.Now.AddDays(2) , 12370.57 },
                { System.DateTime.Now.AddDays(3) , 0 },
                { System.DateTime.Now.AddDays(4) , 0 },
                { System.DateTime.Now.AddDays(5) , 10 },
                { System.DateTime.Now.AddDays(6) , 0 },
                { System.DateTime.Now.AddDays(7) , 0 },
                { System.DateTime.Now.AddDays(8) , 20 },
                { System.DateTime.Now.AddDays(9) , 0 },
                { System.DateTime.Now.AddDays(10) , 0 },
                { System.DateTime.Now.AddDays(11) , 0 },
                { System.DateTime.Now.AddDays(12) , 0 },
                { System.DateTime.Now.AddDays(13) , 30 },
                { System.DateTime.Now.AddDays(14) , 0 },
                { System.DateTime.Now.AddDays(15) , 0 },
                { System.DateTime.Now.AddDays(16) , 0 },
                { System.DateTime.Now.AddDays(17) , 0 },
                { System.DateTime.Now.AddDays(18) , 0 },
                { System.DateTime.Now.AddDays(19) , 50 },
                { System.DateTime.Now.AddDays(20) , 0 },
                { System.DateTime.Now.AddDays(21) , 0 },
                { System.DateTime.Now.AddDays(22) , 0 },
                { System.DateTime.Now.AddDays(23) , 0 },
                { System.DateTime.Now.AddDays(24) , 0 },
                { System.DateTime.Now.AddDays(25) , 0 },
                { System.DateTime.Now.AddDays(26) , 0 },
                { System.DateTime.Now.AddDays(27) , 0 },
                { System.DateTime.Now.AddDays(28) , 0 },
                { System.DateTime.Now.AddDays(29) , 0 },
                { System.DateTime.Now.AddDays(30) , 0 },
            };
            return datas;
        }

        public Dictionary<DateTime, double> GetSmallAverageDatas()
        {
            return GenerateSmallData();
        }

        public Dictionary<DateTime, double> GetSmallChartDatas()
        {
            return GenerateSmallData();
        }

        private Dictionary<DateTime, double> GenerateSmallData()
        {
            Dictionary<DateTime, double> datas = new Dictionary<DateTime, double>();
            Random random = new Random();
            for (int i = 1; i <= 30; i++)
            {
                DateTime dateTime = System.DateTime.Now.AddDays(i);
                double data = random.NextDouble() + 1;
                datas.Add(dateTime, data);
            }
            return datas;
        }

        private Dictionary<DateTime, double> GenerateBigData()
        {
            Dictionary<DateTime, double> datas = new Dictionary<DateTime, double>();
            Random random = new Random();
            for (int i = 1; i <= 30; i++)
            {
                DateTime dateTime = System.DateTime.Now.AddDays(i);
                double data = random.Next(1, 12) * 10000;
                datas.Add(dateTime, data);
            }
            return datas;
        }
    }
}
