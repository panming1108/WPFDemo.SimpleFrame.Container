using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.UXs.BusyIndicator
{
    public class UpLoadingIndicator : ContentControl
    {
        public bool IsUpLoading
        {
            get { return (bool)GetValue(IsUpLoadingProperty); }
            set { SetValue(IsUpLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsUpLoadingProperty =
            DependencyProperty.Register("IsUpLoading", typeof(bool), typeof(UpLoadingIndicator));       
    }
}
