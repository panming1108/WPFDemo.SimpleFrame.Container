using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
	public interface IPixelAlgo
	{
		void MapToBackBuffer(byte stride, int pixelWidth, int pixelHeight, int[] backBuffer, PixelRect dirArea, int[] map, int[] colors);

		void MapToBackBuffer(byte stride, int pixelWidth, int pixelHeight, IntPtr backBuffer, PixelRect dirArea, int[] map, int[] colors);

		void RectToBackBuffer(byte stride, int pixelWidth, int pixelHeight, int[] backBuffer, PixelRect dirArea, PixelRect pixelRect, int color);

		void RectToBackBuffer(byte stride, int pixelWidth, int pixelHeight, IntPtr backBuffer, PixelRect dirArea, PixelRect pixelRect, int color);

		void PointsToBackBuffer(byte stride, int pixelWidth, int pixelHeight, int[] backBuffer, PixelRect dirArea, IList<PixelPointEx> pixelPoints, Dictionary<int, int> colors);

		void PointsToBackBuffer(byte stride, int pixelWidth, int pixelHeight, IntPtr backBuffer, PixelRect dirArea, IList<PixelPointEx> pixelPoints, Dictionary<int, int> colors);
	}
}
