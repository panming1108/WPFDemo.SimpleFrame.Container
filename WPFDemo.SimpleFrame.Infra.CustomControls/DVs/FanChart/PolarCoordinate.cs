using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DVs.FanChart
{
    /// <summary>
    /// 极坐标
    /// </summary>
    public class PolarCoordinate
    {
        /// <summary>
        /// 半径
        /// </summary>
        public double R { get; set; }

        /// <summary>
        /// 角度(弧度制)
        /// </summary>
        public double Angle { get; set; }

        /// <summary>
        /// 原点
        /// </summary>
        public Point Origin { get; private set; }

        public const double ANGLE90 = 1 * 2 * Math.PI / 4;
        public const double ANGLE180 = 2 * 2 * Math.PI / 4;
        public const double ANGLE270 = 3 * 2 * Math.PI / 4;
        public const double ANGLE360 = 4 * 2 * Math.PI / 4;

        public PolarCoordinate(double r, double angle, Point origin)
        {
            R = r;
            Angle = angle;
            Origin = origin;
        }

        public PolarCoordinate Convert2PolarCoordinateFromXYPoint(Point point)
        {
            double x = point.X - Origin.X;
            double y = point.Y - Origin.Y;
            double r = Math.Sqrt(x * x + y * y);
            double angle = Math.Atan(Math.Abs(y) / Math.Abs(x));
            switch (GetQuadrantByXYCoordinate(x, y))
            {
                case QuadrantEnum.First:
                    break;
                case QuadrantEnum.PlusY:
                    angle = ANGLE90;
                    break;
                case QuadrantEnum.Second:
                    angle = ANGLE180 - angle;
                    break;
                case QuadrantEnum.MinusX:
                    angle = ANGLE180;
                    break;
                case QuadrantEnum.Third:
                    angle = ANGLE180 + angle;
                    break;
                case QuadrantEnum.MinusY:
                    angle = ANGLE270;
                    break;
                case QuadrantEnum.Forth:
                    angle = ANGLE360 - angle;
                    break;
                case QuadrantEnum.PlusX:
                    angle = 0;
                    break;
                case QuadrantEnum.Origin:
                    angle = 0;
                    break;
                default:
                    throw new Exception("Error Angle");
            }
            return new PolarCoordinate(r, angle, Origin);
        }

        public Point Convert2XYPoint()
        {
            return new Point(Origin.X + R * Math.Cos(Angle), Origin.Y + R * Math.Sin(Angle));
        }

        public QuadrantEnum GetQuadrantByXYCoordinate(double v1, double v2)
        {
            if (v1 > 0 && v2 > 0)
            {
                return QuadrantEnum.First;
            }
            if (v1 > 0 && v2 < 0)
            {
                return QuadrantEnum.Forth;
            }
            if (v1 > 0 && v2 == 0)
            {
                return QuadrantEnum.PlusX;
            }
            if (v1 < 0 && v2 > 0)
            {
                return QuadrantEnum.Second;
            }
            if (v1 < 0 && v2 < 0)
            {
                return QuadrantEnum.Third;
            }
            if (v1 < 0 && v2 == 0)
            {
                return QuadrantEnum.MinusX;
            }
            if (v1 == 0 && v2 > 0)
            {
                return QuadrantEnum.PlusY;
            }
            if (v1 == 0 && v2 < 0)
            {
                return QuadrantEnum.MinusY;
            }
            if (v1 == 0 && v2 == 0)
            {
                return QuadrantEnum.Origin;
            }
            throw new Exception("Quadrant Error");
        }
    }
}
