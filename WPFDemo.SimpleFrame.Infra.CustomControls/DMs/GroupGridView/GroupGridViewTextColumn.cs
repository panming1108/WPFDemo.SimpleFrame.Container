using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs
{
    public class GroupGridViewTextColumn : GroupGridViewColumn
    {
        public BindingBase Binding { get; set; }

        protected override void LoadedGridViewCell(GroupGridViewCell cell)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.SetBinding(TextBlock.TextProperty, Binding);
            cell.Content = textBlock;
        }
    }
}
