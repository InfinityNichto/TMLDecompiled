using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Biomes;
using Terraria.GameContent.Biomes.CaveHouse;
using Terraria.GameContent.Metadata;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace Terraria.ModLoader;

/// <summary>
/// This serves as the central class from which tile-related functions are supported and carried out.
/// </summary>
public static class TileLoader
{
	private delegate void DelegateNumDust(int i, int j, int type, bool fail, ref int num);

	private delegate bool DelegateCreateDust(int i, int j, int type, ref int dustType);

	private delegate void DelegateDropCritterChance(int i, int j, int type, ref int wormChance, ref int grassHopperChance, ref int jungleGrubChance);

	private delegate bool DelegateCanKillTile(int i, int j, int type, ref bool blockDamaged);

	private delegate void DelegateKillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem);

	private delegate void DelegateModifyLight(int i, int j, int type, ref float r, ref float g, ref float b);

	private delegate bool? DelegateIsTileBiomeSightable(int i, int j, int type, ref Color sightColor);

	private delegate void DelegateSetSpriteEffects(int i, int j, int type, ref SpriteEffects spriteEffects);

	private delegate void DelegateDrawEffects(int i, int j, int type, SpriteBatch spriteBatch, ref TileDrawInfo drawData);

	private delegate bool DelegateTileFrame(int i, int j, int type, ref bool resetFrame, ref bool noBreak);

	private delegate void DelegateChangeWaterfallStyle(int type, ref int style);

	private static int nextTile = TileID.Count;

	internal static readonly IList<ModTile> tiles = new List<ModTile>();

	internal static readonly IList<GlobalTile> globalTiles = new List<GlobalTile>();

	/// <summary> Maps Tile type and Tile style to the Item type that places the tile with the style. </summary>
	internal static readonly Dictionary<(int, int), int> tileTypeAndTileStyleToItemType = new Dictionary<(int, int), int>();

	private static bool loaded = false;

	private static readonly int vanillaChairCount = TileID.Sets.RoomNeeds.CountsAsChair.Length;

	private static readonly int vanillaTableCount = TileID.Sets.RoomNeeds.CountsAsTable.Length;

	private static readonly int vanillaTorchCount = TileID.Sets.RoomNeeds.CountsAsTorch.Length;

	private static readonly int vanillaDoorCount = TileID.Sets.RoomNeeds.CountsAsDoor.Length;

	private static Func<int, int, int, bool, bool>[] HookKillSound;

	private static DelegateNumDust[] HookNumDust;

	private static DelegateCreateDust[] HookCreateDust;

	private static DelegateDropCritterChance[] HookDropCritterChance;

	private static Func<int, int, int, bool>[] HookCanDrop;

	private static Action<int, int, int>[] HookDrop;

	private static DelegateCanKillTile[] HookCanKillTile;

	private static DelegateKillTile[] HookKillTile;

	private static Func<int, int, int, bool>[] HookCanExplode;

	private static Action<int, int, int, bool>[] HookNearbyEffects;

	private static DelegateModifyLight[] HookModifyLight;

	private static Func<int, int, int, Player, bool?>[] HookIsTileDangerous;

	private static DelegateIsTileBiomeSightable[] HookIsTileBiomeSightable;

	private static Func<int, int, int, bool?>[] HookIsTileSpelunkable;

	private static DelegateSetSpriteEffects[] HookSetSpriteEffects;

	private static Action[] HookAnimateTile;

	private static Func<int, int, int, SpriteBatch, bool>[] HookPreDraw;

	private static DelegateDrawEffects[] HookDrawEffects;

	private static Action<int, int, int, SpriteBatch>[] HookPostDraw;

	private static Action<int, int, int, SpriteBatch>[] HookSpecialDraw;

	private static Action<int, int, int>[] HookRandomUpdate;

	private static DelegateTileFrame[] HookTileFrame;

	private static Func<int, int, int, bool>[] HookCanPlace;

	private static Func<int, int, int, int, bool>[] HookCanReplace;

	private static Func<int, int[]>[] HookAdjTiles;

	private static Action<int, int, int>[] HookRightClick;

	private static Action<int, int, int>[] HookMouseOver;

	private static Action<int, int, int>[] HookMouseOverFar;

	private static Func<int, int, int, Item, bool>[] HookAutoSelect;

	private static Func<int, int, int, bool>[] HookPreHitWire;

	private static Action<int, int, int>[] HookHitWire;

	private static Func<int, int, int, bool>[] HookSlope;

	private static Action<int, Player>[] HookFloorVisuals;

	private static DelegateChangeWaterfallStyle[] HookChangeWaterfallStyle;

	private static Action<int, int, int, Item>[] HookPlaceInWorld;

	private static Action[] HookPostSetupTileMerge;

	public static int TileCount => nextTile;

	internal static int ReserveTileID()
	{
		if (ModNet.AllowVanillaClients)
		{
			throw new Exception("Adding tiles breaks vanilla client compatibility");
		}
		int result = nextTile;
		nextTile++;
		return result;
	}

	/// <summary>
	/// Gets the ModTile instance with the given type. If no ModTile with the given type exists, returns null.
	/// </summary>
	/// <param name="type">The type of the ModTile</param>
	/// <returns>The ModTile instance in the tiles array, null if not found.</returns>
	public static ModTile GetTile(int type)
	{
		if (type < TileID.Count || type >= TileCount)
		{
			return null;
		}
		return tiles[type - TileID.Count];
	}

	private static void Resize2DArray<T>(ref T[,] array, int newSize)
	{
		int dim1 = array.GetLength(0);
		int dim2 = array.GetLength(1);
		T[,] newArray = new T[newSize, dim2];
		for (int i = 0; i < newSize && i < dim1; i++)
		{
			for (int j = 0; j < dim2; j++)
			{
				newArray[i, j] = array[i, j];
			}
		}
		array = newArray;
	}

	internal static void ResizeArrays(bool unloading = false)
	{
		Array.Resize(ref TextureAssets.Tile, nextTile);
		Array.Resize(ref TextureAssets.HighlightMask, nextTile);
		LoaderUtils.ResetStaticMembers(typeof(TileID));
		Array.Resize(ref Main.SceneMetrics._tileCounts, nextTile);
		Array.Resize(ref Main.PylonSystem._sceneMetrics._tileCounts, nextTile);
		Array.Resize(ref Main.tileLighted, nextTile);
		Array.Resize(ref Main.tileMergeDirt, nextTile);
		Array.Resize(ref Main.tileCut, nextTile);
		Array.Resize(ref Main.tileAlch, nextTile);
		Array.Resize(ref Main.tileShine, nextTile);
		Array.Resize(ref Main.tileShine2, nextTile);
		Array.Resize(ref Main.tileStone, nextTile);
		Array.Resize(ref Main.tileAxe, nextTile);
		Array.Resize(ref Main.tileHammer, nextTile);
		Array.Resize(ref Main.tileWaterDeath, nextTile);
		Array.Resize(ref Main.tileLavaDeath, nextTile);
		Array.Resize(ref Main.tileTable, nextTile);
		Array.Resize(ref Main.tileBlockLight, nextTile);
		Array.Resize(ref Main.tileNoSunLight, nextTile);
		Array.Resize(ref Main.tileDungeon, nextTile);
		Array.Resize(ref Main.tileSpelunker, nextTile);
		Array.Resize(ref Main.tileSolidTop, nextTile);
		Array.Resize(ref Main.tileSolid, nextTile);
		Array.Resize(ref Main.tileBouncy, nextTile);
		Array.Resize(ref Main.tileLargeFrames, nextTile);
		Array.Resize(ref Main.tileRope, nextTile);
		Array.Resize(ref Main.tileBrick, nextTile);
		Array.Resize(ref Main.tileMoss, nextTile);
		Array.Resize(ref Main.tileNoAttach, nextTile);
		Array.Resize(ref Main.tileNoFail, nextTile);
		Array.Resize(ref Main.tileObsidianKill, nextTile);
		Array.Resize(ref Main.tileFrameImportant, nextTile);
		Array.Resize(ref Main.tilePile, nextTile);
		Array.Resize(ref Main.tileBlendAll, nextTile);
		Array.Resize(ref Main.tileContainer, nextTile);
		Array.Resize(ref Main.tileSign, nextTile);
		Array.Resize(ref Main.tileSand, nextTile);
		Array.Resize(ref Main.tileFlame, nextTile);
		Array.Resize(ref Main.tileFrame, nextTile);
		Array.Resize(ref Main.tileFrameCounter, nextTile);
		Array.Resize(ref Main.tileMerge, nextTile);
		Array.Resize(ref Main.tileOreFinderPriority, nextTile);
		Array.Resize(ref Main.tileGlowMask, nextTile);
		Array.Resize(ref Main.tileCracked, nextTile);
		Array.Resize(ref WorldGen.tileCounts, nextTile);
		Array.Resize(ref WorldGen.houseTile, nextTile);
		Array.Resize(ref CorruptionPitBiome.ValidTiles, nextTile);
		Array.Resize(ref TileMaterials.MaterialsByTileId, nextTile);
		Array.Resize(ref HouseUtils.BlacklistedTiles, nextTile);
		Array.Resize(ref HouseUtils.BeelistedTiles, nextTile);
		for (int i = 0; i < nextTile; i++)
		{
			Array.Resize(ref Main.tileMerge[i], nextTile);
		}
		for (int j = TileID.Count; j < nextTile; j++)
		{
			Main.tileGlowMask[j] = -1;
			TileMaterials.MaterialsByTileId[j] = TileMaterials._materialsByName["Default"];
		}
		while (TileObjectData._data.Count < nextTile)
		{
			TileObjectData._data.Add(null);
		}
		ModLoader.BuildGlobalHook<GlobalTile, Func<int, int, int, bool, bool>>(ref HookKillSound, globalTiles, (GlobalTile g) => g.KillSound);
		ModLoader.BuildGlobalHook<GlobalTile, DelegateNumDust>(ref HookNumDust, globalTiles, (GlobalTile g) => g.NumDust);
		ModLoader.BuildGlobalHook<GlobalTile, DelegateCreateDust>(ref HookCreateDust, globalTiles, (GlobalTile g) => g.CreateDust);
		ModLoader.BuildGlobalHook<GlobalTile, DelegateDropCritterChance>(ref HookDropCritterChance, globalTiles, (GlobalTile g) => g.DropCritterChance);
		ModLoader.BuildGlobalHook<GlobalTile, Func<int, int, int, bool>>(ref HookCanDrop, globalTiles, (GlobalTile g) => g.CanDrop);
		ModLoader.BuildGlobalHook<GlobalTile, Action<int, int, int>>(ref HookDrop, globalTiles, (GlobalTile g) => g.Drop);
		ModLoader.BuildGlobalHook<GlobalTile, DelegateCanKillTile>(ref HookCanKillTile, globalTiles, (GlobalTile g) => g.CanKillTile);
		ModLoader.BuildGlobalHook<GlobalTile, DelegateKillTile>(ref HookKillTile, globalTiles, (GlobalTile g) => g.KillTile);
		ModLoader.BuildGlobalHook<GlobalTile, Func<int, int, int, bool>>(ref HookCanExplode, globalTiles, (GlobalTile g) => g.CanExplode);
		ModLoader.BuildGlobalHook<GlobalTile, Action<int, int, int, bool>>(ref HookNearbyEffects, globalTiles, (GlobalTile g) => g.NearbyEffects);
		ModLoader.BuildGlobalHook<GlobalTile, DelegateModifyLight>(ref HookModifyLight, globalTiles, (GlobalTile g) => g.ModifyLight);
		ModLoader.BuildGlobalHook<GlobalTile, Func<int, int, int, Player, bool?>>(ref HookIsTileDangerous, globalTiles, (GlobalTile g) => g.IsTileDangerous);
		ModLoader.BuildGlobalHook<GlobalTile, DelegateIsTileBiomeSightable>(ref HookIsTileBiomeSightable, globalTiles, (GlobalTile g) => g.IsTileBiomeSightable);
		ModLoader.BuildGlobalHook<GlobalTile, Func<int, int, int, bool?>>(ref HookIsTileSpelunkable, globalTiles, (GlobalTile g) => g.IsTileSpelunkable);
		ModLoader.BuildGlobalHook<GlobalTile, DelegateSetSpriteEffects>(ref HookSetSpriteEffects, globalTiles, (GlobalTile g) => g.SetSpriteEffects);
		ModLoader.BuildGlobalHook<GlobalTile, Action>(ref HookAnimateTile, globalTiles, (GlobalTile g) => g.AnimateTile);
		ModLoader.BuildGlobalHook<GlobalTile, Func<int, int, int, SpriteBatch, bool>>(ref HookPreDraw, globalTiles, (GlobalTile g) => g.PreDraw);
		ModLoader.BuildGlobalHook<GlobalTile, DelegateDrawEffects>(ref HookDrawEffects, globalTiles, (GlobalTile g) => g.DrawEffects);
		ModLoader.BuildGlobalHook<GlobalTile, Action<int, int, int, SpriteBatch>>(ref HookPostDraw, globalTiles, (GlobalTile g) => g.PostDraw);
		ModLoader.BuildGlobalHook<GlobalTile, Action<int, int, int, SpriteBatch>>(ref HookSpecialDraw, globalTiles, (GlobalTile g) => g.SpecialDraw);
		ModLoader.BuildGlobalHook<GlobalTile, Action<int, int, int>>(ref HookRandomUpdate, globalTiles, (GlobalTile g) => g.RandomUpdate);
		ModLoader.BuildGlobalHook<GlobalTile, DelegateTileFrame>(ref HookTileFrame, globalTiles, (GlobalTile g) => g.TileFrame);
		ModLoader.BuildGlobalHook<GlobalTile, Func<int, int, int, bool>>(ref HookCanPlace, globalTiles, (GlobalTile g) => g.CanPlace);
		ModLoader.BuildGlobalHook<GlobalTile, Func<int, int, int, int, bool>>(ref HookCanReplace, globalTiles, (GlobalTile g) => g.CanReplace);
		ModLoader.BuildGlobalHook<GlobalTile, Func<int, int[]>>(ref HookAdjTiles, globalTiles, (GlobalTile g) => g.AdjTiles);
		ModLoader.BuildGlobalHook<GlobalTile, Action<int, int, int>>(ref HookRightClick, globalTiles, (GlobalTile g) => g.RightClick);
		ModLoader.BuildGlobalHook<GlobalTile, Action<int, int, int>>(ref HookMouseOver, globalTiles, (GlobalTile g) => g.MouseOver);
		ModLoader.BuildGlobalHook<GlobalTile, Action<int, int, int>>(ref HookMouseOverFar, globalTiles, (GlobalTile g) => g.MouseOverFar);
		ModLoader.BuildGlobalHook<GlobalTile, Func<int, int, int, Item, bool>>(ref HookAutoSelect, globalTiles, (GlobalTile g) => g.AutoSelect);
		ModLoader.BuildGlobalHook<GlobalTile, Func<int, int, int, bool>>(ref HookPreHitWire, globalTiles, (GlobalTile g) => g.PreHitWire);
		ModLoader.BuildGlobalHook<GlobalTile, Action<int, int, int>>(ref HookHitWire, globalTiles, (GlobalTile g) => g.HitWire);
		ModLoader.BuildGlobalHook<GlobalTile, Func<int, int, int, bool>>(ref HookSlope, globalTiles, (GlobalTile g) => g.Slope);
		ModLoader.BuildGlobalHook<GlobalTile, Action<int, Player>>(ref HookFloorVisuals, globalTiles, (GlobalTile g) => g.FloorVisuals);
		ModLoader.BuildGlobalHook<GlobalTile, DelegateChangeWaterfallStyle>(ref HookChangeWaterfallStyle, globalTiles, (GlobalTile g) => g.ChangeWaterfallStyle);
		ModLoader.BuildGlobalHook<GlobalTile, Action<int, int, int, Item>>(ref HookPlaceInWorld, globalTiles, (GlobalTile g) => g.PlaceInWorld);
		ModLoader.BuildGlobalHook<GlobalTile, Action>(ref HookPostSetupTileMerge, globalTiles, (GlobalTile g) => g.PostSetupTileMerge);
		if (!unloading)
		{
			loaded = true;
		}
	}

	internal static void PostSetupContent()
	{
		Main.SetupAllBlockMerge();
		PostSetupTileMerge();
	}

	internal static void Unload()
	{
		loaded = false;
		nextTile = TileID.Count;
		tiles.Clear();
		globalTiles.Clear();
		tileTypeAndTileStyleToItemType.Clear();
		Main.QueueMainThreadAction(delegate
		{
			Main.instance.TilePaintSystem.Reset();
		});
		Array.Resize(ref TileID.Sets.RoomNeeds.CountsAsChair, vanillaChairCount);
		Array.Resize(ref TileID.Sets.RoomNeeds.CountsAsTable, vanillaTableCount);
		Array.Resize(ref TileID.Sets.RoomNeeds.CountsAsTorch, vanillaTorchCount);
		Array.Resize(ref TileID.Sets.RoomNeeds.CountsAsDoor, vanillaDoorCount);
		while (TileObjectData._data.Count > TileID.Count)
		{
			TileObjectData._data.RemoveAt(TileObjectData._data.Count - 1);
		}
	}

	public static void CheckModTile(int i, int j, int type)
	{
		if (type <= TileID.Count || WorldGen.destroyObject)
		{
			return;
		}
		TileObjectData tileData = TileObjectData.GetTileData(type, 0);
		if (tileData == null)
		{
			return;
		}
		int frameX = Main.tile[i, j].frameX;
		int frameY = Main.tile[i, j].frameY;
		int subX = frameX / tileData.CoordinateFullWidth;
		int subY = frameY / tileData.CoordinateFullHeight;
		int wrap = tileData.StyleWrapLimit;
		if (wrap == 0)
		{
			wrap = 1;
		}
		int num = (tileData.StyleHorizontal ? (subY * wrap + subX) : (subX * wrap + subY));
		int style = num / tileData.StyleMultiplier;
		int alternate = num % tileData.StyleMultiplier;
		for (int k = 0; k < tileData.AlternatesCount; k++)
		{
			if (alternate >= tileData.Alternates[k].Style && alternate <= tileData.Alternates[k].Style + tileData.RandomStyleRange)
			{
				alternate = k;
				break;
			}
		}
		tileData = TileObjectData.GetTileData(type, style, alternate + 1);
		int partFrameX = frameX % tileData.CoordinateFullWidth;
		int partFrameY = frameY % tileData.CoordinateFullHeight;
		int partX = partFrameX / (tileData.CoordinateWidth + tileData.CoordinatePadding);
		int partY = 0;
		for (int remainingFrameY = partFrameY; partY < tileData.Height && remainingFrameY - tileData.CoordinateHeights[partY] + tileData.CoordinatePadding >= 0; partY++)
		{
			remainingFrameY -= tileData.CoordinateHeights[partY] + tileData.CoordinatePadding;
		}
		int originalI = i;
		int originalJ = j;
		i -= partX;
		j -= partY;
		int originX = i + tileData.Origin.X;
		int originY = j + tileData.Origin.Y;
		bool partiallyDestroyed = false;
		for (int x3 = i; x3 < i + tileData.Width; x3++)
		{
			for (int y3 = j; y3 < j + tileData.Height; y3++)
			{
				if (!Main.tile[x3, y3].active() || Main.tile[x3, y3].type != type)
				{
					partiallyDestroyed = true;
					break;
				}
			}
			if (partiallyDestroyed)
			{
				break;
			}
		}
		if (partiallyDestroyed || !TileObject.CanPlace(originX, originY, type, style, 0, out var _, onlyCheck: true, null, checkStay: true))
		{
			WorldGen.destroyObject = true;
			if (tileData.Width != 1 || tileData.Height != 1)
			{
				WorldGen.KillTile_DropItems(originalI, originalJ, Main.tile[originalI, originalJ], includeLargeObjectDrops: true, includeAllModdedLargeObjectDrops: true);
			}
			for (int x2 = i; x2 < i + tileData.Width; x2++)
			{
				for (int y2 = j; y2 < j + tileData.Height; y2++)
				{
					if (Main.tile[x2, y2].type == type && Main.tile[x2, y2].active())
					{
						WorldGen.KillTile(x2, y2);
					}
				}
			}
			KillMultiTile(i, j, frameX - partFrameX, frameY - partFrameY, type);
			WorldGen.destroyObject = false;
			for (int x = i - 1; x < i + tileData.Width + 2; x++)
			{
				for (int y = j - 1; y < j + tileData.Height + 2; y++)
				{
					WorldGen.TileFrame(x, y);
				}
			}
		}
		TileObject.objectPreview.Active = false;
	}

	public static int OpenDoorID(Tile tile)
	{
		ModTile modTile = GetTile(tile.type);
		if (modTile != null)
		{
			return TileID.Sets.OpenDoorID[modTile.Type];
		}
		if (tile.type == 10 && (tile.frameY < 594 || tile.frameY > 646 || tile.frameX >= 54))
		{
			return 11;
		}
		return -1;
	}

	public static int CloseDoorID(Tile tile)
	{
		ModTile modTile = GetTile(tile.type);
		if (modTile != null)
		{
			return TileID.Sets.CloseDoorID[modTile.Type];
		}
		if (tile.type == 11)
		{
			return 10;
		}
		return -1;
	}

	/// <inheritdoc cref="M:Terraria.ModLoader.TileLoader.IsClosedDoor(System.Int32)" />
	public static bool IsClosedDoor(Tile tile)
	{
		return IsClosedDoor(tile.type);
	}

	/// <summary>
	/// Returns true if the tile is a vanilla or modded closed door.
	/// </summary>
	public static bool IsClosedDoor(int type)
	{
		if (GetTile(type) != null)
		{
			return TileID.Sets.OpenDoorID[type] > -1;
		}
		return type == 10;
	}

	/// <summary> Returns the default name for a modded chest or dresser with the provided FrameX and FrameY values. </summary>
	public static string DefaultContainerName(int type, int frameX, int frameY)
	{
		return GetTile(type)?.DefaultContainerName(frameX, frameY)?.Value ?? string.Empty;
	}

	public static bool IsModMusicBox(Tile tile)
	{
		if (MusicLoader.tileToMusic.ContainsKey(tile.type))
		{
			return MusicLoader.tileToMusic[tile.type].ContainsKey(tile.frameY / 36 * 36);
		}
		return false;
	}

	public static bool HasSmartInteract(int i, int j, int type, SmartInteractScanSettings settings)
	{
		return GetTile(type)?.HasSmartInteract(i, j, settings) ?? false;
	}

	public static void ModifySmartInteractCoords(int type, ref int width, ref int height, ref int frameWidth, ref int frameHeight, ref int extraY)
	{
		ModTile modTile = GetTile(type);
		if (modTile != null)
		{
			TileObjectData data = TileObjectData.GetTileData(type, 0);
			if (data != null)
			{
				width = data.Width;
				height = data.Height;
				frameWidth = data.CoordinateWidth + data.CoordinatePadding;
				frameHeight = data.CoordinateHeights[0] + data.CoordinatePadding;
				extraY = data.CoordinateFullHeight % frameHeight;
				modTile.ModifySmartInteractCoords(ref width, ref height, ref frameWidth, ref frameHeight, ref extraY);
			}
		}
	}

	public static void ModifySittingTargetInfo(int i, int j, int type, ref TileRestingInfo info)
	{
		ModTile modTile = GetTile(type);
		if (modTile != null)
		{
			modTile.ModifySittingTargetInfo(i, j, ref info);
		}
		else
		{
			info.AnchorTilePosition.Y++;
		}
	}

	public static void ModifySleepingTargetInfo(int i, int j, int type, ref TileRestingInfo info)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		ModTile modTile = GetTile(type);
		if (modTile != null)
		{
			info.VisualOffset = new Vector2(-9f, 1f);
			modTile.ModifySleepingTargetInfo(i, j, ref info);
		}
	}

	public static bool KillSound(int i, int j, int type, bool fail)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		Func<int, int, int, bool, bool>[] hookKillSound = HookKillSound;
		for (int k = 0; k < hookKillSound.Length; k++)
		{
			if (!hookKillSound[k](i, j, type, fail))
			{
				return false;
			}
		}
		ModTile modTile = GetTile(type);
		if (modTile != null)
		{
			if (!modTile.KillSound(i, j, fail))
			{
				return false;
			}
			SoundEngine.PlaySound(modTile.HitSound, (Vector2?)new Vector2((float)(i * 16), (float)(j * 16)));
			return false;
		}
		return true;
	}

	public static void NumDust(int i, int j, int type, bool fail, ref int numDust)
	{
		GetTile(type)?.NumDust(i, j, fail, ref numDust);
		DelegateNumDust[] hookNumDust = HookNumDust;
		for (int k = 0; k < hookNumDust.Length; k++)
		{
			hookNumDust[k](i, j, type, fail, ref numDust);
		}
	}

	public static bool CreateDust(int i, int j, int type, ref int dustType)
	{
		DelegateCreateDust[] hookCreateDust = HookCreateDust;
		for (int k = 0; k < hookCreateDust.Length; k++)
		{
			if (!hookCreateDust[k](i, j, type, ref dustType))
			{
				return false;
			}
		}
		return GetTile(type)?.CreateDust(i, j, ref dustType) ?? true;
	}

	public static void DropCritterChance(int i, int j, int type, ref int wormChance, ref int grassHopperChance, ref int jungleGrubChance)
	{
		GetTile(type)?.DropCritterChance(i, j, ref wormChance, ref grassHopperChance, ref jungleGrubChance);
		DelegateDropCritterChance[] hookDropCritterChance = HookDropCritterChance;
		for (int k = 0; k < hookDropCritterChance.Length; k++)
		{
			hookDropCritterChance[k](i, j, type, ref wormChance, ref grassHopperChance, ref jungleGrubChance);
		}
	}

	public static bool Drop(int i, int j, int type, bool includeLargeObjectDrops = true)
	{
		bool isLarge = false;
		if (Main.tileFrameImportant[type])
		{
			TileObjectData tileData = TileObjectData.GetTileData(type, 0);
			if (tileData != null)
			{
				if (tileData.Width != 1 || tileData.Height != 1)
				{
					isLarge = true;
				}
			}
			else if (TileID.Sets.IsMultitile[type])
			{
				isLarge = true;
			}
		}
		if (!includeLargeObjectDrops && isLarge)
		{
			return true;
		}
		_ = Main.tile[i, j];
		bool dropItem = GetTile(type)?.CanDrop(i, j) ?? true;
		Func<int, int, int, bool>[] hookCanDrop = HookCanDrop;
		foreach (Func<int, int, int, bool> hook in hookCanDrop)
		{
			dropItem &= hook(i, j, type);
		}
		if (!dropItem)
		{
			return false;
		}
		Action<int, int, int>[] hookDrop = HookDrop;
		for (int k = 0; k < hookDrop.Length; k++)
		{
			hookDrop[k](i, j, type);
		}
		return true;
	}

	public static void GetItemDrops(int x, int y, Tile tileCache, bool includeLargeObjectDrops = false, bool includeAllModdedLargeObjectDrops = false)
	{
		ModTile modTile = GetTile(tileCache.TileType);
		if (modTile == null)
		{
			return;
		}
		bool needDrops = false;
		TileObjectData tileData = TileObjectData.GetTileData(tileCache.TileType, 0);
		if (tileData == null)
		{
			needDrops = true;
		}
		else if (tileData.Width == 1 && tileData.Height == 1)
		{
			needDrops = !includeAllModdedLargeObjectDrops;
		}
		else if (includeAllModdedLargeObjectDrops)
		{
			needDrops = true;
		}
		else if (includeLargeObjectDrops && (TileID.Sets.BasicChest[tileCache.type] || TileID.Sets.BasicDresser[tileCache.type] || TileID.Sets.Campfire[tileCache.type]))
		{
			needDrops = true;
		}
		if (!needDrops)
		{
			return;
		}
		IEnumerable<Item> itemDrops = modTile.GetItemDrops(x, y);
		if (itemDrops == null)
		{
			return;
		}
		foreach (Item item in itemDrops)
		{
			item.Prefix(-1);
			int num = Item.NewItem(WorldGen.GetItemSource_FromTileBreak(x, y), x * 16, y * 16, 16, 16, item);
			Main.item[num].TryCombiningIntoNearbyItems(num);
		}
	}

	/// <summary>
	/// Retrieves the item type that would drop from a tile of the specified type and style. This method is only reliable for modded tile types. This method can be used in <see cref="M:Terraria.ModLoader.ModTile.GetItemDrops(System.Int32,System.Int32)" /> for tiles that have custom tile style logic. If the specified style is not found, a fallback item will be returned if one has been registered through <see cref="M:Terraria.ModLoader.ModTile.RegisterItemDrop(System.Int32,System.Int32[])" /> usage.<br />
	/// Modders querying modded tile drops should use <see cref="M:Terraria.ModLoader.ModTile.GetItemDrops(System.Int32,System.Int32)" /> directly rather that use this method so that custom drop logic is accounted for.
	/// <br /> A return of 0 indicates that no item would drop from the tile.
	/// </summary>
	/// <param name="type"></param>
	/// <param name="style"></param>
	/// <returns></returns>
	public static int GetItemDropFromTypeAndStyle(int type, int style = 0)
	{
		if (tileTypeAndTileStyleToItemType.TryGetValue((type, style), out var value) || tileTypeAndTileStyleToItemType.TryGetValue((type, -1), out value))
		{
			return value;
		}
		return 0;
	}

	public static bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
	{
		DelegateCanKillTile[] hookCanKillTile = HookCanKillTile;
		for (int k = 0; k < hookCanKillTile.Length; k++)
		{
			if (!hookCanKillTile[k](i, j, type, ref blockDamaged))
			{
				return false;
			}
		}
		return GetTile(type)?.CanKillTile(i, j, ref blockDamaged) ?? true;
	}

	public static void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
	{
		GetTile(type)?.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
		DelegateKillTile[] hookKillTile = HookKillTile;
		for (int k = 0; k < hookKillTile.Length; k++)
		{
			hookKillTile[k](i, j, type, ref fail, ref effectOnly, ref noItem);
		}
	}

	public static void KillMultiTile(int i, int j, int frameX, int frameY, int type)
	{
		GetTile(type)?.KillMultiTile(i, j, frameX, frameY);
	}

	public static bool CanExplode(int i, int j)
	{
		int type = Main.tile[i, j].type;
		ModTile modTile = GetTile(type);
		if (modTile != null && !modTile.CanExplode(i, j))
		{
			return false;
		}
		Func<int, int, int, bool>[] hookCanExplode = HookCanExplode;
		for (int k = 0; k < hookCanExplode.Length; k++)
		{
			if (!hookCanExplode[k](i, j, type))
			{
				return false;
			}
		}
		return true;
	}

	public static void NearbyEffects(int i, int j, int type, bool closer)
	{
		GetTile(type)?.NearbyEffects(i, j, closer);
		Action<int, int, int, bool>[] hookNearbyEffects = HookNearbyEffects;
		for (int k = 0; k < hookNearbyEffects.Length; k++)
		{
			hookNearbyEffects[k](i, j, type, closer);
		}
	}

	public static void ModifyTorchLuck(Player player, ref float positiveLuck, ref float negativeLuck)
	{
		foreach (int item in player.NearbyModTorch)
		{
			float f = GetTile(item).GetTorchLuck(player);
			if (f > 0f)
			{
				positiveLuck += f;
			}
			else
			{
				negativeLuck += 0f - f;
			}
		}
	}

	public static void ModifyLight(int i, int j, int type, ref float r, ref float g, ref float b)
	{
		if (Main.tileLighted[type])
		{
			GetTile(type)?.ModifyLight(i, j, ref r, ref g, ref b);
			DelegateModifyLight[] hookModifyLight = HookModifyLight;
			for (int k = 0; k < hookModifyLight.Length; k++)
			{
				hookModifyLight[k](i, j, type, ref r, ref g, ref b);
			}
		}
	}

	public static bool? IsTileDangerous(int i, int j, int type, Player player)
	{
		bool? retVal = null;
		ModTile modTile = GetTile(type);
		if (modTile != null && modTile.IsTileDangerous(i, j, player))
		{
			retVal = true;
		}
		Func<int, int, int, Player, bool?>[] hookIsTileDangerous = HookIsTileDangerous;
		for (int k = 0; k < hookIsTileDangerous.Length; k++)
		{
			bool? globalRetVal = hookIsTileDangerous[k](i, j, type, player);
			if (globalRetVal.HasValue)
			{
				if (!globalRetVal.Value)
				{
					return false;
				}
				retVal = true;
			}
		}
		return retVal;
	}

	public static bool? IsTileBiomeSightable(int i, int j, int type, ref Color sightColor)
	{
		bool? retVal = null;
		ModTile modTile = GetTile(type);
		if (modTile != null && modTile.IsTileBiomeSightable(i, j, ref sightColor))
		{
			retVal = true;
		}
		DelegateIsTileBiomeSightable[] hookIsTileBiomeSightable = HookIsTileBiomeSightable;
		for (int k = 0; k < hookIsTileBiomeSightable.Length; k++)
		{
			bool? globalRetVal = hookIsTileBiomeSightable[k](i, j, type, ref sightColor);
			if (globalRetVal.HasValue)
			{
				if (!globalRetVal.Value)
				{
					return false;
				}
				retVal = true;
			}
		}
		return retVal;
	}

	public static bool? IsTileSpelunkable(int i, int j, int type)
	{
		bool? retVal = null;
		ModTile modTile = GetTile(type);
		if (!Main.tileSpelunker[type] && modTile != null && modTile.IsTileSpelunkable(i, j))
		{
			retVal = true;
		}
		Func<int, int, int, bool?>[] hookIsTileSpelunkable = HookIsTileSpelunkable;
		for (int k = 0; k < hookIsTileSpelunkable.Length; k++)
		{
			bool? globalRetVal = hookIsTileSpelunkable[k](i, j, type);
			if (globalRetVal.HasValue)
			{
				if (!globalRetVal.Value)
				{
					return false;
				}
				retVal = true;
			}
		}
		return retVal;
	}

	public static void SetSpriteEffects(int i, int j, int type, ref SpriteEffects spriteEffects)
	{
		GetTile(type)?.SetSpriteEffects(i, j, ref spriteEffects);
		DelegateSetSpriteEffects[] hookSetSpriteEffects = HookSetSpriteEffects;
		for (int k = 0; k < hookSetSpriteEffects.Length; k++)
		{
			hookSetSpriteEffects[k](i, j, type, ref spriteEffects);
		}
	}

	public static void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
	{
		Tile tile = Main.tile[i, j];
		if (tile.type < TileID.Count)
		{
			return;
		}
		TileObjectData tileData = TileObjectData.GetTileData(tile.type, 0);
		if (tileData != null)
		{
			int partY = 0;
			for (int remainingFrameY = tile.frameY % tileData.CoordinateFullHeight; partY < tileData.Height && remainingFrameY - tileData.CoordinateHeights[partY] + tileData.CoordinatePadding >= 0; partY++)
			{
				remainingFrameY -= tileData.CoordinateHeights[partY] + tileData.CoordinatePadding;
			}
			width = tileData.CoordinateWidth;
			offsetY = tileData.DrawYOffset;
			height = tileData.CoordinateHeights[partY];
		}
		GetTile(tile.type).SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref tileFrameX, ref tileFrameY);
	}

	public static void AnimateTiles()
	{
		if (loaded)
		{
			for (int i = 0; i < tiles.Count; i++)
			{
				ModTile modTile = tiles[i];
				modTile.AnimateTile(ref Main.tileFrame[modTile.Type], ref Main.tileFrameCounter[modTile.Type]);
			}
			Action[] hookAnimateTile = HookAnimateTile;
			for (int j = 0; j < hookAnimateTile.Length; j++)
			{
				hookAnimateTile[j]();
			}
		}
	}

	/// <summary>
	/// Sets the animation frame. Sets frameYOffset = modTile.animationFrameHeight * Main.tileFrame[type]; and then calls ModTile.AnimateIndividualTile
	/// </summary>
	/// <param name="type">The tile type.</param>
	/// <param name="i">The x position in tile coordinates.</param>
	/// <param name="j">The y position in tile coordinates.</param>
	/// <param name="frameXOffset">The offset to frameX.</param>
	/// <param name="frameYOffset">The offset to frameY.</param>
	public static void SetAnimationFrame(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
	{
		ModTile modTile = GetTile(type);
		if (modTile != null)
		{
			frameYOffset = modTile.AnimationFrameHeight * Main.tileFrame[type];
			modTile.AnimateIndividualTile(type, i, j, ref frameXOffset, ref frameYOffset);
		}
	}

	public static bool PreDraw(int i, int j, int type, SpriteBatch spriteBatch)
	{
		Func<int, int, int, SpriteBatch, bool>[] hookPreDraw = HookPreDraw;
		for (int k = 0; k < hookPreDraw.Length; k++)
		{
			if (!hookPreDraw[k](i, j, type, spriteBatch))
			{
				return false;
			}
		}
		return GetTile(type)?.PreDraw(i, j, spriteBatch) ?? true;
	}

	public static void DrawEffects(int i, int j, int type, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
	{
		GetTile(type)?.DrawEffects(i, j, spriteBatch, ref drawData);
		DelegateDrawEffects[] hookDrawEffects = HookDrawEffects;
		for (int k = 0; k < hookDrawEffects.Length; k++)
		{
			hookDrawEffects[k](i, j, type, spriteBatch, ref drawData);
		}
	}

	public static void PostDraw(int i, int j, int type, SpriteBatch spriteBatch)
	{
		GetTile(type)?.PostDraw(i, j, spriteBatch);
		Action<int, int, int, SpriteBatch>[] hookPostDraw = HookPostDraw;
		for (int k = 0; k < hookPostDraw.Length; k++)
		{
			hookPostDraw[k](i, j, type, spriteBatch);
		}
	}

	/// <summary>
	/// Special Draw calls ModTile and GlobalTile SpecialDraw methods. Special Draw is called at the end of the DrawSpecialTilesLegacy loop, allowing for basically another layer above tiles. Use DrawEffects hook to queue for SpecialDraw.
	/// </summary>
	public static void SpecialDraw(int type, int specialTileX, int specialTileY, SpriteBatch spriteBatch)
	{
		GetTile(type)?.SpecialDraw(specialTileX, specialTileY, spriteBatch);
		Action<int, int, int, SpriteBatch>[] hookSpecialDraw = HookSpecialDraw;
		for (int i = 0; i < hookSpecialDraw.Length; i++)
		{
			hookSpecialDraw[i](specialTileX, specialTileY, type, spriteBatch);
		}
	}

	public static void RandomUpdate(int i, int j, int type)
	{
		if (Main.tile[i, j].active())
		{
			GetTile(type)?.RandomUpdate(i, j);
			Action<int, int, int>[] hookRandomUpdate = HookRandomUpdate;
			for (int k = 0; k < hookRandomUpdate.Length; k++)
			{
				hookRandomUpdate[k](i, j, type);
			}
		}
	}

	public static bool TileFrame(int i, int j, int type, ref bool resetFrame, ref bool noBreak)
	{
		ModTile modTile = GetTile(type);
		bool flag = true;
		if (modTile != null)
		{
			flag = modTile.TileFrame(i, j, ref resetFrame, ref noBreak);
		}
		DelegateTileFrame[] hookTileFrame = HookTileFrame;
		foreach (DelegateTileFrame hook in hookTileFrame)
		{
			flag &= hook(i, j, type, ref resetFrame, ref noBreak);
		}
		return flag;
	}

	public static void PickPowerCheck(Tile target, int pickPower, ref int damage)
	{
		ModTile modTile = GetTile(target.type);
		if (modTile != null && pickPower < modTile.MinPick)
		{
			damage = 0;
		}
	}

	public static bool CanPlace(int i, int j, int type)
	{
		Func<int, int, int, bool>[] hookCanPlace = HookCanPlace;
		for (int k = 0; k < hookCanPlace.Length; k++)
		{
			if (!hookCanPlace[k](i, j, type))
			{
				return false;
			}
		}
		return GetTile(type)?.CanPlace(i, j) ?? true;
	}

	public static bool CanReplace(int i, int j, int type, int tileTypeBeingPlaced)
	{
		Func<int, int, int, int, bool>[] hookCanReplace = HookCanReplace;
		for (int k = 0; k < hookCanReplace.Length; k++)
		{
			if (!hookCanReplace[k](i, j, type, tileTypeBeingPlaced))
			{
				return false;
			}
		}
		return GetTile(type)?.CanReplace(i, j, tileTypeBeingPlaced) ?? true;
	}

	public static void AdjTiles(Player player, int type)
	{
		ModTile modTile = GetTile(type);
		if (modTile != null)
		{
			int[] adjTiles = modTile.AdjTiles;
			foreach (int j in adjTiles)
			{
				player.adjTile[j] = true;
			}
		}
		Func<int, int[]>[] hookAdjTiles = HookAdjTiles;
		for (int k = 0; k < hookAdjTiles.Length; k++)
		{
			int[] adjTiles = hookAdjTiles[k](type);
			foreach (int i in adjTiles)
			{
				player.adjTile[i] = true;
			}
		}
	}

	public static bool RightClick(int i, int j)
	{
		bool returnValue = false;
		int type = Main.tile[i, j].type;
		ModTile tile = GetTile(type);
		if (tile != null && tile.RightClick(i, j))
		{
			returnValue = true;
		}
		Action<int, int, int>[] hookRightClick = HookRightClick;
		for (int k = 0; k < hookRightClick.Length; k++)
		{
			hookRightClick[k](i, j, type);
		}
		return returnValue;
	}

	public static void MouseOver(int i, int j)
	{
		int type = Main.tile[i, j].type;
		GetTile(type)?.MouseOver(i, j);
		Action<int, int, int>[] hookMouseOver = HookMouseOver;
		for (int k = 0; k < hookMouseOver.Length; k++)
		{
			hookMouseOver[k](i, j, type);
		}
	}

	public static void MouseOverFar(int i, int j)
	{
		int type = Main.tile[i, j].type;
		GetTile(type)?.MouseOverFar(i, j);
		Action<int, int, int>[] hookMouseOverFar = HookMouseOverFar;
		for (int k = 0; k < hookMouseOverFar.Length; k++)
		{
			hookMouseOverFar[k](i, j, type);
		}
	}

	public static int AutoSelect(int i, int j, Player player)
	{
		if (!Main.tile[i, j].active())
		{
			return -1;
		}
		int type = Main.tile[i, j].type;
		ModTile modTile = GetTile(type);
		for (int k = 0; k < 50; k++)
		{
			Item item = player.inventory[k];
			if (item.type == 0 || item.stack == 0)
			{
				continue;
			}
			if (modTile != null && modTile.AutoSelect(i, j, item))
			{
				return k;
			}
			Func<int, int, int, Item, bool>[] hookAutoSelect = HookAutoSelect;
			for (int l = 0; l < hookAutoSelect.Length; l++)
			{
				if (hookAutoSelect[l](i, j, type, item))
				{
					return k;
				}
			}
		}
		return -1;
	}

	public static bool PreHitWire(int i, int j, int type)
	{
		Func<int, int, int, bool>[] hookPreHitWire = HookPreHitWire;
		for (int k = 0; k < hookPreHitWire.Length; k++)
		{
			if (!hookPreHitWire[k](i, j, type))
			{
				return false;
			}
		}
		return true;
	}

	public static void HitWire(int i, int j, int type)
	{
		GetTile(type)?.HitWire(i, j);
		Action<int, int, int>[] hookHitWire = HookHitWire;
		for (int k = 0; k < hookHitWire.Length; k++)
		{
			hookHitWire[k](i, j, type);
		}
	}

	public static void FloorVisuals(int type, Player player)
	{
		GetTile(type)?.FloorVisuals(player);
		Action<int, Player>[] hookFloorVisuals = HookFloorVisuals;
		for (int i = 0; i < hookFloorVisuals.Length; i++)
		{
			hookFloorVisuals[i](type, player);
		}
	}

	public static bool Slope(int i, int j, int type)
	{
		Func<int, int, int, bool>[] hookSlope = HookSlope;
		for (int k = 0; k < hookSlope.Length; k++)
		{
			if (!hookSlope[k](i, j, type))
			{
				return true;
			}
		}
		ModTile tile = GetTile(type);
		if (tile == null)
		{
			return false;
		}
		return !tile.Slope(i, j);
	}

	public static bool HasWalkDust(int type)
	{
		return GetTile(type)?.HasWalkDust() ?? false;
	}

	public static void WalkDust(int type, ref int dustType, ref bool makeDust, ref Color color)
	{
		GetTile(type)?.WalkDust(ref dustType, ref makeDust, ref color);
	}

	public static void ChangeWaterfallStyle(int type, ref int style)
	{
		GetTile(type)?.ChangeWaterfallStyle(ref style);
		DelegateChangeWaterfallStyle[] hookChangeWaterfallStyle = HookChangeWaterfallStyle;
		for (int i = 0; i < hookChangeWaterfallStyle.Length; i++)
		{
			hookChangeWaterfallStyle[i](type, ref style);
		}
	}

	public static bool SaplingGrowthType(int soilType, ref int saplingType, ref int style)
	{
		int originalType = saplingType;
		int originalStyle = style;
		ModTree treeGrown = PlantLoader.Get<ModTree>(5, soilType);
		if (treeGrown == null)
		{
			ModPalmTree palmGrown = PlantLoader.Get<ModPalmTree>(323, soilType);
			if (palmGrown == null)
			{
				return false;
			}
			saplingType = palmGrown.SaplingGrowthType(ref style);
		}
		else
		{
			saplingType = treeGrown.SaplingGrowthType(ref style);
		}
		if (TileID.Sets.TreeSapling[saplingType])
		{
			return true;
		}
		saplingType = originalType;
		style = originalStyle;
		return false;
	}

	public static bool CanGrowModTree(int type)
	{
		return PlantLoader.Exists(5, type);
	}

	public static void TreeDust(Tile tile, ref int dust)
	{
		if (tile.active())
		{
			ModTree tree = PlantLoader.Get<ModTree>(5, tile.type);
			if (tree != null)
			{
				dust = tree.CreateDust();
			}
		}
	}

	public static bool CanDropAcorn(int type)
	{
		return PlantLoader.Get<ModTree>(5, type)?.CanDropAcorn() ?? false;
	}

	public static void DropTreeWood(int type, ref int wood)
	{
		ModTree tree = PlantLoader.Get<ModTree>(5, type);
		if (tree != null)
		{
			wood = tree.DropWood();
		}
	}

	public static bool CanGrowModPalmTree(int type)
	{
		return PlantLoader.Exists(323, type);
	}

	public static void PalmTreeDust(Tile tile, ref int dust)
	{
		if (tile.active())
		{
			ModPalmTree tree = PlantLoader.Get<ModPalmTree>(323, tile.type);
			if (tree != null)
			{
				dust = tree.CreateDust();
			}
		}
	}

	public static void DropPalmTreeWood(int type, ref int wood)
	{
		ModPalmTree tree = PlantLoader.Get<ModPalmTree>(323, type);
		if (tree != null)
		{
			wood = tree.DropWood();
		}
	}

	public static bool CanGrowModCactus(int type)
	{
		if (!PlantLoader.Exists(80, type))
		{
			return TileIO.Tiles.unloadedTypes.Contains((ushort)type);
		}
		return true;
	}

	public static Texture2D GetCactusTexture(int type)
	{
		return PlantLoader.Get<ModCactus>(80, type)?.GetTexture().Value;
	}

	public static void PlaceInWorld(int i, int j, Item item)
	{
		int type = item.createTile;
		if (type >= 0)
		{
			Action<int, int, int, Item>[] hookPlaceInWorld = HookPlaceInWorld;
			for (int k = 0; k < hookPlaceInWorld.Length; k++)
			{
				hookPlaceInWorld[k](i, j, type, item);
			}
			GetTile(type)?.PlaceInWorld(i, j, item);
		}
	}

	public static void PostSetupTileMerge()
	{
		Action[] hookPostSetupTileMerge = HookPostSetupTileMerge;
		for (int i = 0; i < hookPostSetupTileMerge.Length; i++)
		{
			hookPostSetupTileMerge[i]();
		}
		foreach (ModTile tile in tiles)
		{
			tile.PostSetupTileMerge();
		}
	}

	public static bool IsLockedChest(int i, int j, int type)
	{
		return GetTile(type)?.IsLockedChest(i, j) ?? false;
	}

	public static bool UnlockChest(int i, int j, int type, ref short frameXAdjustment, ref int dustType, ref bool manual)
	{
		return GetTile(type)?.UnlockChest(i, j, ref frameXAdjustment, ref dustType, ref manual) ?? false;
	}

	public static bool LockChest(int i, int j, int type, ref short frameXAdjustment, ref bool manual)
	{
		return GetTile(type)?.LockChest(i, j, ref frameXAdjustment, ref manual) ?? false;
	}

	public static void RecountTiles(SceneMetrics metrics)
	{
		int num2 = (metrics.DungeonTileCount = 0);
		int num4 = (metrics.SandTileCount = num2);
		int num6 = (metrics.MushroomTileCount = num4);
		int num8 = (metrics.JungleTileCount = num6);
		int num10 = (metrics.SnowTileCount = num8);
		int num12 = (metrics.BloodTileCount = num10);
		int holyTileCount = (metrics.EvilTileCount = num12);
		metrics.HolyTileCount = holyTileCount;
		for (int i = 0; i < TileCount; i++)
		{
			int tileCount = metrics._tileCounts[i];
			if (tileCount != 0)
			{
				metrics.HolyTileCount += tileCount * TileID.Sets.HallowBiome[i];
				metrics.SnowTileCount += tileCount * TileID.Sets.SnowBiome[i];
				metrics.MushroomTileCount += tileCount * TileID.Sets.MushroomBiome[i];
				metrics.SandTileCount += tileCount * TileID.Sets.SandBiome[i];
				metrics.DungeonTileCount += tileCount * TileID.Sets.DungeonBiome[i];
				int jungle = 0;
				int corrupt;
				int crimson;
				if (!Main.remixWorld)
				{
					corrupt = TileID.Sets.CorruptBiome[i];
					crimson = TileID.Sets.CrimsonBiome[i];
					jungle = TileID.Sets.JungleBiome[i];
				}
				else
				{
					corrupt = TileID.Sets.RemixCorruptBiome[i];
					crimson = TileID.Sets.RemixCrimsonBiome[i];
					jungle = TileID.Sets.RemixJungleBiome[i];
				}
				metrics.EvilTileCount += tileCount * corrupt;
				metrics.BloodTileCount += tileCount * crimson;
				metrics.JungleTileCount += tileCount * jungle;
			}
		}
	}

	internal static void FinishSetup()
	{
		for (int i = 0; i < ItemLoader.ItemCount; i++)
		{
			Item item = ContentSamples.ItemsByType[i];
			if (!ItemID.Sets.DisableAutomaticPlaceableDrop[i] && item.createTile > -1)
			{
				tileTypeAndTileStyleToItemType.TryAdd((item.createTile, item.placeStyle), item.type);
			}
		}
	}
}
