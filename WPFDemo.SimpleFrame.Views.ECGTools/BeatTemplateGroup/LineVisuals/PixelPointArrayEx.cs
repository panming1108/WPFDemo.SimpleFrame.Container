using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
	public class PixelPointArrayEx
	{
		private readonly int _type;

		private readonly IList<PixelPoint> _points;

		public int Type => _type;

		public IList<PixelPoint> Points => _points;

		public PixelPointArrayEx(int type, IList<PixelPoint> points)
		{
			_type = type;
			_points = points;
		}
	}
}
