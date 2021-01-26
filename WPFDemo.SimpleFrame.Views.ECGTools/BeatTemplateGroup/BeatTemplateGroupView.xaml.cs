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
using System.Windows.Threading;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
    /// <summary>
    /// BeatTemplateGroupView.xaml 的交互逻辑
    /// </summary>
    public partial class BeatTemplateGroupView : UserControl
    {
        private Random _random = new Random();
        private readonly DispatcherTimer _dispatcherTimer;
        public BeatTemplateGroupView()
        {
            _dispatcherTimer = new DispatcherTimer(DispatcherPriority.Send)
            {
                Interval = TimeSpan.FromMilliseconds(20)
            };
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            _dispatcherTimer.Start();
            InitializeComponent();
            Loaded += BeatTemplateGroupView_Loaded;
            Unloaded += BeatTemplateGroupView_Unloaded;
            MouseLeftButtonDown += BeatTemplateGroupView_MouseLeftButtonDown;
            MouseLeftButtonUp += BeatTemplateGroupView_MouseLeftButtonUp;
            MouseRightButtonUp += BeatTemplateGroupView_MouseRightButtonUp;
            KeyDown += BeatTemplateGroupView_KeyDown;
        }

        private void BeatTemplateGroupView_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateData();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            
        }

        private void BeatTemplateGroupView_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void BeatTemplateGroupView_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void BeatTemplateGroupView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void BeatTemplateGroupView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var currentPoint = Mouse.GetPosition(this);
            BeatTemplateItemView itemView = null;
            foreach (var item in PART_GroupItemsControl.Items)
            {
                var groupItemView = item as BeatTemplateGroupItemView;
                var result = groupItemView.IsBeatTemplateItemView(currentPoint, out itemView);
                if(result)
                {
                    break;
                }
            }
            if(itemView != null)
            {
                //拖动模板
            }
            else
            {
                //框选
            }
        }

        public void GenerateData()
        {
            var groupCount = _random.Next(10, 20);
            var itemCount = _random.Next(10, 20);
            PART_GroupItemsControl.Items.Clear();
            for (int i = 0; i < 15; i++)
            {
                BeatTemplateGroupItemView groupItemView = new BeatTemplateGroupItemView(this);
                var source = new List<BeatTemplate>();
                for (int j = 0; j < 15; j++)
                {
                    BeatTemplate beatTemplate = new BeatTemplate() { TypeName = ((BeatTypeEnum)(j % 3)).ToString() };
                    source.Add(beatTemplate);
                }
                groupItemView.SetGroupItemItemsSource(source);
                PART_GroupItemsControl.Items.Add(groupItemView);
            }
        }

        private void BeatTemplateGroupView_Unloaded(object sender, RoutedEventArgs e)
        {
            _dispatcherTimer.Tick -= DispatcherTimer_Tick;
            Loaded -= BeatTemplateGroupView_Loaded;
            Unloaded -= BeatTemplateGroupView_Unloaded;
            MouseLeftButtonDown -= BeatTemplateGroupView_MouseLeftButtonDown;
            MouseLeftButtonUp -= BeatTemplateGroupView_MouseLeftButtonUp;
            MouseRightButtonUp -= BeatTemplateGroupView_MouseRightButtonUp;
            KeyDown -= BeatTemplateGroupView_KeyDown;
        }
    }
}
