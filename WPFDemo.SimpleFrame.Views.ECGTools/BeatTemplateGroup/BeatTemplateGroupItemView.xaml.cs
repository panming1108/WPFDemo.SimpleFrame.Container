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
        public BeatTemplateGroupItemView()
        {
            InitializeComponent();
        }

        public void SetGroupItemItemsSource(IList groupItemItemsSource)
        {
            foreach (var item in groupItemItemsSource)
            {
                var data = item as BeatTemplate;
                BeatTemplateItemView itemView = new BeatTemplateItemView
                {
                    DataContext = data
                };
                Items.Add(itemView);
            }
        }
    }
}
