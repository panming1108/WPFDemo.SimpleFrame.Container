using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public enum BeatTypeEnum
    {
        [Description("窦性")]
        N,
        [Description("房性")]
        S,
        [Description("室性")]
        V
    }
}
