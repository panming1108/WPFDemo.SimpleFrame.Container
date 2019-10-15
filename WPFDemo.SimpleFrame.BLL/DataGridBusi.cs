using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.Infra.Enums;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.BLL
{
    public class DataGridBusi : IDataGridBusi
    {
        public Task<List<DataGridModel>> GetDataGridSource()
        {
            return Task.Factory.StartNew(
                ()=> 
                {
                    List<DataGridModel> persons = new List<DataGridModel>();
                    for (int i = 0; i < 5; i++)
                    {
                        DataGridModel person = new DataGridModel()
                        {
                            ID = i,
                            Avatar = i * 100,
                            Number = i * 10010,
                            Name = "测试" + i,
                            Product = "Product" + i,
                            Quantity = 300 + i,
                            Status = (StatusEnum)(i % 4),
                        };
                        persons.Add(person);
                    }
                    return persons;
                });
        }
    }
}
