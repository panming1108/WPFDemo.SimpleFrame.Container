﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WPFDemo.SimpleFrame.Infra.Helper;

namespace WPFDemo.SimpleFrame.Container.IViewModels
{
    public interface IMainViewModel
    {
        ICommand PageNaviCommand { get; set; }
        List<EnumStructInfo> PageNaviSource { get; set; }
    }
}
