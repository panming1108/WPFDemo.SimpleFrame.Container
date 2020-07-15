using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.ECGTools
{
    [TemplatePart(Name = "PART_ScrollViewer", Type = typeof(ScrollViewer))]
    public class BeatItemListBox : ListBox
    {
        private ScrollViewer PART_ScrollViewer;
        public bool IsWide
        {
            get { return (bool)GetValue(IsWideProperty); }
            set { SetValue(IsWideProperty, value); }
        }

        public static readonly DependencyProperty IsWideProperty =
            DependencyProperty.Register(nameof(IsWide), typeof(bool), typeof(BeatItemListBox));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_ScrollViewer = GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
            if(PART_ScrollViewer != null)
            {
                PART_ScrollViewer.ScrollChanged += PART_ScrollViewer_ScrollChanged;
            }
        }

        private void PART_ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new BeatItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is BeatItem;
        }

        internal void ReverseSelect()
        {
            foreach (var item in Items)
            {
                var beatItem = ItemContainerGenerator.ContainerFromItem(item) as BeatItem;
                SetIsSelected(beatItem, !GetIsSelected(beatItem));
            }
        }
    }
}
