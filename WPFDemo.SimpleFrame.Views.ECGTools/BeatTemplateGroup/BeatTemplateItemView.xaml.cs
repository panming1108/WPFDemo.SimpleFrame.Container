﻿using System;
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
    /// BeatTemplateItemView.xaml 的交互逻辑
    /// </summary>
    public partial class BeatTemplateItemView : UserControl
    {
        private readonly BrushConverter _brushConverter;
        private readonly Brush _hoverBorderBrush;
        private readonly Brush _selectedBorderBrush;
        private readonly Brush _commonBorderBrush;
        private readonly Pen _pen;
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnSelectedChanged(value);
                }
            }
        }

        private void OnSelectedChanged(bool newSelectedValue)
        {
            if (newSelectedValue)
            {
                PART_Border.BorderBrush = _selectedBorderBrush;
            }
            else
            {
                PART_Border.BorderBrush = _commonBorderBrush;
            }
        }
        public BeatTemplateItemView()
        {
            _brushConverter = new BrushConverter();
            _hoverBorderBrush = (Brush)_brushConverter.ConvertFromString("#00AAFF");
            _commonBorderBrush = (Brush)_brushConverter.ConvertFromString("#AEBFCC");
            _selectedBorderBrush = (Brush)_brushConverter.ConvertFromString("#00AAFF");
            _pen = new Pen(Brushes.Black, 1);
            InitializeComponent();
            MouseEnter += BeatTemplateItemView_MouseEnter;
            MouseLeave += BeatTemplateItemView_MouseLeave;
            Unloaded += BeatTemplateItemView_Unloaded;
        }

        private void BeatTemplateItemView_Unloaded(object sender, RoutedEventArgs e)
        {
            MouseEnter -= BeatTemplateItemView_MouseEnter;
            MouseLeave -= BeatTemplateItemView_MouseLeave;
            Unloaded -= BeatTemplateItemView_Unloaded;
        }

        private void BeatTemplateItemView_MouseLeave(object sender, MouseEventArgs e)
        {
            if (IsSelected)
            {
                PART_Border.BorderBrush = _selectedBorderBrush;
            }
            else
            {
                PART_Border.BorderBrush = _commonBorderBrush;
            }
        }

        private void BeatTemplateItemView_MouseEnter(object sender, MouseEventArgs e)
        {
            PART_Border.BorderBrush = _hoverBorderBrush;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var data = (BeatTemplate)DataContext;
            PART_TypeName.Text = data.TypeName;
            PART_AddFlag.Visibility = Visibility.Visible;
            PART_CheckFlag.Visibility = Visibility.Visible;
            PART_Count.Text = "3355";
            PART_Percent.Text = "43.9%";
            Height = 132;
        }
    }
}