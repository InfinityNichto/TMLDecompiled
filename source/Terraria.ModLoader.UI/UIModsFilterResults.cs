namespace Terraria.ModLoader.UI;

public class UIModsFilterResults
{
	public int filteredBySearch;

	public int filteredByModSide;

	public int filteredByEnabled;

	public bool AnyFiltered
	{
		get
		{
			if (filteredBySearch <= 0 && filteredByModSide <= 0)
			{
				return filteredByEnabled > 0;
			}
			return true;
		}
	}
}
