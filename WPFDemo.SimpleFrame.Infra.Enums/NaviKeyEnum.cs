using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Infra.Enums
{
    public enum NaviKeyEnum
    {
        [Description("DMsPage.xaml")]
        DMs,
        [Description("DVsPage.xaml")]
        DVs,
        [Description("EditorsPage.xaml")]
        Editors,
        [Description("LayOutPage.xaml")]
        LayOut,
        [Description("NavisPage.xaml")]
        Navis,
        [Description("SchedulesPage.xaml")]
        Schedules,
        [Description("UXsPage.xaml")]
        UXs
    }
}
