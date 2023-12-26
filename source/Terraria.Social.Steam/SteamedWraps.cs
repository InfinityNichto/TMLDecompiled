using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Threading;
using ReLogic.OS;
using Steamworks;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Engine;
using Terraria.ModLoader.UI.DownloadManager;
using Terraria.ModLoader.UI.ModBrowser;
using Terraria.Social.Base;

namespace Terraria.Social.Steam;

public static class SteamedWraps
{
	public struct ItemInstallInfo
	{
		public string installPath;

		public uint lastUpdatedTime;
	}

	internal const uint thisApp = 1281930u;

	private const int PlaytimePagingConst = 100;

	private static List<PublishedFileId_t> deletedItems = new List<PublishedFileId_t>();

	public static bool SteamClient { get; set; }

	public static bool FamilyShared { get; set; } = false;


	internal static bool SteamAvailable { get; set; }

	internal static string GetCurrentSteamLangKey()
	{
		return (GameCulture.CultureName)LanguageManager.Instance.ActiveCulture.LegacyId switch
		{
			GameCulture.CultureName.German => "german", 
			GameCulture.CultureName.Italian => "italian", 
			GameCulture.CultureName.French => "french", 
			GameCulture.CultureName.Spanish => "spanish", 
			GameCulture.CultureName.Russian => "russian", 
			GameCulture.CultureName.Chinese => "schinese", 
			GameCulture.CultureName.Portuguese => "portuguese", 
			GameCulture.CultureName.Polish => "polish", 
			_ => "english", 
		};
	}

	internal static void ReportCheckSteamLogs()
	{
		string workshopLogLoc = "";
		if (Platform.IsWindows)
		{
			workshopLogLoc = "C:/Program Files (x86)/Steam/logs/workshop_log.txt";
		}
		else if (Platform.IsOSX)
		{
			workshopLogLoc = "~/Library/Application Support/Steam/logs/workshop_log.txt";
		}
		else if (Platform.IsLinux)
		{
			workshopLogLoc = "/home/user/.local/share/Steam/logs/workshop_log.txt";
		}
		Utils.LogAndConsoleInfoMessage(Language.GetTextValue("tModLoader.ConsultSteamLogs", workshopLogLoc));
	}

	public static void QueueForceValidateSteamInstall()
	{
		if (SteamClient)
		{
			if (Environment.GetEnvironmentVariable("SteamClientLaunch") != "1")
			{
				Logging.tML.Info((object)"Launched Outside of Steam. Skipping attempt to trigger 'verify local files' in Steam. If error persists, please attempt this manually");
				return;
			}
			SteamApps.MarkContentCorrupt(false);
			Logging.tML.Info((object)"Marked tModLoader installation files as corrupt in Steam. On Next Launch, User will have 'Verify Local Files' ran");
		}
	}

	internal static void Initialize()
	{
		if (!FamilyShared && SocialAPI.Mode == SocialMode.Steam)
		{
			SteamAvailable = true;
			SteamClient = true;
			Logging.tML.Info((object)"SteamBackend: Running standard Steam Desktop Client API");
		}
		else if (!Main.dedServ && !TryInitViaGameServer())
		{
			Utils.ShowFancyErrorMessage("Steam Game Server failed to Init. Steam Workshop downloading on GoG is unavailable. Make sure Steam is installed", 0);
		}
	}

	public static bool TryInitViaGameServer()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		Terraria.ModLoader.Engine.Steam.SetAppId(Terraria.ModLoader.Engine.Steam.TMLAppID_t);
		try
		{
			if (!GameServer.Init(0u, (ushort)7775, (ushort)7774, (EServerMode)1, "0.11.9.0"))
			{
				return false;
			}
			SteamGameServer.SetGameDescription("tModLoader Mod Browser");
			SteamGameServer.SetProduct(1281930u.ToString());
			SteamGameServer.LogOnAnonymous();
		}
		catch (Exception e)
		{
			Logging.tML.Error((object)e);
			return false;
		}
		Logging.tML.Info((object)"SteamBackend: Running non-standard Steam GameServer API");
		SteamAvailable = true;
		return true;
	}

	public static void ReleaseWorkshopHandle(UGCQueryHandle_t handle)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if (SteamClient)
		{
			SteamUGC.ReleaseQueryUGCRequest(handle);
		}
		else if (SteamAvailable)
		{
			SteamGameServerUGC.ReleaseQueryUGCRequest(handle);
		}
	}

	public static SteamUGCDetails_t FetchItemDetails(UGCQueryHandle_t handle, uint index)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		SteamUGCDetails_t pDetails = default(SteamUGCDetails_t);
		if (SteamClient)
		{
			SteamUGC.GetQueryUGCResult(handle, index, ref pDetails);
		}
		else if (SteamAvailable)
		{
			SteamGameServerUGC.GetQueryUGCResult(handle, index, ref pDetails);
		}
		return pDetails;
	}

	public static PublishedFileId_t[] FetchItemDependencies(UGCQueryHandle_t handle, uint index, uint numChildren)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		PublishedFileId_t[] deps = (PublishedFileId_t[])(object)new PublishedFileId_t[numChildren];
		if (SteamClient)
		{
			SteamUGC.GetQueryUGCChildren(handle, index, deps, numChildren);
		}
		else if (SteamAvailable)
		{
			SteamGameServerUGC.GetQueryUGCChildren(handle, index, deps, numChildren);
		}
		return deps;
	}

	private static void ModifyQueryHandle(ref UGCQueryHandle_t qHandle, QueryParameters qP)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		FilterByText(ref qHandle, qP.searchGeneric);
		FilterByTags(ref qHandle, qP.searchTags);
		FilterModSide(ref qHandle, qP.modSideFilter);
		if (SteamClient)
		{
			SteamUGC.SetAllowCachedResponse(qHandle, 0u);
			SteamUGC.SetLanguage(qHandle, GetCurrentSteamLangKey());
			SteamUGC.SetReturnChildren(qHandle, true);
			SteamUGC.SetReturnKeyValueTags(qHandle, true);
			SteamUGC.SetReturnPlaytimeStats(qHandle, 30u);
		}
		else if (SteamAvailable)
		{
			SteamGameServerUGC.SetAllowCachedResponse(qHandle, 0u);
			SteamGameServerUGC.SetLanguage(qHandle, GetCurrentSteamLangKey());
			SteamGameServerUGC.SetReturnChildren(qHandle, true);
			SteamGameServerUGC.SetReturnKeyValueTags(qHandle, true);
			SteamGameServerUGC.SetReturnPlaytimeStats(qHandle, 30u);
		}
	}

	private static void FilterModSide(ref UGCQueryHandle_t qHandle, ModSideFilter side)
	{
		if (side != 0)
		{
			FilterByTags(ref qHandle, new string[1] { side.ToString() });
		}
	}

	private static void FilterByTags(ref UGCQueryHandle_t qHandle, string[] tags)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		if (tags == null)
		{
			return;
		}
		foreach (string tag in tags)
		{
			if (SteamClient)
			{
				SteamUGC.AddRequiredTag(qHandle, tag);
			}
			else if (SteamAvailable)
			{
				SteamGameServerUGC.AddRequiredTag(qHandle, tag);
			}
		}
	}

	private static void FilterByInternalName(ref UGCQueryHandle_t qHandle, string internalName)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		if (internalName != null)
		{
			if (SteamClient)
			{
				SteamUGC.AddRequiredKeyValueTag(qHandle, "name", internalName);
			}
			else if (SteamAvailable)
			{
				SteamGameServerUGC.AddRequiredKeyValueTag(qHandle, "name", internalName);
			}
		}
	}

	private static void FilterByText(ref UGCQueryHandle_t qHandle, string text)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		if (!string.IsNullOrEmpty(text))
		{
			if (SteamClient)
			{
				SteamUGC.SetSearchText(qHandle, text);
			}
			else if (SteamAvailable)
			{
				SteamGameServerUGC.SetSearchText(qHandle, text);
			}
		}
	}

	public static SteamAPICall_t GenerateDirectItemsQuery(string[] modId)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		PublishedFileId_t[] publishId = Array.ConvertAll(modId, (Converter<string, PublishedFileId_t>)((string s) => new PublishedFileId_t(ulong.Parse(s))));
		if (SteamClient)
		{
			UGCQueryHandle_t qHandle = SteamUGC.CreateQueryUGCDetailsRequest(publishId, (uint)publishId.Length);
			ModifyQueryHandle(ref qHandle, default(QueryParameters));
			return SteamUGC.SendQueryUGCRequest(qHandle);
		}
		if (SteamAvailable)
		{
			UGCQueryHandle_t qHandle2 = SteamGameServerUGC.CreateQueryUGCDetailsRequest(publishId, (uint)publishId.Length);
			ModifyQueryHandle(ref qHandle2, default(QueryParameters));
			return SteamGameServerUGC.SendQueryUGCRequest(qHandle2);
		}
		return default(SteamAPICall_t);
	}

	public static EUGCQuery CalculateQuerySort(QueryParameters qParams)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		if (!string.IsNullOrEmpty(qParams.searchGeneric))
		{
			return (EUGCQuery)11;
		}
		return (EUGCQuery)(qParams.sortingParamater switch
		{
			ModBrowserSortMode.DownloadsDescending => 12, 
			ModBrowserSortMode.Hot => 13, 
			ModBrowserSortMode.RecentlyUpdated => 19, 
			_ => 11, 
		});
	}

	public static SteamAPICall_t GenerateAndSubmitModBrowserQuery(uint page, QueryParameters qP, string internalName = null)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		if (SteamClient)
		{
			UGCQueryHandle_t qHandle = SteamUGC.CreateQueryAllUGCRequest(CalculateQuerySort(qP), (EUGCMatchingUGCType)0, new AppId_t(1281930u), new AppId_t(1281930u), page);
			ModifyQueryHandle(ref qHandle, qP);
			FilterByInternalName(ref qHandle, internalName);
			return SteamUGC.SendQueryUGCRequest(qHandle);
		}
		if (SteamAvailable)
		{
			UGCQueryHandle_t qHandle2 = SteamGameServerUGC.CreateQueryAllUGCRequest(CalculateQuerySort(qP), (EUGCMatchingUGCType)0, new AppId_t(1281930u), new AppId_t(1281930u), page);
			ModifyQueryHandle(ref qHandle2, qP);
			FilterByInternalName(ref qHandle2, internalName);
			return SteamGameServerUGC.SendQueryUGCRequest(qHandle2);
		}
		return default(SteamAPICall_t);
	}

	public static void FetchPlayTimeStats(UGCQueryHandle_t handle, uint index, out ulong hot, out ulong downloads)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		if (SteamClient)
		{
			SteamUGC.GetQueryUGCStatistic(handle, index, (EItemStatistic)3, ref downloads);
			SteamUGC.GetQueryUGCStatistic(handle, index, (EItemStatistic)11, ref hot);
		}
		else if (SteamAvailable)
		{
			SteamGameServerUGC.GetQueryUGCStatistic(handle, index, (EItemStatistic)3, ref downloads);
			SteamGameServerUGC.GetQueryUGCStatistic(handle, index, (EItemStatistic)11, ref hot);
		}
		else
		{
			hot = 0uL;
			downloads = 0uL;
		}
	}

	public static void FetchPreviewImageUrl(UGCQueryHandle_t handle, uint index, out string modIconUrl)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		if (SteamClient)
		{
			SteamUGC.GetQueryUGCPreviewURL(handle, index, ref modIconUrl, 1000u);
		}
		else if (SteamAvailable)
		{
			SteamGameServerUGC.GetQueryUGCPreviewURL(handle, index, ref modIconUrl, 1000u);
		}
		else
		{
			modIconUrl = null;
		}
	}

	public static void FetchMetadata(UGCQueryHandle_t handle, uint index, out NameValueCollection metadata)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		metadata = new NameValueCollection();
		uint keyCount = (SteamClient ? SteamUGC.GetQueryUGCNumKeyValueTags(handle, index) : (SteamAvailable ? SteamGameServerUGC.GetQueryUGCNumKeyValueTags(handle, index) : 0u));
		string key = default(string);
		string val = default(string);
		for (uint i = 0u; i < keyCount; i++)
		{
			if (SteamClient)
			{
				SteamUGC.GetQueryUGCKeyValueTag(handle, index, i, ref key, 255u, ref val, 255u);
			}
			else
			{
				SteamGameServerUGC.GetQueryUGCKeyValueTag(handle, index, i, ref key, 255u, ref val, 255u);
			}
			metadata[key] = val;
		}
	}

	public static void RunCallbacks()
	{
		if (SteamClient)
		{
			SteamAPI.RunCallbacks();
		}
		else if (SteamAvailable)
		{
			GameServer.RunCallbacks();
		}
	}

	public static void StopPlaytimeTracking()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		if (SteamClient)
		{
			SteamUGC.StopPlaytimeTrackingForAllItems();
		}
		else if (SteamAvailable)
		{
			SteamGameServerUGC.StopPlaytimeTrackingForAllItems();
		}
	}

	public static void BeginPlaytimeTracking()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		if (!SteamAvailable)
		{
			return;
		}
		List<PublishedFileId_t> list = new List<PublishedFileId_t>();
		Mod[] mods = Terraria.ModLoader.ModLoader.Mods;
		for (int j = 0; j < mods.Length; j++)
		{
			if (WorkshopHelper.GetPublishIdLocal(mods[j].File, out var publishId))
			{
				list.Add(new PublishedFileId_t(publishId));
			}
		}
		int count = list.Count;
		if (count == 0)
		{
			return;
		}
		int pg = count / 100;
		int rem = count % 100;
		for (int i = 0; i < pg + 1; i++)
		{
			List<PublishedFileId_t> pgList = list.GetRange(i * 100, (i == pg) ? rem : 100);
			if (SteamClient)
			{
				SteamUGC.StartPlaytimeTracking(pgList.ToArray(), (uint)pgList.Count);
			}
			else if (SteamAvailable)
			{
				SteamGameServerUGC.StartPlaytimeTracking(pgList.ToArray(), (uint)pgList.Count);
			}
		}
	}

	internal static void OnGameExitCleanup()
	{
		if (!SteamAvailable)
		{
			CleanupACF();
			return;
		}
		if (SteamClient)
		{
			SteamAPI.Shutdown();
			return;
		}
		GameServer.Shutdown();
		CleanupACF();
	}

	public static uint GetWorkshopItemState(PublishedFileId_t publishId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		if (SteamClient)
		{
			return SteamUGC.GetItemState(publishId);
		}
		if (SteamAvailable)
		{
			return SteamGameServerUGC.GetItemState(publishId);
		}
		return 0u;
	}

	public static ItemInstallInfo GetInstallInfo(PublishedFileId_t publishId)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		string installPath = null;
		uint lastUpdatedTime = 0u;
		if (SteamClient)
		{
			ulong installSize = default(ulong);
			SteamUGC.GetItemInstallInfo(publishId, ref installSize, ref installPath, 1000u, ref lastUpdatedTime);
		}
		else if (SteamAvailable)
		{
			ulong installSize2 = default(ulong);
			SteamGameServerUGC.GetItemInstallInfo(publishId, ref installSize2, ref installPath, 1000u, ref lastUpdatedTime);
		}
		ItemInstallInfo result = default(ItemInstallInfo);
		result.installPath = installPath;
		result.lastUpdatedTime = lastUpdatedTime;
		return result;
	}

	public static void UninstallWorkshopItem(PublishedFileId_t publishId, string installPath = null)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrEmpty(installPath))
		{
			installPath = GetInstallInfo(publishId).installPath;
		}
		if (Directory.Exists(installPath))
		{
			if (SteamClient)
			{
				SteamUGC.UnsubscribeItem(publishId);
			}
			Directory.Delete(installPath, recursive: true);
			if (!SteamClient)
			{
				deletedItems.Add(publishId);
			}
		}
	}

	private static void CleanupACF()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		foreach (PublishedFileId_t deletedItem in deletedItems)
		{
			UninstallACF(deletedItem);
		}
	}

	private static void UninstallACF(PublishedFileId_t publishId)
	{
		string path = Path.Combine(Directory.GetCurrentDirectory(), "steamapps", "workshop", "appworkshop_" + 1281930u + ".acf");
		string[] acf = File.ReadAllLines(path);
		using StreamWriter w = new StreamWriter(path);
		int blockLines = 5;
		int skip = 0;
		for (int i = 0; i < acf.Length; i++)
		{
			if (acf[i].Contains(((object)(PublishedFileId_t)(ref publishId)).ToString()))
			{
				skip = blockLines;
			}
			else if (skip > 0)
			{
				skip--;
			}
			else
			{
				w.WriteLine(acf[i]);
			}
		}
	}

	public static bool IsWorkshopItemInstalled(PublishedFileId_t publishId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		uint workshopItemState = GetWorkshopItemState(publishId);
		bool installed = (workshopItemState & 4) != 0;
		bool downloading = (workshopItemState & 0x30) != 0;
		if (installed)
		{
			return !downloading;
		}
		return false;
	}

	public static bool DoesWorkshopItemNeedUpdate(PublishedFileId_t publishId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		uint currState = GetWorkshopItemState(publishId);
		if ((currState & 8) == 0 && currState != 0)
		{
			return (currState & 0x20) != 0;
		}
		return true;
	}

	/// <summary>
	/// Updates and/or Downloads the Item specified by publishId
	/// </summary>
	internal static void Download(PublishedFileId_t publishId, IDownloadProgress uiProgress = null, bool forceUpdate = false)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		if (!SteamAvailable)
		{
			return;
		}
		if (SteamClient)
		{
			SteamUGC.SubscribeItem(publishId);
		}
		if (DoesWorkshopItemNeedUpdate(publishId) || forceUpdate)
		{
			Utils.LogAndConsoleInfoMessage(Language.GetTextValue("tModLoader.SteamDownloader"));
			if (!((!SteamClient) ? SteamGameServerUGC.DownloadItem(publishId, true) : SteamUGC.DownloadItem(publishId, true)))
			{
				ReportCheckSteamLogs();
				throw new ArgumentException("Downloading Workshop Item failed due to unknown reasons");
			}
			InnerDownloadHandler(uiProgress, publishId);
			Utils.LogAndConsoleInfoMessage(Language.GetTextValue("tModLoader.EndDownload"));
		}
		else
		{
			Utils.LogAndConsoleErrorMessage(Language.GetTextValue("tModLoader.SteamRejectUpdate", publishId));
		}
	}

	private static void InnerDownloadHandler(IDownloadProgress uiProgress, PublishedFileId_t publishId)
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		int nextPercentageToLog = 10;
		int numFailures = 0;
		ulong dlBytes = default(ulong);
		ulong totalBytes = default(ulong);
		while (!IsWorkshopItemInstalled(publishId))
		{
			if (SteamClient)
			{
				SteamUGC.GetItemDownloadInfo(publishId, ref dlBytes, ref totalBytes);
			}
			else
			{
				SteamGameServerUGC.GetItemDownloadInfo(publishId, ref dlBytes, ref totalBytes);
			}
			if (totalBytes == 0L)
			{
				if (numFailures++ < 10)
				{
					Thread.Sleep(100);
					continue;
				}
				break;
			}
			uiProgress?.UpdateDownloadProgress((float)dlBytes / (float)Math.Max(totalBytes, 1uL), (long)dlBytes, (long)totalBytes);
			int percentage = (int)MathF.Round((float)dlBytes / (float)totalBytes * 100f);
			if (percentage >= nextPercentageToLog)
			{
				Utils.LogAndConsoleInfoMessage(Language.GetTextValue("tModLoader.DownloadProgress", percentage));
				nextPercentageToLog = percentage + 10;
				if (nextPercentageToLog > 100 && nextPercentageToLog != 110)
				{
					nextPercentageToLog = 100;
				}
			}
			if ((float)(dlBytes / totalBytes) != 1f)
			{
				continue;
			}
			break;
		}
	}
}
