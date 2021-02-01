using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
	public struct PixelPointEx
	{
		private readonly int _type;

		private readonly PixelPoint _pixelPoint;

		public int Type => _type;

		public PixelPoint PixelPoint => _pixelPoint;

		public PixelPointEx(int type, PixelPoint pixelPoint)
		{
			_type = type;
			_pixelPoint = pixelPoint;
		}

		public static bool operator ==(PixelPointEx point1, PixelPointEx point2)
		{
			if (point1.PixelPoint == point2.PixelPoint)
			{
				return point1.Type == point2.Type;
			}
			return false;
		}

		public static bool operator !=(PixelPointEx point1, PixelPointEx point2)
		{
			return !(point1 == point2);
		}

		public static bool Equals(PixelPointEx point1, PixelPointEx point2)
		{
			if (point1.PixelPoint.Equals(point2.PixelPoint))
			{
				return point1.Type.Equals(point2.Type);
			}
			return false;
		}

		public override bool Equals(object o)
		{
			if (o == null || !(o is PixelPointEx))
			{
				return false;
			}
			PixelPointEx point = (PixelPointEx)o;
			return Equals(this, point);
		}

		public bool Equals(PixelPointEx value)
		{
			return Equals(this, value);
		}

		public override int GetHashCode()
		{
			return PixelPoint.GetHashCode() ^ Type.GetHashCode();
		}
	}
}
