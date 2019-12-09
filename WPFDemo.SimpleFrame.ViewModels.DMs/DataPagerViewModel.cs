using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.Infra.Models;
using WPFDemo.SimpleFrame.Infra.MVVM.VMOnly;
using WPFDemo.SimpleFrame.IViewModels.DMs;

namespace WPFDemo.SimpleFrame.ViewModels.DMs
{
    public class DataPagerViewModel : DataPagerQueryViewModel<Student>, IDataPagerViewModel
    {
        private IStudentBusi _studentBusi;       

        public DataPagerViewModel(IStudentBusi studentBusi)
        {
            _studentBusi = studentBusi;
            PageSize = 10;
            PageSizeSource = new int[] { 10, 20, 30 };
        }

        protected override async Task PageSearch(int pageSize, int pageNo)
        {
            //var result = await _studentBusi.GetStudents(pageNo, pageSize);
            //ItemCount = 9999;
            //PageSize = result.PageSize;
            //PageNo = result.PageNo;
            //DataSource = result.Students;
            var result = await _studentBusi.GetStudents();
            ItemCount = 9999;
            PageSize = 10;
            PageNo = 1;
            DataSource = result;
        }

        protected async override Task Loaded()
        {
            await PageSearch(PageSize, PageNo);
        }

        protected async override Task UnLoaded()
        {
            await TaskEx.FromResult(0);
        }
    }
}
