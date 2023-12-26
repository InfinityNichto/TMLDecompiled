using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ReLogic.IO;
using ReLogic.OS;
using SDL2;
using Terraria.Initializers;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.Engine;
using Terraria.Social;
using Terraria.Utilities;

namespace Terraria;

public static class Program
{
	public static bool IsXna = false;

	public static bool IsFna = true;

	public static bool IsMono = Type.GetType("Mono.Runtime") != null;

	public static Dictionary<string, string> LaunchParameters = new Dictionary<string, string>();

	public static string SavePath;

	public const string TerrariaSaveFolderPath = "Terraria";

	private static int ThingsToLoad;

	private static int ThingsLoaded;

	public static bool LoadedEverything;

	public static IntPtr JitForcedMethodCache;

	public const string PreviewFolder = "tModLoader-preview";

	public const string ReleaseFolder = "tModLoader";

	public const string DevFolder = "tModLoader-dev";

	public const string Legacy143Folder = "tModLoader-1.4.3";

	private const int HighDpiThreshold = 96;

	public static Thread MainThread { get; private set; }

	public static bool IsMainThread => Thread.CurrentThread == MainThread;

	public static float LoadedPercentage
	{
		get
		{
			if (ThingsToLoad == 0)
			{
				return 1f;
			}
			return (float)ThingsLoaded / (float)ThingsToLoad;
		}
	}

	public static string SavePathShared { get; private set; }

	public static string SaveFolderName
	{
		get
		{
			if (!BuildInfo.IsStable)
			{
				if (!BuildInfo.IsPreview)
				{
					return "tModLoader-dev";
				}
				return "tModLoader-preview";
			}
			return "tModLoader";
		}
	}

	public static void StartForceLoad()
	{
		if (!Main.SkipAssemblyLoad)
		{
			Thread thread = new Thread(ForceLoadThread);
			thread.IsBackground = true;
			thread.Start();
		}
		else
		{
			LoadedEverything = true;
		}
	}

	public static void ForceLoadThread(object threadContext)
	{
		ForceLoadAssembly(Assembly.GetExecutingAssembly(), initializeStaticMembers: true);
		LoadedEverything = true;
	}

	private static void ForceJITOnAssembly(IEnumerable<Type> types)
	{
		IEnumerable<MethodInfo> methodsToJIT = CollectMethodsToJIT(types);
		if (Environment.ProcessorCount > 1)
		{
			methodsToJIT.AsParallel().AsUnordered().ForAll(ForceJITOnMethod);
			return;
		}
		foreach (MethodInfo item in methodsToJIT)
		{
			ForceJITOnMethod(item);
		}
	}

	private static void ForceStaticInitializers(Assembly assembly)
	{
		Type[] types = assembly.GetTypes();
		foreach (Type type in types)
		{
			if (!type.IsGenericType)
			{
				RuntimeHelpers.RunClassConstructor(type.TypeHandle);
			}
		}
	}

	private static void ForceLoadAssembly(Assembly assembly, bool initializeStaticMembers)
	{
		Type[] types = assembly.GetTypes();
		ThingsToLoad = types.Select((Type type) => GetAllMethods(type).Count()).Sum();
		ForceJITOnAssembly(types);
		if (initializeStaticMembers)
		{
			ForceStaticInitializers(assembly);
		}
	}

	private static void ForceLoadAssembly(string name, bool initializeStaticMembers)
	{
		Assembly assembly = null;
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		for (int i = 0; i < assemblies.Length; i++)
		{
			if (assemblies[i].GetName().Name.Equals(name))
			{
				assembly = assemblies[i];
				break;
			}
		}
		if (assembly == null)
		{
			assembly = Assembly.Load(name);
		}
		ForceLoadAssembly(assembly, initializeStaticMembers);
	}

	private static void SetupLogging()
	{
		if (LaunchParameters.ContainsKey("-logfile"))
		{
			string text = LaunchParameters["-logfile"];
			text = ((text != null && !(text.Trim() == "")) ? Path.Combine(text, $"Log_{DateTime.Now:yyyyMMddHHmmssfff}.log") : Path.Combine(SavePath, "Logs", $"Log_{DateTime.Now:yyyyMMddHHmmssfff}.log"));
			ConsoleOutputMirror.ToFile(text);
		}
		CrashWatcher.Inititialize();
		CrashWatcher.DumpOnException = LaunchParameters.ContainsKey("-minidump");
		CrashWatcher.LogAllExceptions = LaunchParameters.ContainsKey("-logerrors");
		if (LaunchParameters.ContainsKey("-fulldump"))
		{
			Console.WriteLine("Full Dump logs enabled.");
			CrashWatcher.EnableCrashDumps(CrashDump.Options.WithFullMemory);
		}
	}

	private static void InitializeConsoleOutput()
	{
		if (Debugger.IsAttached)
		{
			return;
		}
		if (!Main.dedServ && !LaunchParameters.ContainsKey("-console"))
		{
			Platform.Get<IWindowService>().HideConsole();
		}
		try
		{
			Console.OutputEncoding = Encoding.UTF8;
			if (Platform.IsWindows)
			{
				Console.InputEncoding = Encoding.Unicode;
			}
			else
			{
				Console.InputEncoding = Encoding.UTF8;
			}
		}
		catch
		{
		}
	}

	public static void LaunchGame(string[] args, bool monoArgs = false)
	{
		MainThread = Thread.CurrentThread;
		MainThread.Name = "Main Thread";
		ProcessLaunchArgs(args, monoArgs, out var isServer);
		StartupSequenceTml(isServer);
		LaunchGame_(isServer);
	}

	public static void LaunchGame_(bool isServer)
	{
		Main.dedServ = isServer;
		if (isServer)
		{
			Main.myPlayer = 255;
		}
		string build = LaunchInitializer.TryParameter("-build");
		if (build != null)
		{
			ModCompile.BuildModCommandLine(build);
		}
		if (Main.dedServ)
		{
			Environment.SetEnvironmentVariable("FNA_PLATFORM_BACKEND", "NONE");
		}
		ThreadPool.SetMinThreads(8, 8);
		InitializeConsoleOutput();
		Platform.Get<IWindowService>().SetQuickEditEnabled(enabled: false);
		RunGame();
	}

	public static void RunGame()
	{
		LanguageManager.Instance.SetLanguage(GameCulture.DefaultCulture);
		if (Platform.IsOSX)
		{
			Main.OnEngineLoad += delegate
			{
				((Game)Main.instance).IsMouseVisible = false;
			};
		}
		InstallVerifier.Startup();
		try
		{
			Terraria.ModLoader.ModLoader.EngineInit();
			Main main = new Main();
			try
			{
				Lang.InitializeLegacyLocalization();
				SocialAPI.Initialize();
				LaunchInitializer.LoadParameters(main);
				Main.OnEnginePreload += StartForceLoad;
				if (Main.dedServ)
				{
					main.DedServ();
				}
				else
				{
					((Game)main).Run();
				}
			}
			finally
			{
				((IDisposable)main)?.Dispose();
			}
		}
		catch (Exception e)
		{
			ErrorReporting.FatalExit("Main engine crash", e);
		}
	}

	private static IEnumerable<MethodInfo> GetAllMethods(Type type)
	{
		return type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
	}

	private static IEnumerable<MethodInfo> CollectMethodsToJIT(IEnumerable<Type> types)
	{
		return from type in types
			from method in GetAllMethods(type)
			where !method.IsAbstract && !method.ContainsGenericParameters && method.GetMethodBody() != null
			select method;
	}

	private static void ForceJITOnMethod(MethodInfo method)
	{
		RuntimeHelpers.PrepareMethod(method.MethodHandle);
		Interlocked.Increment(ref ThingsLoaded);
	}

	private static void ForceStaticInitializers(Type[] types)
	{
		foreach (Type type in types)
		{
			if (!type.IsGenericType)
			{
				RuntimeHelpers.RunClassConstructor(type.TypeHandle);
			}
		}
	}

	private static void PortOldSaveDirectories(string savePath)
	{
		string oldBetas = Path.Combine(savePath, "ModLoader", "Beta");
		if (!Directory.Exists(oldBetas))
		{
			return;
		}
		Logging.tML.Info((object)("Old tModLoader alpha folder \"" + oldBetas + "\" found, attempting folder migration"));
		string newPath = Path.Combine(savePath, "tModLoader");
		if (Directory.Exists(newPath))
		{
			Logging.tML.Warn((object)$"Both \"{oldBetas}\" and \"{newPath}\" exist, assuming user launched old tModLoader alpha, aborting migration");
			return;
		}
		Logging.tML.Info((object)$"Migrating from \"{oldBetas}\" to \"{newPath}\"");
		Directory.Move(oldBetas, newPath);
		Logging.tML.Info((object)"Old alpha folder to new location migration success");
		string[] array = new string[3] { "Mod Reader", "Mod Sources", "Mod Configs" };
		foreach (string subDir in array)
		{
			string newSaveOriginalSubDirPath = Path.Combine(newPath, subDir);
			if (Directory.Exists(newSaveOriginalSubDirPath))
			{
				string newSaveNewSubDirPath = Path.Combine(newPath, subDir.Replace(" ", ""));
				Logging.tML.Info((object)$"Renaming from \"{newSaveOriginalSubDirPath}\" to \"{newSaveNewSubDirPath}\"");
				Directory.Move(newSaveOriginalSubDirPath, newSaveNewSubDirPath);
			}
		}
		Logging.tML.Info((object)"Folder Renames Success");
	}

	private static void PortCommonFilesToStagingBranches(string savePath)
	{
		if (BuildInfo.IsStable)
		{
			return;
		}
		string releasePath = Path.Combine(savePath, "tModLoader");
		string newPath = Path.Combine(savePath, SaveFolderName);
		if (Directory.Exists(releasePath) && !Directory.Exists(newPath))
		{
			Directory.CreateDirectory(newPath);
			Logging.tML.Info((object)"Cloning common files from Stable to preview and dev.");
			if (File.Exists(Path.Combine(releasePath, "config.json")))
			{
				File.Copy(Path.Combine(releasePath, "config.json"), Path.Combine(newPath, "config.json"));
			}
			if (File.Exists(Path.Combine(releasePath, "input profiles.json")))
			{
				File.Copy(Path.Combine(releasePath, "input profiles.json"), Path.Combine(newPath, "input profiles.json"));
			}
		}
	}

	/// <summary>
	/// Super Save Path is the parent directory containing both folders. Usually Program.SavePath or Steam Cloud
	/// Source is of variety StableFolder, PreviewFolder... etc
	/// Destination is of variety StableFolder, PreviewFolder... etc
	/// maxVersionOfSource is used to determine if we even should port the files. Example: 1.4.3-Legacy has maxVersion of 2022.9
	/// isAtomicLockable could be expressed as CopyToNewlyCreatedDestinationFolderViaTempFolder if that makes more sense to the reader.
	/// </summary>
	private static void PortFilesFromXtoY(string superSavePath, string source, string destination, string maxVersionOfSource, bool isCloud, bool isAtomicLockable)
	{
		string newFolderPath = Path.Combine(superSavePath, destination);
		string newFolderPathTemp = Path.Combine(superSavePath, destination + "-temp");
		string oldFolderPath = Path.Combine(superSavePath, source);
		string cloudName = (isCloud ? "Steam Cloud" : "Local Files");
		if (!Directory.Exists(oldFolderPath))
		{
			return;
		}
		string portFilePath = Path.Combine(superSavePath, destination, (maxVersionOfSource == "2022.9") ? ("143ported_" + cloudName + ".txt") : $"{maxVersionOfSource}{destination}ported_{cloudName}.txt");
		if ((isAtomicLockable && Directory.Exists(newFolderPath)) || (!isAtomicLockable && File.Exists(portFilePath)))
		{
			return;
		}
		if (newFolderPath.Contains("OneDrive"))
		{
			Logging.tML.Info((object)"Ensuring OneDrive is running before starting to Migrate Files");
			try
			{
				string oneDrivePath1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft\\OneDrive\\OneDrive.exe");
				string oneDrivePath2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Microsoft OneDrive\\OneDrive.exe");
				if (File.Exists(oneDrivePath1))
				{
					Process.Start(oneDrivePath1);
				}
				else if (File.Exists(oneDrivePath2))
				{
					Process.Start(oneDrivePath2);
				}
				Thread.Sleep(3000);
			}
			catch
			{
			}
		}
		string sourceFolderConfig = Path.Combine(LaunchParameters.ContainsKey("-savedirectory") ? LaunchParameters["-savedirectory"] : Platform.Get<IPathService>().GetStoragePath("Terraria"), source, "config.json");
		if (!File.Exists(sourceFolderConfig))
		{
			Logging.tML.Info((object)("No config.json found at " + sourceFolderConfig + "\nAssuming nothing to port"));
			return;
		}
		string lastLaunchedTml = null;
		try
		{
			if (JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(sourceFolderConfig)).TryGetValue("LastLaunchedTModLoaderVersion", out var lastLaunchedTmlObject))
			{
				lastLaunchedTml = (string)lastLaunchedTmlObject;
			}
		}
		catch (Exception e2)
		{
			e2.HelpLink = "https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Usage-FAQ#configjson-corrupted";
			ErrorReporting.FatalExit($"Attempt to Port from \"{oldFolderPath}\" to \"{newFolderPath}\" aborted, the \"{sourceFolderConfig}\" file is corrupted.", e2);
		}
		if (string.IsNullOrEmpty(lastLaunchedTml))
		{
			ErrorReporting.FatalExit($"Attempt to Port from \"{oldFolderPath}\" to \"{newFolderPath}\" aborted, the \"{sourceFolderConfig}\" file is missing the \"LastLaunchedTModLoaderVersion\" entry. If porting is desired, follow the instructions at \"https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Usage-FAQ#manually-port\"");
			return;
		}
		if (new Version(lastLaunchedTml).MajorMinor() > new Version(maxVersionOfSource))
		{
			Logging.tML.Info((object)$"Attempt to Port from \"{oldFolderPath}\" to \"{newFolderPath}\" aborted, \"{lastLaunchedTml}\" is a newer version.");
			return;
		}
		Logging.tML.Info((object)($"Cloning current {source} files to {destination} save folder. Porting {cloudName}." + "\nThis may take a few minutes for a large amount of files."));
		try
		{
			FileUtilities.CopyFolderEXT(oldFolderPath, isAtomicLockable ? newFolderPathTemp : newFolderPath, isCloud, new Regex("(Workshop|ModSources)($|/|\\\\)"), overwriteAlways: false, overwriteOld: true);
		}
		catch (Exception e)
		{
			e.HelpLink = "https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Usage-FAQ#migration-failed";
			ErrorReporting.FatalExit("Migration Failed, please consult the instructions in the \"Migration Failed\" section at \"" + e.HelpLink + "\" for more information.", e);
		}
		if (isAtomicLockable)
		{
			Directory.Move(newFolderPathTemp, newFolderPath);
		}
		else if (isCloud)
		{
			if (SocialAPI.Cloud != null)
			{
				SocialAPI.Cloud.Write(FileUtilities.ConvertToRelativePath(superSavePath, portFilePath), new byte[0]);
			}
		}
		else
		{
			File.Create(portFilePath);
		}
		Logging.tML.Info((object)("Porting " + cloudName + " finished"));
	}

	internal static void PortFilesMaster(string savePath, bool isCloud)
	{
		PortOldSaveDirectories(savePath);
		PortCommonFilesToStagingBranches(savePath);
		PortFilesFromXtoY(savePath, "tModLoader", "tModLoader-1.4.3", "2022.9", isCloud, !isCloud);
		if (BuildInfo.IsStable)
		{
			PortFilesFromXtoY(savePath, "tModLoader-preview", "tModLoader", "2023.6", isCloud, isAtomicLockable: false);
		}
	}

	private static void SetSavePath()
	{
		if (LaunchParameters.TryGetValue("-tmlsavedirectory", out var customSavePath))
		{
			SavePathShared = customSavePath;
			SavePath = customSavePath;
		}
		else if (File.Exists("savehere.txt"))
		{
			SavePathShared = "tModLoader";
			SavePath = SaveFolderName;
		}
		else
		{
			SavePathShared = Path.Combine(SavePath, "tModLoader");
			string savePathCopy = SavePath;
			SavePath = Path.Combine(SavePath, SaveFolderName);
			try
			{
				PortFilesMaster(savePathCopy, isCloud: false);
			}
			catch (Exception e)
			{
				ErrorReporting.FatalExit("An error occured migrating files and folders to the new structure", e);
			}
		}
		Logging.tML.Info((object)("Saves Are Located At: " + Path.GetFullPath(SavePath)));
		if (ControlledFolderAccessSupport.ControlledFolderAccessDetectionPrevented)
		{
			Logging.tML.Info((object)"Controlled Folder Access detection failed, something is preventing the game from accessing the registry.");
		}
		if (ControlledFolderAccessSupport.ControlledFolderAccessDetected)
		{
			Logging.tML.Info((object)("Controlled Folder Access feature detected. If game fails to launch make sure to add \"" + Environment.ProcessPath + "\" to the \"Allow an app through Controlled folder access\" menu found in the \"Ransomware protection\" menu."));
		}
	}

	private static void StartupSequenceTml(bool isServer)
	{
		try
		{
			ControlledFolderAccessSupport.CheckFileSystemAccess();
			Logging.Init(isServer ? Logging.LogFile.Server : Logging.LogFile.Client);
			if (Platform.Current.Type == PlatformType.Windows && RuntimeInformation.ProcessArchitecture != Architecture.X64)
			{
				ErrorReporting.FatalExit("The current Windows Architecture of your System is CURRENTLY unsupported. Aborting...");
			}
			Logging.LogStartup(isServer);
			SetSavePath();
			if (ModCompile.DeveloperMode)
			{
				Logging.tML.Info((object)"Developer mode enabled");
			}
			AttemptSupportHighDPI(isServer);
			if (!isServer)
			{
				NativeLibraries.CheckNativeFAudioDependencies();
				FNALogging.RedirectLogs();
			}
		}
		catch (Exception ex)
		{
			ErrorReporting.FatalExit("An unexpected error occurred during tML startup", ex);
		}
	}

	private static void ProcessLaunchArgs(string[] args, bool monoArgs, out bool isServer)
	{
		isServer = false;
		try
		{
			if (monoArgs)
			{
				args = Utils.ConvertMonoArgsToDotNet(args);
			}
			LaunchParameters = Utils.ParseArguements(args);
			if (LaunchParameters.ContainsKey("-terrariasteamclient"))
			{
				TerrariaSteamClient.Run();
				Environment.Exit(1);
			}
			SavePath = (LaunchParameters.ContainsKey("-savedirectory") ? LaunchParameters["-savedirectory"] : Platform.Get<IPathService>().GetStoragePath("Terraria"));
			isServer = LaunchParameters.ContainsKey("-server");
		}
		catch (Exception e)
		{
			ErrorReporting.FatalExit("Unhandled Issue with Launch Arguments. Please verify sources such as Steam Launch Options, cli-ArgsConfig, and VS profiles", e);
		}
	}

	private static void AttemptSupportHighDPI(bool isServer)
	{
		if (!isServer)
		{
			if (Platform.IsWindows)
			{
				SetProcessDPIAware();
			}
			SDL.SDL_VideoInit((string)null);
			float ddpi = default(float);
			float hdpi = default(float);
			float vdpi = default(float);
			SDL.SDL_GetDisplayDPI(0, ref ddpi, ref hdpi, ref vdpi);
			Logging.tML.Info((object)$"Display DPI: Diagonal DPI is {ddpi}. Vertical DPI is {vdpi}. Horizontal DPI is {hdpi}");
			if (ddpi >= 96f || hdpi >= 96f || vdpi >= 96f)
			{
				Environment.SetEnvironmentVariable("FNA_GRAPHICS_ENABLE_HIGHDPI", "1");
				Logging.tML.Info((object)"High DPI Display detected: setting FNA to highdpi mode");
			}
		}
		[DllImport("user32.dll", EntryPoint = "SetProcessDPIAware")]
		static extern bool SetProcessDPIAware();
	}
}
