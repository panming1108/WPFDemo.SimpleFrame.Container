using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.Infra.Models;
using WPFDemo.SimpleFrame.Infra.MVVM;
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

        protected override void Loaded()
        {
            ListBoxSource = _listBoxBusi.GetListBoxSource();
        }

        protected override void UnLoaded()
        {
            
        }
    }
}
