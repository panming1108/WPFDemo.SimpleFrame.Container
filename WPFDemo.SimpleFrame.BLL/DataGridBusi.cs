using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.Infra.Enums;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.BLL
{
    public class DataGridBusi : IDataGridBusi
    {
        public List<DataGridModel> GetDataGridSource()
        {
            List<DataGridModel> persons = new List<DataGridModel>();
            for (int i = 0; i < 4; i++)
            {
                DataGridModel person = new DataGridModel()
                {
                    ID = i,
                    Avatar = i * 100,
                    Number = i * 10010,
                    Name = "测试" + i,
                    Product = "Product" + i,
                    Quantity = 300 + i,
                    Status = (StatusEnum)i,
                };
                persons.Add(person);
            }
            return persons;
        }
    }
}
