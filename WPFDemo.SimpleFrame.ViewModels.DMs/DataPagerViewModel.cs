using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.Infra.Models;
using WPFDemo.SimpleFrame.Infra.MVVM;
using WPFDemo.SimpleFrame.IViewModels.DMs;

namespace WPFDemo.SimpleFrame.ViewModels.DMs
{
    public class DataPagerViewModel : BaseViewModel, IDataPagerViewModel
    {
        private List<Student> _students;
        private int _pageNo;
        private int _pageSize;
        private int _itemCount;

        private IStudentBusi _studentBusi;

        public List<Student> Students
        {
            get => _students;
            set
            {
                _students = value;
                OnPropertyChanged(() => Students);
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
                    PageSearch(_pageSize, _pageNo);
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
                    PageSearch(_pageSize, 1);
                    OnPropertyChanged(() => PageSize);
                }
            }
        }

        public DataPagerViewModel(IStudentBusi studentBusi)
        {
            _studentBusi = studentBusi;
            _pageNo = 1;
            _pageSize = 5;
        }

        private void PageSearch(int pageSize, int pageNo)
        {
            var result = _studentBusi.GetStudents(pageNo, pageSize);
            ItemCount = 9999;
            PageSize = result.PageSize;
            PageNo = result.PageNo;
            Students = result.Students;
        }

        protected override void Loaded()
        {
            PageSearch(_pageSize, _pageNo);
        }

        protected override void UnLoaded()
        {
            
        }
    }
}
