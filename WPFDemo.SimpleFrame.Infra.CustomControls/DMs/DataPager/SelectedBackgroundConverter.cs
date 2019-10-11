using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs.DataPager
{
    public class SelectedBackgroundConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
            {
                return false;
            }
            string strInputValue = values[0].ToString();
            int inputValue = 0;
            bool inputValueIsNumber = int.TryParse(values[0].ToString(), out inputValue);
            int nowTime = 0;
            bool nowValueIsNumber = int.TryParse(values[1].ToString(), out nowTime);
            if (inputValueIsNumber && nowValueIsNumber)
            {
                return inputValue == nowTime;
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public static class DataPagerConverter
    {
        private static readonly SelectedBackgroundConverter _converter = new SelectedBackgroundConverter();

        public static SelectedBackgroundConverter SelectedBackgroundConverter
        {
            get => _converter;
        }
    }
}
