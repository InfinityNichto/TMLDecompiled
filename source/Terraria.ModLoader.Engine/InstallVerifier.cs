using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using ReLogic.OS;
using Terraria.Localization;

namespace Terraria.ModLoader.Engine;

internal static class InstallVerifier
{
	private static string VanillaExe;

	private const string TerrariaVersion = "1.4.4.9";

	private static string CheckExe;

	private static string vanillaExePath;

	public static DistributionPlatform DistributionPlatform;

	private static string steamAPIPath;

	private static string vanillaSteamAPI;

	private static byte[] steamAPIHash;

	private static byte[] gogHash;

	private static byte[] steamHash;

	static InstallVerifier()
	{
		VanillaExe = "Terraria.exe";
		CheckExe = "Terraria_v1.4.4.9.exe";
		if (Platform.IsWindows)
		{
			if (IntPtr.Size == 4)
			{
				steamAPIPath = "Libraries/Native/Windows32/steam_api.dll";
				steamAPIHash = ToByteArray("56d9f94d37cb8f03049a1cc3062bffaf");
			}
			else
			{
				steamAPIPath = "Libraries/Native/Windows/steam_api64.dll";
				steamAPIHash = ToByteArray("500475b20083ccdc64f12d238cab687a");
			}
			vanillaSteamAPI = "steam_api.dll";
			gogHash = ToByteArray("efccd835e6b54697e05e8a4b72d935cd");
			steamHash = ToByteArray("4530e0acfa4c789f462addb77b405ccb");
		}
		else if (Platform.IsOSX)
		{
			steamAPIPath = "Libraries/Native/OSX/libsteam_api64.dylib";
			steamAPIHash = ToByteArray("801e9bf5e5899a41c5999811d870b1ca");
			vanillaSteamAPI = "libsteam_api.dylib";
			gogHash = ToByteArray("da2b740b4c6031df3a8b1f68b40cb82b");
			steamHash = ToByteArray("4512beef5d7607fa1771c3fdf6cdc712");
		}
		else if (Platform.IsLinux)
		{
			steamAPIPath = "Libraries/Native/Linux/libsteam_api64.so";
			steamAPIHash = ToByteArray("ccdf20f0b2f9abbe1fea8314b9fab096");
			vanillaSteamAPI = "libsteam_api.so";
			gogHash = ToByteArray("9db40ef7cd4b37794cfe29e8866bb6b4");
			steamHash = ToByteArray("2ff21c600897a9485ca5ae645a06202d");
		}
		else
		{
			ErrorReporting.FatalExit(Language.GetTextValue("tModLoader.UnknownVerificationOS"));
		}
	}

	private static bool HashMatchesFile(string path, byte[] hash)
	{
		using MD5 md5 = MD5.Create();
		using FileStream stream = File.OpenRead(path);
		return hash.SequenceEqual(md5.ComputeHash(stream));
	}

	private static byte[] ToByteArray(string hexString, bool forceLowerCase = true)
	{
		if (forceLowerCase)
		{
			hexString = hexString.ToLower();
		}
		byte[] retval = new byte[hexString.Length / 2];
		for (int i = 0; i < hexString.Length; i += 2)
		{
			retval[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
		}
		return retval;
	}

	internal static void Startup()
	{
		DistributionPlatform = DetectPlatform(out var detectionDetails);
		Logging.tML.Info((object)$"Distribution Platform: {DistributionPlatform}. Detection method: {detectionDetails}");
		if (DistributionPlatform == DistributionPlatform.GoG)
		{
			CheckGoG();
		}
		else
		{
			CheckSteam();
		}
	}

	private static DistributionPlatform DetectPlatform(out string detectionDetails)
	{
		if (Program.LaunchParameters.ContainsKey("-steam"))
		{
			detectionDetails = "-steam launch parameter";
			return DistributionPlatform.Steam;
		}
		if (Directory.GetCurrentDirectory().Contains("steamapps", StringComparison.OrdinalIgnoreCase))
		{
			detectionDetails = "CWD is /steamapps/";
			return DistributionPlatform.Steam;
		}
		if (!ObtainVanillaExePath(out var vanillaSteamAPIDir, out vanillaExePath))
		{
			detectionDetails = VanillaExe + " or " + CheckExe + " not found nearby";
			return DistributionPlatform.Steam;
		}
		if (Platform.IsOSX)
		{
			vanillaSteamAPIDir = Path.Combine(Directory.GetParent(vanillaSteamAPIDir).FullName, "MacOS", "osx");
		}
		if (File.Exists(Path.Combine(vanillaSteamAPIDir, vanillaSteamAPI)))
		{
			detectionDetails = vanillaSteamAPI + " found";
			return DistributionPlatform.Steam;
		}
		detectionDetails = Path.GetFileName(vanillaExePath) + " found, no steam files or directories nearby.";
		return DistributionPlatform.GoG;
	}

	private static bool ObtainVanillaExePath(out string vanillaPath, out string exePath)
	{
		vanillaPath = Directory.GetCurrentDirectory();
		if (CheckForExe(vanillaPath, out exePath))
		{
			return true;
		}
		vanillaPath = Directory.GetParent(vanillaPath).FullName;
		if (CheckForExe(vanillaPath, out exePath))
		{
			return true;
		}
		vanillaPath = Path.Combine(vanillaPath, "Terraria");
		if (Platform.IsOSX)
		{
			if (Directory.Exists("../Terraria/Terraria.app/"))
			{
				vanillaPath = "../Terraria/Terraria.app/Contents/Resources/";
			}
			else if (Directory.Exists("../Terraria.app/"))
			{
				vanillaPath = "../Terraria.app/Contents/Resources/";
			}
		}
		return CheckForExe(vanillaPath, out exePath);
	}

	private static bool CheckForExe(string vanillaPath, out string exePath)
	{
		exePath = Path.Combine(vanillaPath, CheckExe);
		if (File.Exists(exePath))
		{
			return true;
		}
		exePath = Path.Combine(vanillaPath, VanillaExe);
		if (File.Exists(exePath))
		{
			return true;
		}
		return false;
	}

	private static void CheckSteam()
	{
		if (!HashMatchesFile(steamAPIPath, steamAPIHash))
		{
			Utils.OpenToURL("https://terraria.org");
			ErrorReporting.FatalExit(Language.GetTextValue("tModLoader.SteamAPIHashMismatch"));
		}
		else if (!Main.dedServ)
		{
			TerrariaSteamClient.LaunchResult result = TerrariaSteamClient.Launch();
			switch (result)
			{
			case TerrariaSteamClient.LaunchResult.ErrClientProcDied:
				ErrorReporting.FatalExit("The terraria steam client process exited unexpectedly");
				break;
			case TerrariaSteamClient.LaunchResult.ErrSteamInitFailed:
				ErrorReporting.FatalExit(Language.GetTextValue("tModLoader.SteamInitFailed"));
				break;
			case TerrariaSteamClient.LaunchResult.ErrNotInstalled:
				ErrorReporting.FatalExit(Language.GetTextValue("tModLoader.TerrariaNotInstalled"));
				break;
			case TerrariaSteamClient.LaunchResult.ErrInstallOutOfDate:
				ErrorReporting.FatalExit(Language.GetTextValue("tModLoader.TerrariaOutOfDateMessage"));
				break;
			default:
				throw new Exception("Unsupported result type: " + result);
			case TerrariaSteamClient.LaunchResult.Ok:
				break;
			}
		}
	}

	private static void CheckGoG()
	{
		if (!HashMatchesFile(vanillaExePath, gogHash) && !HashMatchesFile(vanillaExePath, steamHash))
		{
			ErrorReporting.FatalExit(Language.GetTextValue("tModLoader.GOGHashMismatch", vanillaExePath, "1.4.4.9", CheckExe));
		}
		if (Path.GetFileName(vanillaExePath) != CheckExe)
		{
			string pathToCheckExe = Path.Combine(Path.GetDirectoryName(vanillaExePath), CheckExe);
			Logging.tML.Info((object)("Backing up " + Path.GetFileName(vanillaExePath) + " to " + CheckExe));
			File.Copy(vanillaExePath, pathToCheckExe);
		}
	}
}
