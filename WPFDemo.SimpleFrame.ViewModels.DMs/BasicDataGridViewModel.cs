﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.Infra.Models;
using WPFDemo.SimpleFrame.Infra.MVVM;
using WPFDemo.SimpleFrame.IViewModels.DMs;

namespace WPFDemo.SimpleFrame.ViewModels.DMs
{
    public class BasicDataGridViewModel : BaseViewModel, IBasicDataGridViewModel
    {
        private IDataGridBusi _dataGridBusi;
        private List<DataGridModel> _dataGridSource;
        public List<DataGridModel> DataGridSource
        {
            get => _dataGridSource;
            set
            {
                _dataGridSource = value;
                OnPropertyChanged(() => DataGridSource);
            }
        }

        public BasicDataGridViewModel(IDataGridBusi dataGridBusi)
        {
            _dataGridBusi = dataGridBusi;
        }

        protected override void Loaded()
        {
            DataGridSource = _dataGridBusi.GetDataGridSource();
        }

        protected override void UnLoaded()
        {
            
        }
    }
}