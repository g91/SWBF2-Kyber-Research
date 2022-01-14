using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using KyberClient.Properties;

namespace KyberClient
{
	// Token: 0x02000002 RID: 2
	public partial class SettingsWindow : Window
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002050 File Offset: 0x00000250
		public SettingsWindow()
		{
			this.InitializeComponent();
			this.txt_Version.Text = "v" + this.version;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020A8 File Offset: 0x000002A8
		public string GetKyberTempDir()
		{
			string text = Path.Combine(Path.GetTempPath(), "Kyber");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			return text;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020D5 File Offset: 0x000002D5
		public void refresh()
		{
			Settings.Default.DLLchannel = this.DLLChannel.Text;
			Settings.Default.Save();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020F6 File Offset: 0x000002F6
		private void window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Keyboard.ClearFocus();
			this.refresh();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002103 File Offset: 0x00000303
		private void DLLChannel_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.refresh();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000210C File Offset: 0x0000030C
		private void Reset_Settings(object sender, RoutedEventArgs e)
		{
			if (File.Exists(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath))
			{
				File.Delete(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath);
				Settings.Default.DLLchannel = (Settings.Default.Properties["DLLchannel"].DefaultValue as string);
				Settings.Default.Save();
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002170 File Offset: 0x00000370
		private void Defender_DLL(object sender, RoutedEventArgs e)
		{
			string kyberTempDir = this.GetKyberTempDir();
			string location = Assembly.GetExecutingAssembly().Location;
			new Process
			{
				StartInfo = new ProcessStartInfo
				{
					WindowStyle = ProcessWindowStyle.Hidden,
					FileName = "cmd.exe",
					Arguments = string.Concat(new string[]
					{
						"/C powershell -inputformat none -outputformat none -NonInteractive -Command \"Add-MpPreference -ExclusionPath '",
						kyberTempDir,
						"'\"&powershell -inputformat none -outputformat none -NonInteractive -Command \"Add-MpPreference -ExclusionPath '",
						location,
						"'\""
					}),
					Verb = "runas"
				}
			}.Start();
			this.refresh();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021FD File Offset: 0x000003FD
		private void Reload_Click(object sender, RoutedEventArgs e)
		{
			new MainWindow().DownloadDll();
			this.refresh();
		}

		// Token: 0x04000001 RID: 1
		private string version = Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, 5);
	}
}
