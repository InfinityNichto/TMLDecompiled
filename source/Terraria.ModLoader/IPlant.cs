using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.ModLoader;

public interface IPlant : ILoadable
{
	int PlantTileId { get; }

	int VanillaCount { get; }

	int[] GrowsOnTileId { get; set; }

	Asset<Texture2D> GetTexture();

	void SetStaticDefaults();

	void ILoadable.Load(Mod mod)
	{
		PlantLoader.plantList.Add(this);
	}

	void ILoadable.Unload()
	{
	}
}
