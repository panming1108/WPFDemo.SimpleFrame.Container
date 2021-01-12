using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public interface ISelectItem
    {
        bool IsSelected { get; set; }

        object DataContext { get; set; }

        ISelectItemsContainer Container { get; }
    }
}
