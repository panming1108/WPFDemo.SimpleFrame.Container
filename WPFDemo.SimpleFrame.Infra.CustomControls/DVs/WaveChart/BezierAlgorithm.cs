using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DVs.WaveChart
{
    /// <summary>
    /// 贝塞尔曲线算法类
    /// </summary>
    public class BezierAlgorithm
    {
        /// <summary>
        /// 贝塞尔曲线阶数
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 比例变化步长
        /// </summary>
        public double RateStep { get; set; }

        private double _rt;

        public BezierAlgorithm()
        {

        }

        public BezierAlgorithm(int level, double rateStep, double rt = 0.3)
        {
            Level = level;
            RateStep = rateStep;
            _rt = rt;
        }

        /// <summary>
        /// 根据三次贝塞尔曲线公式，获取一段Bezier曲线上的所有点
        /// </summary>
        /// <param name="prev">前一个点</param>
        /// <param name="current">当前点</param>
        /// <param name="next">下一个点</param>
        /// <param name="nextNext">下下一个点</param>
        /// <returns></returns>
        public List<Point> GetThirdLevelBezierPoints(Point prev, Point current, Point next, Point nextNext)
        {
            Point control1 = GetControlPoint(_rt, prev, current, next).Item2;
            Point control2 = GetControlPoint(_rt, current, next, nextNext).Item1;

            return GetBezierPoints(new List<Point>() { current, control1, control2, next });
        }

        /// <summary>
        /// 获取贝塞尔曲线上的点
        /// </summary>
        /// <param name="points">按开始点，控制点，结束点的顺序的点的集合</param>
        /// <returns></returns>
        public List<Point> GetBezierPoints(List<Point> points)
        {
            if(points == null)
            {
                throw new Exception("points are null!");
            }
            if(points.Count != Level + 1)
            {
                throw new Exception("points' count dose not match Level!");
            }
            List<Point> results = new List<Point>();

            for (double t = 0; t <= 1; t += RateStep)
            {
                results.Add(CaculatePoint(points, t, Level, 0));
            }

            return results;
        }

        /// <summary>
        /// 计算特定比例下的点
        /// </summary>
        /// <param name="points">点集合</param>
        /// <param name="rate">比例</param>
        /// <param name="k">阶数</param>
        /// <param name="i">下标</param>
        /// <returns></returns>
        private Point CaculatePoint(List<Point> points, double rate, int level, int i)
        {
            if(level == 0)
            {
                return points[i];
            }
            else
            {
                Point point = new Point()
                {
                    X = (1 - rate) * CaculatePoint(points, rate, level - 1, i).X + rate * CaculatePoint(points, rate, level - 1, i + 1).X,
                    Y = (1 - rate) * CaculatePoint(points, rate, level - 1, i).Y + rate * CaculatePoint(points, rate, level - 1, i + 1).Y,
                };
                return point;
            }
        }

        public BezierSegment GetBezierSegment(Point prev, Point current, Point next, Point nextNext, bool isStroked = true)
        {
            var tuple1 = GetControlPoint(_rt, prev, current, next);
            var tuple2 = GetControlPoint(_rt, current, next, nextNext);

            return new BezierSegment(tuple1.Item2, tuple2.Item1, next, isStroked);
        }

        /// <summary>
        /// 获取两个控制点
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="pointPrev"></param>
        /// <param name="pointCur"></param>
        /// <param name="pointNext"></param>
        /// <returns></returns>
        private Tuple<Point, Point> GetControlPoint(double rt, Point pointPrev, Point pointCur, Point pointNext)
        {
            var a = pointPrev;
            var b = pointCur;
            var c = pointNext;
            var v1 = new Vector(a.X - b.X, a.Y - b.Y);
            var v2 = new Vector(c.X - b.X, c.Y - b.Y);
            var v1Len = v1.Length();
            var v2Len = v2.Length();
            var t1 = v1.Normalize();
            var t2 = v2.Normalize();
            var t3 = t1.Add(t2);
            var centerV = t3.Normalize();
            if (double.IsNaN(centerV.X) && double.IsNaN(centerV.Y))
            {
                var p1 = new Point((a.X + b.X) / 2, (a.Y + b.Y) / 2);
                var p2 = new Point((c.X + b.X) / 2, (c.Y + b.Y) / 2);
                return new Tuple<Point, Point>(p1, p2);
            }
            var ncp1 = new Vector(centerV.Y, centerV.X * -1);
            var ncp2 = new Vector(centerV.Y * -1, centerV.X);
            Vector vectorB = new Vector(b.X, b.Y);
            Vector item1, item2;
            if (ncp1.Angle(v1) < 90)
            {
                item1 = ncp1.Multiply(v1Len * rt).Add(vectorB);
                item2 = ncp2.Multiply(v2Len * rt).Add(vectorB);
            }
            else
            {
                item1 = ncp2.Multiply(v1Len * rt).Add(vectorB);
                item2 = ncp1.Multiply(v2Len * rt).Add(vectorB);
            }

            Point item1Center = new Point((pointPrev.X + pointCur.X) / 2, (pointPrev.Y + pointCur.Y) / 2);
            Point item2Center = new Point((pointCur.X + pointNext.X) / 2, (pointCur.Y + pointNext.Y) / 2);

            if (item1.X > pointCur.X)
            {
                item1.X = pointCur.X;
            }
            if (item2.X < pointCur.X)
            {
                item2.X = pointCur.X;
            }

            if (item1.X < item1Center.X)
            {
                item1.X = item1Center.X;
            }
            if (item2.X > item2Center.X)
            {
                item2.X = item2Center.X;
            }

            return new Tuple<Point, Point>(item1.ToPoint(), item2.ToPoint());
        }
    }
}
