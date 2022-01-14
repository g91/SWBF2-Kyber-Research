using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace ConsoleApp2
{
    internal class Program
    {
		static string buildVersion = "1.0.8";
		static string rootDir;
		static string DLLchannel = "stable";
		static List<int> injectedTo = new List<int>();

		public static void DownloadDll()
		{
			Console.WriteLine("call DownloadDll");
			string dllPath = GetDllPath();
			using (WebClient webClient = new WebClient())
			{
				try
				{
					webClient.DownloadFile("https://kyber.gg/api/downloads/distributions/" + DLLchannel + "/dll", "Kyber.dll");
				}
				catch (WebException ex)
				{
					using (StreamReader streamReader = new StreamReader(ex.Response.GetResponseStream()))
					{
						Console.WriteLine("WebException: catch" +  streamReader.ReadToEnd());
					}
				}
				catch (Exception ex2)
				{
					Console.WriteLine("Exception: catch" + ex2.InnerException.Message);
				}
			}
		}

		public static void InjectionThread()
		{
			for (;;)
			{
				Process[] processesByName = Process.GetProcessesByName("starwarsbattlefrontii");
				for (int i = 0; i < injectedTo.Count; i++)
				{
					bool flag = false;
					for (int j = 0; j < processesByName.Length; j++)
					{
						if (processesByName[j].Id == injectedTo[i])
						{
							flag = true;
						}
					}
					if (!flag)
					{
						injectedTo.RemoveAt(i);
					}
				}
				if (processesByName.Length == 0)
				{
					Thread.Sleep(250);
				}
				else if (injectedTo.Count > 0)
				{
					Thread.Sleep(2000);
				}
				else
				{
					DownloadDll();
					DllInjector.Inject(processesByName.ElementAt(0).Id, "Kyber.dll");
					injectedTo.Add(processesByName.ElementAt(0).Id);
				}
			}
		}

		public static void Main(string[] args)
        	{
			DownloadDll();
			new Thread(new ThreadStart(InjectionThread))
			{
				IsBackground = true
			}.Start();


			Console.Read();
		}
    }
}
