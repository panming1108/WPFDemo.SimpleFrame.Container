using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs.DataGrid
{
    public class DataGridIconsConverter : DependencyObject, IMultiValueConverter
    {
        public static IMultiValueConverter ValueConverter { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return ValueConverter?.Convert(values, targetType, parameter, culture);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return ValueConverter?.ConvertBack(value, targetTypes, parameter, culture);
        }
    }
}
