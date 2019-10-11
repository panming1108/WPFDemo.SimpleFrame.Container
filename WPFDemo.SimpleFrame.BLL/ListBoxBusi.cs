using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.BLL
{
    public class ListBoxBusi : IListBoxBusi
    {
        public List<ListBoxModel> GetListBoxSource()
        {
            List<ListBoxModel> messages = new List<ListBoxModel>();
            ListBoxModel message1 = new ListBoxModel()
            {
                Name = "陈木方",
                Age = 45,
                AgeUnit = "岁",
                Source = "急诊",
                Flag = "001001",
                CheckTime = DateTime.Now,
                CheckDepartment = "001检查机构",
            };
            ListBoxModel message2 = new ListBoxModel()
            {
                Name = "陈木方",
                Age = 45,
                AgeUnit = "岁",
                Source = "急诊",
                Flag = "001001",
                CheckTime = DateTime.Now,
                CheckDepartment = "001检查机构",
            };
            ListBoxModel message3 = new ListBoxModel()
            {
                Name = "陈木方",
                Age = 45,
                AgeUnit = "岁",
                Source = "急诊",
                Flag = "001001",
                CheckTime = DateTime.Now,
                CheckDepartment = "001检查机构",
            };
            ListBoxModel message4 = new ListBoxModel()
            {
                Name = "陈木方",
                Age = 45,
                AgeUnit = "岁",
                Source = "急诊",
                Flag = "001001",
                CheckTime = DateTime.Now,
                CheckDepartment = "001检查机构",
            };
            messages.Add(message1);
            messages.Add(message2);
            messages.Add(message3);
            messages.Add(message4);
            return messages;
        }
    }
}
