using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs.ListBox
{
    public class DpObject : Freezable
    {

        public List<string> Source
        {
            get { return (List<string>)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(List<string>), typeof(DpObject), new PropertyMetadata(OnSourceChanged));

        public string Result
        {
            get { return (string)GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }

        public static readonly DependencyProperty ResultProperty =
            DependencyProperty.Register(nameof(Result), typeof(string), typeof(DpObject));



        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DpObject dpObject = d as DpObject;
            if(dpObject.Source.Count > 0)
            {
                dpObject.Result = dpObject.Source[0];
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new DpObject();
        }
    }
}
