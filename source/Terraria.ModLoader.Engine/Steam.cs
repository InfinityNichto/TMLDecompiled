using System;
using System.IO;
using ReLogic.OS;
using Steamworks;
using Terraria.ModLoader.UI;
using Terraria.Social;
using Terraria.Social.Steam;

namespace Terraria.ModLoader.Engine;

internal class Steam
{
	public const uint TMLAppID = 1281930u;

	public const uint TerrariaAppID = 105600u;

	public static ulong lastAvailableSteamCloudStorage = ulong.MaxValue;

	public static AppId_t TMLAppID_t => new AppId_t(1281930u);

	public static AppId_t TerrariaAppId_t => new AppId_t(105600u);

	public static bool CheckSteamCloudStorageSufficient(ulong input)
	{
		if (SocialAPI.Cloud != null)
		{
			return input < lastAvailableSteamCloudStorage;
		}
		return true;
	}

	public static void RecalculateAvailableSteamCloudStorage()
	{
		if (SocialAPI.Cloud != null)
		{
			ulong num = default(ulong);
			SteamRemoteStorage.GetQuota(ref num, ref lastAvailableSteamCloudStorage);
		}
	}

	internal static void PostInit()
	{
		RecalculateAvailableSteamCloudStorage();
		Logging.Terraria.Info((object)("Steam Cloud Quota: " + UIMemoryBar.SizeSuffix((long)lastAvailableSteamCloudStorage) + " available"));
		string branchName = default(string);
		bool OnBetaBranch = SteamApps.GetCurrentBetaName(ref branchName, 1000);
		Logging.tML.Info((object)("Steam beta branch: " + (OnBetaBranch ? branchName : "None")));
	}

	public static string GetSteamTerrariaInstallDir()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		string terrariaInstallLocation = default(string);
		SteamApps.GetAppInstallDir(TerrariaAppId_t, ref terrariaInstallLocation, 1000u);
		if (terrariaInstallLocation == null)
		{
			terrariaInstallLocation = "../Terraria";
			Logging.Terraria.Warn((object)("Steam reports no terraria install directory. Falling back to " + terrariaInstallLocation));
		}
		if (Platform.IsOSX)
		{
			terrariaInstallLocation = Path.Combine(terrariaInstallLocation, "Terraria.app", "Contents", "Resources");
		}
		Logging.tML.Info((object)("Terraria Steam Install Location assumed to be: " + terrariaInstallLocation));
		return terrariaInstallLocation;
	}

	internal static void SetAppId(AppId_t appId)
	{
		string steam_appid_path = "steam_appid.txt";
		if (Environment.GetEnvironmentVariable("SteamClientLaunch") != "1" || SteamedWraps.FamilyShared || InstallVerifier.DistributionPlatform == DistributionPlatform.GoG)
		{
			File.WriteAllText(steam_appid_path, ((object)(AppId_t)(ref appId)).ToString());
			return;
		}
		try
		{
			File.Delete(steam_appid_path);
		}
		catch (IOException)
		{
		}
		if (!(Environment.GetEnvironmentVariable("SteamAppId") != ((object)(AppId_t)(ref appId)).ToString()))
		{
			return;
		}
		throw new Exception("Cannot overwrite steam env. SteamAppId=" + Environment.GetEnvironmentVariable("SteamAppId"));
	}
}
