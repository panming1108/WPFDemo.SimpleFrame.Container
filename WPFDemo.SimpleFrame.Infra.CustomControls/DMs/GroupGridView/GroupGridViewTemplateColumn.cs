using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs
{
    public class GroupGridViewTemplateColumn : GroupGridViewColumn
    {
        public DataTemplate CellTemplate
        {
            get { return (DataTemplate)GetValue(CellTemplateProperty); }
            set { SetValue(CellTemplateProperty, value); }
        }

        public static readonly DependencyProperty CellTemplateProperty =
            DependencyProperty.Register(nameof(CellTemplate), typeof(DataTemplate), typeof(GroupGridViewTemplateColumn));

        protected override void LoadedGridViewCell(GroupGridViewCell cell)
        {
            cell.SetBinding(GroupGridViewCell.ContentProperty, new Binding());
            cell.ContentTemplate = CellTemplate;
        }
    }
}
