using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.IDAL
{
    public interface IStudentDAL
    {
        Task<List<Student>> GetStudents();
    }
}
