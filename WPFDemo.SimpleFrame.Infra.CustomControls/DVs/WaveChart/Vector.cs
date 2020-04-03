using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DVs.WaveChart
{
    internal class Vector
    {
        internal double X { get; set; }
        internal double Y { get; set; }
        internal Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// 向量长度
        /// </summary>
        /// <returns></returns>
        internal double Length()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        /// <summary>
        /// 该向量对应的单位向量
        /// </summary>
        /// <returns></returns>
        internal Vector Normalize()
        {
            var inv = 1 / Length();
            return new Vector(X * inv, Y * inv);
        }

        /// <summary>
        /// 向量加法
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        internal Vector Add(Vector vector)
        {
            return new Vector(X + vector.X, Y + vector.Y);
        }

        /// <summary>
        /// 向量长度加倍
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        internal Vector Multiply(double f)
        {
            return new Vector(X * f, Y * f);
        }

        /// <summary>
        /// 向量积
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        internal double Dot(Vector vector)
        {
            return X * vector.X + Y * vector.Y;
        }

        /// <summary>
        /// 与另一向量的弧度制弧度制
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        internal double Angle(Vector vector)
        {
            return Math.Acos(Dot(vector) / (Length() * vector.Length())) * 180 / Math.PI;
        }

        /// <summary>
        /// 转化成点
        /// </summary>
        /// <returns></returns>
        internal Point ToPoint()
        {
            return new Point(X, Y);
        }
    }
}
