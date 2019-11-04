using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using WPFDemo.SimpleFrame.Infra.Models;

namespace WPFDemo.SimpleFrame.Views.Test.Converters
{
    public class IconConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values != null && values.Count() >= 2)
            {
                var stu = values[0] as Student;
                var img = (IconModel)values[1];
                switch (img.Source)
                {
                    case "/WPFDemo.SimpleFrame.Views.Test;component/Images/critical.png":
                        if(stu.Id % 4 == 3 && stu.Id % 3 == 2)
                        {
                            return Visibility.Visible;
                        }
                        else
                        {
                            return Visibility.Collapsed;
                        }
                    case "/WPFDemo.SimpleFrame.Views.Test;component/Images/urgent.png":
                        if (stu.Id % 4 == 2 && stu.Id % 3 == 2)
                        {
                            return Visibility.Visible;
                        }
                        else
                        {
                            return Visibility.Collapsed;
                        }
                    case "/WPFDemo.SimpleFrame.Views.Test;component/Images/warning.png":
                        if (stu.Id % 4 == 1 && stu.Id % 3 == 1)
                        {
                            return Visibility.Visible;
                        }
                        else
                        {
                            return Visibility.Collapsed;
                        }
                    default:
                        break;
                }
            }
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
