using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public interface ISelectAction
    {
        ISelectMaskPaint SelectMaskPaint { get; }
        void Click();
        void MouseDown(Point currentPoint);
        void Draging(Point currentPoint);
        void DragOver();
    }
}
