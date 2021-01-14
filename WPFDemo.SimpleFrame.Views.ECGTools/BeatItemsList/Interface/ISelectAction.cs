using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatItemsList
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
