using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public interface IContainerBehavior
    {
        void MouseLeftButtonDown(Canvas canvas, object sender, MouseButtonEventArgs e);

        void MouseLeftButtonUp(Canvas canvas, object sender, MouseButtonEventArgs e);

        void MouseMove(Canvas canvas, object sender);
    }
}
