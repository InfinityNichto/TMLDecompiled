using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria;

/// <summary>
/// A data structure used for accessing information about tiles, walls, wires, and liquids at a single position in the world.<para />
/// Vanilla tile code and a mods tile code will be quite different, since tModLoader reworked how tiles function to improve performance. This means that copying vanilla code will leave you with many errors. Running the code through tModPorter will fix most of the issues, however.<para />
/// For your sanity, all of the changes are well documented to make it easier to port vanilla code.
/// </summary>
public readonly struct Tile
{
	internal readonly uint TileId;

	/// <summary>
	/// The <see cref="T:Terraria.ID.TileID" /> of the tile at this position.<br />
	/// This value is only valid if <see cref="P:Terraria.Tile.HasTile" /> is true.<br />
	/// Legacy/vanilla equivalent is <see cref="P:Terraria.Tile.type" />.
	/// </summary>
	public ref ushort TileType => ref Get<TileTypeData>().Type;

	/// <summary>
	/// The <see cref="T:Terraria.ID.WallID" /> of the wall at this position.<br />
	/// A value of 0 indicates no wall.<br />
	/// Legacy/vanilla equivalent is <see cref="P:Terraria.Tile.wall" />.
	/// </summary>
	public ref ushort WallType => ref Get<WallTypeData>().Type;

	/// <summary>
	/// Whether there is a tile at this position. Check this whenever you are accessing data from a tile to avoid getting data from an empty tile.<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.active" /> or <see cref="M:Terraria.Tile.active(System.Boolean)" />.
	/// </summary>
	/// <remarks>
	/// Actuated tiles are not solid, so use <see cref="P:Terraria.Tile.HasUnactuatedTile" /> instead of <see cref="P:Terraria.Tile.HasTile" /> for collision checks.<br />
	/// This only corresponds to whether a tile exists, however, a wall can exist without a tile. To check if a wall exists, use <c>tile.WallType != WallID.None</c>.
	/// </remarks>
	public bool HasTile
	{
		get
		{
			return Get<TileWallWireStateData>().HasTile;
		}
		set
		{
			Get<TileWallWireStateData>().HasTile = value;
		}
	}

	/// <summary>
	/// Whether the tile at this position is actuated by an actuator.<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.inActive" /> or <see cref="M:Terraria.Tile.inActive(System.Boolean)" />.
	/// </summary>
	/// <remarks>
	/// Actuated tiles are <strong>not</strong> solid.
	/// </remarks>
	public bool IsActuated
	{
		get
		{
			return Get<TileWallWireStateData>().IsActuated;
		}
		set
		{
			Get<TileWallWireStateData>().IsActuated = value;
		}
	}

	/// <summary>
	/// Whether there is an actuator at this position.<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.actuator" /> or <see cref="M:Terraria.Tile.actuator(System.Boolean)" />.
	/// </summary>
	public bool HasActuator
	{
		get
		{
			return Get<TileWallWireStateData>().HasActuator;
		}
		set
		{
			Get<TileWallWireStateData>().HasActuator = value;
		}
	}

	/// <summary>
	/// Whether there is a tile at this position that isn't actuated.<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.nactive" />.
	/// </summary>
	/// <remarks>
	/// Actuated tiles are not solid, so use <see cref="P:Terraria.Tile.HasUnactuatedTile" /> instead of <see cref="P:Terraria.Tile.HasTile" /> for collision checks.<br />
	/// When checking if a tile exists, use <see cref="P:Terraria.Tile.HasTile" /> instead of <see cref="P:Terraria.Tile.HasUnactuatedTile" />.
	/// </remarks>
	public bool HasUnactuatedTile
	{
		get
		{
			if (HasTile)
			{
				return !IsActuated;
			}
			return false;
		}
	}

	/// <summary>
	/// The slope shape of the tile, which can be changed by hammering.<br />
	/// Used by <see cref="M:Terraria.WorldGen.SlopeTile(System.Int32,System.Int32,System.Int32,System.Boolean)" /> and <see cref="P:Terraria.Tile.BlockType" />.<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.slope" /> or <see cref="M:Terraria.Tile.slope(System.Byte)" />.
	/// </summary>
	public SlopeType Slope
	{
		get
		{
			return Get<TileWallWireStateData>().Slope;
		}
		set
		{
			Get<TileWallWireStateData>().Slope = value;
		}
	}

	/// <summary>
	/// The <see cref="P:Terraria.Tile.Slope" /> and <see cref="P:Terraria.Tile.IsHalfBlock" /> of this tile combined, which can be changed by hammering.<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.blockType" />.
	/// </summary>
	public BlockType BlockType
	{
		get
		{
			return Get<TileWallWireStateData>().BlockType;
		}
		set
		{
			Get<TileWallWireStateData>().BlockType = value;
		}
	}

	/// <summary>
	/// Whether a tile is a half block shape, which can be changed by hammering.<br />
	/// Used by <see cref="M:Terraria.WorldGen.PoundTile(System.Int32,System.Int32)" /> and <see cref="P:Terraria.Tile.BlockType" />.<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.halfBrick" /> or <see cref="M:Terraria.Tile.halfBrick(System.Boolean)" />.
	/// </summary>
	public bool IsHalfBlock
	{
		get
		{
			return Get<TileWallWireStateData>().IsHalfBlock;
		}
		set
		{
			Get<TileWallWireStateData>().IsHalfBlock = value;
		}
	}

	/// <summary>
	/// Whether a tile's <see cref="P:Terraria.Tile.Slope" /> has a solid top side (<see cref="F:Terraria.ID.SlopeType.SlopeDownLeft" /> or <see cref="F:Terraria.ID.SlopeType.SlopeDownRight" />).<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.topSlope" />.
	/// </summary>
	public bool TopSlope
	{
		get
		{
			if (Slope != SlopeType.SlopeDownLeft)
			{
				return Slope == SlopeType.SlopeDownRight;
			}
			return true;
		}
	}

	/// <summary>
	/// Whether a tile's <see cref="P:Terraria.Tile.Slope" /> has a solid bottom side (<see cref="F:Terraria.ID.SlopeType.SlopeUpLeft" /> or <see cref="F:Terraria.ID.SlopeType.SlopeUpRight" />).<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.bottomSlope" />.
	/// </summary>
	public bool BottomSlope
	{
		get
		{
			if (Slope != SlopeType.SlopeUpLeft)
			{
				return Slope == SlopeType.SlopeUpRight;
			}
			return true;
		}
	}

	/// <summary>
	/// Whether a tile's <see cref="P:Terraria.Tile.Slope" /> has a solid left side (<see cref="F:Terraria.ID.SlopeType.SlopeDownRight" /> or <see cref="F:Terraria.ID.SlopeType.SlopeUpRight" />).<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.leftSlope" />.
	/// </summary>
	public bool LeftSlope
	{
		get
		{
			if (Slope != SlopeType.SlopeDownRight)
			{
				return Slope == SlopeType.SlopeUpRight;
			}
			return true;
		}
	}

	/// <summary>
	/// Whether a tile's <see cref="P:Terraria.Tile.Slope" /> has a solid right side (<see cref="F:Terraria.ID.SlopeType.SlopeDownLeft" /> or <see cref="F:Terraria.ID.SlopeType.SlopeUpLeft" />).<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.rightSlope" />.
	/// </summary>
	public bool RightSlope
	{
		get
		{
			if (Slope != SlopeType.SlopeDownLeft)
			{
				return Slope == SlopeType.SlopeUpLeft;
			}
			return true;
		}
	}

	/// <summary>
	/// The X coordinate of the top left corner of the area in the spritesheet for the <see cref="P:Terraria.Tile.TileType" /> to be used to draw the tile at this position.<para />
	/// For a Framed tile, this value is set automatically according to the framing logic as the world loads or other tiles are placed or mined nearby. See <see href="https://github.com/tModLoader/tModLoader/wiki/Basic-Tile#framed-vs-frameimportant-tiles">Framed vs FrameImportant</see> for more info. For <see cref="F:Terraria.Main.tileFrameImportant" /> tiles, this value will not change due to tile framing and will be saved and synced in Multiplayer. In either case, <see cref="P:Terraria.Tile.TileFrameX" /> and <see cref="P:Terraria.Tile.TileFrameY" /> correspond to the coordinates of the top left corner of the area in the spritesheet corresponding to the <see cref="P:Terraria.Tile.TileType" /> that should be drawn at this position. Custom drawing logic can adjust these values.<para />
	/// Some tiles such as Christmas Tree and Weapon Rack use the higher bits of these fields to do tile-specific behaviors. Modders should not attempt to do similar approaches, but should use <see cref="T:Terraria.ModLoader.ModTileEntity" />s.<para />
	/// Legacy/vanilla equivalent is <see cref="P:Terraria.Tile.frameX" />.
	/// </summary>
	public ref short TileFrameX => ref Get<TileWallWireStateData>().TileFrameX;

	/// <summary>
	/// The Y coordinate of the top left corner of the area in the spritesheet for the <see cref="P:Terraria.Tile.TileType" /> to be used to draw the tile at this position.<para />
	/// For a Framed tile, this value is set automatically according to the framing logic as the world loads or other tiles are placed or mined nearby. See <see href="https://github.com/tModLoader/tModLoader/wiki/Basic-Tile#framed-vs-frameimportant-tiles">Framed vs FrameImportant</see> for more info. For <see cref="F:Terraria.Main.tileFrameImportant" /> tiles, this value will not change due to tile framing and will be saved and synced in Multiplayer. In either case, <see cref="P:Terraria.Tile.TileFrameX" /> and <see cref="P:Terraria.Tile.TileFrameY" /> correspond to the coordinates of the top left corner of the area in the spritesheet corresponding to the <see cref="P:Terraria.Tile.TileType" /> that should be drawn at this position. Custom drawing logic can adjust these values.<para />
	/// Some tiles such as Christmas Tree and Weapon Rack use the higher bits of these fields to do tile-specific behaviors. Modders should not attempt to do similar approaches, but should use <see cref="T:Terraria.ModLoader.ModTileEntity" />s.<para />
	/// Legacy/vanilla equivalent is <see cref="P:Terraria.Tile.frameY" />.
	/// </summary>
	public ref short TileFrameY => ref Get<TileWallWireStateData>().TileFrameY;

	/// <summary>
	/// The X coordinate of the top left corner of the area in the spritesheet for the <see cref="P:Terraria.Tile.WallType" /> to be used to draw the wall at this position.<para />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.wallFrameX" /> or <see cref="M:Terraria.Tile.wallFrameX(System.Int32)" />.
	/// </summary>
	public int WallFrameX
	{
		get
		{
			return Get<TileWallWireStateData>().WallFrameX;
		}
		set
		{
			Get<TileWallWireStateData>().WallFrameX = value;
		}
	}

	/// <summary>
	/// The Y coordinate of the top left corner of the area in the spritesheet for the <see cref="P:Terraria.Tile.WallType" /> to be used to draw the wall at this position.<para />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.wallFrameY" /> or <see cref="M:Terraria.Tile.wallFrameY(System.Int32)" />.
	/// </summary>
	public int WallFrameY
	{
		get
		{
			return Get<TileWallWireStateData>().WallFrameY;
		}
		set
		{
			Get<TileWallWireStateData>().WallFrameY = value;
		}
	}

	/// <summary>
	/// The random style number the tile at this position has, which is random number between 0 and 2 (inclusive).<br />
	/// This is used in non-<see cref="F:Terraria.Main.tileFrameImportant" /> tiles (aka "Terrain" tiles) to provide visual variation and is not synced in multiplayer nor will it be preserved when saving and loading the world.<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.frameNumber" /> or <see cref="M:Terraria.Tile.frameNumber(System.Byte)" />.
	/// </summary>
	public int TileFrameNumber
	{
		get
		{
			return Get<TileWallWireStateData>().TileFrameNumber;
		}
		set
		{
			Get<TileWallWireStateData>().TileFrameNumber = value;
		}
	}

	/// <summary>
	/// The random style number the wall at this position has, which is a random number between 0 and 2 (inclusive).<br />
	/// This is used to provide visual variation and is not synced in multiplayer nor will it be preserved when saving and loading the world.<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.wallFrameNumber" /> or <see cref="M:Terraria.Tile.wallFrameNumber(System.Byte)" />.
	/// </summary>
	public int WallFrameNumber
	{
		get
		{
			return Get<TileWallWireStateData>().WallFrameNumber;
		}
		set
		{
			Get<TileWallWireStateData>().WallFrameNumber = value;
		}
	}

	/// <summary>
	/// The <see cref="T:Terraria.ID.PaintID" /> the tile at this position is painted with. Is <see cref="F:Terraria.ID.PaintID.None" /> if not painted.<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.color" /> or <see cref="M:Terraria.Tile.color(System.Byte)" />.
	/// </summary>
	public byte TileColor
	{
		get
		{
			return Get<TileWallWireStateData>().TileColor;
		}
		set
		{
			Get<TileWallWireStateData>().TileColor = value;
		}
	}

	/// <summary>
	/// The <see cref="T:Terraria.ID.PaintID" /> the wall at this position is painted with. Is <see cref="F:Terraria.ID.PaintID.None" /> if not painted.<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.wallColor" /> or <see cref="M:Terraria.Tile.wallColor(System.Byte)" />.
	/// </summary>
	public byte WallColor
	{
		get
		{
			return Get<TileWallWireStateData>().WallColor;
		}
		set
		{
			Get<TileWallWireStateData>().WallColor = value;
		}
	}

	/// <summary>
	/// The amount of liquid at this position.<br />
	/// Ranges from 0, no liquid, to 255, filled with liquid.<br />
	/// Legacy/vanilla equivalent is <see cref="P:Terraria.Tile.liquid" />.
	/// </summary>
	public ref byte LiquidAmount => ref Get<LiquidData>().Amount;

	/// <summary>
	/// The <see cref="T:Terraria.ID.LiquidID" /> of the liquid at this position.<br />
	/// Make sure to check that <see cref="P:Terraria.Tile.LiquidAmount" /> is greater than 0.<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.liquidType" /> or <see cref="M:Terraria.Tile.liquidType(System.Int32)" />.
	/// </summary>
	public int LiquidType
	{
		get
		{
			return Get<LiquidData>().LiquidType;
		}
		set
		{
			Get<LiquidData>().LiquidType = value;
		}
	}

	/// <summary>
	/// Whether the liquid at this position should skip updating for 1 tick.<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.skipLiquid" /> or <see cref="M:Terraria.Tile.skipLiquid(System.Boolean)" />.
	/// </summary>
	public bool SkipLiquid
	{
		get
		{
			return Get<LiquidData>().SkipLiquid;
		}
		set
		{
			Get<LiquidData>().SkipLiquid = value;
		}
	}

	/// <summary>
	/// Whether there is liquid at this position.<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.checkingLiquid" /> or <see cref="M:Terraria.Tile.checkingLiquid(System.Boolean)" />.
	/// </summary>
	public bool CheckingLiquid
	{
		get
		{
			return Get<LiquidData>().CheckingLiquid;
		}
		set
		{
			Get<LiquidData>().CheckingLiquid = value;
		}
	}

	/// <summary>
	/// Whether there is red wire at this position.<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.wire" /> or <see cref="M:Terraria.Tile.wire(System.Boolean)" />.
	/// </summary>
	public bool RedWire
	{
		get
		{
			return Get<TileWallWireStateData>().RedWire;
		}
		set
		{
			Get<TileWallWireStateData>().RedWire = value;
		}
	}

	/// <summary>
	/// Whether there is green wire at this position.<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.wire3" /> or <see cref="M:Terraria.Tile.wire3(System.Boolean)" />.
	/// </summary>
	public bool GreenWire
	{
		get
		{
			return Get<TileWallWireStateData>().GreenWire;
		}
		set
		{
			Get<TileWallWireStateData>().GreenWire = value;
		}
	}

	/// <summary>
	/// Whether there is blue wire at this position.<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.wire2" /> or <see cref="M:Terraria.Tile.wire2(System.Boolean)" />.
	/// </summary>
	public bool BlueWire
	{
		get
		{
			return Get<TileWallWireStateData>().BlueWire;
		}
		set
		{
			Get<TileWallWireStateData>().BlueWire = value;
		}
	}

	/// <summary>
	/// Whether there is yellow wire at this position.<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.wire4" /> or <see cref="M:Terraria.Tile.wire4(System.Boolean)" />.
	/// </summary>
	public bool YellowWire
	{
		get
		{
			return Get<TileWallWireStateData>().YellowWire;
		}
		set
		{
			Get<TileWallWireStateData>().YellowWire = value;
		}
	}

	/// <summary>
	/// Whether the tile at this position is invisible. Used by <see cref="F:Terraria.ID.ItemID.EchoCoating" />.<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.invisibleBlock" /> or <see cref="M:Terraria.Tile.invisibleBlock(System.Boolean)" />.
	/// </summary>
	public bool IsTileInvisible
	{
		get
		{
			return Get<TileWallBrightnessInvisibilityData>().IsTileInvisible;
		}
		set
		{
			Get<TileWallBrightnessInvisibilityData>().IsTileInvisible = value;
		}
	}

	/// <summary>
	/// Whether the wall at this position is invisible. Used by <see cref="F:Terraria.ID.ItemID.EchoCoating" />.<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.invisibleWall" /> or <see cref="M:Terraria.Tile.invisibleWall(System.Boolean)" />.
	/// </summary>
	public bool IsWallInvisible
	{
		get
		{
			return Get<TileWallBrightnessInvisibilityData>().IsWallInvisible;
		}
		set
		{
			Get<TileWallBrightnessInvisibilityData>().IsWallInvisible = value;
		}
	}

	/// <summary>
	/// Whether the tile at this position is fully illuminated. Used by <see cref="F:Terraria.ID.ItemID.GlowPaint" />.<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.fullbrightBlock" /> or <see cref="M:Terraria.Tile.fullbrightBlock(System.Boolean)" />.
	/// </summary>
	public bool IsTileFullbright
	{
		get
		{
			return Get<TileWallBrightnessInvisibilityData>().IsTileFullbright;
		}
		set
		{
			Get<TileWallBrightnessInvisibilityData>().IsTileFullbright = value;
		}
	}

	/// <summary>
	/// Whether the wall at this position is fully illuminated. Used by <see cref="F:Terraria.ID.ItemID.GlowPaint" />.<br />
	/// Legacy/vanilla equivalent is <see cref="M:Terraria.Tile.fullbrightWall" /> or <see cref="M:Terraria.Tile.fullbrightWall(System.Boolean)" />.
	/// </summary>
	public bool IsWallFullbright
	{
		get
		{
			return Get<TileWallBrightnessInvisibilityData>().IsWallFullbright;
		}
		set
		{
			Get<TileWallBrightnessInvisibilityData>().IsWallFullbright = value;
		}
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.TileType" /> instead.
	/// </summary>
	internal ref ushort type => ref TileType;

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.WallType" /> instead.
	/// </summary>
	internal ref ushort wall => ref WallType;

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.TileFrameX" /> instead.
	/// </summary>
	internal ref short frameX => ref TileFrameX;

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.TileFrameY" /> instead.
	/// </summary>
	internal ref short frameY => ref TileFrameY;

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.LiquidAmount" /> instead.
	/// </summary>
	internal ref byte liquid => ref LiquidAmount;

	public object Clone()
	{
		return MemberwiseClone();
	}

	/// <summary>
	/// Resets all of the data at this position.<br />
	/// To only remove the tile data, use <see cref="M:Terraria.Tile.ClearTile" />.
	/// </summary>
	public void ClearEverything()
	{
		TileData.ClearSingle(TileId);
	}

	/// <summary>
	/// Resets the tile data at this position.<br />
	/// Sets <see cref="P:Terraria.Tile.HasTile" /> and <see cref="P:Terraria.Tile.IsActuated" /> to <see langword="false" /> and sets the <see cref="P:Terraria.Tile.BlockType" /> to <see cref="F:Terraria.ID.BlockType.Solid" />.
	/// </summary>
	/// <remarks>
	/// Does not reset data related to walls, wires, or anything else. For that, use <see cref="M:Terraria.Tile.ClearEverything" />.
	/// </remarks>
	public void ClearTile()
	{
		slope(0);
		halfBrick(halfBrick: false);
		active(active: false);
		inActive(inActive: false);
	}

	/// <summary>
	/// Copies all data from the given position to this position.
	/// </summary>
	/// <param name="from">The position to copy the data from.</param>
	public void CopyFrom(Tile from)
	{
		TileData.CopySingle(from.TileId, TileId);
	}

	/// <summary>
	/// Legacy code, consider the data you want to compare directly.
	/// </summary>
	/// <param name="compTile"></param>
	/// <returns></returns>
	internal bool isTheSameAs(Tile compTile)
	{
		if (Get<TileWallWireStateData>().NonFrameBits != compTile.Get<TileWallWireStateData>().NonFrameBits)
		{
			return false;
		}
		if (wall != compTile.wall || liquid != compTile.liquid)
		{
			return false;
		}
		if (liquid > 0 && LiquidType != compTile.LiquidType)
		{
			return false;
		}
		if (Get<TileWallBrightnessInvisibilityData>().Data != compTile.Get<TileWallBrightnessInvisibilityData>().Data)
		{
			return false;
		}
		if (HasTile)
		{
			if (type != compTile.type)
			{
				return false;
			}
			if (Main.tileFrameImportant[type] && (frameX != compTile.frameX || frameY != compTile.frameY))
			{
				return false;
			}
		}
		return true;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.BlockType" /> instead.
	/// </summary>
	/// <returns></returns>
	internal int blockType()
	{
		if (halfBrick())
		{
			return 1;
		}
		int num = slope();
		if (num > 0)
		{
			num++;
		}
		return num;
	}

	/// <summary>
	/// Resets all of the data at this position except for the <see cref="P:Terraria.Tile.WallType" />, and sets <see cref="P:Terraria.Tile.TileType" /> to <paramref name="type" />. 
	/// </summary>
	/// <param name="type">The <see cref="T:Terraria.ID.TileID" /> to set this tile to.</param>
	public void ResetToType(ushort type)
	{
		ClearMetadata();
		HasTile = true;
		TileType = type;
	}

	internal void ClearMetadata()
	{
		Get<LiquidData>() = default(LiquidData);
		Get<TileWallWireStateData>() = default(TileWallWireStateData);
		Get<TileWallBrightnessInvisibilityData>() = default(TileWallBrightnessInvisibilityData);
	}

	internal Color actColor(Color oldColor)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		if (!inActive())
		{
			return oldColor;
		}
		double num = 0.4;
		return new Color((int)(byte)(num * (double)(int)((Color)(ref oldColor)).R), (int)(byte)(num * (double)(int)((Color)(ref oldColor)).G), (int)(byte)(num * (double)(int)((Color)(ref oldColor)).B), (int)((Color)(ref oldColor)).A);
	}

	internal void actColor(ref Vector3 oldColor)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		if (inActive())
		{
			oldColor *= 0.4f;
		}
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.TopSlope" /> instead.
	/// </summary>
	/// <returns></returns>
	internal bool topSlope()
	{
		byte b = slope();
		if (b != 1)
		{
			return b == 2;
		}
		return true;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.BottomSlope" /> instead.
	/// </summary>
	/// <returns></returns>
	internal bool bottomSlope()
	{
		byte b = slope();
		if (b != 3)
		{
			return b == 4;
		}
		return true;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.LeftSlope" /> instead.
	/// </summary>
	/// <returns></returns>
	internal bool leftSlope()
	{
		byte b = slope();
		if (b != 2)
		{
			return b == 4;
		}
		return true;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.RightSlope" /> instead.
	/// </summary>
	/// <returns></returns>
	internal bool rightSlope()
	{
		byte b = slope();
		if (b != 1)
		{
			return b == 3;
		}
		return true;
	}

	/// <summary>
	/// Clears the specified data at this position based on the given <see cref="T:Terraria.DataStructures.TileDataType" />.
	/// </summary>
	/// <param name="types">The <see cref="T:Terraria.DataStructures.TileDataType" /> to clear.</param>
	public void Clear(TileDataType types)
	{
		if ((types & TileDataType.Tile) != 0)
		{
			type = 0;
			active(active: false);
			frameX = 0;
			frameY = 0;
		}
		if ((types & TileDataType.Wall) != 0)
		{
			wall = 0;
			wallFrameX(0);
			wallFrameY(0);
		}
		if ((types & TileDataType.TilePaint) != 0)
		{
			ClearBlockPaintAndCoating();
		}
		if ((types & TileDataType.WallPaint) != 0)
		{
			ClearWallPaintAndCoating();
		}
		if ((types & TileDataType.Liquid) != 0)
		{
			liquid = 0;
			liquidType(0);
			checkingLiquid(checkingLiquid: false);
		}
		if ((types & TileDataType.Slope) != 0)
		{
			slope(0);
			halfBrick(halfBrick: false);
		}
		if ((types & TileDataType.Wiring) != 0)
		{
			wire(wire: false);
			wire2(wire2: false);
			wire3(wire3: false);
			wire4(wire4: false);
		}
		if ((types & TileDataType.Actuator) != 0)
		{
			actuator(actuator: false);
			inActive(inActive: false);
		}
	}

	/// <summary>
	/// Slopes a tile based on the tiles adjacent to it.
	/// </summary>
	/// <param name="x">The X coordinate of the tile.</param>
	/// <param name="y">The Y coordinate of the tile.</param>
	/// <param name="applyToNeighbors">Whether the adjacent tiles should be automatically smoothed.</param>
	/// <param name="sync">Whether the changes should automatically be synced to multiplayer.</param>
	public static void SmoothSlope(int x, int y, bool applyToNeighbors = true, bool sync = false)
	{
		if (applyToNeighbors)
		{
			SmoothSlope(x + 1, y, applyToNeighbors: false, sync);
			SmoothSlope(x - 1, y, applyToNeighbors: false, sync);
			SmoothSlope(x, y + 1, applyToNeighbors: false, sync);
			SmoothSlope(x, y - 1, applyToNeighbors: false, sync);
		}
		Tile tile = Main.tile[x, y];
		if (!WorldGen.CanPoundTile(x, y) || !WorldGen.SolidOrSlopedTile(x, y))
		{
			return;
		}
		bool flag = !WorldGen.TileEmpty(x, y - 1);
		bool flag2 = !WorldGen.SolidOrSlopedTile(x, y - 1) && flag;
		bool flag3 = WorldGen.SolidOrSlopedTile(x, y + 1);
		bool flag4 = WorldGen.SolidOrSlopedTile(x - 1, y);
		bool flag5 = WorldGen.SolidOrSlopedTile(x + 1, y);
		int num = ((flag ? 1 : 0) << 3) | ((flag3 ? 1 : 0) << 2) | ((flag4 ? 1 : 0) << 1) | (flag5 ? 1 : 0);
		bool flag6 = tile.halfBrick();
		int num2 = tile.slope();
		switch (num)
		{
		case 10:
			if (!flag2)
			{
				tile.halfBrick(halfBrick: false);
				tile.slope(3);
			}
			break;
		case 9:
			if (!flag2)
			{
				tile.halfBrick(halfBrick: false);
				tile.slope(4);
			}
			break;
		case 6:
			tile.halfBrick(halfBrick: false);
			tile.slope(1);
			break;
		case 5:
			tile.halfBrick(halfBrick: false);
			tile.slope(2);
			break;
		case 4:
			tile.slope(0);
			tile.halfBrick(halfBrick: true);
			break;
		default:
			tile.halfBrick(halfBrick: false);
			tile.slope(0);
			break;
		}
		if (sync)
		{
			int num3 = tile.slope();
			bool flag7 = flag6 != tile.halfBrick();
			bool flag8 = num2 != num3;
			if (flag7 && flag8)
			{
				NetMessage.SendData(17, -1, -1, null, 23, x, y, num3);
			}
			else if (flag7)
			{
				NetMessage.SendData(17, -1, -1, null, 7, x, y, 1f);
			}
			else if (flag8)
			{
				NetMessage.SendData(17, -1, -1, null, 14, x, y, num3);
			}
		}
	}

	/// <summary>
	/// Copies the paint and coating data from the specified tile to this tile.<br />
	/// Does not copy wall paint and coating data.
	/// </summary>
	/// <param name="other">The <see cref="T:Terraria.Tile" /> to copy the data from.</param>
	public void CopyPaintAndCoating(Tile other)
	{
		color(other.color());
		invisibleBlock(other.invisibleBlock());
		fullbrightBlock(other.fullbrightBlock());
	}

	/// <summary>
	/// Gets the paint and coating information from the tile at this position as a <see cref="T:Terraria.TileColorCache" />.
	/// </summary>
	/// <returns>A <see cref="T:Terraria.TileColorCache" /> representing the paint and coatings on the tile at this position.</returns>
	public TileColorCache BlockColorAndCoating()
	{
		TileColorCache result = default(TileColorCache);
		result.Color = color();
		result.FullBright = fullbrightBlock();
		result.Invisible = invisibleBlock();
		return result;
	}

	/// <summary>
	/// Gets the paint and coating information from the wall at this position as a <see cref="T:Terraria.TileColorCache" />.
	/// </summary>
	/// <returns>A <see cref="T:Terraria.TileColorCache" /> representing the paint and coatings on the wall at this position.</returns>
	public TileColorCache WallColorAndCoating()
	{
		TileColorCache result = default(TileColorCache);
		result.Color = wallColor();
		result.FullBright = fullbrightWall();
		result.Invisible = invisibleWall();
		return result;
	}

	/// <summary>
	/// Sets the paint and coating of the tile at this position based on the given <see cref="T:Terraria.TileColorCache" />.
	/// </summary>
	/// <param name="cache">The <see cref="T:Terraria.TileColorCache" /> to apply.</param>
	public void UseBlockColors(TileColorCache cache)
	{
		cache.ApplyToBlock(this);
	}

	/// <summary>
	/// Sets the paint and coating of the wall at this position based on the given <see cref="T:Terraria.TileColorCache" />.
	/// </summary>
	/// <param name="cache">The <see cref="T:Terraria.TileColorCache" /> to apply.</param>
	public void UseWallColors(TileColorCache cache)
	{
		cache.ApplyToWall(this);
	}

	/// <summary>
	/// Clears any paint or coating on the tile at this position.
	/// </summary>
	public void ClearBlockPaintAndCoating()
	{
		color(0);
		fullbrightBlock(fullbrightBlock: false);
		invisibleBlock(invisibleBlock: false);
	}

	/// <summary>
	/// Clears any paint or coating on the wall at this position.
	/// </summary>
	public void ClearWallPaintAndCoating()
	{
		wallColor(0);
		fullbrightWall(fullbrightWall: false);
		invisibleWall(invisibleWall: false);
	}

	public override string ToString()
	{
		return "Tile Type:" + type + " Active:" + active() + " Wall:" + wall + " Slope:" + slope() + " fX:" + frameX + " fY:" + frameY;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal Tile(uint tileId)
	{
		TileId = tileId;
	}

	/// <summary>
	/// Used to get a reference to the <see cref="T:Terraria.ITileData" /> at this position.
	/// </summary>
	/// <typeparam name="T">The <see cref="T:Terraria.ITileData" /> to get.</typeparam>
	/// <returns>The <see cref="T:Terraria.ITileData" /> of type <typeparamref name="T" /> at this position.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe ref T Get<T>() where T : unmanaged, ITileData
	{
		return ref TileData<T>.ptr[TileId];
	}

	public override int GetHashCode()
	{
		return (int)TileId;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(Tile tile, Tile tile2)
	{
		return tile.TileId == tile2.TileId;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(Tile tile, Tile tile2)
	{
		return tile.TileId != tile2.TileId;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(Tile tile, ArgumentException justSoYouCanCompareWithNull)
	{
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(Tile tile, ArgumentException justSoYouCanCompareWithNull)
	{
		return true;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.HasTile" /> instead.
	/// </summary>
	/// <returns></returns>
	internal bool active()
	{
		return HasTile;
	}

	/// <inheritdoc cref="M:Terraria.Tile.active" />
	internal void active(bool active)
	{
		HasTile = active;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.IsActuated" /> instead.
	/// </summary>
	/// <returns></returns>
	internal bool inActive()
	{
		return IsActuated;
	}

	/// <inheritdoc cref="M:Terraria.Tile.inActive" />
	internal void inActive(bool inActive)
	{
		IsActuated = inActive;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.HasActuator" /> instead.
	/// </summary>
	/// <returns></returns>
	internal bool actuator()
	{
		return HasActuator;
	}

	/// <inheritdoc cref="M:Terraria.Tile.actuator" />
	internal void actuator(bool actuator)
	{
		HasActuator = actuator;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.HasUnactuatedTile" /> instead.
	/// </summary>
	/// <returns></returns>
	internal bool nactive()
	{
		return HasUnactuatedTile;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.Slope" /> instead.
	/// </summary>
	/// <returns></returns>
	internal byte slope()
	{
		return (byte)Slope;
	}

	/// <inheritdoc cref="M:Terraria.Tile.slope" />
	internal void slope(byte slope)
	{
		Slope = (SlopeType)slope;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.IsHalfBlock" /> instead.
	/// </summary>
	/// <returns></returns>
	internal bool halfBrick()
	{
		return IsHalfBlock;
	}

	/// <inheritdoc cref="M:Terraria.Tile.halfBrick" />
	internal void halfBrick(bool halfBrick)
	{
		IsHalfBlock = halfBrick;
	}

	/// <summary>
	/// Legacy code, use <c>tile1.Slope == tile2.Slope</c> instead.
	/// </summary>
	/// <param name="tile"></param>
	/// <returns></returns>
	internal bool HasSameSlope(Tile tile)
	{
		return Slope == tile.Slope;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.WallFrameX" /> instead.
	/// </summary>
	/// <returns></returns>
	internal int wallFrameX()
	{
		return WallFrameX;
	}

	/// <inheritdoc cref="M:Terraria.Tile.wallFrameX" />
	internal void wallFrameX(int wallFrameX)
	{
		WallFrameX = wallFrameX;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.WallFrameY" /> instead.
	/// </summary>
	/// <returns></returns>
	internal int wallFrameY()
	{
		return WallFrameY;
	}

	/// <inheritdoc cref="M:Terraria.Tile.wallFrameY" />
	internal void wallFrameY(int wallFrameY)
	{
		WallFrameY = wallFrameY;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.TileFrameNumber" /> instead.
	/// </summary>
	/// <returns></returns>
	internal byte frameNumber()
	{
		return (byte)TileFrameNumber;
	}

	/// <inheritdoc cref="M:Terraria.Tile.frameNumber" />
	internal void frameNumber(byte frameNumber)
	{
		TileFrameNumber = frameNumber;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.WallFrameNumber" /> instead.
	/// </summary>
	/// <returns></returns>
	internal byte wallFrameNumber()
	{
		return (byte)WallFrameNumber;
	}

	/// <inheritdoc cref="M:Terraria.Tile.wallFrameNumber" />
	internal void wallFrameNumber(byte wallFrameNumber)
	{
		WallFrameNumber = wallFrameNumber;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.TileColor" /> instead.
	/// </summary>
	/// <returns></returns>
	internal byte color()
	{
		return TileColor;
	}

	/// <inheritdoc cref="M:Terraria.Tile.color" />
	internal void color(byte color)
	{
		TileColor = color;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.WallColor" /> instead.
	/// </summary>
	/// <returns></returns>
	internal byte wallColor()
	{
		return WallColor;
	}

	/// <inheritdoc cref="M:Terraria.Tile.wallColor" />
	internal void wallColor(byte wallColor)
	{
		WallColor = wallColor;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.LiquidType" /> instead.
	/// </summary>
	/// <returns></returns>
	internal byte liquidType()
	{
		return (byte)LiquidType;
	}

	/// <inheritdoc cref="M:Terraria.Tile.liquidType" />
	internal void liquidType(int liquidType)
	{
		LiquidType = liquidType;
	}

	/// <summary>
	/// Legacy code, use <c>tile.LiquidType == LiquidID.Lava</c> instead.
	/// </summary>
	/// <returns></returns>
	internal bool lava()
	{
		return LiquidType == 1;
	}

	/// <inheritdoc cref="M:Terraria.Tile.lava" />
	internal void lava(bool lava)
	{
		SetIsLiquidType(1, lava);
	}

	/// <summary>
	/// Legacy code, use <c>tile.LiquidType == LiquidID.Honey</c> instead.
	/// </summary>
	/// <returns></returns>
	internal bool honey()
	{
		return LiquidType == 2;
	}

	/// <inheritdoc cref="M:Terraria.Tile.honey" />
	internal void honey(bool honey)
	{
		SetIsLiquidType(2, honey);
	}

	/// <summary>
	/// Legacy code, use <c>tile.LiquidType == LiquidID.Shimmer</c> instead.
	/// </summary>
	/// <returns></returns>
	internal bool shimmer()
	{
		return LiquidType == 3;
	}

	/// <inheritdoc cref="M:Terraria.Tile.shimmer" />
	internal void shimmer(bool shimmer)
	{
		SetIsLiquidType(3, shimmer);
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.SkipLiquid" /> instead.
	/// </summary>
	/// <returns></returns>
	internal bool skipLiquid()
	{
		return SkipLiquid;
	}

	/// <inheritdoc cref="M:Terraria.Tile.skipLiquid" />
	internal void skipLiquid(bool skipLiquid)
	{
		SkipLiquid = skipLiquid;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.CheckingLiquid" /> instead.
	/// </summary>
	/// <returns></returns>
	internal bool checkingLiquid()
	{
		return CheckingLiquid;
	}

	/// <inheritdoc cref="M:Terraria.Tile.checkingLiquid" />
	internal void checkingLiquid(bool checkingLiquid)
	{
		CheckingLiquid = checkingLiquid;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.RedWire" /> instead.
	/// </summary>
	/// <returns></returns>
	internal bool wire()
	{
		return RedWire;
	}

	/// <inheritdoc cref="M:Terraria.Tile.wire" />
	internal void wire(bool wire)
	{
		RedWire = wire;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.BlueWire" /> instead.
	/// </summary>
	/// <returns></returns>
	internal bool wire2()
	{
		return BlueWire;
	}

	/// <inheritdoc cref="M:Terraria.Tile.wire2" />
	internal void wire2(bool wire2)
	{
		BlueWire = wire2;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.GreenWire" /> instead.
	/// </summary>
	/// <returns></returns>
	internal bool wire3()
	{
		return GreenWire;
	}

	/// <inheritdoc cref="M:Terraria.Tile.wire3" />
	internal void wire3(bool wire3)
	{
		GreenWire = wire3;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.YellowWire" /> instead.
	/// </summary>
	/// <returns></returns>
	internal bool wire4()
	{
		return YellowWire;
	}

	/// <inheritdoc cref="M:Terraria.Tile.wire4" />
	internal void wire4(bool wire4)
	{
		YellowWire = wire4;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.IsTileInvisible" /> instead.
	/// </summary>
	/// <returns></returns>
	internal bool invisibleBlock()
	{
		return IsTileInvisible;
	}

	/// <inheritdoc cref="M:Terraria.Tile.invisibleBlock" />
	internal void invisibleBlock(bool invisibleBlock)
	{
		IsTileInvisible = invisibleBlock;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.IsWallInvisible" /> instead.
	/// </summary>
	/// <returns></returns>
	internal bool invisibleWall()
	{
		return IsWallInvisible;
	}

	/// <inheritdoc cref="M:Terraria.Tile.invisibleWall" />
	internal void invisibleWall(bool invisibleWall)
	{
		IsWallInvisible = invisibleWall;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.IsTileFullbright" /> instead.
	/// </summary>
	/// <returns></returns>
	internal bool fullbrightBlock()
	{
		return IsTileFullbright;
	}

	/// <inheritdoc cref="M:Terraria.Tile.fullbrightBlock" />
	internal void fullbrightBlock(bool fullbrightBlock)
	{
		IsTileFullbright = fullbrightBlock;
	}

	/// <summary>
	/// Legacy code, use <see cref="P:Terraria.Tile.IsWallFullbright" /> instead.
	/// </summary>
	/// <returns></returns>
	internal bool fullbrightWall()
	{
		return IsWallFullbright;
	}

	/// <inheritdoc cref="M:Terraria.Tile.fullbrightWall" />
	internal void fullbrightWall(bool fullbrightWall)
	{
		IsWallFullbright = fullbrightWall;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SetIsLiquidType(int liquidId, bool value)
	{
		if (value)
		{
			LiquidType = liquidId;
		}
		else if (LiquidType == liquidId)
		{
			LiquidType = 0;
		}
	}
}
