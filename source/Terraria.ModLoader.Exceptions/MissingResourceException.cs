using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.Localization;

namespace Terraria.ModLoader.Exceptions;

public class MissingResourceException : Exception
{
	public override string HelpLink => "https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-FAQ#terrariamodloadermodgettexturestring-name-error";

	public MissingResourceException()
	{
	}

	public MissingResourceException(string message)
		: base(message)
	{
	}

	public MissingResourceException(string message, Exception inner)
		: base(message, inner)
	{
	}

	public MissingResourceException(List<string> reasons, string assetPath, ICollection<string> keys)
		: this(ProcessMessage(reasons, assetPath, keys))
	{
	}

	public static string ProcessMessage(List<string> reasons, string assetPath, ICollection<string> keys)
	{
		if (reasons.Count > 0)
		{
			reasons.Insert(0, "Failed to load asset: \"" + assetPath + "\"");
			return string.Join(Environment.NewLine, reasons);
		}
		string closestMatch = LevenshteinDistance.FolderAwareEditDistance(assetPath, keys.ToArray());
		if (closestMatch != null && closestMatch != "")
		{
			var (a, b) = LevenshteinDistance.ComputeColorTaggedString(assetPath, closestMatch);
			return Language.GetTextValue("tModLoader.LoadErrorResourceNotFoundPathHint", assetPath, closestMatch) + "\n" + a + "\n" + b + "\n";
		}
		return assetPath;
	}
}
