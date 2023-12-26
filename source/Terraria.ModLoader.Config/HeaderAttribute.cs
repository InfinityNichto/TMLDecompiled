using System;
using Terraria.Localization;

namespace Terraria.ModLoader.Config;

/// <summary>
/// This attribute adds a label above this property or field in the ModConfig UI that acts as a header. Use this to delineate sections within your config. <br />
/// Note that fields will be in order, and properties will be in order, but fields and properties will not be interleaved together in the source code order. <br />
/// <br />
/// Header accept either a translation key or an identifier. <br />
/// To use a translation key, the value passed in must start with "$". <br />
/// A value passed in that does not start with "$" is interpreted as an identifier. The identifier is used to construct the localization key "Mods.{ModName}.Configs.{ConfigName}.Headers.{Identifier}" <br />
/// No spaces are allowed in translation keys, so avoid spaces <br />
/// Annotations on members of non-ModConfig classes need to supply a localization key using this attribute to be localized, no localization key can be correctly assumed using just an identifier. <br />
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class HeaderAttribute : Attribute
{
	internal string key;

	internal string identifier;

	internal readonly bool malformed;

	public bool IsIdentifier => identifier != null;

	public string Header => Language.GetTextValue(key);

	public HeaderAttribute(string identifierOrKey)
	{
		if (string.IsNullOrWhiteSpace(identifierOrKey) || identifierOrKey.Contains(' '))
		{
			malformed = true;
		}
		else if (!identifierOrKey.StartsWith("$"))
		{
			identifier = identifierOrKey;
		}
		else
		{
			key = identifierOrKey.Substring(1);
		}
	}
}
