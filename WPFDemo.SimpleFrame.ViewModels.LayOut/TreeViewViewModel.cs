using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo.SimpleFrame.IBLL;
using WPFDemo.SimpleFrame.Infra.Enums;
using WPFDemo.SimpleFrame.Infra.Messager;
using WPFDemo.SimpleFrame.Infra.Models;
using WPFDemo.SimpleFrame.Infra.MVVM.VMOnly;
using WPFDemo.SimpleFrame.IViewModels.LayOut;

namespace WPFDemo.SimpleFrame.ViewModels.LayOut
{
    public class TreeViewViewModel : BaseViewModel, ITreeViewViewModel
    {
        private ITreeViewBusi _treeViewBusi;
        private List<TreeViewNode> _iconNodes;
        private List<TreeViewNode> _imageNodes;
        private TreeViewNode _currentNode;

        public List<TreeViewNode> IconNodes
        {
            get => _iconNodes;
            set
            {
                _iconNodes = value;
                OnPropertyChanged(() => IconNodes);
            }
        }
        public List<TreeViewNode> ImageNodes
        {
            get => _imageNodes;
            set
            {
                _imageNodes = value;
                OnPropertyChanged(() => ImageNodes);
            }
        }

        public TreeViewNode CurrentNode
        {
            get => _currentNode;
            set
            {
                _currentNode = value;
                OnGetCurrentName(_currentNode);
                OnPropertyChanged(() => CurrentNode);
            }
        }

        private void OnGetCurrentName(TreeViewNode currentName)
        {
            MessagerInstance.GetMessager().Send(MessagerKeyEnum.PopupNotifyBox, new PopupNotifyObject("通知", currentName.Name));
        }

        public TreeViewViewModel(ITreeViewBusi treeViewBusi)
        {
            _treeViewBusi = treeViewBusi;
            _iconNodes = new List<TreeViewNode>();
            _imageNodes = new List<TreeViewNode>();
        }

        protected async  override Task Loaded()
        {
            IconNodes = await _treeViewBusi.GetTreeViewIconSource();
            ImageNodes = await _treeViewBusi.GetTreeViewImageSource();
        }

        protected async override Task UnLoaded()
        {
            await TaskEx.FromResult(0);
        }
    }
}
