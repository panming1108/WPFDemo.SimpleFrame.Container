using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
	public struct PixelPoint
	{
		private readonly int _x;

		private readonly int _y;

		public int X => _x;

		public int Y => _y;

		public PixelPoint(int x, int y)
		{
			_x = x;
			_y = y;
		}

		public static bool operator ==(PixelPoint point1, PixelPoint point2)
		{
			if (point1.X == point2.X)
			{
				return point1.Y == point2.Y;
			}
			return false;
		}

		public static bool operator !=(PixelPoint point1, PixelPoint point2)
		{
			return !(point1 == point2);
		}

		public static bool Equals(PixelPoint point1, PixelPoint point2)
		{
			if (point1.X.Equals(point2.X))
			{
				return point1.Y.Equals(point2.Y);
			}
			return false;
		}

		public override bool Equals(object o)
		{
			if (o == null || !(o is PixelPoint))
			{
				return false;
			}
			PixelPoint point = (PixelPoint)o;
			return Equals(this, point);
		}

		public bool Equals(PixelPoint value)
		{
			return Equals(this, value);
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode();
		}
	}
}
