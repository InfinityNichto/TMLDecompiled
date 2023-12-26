using System;
using System.Text.RegularExpressions;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Terraria.Localization;

/// <summary>
/// Contains methods to access or retrieve localization values. The <see href="https://github.com/tModLoader/tModLoader/wiki/Localization">Localization Guide</see> teaches more about localization.
/// </summary>
public static class Language
{
	/// <summary>
	/// The language the game is currently using.
	/// </summary>
	public static GameCulture ActiveCulture => LanguageManager.Instance.ActiveCulture;

	/// <summary>
	/// Retrieves a LocalizedText object for a specified localization key. The actual text value can be retrieved from LocalizedText by accessing the <see cref="P:Terraria.Localization.LocalizedText.Value" /> property or by using the <see cref="M:Terraria.Localization.Language.GetTextValue(System.String)" /> method directly.<br /><br />
	/// Using LocalizedText instead of string is prefered when the value is stored. If the user switches languages or if resource packs or mods change text, the LocalizedText will automatically receive the new value. In turn, mods using those LocalizedText will also start displaying the updated values.<br /><br />
	///
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public static LocalizedText GetText(string key)
	{
		return LanguageManager.Instance.GetText(key);
	}

	/// <summary>
	/// Retrieves the text value for a specified localization key. The text returned will be for the currently selected language.
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public static string GetTextValue(string key)
	{
		return LanguageManager.Instance.GetTextValue(key);
	}

	/// <inheritdoc cref="M:Terraria.Localization.LocalizedText.Format(System.Object[])" />
	public static string GetTextValue(string key, object arg0)
	{
		return LanguageManager.Instance.GetTextValue(key, arg0);
	}

	/// <inheritdoc cref="M:Terraria.Localization.LocalizedText.Format(System.Object[])" />
	public static string GetTextValue(string key, object arg0, object arg1)
	{
		return LanguageManager.Instance.GetTextValue(key, arg0, arg1);
	}

	/// <inheritdoc cref="M:Terraria.Localization.LocalizedText.Format(System.Object[])" />
	public static string GetTextValue(string key, object arg0, object arg1, object arg2)
	{
		return LanguageManager.Instance.GetTextValue(key, arg0, arg1, arg2);
	}

	/// <inheritdoc cref="M:Terraria.Localization.LocalizedText.Format(System.Object[])" />
	public static string GetTextValue(string key, params object[] args)
	{
		return LanguageManager.Instance.GetTextValue(key, args);
	}

	/// <inheritdoc cref="M:Terraria.Localization.LocalizedText.FormatWith(System.Object)" />
	public static string GetTextValueWith(string key, object obj)
	{
		return LanguageManager.Instance.GetText(key).FormatWith(obj);
	}

	/// <summary>
	/// Checks if a LocalizedText with the provided key has been registered or not. This can be used to avoid retrieving dummy values from <see cref="M:Terraria.Localization.Language.GetText(System.String)" /> and <see cref="M:Terraria.Localization.Language.GetTextValue(System.String)" /> and instead load a fallback value or do other logic. If the key should be created if it doesn't exist, <see cref="M:Terraria.Localization.Language.GetOrRegister(System.String,System.Func{System.String})" /> can be used instead.
	/// <br /><br /> Note that modded keys will be registered during mod loading and <see cref="M:Terraria.ModLoader.Mod.SetupContent" /> is the earliest point where all keys should be registered with values loaded for the current language.
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public static bool Exists(string key)
	{
		return LanguageManager.Instance.Exists(key);
	}

	public static int GetCategorySize(string key)
	{
		return LanguageManager.Instance.GetCategorySize(key);
	}

	public static LocalizedText[] FindAll(Regex regex)
	{
		return LanguageManager.Instance.FindAll(regex);
	}

	/// <summary>
	/// Finds all LocalizedText that satisfy the <paramref name="filter" /> parameter. Typically used with <see cref="M:Terraria.Lang.CreateDialogFilter(System.String)" /> or <see cref="M:Terraria.Lang.CreateDialogFilter(System.String,System.Object)" /> as the <paramref name="filter" /> argument to find all LocalizedText that have a specific key prefix and satisfy provided conditions.
	/// </summary>
	/// <param name="filter"></param>
	/// <returns></returns>
	public static LocalizedText[] FindAll(LanguageSearchFilter filter)
	{
		return LanguageManager.Instance.FindAll(filter);
	}

	/// <summary>
	/// Selects a single random LocalizedText that satisfies the <paramref name="filter" /> parameter. Typically used with <see cref="M:Terraria.Lang.CreateDialogFilter(System.String)" /> or <see cref="M:Terraria.Lang.CreateDialogFilter(System.String,System.Object)" /> as the <paramref name="filter" /> argument to find a random LocalizedText that has a specific key prefix and satisfies the provided conditions.
	/// </summary>
	/// <param name="filter"></param>
	/// <param name="random"></param>
	/// <returns></returns>
	public static LocalizedText SelectRandom(LanguageSearchFilter filter, UnifiedRandom random = null)
	{
		return LanguageManager.Instance.SelectRandom(filter, random);
	}

	public static LocalizedText RandomFromCategory(string categoryName, UnifiedRandom random = null)
	{
		return LanguageManager.Instance.RandomFromCategory(categoryName, random);
	}

	/// <summary>
	/// Returns a <see cref="T:Terraria.Localization.LocalizedText" /> for a given key.
	/// <br />If no existing localization exists for the key, it will be defined so it can be exported to a matching mod localization file.
	/// </summary>
	/// <param name="key">The localization key</param>
	/// <param name="makeDefaultValue">A factory method for creating the default value, used to update localization files with missing entries</param>
	/// <returns></returns>
	public static LocalizedText GetOrRegister(string key, Func<string> makeDefaultValue = null)
	{
		return LanguageManager.Instance.GetOrRegister(key, makeDefaultValue);
	}

	[Obsolete("Pass mod.GetLocalizationKey(key) directly")]
	public static LocalizedText GetOrRegister(Mod mod, string key, Func<string> makeDefaultValue = null)
	{
		return GetOrRegister(mod.GetLocalizationKey(key), makeDefaultValue);
	}
}
