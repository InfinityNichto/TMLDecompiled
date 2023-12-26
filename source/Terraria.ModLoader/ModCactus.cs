using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.ModLoader;

/// <summary>
/// This class represents a type of modded cactus.
/// This class encapsulates a function for retrieving the cactus's texture and an array for type of soil it grows on.
/// </summary>
public abstract class ModCactus : IPlant, ILoadable
{
	/// <summary>
	/// The cactus will share a tile ID with the vanilla cacti (80), so that the cacti can freely convert between each other if the sand below is converted.
	/// </summary>
	public int PlantTileId => 80;

	public int VanillaCount => 1;

	public int[] GrowsOnTileId { get; set; }

	public abstract void SetStaticDefaults();

	public abstract Asset<Texture2D> GetTexture();

	public abstract Asset<Texture2D> GetFruitTexture();
}
