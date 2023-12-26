using Terraria.ModLoader;

namespace Terraria.GameContent.Personalities;

public abstract class AShoppingBiome : IShoppingBiome, ILoadable
{
	public string NameKey { get; protected set; }

	internal AShoppingBiome()
	{
	}

	public abstract bool IsInBiome(Player player);

	void ILoadable.Load(Mod mod)
	{
	}

	void ILoadable.Unload()
	{
	}
}
