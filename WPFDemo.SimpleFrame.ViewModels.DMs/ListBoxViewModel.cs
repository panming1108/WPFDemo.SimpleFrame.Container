using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.Infra.Models;
using WPFDemo.SimpleFrame.Infra.MVVM.VMOnly;
using WPFDemo.SimpleFrame.IViewModels.DMs;

namespace WPFDemo.SimpleFrame.ViewModels.DMs
{
    public class ListBoxViewModel : BaseViewModel, IListBoxViewModel
    {
        private List<string> _fileListSource;
        public List<string> FileListSource
        {
            get => _fileListSource;
            set
            {
                _fileListSource = value;
                OnPropertyChanged(() => FileListSource);
            }
        }
        private IList _selectedFiles;
        public IList SelectedFiles
        {
            get => _selectedFiles;
            set
            {
                _selectedFiles = value;
                OnPropertyChanged(() => SelectedFiles);
            }
        }

        private IListBoxBusi _listBoxBusi;
        private ObservableCollection<ListBoxModel> _listBoxSource;
        public ObservableCollection<ListBoxModel> ListBoxSource
        {
            get => _listBoxSource;
            set
            {
                _listBoxSource = value;
                OnPropertyChanged(() => ListBoxSource);
            }
        }

        private int count = 0;
        private int _startLazyLoadCount;
        public int StartLazyLoadCount
        {
            get => _startLazyLoadCount;
            set
            {
                _startLazyLoadCount = value;
                OnPropertyChanged(() => StartLazyLoadCount);
            }
        }

        private ObservableCollection<FlushModel<Student>> _students;
        public ObservableCollection<FlushModel<Student>> Students
        {
            get => _students;
            set
            {
                _students = value;
                OnPropertyChanged(() => Students);
            }
        }

        public List<FlushModel<Student>> AllStudents;

        public ICommand InsertCommand { get; set; }
        public ICommand LazyLoadCommand { get; set; }
        public ICommand SelectedItemsAnalysisCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }

        public ListBoxViewModel(IListBoxBusi listBoxBusi)
        {
            _listBoxBusi = listBoxBusi;
            _startLazyLoadCount = 20;
            _students = new ObservableCollection<FlushModel<Student>>();
            _fileListSource = new List<string>();
            AllStudents = new List<FlushModel<Student>>();
            InsertCommand = new AsyncDelegateCommand<object>(OnInsert);
            LazyLoadCommand = new AsyncDelegateCommand<object>(OnLazyLoad);
            SelectedItemsAnalysisCommand = new AsyncDelegateCommand<object>(OnSelectedItemsAnalysis);
            SelectionChangedCommand = new AsyncDelegateCommand<object>(OnSelectionChanged);
            //_selectedFiles = new List<object>() { "999" };
        }

        private async Task OnSelectionChanged(object arg)
        {
            await TaskEx.FromResult(0);
        }

        private async Task OnSelectedItemsAnalysis(object arg)
        {
            await TaskEx.FromResult(0);
        }

        private async Task OnLazyLoad(object arg)
        {
            var result = LazyLoadSearch(Students.Count, 5);
            foreach (var item in result)
            {
                item.IsFlush = false;
                Students.Add(item);
            }
            await TaskEx.FromResult(0);
        }

        private IEnumerable<FlushModel<Student>> LazyLoadSearch(int haveLoadedCount, int loadedCount)
        {
            IEnumerable<FlushModel<Student>> result = new List<FlushModel<Student>>();
            if (haveLoadedCount < AllStudents.Count)
            {
                if (AllStudents.Count >= haveLoadedCount + loadedCount)
                {
                    result = AllStudents.Skip(AllStudents.Count - haveLoadedCount - loadedCount).Take(loadedCount).Reverse();
                }
                else
                {
                    result = AllStudents.Take(AllStudents.Count - haveLoadedCount).Reverse();
                }
            }
            return result;
        }

        private async Task OnInsert(object arg)
        {
            count++;
            FlushModel<Student> student = new FlushModel<Student>(new Student() { Id = count, Name = "张三" + count, Age = count });
            AllStudents.Add(student);
            if (Students.Count >= StartLazyLoadCount)
            {
                Students.RemoveAt(Students.Count - 1);
            }
            Students.Insert(0, student);
            await TaskEx.FromResult(0);
        }

        protected async override Task Loaded()
        {
            FileListSource = new List<string>()
            {
                "111",
                "222",
                "333",
                "444",
                "555",
                "666",
                "777",
                "888",
                "999",
            };
            SelectedFiles = new List<string>() { "999", "888" };
            ListBoxSource = new ObservableCollection<ListBoxModel>(await _listBoxBusi.GetListBoxSource());
        }

        protected async override Task UnLoaded()
        {
            await TaskEx.FromResult(0);
        }
    }
}
