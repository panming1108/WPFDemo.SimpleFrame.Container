using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs.DataGrid
{
    public class TextLengthConverter : IValueConverter
    {
        public int DisplayCharactorsCount { get; set; }
        public string ReplaceString { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {
                string txt = value.ToString();
                if(DisplayCharactorsCount <= 0)
                {
                    return txt;
                }
                else
                {
                    if(txt.Length <= DisplayCharactorsCount)
                    {
                       return txt;
                    }
                    else
                    {
                        return txt.Substring(0, DisplayCharactorsCount) + ReplaceString?.ToString();
                    }
                }
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
