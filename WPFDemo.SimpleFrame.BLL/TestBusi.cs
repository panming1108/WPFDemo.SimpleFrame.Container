using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFDemo.SimpleFrame.IBLL;

namespace WPFDemo.SimpleFrame.BLL
{
    public class TestBusi : ITestBusi
    {
        public string GetTestString()
        {
            return "TestBusiString" + DateTime.Now;
        }
    }
}
