﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFDemo.SimpleFrame.Infra.Ioc;
using WPFDemo.SimpleFrame.IViewModels.Test;

namespace WPFDemo.SimpleFrame.Views.Test
{
    /// <summary>
    /// TestView.xaml 的交互逻辑
    /// </summary>
    public partial class TestView : UserControl
    {
        public TestView()
        {
            InitializeComponent();
            DataContext = IocManagerInstance.ResolveType<ITestViewModel>();
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("进入");
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("出来");
        }
    }
}
