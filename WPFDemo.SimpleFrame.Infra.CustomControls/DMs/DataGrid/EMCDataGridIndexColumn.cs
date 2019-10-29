using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs.DataGrid
{
    public class EMCDataGridIndexColumn : DataGridTextColumn
    {
        public EMCDataGridIndexColumn()
        {
            Binding binding = new Binding();
            binding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(DataGridRow), 1);
            binding.Path = new PropertyPath("Header");
            Binding = binding;
        }
    }
}
