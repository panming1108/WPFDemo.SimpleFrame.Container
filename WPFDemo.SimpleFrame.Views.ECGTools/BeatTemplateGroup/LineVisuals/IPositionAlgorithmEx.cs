using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    public interface IPositionAlgorithmEx
    {
        bool Judge(int x, int y);
    }
}
