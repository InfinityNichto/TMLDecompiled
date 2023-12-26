using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Enums;
using Terraria.GameContent;

namespace Terraria.ModLoader;

/// <summary>
/// This class represents a type of modded palm tree.
/// The palm tree will share a tile ID with the vanilla palm trees (323), so that the trees can freely convert between each other if the sand below is converted.
/// This class encapsulates several functions that distinguish each type of palm tree from each other.
/// </summary>
public abstract class ModPalmTree : ITree, IPlant, ILoadable
{
	public const int VanillaStyleCount = 8;

	/// <summary>
	/// The tree will share a tile ID with the vanilla palm trees (323), so that the trees can freely convert between each other if the sand below is converted.
	/// </summary>
	public int PlantTileId => 323;

	public int VanillaCount => 8;

	public abstract TreePaintingSettings TreeShaderSettings { get; }

	public int[] GrowsOnTileId { get; set; }

	/// <summary>
	/// Used mostly for vanilla tree shake loot tables
	/// </summary>
	public virtual TreeTypes CountsAsTreeType => TreeTypes.Palm;

	public abstract void SetStaticDefaults();

	public abstract Asset<Texture2D> GetTexture();

	/// <summary>
	/// Return the type of dust created when this palm tree is destroyed. Returns 215 by default.
	/// </summary>
	/// <returns></returns>
	public virtual int CreateDust()
	{
		return 215;
	}

	/// <summary>
	/// Return the type of gore created when the tree grow, being shook and falling leaves on windy days, returns -1 by default
	/// </summary>
	/// <returns></returns>
	public virtual int TreeLeaf()
	{
		return -1;
	}

	/// <summary>
	/// Executed on tree shake, return false to skip vanilla tree shake drops
	/// </summary>
	/// <returns></returns>
	public virtual bool Shake(int x, int y, ref bool createLeaves)
	{
		return true;
	}

	/// <summary>
	/// Defines the sapling that can eventually grow into a tree. The type of the sapling should be returned here. Returns 20 and style 0 by default.
	/// The style parameter will determine which sapling is chosen if multiple sapling types share the same ID;
	/// even if you only have a single sapling in an ID, you must still set this to 0.
	/// </summary>
	/// <param name="style"></param>
	/// <returns></returns>
	public virtual int SaplingGrowthType(ref int style)
	{
		style = 1;
		return 20;
	}

	/// <summary>
	/// The ID of the item that is dropped in bulk when this palm tree is destroyed.
	/// </summary>
	/// <returns></returns>
	public abstract int DropWood();

	/// <summary>
	/// Return the texture containing the possible tree tops that can be drawn above this palm tree.
	/// </summary>
	/// <returns></returns>
	public abstract Asset<Texture2D> GetTopTextures();

	/// <summary>
	/// Return the texture containing the possible tree tops that can be drawn above this palm tree.
	/// </summary>
	/// <returns></returns>
	public abstract Asset<Texture2D> GetOasisTopTextures();
}
