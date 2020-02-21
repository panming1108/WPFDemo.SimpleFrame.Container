using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Infra.Models
{
    public class FlushModel<T>
    {
        public bool IsFlush { get; set; }
        public T Data { get; set; }

        public FlushModel(T data, bool isFlush = true)
        {
            Data = data;
            IsFlush = isFlush;
        }
    }
}
