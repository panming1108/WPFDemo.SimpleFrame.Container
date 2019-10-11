using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.IViewModels.DMs
{
    public interface IDataPagerViewModel
    {
        int PageNo { get; set; }
        int PageSize { get; set; }
        int ItemCount { get; set; }
        List<Student> Students { get; set; } 
    }
}
