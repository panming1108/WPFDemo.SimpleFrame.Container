using System;
using System.Collections.Generic;
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
    /// BeatTemplateGroupView.xaml 的交互逻辑
    /// </summary>
    public partial class BeatTemplateGroupView : UserControl
    {
        public BeatTemplateGroupView()
        {
            InitializeComponent();
            for (int i = 0; i < 5; i++)
            {
                BeatTemplateGroupItemView groupItemView = new BeatTemplateGroupItemView();
                var source = new List<BeatTemplate>();
                for (int j = 0; j < 5; j++)
                {
                    BeatTemplate beatTemplate = new BeatTemplate() { TypeName = ((BeatTypeEnum)(j % 3)).ToString() };
                    source.Add(beatTemplate);
                }
                groupItemView.SetGroupItemItemsSource(source);
                PART_GroupItemsControl.Items.Add(groupItemView);
            }
        }
    }
}
