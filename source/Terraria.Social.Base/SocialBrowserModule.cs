using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.UI.DownloadManager;
using Terraria.ModLoader.UI.ModBrowser;

namespace Terraria.Social.Base;

public interface SocialBrowserModule
{
	internal List<ModDownloadItem> CachedInstalledModDownloadItems { get; set; }

	bool Initialize();

	IAsyncEnumerable<ModDownloadItem> QueryBrowser(QueryParameters queryParams, [EnumeratorCancellation] CancellationToken token = default(CancellationToken));

	List<ModDownloadItem> DirectQueryItems(QueryParameters queryParams, out List<string> missingMods);

	string GetModWebPage(ModPubId_t item);

	bool GetModIdFromLocalFiles(TmodFile modFile, out ModPubId_t item);

	List<ModDownloadItem> DirectQueryInstalledMDItems(QueryParameters qParams = default(QueryParameters))
	{
		IReadOnlyList<LocalMod> installedMods = GetInstalledMods();
		List<ModPubId_t> listIds = new List<ModPubId_t>();
		foreach (LocalMod mod in installedMods)
		{
			if (GetModIdFromLocalFiles(mod.modFile, out var id))
			{
				listIds.Add(id);
			}
		}
		qParams.searchModIds = listIds.ToArray();
		List<string> missingMods;
		return DirectQueryItems(qParams, out missingMods);
	}

	List<ModDownloadItem> GetInstalledModDownloadItems()
	{
		if (CachedInstalledModDownloadItems == null)
		{
			CachedInstalledModDownloadItems = DirectQueryInstalledMDItems();
		}
		return CachedInstalledModDownloadItems;
	}

	bool DoesAppNeedRestartToReinstallItem(ModPubId_t modId);

	internal bool DoesItemNeedUpdate(ModPubId_t modId, LocalMod installed, Version webVersion);

	internal IReadOnlyList<LocalMod> GetInstalledMods();

	internal LocalMod IsItemInstalled(string slug)
	{
		return (from t in GetInstalledMods()
			where t.Name == slug
			select t).FirstOrDefault();
	}

	internal void DownloadItem(ModDownloadItem item, IDownloadProgress uiProgress);

	void GetDependenciesRecursive(HashSet<ModDownloadItem> set)
	{
		HashSet<ModPubId_t> fullList = set.Select((ModDownloadItem x) => x.PublishId).ToHashSet();
		HashSet<ModPubId_t> iterationList = new HashSet<ModPubId_t>();
		HashSet<ModDownloadItem> iterationSet = set;
		while (true)
		{
			foreach (ModDownloadItem item in iterationSet)
			{
				iterationList.UnionWith(item.ModReferenceByModId);
			}
			iterationList.ExceptWith(fullList);
			if (iterationList.Count <= 0)
			{
				break;
			}
			iterationSet = DirectQueryItems(new QueryParameters
			{
				searchModIds = iterationList.ToArray()
			}, out var notFoundMods).ToHashSet();
			if (notFoundMods.Any())
			{
				notFoundMods = notFoundMods;
			}
			fullList.UnionWith(iterationList);
			set.UnionWith(iterationSet);
		}
	}

	static string GetBrowserVersionNumber(Version tmlVersion)
	{
		if (tmlVersion < new Version(0, 12))
		{
			return "1.3";
		}
		if (tmlVersion < new Version(2022, 10))
		{
			return "1.4.3";
		}
		if (tmlVersion < new Version(2023, 3, 85))
		{
			return "1.4.4-Transitive";
		}
		return "1.4.4";
	}
}
