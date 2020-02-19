using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs.ListBox
{
    public class EMCFlushListBox : System.Windows.Controls.ListBox
    {
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is EMCFlushListBoxItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new EMCFlushListBoxItem();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var listItem = element as EMCFlushListBoxItem;
            if(listItem != null)
            {
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.Duration = TimeSpan.FromMilliseconds(500);
                doubleAnimation.From = 1;
                doubleAnimation.To = 0;
                doubleAnimation.RepeatBehavior = new RepeatBehavior(TimeSpan.FromSeconds(10));
                doubleAnimation.AutoReverse = true;
                listItem.BeginAnimation(OpacityProperty, doubleAnimation);
            }
        }
    }
}
