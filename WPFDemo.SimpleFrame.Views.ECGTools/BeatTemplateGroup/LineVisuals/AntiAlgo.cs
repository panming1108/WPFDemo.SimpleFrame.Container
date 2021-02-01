using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
	public sealed class AntiAlgo : IAntiAlgo
	{
		public void DrawLineBresenham(int[] pixels, int pixelWidth, int pixelHeight, int x1, int y1, int x2, int y2, int color)
		{
			int num = x2 - x1;
			int num2 = y2 - y1;
			int num3 = 0;
			if (num < 0)
			{
				num = -num;
				num3 = -1;
			}
			else if (num > 0)
			{
				num3 = 1;
			}
			int num4 = 0;
			if (num2 < 0)
			{
				num2 = -num2;
				num4 = -1;
			}
			else if (num2 > 0)
			{
				num4 = 1;
			}
			int num5;
			int num6;
			int num7;
			int num8;
			int num9;
			int num10;
			if (num > num2)
			{
				num5 = num3;
				num6 = 0;
				num7 = num3;
				num8 = num4;
				num9 = num2;
				num10 = num;
			}
			else
			{
				num5 = 0;
				num6 = num4;
				num7 = num3;
				num8 = num4;
				num9 = num;
				num10 = num2;
			}
			int num11 = x1;
			int num12 = y1;
			int num13 = num10 >> 1;
			if (num12 < pixelHeight && num12 >= 0 && num11 < pixelWidth && num11 >= 0)
			{
				pixels[num12 * pixelWidth + num11] = color;
			}
			for (int i = 0; i < num10; i++)
			{
				num13 -= num9;
				if (num13 < 0)
				{
					num13 += num10;
					num11 += num7;
					num12 += num8;
				}
				else
				{
					num11 += num5;
					num12 += num6;
				}
				if (num12 < pixelHeight && num12 >= 0 && num11 < pixelWidth && num11 >= 0)
				{
					pixels[num12 * pixelWidth + num11] = color;
				}
			}
		}

		public void DrawLineDDA(int[] pixels, int pixelWidth, int pixelHeight, int x1, int y1, int x2, int y2, int color)
		{
			int num = x2 - x1;
			int num2 = y2 - y1;
			int num3 = (num2 >= 0) ? num2 : (-num2);
			int num4 = (num >= 0) ? num : (-num);
			if (num4 > num3)
			{
				num3 = num4;
			}
			if (num3 == 0)
			{
				return;
			}
			float num5 = (float)num / (float)num3;
			float num6 = (float)num2 / (float)num3;
			float num7 = x1;
			float num8 = y1;
			for (int i = 0; i < num3; i++)
			{
				if (num8 < (float)pixelHeight && num8 >= 0f && num7 < (float)pixelWidth && num7 >= 0f)
				{
					pixels[(int)num8 * pixelWidth + (int)num7] = color;
				}
				num7 += num5;
				num8 += num6;
			}
		}

		public void DrawOptimizedDDA(int[] pixels, int pixelWidth, int pixelHeight, int x1, int y1, int x2, int y2, int color)
		{
			int num = x2 - x1;
			int num2 = y2 - y1;
			int num3 = (num2 < 0) ? (-num2) : num2;
			int num4 = (num < 0) ? (-num) : num;
			if (num4 > num3)
			{
				if (num < 0)
				{
					int num5 = x1;
					x1 = x2;
					x2 = num5;
					int num6 = y1;
					y1 = y2;
					y2 = num6;
				}
				int num7 = (num2 << 8) / num;
				int num8 = y1 << 8;
				int num9 = y2 << 8;
				int num10 = pixelHeight << 8;
				if (y1 < y2)
				{
					if (y1 >= pixelHeight || y2 < 0)
					{
						return;
					}
					if (num8 < 0)
					{
						if (num7 == 0)
						{
							return;
						}
						int num11 = num8;
						num8 = num7 - 1 + (num8 + 1) % num7;
						x1 += (num8 - num11) / num7;
					}
					if (num9 >= num10 && num7 != 0)
					{
						num9 = num10 - 1 - (num10 - 1 - num8) % num7;
						x2 = x1 + (num9 - num8) / num7;
					}
				}
				else
				{
					if (y2 >= pixelHeight || y1 < 0)
					{
						return;
					}
					if (num8 >= num10)
					{
						if (num7 == 0)
						{
							return;
						}
						int num12 = num8;
						num8 = num10 - 1 + (num7 - (num10 - 1 - num12) % num7);
						x1 += (num8 - num12) / num7;
					}
					if (num9 < 0 && num7 != 0)
					{
						num9 = num8 % num7;
						x2 = x1 + (num9 - num8) / num7;
					}
				}
				if (x1 < 0)
				{
					num8 -= num7 * x1;
					x1 = 0;
				}
				if (x2 >= pixelWidth)
				{
					x2 = pixelWidth - 1;
				}
				int num13 = num8;
				int num14 = num13 >> 8;
				int num15 = num14;
				int num16 = x1 + num14 * pixelWidth;
				int num17 = (num7 < 0) ? (1 - pixelWidth) : (1 + pixelWidth);
				for (int i = x1; i <= x2; i++)
				{
					pixels[num16] = color;
					num13 += num7;
					num14 = num13 >> 8;
					if (num14 != num15)
					{
						num15 = num14;
						num16 += num17;
					}
					else
					{
						num16++;
					}
				}
			}
			else
			{
				if (num3 == 0)
				{
					return;
				}
				if (num2 < 0)
				{
					int num18 = x1;
					x1 = x2;
					x2 = num18;
					int num19 = y1;
					y1 = y2;
					y2 = num19;
				}
				int num20 = x1 << 8;
				int num21 = x2 << 8;
				int num22 = pixelWidth << 8;
				int num23 = (num << 8) / num2;
				if (x1 < x2)
				{
					if (x1 >= pixelWidth || x2 < 0)
					{
						return;
					}
					if (num20 < 0)
					{
						if (num23 == 0)
						{
							return;
						}
						int num24 = num20;
						num20 = num23 - 1 + (num20 + 1) % num23;
						y1 += (num20 - num24) / num23;
					}
					if (num21 >= num22 && num23 != 0)
					{
						num21 = num22 - 1 - (num22 - 1 - num20) % num23;
						y2 = y1 + (num21 - num20) / num23;
					}
				}
				else
				{
					if (x2 >= pixelWidth || x1 < 0)
					{
						return;
					}
					if (num20 >= num22)
					{
						if (num23 == 0)
						{
							return;
						}
						int num25 = num20;
						num20 = num22 - 1 + (num23 - (num22 - 1 - num25) % num23);
						y1 += (num20 - num25) / num23;
					}
					if (num21 < 0 && num23 != 0)
					{
						num21 = num20 % num23;
						y2 = y1 + (num21 - num20) / num23;
					}
				}
				if (y1 < 0)
				{
					num20 -= num23 * y1;
					y1 = 0;
				}
				if (y2 >= pixelHeight)
				{
					y2 = pixelHeight - 1;
				}
				int num26 = num20 + (y1 * pixelWidth << 8);
				int num27 = (pixelWidth << 8) + num23;
				for (int j = y1; j <= y2; j++)
				{
					pixels[num26 >> 8] = color;
					num26 += num27;
				}
			}
		}

		public void DrawLineAa(int[] pixels, int pixelWidth, int pixelHeight, int x1, int y1, int x2, int y2, int color)
		{
			if (x1 == x2 && y1 == y2)
			{
				return;
			}
			if (x1 < 1)
			{
				x1 = 1;
			}
			if (x1 > pixelWidth - 2)
			{
				x1 = pixelWidth - 2;
			}
			if (y1 < 1)
			{
				y1 = 1;
			}
			if (y1 > pixelHeight - 2)
			{
				y1 = pixelHeight - 2;
			}
			if (x2 < 1)
			{
				x2 = 1;
			}
			if (x2 > pixelWidth - 2)
			{
				x2 = pixelWidth - 2;
			}
			if (y2 < 1)
			{
				y2 = 1;
			}
			if (y2 > pixelHeight - 2)
			{
				y2 = pixelHeight - 2;
			}
			int num = y1 * pixelWidth + x1;
			int num2 = x2 - x1;
			int num3 = y2 - y1;
			int num4 = (color >> 24) & 0xFF;
			uint srb = (uint)(color & 0xFF00FF);
			uint sg = (uint)((color >> 8) & 0xFF);
			int num5 = num2;
			int num6 = num3;
			if (num2 < 0)
			{
				num5 = -num2;
			}
			if (num3 < 0)
			{
				num6 = -num3;
			}
			int num7;
			int num8;
			int num9;
			int num10;
			int num11;
			int num12;
			if (num5 > num6)
			{
				num7 = num5;
				num8 = num6;
				num9 = x2;
				num10 = y2;
				num11 = 1;
				num12 = pixelWidth;
				if (num2 < 0)
				{
					num11 = -num11;
				}
				if (num3 < 0)
				{
					num12 = -num12;
				}
			}
			else
			{
				num7 = num6;
				num8 = num5;
				num9 = y2;
				num10 = x2;
				num11 = pixelWidth;
				num12 = 1;
				if (num3 < 0)
				{
					num11 = -num11;
				}
				if (num2 < 0)
				{
					num12 = -num12;
				}
			}
			int num13 = num9 + num7;
			int num14 = (num8 << 1) - num7;
			int num15 = num8 << 1;
			int num16 = num8 - num7 << 1;
			double num17 = 1.0 / (4.0 * Math.Sqrt(num7 * num7 + num8 * num8));
			double num18 = 0.75 - 2.0 * ((double)num7 * num17);
			int num19 = (int)(num17 * 1024.0);
			int num20 = (int)(num18 * 1024.0 * (double)num4);
			int num21 = (int)(768.0 * (double)num4);
			int num22 = num19 * num4;
			int num23 = num7 * num22;
			int num24 = num14 * num22;
			int num25 = 0;
			int num26 = num15 * num22;
			int num27 = num16 * num22;
			do
			{
				AlphaBlendNormalOnPremultiplied(pixels, num, num21 - num25 >> 10, srb, sg);
				AlphaBlendNormalOnPremultiplied(pixels, num + num12, num20 + num25 >> 10, srb, sg);
				AlphaBlendNormalOnPremultiplied(pixels, num - num12, num20 - num25 >> 10, srb, sg);
				if (num14 < 0)
				{
					num25 = num24 + num23;
					num14 += num15;
					num24 += num26;
				}
				else
				{
					num25 = num24 - num23;
					num14 += num16;
					num24 += num27;
					num10++;
					num += num12;
				}
				num9++;
				num += num11;
			}
			while (num9 < num13);
		}

		private void AlphaBlendNormalOnPremultiplied(int[] pixels, int index, int sa, uint srb, uint sg)
		{
			int num = pixels[index];
			uint num2 = (uint)num >> 24;
			uint num3 = ((uint)num >> 8) & 0xFF;
			uint num4 = (uint)(num & 0xFF00FF);
			pixels[index] = (int)((sa + (num2 * (255 - sa) * 32897 >> 23) << 24) | (((sg - num3) * sa + (num3 << 8)) & 4294967040u) | ((((srb - num4) * sa >> 8) + num4) & 0xFF00FF));
		}

		public void FillEllipse(int[] pixels, int pixelWidth, int pixelHeight, int x1, int y1, int x2, int y2, int color)
		{
			int num = x2 - x1 >> 1;
			int num2 = y2 - y1 >> 1;
			int xc = x1 + num;
			int yc = y1 + num2;
			FillEllipseCentered(pixels, pixelWidth, pixelHeight, xc, yc, num, num2, color);
		}

		public void FillEllipseCentered(int[] pixels, int pixelWidth, int pixelHeight, int xc, int yc, int xr, int yr, int color, bool doAlphaBlend)
		{
			if (xr < 1 || yr < 1 || xc - xr >= pixelWidth || xc + xr < 0 || yc - yr >= pixelHeight || yc + yr < 0)
			{
				return;
			}
			int num = xr;
			int num2 = 0;
			int num3 = xr * xr << 1;
			int num4 = yr * yr << 1;
			int num5 = yr * yr * (1 - (xr << 1));
			int num6 = xr * xr;
			int num7 = 0;
			int num8 = num4 * xr;
			int num9 = 0;
			int num10 = (color >> 24) & 0xFF;
			int sr = (color >> 16) & 0xFF;
			int sg = (color >> 8) & 0xFF;
			int sb = color & 0xFF;
			bool flag = !doAlphaBlend || num10 == 255;
			int num13;
			int num14;
			int num11;
			int num12;
			while (num8 >= num9)
			{
				num11 = yc + num2;
				num12 = yc - num2 - 1;
				if (num11 < 0)
				{
					num11 = 0;
				}
				if (num11 >= pixelHeight)
				{
					num11 = pixelHeight - 1;
				}
				if (num12 < 0)
				{
					num12 = 0;
				}
				if (num12 >= pixelHeight)
				{
					num12 = pixelHeight - 1;
				}
				num13 = num11 * pixelWidth;
				num14 = num12 * pixelWidth;
				int num15 = xc + num;
				int num16 = xc - num;
				if (num15 < 0)
				{
					num15 = 0;
				}
				if (num15 >= pixelWidth)
				{
					num15 = pixelWidth - 1;
				}
				if (num16 < 0)
				{
					num16 = 0;
				}
				if (num16 >= pixelWidth)
				{
					num16 = pixelWidth - 1;
				}
				if (flag)
				{
					for (int i = num16; i <= num15; i++)
					{
						pixels[i + num13] = color;
						pixels[i + num14] = color;
					}
				}
				else
				{
					for (int j = num16; j <= num15; j++)
					{
						pixels[j + num13] = AlphaBlendColors(pixels[j + num13], num10, sr, sg, sb);
						pixels[j + num14] = AlphaBlendColors(pixels[j + num14], num10, sr, sg, sb);
					}
				}
				num2++;
				num9 += num3;
				num7 += num6;
				num6 += num3;
				if (num5 + (num7 << 1) > 0)
				{
					num--;
					num8 -= num4;
					num7 += num5;
					num5 += num4;
				}
			}
			num = 0;
			num2 = yr;
			num11 = yc + num2;
			num12 = yc - num2;
			if (num11 < 0)
			{
				num11 = 0;
			}
			if (num11 >= pixelHeight)
			{
				num11 = pixelHeight - 1;
			}
			if (num12 < 0)
			{
				num12 = 0;
			}
			if (num12 >= pixelHeight)
			{
				num12 = pixelHeight - 1;
			}
			num13 = num11 * pixelWidth;
			num14 = num12 * pixelWidth;
			num5 = yr * yr;
			num6 = xr * xr * (1 - (yr << 1));
			num7 = 0;
			num8 = 0;
			num9 = num3 * yr;
			while (num8 <= num9)
			{
				int num15 = xc + num;
				int num16 = xc - num;
				if (num15 < 0)
				{
					num15 = 0;
				}
				if (num15 >= pixelWidth)
				{
					num15 = pixelWidth - 1;
				}
				if (num16 < 0)
				{
					num16 = 0;
				}
				if (num16 >= pixelWidth)
				{
					num16 = pixelWidth - 1;
				}
				if (flag)
				{
					for (int k = num16; k <= num15; k++)
					{
						pixels[k + num13] = color;
						pixels[k + num14] = color;
					}
				}
				else
				{
					for (int l = num16; l <= num15; l++)
					{
						pixels[l + num13] = AlphaBlendColors(pixels[l + num13], num10, sr, sg, sb);
						pixels[l + num14] = AlphaBlendColors(pixels[l + num14], num10, sr, sg, sb);
					}
				}
				num++;
				num8 += num4;
				num7 += num5;
				num5 += num4;
				if (num6 + (num7 << 1) > 0)
				{
					num2--;
					num11 = yc + num2;
					num12 = yc - num2;
					if (num11 < 0)
					{
						num11 = 0;
					}
					if (num11 >= pixelHeight)
					{
						num11 = pixelHeight - 1;
					}
					if (num12 < 0)
					{
						num12 = 0;
					}
					if (num12 >= pixelHeight)
					{
						num12 = pixelHeight - 1;
					}
					num13 = num11 * pixelWidth;
					num14 = num12 * pixelWidth;
					num9 -= num3;
					num7 += num6;
					num6 += num3;
				}
			}
		}

		private int AlphaBlendColors(int pixel, int sa, int sr, int sg, int sb)
		{
			int num = (pixel >> 24) & 0xFF;
			int num2 = (pixel >> 16) & 0xFF;
			int num3 = (pixel >> 8) & 0xFF;
			int num4 = pixel & 0xFF;
			return (sa + (num * (255 - sa) * 32897 >> 23) << 24) | (sr + (num2 * (255 - sa) * 32897 >> 23) << 16) | (sg + (num3 * (255 - sa) * 32897 >> 23) << 8) | (sb + (num4 * (255 - sa) * 32897 >> 23));
		}

		public void FillEllipseCentered(int[] pixels, int pixelWidth, int pixelHeight, int xc, int yc, int xr, int yr, int color)
		{
			if (xr < 1 || yr < 1)
			{
				return;
			}
			int num = xr;
			int num2 = 0;
			int num3 = xr * xr << 1;
			int num4 = yr * yr << 1;
			int num5 = yr * yr * (1 - (xr << 1));
			int num6 = xr * xr;
			int num7 = 0;
			int num8 = num4 * xr;
			int num9 = 0;
			int num12;
			int num13;
			int num10;
			int num11;
			while (num8 >= num9)
			{
				num10 = yc + num2;
				num11 = yc - num2;
				if (num10 < 0)
				{
					num10 = 0;
				}
				if (num10 >= pixelHeight)
				{
					num10 = pixelHeight - 1;
				}
				if (num11 < 0)
				{
					num11 = 0;
				}
				if (num11 >= pixelHeight)
				{
					num11 = pixelHeight - 1;
				}
				num12 = num10 * pixelWidth;
				num13 = num11 * pixelWidth;
				int num14 = xc + num;
				int num15 = xc - num;
				if (num14 < 0)
				{
					num14 = 0;
				}
				if (num14 >= pixelWidth)
				{
					num14 = pixelWidth - 1;
				}
				if (num15 < 0)
				{
					num15 = 0;
				}
				if (num15 >= pixelWidth)
				{
					num15 = pixelWidth - 1;
				}
				for (int i = num15; i <= num14; i++)
				{
					pixels[i + num12] = color;
					pixels[i + num13] = color;
				}
				num2++;
				num9 += num3;
				num7 += num6;
				num6 += num3;
				if (num5 + (num7 << 1) > 0)
				{
					num--;
					num8 -= num4;
					num7 += num5;
					num5 += num4;
				}
			}
			num = 0;
			num2 = yr;
			num10 = yc + num2;
			num11 = yc - num2;
			if (num10 < 0)
			{
				num10 = 0;
			}
			if (num10 >= pixelHeight)
			{
				num10 = pixelHeight - 1;
			}
			if (num11 < 0)
			{
				num11 = 0;
			}
			if (num11 >= pixelHeight)
			{
				num11 = pixelHeight - 1;
			}
			num12 = num10 * pixelWidth;
			num13 = num11 * pixelWidth;
			num5 = yr * yr;
			num6 = xr * xr * (1 - (yr << 1));
			num7 = 0;
			num8 = 0;
			num9 = num3 * yr;
			while (num8 <= num9)
			{
				int num14 = xc + num;
				int num15 = xc - num;
				if (num14 < 0)
				{
					num14 = 0;
				}
				if (num14 >= pixelWidth)
				{
					num14 = pixelWidth - 1;
				}
				if (num15 < 0)
				{
					num15 = 0;
				}
				if (num15 >= pixelWidth)
				{
					num15 = pixelWidth - 1;
				}
				for (int j = num15; j <= num14; j++)
				{
					pixels[j + num12] = color;
					pixels[j + num13] = color;
				}
				num++;
				num8 += num4;
				num7 += num5;
				num5 += num4;
				if (num6 + (num7 << 1) > 0)
				{
					num2--;
					num10 = yc + num2;
					num11 = yc - num2;
					if (num10 < 0)
					{
						num10 = 0;
					}
					if (num10 >= pixelHeight)
					{
						num10 = pixelHeight - 1;
					}
					if (num11 < 0)
					{
						num11 = 0;
					}
					if (num11 >= pixelHeight)
					{
						num11 = pixelHeight - 1;
					}
					num12 = num10 * pixelWidth;
					num13 = num11 * pixelWidth;
					num9 -= num3;
					num7 += num6;
					num6 += num3;
				}
			}
		}

		public void DrawEllipse(int[] pixels, int pixelWidth, int pixelHeight, int x1, int y1, int x2, int y2, int color)
		{
			int num = x2 - x1 >> 1;
			int num2 = y2 - y1 >> 1;
			int xc = x1 + num;
			int yc = y1 + num2;
			DrawEllipseCentered(pixels, pixelWidth, pixelHeight, xc, yc, num, num2, color);
		}

		private void DrawEllipseCentered(int[] pixels, int pixelWidth, int pixelHeight, int xc, int yc, int xr, int yr, int color)
		{
			if (xr < 1 || yr < 1)
			{
				return;
			}
			int num = xr;
			int num2 = 0;
			int num3 = xr * xr << 1;
			int num4 = yr * yr << 1;
			int num5 = yr * yr * (1 - (xr << 1));
			int num6 = xr * xr;
			int num7 = 0;
			int num8 = num4 * xr;
			int num9 = 0;
			int num12;
			int num13;
			int num10;
			int num11;
			while (num8 >= num9)
			{
				num10 = yc + num2;
				num11 = yc - num2;
				if (num10 < 0)
				{
					num10 = 0;
				}
				if (num10 >= pixelHeight)
				{
					num10 = pixelHeight - 1;
				}
				if (num11 < 0)
				{
					num11 = 0;
				}
				if (num11 >= pixelHeight)
				{
					num11 = pixelHeight - 1;
				}
				num12 = num10 * pixelWidth;
				num13 = num11 * pixelWidth;
				int num14 = xc + num;
				int num15 = xc - num;
				if (num14 < 0)
				{
					num14 = 0;
				}
				if (num14 >= pixelWidth)
				{
					num14 = pixelWidth - 1;
				}
				if (num15 < 0)
				{
					num15 = 0;
				}
				if (num15 >= pixelWidth)
				{
					num15 = pixelWidth - 1;
				}
				pixels[num14 + num12] = color;
				pixels[num15 + num12] = color;
				pixels[num15 + num13] = color;
				pixels[num14 + num13] = color;
				num2++;
				num9 += num3;
				num7 += num6;
				num6 += num3;
				if (num5 + (num7 << 1) > 0)
				{
					num--;
					num8 -= num4;
					num7 += num5;
					num5 += num4;
				}
			}
			num = 0;
			num2 = yr;
			num10 = yc + num2;
			num11 = yc - num2;
			if (num10 < 0)
			{
				num10 = 0;
			}
			if (num10 >= pixelHeight)
			{
				num10 = pixelHeight - 1;
			}
			if (num11 < 0)
			{
				num11 = 0;
			}
			if (num11 >= pixelHeight)
			{
				num11 = pixelHeight - 1;
			}
			num12 = num10 * pixelWidth;
			num13 = num11 * pixelWidth;
			num5 = yr * yr;
			num6 = xr * xr * (1 - (yr << 1));
			num7 = 0;
			num8 = 0;
			num9 = num3 * yr;
			while (num8 <= num9)
			{
				int num14 = xc + num;
				int num15 = xc - num;
				if (num14 < 0)
				{
					num14 = 0;
				}
				if (num14 >= pixelWidth)
				{
					num14 = pixelWidth - 1;
				}
				if (num15 < 0)
				{
					num15 = 0;
				}
				if (num15 >= pixelWidth)
				{
					num15 = pixelWidth - 1;
				}
				pixels[num14 + num12] = color;
				pixels[num15 + num12] = color;
				pixels[num15 + num13] = color;
				pixels[num14 + num13] = color;
				num++;
				num8 += num4;
				num7 += num5;
				num5 += num4;
				if (num6 + (num7 << 1) > 0)
				{
					num2--;
					num10 = yc + num2;
					num11 = yc - num2;
					if (num10 < 0)
					{
						num10 = 0;
					}
					if (num10 >= pixelHeight)
					{
						num10 = pixelHeight - 1;
					}
					if (num11 < 0)
					{
						num11 = 0;
					}
					if (num11 >= pixelHeight)
					{
						num11 = pixelHeight - 1;
					}
					num12 = num10 * pixelWidth;
					num13 = num11 * pixelWidth;
					num9 -= num3;
					num7 += num6;
					num6 += num3;
				}
			}
		}
	}
}
