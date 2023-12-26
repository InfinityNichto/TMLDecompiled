using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Core;
using Terraria.Social.Base;
using Terraria.UI.Chat;

namespace Terraria.ModLoader.UI.ModBrowser;

public class ModDownloadItem
{
	public readonly string ModName;

	public readonly string DisplayName;

	public readonly string DisplayNameClean;

	public readonly ModPubId_t PublishId;

	public readonly string OwnerId;

	public readonly Version Version;

	public readonly string Author;

	public readonly string ModIconUrl;

	public readonly DateTime TimeStamp;

	public readonly string ModReferencesBySlug;

	public readonly ModPubId_t[] ModReferenceByModId;

	public readonly ModSide ModSide;

	public readonly int Downloads;

	public readonly int Hot;

	public readonly string Homepage;

	public readonly Version ModloaderVersion;

	internal LocalMod Installed;

	public bool NeedUpdate { get; private set; }

	public bool AppNeedRestartToReinstall { get; private set; }

	public bool IsInstalled => Installed != null;

	public ModDownloadItem(string displayName, string name, Version version, string author, string modReferences, ModSide modSide, string modIconUrl, string publishId, int downloads, int hot, DateTime timeStamp, Version modloaderversion, string homepage, string ownerId, string[] referencesById)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		ModName = name;
		DisplayName = displayName;
		DisplayNameClean = string.Join("", from x in ChatManager.ParseMessage(displayName, Color.White)
			where x.GetType() == typeof(TextSnippet)
			select x.Text);
		PublishId = new ModPubId_t
		{
			m_ModPubId = publishId
		};
		OwnerId = ownerId;
		Author = author;
		ModReferencesBySlug = modReferences;
		ModReferenceByModId = Array.ConvertAll(referencesById, delegate(string x)
		{
			ModPubId_t result = default(ModPubId_t);
			result.m_ModPubId = x;
			return result;
		});
		ModSide = modSide;
		ModIconUrl = modIconUrl;
		Downloads = downloads;
		Hot = hot;
		Homepage = homepage;
		TimeStamp = timeStamp;
		Version = version;
		ModloaderVersion = modloaderversion;
	}

	internal void UpdateInstallState()
	{
		Installed = Interface.modBrowser.SocialBackend.IsItemInstalled(ModName);
		NeedUpdate = Installed != null && Interface.modBrowser.SocialBackend.DoesItemNeedUpdate(PublishId, Installed, Version);
		AppNeedRestartToReinstall = Installed == null && Interface.modBrowser.SocialBackend.DoesAppNeedRestartToReinstallItem(PublishId);
	}

	public override bool Equals(object obj)
	{
		return Equals(obj as ModDownloadItem);
	}

	private (string, string, Version) GetComparable()
	{
		return (ModName, PublishId.m_ModPubId, Version);
	}

	public bool Equals(ModDownloadItem item)
	{
		if (item == null)
		{
			return false;
		}
		(string, string, Version) comparable = GetComparable();
		(string, string, Version) comparable2 = item.GetComparable();
		if (comparable.Item1 == comparable2.Item1 && comparable.Item2 == comparable2.Item2)
		{
			return comparable.Item3 == comparable2.Item3;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return GetComparable().GetHashCode();
	}

	public static IEnumerable<ModDownloadItem> NeedsInstallOrUpdate(IEnumerable<ModDownloadItem> downloads)
	{
		return downloads.Where(delegate(ModDownloadItem item)
		{
			if (item == null)
			{
				return false;
			}
			item.UpdateInstallState();
			return !item.IsInstalled || item.NeedUpdate;
		});
	}
}
