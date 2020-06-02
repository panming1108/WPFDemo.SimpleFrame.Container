using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs
{
    public class RowData
    {
        public int Row { get; set; }

        public object Data { get; set; }

        public bool IsSelected { get; set; }

        public RowData(int row, object data)
        {
            Row = row;
            Data = data;
        }
    }
}
