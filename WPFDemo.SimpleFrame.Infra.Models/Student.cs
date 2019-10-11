using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Infra.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public override string ToString()
        {
            return Id + "," + Name + "," + Age;
        }
    }
}
