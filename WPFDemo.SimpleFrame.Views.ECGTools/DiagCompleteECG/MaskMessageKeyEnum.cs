﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public class MaskMessageKeyEnum
    {
        public const string StartDragArea = nameof(StartDragArea);
        public const string DragAreaMouseUp = nameof(DragAreaMouseUp);
        public const string RenderAFMask = nameof(RenderAFMask);
        public const string DragAFMaskOver = nameof(DragAFMaskOver);
        public const string ClearFlag = nameof(ClearFlag);
        public const string SetStartFlag = nameof(SetStartFlag);
        public const string SetEndFlag = nameof(SetEndFlag);
        public const string ChangedMaskPosition = nameof(ChangedMaskPosition);
    }
}
