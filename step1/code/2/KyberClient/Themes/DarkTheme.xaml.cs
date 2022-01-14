using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace KyberClient.Themes
{
	// Token: 0x02000009 RID: 9
	public partial class DarkTheme : ResourceDictionary, IStyleConnector
	{
		// Token: 0x06000035 RID: 53 RVA: 0x00002E4C File Offset: 0x0000104C
		private void CloseWindow_Event(object sender, RoutedEventArgs e)
		{
			if (e.Source != null)
			{
				try
				{
					this.CloseWind(Window.GetWindow((FrameworkElement)e.Source));
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002E8C File Offset: 0x0000108C
		private void AutoMinimize_Event(object sender, RoutedEventArgs e)
		{
			if (e.Source != null)
			{
				try
				{
					this.MaximizeRestore(Window.GetWindow((FrameworkElement)e.Source));
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002ECC File Offset: 0x000010CC
		private void Minimize_Event(object sender, RoutedEventArgs e)
		{
			if (e.Source != null)
			{
				try
				{
					this.MinimizeWind(Window.GetWindow((FrameworkElement)e.Source));
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000021AC File Offset: 0x000003AC
		public void CloseWind(Window window)
		{
			if (window.Title == "KYBER")
			{
				Application.Current.Shutdown();
				return;
			}
			window.Hide();
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000021D1 File Offset: 0x000003D1
		public void MaximizeRestore(Window window)
		{
			if (window.WindowState == WindowState.Maximized)
			{
				window.WindowState = WindowState.Normal;
				return;
			}
			if (window.WindowState == WindowState.Normal)
			{
				window.WindowState = WindowState.Maximized;
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000021F3 File Offset: 0x000003F3
		public void MinimizeWind(Window window)
		{
			window.WindowState = WindowState.Minimized;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002205 File Offset: 0x00000405
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				((Button)target).Click += this.Minimize_Event;
				return;
			}
			if (connectionId != 2)
			{
				return;
			}
			((Button)target).Click += this.CloseWindow_Event;
		}

		// Token: 0x0400001E RID: 30
		public const string WrapperGridName = "WrapperGrid";
	}
}
