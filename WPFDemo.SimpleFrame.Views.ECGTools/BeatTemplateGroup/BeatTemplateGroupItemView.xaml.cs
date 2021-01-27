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
using WPFDemo.SimpleFrame.Infra.Helper;
using WPFDemo.SimpleFrame.Infra.Messager;
using WPFDemo.SimpleFrame.Infra.Models;

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
        public bool IsSType { get; set; }//是否房性

        public string CategoryNameEn
        {
            get { return (string)GetValue(CategoryNameEnProperty); }
            set { SetValue(CategoryNameEnProperty, value); }
        }
        public string CategoryName
        {
            get { return (string)GetValue(CategoryNameProperty); }
            set { SetValue(CategoryNameProperty, value); }
        }
        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }
        public double Percent
        {
            get { return (double)GetValue(PercentProperty); }
            set { SetValue(PercentProperty, value); }
        }

        public static readonly DependencyProperty PercentProperty =
            DependencyProperty.Register(nameof(Percent), typeof(double), typeof(BeatTemplateGroupItemView));
        public static readonly DependencyProperty CountProperty =
            DependencyProperty.Register(nameof(Count), typeof(int), typeof(BeatTemplateGroupItemView));
        public static readonly DependencyProperty CategoryNameProperty =
            DependencyProperty.Register(nameof(CategoryName), typeof(string), typeof(BeatTemplateGroupItemView));
        public static readonly DependencyProperty CategoryNameEnProperty =
            DependencyProperty.Register(nameof(CategoryNameEn), typeof(string), typeof(BeatTemplateGroupItemView), new PropertyMetadata(OnCategoryNameEnChanged));

        private static void OnCategoryNameEnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var groupItem = d as BeatTemplateGroupItemView;
            var type = (BeatTypeEnum)Enum.Parse(typeof(BeatTypeEnum), groupItem.CategoryNameEn);
            groupItem.CategoryName = EnumHelper.GetDescription(type);
            if (type == BeatTypeEnum.S)
            {
                groupItem.IsSType = true;
                if (groupItem.GroupView.IsAtrialPattern)
                {
                    groupItem.PART_FormRadioBtn.IsChecked = true;
                }
                else
                {
                    groupItem.PART_EventRadioBtn.IsChecked = true;
                }
                groupItem.PART_FormRadioBtn.Visibility = Visibility.Visible;
                groupItem.PART_EventRadioBtn.Visibility = Visibility.Visible;
            }
            else
            {
                groupItem.PART_FormRadioBtn.Visibility = Visibility.Collapsed;
                groupItem.PART_EventRadioBtn.Visibility = Visibility.Collapsed;
            }
        }

        public BeatTemplateGroupItemView(BeatTemplateGroupView groupView)
        {
            _groupView = groupView;
            IsSType = false;
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

        private void PART_FormEventRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            if(Items.Count <= 0 || !IsSType)
            {
                return;
            }
            var btn = sender as RadioButton;
            MessagerInstance.GetMessager().Send("PopupNotifyBox", new PopupNotifyObject("通知", "切换成" + btn.Name));
        }

        private void PART_SelectAllBtn_Click(object sender, RoutedEventArgs e)
        {
            GroupView.SelectedItemsCollection.TryClearItems();
            foreach (var item in Items)
            {
                var itemView = item as BeatTemplateItemView;
                itemView.IsSelected = true;
            }
            GroupView.OnGroupItemsSelectAll();
        }
    }
}
