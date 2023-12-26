using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Enums;
using Terraria.GameContent;

namespace Terraria.ModLoader;

/// <summary>
/// This class represents a type of modded tree.
/// The tree will share a tile ID with the vanilla trees (5), so that the trees can freely convert between each other if the soil below is converted.
/// This class encapsulates several functions that distinguish each type of tree from each other.
/// </summary>
public abstract class ModTree : ITree, IPlant, ILoadable
{
	public const int VanillaStyleCount = 7;

	public const int VanillaTopTextureCount = 100;

	/// <summary>
	/// The tree will share a tile ID with the vanilla trees (5), so that the trees can freely convert between each other if the soil below is converted.
	/// </summary>
	public int PlantTileId => 5;

	public int VanillaCount => 7;

	public abstract TreePaintingSettings TreeShaderSettings { get; }

	public int[] GrowsOnTileId { get; set; }

	/// <summary>
	/// Used mostly for vanilla tree shake loot tables
	/// </summary>
	public virtual TreeTypes CountsAsTreeType => TreeTypes.Forest;

	public abstract void SetStaticDefaults();

	public abstract Asset<Texture2D> GetTexture();

	/// <summary>
	/// Return the type of dust created when this tree is destroyed. Returns 7 by default.
	/// </summary>
	/// <returns></returns>
	public virtual int CreateDust()
	{
		return 7;
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
	/// Executed on tree shake, return false to skip vanilla tree shake drops.<br />
	/// The x and y coordinates correspond to the top of the tree, where items usually spawn.
	/// </summary>
	/// <returns></returns>
	public virtual bool Shake(int x, int y, ref bool createLeaves)
	{
		return true;
	}

	/// <summary>
	/// Whether or not this tree can drop acorns. Returns true by default.
	/// </summary>
	/// <returns></returns>
	public virtual bool CanDropAcorn()
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
		style = 0;
		return 20;
	}

	/// <summary>
	/// The ID of the item that is dropped in bulk when this tree is destroyed.
	/// </summary>
	/// <returns></returns>
	public abstract int DropWood();

	public abstract void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight);

	/// <summary>
	/// Return the texture containing the possible tree tops that can be drawn above this tree.
	/// The framing was determined under <cref>SetTreeFoliageSettings</cref>
	/// </summary>
	public abstract Asset<Texture2D> GetTopTextures();

	/// <summary>
	/// Return the texture containing the possible tree branches that can be drawn next to this tree.
	/// The framing was determined under <cref>SetTreeFoliageSettings</cref>
	/// </summary>
	public abstract Asset<Texture2D> GetBranchTextures();
}
