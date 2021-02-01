using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
	public class DisplayHelper
	{
		public static float GetFixedDpi(float ScaleDpi)
		{
			return GetDpiFactor() * ScaleDpi;
		}

		public static float GetDpiFactor()
		{
			using (Graphics graphics = Graphics.FromImage(new Bitmap(1, 1)))
			{
				return graphics.DpiX / 96f;
			}
		}

		public static float GetDpiFactor(Visual visual)
		{
			return (float)PresentationSource.FromVisual(visual).CompositionTarget.TransformToDevice.M11;
		}

		public static void SetTargetSoftRender(Visual visual)
		{
			(PresentationSource.FromVisual(visual) as HwndSource).CompositionTarget.RenderMode = RenderMode.SoftwareOnly;
		}

		public static void SetProcessSoftRender()
		{
			RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
		}
	}
}
