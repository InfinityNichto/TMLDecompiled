using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Threading;
using Terraria.ModLoader;
using Terraria.ModLoader.Engine;
using tModPorter;

namespace Terraria;

internal static class MonoLaunch
{
	private static readonly Dictionary<string, IntPtr> assemblies = new Dictionary<string, IntPtr>();

	public static readonly string NativesDir = Path.Combine(Environment.CurrentDirectory, "Libraries", "Native", NativePlatformDir);

	public static readonly object resolverLock = new object();

	private static string NativePlatformDir
	{
		get
		{
			if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
				{
					if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
					{
						throw new InvalidOperationException("Unknown OS.");
					}
					return "OSX";
				}
				return "Linux";
			}
			return "Windows";
		}
	}

	private static void Main(string[] args)
	{
		int i = Array.IndexOf(args, "-tModPorter");
		if (i >= 0)
		{
			Program.Main(args.Skip(i + 1).ToArray()).GetAwaiter().GetResult();
			return;
		}
		Thread.CurrentThread.Name = "Entry Thread";
		NativeLibraries.SetNativeLibraryPath(NativesDir);
		AssemblyLoadContext.Default.ResolvingUnmanagedDll += ResolveNativeLibrary;
		Environment.SetEnvironmentVariable("FNA_WORKAROUND_WINDOW_RESIZABLE", "1");
		if (File.Exists("cli-argsConfig.txt"))
		{
			args = args.Concat(File.ReadAllLines("cli-argsConfig.txt").SelectMany((string a) => a.Split(" ", 2))).ToArray();
		}
		if (File.Exists("env-argsConfig.txt"))
		{
			foreach (string[] environmentVar in from text in File.ReadAllLines("env-argsConfig.txt")
				select text.Split("=") into envVar
				where envVar.Length == 2
				select envVar)
			{
				Environment.SetEnvironmentVariable(environmentVar[0], environmentVar[1]);
			}
		}
		Action LocalLaunchGame = delegate
		{
			Main_End(args);
		};
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			new Thread(LocalLaunchGame.Invoke).Start();
			Thread.CurrentThread.IsBackground = true;
		}
		else
		{
			LocalLaunchGame();
		}
	}

	private static void Main_End(string[] args)
	{
		Program.LaunchGame(args, monoArgs: true);
	}

	private static IntPtr ResolveNativeLibrary(Assembly assembly, string name)
	{
		lock (resolverLock)
		{
			if (assemblies.TryGetValue(name, out var handle))
			{
				return handle;
			}
			Logging.tML.Debug((object)("Native Resolve: " + assembly.FullName + " -> " + name));
			string path = Directory.GetFiles(NativesDir, "*" + name + "*", SearchOption.AllDirectories).FirstOrDefault();
			if (path == null)
			{
				Logging.tML.Debug((object)"\tnot found");
				return IntPtr.Zero;
			}
			Logging.tML.Debug((object)("\tattempting load " + path));
			handle = NativeLibrary.Load(path);
			Logging.tML.Debug((object)"\tsuccess");
			return assemblies[name] = handle;
		}
	}
}
