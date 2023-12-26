using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Steamworks;
using Terraria.DataStructures;
using Terraria.GameContent.UI.States;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.Social.Base;
using Terraria.Utilities;

namespace Terraria.Social.Steam;

public class WorkshopSocialModule : Terraria.Social.Base.WorkshopSocialModule
{
	private WorkshopHelper.UGCBased.Downloader _downloader;

	private WorkshopHelper.UGCBased.PublishedItemsFinder _publishedItems;

	private List<WorkshopHelper.UGCBased.APublisherInstance> _publisherInstances;

	private string _contentBaseFolder;

	private ulong currPublishID;

	private ulong existingAuthorID;

	public override void Initialize()
	{
		base.Branding = new WorkshopBranding
		{
			ResourcePackBrand = ResourcePack.BrandingType.SteamWorkshop
		};
		_publisherInstances = new List<WorkshopHelper.UGCBased.APublisherInstance>();
		base.ProgressReporter = new WorkshopProgressReporter(_publisherInstances);
		base.SupportedTags = new SupportedWorkshopTags();
		_contentBaseFolder = Main.SavePath + Path.DirectorySeparatorChar + "Workshop";
		_downloader = WorkshopHelper.UGCBased.Downloader.Create();
		_publishedItems = WorkshopHelper.UGCBased.PublishedItemsFinder.Create();
		WorkshopIssueReporter workshopIssueReporter = new WorkshopIssueReporter();
		workshopIssueReporter.OnNeedToOpenUI += _issueReporter_OnNeedToOpenUI;
		workshopIssueReporter.OnNeedToNotifyUI += _issueReporter_OnNeedToNotifyUI;
		base.IssueReporter = workshopIssueReporter;
		UIWorkshopHub.OnWorkshopHubMenuOpened += RefreshSubscriptionsAndPublishings;
	}

	private void _issueReporter_OnNeedToNotifyUI()
	{
		Main.IssueReporterIndicator.AttemptLettingPlayerKnow();
		Main.WorkshopPublishingIndicator.Hide();
	}

	private void _issueReporter_OnNeedToOpenUI()
	{
		Main.OpenReportsMenu();
	}

	public override void Shutdown()
	{
	}

	public override void LoadEarlyContent()
	{
		RefreshSubscriptionsAndPublishings();
	}

	private void RefreshSubscriptionsAndPublishings()
	{
		_downloader.Refresh(base.IssueReporter);
		_publishedItems.Refresh();
	}

	public override List<string> GetListOfSubscribedWorldPaths()
	{
		return _downloader.WorldPaths.Select((string folderPath) => folderPath + Path.DirectorySeparatorChar + "world.wld").ToList();
	}

	public override List<string> GetListOfSubscribedResourcePackPaths()
	{
		return _downloader.ResourcePackPaths;
	}

	public override bool TryGetPath(string pathEnd, out string fullPathFound)
	{
		fullPathFound = null;
		string text = _downloader.ResourcePackPaths.FirstOrDefault((string x) => x.EndsWith(pathEnd));
		if (text == null)
		{
			return false;
		}
		fullPathFound = text;
		return true;
	}

	private void Forget(WorkshopHelper.UGCBased.APublisherInstance instance)
	{
		_publisherInstances.Remove(instance);
		RefreshSubscriptionsAndPublishings();
	}

	public override void PublishWorld(WorldFileData world, WorkshopItemPublishSettings settings)
	{
		string name = world.Name;
		string textForWorld = GetTextForWorld(world);
		string[] usedTagsInternalNames = settings.GetUsedTagsInternalNames();
		string text = GetTemporaryFolderPath() + world.GetFileName(includeExtension: false);
		if (MakeTemporaryFolder(text))
		{
			WorkshopHelper.UGCBased.WorldPublisherInstance worldPublisherInstance = new WorkshopHelper.UGCBased.WorldPublisherInstance(world);
			_publisherInstances.Add(worldPublisherInstance);
			worldPublisherInstance.PublishContent(_publishedItems, base.IssueReporter, Forget, name, textForWorld, text, settings.PreviewImagePath, settings.Publicity, usedTagsInternalNames, null, 0uL);
		}
	}

	private string GetTextForWorld(WorldFileData world)
	{
		string text = "This is \"";
		text += world.Name;
		string text2 = "";
		text2 = world.WorldSizeX switch
		{
			4200 => "small", 
			6400 => "medium", 
			8400 => "large", 
			_ => "custom", 
		};
		string text3 = "";
		text3 = world.GameMode switch
		{
			3 => "journey", 
			0 => "classic", 
			1 => "expert", 
			2 => "master", 
			_ => "custom", 
		};
		text = text + "\", a " + text2.ToLower() + " " + text3.ToLower() + " world";
		text = text + " infected by the " + (world.HasCorruption ? "corruption" : "crimson");
		if (world.IsHardMode)
		{
			text += ", in hardmode";
		}
		return text + ".";
	}

	public override void PublishResourcePack(ResourcePack resourcePack, WorkshopItemPublishSettings settings)
	{
		if (resourcePack.IsCompressed)
		{
			base.IssueReporter.ReportInstantUploadProblem("Workshop.ReportIssue_CannotPublishZips");
			return;
		}
		string name = resourcePack.Name;
		string text = resourcePack.Description;
		if (string.IsNullOrWhiteSpace(text))
		{
			text = "";
		}
		string[] usedTagsInternalNames = settings.GetUsedTagsInternalNames();
		string fullPath = resourcePack.FullPath;
		WorkshopHelper.UGCBased.ResourcePackPublisherInstance resourcePackPublisherInstance = new WorkshopHelper.UGCBased.ResourcePackPublisherInstance(resourcePack);
		_publisherInstances.Add(resourcePackPublisherInstance);
		resourcePackPublisherInstance.PublishContent(_publishedItems, base.IssueReporter, Forget, name, text, fullPath, settings.PreviewImagePath, settings.Publicity, usedTagsInternalNames, null, 0uL);
	}

	private string GetTemporaryFolderPath()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		string text = SteamUser.GetSteamID().m_SteamID.ToString();
		return _contentBaseFolder + Path.DirectorySeparatorChar + text + Path.DirectorySeparatorChar;
	}

	private bool MakeTemporaryFolder(string temporaryFolderPath)
	{
		bool result = true;
		if (!Utils.TryCreatingDirectory(temporaryFolderPath))
		{
			base.IssueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_CouldNotCreateTemporaryFolder!");
			result = false;
		}
		return result;
	}

	public override void ImportDownloadedWorldToLocalSaves(WorldFileData world, string newFileName = null, string newDisplayName = null)
	{
		Main.menuMode = 10;
		world.CopyToLocal(newFileName, newDisplayName);
	}

	public List<IssueReport> GetReports()
	{
		List<IssueReport> list = new List<IssueReport>();
		if (base.IssueReporter != null)
		{
			list.AddRange(base.IssueReporter.GetReports());
		}
		return list;
	}

	public override bool TryGetInfoForWorld(WorldFileData world, out FoundWorkshopEntryInfo info)
	{
		info = null;
		string text = GetTemporaryFolderPath() + world.GetFileName(includeExtension: false);
		if (!Directory.Exists(text))
		{
			return false;
		}
		if (AWorkshopEntry.TryReadingManifest(text + Path.DirectorySeparatorChar + "workshop.json", out info))
		{
			return true;
		}
		return false;
	}

	public override bool TryGetInfoForResourcePack(ResourcePack resourcePack, out FoundWorkshopEntryInfo info)
	{
		info = null;
		string fullPath = resourcePack.FullPath;
		if (!Directory.Exists(fullPath))
		{
			return false;
		}
		if (AWorkshopEntry.TryReadingManifest(fullPath + Path.DirectorySeparatorChar + "workshop.json", out info))
		{
			return true;
		}
		return false;
	}

	public override List<string> GetListOfMods()
	{
		return _downloader.ModPaths;
	}

	public override bool TryGetInfoForMod(TmodFile modFile, out FoundWorkshopEntryInfo info)
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		info = null;
		QueryParameters query = default(QueryParameters);
		query.searchModSlugs = new string[1] { modFile.Name };
		query.queryType = QueryType.SearchDirect;
		if (!WorkshopHelper.TryGetModDownloadItemsByInternalName(query, out var mods))
		{
			base.IssueReporter.ReportInstantUploadProblem("tModLoader.NoWorkshopAccess");
			return false;
		}
		if (!mods.Any())
		{
			return false;
		}
		currPublishID = ulong.Parse(mods[0].PublishId.m_ModPubId);
		existingAuthorID = ulong.Parse(mods[0].OwnerId);
		SteamedWraps.Download(new PublishedFileId_t(currPublishID), null, forceUpdate: true);
		return ModOrganizer.TryReadManifest(Path.Combine(Directory.GetParent(ModOrganizer.WorkshopFileFinder.ModPaths[0]).ToString(), $"{currPublishID}"), out info);
	}

	public override bool PublishMod(TmodFile modFile, NameValueCollection buildData, WorkshopItemPublishSettings settings)
	{
		try
		{
			return _PublishMod(modFile, buildData, settings);
		}
		catch
		{
			base.IssueReporter.ReportInstantUploadProblem("tModLoader.NoWorkshopAccess");
			return false;
		}
	}

	private bool _PublishMod(TmodFile modFile, NameValueCollection buildData, WorkshopItemPublishSettings settings)
	{
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		if (!SteamedWraps.SteamClient)
		{
			base.IssueReporter.ReportInstantUploadProblem("tModLoader.SteamPublishingLimit");
			return false;
		}
		if (modFile.TModLoaderVersion.MajorMinor() != BuildInfo.tMLVersion.MajorMinor())
		{
			base.IssueReporter.ReportInstantUploadProblem("tModLoader.WrongVersionCantPublishError");
			return false;
		}
		if (BuildInfo.IsDev)
		{
			base.IssueReporter.ReportInstantUploadProblem("tModLoader.BetaModCantPublishError");
			return false;
		}
		string workshopFolderPath = GetTemporaryFolderPath() + modFile.Name;
		buildData["versionsummary"] = $"{new Version(buildData["modloaderversion"])}:{buildData["version"]}";
		buildData["trueversion"] = buildData["version"];
		if (currPublishID != 0L)
		{
			CSteamID currID = SteamUser.GetSteamID();
			if (existingAuthorID != currID.m_SteamID)
			{
				base.IssueReporter.ReportInstantUploadProblem("tModLoader.ModAlreadyUploaded");
				return false;
			}
			workshopFolderPath = Path.Combine(Directory.GetParent(ModOrganizer.WorkshopFileFinder.ModPaths[0]).ToString(), $"{currPublishID}");
			FixErrorsInWorkshopFolder(workshopFolderPath);
			if (!CalculateVersionsData(workshopFolderPath, ref buildData))
			{
				base.IssueReporter.ReportInstantUploadProblem("tModLoader.ModVersionInfoUnchanged");
				return false;
			}
		}
		string name = buildData["displaynameclean"];
		if (name.Length >= 129)
		{
			base.IssueReporter.ReportInstantUploadProblem("tModLoader.TitleLengthExceedLimit");
			return false;
		}
		string description = buildData["description"] + "\n[quote=tModLoader]Developed By " + buildData["author"] + "[/quote]";
		if (description.Length >= 8000)
		{
			base.IssueReporter.ReportInstantUploadProblem("tModLoader.DescriptionLengthExceedLimit");
			return false;
		}
		List<string> tagsList = new List<string>();
		tagsList.AddRange(settings.GetUsedTagsInternalNames());
		tagsList.Add(buildData["modside"]);
		if (!TryCalculateWorkshopDeps(ref buildData))
		{
			base.IssueReporter.ReportInstantUploadProblem("tModLoader.NoWorkshopAccess");
			return false;
		}
		string contentFolderPath = $"{workshopFolderPath}/{BuildInfo.tMLVersion.Major}.{BuildInfo.tMLVersion.Minor}";
		if (MakeTemporaryFolder(contentFolderPath))
		{
			string modPath = Path.Combine(contentFolderPath, modFile.Name + ".tmod");
			File.Copy(modFile.path, modPath, overwrite: true);
			ModOrganizer.CleanupOldPublish(workshopFolderPath);
			tagsList.AddRange(DetermineSupportedVersionsFromWorkshop(workshopFolderPath));
			WorkshopHelper.ModPublisherInstance modPublisherInstance = new WorkshopHelper.ModPublisherInstance();
			_publisherInstances.Add(modPublisherInstance);
			modPublisherInstance.PublishContent(_publishedItems, base.IssueReporter, Forget, name, description, workshopFolderPath, settings.PreviewImagePath, settings.Publicity, tagsList.ToArray(), buildData, currPublishID, settings.ChangeNotes);
			return true;
		}
		return false;
	}

	public static bool CalculateVersionsData(string workshopPath, ref NameValueCollection buildData)
	{
		foreach (string item in Directory.EnumerateFiles(workshopPath, "*.tmod*", SearchOption.AllDirectories))
		{
			LocalMod mod = OpenModFile(item);
			if (mod.tModLoaderVersion.MajorMinor() <= BuildInfo.tMLVersion.MajorMinor() && mod.properties.version >= new Version(buildData["version"]))
			{
				return false;
			}
			if (mod.tModLoaderVersion.MajorMinor() != BuildInfo.tMLVersion.MajorMinor())
			{
				buildData["versionsummary"] += $";{mod.tModLoaderVersion}:{mod.properties.version}";
			}
		}
		return true;
	}

	internal static HashSet<string> DetermineSupportedVersionsFromWorkshop(string repo)
	{
		return (from info in ModOrganizer.AnalyzeWorkshopTmods(repo)
			select SocialBrowserModule.GetBrowserVersionNumber(info.tModVersion)).ToHashSet();
	}

	internal static LocalMod OpenModFile(string path)
	{
		TmodFile sModFile = new TmodFile(path);
		using (sModFile.Open())
		{
			return new LocalMod(sModFile);
		}
	}

	private static bool TryCalculateWorkshopDeps(ref NameValueCollection buildData)
	{
		string workshopDeps = "";
		if (buildData["modreferences"].Length > 0)
		{
			QueryParameters query = default(QueryParameters);
			query.searchModSlugs = buildData["modreferences"].Split(",");
			if (!WorkshopHelper.TryGetPublishIdByInternalName(query, out var modIds))
			{
				return false;
			}
			foreach (string modRef in modIds)
			{
				if (modRef != "0")
				{
					workshopDeps = workshopDeps + modRef + ",";
				}
			}
		}
		buildData["workshopdeps"] = workshopDeps;
		return true;
	}

	public static void FixErrorsInWorkshopFolder(string workshopFolderPath)
	{
		if (Directory.Exists(Path.Combine(workshopFolderPath, "bin")))
		{
			foreach (string item in Directory.EnumerateFiles(workshopFolderPath))
			{
				File.Delete(item);
			}
			foreach (string sourceFolder in Directory.EnumerateDirectories(workshopFolderPath))
			{
				if (!sourceFolder.Contains("2022.0"))
				{
					Directory.Delete(sourceFolder, recursive: true);
				}
			}
		}
		string devRemnant = Path.Combine(workshopFolderPath, "9999.0");
		if (Directory.Exists(devRemnant))
		{
			Directory.Delete(devRemnant, recursive: true);
		}
	}

	public static void SteamCMDPublishPreparer(string modFolder)
	{
		if (!Program.LaunchParameters.ContainsKey("-ciprep") || !Program.LaunchParameters.ContainsKey("-publishedmodfiles"))
		{
			return;
		}
		Console.WriteLine("Preparing Files for CI...");
		Program.LaunchParameters.TryGetValue("-ciprep", out var changeNotes);
		Program.LaunchParameters.TryGetValue("-publishedmodfiles", out var publishedModFiles);
		Program.LaunchParameters.TryGetValue("-uploadfolder", out var uploadFolder);
		string publishFolder = ModOrganizer.modPath + "/Workshop";
		string modName = Directory.GetParent(modFolder).Name;
		string newModPath = Path.Combine(ModOrganizer.modPath, modName + ".tmod");
		LocalMod newMod = OpenModFile(newModPath);
		NameValueCollection buildData = new NameValueCollection
		{
			["version"] = newMod.properties.version.ToString(),
			["versionsummary"] = $"{newMod.tModLoaderVersion}:{newMod.properties.version}",
			["description"] = newMod.properties.description
		};
		if (!CalculateVersionsData(publishedModFiles, ref buildData))
		{
			Utils.LogAndConsoleErrorMessage("Unable to update mod. " + buildData["version"] + " is not higher than existing version");
			return;
		}
		Console.WriteLine($"Built Mod Version is: {buildData["version"]}. tMod Version is: {BuildInfo.tMLVersion}");
		string contentFolder = $"{publishFolder}/{BuildInfo.tMLVersion.MajorMinor()}";
		if (!Directory.Exists(contentFolder))
		{
			Directory.CreateDirectory(contentFolder);
		}
		FileUtilities.CopyFolder(publishedModFiles, publishFolder);
		File.Copy(newModPath, Path.Combine(contentFolder, modName + ".tmod"), overwrite: true);
		ModOrganizer.CleanupOldPublish(publishFolder);
		string workshopDescFile = Path.Combine(modFolder, "description_workshop.txt");
		string descriptionFinal = string.Concat(str1: File.Exists(workshopDescFile) ? File.ReadAllText(workshopDescFile) : buildData["description"], str0: $"[quote=GithubActions(Don't Modify)]Version Summary {buildData["versionsummary"]}\nDeveloped By {buildData["author"]}[/quote]");
		AWorkshopEntry.TryReadingManifest(Path.Combine(publishedModFiles, "workshop.json"), out var steamInfo);
		string vdf = ModOrganizer.modPath + "/publish.vdf";
		string[] lines = new string[8]
		{
			"\"workshopitem\"",
			"{",
			"\"appid\" \"1281930\"",
			"\"publishedfileid\" \"" + steamInfo.workshopEntryId + "\"",
			"\"contentfolder\" \"" + uploadFolder + "/Workshop\"",
			"\"changenote\" \"" + changeNotes + "\"",
			"\"description\" \"" + descriptionFinal + "\"",
			"}"
		};
		if (File.Exists(vdf))
		{
			File.Delete(vdf);
		}
		File.WriteAllLines(vdf, lines);
		Console.WriteLine("CI Files Prepared");
	}
}
