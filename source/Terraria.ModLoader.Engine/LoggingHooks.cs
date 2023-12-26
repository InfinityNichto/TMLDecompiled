using System;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MonoMod.RuntimeDetour;

namespace Terraria.ModLoader.Engine;

internal static class LoggingHooks
{
	private delegate int orig_WriteFileNative(IntPtr hFile, ReadOnlySpan<byte> bytes, bool useFileAPIs);

	private delegate int hook_WriteFileNative(orig_WriteFileNative orig, IntPtr hFile, ReadOnlySpan<byte> bytes, bool useFileAPIs);

	private delegate void orig_StackTrace_CaptureStackTrace(StackTrace self, int skipFrames, bool fNeedFileInfo, Exception e);

	private delegate void hook_StackTrace_CaptureStackTrace(orig_StackTrace_CaptureStackTrace orig, StackTrace self, int skipFrames, bool fNeedFileInfo, Exception e);

	private delegate ValueTask<HttpResponseMessage> orig_SendAsyncCore(object self, HttpRequestMessage request, Uri? proxyUri, bool async, bool doRequestAuth, bool isProxyConnect, CancellationToken cancellationToken);

	private delegate ValueTask<HttpResponseMessage> hook_SendAsyncCore(orig_SendAsyncCore orig, object self, HttpRequestMessage request, Uri? proxyUri, bool async, bool doRequestAuth, bool isProxyConnect, CancellationToken cancellationToken);

	private static Hook writeFileNativeHook;

	private static Hook processStartHook;

	private static Hook stackTrace_CaptureStackTrace;

	private static Hook httpSendAsyncHook;

	internal static void Init()
	{
		FixBrokenConsolePipeError();
		PrettifyStackTraceSources();
		HookWebRequests();
		HookProcessStart();
	}

	private static void FixBrokenConsolePipeError()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		if (OperatingSystem.IsWindows())
		{
			writeFileNativeHook = new Hook((MethodBase)typeof(Console).Assembly.GetType("System.ConsolePal").GetNestedType("WindowsConsoleStream", BindingFlags.NonPublic).GetMethod("WriteFileNative", BindingFlags.Static | BindingFlags.NonPublic), (Delegate)(hook_WriteFileNative)delegate(orig_WriteFileNative orig, IntPtr hFile, ReadOnlySpan<byte> bytes, bool useFileAPIs)
			{
				int num = orig(hFile, bytes, useFileAPIs);
				return (num != 233) ? num : 0;
			});
		}
	}

	private static void HookProcessStart()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		processStartHook = new Hook((MethodBase)typeof(Process).GetMethod("Start", BindingFlags.Instance | BindingFlags.Public), (Delegate)(Func<Func<Process, bool>, Process, bool>)delegate(Func<Process, bool> orig, Process self)
		{
			Logging.tML.Debug((object)$"Process.Start (UseShellExecute = {self.StartInfo.UseShellExecute}): \"{self.StartInfo.FileName}\" {self.StartInfo.Arguments}");
			return orig(self);
		});
	}

	private static void Hook_StackTrace_CaptureStackTrace(orig_StackTrace_CaptureStackTrace orig, StackTrace self, int skipFrames, bool fNeedFileInfo, Exception e)
	{
		int skipHookFrames = ((e == null) ? 3 : 0);
		orig(self, skipFrames + skipHookFrames, fNeedFileInfo, e);
		if (fNeedFileInfo)
		{
			Logging.PrettifyStackTraceSources(self.GetFrames());
		}
	}

	private static void PrettifyStackTraceSources()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		stackTrace_CaptureStackTrace = new Hook((MethodBase)typeof(StackTrace).GetMethod("CaptureStackTrace", BindingFlags.Instance | BindingFlags.NonPublic), (Delegate)new hook_StackTrace_CaptureStackTrace(Hook_StackTrace_CaptureStackTrace));
	}

	/// <summary>
	/// Attempt to hook the .NET internal methods to log when requests are sent to web addresses.
	/// Use the right internal methods to capture redirects
	/// </summary>
	private static void HookWebRequests()
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		try
		{
			MethodInfo sendAsyncCoreMethodInfo = typeof(HttpClient).Assembly.GetType("System.Net.Http.HttpConnectionPoolManager")?.GetMethod("SendAsyncCore", BindingFlags.Instance | BindingFlags.Public);
			if (sendAsyncCoreMethodInfo != null)
			{
				httpSendAsyncHook = new Hook((MethodBase)sendAsyncCoreMethodInfo, (Delegate)(hook_SendAsyncCore)delegate(orig_SendAsyncCore orig, object self, HttpRequestMessage request, Uri? proxyUri, bool async, bool doRequestAuth, bool isProxyConnect, CancellationToken cancellationToken)
				{
					if (IncludeURIInRequestLogging(request.RequestUri))
					{
						Logging.tML.Debug((object)$"Web Request: {request.RequestUri}");
					}
					return orig(self, request, proxyUri, async, doRequestAuth, isProxyConnect, cancellationToken);
				});
				return;
			}
		}
		catch
		{
		}
		Logging.tML.Warn((object)"HttpWebRequest send/submit method not found");
	}

	private static bool IncludeURIInRequestLogging(Uri uri)
	{
		if (uri.IsLoopback && uri.LocalPath.Contains("game_"))
		{
			return false;
		}
		return true;
	}
}
