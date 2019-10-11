using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.IBLL
{
    public interface IStudentBusi
    {
        QueryResult GetStudents(int pageNo, int pageSize);
    }
}
