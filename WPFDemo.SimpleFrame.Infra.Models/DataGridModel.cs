using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFDemo.SimpleFrame.Infra.Enums;

namespace WPFDemo.SimpleFrame.Infra.Models
{
    public class DataGridModel
    {
        public int ID { get; set; }
        public int Avatar { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public StatusEnum Status { get; set; }
    }
}
