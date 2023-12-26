using Terraria.Localization;

namespace Terraria.ModLoader.UI.ModBrowser;

public static class ModBrowserSortModesExtensions
{
	public static string ToFriendlyString(this ModBrowserSortMode sortmode)
	{
		return sortmode switch
		{
			ModBrowserSortMode.DownloadsDescending => Language.GetTextValue("tModLoader.MBSortDownloadDesc"), 
			ModBrowserSortMode.RecentlyUpdated => Language.GetTextValue("tModLoader.MBSortByRecentlyUpdated"), 
			ModBrowserSortMode.Hot => Language.GetTextValue("tModLoader.MBSortByPopularity"), 
			_ => "Unknown Sort", 
		};
	}
}
