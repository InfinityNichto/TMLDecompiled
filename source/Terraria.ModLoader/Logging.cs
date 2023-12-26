using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using Microsoft.Xna.Framework;
using ReLogic.OS;
using Terraria.Localization;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.Engine;

namespace Terraria.ModLoader;

public static class Logging
{
	public enum LogFile
	{
		Client,
		Server,
		TerrariaSteamClient
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public readonly ref struct QuietExceptionHandle
	{
		public QuietExceptionHandle()
		{
			quietExceptionCount++;
		}

		public void Dispose()
		{
			quietExceptionCount--;
		}
	}

	public static readonly string LogDir = "tModLoader-Logs";

	public static readonly string LogArchiveDir = Path.Combine(LogDir, "Old");

	private static readonly Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

	private static readonly List<string> initWarnings = new List<string>();

	private static readonly Regex statusRegex = new Regex("(.+?)[: \\d]*%$");

	[ThreadStatic]
	private static int quietExceptionCount;

	private static readonly HashSet<string> pastExceptions = new HashSet<string>();

	private static readonly HashSet<string> ignoreTypes = new HashSet<string> { "ReLogic.Peripherals.RGB.DeviceInitializationException", "System.Threading.Tasks.TaskCanceledException" };

	private static readonly HashSet<string> ignoreSources = new HashSet<string> { "MP3Sharp" };

	private static readonly List<string> ignoreContents = new List<string>
	{
		"System.Console.set_OutputEncoding", "Terraria.ModLoader.Core.ModCompile", "Delegate.CreateDelegateNoSecurityCheck", "MethodBase.GetMethodBody", "System.Int32.Parse..Terraria.Main.DedServ_PostModLoad", "Convert.ToInt32..Terraria.Main.DedServ_PostModLoad", "Terraria.Net.Sockets.TcpSocket.Terraria.Net.Sockets.ISocket.AsyncSend", "System.Diagnostics.Process.Kill", "UwUPnP", "System.Threading.CancellationTokenSource.Cancel",
		"System.Net.Http.HttpConnectionPool.AddHttp11ConnectionAsync", "ReLogic.Peripherals.RGB.SteelSeries.GameSenseConnection._sendMsg"
	};

	private static readonly List<string> ignoreMessages = new List<string> { "A blocking operation was interrupted by a call to WSACancelBlockingCall", "The request was aborted: The request was canceled.", "Object name: 'System.Net.Sockets.Socket'.", "Object name: 'System.Net.Sockets.NetworkStream'", "This operation cannot be performed on a completed asynchronous result object.", "Object name: 'SslStream'.", "Unable to load DLL 'Microsoft.DiaSymReader.Native.x86.dll'" };

	private static readonly List<string> ignoreThrowingMethods = new List<string> { "System.Net.Sockets.Socket.AwaitableSocketAsyncEventArgs.ThrowException", "Terraria.Lighting.doColors_Mode", "System.Threading.CancellationToken.Throw" };

	private static Exception previousException;

	private const BindingFlags InstanceNonPublic = BindingFlags.Instance | BindingFlags.NonPublic;

	internal static readonly FieldInfo f_fileName = typeof(StackFrame).GetField("_fileName", BindingFlags.Instance | BindingFlags.NonPublic) ?? typeof(StackFrame).GetField("strFileName", BindingFlags.Instance | BindingFlags.NonPublic);

	private static readonly Assembly terrariaAssembly = Assembly.GetExecutingAssembly();

	public static string LogPath { get; private set; }

	/// <summary> Available for logging when Mod.Logging is not available, such as field initialization </summary>
	public static ILog PublicLogger { get; } = LogManager.GetLogger("PublicLogger");


	internal static ILog Terraria { get; } = LogManager.GetLogger("Terraria");


	internal static ILog tML { get; } = LogManager.GetLogger("tML");


	internal static ILog FNA { get; } = LogManager.GetLogger("FNA");


	internal static void Init(LogFile logFile)
	{
		if (Program.LaunchParameters.ContainsKey("-build"))
		{
			return;
		}
		Utils.TryCreatingDirectory(LogDir);
		try
		{
			InitLogPaths(logFile);
			ConfigureAppenders(logFile);
		}
		catch (Exception e)
		{
			ErrorReporting.FatalExit("Failed to init logging", e);
		}
	}

	internal static void LogStartup(bool dedServ)
	{
		tML.InfoFormat("Starting tModLoader {0} {1} built {2}", (object)(dedServ ? "server" : "client"), (object)BuildInfo.BuildIdentifier, (object)$"{BuildInfo.BuildDate:g}");
		tML.InfoFormat("Log date: {0}", (object)DateTime.Now.ToString("d"));
		tML.InfoFormat("Running on {0} (v{1}) {2} {3} {4}", new object[5]
		{
			Platform.Current.Type,
			Environment.OSVersion.Version,
			RuntimeInformation.ProcessArchitecture,
			FrameworkVersion.Framework,
			FrameworkVersion.Version
		});
		tML.InfoFormat("FrameworkDescription: {0}", (object)RuntimeInformation.FrameworkDescription);
		tML.InfoFormat("Executable: {0}", (object)Assembly.GetEntryAssembly().Location);
		tML.InfoFormat("Working Directory: {0}", (object)Path.GetFullPath(Directory.GetCurrentDirectory()));
		string args2 = string.Join(' ', Environment.GetCommandLineArgs().Skip(1));
		if (!string.IsNullOrEmpty(args2) || Program.LaunchParameters.Any())
		{
			tML.InfoFormat("Launch Parameters: {0}", (object)args2);
			tML.InfoFormat("Parsed Launch Parameters: {0}", (object)string.Join(' ', Program.LaunchParameters.Select((KeyValuePair<string, string> p) => (p.Key + " " + p.Value).Trim())));
		}
		DumpEnvVars();
		string stackLimit = Environment.GetEnvironmentVariable("COMPlus_DefaultStackSize");
		if (!string.IsNullOrEmpty(stackLimit))
		{
			tML.InfoFormat("Override Default Thread Stack Size Limit: {0}", (object)stackLimit);
		}
		foreach (string line in initWarnings)
		{
			tML.Warn((object)line);
		}
		AppDomain.CurrentDomain.UnhandledException += delegate(object s, UnhandledExceptionEventArgs args)
		{
			tML.Error((object)"Unhandled Exception", args.ExceptionObject as Exception);
		};
		LogFirstChanceExceptions();
		AssemblyResolving.Init();
		LoggingHooks.Init();
		LogArchiver.ArchiveLogs();
	}

	private static void ConfigureAppenders(LogFile logFile)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		PatternLayout layout = new PatternLayout
		{
			ConversionPattern = "[%d{HH:mm:ss.fff}] [%t/%level] [%logger]: %m%n"
		};
		((LayoutSkeleton)layout).ActivateOptions();
		List<IAppender> appenders = new List<IAppender>();
		if (logFile == LogFile.Client)
		{
			appenders.Add((IAppender)new ConsoleAppender
			{
				Name = "ConsoleAppender",
				Layout = (ILayout)(object)layout
			});
		}
		appenders.Add((IAppender)new DebugAppender
		{
			Name = "DebugAppender",
			Layout = (ILayout)(object)layout
		});
		FileAppender fileAppender = new FileAppender
		{
			Name = "FileAppender",
			File = LogPath,
			AppendToFile = false,
			Encoding = encoding,
			Layout = (ILayout)(object)layout
		};
		((AppenderSkeleton)fileAppender).ActivateOptions();
		appenders.Add((IAppender)(object)fileAppender);
		BasicConfigurator.Configure(appenders.ToArray());
	}

	private static void InitLogPaths(LogFile logFile)
	{
		string mainLogName = logFile.ToString().ToLowerInvariant();
		List<string> baseLogNames = new List<string> { mainLogName };
		if (logFile != LogFile.TerrariaSteamClient)
		{
			baseLogNames.Add("environment-" + mainLogName);
		}
		if (logFile == LogFile.Client)
		{
			baseLogNames.Add(LogFile.TerrariaSteamClient.ToString().ToLowerInvariant());
		}
		string logFileName = GetFreeLogFileName(baseLogNames, logFile != LogFile.TerrariaSteamClient);
		LogPath = Path.Combine(LogDir, logFileName);
	}

	private static string GetFreeLogFileName(List<string> baseLogNames, bool roll)
	{
		string baseLogName = baseLogNames[0];
		Regex pattern = new Regex("(?:" + string.Join('|', baseLogNames) + ")(\\d*)\\.log$");
		List<string> existingLogs = (from s in Directory.GetFiles(LogDir)
			where pattern.IsMatch(Path.GetFileName(s))
			select s).ToList();
		if (!existingLogs.All(CanOpen))
		{
			int i = existingLogs.Select(delegate(string s)
			{
				string value = pattern.Match(Path.GetFileName(s)).Groups[1].Value;
				return (value.Length == 0) ? 1 : int.Parse(value);
			}).Max();
			return $"{baseLogName}{i + 1}.log";
		}
		if (roll)
		{
			RenameToOld(existingLogs);
		}
		else if (existingLogs.Any())
		{
			IEnumerable<string> logNames = existingLogs.Select((string s) => Path.GetFileName(s));
			initWarnings.Add($"Old log files found which should have already been archived. The {baseLogName}.log will be overwritten. [{string.Join(", ", logNames)}]");
		}
		return baseLogName + ".log";
	}

	private static void RenameToOld(List<string> existingLogs)
	{
		foreach (string existingLog in existingLogs.OrderBy(File.GetCreationTime))
		{
			string oldExt = ".old";
			int i = 0;
			while (File.Exists(existingLog + oldExt))
			{
				oldExt = $".old{++i}";
			}
			try
			{
				File.Move(existingLog, existingLog + oldExt);
			}
			catch (IOException e)
			{
				initWarnings.Add($"Move failed during log initialization: {existingLog} -> {Path.GetFileName(existingLog)}{oldExt}\n{e}");
			}
		}
	}

	private static bool CanOpen(string fileName)
	{
		try
		{
			using (new FileStream(fileName, FileMode.Append))
			{
			}
			return true;
		}
		catch (IOException)
		{
			return false;
		}
	}

	private static void AddChatMessage(string msg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		AddChatMessage(msg, Color.OrangeRed);
	}

	private static void AddChatMessage(string msg, Color color)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.gameMenu)
		{
			float soundVolume = Main.soundVolume;
			Main.soundVolume = 0f;
			Main.NewText(msg, color);
			Main.soundVolume = soundVolume;
		}
	}

	internal static void LogStatusChange(string oldStatusText, string newStatusText)
	{
		string oldBase = statusRegex.Match(oldStatusText).Groups[1].Value;
		string newBase = statusRegex.Match(newStatusText).Groups[1].Value;
		if (newBase != oldBase && newBase.Length > 0)
		{
			LogManager.GetLogger("StatusText").Info((object)newBase);
		}
	}

	internal static void ServerConsoleLine(string msg)
	{
		ServerConsoleLine(msg, Level.Info);
	}

	internal static void ServerConsoleLine(string msg, Level level, Exception ex = null, ILog log = null)
	{
		if (level == Level.Warn)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
		}
		else if (level == Level.Error)
		{
			Console.ForegroundColor = ConsoleColor.Red;
		}
		Console.WriteLine(msg);
		Console.ResetColor();
		((ILoggerWrapper)(log ?? Terraria)).Logger.Log((Type)null, level, (object)msg, ex);
	}

	private static void DumpEnvVars()
	{
		try
		{
			string fileName = "environment-" + Path.GetFileName(LogPath);
			using FileStream f = File.OpenWrite(Path.Combine(LogDir, fileName));
			using StreamWriter w = new StreamWriter(f);
			foreach (object key in Environment.GetEnvironmentVariables().Keys)
			{
				w.WriteLine($"{key}={Environment.GetEnvironmentVariable((string)key)}");
			}
		}
		catch (Exception e)
		{
			tML.Error((object)"Failed to dump env vars", e);
		}
	}

	public static void IgnoreExceptionSource(string source)
	{
		ignoreSources.Add(source);
	}

	public static void IgnoreExceptionContents(string source)
	{
		if (!ignoreContents.Contains(source))
		{
			ignoreContents.Add(source);
		}
	}

	internal static void ResetPastExceptions()
	{
		pastExceptions.Clear();
	}

	private static void LogFirstChanceExceptions()
	{
		AppDomain.CurrentDomain.FirstChanceException += FirstChanceExceptionHandler;
	}

	private static void FirstChanceExceptionHandler(object sender, FirstChanceExceptionEventArgs args)
	{
		if (quietExceptionCount > 0)
		{
			return;
		}
		using (new QuietExceptionHandle())
		{
			bool oom = args.Exception is OutOfMemoryException;
			if (oom)
			{
				TryFreeingMemory();
			}
			try
			{
				if (!oom && (args.Exception == previousException || args.Exception is ThreadAbortException || ignoreTypes.Contains(args.Exception.GetType().FullName) || ignoreSources.Contains(args.Exception.Source) || ignoreMessages.Any((string str) => args.Exception.Message?.Contains(str) ?? false) || ignoreThrowingMethods.Any((string str) => args.Exception.StackTrace?.Contains(str) ?? false)))
				{
					return;
				}
				StackTrace stackTrace = new StackTrace(1, fNeedFileInfo: true);
				string traceString = stackTrace.ToString();
				if (!oom && ignoreContents.Any((string s) => MatchContents(traceString, s)))
				{
					return;
				}
				traceString = stackTrace.ToString();
				string text = traceString;
				int num = traceString.IndexOf('\n');
				traceString = text.Substring(num, text.Length - num);
				string exString = args.Exception.GetType()?.ToString() + ": " + args.Exception.Message + traceString;
				lock (pastExceptions)
				{
					if (!pastExceptions.Add(exString))
					{
						return;
					}
				}
				tML.Warn((object)(Language.GetTextValue("tModLoader.RuntimeErrorSilentlyCaughtException") + "\n" + exString));
				previousException = args.Exception;
				string msg = args.Exception.Message + " " + Language.GetTextValue("tModLoader.RuntimeErrorSeeLogsForFullTrace", Path.GetFileName(LogPath));
				if (Program.SavePathShared == null || Main.dedServ)
				{
					Console.ForegroundColor = ConsoleColor.DarkMagenta;
					Console.WriteLine(msg);
					Console.ResetColor();
				}
				else if (Program.SavePathShared != null && ModCompile.activelyModding && !Main.gameMenu)
				{
					AddChatMessage(msg);
				}
				if (oom)
				{
					ErrorReporting.FatalExit(Language.GetTextValue("tModLoader.OutOfMemory"));
				}
			}
			catch (Exception e)
			{
				tML.Warn((object)"FirstChanceExceptionHandler exception", e);
			}
		}
	}

	private static bool MatchContents(ReadOnlySpan<char> traceString, ReadOnlySpan<char> contentPattern)
	{
		while (true)
		{
			int sep = contentPattern.IndexOf("..");
			ReadOnlySpan<char> readOnlySpan;
			ReadOnlySpan<char> readOnlySpan2;
			if (sep < 0)
			{
				readOnlySpan = contentPattern;
			}
			else
			{
				readOnlySpan2 = contentPattern;
				readOnlySpan = readOnlySpan2.Slice(0, sep);
			}
			ReadOnlySpan<char> i = readOnlySpan;
			int f = traceString.IndexOf(i);
			if (f < 0)
			{
				return false;
			}
			if (sep < 0)
			{
				break;
			}
			readOnlySpan2 = traceString;
			int num = f + i.Length;
			traceString = readOnlySpan2.Slice(num, readOnlySpan2.Length - num);
			readOnlySpan2 = contentPattern;
			num = sep + 2;
			contentPattern = readOnlySpan2.Slice(num, readOnlySpan2.Length - num);
		}
		return true;
	}

	public static void PrettifyStackTraceSources(StackFrame[] frames)
	{
		if (frames == null)
		{
			return;
		}
		foreach (StackFrame frame in frames)
		{
			string filename = frame.GetFileName();
			Assembly assembly = frame.GetMethod()?.DeclaringType?.Assembly;
			if (filename == null || assembly == null)
			{
				continue;
			}
			string trim;
			if (AssemblyManager.GetAssemblyOwner(assembly, out var modName))
			{
				trim = modName;
			}
			else
			{
				if (!(assembly == terrariaAssembly))
				{
					continue;
				}
				trim = "tModLoader";
			}
			int index = filename.LastIndexOf(trim, StringComparison.InvariantCultureIgnoreCase);
			if (index > 0)
			{
				filename = filename.Substring(index);
				f_fileName.SetValue(frame, filename);
			}
		}
	}

	private static void TryFreeingMemory()
	{
		Main.tile = new Tilemap(0, 0);
		GC.Collect();
	}
}
