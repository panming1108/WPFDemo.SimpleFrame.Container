using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using WPFDemo.SimpleFrame.Container.IViewModels;

namespace WPFDemo.SimpleFrame.Container
{
    public class DateTimeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            if (DateTime.TryParse(value.ToString() ,out DateTime date))
            {
                if(date.Year == 2019)
                {
                    return new ValidationResult(true, string.Empty);
                }
                else
                {
                    return new ValidationResult(false, "不是2019");
                }
            }
            else
            {
                return new ValidationResult(false, "不是日期格式");
            }
        }
    }
}
