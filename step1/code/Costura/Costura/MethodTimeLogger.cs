using System;
using System.Reflection;

// Token: 0x02000002 RID: 2
internal static class MethodTimeLogger
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public static void Log(MethodBase methodBase, long milliseconds, string message)
	{
		MethodTimeLogger.Log(methodBase.DeclaringType ?? typeof(object), methodBase.Name, milliseconds, message);
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002073 File Offset: 0x00000273
	public static void Log(Type type, string methodName, long milliseconds, string message)
	{
	}
}
