using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DVs.WaveChart
{
    public class EMCWaveChart : Control
    {
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

        public EMCWaveChart()
        {
            Loaded += EMCWaveChart_Loaded;
            Unloaded += EMCWaveChart_Unloaded;
        }

        private void EMCWaveChart_Unloaded(object sender, RoutedEventArgs e)
        {
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
            }
        }
    }
}
