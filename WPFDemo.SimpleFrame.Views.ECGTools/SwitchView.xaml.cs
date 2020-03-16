﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using WPFDemo.SimpleFrame.Infra.CustomControls.ECGTools;
using WPFDemo.SimpleFrame.Infra.Helper;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    /// <summary>
    /// SwitchView.xaml 的交互逻辑
    /// </summary>
    public partial class SwitchView : UserControl
    {
        private Timer _timer;
        public SwitchView()
        {
            InitializeComponent();
            Loaded += SwitchView_Loaded;
            Unloaded += SwitchView_Unloaded;

            _timer = new Timer(1000);
            _timer.Elapsed += Timer_Elapsed;
        }

        private void SwitchView_Unloaded(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            _timer.Dispose();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DispatcherHelper.InvokeOnUIThread(new Action(
                    ()=> 
                    {
                        HR.HR = new Random().Next(40, 120);
                    }));
        }

        private void SwitchView_Loaded(object sender, RoutedEventArgs e)
        {
            var list = new string[] { "N*1", "6*2", "6*2+1", "3*4", "3*4+1" };
            PART_LayOutSwitch.ItemsSource = list;
            PART_LeadSwitch.ItemsSource = list;
            PART_GroupLeadSwitch.GroupSource = new Dictionary<string, IEnumerable<string>>()
            {
                { "胸导联" , new string[]{"l", "ll", "lll", "aVR", "aVL", "aVF" } },
                { "肢体导联", new string[]{"V1","V2","V3", "V4", "V5", "V6" } }
            };


            PART_LayOutSwitch.SelectedItem = list[1];

            PART_LeadSwitch.SelectedItems.Add(list[0]);
            PART_LeadSwitch.SelectedItems.Add(list[1]);

            PART_GroupLeadSwitch.SelectedItems.Add("l");
            PART_GroupLeadSwitch.SelectedItems.Add("V1");

            //PART_GroupLeadSwitch.SelectedItems.Add(list[0]);
            //PART_GroupLeadSwitch.SelectedItems.Add(list[1]);

            _timer.Start();
        }

        private void PART_LayOutSwitch_SelectionChanged(object sender, LeadSwitchSelectionChangedEventArgs e)
        {
            string result = string.Empty;
            foreach (var item in e.SelectedItems)
            {
                result += item.ToString() + ",";
            }
            PART_LayOutSwitchText.Text = result.TrimEnd(',');
        }

        private void PART_LeadSwitch_SelectionChanged(object sender, LeadSwitchSelectionChangedEventArgs e)
        {
            string result = string.Empty;
            foreach (var item in e.SelectedItems)
            {
                result += item.ToString() + ",";
            }
            PART_LeadSwitchText.Text = result.TrimEnd(',');
        }

        private void PART_GroupLeadSwitch_SelectedItemsChanged(object sender, LeadSwitchSelectionChangedEventArgs e)
        {
            string result = string.Empty;
            foreach (var item in e.SelectedItems)
            {
                result += item.ToString() + ",";
            }
            PART_GroupLeadSwitchText.Text = result.TrimEnd(',');
        }
    }

    public enum TwelveLeadLayoutEnum
    {
        [Description("N*1")]
        TwelveOne = 1,
        [Description("6*2")]
        SixTwo = 2,
        [Description("6*2+1")]
        SixTwoAppend = 3,
        [Description("3*4")]
        ThreeFour = 4,
        [Description("3*4+1")]
        ThreeFourAppend = 5
    }
}
