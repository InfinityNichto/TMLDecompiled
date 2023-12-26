using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Terraria.Localization;

/// <summary>
/// Contains the localization value corresponding to a key for the current game language. Automatically updates as language, mods, and resource packs change. The <see href="https://github.com/tModLoader/tModLoader/wiki/Localization">Localization Guide</see> teaches more about localization.
/// </summary>
public class LocalizedText
{
	public static readonly LocalizedText Empty = new LocalizedText("", "");

	private static Regex _substitutionRegex = new Regex("{(\\?(?:!)?)?([a-zA-Z][\\w\\.]*)}", RegexOptions.Compiled);

	public readonly string Key;

	private string _value;

	private bool? _hasPlurals;

	public static readonly Regex PluralizationPatternRegex = new Regex("{\\^(\\d+):([^\\r\\n]+?)}", RegexOptions.Compiled);

	/// <summary>
	/// Retrieves the text value. This is the actual text the user should see.
	/// </summary>
	public string Value
	{
		get
		{
			return _value;
		}
		private set
		{
			_value = value;
			_hasPlurals = null;
			BoundArgs = null;
		}
	}

	/// <summary>
	/// Returns the args used with <see cref="M:Terraria.Localization.LocalizedText.WithFormatArgs(System.Object[])" /> to create this text, if any.
	/// </summary>
	public object[] BoundArgs { get; private set; }

	internal LocalizedText(string key, string text)
	{
		Key = key;
		Value = text;
	}

	internal void SetValue(string text)
	{
		Value = text;
	}

	/// <summary>
	/// Creates a string from this LocalizedText populated with data from the provided <paramref name="obj" /> parameter. The properties of the provided object are substituted by name into the placeholders of the original text. For example, when used with <see cref="M:Terraria.Lang.CreateDialogSubstitutionObject(Terraria.NPC)" />, the text "{Nurse}" will be replaced with the first name of the Nurse in the world. Modded substitutions are not currently supported. <br /><br />
	/// When used in conjunction with <see cref="M:Terraria.Localization.Language.SelectRandom(Terraria.Localization.LanguageSearchFilter,Terraria.Utilities.UnifiedRandom)" /> and <see cref="M:Terraria.Lang.CreateDialogFilter(System.String,System.Object)" />, simple boolean conditions expressed in each LocalizedText can be used to filter a collection of LocalizedText.  <br /><br />
	/// <see cref="M:Terraria.Localization.LocalizedText.Format(System.Object[])" /> is more commonly used to format LocalizedText placeholders. That method replaces placeholders such as "{0}", "{1}", etc with the string representation of the corresponding objects provided.
	/// </summary>
	/// <param name="obj"></param>
	/// <returns></returns>
	public string FormatWith(object obj)
	{
		string value = Value;
		PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
		return _substitutionRegex.Replace(value, delegate(Match match)
		{
			if (match.Groups[1].Length != 0)
			{
				return "";
			}
			string name = match.Groups[2].ToString();
			PropertyDescriptor propertyDescriptor = properties.Find(name, ignoreCase: false);
			return (propertyDescriptor != null) ? (propertyDescriptor.GetValue(obj) ?? "").ToString() : "";
		});
	}

	/// <summary>
	/// Checks if the conditions embedded in this LocalizedText are satisfied by the <paramref name="obj" /> argument.
	/// For example when used with <see cref="M:Terraria.Lang.CreateDialogSubstitutionObject(Terraria.NPC)" /> as the <paramref name="obj" /> argument, "{?Rain}" at the start of a LocalizedText value will cause false to be returned if it is not raining. "{?!Rain}" would do the opposite. If all conditions are satisfied, true is returned.<br />
	/// The method is typically used indirectly by using <see cref="M:Terraria.Lang.CreateDialogFilter(System.String,System.Object)" />.
	/// </summary>
	/// <param name="obj"></param>
	/// <returns></returns>
	public bool CanFormatWith(object obj)
	{
		PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
		foreach (Match item in _substitutionRegex.Matches(Value))
		{
			string name = item.Groups[2].ToString();
			PropertyDescriptor propertyDescriptor = properties.Find(name, ignoreCase: false);
			if (propertyDescriptor == null)
			{
				return false;
			}
			object value = propertyDescriptor.GetValue(obj);
			if (value == null)
			{
				return false;
			}
			if (item.Groups[1].Length != 0 && ((value as bool?).GetValueOrDefault() ^ (item.Groups[1].Length == 1)))
			{
				return false;
			}
		}
		return true;
	}

	/// <summary> Convert this <see cref="T:Terraria.Localization.LocalizedText" /> to a <see cref="T:Terraria.Localization.NetworkText" /> for use in various network code applications. Non-chat messages sent to other players should be sent as <see cref="T:Terraria.Localization.NetworkText" /> to facilitate localization. </summary>
	public NetworkText ToNetworkText()
	{
		return NetworkText.FromKey(Key, BoundArgs ?? Array.Empty<object>());
	}

	/// <inheritdoc cref="M:Terraria.Localization.LocalizedText.ToNetworkText" />
	public NetworkText ToNetworkText(params object[] substitutions)
	{
		return NetworkText.FromKey(Key, substitutions);
	}

	public static explicit operator string(LocalizedText text)
	{
		return text.Value;
	}

	public override string ToString()
	{
		return Value;
	}

	public static int CardinalPluralRule(GameCulture culture, int count)
	{
		int mod_i10 = count % 10;
		int mod_i11 = count % 100;
		switch (culture.LegacyId)
		{
		case 6:
			if (mod_i10 == 1 && mod_i11 != 11)
			{
				return 0;
			}
			if (contains(mod_i10, 2, 4) && !contains(mod_i11, 12, 14))
			{
				return 1;
			}
			return 2;
		case 1:
		case 2:
		case 3:
		case 5:
		case 8:
			if (count != 1)
			{
				return 1;
			}
			return 0;
		case 4:
			if (count != 0 && count != 1)
			{
				return 1;
			}
			return 0;
		case 9:
			if (count == 1)
			{
				return 0;
			}
			if (contains(mod_i10, 2, 4) && !contains(mod_i11, 12, 14))
			{
				return 1;
			}
			return 2;
		default:
			return 0;
		}
		static bool contains(int i, int a, int b)
		{
			if (i >= a)
			{
				return i <= b;
			}
			return false;
		}
	}

	public static string ApplyPluralization(string value, params object[] args)
	{
		return PluralizationPatternRegex.Replace(value, delegate(Match match)
		{
			int num = Convert.ToInt32(match.Groups[1].Value);
			string[] array = match.Groups[2].Value.Split(';');
			int count = Convert.ToInt32(args[num]);
			int val = CardinalPluralRule(Language.ActiveCulture, count);
			return array[Math.Min(val, array.Length - 1)];
		});
	}

	/// <summary>
	/// Creates a string from this LocalizedText populated with data from the provided <paramref name="args" /> arguments. Formats the string in the same manner as <see href="https://learn.microsoft.com/en-us/dotnet/api/system.string.format?view=net-6.0">string.Format</see>. Placeholders such as "{0}", "{1}", etc will be replaced with the string representation of the corresponding objects provided.<br />
	/// Additionally, pluralization is supported as well. The <see href="https://github.com/tModLoader/tModLoader/wiki/Contributing-Localization#placeholders">Contributing Localization Guide</see> teaches more about placeholders and plural support.
	///
	/// </summary>
	/// <param name="args"></param>
	/// <returns></returns>
	public string Format(params object[] args)
	{
		string value = Value;
		bool valueOrDefault = _hasPlurals.GetValueOrDefault();
		bool num;
		if (!_hasPlurals.HasValue)
		{
			valueOrDefault = PluralizationPatternRegex.IsMatch(value);
			_hasPlurals = valueOrDefault;
			num = valueOrDefault;
		}
		else
		{
			num = valueOrDefault;
		}
		if (num)
		{
			value = ApplyPluralization(value, args);
		}
		try
		{
			return string.Format(value, args);
		}
		catch (FormatException e)
		{
			throw new Exception($"The localization key:\n  \"{Key}\"\nwith a value of:\n  \"{value}\"\nfailed to be formatted with the inputs:\n  [{string.Join(", ", args)}]", e);
		}
	}

	/// <summary>
	/// Creates a new LocalizedText with the supplied arguments formatted into the value (via <see cref="M:System.String.Format(System.String,System.Object[])" />)<br />
	/// Will automatically update to re-format the string with cached args when language changes. <br />
	///             <br />
	/// The resulting LocalizedText should be stored statically. Should not be used to create 'throwaway' LocalizedText instances. <br />
	/// Use <see cref="M:Terraria.Localization.LocalizedText.Format(System.Object[])" /> instead for repeated on-demand formatting with different args.
	/// <br /> The <see href="https://github.com/tModLoader/tModLoader/wiki/Localization#string-formatting">Localization Guide</see> teaches more about using placeholders in localization.
	/// </summary>
	/// <param name="args">The substitution args</param>
	/// <returns></returns>
	public LocalizedText WithFormatArgs(params object[] args)
	{
		return LanguageManager.Instance.BindFormatArgs(Key, args);
	}

	internal void BindArgs(object[] args)
	{
		SetValue(Format(args));
		BoundArgs = args;
	}
}
