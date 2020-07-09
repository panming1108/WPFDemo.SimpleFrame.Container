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
            var column = item as GroupGridViewColumn;
            column.InitGridViewCell(cell);
        }
    }
}
