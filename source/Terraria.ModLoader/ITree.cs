using Terraria.Enums;

namespace Terraria.ModLoader;

public interface ITree : IPlant, ILoadable
{
	TreeTypes CountsAsTreeType { get; }

	int TreeLeaf();

	bool Shake(int x, int y, ref bool createLeaves);
}
