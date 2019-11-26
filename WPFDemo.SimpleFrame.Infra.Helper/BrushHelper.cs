using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Infra.Helper
{
    public static class BrushHelper
    {
        public static Brush ConvertToBrushFromString(string colorString)
        {
            BrushConverter brushConverter = new BrushConverter();
            return (Brush)brushConverter.ConvertFromString(colorString);
        }

        public static Color ConvertToColorFromString(string colorString)
        {
            return (Color)ColorConverter.ConvertFromString(colorString);
        }
    }
}
