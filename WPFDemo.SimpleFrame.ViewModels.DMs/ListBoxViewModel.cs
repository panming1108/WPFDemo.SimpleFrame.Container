using System;
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
        public ICommand InsertCommand { get; set; }
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

        public ListBoxViewModel(IListBoxBusi listBoxBusi)
        {
            _listBoxBusi = listBoxBusi;
            InsertCommand = new AsyncDelegateCommand<object>(OnInsert);
        }

        private async Task OnInsert(object arg)
        {
            ListBoxModel message1 = new ListBoxModel()
            {
                Name = "陈木方",
                Age = 45,
                AgeUnit = "岁",
                Source = "急诊",
                Flag = "001001",
                CheckTime = DateTime.Now,
                CheckDepartment = DateTime.Now.ToString(),
            };
            ListBoxSource.Insert(0, message1);
            await TaskEx.FromResult(0);
        }

        protected async override Task Loaded()
        {
            ListBoxSource = new ObservableCollection<ListBoxModel>(await _listBoxBusi.GetListBoxSource());
        }

        protected async override Task UnLoaded()
        {
            await TaskEx.FromResult(0);
        }
    }
}
