using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.UXs.ProgressBar
{
    public class DistributionProgressBar : ItemsControl
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
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
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
            DependencyProperty.Register(nameof(UnUploadBackground), typeof(Brush), typeof(DistributionProgressBar));
        public static readonly DependencyProperty UploadedBackgroundProperty =
            DependencyProperty.Register(nameof(UploadedBackground), typeof(Brush), typeof(DistributionProgressBar));
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(DistributionProgressBar));
        public static readonly DependencyProperty EndTimeProperty =
            DependencyProperty.Register(nameof(EndTime), typeof(DateTime), typeof(DistributionProgressBar));
        public static readonly DependencyProperty StartTimeProperty =
            DependencyProperty.Register(nameof(StartTime), typeof(DateTime), typeof(DistributionProgressBar));


        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is DistributionProgressItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new DistributionProgressItem();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            DistributionProgressItem progressItem = element as DistributionProgressItem;
            progressItem.Width = progressItem.TotalSeconds / TotalSeconds * ActualWidth;
        }
    }
}
