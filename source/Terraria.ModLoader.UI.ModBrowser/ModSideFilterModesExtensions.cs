using Terraria.Localization;

namespace Terraria.ModLoader.UI.ModBrowser;

public static class ModSideFilterModesExtensions
{
	public static string ToFriendlyString(this ModSideFilter modSideFilterMode)
	{
		return modSideFilterMode switch
		{
			ModSideFilter.All => Language.GetTextValue("tModLoader.MBShowMSAll"), 
			ModSideFilter.Both => Language.GetTextValue("tModLoader.MBShowMSBoth"), 
			ModSideFilter.Client => Language.GetTextValue("tModLoader.MBShowMSClient"), 
			ModSideFilter.Server => Language.GetTextValue("tModLoader.MBShowMSServer"), 
			ModSideFilter.NoSync => Language.GetTextValue("tModLoader.MBShowMSNoSync"), 
			_ => "Unknown Sort", 
		};
	}
}
