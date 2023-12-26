using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using CsvHelper;
using Newtonsoft.Json;
using ReLogic.Content.Sources;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.Initializers;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Terraria.Localization;

public class LanguageManager
{
	private struct TextBinding : IEquatable<TextBinding>
	{
		public readonly string key;

		public readonly object[] args;

		public TextBinding(string key, object[] args)
		{
			this.key = key;
			this.args = args;
		}

		public bool Equals(TextBinding other)
		{
			if (key == other.key)
			{
				return args.SequenceEqual(other.args);
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is TextBinding)
			{
				return Equals((TextBinding)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			HashCode hash = default(HashCode);
			hash.Add(key);
			object[] array = args;
			foreach (object arg in array)
			{
				hash.Add(arg);
			}
			return hash.ToHashCode();
		}
	}

	public static LanguageManager Instance = new LanguageManager();

	internal readonly Dictionary<string, LocalizedText> _localizedTexts = new Dictionary<string, LocalizedText>();

	private readonly Dictionary<string, List<string>> _categoryGroupedKeys = new Dictionary<string, List<string>>();

	private GameCulture _fallbackCulture = GameCulture.DefaultCulture;

	private IContentSource[] _contentSources = Array.Empty<IContentSource>();

	private HashSet<string> _moddedKeys = new HashSet<string>();

	private Dictionary<TextBinding, LocalizedText> boundTextCache = new Dictionary<TextBinding, LocalizedText>();

	private List<LocalizedText> boundTexts = new List<LocalizedText>();

	public GameCulture ActiveCulture { get; private set; }

	public event LanguageChangeCallback OnLanguageChanging;

	public event LanguageChangeCallback OnLanguageChanged;

	private LanguageManager()
	{
		_localizedTexts[""] = LocalizedText.Empty;
	}

	public int GetCategorySize(string name)
	{
		if (_categoryGroupedKeys.ContainsKey(name))
		{
			return _categoryGroupedKeys[name].Count;
		}
		return 0;
	}

	public void SetLanguage(int legacyId)
	{
		GameCulture language = GameCulture.FromLegacyId(legacyId);
		SetLanguage(language);
	}

	public void SetLanguage(string cultureName)
	{
		GameCulture language = GameCulture.FromName(cultureName);
		SetLanguage(language);
	}

	public int EstimateWordCount()
	{
		int num = 0;
		foreach (string key in _localizedTexts.Keys)
		{
			string textValue = GetTextValue(key);
			textValue.Replace(",", "").Replace(".", "").Replace("\"", "")
				.Trim();
			string[] array = textValue.Split(' ');
			string[] array2 = textValue.Split(' ');
			if (array.Length == array2.Length)
			{
				string[] array3 = array;
				foreach (string text in array3)
				{
					if (!string.IsNullOrWhiteSpace(text) && text.Length >= 1)
					{
						num++;
					}
				}
				continue;
			}
			return num;
		}
		return num;
	}

	private void SetAllTextValuesToKeys()
	{
		foreach (KeyValuePair<string, LocalizedText> localizedText in _localizedTexts)
		{
			localizedText.Value.SetValue(localizedText.Key);
		}
	}

	private string[] GetLanguageFilesForCulture(GameCulture culture)
	{
		Assembly.GetExecutingAssembly();
		return Array.FindAll(typeof(Program).Assembly.GetManifestResourceNames(), (string element) => element.StartsWith("Terraria.Localization.Content." + culture.CultureInfo.Name.Replace('-', '_')) && element.EndsWith(".json"));
	}

	public void SetLanguage(GameCulture culture)
	{
		if (ActiveCulture != culture)
		{
			ActiveCulture = culture;
			ReloadLanguage();
			Thread.CurrentThread.CurrentCulture = culture.CultureInfo;
			Thread.CurrentThread.CurrentUICulture = culture.CultureInfo;
			if (this.OnLanguageChanged != null)
			{
				this.OnLanguageChanged(this);
			}
			_ = FontAssets.DeathText;
		}
	}

	internal void ReloadLanguage(bool resetValuesToKeysFirst = true)
	{
		if (resetValuesToKeysFirst)
		{
			SetAllTextValuesToKeys();
		}
		LoadFilesForCulture(_fallbackCulture);
		if (ActiveCulture != _fallbackCulture)
		{
			LoadFilesForCulture(ActiveCulture);
		}
		LoadActiveCultureTranslationsFromSources();
		ProcessCopyCommandsInTexts();
		RecalculateBoundTextValues();
		ChatInitializer.PrepareAliases();
		SystemLoader.OnLocalizationsLoaded();
	}

	private void LoadFilesForCulture(GameCulture culture)
	{
		string[] languageFilesForCulture = GetLanguageFilesForCulture(culture);
		foreach (string text in languageFilesForCulture)
		{
			try
			{
				string text2 = Utils.ReadEmbeddedResource(text);
				if (text2 == null || text2.Length < 2)
				{
					throw new FormatException();
				}
				LoadLanguageFromFileTextJson(text2, canCreateCategories: true);
			}
			catch (Exception)
			{
				if (Debugger.IsAttached)
				{
					Debugger.Break();
				}
				Console.WriteLine("Failed to load language file: " + text);
				break;
			}
		}
		LocalizationLoader.LoadModTranslations(culture);
	}

	public void UseSources(List<IContentSource> sourcesFromLowestToHighest)
	{
		if (!_contentSources.SequenceEqual(sourcesFromLowestToHighest))
		{
			_contentSources = sourcesFromLowestToHighest.ToArray();
			ReloadLanguage();
		}
	}

	private void LoadActiveCultureTranslationsFromSources()
	{
		IContentSource[] contentSources = _contentSources;
		string assetNameStart = string.Concat(str2: ActiveCulture.Name, str0: "Localization", str1: Path.DirectorySeparatorChar.ToString()).ToLower();
		IContentSource[] array = contentSources;
		foreach (IContentSource item in array)
		{
			foreach (string item2 in item.GetAllAssetsStartingWith(assetNameStart))
			{
				string extension = item.GetExtension(item2);
				if (!(extension == ".json") && !(extension == ".csv"))
				{
					continue;
				}
				using Stream stream = item.OpenStream(item2);
				using StreamReader streamReader = new StreamReader(stream);
				string fileText = streamReader.ReadToEnd();
				if (extension == ".json")
				{
					LoadLanguageFromFileTextJson(fileText, canCreateCategories: false);
				}
				if (extension == ".csv")
				{
					LoadLanguageFromFileTextCsv(fileText);
				}
			}
		}
	}

	public void LoadLanguageFromFileTextCsv(string fileText)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		using TextReader reader = new StringReader(fileText);
		CsvReader csvReader = new CsvReader(reader);
		try
		{
			csvReader.Configuration.HasHeaderRecord = true;
			if (!csvReader.ReadHeader())
			{
				return;
			}
			string[] fieldHeaders = csvReader.FieldHeaders;
			int num = -1;
			int num2 = -1;
			for (int i = 0; i < fieldHeaders.Length; i++)
			{
				string text3 = fieldHeaders[i].ToLower();
				if (text3 == "translation")
				{
					num2 = i;
				}
				if (text3 == "key")
				{
					num = i;
				}
			}
			if (num == -1 || num2 == -1)
			{
				return;
			}
			int num3 = Math.Max(num, num2) + 1;
			while (csvReader.Read())
			{
				string[] currentRecord = csvReader.CurrentRecord;
				if (currentRecord.Length >= num3)
				{
					string text2 = currentRecord[num];
					string value = currentRecord[num2];
					if (!string.IsNullOrWhiteSpace(text2) && !string.IsNullOrWhiteSpace(value) && _localizedTexts.ContainsKey(text2))
					{
						_localizedTexts[text2].SetValue(value);
					}
				}
			}
		}
		finally
		{
			((IDisposable)csvReader)?.Dispose();
		}
	}

	public void LoadLanguageFromFileTextJson(string fileText, bool canCreateCategories)
	{
		foreach (KeyValuePair<string, Dictionary<string, string>> item in JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(fileText))
		{
			_ = item.Key;
			foreach (KeyValuePair<string, string> item2 in item.Value)
			{
				string key = item.Key + "." + item2.Key;
				if (_localizedTexts.ContainsKey(key))
				{
					_localizedTexts[key].SetValue(item2.Value);
				}
				else if (canCreateCategories)
				{
					_localizedTexts.Add(key, new LocalizedText(key, item2.Value));
					if (!_categoryGroupedKeys.ContainsKey(item.Key))
					{
						_categoryGroupedKeys.Add(item.Key, new List<string>());
					}
					_categoryGroupedKeys[item.Key].Add(item2.Key);
				}
			}
		}
	}

	[Conditional("DEBUG")]
	private void ValidateAllCharactersContainedInFont(DynamicSpriteFont font)
	{
		if (font == null)
		{
			return;
		}
		string text = "";
		foreach (LocalizedText value2 in _localizedTexts.Values)
		{
			string value = value2.Value;
			for (int i = 0; i < value.Length; i++)
			{
				char c = value[i];
				if (!font.IsCharacterSupported(c))
				{
					string[] obj = new string[7]
					{
						text,
						value2.Key,
						", ",
						c.ToString(),
						", ",
						null,
						null
					};
					int num = c;
					obj[5] = num.ToString();
					obj[6] = "\n";
					text = string.Concat(obj);
				}
			}
		}
	}

	public LocalizedText[] FindAll(Regex regex)
	{
		int num = 0;
		foreach (KeyValuePair<string, LocalizedText> localizedText3 in _localizedTexts)
		{
			if (regex.IsMatch(localizedText3.Key))
			{
				num++;
			}
		}
		LocalizedText[] array = new LocalizedText[num];
		int num2 = 0;
		foreach (KeyValuePair<string, LocalizedText> localizedText2 in _localizedTexts)
		{
			if (regex.IsMatch(localizedText2.Key))
			{
				array[num2] = localizedText2.Value;
				num2++;
			}
		}
		return array;
	}

	public LocalizedText[] FindAll(LanguageSearchFilter filter)
	{
		LinkedList<LocalizedText> linkedList = new LinkedList<LocalizedText>();
		foreach (KeyValuePair<string, LocalizedText> localizedText in _localizedTexts)
		{
			if (filter(localizedText.Key, localizedText.Value))
			{
				linkedList.AddLast(localizedText.Value);
			}
		}
		return linkedList.ToArray();
	}

	public LocalizedText SelectRandom(LanguageSearchFilter filter, UnifiedRandom random = null)
	{
		int num = 0;
		foreach (KeyValuePair<string, LocalizedText> localizedText in _localizedTexts)
		{
			if (filter(localizedText.Key, localizedText.Value))
			{
				num++;
			}
		}
		int num2 = (random ?? Main.rand).Next(num);
		foreach (KeyValuePair<string, LocalizedText> localizedText2 in _localizedTexts)
		{
			if (filter(localizedText2.Key, localizedText2.Value) && --num == num2)
			{
				return localizedText2.Value;
			}
		}
		return LocalizedText.Empty;
	}

	public LocalizedText RandomFromCategory(string categoryName, UnifiedRandom random = null)
	{
		if (!_categoryGroupedKeys.ContainsKey(categoryName))
		{
			return new LocalizedText(categoryName + ".RANDOM", categoryName + ".RANDOM");
		}
		List<string> list = GetKeysInCategory(categoryName);
		return GetText(categoryName + "." + list[(random ?? Main.rand).Next(list.Count)]);
	}

	public LocalizedText IndexedFromCategory(string categoryName, int index)
	{
		if (!_categoryGroupedKeys.ContainsKey(categoryName))
		{
			return new LocalizedText(categoryName + ".INDEXED", categoryName + ".INDEXED");
		}
		List<string> list = GetKeysInCategory(categoryName);
		int index2 = index % list.Count;
		return GetText(categoryName + "." + list[index2]);
	}

	public bool Exists(string key)
	{
		return _localizedTexts.ContainsKey(key);
	}

	public LocalizedText GetText(string key)
	{
		if (!_localizedTexts.TryGetValue(key, out var text))
		{
			return new LocalizedText(key, key);
		}
		return text;
	}

	public LocalizedText GetOrRegister(string key, Func<string> makeDefaultValue = null)
	{
		if (!_localizedTexts.TryGetValue(key, out var text))
		{
			_moddedKeys.Add(key);
			text = (_localizedTexts[key] = new LocalizedText(key, makeDefaultValue?.Invoke() ?? key));
		}
		return text;
	}

	public string GetTextValue(string key)
	{
		if (_localizedTexts.ContainsKey(key))
		{
			return _localizedTexts[key].Value;
		}
		return key;
	}

	public string GetTextValue(string key, object arg0)
	{
		if (_localizedTexts.ContainsKey(key))
		{
			return _localizedTexts[key].Format(arg0);
		}
		return key;
	}

	public string GetTextValue(string key, object arg0, object arg1)
	{
		if (_localizedTexts.ContainsKey(key))
		{
			return _localizedTexts[key].Format(arg0, arg1);
		}
		return key;
	}

	public string GetTextValue(string key, object arg0, object arg1, object arg2)
	{
		if (_localizedTexts.ContainsKey(key))
		{
			return _localizedTexts[key].Format(arg0, arg1, arg2);
		}
		return key;
	}

	public string GetTextValue(string key, params object[] args)
	{
		if (_localizedTexts.ContainsKey(key))
		{
			return _localizedTexts[key].Format(args);
		}
		return key;
	}

	public void SetFallbackCulture(GameCulture culture)
	{
		_fallbackCulture = culture;
	}

	public List<string> GetKeysInCategory(string categoryName)
	{
		return _categoryGroupedKeys[categoryName];
	}

	public List<string> GetLocalizedEntriesInCategory(string categoryName)
	{
		List<string> keysInCategory = GetKeysInCategory(categoryName);
		List<string> localizedList = new List<string>();
		foreach (string key in keysInCategory)
		{
			localizedList.Add(GetText(categoryName + "." + key).Value);
		}
		return localizedList;
	}

	internal void UnloadModdedEntries()
	{
		foreach (string key in _moddedKeys)
		{
			_localizedTexts.Remove(key);
		}
		_moddedKeys.Clear();
		ResetBoundTexts();
		ReloadLanguage();
	}

	private void ProcessCopyCommandsInTexts()
	{
		Regex referenceRegex = new Regex("{\\$([\\w\\.]+)(?:@(\\d+))?}", RegexOptions.Compiled);
		Regex argRemappingRegex = new Regex("(?<={\\^?)(\\d+)(?=(?::[^\\r\\n]+?)?})", RegexOptions.Compiled);
		HashSet<LocalizedText> processed = new HashSet<LocalizedText>();
		foreach (LocalizedText text2 in _localizedTexts.Values)
		{
			Process(text2);
		}
		string FindKeyInScope(string key, string scope)
		{
			if (Exists(key))
			{
				return key;
			}
			string[] splitKey = scope.Split(".");
			for (int i = splitKey.Length - 1; i >= 0; i--)
			{
				string combinedKey = string.Join(".", splitKey.Take(i + 1)) + "." + key;
				if (Exists(combinedKey))
				{
					return combinedKey;
				}
			}
			return key;
		}
		void Process(LocalizedText text)
		{
			if (processed.Add(text))
			{
				string newValue = referenceRegex.Replace(text.Value, delegate(Match match)
				{
					LocalizedText text3 = GetText(FindKeyInScope(match.Groups[1].Value, text.Key));
					Process(text3);
					string text4 = text3.Value;
					if (match.Groups[2].Success && int.TryParse(match.Groups[2].Value, out var offset))
					{
						text4 = argRemappingRegex.Replace(text4, (Match match) => (int.Parse(match.Groups[1].Value) + offset).ToString());
					}
					return text4;
				});
				text.SetValue(newValue);
			}
		}
	}

	internal void ResetBoundTexts()
	{
		boundTextCache.Clear();
		boundTexts.Clear();
	}

	internal LocalizedText BindFormatArgs(string key, object[] args)
	{
		TextBinding binding = new TextBinding(key, args);
		if (boundTextCache.TryGetValue(binding, out var text))
		{
			return text;
		}
		text = new LocalizedText(key, GetTextValue(key));
		text.BindArgs(args);
		boundTextCache[binding] = text;
		boundTexts.Add(text);
		return text;
	}

	internal void RecalculateBoundTextValues()
	{
		foreach (LocalizedText text in boundTexts)
		{
			object[] args = text.BoundArgs;
			text.SetValue(GetTextValue(text.Key));
			text.BindArgs(args);
		}
	}
}
