using System;
using System.Runtime.InteropServices;
using System.Text;

namespace KyberClient
{
	// Token: 0x02000004 RID: 4
	public static class DllInjector
	{
		// Token: 0x0600000F RID: 15
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		// Token: 0x06000010 RID: 16
		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

		// Token: 0x06000011 RID: 17
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		// Token: 0x06000012 RID: 18
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		// Token: 0x06000013 RID: 19
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetModuleHandleA(string lpModuleName);

		// Token: 0x06000014 RID: 20
		[DllImport("kernelbase.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr LoadLibraryA(string lpModuleName);

		// Token: 0x06000015 RID: 21
		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		// Token: 0x06000016 RID: 22
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

		// Token: 0x06000017 RID: 23
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

		// Token: 0x06000018 RID: 24
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);

		// Token: 0x06000019 RID: 25
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool CloseHandle(IntPtr hObject);

		// Token: 0x0600001A RID: 26 RVA: 0x00002514 File Offset: 0x00000714
		public static bool Inject(int processId, string dllName)
		{
			IntPtr hProcess = DllInjector.OpenProcess(1082, false, processId);
			IntPtr procAddress = DllInjector.GetProcAddress(DllInjector.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
			IntPtr intPtr = DllInjector.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)((dllName.Length + 1) * Marshal.SizeOf(typeof(char))), 12288U, 4U);
			UIntPtr uintPtr;
			DllInjector.WriteProcessMemory(hProcess, intPtr, Encoding.Default.GetBytes(dllName), (uint)((dllName.Length + 1) * Marshal.SizeOf(typeof(char))), out uintPtr);
			IntPtr intPtr2;
			DllInjector.CreateRemoteThread(hProcess, IntPtr.Zero, 0U, procAddress, intPtr, 0U, out intPtr2);
			return true;
		}

		// Token: 0x04000006 RID: 6
		private const int PROCESS_CREATE_THREAD = 2;

		// Token: 0x04000007 RID: 7
		private const int PROCESS_QUERY_INFORMATION = 1024;

		// Token: 0x04000008 RID: 8
		private const int PROCESS_VM_OPERATION = 8;

		// Token: 0x04000009 RID: 9
		private const int PROCESS_VM_WRITE = 32;

		// Token: 0x0400000A RID: 10
		private const int PROCESS_VM_READ = 16;

		// Token: 0x0400000B RID: 11
		private const uint MEM_COMMIT = 4096U;

		// Token: 0x0400000C RID: 12
		private const uint MEM_RESERVE = 8192U;

		// Token: 0x0400000D RID: 13
		private const uint PAGE_READWRITE = 4U;
	}
}
