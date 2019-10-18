using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.Editors
{
    public class EditorsAttachProperty
    {
        public static string GetWatermark(DependencyObject obj)
        {
            return (string)obj.GetValue(WatermarkProperty);
        }

        public static void SetWatermark(DependencyObject obj, string value)
        {
            obj.SetValue(WatermarkProperty, value);
        }

        // Using a DependencyProperty as the backing store for Watermark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached("Watermark", typeof(string), typeof(EditorsAttachProperty), new PropertyMetadata(""));


        public static ControlTemplate GetAttachContent(DependencyObject obj)
        {
            return (ControlTemplate)obj.GetValue(AttachContentProperty);
        }

        public static void SetAttachContent(DependencyObject obj, ControlTemplate value)
        {
            obj.SetValue(AttachContentProperty, value);
        }

        // Using a DependencyProperty as the backing store for AttachContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AttachContentProperty =
            DependencyProperty.RegisterAttached("AttachContent", typeof(ControlTemplate), typeof(EditorsAttachProperty), new PropertyMetadata(null));
    }
}
