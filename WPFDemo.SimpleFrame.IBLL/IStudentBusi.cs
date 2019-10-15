using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.IBLL
{
    public interface IStudentBusi
    {
        Task<QueryResult> GetStudents(int pageNo, int pageSize);
    }
}
