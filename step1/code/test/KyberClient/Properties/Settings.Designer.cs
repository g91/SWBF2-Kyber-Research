using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace KyberClient.Properties
{
	// Token: 0x02000008 RID: 8
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002167 File Offset: 0x00000367
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000031 RID: 49 RVA: 0x0000216E File Offset: 0x0000036E
		// (set) Token: 0x06000032 RID: 50 RVA: 0x00002180 File Offset: 0x00000380
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("stable")]
		public string DLLchannel
		{
			get
			{
				return (string)this["DLLchannel"];
			}
			set
			{
				this["DLLchannel"] = value;
			}
		}

		// Token: 0x0400001D RID: 29
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
