﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private List<Student> _selectedRows;
        public List<Student> SeletedRows
        {
            get => _selectedRows;
            set
            {
                _selectedRows = value;
                OnPropertyChanged(() => SeletedRows);
            }
        }
        private List<IconModel> _iconsSource;
        public List<IconModel> IconsSource
        {
            get => _iconsSource;
            set
            {
                _iconsSource = value;
                OnPropertyChanged(() => IconsSource);
            }
        }

        private List<Student> _groupStudents;
        public List<Student> GroupStudents
        {
            get => _groupStudents;
            set
            {
                _groupStudents = value;
                OnPropertyChanged(() => GroupStudents);
            }
        }

        public ICommand MouseDoubleClickCommand { get; set; }
        public ICommand MenuOneCommand { get; set; }
        public ICommand MenuTwoCommand { get; set; }
        public ICommand MenuThreeCommand { get; set; }
        public ICommand IconOneCommand { get; set; }
        public ICommand IconTwoCommand { get; set; }
        public ICommand IconThreeCommand { get; set; }

        public TestViewModel(IStudentBusi studentBusi)
        {
            _studentBusi = studentBusi;
            _selectedRows = new List<Student>();
            _groupStudents = new List<Student>();
            PageSize = 10;
            PageSizeSource = new int[] { 10, 20, 30 };
            _iconsSource = new List<IconModel>();
            MouseDoubleClickCommand = new AsyncDelegateCommand<object>(OnMouseDoubleClick);
            MenuOneCommand = new AsyncDelegateCommand<object>(OnMenuOne);
            MenuTwoCommand = new AsyncDelegateCommand<object>(OnMenuTwo);
            MenuThreeCommand = new AsyncDelegateCommand<object>(OnMenuThree);
            IconOneCommand = new AsyncDelegateCommand<object>(OnIconOne);
            IconTwoCommand = new AsyncDelegateCommand<object>(OnIconTwo);
            IconThreeCommand = new AsyncDelegateCommand<object>(OnIconThree);
        }

        private async Task OnIconThree(object arg)
        {
            await TaskEx.FromResult(0);
            Debug.WriteLine("IconThree:" + arg.ToString());
        }

        private async Task OnIconTwo(object arg)
        {
            await TaskEx.FromResult(0);
            Debug.WriteLine("IconTwo:" + arg.ToString());
        }

        private async Task OnIconOne(object arg)
        {
            await TaskEx.FromResult(0);
            Debug.WriteLine("IconOne:" + arg.ToString());
        }

        private async Task OnMenuThree(object arg)
        {
            await TaskEx.FromResult(0);
            Debug.WriteLine("MenuThree:" + arg.ToString());
        }

        private async Task OnMenuTwo(object arg)
        {
            await TaskEx.FromResult(0);
            Debug.WriteLine("MenuTwo:" + arg.ToString());
        }

        private async Task OnMenuOne(object arg)
        {
            await TaskEx.FromResult(0);
            Debug.WriteLine("MenuOne:" + arg.ToString());
        }

        private async Task OnMouseDoubleClick(object arg)
        {
            await TaskEx.FromResult(0);
            Debug.WriteLine(arg.ToString());
        }

        protected async override Task Loaded()
        {
            List<IconModel> icons = new List<IconModel>()
            {
                new IconModel("/WPFDemo.SimpleFrame.Views.Test;component/Images/critical.png", "critical", IconOneCommand),
                new IconModel("/WPFDemo.SimpleFrame.Views.Test;component/Images/urgent.png", "urgent", IconTwoCommand),
                new IconModel("/WPFDemo.SimpleFrame.Views.Test;component/Images/warning.png", "warning", IconThreeCommand)
            };
            IconsSource = icons;
            await PageSearch(PageSize, PageNo);
            var source = new List<Student>()
            {
                new Student(){ Id = 0, Name = "张三", Age = 43, ParentId = -1 },
                new Student(){ Id = 1, Name = "张三0", Age = 21, ParentId = 0 },
                new Student(){ Id = 2, Name = "张三1", Age = 22, ParentId = 0 },
                new Student(){ Id = 3, Name = "李四", Age = 47, ParentId = -1 },
                new Student(){ Id = 4, Name = "李四0", Age = 23, ParentId = 3 },
                new Student(){ Id = 5, Name = "李四1", Age = 24, ParentId = 3 },
            };
            source.ForEach(x => x.Parent = source.SingleOrDefault(w => w.Id == x.ParentId));
            GroupStudents = source.Where(x => x.ParentId != -1).ToList();
        }

        protected async override Task UnLoaded()
        {
            await TaskEx.FromResult(0);
        }

        protected async override Task PageSearch(int pageNo, int pageSize)
        {
            var result = await _studentBusi.GetStudents(pageNo, pageSize);
            ItemCount = 9999;
            PageSize = result.PageSize;
            PageNo = result.PageNo;
            DataSource = result.Students;
        }
    }
}
