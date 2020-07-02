using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs
{
    public class GroupGridViewTextColumn : GroupGridViewColumn
    {
        public BindingBase Binding { get; set; }
    }
}
