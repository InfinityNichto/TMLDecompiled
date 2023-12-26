using Terraria.Localization;

namespace Terraria.ModLoader.UI;

public static class EnabledFilterModesExtensions
{
	public static string ToFriendlyString(this EnabledFilter updateFilterMode)
	{
		return updateFilterMode switch
		{
			EnabledFilter.All => Language.GetTextValue("tModLoader.ModsShowAllMods"), 
			EnabledFilter.EnabledOnly => Language.GetTextValue("tModLoader.ModsShowEnabledMods"), 
			EnabledFilter.DisabledOnly => Language.GetTextValue("tModLoader.ModsShowDisabledMods"), 
			_ => "Unknown Sort", 
		};
	}
}
