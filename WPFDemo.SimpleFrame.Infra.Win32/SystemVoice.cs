using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace WPFDemo.SimpleFrame.Infra.Win32
{
    public class SystemVoice
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);
        const uint WM_APPCOMMAND = 0x319;
        const uint APPCOMMAND_VOLUME_UP = 0x0a;
        const uint APPCOMMAND_VOLUME_DOWN = 0x09;
        const uint APPCOMMAND_VOLUME_MUTE = 0x08;

        /// <summary>
        /// 减少音量
        /// </summary>
        public void VoiceDown()
        {
            SendMessage(Process.GetCurrentProcess().MainWindowHandle, WM_APPCOMMAND, 0x30292, APPCOMMAND_VOLUME_DOWN * 0x10000);
        }

        /// <summary>
        /// 增大音量
        /// </summary>
        public void VoiceUp()
        {
            SendMessage(Process.GetCurrentProcess().MainWindowHandle, WM_APPCOMMAND, 0x30292, APPCOMMAND_VOLUME_UP * 0x10000);
        }

        /// <summary>
        /// 静音
        /// </summary>
        public static void VoiceClose()
        {
            SendMessage(Process.GetCurrentProcess().MainWindowHandle, WM_APPCOMMAND, 0x200eb0, APPCOMMAND_VOLUME_MUTE * 0x10000);
        }
    }
}
