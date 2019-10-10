using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.Infra.MVVM;
using WPFDemo.SimpleFrame.IViewModels.Test;

namespace WPFDemo.SimpleFrame.ViewModels.Test
{
    public class TestViewModel : BaseViewModel, ITestViewModel
    {
        private ITestBusi _testBusi;
        private string _testViewText;
        public string TestViewText
        {
            get => _testViewText;
            set
            {
                if (_testViewText != value)
                {
                    _testViewText = value;
                    OnPropertyChanged(() => TestViewText);
                }
            }
        }

        public ICommand TestViewClickCommand { get; set; }

        public TestViewModel(ITestBusi testBusi)
        {
            _testBusi = testBusi;
            InitCommands();
        }

        private void InitCommands()
        {
            TestViewClickCommand = new DelegateCommand(OnTestViewClick);
        }

        private void OnTestViewClick()
        {
            TestViewText = "TestView:" + _testBusi.GetTestString();
        }

        protected override void UnLoaded()
        {
            throw new NotImplementedException();
        }

        protected override void Loaded()
        {
            throw new NotImplementedException();
        }
    }
}
