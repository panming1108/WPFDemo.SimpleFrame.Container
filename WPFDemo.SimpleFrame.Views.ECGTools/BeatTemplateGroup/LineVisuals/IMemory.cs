using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public interface IMemory
    {
        int[] Merge(byte stride, int pixelWidth, int pixelHeight, PixelRect dirArea, IntPtr src);
    }
}
