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
        /// <summary>
        /// 减少音量
        /// </summary>
        public void VoiceDown()
        {
            Win32Api.SendMessage(Process.GetCurrentProcess().MainWindowHandle, Win32Api.WM_APPCOMMAND, 0x30292, Win32Api.APPCOMMAND_VOLUME_DOWN * 0x10000);
        }

        /// <summary>
        /// 增大音量
        /// </summary>
        public void VoiceUp()
        {
            Win32Api.SendMessage(Process.GetCurrentProcess().MainWindowHandle, Win32Api.WM_APPCOMMAND, 0x30292, Win32Api.APPCOMMAND_VOLUME_UP * 0x10000);
        }

        /// <summary>
        /// 静音
        /// </summary>
        public static void VoiceClose()
        {
            Win32Api.SendMessage(Process.GetCurrentProcess().MainWindowHandle, Win32Api.WM_APPCOMMAND, 0x200eb0, Win32Api.APPCOMMAND_VOLUME_MUTE * 0x10000);
        }
    }
}
