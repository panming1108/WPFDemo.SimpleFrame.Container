using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
	public interface IAntiAlgo
	{
		void DrawLineBresenham(int[] pixels, int pixelWidth, int pixelHeight, int x1, int y1, int x2, int y2, int color);

		void DrawLineDDA(int[] pixels, int pixelWidth, int pixelHeight, int x1, int y1, int x2, int y2, int color);

		void DrawOptimizedDDA(int[] pixels, int pixelWidth, int pixelHeight, int x1, int y1, int x2, int y2, int color);

		void DrawLineAa(int[] pixels, int pixelWidth, int pixelHeight, int x1, int y1, int x2, int y2, int color);

		void FillEllipse(int[] pixels, int pixelWidth, int pixelHeight, int x1, int y1, int x2, int y2, int color);

		void FillEllipseCentered(int[] pixels, int pixelWidth, int pixelHeight, int xc, int yc, int xr, int yr, int color);

		void FillEllipseCentered(int[] pixels, int pixelWidth, int pixelHeight, int xc, int yc, int xr, int yr, int color, bool doAlphaBlend);

		void DrawEllipse(int[] pixels, int pixelWidth, int pixelHeight, int x1, int y1, int x2, int y2, int color);
	}
}
