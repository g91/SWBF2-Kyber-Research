using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleApp2
{
	public static class DllInjector
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetModuleHandleA(string lpModuleName);

		[DllImport("kernelbase.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr LoadLibraryA(string lpModuleName);

		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool CloseHandle(IntPtr hObject);

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

		private const int PROCESS_CREATE_THREAD = 2;
		private const int PROCESS_QUERY_INFORMATION = 1024;
		private const int PROCESS_VM_OPERATION = 8;
		private const int PROCESS_VM_WRITE = 32;
		private const int PROCESS_VM_READ = 16;
		private const uint MEM_COMMIT = 4096U;
		private const uint MEM_RESERVE = 8192U;
		private const uint PAGE_READWRITE = 4U;
	}
}
