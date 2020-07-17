using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.Infra.Enums;
using WPFDemo.SimpleFrame.Infra.Messager;
using WPFDemo.SimpleFrame.Infra.Models;
using WPFDemo.SimpleFrame.Infra.MVVM.VMOnly;
using WPFDemo.SimpleFrame.Infra.Tools;
using WPFDemo.SimpleFrame.IViewModels.DMs;

namespace WPFDemo.SimpleFrame.ViewModels.DMs
{
    public class DataPagerViewModel : DataPagerQueryViewModel<Student>, IDataPagerViewModel
    {
        private readonly IStudentBusi _studentBusi;

        public DataPagerViewModel(IStudentBusi studentBusi)
        {
            _studentBusi = studentBusi;
        }

        protected async override Task Loaded()
        {
            await PageSearch(PageNo, PageSize);
        }

        protected async override Task UnLoaded()
        {
            await TaskEx.FromResult(0);
        }

        protected override async Task PageSearch(int pageNo, int pageSize)
        {
            var result = await _studentBusi.GetStudents(pageNo, pageSize);
            ItemCount = 9999;
            PageSize = result.PageSize;
            PageNo = result.PageNo;
            DataSource = result.Students;
        }
    }
}
