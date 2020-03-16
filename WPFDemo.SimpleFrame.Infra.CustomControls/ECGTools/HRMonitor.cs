using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.ECGTools
{
    public class HRMonitor : TextBox
    {
        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }
        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public Brush AbnormalBrush
        {
            get { return (Brush)GetValue(AbnormalBrushProperty); }
            set { SetValue(AbnormalBrushProperty, value); }
        }
        public double UnitFontSize
        {
            get { return (double)GetValue(UnitFontSizeProperty); }
            set { SetValue(UnitFontSizeProperty, value); }
        }
        public int HR
        {
            get { return (int)GetValue(HRProperty); }
            set { SetValue(HRProperty, value); }
        }

        public Brush NormalBrush
        {
            get { return (Brush)GetValue(NormalBrushProperty); }
            set { SetValue(NormalBrushProperty, value); }
        }
        public static readonly DependencyProperty NormalBrushProperty =
            DependencyProperty.Register("NormalBrush", typeof(Brush), typeof(HRMonitor));
        public static readonly DependencyProperty HRProperty =
            DependencyProperty.Register("HR", typeof(int), typeof(HRMonitor), new PropertyMetadata(OnHRChanged));
        public static readonly DependencyProperty UnitFontSizeProperty =
            DependencyProperty.Register("UnitFontSize", typeof(double), typeof(HRMonitor));
        public static readonly DependencyProperty AbnormalBrushProperty =
            DependencyProperty.Register("AbnormalBrush", typeof(Brush), typeof(HRMonitor));
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(int), typeof(HRMonitor));
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(int), typeof(HRMonitor));
        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register("Unit", typeof(string), typeof(HRMonitor));

        private static void OnHRChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HRMonitor hRMonitor = d as HRMonitor;
            if(hRMonitor == null)
            {
                return;
            }
            int newHR = (int)e.NewValue;
            hRMonitor.ChangeForeground(newHR);
        }

        private void ChangeForeground(int hR)
        {
            if(hR < Minimum || hR > Maximum)
            {
                Foreground = AbnormalBrush;
            }
            else
            {
                Foreground = NormalBrush;
            }
        }
    }
}
