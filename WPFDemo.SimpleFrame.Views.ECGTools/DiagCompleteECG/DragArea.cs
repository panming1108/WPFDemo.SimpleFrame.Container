using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public class DragArea
    {
        /// <summary>
        /// 画线
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="stroke"></param>
        /// <param name="strokeThickness"></param>
        /// <returns></returns>
        public Line RenderLine(Point startPoint, Point endPoint, Brush stroke, double strokeThickness)
        {
            Line line = new Line()
            {
                X1 = startPoint.X,
                Y1 = startPoint.Y,
                X2 = endPoint.X,
                Y2 = endPoint.Y,
                Stroke = stroke,
                StrokeThickness = strokeThickness,
            };
            return line;
        }

        /// <summary>
        /// 画拖拽区域矩形
        /// </summary>
        /// <param name="leftTopPoint"></param>
        /// <param name="rightBottomPoint"></param>
        /// <param name="stroke"></param>
        /// <param name="fill"></param>
        /// <param name="strokeThickness"></param>
        /// <returns></returns>
        public Rectangle RenderDragAreaRect(Point leftTopPoint, Point rightBottomPoint, Brush fill)
        {
            return RenderRect(leftTopPoint, rightBottomPoint, null, fill, 0, new ContextMenu());
        }

        /// <summary>
        /// 画普通矩形
        /// </summary>
        /// <param name="leftTopPoint"></param>
        /// <param name="rightBottomPoint"></param>
        /// <param name="stroke"></param>
        /// <param name="fill"></param>
        /// <param name="strokeThickness"></param>
        /// <param name="contextMenu"></param>
        /// <returns></returns>
        public Rectangle RenderRect(Point leftTopPoint, Point rightBottomPoint, Brush stroke, Brush fill, double strokeThickness, ContextMenu contextMenu)
        {
            var height = Math.Abs(leftTopPoint.Y - rightBottomPoint.Y);
            Rectangle rectangle = new Rectangle()
            {
                Width = Math.Abs(leftTopPoint.X - rightBottomPoint.X),
                Height = height,
                Fill = fill,
                Stroke = stroke,
                StrokeThickness = strokeThickness,
                ContextMenu = contextMenu,
            };
            Canvas.SetLeft(rectangle, leftTopPoint.X);
            Canvas.SetTop(rectangle, 0);
            return rectangle;
        }
    }
}
