using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Infra.Models
{
    public class QueryResult
    {
        public List<Student> Students { get; set; }
        public int PageSize { get; set; }
        public int PageNo { get; set; }
    }
}
