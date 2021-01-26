using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    /// <summary>
    /// BeatTemplateGroupItemView.xaml 的交互逻辑
    /// </summary>
    public partial class BeatTemplateGroupItemView : UserControl
    {
        public UIElementCollection Items => PART_GroupItemWrapPanel.Children;
        private BeatTemplateGroupView _groupView;
        public BeatTemplateGroupView GroupView => _groupView;
        public BeatTemplateGroupItemView(BeatTemplateGroupView groupView)
        {
            _groupView = groupView;
            InitializeComponent();
        }

        public void SetGroupItemItemsSource(IList groupItemItemsSource)
        {
            foreach (var item in groupItemItemsSource)
            {
                var data = item as BeatTemplate;
                BeatTemplateItemView itemView = new BeatTemplateItemView(this)
                {
                    DataContext = data
                };
                Items.Add(itemView);
            }
        }

        public bool IsBeatTemplateItemView(Point currentPoint, out BeatTemplateItemView beatTemplateItemView)
        {
            beatTemplateItemView = null;
            for (int i = 0; i < Items.Count; i++)
            {
                var rect = GetItemBound(Items[i]);
                if(rect.Contains(currentPoint))
                {
                    beatTemplateItemView = Items[i] as BeatTemplateItemView;
                    return true;
                }
            }
            return false;
        }

        public bool IsBeatTemplateGroupItemHeader(Point currentPoint, out BeatTemplateGroupItemView beatTemplateGroupItemView)
        {
            beatTemplateGroupItemView = null;
            var rect = GetItemBound(PART_AddMask);
            if (rect.Contains(currentPoint))
            {
                beatTemplateGroupItemView = this;
                return true;
            }
            return false;
        }

        private Rect GetItemBound(object item)
        {
            if (!(item is FrameworkElement itemView))
            {
                return Rect.Empty;
            }
            else
            {
                var topLeft = itemView.TranslatePoint(new Point(0, 0), GroupView);
                return new Rect(topLeft.X, topLeft.Y, itemView.ActualWidth, itemView.ActualHeight);
            }
        }
    }
}
