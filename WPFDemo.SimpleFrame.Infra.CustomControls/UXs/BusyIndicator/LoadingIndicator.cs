using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.UXs
{
    public class LoadingIndicator : ContentControl
    {
        public double ContentHeight
        {
            get { return (double)GetValue(ContentHeightProperty); }
            set { SetValue(ContentHeightProperty, value); }
        }
        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }
        public double ContentWidth
        {
            get { return (double)GetValue(ContentWidthProperty); }
            set { SetValue(ContentWidthProperty, value); }
        }

        public static readonly DependencyProperty ContentHeightProperty =
            DependencyProperty.Register(nameof(ContentHeight), typeof(double), typeof(LoadingIndicator));
        public static readonly DependencyProperty ContentWidthProperty =
            DependencyProperty.Register(nameof(ContentWidth), typeof(double), typeof(LoadingIndicator));
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(LoadingIndicator), new PropertyMetadata(false));
    }
}
