using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.Infra.Models;
using WPFDemo.SimpleFrame.Infra.MVVM.VMOnly;
using WPFDemo.SimpleFrame.IViewModelsEditors;

namespace WPFDemo.SimpleFrame.ViewModels.Editors
{
    public class ButtonGroupViewModel : BaseViewModel, IButtonGroupViewModel
    {
        private IButtonGroupBusi _buttonGroupBusi;
        private List<string> _radioButtonsSource;
        private List<string> _radioButtonUnSelected;
        private List<string> _checkBoxsSource;
        private string _radioButtonSelectedString;
        private Student _selectedStudent;
        private List<Student> _radioStudentSource;
        private List<Student> _notSelectedStudents;
        private List<Student> _haveCheckedStudent;
        private List<Student> _clickComboBoxItemsSource;
        private List<Student> _multiComboBoxItemsSource;
        private List<Student> _initSelectedItems;
        public List<Student> InitSelectedItems
        {
            get => _initSelectedItems;
            set
            {
                _initSelectedItems = value;
                OnPropertyChanged(() => InitSelectedItems);
            }
        }
        public List<Student> MultiComboBoxItemsSource
        {
            get => _multiComboBoxItemsSource;
            set
            {
                _multiComboBoxItemsSource = value;
                OnPropertyChanged(() => MultiComboBoxItemsSource);
            }
        }
        public List<Student> ClickComboBoxItemsSource
        {
            get => _clickComboBoxItemsSource;
            set
            {
                _clickComboBoxItemsSource = value;
                OnPropertyChanged(() => ClickComboBoxItemsSource);
            }
        }
        public List<Student> HaveCheckStudent
        {
            get => _haveCheckedStudent;
            set
            {
                _haveCheckedStudent = value;
                OnPropertyChanged(()=>HaveCheckStudent);
            }
        }
        public List<Student> NotSelectedStudents
        {
            get => _notSelectedStudents;
            set
            {
                _notSelectedStudents = value;
            }
        }
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(() => SelectedStudent);
            }
        }


        public List<Student> RadioStudentSource
        {
            get => _radioStudentSource;
            set
            {
                _radioStudentSource = value;
                OnPropertyChanged(() => RadioStudentSource);
            }
        }

        public string RadioButtonSelectedString
        {
            get => _radioButtonSelectedString;
            set
            {
                _radioButtonSelectedString = value;
                OnPropertyChanged(() => RadioButtonSelectedString);
            }
        }

        public List<string> RadioButtonUnSelected
        {
            get => _radioButtonUnSelected;
            set
            {
                _radioButtonUnSelected = value;
                OnPropertyChanged(() => RadioButtonUnSelected);
            }
        }

        public List<string> RadioButtonsSource
        {
            get => _radioButtonsSource;
            set
            {
                _radioButtonsSource = value;
                OnPropertyChanged(() => RadioButtonsSource);
            }
        }
        public List<string> CheckBoxsSource
        {
            get => _checkBoxsSource;
            set
            {
                _checkBoxsSource = value;
                OnPropertyChanged(() => CheckBoxsSource);
            }
        }    

        public ICommand RadioButtonCommand { get; set; }
        public ICommand SelectionCommand { get; set; }

        public ButtonGroupViewModel(IButtonGroupBusi buttonGroupBusi)
        {
            _buttonGroupBusi = buttonGroupBusi;
            _radioButtonsSource = new List<string>();
            _checkBoxsSource = new List<string>();
            _radioButtonUnSelected = new List<string>();
            _notSelectedStudents = new List<Student>();
            _clickComboBoxItemsSource = new List<Student>();
            _multiComboBoxItemsSource = new List<Student>();
            RadioButtonCommand = new AsyncDelegateCommand<Student>(OnRadioButtonChecked);
            SelectionCommand = new AsyncDelegateCommand<object>(OnSelectionChanged);
        }

        private async Task OnSelectionChanged(object arg)
        {            
            await TaskEx.FromResult(0);
        }

        private async Task OnRadioButtonChecked(Student student)
        {
            await TaskEx.FromResult(0);
            Debug.WriteLine(student.ToString());
        }

        protected async override Task Loaded()
        {
            RadioButtonsSource = await _buttonGroupBusi.GetRadioButtonsSource();
            RadioStudentSource = await _buttonGroupBusi.GetRadioButtonsStudentSource();
            CheckBoxsSource = await _buttonGroupBusi.GetCheckBoxsSource();
            var list = new List<Student>()
            {
                new Student(0,"全部", 10),
                new Student(1,"单发", 20),
                new Student(2,"成对", 30),
                new Student(3,"房速", 40),
                new Student(4,"房早未下传", 50),
            };            
            ClickComboBoxItemsSource = list;
            MultiComboBoxItemsSource = new List<Student>() 
            {
                new Student(0,"全部", 10),
                new Student(1,"单发", 20),
                new Student(2,"成对", 30),
                new Student(3,"房速", 40),
                new Student(4,"房早未下传", 50),
            };
            InitSelectedItems = new List<Student>() { MultiComboBoxItemsSource[1], MultiComboBoxItemsSource[3] };
        }

        protected async override Task UnLoaded()
        {
            await TaskEx.FromResult(0);
        }
    }
}
