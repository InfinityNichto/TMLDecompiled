using System;
using Terraria.Localization;

namespace Terraria.ModLoader.Config;

[Obsolete("Tooltips are now automatically localized in localization files. Use TooltipKeyAttribute to customize the key or use the autogenerated translation key instead.")]
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class TooltipAttribute : Attribute
{
	private readonly string tooltip;

	public string Tooltip
	{
		get
		{
			if (!tooltip.StartsWith("$"))
			{
				return tooltip;
			}
			return Language.GetTextValue(tooltip.Substring(1));
		}
	}

	public string LocalizationEntry
	{
		get
		{
			if (!tooltip.StartsWith("$"))
			{
				return tooltip;
			}
			return "{" + tooltip + "}";
		}
	}

	public TooltipAttribute(string tooltip)
	{
		this.tooltip = tooltip;
	}
}
