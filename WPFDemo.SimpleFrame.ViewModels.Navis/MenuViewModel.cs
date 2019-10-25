using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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

        public ICommand ItemCommand { get; set; }

        public ICommand ItemMouseOverCommand { get; set; }
        public MenuViewModel(ITreeViewBusi treeViewBusi)
        {
            _treeViewBusi = treeViewBusi;
            _menuSource = new List<TreeViewNode>();
            ItemCommand = new AsyncDelegateCommand<object>(OnItemSelected);
            ItemMouseOverCommand = new AsyncDelegateCommand<object>(OnItemMouseOver);
        }

        private async Task OnItemMouseOver(object arg)
        {
            await TaskEx.FromResult(0);
            var d = arg as TreeViewNode;
            Debug.WriteLine("MouseOver:" + d.Name + d.InputGestureText);
        }

        private async Task OnItemSelected(object item)
        {
            await TaskEx.FromResult(0);
            var d = item as TreeViewNode;
            if(d != null)
            {
                Debug.WriteLine(d.Name + d.InputGestureText);
            }
            else
            {
                Debug.WriteLine("asdfajksdfhk");
            }
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
