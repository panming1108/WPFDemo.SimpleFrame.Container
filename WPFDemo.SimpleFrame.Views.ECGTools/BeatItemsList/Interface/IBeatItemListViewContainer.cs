using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public interface IBeatItemListViewContainer<T,T1>
    {
        Dictionary<T, T1> ItemsSource { get; }

        ItemsSourceHandler<T, T1> ItemsSourceHandler { get; }
    }
}
