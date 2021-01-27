using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public interface IWorkAction
    {
        void Click();
        void MouseDown(Point currentPoint);
        bool Draging(Point currentPoint);
        void DragOver();
    }
}
