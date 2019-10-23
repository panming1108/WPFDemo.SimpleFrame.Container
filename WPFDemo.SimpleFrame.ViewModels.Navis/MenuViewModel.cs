using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.Infra.Models;
using WPFDemo.SimpleFrame.Infra.MVVM.VMOnly;
using WPFDemo.SimpleFrame.IViewModels.Navis;

namespace WPFDemo.SimpleFrame.ViewModels.Navis
{
    public class MenuViewModel : BaseViewModel, IMenuViewModel
    {
        private ITreeViewBusi _treeViewBusi;
        private List<TreeViewNode> _menuSource;
        public List<TreeViewNode> MenuSource
        {
            get => _menuSource;
            set
            {
                _menuSource = value;
                OnPropertyChanged(() => MenuSource);
            }
        }

        public MenuViewModel(ITreeViewBusi treeViewBusi)
        {
            _treeViewBusi = treeViewBusi;
            _menuSource = new List<TreeViewNode>();
        }

        protected async override Task Loaded()
        {
            MenuSource = await _treeViewBusi.GetMenuSource();
        }

        protected async override Task UnLoaded()
        {
            await TaskEx.FromResult(0);
        }
    }
}
