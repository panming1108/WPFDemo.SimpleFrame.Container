using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public abstract class MaskActionBase : IDisposable
    {
        /// <summary>
        /// 工具面板高度
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// 工具面板宽度
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// 工具面板上方偏移
        /// </summary>
        public double LeftOffset { get; set; }

        /// <summary>
        /// 工具面板左方偏移
        /// </summary>
        public double TopOffset { get; set; }

        /// <summary>
        /// 工具面板绘画形状集合
        /// </summary>
        public DrawingCollection DrawingChildren { get; set; } = new DrawingCollection();

        /// <summary>
        /// 工具面板绘画文字集合
        /// </summary>
        public List<MaskText> DrawingTexts { get; set; } = new List<MaskText>();

        /// <summary>
        /// 光标
        /// </summary>
        public Cursor Cursor { get; private set; }

        /// <summary>
        /// 右击菜单
        /// </summary>
        public IEnumerable ContextMenuItems { get; private set; }

        protected BrushConverter _brushConverter = new BrushConverter();

        public MaskActionBase(double leftOffset, double topOffset)
        {
            LeftOffset = leftOffset;
            TopOffset = topOffset;
        }

        /// <summary>
        /// 左键按住拖动
        /// </summary>
        /// <param name="currentPoint"></param>
        public virtual void DrawingDrag(Point currentPoint) { }

        /// <summary>
        /// 左键按住拖动结束，左键抬起
        /// </summary>
        /// <param name="currentPoint"></param>
        public virtual void DrawingDragOver(Point currentPoint) { }

        /// <summary>
        /// 左键抬起
        /// </summary>
        /// <param name="currentPoint"></param>
        public virtual void DrawingMouseUp(Point currentPoint) { }

        /// <summary>
        /// 双击
        /// </summary>
        /// <param name="currentPoint"></param>
        public virtual void DrawingMouseDoubleClick(Point currentPoint) { }

        /// <summary>
        /// 鼠标悬浮
        /// </summary>
        /// <param name="currentPoint"></param>
        public virtual void DrawingMouseOver(Point currentPoint) 
        {
            Cursor = SetMouseOverCursor(currentPoint);
        }

        /// <summary>
        /// 鼠标右击
        /// </summary>
        public virtual void DrawingMouseRightButtonDown(Point currentPoint) 
        {
            ContextMenuItems = SetContextMenuItems(currentPoint);
        }

        /// <summary>
        /// 滚轮
        /// </summary>
        /// <param name="offset"></param>
        public virtual void DrawingMouseWheel(double offset) { }

        /// <summary>
        /// 初始化工具遮罩，Add时触发
        /// </summary>
        public virtual void InitMask() { }

        /// <summary>
        /// 准备工具遮罩，MouseLeftButtonDown时触发
        /// </summary>
        /// <param name="currentPoint"></param>
        public virtual void PrepareMask(Point currentPoint) { }

        /// <summary>
        /// 重置工具遮罩，Remove时触发
        /// </summary>
        public virtual void ResetMask() { }

        /// <summary>
        /// Dispose
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// 工具遮罩宽高改变
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public virtual void RenderMaskSize(double height, double width)
        {
            Height = height;
            Width = width;
        }

        /// <summary>
        /// 鼠标悬浮时获取光标
        /// </summary>
        /// <param name="currentPoint"></param>
        /// <returns></returns>
        protected virtual Cursor SetMouseOverCursor(Point currentPoint)
        {
            return Cursors.Arrow;
        }

        /// <summary>
        /// 获取右击菜单
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable SetContextMenuItems(Point currentPoint)
        {
            return null;
        }
    }
}
