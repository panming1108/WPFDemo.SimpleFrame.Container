using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Infra.Enums
{
    public enum PageKeyEnum
    {
        [Description("主页")]
        HomePage,

        [Description("导航页")]
        NaviPage,

        [Description("测试页")]
        TestPage
    }
}
