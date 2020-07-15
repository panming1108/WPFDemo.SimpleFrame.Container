using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo.SimpleFrame.Infra.MVVM.VMOnly;
using WPFDemo.SimpleFrame.IViewModels.ECGTools;

namespace WPFDemo.SimpleFrame.ViewModels.ECGTools
{
    public class BeatItemListViewModel : BaseViewModel, IBeatItemListViewModel
    {
        private List<int> _rList;
        public List<int> RList
        {
            get => _rList;
            set
            {
                _rList = value;
                OnPropertyChanged(nameof(RList));
            }
        }

        public BeatItemListViewModel()
        {
            _rList = new List<int>();
        }

        protected override async Task Loaded()
        {
            var list = new List<int>();
            for (int i = 0; i < 30; i++)
            {
                list.Add(i);
            }
            RList = list;
            await TaskEx.FromResult(0);
        }

        protected override async Task UnLoaded()
        {
            await TaskEx.FromResult(0);
        }
    }
}
