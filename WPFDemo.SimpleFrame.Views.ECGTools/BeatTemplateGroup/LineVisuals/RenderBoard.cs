using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
	internal sealed class RenderBoard : Image
	{
		public RenderBoard()
			: this(BitmapScalingMode.NearestNeighbor, CachingHint.Cache, EdgeMode.Aliased, ClearTypeHint.Auto, useLayoutRounding: false, snapsToDevicePixels: false)
		{
		}

		public RenderBoard(BitmapScalingMode bitmapScalingMode, CachingHint cachingHint, EdgeMode edgeMode, ClearTypeHint clearTypeHint, bool useLayoutRounding, bool snapsToDevicePixels)
		{
			RenderOptions.SetBitmapScalingMode(this, bitmapScalingMode);
			RenderOptions.SetEdgeMode(this, edgeMode);
			RenderOptions.SetClearTypeHint(this, clearTypeHint);
			base.UseLayoutRounding = useLayoutRounding;
			base.SnapsToDevicePixels = snapsToDevicePixels;
			if (cachingHint == CachingHint.Cache)
			{
				RenderOptions.SetCachingHint(this, CachingHint.Cache);
				RenderOptions.SetCacheInvalidationThresholdMinimum(this, 0.5);
				RenderOptions.SetCacheInvalidationThresholdMaximum(this, 2.0);
			}
			base.HorizontalAlignment = HorizontalAlignment.Left;
			base.VerticalAlignment = VerticalAlignment.Top;
		}

		public void Init(ImageSource imageSource)
		{
			base.Source = imageSource;
			base.Stretch = Stretch.None;
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			if (DisplayHelper.GetDpiFactor() == 1.5f)
			{
				DisplayHelper.SetTargetSoftRender(this);
			}
			base.OnRenderSizeChanged(sizeInfo);
		}
	}
}
