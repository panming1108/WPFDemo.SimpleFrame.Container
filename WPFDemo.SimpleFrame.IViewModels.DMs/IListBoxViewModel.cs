using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.IViewModels.DMs
{
    public interface IListBoxViewModel
    {
        ObservableCollection<ListBoxModel> ListBoxSource { get; set; }
    }
}
