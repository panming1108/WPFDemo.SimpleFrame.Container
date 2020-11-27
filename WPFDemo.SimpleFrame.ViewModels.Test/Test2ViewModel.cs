using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFDemo.SimpleFrame.Infra.MVVM.VMOnly;
using WPFDemo.SimpleFrame.IViewModels.Test;

namespace WPFDemo.SimpleFrame.ViewModels.Test
{
    public class Test2ViewModel : BaseViewModel, ITest2ViewModel
    {
        public ICommand StartCommand { get; set; }

        public Test2ViewModel()
        {
            StartCommand = new AsyncDelegateCommand<object>(OnStart);
        }

        private async Task OnStart(object arg)
        {
            Console.WriteLine("开始加载");
            await TaskEx.FromResult(0);
        }

        protected override async Task Loaded()
        {
            
        }

        protected override async Task UnLoaded()
        {
            
        }
    }
}
