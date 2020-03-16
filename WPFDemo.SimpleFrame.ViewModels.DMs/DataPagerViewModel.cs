using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.Infra.Enums;
using WPFDemo.SimpleFrame.Infra.Messager;
using WPFDemo.SimpleFrame.Infra.Models;
using WPFDemo.SimpleFrame.Infra.MVVM.VMOnly;
using WPFDemo.SimpleFrame.IViewModels.DMs;

namespace WPFDemo.SimpleFrame.ViewModels.DMs
{
    public class DataPagerViewModel : BaseViewModel, IDataPagerViewModel
    {
        private IStudentBusi _studentBusi;
        private List<Student> _dataSource;
        private int _pageNo;
        private int _pageSize;
        private int _itemCount;
        private int[] _pageSizeSource;

        public int[] PageSizeSource
        {
            get => _pageSizeSource;
            set
            {
                _pageSizeSource = value;
                OnPropertyChanged(() => PageSizeSource);
            }
        }

        public List<Student> DataSource
        {
            get => _dataSource;
            set
            {
                _dataSource = value;
                OnPropertyChanged(() => DataSource);
            }
        }

        public int ItemCount
        {
            get { return _itemCount; }
            set
            {
                _itemCount = value;
                OnPropertyChanged(() => ItemCount);
            }
        }
        public int PageNo
        {
            get { return _pageNo; }
            set
            {
                if (_pageNo != value)
                {
                    _pageNo = value;
                    OnPropertyChanged(() => PageNo);
                }
            }
        }

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                if (_pageSize != value)
                {
                    _pageSize = value;
                    OnPropertyChanged(() => PageSize);
                }
            }
        }

        public Func<int, int, Task> SearchCallBack { get; set; }

        public DataPagerViewModel(IStudentBusi studentBusi)
        {
            _studentBusi = studentBusi;
            _pageNo = 1;
            _dataSource = new List<Student>();
            _pageSize = 10;
            PageSizeSource = new int[] { 10, 20, 30 };
            SearchCallBack = new Func<int, int, Task>(OnSearchCallBack);
        }

        private async Task OnSearchCallBack(int pageNo, int pageSize)
        {
            await PageSearch(pageNo, pageSize);
        }

        protected async Task PageSearch(int pageNo, int pageSize)
        {
            //MessagerInstance.GetMessager().Send<BusyStateEnum>(MessagerKeyEnum.IsBusy, BusyStateEnum.IsBusy);
            var result = await _studentBusi.GetStudents(pageNo, pageSize);
            ItemCount = 9999;
            PageSize = result.PageSize;
            PageNo = result.PageNo;
            DataSource = result.Students;
            //MessagerInstance.GetMessager().Send<BusyStateEnum>(MessagerKeyEnum.IsBusy, BusyStateEnum.NotBusy);
            //var result = await _studentBusi.GetStudents();
            //ItemCount = 9999;
            //PageSize = 10;
            //PageNo = 1;
            //DataSource = result;
        }

        protected async override Task Loaded()
        {
            await PageSearch(PageNo, PageSize);
        }

        protected async override Task UnLoaded()
        {
            await TaskEx.FromResult(0);
        }
    }
}
