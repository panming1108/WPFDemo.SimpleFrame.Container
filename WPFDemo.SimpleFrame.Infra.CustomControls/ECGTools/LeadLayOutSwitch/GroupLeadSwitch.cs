using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.ECGTools
{
    public class GroupLeadSwitch : ListBox
    {
        private const string PART_GROUPLISTBOX = "PART_GroupListBox";
        private const string PART_SELECTALLBTN = "PART_SelectAllBtn";

        private bool _isLoaded = false;
        private bool _isStartGroupSelected = false;

        public event EventHandler<LeadSwitchSelectionChangedEventArgs> SelectedItemsChanged;

        private ListBox _groupListBox;
        private Button _selectAllBtn;

        public Dictionary<string, IEnumerable<string>> GroupSource
        {
            get { return (Dictionary<string, IEnumerable<string>>)GetValue(GroupSourceProperty); }
            set { SetValue(GroupSourceProperty, value); }
        }

        public static readonly DependencyProperty GroupSourceProperty =
            DependencyProperty.Register("GroupSource", typeof(Dictionary<string, IEnumerable<string>>), typeof(GroupLeadSwitch), new PropertyMetadata(OnGroupSourceChanged));

        public GroupLeadSwitch()
        {
            Loaded += GroupLeadSwitch_Loaded;
        }

        public void GroupLeadSwitch_Loaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true;
        }

        public void GroupLeadSwitch_Unloaded()
        {
            if (_groupListBox != null)
            {
                _groupListBox.SelectionChanged -= GroupListBox_SelectionChanged;
            }
            if (_selectAllBtn != null)
            {
                _selectAllBtn.Click -= SelectAllBtn_Click;
            }
            Loaded -= GroupLeadSwitch_Loaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (_groupListBox != null)
            {
                _groupListBox.SelectionChanged -= GroupListBox_SelectionChanged;
            }
            if (_selectAllBtn != null)
            {
                _selectAllBtn.Click -= SelectAllBtn_Click;
            }
            _groupListBox = GetTemplateChild(PART_GROUPLISTBOX) as ListBox;
            _selectAllBtn = GetTemplateChild(PART_SELECTALLBTN) as Button;
            if(_groupListBox != null)
            {
                _groupListBox.SelectionChanged += GroupListBox_SelectionChanged;
            }
            if(_selectAllBtn != null)
            {
                _selectAllBtn.Click += SelectAllBtn_Click;
            }
        }

        private void SelectAllBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItems.Count != Items.Count)
            {
                SelectAll();
            }
            else
            {
                _isStartGroupSelected = true;
                UnselectAll();                
                var groupLeadSwitchItem = ItemContainerGenerator.ContainerFromItem(Items[0]) as ListBoxItem;
                _isStartGroupSelected = false;
                groupLeadSwitchItem.IsSelected = true;
            }
            _groupListBox.UnselectAll();
        }

        private void GroupListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            var groupItemValue = listBox.SelectedValue;
            if(groupItemValue == null)
            {
                return;
            }
            _isStartGroupSelected = true;
            var list = new List<ListBoxItem>();
            foreach (var item in ItemsSource)
            {
                var groupLeadSwitchItem = ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
                bool oldResult = groupLeadSwitchItem.IsSelected;
                var values = GroupSource[groupItemValue.ToString()];
                bool newResult = values.Contains(groupLeadSwitchItem.Content.ToString());
                if(oldResult != newResult)
                {
                    list.Add(groupLeadSwitchItem);
                }
            }
            foreach (var item in list)
            {
                if(item == list.Last())
                {
                    _isStartGroupSelected = false;
                }
                item.IsSelected = !item.IsSelected;
            }
        }

        private static void OnGroupSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d == null)
            {
                return;
            }
            var groupLead = d as GroupLeadSwitch;
            var source = e.NewValue as Dictionary<string, IEnumerable<string>>;
            var values = new List<string>();
            for (int i = 0; i < source.Count; i++)
            {
                var keyValue = source.ElementAt(i);
                foreach (var item in keyValue.Value)
                {
                    values.Add(item);
                }
            }
            groupLead.ItemsSource = values;
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            if(!_isLoaded)
            {
                return;
            }
            _groupListBox.UnselectAll();
            if(!_isStartGroupSelected)
            {
                OnSelectionChanged(new LeadSwitchSelectionChangedEventArgs(SelectedItems));
            }
        }

        private void OnSelectionChanged(LeadSwitchSelectionChangedEventArgs args)
        {
            SelectedItemsChanged?.Invoke(this, args);
        }
    }
}
