using Terraria.Localization;

namespace Terraria.ModLoader.UI;

public static class ModsMenuSortModesExtensions
{
	public static string ToFriendlyString(this ModsMenuSortMode sortmode)
	{
		return sortmode switch
		{
			ModsMenuSortMode.RecentlyUpdated => Language.GetTextValue("tModLoader.ModsSortRecently"), 
			ModsMenuSortMode.DisplayNameAtoZ => Language.GetTextValue("tModLoader.ModsSortNamesAlph"), 
			ModsMenuSortMode.DisplayNameZtoA => Language.GetTextValue("tModLoader.ModsSortNamesReverseAlph"), 
			_ => "Unknown Sort", 
		};
	}
}
