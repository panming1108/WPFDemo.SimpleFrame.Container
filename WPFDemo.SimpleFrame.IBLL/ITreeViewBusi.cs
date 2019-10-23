using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.IBLL
{
    public interface ITreeViewBusi
    {
        Task<List<TreeViewNode>> GetTreeViewIconSource();
        Task<List<TreeViewNode>> GetTreeViewImageSource();

        Task<List<TreeViewNode>> GetMenuSource();
    }
}
