using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
	public struct PixelRect
	{
		private readonly int _width;

		private readonly int _height;

		private readonly int _x;

		private readonly int _y;

		public int Width => _width;

		public int Height => _height;

		public int X => _x;

		public int Y => _y;

		public PixelPoint BottomRight => new PixelPoint(_x + _width - 1, _y + _height - 1);

		public PixelPoint BottomLeft => new PixelPoint(_x, _y + _height - 1);

		public PixelPoint TopRight => new PixelPoint(_x + _width - 1, _y);

		public PixelPoint TopLeft => new PixelPoint(_x, _y);

		public PixelPoint Center => new PixelPoint(_x + _width / 2, _y + _height / 2);

		public static PixelRect Empty => new PixelRect(0, 0, 0, 0);

		public bool IsEmpty
		{
			get
			{
				if (_width > 0)
				{
					return _height <= 0;
				}
				return true;
			}
		}

		public PixelRect(PixelPoint[] points)
		{
			int x = points[0].X;
			int y = points[0].Y;
			int x2 = points[0].X;
			int y2 = points[0].Y;
			for (int i = 0; i < points.Length; i++)
			{
				PixelPoint pixelPoint = points[i];
				if (pixelPoint.X > x)
				{
					x = pixelPoint.X;
				}
				else if (pixelPoint.X < x2)
				{
					x2 = pixelPoint.X;
				}
				if (pixelPoint.Y > y)
				{
					y = pixelPoint.Y;
				}
				else if (pixelPoint.Y < y2)
				{
					y2 = pixelPoint.Y;
				}
			}
			_x = x2;
			_y = y2;
			_width = x - x2 + 1;
			_height = y - y2 + 1;
		}

		public PixelRect(PixelPoint p1, PixelPoint p2)
		{
			this = new PixelRect(new PixelPoint[2]
			{
			p1,
			p2
			});
		}

		public PixelRect(PixelRect rect1, PixelRect rect2)
		{
			this = new PixelRect(new PixelPoint[4]
			{
			rect1.TopLeft,
			rect1.BottomRight,
			rect2.TopLeft,
			rect2.BottomRight
			});
		}

		public PixelRect(int x, int y, int width, int height)
		{
			_width = width;
			_height = height;
			_x = x;
			_y = y;
		}

		public PixelRect(int width, int height)
		{
			_width = width;
			_height = height;
			_x = 0;
			_y = 0;
		}

		public static bool operator ==(PixelRect rect1, PixelRect rect2)
		{
			if (rect1.X == rect2.X && rect1.Y == rect2.Y && rect1.Width == rect2.Width)
			{
				return rect1.Height == rect2.Height;
			}
			return false;
		}

		public static bool operator !=(PixelRect rect1, PixelRect rect2)
		{
			return !(rect1 == rect2);
		}

		public static bool Equals(PixelRect rect1, PixelRect rect2)
		{
			if (rect1.IsEmpty)
			{
				return rect2.IsEmpty;
			}
			if (rect1.X.Equals(rect2.X) && rect1.Y.Equals(rect2.Y) && rect1.Width.Equals(rect2.Width))
			{
				return rect1.Height.Equals(rect2.Height);
			}
			return false;
		}

		public override bool Equals(object o)
		{
			if (o == null || !(o is PixelRect))
			{
				return false;
			}
			PixelRect rect = (PixelRect)o;
			return Equals(this, rect);
		}

		public bool Equals(PixelRect value)
		{
			return Equals(this, value);
		}

		public override int GetHashCode()
		{
			if (IsEmpty)
			{
				return 0;
			}
			return X.GetHashCode() ^ Y.GetHashCode() ^ Width.GetHashCode() ^ Height.GetHashCode();
		}
	}
}
