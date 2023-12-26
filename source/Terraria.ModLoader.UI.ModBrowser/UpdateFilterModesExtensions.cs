using Terraria.Localization;

namespace Terraria.ModLoader.UI.ModBrowser;

public static class UpdateFilterModesExtensions
{
	public static string ToFriendlyString(this UpdateFilter updateFilterMode)
	{
		return updateFilterMode switch
		{
			UpdateFilter.All => Language.GetTextValue("tModLoader.MBShowAllMods"), 
			UpdateFilter.Available => Language.GetTextValue("tModLoader.MBShowNotInstalledUpdates"), 
			UpdateFilter.UpdateOnly => Language.GetTextValue("tModLoader.MBShowUpdates"), 
			UpdateFilter.InstalledOnly => Language.GetTextValue("tModLoader.MBShowInstalled"), 
			_ => "Unknown Sort", 
		};
	}
}
