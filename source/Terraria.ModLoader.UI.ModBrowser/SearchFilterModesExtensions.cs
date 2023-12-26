using Terraria.Localization;

namespace Terraria.ModLoader.UI.ModBrowser;

public static class SearchFilterModesExtensions
{
	public static string ToFriendlyString(this SearchFilter searchFilterMode)
	{
		return searchFilterMode switch
		{
			SearchFilter.Name => Language.GetTextValue("tModLoader.ModsSearchByModName"), 
			SearchFilter.Author => Language.GetTextValue("tModLoader.ModsSearchByAuthor"), 
			_ => "Unknown Sort", 
		};
	}
}
