using Terraria.ModLoader.UI.ModBrowser;

namespace Terraria.Social.Base;

public struct QueryParameters
{
	public string[] searchTags;

	public ModPubId_t[] searchModIds;

	public string[] searchModSlugs;

	public string searchGeneric;

	public string searchAuthor;

	public ModBrowserSortMode sortingParamater;

	public UpdateFilter updateStatusFilter;

	public ModSideFilter modSideFilter;

	public QueryType queryType;
}
