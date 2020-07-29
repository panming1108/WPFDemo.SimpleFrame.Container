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
    }
}
