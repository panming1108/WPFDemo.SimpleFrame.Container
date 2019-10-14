using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFDemo.SimpleFrame.Infra.DialogConsumer.Interfaces
{
    public interface IBusyIndicatorConsumer : IDisposable
    {
        void Init(Window owner);
    }
}
