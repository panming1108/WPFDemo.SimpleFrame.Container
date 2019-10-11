using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.BLL
{
    public class StudentBusi : IStudentBusi
    {
        public QueryResult GetStudents(int pageNo, int pageSize)
        {
            QueryResult queryResult = new QueryResult();
            List<Student> students = new List<Student>();
            SqlConnectionStringBuilder sqlConnectionString = new SqlConnectionStringBuilder()
            {
                DataSource = ".",
                InitialCatalog = "Test",
                UserID = "sa",
                Password = "83894680pm"
            };
            SqlConnection con = new SqlConnection(sqlConnectionString.ConnectionString);
            con.Open();
            string sql = string.Format("select top {0} * from(select row_number() over(order by a.id asc) as rownumber, * from a) temp_row where rownumber > ({1} - 1) * {0}; ", pageSize, pageNo);
            SqlCommand command = new SqlCommand(sql, con);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Student student = new Student()
                {
                    Id = (int)reader["id"],
                    Name = (string)reader["name"],
                    Age = (int)reader["age"]
                };
                students.Add(student);
            }
            queryResult.Students = students;
            queryResult.PageSize = pageSize;
            queryResult.PageNo = pageNo;
            reader.Close();
            con.Close();
            return queryResult;
        }
    }
}
