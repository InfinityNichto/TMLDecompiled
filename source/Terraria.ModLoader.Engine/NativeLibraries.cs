using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Terraria.ModLoader.Engine;

internal class NativeLibraries
{
	private const string WindowsVersionNDesc = "Windows Versions N and KN are missing some media features.\n\nFollow the instructions in the Microsoft website\n\nSearch \"Media Feature Pack list for Windows N editions\" if the page doesn't open automatically.";

	private const string WindowsVersionNUrl = "https://support.microsoft.com/en-us/topic/media-feature-pack-list-for-windows-n-editions-c1c6fffa-d052-8338-7a79-a4bb980a700a";

	private const string FailedDependency = "Unexpected failure in verifying dependency. Please reach out in the tModLoader Discord for support";

	private const int LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 4096;

	internal static void CheckNativeFAudioDependencies()
	{
		if (!OperatingSystem.IsWindows())
		{
			return;
		}
		try
		{
			NativeLibrary.Load("vcruntime140.dll", Assembly.GetExecutingAssembly(), DllImportSearchPath.System32);
		}
		catch (DllNotFoundException e4)
		{
			e4.HelpLink = "https://www.microsoft.com/en-us/download/details.aspx?id=53587";
			ErrorReporting.FatalExit("Microsoft Visual C++ 2015 Redistributable Update 3 is missing. You will need to download and install it from the Microsoft website.", e4);
		}
		catch (Exception e3)
		{
			ErrorReporting.FatalExit("vcruntime140.dll: Unexpected failure in verifying dependency. Please reach out in the tModLoader Discord for support", e3);
		}
		try
		{
			NativeLibrary.Load("mfplat.dll", Assembly.GetExecutingAssembly(), DllImportSearchPath.System32);
		}
		catch (DllNotFoundException e2)
		{
			e2.HelpLink = "https://support.microsoft.com/en-us/topic/media-feature-pack-list-for-windows-n-editions-c1c6fffa-d052-8338-7a79-a4bb980a700a";
			ErrorReporting.FatalExit("Windows Versions N and KN are missing some media features.\n\nFollow the instructions in the Microsoft website\n\nSearch \"Media Feature Pack list for Windows N editions\" if the page doesn't open automatically.", e2);
		}
		catch (BadImageFormatException ex)
		{
			ex.HelpLink = "https://support.microsoft.com/en-us/topic/media-feature-pack-list-for-windows-n-editions-c1c6fffa-d052-8338-7a79-a4bb980a700a";
			ErrorReporting.FatalExit("https://support.microsoft.com/en-us/topic/media-feature-pack-list-for-windows-n-editions-c1c6fffa-d052-8338-7a79-a4bb980a700a\n\nIf this doesn't work try Search \"MFPlat.DLL is either not designed to run on Windows\" and follow those instructions");
		}
		catch (Exception e)
		{
			ErrorReporting.FatalExit("mfplat.dll: Unexpected failure in verifying dependency. Please reach out in the tModLoader Discord for support", e);
		}
	}

	internal static void SetNativeLibraryPath(string nativesDir)
	{
		if (!OperatingSystem.IsWindows())
		{
			return;
		}
		try
		{
			SetDefaultDllDirectories(4096);
			AddDllDirectory(nativesDir);
		}
		catch
		{
			SetDllDirectory(nativesDir);
		}
	}

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool SetDefaultDllDirectories(int directoryFlags);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	private static extern void AddDllDirectory(string lpPathName);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool SetDllDirectory(string lpPathName);
}
