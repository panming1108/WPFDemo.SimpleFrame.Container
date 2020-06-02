using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFDemo.SimpleFrame.Infra.Models;
using WPFDemo.SimpleFrame.Infra.MVVM.VMOnly;
using WPFDemo.SimpleFrame.IViewModels.DMs;

namespace WPFDemo.SimpleFrame.ViewModels.DMs
{
    public class ExpandRadioGroupViewModel : BaseViewModel, IExpandRadioGroupViewModel
    {
        private List<Student> _radioGroupSource;
        public List<Student> RadioGroupSource
        {
            get => _radioGroupSource;
            set
            {
                _radioGroupSource = value;
                OnPropertyChanged(() => RadioGroupSource);
            }
        }

        private string _selectedText;
        public string SelectedText
        {
            get => _selectedText;
            set
            {
                _selectedText = value;
                OnPropertyChanged(() => SelectedText);
            }
        }

        public ICommand SelectedItemChangedCommand { get; set; }

        public ExpandRadioGroupViewModel()
        {
            _radioGroupSource = new List<Student>();
            SelectedItemChangedCommand = new AsyncDelegateCommand<object>(OnSelectedItemChanged);
        }

        private async Task OnSelectedItemChanged(object arg)
        {
            Student student = arg as Student;
            SelectedText = student.Name;
            await TaskEx.FromResult(0);
        }

        protected async override Task Loaded()
        {
            List<Student> source = new List<Student>()
            { 
                new Student("2.1.1"),
                new Student("2.1.2"),
                new Student("2.1.3"),
                new Student("2.1.4"),
                new Student("2.1.5"),
                new Student("2.1.6"),
                new Student("2.1.7"),
                new Student("2.1.8"),
                new Student("2.1.9"),
                new Student("2.1.10"),
                new Student("2.1.11"),
                new Student("2.1.12"),
                new Student("2.1.13"),
                new Student("2.1.14"),
                new Student("2.1.15"),
                new Student("2.1.16"),
                new Student("2.1.17"),
                new Student("2.1.18"),
                new Student("2.1.19"),
                new Student("2.1.20"),
                new Student("2.1.21"),
                new Student("2.1.22"),
                new Student("2.1.23"),
                new Student("2.1.24"),
                new Student("2.1.25"),
                new Student("2.1.26"),
                new Student("2.1.27"),
                new Student("2.1.28"),
                new Student("2.1.29")
            };
            RadioGroupSource = source;
            await TaskEx.FromResult(0);
        }

        protected async override Task UnLoaded()
        {
            await TaskEx.FromResult(0);
        }
    }
}
