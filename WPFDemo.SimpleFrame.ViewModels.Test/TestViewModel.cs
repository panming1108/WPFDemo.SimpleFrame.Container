using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.Infra.Models;
using WPFDemo.SimpleFrame.Infra.MVVM.VMOnly;
using WPFDemo.SimpleFrame.IViewModels.Test;

namespace WPFDemo.SimpleFrame.ViewModels.Test
{
    public class TestViewModel : DataPagerQueryViewModel<Student>, ITestViewModel
    {
        private IStudentBusi _studentBusi;

        public TestViewModel(IStudentBusi studentBusi)
        {
            _studentBusi = studentBusi;
            PageSize = 10;
            PageSizeSource = new int[] { 10, 20, 30 };
        }
        protected async override Task Loaded()
        {
            await PageSearch(PageSize, PageNo);
        }

        protected async override Task UnLoaded()
        {
            await TaskEx.FromResult(0);
        }

        protected async override Task PageSearch(int pageSize, int pageNo)
        {
            var result = await _studentBusi.GetStudents(pageNo, pageSize);
            ItemCount = 9999;
            PageSize = result.PageSize;
            PageNo = result.PageNo;
            DataSource = result.Students;
        }
    }
}
