using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WPFDemo.SimpleFrame.Infra.Win32
{
    public class SimulateKeyBoard
    {
        public static void KeyEvent(Keys key, bool keyup)
        {
            Win32Api.keybd_event(key, 0, (uint)(keyup ? Win32Api.KEYEVENTF_KEYUP : 0), 0);           
        }
    }
}
