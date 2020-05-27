using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DVs.WaveChart
{
    public class EMCWaveChart : Control
    {
        private DispatcherTimer _timer;

        public Dictionary<string, double> ItemsSource
        {
            get { return (Dictionary<string, double>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(Dictionary<string, double>), typeof(EMCWaveChart), new PropertyMetadata(OnSourceChanged));

        public WaveIntervalConverter WaveIntervalConverter
        {
            get { return (WaveIntervalConverter)GetValue(WaveIntervalConverterProperty); }
            set { SetValue(WaveIntervalConverterProperty, value); }
        }

        public static readonly DependencyProperty WaveIntervalConverterProperty =
            DependencyProperty.Register(nameof(WaveIntervalConverter), typeof(WaveIntervalConverter), typeof(EMCWaveChart));

        public int OrdinateCount
        {
            get { return (int)GetValue(OrdinateCountProperty); }
            set { SetValue(OrdinateCountProperty, value); }
        }

        public static readonly DependencyProperty OrdinateCountProperty =
            DependencyProperty.Register(nameof(OrdinateCount), typeof(int), typeof(EMCWaveChart), new PropertyMetadata(5));

        public double AxisOffset
        {
            get { return (double)GetValue(AxisOffsetProperty); }
            set { SetValue(AxisOffsetProperty, value); }
        }

        public static readonly DependencyProperty AxisOffsetProperty =
            DependencyProperty.Register(nameof(AxisOffset), typeof(double), typeof(EMCWaveChart), new PropertyMetadata(20.0));

        public bool IsDrawPoint
        {
            get { return (bool)GetValue(IsDrawPointProperty); }
            set { SetValue(IsDrawPointProperty, value); }
        }

        public static readonly DependencyProperty IsDrawPointProperty =
            DependencyProperty.Register(nameof(IsDrawPoint), typeof(bool), typeof(EMCWaveChart), new PropertyMetadata(true));

        public bool IsDrawOrdinateAxis
        {
            get { return (bool)GetValue(IsDrawOrdinateAxisProperty); }
            set { SetValue(IsDrawOrdinateAxisProperty, value); }
        }

        public static readonly DependencyProperty IsDrawOrdinateAxisProperty =
            DependencyProperty.Register(nameof(IsDrawOrdinateAxis), typeof(bool), typeof(EMCWaveChart), new PropertyMetadata(true));

        public bool IsDrawAbscissaAxis
        {
            get { return (bool)GetValue(IsDrawAbscissaAxisProperty); }
            set { SetValue(IsDrawAbscissaAxisProperty, value); }
        }

        public static readonly DependencyProperty IsDrawAbscissaAxisProperty =
            DependencyProperty.Register(nameof(IsDrawAbscissaAxis), typeof(bool), typeof(EMCWaveChart), new PropertyMetadata(true));

        public LineModeEnum LineMode
        {
            get { return (LineModeEnum)GetValue(LineModeProperty); }
            set { SetValue(LineModeProperty, value); }
        }

        public static readonly DependencyProperty LineModeProperty =
            DependencyProperty.Register(nameof(LineMode), typeof(LineModeEnum), typeof(EMCWaveChart), new PropertyMetadata(LineModeEnum.StraightLine));

        public double AxisStrokeThickness
        {
            get { return (double)GetValue(AxisStrokeThicknessProperty); }
            set { SetValue(AxisStrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty AxisStrokeThicknessProperty =
            DependencyProperty.Register(nameof(AxisStrokeThickness), typeof(double), typeof(EMCWaveChart), new PropertyMetadata(1.0));

        public Brush AxisStrokeBrush
        {
            get { return (Brush)GetValue(AxisStrokeBrushProperty); }
            set { SetValue(AxisStrokeBrushProperty, value); }
        }

        public static readonly DependencyProperty AxisStrokeBrushProperty =
            DependencyProperty.Register(nameof(AxisStrokeBrush), typeof(Brush), typeof(EMCWaveChart));

        public double WaveStrokeThickness
        {
            get { return (double)GetValue(WaveStrokeThicknessProperty); }
            set { SetValue(WaveStrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty WaveStrokeThicknessProperty =
            DependencyProperty.Register(nameof(WaveStrokeThickness), typeof(double), typeof(EMCWaveChart), new PropertyMetadata(1.0));

        public Brush WaveStrokeBrush
        {
            get { return (Brush)GetValue(WaveStrokeBrushProperty); }
            set { SetValue(WaveStrokeBrushProperty, value); }
        }

        public static readonly DependencyProperty WaveStrokeBrushProperty =
            DependencyProperty.Register(nameof(WaveStrokeBrush), typeof(Brush), typeof(EMCWaveChart));

        public double PointRadius
        {
            get { return (double)GetValue(PointRadiusProperty); }
            set { SetValue(PointRadiusProperty, value); }
        }

        public static readonly DependencyProperty PointRadiusProperty =
            DependencyProperty.Register(nameof(PointRadius), typeof(double), typeof(EMCWaveChart), new PropertyMetadata(2.0));

        public Brush PointFillBrush
        {
            get { return (Brush)GetValue(PointFillBrushProperty); }
            set { SetValue(PointFillBrushProperty, value); }
        }

        public static readonly DependencyProperty PointFillBrushProperty =
            DependencyProperty.Register(nameof(PointFillBrush), typeof(Brush), typeof(EMCWaveChart));

        public bool IsWaveFill
        {
            get { return (bool)GetValue(IsWaveFillProperty); }
            set { SetValue(IsWaveFillProperty, value); }
        }

        public static readonly DependencyProperty IsWaveFillProperty =
            DependencyProperty.Register(nameof(IsWaveFill), typeof(bool), typeof(EMCWaveChart), new PropertyMetadata(true));

        public Brush WaveFillBrush
        {
            get { return (Brush)GetValue(WaveFillBrushProperty); }
            set { SetValue(WaveFillBrushProperty, value); }
        }

        public static readonly DependencyProperty WaveFillBrushProperty =
            DependencyProperty.Register(nameof(WaveFillBrush), typeof(Brush), typeof(EMCWaveChart));

        public double RectBorderThickness
        {
            get { return (double)GetValue(RectBorderThicknessProperty); }
            set { SetValue(RectBorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty RectBorderThicknessProperty =
            DependencyProperty.Register(nameof(RectBorderThickness), typeof(double), typeof(EMCWaveChart), new PropertyMetadata(1.0));

        public Brush RectBorderBrush
        {
            get { return (Brush)GetValue(RectBorderBrushProperty); }
            set { SetValue(RectBorderBrushProperty, value); }
        }

        public static readonly DependencyProperty RectBorderBrushProperty =
            DependencyProperty.Register(nameof(RectBorderBrush), typeof(Brush), typeof(EMCWaveChart));

        public Brush RectFillBrush
        {
            get { return (Brush)GetValue(RectFillBrushProperty); }
            set { SetValue(RectFillBrushProperty, value); }
        }

        public static readonly DependencyProperty RectFillBrushProperty =
            DependencyProperty.Register(nameof(RectFillBrush), typeof(Brush), typeof(EMCWaveChart));

        public bool IsDisplayHistogram
        {
            get { return (bool)GetValue(IsDisplayHistogramProperty); }
            set { SetValue(IsDisplayHistogramProperty, value); }
        }

        public static readonly DependencyProperty IsDisplayHistogramProperty =
            DependencyProperty.Register(nameof(IsDisplayHistogram), typeof(bool), typeof(EMCWaveChart));

        public bool IsDisplayLineWave
        {
            get { return (bool)GetValue(IsDisplayLineWaveProperty); }
            set { SetValue(IsDisplayLineWaveProperty, value); }
        }

        public static readonly DependencyProperty IsDisplayLineWaveProperty =
            DependencyProperty.Register(nameof(IsDisplayLineWave), typeof(bool), typeof(EMCWaveChart));

        public bool IsAnimationOpen
        {
            get { return (bool)GetValue(IsAnimationOpenProperty); }
            set { SetValue(IsAnimationOpenProperty, value); }
        }

        public static readonly DependencyProperty IsAnimationOpenProperty =
            DependencyProperty.Register(nameof(IsAnimationOpen), typeof(bool), typeof(EMCWaveChart));

        public Dictionary<string, double> RelayItemsSource
        {
            get { return (Dictionary<string, double>)GetValue(RelayItemsSourceProperty); }
            set { SetValue(RelayItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty RelayItemsSourceProperty =
            DependencyProperty.Register(nameof(RelayItemsSource), typeof(Dictionary<string, double>), typeof(EMCWaveChart));

        public double AnimationSeconds
        {
            get { return (double)GetValue(AnimationSecondsProperty); }
            set { SetValue(AnimationSecondsProperty, value); }
        }

        public static readonly DependencyProperty AnimationSecondsProperty =
            DependencyProperty.Register(nameof(AnimationSeconds), typeof(double), typeof(EMCWaveChart), new PropertyMetadata(0.1));

        public EMCWaveChart()
        {
            Loaded += EMCWaveChart_Loaded;
            Unloaded += EMCWaveChart_Unloaded;
        }

        private void EMCWaveChart_Unloaded(object sender, RoutedEventArgs e)
        {
            DisposeTimer();
            Loaded -= EMCWaveChart_Loaded;
            Unloaded -= EMCWaveChart_Unloaded;
        }

        private void EMCWaveChart_Loaded(object sender, RoutedEventArgs e)
        {
            if(ItemsSource == null)
            {
                return;
            }
            if(ItemsSource.Count == 0)
            {
                return;
            }
            WaveIntervalConverter = new WaveIntervalConverter(ItemsSource, OrdinateCount, ActualWidth, ActualHeight, AxisOffset);
            if (IsAnimationOpen)
            {
                InitTimer();
            }
            else
            {
                RelayItemsSource = ItemsSource;
            }
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d != null)
            {
                EMCWaveChart waveChart = d as EMCWaveChart;
                if (waveChart.WaveIntervalConverter != null)
                {
                    waveChart.WaveIntervalConverter = null;
                }
                var newValue = (Dictionary<string, double>)e.NewValue;
                if (newValue == null)
                {
                    return;
                }
                if (newValue.Count == 0)
                {
                    return;
                }
                waveChart.WaveIntervalConverter = new WaveIntervalConverter(newValue, waveChart.OrdinateCount, waveChart.ActualWidth, waveChart.ActualHeight, waveChart.AxisOffset);
                if (waveChart.IsAnimationOpen)
                {
                    waveChart.InitTimer();
                }
                else
                {
                    waveChart.RelayItemsSource = newValue;
                }
            }
        }

        private void InitTimer()
        {
            DisposeTimer();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(42);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void DisposeTimer()
        {
            if (_timer != null)
            {
                _count = 0;
                _timer.Stop();
                _timer = null;
            }
        }

        private int _count = 0;
        private void Timer_Tick(object sender, EventArgs e)
        {
            RelayItemsSource = GenerateRelayItemsSource(_count, out bool isEnd);
            _count++;
            if(isEnd)
            {
                DisposeTimer();
            }
        }

        private Dictionary<string, double> GenerateRelayItemsSource(int count, out bool isEnd)
        {
            if(count * 24 >= AnimationSeconds * 1000)
            {
                isEnd = true;
                return ItemsSource;
            }
            var result = new Dictionary<string, double>();
            foreach (var item in ItemsSource)
            {
                result.Add(item.Key, item.Value * (count * 24 / (AnimationSeconds * 1000)));
            }
            isEnd = false;
            return result;
        }
    }
}
