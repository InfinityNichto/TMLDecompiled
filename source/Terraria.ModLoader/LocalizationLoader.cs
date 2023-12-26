using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Hjson;
using Newtonsoft.Json.Linq;
using Terraria.Localization;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.Utilities;

namespace Terraria.ModLoader;

public static class LocalizationLoader
{
	public record LocalizationFile(string path, string prefix, List<LocalizationEntry> Entries);

	public record LocalizationEntry(string key, string value, string comment, JsonType type = 0)
	{
		[CompilerGenerated]
		protected virtual bool PrintMembers(StringBuilder builder)
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("key = ");
			builder.Append((object?)key);
			builder.Append(", value = ");
			builder.Append((object?)value);
			builder.Append(", comment = ");
			builder.Append((object?)comment);
			builder.Append(", type = ");
			JsonType val = type;
			builder.Append(((object)(JsonType)(ref val)).ToString());
			return true;
		}

		[CompilerGenerated]
		public void Deconstruct(out string key, out string value, out string comment, out JsonType type)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected I4, but got Unknown
			key = this.key;
			value = this.value;
			comment = this.comment;
			type = (JsonType)(int)this.type;
		}
	}

	public class CommentedWscJsonObject : WscJsonObject
	{
		public List<string> CommentedOut { get; private set; }

		public CommentedWscJsonObject()
		{
			CommentedOut = new List<string>();
		}
	}

	private static readonly Dictionary<string, Dictionary<GameCulture, int>> localizationEntriesCounts = new Dictionary<string, Dictionary<GameCulture, int>>();

	private static Regex referenceRegex = new Regex("{\\$([\\w\\.]+)(?:@(\\d+))?}", RegexOptions.Compiled);

	private const int defaultWatcherCooldown = 60;

	private static readonly Dictionary<Mod, FileSystemWatcher> localizationFileWatchers = new Dictionary<Mod, FileSystemWatcher>();

	private static readonly HashSet<(string Mod, string fileName)> changedFiles = new HashSet<(string, string)>();

	private static readonly HashSet<(string Mod, string fileName)> pendingFiles = new HashSet<(string, string)>();

	internal static readonly HashSet<string> changedMods = new HashSet<string>();

	private static int watcherCooldown;

	internal static void Autoload(Mod mod)
	{
		LanguageManager lang = LanguageManager.Instance;
		string gameTipPrefix = "Mods." + mod.Name + ".GameTips.";
		foreach (var item in LoadTranslations(mod, GameCulture.DefaultCulture))
		{
			string key = item.key;
			LocalizedText text = lang.GetOrRegister(key);
			if (key.StartsWith(gameTipPrefix))
			{
				Main.gameTips.allTips.Add(new GameTipData(text, mod));
			}
		}
	}

	public static void LoadModTranslations(GameCulture culture)
	{
		LanguageManager lang = LanguageManager.Instance;
		Mod[] mods = ModLoader.Mods;
		for (int i = 0; i < mods.Length; i++)
		{
			foreach (var (key, value) in LoadTranslations(mods[i], culture))
			{
				lang.GetText(key).SetValue(value);
			}
		}
	}

	internal static void UpgradeLangFile(string langFile, string modName)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Invalid comparison between Unknown and I4
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		string[] array = File.ReadAllLines(langFile, Encoding.UTF8);
		JObject modObject = new JObject();
		JObject val = new JObject();
		val.Add(modName, (JToken)(object)modObject);
		JObject modsObject = val;
		JObject val2 = new JObject();
		val2.Add("Mods", (JToken)(object)modsObject);
		JObject rootObject = val2;
		string[] array2 = array;
		foreach (string line in array2)
		{
			if (line.Trim().StartsWith("#"))
			{
				continue;
			}
			int split = line.IndexOf('=');
			if (split < 0)
			{
				continue;
			}
			string key = line.Substring(0, split).Trim().Replace(" ", "_");
			string value = line.Substring(split + 1);
			if (value.Length == 0)
			{
				continue;
			}
			value = value.Replace("\\n", "\n");
			string[] splitKey = key.Split(".");
			JObject curObj = modObject;
			foreach (string i in splitKey.SkipLast(1))
			{
				if (!curObj.ContainsKey(i))
				{
					curObj.Add(i, (JToken)new JObject());
				}
				JToken existingVal = curObj.GetValue(i);
				if ((int)existingVal.Type == 1)
				{
					curObj = (JObject)existingVal;
					continue;
				}
				curObj[i] = (JToken)new JObject();
				curObj = (JObject)curObj.GetValue(i);
				curObj["$parentVal"] = existingVal;
			}
			string lastKey = splitKey.Last();
			if (curObj.ContainsKey(splitKey.Last()) && curObj[lastKey] is JObject)
			{
				((JObject)curObj[lastKey]).Add("$parentValue", JToken.op_Implicit(value));
			}
			curObj.Add(splitKey.Last(), JToken.op_Implicit(value));
		}
		string? path = Path.ChangeExtension(langFile, "hjson");
		string hjsonContents = JsonValue.Parse(((object)rootObject).ToString()).ToFancyHjsonString();
		File.WriteAllText(path, hjsonContents);
		File.Move(langFile, langFile + ".legacy", overwrite: true);
	}

	/// <summary>
	/// Derives a culture and shared prefix from a localization file path. Prefix will be found after culture, either separated by an underscore or nested in the folder.
	/// <br /> Some examples:<code>
	/// Localization/en-US_Mods.ExampleMod.hjson
	/// Localization/en-US/Mods.ExampleMod.hjson
	/// en-US_Mods.ExampleMod.hjson
	/// en-US/Mods.ExampleMod.hjson
	/// </code>
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static (GameCulture culture, string prefix) GetCultureAndPrefixFromPath(string path)
	{
		path = Path.ChangeExtension(path, null);
		GameCulture culture2 = null;
		string prefix = null;
		string[] array = path.Split("/");
		for (int i = 0; i < array.Length; i++)
		{
			string[] splitByUnderscore = array[i].Split("_");
			for (int underscoreSplitIndex = 0; underscoreSplitIndex < splitByUnderscore.Length; underscoreSplitIndex++)
			{
				string underscorePart = splitByUnderscore[underscoreSplitIndex];
				GameCulture parsedCulture = GameCulture.KnownCultures.FirstOrDefault((GameCulture culture) => culture.Name == underscorePart);
				if (parsedCulture != null)
				{
					culture2 = parsedCulture;
				}
				else if (parsedCulture == null && culture2 != null)
				{
					prefix = string.Join("_", splitByUnderscore.Skip(underscoreSplitIndex));
					return (culture: culture2, prefix: prefix);
				}
			}
		}
		if (culture2 != null)
		{
			return (culture: culture2, prefix: "");
		}
		Logging.tML.Warn((object)("The localization file " + path + " doesn't match expected file naming patterns, it will load as English"));
		return (culture: GameCulture.DefaultCulture, prefix: "");
	}

	private static List<(string key, string value)> LoadTranslations(Mod mod, GameCulture culture)
	{
		if (mod.File == null)
		{
			return new List<(string, string)>();
		}
		try
		{
			List<(string, string)> flattened = new List<(string, string)>();
			foreach (TmodFile.FileEntry translationFile in mod.File.Where((TmodFile.FileEntry entry) => Path.GetExtension(entry.Name) == ".hjson"))
			{
				var (fileCulture, prefix) = GetCultureAndPrefixFromPath(translationFile.Name);
				if (fileCulture != culture)
				{
					continue;
				}
				using Stream stream = mod.File.GetStream(translationFile);
				using StreamReader streamReader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
				string translationFileContents = streamReader.ReadToEnd();
				string modpath = Path.Combine(mod.Name, translationFile.Name).Replace('/', '\\');
				if (changedFiles.Select(((string Mod, string fileName) x) => Path.Join(x.Mod, x.fileName)).Contains(modpath))
				{
					string path2 = Path.Combine(ModCompile.ModSourcePath, modpath);
					if (File.Exists(path2))
					{
						try
						{
							translationFileContents = File.ReadAllText(path2);
						}
						catch (Exception)
						{
						}
					}
				}
				string jsonString;
				try
				{
					jsonString = ((object)HjsonValue.Parse(translationFileContents)).ToString();
				}
				catch (Exception e)
				{
					throw new Exception("The localization file \"" + translationFile.Name + "\" is malformed and failed to load: ", e);
				}
				foreach (JToken t in ((JToken)JObject.Parse(jsonString)).SelectTokens("$..*"))
				{
					if (t.HasValues)
					{
						continue;
					}
					JObject obj = (JObject)(object)((t is JObject) ? t : null);
					if (obj != null && ((JContainer)obj).Count == 0)
					{
						continue;
					}
					string path = "";
					JToken current = t;
					for (JToken parent = (JToken)(object)t.Parent; parent != null; parent = (JToken)(object)parent.Parent)
					{
						JProperty property = (JProperty)(object)((parent is JProperty) ? parent : null);
						string text;
						if (property == null)
						{
							JArray array = (JArray)(object)((parent is JArray) ? parent : null);
							text = ((array == null) ? path : (array.IndexOf(current) + ((path == string.Empty) ? string.Empty : ("." + path))));
						}
						else
						{
							text = property.Name + ((path == string.Empty) ? string.Empty : ("." + path));
						}
						path = text;
						current = parent;
					}
					path = path.Replace(".$parentVal", "");
					if (!string.IsNullOrWhiteSpace(prefix))
					{
						path = prefix + "." + path;
					}
					flattened.Add((path, ((object)t).ToString()));
				}
			}
			return flattened;
		}
		catch (Exception ex2)
		{
			ex2.Data["mod"] = mod.Name;
			throw;
		}
	}

	internal static void FinishSetup()
	{
		UpdateLocalizationFiles();
		SetupFileWatchers();
	}

	internal static void UpdateLocalizationFiles()
	{
		Mod[] mods = ModLoader.Mods;
		foreach (Mod mod in mods)
		{
			try
			{
				UpdateLocalizationFilesForMod(mod);
			}
			catch (Exception ex)
			{
				ex.Data["mod"] = mod.Name;
				throw;
			}
		}
	}

	private static void UpdateLocalizationFilesForMod(Mod mod, string outputPath = null, GameCulture specificCulture = null)
	{
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Expected O, but got Unknown
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Expected O, but got Unknown
		HashSet<GameCulture> desiredCultures = new HashSet<GameCulture>();
		if (specificCulture != null)
		{
			desiredCultures.Add(specificCulture);
		}
		List<Mod> mods = new List<Mod> { mod };
		string sourceFolder = outputPath ?? Path.Combine(ModCompile.ModSourcePath, mod.Name);
		if (!Directory.Exists(sourceFolder))
		{
			return;
		}
		string localBuiltTModFile = Path.Combine(ModLoader.ModPath, mod.Name + ".tmod");
		if (outputPath == null && !File.Exists(localBuiltTModFile))
		{
			return;
		}
		DateTime modLastModified = File.GetLastWriteTime(mod.File.path);
		if (mod.TranslationForMods != null)
		{
			foreach (string translationForMod in mod.TranslationForMods)
			{
				ModLoader.TryGetMod(translationForMod, out var otherMod);
				if (otherMod == null)
				{
					return;
				}
				mods.Add(otherMod);
				DateTime otherModLastModified = File.GetLastWriteTime(otherMod.File.path);
				if (otherModLastModified > modLastModified)
				{
					modLastModified = otherModLastModified;
				}
			}
		}
		Dictionary<GameCulture, List<LocalizationFile>> localizationFilesByCulture = new Dictionary<GameCulture, List<LocalizationFile>>();
		Dictionary<string, string> localizationFileContentsByPath = new Dictionary<string, string>();
		foreach (Mod inputMod in mods)
		{
			foreach (TmodFile.FileEntry translationFile in inputMod.File.Where((TmodFile.FileEntry entry) => Path.GetExtension(entry.Name) == ".hjson"))
			{
				using Stream stream = inputMod.File.GetStream(translationFile);
				using StreamReader streamReader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
				string translationFileContents = streamReader.ReadToEnd();
				(GameCulture culture, string prefix) cultureAndPrefixFromPath = GetCultureAndPrefixFromPath(translationFile.Name);
				GameCulture culture4 = cultureAndPrefixFromPath.culture;
				string prefix2 = cultureAndPrefixFromPath.prefix;
				string fixedFileName = translationFile.Name;
				if (culture4 == GameCulture.DefaultCulture && !fixedFileName.Contains("en-US"))
				{
					fixedFileName = Path.Combine(Path.GetDirectoryName(fixedFileName), "en-US.hjson").Replace("\\", "/");
				}
				if (!localizationFilesByCulture.TryGetValue(culture4, out var fileList2))
				{
					fileList2 = (localizationFilesByCulture[culture4] = new List<LocalizationFile>());
				}
				if (inputMod == mod)
				{
					desiredCultures.Add(culture4);
					if (!localizationFileContentsByPath.ContainsKey(translationFile.Name))
					{
						localizationFileContentsByPath[translationFile.Name] = translationFileContents;
					}
				}
				JsonValue jsonValueEng;
				try
				{
					jsonValueEng = HjsonValue.Parse(translationFileContents, new HjsonOptions
					{
						KeepWsc = true
					});
				}
				catch (Exception e2)
				{
					throw new Exception("The localization file \"" + translationFile.Name + "\" is malformed and failed to load: ", e2);
				}
				List<LocalizationEntry> entries = ParseLocalizationEntries((WscJsonObject)jsonValueEng, prefix2);
				if (!fileList2.Any((LocalizationFile x) => x.path == fixedFileName))
				{
					fileList2.Add(new LocalizationFile(fixedFileName, prefix2, entries));
					continue;
				}
				LocalizationFile localizationFile = fileList2.First((LocalizationFile x) => x.path == fixedFileName);
				foreach (LocalizationEntry entry4 in entries)
				{
					if (!localizationFile.Entries.Exists((LocalizationEntry x) => x.key == entry4.key))
					{
						localizationFile.Entries.Add(entry4);
					}
				}
			}
		}
		if (!localizationFilesByCulture.TryGetValue(GameCulture.DefaultCulture, out var baseLocalizationFiles))
		{
			localizationFilesByCulture[GameCulture.DefaultCulture] = (baseLocalizationFiles = new List<LocalizationFile>());
			desiredCultures.Add(GameCulture.DefaultCulture);
			string prefix = "Mods." + mod.Name;
			string translationFileName = "Localization/en-US_" + prefix + ".hjson";
			baseLocalizationFiles.Add(new LocalizationFile(translationFileName, prefix, new List<LocalizationEntry>()));
		}
		Dictionary<string, List<LocalizationEntry>> duplicates = (from w in baseLocalizationFiles.SelectMany((LocalizationFile f) => f.Entries)
			where (int)w.type == 0
			select w into x
			group x by x.key into c
			where c.Count() > 1
			select c).ToDictionary((IGrouping<string, LocalizationEntry> g) => g.Key, (IGrouping<string, LocalizationEntry> g) => g.ToList());
		foreach (LocalizationFile baseLocalizationFile in baseLocalizationFiles.OrderByDescending((LocalizationFile x) => x.path.Length))
		{
			List<LocalizationEntry> toRemove = new List<LocalizationEntry>();
			foreach (LocalizationEntry entry3 in baseLocalizationFile.Entries)
			{
				if (duplicates.ContainsKey(entry3.key))
				{
					duplicates.Remove(entry3.key);
					toRemove.Add(entry3);
				}
			}
			foreach (LocalizationEntry entry2 in toRemove)
			{
				baseLocalizationFile.Entries.Remove(entry2);
			}
		}
		HashSet<string> baseLocalizationKeys = baseLocalizationFiles.SelectMany((LocalizationFile f) => f.Entries.Select((LocalizationEntry e) => e.key)).ToHashSet();
		foreach (LocalizedText translation in LanguageManager.Instance._localizedTexts.Values)
		{
			if (translation.Key.StartsWith("Mods." + mod.Name + ".") && !baseLocalizationKeys.Contains(translation.Key))
			{
				LocalizationEntry newEntry = new LocalizationEntry(translation.Key, translation.Value, null, (JsonType)0);
				AddEntryToHJSON(FindHJSONFileForKey(baseLocalizationFiles, newEntry.key), newEntry.key, newEntry.value);
			}
		}
		IEnumerable<GameCulture> targetCultures = desiredCultures.ToList();
		if (specificCulture != null)
		{
			targetCultures = new GameCulture[1] { specificCulture };
			if (!localizationFilesByCulture.TryGetValue(specificCulture, out var fileList))
			{
				fileList = (localizationFilesByCulture[specificCulture] = new List<LocalizationFile>());
			}
		}
		foreach (GameCulture culture3 in targetCultures)
		{
			IEnumerable<LocalizationEntry> enumerable = localizationFilesByCulture[culture3].SelectMany((LocalizationFile f) => f.Entries);
			Dictionary<string, string> localizationsForCulture = new Dictionary<string, string>();
			foreach (LocalizationEntry localizationEntry in enumerable)
			{
				if (localizationEntry.value != null)
				{
					string key = localizationEntry.key;
					if (key.EndsWith(".$parentVal"))
					{
						key = key.Replace(".$parentVal", "");
					}
					localizationsForCulture[key] = localizationEntry.value;
				}
			}
			foreach (LocalizationFile item in baseLocalizationFiles)
			{
				string hjsonContents = LocalizationFileToHjsonText(item, localizationsForCulture).ReplaceLineEndings();
				string outputFileName = GetPathForCulture(item, culture3);
				string outputFilePath = Path.Combine(sourceFolder, outputFileName);
				DateTime dateTime = File.GetLastWriteTime(outputFilePath);
				if (!localizationFileContentsByPath.TryGetValue(outputFileName, out var existingFileContents) || (existingFileContents.ReplaceLineEndings() != hjsonContents && dateTime < modLastModified) || specificCulture != null)
				{
					Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath));
					File.WriteAllText(outputFilePath, hjsonContents);
					changedMods.Add(mod.Name);
				}
			}
		}
		HashSet<string> outputPathsForAllLangs = localizationFilesByCulture.Keys.SelectMany((GameCulture culture) => baseLocalizationFiles.Select((LocalizationFile baseFile) => GetPathForCulture(baseFile, culture))).ToHashSet();
		foreach (string name in localizationFileContentsByPath.Keys.Except(outputPathsForAllLangs))
		{
			string originalPath = Path.Combine(sourceFolder, name);
			string newPath = originalPath + ".legacy";
			if (File.Exists(originalPath))
			{
				File.Move(originalPath, newPath);
			}
		}
		if (specificCulture != null)
		{
			return;
		}
		Dictionary<GameCulture, int> localizationCounts = new Dictionary<GameCulture, int>();
		foreach (GameCulture culture2 in targetCultures)
		{
			int countNonTrivialEntries = (from x in localizationFilesByCulture[culture2].SelectMany((LocalizationFile f) => f.Entries).ToList()
				where HasTextThatNeedsLocalization(x.value)
				select x).Count();
			localizationCounts.Add(culture2, countNonTrivialEntries);
		}
		localizationEntriesCounts[mod.Name] = localizationCounts;
		string translationsNeededPath = Path.Combine(sourceFolder, "Localization", "TranslationsNeeded.txt");
		if (File.Exists(translationsNeededPath))
		{
			int countMaxEntries = localizationCounts.DefaultIfEmpty().Max((KeyValuePair<GameCulture, int> x) => x.Value);
			string neededText = string.Join(Environment.NewLine, from x in localizationCounts
				orderby x.Key.LegacyId
				select $"{x.Key.Name}, {x.Value}/{countMaxEntries}, {(float)x.Value / (float)countMaxEntries:0%}, missing {countMaxEntries - x.Value}") + Environment.NewLine;
			if (File.ReadAllText(translationsNeededPath).ReplaceLineEndings() != neededText.ReplaceLineEndings())
			{
				File.WriteAllText(translationsNeededPath, neededText);
			}
		}
	}

	private static string GetPathForCulture(LocalizationFile file, GameCulture culture)
	{
		return file.path.Replace("en-US", culture.CultureInfo.Name);
	}

	private static string LocalizationFileToHjsonText(LocalizationFile baseFile, Dictionary<string, string> localizationsForCulture)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Invalid comparison between Unknown and I4
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Invalid comparison between Unknown and I4
		Dictionary<string, int> prefixCounts = new Dictionary<string, int>();
		foreach (LocalizationEntry entry4 in baseFile.Entries)
		{
			if ((int)entry4.type != 2)
			{
				string[] splitKey2 = GetKeyFromFilePrefixAndEntry(baseFile, entry4).Split(".");
				for (int k = 0; k < splitKey2.Length; k++)
				{
					string partialKey2 = string.Join(".", splitKey2.Take(k + 1));
					prefixCounts.TryGetValue(partialKey2, out var count4);
					prefixCounts[partialKey2] = count4 + 1;
				}
			}
		}
		for (int i = baseFile.Entries.Count - 1; i >= 0; i--)
		{
			LocalizationEntry entry3 = baseFile.Entries[i];
			if ((int)entry3.type == 2)
			{
				string key2 = GetKeyFromFilePrefixAndEntry(baseFile, entry3);
				if (prefixCounts.TryGetValue(key2, out var count3) && count3 <= 1)
				{
					baseFile.Entries.RemoveAt(i);
				}
			}
			if ((int)entry3.type == 0)
			{
				string key = GetKeyFromFilePrefixAndEntry(baseFile, entry3);
				if (prefixCounts.TryGetValue(key, out var count2) && count2 > 1)
				{
					baseFile.Entries[i] = entry3 with
					{
						key = entry3.key + ".$parentVal"
					};
				}
			}
		}
		CommentedWscJsonObject rootObject = new CommentedWscJsonObject();
		foreach (LocalizationEntry entry2 in baseFile.Entries)
		{
			CommentedWscJsonObject parent2 = rootObject;
			string[] splitKey = GetKeyFromFilePrefixAndEntry(baseFile, entry2).Split(".");
			string finalKey = splitKey[^1];
			for (int j = 0; j < splitKey.Length - 1; j++)
			{
				string partialKey = string.Join(".", splitKey.Take(j + 1));
				if (prefixCounts.TryGetValue(partialKey, out var count) && count <= 1)
				{
					finalKey = string.Join(".", splitKey.Skip(j));
					break;
				}
				string l = splitKey[j];
				if (((JsonValue)parent2).ContainsKey(l))
				{
					parent2 = (CommentedWscJsonObject)(object)((JsonValue)parent2)[l];
					continue;
				}
				CommentedWscJsonObject newParent = new CommentedWscJsonObject();
				((JsonObject)parent2).Add(l, (JsonValue)(object)newParent);
				parent2 = newParent;
			}
			if (entry2.value == null && (int)entry2.type == 2)
			{
				if (!((JsonValue)parent2).ContainsKey(splitKey[^1]))
				{
					PlaceCommentAboveNewEntry(entry2, parent2);
					((JsonObject)parent2).Add(splitKey[^1], (JsonValue)(object)new CommentedWscJsonObject());
				}
				continue;
			}
			string realKey = entry2.key.Replace(".$parentVal", "");
			if (!localizationsForCulture.TryGetValue(realKey, out var value))
			{
				parent2.CommentedOut.Add(finalKey);
				value = entry2.value;
			}
			PlaceCommentAboveNewEntry(entry2, parent2);
			((JsonObject)parent2).Add(finalKey, JsonValue.op_Implicit(value));
		}
		return ((JsonValue)(object)rootObject).ToFancyHjsonString() + Environment.NewLine;
		static string GetKeyFromFilePrefixAndEntry(LocalizationFile baseLocalizationFileEntry, LocalizationEntry entry)
		{
			string key3 = entry.key;
			if (!string.IsNullOrWhiteSpace(baseLocalizationFileEntry.prefix))
			{
				key3 = key3.Substring(baseLocalizationFileEntry.prefix.Length + 1);
			}
			return key3;
		}
		static void PlaceCommentAboveNewEntry(LocalizationEntry entry, CommentedWscJsonObject parent)
		{
			if (((JsonValue)parent).Count == 0)
			{
				((WscJsonObject)parent).Comments[""] = entry.comment;
			}
			else
			{
				string actualCommentKey = ((JsonObject)parent).Keys.Last();
				((WscJsonObject)parent).Comments[actualCommentKey] = entry.comment;
			}
		}
	}

	private static List<LocalizationEntry> ParseLocalizationEntries(WscJsonObject jsonObjectEng, string prefix)
	{
		List<LocalizationEntry> existingKeys = new List<LocalizationEntry>();
		RecurseThrough(jsonObjectEng, prefix);
		return existingKeys;
		static string GetCommentFromIndex(int index, WscJsonObject original)
		{
			int actualOrderIndex = index - 1;
			string actualCommentKey = ((actualOrderIndex == -1) ? "" : original.Order[actualOrderIndex]);
			return original.Comments[actualCommentKey];
		}
		void RecurseThrough(WscJsonObject original, string prefix)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Invalid comparison between Unknown and I4
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			int index2 = 0;
			foreach (KeyValuePair<string, JsonValue> item in (IEnumerable<KeyValuePair<string, JsonValue>>)original)
			{
				if ((int)item.Value.JsonType == 2)
				{
					JsonValue value = item.Value;
					JsonValue obj = ((value is WscJsonObject) ? value : null);
					string newPrefix = (string.IsNullOrWhiteSpace(prefix) ? item.Key : (prefix + "." + item.Key));
					string comment = GetCommentFromIndex(index2, original);
					existingKeys.Add(new LocalizationEntry(newPrefix, null, comment, (JsonType)2));
					JsonObject obj2 = JsonUtil.Qo(obj);
					RecurseThrough((WscJsonObject)(object)((obj2 is WscJsonObject) ? obj2 : null), newPrefix);
				}
				else if ((int)item.Value.JsonType == 0)
				{
					string localizationValue = JsonUtil.Qs(item.Value);
					string key = (string.IsNullOrWhiteSpace(prefix) ? item.Key : (prefix + "." + item.Key));
					if (key.EndsWith(".$parentVal"))
					{
						key = key.Replace(".$parentVal", "");
					}
					string comment2 = GetCommentFromIndex(index2, original);
					existingKeys.Add(new LocalizationEntry(key, localizationValue, comment2, (JsonType)0));
				}
				index2++;
			}
		}
	}

	private static LocalizationFile FindHJSONFileForKey(List<LocalizationFile> files, string key)
	{
		int levelFound = -1;
		LocalizationFile best = null;
		foreach (LocalizationFile file in files)
		{
			if (string.IsNullOrWhiteSpace(file.prefix) || key.StartsWith(file.prefix))
			{
				int level = LongestMatchingPrefix(file, key);
				if (level > levelFound)
				{
					levelFound = level;
					best = file;
				}
			}
		}
		if (best == null)
		{
			best = new LocalizationFile("en-US.hjson", "", new List<LocalizationEntry>());
			files.Add(best);
		}
		return best;
	}

	internal static int LongestMatchingPrefix(LocalizationFile file, string key)
	{
		int num = ((!string.IsNullOrWhiteSpace(file.prefix)) ? file.prefix.Split(".").Length : 0);
		List<LocalizationEntry> localizationEntries = file.Entries;
		string[] splitKey = key.Split(".");
		for (int i = num; i < splitKey.Length; i++)
		{
			_ = splitKey[i];
			string partialKey = string.Join(".", splitKey.Take(i + 1));
			if (!localizationEntries.Any((LocalizationEntry x) => x.key.StartsWith(partialKey)))
			{
				return i;
			}
		}
		return splitKey.Length;
	}

	internal static void AddEntryToHJSON(LocalizationFile file, string key, string value, string comment = null)
	{
		int index = 0;
		string[] splitKey = key.Split(".");
		for (int i = 0; i < splitKey.Length - 1; i++)
		{
			_ = splitKey[i];
			string partialKey = string.Join(".", splitKey.Take(i + 1));
			int newIndex = file.Entries.FindLastIndex((LocalizationEntry x) => x.key.StartsWith(partialKey));
			if (newIndex != -1)
			{
				index = newIndex;
			}
		}
		int placementIndex = ((file.Entries.Count > 0) ? (index + 1) : 0);
		file.Entries.Insert(placementIndex, new LocalizationEntry(key, value, comment, (JsonType)0));
	}

	internal static bool ExtractLocalizationFiles(string modName)
	{
		string dir = Path.Combine(Main.SavePath, "ModLocalization", modName);
		if (Directory.Exists(dir))
		{
			Directory.Delete(dir, recursive: true);
		}
		Directory.CreateDirectory(dir);
		ModLoader.TryGetMod(modName, out var mod);
		if (mod == null)
		{
			Logging.tML.Error((object)("Somehow " + modName + " was not loaded"));
			return false;
		}
		UpdateLocalizationFilesForMod(mod, dir, Language.ActiveCulture);
		Utils.OpenFolder(dir);
		return true;
	}

	internal static Dictionary<GameCulture, int> GetLocalizationCounts(Mod mod)
	{
		if (localizationEntriesCounts.TryGetValue(mod.Name, out var results))
		{
			return results;
		}
		results = new Dictionary<GameCulture, int>();
		foreach (GameCulture culture in GameCulture.KnownCultures)
		{
			int countNonTrivialEntries = (from x in LoadTranslations(mod, culture)
				where HasTextThatNeedsLocalization(x.value)
				select x).Count();
			results.Add(culture, countNonTrivialEntries);
		}
		localizationEntriesCounts[mod.Name] = results;
		return results;
	}

	private static bool HasTextThatNeedsLocalization(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return false;
		}
		if (string.IsNullOrWhiteSpace(referenceRegex.Replace(value, "")))
		{
			return false;
		}
		return true;
	}

	private static void SetupFileWatchers()
	{
		Mod[] mods = ModLoader.Mods;
		foreach (Mod mod in mods)
		{
			string path = Path.Combine(ModCompile.ModSourcePath, mod.Name);
			if (!Directory.Exists(path))
			{
				continue;
			}
			try
			{
				FileSystemWatcher localizationFileWatcher = new FileSystemWatcher();
				localizationFileWatcher.Path = path;
				localizationFileWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
				localizationFileWatcher.Filter = "*.hjson";
				localizationFileWatcher.IncludeSubdirectories = true;
				localizationFileWatcher.Changed += delegate(object a, FileSystemEventArgs b)
				{
					HandleFileChangedOrRenamed(mod.Name, b.Name);
				};
				localizationFileWatcher.Renamed += delegate(object a, RenamedEventArgs b)
				{
					HandleFileChangedOrRenamed(mod.Name, b.Name);
				};
				localizationFileWatcher.EnableRaisingEvents = true;
				localizationFileWatchers[mod] = localizationFileWatcher;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}

	internal static void Unload()
	{
		LanguageManager.Instance.UnloadModdedEntries();
		UnloadFileWatchers();
	}

	private static void UnloadFileWatchers()
	{
		foreach (FileSystemWatcher value in localizationFileWatchers.Values)
		{
			value.EnableRaisingEvents = false;
			value.Dispose();
		}
		localizationFileWatchers.Clear();
	}

	private static void HandleFileChangedOrRenamed(string modName, string fileName)
	{
		watcherCooldown = 60;
		lock (pendingFiles)
		{
			pendingFiles.Add((modName, fileName));
		}
	}

	internal static void HandleModBuilt(string modName)
	{
		changedMods.Remove(modName);
		changedFiles.RemoveWhere(((string Mod, string fileName) x) => x.Mod == modName);
	}

	internal static void Update()
	{
		if (watcherCooldown <= 0)
		{
			return;
		}
		watcherCooldown--;
		if (watcherCooldown != 0)
		{
			return;
		}
		lock (pendingFiles)
		{
			Utils.LogAndChatAndConsoleInfoMessage(Language.GetTextValue("tModLoader.WatchLocalizationFileMessage", string.Join(", ", pendingFiles.Select(((string Mod, string fileName) x) => Path.Join(x.Mod, x.fileName)))));
		}
		lock (pendingFiles)
		{
			changedMods.UnionWith(pendingFiles.Select(((string Mod, string fileName) x) => x.Mod));
			changedFiles.UnionWith(pendingFiles);
			pendingFiles.Clear();
		}
		LanguageManager.Instance.ReloadLanguage();
	}
}
