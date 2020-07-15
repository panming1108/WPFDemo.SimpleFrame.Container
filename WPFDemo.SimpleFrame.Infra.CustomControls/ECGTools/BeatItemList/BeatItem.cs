using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.ECGTools
{
    public class BeatItem : ListBoxItem
    {
        private string[] arrays = new string[26] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        public string BeatTypeName
        {
            get { return (string)GetValue(BeatTypeNameProperty); }
            set { SetValue(BeatTypeNameProperty, value); }
        }
        public double Percent
        {
            get { return (double)GetValue(PercentProperty); }
            set { SetValue(PercentProperty, value); }
        }
        public int R
        {
            get { return (int)GetValue(RProperty); }
            set { SetValue(RProperty, value); }
        }
        public Point[] WaveData
        {
            get { return (Point[])GetValue(WaveDataProperty); }
            set { SetValue(WaveDataProperty, value); }
        }

        public static readonly DependencyProperty WaveDataProperty =
            DependencyProperty.Register(nameof(WaveData), typeof(Point[]), typeof(BeatItem));
        public static readonly DependencyProperty RProperty =
            DependencyProperty.Register(nameof(R), typeof(int), typeof(BeatItem), new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));
        public static readonly DependencyProperty BeatTypeNameProperty =
            DependencyProperty.Register(nameof(BeatTypeName), typeof(string), typeof(BeatItem));
        public static readonly DependencyProperty PercentProperty =
            DependencyProperty.Register(nameof(Percent), typeof(double), typeof(BeatItem));
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var data = GeneratePointArrays(R, (int)ActualWidth);
            BeatTypeName = arrays[R % 26];
            Percent = (data[0].X + data[0].Y) % 100;
            WaveData = data;
        }

        private Point[] GeneratePointArrays(int seed, int count)
        {
            var result = new Point[count];
            Random random = new Random(seed);
            for (int i = 0; i < count; i++)
            {
                Point point = new Point()
                {
                    X = i,
                    Y = random.Next((int)ActualHeight / 4, (int)ActualHeight * 3 / 4)
                };
                result[i] = point;
            }
            return result;
        }
    }
}
