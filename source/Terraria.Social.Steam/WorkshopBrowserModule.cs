using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Steamworks;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.Engine;
using Terraria.ModLoader.UI.DownloadManager;
using Terraria.ModLoader.UI.ModBrowser;
using Terraria.Social.Base;

namespace Terraria.Social.Steam;

internal class WorkshopBrowserModule : SocialBrowserModule
{
	public static WorkshopBrowserModule Instance = new WorkshopBrowserModule();

	private HashSet<string> intermediateInstallStateMods = new HashSet<string>();

	public List<ModDownloadItem> CachedInstalledModDownloadItems { get; set; }

	private IReadOnlyList<LocalMod> InstalledItems { get; set; }

	private PublishedFileId_t GetId(ModPubId_t modId)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		return new PublishedFileId_t(ulong.Parse(modId.m_ModPubId));
	}

	public WorkshopBrowserModule()
	{
		ModOrganizer.OnLocalModsChanged += OnLocalModsChanged;
	}

	public bool Initialize()
	{
		OnLocalModsChanged(null, isDeletion: false);
		return true;
	}

	private void OnLocalModsChanged(HashSet<string> modSlugs, bool isDeletion)
	{
		InstalledItems = ModOrganizer.FindWorkshopMods();
		if (SteamedWraps.SteamAvailable)
		{
			CachedInstalledModDownloadItems = ((SocialBrowserModule)this).DirectQueryInstalledMDItems(default(QueryParameters));
		}
		if (!isDeletion)
		{
			return;
		}
		foreach (string item in modSlugs)
		{
			intermediateInstallStateMods.Add(item);
		}
	}

	public IReadOnlyList<LocalMod> GetInstalledMods()
	{
		if (InstalledItems == null)
		{
			InstalledItems = ModOrganizer.FindWorkshopMods();
		}
		return InstalledItems;
	}

	public bool GetModIdFromLocalFiles(TmodFile modFile, out ModPubId_t modId)
	{
		ulong publishId;
		bool publishIdLocal = WorkshopHelper.GetPublishIdLocal(modFile, out publishId);
		modId = new ModPubId_t
		{
			m_ModPubId = publishId.ToString()
		};
		return publishIdLocal;
	}

	public bool DoesItemNeedUpdate(ModPubId_t modId, LocalMod installed, Version webVersion)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		if (installed.properties.version < webVersion)
		{
			return true;
		}
		if (SteamedWraps.SteamAvailable && SteamedWraps.DoesWorkshopItemNeedUpdate(GetId(modId)))
		{
			return true;
		}
		return false;
	}

	public bool DoesAppNeedRestartToReinstallItem(ModPubId_t modId)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return SteamedWraps.IsWorkshopItemInstalled(GetId(modId));
	}

	public void DownloadItem(ModDownloadItem item, IDownloadProgress uiProgress)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		item.UpdateInstallState();
		PublishedFileId_t publishId = default(PublishedFileId_t);
		((PublishedFileId_t)(ref publishId))._002Ector(ulong.Parse(item.PublishId.m_ModPubId));
		bool forceUpdate = item.NeedUpdate || !SteamedWraps.IsWorkshopItemInstalled(publishId);
		uiProgress?.DownloadStarted(item.DisplayName);
		Utils.LogAndConsoleInfoMessage(Language.GetTextValue("tModLoader.BeginDownload", item.DisplayName));
		SteamedWraps.Download(publishId, uiProgress, forceUpdate);
		EnsureInstallationComplete(item);
	}

	public void EnsureInstallationComplete(ModDownloadItem item)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		Logging.tML.Info((object)"Validating Installation Has Completed: Step 1 / 2");
		string workshopFolder = WorkshopHelper.GetWorkshopFolder(Terraria.ModLoader.Engine.Steam.TMLAppID_t);
		AppId_t tMLAppID_t = Terraria.ModLoader.Engine.Steam.TMLAppID_t;
		string itemFolder = Path.Combine(workshopFolder, "content", ((object)(AppId_t)(ref tMLAppID_t)).ToString(), item.PublishId.m_ModPubId.ToString());
		for (int j = 0; j < 30; j++)
		{
			Thread.Sleep(500);
			if (Directory.Exists(itemFolder))
			{
				break;
			}
			Logging.tML.Info((object)$"Workshop Folder Missing. Awaiting. Attempt {j} / 20");
		}
		if (!Directory.Exists(itemFolder))
		{
			throw new Exception("Workshop Item " + item.DisplayNameClean + " Failed to Install during this play session!\nPlease restart the game to resolve.");
		}
		Logging.tML.Info((object)"Validating Installation Has Completed: Step 2 / 2");
		for (int i = 0; i < 20; i++)
		{
			Thread.Sleep(500);
			string fileName = ModOrganizer.GetActiveTmodInRepo(itemFolder);
			if (string.IsNullOrEmpty(fileName))
			{
				continue;
			}
			TmodFile modFile = new TmodFile(fileName);
			using (modFile.Open())
			{
				if (modFile.Version == item.Version)
				{
					break;
				}
			}
			Logging.tML.Info((object)$"Mod Update Not Received. Awaiting. Attempt {i} / 20");
		}
	}

	public string GetModWebPage(ModPubId_t modId)
	{
		return "https://steamcommunity.com/sharedfiles/filedetails/?id=" + modId.m_ModPubId;
	}

	/// <summary>
	/// Assumes Intialize has been run prior to use.
	/// </summary>
	public async IAsyncEnumerable<ModDownloadItem> QueryBrowser(QueryParameters queryParams, [EnumeratorCancellation] CancellationToken token = default(CancellationToken))
	{
		if (!SteamedWraps.SteamAvailable)
		{
			yield break;
		}
		if (queryParams.searchModIds != null && queryParams.searchModIds.Any())
		{
			List<string> missingMods;
			foreach (ModDownloadItem item4 in DirectQueryItems(queryParams, out missingMods))
			{
				yield return item4;
			}
			yield break;
		}
		switch (queryParams.updateStatusFilter)
		{
		case UpdateFilter.All:
			await foreach (ModDownloadItem item2 in WorkshopHelper.QueryHelper.QueryWorkshop(queryParams, token))
			{
				if (CachedInstalledModDownloadItems.Contains(item2) || intermediateInstallStateMods.Contains(item2.ModName))
				{
					item2.UpdateInstallState();
				}
				yield return item2;
			}
			break;
		case UpdateFilter.Available:
			await foreach (ModDownloadItem item3 in WorkshopHelper.QueryHelper.QueryWorkshop(queryParams, token))
			{
				if (!CachedInstalledModDownloadItems.Contains(item3) && !intermediateInstallStateMods.Contains(item3.ModName))
				{
					yield return item3;
				}
			}
			break;
		case UpdateFilter.UpdateOnly:
			foreach (ModDownloadItem item5 in CachedInstalledModDownloadItems.Where((ModDownloadItem item) => item.NeedUpdate))
			{
				yield return item5;
			}
			break;
		case UpdateFilter.InstalledOnly:
			foreach (ModDownloadItem cachedInstalledModDownloadItem in CachedInstalledModDownloadItems)
			{
				yield return cachedInstalledModDownloadItem;
			}
			break;
		}
	}

	public List<ModDownloadItem> DirectQueryItems(QueryParameters queryParams, out List<string> missingMods)
	{
		if (queryParams.searchModIds == null || !SteamedWraps.SteamAvailable)
		{
			throw new Exception("Unexpected Call of DirectQueryItems while either Steam is not initialized or query parameters.searchModIds is null");
		}
		return new WorkshopHelper.QueryHelper.AQueryInstance(queryParams).QueryItemsSynchronously(out missingMods);
	}
}
