using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.IViewModels.Test
{
    public interface ITestViewModel
    {
        List<Student> GroupStudents { get; set; }
    }
}
