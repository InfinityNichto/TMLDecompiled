using Terraria.DataStructures;

namespace Terraria.GameContent.UI.ResourceSets;

public interface IPlayerResourcesDisplaySet : IConfigKeyHolder
{
	string DisplayedName { get; }

	void Draw();

	void TryToHover();
}
