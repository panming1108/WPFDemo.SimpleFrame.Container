using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
	public struct RenderPen
	{
		private readonly int _brush;

		private readonly double _thickness;

		public int Brush => _brush;

		public double Thickness => _thickness;

		public RenderPen(int brush, double thickness)
		{
			_brush = brush;
			_thickness = thickness;
		}
	}
}
