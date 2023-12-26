using System;

namespace Terraria.ModLoader.Config;

/// <summary>
/// By default, string fields will provide the user with a text input field. Use this attribute to restrict strings to a selection of options.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class OptionStringsAttribute : Attribute
{
	public string[] OptionLabels { get; set; }

	public OptionStringsAttribute(string[] optionLabels)
	{
		OptionLabels = optionLabels;
	}
}
