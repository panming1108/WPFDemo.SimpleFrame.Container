using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs
{
    [TemplatePart(Name = "PART_ListBox", Type = typeof(System.Windows.Controls.ListBox))]
    [TemplatePart(Name = "PART_PrevBtn", Type = typeof(Button))]
    [TemplatePart(Name = "PART_NextBtn", Type = typeof(Button))]
    public class SwitchCheckListBox : Control
    {
        private System.Windows.Controls.ListBox PART_ListBox;
        private Button PART_PrevBtn;
        private Button PART_NextBtn;

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public bool CanMovetoNext
        {
            get { return (bool)GetValue(CanMovetoNextProperty); }
            set { SetValue(CanMovetoNextProperty, value); }
        }

        public bool CanMovePrev
        {
            get { return (bool)GetValue(CanMovePrevProperty); }
            set { SetValue(CanMovePrevProperty, value); }
        }

        public ICommand SelectedItemChangedCommand
        {
            get { return (ICommand)GetValue(SelectedItemChangedCommandProperty); }
            set { SetValue(SelectedItemChangedCommandProperty, value); }
        }

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        public IList<SwitchCheckListBoxItem> DisplaySource
        {
            get { return (IList<SwitchCheckListBoxItem>)GetValue(DisplaySourceProperty); }
            set { SetValue(DisplaySourceProperty, value); }
        }

        public int DisplayCount
        {
            get { return (int)GetValue(DisplayCountProperty); }
            set { SetValue(DisplayCountProperty, value); }
        }

        public static readonly DependencyProperty DisplayCountProperty =
            DependencyProperty.Register(nameof(DisplayCount), typeof(int), typeof(SwitchCheckListBox), new PropertyMetadata(3));

        public static readonly DependencyProperty DisplaySourceProperty =
            DependencyProperty.Register(nameof(DisplaySource), typeof(IList<SwitchCheckListBoxItem>), typeof(SwitchCheckListBox));

        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(nameof(DisplayMemberPath), typeof(string), typeof(SwitchCheckListBox));

        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(SwitchCheckListBox), new PropertyMetadata(0, OnSelectedIndexChanged));

        public static readonly DependencyProperty SelectedItemChangedCommandProperty =
            DependencyProperty.Register(nameof(SelectedItemChangedCommand), typeof(ICommand), typeof(SwitchCheckListBox));

        public static readonly DependencyProperty CanMovePrevProperty =
            DependencyProperty.Register(nameof(CanMovePrev), typeof(bool), typeof(SwitchCheckListBox));

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(SwitchCheckListBox), new PropertyMetadata(OnItemsSourceChanged));

        public static readonly DependencyProperty CanMovetoNextProperty =
            DependencyProperty.Register(nameof(CanMovetoNext), typeof(bool), typeof(SwitchCheckListBox));

        public SwitchCheckListBox()
        {
            Unloaded += SwitchCheckListBox_Unloaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UnloadedControls();
            PART_ListBox = GetTemplateChild("PART_ListBox") as System.Windows.Controls.ListBox;
            PART_PrevBtn = GetTemplateChild("PART_PrevBtn") as Button;
            PART_NextBtn = GetTemplateChild("PART_NextBtn") as Button;
            if (PART_ListBox != null)
            {
                PART_ListBox.SelectionChanged += PART_ListBox_SelectionChanged;
            }
            if(PART_PrevBtn != null)
            {
                PART_PrevBtn.Click += PART_PrevBtn_Click;
            }
            if (PART_NextBtn != null)
            {
                PART_NextBtn.Click += PART_NextBtn_Click; ;
            }
        }

        private void PART_NextBtn_Click(object sender, RoutedEventArgs e)
        {
            SelectedIndex++;
            CheckBtnEnable();
        }

        private void CheckBtnEnable()
        {
            CanMovetoNext = SelectedIndex < GetItemsSourceCount() - 1;
            CanMovePrev = SelectedIndex > 0;
        }

        private void PART_PrevBtn_Click(object sender, RoutedEventArgs e)
        {
            SelectedIndex--;
            CheckBtnEnable();
        }

        private void PART_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as System.Windows.Controls.ListBox;
            if(listBox.SelectedItem == null)
            {
                return;
            }
            var item = listBox.SelectedItem as SwitchCheckListBoxItem;
            SelectedIndex = item.Index;
            CanMovePrev = false;
            CanMovetoNext = false;
            SelectedItemChangedCommand?.Execute(item.Data);
            CheckBtnEnable();
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SwitchCheckListBox;
            control.ChangedDisplaySource(control.SelectedIndex);
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SwitchCheckListBox;
            var total = control.GetItemsSourceCount();
            var newValue = (int)e.NewValue;
            if(newValue > total - 1)
            {
                control.SelectedIndex = total - 1;
                return;
            }
            if(newValue < 0)
            {
                control.SelectedIndex = 0;
                return;
            }
            control.ChangedDisplaySource(newValue);
        }

        private void ChangedDisplaySource(int index)
        {
            var startIndex = index - ((DisplayCount / 2) - (DisplayCount % 2 == 0 ? 1 : 0));
            if(startIndex < 0)
            {
                startIndex = 0;
            }
            if(startIndex > GetItemsSourceCount() - DisplayCount)
            {
                startIndex = GetItemsSourceCount() - DisplayCount;
            }
            DisplaySource = GetItemsRange(startIndex);
        }

        private IList<SwitchCheckListBoxItem> GetItemsRange(int startIndex)
        {
            IList<SwitchCheckListBoxItem> result = new List<SwitchCheckListBoxItem>();
            for (int i = startIndex; i < startIndex + DisplayCount; i++)
            {
                var data = GetItemsByIndex(i);
                if(data != null)
                {
                    result.Add(new SwitchCheckListBoxItem() { Data = GetItemsByIndex(i), IsSelected = i == SelectedIndex, Index = i });
                }
            }
            return result;
        }

        private object GetItemsByIndex(int index)
        {
            int count = 0;
            object result = null;
            foreach (var item in ItemsSource)
            {
                if(count == index)
                {
                    result = item;
                    break;
                }
                count++;
            }
            return result;
        }

        private int GetItemsSourceCount()
        {
            int result = 0;
            if(ItemsSource != null)
            {
                foreach (var item in ItemsSource)
                {
                    result++;
                }
            }
            return result;
        }

        private void SwitchCheckListBox_Unloaded(object sender, RoutedEventArgs e)
        {
            UnloadedControls();
            Unloaded -= SwitchCheckListBox_Unloaded;
        }

        private void UnloadedControls()
        {
            if (PART_ListBox != null)
            {
                PART_ListBox.SelectionChanged -= PART_ListBox_SelectionChanged;
            }
            if (PART_PrevBtn != null)
            {
                PART_PrevBtn.Click -= PART_PrevBtn_Click;
            }
            if (PART_NextBtn != null)
            {
                PART_NextBtn.Click -= PART_NextBtn_Click; ;
            }
        }
    }
}
