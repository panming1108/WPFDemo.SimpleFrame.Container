using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
{
    public interface IBeatItemListViewContainer<T>
    {
        List<T> ItemsSource { get; }

        ItemsSourceHandler<T> ItemsSourceHandler { get; }
    }
}
