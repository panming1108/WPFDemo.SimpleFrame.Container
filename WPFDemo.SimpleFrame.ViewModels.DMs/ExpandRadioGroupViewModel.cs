using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
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

        private List<VerifyData> _verifyDataSource;
        public List<VerifyData> VerifyDataSource
        {
            get => _verifyDataSource;
            set
            {
                _verifyDataSource = value;
                OnPropertyChanged(() => VerifyDataSource);
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
            _verifyDataSource = new List<VerifyData>();
            SelectedItemChangedCommand = new AsyncDelegateCommand<object>(OnSelectedItemChanged);
        }

        private async Task OnSelectedItemChanged(object arg)
        {
            await TaskEx.Delay(2000);
            Student student = arg as Student;
            SelectedText = student.Name;
        }

        protected async override Task Loaded()
        {
            List<Student> source = new List<Student>();
            for (int i = 0; i < 30; i++)
            {
                Student student = new Student(i, "张三" + i, 20 + i);
                source.Add(student);
            }
            RadioGroupSource = source;
            VerifyData v1 = new VerifyData() { Id = 1, ParentId = 0, Type = "窦性心律", PositiveCount = 200000, PositiveRate = 0.89, NagetiveCount = 18000, NagetiveRate = 0.74, Sensitivity = 0.74, Specificity = 0.89 };
            VerifyData v11 = new VerifyData() { Id = 2, ParentId = 1, Type = "窦性心律", PositiveCount = 200000, PositiveRate = 0.89, NagetiveCount = 18000, NagetiveRate = 0.74, Sensitivity = 0.74, Specificity = 0.89 };
            VerifyData v12 = new VerifyData() { Id = 3, ParentId = 1, Type = "窦性心动过速", PositiveCount = 200000, PositiveRate = 0.89, NagetiveCount = 18000, NagetiveRate = 0.74, Sensitivity = 0.74, Specificity = 0.89 };
            VerifyData v13 = new VerifyData() { Id = 4, ParentId = 1, Type = "窦性心动过缓", PositiveCount = 200000, PositiveRate = 0.89, NagetiveCount = 18000, NagetiveRate = 0.74, Sensitivity = 0.74, Specificity = 0.89 };
            VerifyData v14 = new VerifyData() { Id = 5, ParentId = 1, Type = "窦性心律不齐", PositiveCount = 200000, PositiveRate = 0.89, NagetiveCount = 18000, NagetiveRate = 0.74, Sensitivity = 0.74, Specificity = 0.89 };
            VerifyData v15 = new VerifyData() { Id = 6, ParentId = 1, Type = "窦性心律不齐1", PositiveCount = 200000, PositiveRate = 0.89, NagetiveCount = 18000, NagetiveRate = 0.74, Sensitivity = 0.74, Specificity = 0.89 };
            VerifyData v2 = new VerifyData() { Id = 6, ParentId = 0, Type = "房性心律", PositiveCount = 200000, PositiveRate = 0.89, NagetiveCount = 18000, NagetiveRate = 0.74, Sensitivity = 0.74, Specificity = 0.89 };
            VerifyData v21 = new VerifyData() { Id = 7, ParentId = 6, Type = "房性心率", PositiveCount = 200000, PositiveRate = 0.89, NagetiveCount = 18000, NagetiveRate = 0.74, Sensitivity = 0.74, Specificity = 0.89 };
            VerifyData v22 = new VerifyData() { Id = 8, ParentId = 6, Type = "房性心动过速", PositiveCount = 200000, PositiveRate = 0.89, NagetiveCount = 18000, NagetiveRate = 0.74, Sensitivity = 0.74, Specificity = 0.89 };

            v1.Children = new List<VerifyData>() { v11, v12, v13, v14, v15 };
            v2.Children = new List<VerifyData>() { v21, v22 };

            VerifyDataSource = new List<VerifyData>() { v1, v2 };
            await TaskEx.FromResult(0);
        }

        protected async override Task UnLoaded()
        {
            await TaskEx.FromResult(0);
        }
    }

    public class VerifyData
    {
        public long Id { get; set; }
        public long ParentId { get; set; }
        public string Type { get; set; }
        public int PositiveCount { get; set; }
        public int NagetiveCount { get; set; }
        public double PositiveRate { get; set; }
        public double NagetiveRate { get; set; }
        public double Sensitivity { get; set; }
        public double Specificity { get; set; }
        public List<VerifyData> Children { get; set; }
    }
}
