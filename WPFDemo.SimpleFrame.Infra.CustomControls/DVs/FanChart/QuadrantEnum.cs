using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DVs.FanChart
{
    public enum QuadrantEnum
    {
        [Description("第一象限")]
        First,
        [Description("Y正半轴")]
        PlusY,
        [Description("第二象限")]
        Second,
        [Description("X负半轴")]
        MinusX,
        [Description("第三象限")]
        Third,
        [Description("Y负半轴")]
        MinusY,
        [Description("第四象限")]
        Forth,
        [Description("X正半轴")]
        PlusX,
        [Description("原点")]
        Origin
    }
}
