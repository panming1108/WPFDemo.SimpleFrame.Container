using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.IDAL;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.BLL
{
    public class StudentBusi : IStudentBusi
    {
        private IStudentDAL _studentDAL;

        public StudentBusi(IStudentDAL studentDAL)
        {
            _studentDAL = studentDAL;
        }

        public async Task<QueryResult> GetStudents(int pageNo, int pageSize)
        {
            await TaskEx.Delay(500);
            QueryResult queryResult = new QueryResult();
            List<Student> students = new List<Student>();
            int start = pageSize * (pageNo - 1);
            int end = start + pageSize;
            if (end >= 9999)
            {
                end = 9999;
            }
            for (int i = start; i < end; i++)
            {
                Student student = new Student()
                {
                    Id = i,
                    Name = "Testaksjdfhkjashdfkjashkdfhkaskjasdhfk" + i * 10,
                    Age = i * 10,
                    IsEnabled = i % 4 != 0
                };
                students.Add(student);
            }
            queryResult.Students = students;
            queryResult.PageSize = pageSize;
            queryResult.PageNo = pageNo;

            return queryResult;
        }

        public async Task<List<Student>> GetStudents()
        {
            return await _studentDAL.GetStudents();
        }
    }
}
