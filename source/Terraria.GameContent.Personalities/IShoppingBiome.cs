namespace Terraria.GameContent.Personalities;

public interface IShoppingBiome
{
	string NameKey { get; }

	bool IsInBiome(Player player);
}
