using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Steamworks;
using Terraria.Localization;
using Terraria.ModLoader.Exceptions;
using Terraria.ModLoader.UI;
using Terraria.Social.Base;
using Terraria.Social.Steam;

namespace Terraria.ModLoader.Core;

/// <summary>
/// Responsible for sorting, dependency verification and organizing which mods to load
/// </summary>
internal static class ModOrganizer
{
	internal delegate void LocalModsChangedDelegate(HashSet<string> modSlugs, bool isDeletion);

	private enum SearchFolders
	{

	}

	internal static string modPath = Path.Combine(Main.SavePath, "Mods");

	internal static string commandLineModPack;

	private static Dictionary<string, LocalMod> modsDirCache = new Dictionary<string, LocalMod>();

	private static List<string> readFailures = new List<string>();

	internal static string lastLaunchedModsFilePath = Path.Combine(Main.SavePath, "LastLaunchedMods.txt");

	internal static List<(string ModName, Version previousVersion)> modsThatUpdatedSinceLastLaunch = new List<(string, Version)>();

	internal static WorkshopHelper.UGCBased.Downloader WorkshopFileFinder = new WorkshopHelper.UGCBased.Downloader();

	internal static string ModPackActive = null;

	private static readonly Regex PublishFolderMetadata = new Regex("[/|\\\\]([0-9]{4}[.][0-9]{1,2})[/|\\\\]");

	internal static event LocalModsChangedDelegate OnLocalModsChanged;

	internal static event LocalModsChangedDelegate PostLocalModsChanged;

	internal static void LocalModsChanged(HashSet<string> modSlugs, bool isDeletion)
	{
		ModOrganizer.OnLocalModsChanged?.Invoke(modSlugs, isDeletion);
		ModOrganizer.PostLocalModsChanged?.Invoke(modSlugs, isDeletion);
	}

	/// <summary>Mods in workshop folders, not in dev folder or modpacks</summary>
	internal static IReadOnlyList<LocalMod> FindWorkshopMods()
	{
		return _FindMods(ignoreModsFolder: true);
	}

	/// <summary>Mods in dev folder, not in workshop or modpacks</summary>
	internal static IReadOnlyList<LocalMod> FindDevFolderMods()
	{
		return _FindMods(ignoreModsFolder: false, ignoreWorkshop: true);
	}

	/// <summary>Mods from any location, using the default internal priority logic</summary>
	internal static LocalMod[] FindMods(bool logDuplicates = false)
	{
		return _FindMods(ignoreModsFolder: false, ignoreWorkshop: false, logDuplicates);
	}

	internal static LocalMod[] _FindMods(bool ignoreModsFolder = false, bool ignoreWorkshop = false, bool logDuplicates = false)
	{
		Directory.CreateDirectory(ModLoader.ModPath);
		List<LocalMod> mods = new List<LocalMod>();
		HashSet<string> names = new HashSet<string>();
		DeleteTemporaryFiles();
		WorkshopFileFinder.Refresh(new WorkshopIssueReporter());
		if (!ignoreModsFolder && !string.IsNullOrEmpty(ModPackActive))
		{
			if (Directory.Exists(ModPackActive))
			{
				Logging.tML.Info((object)("Loaded Mods from Active Mod Pack: " + ModPackActive));
				string[] files = Directory.GetFiles(ModPackActive, "*.tmod", SearchOption.AllDirectories);
				for (int i = 0; i < files.Length; i++)
				{
					AttemptLoadMod(files[i], ref mods, ref names, logDuplicates, devLocation: true);
				}
			}
			else
			{
				ModPackActive = null;
			}
		}
		if (!ignoreModsFolder)
		{
			string[] files = Directory.GetFiles(modPath, "*.tmod", SearchOption.TopDirectoryOnly);
			for (int i = 0; i < files.Length; i++)
			{
				AttemptLoadMod(files[i], ref mods, ref names, logDuplicates, devLocation: true);
			}
		}
		if (!ignoreWorkshop)
		{
			foreach (string modPath in WorkshopFileFinder.ModPaths)
			{
				string fileName = GetActiveTmodInRepo(modPath);
				if (fileName != null)
				{
					AttemptLoadMod(fileName, ref mods, ref names, logDuplicates, devLocation: false);
				}
			}
		}
		return mods.OrderBy<LocalMod, string>((LocalMod x) => x.Name, StringComparer.InvariantCulture).ToArray();
	}

	private static bool AttemptLoadMod(string fileName, ref List<LocalMod> mods, ref HashSet<string> names, bool logDuplicates, bool devLocation)
	{
		DateTime lastModified = File.GetLastWriteTime(fileName);
		if (!modsDirCache.TryGetValue(fileName, out var mod) || mod.lastModified != lastModified)
		{
			try
			{
				TmodFile modFile = new TmodFile(fileName);
				using (modFile.Open())
				{
					mod = new LocalMod(modFile)
					{
						lastModified = lastModified
					};
				}
				if (SkipModForPreviewNotPlayable(mod))
				{
					Logging.tML.Warn((object)$"Ignoring {mod.Name} found at: {fileName}. Preview Mod not available on Preview");
					return false;
				}
			}
			catch (Exception e)
			{
				if (!readFailures.Contains(fileName))
				{
					Logging.tML.Warn((object)("Failed to read " + fileName), e);
				}
				else
				{
					readFailures.Add(fileName);
				}
				return false;
			}
			modsDirCache[fileName] = mod;
		}
		if (names.Add(mod.Name))
		{
			mods.Add(mod);
		}
		else if (logDuplicates)
		{
			Logging.tML.Warn((object)$"Ignoring {mod.Name} found at: {fileName}. A mod with the same name already exists.");
		}
		return true;
	}

	internal static bool ApplyPreviewChecks(LocalMod mod)
	{
		if (BuildInfo.IsPreview)
		{
			return IsModFromSteam(mod.modFile.path);
		}
		return false;
	}

	internal static bool CheckStableBuildOnPreview(LocalMod mod)
	{
		if (ApplyPreviewChecks(mod))
		{
			return mod.properties.buildVersion.MajorMinor() <= BuildInfo.stableVersion.MajorMinor();
		}
		return false;
	}

	internal static bool SkipModForPreviewNotPlayable(LocalMod mod)
	{
		if (ApplyPreviewChecks(mod) && !mod.properties.playableOnPreview)
		{
			return mod.properties.buildVersion.MajorMinor() > BuildInfo.stableVersion;
		}
		return false;
	}

	internal static bool IsModFromSteam(string modPath)
	{
		return modPath.Contains(Path.Combine("workshop"), StringComparison.InvariantCultureIgnoreCase);
	}

	internal static HashSet<string> IdentifyMissingWorkshopDependencies()
	{
		IReadOnlyList<LocalMod> mods = FindWorkshopMods();
		string[] installedSlugs = mods.Select((LocalMod s) => s.Name).ToArray();
		HashSet<string> missingModSlugs = new HashSet<string>();
		foreach (LocalMod item in mods.Where((LocalMod m) => m.properties.modReferences.Length != 0))
		{
			IEnumerable<string> missingDeps = from dep in item.properties.modReferences
				select dep.mod into slug
				where !installedSlugs.Contains(slug)
				select slug;
			missingModSlugs.UnionWith(missingDeps);
		}
		return missingModSlugs;
	}

	/// <summary>
	/// Returns changes based on last time <see cref="M:Terraria.ModLoader.Core.ModOrganizer.SaveLastLaunchedMods" /> was called. Can be null if no changes.
	/// </summary>
	internal static string DetectModChangesForInfoMessage()
	{
		if (!ModLoader.showNewUpdatedModsInfo || !File.Exists(lastLaunchedModsFilePath))
		{
			return null;
		}
		Dictionary<string, LocalMod> currMods = FindWorkshopMods().ToDictionary((LocalMod mod) => mod.Name, (LocalMod mod) => mod);
		Logging.tML.Info((object)("3 most recently changed workshop mods: " + string.Join(", ", from x in currMods.OrderByDescending((KeyValuePair<string, LocalMod> x) => x.Value.lastModified).Take(3)
			select $"{x.Value.Name} v{x.Value.properties.version} {x.Value.lastModified:d}")));
		try
		{
			IEnumerable<string> enumerable = File.ReadLines(lastLaunchedModsFilePath);
			Dictionary<string, Version> lastMods = new Dictionary<string, Version>();
			foreach (string item2 in enumerable)
			{
				string[] parts = item2.Split(' ');
				if (parts.Length == 2)
				{
					string name3 = parts[0];
					string versionString = parts[1];
					lastMods.Add(name3, new Version(versionString));
				}
			}
			List<string> newMods = new List<string>();
			List<string> updatedMods = new List<string>();
			StringBuilder messages = new StringBuilder();
			foreach (KeyValuePair<string, LocalMod> item in currMods)
			{
				string name2 = item.Key;
				Version version = item.Value.properties.version;
				Version lastVersion2;
				if (!lastMods.ContainsKey(name2))
				{
					newMods.Add(name2);
					modsThatUpdatedSinceLastLaunch.Add((name2, null));
				}
				else if (lastMods.TryGetValue(name2, out lastVersion2) && lastVersion2 < version)
				{
					updatedMods.Add(name2);
					modsThatUpdatedSinceLastLaunch.Add((name2, lastVersion2));
				}
			}
			if (newMods.Count > 0)
			{
				messages.Append(Language.GetTextValue("tModLoader.ShowNewUpdatedModsInfoMessageNewMods"));
				foreach (string newMod in newMods)
				{
					StringBuilder stringBuilder = messages;
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(6, 2, stringBuilder);
					handler.AppendLiteral("\n  ");
					handler.AppendFormatted(newMod);
					handler.AppendLiteral(" (");
					handler.AppendFormatted(currMods[newMod].DisplayName);
					handler.AppendLiteral(")");
					stringBuilder2.Append(ref handler);
				}
			}
			if (updatedMods.Count > 0)
			{
				messages.Append(Language.GetTextValue("tModLoader.ShowNewUpdatedModsInfoMessageUpdatedMods"));
				foreach (string name in updatedMods)
				{
					string displayName = currMods[name].DisplayName;
					Version lastVersion = lastMods[name];
					Version currVersion = currMods[name].properties.version;
					StringBuilder stringBuilder = messages;
					StringBuilder stringBuilder3 = stringBuilder;
					StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(13, 4, stringBuilder);
					handler.AppendLiteral("\n  ");
					handler.AppendFormatted(name);
					handler.AppendLiteral(" (");
					handler.AppendFormatted(displayName);
					handler.AppendLiteral(") v");
					handler.AppendFormatted(lastVersion);
					handler.AppendLiteral(" -> v");
					handler.AppendFormatted(currVersion);
					stringBuilder3.Append(ref handler);
				}
			}
			return (messages.Length > 0) ? messages.ToString() : null;
		}
		catch
		{
			return null;
		}
	}

	/// <summary>
	/// Collects local mod status and saves it to a file.
	/// </summary>
	internal static void SaveLastLaunchedMods()
	{
		if (Main.dedServ || !ModLoader.showNewUpdatedModsInfo)
		{
			return;
		}
		IReadOnlyList<LocalMod> readOnlyList = FindWorkshopMods();
		StringBuilder fileText = new StringBuilder();
		foreach (LocalMod mod in readOnlyList)
		{
			StringBuilder stringBuilder = fileText;
			StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(2, 2, stringBuilder);
			handler.AppendFormatted(mod.Name);
			handler.AppendLiteral(" ");
			handler.AppendFormatted(mod.properties.version);
			handler.AppendLiteral("\n");
			stringBuilder.Append(ref handler);
		}
		File.WriteAllText(lastLaunchedModsFilePath, fileText.ToString());
	}

	private static void DeleteTemporaryFiles()
	{
		foreach (string path in GetTemporaryFiles())
		{
			Logging.tML.Info((object)("Cleaning up leftover temporary file " + Path.GetFileName(path)));
			try
			{
				File.Delete(path);
			}
			catch (Exception e)
			{
				Logging.tML.Error((object)("Could not delete leftover temporary file " + Path.GetFileName(path)), e);
			}
		}
	}

	private static IEnumerable<string> GetTemporaryFiles()
	{
		return Directory.GetFiles(modPath, "*.tmp", SearchOption.TopDirectoryOnly).Union(Directory.GetFiles(modPath, "temporaryDownload.tmod", SearchOption.TopDirectoryOnly));
	}

	internal static bool LoadSide(ModSide side)
	{
		return side != (ModSide)(Main.dedServ ? 1 : 2);
	}

	internal static List<LocalMod> SelectAndSortMods(IEnumerable<LocalMod> availableMods, CancellationToken token)
	{
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		List<string> missing = ModLoader.EnabledMods.Except(availableMods.Select((LocalMod mod) => mod.Name)).ToList();
		if (missing.Any())
		{
			Logging.tML.Info((object)("Missing previously enabled mods: " + string.Join(", ", missing)));
			foreach (string name in missing)
			{
				ModLoader.EnabledMods.Remove(name);
			}
			SaveEnabledMods();
		}
		if ((((Game)Main.instance).IsActive && Main.oldKeyState.PressingShift()) || ModLoader.skipLoad || token.IsCancellationRequested)
		{
			ModLoader.skipLoad = false;
			Interface.loadMods.SetLoadStage("tModLoader.CancellingLoading");
			return new List<LocalMod>();
		}
		CommandLineModPackOverride(availableMods);
		Interface.loadMods.SetLoadStage("tModLoader.MSFinding");
		foreach (LocalMod item in GetModsToLoad(availableMods))
		{
			EnableWithDeps(item, availableMods);
		}
		SaveEnabledMods();
		List<LocalMod> modsToLoad = GetModsToLoad(availableMods);
		try
		{
			EnsureDependenciesExist(modsToLoad, includeWeak: false);
			EnsureTargetVersionsMet(modsToLoad);
			return Sort(modsToLoad);
		}
		catch (ModSortingException e)
		{
			e.Data["mods"] = e.errored.Select((LocalMod m) => m.Name).ToArray();
			e.Data["hideStackTrace"] = true;
			throw;
		}
	}

	private static List<LocalMod> GetModsToLoad(IEnumerable<LocalMod> availableMods)
	{
		List<LocalMod> list = availableMods.Where((LocalMod mod) => mod.Enabled && LoadSide(mod.properties.side)).ToList();
		VerifyNames(list);
		return list;
	}

	private static void CommandLineModPackOverride(IEnumerable<LocalMod> mods)
	{
		if (string.IsNullOrWhiteSpace(commandLineModPack))
		{
			return;
		}
		if (!commandLineModPack.EndsWith(".json"))
		{
			commandLineModPack += ".json";
		}
		string filePath = Path.Combine(UIModPacks.ModPacksDirectory, commandLineModPack);
		try
		{
			Directory.CreateDirectory(UIModPacks.ModPacksDirectory);
			Logging.ServerConsoleLine(Language.GetTextValue("tModLoader.LoadingSpecifiedModPack", commandLineModPack));
			HashSet<string> modSet = JsonConvert.DeserializeObject<HashSet<string>>(File.ReadAllText(filePath));
			foreach (LocalMod mod in mods)
			{
				mod.Enabled = modSet.Contains(mod.Name);
			}
		}
		catch (Exception e)
		{
			throw new Exception((e is FileNotFoundException) ? Language.GetTextValue("tModLoader.ModPackDoesNotExist", filePath) : Language.GetTextValue("tModLoader.ModPackMalformed", commandLineModPack), e);
		}
		finally
		{
			commandLineModPack = null;
		}
	}

	internal static void EnableWithDeps(LocalMod mod, IEnumerable<LocalMod> availableMods)
	{
		mod.Enabled = true;
		foreach (string depName in mod.properties.RefNames(includeWeak: false))
		{
			LocalMod dep = availableMods.SingleOrDefault((LocalMod m) => m.Name == depName);
			if (dep != null && !dep.Enabled)
			{
				EnableWithDeps(dep, availableMods);
			}
		}
	}

	private static void VerifyNames(List<LocalMod> mods)
	{
		HashSet<string> names = new HashSet<string>();
		List<string> errors = new List<string>();
		List<LocalMod> erroredMods = new List<LocalMod>();
		foreach (LocalMod mod in mods)
		{
			if (mod.Name.Length == 0)
			{
				errors.Add(Language.GetTextValue("tModLoader.BuildErrorModNameEmpty"));
			}
			else if (mod.Name.Equals("Terraria", StringComparison.InvariantCultureIgnoreCase))
			{
				errors.Add(Language.GetTextValue("tModLoader.BuildErrorModNamedTerraria"));
			}
			else if (mod.Name.IndexOf('.') >= 0)
			{
				errors.Add(Language.GetTextValue("tModLoader.BuildErrorModNameHasPeriod"));
			}
			else
			{
				if (names.Add(mod.Name))
				{
					continue;
				}
				errors.Add(Language.GetTextValue("tModLoader.BuildErrorTwoModsSameName", mod.Name));
			}
			erroredMods.Add(mod);
		}
		if (erroredMods.Count > 0)
		{
			Exception ex = new Exception(string.Join("\n", errors));
			ex.Data["mods"] = erroredMods.Select((LocalMod m) => m.Name).ToArray();
			throw ex;
		}
	}

	internal static void EnsureDependenciesExist(ICollection<LocalMod> mods, bool includeWeak)
	{
		Dictionary<string, LocalMod> nameMap = mods.ToDictionary((LocalMod mod) => mod.Name);
		HashSet<LocalMod> errored = new HashSet<LocalMod>();
		StringBuilder errorLog = new StringBuilder();
		foreach (LocalMod mod2 in mods)
		{
			foreach (string depName in mod2.properties.RefNames(includeWeak))
			{
				if (!nameMap.ContainsKey(depName))
				{
					errored.Add(mod2);
					errorLog.AppendLine(Language.GetTextValue("tModLoader.LoadErrorDependencyMissing", depName, mod2));
				}
			}
		}
		if (errored.Count > 0)
		{
			throw new ModSortingException(errored, errorLog.ToString());
		}
	}

	internal static void EnsureTargetVersionsMet(ICollection<LocalMod> mods)
	{
		Dictionary<string, LocalMod> nameMap = mods.ToDictionary((LocalMod mod) => mod.Name);
		HashSet<LocalMod> errored = new HashSet<LocalMod>();
		StringBuilder errorLog = new StringBuilder();
		foreach (LocalMod mod2 in mods)
		{
			foreach (BuildProperties.ModReference dep in mod2.properties.Refs(includeWeak: true))
			{
				if (!(dep.target == null) && nameMap.TryGetValue(dep.mod, out var inst))
				{
					if (inst.properties.version < dep.target)
					{
						errored.Add(mod2);
						errorLog.AppendLine(Language.GetTextValue("tModLoader.LoadErrorDependencyVersionTooLow", mod2, dep.target, dep.mod, inst.properties.version));
					}
					else if (inst.properties.version.Major != dep.target.Major)
					{
						errored.Add(mod2);
						errorLog.AppendLine(Language.GetTextValue("tModLoader.LoadErrorMajorVersionMismatch", mod2, dep.target, dep.mod, inst.properties.version));
					}
				}
			}
		}
		if (errored.Count > 0)
		{
			throw new ModSortingException(errored, errorLog.ToString());
		}
	}

	internal static void EnsureSyncedDependencyStability(TopoSort<LocalMod> synced, TopoSort<LocalMod> full)
	{
		HashSet<LocalMod> errored = new HashSet<LocalMod>();
		StringBuilder errorLog = new StringBuilder();
		foreach (LocalMod mod in synced.list)
		{
			List<List<LocalMod>> chains = new List<List<LocalMod>>();
			Action<LocalMod, Stack<LocalMod>> FindChains = null;
			FindChains = delegate(LocalMod search, Stack<LocalMod> stack)
			{
				stack.Push(search);
				if (search.properties.side == ModSide.Both && stack.Count > 1)
				{
					if (stack.Count > 2)
					{
						chains.Add(stack.Reverse().ToList());
					}
				}
				else
				{
					foreach (LocalMod current in full.Dependencies(search))
					{
						FindChains(current, stack);
					}
				}
				stack.Pop();
			};
			FindChains(mod, new Stack<LocalMod>());
			if (chains.Count == 0)
			{
				continue;
			}
			ISet<LocalMod> syncedDependencies = synced.AllDependencies(mod);
			foreach (List<LocalMod> chain in chains)
			{
				if (!syncedDependencies.Contains(chain.Last()))
				{
					errored.Add(mod);
					errorLog.AppendLine(mod?.ToString() + " indirectly depends on " + chain.Last()?.ToString() + " via " + string.Join(" -> ", chain));
				}
			}
		}
		if (errored.Count > 0)
		{
			errorLog.AppendLine("Some of these mods may not exist on both client and server. Add a direct sort entries or weak references.");
			throw new ModSortingException(errored, errorLog.ToString());
		}
	}

	private static TopoSort<LocalMod> BuildSort(ICollection<LocalMod> mods)
	{
		Dictionary<string, LocalMod> nameMap = mods.ToDictionary((LocalMod mod) => mod.Name);
		return new TopoSort<LocalMod>(mods, (LocalMod mod) => from name in mod.properties.sortAfter.Where(nameMap.ContainsKey)
			select nameMap[name], (LocalMod mod) => from name in mod.properties.sortBefore.Where(nameMap.ContainsKey)
			select nameMap[name]);
	}

	internal static List<LocalMod> Sort(ICollection<LocalMod> mods)
	{
		List<LocalMod> list = mods.OrderBy((LocalMod mod) => mod.Name).ToList();
		TopoSort<LocalMod> syncedSort = BuildSort(list.Where((LocalMod mod) => mod.properties.side == ModSide.Both).ToList());
		TopoSort<LocalMod> fullSort = BuildSort(list);
		EnsureSyncedDependencyStability(syncedSort, fullSort);
		try
		{
			List<LocalMod> syncedList = syncedSort.Sort();
			for (int i = 1; i < syncedList.Count; i++)
			{
				fullSort.AddEntry(syncedList[i - 1], syncedList[i]);
			}
			return fullSort.Sort();
		}
		catch (TopoSort<LocalMod>.SortingException e)
		{
			throw new ModSortingException(e.set, e.Message);
		}
	}

	internal static void SaveEnabledMods()
	{
		Directory.CreateDirectory(ModLoader.ModPath);
		string json = JsonConvert.SerializeObject((object)ModLoader.EnabledMods, (Formatting)1);
		File.WriteAllText(Path.Combine(modPath, "enabled.json"), json);
	}

	internal static HashSet<string> LoadEnabledMods()
	{
		try
		{
			string path = Path.Combine(modPath, "enabled.json");
			if (!File.Exists(path))
			{
				Logging.tML.Warn((object)"Did not find enabled.json file");
				return new HashSet<string>();
			}
			return JsonConvert.DeserializeObject<HashSet<string>>(File.ReadAllText(path)) ?? new HashSet<string>();
		}
		catch (Exception e)
		{
			Logging.tML.Warn((object)"Unknown error occurred when trying to read enabled.json", e);
			return new HashSet<string>();
		}
	}

	internal static string GetActiveTmodInRepo(string repo)
	{
		IEnumerable<(string, Version, bool)> information = from t in AnalyzeWorkshopTmods(repo)
			where !SocialBrowserModule.GetBrowserVersionNumber(t.tModVersion).Contains("Transitive")
			select t;
		if (information == null || information.Count() == 0)
		{
			Logging.tML.Warn((object)("Unexpectedly missing .tMods in Workshop Folder " + repo));
			return null;
		}
		(string, Version, bool) recommendedTmod = (from t in information
			where t.tModVersion <= BuildInfo.tMLVersion
			orderby t.tModVersion descending
			select t).FirstOrDefault();
		(string, Version, bool) tuple = recommendedTmod;
		if (tuple.Item1 == null && tuple.Item2 == null && !tuple.Item3)
		{
			Logging.tML.Warn((object)("No .tMods found for this version in Workshop Folder " + repo + ". Defaulting to show newest"));
			return information.OrderByDescending<(string, Version, bool), Version>(((string file, Version tModVersion, bool isInFolder) t) => t.tModVersion).First().Item1;
		}
		return recommendedTmod.Item1;
	}

	/// <summary>
	/// Must Be called AFTER the new files are added to the publishing repo.
	/// Assumes one .tmod per YYYY.XX folder in the publishing repo
	/// </summary>
	/// <param name="repo"></param>
	internal static void CleanupOldPublish(string repo)
	{
		if (BuildInfo.IsPreview)
		{
			RemoveSkippablePreview(repo);
		}
		if (Directory.GetFiles(repo, "*.tmod", SearchOption.AllDirectories).Length <= 3)
		{
			return;
		}
		List<(string, Version, bool)> information = AnalyzeWorkshopTmods(repo);
		if (information == null || information.Count() <= 3)
		{
			return;
		}
		(string, int)[] array = new(string, int)[4]
		{
			("1.4.3", 1),
			("1.4.4", 3),
			("1.3", 1),
			("1.4.4-Transitive", 0)
		};
		for (int i = 0; i < array.Length; i++)
		{
			(string, int) requirement = array[i];
			foreach (var item in GetOrderedTmodWorkshopInfoForVersion(information, requirement.Item1).Skip(requirement.Item2))
			{
				if (item.isInFolder)
				{
					Directory.Delete(Path.GetDirectoryName(item.file), recursive: true);
				}
				else
				{
					File.Delete(item.file);
				}
			}
		}
	}

	internal static IOrderedEnumerable<(string file, Version tModVersion, bool isInFolder)> GetOrderedTmodWorkshopInfoForVersion(List<(string file, Version tModVersion, bool isInFolder)> information, string tmlVersion)
	{
		return from t in information
			where SocialBrowserModule.GetBrowserVersionNumber(t.tModVersion) == tmlVersion
			orderby t.tModVersion descending
			select t;
	}

	internal static List<(string file, Version tModVersion, bool isInFolder)> AnalyzeWorkshopTmods(string repo)
	{
		string[] files = Directory.GetFiles(repo, "*.tmod", SearchOption.AllDirectories);
		List<(string, Version, bool)> information = new List<(string, Version, bool)>();
		string[] array = files;
		foreach (string filename in array)
		{
			Match match = PublishFolderMetadata.Match(filename);
			if (match.Success)
			{
				information.Add((filename, new Version(match.Groups[1].Value), true));
			}
			else
			{
				information.Add((filename, new Version(0, 12), false));
			}
		}
		return information;
	}

	private static void RemoveSkippablePreview(string repo)
	{
		string[] tmods = Directory.GetFiles(repo, "*.tmod", SearchOption.AllDirectories);
		foreach (string filename in tmods)
		{
			Match match = PublishFolderMetadata.Match(filename);
			if (match.Success)
			{
				Version checkVersion = new Version(match.Groups[1].Value);
				if (checkVersion > BuildInfo.stableVersion && checkVersion < BuildInfo.tMLVersion.MajorMinor())
				{
					Directory.Delete(Path.GetDirectoryName(filename), recursive: true);
				}
			}
		}
	}

	internal static void DeleteMod(LocalMod tmod)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		string tmodPath = tmod.modFile.path;
		string parentDir = GetParentDir(tmodPath);
		if (TryReadManifest(parentDir, out var info))
		{
			SteamedWraps.UninstallWorkshopItem(new PublishedFileId_t(info.workshopEntryId), parentDir);
		}
		else
		{
			File.Delete(tmodPath);
		}
		LocalModsChanged(new HashSet<string> { tmod.Name }, isDeletion: true);
	}

	internal static bool TryReadManifest(string parentDir, out FoundWorkshopEntryInfo info)
	{
		info = null;
		if (!IsModFromSteam(parentDir))
		{
			return false;
		}
		return AWorkshopEntry.TryReadingManifest(parentDir + Path.DirectorySeparatorChar + "workshop.json", out info);
	}

	internal static string GetParentDir(string tmodPath)
	{
		string parentDir = Directory.GetParent(tmodPath).ToString();
		if (!IsModFromSteam(parentDir))
		{
			return parentDir;
		}
		if (PublishFolderMetadata.Match(parentDir + Path.DirectorySeparatorChar).Success)
		{
			parentDir = Directory.GetParent(parentDir).ToString();
		}
		return parentDir;
	}
}
