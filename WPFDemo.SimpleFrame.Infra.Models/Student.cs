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
        public bool IsEnabled { get; set; }
        public Student Parent { get; set; }
        public int ParentId { get; set; }

        public Student()
        {

        }

        public Student(string name)
        {
            Name = name;
        }

        public Student(int id, string name, int age)
        {
            Id = id;
            Name = name;
            Age = age;
        }

        public Student(int id, string name, int age, bool isEnabled)
        {
            Id = id;
            Name = name;
            Age = age;
            IsEnabled = isEnabled;
        }


        public override string ToString()
        {
            return Id + "," + Name + "," + Age + "," + IsEnabled;
        }
    }
}
