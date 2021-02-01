using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public interface IDimAlgo
    {
        int[] DownDim(IEnumerable<short[]> source, short widthStart, short heightStart, short width, short height);
    }
}
