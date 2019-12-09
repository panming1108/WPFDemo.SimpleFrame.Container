﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo.SimpleFrame.IDAL;
using WPFDemo.SimpleFrame.Infra.Models;
using WPFDemo.SimpleFrame.Infra.DALOnly.SQLiteHelper;
using System.Configuration;

namespace WPFDemo.SimpleFrame.DAL
{
    public class StudentDAL : DbBaseManagement, IStudentDAL
    {
        public StudentDAL()
        {
            DBPath = ConfigurationManager.AppSettings["DBSourceUri"].ToString();
        }

        public async Task<List<Student>> GetStudents()
        {
            string sql = "select * from TbStudent;";
            var result = await SqlQueryList<Student>(sql);
            return result.ToList();
        }
    }
}