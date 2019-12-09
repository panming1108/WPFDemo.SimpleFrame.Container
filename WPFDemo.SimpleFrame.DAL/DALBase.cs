using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using WPFDemo.SimpleFrame.Infra.DALOnly.SQLiteHelper;

namespace WPFDemo.SimpleFrame.DAL
{
    public class DALBase : DbBaseManagement
    {
        public DALBase()
        {
            DBPath = ConfigurationManager.AppSettings["DBSourceUri"].ToString();
        }
    }
}
