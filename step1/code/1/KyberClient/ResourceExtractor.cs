using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace KyberClient
{
	// Token: 0x02000005 RID: 5
	public static class ResourceExtractor
	{
		// Token: 0x0600001B RID: 27 RVA: 0x00002400 File Offset: 0x00000600
		public static bool ExtractResourceToFile(string resourceName, string filename)
		{
			using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
			{
				if (manifestResourceStream == null)
				{
					return false;
				}
				using (FileStream fileStream = new FileStream(filename, FileMode.Create))
				{
					byte[] array = new byte[manifestResourceStream.Length];
					manifestResourceStream.Read(array, 0, array.Length);
					try
					{
						fileStream.Write(array, 0, array.Length);
					}
					catch (IOException)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002494 File Offset: 0x00000694
		public static DateTime? GetBuildUTCTimestamp()
		{
			DateTime? dateTime;
			using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BattlefrontToolsClient.BuildTimestamp.txt"))
			{
				if (manifestResourceStream == null)
				{
					dateTime = null;
					dateTime = dateTime;
				}
				else
				{
					byte[] array = new byte[manifestResourceStream.Length];
					manifestResourceStream.Read(array, 0, array.Length);
					string @string = Encoding.Unicode.GetString(array);
					if (@string.Length <= 50)
					{
						dateTime = null;
					}
					else
					{
						int num = @string.LastIndexOf("\n", @string.Length - 2);
						if (num < 0)
						{
							dateTime = null;
						}
						else
						{
							string text = @string.Substring(num + 1);
							if (text.Length < 24)
							{
								dateTime = null;
							}
							else
							{
								string s = text.Substring(22);
								int num2;
								try
								{
									num2 = int.Parse(s, NumberStyles.AllowTrailingWhite);
								}
								catch (FormatException)
								{
									return null;
								}
								catch (OverflowException)
								{
									return null;
								}
								int num3 = num2 / 60;
								int num4 = num2 % 60;
								dateTime = new DateTime?(DateTime.ParseExact(string.Format("{0}{1:00}:{2:00}", text.Substring(0, 22), num3, num4), "yyyyMMddHHmmss.ffffffzzz", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal));
							}
						}
					}
				}
			}
			return dateTime;
		}
	}
}
