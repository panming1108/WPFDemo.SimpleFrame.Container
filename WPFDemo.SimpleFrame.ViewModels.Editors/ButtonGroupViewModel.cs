﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public ButtonGroupViewModel(IButtonGroupBusi buttonGroupBusi)
        {
            _buttonGroupBusi = buttonGroupBusi;
            _radioButtonsSource = new List<string>();
            _checkBoxsSource = new List<string>();
            _radioButtonUnSelected = new List<string>();
            _notSelectedStudents = new List<Student>();
        }

        protected async override Task Loaded()
        {
            RadioButtonsSource = await _buttonGroupBusi.GetRadioButtonsSource();
            RadioStudentSource = await _buttonGroupBusi.GetRadioButtonsStudentSource();
            CheckBoxsSource = await _buttonGroupBusi.GetCheckBoxsSource();
        }

        protected async override Task UnLoaded()
        {
            await TaskEx.FromResult(0);
        }
    }
}
