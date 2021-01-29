using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public enum EventCodeEnum
    {
        [Description("房早单发")]
        SVRSingle,

        [Description("房早二联律")]
        SVRBigeminy,

        [Description("房早三联律")]
        SVRTrigeminy,

        [Description("房早成对")]
        SVRPair,

        [Description("房性心动过速")]
        SVROverSpeed,

        [Description("房早伴差传")]
        SVRDifference,

        [Description("房性逸搏")]
        SVREscape,
    }
}
