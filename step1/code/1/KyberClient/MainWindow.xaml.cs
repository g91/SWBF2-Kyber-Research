using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using KyberClient.Properties;

namespace KyberClient
{
	// Token: 0x02000006 RID: 6
	public partial class MainWindow : Window
	{
		// Token: 0x0600001D RID: 29 RVA: 0x0000262C File Offset: 0x0000082C
		private static string ToHex(byte[] bytes, bool upperCase)
		{
			StringBuilder stringBuilder = new StringBuilder(bytes.Length * 2);
			for (int i = 0; i < bytes.Length; i++)
			{
				stringBuilder.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000267C File Offset: 0x0000087C
		public string SHA256CheckSum(string filePath)
		{
			string result;
			using (SHA256 sha = SHA256.Create())
			{
				using (FileStream fileStream = File.OpenRead(filePath))
				{
					result = MainWindow.ToHex(sha.ComputeHash(fileStream), false);
				}
			}
			return result;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000026D8 File Offset: 0x000008D8
		private static string SHA256HexHashString(byte[] bufIn)
		{
			string result;
			using (SHA256 sha = SHA256.Create())
			{
				result = MainWindow.ToHex(sha.ComputeHash(bufIn), false);
			}
			return result;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002718 File Offset: 0x00000918
		public string GetDllPath()
		{
			this.rootDir = Path.Combine(Path.GetTempPath(), "Kyber", this.buildVersion ?? "default");
			if (!Directory.Exists(this.rootDir))
			{
				Directory.CreateDirectory(this.rootDir);
			}
			return Path.Combine(this.rootDir, "Kyber.dll");
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002774 File Offset: 0x00000974
		public void DownloadDll()
		{
			string dllPath = this.GetDllPath();
			using (WebClient webClient = new WebClient())
			{
				try
				{
					string a = File.Exists(dllPath) ? this.SHA256CheckSum(dllPath) : "";
					string b = webClient.DownloadString("https://kyber.gg/api/hashes/distributions/" + Settings.Default.DLLchannel + "/dll");
					if (a != b)
					{
						webClient.DownloadFile("https://kyber.gg/api/downloads/distributions/" + Settings.Default.DLLchannel + "/dll", dllPath);
					}
				}
				catch (WebException ex)
				{
					using (StreamReader streamReader = new StreamReader(ex.Response.GetResponseStream()))
					{
						MessageBox.Show(streamReader.ReadToEnd(), "Exception", MessageBoxButton.OK, MessageBoxImage.Hand);
						Settings.Default.DLLchannel = (Settings.Default.Properties["DLLchannel"].DefaultValue as string);
						Settings.Default.Save();
					}
				}
				catch (Exception ex2)
				{
					MessageBox.Show(ex2.InnerException.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Hand);
					Settings.Default.DLLchannel = (Settings.Default.Properties["DLLchannel"].DefaultValue as string);
					Settings.Default.Save();
				}
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002910 File Offset: 0x00000B10
		public void checkVersion()
		{
			try
			{
				using (WebClient webClient = new WebClient())
				{
					Version value = new Version(webClient.DownloadString("https://kyber.gg/api/version/launcher"));
					if (new Version(Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(",", ".")).CompareTo(value) < 0 && MessageBox.Show("You are using an outdated version of Kyber Client." + Environment.NewLine + "Would you like to download the latest version?", "Outdated Kyber Client", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
					{
						Process.Start("https://kyber.gg/#download");
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.InnerException.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Hand);
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000029D8 File Offset: 0x00000BD8
		public void InjectionThread()
		{
			for (;;)
			{
				Process[] processesByName = Process.GetProcessesByName("starwarsbattlefrontii");
				for (int i = 0; i < this.injectedTo.Count; i++)
				{
					bool flag = false;
					for (int j = 0; j < processesByName.Length; j++)
					{
						if (processesByName[j].Id == this.injectedTo[i])
						{
							flag = true;
							base.Dispatcher.Invoke(delegate()
							{
								this.Injected.Visibility = Visibility.Visible;
								this.NotInjected.Visibility = Visibility.Hidden;
							});
						}
					}
					if (!flag)
					{
						this.injectedTo.RemoveAt(i);
						base.Dispatcher.Invoke(delegate()
						{
							this.Injected.Visibility = Visibility.Hidden;
							this.NotInjected.Visibility = Visibility.Visible;
						});
					}
				}
				if (processesByName.Length == 0)
				{
					Thread.Sleep(250);
				}
				else if (this.injectedTo.Count > 0)
				{
					Thread.Sleep(2000);
				}
				else
				{
					this.DownloadDll();
					DllInjector.Inject(processesByName.ElementAt(0).Id, this.GetDllPath());
					this.injectedTo.Add(processesByName.ElementAt(0).Id);
				}
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002AD4 File Offset: 0x00000CD4
		public MainWindow()
		{
			this.InitializeComponent();
			this.txt_Version.Text = "v" + Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, 5).Replace(",", ".");
			base.KeyDown += this.MainWindow_KeyDown;
			this.DownloadDll();
			this.checkVersion();
			new Thread(new ThreadStart(this.InjectionThread))
			{
				IsBackground = true
			}.Start();
			DateTime? buildUTCTimestamp = ResourceExtractor.GetBuildUTCTimestamp();
			if (buildUTCTimestamp != null)
			{
				this.buildTimestampUTC = buildUTCTimestamp.Value;
				this.buildVersion = buildUTCTimestamp.Value.ToString("yyyy.MM.dd-HHmmss");
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002BB3 File Offset: 0x00000DB3
		private void window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Keyboard.ClearFocus();
			base.Focus();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002BC1 File Offset: 0x00000DC1
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Process.Start("https://kyber.gg/servers");
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002BCE File Offset: 0x00000DCE
		private void MainWindow_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.F12)
			{
				this.settingsWindow.Show();
				this.settingsWindow.Focus();
			}
		}

		// Token: 0x0400000E RID: 14
		public static bool DLLchannel;

		// Token: 0x0400000F RID: 15
		private SettingsWindow settingsWindow = new SettingsWindow();

		// Token: 0x04000010 RID: 16
		private DateTime buildTimestampUTC;

		// Token: 0x04000011 RID: 17
		private string buildVersion;

		// Token: 0x04000012 RID: 18
		public string rootDir;

		// Token: 0x04000013 RID: 19
		private List<int> injectedTo = new List<int>();
	}
}
