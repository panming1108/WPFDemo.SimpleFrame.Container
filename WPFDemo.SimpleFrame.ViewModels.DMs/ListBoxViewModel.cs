using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.Infra.Models;
using WPFDemo.SimpleFrame.Infra.MVVM.VMOnly;
using WPFDemo.SimpleFrame.IViewModels.DMs;

namespace WPFDemo.SimpleFrame.ViewModels.DMs
{
    public class ListBoxViewModel : BaseViewModel, IListBoxViewModel
    {
        private IListBoxBusi _listBoxBusi;
        private List<ListBoxModel> _listBoxSource;
        public List<ListBoxModel> ListBoxSource
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
        }

        protected async override Task Loaded()
        {
            ListBoxSource = await _listBoxBusi.GetListBoxSource();
        }

        protected async override Task UnLoaded()
        {
            await TaskEx.FromResult(0);
        }
    }
}
