using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo.SimpleFrame.Infra.MVVM.VMOnly;
using WPFDemo.SimpleFrame.IViewModels.Test;

namespace WPFDemo.SimpleFrame.ViewModels.Test
{
    public class Test2Module2ViewModel : BaseViewModel, ITest2Module2ViewModel
    {
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(() => IsLoading);
            }
        }
        protected override async Task Loaded()
        {
            IsLoading = true;
        }

        protected override async Task UnLoaded()
        {

        }
    }
}
