﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.UXs.ProgressBar
{
    public class DistributionProgressItem : ContentControl
    {
        internal double TotalSeconds => (EndTime - StartTime).TotalSeconds;
        public DateTime StartTime
        {
            get { return (DateTime)GetValue(StartTimeProperty); }
            set { SetValue(StartTimeProperty, value); }
        }
        public DateTime EndTime
        {
            get { return (DateTime)GetValue(EndTimeProperty); }
            set { SetValue(EndTimeProperty, value); }
        }
        public bool IsUploaded
        {
            get { return (bool)GetValue(IsUploadedProperty); }
            set { SetValue(IsUploadedProperty, value); }
        }
        public Brush UploadedBackground
        {
            get { return (Brush)GetValue(UploadedBackgroundProperty); }
            set { SetValue(UploadedBackgroundProperty, value); }
        }
        public Brush UnUploadBackground
        {
            get { return (Brush)GetValue(UnUploadBackgroundProperty); }
            set { SetValue(UnUploadBackgroundProperty, value); }
        }

        public static readonly DependencyProperty UnUploadBackgroundProperty =
            DependencyProperty.Register(nameof(UnUploadBackground), typeof(Brush), typeof(DistributionProgressItem));
        public static readonly DependencyProperty UploadedBackgroundProperty =
            DependencyProperty.Register(nameof(UploadedBackground), typeof(Brush), typeof(DistributionProgressItem));
        public static readonly DependencyProperty IsUploadedProperty =
            DependencyProperty.Register(nameof(IsUploaded), typeof(bool), typeof(DistributionProgressItem));
        public static readonly DependencyProperty EndTimeProperty =
            DependencyProperty.Register(nameof(EndTime), typeof(DateTime), typeof(DistributionProgressItem));
        public static readonly DependencyProperty StartTimeProperty =
            DependencyProperty.Register(nameof(StartTime), typeof(DateTime), typeof(DistributionProgressItem));
    }
}
