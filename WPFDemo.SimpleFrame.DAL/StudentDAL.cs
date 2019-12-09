using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPFDemo.SimpleFrame.IDAL;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.DAL
{
    public class StudentDAL : DALBase, IStudentDAL
    {
        public async Task<List<Student>> GetStudents()
        {
            string sql = "select * from Student;";
            var result = await SqlQueryList<Student>(sql);
            return result.ToList();
        }
    }
}
