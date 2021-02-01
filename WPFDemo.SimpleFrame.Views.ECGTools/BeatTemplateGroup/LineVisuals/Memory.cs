using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WPFDemo.SimpleFrame.Views.ECGTools.BeatTemplateGroup
{
	public class Memory
	{
		public static int SizeOf(IntPtr structure)
		{
			return Marshal.SizeOf((object)(long)structure);
		}

		public static IntPtr AllocHGlobal(int cb)
		{
			return Marshal.AllocHGlobal(cb);
		}

		public static void FreeHGlobal(IntPtr hglobal)
		{
			Marshal.FreeHGlobal(hglobal);
		}

		[DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int memcmp(byte[] b1, byte[] b2, long count);

		[DllImport("kernel32.dll")]
		private static extern void RtlMoveMemory(IntPtr dest, IntPtr src, uint count);

		public static void RtlMoveMemory(IntPtr src, IntPtr dest, int count)
		{
			RtlMoveMemory(dest, src, (uint)count);
		}

		public static void Copy(IntPtr source, int[] destination, int startIndex, int length)
		{
			Marshal.Copy(source, destination, startIndex, length);
		}

		public static void BlockCopy(Array src, int srcOffset, Array dst, int dstOffset, int count)
		{
			Buffer.BlockCopy(src, srcOffset, dst, dstOffset, count);
		}
	}
}
