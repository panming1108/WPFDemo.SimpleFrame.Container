using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public class BeatMarkHelper
    {
        public static double GetCurrentBeat(double currentX)
        {
            double resultBeat;
            var beats = BeatInfoCache.GetBeats().Where(x => currentX >= x.Position - 15 && currentX <= x.Position + 15).ToList();
            if (beats.Count() <= 0)
            {
                resultBeat = 0;
            }
            else if (beats.Count() == 1)
            {
                resultBeat = beats.First().Position;
            }
            else
            {
                double minDistance = beats[0].Position - currentX;
                double beat = beats[0].Position;
                foreach (var item in beats)
                {
                    var tempMinDistance = Math.Abs(item.Position - currentX);
                    if (tempMinDistance < minDistance)
                    {
                        beat = item.Position;
                        minDistance = tempMinDistance;
                    }
                }
                resultBeat = beat;
            }
            return resultBeat;
        }

        public static double GetNearBeat(double currentX)
        {
            double resultBeat = BeatInfoCache.GetBeats()[0].Position;
            double minDistance = Math.Abs(BeatInfoCache.GetBeats()[0].Position - currentX);
            foreach (var item in BeatInfoCache.GetBeats())
            {
                var tempDistance = Math.Abs(item.Position - currentX);
                if (tempDistance < minDistance)
                {
                    minDistance = tempDistance;
                    resultBeat = item.Position;
                }
            }
            return resultBeat;           
        }

        public static Tuple<double, double> GetAfArea(double currentX)
        {
            var startAF = 50 + 7 * 100;
            var endAF = 50 + 11 * 100;
            if (currentX >= startAF && currentX <= endAF)
            {
                return new Tuple<double, double>(50 + 7 * 100, 50 + 11 * 100);
            }
            else
            {
                return new Tuple<double, double>(0, 0);
            }
        }
    }
}
