using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WPFDemo.SimpleFrame.Infra.Ioc;
using WPFDemo.SimpleFrame.Infra.MVVM;
using WPFDemo.SimpleFrame.IViewModels.LayOut;
using WPFDemo.SimpleFrame.IViews.LayOut;

namespace WPFDemo.SimpleFrame.ViewModels.LayOut
{
    public class LayOutViewModel : BaseViewModel, ILayOutViewModel
    {
        public ICommand ShowMVVMWindowCommand { get; set; }

        public LayOutViewModel()
        {
            InitCommands();
        }

        private void InitCommands()
        {
            ShowMVVMWindowCommand = new DelegateCommand(OnShowMVVMWindow);
        }

        private void OnShowMVVMWindow()
        {
            var emcWindow = IocManagerInstance.ResolveType<IMVVMEMCWindow>();
            emcWindow.ShowDialog();
        }

        protected override void Loaded()
        {
            
        }

        protected override void UnLoaded()
        {
            
        }
    }
}
