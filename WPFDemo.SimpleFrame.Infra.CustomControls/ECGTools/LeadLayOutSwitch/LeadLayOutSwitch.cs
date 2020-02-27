using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.ECGTools
{
    [TemplatePart(Name = PART_LAYOUTSWITCH, Type = typeof(ListBox))]
    [TemplatePart(Name = PART_GROUPLEADSWITCH, Type = typeof(GroupLeadSwitch))]
    public class LeadLayOutSwitch : Control
    {
        #region Fields
        private const string PART_LAYOUTSWITCH = "PART_LayOutSwitch";
        private const string PART_GROUPLEADSWITCH = "PART_GroupLeadSwitch";
        private ListBox _layOutListBox;
        private GroupLeadSwitch _groupLeadListBox;
        private bool _isLoaded = false;
        #endregion

        #region DependencyProperty
        public SwitchTypeEnum SwitchType
        {
            get { return (SwitchTypeEnum)GetValue(SwitchTypeProperty); }
            set { SetValue(SwitchTypeProperty, value); }
        }
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        public IList SelectedItems
        {
            get { return (IList)GetValue(SelectedItemsProperty); }
            private set { SetValue(SelectedItemsProperty, value); }
        }
        public int MaxSelectCount
        {
            get { return (int)GetValue(MaxSelectCountProperty); }
            set { SetValue(MaxSelectCountProperty, value); }
        }   
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        public UIElement PlacementTarget
        {
            get { return (UIElement)GetValue(PlacementTargetProperty); }
            set { SetValue(PlacementTargetProperty, value); }
        }
        public bool StaysOpen
        {
            get { return (bool)GetValue(StaysOpenProperty); }
            set { SetValue(StaysOpenProperty, value); }
        }
        public PlacementMode Placement
        {
            get { return (PlacementMode)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }
        public double VerticalOffset
        {
            get { return (double)GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }
        public double HorizontalOffset
        {
            get { return (double)GetValue(HorizontalOffsetProperty); }
            set { SetValue(HorizontalOffsetProperty, value); }
        }
        public Style ListBoxStyle
        {
            get { return (Style)GetValue(ListBoxStyleProperty); }
            set { SetValue(ListBoxStyleProperty, value); }
        }
        public Style ListBoxItemStyle
        {
            get { return (Style)GetValue(ListBoxItemStyleProperty); }
            set { SetValue(ListBoxItemStyleProperty, value); }
        }
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        public Dictionary<string, IEnumerable<string>> GroupSource
        {
            get { return (Dictionary<string, IEnumerable<string>>)GetValue(GroupSourceProperty); }
            set { SetValue(GroupSourceProperty, value); }
        }

        public static readonly DependencyProperty GroupSourceProperty =
            DependencyProperty.Register("GroupSource", typeof(Dictionary<string, IEnumerable<string>>), typeof(LeadLayOutSwitch));
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(LeadLayOutSwitch));
        public static readonly DependencyProperty ListBoxItemStyleProperty =
            DependencyProperty.Register("ListBoxItemStyle", typeof(Style), typeof(LeadLayOutSwitch));
        public static readonly DependencyProperty ListBoxStyleProperty =
            DependencyProperty.Register("ListBoxStyle", typeof(Style), typeof(LeadLayOutSwitch));
        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(LeadLayOutSwitch));
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.Register("VerticalOffset", typeof(double), typeof(LeadLayOutSwitch));
        public static readonly DependencyProperty PlacementProperty =
            DependencyProperty.Register("Placement", typeof(PlacementMode), typeof(LeadLayOutSwitch));
        public static readonly DependencyProperty StaysOpenProperty =
            DependencyProperty.Register("StaysOpen", typeof(bool), typeof(LeadLayOutSwitch));
        public static readonly DependencyProperty PlacementTargetProperty =
            DependencyProperty.Register("PlacementTarget", typeof(UIElement), typeof(LeadLayOutSwitch));
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(LeadLayOutSwitch));
        public static readonly DependencyProperty MaxSelectCountProperty =
            DependencyProperty.Register("MaxSelectCount", typeof(int), typeof(LeadLayOutSwitch));
        public static readonly DependencyProperty SwitchTypeProperty =
            DependencyProperty.Register("SwitchType", typeof(SwitchTypeEnum), typeof(LeadLayOutSwitch));
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(LeadLayOutSwitch));
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(LeadLayOutSwitch));
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(IList), typeof(LeadLayOutSwitch), new PropertyMetadata(OnSelectedItemsChanged));
        #endregion

        public LeadLayOutSwitch()
        {
            Loaded += LeadLayOutSwitch_Loaded;
            Unloaded += LeadLayOutSwitch_Unloaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitLayOutListBox();
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null)
            {
                return;
            }
            LeadLayOutSwitch control = d as LeadLayOutSwitch;
            ListBox listBox;
            if(control.SwitchType == SwitchTypeEnum.GroupLead)
            {
                listBox = control._groupLeadListBox;
            }
            else
            {
                listBox = control._layOutListBox;                
            }
            if (listBox == null)
            {
                return;
            }
            foreach (var item in e.NewValue as IList)
            {
                listBox.SelectedItems.Add(item);
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(sender == null)
            {
                return;
            }
            if(!_isLoaded)
            {
                return;
            }
            ListBox listBox = sender as ListBox;
            switch (SwitchType)
            {
                case SwitchTypeEnum.LayOut:
                    IsOpen = false;
                    OnSelectionChanged(new LeadSwitchSelectionChangedEventArgs(e.AddedItems));
                    break;
                case SwitchTypeEnum.Lead:
                    SelectedItems = listBox.SelectedItems;
                    if(MaxSelectCount != 0 && listBox.SelectedItems != null && listBox.SelectedItems.Count > MaxSelectCount)
                    {
                        var listItem = listBox.ItemContainerGenerator.ContainerFromItem(listBox.SelectedItems[0]) as ListBoxItem;
                        listItem.IsSelected = false;
                    }
                    else
                    {
                        OnSelectionChanged(new LeadSwitchSelectionChangedEventArgs(SelectedItems));
                    }
                    break;
                case SwitchTypeEnum.GroupLead:
                    break;
                default:
                    break;
            }
        }

        private void LeadLayOutSwitch_Unloaded(object sender, RoutedEventArgs e)
        {
            UnLoadControls();
            Unloaded -= LeadLayOutSwitch_Unloaded;
        }

        private void InitLayOutListBox()
        {
            if(SwitchType == SwitchTypeEnum.GroupLead)
            {
                _groupLeadListBox = GetTemplateChild(PART_GROUPLEADSWITCH) as GroupLeadSwitch;
                if (_groupLeadListBox != null)
                {
                    SelectedItems = _groupLeadListBox.SelectedItems;
                    _groupLeadListBox.SelectedItemsChanged += GroupLeadListBox_SelectedItemsChanged;
                }
            }
            else
            {
                _layOutListBox = GetTemplateChild(PART_LAYOUTSWITCH) as ListBox;
                if (_layOutListBox != null)
                {
                    SelectedItems = _layOutListBox.SelectedItems;
                    _layOutListBox.SelectionChanged += ListBox_SelectionChanged;
                }            
            }
        }

        private void GroupLeadListBox_SelectedItemsChanged(object sender, LeadSwitchSelectionChangedEventArgs e)
        {
            OnSelectionChanged(e);
        }

        private void UnLoadControls()
        {
            if (_layOutListBox != null)
            {
                _layOutListBox.SelectionChanged -= ListBox_SelectionChanged;
            }
            if (_groupLeadListBox != null)
            {
                _groupLeadListBox.SelectedItemsChanged -= GroupLeadListBox_SelectedItemsChanged;
                _groupLeadListBox.GroupLeadSwitch_Unloaded();
            }
        }

        public event EventHandler<LeadSwitchSelectionChangedEventArgs> SelectionChanged;

        private void OnSelectionChanged(LeadSwitchSelectionChangedEventArgs args)
        {
            SelectionChanged?.Invoke(this, args);
            OnSelectionChanged(args.SelectedItems);
        }

        protected virtual void OnSelectionChanged(IList selectedItems)
        {

        }

        private void LeadLayOutSwitch_Loaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true;
        }
    }
}
