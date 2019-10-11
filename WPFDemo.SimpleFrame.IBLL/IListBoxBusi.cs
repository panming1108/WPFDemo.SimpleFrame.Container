using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.IBLL
{
    public interface IListBoxBusi
    {
        List<ListBoxModel> GetListBoxSource();
    }
}
