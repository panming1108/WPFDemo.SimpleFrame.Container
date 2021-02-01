using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
	public sealed class PixelAlgo : IPixelAlgo, IMemory, IPostionPolyFilter, IDimAlgo
	{
		public unsafe void RectToBackBuffer(byte stride, int pixelWidth, int pixelHeight, IntPtr backBuffer, PixelRect dirArea, PixelRect pixelRect, int color)
		{
			int num = stride * pixelWidth;
			int x = pixelRect.X;
			int y = pixelRect.Y;
			int width = pixelRect.Width;
			int height = pixelRect.Height;
			backBuffer += num * y + stride * x;
			for (int i = 0; i < height; i++)
			{
				int* ptr = (int*)(void*)backBuffer;
				for (int j = 0; j < width; j++)
				{
					*ptr = color;
					ptr++;
				}
				backBuffer += num;
			}
		}

		public unsafe void MapToBackBuffer(byte stride, int pixelWidth, int pixelHeight, IntPtr backBuffer, PixelRect dirArea, int[] map, int[] colors)
		{
			int num = stride * pixelWidth;
			int num2 = num * dirArea.Y + stride * dirArea.X;
			if (num2 != 0)
			{
				backBuffer += num2;
			}
			for (int i = 0; i < dirArea.Height; i++)
			{
				int* ptr = (int*)(void*)backBuffer;
				int num3 = i * dirArea.Width;
				for (int j = 0; j < dirArea.Width; j++)
				{
					*ptr = colors[map[j + num3]];
					ptr++;
				}
				backBuffer += num;
			}
		}

		public unsafe int[] Merge(byte stride, int pixelWidth, int pixelHeight, PixelRect dirArea, IntPtr src)
		{
			int[] array = new int[dirArea.Width * dirArea.Height];
			int num = stride * pixelWidth;
			IntPtr value = src + num * dirArea.Y + stride * dirArea.X;
			for (int i = 0; i < dirArea.Height; i++)
			{
				int* ptr = (int*)(void*)value;
				int num2 = i * dirArea.Width;
				for (int j = 0; j < dirArea.Width; j++)
				{
					array[j + num2] = (int)ptr;
					ptr++;
				}
				value += num;
			}
			return array;
		}

		public void RectToBackBuffer(byte stride, int pixelWidth, int pixelHeight, int[] backBuffer, PixelRect dirArea, PixelRect pixelRect, int color)
		{
			int x = pixelRect.X;
			int y = pixelRect.Y;
			int x2 = pixelRect.TopRight.X;
			int y2 = pixelRect.BottomRight.Y;
			for (int i = y; i <= y2; i++)
			{
				int num = dirArea.Width * (i - dirArea.Y);
				for (int j = x; j <= x2; j++)
				{
					backBuffer[j - dirArea.X + num] = color;
				}
			}
		}

		public int[] Filter(PixelRect approx, PixelRect dirArea, IPositionAlgorithmEx positionAlgorithmEx)
		{
			int[] array = new int[dirArea.Width * dirArea.Height];
			int width = dirArea.Width;
			int x = approx.X;
			int y = approx.Y;
			int x2 = approx.TopRight.X;
			int y2 = approx.BottomRight.Y;
			for (int i = y; i <= y2; i++)
			{
				int num = width * (i - dirArea.Y);
				for (int j = x; j <= x2; j++)
				{
					if (positionAlgorithmEx.Judge(j, i))
					{
						array[j - dirArea.X + num]++;
					}
				}
			}
			return array;
		}

		public bool Filter(PixelRect approx, IPositionAlgorithmEx positionAlgorithmEx, short[] pixelPoints)
		{
			bool result = false;
			int x = approx.X;
			int y = approx.Y;
			int x2 = approx.TopRight.X;
			int y2 = approx.BottomRight.Y;
			for (int i = x; i <= x2; i++)
			{
				int num = pixelPoints[i];
				if (num >= y && num <= y2 && positionAlgorithmEx.Judge(i, num))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		public int[] DownDim(IEnumerable<short[]> source, short widthStart, short heightStart, short width, short height)
		{
			int[] array = new int[width * height];
			int num = 0;
			int num2 = width + widthStart;
			foreach (short[] item in source)
			{
				num = 0;
				for (int i = widthStart; i < num2; i++)
				{
					int num3 = item[i] - heightStart;
					if (num3 >= height || num3 < 0)
					{
						num++;
					}
					else
					{
						array[width * num3 + num]++;
						num++;
					}
				}
			}
			return array;
		}

		public void PointsToBackBuffer(byte stride, int pixelWidth, int pixelHeight, int[] backBuffer, PixelRect dirArea, IList<PixelPointEx> pixelPoints, Dictionary<int, int> colors)
		{
			foreach (PixelPointEx pixelPoint in pixelPoints)
			{
				backBuffer[pixelPoint.PixelPoint.X - dirArea.X + (pixelPoint.PixelPoint.Y - dirArea.Y) * dirArea.Width] = colors[pixelPoint.Type];
			}
		}

		public unsafe void PointsToBackBuffer(byte stride, int pixelWidth, int pixelHeight, IntPtr backBuffer, PixelRect dirArea, IList<PixelPointEx> pixelPoints, Dictionary<int, int> colors)
		{
			int num = stride * pixelWidth;
			foreach (PixelPointEx pixelPoint in pixelPoints)
			{
				*(int*)(void*)(backBuffer + pixelPoint.PixelPoint.Y * num + pixelPoint.PixelPoint.X * stride) = colors[pixelPoint.Type];
			}
		}

		public void MapToBackBuffer(byte stride, int pixelWidth, int pixelHeight, int[] backBuffer, PixelRect dirArea, int[] map, int[] colors)
		{
			for (int i = 0; i < backBuffer.Length; i++)
			{
				backBuffer[i] = colors[map[i]];
			}
		}
	}
}
