﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Infra.Enums
{
    public class MessagerKeyEnum
    {
        [Description("MainPage导航")]
        public static readonly string MainPageNavi = nameof(MainPageNavi);

        [Description("NaviPage导航")]
        public static readonly string NaviPageNavi = nameof(NaviPageNavi);

        [Description("是否繁忙")]
        public static readonly string IsBusy = nameof(IsBusy);

        [Description("弹出通知框")]
        public static readonly string PopupNotifyBox = nameof(PopupNotifyBox);
    }
}
