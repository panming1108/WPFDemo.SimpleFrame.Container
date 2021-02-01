using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
	public interface IPostionPolyFilter
	{
		bool Filter(PixelRect approx, IPositionAlgorithmEx positionAlgorithmEx, short[] pixelPoints);

		int[] Filter(PixelRect approx, PixelRect dirArea, IPositionAlgorithmEx positionAlgorithmEx);
	}
}
