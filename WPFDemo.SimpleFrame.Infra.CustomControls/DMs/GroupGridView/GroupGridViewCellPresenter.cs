using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs
{
    public class GroupGridViewCellPresenter : ItemsControl
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new GroupGridViewCell();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is GroupGridViewCell;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            GroupGridViewCell cell = element as GroupGridViewCell;
            cell.DataContext = DataContext;
            if (item is GroupGridViewTextColumn)
            {
                var column = item as GroupGridViewTextColumn;
                cell.Width = column.Width.DisplayValue;
                TextBlock textBlock = new TextBlock();
                textBlock.SetBinding(TextBlock.TextProperty, column.Binding);
                cell.Content = textBlock;
            }
            else if(item is GroupGridViewTemplateColumn)
            {
                var column = item as GroupGridViewTemplateColumn;
                cell.Width = column.Width.DisplayValue;
                cell.SetBinding(GroupGridViewCell.ContentProperty, new Binding());
                cell.ContentTemplate = column.CellTemplate;
            }
        }
    }
}
