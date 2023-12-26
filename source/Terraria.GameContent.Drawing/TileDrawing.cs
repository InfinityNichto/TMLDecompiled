using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.GameContent.Liquid;
using Terraria.GameContent.Tile_Entities;
using Terraria.Graphics;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.UI;
using Terraria.Utilities;

namespace Terraria.GameContent.Drawing;

public class TileDrawing
{
	private enum TileCounterType
	{
		Tree,
		DisplayDoll,
		HatRack,
		WindyGrass,
		MultiTileGrass,
		MultiTileVine,
		Vine,
		BiomeGrass,
		VoidLens,
		ReverseVine,
		TeleportationPylon,
		MasterTrophy,
		AnyDirectionalGrass,
		Count
	}

	private struct TileFlameData
	{
		public Texture2D flameTexture;

		public ulong flameSeed;

		public int flameCount;

		public Color flameColor;

		public int flameRangeXMin;

		public int flameRangeXMax;

		public int flameRangeYMin;

		public int flameRangeYMax;

		public float flameRangeMultX;

		public float flameRangeMultY;
	}

	private const int MAX_SPECIALS = 9000;

	private const int MAX_SPECIALS_LEGACY = 1000;

	private const float FORCE_FOR_MIN_WIND = 0.08f;

	private const float FORCE_FOR_MAX_WIND = 1.2f;

	private int _leafFrequency = 100000;

	private int[] _specialsCount = new int[13];

	private Point[][] _specialPositions = new Point[13][];

	private Dictionary<Point, int> _displayDollTileEntityPositions = new Dictionary<Point, int>();

	private Dictionary<Point, int> _hatRackTileEntityPositions = new Dictionary<Point, int>();

	private Dictionary<Point, int> _trainingDummyTileEntityPositions = new Dictionary<Point, int>();

	private Dictionary<Point, int> _itemFrameTileEntityPositions = new Dictionary<Point, int>();

	private Dictionary<Point, int> _foodPlatterTileEntityPositions = new Dictionary<Point, int>();

	private Dictionary<Point, int> _weaponRackTileEntityPositions = new Dictionary<Point, int>();

	private Dictionary<Point, int> _chestPositions = new Dictionary<Point, int>();

	private int _specialTilesCount;

	private int[] _specialTileX = new int[1000];

	private int[] _specialTileY = new int[1000];

	private UnifiedRandom _rand;

	private double _treeWindCounter;

	private double _grassWindCounter;

	private double _sunflowerWindCounter;

	private double _vineWindCounter;

	private WindGrid _windGrid = new WindGrid();

	private bool _shouldShowInvisibleBlocks;

	private bool _shouldShowInvisibleBlocks_LastFrame;

	private List<Point> _vineRootsPositions = new List<Point>();

	private List<Point> _reverseVineRootsPositions = new List<Point>();

	private TilePaintSystemV2 _paintSystem;

	private Color _martianGlow = new Color(0, 0, 0, 0);

	private Color _meteorGlow = new Color(100, 100, 100, 0);

	private Color _lavaMossGlow = new Color(150, 100, 50, 0);

	private Color _kryptonMossGlow = new Color(0, 200, 0, 0);

	private Color _xenonMossGlow = new Color(0, 180, 250, 0);

	private Color _argonMossGlow = new Color(225, 0, 125, 0);

	private Color _violetMossGlow = new Color(150, 0, 250, 0);

	private bool _isActiveAndNotPaused;

	private Player _localPlayer = new Player();

	private Color _highQualityLightingRequirement;

	private Color _mediumQualityLightingRequirement;

	private static readonly Vector2 _zero;

	private ThreadLocal<TileDrawInfo> _currentTileDrawInfo = new ThreadLocal<TileDrawInfo>(() => new TileDrawInfo());

	private TileDrawInfo _currentTileDrawInfoNonThreaded = new TileDrawInfo();

	private Vector3[] _glowPaintColorSlices = (Vector3[])(object)new Vector3[9]
	{
		Vector3.One,
		Vector3.One,
		Vector3.One,
		Vector3.One,
		Vector3.One,
		Vector3.One,
		Vector3.One,
		Vector3.One,
		Vector3.One
	};

	private List<DrawData> _voidLensData = new List<DrawData>();

	private bool[] _tileSolid => Main.tileSolid;

	private bool[] _tileSolidTop => Main.tileSolidTop;

	private Dust[] _dust => Main.dust;

	private Gore[] _gore => Main.gore;

	/// <summary>
	/// The wind grid used to exert wind effects on tiles.
	/// </summary>
	public WindGrid Wind => _windGrid;

	private void AddSpecialPoint(int x, int y, TileCounterType type)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		_specialPositions[(int)type][_specialsCount[(int)type]++] = new Point(x, y);
	}

	public TileDrawing(TilePaintSystemV2 paintSystem)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		_paintSystem = paintSystem;
		_rand = new UnifiedRandom();
		for (int i = 0; i < _specialPositions.Length; i++)
		{
			_specialPositions[i] = (Point[])(object)new Point[9000];
		}
	}

	public void PreparePaintForTilesOnScreen()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		if (Main.GameUpdateCount % 6 == 0)
		{
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 vector = default(Vector2);
			((Vector2)(ref vector))._002Ector((float)Main.offScreenRange, (float)Main.offScreenRange);
			if (Main.drawToScreen)
			{
				vector = Vector2.Zero;
			}
			GetScreenDrawArea(unscaledPosition, vector + (Main.Camera.UnscaledPosition - Main.Camera.ScaledPosition), out var firstTileX, out var lastTileX, out var firstTileY, out var lastTileY);
			PrepareForAreaDrawing(firstTileX, lastTileX, firstTileY, lastTileY, prepareLazily: true);
		}
	}

	public void PrepareForAreaDrawing(int firstTileX, int lastTileX, int firstTileY, int lastTileY, bool prepareLazily)
	{
		TilePaintSystemV2.TileVariationkey lookupKey = default(TilePaintSystemV2.TileVariationkey);
		TilePaintSystemV2.WallVariationKey lookupKey2 = default(TilePaintSystemV2.WallVariationKey);
		for (int i = firstTileY; i < lastTileY + 4; i++)
		{
			for (int j = firstTileX - 2; j < lastTileX + 2; j++)
			{
				Tile tile = Main.tile[j, i];
				if (tile == null)
				{
					continue;
				}
				int tileStyle = 0;
				if (tile.active())
				{
					Main.instance.LoadTiles(tile.type);
					lookupKey.TileType = tile.type;
					lookupKey.PaintColor = tile.color();
					switch (tile.type)
					{
					case 5:
						tileStyle = GetTreeBiome(j, i, tile.frameX, tile.frameY);
						break;
					case 323:
						tileStyle = GetPalmTreeBiome(j, i);
						break;
					}
					lookupKey.TileStyle = tileStyle;
					if (lookupKey.PaintColor != 0)
					{
						_paintSystem.RequestTile(ref lookupKey);
					}
				}
				if (tile.wall != 0)
				{
					Main.instance.LoadWall(tile.wall);
					lookupKey2.WallType = tile.wall;
					lookupKey2.PaintColor = tile.wallColor();
					if (lookupKey2.PaintColor != 0)
					{
						_paintSystem.RequestWall(ref lookupKey2);
					}
				}
				if (!prepareLazily)
				{
					MakeExtraPreparations(tile, j, i, tileStyle);
				}
			}
		}
	}

	private void MakeExtraPreparations(Tile tile, int x, int y, int tileStyle)
	{
		switch (tile.type)
		{
		case 5:
		{
			int treeFrame2 = 0;
			int floorY2 = 0;
			int topTextureFrameWidth2 = 0;
			int topTextureFrameHeight2 = 0;
			int treeStyle2 = 0;
			int xoffset2 = (tile.frameX == 44).ToInt() - (tile.frameX == 66).ToInt();
			if (WorldGen.GetCommonTreeFoliageData(x, y, xoffset2, ref treeFrame2, ref treeStyle2, out floorY2, out topTextureFrameWidth2, out topTextureFrameHeight2))
			{
				TilePaintSystemV2.TreeFoliageVariantKey treeFoliageVariantKey3 = default(TilePaintSystemV2.TreeFoliageVariantKey);
				treeFoliageVariantKey3.TextureIndex = treeStyle2;
				treeFoliageVariantKey3.PaintColor = tile.color();
				TilePaintSystemV2.TreeFoliageVariantKey lookupKey3 = treeFoliageVariantKey3;
				_paintSystem.RequestTreeTop(ref lookupKey3);
				_paintSystem.RequestTreeBranch(ref lookupKey3);
			}
			break;
		}
		case 583:
		case 584:
		case 585:
		case 586:
		case 587:
		case 588:
		case 589:
		{
			int treeFrame3 = 0;
			int floorY3 = 0;
			int topTextureFrameWidth3 = 0;
			int topTextureFrameHeight3 = 0;
			int treeStyle3 = 0;
			int xoffset3 = (tile.frameX == 44).ToInt() - (tile.frameX == 66).ToInt();
			if (WorldGen.GetGemTreeFoliageData(x, y, xoffset3, ref treeFrame3, ref treeStyle3, out floorY3, out topTextureFrameWidth3, out topTextureFrameHeight3))
			{
				TilePaintSystemV2.TreeFoliageVariantKey treeFoliageVariantKey = default(TilePaintSystemV2.TreeFoliageVariantKey);
				treeFoliageVariantKey.TextureIndex = treeStyle3;
				treeFoliageVariantKey.PaintColor = tile.color();
				TilePaintSystemV2.TreeFoliageVariantKey lookupKey4 = treeFoliageVariantKey;
				_paintSystem.RequestTreeTop(ref lookupKey4);
				_paintSystem.RequestTreeBranch(ref lookupKey4);
			}
			break;
		}
		case 596:
		case 616:
		{
			int treeFrame = 0;
			int floorY = 0;
			int topTextureFrameWidth = 0;
			int topTextureFrameHeight = 0;
			int treeStyle = 0;
			int xoffset = (tile.frameX == 44).ToInt() - (tile.frameX == 66).ToInt();
			if (WorldGen.GetVanityTreeFoliageData(x, y, xoffset, ref treeFrame, ref treeStyle, out floorY, out topTextureFrameWidth, out topTextureFrameHeight))
			{
				TilePaintSystemV2.TreeFoliageVariantKey treeFoliageVariantKey5 = default(TilePaintSystemV2.TreeFoliageVariantKey);
				treeFoliageVariantKey5.TextureIndex = treeStyle;
				treeFoliageVariantKey5.PaintColor = tile.color();
				TilePaintSystemV2.TreeFoliageVariantKey lookupKey2 = treeFoliageVariantKey5;
				_paintSystem.RequestTreeTop(ref lookupKey2);
				_paintSystem.RequestTreeBranch(ref lookupKey2);
			}
			break;
		}
		case 634:
		{
			int treeFrame4 = 0;
			int floorY4 = 0;
			int topTextureFrameWidth4 = 0;
			int topTextureFrameHeight4 = 0;
			int treeStyle4 = 0;
			int xoffset4 = (tile.frameX == 44).ToInt() - (tile.frameX == 66).ToInt();
			if (WorldGen.GetAshTreeFoliageData(x, y, xoffset4, ref treeFrame4, ref treeStyle4, out floorY4, out topTextureFrameWidth4, out topTextureFrameHeight4))
			{
				TilePaintSystemV2.TreeFoliageVariantKey treeFoliageVariantKey4 = default(TilePaintSystemV2.TreeFoliageVariantKey);
				treeFoliageVariantKey4.TextureIndex = treeStyle4;
				treeFoliageVariantKey4.PaintColor = tile.color();
				TilePaintSystemV2.TreeFoliageVariantKey lookupKey5 = treeFoliageVariantKey4;
				_paintSystem.RequestTreeTop(ref lookupKey5);
				_paintSystem.RequestTreeBranch(ref lookupKey5);
			}
			break;
		}
		case 323:
		{
			int textureIndex = 15;
			bool isOcean = false;
			if (x >= WorldGen.beachDistance && x <= Main.maxTilesX - WorldGen.beachDistance)
			{
				isOcean = true;
			}
			if (isOcean)
			{
				textureIndex = 21;
			}
			if (Math.Abs(tileStyle) >= 8)
			{
				textureIndex = Math.Abs(tileStyle) - 8;
				textureIndex *= -2;
				if (!isOcean)
				{
					textureIndex--;
				}
			}
			TilePaintSystemV2.TreeFoliageVariantKey treeFoliageVariantKey2 = default(TilePaintSystemV2.TreeFoliageVariantKey);
			treeFoliageVariantKey2.TextureIndex = textureIndex;
			treeFoliageVariantKey2.PaintColor = tile.color();
			TilePaintSystemV2.TreeFoliageVariantKey lookupKey = treeFoliageVariantKey2;
			_paintSystem.RequestTreeTop(ref lookupKey);
			_paintSystem.RequestTreeBranch(ref lookupKey);
			break;
		}
		}
	}

	public void Update()
	{
		if (!Main.dedServ)
		{
			double num = Math.Abs(Main.WindForVisuals);
			num = Utils.GetLerpValue(0.08f, 1.2f, (float)num, clamped: true);
			_treeWindCounter += 1.0 / 240.0 + 1.0 / 240.0 * num * 2.0;
			_grassWindCounter += 1.0 / 180.0 + 1.0 / 180.0 * num * 4.0;
			_sunflowerWindCounter += 1.0 / 420.0 + 1.0 / 420.0 * num * 5.0;
			_vineWindCounter += 1.0 / 120.0 + 1.0 / 120.0 * num * 0.4000000059604645;
			UpdateLeafFrequency();
			EnsureWindGridSize();
			_windGrid.Update();
			_shouldShowInvisibleBlocks = Main.ShouldShowInvisibleWalls();
			if (_shouldShowInvisibleBlocks_LastFrame != _shouldShowInvisibleBlocks)
			{
				_shouldShowInvisibleBlocks_LastFrame = _shouldShowInvisibleBlocks;
				Main.sectionManager.SetAllFramedSectionsAsNeedingRefresh();
			}
		}
	}

	public void SpecificHacksForCapture()
	{
		Main.sectionManager.SetAllFramedSectionsAsNeedingRefresh();
	}

	public void PreDrawTiles(bool solidLayer, bool forRenderTargets, bool intoRenderTargets)
	{
		bool flag = intoRenderTargets || Lighting.UpdateEveryFrame;
		if (!solidLayer && flag)
		{
			_specialsCount[5] = 0;
			_specialsCount[4] = 0;
			_specialsCount[8] = 0;
			_specialsCount[6] = 0;
			_specialsCount[3] = 0;
			_specialsCount[12] = 0;
			_specialsCount[0] = 0;
			_specialsCount[9] = 0;
			_specialsCount[10] = 0;
			_specialsCount[11] = 0;
		}
	}

	public void PostDrawTiles(bool solidLayer, bool forRenderTargets, bool intoRenderTargets)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		if (!solidLayer && !intoRenderTargets)
		{
			Main.spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, (Effect)null, Main.Transform);
			DrawMultiTileVines();
			DrawMultiTileGrass();
			DrawVoidLenses();
			DrawTeleportationPylons();
			DrawMasterTrophies();
			DrawGrass();
			DrawAnyDirectionalGrass();
			DrawTrees();
			DrawVines();
			DrawReverseVines();
			Main.spriteBatch.End();
		}
		if (solidLayer && !intoRenderTargets)
		{
			DrawEntities_HatRacks();
			DrawEntities_DisplayDolls();
		}
	}

	public void DrawLiquidBehindTiles(int waterStyleOverride = -1)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)Main.offScreenRange, (float)Main.offScreenRange);
		if (Main.drawToScreen)
		{
			vector = Vector2.Zero;
		}
		GetScreenDrawArea(unscaledPosition, vector + (Main.Camera.UnscaledPosition - Main.Camera.ScaledPosition), out var firstTileX, out var lastTileX, out var firstTileY, out var lastTileY);
		for (int i = firstTileY; i < lastTileY + 4; i++)
		{
			for (int j = firstTileX - 2; j < lastTileX + 2; j++)
			{
				Tile tile = Main.tile[j, i];
				if (tile != null)
				{
					DrawTile_LiquidBehindTile(solidLayer: false, inFrontOfPlayers: false, waterStyleOverride, unscaledPosition, vector, j, i, tile);
				}
			}
		}
	}

	public void Draw(bool solidLayer, bool forRenderTargets, bool intoRenderTargets, int waterStyleOverride = -1)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Unknown result type (might be due to invalid IL or missing references)
		//IL_0946: Unknown result type (might be due to invalid IL or missing references)
		//IL_0947: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_087d: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Unknown result type (might be due to invalid IL or missing references)
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		_isActiveAndNotPaused = !Main.gamePaused && ((Game)Main.instance).IsActive;
		_localPlayer = Main.LocalPlayer;
		Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)Main.offScreenRange, (float)Main.offScreenRange);
		if (Main.drawToScreen)
		{
			vector = Vector2.Zero;
		}
		if (!solidLayer)
		{
			Main.critterCage = false;
		}
		EnsureWindGridSize();
		ClearLegacyCachedDraws();
		bool flag = intoRenderTargets || Main.LightingEveryFrame;
		if (flag)
		{
			ClearCachedTileDraws(solidLayer);
		}
		float num = 255f * (1f - Main.gfxQuality) + 30f * Main.gfxQuality;
		((Color)(ref _highQualityLightingRequirement)).R = (byte)num;
		((Color)(ref _highQualityLightingRequirement)).G = (byte)((double)num * 1.1);
		((Color)(ref _highQualityLightingRequirement)).B = (byte)((double)num * 1.2);
		float num2 = 50f * (1f - Main.gfxQuality) + 2f * Main.gfxQuality;
		((Color)(ref _mediumQualityLightingRequirement)).R = (byte)num2;
		((Color)(ref _mediumQualityLightingRequirement)).G = (byte)((double)num2 * 1.1);
		((Color)(ref _mediumQualityLightingRequirement)).B = (byte)((double)num2 * 1.2);
		GetScreenDrawArea(unscaledPosition, vector + (Main.Camera.UnscaledPosition - Main.Camera.ScaledPosition), out var firstTileX, out var lastTileX, out var firstTileY, out var lastTileY);
		byte b = (byte)(100f + 150f * Main.martianLight);
		_martianGlow = new Color((int)b, (int)b, (int)b, 0);
		TileDrawInfo value = _currentTileDrawInfo.Value;
		for (int j = firstTileX - 2; j < lastTileX + 2; j++)
		{
			for (int i = firstTileY; i < lastTileY + 4; i++)
			{
				Tile tile = Main.tile[j, i];
				if (tile == null)
				{
					Main.tile[j, i] = default(Tile);
					Main.mapTime += 60;
				}
				else
				{
					if (!tile.active() || IsTileDrawLayerSolid(tile.type) != solidLayer)
					{
						continue;
					}
					if (solidLayer)
					{
						DrawTile_LiquidBehindTile(solidLayer, inFrontOfPlayers: false, waterStyleOverride, unscaledPosition, vector, j, i, tile);
					}
					ushort type = tile.type;
					short frameX = tile.frameX;
					short frameY = tile.frameY;
					if (!TextureAssets.Tile[type].IsLoaded)
					{
						Main.instance.LoadTiles(type);
					}
					if (TileLoader.PreDraw(j, i, type, Main.spriteBatch))
					{
						switch (type)
						{
						case 52:
						case 62:
						case 115:
						case 205:
						case 382:
						case 528:
						case 636:
						case 638:
							if (flag)
							{
								CrawlToTopOfVineAndAddSpecialPoint(i, j);
							}
							continue;
						case 549:
							if (flag)
							{
								CrawlToBottomOfReverseVineAndAddSpecialPoint(i, j);
							}
							continue;
						case 34:
							if (frameX % 54 == 0 && frameY % 54 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileVine);
							}
							continue;
						case 454:
							if (frameX % 72 == 0 && frameY % 54 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileVine);
							}
							continue;
						case 42:
						case 270:
						case 271:
						case 572:
						case 581:
						case 660:
							if (frameX % 18 == 0 && frameY % 36 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileVine);
							}
							continue;
						case 91:
							if (frameX % 18 == 0 && frameY % 54 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileVine);
							}
							continue;
						case 95:
						case 126:
						case 444:
							if (frameX % 36 == 0 && frameY % 36 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileVine);
							}
							continue;
						case 465:
						case 591:
						case 592:
							if (frameX % 36 == 0 && frameY % 54 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileVine);
							}
							continue;
						case 27:
							if (frameX % 36 == 0 && frameY == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
							}
							continue;
						case 236:
						case 238:
							if (frameX % 36 == 0 && frameY == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
							}
							continue;
						case 233:
							if (frameY == 0 && frameX % 54 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
							}
							if (frameY == 34 && frameX % 36 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
							}
							continue;
						case 652:
							if (frameX % 36 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
							}
							continue;
						case 651:
							if (frameX % 54 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
							}
							continue;
						case 530:
							if (frameX < 270)
							{
								if (frameX % 54 == 0 && frameY == 0 && flag)
								{
									AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
								}
								continue;
							}
							break;
						case 485:
						case 489:
						case 490:
							if (frameY == 0 && frameX % 36 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
							}
							continue;
						case 521:
						case 522:
						case 523:
						case 524:
						case 525:
						case 526:
						case 527:
							if (frameY == 0 && frameX % 36 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
							}
							continue;
						case 493:
							if (frameY == 0 && frameX % 18 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
							}
							continue;
						case 519:
							if (frameX / 18 <= 4 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
							}
							continue;
						case 373:
						case 374:
						case 375:
						case 461:
							EmitLiquidDrops(i, j, tile, type);
							continue;
						case 491:
							if (flag && frameX == 18 && frameY == 18)
							{
								AddSpecialPoint(j, i, TileCounterType.VoidLens);
							}
							break;
						case 597:
							if (flag && frameX % 54 == 0 && frameY == 0)
							{
								AddSpecialPoint(j, i, TileCounterType.TeleportationPylon);
							}
							break;
						case 617:
							if (flag && frameX % 54 == 0 && frameY % 72 == 0)
							{
								AddSpecialPoint(j, i, TileCounterType.MasterTrophy);
							}
							break;
						case 184:
							if (flag)
							{
								AddSpecialPoint(j, i, TileCounterType.AnyDirectionalGrass);
							}
							continue;
						default:
							if (ShouldSwayInWind(j, i, tile))
							{
								if (flag)
								{
									AddSpecialPoint(j, i, TileCounterType.WindyGrass);
								}
								continue;
							}
							break;
						}
						DrawSingleTile(value, solidLayer, waterStyleOverride, unscaledPosition, vector, j, i);
					}
					TileLoader.PostDraw(j, i, type, Main.spriteBatch);
				}
			}
		}
		if (solidLayer)
		{
			Main.instance.DrawTileCracks(1, Main.player[Main.myPlayer].hitReplace);
			Main.instance.DrawTileCracks(1, Main.player[Main.myPlayer].hitTile);
		}
		DrawSpecialTilesLegacy(unscaledPosition, vector);
		if (TileObject.objectPreview.Active && _localPlayer.cursorItemIconEnabled && Main.placementPreview && !CaptureManager.Instance.Active)
		{
			Main.instance.LoadTiles(TileObject.objectPreview.Type);
			TileObject.DrawPreview(Main.spriteBatch, TileObject.objectPreview, unscaledPosition - vector);
		}
		if (solidLayer)
		{
			TimeLogger.DrawTime(0, stopwatch.Elapsed.TotalMilliseconds);
		}
		else
		{
			TimeLogger.DrawTime(1, stopwatch.Elapsed.TotalMilliseconds);
		}
	}

	private void CrawlToTopOfVineAndAddSpecialPoint(int j, int i)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		int y = j;
		for (int num = j - 1; num > 0; num--)
		{
			Tile tile = Main.tile[i, num];
			if (WorldGen.SolidTile(i, num) || !tile.active())
			{
				y = num + 1;
				break;
			}
		}
		Point item = default(Point);
		((Point)(ref item))._002Ector(i, y);
		if (!_vineRootsPositions.Contains(item))
		{
			_vineRootsPositions.Add(item);
			AddSpecialPoint(i, y, TileCounterType.Vine);
		}
	}

	private void CrawlToBottomOfReverseVineAndAddSpecialPoint(int j, int i)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		int y = j;
		for (int k = j; k < Main.maxTilesY; k++)
		{
			Tile tile = Main.tile[i, k];
			if (WorldGen.SolidTile(i, k) || !tile.active())
			{
				y = k - 1;
				break;
			}
		}
		Point item = default(Point);
		((Point)(ref item))._002Ector(i, y);
		if (!_reverseVineRootsPositions.Contains(item))
		{
			_reverseVineRootsPositions.Add(item);
			AddSpecialPoint(i, y, TileCounterType.ReverseVine);
		}
	}

	private void DrawSingleTile(TileDrawInfo drawData, bool solidLayer, int waterStyleOverride, Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0706: Unknown result type (might be due to invalid IL or missing references)
		//IL_070b: Unknown result type (might be due to invalid IL or missing references)
		//IL_070d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0712: Unknown result type (might be due to invalid IL or missing references)
		//IL_072f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Unknown result type (might be due to invalid IL or missing references)
		//IL_0747: Unknown result type (might be due to invalid IL or missing references)
		//IL_074d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0752: Unknown result type (might be due to invalid IL or missing references)
		//IL_0757: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_062b: Unknown result type (might be due to invalid IL or missing references)
		//IL_063d: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_064f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0651: Unknown result type (might be due to invalid IL or missing references)
		//IL_0660: Unknown result type (might be due to invalid IL or missing references)
		//IL_066e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_086c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0876: Unknown result type (might be due to invalid IL or missing references)
		//IL_087b: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_08db: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aaa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_160f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1632: Unknown result type (might be due to invalid IL or missing references)
		//IL_1643: Unknown result type (might be due to invalid IL or missing references)
		//IL_1648: Unknown result type (might be due to invalid IL or missing references)
		//IL_164a: Unknown result type (might be due to invalid IL or missing references)
		//IL_164f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_170b: Unknown result type (might be due to invalid IL or missing references)
		//IL_171f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1724: Unknown result type (might be due to invalid IL or missing references)
		//IL_1726: Unknown result type (might be due to invalid IL or missing references)
		//IL_172b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1731: Unknown result type (might be due to invalid IL or missing references)
		//IL_1737: Unknown result type (might be due to invalid IL or missing references)
		//IL_1742: Unknown result type (might be due to invalid IL or missing references)
		//IL_166e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1671: Unknown result type (might be due to invalid IL or missing references)
		//IL_167c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1686: Unknown result type (might be due to invalid IL or missing references)
		//IL_1691: Unknown result type (might be due to invalid IL or missing references)
		//IL_165f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1661: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ead: Unknown result type (might be due to invalid IL or missing references)
		//IL_0edb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d08: Unknown result type (might be due to invalid IL or missing references)
		//IL_1012: Unknown result type (might be due to invalid IL or missing references)
		//IL_1014: Unknown result type (might be due to invalid IL or missing references)
		//IL_101b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1022: Unknown result type (might be due to invalid IL or missing references)
		//IL_102d: Unknown result type (might be due to invalid IL or missing references)
		//IL_142d: Unknown result type (might be due to invalid IL or missing references)
		//IL_144d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1457: Unknown result type (might be due to invalid IL or missing references)
		//IL_145e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1469: Unknown result type (might be due to invalid IL or missing references)
		//IL_1514: Unknown result type (might be due to invalid IL or missing references)
		//IL_1522: Unknown result type (might be due to invalid IL or missing references)
		//IL_1527: Unknown result type (might be due to invalid IL or missing references)
		//IL_1550: Unknown result type (might be due to invalid IL or missing references)
		//IL_155a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1561: Unknown result type (might be due to invalid IL or missing references)
		//IL_156c: Unknown result type (might be due to invalid IL or missing references)
		//IL_15a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_15aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_15af: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_15de: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1206: Unknown result type (might be due to invalid IL or missing references)
		//IL_1211: Unknown result type (might be due to invalid IL or missing references)
		//IL_13cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_13dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_13fa: Unknown result type (might be due to invalid IL or missing references)
		drawData.tileCache = Main.tile[tileX, tileY];
		drawData.typeCache = drawData.tileCache.type;
		drawData.tileFrameX = drawData.tileCache.frameX;
		drawData.tileFrameY = drawData.tileCache.frameY;
		drawData.tileLight = Lighting.GetColor(tileX, tileY);
		if (drawData.tileCache.liquid > 0 && drawData.tileCache.type == 518)
		{
			return;
		}
		GetTileDrawData(tileX, tileY, drawData.tileCache, drawData.typeCache, ref drawData.tileFrameX, ref drawData.tileFrameY, out drawData.tileWidth, out drawData.tileHeight, out drawData.tileTop, out drawData.halfBrickHeight, out drawData.addFrX, out drawData.addFrY, out drawData.tileSpriteEffect, out drawData.glowTexture, out drawData.glowSourceRect, out drawData.glowColor);
		drawData.drawTexture = GetTileDrawTexture(drawData.tileCache, tileX, tileY);
		Texture2D highlightTexture = null;
		Rectangle empty = Rectangle.Empty;
		Color highlightColor = Color.Transparent;
		if (TileID.Sets.HasOutlines[drawData.typeCache])
		{
			GetTileOutlineInfo(tileX, tileY, drawData.typeCache, ref drawData.tileLight, ref highlightTexture, ref highlightColor);
		}
		if (_localPlayer.dangerSense && IsTileDangerous(tileX, tileY, _localPlayer, drawData.tileCache, drawData.typeCache))
		{
			if (((Color)(ref drawData.tileLight)).R < byte.MaxValue)
			{
				((Color)(ref drawData.tileLight)).R = byte.MaxValue;
			}
			if (((Color)(ref drawData.tileLight)).G < 50)
			{
				((Color)(ref drawData.tileLight)).G = 50;
			}
			if (((Color)(ref drawData.tileLight)).B < 50)
			{
				((Color)(ref drawData.tileLight)).B = 50;
			}
			if (_isActiveAndNotPaused && _rand.Next(30) == 0)
			{
				int num = Dust.NewDust(new Vector2((float)(tileX * 16), (float)(tileY * 16)), 16, 16, 60, 0f, 0f, 100, default(Color), 0.3f);
				_dust[num].fadeIn = 1f;
				Dust obj = _dust[num];
				obj.velocity *= 0.1f;
				_dust[num].noLight = true;
				_dust[num].noGravity = true;
			}
		}
		if (_localPlayer.findTreasure && Main.IsTileSpelunkable(tileX, tileY, drawData.typeCache, drawData.tileFrameX, drawData.tileFrameY))
		{
			if (((Color)(ref drawData.tileLight)).R < 200)
			{
				((Color)(ref drawData.tileLight)).R = 200;
			}
			if (((Color)(ref drawData.tileLight)).G < 170)
			{
				((Color)(ref drawData.tileLight)).G = 170;
			}
			if (_isActiveAndNotPaused && _rand.Next(60) == 0)
			{
				int num12 = Dust.NewDust(new Vector2((float)(tileX * 16), (float)(tileY * 16)), 16, 16, 204, 0f, 0f, 150, default(Color), 0.3f);
				_dust[num12].fadeIn = 1f;
				Dust obj2 = _dust[num12];
				obj2.velocity *= 0.1f;
				_dust[num12].noLight = true;
			}
		}
		if (_localPlayer.biomeSight)
		{
			Color sightColor = Color.White;
			if (Main.IsTileBiomeSightable(tileX, tileY, drawData.typeCache, drawData.tileFrameX, drawData.tileFrameY, ref sightColor))
			{
				if (((Color)(ref drawData.tileLight)).R < ((Color)(ref sightColor)).R)
				{
					((Color)(ref drawData.tileLight)).R = ((Color)(ref sightColor)).R;
				}
				if (((Color)(ref drawData.tileLight)).G < ((Color)(ref sightColor)).G)
				{
					((Color)(ref drawData.tileLight)).G = ((Color)(ref sightColor)).G;
				}
				if (((Color)(ref drawData.tileLight)).B < ((Color)(ref sightColor)).B)
				{
					((Color)(ref drawData.tileLight)).B = ((Color)(ref sightColor)).B;
				}
				if (_isActiveAndNotPaused && _rand.Next(480) == 0)
				{
					Color newColor = sightColor;
					int num14 = Dust.NewDust(new Vector2((float)(tileX * 16), (float)(tileY * 16)), 16, 16, 267, 0f, 0f, 150, newColor, 0.3f);
					_dust[num14].noGravity = true;
					_dust[num14].fadeIn = 1f;
					Dust obj3 = _dust[num14];
					obj3.velocity *= 0.1f;
					_dust[num14].noLightEmittence = true;
				}
			}
		}
		if (_isActiveAndNotPaused)
		{
			if (!Lighting.UpdateEveryFrame || new FastRandom(Main.TileFrameSeed).WithModifier(tileX, tileY).Next(4) == 0)
			{
				DrawTiles_EmitParticles(tileY, tileX, drawData.tileCache, drawData.typeCache, drawData.tileFrameX, drawData.tileFrameY, drawData.tileLight);
			}
			drawData.tileLight = DrawTiles_GetLightOverride(tileY, tileX, drawData.tileCache, drawData.typeCache, drawData.tileFrameX, drawData.tileFrameY, drawData.tileLight);
		}
		bool flag = false;
		if (((Color)(ref drawData.tileLight)).R >= 1 || ((Color)(ref drawData.tileLight)).G >= 1 || ((Color)(ref drawData.tileLight)).B >= 1)
		{
			flag = true;
		}
		if (drawData.tileCache.wall > 0 && (drawData.tileCache.wall == 318 || drawData.tileCache.fullbrightWall()))
		{
			flag = true;
		}
		flag &= IsVisible(drawData.tileCache);
		CacheSpecialDraws_Part1(tileX, tileY, drawData.typeCache, drawData.tileFrameX, drawData.tileFrameY, !flag);
		CacheSpecialDraws_Part2(tileX, tileY, drawData, !flag);
		if (drawData.typeCache == 72 && drawData.tileFrameX >= 36)
		{
			int num15 = 0;
			if (drawData.tileFrameY == 18)
			{
				num15 = 1;
			}
			else if (drawData.tileFrameY == 36)
			{
				num15 = 2;
			}
			Main.spriteBatch.Draw(TextureAssets.ShroomCap.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X - 22), (float)(tileY * 16 - (int)screenPosition.Y - 26)) + screenOffset, (Rectangle?)new Rectangle(num15 * 62, 0, 60, 42), Lighting.GetColor(tileX, tileY), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		Rectangle rectangle = default(Rectangle);
		((Rectangle)(ref rectangle))._002Ector(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight - drawData.halfBrickHeight);
		Vector2 vector = new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop + drawData.halfBrickHeight)) + screenOffset;
		TileLoader.DrawEffects(tileX, tileY, drawData.typeCache, Main.spriteBatch, ref drawData);
		if (!flag)
		{
			return;
		}
		drawData.colorTint = Color.White;
		drawData.finalColor = GetFinalLight(drawData.tileCache, drawData.typeCache, drawData.tileLight, drawData.colorTint);
		switch (drawData.typeCache)
		{
		case 136:
			switch (drawData.tileFrameX / 18)
			{
			case 1:
				vector.X += -2f;
				break;
			case 2:
				vector.X += 2f;
				break;
			}
			break;
		case 442:
			if (drawData.tileFrameX / 22 == 3)
			{
				vector.X += 2f;
			}
			break;
		case 51:
			drawData.finalColor = drawData.tileLight * 0.5f;
			break;
		case 160:
		case 692:
		{
			Color color = default(Color);
			((Color)(ref color))._002Ector(Main.DiscoR, Main.DiscoG, Main.DiscoB, 255);
			if (drawData.tileCache.inActive())
			{
				color = drawData.tileCache.actColor(color);
			}
			drawData.finalColor = color;
			break;
		}
		case 129:
		{
			drawData.finalColor = new Color(255, 255, 255, 100);
			int num17 = 2;
			if (drawData.tileFrameX >= 324)
			{
				drawData.finalColor = Color.Transparent;
			}
			if (drawData.tileFrameY < 36)
			{
				vector.Y += num17 * (drawData.tileFrameY == 0).ToDirectionInt();
			}
			else
			{
				vector.X += num17 * (drawData.tileFrameY == 36).ToDirectionInt();
			}
			break;
		}
		case 272:
		{
			int num16 = Main.tileFrame[drawData.typeCache];
			num16 += tileX % 2;
			num16 += tileY % 2;
			num16 += tileX % 3;
			num16 += tileY % 3;
			num16 %= 2;
			num16 *= 90;
			drawData.addFrY += num16;
			rectangle.Y += num16;
			break;
		}
		case 80:
		{
			WorldGen.GetCactusType(tileX, tileY, drawData.tileFrameX, drawData.tileFrameY, out var evil, out var good, out var crimson);
			if (evil)
			{
				rectangle.Y += 54;
			}
			if (good)
			{
				rectangle.Y += 108;
			}
			if (crimson)
			{
				rectangle.Y += 162;
			}
			break;
		}
		case 83:
			drawData.drawTexture = GetTileDrawTexture(drawData.tileCache, tileX, tileY);
			break;
		case 323:
			if (drawData.tileCache.frameX <= 132 && drawData.tileCache.frameX >= 88)
			{
				return;
			}
			vector.X += drawData.tileCache.frameY;
			break;
		case 114:
			if (drawData.tileFrameY > 0)
			{
				rectangle.Height += 2;
			}
			break;
		}
		if (drawData.typeCache == 314)
		{
			DrawTile_MinecartTrack(screenPosition, screenOffset, tileX, tileY, drawData);
		}
		else if (drawData.typeCache == 171)
		{
			DrawXmasTree(screenPosition, screenOffset, tileX, tileY, drawData);
		}
		else
		{
			DrawBasicTile(screenPosition, screenOffset, tileX, tileY, drawData, rectangle, vector);
		}
		if (Main.tileGlowMask[drawData.tileCache.type] != -1)
		{
			short num18 = Main.tileGlowMask[drawData.tileCache.type];
			if (TextureAssets.GlowMask.IndexInRange(num18))
			{
				drawData.drawTexture = TextureAssets.GlowMask[num18].Value;
			}
			double num19 = Main.timeForVisualEffects * 0.08;
			Color color2 = Color.White;
			bool flag2 = false;
			switch (drawData.tileCache.type)
			{
			case 633:
				color2 = Color.Lerp(Color.White, drawData.finalColor, 0.75f);
				break;
			case 659:
			case 667:
				color2 = LiquidRenderer.GetShimmerGlitterColor(top: true, tileX, tileY);
				break;
			case 350:
				((Color)(ref color2))._002Ector(new Vector4((float)((0.0 - Math.Cos(((int)(num19 / 6.283) % 3 == 1) ? num19 : 0.0)) * 0.2 + 0.2)));
				break;
			case 381:
			case 517:
			case 687:
				color2 = _lavaMossGlow;
				break;
			case 534:
			case 535:
			case 689:
				color2 = _kryptonMossGlow;
				break;
			case 536:
			case 537:
			case 690:
				color2 = _xenonMossGlow;
				break;
			case 539:
			case 540:
			case 688:
				color2 = _argonMossGlow;
				break;
			case 625:
			case 626:
			case 691:
				color2 = _violetMossGlow;
				break;
			case 627:
			case 628:
			case 692:
				((Color)(ref color2))._002Ector(Main.DiscoR, Main.DiscoG, Main.DiscoB);
				break;
			case 370:
			case 390:
				color2 = _meteorGlow;
				break;
			case 391:
				((Color)(ref color2))._002Ector(250, 250, 250, 200);
				break;
			case 209:
				color2 = PortalHelper.GetPortalColor(Main.myPlayer, (drawData.tileCache.frameX >= 288) ? 1 : 0);
				break;
			case 429:
			case 445:
				drawData.drawTexture = GetTileDrawTexture(drawData.tileCache, tileX, tileY);
				drawData.addFrY = 18;
				break;
			case 129:
			{
				if (drawData.tileFrameX < 324)
				{
					flag2 = true;
					break;
				}
				drawData.drawTexture = GetTileDrawTexture(drawData.tileCache, tileX, tileY);
				color2 = Main.hslToRgb(0.7f + (float)Math.Sin((float)Math.PI * 2f * Main.GlobalTimeWrappedHourly * 0.16f + (float)tileX * 0.3f + (float)tileY * 0.7f) * 0.16f, 1f, 0.5f);
				((Color)(ref color2)).A = (byte)(((Color)(ref color2)).A / 2);
				color2 *= 0.3f;
				int num2 = 72;
				for (float num3 = 0f; num3 < (float)Math.PI * 2f; num3 += (float)Math.PI / 2f)
				{
					Main.spriteBatch.Draw(drawData.drawTexture, vector + num3.ToRotationVector2() * 2f, (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY + num2, drawData.tileWidth, drawData.tileHeight), color2, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
				}
				((Color)(ref color2))._002Ector(255, 255, 255, 100);
				break;
			}
			}
			if (!flag2)
			{
				if (drawData.tileCache.slope() == 0 && !drawData.tileCache.halfBrick())
				{
					Main.spriteBatch.Draw(drawData.drawTexture, vector, (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), color2, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
				}
				else if (drawData.tileCache.halfBrick())
				{
					Main.spriteBatch.Draw(drawData.drawTexture, vector, (Rectangle?)rectangle, color2, 0f, _zero, 1f, (SpriteEffects)0, 0f);
				}
				else if (TileID.Sets.Platforms[drawData.tileCache.type])
				{
					Main.spriteBatch.Draw(drawData.drawTexture, vector, (Rectangle?)rectangle, color2, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					if (drawData.tileCache.slope() == 1 && Main.tile[tileX + 1, tileY + 1].active() && Main.tileSolid[Main.tile[tileX + 1, tileY + 1].type] && Main.tile[tileX + 1, tileY + 1].slope() != 2 && !Main.tile[tileX + 1, tileY + 1].halfBrick() && (!Main.tile[tileX, tileY + 1].active() || (Main.tile[tileX, tileY + 1].blockType() != 0 && Main.tile[tileX, tileY + 1].blockType() != 5) || (!TileID.Sets.BlocksStairs[Main.tile[tileX, tileY + 1].type] && !TileID.Sets.BlocksStairsAbove[Main.tile[tileX, tileY + 1].type])))
					{
						Rectangle value = default(Rectangle);
						((Rectangle)(ref value))._002Ector(198, (int)drawData.tileFrameY, 16, 16);
						if (TileID.Sets.Platforms[Main.tile[tileX + 1, tileY + 1].type] && Main.tile[tileX + 1, tileY + 1].slope() == 0)
						{
							value.X = 324;
						}
						Main.spriteBatch.Draw(drawData.drawTexture, vector + new Vector2(0f, 16f), (Rectangle?)value, color2, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					else if (drawData.tileCache.slope() == 2 && Main.tile[tileX - 1, tileY + 1].active() && Main.tileSolid[Main.tile[tileX - 1, tileY + 1].type] && Main.tile[tileX - 1, tileY + 1].slope() != 1 && !Main.tile[tileX - 1, tileY + 1].halfBrick() && (!Main.tile[tileX, tileY + 1].active() || (Main.tile[tileX, tileY + 1].blockType() != 0 && Main.tile[tileX, tileY + 1].blockType() != 4) || (!TileID.Sets.BlocksStairs[Main.tile[tileX, tileY + 1].type] && !TileID.Sets.BlocksStairsAbove[Main.tile[tileX, tileY + 1].type])))
					{
						Rectangle value2 = default(Rectangle);
						((Rectangle)(ref value2))._002Ector(162, (int)drawData.tileFrameY, 16, 16);
						if (TileID.Sets.Platforms[Main.tile[tileX - 1, tileY + 1].type] && Main.tile[tileX - 1, tileY + 1].slope() == 0)
						{
							value2.X = 306;
						}
						Main.spriteBatch.Draw(drawData.drawTexture, vector + new Vector2(0f, 16f), (Rectangle?)value2, color2, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
				}
				else if (TileID.Sets.HasSlopeFrames[drawData.tileCache.type])
				{
					Main.spriteBatch.Draw(drawData.drawTexture, vector, (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, 16, 16), color2, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
				}
				else
				{
					int num4 = drawData.tileCache.slope();
					int num5 = 2;
					for (int i = 0; i < 8; i++)
					{
						int num6 = i * -2;
						int num7 = 16 - i * 2;
						int num8 = 16 - num7;
						int num9;
						switch (num4)
						{
						case 1:
							num6 = 0;
							num9 = i * 2;
							num7 = 14 - i * 2;
							num8 = 0;
							break;
						case 2:
							num6 = 0;
							num9 = 16 - i * 2 - 2;
							num7 = 14 - i * 2;
							num8 = 0;
							break;
						case 3:
							num9 = i * 2;
							break;
						default:
							num9 = 16 - i * 2 - 2;
							break;
						}
						Main.spriteBatch.Draw(drawData.drawTexture, vector + new Vector2((float)num9, (float)(i * num5 + num6)), (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX + num9, drawData.tileFrameY + drawData.addFrY + num8, num5, num7), color2, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					int num10 = ((num4 <= 2) ? 14 : 0);
					Main.spriteBatch.Draw(drawData.drawTexture, vector + new Vector2(0f, (float)num10), (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY + num10, 16, 2), color2, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
				}
			}
		}
		if (drawData.glowTexture != null)
		{
			Vector2 position = new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset;
			if (TileID.Sets.Platforms[drawData.typeCache])
			{
				position = vector;
			}
			Main.spriteBatch.Draw(drawData.glowTexture, position, (Rectangle?)drawData.glowSourceRect, drawData.glowColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		if (highlightTexture != null)
		{
			((Rectangle)(ref empty))._002Ector(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight);
			int num11 = 0;
			int num13 = 0;
			Main.spriteBatch.Draw(highlightTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + (float)num11, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop + num13)) + screenOffset, (Rectangle?)empty, highlightColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
	}

	/// <summary>
	/// Returns true if the tile is visible.
	/// <para />Tiles painted with Echo Coating as well as the Echo Platform, Echo Block, and Ghostly Stinkbug Blocker tiles will all be invisible unless the player has Echo Sight (Nearby active Echo Chamber tile or wearing Spectre Goggles)
	/// </summary>
	/// <param name="tile"></param>
	/// <returns></returns>
	public static bool IsVisible(Tile tile)
	{
		bool flag = tile.invisibleBlock();
		switch (tile.type)
		{
		case 19:
			if (tile.frameY / 18 == 48)
			{
				flag = true;
			}
			break;
		case 541:
		case 631:
			flag = true;
			break;
		}
		if (flag)
		{
			return Main.instance.TilesRenderer._shouldShowInvisibleBlocks;
		}
		return true;
	}

	public Texture2D GetTileDrawTexture(Tile tile, int tileX, int tileY)
	{
		Texture2D result = TextureAssets.Tile[tile.type].Value;
		int tileStyle = 0;
		int num = tile.type;
		switch (tile.type)
		{
		case 5:
			tileStyle = GetTreeBiome(tileX, tileY, tile.frameX, tile.frameY);
			break;
		case 323:
			tileStyle = GetPalmTreeBiome(tileX, tileY);
			break;
		case 83:
			if (IsAlchemyPlantHarvestable(tile.frameX / 18))
			{
				num = 84;
			}
			Main.instance.LoadTiles(num);
			break;
		case 80:
		case 227:
		{
			WorldGen.GetCactusType(tileX, tileY, tile.frameX, tile.frameY, out var sandType);
			if (!TileLoader.CanGrowModCactus(sandType))
			{
				break;
			}
			if (num == 80)
			{
				tileStyle = sandType + 1;
			}
			else if (tile.frameX == 204 || tile.frameX == 202)
			{
				Asset<Texture2D> asset = PlantLoader.GetCactusFruitTexture(sandType);
				if (asset != null)
				{
					return asset.Value;
				}
			}
			break;
		}
		}
		Texture2D texture2D = _paintSystem.TryGetTileAndRequestIfNotReady(num, tileStyle, tile.color());
		if (texture2D != null)
		{
			result = texture2D;
		}
		return result;
	}

	public Texture2D GetTileDrawTexture(Tile tile, int tileX, int tileY, int paintOverride)
	{
		Texture2D result = TextureAssets.Tile[tile.type].Value;
		int tileStyle = 0;
		int num = tile.type;
		switch (tile.type)
		{
		case 5:
			tileStyle = GetTreeBiome(tileX, tileY, tile.frameX, tile.frameY);
			break;
		case 323:
			tileStyle = GetPalmTreeBiome(tileX, tileY);
			break;
		case 83:
			if (IsAlchemyPlantHarvestable(tile.frameX / 18))
			{
				num = 84;
			}
			Main.instance.LoadTiles(num);
			break;
		case 80:
		case 227:
		{
			WorldGen.GetCactusType(tileX, tileY, tile.frameX, tile.frameY, out var sandType);
			if (!TileLoader.CanGrowModCactus(sandType))
			{
				break;
			}
			if (num == 80)
			{
				tileStyle = sandType + 1;
			}
			else if (tile.frameX == 204 || tile.frameX == 202)
			{
				Asset<Texture2D> asset = PlantLoader.GetCactusFruitTexture(sandType);
				if (asset != null)
				{
					return asset.Value;
				}
			}
			break;
		}
		}
		Texture2D texture2D = _paintSystem.TryGetTileAndRequestIfNotReady(num, tileStyle, paintOverride);
		if (texture2D != null)
		{
			result = texture2D;
		}
		return result;
	}

	private void DrawBasicTile(Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY, TileDrawInfo drawData, Rectangle normalTileRect, Vector2 normalTilePosition)
	{
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1003: Unknown result type (might be due to invalid IL or missing references)
		//IL_100f: Unknown result type (might be due to invalid IL or missing references)
		//IL_101f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0def: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ecb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ecd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0edf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f93: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0612: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_064b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0655: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_06da: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07df: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0811: Unknown result type (might be due to invalid IL or missing references)
		//IL_081d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0827: Unknown result type (might be due to invalid IL or missing references)
		//IL_0833: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Unknown result type (might be due to invalid IL or missing references)
		//IL_0951: Unknown result type (might be due to invalid IL or missing references)
		//IL_0956: Unknown result type (might be due to invalid IL or missing references)
		//IL_0983: Unknown result type (might be due to invalid IL or missing references)
		//IL_098f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0999: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb9: Unknown result type (might be due to invalid IL or missing references)
		Tile tile;
		if (TileID.Sets.Platforms[drawData.typeCache] && WorldGen.IsRope(tileX, tileY) && Main.tile[tileX, tileY - 1] != null)
		{
			tile = Main.tile[tileX, tileY - 1];
			_ = ref tile.type;
			int y = (tileY + tileX) % 3 * 18;
			Texture2D tileDrawTexture = GetTileDrawTexture(Main.tile[tileX, tileY - 1], tileX, tileY);
			if (tileDrawTexture != null)
			{
				Main.spriteBatch.Draw(tileDrawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)(tileY * 16 - (int)screenPosition.Y)) + screenOffset, (Rectangle?)new Rectangle(90, y, 16, 16), drawData.tileLight, 0f, default(Vector2), 1f, drawData.tileSpriteEffect, 0f);
			}
		}
		if (drawData.tileCache.slope() > 0)
		{
			if (TileID.Sets.Platforms[drawData.tileCache.type])
			{
				Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition, (Rectangle?)normalTileRect, drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
				if (drawData.tileCache.slope() == 1)
				{
					tile = Main.tile[tileX + 1, tileY + 1];
					if (tile.active())
					{
						bool[] tileSolid = Main.tileSolid;
						tile = Main.tile[tileX + 1, tileY + 1];
						if (tileSolid[tile.type])
						{
							tile = Main.tile[tileX + 1, tileY + 1];
							if (tile.slope() != 2)
							{
								tile = Main.tile[tileX + 1, tileY + 1];
								if (!tile.halfBrick())
								{
									tile = Main.tile[tileX, tileY + 1];
									if (!tile.active())
									{
										goto IL_0269;
									}
									tile = Main.tile[tileX, tileY + 1];
									if (tile.blockType() != 0)
									{
										tile = Main.tile[tileX, tileY + 1];
										if (tile.blockType() != 5)
										{
											goto IL_0269;
										}
									}
									bool[] blocksStairs = TileID.Sets.BlocksStairs;
									tile = Main.tile[tileX, tileY + 1];
									if (!blocksStairs[tile.type])
									{
										bool[] blocksStairsAbove = TileID.Sets.BlocksStairsAbove;
										tile = Main.tile[tileX, tileY + 1];
										if (!blocksStairsAbove[tile.type])
										{
											goto IL_0269;
										}
									}
								}
							}
						}
					}
				}
				if (drawData.tileCache.slope() != 2)
				{
					return;
				}
				tile = Main.tile[tileX - 1, tileY + 1];
				if (!tile.active())
				{
					return;
				}
				bool[] tileSolid2 = Main.tileSolid;
				tile = Main.tile[tileX - 1, tileY + 1];
				if (!tileSolid2[tile.type])
				{
					return;
				}
				tile = Main.tile[tileX - 1, tileY + 1];
				if (tile.slope() == 1)
				{
					return;
				}
				tile = Main.tile[tileX - 1, tileY + 1];
				if (tile.halfBrick())
				{
					return;
				}
				tile = Main.tile[tileX, tileY + 1];
				if (tile.active())
				{
					tile = Main.tile[tileX, tileY + 1];
					if (tile.blockType() != 0)
					{
						tile = Main.tile[tileX, tileY + 1];
						if (tile.blockType() != 4)
						{
							goto IL_043e;
						}
					}
					bool[] blocksStairs2 = TileID.Sets.BlocksStairs;
					tile = Main.tile[tileX, tileY + 1];
					if (blocksStairs2[tile.type])
					{
						return;
					}
					bool[] blocksStairsAbove2 = TileID.Sets.BlocksStairsAbove;
					tile = Main.tile[tileX, tileY + 1];
					if (blocksStairsAbove2[tile.type])
					{
						return;
					}
				}
				goto IL_043e;
			}
			if (TileID.Sets.HasSlopeFrames[drawData.tileCache.type])
			{
				Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition, (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, 16, 16), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
				return;
			}
			int num = drawData.tileCache.slope();
			int num3 = 2;
			for (int i = 0; i < 8; i++)
			{
				int num4 = i * -2;
				int num5 = 16 - i * 2;
				int num6 = 16 - num5;
				int num7;
				switch (num)
				{
				case 1:
					num4 = 0;
					num7 = i * 2;
					num5 = 14 - i * 2;
					num6 = 0;
					break;
				case 2:
					num4 = 0;
					num7 = 16 - i * 2 - 2;
					num5 = 14 - i * 2;
					num6 = 0;
					break;
				case 3:
					num7 = i * 2;
					break;
				default:
					num7 = 16 - i * 2 - 2;
					break;
				}
				Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2((float)num7, (float)(i * num3 + num4)), (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX + num7, drawData.tileFrameY + drawData.addFrY + num6, num3, num5), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
			int num8 = ((num <= 2) ? 14 : 0);
			Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, (float)num8), (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY + num8, 16, 2), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			return;
		}
		if (!TileID.Sets.Platforms[drawData.typeCache] && !TileID.Sets.IgnoresNearbyHalfbricksWhenDrawn[drawData.typeCache] && _tileSolid[drawData.typeCache] && !TileID.Sets.NotReallySolid[drawData.typeCache] && !drawData.tileCache.halfBrick())
		{
			tile = Main.tile[tileX - 1, tileY];
			if (!tile.halfBrick())
			{
				tile = Main.tile[tileX + 1, tileY];
				if (!tile.halfBrick())
				{
					goto IL_0cc9;
				}
			}
			tile = Main.tile[tileX - 1, tileY];
			if (tile.halfBrick())
			{
				tile = Main.tile[tileX + 1, tileY];
				if (tile.halfBrick())
				{
					Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 8f), (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.addFrY + drawData.tileFrameY + 8, drawData.tileWidth, 8), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					Rectangle value3 = default(Rectangle);
					((Rectangle)(ref value3))._002Ector(126 + drawData.addFrX, drawData.addFrY, 16, 8);
					tile = Main.tile[tileX, tileY - 1];
					if (tile.active())
					{
						tile = Main.tile[tileX, tileY - 1];
						if (!tile.bottomSlope())
						{
							tile = Main.tile[tileX, tileY - 1];
							if (tile.type == drawData.typeCache)
							{
								((Rectangle)(ref value3))._002Ector(90 + drawData.addFrX, drawData.addFrY, 16, 8);
							}
						}
					}
					Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition, (Rectangle?)value3, drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					return;
				}
			}
			tile = Main.tile[tileX - 1, tileY];
			if (tile.halfBrick())
			{
				int num9 = 4;
				if (TileID.Sets.AllBlocksWithSmoothBordersToResolveHalfBlockIssue[drawData.typeCache])
				{
					num9 = 2;
				}
				Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 8f), (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.addFrY + drawData.tileFrameY + 8, drawData.tileWidth, 8), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
				Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2((float)num9, 0f), (Rectangle?)new Rectangle(drawData.tileFrameX + num9 + drawData.addFrX, drawData.addFrY + drawData.tileFrameY, drawData.tileWidth - num9, drawData.tileHeight), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
				Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition, (Rectangle?)new Rectangle(144 + drawData.addFrX, drawData.addFrY, num9, 8), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
				if (num9 == 2)
				{
					Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition, (Rectangle?)new Rectangle(148 + drawData.addFrX, drawData.addFrY, 2, 2), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
				}
				return;
			}
			tile = Main.tile[tileX + 1, tileY];
			if (tile.halfBrick())
			{
				int num10 = 4;
				if (TileID.Sets.AllBlocksWithSmoothBordersToResolveHalfBlockIssue[drawData.typeCache])
				{
					num10 = 2;
				}
				Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 8f), (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.addFrY + drawData.tileFrameY + 8, drawData.tileWidth, 8), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
				Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition, (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.addFrY + drawData.tileFrameY, drawData.tileWidth - num10, drawData.tileHeight), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
				Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2((float)(16 - num10), 0f), (Rectangle?)new Rectangle(144 + (16 - num10), 0, num10, 8), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
				if (num10 == 2)
				{
					Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(14f, 0f), (Rectangle?)new Rectangle(156, 0, 2, 2), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
				}
			}
			return;
		}
		goto IL_0cc9;
		IL_043e:
		Rectangle value2 = default(Rectangle);
		((Rectangle)(ref value2))._002Ector(162, (int)drawData.tileFrameY, 16, 16);
		bool[] platforms = TileID.Sets.Platforms;
		tile = Main.tile[tileX - 1, tileY + 1];
		if (platforms[tile.type])
		{
			tile = Main.tile[tileX - 1, tileY + 1];
			if (tile.slope() == 0)
			{
				value2.X = 306;
			}
		}
		Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 16f), (Rectangle?)value2, drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		return;
		IL_0e81:
		if (TileID.Sets.CritterCageLidStyle[drawData.typeCache] >= 0)
		{
			int num2 = TileID.Sets.CritterCageLidStyle[drawData.typeCache];
			if ((num2 < 3 && normalTileRect.Y % 54 == 0) || (num2 >= 3 && normalTileRect.Y % 36 == 0))
			{
				Vector2 position = normalTilePosition;
				position.Y += 8f;
				Rectangle value4 = normalTileRect;
				value4.Y += 8;
				value4.Height -= 8;
				Main.spriteBatch.Draw(drawData.drawTexture, position, (Rectangle?)value4, drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
				position = normalTilePosition;
				position.Y -= 2f;
				value4 = normalTileRect;
				value4.Y = 0;
				value4.Height = 10;
				Main.spriteBatch.Draw(TextureAssets.CageTop[num2].Value, position, (Rectangle?)value4, drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
			else
			{
				Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition, (Rectangle?)normalTileRect, drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
		}
		else
		{
			Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition, (Rectangle?)normalTileRect, drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		goto IL_101e;
		IL_0269:
		Rectangle value = default(Rectangle);
		((Rectangle)(ref value))._002Ector(198, (int)drawData.tileFrameY, 16, 16);
		bool[] platforms2 = TileID.Sets.Platforms;
		tile = Main.tile[tileX + 1, tileY + 1];
		if (platforms2[tile.type])
		{
			tile = Main.tile[tileX + 1, tileY + 1];
			if (tile.slope() == 0)
			{
				value.X = 324;
			}
		}
		Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 16f), (Rectangle?)value, drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		return;
		IL_0cc9:
		if (Lighting.NotRetro && _tileSolid[drawData.typeCache] && !drawData.tileCache.halfBrick() && !TileID.Sets.DontDrawTileSliced[drawData.tileCache.type])
		{
			DrawSingleTile_SlicedBlock(normalTilePosition, tileX, tileY, drawData);
			return;
		}
		if (drawData.halfBrickHeight != 8)
		{
			goto IL_0e81;
		}
		tile = Main.tile[tileX, tileY + 1];
		if (tile.active())
		{
			bool[] tileSolid3 = _tileSolid;
			tile = Main.tile[tileX, tileY + 1];
			if (tileSolid3[tile.type])
			{
				tile = Main.tile[tileX, tileY + 1];
				if (!tile.halfBrick())
				{
					goto IL_0e81;
				}
			}
		}
		if (TileID.Sets.Platforms[drawData.typeCache])
		{
			Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition, (Rectangle?)normalTileRect, drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		else
		{
			Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition, (Rectangle?)normalTileRect.Modified(0, 0, 0, -4), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 4f), (Rectangle?)new Rectangle(144 + drawData.addFrX, 66 + drawData.addFrY, drawData.tileWidth, 4), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		goto IL_101e;
		IL_101e:
		DrawSingleTile_Flames(screenPosition, screenOffset, tileX, tileY, drawData);
	}

	private int GetPalmTreeBiome(int tileX, int tileY)
	{
		int i;
		for (i = tileY; Main.tile[tileX, i].active() && Main.tile[tileX, i].type == 323; i++)
		{
		}
		return GetPalmTreeVariant(tileX, i);
	}

	private static int GetTreeBiome(int tileX, int tileY, int tileFrameX, int tileFrameY)
	{
		int num = tileX;
		int i = tileY;
		int type = Main.tile[num, i].type;
		if (tileFrameX == 66 && tileFrameY <= 45)
		{
			num++;
		}
		if (tileFrameX == 88 && tileFrameY >= 66 && tileFrameY <= 110)
		{
			num--;
		}
		if (tileFrameY >= 198)
		{
			switch (tileFrameX)
			{
			case 66:
				num--;
				break;
			case 44:
				num++;
				break;
			}
		}
		else if (tileFrameY >= 132)
		{
			switch (tileFrameX)
			{
			case 22:
				num--;
				break;
			case 44:
				num++;
				break;
			}
		}
		for (; Main.tile[num, i].active() && Main.tile[num, i].type == type; i++)
		{
		}
		return GetTreeVariant(num, i);
	}

	public static int GetTreeVariant(int x, int y)
	{
		if (Main.tile[x, y] == null || !Main.tile[x, y].active())
		{
			return -1;
		}
		switch (Main.tile[x, y].type)
		{
		case 23:
		case 661:
			return 0;
		case 60:
			if (!((double)y > Main.worldSurface))
			{
				return 1;
			}
			return 5;
		case 70:
			return 6;
		case 109:
		case 492:
			return 2;
		case 147:
			return 3;
		case 199:
		case 662:
			return 4;
		default:
			if (TileLoader.CanGrowModTree(Main.tile[x, y].type))
			{
				return 7 + Main.tile[x, y].type;
			}
			return -1;
		}
	}

	private TileFlameData GetTileFlameData(int tileX, int tileY, int type, int tileFrameY)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_089e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0956: Unknown result type (might be due to invalid IL or missing references)
		//IL_095b: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_066d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0672: Unknown result type (might be due to invalid IL or missing references)
		//IL_07db: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0724: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0780: Unknown result type (might be due to invalid IL or missing references)
		//IL_0785: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_0616: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fde: Unknown result type (might be due to invalid IL or missing references)
		//IL_1034: Unknown result type (might be due to invalid IL or missing references)
		//IL_1039: Unknown result type (might be due to invalid IL or missing references)
		//IL_108f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1094: Unknown result type (might be due to invalid IL or missing references)
		//IL_1146: Unknown result type (might be due to invalid IL or missing references)
		//IL_114b: Unknown result type (might be due to invalid IL or missing references)
		//IL_10eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f0: Unknown result type (might be due to invalid IL or missing references)
		switch (type)
		{
		case 270:
		{
			TileFlameData tileFlameData = default(TileFlameData);
			tileFlameData.flameTexture = TextureAssets.FireflyJar.Value;
			tileFlameData.flameColor = new Color(200, 200, 200, 0);
			tileFlameData.flameCount = 1;
			return tileFlameData;
		}
		case 271:
		{
			TileFlameData tileFlameData2 = default(TileFlameData);
			tileFlameData2.flameTexture = TextureAssets.LightningbugJar.Value;
			tileFlameData2.flameColor = new Color(200, 200, 200, 0);
			tileFlameData2.flameCount = 1;
			return tileFlameData2;
		}
		case 581:
		{
			TileFlameData tileFlameData3 = default(TileFlameData);
			tileFlameData3.flameTexture = TextureAssets.GlowMask[291].Value;
			tileFlameData3.flameColor = new Color(200, 100, 100, 0);
			tileFlameData3.flameCount = 1;
			return tileFlameData3;
		}
		default:
		{
			if (!Main.tileFlame[type])
			{
				return default(TileFlameData);
			}
			ulong flameSeed = Main.TileFrameSeed ^ (ulong)(((long)tileX << 32) | (uint)tileY);
			int num = 0;
			switch (type)
			{
			case 4:
				num = 0;
				break;
			case 33:
			case 174:
				num = 1;
				break;
			case 100:
			case 173:
				num = 2;
				break;
			case 34:
				num = 3;
				break;
			case 93:
				num = 4;
				break;
			case 49:
				num = 5;
				break;
			case 372:
				num = 16;
				break;
			case 646:
				num = 17;
				break;
			case 98:
				num = 6;
				break;
			case 35:
				num = 7;
				break;
			case 42:
				num = 13;
				break;
			}
			TileFlameData tileFlameData4 = default(TileFlameData);
			tileFlameData4.flameTexture = TextureAssets.Flames[num].Value;
			tileFlameData4.flameSeed = flameSeed;
			TileFlameData result = tileFlameData4;
			switch (num)
			{
			case 7:
				result.flameCount = 4;
				result.flameColor = new Color(50, 50, 50, 0);
				result.flameRangeXMin = -10;
				result.flameRangeXMax = 11;
				result.flameRangeYMin = -10;
				result.flameRangeYMax = 10;
				result.flameRangeMultX = 0f;
				result.flameRangeMultY = 0f;
				break;
			case 1:
				switch (Main.tile[tileX, tileY].frameY / 22)
				{
				case 5:
				case 6:
				case 7:
				case 10:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.075f;
					result.flameRangeMultY = 0.075f;
					break;
				case 8:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.3f;
					result.flameRangeMultY = 0.3f;
					break;
				case 12:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.1f;
					result.flameRangeMultY = 0.15f;
					break;
				case 14:
					result.flameCount = 8;
					result.flameColor = new Color(75, 75, 75, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.1f;
					result.flameRangeMultY = 0.1f;
					break;
				case 16:
					result.flameCount = 4;
					result.flameColor = new Color(75, 75, 75, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.15f;
					break;
				case 27:
				case 28:
					result.flameCount = 1;
					result.flameColor = new Color(75, 75, 75, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				default:
					result.flameCount = 7;
					result.flameColor = new Color(100, 100, 100, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.35f;
					break;
				}
				break;
			case 2:
				switch (Main.tile[tileX, tileY].frameY / 36)
				{
				case 3:
					result.flameCount = 3;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.05f;
					result.flameRangeMultY = 0.15f;
					break;
				case 6:
					result.flameCount = 5;
					result.flameColor = new Color(75, 75, 75, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.15f;
					break;
				case 9:
					result.flameCount = 7;
					result.flameColor = new Color(100, 100, 100, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.3f;
					result.flameRangeMultY = 0.3f;
					break;
				case 11:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.1f;
					result.flameRangeMultY = 0.15f;
					break;
				case 13:
					result.flameCount = 8;
					result.flameColor = new Color(75, 75, 75, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.1f;
					result.flameRangeMultY = 0.1f;
					break;
				case 28:
				case 29:
					result.flameCount = 1;
					result.flameColor = new Color(75, 75, 75, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				default:
					result.flameCount = 7;
					result.flameColor = new Color(100, 100, 100, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.35f;
					break;
				}
				break;
			case 3:
				switch (Main.tile[tileX, tileY].frameY / 54)
				{
				case 8:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.075f;
					result.flameRangeMultY = 0.075f;
					break;
				case 9:
					result.flameCount = 3;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.05f;
					result.flameRangeMultY = 0.15f;
					break;
				case 11:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.3f;
					result.flameRangeMultY = 0.3f;
					break;
				case 15:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.1f;
					result.flameRangeMultY = 0.15f;
					break;
				case 17:
				case 20:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.075f;
					result.flameRangeMultY = 0.075f;
					break;
				case 18:
					result.flameCount = 8;
					result.flameColor = new Color(75, 75, 75, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.1f;
					result.flameRangeMultY = 0.1f;
					break;
				case 34:
				case 35:
					result.flameCount = 1;
					result.flameColor = new Color(75, 75, 75, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				default:
					result.flameCount = 7;
					result.flameColor = new Color(100, 100, 100, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.35f;
					break;
				}
				break;
			case 4:
				switch (Main.tile[tileX, tileY].frameY / 54)
				{
				case 1:
					result.flameCount = 3;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.15f;
					break;
				case 2:
				case 4:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.075f;
					result.flameRangeMultY = 0.075f;
					break;
				case 3:
					result.flameCount = 7;
					result.flameColor = new Color(100, 100, 100, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -20;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.2f;
					result.flameRangeMultY = 0.35f;
					break;
				case 5:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.3f;
					result.flameRangeMultY = 0.3f;
					break;
				case 9:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.1f;
					result.flameRangeMultY = 0.15f;
					break;
				case 13:
					result.flameCount = 8;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.1f;
					result.flameRangeMultY = 0.1f;
					break;
				case 12:
					result.flameCount = 1;
					result.flameColor = new Color(100, 100, 100, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.01f;
					result.flameRangeMultY = 0.01f;
					break;
				case 28:
				case 29:
					result.flameCount = 1;
					result.flameColor = new Color(75, 75, 75, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				default:
					result.flameCount = 7;
					result.flameColor = new Color(100, 100, 100, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.35f;
					break;
				}
				break;
			case 13:
				switch (tileFrameY / 36)
				{
				case 1:
				case 3:
				case 6:
				case 8:
				case 19:
				case 27:
				case 29:
				case 30:
				case 31:
				case 32:
				case 36:
				case 39:
					result.flameCount = 7;
					result.flameColor = new Color(100, 100, 100, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.35f;
					break;
				case 2:
				case 16:
				case 25:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.1f;
					break;
				case 11:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.075f;
					result.flameRangeMultY = 0.075f;
					break;
				case 34:
				case 35:
					result.flameCount = 1;
					result.flameColor = new Color(75, 75, 75, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 44:
					result.flameCount = 7;
					result.flameColor = new Color(100, 100, 100, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.35f;
					break;
				default:
					result.flameCount = 0;
					break;
				}
				break;
			default:
				result.flameCount = 7;
				result.flameColor = new Color(100, 100, 100, 0);
				if (tileFrameY / 22 == 14)
				{
					result.flameColor = new Color((float)Main.DiscoR / 255f, (float)Main.DiscoG / 255f, (float)Main.DiscoB / 255f, 0f);
				}
				result.flameRangeXMin = -10;
				result.flameRangeXMax = 11;
				result.flameRangeYMin = -10;
				result.flameRangeYMax = 1;
				result.flameRangeMultX = 0.15f;
				result.flameRangeMultY = 0.35f;
				break;
			}
			return result;
		}
		}
	}

	private void DrawSingleTile_Flames(Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY, TileDrawInfo drawData)
	{
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0912: Unknown result type (might be due to invalid IL or missing references)
		//IL_0935: Unknown result type (might be due to invalid IL or missing references)
		//IL_0946: Unknown result type (might be due to invalid IL or missing references)
		//IL_094b: Unknown result type (might be due to invalid IL or missing references)
		//IL_094c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0951: Unknown result type (might be due to invalid IL or missing references)
		//IL_0958: Unknown result type (might be due to invalid IL or missing references)
		//IL_0969: Unknown result type (might be due to invalid IL or missing references)
		//IL_0973: Unknown result type (might be due to invalid IL or missing references)
		//IL_097f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aeb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0820: Unknown result type (might be due to invalid IL or missing references)
		//IL_0846: Unknown result type (might be due to invalid IL or missing references)
		//IL_085a: Unknown result type (might be due to invalid IL or missing references)
		//IL_085f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0860: Unknown result type (might be due to invalid IL or missing references)
		//IL_0865: Unknown result type (might be due to invalid IL or missing references)
		//IL_086c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0873: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0614: Unknown result type (might be due to invalid IL or missing references)
		//IL_061b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0caf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_0687: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0691: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0711: Unknown result type (might be due to invalid IL or missing references)
		//IL_0718: Unknown result type (might be due to invalid IL or missing references)
		//IL_0722: Unknown result type (might be due to invalid IL or missing references)
		//IL_072e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ded: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_101e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1038: Unknown result type (might be due to invalid IL or missing references)
		//IL_1042: Unknown result type (might be due to invalid IL or missing references)
		//IL_104e: Unknown result type (might be due to invalid IL or missing references)
		//IL_108f: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_1104: Unknown result type (might be due to invalid IL or missing references)
		//IL_110e: Unknown result type (might be due to invalid IL or missing references)
		//IL_111a: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1217: Unknown result type (might be due to invalid IL or missing references)
		//IL_1231: Unknown result type (might be due to invalid IL or missing references)
		//IL_123b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1247: Unknown result type (might be due to invalid IL or missing references)
		//IL_37f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_381c: Unknown result type (might be due to invalid IL or missing references)
		//IL_382d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3832: Unknown result type (might be due to invalid IL or missing references)
		//IL_3833: Unknown result type (might be due to invalid IL or missing references)
		//IL_3854: Unknown result type (might be due to invalid IL or missing references)
		//IL_386e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3878: Unknown result type (might be due to invalid IL or missing references)
		//IL_3884: Unknown result type (might be due to invalid IL or missing references)
		//IL_38b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_38da: Unknown result type (might be due to invalid IL or missing references)
		//IL_38eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_38f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_38f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3912: Unknown result type (might be due to invalid IL or missing references)
		//IL_3932: Unknown result type (might be due to invalid IL or missing references)
		//IL_393c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3948: Unknown result type (might be due to invalid IL or missing references)
		//IL_39b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_39d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_39e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_39ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_39eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_39f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_39f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_39f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a01: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a06: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fa3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fe2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fe3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3004: Unknown result type (might be due to invalid IL or missing references)
		//IL_3030: Unknown result type (might be due to invalid IL or missing references)
		//IL_303a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3046: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a37: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a39: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a40: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a47: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a53: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a22: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a29: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1997: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_19cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a03: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a19: Unknown result type (might be due to invalid IL or missing references)
		//IL_27dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_27ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_2810: Unknown result type (might be due to invalid IL or missing references)
		//IL_2815: Unknown result type (might be due to invalid IL or missing references)
		//IL_2816: Unknown result type (might be due to invalid IL or missing references)
		//IL_2837: Unknown result type (might be due to invalid IL or missing references)
		//IL_2848: Unknown result type (might be due to invalid IL or missing references)
		//IL_2852: Unknown result type (might be due to invalid IL or missing references)
		//IL_285e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3070: Unknown result type (might be due to invalid IL or missing references)
		//IL_3093: Unknown result type (might be due to invalid IL or missing references)
		//IL_30a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_30a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_30aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_30cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_30dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_30e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_30f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_1400: Unknown result type (might be due to invalid IL or missing references)
		//IL_1405: Unknown result type (might be due to invalid IL or missing references)
		//IL_1406: Unknown result type (might be due to invalid IL or missing references)
		//IL_1427: Unknown result type (might be due to invalid IL or missing references)
		//IL_1438: Unknown result type (might be due to invalid IL or missing references)
		//IL_1442: Unknown result type (might be due to invalid IL or missing references)
		//IL_144e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1518: Unknown result type (might be due to invalid IL or missing references)
		//IL_153e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1552: Unknown result type (might be due to invalid IL or missing references)
		//IL_1557: Unknown result type (might be due to invalid IL or missing references)
		//IL_1558: Unknown result type (might be due to invalid IL or missing references)
		//IL_1579: Unknown result type (might be due to invalid IL or missing references)
		//IL_158a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1594: Unknown result type (might be due to invalid IL or missing references)
		//IL_15a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1608: Unknown result type (might be due to invalid IL or missing references)
		//IL_162e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1642: Unknown result type (might be due to invalid IL or missing references)
		//IL_1647: Unknown result type (might be due to invalid IL or missing references)
		//IL_1648: Unknown result type (might be due to invalid IL or missing references)
		//IL_1669: Unknown result type (might be due to invalid IL or missing references)
		//IL_167a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1684: Unknown result type (might be due to invalid IL or missing references)
		//IL_1690: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a72: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a98: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aac: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ae4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1afa: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_171d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1731: Unknown result type (might be due to invalid IL or missing references)
		//IL_1736: Unknown result type (might be due to invalid IL or missing references)
		//IL_1737: Unknown result type (might be due to invalid IL or missing references)
		//IL_1758: Unknown result type (might be due to invalid IL or missing references)
		//IL_1769: Unknown result type (might be due to invalid IL or missing references)
		//IL_1773: Unknown result type (might be due to invalid IL or missing references)
		//IL_177f: Unknown result type (might be due to invalid IL or missing references)
		//IL_17e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_180d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1821: Unknown result type (might be due to invalid IL or missing references)
		//IL_1826: Unknown result type (might be due to invalid IL or missing references)
		//IL_1827: Unknown result type (might be due to invalid IL or missing references)
		//IL_1848: Unknown result type (might be due to invalid IL or missing references)
		//IL_1859: Unknown result type (might be due to invalid IL or missing references)
		//IL_1863: Unknown result type (might be due to invalid IL or missing references)
		//IL_186f: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_18fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1911: Unknown result type (might be due to invalid IL or missing references)
		//IL_1916: Unknown result type (might be due to invalid IL or missing references)
		//IL_1917: Unknown result type (might be due to invalid IL or missing references)
		//IL_1938: Unknown result type (might be due to invalid IL or missing references)
		//IL_1949: Unknown result type (might be due to invalid IL or missing references)
		//IL_1953: Unknown result type (might be due to invalid IL or missing references)
		//IL_195f: Unknown result type (might be due to invalid IL or missing references)
		//IL_203c: Unknown result type (might be due to invalid IL or missing references)
		//IL_205f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2070: Unknown result type (might be due to invalid IL or missing references)
		//IL_2075: Unknown result type (might be due to invalid IL or missing references)
		//IL_2076: Unknown result type (might be due to invalid IL or missing references)
		//IL_2097: Unknown result type (might be due to invalid IL or missing references)
		//IL_20a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_20be: Unknown result type (might be due to invalid IL or missing references)
		//IL_226d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2293: Unknown result type (might be due to invalid IL or missing references)
		//IL_22a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_22ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_22ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_22ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_22df: Unknown result type (might be due to invalid IL or missing references)
		//IL_22e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_22f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_235d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2383: Unknown result type (might be due to invalid IL or missing references)
		//IL_2397: Unknown result type (might be due to invalid IL or missing references)
		//IL_239c: Unknown result type (might be due to invalid IL or missing references)
		//IL_239d: Unknown result type (might be due to invalid IL or missing references)
		//IL_23be: Unknown result type (might be due to invalid IL or missing references)
		//IL_23cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_23d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_23e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_28b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_28dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_28f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_28f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_28f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2918: Unknown result type (might be due to invalid IL or missing references)
		//IL_2929: Unknown result type (might be due to invalid IL or missing references)
		//IL_2933: Unknown result type (might be due to invalid IL or missing references)
		//IL_293f: Unknown result type (might be due to invalid IL or missing references)
		//IL_244d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2473: Unknown result type (might be due to invalid IL or missing references)
		//IL_2487: Unknown result type (might be due to invalid IL or missing references)
		//IL_248c: Unknown result type (might be due to invalid IL or missing references)
		//IL_248d: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_24bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_24c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_24d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_253c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2562: Unknown result type (might be due to invalid IL or missing references)
		//IL_2576: Unknown result type (might be due to invalid IL or missing references)
		//IL_257b: Unknown result type (might be due to invalid IL or missing references)
		//IL_257c: Unknown result type (might be due to invalid IL or missing references)
		//IL_259d: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_25b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_25c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_262c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2652: Unknown result type (might be due to invalid IL or missing references)
		//IL_2666: Unknown result type (might be due to invalid IL or missing references)
		//IL_266b: Unknown result type (might be due to invalid IL or missing references)
		//IL_266c: Unknown result type (might be due to invalid IL or missing references)
		//IL_268d: Unknown result type (might be due to invalid IL or missing references)
		//IL_269e: Unknown result type (might be due to invalid IL or missing references)
		//IL_26a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_26b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_271c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2742: Unknown result type (might be due to invalid IL or missing references)
		//IL_2756: Unknown result type (might be due to invalid IL or missing references)
		//IL_275b: Unknown result type (might be due to invalid IL or missing references)
		//IL_275c: Unknown result type (might be due to invalid IL or missing references)
		//IL_277d: Unknown result type (might be due to invalid IL or missing references)
		//IL_278e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2798: Unknown result type (might be due to invalid IL or missing references)
		//IL_27a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a33: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a47: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a89: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a95: Unknown result type (might be due to invalid IL or missing references)
		//IL_2afd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b23: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b37: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b79: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b85: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bec: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c12: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c26: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c68: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c74: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d02: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d16: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d58: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d64: Unknown result type (might be due to invalid IL or missing references)
		//IL_314b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3171: Unknown result type (might be due to invalid IL or missing references)
		//IL_3185: Unknown result type (might be due to invalid IL or missing references)
		//IL_318a: Unknown result type (might be due to invalid IL or missing references)
		//IL_318b: Unknown result type (might be due to invalid IL or missing references)
		//IL_31bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_31cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_31d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_31e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2df1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e05: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e47: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e53: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ebb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ee1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ef5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2efa: Unknown result type (might be due to invalid IL or missing references)
		//IL_2efb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f37: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f43: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ddc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ddd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e19: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e25: Unknown result type (might be due to invalid IL or missing references)
		//IL_2117: Unknown result type (might be due to invalid IL or missing references)
		//IL_213d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2151: Unknown result type (might be due to invalid IL or missing references)
		//IL_2156: Unknown result type (might be due to invalid IL or missing references)
		//IL_2157: Unknown result type (might be due to invalid IL or missing references)
		//IL_2178: Unknown result type (might be due to invalid IL or missing references)
		//IL_2189: Unknown result type (might be due to invalid IL or missing references)
		//IL_2193: Unknown result type (might be due to invalid IL or missing references)
		//IL_219f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ec6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ecb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ecc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eed: Unknown result type (might be due to invalid IL or missing references)
		//IL_1efe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f08: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f14: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ff8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2004: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bf7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c39: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c45: Unknown result type (might be due to invalid IL or missing references)
		//IL_373a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3760: Unknown result type (might be due to invalid IL or missing references)
		//IL_3774: Unknown result type (might be due to invalid IL or missing references)
		//IL_3779: Unknown result type (might be due to invalid IL or missing references)
		//IL_377a: Unknown result type (might be due to invalid IL or missing references)
		//IL_379b: Unknown result type (might be due to invalid IL or missing references)
		//IL_37a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_37ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_37b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cad: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ce7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cec: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ced: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d29: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d35: Unknown result type (might be due to invalid IL or missing references)
		//IL_32e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_330a: Unknown result type (might be due to invalid IL or missing references)
		//IL_331e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3323: Unknown result type (might be due to invalid IL or missing references)
		//IL_3324: Unknown result type (might be due to invalid IL or missing references)
		//IL_3345: Unknown result type (might be due to invalid IL or missing references)
		//IL_3356: Unknown result type (might be due to invalid IL or missing references)
		//IL_3360: Unknown result type (might be due to invalid IL or missing references)
		//IL_336c: Unknown result type (might be due to invalid IL or missing references)
		//IL_33d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_33f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_340d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3412: Unknown result type (might be due to invalid IL or missing references)
		//IL_3413: Unknown result type (might be due to invalid IL or missing references)
		//IL_3434: Unknown result type (might be due to invalid IL or missing references)
		//IL_3445: Unknown result type (might be due to invalid IL or missing references)
		//IL_344f: Unknown result type (might be due to invalid IL or missing references)
		//IL_345b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3598: Unknown result type (might be due to invalid IL or missing references)
		//IL_35bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_35cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_35d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_35d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_35f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3604: Unknown result type (might be due to invalid IL or missing references)
		//IL_360e: Unknown result type (might be due to invalid IL or missing references)
		//IL_361a: Unknown result type (might be due to invalid IL or missing references)
		//IL_34d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_34fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_3512: Unknown result type (might be due to invalid IL or missing references)
		//IL_3517: Unknown result type (might be due to invalid IL or missing references)
		//IL_3518: Unknown result type (might be due to invalid IL or missing references)
		//IL_3539: Unknown result type (might be due to invalid IL or missing references)
		//IL_354a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3554: Unknown result type (might be due to invalid IL or missing references)
		//IL_3560: Unknown result type (might be due to invalid IL or missing references)
		if (drawData.typeCache == 548 && drawData.tileFrameX / 54 > 6)
		{
			Main.spriteBatch.Draw(TextureAssets.GlowMask[297].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), Color.White, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		if (drawData.typeCache == 613)
		{
			Main.spriteBatch.Draw(TextureAssets.GlowMask[298].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), Color.White, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		if (drawData.typeCache == 614)
		{
			Main.spriteBatch.Draw(TextureAssets.GlowMask[299].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), Color.White, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		if (drawData.typeCache == 593)
		{
			Main.spriteBatch.Draw(TextureAssets.GlowMask[295].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), Color.White, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		if (drawData.typeCache == 594)
		{
			Main.spriteBatch.Draw(TextureAssets.GlowMask[296].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), Color.White, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		if (drawData.typeCache == 215 && drawData.tileFrameY < 36)
		{
			int num = 15;
			Color color = default(Color);
			((Color)(ref color))._002Ector(255, 255, 255, 0);
			switch (drawData.tileFrameX / 54)
			{
			case 5:
				((Color)(ref color))._002Ector((float)Main.DiscoR / 255f, (float)Main.DiscoG / 255f, (float)Main.DiscoB / 255f, 0f);
				break;
			case 14:
				((Color)(ref color))._002Ector(50, 50, 100, 20);
				break;
			case 15:
				((Color)(ref color))._002Ector(255, 255, 255, 200);
				break;
			}
			Main.spriteBatch.Draw(TextureAssets.Flames[num].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), color, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		if (drawData.typeCache == 85)
		{
			float graveyardVisualIntensity = Main.GraveyardVisualIntensity;
			if (graveyardVisualIntensity > 0f)
			{
				ulong num18 = Main.TileFrameSeed ^ (ulong)(((long)tileX << 32) | (uint)tileY);
				TileFlameData tileFlameData = GetTileFlameData(tileX, tileY, drawData.typeCache, drawData.tileFrameY);
				if (num18 == 0L)
				{
					num18 = tileFlameData.flameSeed;
				}
				tileFlameData.flameSeed = num18;
				Vector2 vector = new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset;
				Rectangle value = default(Rectangle);
				((Rectangle)(ref value))._002Ector(drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight);
				for (int i = 0; i < tileFlameData.flameCount; i++)
				{
					Color color2 = tileFlameData.flameColor * graveyardVisualIntensity;
					float x = (float)Utils.RandomInt(ref tileFlameData.flameSeed, tileFlameData.flameRangeXMin, tileFlameData.flameRangeXMax) * tileFlameData.flameRangeMultX;
					float y = (float)Utils.RandomInt(ref tileFlameData.flameSeed, tileFlameData.flameRangeYMin, tileFlameData.flameRangeYMax) * tileFlameData.flameRangeMultY;
					for (float num29 = 0f; num29 < 1f; num29 += 0.25f)
					{
						Main.spriteBatch.Draw(tileFlameData.flameTexture, vector + new Vector2(x, y) + Vector2.UnitX.RotatedBy(num29 * ((float)Math.PI * 2f)) * 2f, (Rectangle?)value, color2, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					Main.spriteBatch.Draw(tileFlameData.flameTexture, vector, (Rectangle?)value, Color.White * graveyardVisualIntensity, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
				}
			}
		}
		if (drawData.typeCache == 356 && Main.sundialCooldown == 0)
		{
			Texture2D value2 = TextureAssets.GlowMask[325].Value;
			Rectangle value3 = default(Rectangle);
			((Rectangle)(ref value3))._002Ector((int)drawData.tileFrameX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight);
			Color color3 = default(Color);
			((Color)(ref color3))._002Ector(100, 100, 100, 0);
			int num40 = tileX - drawData.tileFrameX / 18;
			int num51 = tileY - drawData.tileFrameY / 18;
			ulong seed = Main.TileFrameSeed ^ (ulong)(((long)num40 << 32) | (uint)num51);
			for (int j = 0; j < 7; j++)
			{
				float num62 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.15f;
				float num73 = (float)Utils.RandomInt(ref seed, -10, 1) * 0.35f;
				Main.spriteBatch.Draw(value2, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num62, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num73) + screenOffset, (Rectangle?)value3, color3, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
		}
		if (drawData.typeCache == 663 && Main.moondialCooldown == 0)
		{
			Texture2D value4 = TextureAssets.GlowMask[335].Value;
			Rectangle value5 = default(Rectangle);
			((Rectangle)(ref value5))._002Ector((int)drawData.tileFrameX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight);
			value5.Y += 54 * Main.moonPhase;
			Main.spriteBatch.Draw(value4, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)value5, Color.White * ((float)(int)Main.mouseTextColor / 255f), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		if (drawData.typeCache == 286)
		{
			Main.spriteBatch.Draw(TextureAssets.GlowSnail.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), new Color(75, 100, 255, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		if (drawData.typeCache == 582)
		{
			Main.spriteBatch.Draw(TextureAssets.GlowMask[293].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), new Color(200, 100, 100, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		if (drawData.typeCache == 391)
		{
			Main.spriteBatch.Draw(TextureAssets.GlowMask[131].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), new Color(250, 250, 250, 200), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		if (drawData.typeCache == 619)
		{
			Main.spriteBatch.Draw(TextureAssets.GlowMask[300].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), new Color(75, 100, 255, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		if (drawData.typeCache == 270)
		{
			Main.spriteBatch.Draw(TextureAssets.FireflyJar.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(200, 200, 200, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		if (drawData.typeCache == 271)
		{
			Main.spriteBatch.Draw(TextureAssets.LightningbugJar.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(200, 200, 200, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		if (drawData.typeCache == 581)
		{
			Main.spriteBatch.Draw(TextureAssets.GlowMask[291].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(200, 200, 200, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		if (drawData.typeCache == 316 || drawData.typeCache == 317 || drawData.typeCache == 318)
		{
			int num105 = tileX - drawData.tileFrameX / 18;
			int num94 = tileY - drawData.tileFrameY / 18;
			int num2 = num105 / 2 * (num94 / 3);
			num2 %= Main.cageFrames;
			Main.spriteBatch.Draw(TextureAssets.JellyfishBowl[drawData.typeCache - 316].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + Main.jellyfishCageFrame[drawData.typeCache - 316, num2] * 36, drawData.tileWidth, drawData.tileHeight), new Color(200, 200, 200, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		if (drawData.typeCache == 149 && drawData.tileFrameX < 54)
		{
			Main.spriteBatch.Draw(TextureAssets.XmasLight.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(200, 200, 200, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		if (drawData.typeCache == 300 || drawData.typeCache == 302 || drawData.typeCache == 303 || drawData.typeCache == 306)
		{
			int num9 = 9;
			if (drawData.typeCache == 302)
			{
				num9 = 10;
			}
			if (drawData.typeCache == 303)
			{
				num9 = 11;
			}
			if (drawData.typeCache == 306)
			{
				num9 = 12;
			}
			Main.spriteBatch.Draw(TextureAssets.Flames[num9].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), new Color(200, 200, 200, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		else if (Main.tileFlame[drawData.typeCache])
		{
			ulong seed2 = Main.TileFrameSeed ^ (ulong)(((long)tileX << 32) | (uint)tileY);
			int typeCache = drawData.typeCache;
			int num10 = 0;
			switch (typeCache)
			{
			case 4:
				num10 = 0;
				break;
			case 33:
			case 174:
				num10 = 1;
				break;
			case 100:
			case 173:
				num10 = 2;
				break;
			case 34:
				num10 = 3;
				break;
			case 93:
				num10 = 4;
				break;
			case 49:
				num10 = 5;
				break;
			case 372:
				num10 = 16;
				break;
			case 646:
				num10 = 17;
				break;
			case 98:
				num10 = 6;
				break;
			case 35:
				num10 = 7;
				break;
			case 42:
				num10 = 13;
				break;
			}
			switch (num10)
			{
			case 7:
			{
				for (int num97 = 0; num97 < 4; num97++)
				{
					float num98 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
					float num99 = (float)Utils.RandomInt(ref seed2, -10, 10) * 0.15f;
					num98 = 0f;
					num99 = 0f;
					Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num98, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num99) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
				}
				break;
			}
			case 1:
				switch (Main.tile[tileX, tileY].frameY / 22)
				{
				case 5:
				case 6:
				case 7:
				case 10:
				{
					for (int num68 = 0; num68 < 7; num68++)
					{
						float num69 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.075f;
						float num70 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.075f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num69, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num70) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 8:
				{
					for (int num75 = 0; num75 < 7; num75++)
					{
						float num76 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.3f;
						float num77 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.3f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num76, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num77) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 12:
				{
					for (int num61 = 0; num61 < 7; num61++)
					{
						float num63 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						float num64 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.15f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num63, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num64) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 14:
				{
					for (int num71 = 0; num71 < 8; num71++)
					{
						float num72 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						float num74 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num72, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num74) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 16:
				{
					for (int num65 = 0; num65 < 4; num65++)
					{
						float num66 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						float num67 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num66, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num67) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 27:
				case 28:
					Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					break;
				default:
				{
					for (int num58 = 0; num58 < 7; num58++)
					{
						float num59 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						float num60 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.35f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num59, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num60) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(100, 100, 100, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				}
				break;
			case 2:
				switch (Main.tile[tileX, tileY].frameY / 36)
				{
				case 3:
				{
					for (int num87 = 0; num87 < 3; num87++)
					{
						float num88 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.05f;
						float num89 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num88, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num89) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 6:
				{
					for (int num93 = 0; num93 < 5; num93++)
					{
						float num95 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						float num96 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num95, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num96) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 9:
				{
					for (int num81 = 0; num81 < 7; num81++)
					{
						float num82 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.3f;
						float num83 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.3f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num82, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num83) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(100, 100, 100, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 11:
				{
					for (int num90 = 0; num90 < 7; num90++)
					{
						float num91 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						float num92 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.15f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num91, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num92) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 13:
				{
					for (int num84 = 0; num84 < 8; num84++)
					{
						float num85 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						float num86 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num85, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num86) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 28:
				case 29:
					Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					break;
				default:
				{
					for (int num78 = 0; num78 < 7; num78++)
					{
						float num79 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						float num80 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.35f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num79, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num80) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(100, 100, 100, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				}
				break;
			case 3:
				switch (Main.tile[tileX, tileY].frameY / 54)
				{
				case 8:
				{
					for (int num26 = 0; num26 < 7; num26++)
					{
						float num27 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.075f;
						float num28 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.075f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num27, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num28) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 9:
				{
					for (int m = 0; m < 3; m++)
					{
						float num15 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.05f;
						float num16 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num15, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num16) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 11:
				{
					for (int num20 = 0; num20 < 7; num20++)
					{
						float num21 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.3f;
						float num22 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.3f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num21, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num22) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 15:
				{
					for (int num30 = 0; num30 < 7; num30++)
					{
						float num31 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						float num32 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.15f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num31, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num32) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 17:
				case 20:
				{
					for (int num23 = 0; num23 < 7; num23++)
					{
						float num24 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.075f;
						float num25 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.075f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num24, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num25) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 18:
				{
					for (int n = 0; n < 8; n++)
					{
						float num17 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						float num19 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num17, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num19) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 34:
				case 35:
					Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					break;
				default:
				{
					for (int l = 0; l < 7; l++)
					{
						float num13 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						float num14 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.35f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num13, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num14) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(100, 100, 100, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				}
				break;
			case 4:
				switch (Main.tile[tileX, tileY].frameY / 54)
				{
				case 1:
				{
					for (int num52 = 0; num52 < 3; num52++)
					{
						float num53 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						float num54 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num53, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num54) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 2:
				case 4:
				{
					for (int num36 = 0; num36 < 7; num36++)
					{
						float num37 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.075f;
						float num38 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.075f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num37, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num38) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 3:
				{
					for (int num45 = 0; num45 < 7; num45++)
					{
						float num46 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.2f;
						float num47 = (float)Utils.RandomInt(ref seed2, -20, 1) * 0.35f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num46, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num47) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(100, 100, 100, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 5:
				{
					for (int num55 = 0; num55 < 7; num55++)
					{
						float num56 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.3f;
						float num57 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.3f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num56, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num57) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 9:
				{
					for (int num48 = 0; num48 < 7; num48++)
					{
						float num49 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						float num50 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.15f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num49, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num50) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 13:
				{
					for (int num42 = 0; num42 < 8; num42++)
					{
						float num43 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						float num44 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.1f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num43, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num44) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 12:
				{
					float num39 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.01f;
					float num41 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.01f;
					Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num39, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num41) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(Utils.RandomInt(ref seed2, 90, 111), Utils.RandomInt(ref seed2, 90, 111), Utils.RandomInt(ref seed2, 90, 111), 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					break;
				}
				case 28:
				case 29:
					Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					break;
				default:
				{
					for (int num33 = 0; num33 < 7; num33++)
					{
						float num34 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						float num35 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.35f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num34, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num35) + screenOffset, (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), new Color(100, 100, 100, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				}
				break;
			case 13:
			{
				int num100 = drawData.tileFrameY / 36;
				switch (num100)
				{
				case 1:
				case 3:
				case 6:
				case 8:
				case 19:
				case 27:
				case 29:
				case 30:
				case 31:
				case 32:
				case 36:
				case 39:
				{
					for (int num5 = 0; num5 < 7; num5++)
					{
						float num6 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						float num7 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.35f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num6, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num7) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(100, 100, 100, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 2:
				case 16:
				case 25:
				{
					for (int num104 = 0; num104 < 7; num104++)
					{
						float num3 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
						float num4 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.1f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num3, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num4) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				default:
					switch (num100)
					{
					case 29:
					{
						for (int num101 = 0; num101 < 7; num101++)
						{
							float num102 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
							float num103 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.15f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num102, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num103) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(25, 25, 25, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 34:
					case 35:
						Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						break;
					}
					break;
				}
				break;
			}
			default:
			{
				Color color4 = default(Color);
				((Color)(ref color4))._002Ector(100, 100, 100, 0);
				if (drawData.tileCache.type == 4)
				{
					switch (drawData.tileCache.frameY / 22)
					{
					case 14:
						((Color)(ref color4))._002Ector((float)Main.DiscoR / 255f, (float)Main.DiscoG / 255f, (float)Main.DiscoB / 255f, 0f);
						break;
					case 22:
						((Color)(ref color4))._002Ector(50, 50, 100, 20);
						break;
					case 23:
						((Color)(ref color4))._002Ector(255, 255, 255, 200);
						break;
					}
				}
				if (drawData.tileCache.type == 646)
				{
					((Color)(ref color4))._002Ector(100, 100, 100, 150);
				}
				for (int k = 0; k < 7; k++)
				{
					float num11 = (float)Utils.RandomInt(ref seed2, -10, 11) * 0.15f;
					float num12 = (float)Utils.RandomInt(ref seed2, -10, 1) * 0.35f;
					Main.spriteBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num11, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num12) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), color4, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
				}
				break;
			}
			}
		}
		if (drawData.typeCache == 144)
		{
			Main.spriteBatch.Draw(TextureAssets.Timer.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(200, 200, 200, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		if (drawData.typeCache == 237)
		{
			Main.spriteBatch.Draw(TextureAssets.SunAltar.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(Main.mouseTextColor / 2, Main.mouseTextColor / 2, Main.mouseTextColor / 2, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
		if (drawData.typeCache != 658 || drawData.tileFrameX % 36 != 0 || drawData.tileFrameY % 54 != 0)
		{
			return;
		}
		int num8 = drawData.tileFrameY / 54;
		if (num8 != 2)
		{
			Texture2D value6 = TextureAssets.GlowMask[334].Value;
			Vector2 vector2 = default(Vector2);
			((Vector2)(ref vector2))._002Ector(0f, -10f);
			Vector2 position = new Vector2((float)(tileX * 16 - (int)screenPosition.X) - (float)drawData.tileWidth / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset + vector2;
			Rectangle value7 = value6.Frame();
			Color color5 = default(Color);
			((Color)(ref color5))._002Ector((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, 0);
			if (num8 == 0)
			{
				color5 *= 0.75f;
			}
			Main.spriteBatch.Draw(value6, position, (Rectangle?)value7, color5, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
	}

	private int GetPalmTreeVariant(int x, int y)
	{
		int num = -1;
		if (Main.tile[x, y].active() && Main.tile[x, y].type == 53)
		{
			num = 0;
		}
		if (Main.tile[x, y].active() && Main.tile[x, y].type == 234)
		{
			num = 1;
		}
		if (Main.tile[x, y].active() && Main.tile[x, y].type == 116)
		{
			num = 2;
		}
		if (Main.tile[x, y].active() && Main.tile[x, y].type == 112)
		{
			num = 3;
		}
		if (WorldGen.IsPalmOasisTree(x))
		{
			num += 4;
		}
		if (Main.tile[x, y].active() && TileLoader.CanGrowModPalmTree(Main.tile[x, y].type))
		{
			num = (8 + Main.tile[x, y].type) * ((!WorldGen.IsPalmOasisTree(x)) ? 1 : (-1));
		}
		return num;
	}

	private void DrawSingleTile_SlicedBlock(Vector2 normalTilePosition, int tileX, int tileY, TileDrawInfo drawData)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_062b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Unknown result type (might be due to invalid IL or missing references)
		//IL_0641: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		Color color = default(Color);
		Vector2 origin = default(Vector2);
		Rectangle value = default(Rectangle);
		Vector3 tileLight = default(Vector3);
		Vector2 position = default(Vector2);
		if (((Color)(ref drawData.tileLight)).R > ((Color)(ref _highQualityLightingRequirement)).R || ((Color)(ref drawData.tileLight)).G > ((Color)(ref _highQualityLightingRequirement)).G || ((Color)(ref drawData.tileLight)).B > ((Color)(ref _highQualityLightingRequirement)).B)
		{
			Vector3[] slices = drawData.colorSlices;
			Lighting.GetColor9Slice(tileX, tileY, ref slices);
			Vector3 vector = ((Color)(ref drawData.tileLight)).ToVector3();
			Vector3 tint = ((Color)(ref drawData.colorTint)).ToVector3();
			if (drawData.tileCache.fullbrightBlock())
			{
				slices = _glowPaintColorSlices;
			}
			for (int i = 0; i < 9; i++)
			{
				value.X = 0;
				value.Y = 0;
				value.Width = 4;
				value.Height = 4;
				switch (i)
				{
				case 1:
					value.Width = 8;
					value.X = 4;
					break;
				case 2:
					value.X = 12;
					break;
				case 3:
					value.Height = 8;
					value.Y = 4;
					break;
				case 4:
					value.Width = 8;
					value.Height = 8;
					value.X = 4;
					value.Y = 4;
					break;
				case 5:
					value.X = 12;
					value.Y = 4;
					value.Height = 8;
					break;
				case 6:
					value.Y = 12;
					break;
				case 7:
					value.Width = 8;
					value.Height = 4;
					value.X = 4;
					value.Y = 12;
					break;
				case 8:
					value.X = 12;
					value.Y = 12;
					break;
				}
				tileLight.X = (slices[i].X + vector.X) * 0.5f;
				tileLight.Y = (slices[i].Y + vector.Y) * 0.5f;
				tileLight.Z = (slices[i].Z + vector.Z) * 0.5f;
				GetFinalLight(drawData.tileCache, drawData.typeCache, ref tileLight, ref tint);
				position.X = normalTilePosition.X + (float)value.X;
				position.Y = normalTilePosition.Y + (float)value.Y;
				value.X += drawData.tileFrameX + drawData.addFrX;
				value.Y += drawData.tileFrameY + drawData.addFrY;
				int num = (int)(tileLight.X * 255f);
				int num2 = (int)(tileLight.Y * 255f);
				int num3 = (int)(tileLight.Z * 255f);
				if (num > 255)
				{
					num = 255;
				}
				if (num2 > 255)
				{
					num2 = 255;
				}
				if (num3 > 255)
				{
					num3 = 255;
				}
				num3 <<= 16;
				num2 <<= 8;
				((Color)(ref color)).PackedValue = (uint)(num | num2 | num3) | 0xFF000000u;
				Main.spriteBatch.Draw(drawData.drawTexture, position, (Rectangle?)value, color, 0f, origin, 1f, drawData.tileSpriteEffect, 0f);
			}
		}
		else if (((Color)(ref drawData.tileLight)).R > ((Color)(ref _mediumQualityLightingRequirement)).R || ((Color)(ref drawData.tileLight)).G > ((Color)(ref _mediumQualityLightingRequirement)).G || ((Color)(ref drawData.tileLight)).B > ((Color)(ref _mediumQualityLightingRequirement)).B)
		{
			Vector3[] slices2 = drawData.colorSlices;
			Lighting.GetColor4Slice(tileX, tileY, ref slices2);
			Vector3 vector2 = ((Color)(ref drawData.tileLight)).ToVector3();
			Vector3 tint2 = ((Color)(ref drawData.colorTint)).ToVector3();
			if (drawData.tileCache.fullbrightBlock())
			{
				slices2 = _glowPaintColorSlices;
			}
			value.Width = 8;
			value.Height = 8;
			for (int j = 0; j < 4; j++)
			{
				value.X = 0;
				value.Y = 0;
				switch (j)
				{
				case 1:
					value.X = 8;
					break;
				case 2:
					value.Y = 8;
					break;
				case 3:
					value.X = 8;
					value.Y = 8;
					break;
				}
				tileLight.X = (slices2[j].X + vector2.X) * 0.5f;
				tileLight.Y = (slices2[j].Y + vector2.Y) * 0.5f;
				tileLight.Z = (slices2[j].Z + vector2.Z) * 0.5f;
				GetFinalLight(drawData.tileCache, drawData.typeCache, ref tileLight, ref tint2);
				position.X = normalTilePosition.X + (float)value.X;
				position.Y = normalTilePosition.Y + (float)value.Y;
				value.X += drawData.tileFrameX + drawData.addFrX;
				value.Y += drawData.tileFrameY + drawData.addFrY;
				int num4 = (int)(tileLight.X * 255f);
				int num5 = (int)(tileLight.Y * 255f);
				int num6 = (int)(tileLight.Z * 255f);
				if (num4 > 255)
				{
					num4 = 255;
				}
				if (num5 > 255)
				{
					num5 = 255;
				}
				if (num6 > 255)
				{
					num6 = 255;
				}
				num6 <<= 16;
				num5 <<= 8;
				((Color)(ref color)).PackedValue = (uint)(num4 | num5 | num6) | 0xFF000000u;
				Main.spriteBatch.Draw(drawData.drawTexture, position, (Rectangle?)value, color, 0f, origin, 1f, drawData.tileSpriteEffect, 0f);
			}
		}
		else
		{
			Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition, (Rectangle?)new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
		}
	}

	private void DrawXmasTree(Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY, TileDrawInfo drawData)
	{
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		if (tileY - drawData.tileFrameY > 0 && drawData.tileFrameY == 7 && Main.tile[tileX, tileY - drawData.tileFrameY] != null)
		{
			drawData.tileTop -= 16 * drawData.tileFrameY;
			drawData.tileFrameX = Main.tile[tileX, tileY - drawData.tileFrameY].frameX;
			drawData.tileFrameY = Main.tile[tileX, tileY - drawData.tileFrameY].frameY;
		}
		if (drawData.tileFrameX < 10)
		{
			return;
		}
		int num = 0;
		if ((drawData.tileFrameY & 1) == 1)
		{
			num++;
		}
		if ((drawData.tileFrameY & 2) == 2)
		{
			num += 2;
		}
		if ((drawData.tileFrameY & 4) == 4)
		{
			num += 4;
		}
		int num2 = 0;
		if ((drawData.tileFrameY & 8) == 8)
		{
			num2++;
		}
		if ((drawData.tileFrameY & 0x10) == 16)
		{
			num2 += 2;
		}
		if ((drawData.tileFrameY & 0x20) == 32)
		{
			num2 += 4;
		}
		int num3 = 0;
		if ((drawData.tileFrameY & 0x40) == 64)
		{
			num3++;
		}
		if ((drawData.tileFrameY & 0x80) == 128)
		{
			num3 += 2;
		}
		if ((drawData.tileFrameY & 0x100) == 256)
		{
			num3 += 4;
		}
		if ((drawData.tileFrameY & 0x200) == 512)
		{
			num3 += 8;
		}
		int num4 = 0;
		if ((drawData.tileFrameY & 0x400) == 1024)
		{
			num4++;
		}
		if ((drawData.tileFrameY & 0x800) == 2048)
		{
			num4 += 2;
		}
		if ((drawData.tileFrameY & 0x1000) == 4096)
		{
			num4 += 4;
		}
		if ((drawData.tileFrameY & 0x2000) == 8192)
		{
			num4 += 8;
		}
		Color color = Lighting.GetColor(tileX + 1, tileY - 3);
		Main.spriteBatch.Draw(TextureAssets.XmasTree[0].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle(0, 0, 64, 128), color, 0f, _zero, 1f, (SpriteEffects)0, 0f);
		if (num > 0)
		{
			num--;
			Color color2 = color;
			if (num != 3)
			{
				((Color)(ref color2))._002Ector(255, 255, 255, 255);
			}
			Main.spriteBatch.Draw(TextureAssets.XmasTree[3].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle(66 * num, 0, 64, 128), color2, 0f, _zero, 1f, (SpriteEffects)0, 0f);
		}
		if (num2 > 0)
		{
			num2--;
			Main.spriteBatch.Draw(TextureAssets.XmasTree[1].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle(66 * num2, 0, 64, 128), color, 0f, _zero, 1f, (SpriteEffects)0, 0f);
		}
		if (num3 > 0)
		{
			num3--;
			Main.spriteBatch.Draw(TextureAssets.XmasTree[2].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle(66 * num3, 0, 64, 128), color, 0f, _zero, 1f, (SpriteEffects)0, 0f);
		}
		if (num4 > 0)
		{
			num4--;
			Main.spriteBatch.Draw(TextureAssets.XmasTree[4].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, (Rectangle?)new Rectangle(66 * num4, 130 * Main.tileFrame[171], 64, 128), new Color(255, 255, 255, 255), 0f, _zero, 1f, (SpriteEffects)0, 0f);
		}
	}

	private void DrawTile_MinecartTrack(Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY, TileDrawInfo drawData)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		drawData.tileLight = GetFinalLight(drawData.tileCache, drawData.typeCache, drawData.tileLight, drawData.colorTint);
		Minecart.TrackColors(tileX, tileY, drawData.tileCache, out var frontColor, out var backColor);
		drawData.drawTexture = GetTileDrawTexture(drawData.tileCache, tileX, tileY, frontColor);
		Texture2D tileDrawTexture = GetTileDrawTexture(drawData.tileCache, tileX, tileY, backColor);
		if (WorldGen.IsRope(tileX, tileY) && Main.tile[tileX, tileY - 1] != null)
		{
			_ = ref Main.tile[tileX, tileY - 1].type;
			int y = (tileY + tileX) % 3 * 18;
			Texture2D tileDrawTexture2 = GetTileDrawTexture(Main.tile[tileX, tileY - 1], tileX, tileY);
			Main.spriteBatch.Draw(tileDrawTexture2, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)(tileY * 16 - (int)screenPosition.Y)) + screenOffset, (Rectangle?)new Rectangle(90, y, 16, 16), drawData.tileLight, 0f, default(Vector2), 1f, drawData.tileSpriteEffect, 0f);
		}
		drawData.tileCache.frameNumber();
		if (drawData.tileFrameY != -1)
		{
			Main.spriteBatch.Draw(tileDrawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)(tileY * 16 - (int)screenPosition.Y)) + screenOffset, (Rectangle?)Minecart.GetSourceRect(drawData.tileFrameY, Main.tileFrame[314]), drawData.tileLight, 0f, default(Vector2), 1f, drawData.tileSpriteEffect, 0f);
		}
		Main.spriteBatch.Draw(drawData.drawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)(tileY * 16 - (int)screenPosition.Y)) + screenOffset, (Rectangle?)Minecart.GetSourceRect(drawData.tileFrameX, Main.tileFrame[314]), drawData.tileLight, 0f, default(Vector2), 1f, drawData.tileSpriteEffect, 0f);
		if (Minecart.DrawLeftDecoration(drawData.tileFrameY))
		{
			Main.spriteBatch.Draw(tileDrawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)((tileY + 1) * 16 - (int)screenPosition.Y)) + screenOffset, (Rectangle?)Minecart.GetSourceRect(36), drawData.tileLight, 0f, default(Vector2), 1f, drawData.tileSpriteEffect, 0f);
		}
		if (Minecart.DrawLeftDecoration(drawData.tileFrameX))
		{
			Main.spriteBatch.Draw(drawData.drawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)((tileY + 1) * 16 - (int)screenPosition.Y)) + screenOffset, (Rectangle?)Minecart.GetSourceRect(36), drawData.tileLight, 0f, default(Vector2), 1f, drawData.tileSpriteEffect, 0f);
		}
		if (Minecart.DrawRightDecoration(drawData.tileFrameY))
		{
			Main.spriteBatch.Draw(tileDrawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)((tileY + 1) * 16 - (int)screenPosition.Y)) + screenOffset, (Rectangle?)Minecart.GetSourceRect(37, Main.tileFrame[314]), drawData.tileLight, 0f, default(Vector2), 1f, drawData.tileSpriteEffect, 0f);
		}
		if (Minecart.DrawRightDecoration(drawData.tileFrameX))
		{
			Main.spriteBatch.Draw(drawData.drawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)((tileY + 1) * 16 - (int)screenPosition.Y)) + screenOffset, (Rectangle?)Minecart.GetSourceRect(37), drawData.tileLight, 0f, default(Vector2), 1f, drawData.tileSpriteEffect, 0f);
		}
		if (Minecart.DrawBumper(drawData.tileFrameX))
		{
			Main.spriteBatch.Draw(drawData.drawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)((tileY - 1) * 16 - (int)screenPosition.Y)) + screenOffset, (Rectangle?)Minecart.GetSourceRect(39), drawData.tileLight, 0f, default(Vector2), 1f, drawData.tileSpriteEffect, 0f);
		}
		else if (Minecart.DrawBouncyBumper(drawData.tileFrameX))
		{
			Main.spriteBatch.Draw(drawData.drawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)((tileY - 1) * 16 - (int)screenPosition.Y)) + screenOffset, (Rectangle?)Minecart.GetSourceRect(38), drawData.tileLight, 0f, default(Vector2), 1f, drawData.tileSpriteEffect, 0f);
		}
	}

	private void DrawTile_LiquidBehindTile(bool solidLayer, bool inFrontOfPlayers, int waterStyleOverride, Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY, Tile tileCache)
	{
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05da: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0702: Unknown result type (might be due to invalid IL or missing references)
		//IL_0707: Unknown result type (might be due to invalid IL or missing references)
		//IL_0714: Unknown result type (might be due to invalid IL or missing references)
		//IL_071b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_079d: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07db: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f4: Unknown result type (might be due to invalid IL or missing references)
		Tile tile = Main.tile[tileX + 1, tileY];
		Tile tile2 = Main.tile[tileX - 1, tileY];
		Tile tile3 = Main.tile[tileX, tileY - 1];
		Tile tile4 = Main.tile[tileX, tileY + 1];
		if (tile == null)
		{
			tile = (Main.tile[tileX + 1, tileY] = default(Tile));
		}
		if (tile2 == null)
		{
			tile2 = (Main.tile[tileX - 1, tileY] = default(Tile));
		}
		if (tile3 == null)
		{
			tile3 = (Main.tile[tileX, tileY - 1] = default(Tile));
		}
		if (tile4 == null)
		{
			tile4 = (Main.tile[tileX, tileY + 1] = default(Tile));
		}
		if (!tileCache.active() || tileCache.inActive() || _tileSolidTop[tileCache.type] || (tileCache.halfBrick() && (tile2.liquid > 160 || tile.liquid > 160) && Main.instance.waterfallManager.CheckForWaterfall(tileX, tileY)) || (TileID.Sets.BlocksWaterDrawingBehindSelf[tileCache.type] && tileCache.slope() == 0))
		{
			return;
		}
		int num = 0;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		int num2 = 0;
		bool flag6 = false;
		int num3 = tileCache.slope();
		int num4 = tileCache.blockType();
		if (tileCache.type == 546 && tileCache.liquid > 0)
		{
			flag5 = true;
			flag4 = true;
			flag = true;
			flag2 = true;
			switch (tileCache.liquidType())
			{
			case 0:
				flag6 = true;
				break;
			case 1:
				num2 = 1;
				break;
			case 2:
				num2 = 11;
				break;
			case 3:
				num2 = 14;
				break;
			}
			num = tileCache.liquid;
		}
		else
		{
			if (tileCache.liquid > 0 && num4 != 0 && (num4 != 1 || tileCache.liquid > 160))
			{
				flag5 = true;
				switch (tileCache.liquidType())
				{
				case 0:
					flag6 = true;
					break;
				case 1:
					num2 = 1;
					break;
				case 2:
					num2 = 11;
					break;
				case 3:
					num2 = 14;
					break;
				}
				if (tileCache.liquid > num)
				{
					num = tileCache.liquid;
				}
			}
			if (tile2.liquid > 0 && num3 != 1 && num3 != 3)
			{
				flag = true;
				switch (tile2.liquidType())
				{
				case 0:
					flag6 = true;
					break;
				case 1:
					num2 = 1;
					break;
				case 2:
					num2 = 11;
					break;
				case 3:
					num2 = 14;
					break;
				}
				if (tile2.liquid > num)
				{
					num = tile2.liquid;
				}
			}
			if (tile.liquid > 0 && num3 != 2 && num3 != 4)
			{
				flag2 = true;
				switch (tile.liquidType())
				{
				case 0:
					flag6 = true;
					break;
				case 1:
					num2 = 1;
					break;
				case 2:
					num2 = 11;
					break;
				case 3:
					num2 = 14;
					break;
				}
				if (tile.liquid > num)
				{
					num = tile.liquid;
				}
			}
			if (tile3.liquid > 0 && num3 != 3 && num3 != 4)
			{
				flag3 = true;
				switch (tile3.liquidType())
				{
				case 0:
					flag6 = true;
					break;
				case 1:
					num2 = 1;
					break;
				case 2:
					num2 = 11;
					break;
				case 3:
					num2 = 14;
					break;
				}
			}
			if (tile4.liquid > 0 && num3 != 1 && num3 != 2)
			{
				if (tile4.liquid > 240)
				{
					flag4 = true;
				}
				switch (tile4.liquidType())
				{
				case 0:
					flag6 = true;
					break;
				case 1:
					num2 = 1;
					break;
				case 2:
					num2 = 11;
					break;
				case 3:
					num2 = 14;
					break;
				}
			}
		}
		if (!flag3 && !flag4 && !flag && !flag2 && !flag5)
		{
			return;
		}
		if (waterStyleOverride != -1)
		{
			Main.waterStyle = waterStyleOverride;
		}
		if (num2 == 0)
		{
			num2 = Main.waterStyle;
		}
		Lighting.GetCornerColors(tileX, tileY, out var vertices);
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)(tileX * 16), (float)(tileY * 16));
		Rectangle liquidSize = default(Rectangle);
		((Rectangle)(ref liquidSize))._002Ector(0, 4, 16, 16);
		if (flag4 && (flag || flag2))
		{
			flag = true;
			flag2 = true;
		}
		if (tileCache.active() && (Main.tileSolidTop[tileCache.type] || !Main.tileSolid[tileCache.type]))
		{
			return;
		}
		if ((!flag3 || !(flag || flag2)) && !(flag4 && flag3))
		{
			if (flag3)
			{
				((Rectangle)(ref liquidSize))._002Ector(0, 4, 16, 4);
				if (tileCache.halfBrick() || tileCache.slope() != 0)
				{
					((Rectangle)(ref liquidSize))._002Ector(0, 4, 16, 12);
				}
			}
			else if (flag4 && !flag && !flag2)
			{
				((Vector2)(ref vector))._002Ector((float)(tileX * 16), (float)(tileY * 16 + 12));
				((Rectangle)(ref liquidSize))._002Ector(0, 4, 16, 4);
			}
			else
			{
				float num8 = (float)(256 - num) / 32f;
				int y = 4;
				if (tile3.liquid == 0 && (num4 != 0 || !WorldGen.SolidTile(tileX, tileY - 1)))
				{
					y = 0;
				}
				int num5 = (int)num8 * 2;
				if (tileCache.slope() != 0)
				{
					((Vector2)(ref vector))._002Ector((float)(tileX * 16), (float)(tileY * 16 + num5));
					((Rectangle)(ref liquidSize))._002Ector(0, num5, 16, 16 - num5);
				}
				else if ((flag && flag2) || tileCache.halfBrick())
				{
					((Vector2)(ref vector))._002Ector((float)(tileX * 16), (float)(tileY * 16 + num5));
					((Rectangle)(ref liquidSize))._002Ector(0, y, 16, 16 - num5);
				}
				else if (flag)
				{
					((Vector2)(ref vector))._002Ector((float)(tileX * 16), (float)(tileY * 16 + num5));
					((Rectangle)(ref liquidSize))._002Ector(0, y, 4, 16 - num5);
				}
				else
				{
					((Vector2)(ref vector))._002Ector((float)(tileX * 16 + 12), (float)(tileY * 16 + num5));
					((Rectangle)(ref liquidSize))._002Ector(0, y, 4, 16 - num5);
				}
			}
		}
		Vector2 position = vector - screenPosition + screenOffset;
		float num6 = 0.5f;
		switch (num2)
		{
		case 1:
			num6 = 1f;
			break;
		case 11:
			num6 = Math.Max(num6 * 1.7f, 1f);
			break;
		}
		if ((double)tileY <= Main.worldSurface || num6 > 1f)
		{
			num6 = 1f;
			if (tileCache.wall == 21)
			{
				num6 = 0.9f;
			}
			else if (tileCache.wall > 0)
			{
				num6 = 0.6f;
			}
		}
		if (tileCache.halfBrick() && tile3.liquid > 0 && tileCache.wall > 0)
		{
			num6 = 0f;
		}
		if (num3 == 4 && tile2.liquid == 0 && !WorldGen.SolidTile(tileX - 1, tileY))
		{
			num6 = 0f;
		}
		if (num3 == 3 && tile.liquid == 0 && !WorldGen.SolidTile(tileX + 1, tileY))
		{
			num6 = 0f;
		}
		ref Color bottomLeftColor = ref vertices.BottomLeftColor;
		bottomLeftColor *= num6;
		ref Color bottomRightColor = ref vertices.BottomRightColor;
		bottomRightColor *= num6;
		ref Color topLeftColor = ref vertices.TopLeftColor;
		topLeftColor *= num6;
		ref Color topRightColor = ref vertices.TopRightColor;
		topRightColor *= num6;
		bool flag7 = false;
		if (flag6)
		{
			for (int i = 0; i < LoaderManager.Get<WaterStylesLoader>().TotalCount; i++)
			{
				if (Main.IsLiquidStyleWater(i) && Main.liquidAlpha[i] > 0f && i != num2)
				{
					DrawPartialLiquid(!solidLayer, tileCache, ref position, ref liquidSize, i, ref vertices);
					flag7 = true;
					break;
				}
			}
		}
		VertexColors colors = vertices;
		float num7 = (flag7 ? Main.liquidAlpha[num2] : 1f);
		ref Color bottomLeftColor2 = ref colors.BottomLeftColor;
		bottomLeftColor2 *= num7;
		ref Color bottomRightColor2 = ref colors.BottomRightColor;
		bottomRightColor2 *= num7;
		ref Color topLeftColor2 = ref colors.TopLeftColor;
		topLeftColor2 *= num7;
		ref Color topRightColor2 = ref colors.TopRightColor;
		topRightColor2 *= num7;
		if (num2 == 14)
		{
			LiquidRenderer.SetShimmerVertexColors(ref colors, solidLayer ? 0.75f : 1f, tileX, tileY);
		}
		DrawPartialLiquid(!solidLayer, tileCache, ref position, ref liquidSize, num2, ref colors);
	}

	private void CacheSpecialDraws_Part1(int tileX, int tileY, int tileType, int drawDataTileFrameX, int drawDataTileFrameY, bool skipDraw)
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		if (tileType == 395)
		{
			Point point = default(Point);
			((Point)(ref point))._002Ector(tileX, tileY);
			if (drawDataTileFrameX % 36 != 0)
			{
				point.X--;
			}
			if (drawDataTileFrameY % 36 != 0)
			{
				point.Y--;
			}
			if (!_itemFrameTileEntityPositions.ContainsKey(point))
			{
				_itemFrameTileEntityPositions[point] = TEItemFrame.Find(point.X, point.Y);
				if (_itemFrameTileEntityPositions[point] != -1)
				{
					AddSpecialLegacyPoint(point);
				}
			}
		}
		if (tileType == 520)
		{
			Point point2 = default(Point);
			((Point)(ref point2))._002Ector(tileX, tileY);
			if (!_foodPlatterTileEntityPositions.ContainsKey(point2))
			{
				_foodPlatterTileEntityPositions[point2] = TEFoodPlatter.Find(point2.X, point2.Y);
				if (_foodPlatterTileEntityPositions[point2] != -1)
				{
					AddSpecialLegacyPoint(point2);
				}
			}
		}
		if (tileType == 471)
		{
			Point point3 = default(Point);
			((Point)(ref point3))._002Ector(tileX, tileY);
			point3.X -= drawDataTileFrameX % 54 / 18;
			point3.Y -= drawDataTileFrameY % 54 / 18;
			if (!_weaponRackTileEntityPositions.ContainsKey(point3))
			{
				_weaponRackTileEntityPositions[point3] = TEWeaponsRack.Find(point3.X, point3.Y);
				if (_weaponRackTileEntityPositions[point3] != -1)
				{
					AddSpecialLegacyPoint(point3);
				}
			}
		}
		if (tileType == 470)
		{
			Point point4 = default(Point);
			((Point)(ref point4))._002Ector(tileX, tileY);
			point4.X -= drawDataTileFrameX % 36 / 18;
			point4.Y -= drawDataTileFrameY % 54 / 18;
			if (!_displayDollTileEntityPositions.ContainsKey(point4))
			{
				_displayDollTileEntityPositions[point4] = TEDisplayDoll.Find(point4.X, point4.Y);
				if (_displayDollTileEntityPositions[point4] != -1)
				{
					AddSpecialLegacyPoint(point4);
				}
			}
		}
		if (tileType == 475)
		{
			Point point5 = default(Point);
			((Point)(ref point5))._002Ector(tileX, tileY);
			point5.X -= drawDataTileFrameX % 54 / 18;
			point5.Y -= drawDataTileFrameY % 72 / 18;
			if (!_hatRackTileEntityPositions.ContainsKey(point5))
			{
				_hatRackTileEntityPositions[point5] = TEHatRack.Find(point5.X, point5.Y);
				if (_hatRackTileEntityPositions[point5] != -1)
				{
					AddSpecialLegacyPoint(point5);
				}
			}
		}
		if (tileType == 412 && drawDataTileFrameX == 0 && drawDataTileFrameY == 0)
		{
			AddSpecialLegacyPoint(tileX, tileY);
		}
		if (tileType == 620 && drawDataTileFrameX == 0 && drawDataTileFrameY == 0)
		{
			AddSpecialLegacyPoint(tileX, tileY);
		}
		if (tileType == 237 && drawDataTileFrameX == 18 && drawDataTileFrameY == 0)
		{
			AddSpecialLegacyPoint(tileX, tileY);
		}
		if (skipDraw)
		{
			return;
		}
		switch (tileType)
		{
		case 323:
			if (drawDataTileFrameX <= 132 && drawDataTileFrameX >= 88)
			{
				AddSpecialPoint(tileX, tileY, TileCounterType.Tree);
			}
			break;
		case 5:
		case 583:
		case 584:
		case 585:
		case 586:
		case 587:
		case 588:
		case 589:
		case 596:
		case 616:
		case 634:
			if (drawDataTileFrameY >= 198 && drawDataTileFrameX >= 22)
			{
				AddSpecialPoint(tileX, tileY, TileCounterType.Tree);
			}
			break;
		}
	}

	private void CacheSpecialDraws_Part2(int tileX, int tileY, TileDrawInfo drawData, bool skipDraw)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		if (TileID.Sets.BasicChest[drawData.typeCache])
		{
			Point key = default(Point);
			((Point)(ref key))._002Ector(tileX, tileY);
			if (drawData.tileFrameX % 36 != 0)
			{
				key.X--;
			}
			if (drawData.tileFrameY % 36 != 0)
			{
				key.Y--;
			}
			if (!_chestPositions.ContainsKey(key))
			{
				_chestPositions[key] = Chest.FindChest(key.X, key.Y);
			}
			int num = drawData.tileFrameX / 18;
			int num6 = drawData.tileFrameY / 18;
			int num2 = drawData.tileFrameX / 36;
			int num3 = num * 18;
			drawData.addFrX = num3 - drawData.tileFrameX;
			int num4 = num6 * 18;
			if (_chestPositions[key] != -1)
			{
				int frame = Main.chest[_chestPositions[key]].frame;
				if (frame == 1)
				{
					num4 += 38;
				}
				if (frame == 2)
				{
					num4 += 76;
				}
			}
			drawData.addFrY = num4 - drawData.tileFrameY;
			if (num6 != 0)
			{
				drawData.tileHeight = 18;
			}
			if (drawData.typeCache == 21 && (num2 == 48 || num2 == 49))
			{
				drawData.glowSourceRect = new Rectangle(16 * (num % 2), drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight);
			}
		}
		if (drawData.typeCache != 378)
		{
			return;
		}
		Point key2 = default(Point);
		((Point)(ref key2))._002Ector(tileX, tileY);
		if (drawData.tileFrameX % 36 != 0)
		{
			key2.X--;
		}
		if (drawData.tileFrameY % 54 != 0)
		{
			key2.Y -= drawData.tileFrameY / 18;
		}
		if (!_trainingDummyTileEntityPositions.ContainsKey(key2))
		{
			_trainingDummyTileEntityPositions[key2] = TETrainingDummy.Find(key2.X, key2.Y);
		}
		if (_trainingDummyTileEntityPositions[key2] != -1)
		{
			int npc = ((TETrainingDummy)TileEntity.ByID[_trainingDummyTileEntityPositions[key2]]).npc;
			if (npc != -1)
			{
				int num5 = Main.npc[npc].frame.Y / 55;
				num5 *= 54;
				num5 += drawData.tileFrameY;
				drawData.addFrY = num5 - drawData.tileFrameY;
			}
		}
	}

	private static Color GetFinalLight(Tile tileCache, ushort typeCache, Color tileLight, Color tint)
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)((float)(((Color)(ref tileLight)).R * ((Color)(ref tint)).R) / 255f);
		int num2 = (int)((float)(((Color)(ref tileLight)).G * ((Color)(ref tint)).G) / 255f);
		int num3 = (int)((float)(((Color)(ref tileLight)).B * ((Color)(ref tint)).B) / 255f);
		if (num > 255)
		{
			num = 255;
		}
		if (num2 > 255)
		{
			num2 = 255;
		}
		if (num3 > 255)
		{
			num3 = 255;
		}
		num3 <<= 16;
		num2 <<= 8;
		((Color)(ref tileLight)).PackedValue = (uint)(num | num2 | num3) | 0xFF000000u;
		if (tileCache.fullbrightBlock())
		{
			tileLight = Color.White;
		}
		if (tileCache.inActive())
		{
			tileLight = tileCache.actColor(tileLight);
		}
		else if (ShouldTileShine(typeCache, tileCache.frameX))
		{
			tileLight = Main.shine(tileLight, typeCache);
		}
		return tileLight;
	}

	private static void GetFinalLight(Tile tileCache, ushort typeCache, ref Vector3 tileLight, ref Vector3 tint)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		tileLight *= tint;
		if (tileCache.inActive())
		{
			tileCache.actColor(ref tileLight);
		}
		else if (ShouldTileShine(typeCache, tileCache.frameX))
		{
			Main.shine(ref tileLight, typeCache);
		}
	}

	private static bool ShouldTileShine(ushort type, short frameX)
	{
		if ((Main.shimmerAlpha > 0f && Main.tileSolid[type]) || type == 165)
		{
			return true;
		}
		if (!Main.tileShine2[type])
		{
			return false;
		}
		switch (type)
		{
		case 467:
		case 468:
			if (frameX >= 144)
			{
				return frameX < 178;
			}
			return false;
		case 21:
		case 441:
			if (frameX >= 36)
			{
				return frameX < 178;
			}
			return false;
		default:
			return true;
		}
	}

	internal static bool IsTileDangerous(int tileX, int tileY, Player localPlayer, Tile tileCache, ushort typeCache)
	{
		bool flag = typeCache == 135 || typeCache == 137 || TileID.Sets.Boulders[typeCache] || typeCache == 141 || typeCache == 210 || typeCache == 442 || typeCache == 443 || typeCache == 444 || typeCache == 411 || typeCache == 485 || typeCache == 85 || typeCache == 654 || (typeCache == 314 && Minecart.IsPressurePlate(tileCache));
		flag |= Main.getGoodWorld && typeCache == 230;
		flag |= Main.dontStarveWorld && typeCache == 80;
		if (tileCache.slope() == 0 && !tileCache.inActive())
		{
			flag = flag || typeCache == 32 || typeCache == 69 || typeCache == 48 || typeCache == 232 || typeCache == 352 || typeCache == 483 || typeCache == 482 || typeCache == 481 || typeCache == 51 || typeCache == 229;
			if (!localPlayer.fireWalk)
			{
				flag = flag || typeCache == 37 || typeCache == 58 || typeCache == 76;
			}
			if (!localPlayer.iceSkate)
			{
				flag = flag || typeCache == 162;
			}
		}
		return TileLoader.IsTileDangerous(tileX, tileY, typeCache, localPlayer) ?? flag;
	}

	private bool IsTileDrawLayerSolid(ushort typeCache)
	{
		if (TileID.Sets.DrawTileInSolidLayer[typeCache].HasValue)
		{
			return TileID.Sets.DrawTileInSolidLayer[typeCache].Value;
		}
		return _tileSolid[typeCache];
	}

	public void GetTileOutlineInfo(int x, int y, ushort typeCache, ref Color tileLight, ref Texture2D highlightTexture, ref Color highlightColor)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		if (Main.InSmartCursorHighlightArea(x, y, out var actuallySelected))
		{
			int num = (((Color)(ref tileLight)).R + ((Color)(ref tileLight)).G + ((Color)(ref tileLight)).B) / 3;
			if (num > 10)
			{
				highlightTexture = TextureAssets.HighlightMask[typeCache].Value;
				highlightColor = Colors.GetSelectionGlowColor(actuallySelected, num);
			}
		}
	}

	private void DrawPartialLiquid(bool behindBlocks, Tile tileCache, ref Vector2 position, ref Rectangle liquidSize, int liquidType, ref VertexColors colors)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		int num = tileCache.slope();
		bool flag = !TileID.Sets.BlocksWaterDrawingBehindSelf[tileCache.type];
		if (!behindBlocks)
		{
			flag = false;
		}
		if (flag || num == 0)
		{
			Main.tileBatch.Draw(TextureAssets.Liquid[liquidType].Value, position, liquidSize, colors, default(Vector2), 1f, (SpriteEffects)0);
			return;
		}
		liquidSize.X += 18 * (num - 1);
		switch (num)
		{
		case 1:
			Main.tileBatch.Draw(TextureAssets.LiquidSlope[liquidType].Value, position, liquidSize, colors, Vector2.Zero, 1f, (SpriteEffects)0);
			break;
		case 2:
			Main.tileBatch.Draw(TextureAssets.LiquidSlope[liquidType].Value, position, liquidSize, colors, Vector2.Zero, 1f, (SpriteEffects)0);
			break;
		case 3:
			Main.tileBatch.Draw(TextureAssets.LiquidSlope[liquidType].Value, position, liquidSize, colors, Vector2.Zero, 1f, (SpriteEffects)0);
			break;
		case 4:
			Main.tileBatch.Draw(TextureAssets.LiquidSlope[liquidType].Value, position, liquidSize, colors, Vector2.Zero, 1f, (SpriteEffects)0);
			break;
		}
	}

	private bool InAPlaceWithWind(int x, int y, int width, int height)
	{
		return WorldGen.InAPlaceWithWind(x, y, width, height);
	}

	public void GetTileDrawData(int x, int y, Tile tileCache, ushort typeCache, ref short tileFrameX, ref short tileFrameY, out int tileWidth, out int tileHeight, out int tileTop, out int halfBrickHeight, out int addFrX, out int addFrY, out SpriteEffects tileSpriteEffect, out Texture2D glowTexture, out Rectangle glowSourceRect, out Color glowColor)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1232: Unknown result type (might be due to invalid IL or missing references)
		//IL_2550: Unknown result type (might be due to invalid IL or missing references)
		//IL_2555: Unknown result type (might be due to invalid IL or missing references)
		//IL_255c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2561: Unknown result type (might be due to invalid IL or missing references)
		//IL_2594: Unknown result type (might be due to invalid IL or missing references)
		//IL_2599: Unknown result type (might be due to invalid IL or missing references)
		//IL_25a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_25a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_25b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_25e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_25f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_25f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_25fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2603: Unknown result type (might be due to invalid IL or missing references)
		//IL_2632: Unknown result type (might be due to invalid IL or missing references)
		//IL_2637: Unknown result type (might be due to invalid IL or missing references)
		//IL_263e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2643: Unknown result type (might be due to invalid IL or missing references)
		//IL_2672: Unknown result type (might be due to invalid IL or missing references)
		//IL_2677: Unknown result type (might be due to invalid IL or missing references)
		//IL_267e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2683: Unknown result type (might be due to invalid IL or missing references)
		//IL_26b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_26b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_26be: Unknown result type (might be due to invalid IL or missing references)
		//IL_26c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_24f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_250b: Unknown result type (might be due to invalid IL or missing references)
		//IL_251c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2521: Unknown result type (might be due to invalid IL or missing references)
		//IL_2748: Unknown result type (might be due to invalid IL or missing references)
		//IL_274d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2764: Unknown result type (might be due to invalid IL or missing references)
		//IL_2775: Unknown result type (might be due to invalid IL or missing references)
		//IL_277a: Unknown result type (might be due to invalid IL or missing references)
		//IL_26f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_26f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2708: Unknown result type (might be due to invalid IL or missing references)
		//IL_270d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d73: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d80: Unknown result type (might be due to invalid IL or missing references)
		//IL_2df5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e02: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e07: Unknown result type (might be due to invalid IL or missing references)
		//IL_2958: Unknown result type (might be due to invalid IL or missing references)
		//IL_295d: Unknown result type (might be due to invalid IL or missing references)
		//IL_296b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2970: Unknown result type (might be due to invalid IL or missing references)
		//IL_329d: Unknown result type (might be due to invalid IL or missing references)
		//IL_32a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_32aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_32af: Unknown result type (might be due to invalid IL or missing references)
		//IL_3328: Unknown result type (might be due to invalid IL or missing references)
		//IL_332d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3335: Unknown result type (might be due to invalid IL or missing references)
		//IL_333a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ab4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ab9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ac1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ac6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e83: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e90: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ec6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ecb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ed3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ed8: Unknown result type (might be due to invalid IL or missing references)
		//IL_30f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_30f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_30fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_3103: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c92: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c97: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ca4: Unknown result type (might be due to invalid IL or missing references)
		//IL_31f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_31f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_31fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3202: Unknown result type (might be due to invalid IL or missing references)
		//IL_29a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_29ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_29b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_29b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dad: Unknown result type (might be due to invalid IL or missing references)
		//IL_2db2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dba: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e34: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e39: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e41: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e46: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c02: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c07: Unknown result type (might be due to invalid IL or missing references)
		//IL_32dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_32e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_32e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_32ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_3367: Unknown result type (might be due to invalid IL or missing references)
		//IL_336c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3374: Unknown result type (might be due to invalid IL or missing references)
		//IL_3379: Unknown result type (might be due to invalid IL or missing references)
		//IL_33ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_33b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_33b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_33be: Unknown result type (might be due to invalid IL or missing references)
		//IL_2af3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2af8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b00: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b05: Unknown result type (might be due to invalid IL or missing references)
		//IL_306a: Unknown result type (might be due to invalid IL or missing references)
		//IL_306f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3077: Unknown result type (might be due to invalid IL or missing references)
		//IL_307c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f73: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f78: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f80: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f85: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b61: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b66: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b73: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a32: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_31a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_31ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_31b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_31ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d26: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d33: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d38: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ffc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3001: Unknown result type (might be due to invalid IL or missing references)
		//IL_3009: Unknown result type (might be due to invalid IL or missing references)
		//IL_300e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f05: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f12: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f17: Unknown result type (might be due to invalid IL or missing references)
		//IL_3130: Unknown result type (might be due to invalid IL or missing references)
		//IL_3135: Unknown result type (might be due to invalid IL or missing references)
		//IL_313d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3142: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cde: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ce3: Unknown result type (might be due to invalid IL or missing references)
		//IL_322f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3234: Unknown result type (might be due to invalid IL or missing references)
		//IL_323c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3241: Unknown result type (might be due to invalid IL or missing references)
		//IL_29e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_29ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_29f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_29f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_24af: Unknown result type (might be due to invalid IL or missing references)
		//IL_24b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_24bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_24c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_33e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_33ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_33f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_33fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_27b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_27b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_27bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_27c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_30a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_30ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_30b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_30bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ba0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ba5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bad: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a71: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a79: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c57: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_27f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_27fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2803: Unknown result type (might be due to invalid IL or missing references)
		//IL_2808: Unknown result type (might be due to invalid IL or missing references)
		//IL_283a: Unknown result type (might be due to invalid IL or missing references)
		//IL_283f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2847: Unknown result type (might be due to invalid IL or missing references)
		//IL_284c: Unknown result type (might be due to invalid IL or missing references)
		//IL_287e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2883: Unknown result type (might be due to invalid IL or missing references)
		//IL_288b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2890: Unknown result type (might be due to invalid IL or missing references)
		//IL_28c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_28c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_28cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_28d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2909: Unknown result type (might be due to invalid IL or missing references)
		//IL_290e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2924: Unknown result type (might be due to invalid IL or missing references)
		//IL_2929: Unknown result type (might be due to invalid IL or missing references)
		tileTop = 0;
		tileWidth = 16;
		tileHeight = 16;
		halfBrickHeight = 0;
		addFrY = Main.tileFrame[typeCache] * 38;
		addFrX = 0;
		tileSpriteEffect = (SpriteEffects)0;
		glowTexture = null;
		glowSourceRect = Rectangle.Empty;
		glowColor = Color.Transparent;
		Color color = Lighting.GetColor(x, y);
		switch (typeCache)
		{
		case 443:
			if (tileFrameX / 36 >= 2)
			{
				tileTop = -2;
			}
			else
			{
				tileTop = 2;
			}
			break;
		case 571:
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			tileTop = 2;
			break;
		case 136:
			if (tileFrameX == 0)
			{
				tileTop = 2;
			}
			break;
		case 561:
			tileTop -= 2;
			tileHeight = 20;
			addFrY = tileFrameY / 18 * 4;
			break;
		case 518:
		{
			int num19 = tileCache.liquid / 16;
			num19 -= 3;
			if (WorldGen.SolidTile(x, y - 1) && num19 > 8)
			{
				num19 = 8;
			}
			if (tileCache.liquid == 0)
			{
				Tile tileSafely = Framing.GetTileSafely(x, y + 1);
				if (tileSafely.nactive())
				{
					switch (tileSafely.blockType())
					{
					case 1:
						num19 = -16 + Math.Max(8, tileSafely.liquid / 16);
						break;
					case 2:
					case 3:
						num19 -= 4;
						break;
					}
				}
			}
			tileTop -= num19;
			break;
		}
		case 330:
		case 331:
		case 332:
		case 333:
			tileTop += 2;
			break;
		case 129:
			addFrY = 0;
			if (tileFrameX >= 324)
			{
				int num15 = (tileFrameX - 324) / 18;
				int num16 = (num15 + Main.tileFrame[typeCache]) % 6 - num15;
				addFrX = num16 * 18;
			}
			break;
		case 5:
		{
			tileWidth = 20;
			tileHeight = 20;
			int treeBiome = GetTreeBiome(x, y, tileFrameX, tileFrameY);
			if (treeBiome < 7)
			{
				tileFrameX += (short)(176 * (treeBiome + 1));
			}
			break;
		}
		case 583:
		case 584:
		case 585:
		case 586:
		case 587:
		case 588:
		case 589:
		case 596:
		case 616:
		case 634:
			tileWidth = 20;
			tileHeight = 20;
			break;
		case 476:
			tileWidth = 20;
			tileHeight = 18;
			break;
		case 323:
		{
			tileWidth = 20;
			tileHeight = 20;
			int palmTreeBiome = GetPalmTreeBiome(x, y);
			if (Math.Abs(palmTreeBiome) >= 8)
			{
				tileFrameY = (short)(22 * ((palmTreeBiome < 0) ? 1 : 0));
			}
			else
			{
				tileFrameY = (short)(22 * palmTreeBiome);
			}
			break;
		}
		case 4:
			tileWidth = 20;
			tileHeight = 20;
			if (WorldGen.SolidTile(x, y - 1))
			{
				tileTop = 4;
			}
			break;
		case 78:
		case 85:
		case 100:
		case 133:
		case 134:
		case 173:
		case 210:
		case 233:
		case 254:
		case 283:
		case 378:
		case 457:
		case 466:
		case 520:
		case 651:
		case 652:
			tileTop = 2;
			break;
		case 530:
		{
			int num42 = y - tileFrameY % 36 / 18 + 2;
			int num57 = x - tileFrameX % 54 / 18;
			WorldGen.GetBiomeInfluence(num57, num57 + 3, num42, num42, out var corruptCount2, out var crimsonCount2, out var hallowedCount2);
			int num44 = corruptCount2;
			if (num44 < crimsonCount2)
			{
				num44 = crimsonCount2;
			}
			if (num44 < hallowedCount2)
			{
				num44 = hallowedCount2;
			}
			int num45 = 0;
			num45 = ((corruptCount2 != 0 || crimsonCount2 != 0 || hallowedCount2 != 0) ? ((hallowedCount2 == num44) ? 1 : ((crimsonCount2 != num44) ? 3 : 2)) : 0);
			addFrY += 36 * num45;
			tileTop = 2;
			break;
		}
		case 485:
		{
			tileTop = 2;
			int num32 = Main.tileFrameCounter[typeCache];
			num32 /= 5;
			int num34 = y - tileFrameY / 18;
			int num35 = x - tileFrameX / 18;
			num32 += num34 + num35;
			num32 %= 4;
			addFrY = num32 * 36;
			break;
		}
		case 489:
		{
			tileTop = 2;
			int num20 = y - tileFrameY / 18;
			int num21 = x - tileFrameX / 18;
			if (InAPlaceWithWind(num21, num20, 2, 3))
			{
				int num23 = Main.tileFrameCounter[typeCache];
				num23 /= 5;
				num23 += num20 + num21;
				num23 %= 16;
				addFrY = num23 * 54;
			}
			break;
		}
		case 490:
		{
			tileTop = 2;
			int y2 = y - tileFrameY / 18;
			int x2 = x - tileFrameX / 18;
			bool num58 = InAPlaceWithWind(x2, y2, 2, 2);
			int num17 = (num58 ? Main.tileFrame[typeCache] : 0);
			int num18 = 0;
			if (num58)
			{
				if (Math.Abs(Main.WindForVisuals) > 0.5f)
				{
					switch (Main.weatherVaneBobframe)
					{
					case 0:
						num18 = 0;
						break;
					case 1:
						num18 = 1;
						break;
					case 2:
						num18 = 2;
						break;
					case 3:
						num18 = 1;
						break;
					case 4:
						num18 = 0;
						break;
					case 5:
						num18 = -1;
						break;
					case 6:
						num18 = -2;
						break;
					case 7:
						num18 = -1;
						break;
					}
				}
				else
				{
					switch (Main.weatherVaneBobframe)
					{
					case 0:
						num18 = 0;
						break;
					case 1:
						num18 = 1;
						break;
					case 2:
						num18 = 0;
						break;
					case 3:
						num18 = -1;
						break;
					case 4:
						num18 = 0;
						break;
					case 5:
						num18 = 1;
						break;
					case 6:
						num18 = 0;
						break;
					case 7:
						num18 = -1;
						break;
					}
				}
			}
			num17 += num18;
			if (num17 < 0)
			{
				num17 += 12;
			}
			num17 %= 12;
			addFrY = num17 * 36;
			break;
		}
		case 33:
		case 49:
		case 174:
		case 372:
		case 646:
			tileHeight = 20;
			tileTop = -4;
			break;
		case 529:
		{
			int num10 = y + 1;
			WorldGen.GetBiomeInfluence(x, x, num10, num10, out var corruptCount, out var crimsonCount, out var hallowedCount);
			int num11 = corruptCount;
			if (num11 < crimsonCount)
			{
				num11 = crimsonCount;
			}
			if (num11 < hallowedCount)
			{
				num11 = hallowedCount;
			}
			int num13 = 0;
			num13 = ((corruptCount != 0 || crimsonCount != 0 || hallowedCount != 0) ? ((hallowedCount == num11) ? 2 : ((crimsonCount != num11) ? 4 : 3)) : ((x < WorldGen.beachDistance || x > Main.maxTilesX - WorldGen.beachDistance) ? 1 : 0));
			addFrY += 34 * num13 - tileFrameY;
			tileHeight = 32;
			tileTop = -14;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		}
		case 3:
		case 24:
		case 61:
		case 71:
		case 110:
		case 201:
		case 637:
			tileHeight = 20;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 20:
		case 590:
		case 595:
			tileHeight = 18;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 615:
			tileHeight = 18;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 16:
		case 17:
		case 18:
		case 26:
		case 32:
		case 69:
		case 72:
		case 77:
		case 79:
		case 124:
		case 137:
		case 138:
		case 352:
		case 462:
		case 487:
		case 488:
		case 574:
		case 575:
		case 576:
		case 577:
		case 578:
		case 664:
			tileHeight = 18;
			break;
		case 654:
			tileTop += 2;
			break;
		case 14:
		case 21:
		case 411:
		case 467:
		case 469:
			if (tileFrameY == 18)
			{
				tileHeight = 18;
			}
			break;
		case 15:
		case 497:
			if (tileFrameY % 40 == 18)
			{
				tileHeight = 18;
			}
			break;
		case 172:
		case 376:
			if (tileFrameY % 38 == 18)
			{
				tileHeight = 18;
			}
			break;
		case 27:
			if (tileFrameY % 74 == 54)
			{
				tileHeight = 18;
			}
			break;
		case 132:
		case 135:
			tileTop = 2;
			tileHeight = 18;
			break;
		case 82:
		case 83:
		case 84:
			tileHeight = 20;
			tileTop = -2;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 324:
			tileWidth = 20;
			tileHeight = 20;
			tileTop = -2;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 494:
			tileTop = 2;
			break;
		case 52:
		case 62:
		case 115:
		case 205:
		case 382:
		case 528:
		case 636:
		case 638:
			tileTop = -2;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 80:
		case 142:
		case 143:
			tileTop = 2;
			break;
		case 139:
		{
			tileTop = 2;
			int num24 = tileFrameY / 2016;
			addFrY -= 2016 * num24;
			addFrX += 72 * num24;
			break;
		}
		case 73:
		case 74:
		case 113:
			tileTop = -12;
			tileHeight = 32;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 388:
		case 389:
		{
			TileObjectData.GetTileData(typeCache, tileFrameX / 18);
			int num9 = 94;
			tileTop = -2;
			if (tileFrameY == num9 - 20 || tileFrameY == num9 * 2 - 20 || tileFrameY == 0 || tileFrameY == num9)
			{
				tileHeight = 18;
			}
			if (tileFrameY != 0 && tileFrameY != num9)
			{
				tileTop = 0;
			}
			break;
		}
		case 227:
			tileWidth = 32;
			tileHeight = 38;
			if (tileFrameX == 238)
			{
				tileTop -= 6;
			}
			else
			{
				tileTop -= 20;
			}
			if (tileFrameX == 204)
			{
				WorldGen.GetCactusType(x, y, tileFrameX, tileFrameY, out var evil, out var good, out var crimson);
				if (good)
				{
					tileFrameX += 238;
				}
				if (evil)
				{
					tileFrameX += 204;
				}
				if (crimson)
				{
					tileFrameX += 272;
				}
			}
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 624:
			tileWidth = 20;
			tileHeight = 16;
			tileTop += 2;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 656:
			tileWidth = 24;
			tileHeight = 34;
			tileTop -= 16;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 579:
		{
			tileWidth = 20;
			tileHeight = 20;
			tileTop -= 2;
			bool flag = (float)(x * 16 + 8) > Main.LocalPlayer.Center.X;
			if (tileFrameX > 0)
			{
				if (flag)
				{
					addFrY = 22;
				}
				else
				{
					addFrY = 0;
				}
			}
			else if (flag)
			{
				addFrY = 0;
			}
			else
			{
				addFrY = 22;
			}
			break;
		}
		case 567:
			tileWidth = 26;
			tileHeight = 18;
			if (tileFrameY == 0)
			{
				tileTop = -2;
			}
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 185:
		case 186:
		case 187:
			tileTop = 2;
			switch (typeCache)
			{
			case 185:
				if (tileFrameY == 18 && tileFrameX >= 576 && tileFrameX <= 882)
				{
					Main.tileShine2[185] = true;
				}
				else
				{
					Main.tileShine2[185] = false;
				}
				if (tileFrameY == 18)
				{
					int num30 = tileFrameX / 1908;
					addFrX -= 1908 * num30;
					addFrY += 18 * num30;
				}
				break;
			case 186:
				if (tileFrameX >= 864 && tileFrameX <= 1170)
				{
					Main.tileShine2[186] = true;
				}
				else
				{
					Main.tileShine2[186] = false;
				}
				break;
			case 187:
			{
				int num29 = tileFrameX / 1890;
				addFrX -= 1890 * num29;
				addFrY += 36 * num29;
				break;
			}
			}
			break;
		case 650:
			tileTop = 2;
			break;
		case 649:
		{
			tileTop = 2;
			int num28 = tileFrameX / 1908;
			addFrX -= 1908 * num28;
			addFrY += 18 * num28;
			break;
		}
		case 647:
			tileTop = 2;
			break;
		case 648:
		{
			tileTop = 2;
			int num27 = tileFrameX / 1890;
			addFrX -= 1890 * num27;
			addFrY += 36 * num27;
			break;
		}
		case 178:
			if (tileFrameY <= 36)
			{
				tileTop = 2;
			}
			break;
		case 184:
			tileWidth = 20;
			if (tileFrameY <= 36)
			{
				tileTop = 2;
			}
			else if (tileFrameY <= 108)
			{
				tileTop = -2;
			}
			break;
		case 519:
			tileTop = 2;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 493:
			if (tileFrameY == 0)
			{
				int num5 = Main.tileFrameCounter[typeCache];
				float num6 = Math.Abs(Main.WindForVisuals);
				int num7 = y - tileFrameY / 18;
				int num8 = x - tileFrameX / 18;
				if (!InAPlaceWithWind(x, num7, 1, 1))
				{
					num6 = 0f;
				}
				if (!(num6 < 0.1f))
				{
					if (num6 < 0.5f)
					{
						num5 /= 20;
						num5 += num7 + num8;
						num5 %= 6;
						num5 = ((!(Main.WindForVisuals < 0f)) ? (num5 + 1) : (6 - num5));
						addFrY = num5 * 36;
					}
					else
					{
						num5 /= 10;
						num5 += num7 + num8;
						num5 %= 6;
						num5 = ((!(Main.WindForVisuals < 0f)) ? (num5 + 7) : (12 - num5));
						addFrY = num5 * 36;
					}
				}
			}
			tileTop = 2;
			break;
		case 28:
		case 105:
		case 470:
		case 475:
		case 506:
		case 547:
		case 548:
		case 552:
		case 560:
		case 597:
		case 613:
		case 621:
		case 622:
		case 623:
		case 653:
			tileTop = 2;
			break;
		case 617:
			tileTop = 2;
			tileFrameY %= 144;
			tileFrameX %= 54;
			break;
		case 614:
			addFrX = Main.tileFrame[typeCache] * 54;
			addFrY = 0;
			tileTop = 2;
			break;
		case 81:
			tileTop -= 8;
			tileHeight = 26;
			tileWidth = 24;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		case 272:
			addFrY = 0;
			break;
		case 106:
			addFrY = Main.tileFrame[typeCache] * 54;
			break;
		case 300:
		case 301:
		case 302:
		case 303:
		case 304:
		case 305:
		case 306:
		case 307:
		case 308:
		case 354:
		case 355:
		case 499:
			addFrY = Main.tileFrame[typeCache] * 54;
			tileTop = 2;
			break;
		case 377:
			addFrY = Main.tileFrame[typeCache] * 38;
			tileTop = 2;
			break;
		case 463:
		case 464:
			addFrY = Main.tileFrame[typeCache] * 72;
			tileTop = 2;
			break;
		case 491:
			tileTop = 2;
			addFrX = 54;
			break;
		case 379:
			addFrY = Main.tileFrame[typeCache] * 90;
			break;
		case 349:
		{
			tileTop = 2;
			int num49 = tileFrameX % 36;
			int num53 = tileFrameY % 54;
			if (Animation.GetTemporaryFrame(x - num49 / 18, y - num53 / 18, out var frameData2))
			{
				tileFrameX = (short)(36 * frameData2 + num49);
			}
			break;
		}
		case 441:
		case 468:
		{
			if (tileFrameY == 18)
			{
				tileHeight = 18;
			}
			int num22 = tileFrameX % 36;
			int num33 = tileFrameY % 38;
			if (Animation.GetTemporaryFrame(x - num22 / 18, y - num33 / 18, out var frameData))
			{
				tileFrameY = (short)(38 * frameData + num33);
			}
			break;
		}
		case 390:
			addFrY = Main.tileFrame[typeCache] * 36;
			break;
		case 412:
			addFrY = 0;
			tileTop = 2;
			break;
		case 406:
		{
			tileHeight = 16;
			if (tileFrameY % 54 >= 36)
			{
				tileHeight = 18;
			}
			int num47 = Main.tileFrame[typeCache];
			if (tileFrameY >= 108)
			{
				num47 = 6 - tileFrameY / 54;
			}
			else if (tileFrameY >= 54)
			{
				num47 = Main.tileFrame[typeCache] - 1;
			}
			addFrY = num47 * 56;
			addFrY += tileFrameY / 54 * 2;
			break;
		}
		case 452:
		{
			int num46 = Main.tileFrame[typeCache];
			if (tileFrameX >= 54)
			{
				num46 = 0;
			}
			addFrY = num46 * 54;
			break;
		}
		case 455:
		{
			addFrY = 0;
			tileTop = 2;
			int num41 = 1 + Main.tileFrame[typeCache];
			if (!BirthdayParty.PartyIsUp)
			{
				num41 = 0;
			}
			addFrY = num41 * 54;
			break;
		}
		case 454:
			addFrY = Main.tileFrame[typeCache] * 54;
			break;
		case 453:
		{
			int num39 = Main.tileFrameCounter[typeCache];
			num39 /= 20;
			int num40 = y - tileFrameY / 18;
			num39 += num40 + x;
			num39 %= 3;
			addFrY = num39 * 54;
			break;
		}
		case 456:
		{
			int num36 = Main.tileFrameCounter[typeCache];
			num36 /= 20;
			int num37 = y - tileFrameY / 18;
			int num38 = x - tileFrameX / 18;
			num36 += num37 + num38;
			num36 %= 4;
			addFrY = num36 * 54;
			break;
		}
		case 405:
		{
			tileHeight = 16;
			if (tileFrameY > 0)
			{
				tileHeight = 18;
			}
			int num31 = Main.tileFrame[typeCache];
			if (tileFrameX >= 54)
			{
				num31 = 0;
			}
			addFrY = num31 * 38;
			break;
		}
		case 12:
		case 31:
		case 96:
		case 639:
		case 665:
			addFrY = Main.tileFrame[typeCache] * 36;
			break;
		case 238:
			tileTop = 2;
			addFrY = Main.tileFrame[typeCache] * 36;
			break;
		case 593:
		{
			if (tileFrameX >= 18)
			{
				addFrX = -18;
			}
			tileTop = 2;
			if (Animation.GetTemporaryFrame(x, y, out var frameData4))
			{
				addFrY = (short)(18 * frameData4);
			}
			else if (tileFrameX < 18)
			{
				addFrY = Main.tileFrame[typeCache] * 18;
			}
			else
			{
				addFrY = 0;
			}
			break;
		}
		case 594:
		{
			if (tileFrameX >= 36)
			{
				addFrX = -36;
			}
			tileTop = 2;
			int num25 = tileFrameX % 36;
			int num26 = tileFrameY % 36;
			if (Animation.GetTemporaryFrame(x - num25 / 18, y - num26 / 18, out var frameData3))
			{
				addFrY = (short)(36 * frameData3);
			}
			else if (tileFrameX < 36)
			{
				addFrY = Main.tileFrame[typeCache] * 36;
			}
			else
			{
				addFrY = 0;
			}
			break;
		}
		case 215:
			if (tileFrameY < 36)
			{
				addFrY = Main.tileFrame[typeCache] * 36;
			}
			else
			{
				addFrY = 252;
			}
			tileTop = 2;
			break;
		case 592:
			addFrY = Main.tileFrame[typeCache] * 54;
			break;
		case 228:
		case 231:
		case 243:
		case 247:
			tileTop = 2;
			addFrY = Main.tileFrame[typeCache] * 54;
			break;
		case 244:
			tileTop = 2;
			if (tileFrameX < 54)
			{
				addFrY = Main.tileFrame[typeCache] * 36;
			}
			else
			{
				addFrY = 0;
			}
			break;
		case 565:
			tileTop = 2;
			if (tileFrameX < 36)
			{
				addFrY = Main.tileFrame[typeCache] * 36;
			}
			else
			{
				addFrY = 0;
			}
			break;
		case 235:
			addFrY = Main.tileFrame[typeCache] * 18;
			break;
		case 217:
		case 218:
		case 564:
			addFrY = Main.tileFrame[typeCache] * 36;
			tileTop = 2;
			break;
		case 219:
		case 220:
		case 642:
			addFrY = Main.tileFrame[typeCache] * 54;
			tileTop = 2;
			break;
		case 270:
		case 271:
		case 581:
		{
			int num14 = Main.tileFrame[typeCache] + x % 6;
			if (x % 2 == 0)
			{
				num14 += 3;
			}
			if (x % 3 == 0)
			{
				num14 += 3;
			}
			if (x % 4 == 0)
			{
				num14 += 3;
			}
			while (num14 > 5)
			{
				num14 -= 6;
			}
			addFrX = num14 * 18;
			addFrY = 0;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		}
		case 660:
		{
			int num4 = Main.tileFrame[typeCache] + x % 5;
			if (x % 2 == 0)
			{
				num4 += 3;
			}
			if (x % 3 == 0)
			{
				num4 += 3;
			}
			if (x % 4 == 0)
			{
				num4 += 3;
			}
			while (num4 > 4)
			{
				num4 -= 5;
			}
			addFrX = num4 * 18;
			addFrY = 0;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		}
		case 572:
		{
			int num3;
			for (num3 = Main.tileFrame[typeCache] + x % 4; num3 > 3; num3 -= 4)
			{
			}
			addFrX = num3 * 18;
			addFrY = 0;
			if (x % 2 == 0)
			{
				tileSpriteEffect = (SpriteEffects)1;
			}
			break;
		}
		case 428:
			tileTop += 4;
			if (PressurePlateHelper.PressurePlatesPressed.ContainsKey(new Point(x, y)))
			{
				addFrX += 18;
			}
			break;
		case 442:
			tileWidth = 20;
			tileHeight = 20;
			switch (tileFrameX / 22)
			{
			case 1:
				tileTop = -4;
				break;
			case 2:
				tileTop = -2;
				tileWidth = 24;
				break;
			case 3:
				tileTop = -2;
				break;
			}
			break;
		case 426:
		case 430:
		case 431:
		case 432:
		case 433:
		case 434:
			addFrY = 90;
			break;
		case 275:
		case 276:
		case 277:
		case 278:
		case 279:
		case 280:
		case 281:
		case 296:
		case 297:
		case 309:
		case 358:
		case 359:
		case 413:
		case 414:
		case 542:
		case 550:
		case 551:
		case 553:
		case 554:
		case 558:
		case 559:
		case 599:
		case 600:
		case 601:
		case 602:
		case 603:
		case 604:
		case 605:
		case 606:
		case 607:
		case 608:
		case 609:
		case 610:
		case 611:
		case 612:
		case 632:
		case 640:
		case 643:
		case 644:
		case 645:
		{
			tileTop = 2;
			Main.critterCage = true;
			int bigAnimalCageFrame = GetBigAnimalCageFrame(x, y, tileFrameX, tileFrameY);
			switch (typeCache)
			{
			case 275:
			case 359:
			case 599:
			case 600:
			case 601:
			case 602:
			case 603:
			case 604:
			case 605:
				addFrY = Main.bunnyCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 550:
			case 551:
				addFrY = Main.turtleCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 542:
				addFrY = Main.owlCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 276:
			case 413:
			case 414:
			case 606:
			case 607:
			case 608:
			case 609:
			case 610:
			case 611:
			case 612:
				addFrY = Main.squirrelCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 277:
				addFrY = Main.mallardCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 278:
				addFrY = Main.duckCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 553:
				addFrY = Main.grebeCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 554:
				addFrY = Main.seagullCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 279:
			case 358:
				addFrY = Main.birdCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 280:
				addFrY = Main.blueBirdCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 281:
				addFrY = Main.redBirdCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 632:
			case 640:
			case 643:
			case 644:
			case 645:
				addFrY = Main.macawCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 296:
			case 297:
				addFrY = Main.scorpionCageFrame[0, bigAnimalCageFrame] * 54;
				break;
			case 309:
				addFrY = Main.penguinCageFrame[bigAnimalCageFrame] * 54;
				break;
			case 558:
			case 559:
				addFrY = Main.seahorseCageFrame[bigAnimalCageFrame] * 54;
				break;
			}
			break;
		}
		case 285:
		case 286:
		case 298:
		case 299:
		case 310:
		case 339:
		case 361:
		case 362:
		case 363:
		case 364:
		case 391:
		case 392:
		case 393:
		case 394:
		case 532:
		case 533:
		case 538:
		case 544:
		case 555:
		case 556:
		case 582:
		case 619:
		case 629:
		{
			tileTop = 2;
			Main.critterCage = true;
			int smallAnimalCageFrame2 = GetSmallAnimalCageFrame(x, y, tileFrameX, tileFrameY);
			switch (typeCache)
			{
			case 285:
				addFrY = Main.snailCageFrame[smallAnimalCageFrame2] * 36;
				break;
			case 286:
			case 582:
				addFrY = Main.snail2CageFrame[smallAnimalCageFrame2] * 36;
				break;
			case 298:
			case 361:
				addFrY = Main.frogCageFrame[smallAnimalCageFrame2] * 36;
				break;
			case 339:
			case 362:
				addFrY = Main.grasshopperCageFrame[smallAnimalCageFrame2] * 36;
				break;
			case 299:
			case 363:
				addFrY = Main.mouseCageFrame[smallAnimalCageFrame2] * 36;
				break;
			case 310:
			case 364:
			case 391:
			case 619:
				addFrY = Main.wormCageFrame[smallAnimalCageFrame2] * 36;
				break;
			case 392:
			case 393:
			case 394:
				addFrY = Main.slugCageFrame[typeCache - 392, smallAnimalCageFrame2] * 36;
				break;
			case 532:
				addFrY = Main.maggotCageFrame[smallAnimalCageFrame2] * 36;
				break;
			case 533:
				addFrY = Main.ratCageFrame[smallAnimalCageFrame2] * 36;
				break;
			case 538:
			case 544:
			case 629:
				addFrY = Main.ladybugCageFrame[smallAnimalCageFrame2] * 36;
				break;
			case 555:
			case 556:
				addFrY = Main.waterStriderCageFrame[smallAnimalCageFrame2] * 36;
				break;
			}
			break;
		}
		case 282:
		case 505:
		case 543:
		{
			tileTop = 2;
			Main.critterCage = true;
			int waterAnimalCageFrame5 = GetWaterAnimalCageFrame(x, y, tileFrameX, tileFrameY);
			addFrY = Main.fishBowlFrame[waterAnimalCageFrame5] * 36;
			break;
		}
		case 598:
		{
			tileTop = 2;
			Main.critterCage = true;
			int waterAnimalCageFrame4 = GetWaterAnimalCageFrame(x, y, tileFrameX, tileFrameY);
			addFrY = Main.lavaFishBowlFrame[waterAnimalCageFrame4] * 36;
			break;
		}
		case 568:
		case 569:
		case 570:
		{
			tileTop = 2;
			Main.critterCage = true;
			int waterAnimalCageFrame3 = GetWaterAnimalCageFrame(x, y, tileFrameX, tileFrameY);
			addFrY = Main.fairyJarFrame[waterAnimalCageFrame3] * 36;
			break;
		}
		case 288:
		case 289:
		case 290:
		case 291:
		case 292:
		case 293:
		case 294:
		case 295:
		case 360:
		case 580:
		case 620:
		{
			tileTop = 2;
			Main.critterCage = true;
			int waterAnimalCageFrame2 = GetWaterAnimalCageFrame(x, y, tileFrameX, tileFrameY);
			int num2 = typeCache - 288;
			if (typeCache == 360 || typeCache == 580 || typeCache == 620)
			{
				num2 = 8;
			}
			addFrY = Main.butterflyCageFrame[num2, waterAnimalCageFrame2] * 36;
			break;
		}
		case 521:
		case 522:
		case 523:
		case 524:
		case 525:
		case 526:
		case 527:
		{
			tileTop = 2;
			Main.critterCage = true;
			int waterAnimalCageFrame = GetWaterAnimalCageFrame(x, y, tileFrameX, tileFrameY);
			int num56 = typeCache - 521;
			addFrY = Main.dragonflyJarFrame[num56, waterAnimalCageFrame] * 36;
			break;
		}
		case 316:
		case 317:
		case 318:
		{
			tileTop = 2;
			Main.critterCage = true;
			int smallAnimalCageFrame = GetSmallAnimalCageFrame(x, y, tileFrameX, tileFrameY);
			int num55 = typeCache - 316;
			addFrY = Main.jellyfishCageFrame[num55, smallAnimalCageFrame] * 36;
			break;
		}
		case 207:
			tileTop = 2;
			if (tileFrameY >= 72)
			{
				addFrY = Main.tileFrame[typeCache];
				int num43 = x;
				if (tileFrameX % 36 != 0)
				{
					num43--;
				}
				addFrY += num43 % 6;
				if (addFrY >= 6)
				{
					addFrY -= 6;
				}
				addFrY *= 72;
			}
			else
			{
				addFrY = 0;
			}
			break;
		case 410:
			if (tileFrameY == 36)
			{
				tileHeight = 18;
			}
			if (tileFrameY >= 56)
			{
				addFrY = Main.tileFrame[typeCache];
				addFrY *= 56;
			}
			else
			{
				addFrY = 0;
			}
			break;
		case 480:
		case 509:
		case 657:
			tileTop = 2;
			if (tileFrameY >= 54)
			{
				addFrY = Main.tileFrame[typeCache];
				addFrY *= 54;
			}
			else
			{
				addFrY = 0;
			}
			break;
		case 658:
			tileTop = 2;
			switch (tileFrameY / 54)
			{
			default:
				addFrY = Main.tileFrame[typeCache];
				addFrY *= 54;
				break;
			case 1:
				addFrY = Main.tileFrame[typeCache];
				addFrY *= 54;
				addFrY += 486;
				break;
			case 2:
				addFrY = Main.tileFrame[typeCache];
				addFrY *= 54;
				addFrY += 972;
				break;
			}
			break;
		case 326:
		case 327:
		case 328:
		case 329:
		case 345:
		case 351:
		case 421:
		case 422:
		case 458:
		case 459:
			addFrY = Main.tileFrame[typeCache] * 90;
			break;
		case 541:
			addFrY = ((!_shouldShowInvisibleBlocks) ? 90 : 0);
			break;
		case 507:
		case 508:
		{
			int num = 20;
			int num12 = (Main.tileFrameCounter[typeCache] + x * 11 + y * 27) % (num * 8);
			addFrY = 90 * (num12 / num);
			break;
		}
		case 336:
		case 340:
		case 341:
		case 342:
		case 343:
		case 344:
			addFrY = Main.tileFrame[typeCache] * 90;
			tileTop = 2;
			break;
		case 89:
			tileTop = 2;
			break;
		case 102:
			tileTop = 2;
			break;
		}
		if (tileCache.halfBrick())
		{
			halfBrickHeight = 8;
		}
		switch (typeCache)
		{
		case 657:
			if (tileFrameY >= 54)
			{
				glowTexture = TextureAssets.GlowMask[330].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
				glowColor = Color.White;
			}
			break;
		case 656:
			glowTexture = TextureAssets.GlowMask[329].Value;
			glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
			glowColor = new Color(255, 255, 255, 0) * ((float)(int)Main.mouseTextColor / 255f);
			break;
		case 634:
			glowTexture = TextureAssets.GlowMask[315].Value;
			glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
			glowColor = Color.White;
			break;
		case 637:
			glowTexture = TextureAssets.Tile[637].Value;
			glowSourceRect = new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight);
			glowColor = Color.Lerp(Color.White, color, 0.75f);
			break;
		case 638:
			glowTexture = TextureAssets.GlowMask[327].Value;
			glowSourceRect = new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight);
			glowColor = Color.Lerp(Color.White, color, 0.75f);
			break;
		case 568:
			glowTexture = TextureAssets.GlowMask[268].Value;
			glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
			glowColor = Color.White;
			break;
		case 569:
			glowTexture = TextureAssets.GlowMask[269].Value;
			glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
			glowColor = Color.White;
			break;
		case 570:
			glowTexture = TextureAssets.GlowMask[270].Value;
			glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
			glowColor = Color.White;
			break;
		case 580:
			glowTexture = TextureAssets.GlowMask[289].Value;
			glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
			glowColor = new Color(225, 110, 110, 0);
			break;
		case 564:
			if (tileCache.frameX < 36)
			{
				glowTexture = TextureAssets.GlowMask[267].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
				glowColor = new Color(200, 200, 200, 0) * ((float)(int)Main.mouseTextColor / 255f);
			}
			addFrY = 0;
			break;
		case 184:
			if (tileCache.frameX == 110)
			{
				glowTexture = TextureAssets.GlowMask[127].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _lavaMossGlow;
			}
			if (tileCache.frameX == 132)
			{
				glowTexture = TextureAssets.GlowMask[127].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _kryptonMossGlow;
			}
			if (tileCache.frameX == 154)
			{
				glowTexture = TextureAssets.GlowMask[127].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _xenonMossGlow;
			}
			if (tileCache.frameX == 176)
			{
				glowTexture = TextureAssets.GlowMask[127].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _argonMossGlow;
			}
			if (tileCache.frameX == 198)
			{
				glowTexture = TextureAssets.GlowMask[127].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _violetMossGlow;
			}
			if (tileCache.frameX == 220)
			{
				glowTexture = TextureAssets.GlowMask[127].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
			}
			break;
		case 463:
			glowTexture = TextureAssets.GlowMask[243].Value;
			glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
			glowColor = new Color(127, 127, 127, 0);
			break;
		case 19:
		{
			int num73 = tileFrameY / 18;
			if (num73 == 26)
			{
				glowTexture = TextureAssets.GlowMask[65].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 18, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num73 == 27)
			{
				glowTexture = TextureAssets.GlowMask[112].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 18, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 90:
		{
			int num69 = tileFrameY / 36;
			if (num69 == 27)
			{
				glowTexture = TextureAssets.GlowMask[52].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 36, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num69 == 28)
			{
				glowTexture = TextureAssets.GlowMask[113].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 36, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 79:
		{
			int num66 = tileFrameY / 36;
			if (num66 == 27)
			{
				glowTexture = TextureAssets.GlowMask[53].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 36, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num66 == 28)
			{
				glowTexture = TextureAssets.GlowMask[114].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 36, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 89:
		{
			int num61 = tileFrameX / 54;
			int num48 = tileFrameX / 1998;
			addFrX -= 1998 * num48;
			addFrY += 36 * num48;
			if (num61 == 29)
			{
				glowTexture = TextureAssets.GlowMask[66].Value;
				glowSourceRect = new Rectangle(tileFrameX % 54, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num61 == 30)
			{
				glowTexture = TextureAssets.GlowMask[123].Value;
				glowSourceRect = new Rectangle(tileFrameX % 54, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 100:
			if (tileFrameX / 36 == 0 && tileFrameY / 36 == 27)
			{
				glowTexture = TextureAssets.GlowMask[68].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 36, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			break;
		case 33:
			if (tileFrameX / 18 == 0 && tileFrameY / 22 == 26)
			{
				glowTexture = TextureAssets.GlowMask[61].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 22, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			break;
		case 15:
		{
			int num71 = tileFrameY / 40;
			if (num71 == 32)
			{
				glowTexture = TextureAssets.GlowMask[54].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 40, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num71 == 33)
			{
				glowTexture = TextureAssets.GlowMask[116].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 40, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 34:
			if (tileFrameX / 54 == 0 && tileFrameY / 54 == 33)
			{
				glowTexture = TextureAssets.GlowMask[55].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 54, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			break;
		case 21:
		case 467:
		{
			int num65 = tileFrameX / 36;
			if (num65 == 48)
			{
				glowTexture = TextureAssets.GlowMask[56].Value;
				glowSourceRect = new Rectangle(tileFrameX % 36, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num65 == 49)
			{
				glowTexture = TextureAssets.GlowMask[117].Value;
				glowSourceRect = new Rectangle(tileFrameX % 36, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 441:
		case 468:
		{
			int num62 = tileFrameX / 36;
			if (num62 == 48)
			{
				glowTexture = TextureAssets.GlowMask[56].Value;
				glowSourceRect = new Rectangle(tileFrameX % 36, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num62 == 49)
			{
				glowTexture = TextureAssets.GlowMask[117].Value;
				glowSourceRect = new Rectangle(tileFrameX % 36, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 10:
			if (tileFrameY / 54 == 32)
			{
				glowTexture = TextureAssets.GlowMask[57].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 54, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			break;
		case 11:
		{
			int num74 = tileFrameY / 54;
			if (num74 == 32)
			{
				glowTexture = TextureAssets.GlowMask[58].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 54, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num74 == 33)
			{
				glowTexture = TextureAssets.GlowMask[119].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 54, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 88:
		{
			int num72 = tileFrameX / 54;
			int num54 = tileFrameX / 1998;
			addFrX -= 1998 * num54;
			addFrY += 36 * num54;
			if (num72 == 24)
			{
				glowTexture = TextureAssets.GlowMask[59].Value;
				glowSourceRect = new Rectangle(tileFrameX % 54, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num72 == 25)
			{
				glowTexture = TextureAssets.GlowMask[120].Value;
				glowSourceRect = new Rectangle(tileFrameX % 54, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 42:
			if (tileFrameY / 36 == 33)
			{
				glowTexture = TextureAssets.GlowMask[63].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 36, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			break;
		case 87:
		{
			int num70 = tileFrameX / 54;
			int num52 = tileFrameX / 1998;
			addFrX -= 1998 * num52;
			addFrY += 36 * num52;
			if (num70 == 26)
			{
				glowTexture = TextureAssets.GlowMask[64].Value;
				glowSourceRect = new Rectangle(tileFrameX % 54, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num70 == 27)
			{
				glowTexture = TextureAssets.GlowMask[121].Value;
				glowSourceRect = new Rectangle(tileFrameX % 54, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 14:
		{
			int num68 = tileFrameX / 54;
			if (num68 == 31)
			{
				glowTexture = TextureAssets.GlowMask[67].Value;
				glowSourceRect = new Rectangle(tileFrameX % 54, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num68 == 32)
			{
				glowTexture = TextureAssets.GlowMask[124].Value;
				glowSourceRect = new Rectangle(tileFrameX % 54, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 93:
		{
			int num67 = tileFrameY / 54;
			int num51 = tileFrameY / 1998;
			addFrY -= 1998 * num51;
			addFrX += 36 * num51;
			tileTop += 2;
			if (num67 == 27)
			{
				glowTexture = TextureAssets.GlowMask[62].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 54, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			break;
		}
		case 18:
		{
			int num64 = tileFrameX / 36;
			if (num64 == 27)
			{
				glowTexture = TextureAssets.GlowMask[69].Value;
				glowSourceRect = new Rectangle(tileFrameX % 36, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num64 == 28)
			{
				glowTexture = TextureAssets.GlowMask[125].Value;
				glowSourceRect = new Rectangle(tileFrameX % 36, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 101:
		{
			int num63 = tileFrameX / 54;
			int num50 = tileFrameX / 1998;
			addFrX -= 1998 * num50;
			addFrY += 72 * num50;
			if (num63 == 28)
			{
				glowTexture = TextureAssets.GlowMask[60].Value;
				glowSourceRect = new Rectangle(tileFrameX % 54, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num63 == 29)
			{
				glowTexture = TextureAssets.GlowMask[115].Value;
				glowSourceRect = new Rectangle(tileFrameX % 54, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 104:
		{
			int num60 = tileFrameX / 36;
			tileTop = 2;
			if (num60 == 24)
			{
				glowTexture = TextureAssets.GlowMask[51].Value;
				glowSourceRect = new Rectangle(tileFrameX % 36, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num60 == 25)
			{
				glowTexture = TextureAssets.GlowMask[118].Value;
				glowSourceRect = new Rectangle(tileFrameX % 36, (int)tileFrameY, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		case 172:
		{
			int num59 = tileFrameY / 38;
			if (num59 == 28)
			{
				glowTexture = TextureAssets.GlowMask[88].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 38, tileWidth, tileHeight);
				glowColor = _martianGlow;
			}
			if (num59 == 29)
			{
				glowTexture = TextureAssets.GlowMask[122].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, tileFrameY % 38, tileWidth, tileHeight);
				glowColor = _meteorGlow;
			}
			break;
		}
		}
		TileLoader.SetSpriteEffects(x, y, typeCache, ref tileSpriteEffect);
		TileLoader.SetDrawPositions(x, y, ref tileWidth, ref tileTop, ref tileHeight, ref tileFrameX, ref tileFrameY);
		TileLoader.SetAnimationFrame(typeCache, x, y, ref addFrX, ref addFrY);
	}

	private bool IsWindBlocked(int x, int y)
	{
		Tile tile = Main.tile[x, y];
		if (tile == null)
		{
			return true;
		}
		if (tile.wall > 0 && !WallID.Sets.AllowsWind[tile.wall])
		{
			return true;
		}
		if ((double)y > Main.worldSurface)
		{
			return true;
		}
		return false;
	}

	public static int GetWaterAnimalCageFrame(int x, int y, int tileFrameX, int tileFrameY)
	{
		int num3 = x - tileFrameX / 18;
		int num2 = y - tileFrameY / 18;
		return num3 / 2 * (num2 / 3) % Main.cageFrames;
	}

	public static int GetSmallAnimalCageFrame(int x, int y, int tileFrameX, int tileFrameY)
	{
		int num3 = x - tileFrameX / 18;
		int num2 = y - tileFrameY / 18;
		return num3 / 3 * (num2 / 3) % Main.cageFrames;
	}

	public static int GetBigAnimalCageFrame(int x, int y, int tileFrameX, int tileFrameY)
	{
		int num3 = x - tileFrameX / 18;
		int num2 = y - tileFrameY / 18;
		return num3 / 6 * (num2 / 4) % Main.cageFrames;
	}

	private void GetScreenDrawArea(Vector2 screenPosition, Vector2 offSet, out int firstTileX, out int lastTileX, out int firstTileY, out int lastTileY)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		firstTileX = (int)((screenPosition.X - offSet.X) / 16f - 1f);
		lastTileX = (int)((screenPosition.X + (float)Main.screenWidth + offSet.X) / 16f) + 2;
		firstTileY = (int)((screenPosition.Y - offSet.Y) / 16f - 1f);
		lastTileY = (int)((screenPosition.Y + (float)Main.screenHeight + offSet.Y) / 16f) + 5;
		if (firstTileX < 4)
		{
			firstTileX = 4;
		}
		if (lastTileX > Main.maxTilesX - 4)
		{
			lastTileX = Main.maxTilesX - 4;
		}
		if (firstTileY < 4)
		{
			firstTileY = 4;
		}
		if (lastTileY > Main.maxTilesY - 4)
		{
			lastTileY = Main.maxTilesY - 4;
		}
		if (Main.sectionManager.AnyUnfinishedSections)
		{
			TimeLogger.DetailedDrawReset();
			WorldGen.SectionTileFrameWithCheck(firstTileX, firstTileY, lastTileX, lastTileY);
			TimeLogger.DetailedDrawTime(5);
		}
		if (Main.sectionManager.AnyNeedRefresh)
		{
			WorldGen.RefreshSections(firstTileX, firstTileY, lastTileX, lastTileY);
		}
	}

	public void ClearCachedTileDraws(bool solidLayer)
	{
		if (solidLayer)
		{
			_displayDollTileEntityPositions.Clear();
			_hatRackTileEntityPositions.Clear();
		}
		else
		{
			_vineRootsPositions.Clear();
			_reverseVineRootsPositions.Clear();
		}
	}

	public void AddSpecialLegacyPoint(Point p)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		AddSpecialLegacyPoint(p.X, p.Y);
	}

	public void AddSpecialLegacyPoint(int x, int y)
	{
		if (_specialTilesCount < _specialTileX.Length)
		{
			_specialTileX[_specialTilesCount] = x;
			_specialTileY[_specialTilesCount] = y;
			_specialTilesCount++;
		}
	}

	private void ClearLegacyCachedDraws()
	{
		_chestPositions.Clear();
		_trainingDummyTileEntityPositions.Clear();
		_foodPlatterTileEntityPositions.Clear();
		_itemFrameTileEntityPositions.Clear();
		_weaponRackTileEntityPositions.Clear();
		_specialTilesCount = 0;
	}

	private Color DrawTiles_GetLightOverride(int j, int i, Tile tileCache, ushort typeCache, short tileFrameX, short tileFrameY, Color tileLight)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		if (tileCache.fullbrightBlock())
		{
			return Color.White;
		}
		switch (typeCache)
		{
		case 541:
		case 631:
			return Color.White;
		case 19:
			if (tileFrameY / 18 == 48)
			{
				return Color.White;
			}
			break;
		case 83:
		{
			int num = tileFrameX / 18;
			if (!IsAlchemyPlantHarvestable(num))
			{
				break;
			}
			if (num == 5)
			{
				((Color)(ref tileLight)).A = (byte)(Main.mouseTextColor / 2);
				((Color)(ref tileLight)).G = Main.mouseTextColor;
				((Color)(ref tileLight)).B = Main.mouseTextColor;
			}
			if (num == 6)
			{
				byte b4 = (byte)((Main.mouseTextColor + ((Color)(ref tileLight)).G * 2) / 3);
				byte b5 = (byte)((Main.mouseTextColor + ((Color)(ref tileLight)).B * 2) / 3);
				if (b4 > ((Color)(ref tileLight)).G)
				{
					((Color)(ref tileLight)).G = b4;
				}
				if (b5 > ((Color)(ref tileLight)).B)
				{
					((Color)(ref tileLight)).B = b5;
				}
			}
			break;
		}
		case 61:
			if (tileFrameX == 144)
			{
				byte b7 = (((Color)(ref tileLight)).B = (byte)(245f - (float)(int)Main.mouseTextColor * 1.5f));
				byte b2 = b7;
				b7 = (((Color)(ref tileLight)).G = b2);
				byte b3 = b7;
				b7 = (((Color)(ref tileLight)).R = b3);
				byte a = b7;
				((Color)(ref tileLight)).A = a;
			}
			break;
		}
		return tileLight;
	}

	private void DrawTiles_EmitParticles(int j, int i, Tile tileCache, ushort typeCache, short tileFrameX, short tileFrameY, Color tileLight)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Unknown result type (might be due to invalid IL or missing references)
		//IL_052c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_074f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0751: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_0695: Unknown result type (might be due to invalid IL or missing references)
		//IL_066b: Unknown result type (might be due to invalid IL or missing references)
		//IL_066d: Unknown result type (might be due to invalid IL or missing references)
		//IL_083e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Unknown result type (might be due to invalid IL or missing references)
		//IL_085f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0803: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f74: Unknown result type (might be due to invalid IL or missing references)
		//IL_099b: Unknown result type (might be due to invalid IL or missing references)
		//IL_099d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d77: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1402: Unknown result type (might be due to invalid IL or missing references)
		//IL_1408: Unknown result type (might be due to invalid IL or missing references)
		//IL_1338: Unknown result type (might be due to invalid IL or missing references)
		//IL_134e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1354: Unknown result type (might be due to invalid IL or missing references)
		//IL_1030: Unknown result type (might be due to invalid IL or missing references)
		//IL_1035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b19: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1389: Unknown result type (might be due to invalid IL or missing references)
		//IL_1393: Unknown result type (might be due to invalid IL or missing references)
		//IL_1398: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_1104: Unknown result type (might be due to invalid IL or missing references)
		//IL_110a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e92: Unknown result type (might be due to invalid IL or missing references)
		//IL_1465: Unknown result type (might be due to invalid IL or missing references)
		//IL_146f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1474: Unknown result type (might be due to invalid IL or missing references)
		//IL_113f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1149: Unknown result type (might be due to invalid IL or missing references)
		//IL_114e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1544: Unknown result type (might be due to invalid IL or missing references)
		//IL_154e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1553: Unknown result type (might be due to invalid IL or missing references)
		//IL_1726: Unknown result type (might be due to invalid IL or missing references)
		//IL_173e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1744: Unknown result type (might be due to invalid IL or missing references)
		//IL_127e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1268: Unknown result type (might be due to invalid IL or missing references)
		//IL_1245: Unknown result type (might be due to invalid IL or missing references)
		//IL_1232: Unknown result type (might be due to invalid IL or missing references)
		//IL_1780: Unknown result type (might be due to invalid IL or missing references)
		//IL_1798: Unknown result type (might be due to invalid IL or missing references)
		//IL_179e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1295: Unknown result type (might be due to invalid IL or missing references)
		//IL_129b: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12da: Unknown result type (might be due to invalid IL or missing references)
		//IL_12df: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_17e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_181c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1834: Unknown result type (might be due to invalid IL or missing references)
		//IL_183a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1660: Unknown result type (might be due to invalid IL or missing references)
		//IL_1678: Unknown result type (might be due to invalid IL or missing references)
		//IL_167e: Unknown result type (might be due to invalid IL or missing references)
		//IL_18de: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_18fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1873: Unknown result type (might be due to invalid IL or missing references)
		//IL_188b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1891: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_16bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_19cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1936: Unknown result type (might be due to invalid IL or missing references)
		//IL_194e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1954: Unknown result type (might be due to invalid IL or missing references)
		//IL_198e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1998: Unknown result type (might be due to invalid IL or missing references)
		//IL_199d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a27: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a95: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b71: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b88: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c00: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c17: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b02: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b20: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c95: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_20f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_20f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_211b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2135: Unknown result type (might be due to invalid IL or missing references)
		//IL_214a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2154: Unknown result type (might be due to invalid IL or missing references)
		//IL_2159: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dad: Unknown result type (might be due to invalid IL or missing references)
		//IL_1db2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eaa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ec4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1edc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ee6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eeb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2013: Unknown result type (might be due to invalid IL or missing references)
		//IL_202d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2045: Unknown result type (might be due to invalid IL or missing references)
		//IL_204f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2054: Unknown result type (might be due to invalid IL or missing references)
		bool num49 = IsVisible(tileCache);
		int leafFrequency = _leafFrequency;
		leafFrequency /= 4;
		if (typeCache == 244 && tileFrameX == 18 && tileFrameY == 18 && _rand.Next(2) == 0)
		{
			if (_rand.Next(500) == 0)
			{
				Gore.NewGore(new Vector2((float)(i * 16 + 8), (float)(j * 16 + 8)), default(Vector2), 415, (float)_rand.Next(51, 101) * 0.01f);
			}
			else if (_rand.Next(250) == 0)
			{
				Gore.NewGore(new Vector2((float)(i * 16 + 8), (float)(j * 16 + 8)), default(Vector2), 414, (float)_rand.Next(51, 101) * 0.01f);
			}
			else if (_rand.Next(80) == 0)
			{
				Gore.NewGore(new Vector2((float)(i * 16 + 8), (float)(j * 16 + 8)), default(Vector2), 413, (float)_rand.Next(51, 101) * 0.01f);
			}
			else if (_rand.Next(10) == 0)
			{
				Gore.NewGore(new Vector2((float)(i * 16 + 8), (float)(j * 16 + 8)), default(Vector2), 412, (float)_rand.Next(51, 101) * 0.01f);
			}
			else if (_rand.Next(3) == 0)
			{
				Gore.NewGore(new Vector2((float)(i * 16 + 8), (float)(j * 16 + 8)), default(Vector2), 411, (float)_rand.Next(51, 101) * 0.01f);
			}
		}
		if (typeCache == 565 && tileFrameX == 0 && tileFrameY == 18 && _rand.Next(3) == 0 && ((Main.drawToScreen && _rand.Next(4) == 0) || !Main.drawToScreen))
		{
			Vector2 val = Utils.ToWorldCoordinates(new Point(i, j), 8f, 8f);
			int type = 1202;
			float scale = 8f + Main.rand.NextFloat() * 1.6f;
			Vector2 position5 = val + new Vector2(0f, -18f);
			Vector2 velocity = Main.rand.NextVector2Circular(0.7f, 0.25f) * 0.4f + Main.rand.NextVector2CircularEdge(1f, 0.4f) * 0.1f;
			velocity *= 4f;
			Gore.NewGorePerfect(position5, velocity, type, scale);
		}
		if (typeCache == 215 && tileFrameY < 36 && _rand.Next(3) == 0 && ((Main.drawToScreen && _rand.Next(4) == 0) || !Main.drawToScreen) && tileFrameY == 0)
		{
			int num20 = Dust.NewDust(new Vector2((float)(i * 16 + 2), (float)(j * 16 - 4)), 4, 8, 31, 0f, 0f, 100);
			if (tileFrameX == 0)
			{
				_dust[num20].position.X += _rand.Next(8);
			}
			if (tileFrameX == 36)
			{
				_dust[num20].position.X -= _rand.Next(8);
			}
			_dust[num20].alpha += _rand.Next(100);
			Dust obj = _dust[num20];
			obj.velocity *= 0.2f;
			_dust[num20].velocity.Y -= 0.5f + (float)_rand.Next(10) * 0.1f;
			_dust[num20].fadeIn = 0.5f + (float)_rand.Next(10) * 0.1f;
		}
		if (typeCache == 592 && tileFrameY == 18 && _rand.Next(3) == 0)
		{
			if ((Main.drawToScreen && _rand.Next(6) == 0) || !Main.drawToScreen)
			{
				int num31 = Dust.NewDust(new Vector2((float)(i * 16 + 2), (float)(j * 16 + 4)), 4, 8, 31, 0f, 0f, 100);
				if (tileFrameX == 0)
				{
					_dust[num31].position.X += _rand.Next(8);
				}
				if (tileFrameX == 36)
				{
					_dust[num31].position.X -= _rand.Next(8);
				}
				_dust[num31].alpha += _rand.Next(100);
				Dust obj2 = _dust[num31];
				obj2.velocity *= 0.2f;
				_dust[num31].velocity.Y -= 0.5f + (float)_rand.Next(10) * 0.1f;
				_dust[num31].fadeIn = 0.5f + (float)_rand.Next(10) * 0.1f;
			}
		}
		else if (typeCache == 406 && tileFrameY == 54 && tileFrameX == 0 && _rand.Next(3) == 0)
		{
			Vector2 position2 = default(Vector2);
			((Vector2)(ref position2))._002Ector((float)(i * 16 + 16), (float)(j * 16 + 8));
			Vector2 velocity2 = default(Vector2);
			((Vector2)(ref velocity2))._002Ector(0f, 0f);
			if (Main.WindForVisuals < 0f)
			{
				velocity2.X = 0f - Main.WindForVisuals;
			}
			int type2 = _rand.Next(825, 828);
			if (_rand.Next(4) == 0)
			{
				Gore.NewGore(position2, velocity2, type2, _rand.NextFloat() * 0.2f + 0.2f);
			}
			else if (_rand.Next(2) == 0)
			{
				Gore.NewGore(position2, velocity2, type2, _rand.NextFloat() * 0.3f + 0.3f);
			}
			else
			{
				Gore.NewGore(position2, velocity2, type2, _rand.NextFloat() * 0.4f + 0.4f);
			}
		}
		else if (typeCache == 452 && tileFrameY == 0 && tileFrameX == 0 && _rand.Next(3) == 0)
		{
			Vector2 position3 = default(Vector2);
			((Vector2)(ref position3))._002Ector((float)(i * 16 + 16), (float)(j * 16 + 8));
			Vector2 velocity3 = default(Vector2);
			((Vector2)(ref velocity3))._002Ector(0f, 0f);
			if (Main.WindForVisuals < 0f)
			{
				velocity3.X = 0f - Main.WindForVisuals;
			}
			int num42 = Main.tileFrame[typeCache];
			int type3 = 907 + num42 / 5;
			if (_rand.Next(2) == 0)
			{
				Gore.NewGore(position3, velocity3, type3, _rand.NextFloat() * 0.4f + 0.4f);
			}
		}
		if (typeCache == 192 && _rand.Next(leafFrequency) == 0)
		{
			EmitLivingTreeLeaf(i, j, 910);
		}
		if (typeCache == 384 && _rand.Next(leafFrequency) == 0)
		{
			EmitLivingTreeLeaf(i, j, 914);
		}
		if (typeCache == 666 && _rand.Next(100) == 0 && j - 1 > 0 && !WorldGen.ActiveAndWalkableTile(i, j - 1))
		{
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.PooFly, new ParticleOrchestraSettings
			{
				PositionInWorld = new Vector2((float)(i * 16 + 8), (float)(j * 16 - 8))
			});
		}
		if (!num49)
		{
			return;
		}
		if (typeCache == 238 && _rand.Next(10) == 0)
		{
			int num44 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 168);
			_dust[num44].noGravity = true;
			_dust[num44].alpha = 200;
		}
		if (typeCache == 139)
		{
			if (tileCache.frameX == 36 && tileCache.frameY % 36 == 0 && (int)Main.timeForVisualEffects % 7 == 0 && _rand.Next(3) == 0)
			{
				int num45 = _rand.Next(570, 573);
				Vector2 position4 = default(Vector2);
				((Vector2)(ref position4))._002Ector((float)(i * 16 + 8), (float)(j * 16 - 8));
				Vector2 velocity4 = default(Vector2);
				((Vector2)(ref velocity4))._002Ector(Main.WindForVisuals * 2f, -0.5f);
				velocity4.X *= 1f + (float)_rand.Next(-50, 51) * 0.01f;
				velocity4.Y *= 1f + (float)_rand.Next(-50, 51) * 0.01f;
				if (num45 == 572)
				{
					position4.X -= 8f;
				}
				if (num45 == 571)
				{
					position4.X -= 4f;
				}
				Gore.NewGore(position4, velocity4, num45, 0.8f);
			}
		}
		else if (typeCache == 463)
		{
			if (tileFrameY == 54 && tileFrameX == 0)
			{
				for (int k = 0; k < 4; k++)
				{
					if (_rand.Next(2) != 0)
					{
						Dust dust = Dust.NewDustDirect(new Vector2((float)(i * 16 + 4), (float)(j * 16)), 36, 8, 16);
						dust.noGravity = true;
						dust.alpha = 140;
						dust.fadeIn = 1.2f;
						dust.velocity = Vector2.Zero;
					}
				}
			}
			if (tileFrameY == 18 && (tileFrameX == 0 || tileFrameX == 36))
			{
				for (int l = 0; l < 1; l++)
				{
					if (_rand.Next(13) == 0)
					{
						Dust dust2 = Dust.NewDustDirect(new Vector2((float)(i * 16), (float)(j * 16)), 8, 8, 274);
						dust2.position = new Vector2((float)(i * 16 + 8), (float)(j * 16 + 8));
						dust2.position.X += ((tileFrameX == 36) ? 4 : (-4));
						dust2.noGravity = true;
						dust2.alpha = 128;
						dust2.fadeIn = 1.2f;
						dust2.noLight = true;
						dust2.velocity = new Vector2(0f, _rand.NextFloatDirection() * 1.2f);
					}
				}
			}
		}
		else if (typeCache == 497)
		{
			if (tileCache.frameY / 40 == 31 && tileCache.frameY % 40 == 0)
			{
				for (int m = 0; m < 1; m++)
				{
					if (_rand.Next(10) == 0)
					{
						Dust dust3 = Dust.NewDustDirect(new Vector2((float)(i * 16), (float)(j * 16 + 8)), 16, 12, 43);
						dust3.noGravity = true;
						dust3.alpha = 254;
						dust3.color = Color.White;
						dust3.scale = 0.7f;
						dust3.velocity = Vector2.Zero;
						dust3.noLight = true;
					}
				}
			}
		}
		else if (typeCache == 165 && tileFrameX >= 162 && tileFrameX <= 214 && tileFrameY == 72)
		{
			if (_rand.Next(60) == 0)
			{
				int num46 = Dust.NewDust(new Vector2((float)(i * 16 + 2), (float)(j * 16 + 6)), 8, 4, 153);
				_dust[num46].scale -= (float)_rand.Next(3) * 0.1f;
				_dust[num46].velocity.Y = 0f;
				_dust[num46].velocity.X *= 0.05f;
				_dust[num46].alpha = 100;
			}
		}
		else if (typeCache == 42 && tileFrameX == 0)
		{
			int num47 = tileFrameY / 36;
			int num48 = tileFrameY / 18 % 2;
			if (num47 == 7 && num48 == 1)
			{
				if (_rand.Next(50) == 0)
				{
					int num10 = Dust.NewDust(new Vector2((float)(i * 16 + 4), (float)(j * 16 + 4)), 8, 8, 58, 0f, 0f, 150);
					Dust obj3 = _dust[num10];
					obj3.velocity *= 0.5f;
				}
				if (_rand.Next(100) == 0)
				{
					int num11 = Gore.NewGore(new Vector2((float)(i * 16 - 2), (float)(j * 16 - 4)), default(Vector2), _rand.Next(16, 18));
					_gore[num11].scale *= 0.7f;
					Gore obj4 = _gore[num11];
					obj4.velocity *= 0.25f;
				}
			}
			else if (num47 == 29 && num48 == 1 && _rand.Next(40) == 0)
			{
				int num12 = Dust.NewDust(new Vector2((float)(i * 16 + 4), (float)(j * 16)), 8, 8, 59, 0f, 0f, 100);
				if (_rand.Next(3) != 0)
				{
					_dust[num12].noGravity = true;
				}
				Dust obj5 = _dust[num12];
				obj5.velocity *= 0.3f;
				_dust[num12].velocity.Y -= 1.5f;
			}
		}
		if (typeCache == 4 && _rand.Next(40) == 0 && tileFrameX < 66)
		{
			int num13 = (int)MathHelper.Clamp((float)(tileCache.frameY / 22), 0f, (float)(TorchID.Count - 1));
			int num14 = TorchID.Dust[num13];
			int num15 = 0;
			num15 = tileFrameX switch
			{
				22 => Dust.NewDust(new Vector2((float)(i * 16 + 6), (float)(j * 16)), 4, 4, num14, 0f, 0f, 100), 
				44 => Dust.NewDust(new Vector2((float)(i * 16 + 2), (float)(j * 16)), 4, 4, num14, 0f, 0f, 100), 
				_ => Dust.NewDust(new Vector2((float)(i * 16 + 4), (float)(j * 16)), 4, 4, num14, 0f, 0f, 100), 
			};
			if (_rand.Next(3) != 0)
			{
				_dust[num15].noGravity = true;
			}
			Dust obj6 = _dust[num15];
			obj6.velocity *= 0.3f;
			_dust[num15].velocity.Y -= 1.5f;
			if (num14 == 66)
			{
				_dust[num15].color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
				_dust[num15].noGravity = true;
			}
		}
		if (typeCache == 93 && _rand.Next(40) == 0 && tileFrameX == 0)
		{
			int num16 = tileFrameY / 54;
			if (tileFrameY / 18 % 3 == 0)
			{
				int num17;
				switch (num16)
				{
				case 0:
				case 6:
				case 7:
				case 8:
				case 10:
				case 14:
				case 15:
				case 16:
					num17 = 6;
					break;
				case 20:
					num17 = 59;
					break;
				default:
					num17 = -1;
					break;
				}
				if (num17 != -1)
				{
					int num18 = Dust.NewDust(new Vector2((float)(i * 16 + 4), (float)(j * 16 + 2)), 4, 4, num17, 0f, 0f, 100);
					if (_rand.Next(3) != 0)
					{
						_dust[num18].noGravity = true;
					}
					Dust obj7 = _dust[num18];
					obj7.velocity *= 0.3f;
					_dust[num18].velocity.Y -= 1.5f;
				}
			}
		}
		if (typeCache == 100 && _rand.Next(40) == 0 && tileFrameX < 36)
		{
			int num19 = tileFrameY / 36;
			if (tileFrameY / 18 % 2 == 0)
			{
				int num21;
				switch (num19)
				{
				case 0:
				case 5:
				case 7:
				case 8:
				case 10:
				case 12:
				case 14:
				case 15:
				case 16:
					num21 = 6;
					break;
				case 20:
					num21 = 59;
					break;
				default:
					num21 = -1;
					break;
				}
				if (num21 != -1)
				{
					int num22 = 0;
					num22 = Dust.NewDust((tileFrameX != 0) ? ((_rand.Next(3) != 0) ? new Vector2((float)(i * 16), (float)(j * 16 + 2)) : new Vector2((float)(i * 16 + 6), (float)(j * 16 + 2))) : ((_rand.Next(3) != 0) ? new Vector2((float)(i * 16 + 14), (float)(j * 16 + 2)) : new Vector2((float)(i * 16 + 4), (float)(j * 16 + 2))), 4, 4, num21, 0f, 0f, 100);
					if (_rand.Next(3) != 0)
					{
						_dust[num22].noGravity = true;
					}
					Dust obj8 = _dust[num22];
					obj8.velocity *= 0.3f;
					_dust[num22].velocity.Y -= 1.5f;
				}
			}
		}
		if (typeCache == 98 && _rand.Next(40) == 0 && tileFrameY == 0 && tileFrameX == 0)
		{
			int num23 = Dust.NewDust(new Vector2((float)(i * 16 + 12), (float)(j * 16 + 2)), 4, 4, 6, 0f, 0f, 100);
			if (_rand.Next(3) != 0)
			{
				_dust[num23].noGravity = true;
			}
			Dust obj9 = _dust[num23];
			obj9.velocity *= 0.3f;
			_dust[num23].velocity.Y -= 1.5f;
		}
		if (typeCache == 49 && tileFrameX == 0 && _rand.Next(2) == 0)
		{
			int num24 = Dust.NewDust(new Vector2((float)(i * 16 + 4), (float)(j * 16 - 4)), 4, 4, 172, 0f, 0f, 100);
			if (_rand.Next(3) == 0)
			{
				_dust[num24].scale = 0.5f;
			}
			else
			{
				_dust[num24].scale = 0.9f;
				_dust[num24].noGravity = true;
			}
			Dust obj10 = _dust[num24];
			obj10.velocity *= 0.3f;
			_dust[num24].velocity.Y -= 1.5f;
		}
		if (typeCache == 372 && tileFrameX == 0 && _rand.Next(2) == 0)
		{
			int num25 = Dust.NewDust(new Vector2((float)(i * 16 + 4), (float)(j * 16 - 4)), 4, 4, 242, 0f, 0f, 100);
			if (_rand.Next(3) == 0)
			{
				_dust[num25].scale = 0.5f;
			}
			else
			{
				_dust[num25].scale = 0.9f;
				_dust[num25].noGravity = true;
			}
			Dust obj11 = _dust[num25];
			obj11.velocity *= 0.3f;
			_dust[num25].velocity.Y -= 1.5f;
		}
		if (typeCache == 646 && tileFrameX == 0)
		{
			_rand.Next(2);
		}
		if (typeCache == 34 && _rand.Next(40) == 0 && tileFrameX < 54)
		{
			int num26 = tileFrameY / 54;
			int num27 = tileFrameX / 18 % 3;
			if (tileFrameY / 18 % 3 == 1 && num27 != 1)
			{
				int num28;
				switch (num26)
				{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 12:
				case 13:
				case 16:
				case 19:
				case 21:
					num28 = 6;
					break;
				case 25:
					num28 = 59;
					break;
				default:
					num28 = -1;
					break;
				}
				if (num28 != -1)
				{
					int num29 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16 + 2)), 14, 6, num28, 0f, 0f, 100);
					if (_rand.Next(3) != 0)
					{
						_dust[num29].noGravity = true;
					}
					Dust obj12 = _dust[num29];
					obj12.velocity *= 0.3f;
					_dust[num29].velocity.Y -= 1.5f;
				}
			}
		}
		if (typeCache == 83)
		{
			int style = tileFrameX / 18;
			if (IsAlchemyPlantHarvestable(style))
			{
				EmitAlchemyHerbParticles(j, i, style);
			}
		}
		if (typeCache == 22 && _rand.Next(400) == 0)
		{
			Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 14);
		}
		else if ((typeCache == 23 || typeCache == 24 || typeCache == 32) && _rand.Next(500) == 0)
		{
			Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 14);
		}
		else if (typeCache == 25 && _rand.Next(700) == 0)
		{
			Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 14);
		}
		else if (typeCache == 112 && _rand.Next(700) == 0)
		{
			Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 14);
		}
		else if (typeCache == 31 && _rand.Next(20) == 0)
		{
			if (tileFrameX >= 36)
			{
				int num30 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 5, 0f, 0f, 100);
				_dust[num30].velocity.Y = 0f;
				_dust[num30].velocity.X *= 0.3f;
			}
			else
			{
				Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 14, 0f, 0f, 100);
			}
		}
		else if (typeCache == 26 && _rand.Next(20) == 0)
		{
			if (tileFrameX >= 54)
			{
				int num32 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 5, 0f, 0f, 100);
				_dust[num32].scale = 1.5f;
				_dust[num32].noGravity = true;
				Dust obj13 = _dust[num32];
				obj13.velocity *= 0.75f;
			}
			else
			{
				Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 14, 0f, 0f, 100);
			}
		}
		else if ((typeCache == 71 || typeCache == 72) && tileCache.color() == 0 && _rand.Next(500) == 0)
		{
			Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 41, 0f, 0f, 250, default(Color), 0.8f);
		}
		else if ((typeCache == 17 || typeCache == 77 || typeCache == 133) && _rand.Next(40) == 0)
		{
			if (tileFrameX == 18 && tileFrameY == 18)
			{
				int num33 = Dust.NewDust(new Vector2((float)(i * 16 - 4), (float)(j * 16 - 6)), 8, 6, 6, 0f, 0f, 100);
				if (_rand.Next(3) != 0)
				{
					_dust[num33].noGravity = true;
				}
			}
		}
		else if (typeCache == 405 && _rand.Next(20) == 0)
		{
			if (tileFrameX == 18 && tileFrameY == 18)
			{
				int num34 = Dust.NewDust(new Vector2((float)(i * 16 - 4), (float)(j * 16 - 6)), 24, 10, 6, 0f, 0f, 100);
				if (_rand.Next(5) != 0)
				{
					_dust[num34].noGravity = true;
				}
			}
		}
		else if (typeCache == 37 && _rand.Next(250) == 0)
		{
			int num35 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 6, 0f, 0f, 0, default(Color), _rand.Next(3));
			if (_dust[num35].scale > 1f)
			{
				_dust[num35].noGravity = true;
			}
		}
		else if ((typeCache == 58 || typeCache == 76 || typeCache == 684) && _rand.Next(250) == 0)
		{
			int num36 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 6, 0f, 0f, 0, default(Color), _rand.Next(3));
			if (_dust[num36].scale > 1f)
			{
				_dust[num36].noGravity = true;
			}
			_dust[num36].noLight = true;
		}
		else if (typeCache == 61)
		{
			if (tileFrameX == 144 && _rand.Next(60) == 0)
			{
				int num37 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 44, 0f, 0f, 250, default(Color), 0.4f);
				_dust[num37].fadeIn = 0.7f;
			}
		}
		else if (Main.tileShine[typeCache] > 0)
		{
			if (((Color)(ref tileLight)).R <= 20 && ((Color)(ref tileLight)).B <= 20 && ((Color)(ref tileLight)).G <= 20)
			{
				return;
			}
			int num38 = ((Color)(ref tileLight)).R;
			if (((Color)(ref tileLight)).G > num38)
			{
				num38 = ((Color)(ref tileLight)).G;
			}
			if (((Color)(ref tileLight)).B > num38)
			{
				num38 = ((Color)(ref tileLight)).B;
			}
			num38 /= 30;
			if (_rand.Next(Main.tileShine[typeCache]) >= num38 || ((typeCache == 21 || typeCache == 441) && (tileFrameX < 36 || tileFrameX >= 180) && (tileFrameX < 396 || tileFrameX > 409)) || ((typeCache == 467 || typeCache == 468) && (tileFrameX < 144 || tileFrameX >= 180)))
			{
				return;
			}
			Color newColor = Color.White;
			switch (typeCache)
			{
			case 178:
			{
				switch (tileFrameX / 18)
				{
				case 0:
					((Color)(ref newColor))._002Ector(255, 0, 255, 255);
					break;
				case 1:
					((Color)(ref newColor))._002Ector(255, 255, 0, 255);
					break;
				case 2:
					((Color)(ref newColor))._002Ector(0, 0, 255, 255);
					break;
				case 3:
					((Color)(ref newColor))._002Ector(0, 255, 0, 255);
					break;
				case 4:
					((Color)(ref newColor))._002Ector(255, 0, 0, 255);
					break;
				case 5:
					((Color)(ref newColor))._002Ector(255, 255, 255, 255);
					break;
				case 6:
					((Color)(ref newColor))._002Ector(255, 255, 0, 255);
					break;
				}
				int num39 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 43, 0f, 0f, 254, newColor, 0.5f);
				Dust obj14 = _dust[num39];
				obj14.velocity *= 0f;
				return;
			}
			case 63:
				((Color)(ref newColor))._002Ector(0, 0, 255, 255);
				break;
			}
			if (typeCache == 64)
			{
				((Color)(ref newColor))._002Ector(255, 0, 0, 255);
			}
			if (typeCache == 65)
			{
				((Color)(ref newColor))._002Ector(0, 255, 0, 255);
			}
			if (typeCache == 66)
			{
				((Color)(ref newColor))._002Ector(255, 255, 0, 255);
			}
			if (typeCache == 67)
			{
				((Color)(ref newColor))._002Ector(255, 0, 255, 255);
			}
			if (typeCache == 68)
			{
				((Color)(ref newColor))._002Ector(255, 255, 255, 255);
			}
			if (typeCache == 12 || typeCache == 665)
			{
				((Color)(ref newColor))._002Ector(255, 0, 0, 255);
			}
			if (typeCache == 639)
			{
				((Color)(ref newColor))._002Ector(0, 0, 255, 255);
			}
			if (typeCache == 204)
			{
				((Color)(ref newColor))._002Ector(255, 0, 0, 255);
			}
			if (typeCache == 211)
			{
				((Color)(ref newColor))._002Ector(50, 255, 100, 255);
			}
			int num40 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 43, 0f, 0f, 254, newColor, 0.5f);
			Dust obj15 = _dust[num40];
			obj15.velocity *= 0f;
		}
		else if (Main.tileSolid[tileCache.type] && Main.shimmerAlpha > 0f && (((Color)(ref tileLight)).R > 20 || ((Color)(ref tileLight)).B > 20 || ((Color)(ref tileLight)).G > 20))
		{
			int num41 = ((Color)(ref tileLight)).R;
			if (((Color)(ref tileLight)).G > num41)
			{
				num41 = ((Color)(ref tileLight)).G;
			}
			if (((Color)(ref tileLight)).B > num41)
			{
				num41 = ((Color)(ref tileLight)).B;
			}
			int maxValue = 500;
			if ((float)_rand.Next(maxValue) < 2f * Main.shimmerAlpha)
			{
				Color white = Color.White;
				float scale2 = ((float)num41 / 255f + 1f) / 2f;
				int num43 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 43, 0f, 0f, 254, white, scale2);
				Dust obj16 = _dust[num43];
				obj16.velocity *= 0f;
			}
		}
	}

	/// <summary>
	/// Emits a single living tree leaf or other gore instance directly below the target tile.<br />
	/// With a 50% chance, also emits a second leaf or other gore instance directly to the side of the target tile, dependent on wind direction.<br />
	/// Used by vanilla's two types of Living Trees, from which this method and its two submethods get their collective name.<br />
	/// </summary>
	/// <param name="i">The X coordinate of the target tile.</param>
	/// <param name="j">The Y coordinate of the target tile.</param>
	/// <param name="leafGoreType">The numerical ID of the leaf or other gore instance that should be spawned.</param>
	public void EmitLivingTreeLeaf(int i, int j, int leafGoreType)
	{
		EmitLivingTreeLeaf_Below(i, j, leafGoreType);
		if (_rand.Next(2) == 0)
		{
			EmitLivingTreeLeaf_Sideways(i, j, leafGoreType);
		}
	}

	/// <summary>
	/// Emits a single living tree leaf or other gore instance directly below the target tile.<br />
	/// </summary>
	/// <param name="x">The X coordinate of the target tile.</param>
	/// <param name="y">The Y coordinate of the target tile.</param>
	/// <param name="leafGoreType">The numerical ID of the leaf or other gore instance that should be spawned.</param>
	public void EmitLivingTreeLeaf_Below(int x, int y, int leafGoreType)
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		Tile tile = Main.tile[x, y + 1];
		if (!WorldGen.SolidTile(tile) && tile.liquid <= 0)
		{
			float windForVisuals = Main.WindForVisuals;
			if ((!(windForVisuals < -0.2f) || (!WorldGen.SolidTile(Main.tile[x - 1, y + 1]) && !WorldGen.SolidTile(Main.tile[x - 2, y + 1]))) && (!(windForVisuals > 0.2f) || (!WorldGen.SolidTile(Main.tile[x + 1, y + 1]) && !WorldGen.SolidTile(Main.tile[x + 2, y + 1]))))
			{
				Gore.NewGorePerfect(new Vector2((float)(x * 16), (float)(y * 16 + 16)), Vector2.Zero, leafGoreType).Frame.CurrentColumn = Main.tile[x, y].color();
			}
		}
	}

	/// <summary>
	/// Emits a single living tree leaf or other gore instance directly to the side of the target tile, dependent on wind direction.<br />
	/// </summary>
	/// <param name="x">The X coordinate of the target tile.</param>
	/// <param name="y">The Y coordinate of the target tile.</param>
	/// <param name="leafGoreType">The numerical ID of the leaf or other gore instance that should be spawned.</param>
	public void EmitLivingTreeLeaf_Sideways(int x, int y, int leafGoreType)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		if (Main.WindForVisuals > 0.2f)
		{
			num = 1;
		}
		else if (Main.WindForVisuals < -0.2f)
		{
			num = -1;
		}
		Tile tile = Main.tile[x + num, y];
		if (!WorldGen.SolidTile(tile) && tile.liquid <= 0)
		{
			int num2 = 0;
			if (num == -1)
			{
				num2 = -10;
			}
			Gore.NewGorePerfect(new Vector2((float)(x * 16 + 8 + 4 * num + num2), (float)(y * 16 + 8)), Vector2.Zero, leafGoreType).Frame.CurrentColumn = Main.tile[x, y].color();
		}
	}

	private void EmitLiquidDrops(int j, int i, Tile tileCache, ushort typeCache)
	{
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		int num = 60;
		switch (typeCache)
		{
		case 374:
			num = 120;
			break;
		case 375:
			num = 180;
			break;
		case 461:
			num = 180;
			break;
		}
		if (tileCache.liquid != 0 || _rand.Next(num * 2) != 0)
		{
			return;
		}
		Rectangle rectangle = default(Rectangle);
		((Rectangle)(ref rectangle))._002Ector(i * 16, j * 16, 16, 16);
		rectangle.X -= 34;
		rectangle.Width += 68;
		rectangle.Y -= 100;
		rectangle.Height = 400;
		Rectangle value = default(Rectangle);
		for (int k = 0; k < 600; k++)
		{
			if (_gore[k].active && ((_gore[k].type >= 706 && _gore[k].type <= 717) || _gore[k].type == 943 || _gore[k].type == 1147 || (_gore[k].type >= 1160 && _gore[k].type <= 1162)))
			{
				((Rectangle)(ref value))._002Ector((int)_gore[k].position.X, (int)_gore[k].position.Y, 16, 16);
				if (((Rectangle)(ref rectangle)).Intersects(value))
				{
					return;
				}
			}
		}
		Vector2 position = new Vector2((float)(i * 16), (float)(j * 16));
		int type = 706;
		if (Main.waterStyle == 14)
		{
			type = 706;
		}
		else if (Main.waterStyle == 13)
		{
			type = 706;
		}
		else if (Main.waterStyle == 12)
		{
			type = 1147;
		}
		else if (Main.waterStyle >= 15)
		{
			type = LoaderManager.Get<WaterStylesLoader>().Get(Main.waterStyle).GetDropletGore();
		}
		else if (Main.waterStyle > 1)
		{
			type = 706 + Main.waterStyle - 1;
		}
		if (typeCache == 374)
		{
			type = 716;
		}
		if (typeCache == 375)
		{
			type = 717;
		}
		if (typeCache == 461)
		{
			type = 943;
			if (Main.player[Main.myPlayer].ZoneCorrupt)
			{
				type = 1160;
			}
			if (Main.player[Main.myPlayer].ZoneCrimson)
			{
				type = 1161;
			}
			if (Main.player[Main.myPlayer].ZoneHallow)
			{
				type = 1162;
			}
		}
		int num2 = Gore.NewGore(position, default(Vector2), type);
		Gore obj = _gore[num2];
		obj.velocity *= 0f;
	}

	/// <summary>
	/// Fetches the degree to which wind would/should affect a tile at the given location.
	/// </summary>
	/// <param name="x">The X coordinate of the theoretical target tile.</param>
	/// <param name="y">The Y coordinate of the theoretical target tile.</param>
	/// <param name="windCounter"></param>
	/// <returns>
	/// If <see cref="F:Terraria.Main.SettingsEnabled_TilesSwayInWind" /> is false or the tile is below surface level, 0.<br />
	/// Otherwise, returns a value from 0.08f to 0.18f.
	/// </returns>
	public float GetWindCycle(int x, int y, double windCounter)
	{
		if (!Main.SettingsEnabled_TilesSwayInWind)
		{
			return 0f;
		}
		float num = (float)x * 0.5f + (float)(y / 100) * 0.5f;
		float num2 = (float)Math.Cos(windCounter * 6.2831854820251465 + (double)num) * 0.5f;
		if (Main.remixWorld)
		{
			if (!((double)y > Main.worldSurface))
			{
				return 0f;
			}
			num2 += Main.WindForVisuals;
		}
		else
		{
			if (!((double)y < Main.worldSurface))
			{
				return 0f;
			}
			num2 += Main.WindForVisuals;
		}
		float lerpValue = Utils.GetLerpValue(0.08f, 0.18f, Math.Abs(Main.WindForVisuals), clamped: true);
		return num2 * lerpValue;
	}

	/// <summary>
	/// Determines whether or not the tile at the given location should be able to sway in the wind.
	/// </summary>
	/// <param name="x">The X coordinate of the given tile.</param>
	/// <param name="y">The Y coordinate of the given tile.</param>
	/// <param name="tileCache">The tile to determine the sway-in-wind-ability of.</param>
	/// <returns>
	/// False if something dictates that the tile should NOT be able to sway in the wind; returns true by default.<br />
	/// Vanilla conditions that prevent wind sway are, in this order:<br />
	/// - if <see cref="F:Terraria.Main.SettingsEnabled_TilesSwayInWind" /> is false<br />
	/// - if <see cref="F:Terraria.ID.TileID.Sets.SwaysInWindBasic" /> is false for the tile type of <paramref name="tileCache" /><br />
	/// - if the tile is an Orange Bloodroot
	/// - if the tile is a Pink Prickly Pear on any vanilla cactus variant
	/// </returns>
	public bool ShouldSwayInWind(int x, int y, Tile tileCache)
	{
		if (!Main.SettingsEnabled_TilesSwayInWind)
		{
			return false;
		}
		if (!TileID.Sets.SwaysInWindBasic[tileCache.type])
		{
			return false;
		}
		if (tileCache.type == 227 && (tileCache.frameX == 204 || tileCache.frameX == 238 || tileCache.frameX == 408 || tileCache.frameX == 442 || tileCache.frameX == 476))
		{
			return false;
		}
		return true;
	}

	private void UpdateLeafFrequency()
	{
		float num = Math.Abs(Main.WindForVisuals);
		if (num <= 0.1f)
		{
			_leafFrequency = 2000;
		}
		else if (num <= 0.2f)
		{
			_leafFrequency = 1000;
		}
		else if (num <= 0.3f)
		{
			_leafFrequency = 450;
		}
		else if (num <= 0.4f)
		{
			_leafFrequency = 300;
		}
		else if (num <= 0.5f)
		{
			_leafFrequency = 200;
		}
		else if (num <= 0.6f)
		{
			_leafFrequency = 130;
		}
		else if (num <= 0.7f)
		{
			_leafFrequency = 75;
		}
		else if (num <= 0.8f)
		{
			_leafFrequency = 50;
		}
		else if (num <= 0.9f)
		{
			_leafFrequency = 40;
		}
		else if (num <= 1f)
		{
			_leafFrequency = 30;
		}
		else if (num <= 1.1f)
		{
			_leafFrequency = 20;
		}
		else
		{
			_leafFrequency = 10;
		}
		_leafFrequency *= 7;
	}

	private void EnsureWindGridSize()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
		Vector2 offSet = default(Vector2);
		((Vector2)(ref offSet))._002Ector((float)Main.offScreenRange, (float)Main.offScreenRange);
		if (Main.drawToScreen)
		{
			offSet = Vector2.Zero;
		}
		GetScreenDrawArea(unscaledPosition, offSet, out var firstTileX, out var lastTileX, out var firstTileY, out var lastTileY);
		_windGrid.SetSize(lastTileX - firstTileX, lastTileY - firstTileY);
	}

	private void EmitTreeLeaves(int tilePosX, int tilePosY, int grassPosX, int grassPosY)
	{
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		if (!_isActiveAndNotPaused)
		{
			return;
		}
		int treeHeight = grassPosY - tilePosY;
		Tile tile = Main.tile[tilePosX, tilePosY];
		if (tile.liquid > 0)
		{
			return;
		}
		WorldGen.GetTreeLeaf(tilePosX, tile, Main.tile[grassPosX, grassPosY], ref treeHeight, out var _, out var passStyle);
		int num;
		switch (passStyle)
		{
		case -1:
		case 912:
		case 913:
		case 1278:
			return;
		default:
			num = ((passStyle >= 1113 && passStyle <= 1121) ? 1 : 0);
			break;
		case 917:
		case 918:
		case 919:
		case 920:
		case 921:
		case 922:
		case 923:
		case 924:
		case 925:
			num = 1;
			break;
		}
		bool flag = (byte)num != 0;
		int num2 = _leafFrequency;
		bool flag2 = tilePosX - grassPosX != 0;
		if (flag)
		{
			num2 /= 2;
		}
		if (!WorldGen.DoesWindBlowAtThisHeight(tilePosY))
		{
			num2 = 10000;
		}
		if (flag2)
		{
			num2 *= 3;
		}
		if (_rand.Next(num2) != 0)
		{
			return;
		}
		int num3 = 2;
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)(tilePosX * 16 + 8), (float)(tilePosY * 16 + 8));
		if (flag2)
		{
			int num4 = tilePosX - grassPosX;
			vector.X += num4 * 12;
			int num5 = 0;
			if (tile.frameY == 220)
			{
				num5 = 1;
			}
			else if (tile.frameY == 242)
			{
				num5 = 2;
			}
			if (tile.frameX == 66)
			{
				switch (num5)
				{
				case 0:
					vector += new Vector2(0f, -6f);
					break;
				case 1:
					vector += new Vector2(0f, -6f);
					break;
				case 2:
					vector += new Vector2(0f, 8f);
					break;
				}
			}
			else
			{
				switch (num5)
				{
				case 0:
					vector += new Vector2(0f, 4f);
					break;
				case 1:
					vector += new Vector2(2f, -6f);
					break;
				case 2:
					vector += new Vector2(6f, -6f);
					break;
				}
			}
		}
		else
		{
			vector += new Vector2(-16f, -16f);
			if (flag)
			{
				vector.Y -= Main.rand.Next(0, 28) * 4;
			}
		}
		if (!WorldGen.SolidTile(vector.ToTileCoordinates()))
		{
			Gore.NewGoreDirect(vector, Utils.RandomVector2(Main.rand, -num3, num3), passStyle, 0.7f + Main.rand.NextFloat() * 0.6f).Frame.CurrentColumn = Main.tile[tilePosX, tilePosY].color();
		}
	}

	private void DrawSpecialTilesLegacy(Vector2 screenPosition, Vector2 offSet)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_074e: Unknown result type (might be due to invalid IL or missing references)
		//IL_075a: Unknown result type (might be due to invalid IL or missing references)
		//IL_075f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Unknown result type (might be due to invalid IL or missing references)
		//IL_076e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_0782: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Unknown result type (might be due to invalid IL or missing references)
		//IL_088c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0898: Unknown result type (might be due to invalid IL or missing references)
		//IL_089d: Unknown result type (might be due to invalid IL or missing references)
		//IL_089e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05db: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0606: Unknown result type (might be due to invalid IL or missing references)
		//IL_0616: Unknown result type (might be due to invalid IL or missing references)
		//IL_0622: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Unknown result type (might be due to invalid IL or missing references)
		//IL_0628: Unknown result type (might be due to invalid IL or missing references)
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_0645: Unknown result type (might be due to invalid IL or missing references)
		//IL_064c: Unknown result type (might be due to invalid IL or missing references)
		//IL_065a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_091e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0923: Unknown result type (might be due to invalid IL or missing references)
		//IL_0928: Unknown result type (might be due to invalid IL or missing references)
		//IL_092a: Unknown result type (might be due to invalid IL or missing references)
		//IL_093a: Unknown result type (might be due to invalid IL or missing references)
		//IL_093f: Unknown result type (might be due to invalid IL or missing references)
		//IL_094c: Unknown result type (might be due to invalid IL or missing references)
		//IL_095b: Unknown result type (might be due to invalid IL or missing references)
		//IL_096c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0971: Unknown result type (might be due to invalid IL or missing references)
		//IL_0976: Unknown result type (might be due to invalid IL or missing references)
		//IL_097f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0981: Unknown result type (might be due to invalid IL or missing references)
		//IL_0988: Unknown result type (might be due to invalid IL or missing references)
		//IL_098f: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_068b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0697: Unknown result type (might be due to invalid IL or missing references)
		//IL_069c: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		Vector2 origin = default(Vector2);
		Color color5 = default(Color);
		Rectangle value7 = default(Rectangle);
		for (int i = 0; i < _specialTilesCount; i++)
		{
			int num = _specialTileX[i];
			int num10 = _specialTileY[i];
			Tile tile = Main.tile[num, num10];
			ushort type = tile.type;
			short frameX = tile.frameX;
			short frameY = tile.frameY;
			if (type == 237)
			{
				Main.spriteBatch.Draw(TextureAssets.SunOrb.Value, new Vector2((float)(num * 16 - (int)screenPosition.X) + 8f, (float)(num10 * 16 - (int)screenPosition.Y - 36)) + offSet, (Rectangle?)new Rectangle(0, 0, TextureAssets.SunOrb.Width(), TextureAssets.SunOrb.Height()), new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, 0), Main.sunCircle, new Vector2((float)(TextureAssets.SunOrb.Width() / 2), (float)(TextureAssets.SunOrb.Height() / 2)), 1f, (SpriteEffects)0, 0f);
			}
			if (type == 334 && frameX >= 5000)
			{
				_ = frameY / 18;
				int num11 = frameX;
				int num12 = 0;
				int num13 = num11 % 5000;
				num13 -= 100;
				while (num11 >= 5000)
				{
					num12++;
					num11 -= 5000;
				}
				int frameX2 = Main.tile[num + 1, num10].frameX;
				frameX2 = ((frameX2 < 25000) ? (frameX2 - 10000) : (frameX2 - 25000));
				Item item = new Item();
				item.netDefaults(num13);
				item.Prefix(frameX2);
				Main.instance.LoadItem(item.type);
				Texture2D value = TextureAssets.Item[item.type].Value;
				Rectangle value2 = ((Main.itemAnimations[item.type] == null) ? value.Frame() : Main.itemAnimations[item.type].GetFrame(value));
				int width = value2.Width;
				int height = value2.Height;
				float num14 = 1f;
				if (width > 40 || height > 40)
				{
					num14 = ((width <= height) ? (40f / (float)height) : (40f / (float)width));
				}
				num14 *= item.scale;
				SpriteEffects effects = (SpriteEffects)0;
				if (num12 >= 3)
				{
					effects = (SpriteEffects)1;
				}
				Color color = Lighting.GetColor(num, num10);
				Main.spriteBatch.Draw(value, new Vector2((float)(num * 16 - (int)screenPosition.X + 24), (float)(num10 * 16 - (int)screenPosition.Y + 8)) + offSet, (Rectangle?)value2, Lighting.GetColor(num, num10), 0f, new Vector2((float)(width / 2), (float)(height / 2)), num14, effects, 0f);
				if (item.color != default(Color))
				{
					Main.spriteBatch.Draw(value, new Vector2((float)(num * 16 - (int)screenPosition.X + 24), (float)(num10 * 16 - (int)screenPosition.Y + 8)) + offSet, (Rectangle?)value2, item.GetColor(color), 0f, new Vector2((float)(width / 2), (float)(height / 2)), num14, effects, 0f);
				}
			}
			if (type == 395)
			{
				Item item2 = ((TEItemFrame)TileEntity.ByPosition[new Point16(num, num10)]).item;
				Vector2 screenPositionForItemCenter = new Vector2((float)(num * 16 - (int)screenPosition.X + 16), (float)(num10 * 16 - (int)screenPosition.Y + 16)) + offSet;
				Color color2 = Lighting.GetColor(num, num10);
				ItemSlot.DrawItemIcon(item2, 31, Main.spriteBatch, screenPositionForItemCenter, item2.scale, 20f, color2);
			}
			if (type == 520)
			{
				Item item3 = ((TEFoodPlatter)TileEntity.ByPosition[new Point16(num, num10)]).item;
				if (!item3.IsAir)
				{
					Main.instance.LoadItem(item3.type);
					Texture2D value3 = TextureAssets.Item[item3.type].Value;
					Rectangle value4 = ((!ItemID.Sets.IsFood[item3.type]) ? value3.Frame() : value3.Frame(1, 3, 0, 2));
					int width2 = value4.Width;
					int height2 = value4.Height;
					float num15 = 1f;
					SpriteEffects effects2 = (SpriteEffects)(tile.frameX == 0);
					Color color3 = Lighting.GetColor(num, num10);
					Color currentColor = color3;
					float scale = 1f;
					ItemSlot.GetItemLight(ref currentColor, ref scale, item3);
					num15 *= scale;
					Vector2 position = new Vector2((float)(num * 16 - (int)screenPosition.X + 8), (float)(num10 * 16 - (int)screenPosition.Y + 16)) + offSet;
					position.Y += 2f;
					((Vector2)(ref origin))._002Ector((float)(width2 / 2), (float)height2);
					Main.spriteBatch.Draw(value3, position, (Rectangle?)value4, currentColor, 0f, origin, num15, effects2, 0f);
					if (item3.color != default(Color))
					{
						Main.spriteBatch.Draw(value3, position, (Rectangle?)value4, item3.GetColor(color3), 0f, origin, num15, effects2, 0f);
					}
				}
			}
			if (type == 471)
			{
				Item item4 = (TileEntity.ByPosition[new Point16(num, num10)] as TEWeaponsRack).item;
				Main.GetItemDrawFrame(item4.type, out var itemTexture, out var itemFrame);
				int width3 = itemFrame.Width;
				int height3 = itemFrame.Height;
				float num16 = 1f;
				float num17 = 40f;
				if ((float)width3 > num17 || (float)height3 > num17)
				{
					num16 = ((width3 <= height3) ? (num17 / (float)height3) : (num17 / (float)width3));
				}
				num16 *= item4.scale;
				SpriteEffects effects3 = (SpriteEffects)1;
				if (tile.frameX < 54)
				{
					effects3 = (SpriteEffects)0;
				}
				Color color4 = Lighting.GetColor(num, num10);
				Color currentColor2 = color4;
				float scale2 = 1f;
				ItemSlot.GetItemLight(ref currentColor2, ref scale2, item4);
				num16 *= scale2;
				Main.spriteBatch.Draw(itemTexture, new Vector2((float)(num * 16 - (int)screenPosition.X + 24), (float)(num10 * 16 - (int)screenPosition.Y + 24)) + offSet, (Rectangle?)itemFrame, currentColor2, 0f, new Vector2((float)(width3 / 2), (float)(height3 / 2)), num16, effects3, 0f);
				if (item4.color != default(Color))
				{
					Main.spriteBatch.Draw(itemTexture, new Vector2((float)(num * 16 - (int)screenPosition.X + 24), (float)(num10 * 16 - (int)screenPosition.Y + 24)) + offSet, (Rectangle?)itemFrame, item4.GetColor(color4), 0f, new Vector2((float)(width3 / 2), (float)(height3 / 2)), num16, effects3, 0f);
				}
			}
			if (type == 412)
			{
				Texture2D value5 = TextureAssets.GlowMask[202].Value;
				int num2 = Main.tileFrame[type] / 60;
				int frameY2 = (num2 + 1) % 4;
				float num3 = (float)(Main.tileFrame[type] % 60) / 60f;
				((Color)(ref color5))._002Ector(255, 255, 255, 255);
				Main.spriteBatch.Draw(value5, new Vector2((float)(num * 16 - (int)screenPosition.X), (float)(num10 * 16 - (int)screenPosition.Y + 10)) + offSet, (Rectangle?)value5.Frame(1, 4, 0, num2), color5 * (1f - num3), 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
				Main.spriteBatch.Draw(value5, new Vector2((float)(num * 16 - (int)screenPosition.X), (float)(num10 * 16 - (int)screenPosition.Y + 10)) + offSet, (Rectangle?)value5.Frame(1, 4, 0, frameY2), color5 * num3, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
			}
			if (type == 620)
			{
				Texture2D value6 = TextureAssets.Extra[202].Value;
				_ = (float)(Main.tileFrame[type] % 60) / 60f;
				int num4 = 2;
				Main.critterCage = true;
				int waterAnimalCageFrame = GetWaterAnimalCageFrame(num, num10, frameX, frameY);
				int num5 = 8;
				int num6 = Main.butterflyCageFrame[num5, waterAnimalCageFrame];
				int num7 = 6;
				float num8 = 1f;
				((Rectangle)(ref value7))._002Ector(0, 34 * num6, 32, 32);
				Vector2 vector = new Vector2((float)(num * 16 - (int)screenPosition.X), (float)(num10 * 16 - (int)screenPosition.Y + num4)) + offSet;
				Main.spriteBatch.Draw(value6, vector, (Rectangle?)value7, new Color(255, 255, 255, 255), 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
				for (int j = 0; j < num7; j++)
				{
					Color color6 = Utils.MultiplyRGBA(new Color(127, 127, 127, 0), Main.hslToRgb((Main.GlobalTimeWrappedHourly + (float)j / (float)num7) % 1f, 1f, 0.5f));
					color6 *= 1f - num8 * 0.5f;
					((Color)(ref color6)).A = 0;
					int num9 = 2;
					Vector2 position2 = vector + ((float)j / (float)num7 * ((float)Math.PI * 2f)).ToRotationVector2() * ((float)num9 * num8 + 2f);
					Main.spriteBatch.Draw(value6, position2, (Rectangle?)value7, color6, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
				}
				Main.spriteBatch.Draw(value6, vector, (Rectangle?)value7, new Color(255, 255, 255, 0) * 0.1f, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
			}
			TileLoader.SpecialDraw(type, num, num10, Main.spriteBatch);
		}
	}

	private void DrawEntities_DisplayDolls()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, (Effect)null, Main.Transform);
		foreach (KeyValuePair<Point, int> displayDollTileEntityPosition in _displayDollTileEntityPositions)
		{
			if (displayDollTileEntityPosition.Value != -1 && TileEntity.ByPosition.TryGetValue(new Point16(displayDollTileEntityPosition.Key.X, displayDollTileEntityPosition.Key.Y), out var value))
			{
				(value as TEDisplayDoll).Draw(displayDollTileEntityPosition.Key.X, displayDollTileEntityPosition.Key.Y);
			}
		}
		Main.spriteBatch.End();
	}

	private void DrawEntities_HatRacks()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, (Effect)null, Main.Transform);
		foreach (KeyValuePair<Point, int> hatRackTileEntityPosition in _hatRackTileEntityPositions)
		{
			if (hatRackTileEntityPosition.Value != -1 && TileEntity.ByPosition.TryGetValue(new Point16(hatRackTileEntityPosition.Key.X, hatRackTileEntityPosition.Key.Y), out var value))
			{
				(value as TEHatRack).Draw(hatRackTileEntityPosition.Key.X, hatRackTileEntityPosition.Key.Y);
			}
		}
		Main.spriteBatch.End();
	}

	private void DrawTrees()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0962: Unknown result type (might be due to invalid IL or missing references)
		//IL_097b: Unknown result type (might be due to invalid IL or missing references)
		//IL_098a: Unknown result type (might be due to invalid IL or missing references)
		//IL_098f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0990: Unknown result type (might be due to invalid IL or missing references)
		//IL_0995: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Unknown result type (might be due to invalid IL or missing references)
		//IL_072e: Unknown result type (might be due to invalid IL or missing references)
		//IL_072f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Unknown result type (might be due to invalid IL or missing references)
		//IL_0739: Unknown result type (might be due to invalid IL or missing references)
		//IL_073a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Unknown result type (might be due to invalid IL or missing references)
		//IL_074e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0753: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a29: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_0594: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_081a: Unknown result type (might be due to invalid IL or missing references)
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0828: Unknown result type (might be due to invalid IL or missing references)
		//IL_0835: Unknown result type (might be due to invalid IL or missing references)
		//IL_083f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0850: Unknown result type (might be due to invalid IL or missing references)
		Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
		Vector2 zero = Vector2.Zero;
		int num = 0;
		int num12 = _specialsCount[num];
		float num15 = 0.08f;
		float num16 = 0.06f;
		for (int i = 0; i < num12; i++)
		{
			Point val = _specialPositions[num][i];
			int x = val.X;
			int y = val.Y;
			Tile tile = Main.tile[x, y];
			if (tile == null || !tile.active())
			{
				continue;
			}
			ushort type = tile.type;
			short frameX = tile.frameX;
			short frameY = tile.frameY;
			bool flag = tile.wall > 0;
			WorldGen.GetTreeFoliageDataMethod getTreeFoliageDataMethod = null;
			try
			{
				bool flag2 = false;
				switch (type)
				{
				case 5:
					flag2 = true;
					getTreeFoliageDataMethod = WorldGen.GetCommonTreeFoliageData;
					break;
				case 583:
				case 584:
				case 585:
				case 586:
				case 587:
				case 588:
				case 589:
					flag2 = true;
					getTreeFoliageDataMethod = WorldGen.GetGemTreeFoliageData;
					break;
				case 596:
				case 616:
					flag2 = true;
					getTreeFoliageDataMethod = WorldGen.GetVanityTreeFoliageData;
					break;
				case 634:
					flag2 = true;
					getTreeFoliageDataMethod = WorldGen.GetAshTreeFoliageData;
					break;
				}
				if (flag2 && frameY >= 198 && frameX >= 22)
				{
					int treeFrame = WorldGen.GetTreeFrame(tile);
					int topTextureFrameWidth4;
					int topTextureFrameHeight4;
					switch (frameX)
					{
					case 22:
					{
						int treeStyle3 = 0;
						int topTextureFrameWidth3 = 80;
						int topTextureFrameHeight3 = 80;
						int num5 = 0;
						int grassPosX = x + num5;
						int floorY3 = y;
						if (!getTreeFoliageDataMethod(x, y, num5, ref treeFrame, ref treeStyle3, out floorY3, out topTextureFrameWidth3, out topTextureFrameHeight3))
						{
							continue;
						}
						EmitTreeLeaves(x, y, grassPosX, floorY3);
						if (treeStyle3 == 14)
						{
							float num6 = (float)_rand.Next(28, 42) * 0.005f;
							num6 += (float)(270 - Main.mouseTextColor) / 1000f;
							if (tile.color() == 0)
							{
								Lighting.AddLight(x, y, 0.1f, 0.2f + num6 / 2f, 0.7f + num6);
							}
							else
							{
								Color color5 = WorldGen.paintColor(tile.color());
								float r3 = (float)(int)((Color)(ref color5)).R / 255f;
								float g3 = (float)(int)((Color)(ref color5)).G / 255f;
								float b3 = (float)(int)((Color)(ref color5)).B / 255f;
								Lighting.AddLight(x, y, r3, g3, b3);
							}
						}
						byte tileColor3 = tile.color();
						Texture2D treeTopTexture = GetTreeTopTexture(treeStyle3, 0, tileColor3);
						Vector2 vector = (vector = new Vector2((float)(x * 16 - (int)unscaledPosition.X + 8), (float)(y * 16 - (int)unscaledPosition.Y + 16)) + zero);
						float num7 = 0f;
						if (!flag)
						{
							num7 = GetWindCycle(x, y, _treeWindCounter);
						}
						vector.X += num7 * 2f;
						vector.Y += Math.Abs(num7) * 2f;
						Color color6 = Lighting.GetColor(x, y);
						if (tile.fullbrightBlock())
						{
							color6 = Color.White;
						}
						Main.spriteBatch.Draw(treeTopTexture, vector, (Rectangle?)new Rectangle(treeFrame * (topTextureFrameWidth3 + 2), 0, topTextureFrameWidth3, topTextureFrameHeight3), color6, num7 * num15, new Vector2((float)(topTextureFrameWidth3 / 2), (float)topTextureFrameHeight3), 1f, (SpriteEffects)0, 0f);
						if (type == 634)
						{
							Texture2D value3 = TextureAssets.GlowMask[316].Value;
							Color white3 = Color.White;
							Main.spriteBatch.Draw(value3, vector, (Rectangle?)new Rectangle(treeFrame * (topTextureFrameWidth3 + 2), 0, topTextureFrameWidth3, topTextureFrameHeight3), white3, num7 * num15, new Vector2((float)(topTextureFrameWidth3 / 2), (float)topTextureFrameHeight3), 1f, (SpriteEffects)0, 0f);
						}
						break;
					}
					case 44:
					{
						int treeStyle2 = 0;
						int num21 = x;
						int floorY2 = y;
						int num2 = 1;
						if (!getTreeFoliageDataMethod(x, y, num2, ref treeFrame, ref treeStyle2, out floorY2, out topTextureFrameWidth4, out topTextureFrameHeight4))
						{
							continue;
						}
						EmitTreeLeaves(x, y, num21 + num2, floorY2);
						if (treeStyle2 == 14)
						{
							float num3 = (float)_rand.Next(28, 42) * 0.005f;
							num3 += (float)(270 - Main.mouseTextColor) / 1000f;
							if (tile.color() == 0)
							{
								Lighting.AddLight(x, y, 0.1f, 0.2f + num3 / 2f, 0.7f + num3);
							}
							else
							{
								Color color3 = WorldGen.paintColor(tile.color());
								float r2 = (float)(int)((Color)(ref color3)).R / 255f;
								float g2 = (float)(int)((Color)(ref color3)).G / 255f;
								float b2 = (float)(int)((Color)(ref color3)).B / 255f;
								Lighting.AddLight(x, y, r2, g2, b2);
							}
						}
						byte tileColor2 = tile.color();
						Texture2D treeBranchTexture2 = GetTreeBranchTexture(treeStyle2, 0, tileColor2);
						Vector2 position2 = new Vector2((float)(x * 16), (float)(y * 16)) - unscaledPosition.Floor() + zero + new Vector2(16f, 12f);
						float num4 = 0f;
						if (!flag)
						{
							num4 = GetWindCycle(x, y, _treeWindCounter);
						}
						if (num4 > 0f)
						{
							position2.X += num4;
						}
						position2.X += Math.Abs(num4) * 2f;
						Color color4 = Lighting.GetColor(x, y);
						if (tile.fullbrightBlock())
						{
							color4 = Color.White;
						}
						Main.spriteBatch.Draw(treeBranchTexture2, position2, (Rectangle?)new Rectangle(0, treeFrame * 42, 40, 40), color4, num4 * num16, new Vector2(40f, 24f), 1f, (SpriteEffects)0, 0f);
						if (type == 634)
						{
							Texture2D value2 = TextureAssets.GlowMask[317].Value;
							Color white2 = Color.White;
							Main.spriteBatch.Draw(value2, position2, (Rectangle?)new Rectangle(0, treeFrame * 42, 40, 40), white2, num4 * num16, new Vector2(40f, 24f), 1f, (SpriteEffects)0, 0f);
						}
						break;
					}
					case 66:
					{
						int treeStyle = 0;
						int num17 = x;
						int floorY = y;
						int num18 = -1;
						if (!getTreeFoliageDataMethod(x, y, num18, ref treeFrame, ref treeStyle, out floorY, out topTextureFrameHeight4, out topTextureFrameWidth4))
						{
							continue;
						}
						EmitTreeLeaves(x, y, num17 + num18, floorY);
						if (treeStyle == 14)
						{
							float num19 = (float)_rand.Next(28, 42) * 0.005f;
							num19 += (float)(270 - Main.mouseTextColor) / 1000f;
							if (tile.color() == 0)
							{
								Lighting.AddLight(x, y, 0.1f, 0.2f + num19 / 2f, 0.7f + num19);
							}
							else
							{
								Color color = WorldGen.paintColor(tile.color());
								float r = (float)(int)((Color)(ref color)).R / 255f;
								float g = (float)(int)((Color)(ref color)).G / 255f;
								float b = (float)(int)((Color)(ref color)).B / 255f;
								Lighting.AddLight(x, y, r, g, b);
							}
						}
						byte tileColor = tile.color();
						Texture2D treeBranchTexture = GetTreeBranchTexture(treeStyle, 0, tileColor);
						Vector2 position = new Vector2((float)(x * 16), (float)(y * 16)) - unscaledPosition.Floor() + zero + new Vector2(0f, 18f);
						float num20 = 0f;
						if (!flag)
						{
							num20 = GetWindCycle(x, y, _treeWindCounter);
						}
						if (num20 < 0f)
						{
							position.X += num20;
						}
						position.X -= Math.Abs(num20) * 2f;
						Color color2 = Lighting.GetColor(x, y);
						if (tile.fullbrightBlock())
						{
							color2 = Color.White;
						}
						Main.spriteBatch.Draw(treeBranchTexture, position, (Rectangle?)new Rectangle(42, treeFrame * 42, 40, 40), color2, num20 * num16, new Vector2(0f, 30f), 1f, (SpriteEffects)0, 0f);
						if (type == 634)
						{
							Texture2D value = TextureAssets.GlowMask[317].Value;
							Color white = Color.White;
							Main.spriteBatch.Draw(value, position, (Rectangle?)new Rectangle(42, treeFrame * 42, 40, 40), white, num20 * num16, new Vector2(0f, 30f), 1f, (SpriteEffects)0, 0f);
						}
						break;
					}
					}
				}
				if (type != 323 || frameX < 88 || frameX > 132)
				{
					continue;
				}
				int num8 = 0;
				switch (frameX)
				{
				case 110:
					num8 = 1;
					break;
				case 132:
					num8 = 2;
					break;
				}
				int treeTextureIndex = 15;
				int num9 = 80;
				int num10 = 80;
				int num11 = 32;
				int num13 = 0;
				int palmTreeBiome = GetPalmTreeBiome(x, y);
				int y2 = palmTreeBiome * 82;
				if (palmTreeBiome >= 4 && palmTreeBiome <= 7)
				{
					treeTextureIndex = 21;
					num9 = 114;
					num10 = 98;
					y2 = (palmTreeBiome - 4) * 98;
					num11 = 48;
					num13 = 2;
				}
				if (Math.Abs(palmTreeBiome) >= 8)
				{
					y2 = 0;
					if (palmTreeBiome < 0)
					{
						num9 = 114;
						num10 = 98;
						num11 = 48;
						num13 = 2;
					}
					treeTextureIndex = Math.Abs(palmTreeBiome) - 8;
					treeTextureIndex *= -2;
					if (palmTreeBiome < 0)
					{
						treeTextureIndex--;
					}
				}
				int frameY2 = Main.tile[x, y].frameY;
				byte tileColor4 = tile.color();
				Texture2D treeTopTexture2 = GetTreeTopTexture(treeTextureIndex, palmTreeBiome, tileColor4);
				Vector2 position3 = new Vector2((float)(x * 16 - (int)unscaledPosition.X - num11 + frameY2 + num9 / 2), (float)(y * 16 - (int)unscaledPosition.Y + 16 + num13)) + zero;
				float num14 = 0f;
				if (!flag)
				{
					num14 = GetWindCycle(x, y, _treeWindCounter);
				}
				position3.X += num14 * 2f;
				position3.Y += Math.Abs(num14) * 2f;
				Color color7 = Lighting.GetColor(x, y);
				if (tile.fullbrightBlock())
				{
					color7 = Color.White;
				}
				Main.spriteBatch.Draw(treeTopTexture2, position3, (Rectangle?)new Rectangle(num8 * (num9 + 2), y2, num9, num10), color7, num14 * num15, new Vector2((float)(num9 / 2), (float)num10), 1f, (SpriteEffects)0, 0f);
			}
			catch
			{
			}
		}
	}

	private Texture2D GetTreeTopTexture(int treeTextureIndex, int treeTextureStyle, byte tileColor)
	{
		Texture2D texture2D = _paintSystem.TryGetTreeTopAndRequestIfNotReady(treeTextureIndex, treeTextureStyle, tileColor);
		if (texture2D == null)
		{
			if (treeTextureIndex < 0 || treeTextureIndex >= 100)
			{
				treeTextureIndex = 0;
			}
			texture2D = TextureAssets.TreeTop[treeTextureIndex].Value;
		}
		return texture2D;
	}

	private Texture2D GetTreeBranchTexture(int treeTextureIndex, int treeTextureStyle, byte tileColor)
	{
		Texture2D texture2D = _paintSystem.TryGetTreeBranchAndRequestIfNotReady(treeTextureIndex, treeTextureStyle, tileColor);
		if (texture2D == null)
		{
			if (treeTextureIndex < 0 || treeTextureIndex >= 100)
			{
				treeTextureIndex = 0;
			}
			texture2D = TextureAssets.TreeBranch[treeTextureIndex].Value;
		}
		return texture2D;
	}

	private void DrawGrass()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
		Vector2 zero = Vector2.Zero;
		int num = 3;
		int num2 = _specialsCount[num];
		for (int i = 0; i < num2; i++)
		{
			Point val = _specialPositions[num][i];
			int x = val.X;
			int y = val.Y;
			Tile tile = Main.tile[x, y];
			if (tile == null || !tile.active() || !IsVisible(tile))
			{
				continue;
			}
			ushort type = tile.type;
			short tileFrameX = tile.frameX;
			short tileFrameY = tile.frameY;
			GetTileDrawData(x, y, tile, type, ref tileFrameX, ref tileFrameY, out var tileWidth, out var tileHeight, out var tileTop, out var halfBrickHeight, out var addFrX, out var addFrY, out var tileSpriteEffect, out var glowTexture, out var glowSourceRect, out var glowColor);
			bool flag = _rand.Next(4) == 0;
			Color tileLight = Lighting.GetColor(x, y);
			DrawAnimatedTile_AdjustForVisionChangers(x, y, tile, type, tileFrameX, tileFrameY, ref tileLight, flag);
			tileLight = DrawTiles_GetLightOverride(y, x, tile, type, tileFrameX, tileFrameY, tileLight);
			if (_isActiveAndNotPaused && flag)
			{
				DrawTiles_EmitParticles(y, x, tile, type, tileFrameX, tileFrameY, tileLight);
			}
			if (type == 83 && IsAlchemyPlantHarvestable(tileFrameX / 18))
			{
				type = 84;
				Main.instance.LoadTiles(type);
			}
			Vector2 position = new Vector2((float)(x * 16 - (int)unscaledPosition.X + 8), (float)(y * 16 - (int)unscaledPosition.Y + 16)) + zero;
			_ = _grassWindCounter;
			float num3 = GetWindCycle(x, y, _grassWindCounter);
			if (!WallID.Sets.AllowsWind[tile.wall])
			{
				num3 = 0f;
			}
			if (!InAPlaceWithWind(x, y, 1, 1))
			{
				num3 = 0f;
			}
			num3 += GetWindGridPush(x, y, 20, 0.35f);
			position.X += num3 * 1f;
			position.Y += Math.Abs(num3) * 1f;
			Texture2D tileDrawTexture = GetTileDrawTexture(tile, x, y);
			if (tileDrawTexture != null)
			{
				Main.spriteBatch.Draw(tileDrawTexture, position, (Rectangle?)new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight), tileLight, num3 * 0.1f, new Vector2((float)(tileWidth / 2), (float)(16 - halfBrickHeight - tileTop)), 1f, tileSpriteEffect, 0f);
				if (glowTexture != null)
				{
					Main.spriteBatch.Draw(glowTexture, position, (Rectangle?)glowSourceRect, glowColor, num3 * 0.1f, new Vector2((float)(tileWidth / 2), (float)(16 - halfBrickHeight - tileTop)), 1f, tileSpriteEffect, 0f);
				}
			}
		}
	}

	private void DrawAnyDirectionalGrass()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
		Vector2 zero = Vector2.Zero;
		int num = 12;
		int num2 = _specialsCount[num];
		Vector2 origin = default(Vector2);
		for (int i = 0; i < num2; i++)
		{
			Point val = _specialPositions[num][i];
			int x = val.X;
			int y = val.Y;
			Tile tile = Main.tile[x, y];
			if (tile == null || !tile.active() || !IsVisible(tile))
			{
				continue;
			}
			ushort type = tile.type;
			short tileFrameX = tile.frameX;
			short tileFrameY = tile.frameY;
			GetTileDrawData(x, y, tile, type, ref tileFrameX, ref tileFrameY, out var tileWidth, out var tileHeight, out var tileTop, out var halfBrickHeight, out var addFrX, out var addFrY, out var tileSpriteEffect, out var glowTexture, out var _, out var glowColor);
			bool flag = _rand.Next(4) == 0;
			Color tileLight = Lighting.GetColor(x, y);
			DrawAnimatedTile_AdjustForVisionChangers(x, y, tile, type, tileFrameX, tileFrameY, ref tileLight, flag);
			tileLight = DrawTiles_GetLightOverride(y, x, tile, type, tileFrameX, tileFrameY, tileLight);
			if (_isActiveAndNotPaused && flag)
			{
				DrawTiles_EmitParticles(y, x, tile, type, tileFrameX, tileFrameY, tileLight);
			}
			if (type == 83 && IsAlchemyPlantHarvestable(tileFrameX / 18))
			{
				type = 84;
				Main.instance.LoadTiles(type);
			}
			Vector2 position = new Vector2((float)(x * 16 - (int)unscaledPosition.X), (float)(y * 16 - (int)unscaledPosition.Y)) + zero;
			_ = _grassWindCounter;
			float num3 = GetWindCycle(x, y, _grassWindCounter);
			if (!WallID.Sets.AllowsWind[tile.wall])
			{
				num3 = 0f;
			}
			if (!InAPlaceWithWind(x, y, 1, 1))
			{
				num3 = 0f;
			}
			GetWindGridPush2Axis(x, y, 20, 0.35f, out var pushX, out var pushY);
			int num4 = 1;
			int num5 = 0;
			((Vector2)(ref origin))._002Ector((float)(tileWidth / 2), (float)(16 - halfBrickHeight - tileTop));
			switch (tileFrameY / 54)
			{
			case 0:
				num4 = 1;
				num5 = 0;
				((Vector2)(ref origin))._002Ector((float)(tileWidth / 2), (float)(16 - halfBrickHeight - tileTop));
				position.X += 8f;
				position.Y += 16f;
				position.X += num3;
				position.Y += Math.Abs(num3);
				break;
			case 1:
				num3 *= -1f;
				num4 = -1;
				num5 = 0;
				((Vector2)(ref origin))._002Ector((float)(tileWidth / 2), (float)(-tileTop));
				position.X += 8f;
				position.X += 0f - num3;
				position.Y += 0f - Math.Abs(num3);
				break;
			case 2:
				num4 = 0;
				num5 = 1;
				((Vector2)(ref origin))._002Ector(2f, (float)((16 - halfBrickHeight - tileTop) / 2));
				position.Y += 8f;
				position.Y += num3;
				position.X += 0f - Math.Abs(num3);
				break;
			case 3:
				num3 *= -1f;
				num4 = 0;
				num5 = -1;
				((Vector2)(ref origin))._002Ector(14f, (float)((16 - halfBrickHeight - tileTop) / 2));
				position.X += 16f;
				position.Y += 8f;
				position.Y += 0f - num3;
				position.X += Math.Abs(num3);
				break;
			}
			num3 += pushX * (float)num4 + pushY * (float)num5;
			Texture2D tileDrawTexture = GetTileDrawTexture(tile, x, y);
			if (tileDrawTexture != null)
			{
				Main.spriteBatch.Draw(tileDrawTexture, position, (Rectangle?)new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight), tileLight, num3 * 0.1f, origin, 1f, tileSpriteEffect, 0f);
				if (glowTexture != null)
				{
					Main.spriteBatch.Draw(glowTexture, position, (Rectangle?)new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight), glowColor, num3 * 0.1f, origin, 1f, tileSpriteEffect, 0f);
				}
			}
		}
	}

	private void DrawAnimatedTile_AdjustForVisionChangers(int i, int j, Tile tileCache, ushort typeCache, short tileFrameX, short tileFrameY, ref Color tileLight, bool canDoDust)
	{
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		if (_localPlayer.dangerSense && IsTileDangerous(i, j, _localPlayer, tileCache, typeCache))
		{
			if (((Color)(ref tileLight)).R < byte.MaxValue)
			{
				((Color)(ref tileLight)).R = byte.MaxValue;
			}
			if (((Color)(ref tileLight)).G < 50)
			{
				((Color)(ref tileLight)).G = 50;
			}
			if (((Color)(ref tileLight)).B < 50)
			{
				((Color)(ref tileLight)).B = 50;
			}
			if (_isActiveAndNotPaused && canDoDust && _rand.Next(30) == 0)
			{
				int num = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 60, 0f, 0f, 100, default(Color), 0.3f);
				_dust[num].fadeIn = 1f;
				Dust obj = _dust[num];
				obj.velocity *= 0.1f;
				_dust[num].noLight = true;
				_dust[num].noGravity = true;
			}
		}
		if (_localPlayer.findTreasure && Main.IsTileSpelunkable(i, j, typeCache, tileFrameX, tileFrameY))
		{
			if (((Color)(ref tileLight)).R < 200)
			{
				((Color)(ref tileLight)).R = 200;
			}
			if (((Color)(ref tileLight)).G < 170)
			{
				((Color)(ref tileLight)).G = 170;
			}
			if (_isActiveAndNotPaused && _rand.Next(60) == 0 && canDoDust)
			{
				int num2 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 204, 0f, 0f, 150, default(Color), 0.3f);
				_dust[num2].fadeIn = 1f;
				Dust obj2 = _dust[num2];
				obj2.velocity *= 0.1f;
				_dust[num2].noLight = true;
			}
		}
		if (!_localPlayer.biomeSight)
		{
			return;
		}
		Color sightColor = Color.White;
		if (Main.IsTileBiomeSightable(i, j, typeCache, tileFrameX, tileFrameY, ref sightColor))
		{
			if (((Color)(ref tileLight)).R < ((Color)(ref sightColor)).R)
			{
				((Color)(ref tileLight)).R = ((Color)(ref sightColor)).R;
			}
			if (((Color)(ref tileLight)).G < ((Color)(ref sightColor)).G)
			{
				((Color)(ref tileLight)).G = ((Color)(ref sightColor)).G;
			}
			if (((Color)(ref tileLight)).B < ((Color)(ref sightColor)).B)
			{
				((Color)(ref tileLight)).B = ((Color)(ref sightColor)).B;
			}
			if (_isActiveAndNotPaused && canDoDust && _rand.Next(480) == 0)
			{
				Color newColor = sightColor;
				int num3 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 267, 0f, 0f, 150, newColor, 0.3f);
				_dust[num3].noGravity = true;
				_dust[num3].fadeIn = 1f;
				Dust obj3 = _dust[num3];
				obj3.velocity *= 0.1f;
				_dust[num3].noLightEmittence = true;
			}
		}
	}

	/// <summary>
	/// Determines how much wind should affect a theoretical tile at the target location on the current update tick.
	/// </summary>
	/// <param name="i">The X coordinate of the theoretical target tile.</param>
	/// <param name="j">The Y coordinate of the theoretical target tile.</param>
	/// <param name="pushAnimationTimeTotal">The total amount of time, in ticks, that a wind push cycle for the theoretical target tile should last for.</param>
	/// <param name="pushForcePerFrame">The amount which wind should affect the theoretical target tile per frame.</param>
	/// <returns>
	/// The degree to which wind should affect the theoretical target tile, represented as a float.
	/// </returns>
	public float GetWindGridPush(int i, int j, int pushAnimationTimeTotal, float pushForcePerFrame)
	{
		_windGrid.GetWindTime(i, j, pushAnimationTimeTotal, out var windTimeLeft, out var directionX, out var _);
		if (windTimeLeft >= pushAnimationTimeTotal / 2)
		{
			return (float)(pushAnimationTimeTotal - windTimeLeft) * pushForcePerFrame * (float)directionX;
		}
		return (float)windTimeLeft * pushForcePerFrame * (float)directionX;
	}

	private void GetWindGridPush2Axis(int i, int j, int pushAnimationTimeTotal, float pushForcePerFrame, out float pushX, out float pushY)
	{
		_windGrid.GetWindTime(i, j, pushAnimationTimeTotal, out var windTimeLeft, out var directionX, out var directionY);
		if (windTimeLeft >= pushAnimationTimeTotal / 2)
		{
			pushX = (float)(pushAnimationTimeTotal - windTimeLeft) * pushForcePerFrame * (float)directionX;
			pushY = (float)(pushAnimationTimeTotal - windTimeLeft) * pushForcePerFrame * (float)directionY;
		}
		else
		{
			pushX = (float)windTimeLeft * pushForcePerFrame * (float)directionX;
			pushY = (float)windTimeLeft * pushForcePerFrame * (float)directionY;
		}
	}

	/// <summary>
	/// Determines how much wind should affect a theoretical tile at the target location on the current update tick.<br />
	/// More complex version of <see cref="M:Terraria.GameContent.Drawing.TileDrawing.GetWindGridPush(System.Int32,System.Int32,System.Int32,System.Single)" />.
	/// </summary>
	/// <param name="i">The X coordinate of the theoretical target tile.</param>
	/// <param name="j">The Y coordinate of the theoretical target tile.</param>
	/// <param name="pushAnimationTimeTotal">The total amount of time, in ticks, that a wind push cycle for the theoretical target tile should last for.</param>
	/// <param name="totalPushForce"></param>
	/// <param name="loops"></param>
	/// <param name="flipDirectionPerLoop"></param>
	/// <returns></returns>
	public float GetWindGridPushComplex(int i, int j, int pushAnimationTimeTotal, float totalPushForce, int loops, bool flipDirectionPerLoop)
	{
		_windGrid.GetWindTime(i, j, pushAnimationTimeTotal, out var windTimeLeft, out var directionX, out var _);
		float num4 = (float)windTimeLeft / (float)pushAnimationTimeTotal;
		int num2 = (int)(num4 * (float)loops);
		float num3 = num4 * (float)loops % 1f;
		_ = 1f / (float)loops;
		if (flipDirectionPerLoop && num2 % 2 == 1)
		{
			directionX *= -1;
		}
		if (num4 * (float)loops % 1f > 0.5f)
		{
			return (1f - num3) * totalPushForce * (float)directionX * (float)(loops - num2);
		}
		return num3 * totalPushForce * (float)directionX * (float)(loops - num2);
	}

	private void DrawMasterTrophies()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		int num = 11;
		int num2 = _specialsCount[num];
		for (int i = 0; i < num2; i++)
		{
			Point p = _specialPositions[num][i];
			Tile tile = Main.tile[p.X, p.Y];
			if (tile != null && tile.active())
			{
				Texture2D value = TextureAssets.Extra[198].Value;
				int frameY = tile.frameX / 54;
				bool num6 = tile.frameY / 72 != 0;
				int horizontalFrames = 1;
				int verticalFrames = 28;
				Rectangle rectangle = value.Frame(horizontalFrames, verticalFrames, 0, frameY);
				Vector2 origin = rectangle.Size() / 2f;
				Vector2 val = p.ToWorldCoordinates(24f, 64f);
				float num3 = (float)Math.Sin(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f) / 5f);
				Vector2 vector2 = val + new Vector2(0f, -40f) + new Vector2(0f, num3 * 4f);
				Color color = Lighting.GetColor(p.X, p.Y);
				SpriteEffects effects = (SpriteEffects)(num6 ? 1 : 0);
				Main.spriteBatch.Draw(value, vector2 - Main.screenPosition, (Rectangle?)rectangle, color, 0f, origin, 1f, effects, 0f);
				float num4 = (float)Math.Sin(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f) / 2f) * 0.3f + 0.7f;
				Color color2 = color;
				((Color)(ref color2)).A = 0;
				color2 = color2 * 0.1f * num4;
				for (float num5 = 0f; num5 < 1f; num5 += 1f / 6f)
				{
					Main.spriteBatch.Draw(value, vector2 - Main.screenPosition + ((float)Math.PI * 2f * num5).ToRotationVector2() * (6f + num3 * 2f), (Rectangle?)rectangle, color2, 0f, origin, 1f, effects, 0f);
				}
			}
		}
	}

	private void DrawTeleportationPylons()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		int num = 10;
		int num2 = _specialsCount[num];
		for (int i = 0; i < num2; i++)
		{
			Point p = _specialPositions[num][i];
			Tile tile = Main.tile[p.X, p.Y];
			if (tile == null || !tile.active())
			{
				continue;
			}
			Texture2D value = TextureAssets.Extra[181].Value;
			int num3 = tile.frameX / 54;
			int num4 = 3;
			int horizontalFrames = num4 + 9;
			int verticalFrames = 8;
			int frameY = (Main.tileFrameCounter[597] + p.X + p.Y) % 64 / 8;
			Rectangle rectangle = value.Frame(horizontalFrames, verticalFrames, num4 + num3, frameY);
			Rectangle value2 = value.Frame(horizontalFrames, verticalFrames, 2, frameY);
			value.Frame(horizontalFrames, verticalFrames, 0, frameY);
			Vector2 origin = rectangle.Size() / 2f;
			Vector2 val = p.ToWorldCoordinates(24f, 64f);
			float num5 = (float)Math.Sin(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f) / 5f);
			Vector2 vector2 = val + new Vector2(0f, -40f) + new Vector2(0f, num5 * 4f);
			bool flag = _rand.Next(4) == 0;
			if (_isActiveAndNotPaused && flag && _rand.Next(10) == 0)
			{
				Rectangle dustBox = Utils.CenteredRectangle(vector2, rectangle.Size());
				TeleportPylonsSystem.SpawnInWorldDust(num3, dustBox);
			}
			Color color = Lighting.GetColor(p.X, p.Y);
			color = Color.Lerp(color, Color.White, 0.8f);
			Main.spriteBatch.Draw(value, vector2 - Main.screenPosition, (Rectangle?)rectangle, color * 0.7f, 0f, origin, 1f, (SpriteEffects)0, 0f);
			float num6 = (float)Math.Sin(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f) / 1f) * 0.2f + 0.8f;
			Color color2 = new Color(255, 255, 255, 0) * 0.1f * num6;
			for (float num7 = 0f; num7 < 1f; num7 += 1f / 6f)
			{
				Main.spriteBatch.Draw(value, vector2 - Main.screenPosition + ((float)Math.PI * 2f * num7).ToRotationVector2() * (6f + num5 * 2f), (Rectangle?)rectangle, color2, 0f, origin, 1f, (SpriteEffects)0, 0f);
			}
			int num8 = 0;
			if (Main.InSmartCursorHighlightArea(p.X, p.Y, out var actuallySelected))
			{
				num8 = 1;
				if (actuallySelected)
				{
					num8 = 2;
				}
			}
			if (num8 != 0)
			{
				int num9 = (((Color)(ref color)).R + ((Color)(ref color)).G + ((Color)(ref color)).B) / 3;
				if (num9 > 10)
				{
					Color selectionGlowColor = Colors.GetSelectionGlowColor(num8 == 2, num9);
					Main.spriteBatch.Draw(value, vector2 - Main.screenPosition, (Rectangle?)value2, selectionGlowColor, 0f, origin, 1f, (SpriteEffects)0, 0f);
				}
			}
		}
	}

	private void DrawVoidLenses()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		int num = 8;
		int num2 = _specialsCount[num];
		_voidLensData.Clear();
		for (int i = 0; i < num2; i++)
		{
			Point p = _specialPositions[num][i];
			VoidLensHelper voidLensHelper = new VoidLensHelper(p.ToWorldCoordinates(), 1f);
			if (!Main.gamePaused)
			{
				voidLensHelper.Update();
			}
			int selectionMode = 0;
			if (Main.InSmartCursorHighlightArea(p.X, p.Y, out var actuallySelected))
			{
				selectionMode = 1;
				if (actuallySelected)
				{
					selectionMode = 2;
				}
			}
			voidLensHelper.DrawToDrawData(_voidLensData, selectionMode);
		}
		foreach (DrawData voidLensDatum in _voidLensData)
		{
			voidLensDatum.Draw(Main.spriteBatch);
		}
	}

	private void DrawMultiTileGrass()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
		Vector2 zero = Vector2.Zero;
		int num = 4;
		int num2 = _specialsCount[num];
		for (int i = 0; i < num2; i++)
		{
			Point val = _specialPositions[num][i];
			int x = val.X;
			int num3 = val.Y;
			int sizeX = 1;
			int num4 = 1;
			Tile tile = Main.tile[x, num3];
			if (tile != null && tile.active())
			{
				switch (Main.tile[x, num3].type)
				{
				case 27:
					sizeX = 2;
					num4 = 5;
					break;
				case 236:
				case 238:
					sizeX = (num4 = 2);
					break;
				case 233:
					sizeX = ((Main.tile[x, num3].frameY != 0) ? 2 : 3);
					num4 = 2;
					break;
				case 530:
				case 651:
					sizeX = 3;
					num4 = 2;
					break;
				case 485:
				case 490:
				case 521:
				case 522:
				case 523:
				case 524:
				case 525:
				case 526:
				case 527:
				case 652:
					sizeX = 2;
					num4 = 2;
					break;
				case 489:
					sizeX = 2;
					num4 = 3;
					break;
				case 493:
					sizeX = 1;
					num4 = 2;
					break;
				case 519:
					sizeX = 1;
					num4 = ClimbCatTail(x, num3);
					num3 -= num4 - 1;
					break;
				}
				DrawMultiTileGrassInWind(unscaledPosition, zero, x, num3, sizeX, num4);
			}
		}
	}

	private int ClimbCatTail(int originx, int originy)
	{
		int num = 0;
		int num2 = originy;
		while (num2 > 10)
		{
			Tile tile = Main.tile[originx, num2];
			if (!tile.active() || tile.type != 519)
			{
				break;
			}
			if (tile.frameX >= 180)
			{
				num++;
				break;
			}
			num2--;
			num++;
		}
		return num;
	}

	private void DrawMultiTileVines()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
		Vector2 zero = Vector2.Zero;
		int num = 5;
		int num2 = _specialsCount[num];
		for (int i = 0; i < num2; i++)
		{
			Point val = _specialPositions[num][i];
			int x = val.X;
			int y = val.Y;
			int sizeX = 1;
			int sizeY = 1;
			Tile tile = Main.tile[x, y];
			if (tile != null && tile.active())
			{
				switch (Main.tile[x, y].type)
				{
				case 34:
					sizeX = 3;
					sizeY = 3;
					break;
				case 454:
					sizeX = 4;
					sizeY = 3;
					break;
				case 42:
				case 270:
				case 271:
				case 572:
				case 581:
				case 660:
					sizeX = 1;
					sizeY = 2;
					break;
				case 91:
					sizeX = 1;
					sizeY = 3;
					break;
				case 95:
				case 126:
				case 444:
					sizeX = 2;
					sizeY = 2;
					break;
				case 465:
				case 591:
				case 592:
					sizeX = 2;
					sizeY = 3;
					break;
				}
				DrawMultiTileVinesInWind(unscaledPosition, zero, x, y, sizeX, sizeY);
			}
		}
	}

	private void DrawVines()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
		Vector2 zero = Vector2.Zero;
		int num = 6;
		int num2 = _specialsCount[num];
		for (int i = 0; i < num2; i++)
		{
			Point val = _specialPositions[num][i];
			int x = val.X;
			int y = val.Y;
			DrawVineStrip(unscaledPosition, zero, x, y);
		}
	}

	private void DrawReverseVines()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
		Vector2 zero = Vector2.Zero;
		int num = 9;
		int num2 = _specialsCount[num];
		for (int i = 0; i < num2; i++)
		{
			Point val = _specialPositions[num][i];
			int x = val.X;
			int y = val.Y;
			DrawRisingVineStrip(unscaledPosition, zero, x, y);
		}
	}

	private void DrawMultiTileGrassInWind(Vector2 screenPosition, Vector2 offSet, int topLeftX, int topLeftY, int sizeX, int sizeY)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		float windCycle = GetWindCycle(topLeftX, topLeftY, _sunflowerWindCounter);
		new Vector2((float)(sizeX * 16) * 0.5f, (float)(sizeY * 16));
		Vector2 vector = new Vector2((float)(topLeftX * 16 - (int)screenPosition.X) + (float)sizeX * 16f * 0.5f, (float)(topLeftY * 16 - (int)screenPosition.Y + 16 * sizeY)) + offSet;
		float num = 0.07f;
		int type = Main.tile[topLeftX, topLeftY].type;
		Texture2D texture2D = null;
		Color color = Color.Transparent;
		bool flag = InAPlaceWithWind(topLeftX, topLeftY, sizeX, sizeY);
		switch (type)
		{
		case 27:
			texture2D = TextureAssets.Flames[14].Value;
			color = Color.White;
			break;
		case 519:
			flag = InAPlaceWithWind(topLeftX, topLeftY, sizeX, 1);
			break;
		default:
			num = 0.15f;
			break;
		case 521:
		case 522:
		case 523:
		case 524:
		case 525:
		case 526:
		case 527:
			num = 0f;
			flag = false;
			break;
		}
		Vector2 vector3 = default(Vector2);
		for (int i = topLeftX; i < topLeftX + sizeX; i++)
		{
			for (int j = topLeftY; j < topLeftY + sizeY; j++)
			{
				Tile tile = Main.tile[i, j];
				ushort type2 = tile.type;
				if (type2 != type || !IsVisible(tile))
				{
					continue;
				}
				Math.Abs(((float)(i - topLeftX) + 0.5f) / (float)sizeX - 0.5f);
				short tileFrameX = tile.frameX;
				short tileFrameY = tile.frameY;
				float num2 = 1f - (float)(j - topLeftY + 1) / (float)sizeY;
				if (num2 == 0f)
				{
					num2 = 0.1f;
				}
				if (!flag)
				{
					num2 = 0f;
				}
				GetTileDrawData(i, j, tile, type2, ref tileFrameX, ref tileFrameY, out var tileWidth, out var tileHeight, out var tileTop, out var halfBrickHeight, out var addFrX, out var addFrY, out var tileSpriteEffect, out var _, out var _, out var _);
				bool flag2 = _rand.Next(4) == 0;
				Color tileLight = Lighting.GetColor(i, j);
				DrawAnimatedTile_AdjustForVisionChangers(i, j, tile, type2, tileFrameX, tileFrameY, ref tileLight, flag2);
				tileLight = DrawTiles_GetLightOverride(j, i, tile, type2, tileFrameX, tileFrameY, tileLight);
				if (_isActiveAndNotPaused && flag2)
				{
					DrawTiles_EmitParticles(j, i, tile, type2, tileFrameX, tileFrameY, tileLight);
				}
				Vector2 vector2 = new Vector2((float)(i * 16 - (int)screenPosition.X), (float)(j * 16 - (int)screenPosition.Y + tileTop)) + offSet;
				if (tile.type == 493 && tile.frameY == 0)
				{
					if (Main.WindForVisuals >= 0f)
					{
						tileSpriteEffect = (SpriteEffects)(tileSpriteEffect ^ 1);
					}
					if (!((Enum)tileSpriteEffect).HasFlag((Enum)(object)(SpriteEffects)1))
					{
						vector2.X -= 6f;
					}
					else
					{
						vector2.X += 6f;
					}
				}
				((Vector2)(ref vector3))._002Ector(windCycle * 1f, Math.Abs(windCycle) * 2f * num2);
				Vector2 origin = vector - vector2;
				Texture2D tileDrawTexture = GetTileDrawTexture(tile, i, j);
				if (tileDrawTexture != null)
				{
					Main.spriteBatch.Draw(tileDrawTexture, vector + new Vector2(0f, vector3.Y), (Rectangle?)new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight), tileLight, windCycle * num * num2, origin, 1f, tileSpriteEffect, 0f);
					if (texture2D != null)
					{
						Main.spriteBatch.Draw(texture2D, vector + new Vector2(0f, vector3.Y), (Rectangle?)new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight), color, windCycle * num * num2, origin, 1f, tileSpriteEffect, 0f);
					}
				}
			}
		}
	}

	private void DrawVineStrip(Vector2 screenPosition, Vector2 offSet, int x, int startY)
	{
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		int num2 = 0;
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)(x * 16 + 8), (float)(startY * 16 - 2));
		float amount = Math.Abs(Main.WindForVisuals) / 1.2f;
		amount = MathHelper.Lerp(0.2f, 1f, amount);
		float num3 = -0.08f * amount;
		float windCycle = GetWindCycle(x, startY, _vineWindCounter);
		float num4 = 0f;
		float num5 = 0f;
		for (int i = startY; i < Main.maxTilesY - 10; i++)
		{
			Tile tile = Main.tile[x, i];
			if (tile == null)
			{
				continue;
			}
			ushort type = tile.type;
			if (!tile.active() || !TileID.Sets.VineThreads[type])
			{
				break;
			}
			num++;
			if (num2 >= 5)
			{
				num3 += 0.0075f * amount;
			}
			if (num2 >= 2)
			{
				num3 += 0.0025f;
			}
			if (Main.remixWorld)
			{
				if (WallID.Sets.AllowsWind[tile.wall] && (double)i > Main.worldSurface)
				{
					num2++;
				}
			}
			else if (WallID.Sets.AllowsWind[tile.wall] && (double)i < Main.worldSurface)
			{
				num2++;
			}
			float windGridPush = GetWindGridPush(x, i, 20, 0.01f);
			num4 = ((windGridPush != 0f || num5 == 0f) ? (num4 - windGridPush) : (num4 * -0.78f));
			num5 = windGridPush;
			short tileFrameX = tile.frameX;
			short tileFrameY = tile.frameY;
			Color color = Lighting.GetColor(x, i);
			GetTileDrawData(x, i, tile, type, ref tileFrameX, ref tileFrameY, out var tileWidth, out var tileHeight, out var tileTop, out var halfBrickHeight, out var addFrX, out var addFrY, out var tileSpriteEffect, out var glowTexture, out var glowSourceRect, out var glowColor);
			Vector2 position = new Vector2((float)(-(int)screenPosition.X), (float)(-(int)screenPosition.Y)) + offSet + vector;
			if (tile.fullbrightBlock())
			{
				color = Color.White;
			}
			float num6 = (float)num2 * num3 * windCycle + num4;
			if (_localPlayer.biomeSight)
			{
				Color sightColor = Color.White;
				if (Main.IsTileBiomeSightable(x, i, type, tileFrameX, tileFrameY, ref sightColor))
				{
					if (((Color)(ref color)).R < ((Color)(ref sightColor)).R)
					{
						((Color)(ref color)).R = ((Color)(ref sightColor)).R;
					}
					if (((Color)(ref color)).G < ((Color)(ref sightColor)).G)
					{
						((Color)(ref color)).G = ((Color)(ref sightColor)).G;
					}
					if (((Color)(ref color)).B < ((Color)(ref sightColor)).B)
					{
						((Color)(ref color)).B = ((Color)(ref sightColor)).B;
					}
					if (_isActiveAndNotPaused && _rand.Next(480) == 0)
					{
						Color newColor = sightColor;
						int num7 = Dust.NewDust(new Vector2((float)(x * 16), (float)(i * 16)), 16, 16, 267, 0f, 0f, 150, newColor, 0.3f);
						_dust[num7].noGravity = true;
						_dust[num7].fadeIn = 1f;
						Dust obj = _dust[num7];
						obj.velocity *= 0.1f;
						_dust[num7].noLightEmittence = true;
					}
				}
			}
			Texture2D tileDrawTexture = GetTileDrawTexture(tile, x, i);
			if (tileDrawTexture == null)
			{
				break;
			}
			if (IsVisible(tile))
			{
				Main.spriteBatch.Draw(tileDrawTexture, position, (Rectangle?)new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight), color, num6, new Vector2((float)(tileWidth / 2), (float)(halfBrickHeight - tileTop)), 1f, tileSpriteEffect, 0f);
				if (glowTexture != null)
				{
					Main.spriteBatch.Draw(glowTexture, position, (Rectangle?)glowSourceRect, glowColor, num6, new Vector2((float)(tileWidth / 2), (float)(halfBrickHeight - tileTop)), 1f, tileSpriteEffect, 0f);
				}
			}
			vector += (num6 + (float)Math.PI / 2f).ToRotationVector2() * 16f;
		}
	}

	private void DrawRisingVineStrip(Vector2 screenPosition, Vector2 offSet, int x, int startY)
	{
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		int num2 = 0;
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)(x * 16 + 8), (float)(startY * 16 + 16 + 2));
		float amount = Math.Abs(Main.WindForVisuals) / 1.2f;
		amount = MathHelper.Lerp(0.2f, 1f, amount);
		float num3 = -0.08f * amount;
		float windCycle = GetWindCycle(x, startY, _vineWindCounter);
		float num4 = 0f;
		float num5 = 0f;
		for (int num6 = startY; num6 > 10; num6--)
		{
			Tile tile = Main.tile[x, num6];
			if (tile != null)
			{
				ushort type = tile.type;
				if (!tile.active() || !TileID.Sets.ReverseVineThreads[type])
				{
					break;
				}
				num++;
				if (num2 >= 5)
				{
					num3 += 0.0075f * amount;
				}
				if (num2 >= 2)
				{
					num3 += 0.0025f;
				}
				if (WallID.Sets.AllowsWind[tile.wall] && (double)num6 < Main.worldSurface)
				{
					num2++;
				}
				float windGridPush = GetWindGridPush(x, num6, 40, -0.004f);
				num4 = ((windGridPush != 0f || num5 == 0f) ? (num4 - windGridPush) : (num4 * -0.78f));
				num5 = windGridPush;
				short tileFrameX = tile.frameX;
				short tileFrameY = tile.frameY;
				Color color = Lighting.GetColor(x, num6);
				GetTileDrawData(x, num6, tile, type, ref tileFrameX, ref tileFrameY, out var tileWidth, out var tileHeight, out var tileTop, out var halfBrickHeight, out var addFrX, out var addFrY, out var tileSpriteEffect, out var _, out var _, out var _);
				Vector2 position = new Vector2((float)(-(int)screenPosition.X), (float)(-(int)screenPosition.Y)) + offSet + vector;
				float num7 = (float)num2 * (0f - num3) * windCycle + num4;
				Texture2D tileDrawTexture = GetTileDrawTexture(tile, x, num6);
				if (tileDrawTexture == null)
				{
					break;
				}
				if (IsVisible(tile))
				{
					Main.spriteBatch.Draw(tileDrawTexture, position, (Rectangle?)new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight), color, num7, new Vector2((float)(tileWidth / 2), (float)(halfBrickHeight - tileTop + tileHeight)), 1f, tileSpriteEffect, 0f);
				}
				vector += (num7 - (float)Math.PI / 2f).ToRotationVector2() * 16f;
			}
		}
	}

	private float GetAverageWindGridPush(int topLeftX, int topLeftY, int sizeX, int sizeY, int totalPushTime, float pushForcePerFrame)
	{
		float num = 0f;
		int num2 = 0;
		for (int i = 0; i < sizeX; i++)
		{
			for (int j = 0; j < sizeY; j++)
			{
				float windGridPush = GetWindGridPush(topLeftX + i, topLeftY + j, totalPushTime, pushForcePerFrame);
				if (windGridPush != 0f)
				{
					num += windGridPush;
					num2++;
				}
			}
		}
		if (num2 == 0)
		{
			return 0f;
		}
		return num / (float)num2;
	}

	private float GetHighestWindGridPushComplex(int topLeftX, int topLeftY, int sizeX, int sizeY, int totalPushTime, float pushForcePerFrame, int loops, bool swapLoopDir)
	{
		float result = 0f;
		int num = int.MaxValue;
		for (int i = 0; i < 1; i++)
		{
			for (int j = 0; j < sizeY; j++)
			{
				_windGrid.GetWindTime(topLeftX + i + sizeX / 2, topLeftY + j, totalPushTime, out var windTimeLeft, out var _, out var _);
				float windGridPushComplex = GetWindGridPushComplex(topLeftX + i, topLeftY + j, totalPushTime, pushForcePerFrame, loops, swapLoopDir);
				if (windTimeLeft < num && windTimeLeft != 0)
				{
					result = windGridPushComplex;
					num = windTimeLeft;
				}
			}
		}
		return result;
	}

	private void DrawMultiTileVinesInWind(Vector2 screenPosition, Vector2 offSet, int topLeftX, int topLeftY, int sizeX, int sizeY)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_0606: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0719: Unknown result type (might be due to invalid IL or missing references)
		//IL_071e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0743: Unknown result type (might be due to invalid IL or missing references)
		//IL_0745: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0770: Unknown result type (might be due to invalid IL or missing references)
		//IL_077e: Unknown result type (might be due to invalid IL or missing references)
		//IL_078a: Unknown result type (might be due to invalid IL or missing references)
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0790: Unknown result type (might be due to invalid IL or missing references)
		//IL_0795: Unknown result type (might be due to invalid IL or missing references)
		//IL_0797: Unknown result type (might be due to invalid IL or missing references)
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_079b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0764: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Unknown result type (might be due to invalid IL or missing references)
		//IL_0897: Unknown result type (might be due to invalid IL or missing references)
		//IL_089e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0860: Unknown result type (might be due to invalid IL or missing references)
		//IL_0865: Unknown result type (might be due to invalid IL or missing references)
		//IL_086e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0870: Unknown result type (might be due to invalid IL or missing references)
		//IL_0877: Unknown result type (might be due to invalid IL or missing references)
		//IL_087b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0882: Unknown result type (might be due to invalid IL or missing references)
		//IL_0963: Unknown result type (might be due to invalid IL or missing references)
		//IL_0965: Unknown result type (might be due to invalid IL or missing references)
		//IL_096c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0970: Unknown result type (might be due to invalid IL or missing references)
		//IL_0977: Unknown result type (might be due to invalid IL or missing references)
		//IL_0915: Unknown result type (might be due to invalid IL or missing references)
		//IL_091a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_0927: Unknown result type (might be due to invalid IL or missing references)
		//IL_0929: Unknown result type (might be due to invalid IL or missing references)
		//IL_092b: Unknown result type (might be due to invalid IL or missing references)
		//IL_092d: Unknown result type (might be due to invalid IL or missing references)
		//IL_092f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0938: Unknown result type (might be due to invalid IL or missing references)
		//IL_093a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0941: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Unknown result type (might be due to invalid IL or missing references)
		//IL_094c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2e: Unknown result type (might be due to invalid IL or missing references)
		float windCycle = GetWindCycle(topLeftX, topLeftY, _sunflowerWindCounter);
		float num = windCycle;
		int totalPushTime = 60;
		float pushForcePerFrame = 1.26f;
		float highestWindGridPushComplex = GetHighestWindGridPushComplex(topLeftX, topLeftY, sizeX, sizeY, totalPushTime, pushForcePerFrame, 3, swapLoopDir: true);
		windCycle += highestWindGridPushComplex;
		new Vector2((float)(sizeX * 16) * 0.5f, 0f);
		Vector2 vector = new Vector2((float)(topLeftX * 16 - (int)screenPosition.X) + (float)sizeX * 16f * 0.5f, (float)(topLeftY * 16 - (int)screenPosition.Y)) + offSet;
		float num2 = 0.07f;
		Tile tile = Main.tile[topLeftX, topLeftY];
		int type = tile.type;
		Vector2 vector2 = default(Vector2);
		((Vector2)(ref vector2))._002Ector(0f, -2f);
		vector += vector2;
		bool num8;
		if (type == 465 || (uint)(type - 591) <= 1u)
		{
			if (WorldGen.IsBelowANonHammeredPlatform(topLeftX, topLeftY))
			{
				num8 = WorldGen.IsBelowANonHammeredPlatform(topLeftX + 1, topLeftY);
				goto IL_00fb;
			}
		}
		else if (sizeX == 1)
		{
			num8 = WorldGen.IsBelowANonHammeredPlatform(topLeftX, topLeftY);
			goto IL_00fb;
		}
		goto IL_011d;
		IL_00fb:
		if (num8)
		{
			vector.Y -= 8f;
			vector2.Y -= 8f;
		}
		goto IL_011d;
		IL_011d:
		Texture2D texture2D = null;
		Color color = Color.Transparent;
		float? num3 = null;
		float num4 = 1f;
		float num5 = -4f;
		bool flag2 = false;
		num2 = 0.15f;
		switch (type)
		{
		case 34:
		case 126:
			num3 = 1f;
			num5 = 0f;
			switch (tile.frameY / 54 + tile.frameX / 108 * 37)
			{
			case 9:
				num3 = null;
				num5 = -1f;
				flag2 = true;
				num2 *= 0.3f;
				break;
			case 11:
				num2 *= 0.5f;
				break;
			case 12:
				num3 = null;
				num5 = -1f;
				break;
			case 18:
				num3 = null;
				num5 = -1f;
				break;
			case 21:
				num3 = null;
				num5 = -1f;
				break;
			case 23:
				num3 = 0f;
				break;
			case 25:
				num3 = null;
				num5 = -1f;
				flag2 = true;
				break;
			case 32:
				num2 *= 0.5f;
				break;
			case 33:
				num2 *= 0.5f;
				break;
			case 35:
				num3 = 0f;
				break;
			case 36:
				num3 = null;
				num5 = -1f;
				flag2 = true;
				break;
			case 37:
				num3 = null;
				num5 = -1f;
				flag2 = true;
				num2 *= 0.5f;
				break;
			case 39:
				num3 = null;
				num5 = -1f;
				flag2 = true;
				break;
			case 40:
			case 41:
			case 42:
			case 43:
				num3 = null;
				num5 = -2f;
				flag2 = true;
				num2 *= 0.5f;
				break;
			case 44:
				num3 = null;
				num5 = -3f;
				break;
			}
			break;
		case 42:
			num3 = 1f;
			num5 = 0f;
			switch (tile.frameY / 36)
			{
			case 0:
				num3 = null;
				num5 = -1f;
				break;
			case 9:
				num3 = 0f;
				break;
			case 12:
				num3 = null;
				num5 = -1f;
				break;
			case 14:
				num3 = null;
				num5 = -1f;
				break;
			case 28:
				num3 = null;
				num5 = -1f;
				break;
			case 30:
				num3 = 0f;
				break;
			case 32:
				num3 = 0f;
				break;
			case 33:
				num3 = 0f;
				break;
			case 34:
				num3 = null;
				num5 = -1f;
				break;
			case 35:
				num3 = 0f;
				break;
			case 38:
				num3 = null;
				num5 = -1f;
				break;
			case 39:
				num3 = null;
				num5 = -1f;
				flag2 = true;
				break;
			case 40:
			case 41:
			case 42:
			case 43:
				num3 = 0f;
				num3 = null;
				num5 = -1f;
				flag2 = true;
				break;
			}
			break;
		case 95:
		case 270:
		case 271:
		case 444:
		case 454:
		case 572:
		case 581:
		case 660:
			num3 = 1f;
			num5 = 0f;
			break;
		case 591:
			num4 = 0.5f;
			num5 = -2f;
			break;
		case 592:
			num4 = 0.5f;
			num5 = -2f;
			texture2D = TextureAssets.GlowMask[294].Value;
			((Color)(ref color))._002Ector(255, 255, 255, 0);
			break;
		}
		if (flag2)
		{
			vector += new Vector2(0f, 16f);
		}
		num2 *= -1f;
		if (!InAPlaceWithWind(topLeftX, topLeftY, sizeX, sizeY))
		{
			windCycle -= num;
		}
		ulong num6 = 0uL;
		Vector2 vector4 = default(Vector2);
		Rectangle rectangle = default(Rectangle);
		for (int i = topLeftX; i < topLeftX + sizeX; i++)
		{
			for (int j = topLeftY; j < topLeftY + sizeY; j++)
			{
				Tile tile2 = Main.tile[i, j];
				ushort type2 = tile2.type;
				if (type2 != type || !IsVisible(tile2))
				{
					continue;
				}
				Math.Abs(((float)(i - topLeftX) + 0.5f) / (float)sizeX - 0.5f);
				short tileFrameX = tile2.frameX;
				short tileFrameY = tile2.frameY;
				float num7 = (float)(j - topLeftY + 1) / (float)sizeY;
				if (num7 == 0f)
				{
					num7 = 0.1f;
				}
				if (num3.HasValue)
				{
					num7 = num3.Value;
				}
				if (flag2 && j == topLeftY)
				{
					num7 = 0f;
				}
				GetTileDrawData(i, j, tile2, type2, ref tileFrameX, ref tileFrameY, out var tileWidth, out var tileHeight, out var tileTop, out var halfBrickHeight, out var addFrX, out var addFrY, out var tileSpriteEffect, out var _, out var _, out var _);
				bool flag3 = _rand.Next(4) == 0;
				Color tileLight = Lighting.GetColor(i, j);
				DrawAnimatedTile_AdjustForVisionChangers(i, j, tile2, type2, tileFrameX, tileFrameY, ref tileLight, flag3);
				tileLight = DrawTiles_GetLightOverride(j, i, tile2, type2, tileFrameX, tileFrameY, tileLight);
				if (_isActiveAndNotPaused && flag3)
				{
					DrawTiles_EmitParticles(j, i, tile2, type2, tileFrameX, tileFrameY, tileLight);
				}
				Vector2 vector3 = new Vector2((float)(i * 16 - (int)screenPosition.X), (float)(j * 16 - (int)screenPosition.Y + tileTop)) + offSet;
				vector3 += vector2;
				((Vector2)(ref vector4))._002Ector(windCycle * num4, Math.Abs(windCycle) * num5 * num7);
				Vector2 vector5 = vector - vector3;
				Texture2D tileDrawTexture = GetTileDrawTexture(tile2, i, j);
				if (tileDrawTexture != null)
				{
					Vector2 vector6 = vector + new Vector2(0f, vector4.Y);
					((Rectangle)(ref rectangle))._002Ector(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight);
					float rotation = windCycle * num2 * num7;
					if (type2 == 660 && j == topLeftY + sizeY - 1)
					{
						Texture2D value = TextureAssets.Extra[260].Value;
						_ = ((float)((i + j) % 200) * 0.11f + (float)Main.timeForVisualEffects / 360f) % 1f;
						Color white = Color.White;
						Main.spriteBatch.Draw(value, vector6, (Rectangle?)rectangle, white, rotation, vector5, 1f, tileSpriteEffect, 0f);
					}
					Main.spriteBatch.Draw(tileDrawTexture, vector6, (Rectangle?)rectangle, tileLight, rotation, vector5, 1f, tileSpriteEffect, 0f);
					if (type2 == 660 && j == topLeftY + sizeY - 1)
					{
						Texture2D value2 = TextureAssets.Extra[260].Value;
						Color color2 = Main.hslToRgb(((float)((i + j) % 200) * 0.11f + (float)Main.timeForVisualEffects / 360f) % 1f, 1f, 0.8f);
						((Color)(ref color2)).A = 127;
						Rectangle value3 = rectangle;
						Vector2 position = vector6;
						Vector2 origin = vector5;
						Main.spriteBatch.Draw(value2, position, (Rectangle?)value3, color2, rotation, origin, 1f, tileSpriteEffect, 0f);
					}
					if (texture2D != null)
					{
						Main.spriteBatch.Draw(texture2D, vector6, (Rectangle?)rectangle, color, rotation, vector5, 1f, tileSpriteEffect, 0f);
					}
					TileFlameData tileFlameData = GetTileFlameData(i, j, type2, tileFrameY);
					if (num6 == 0L)
					{
						num6 = tileFlameData.flameSeed;
					}
					tileFlameData.flameSeed = num6;
					for (int k = 0; k < tileFlameData.flameCount; k++)
					{
						float x = (float)Utils.RandomInt(ref tileFlameData.flameSeed, tileFlameData.flameRangeXMin, tileFlameData.flameRangeXMax) * tileFlameData.flameRangeMultX;
						float y = (float)Utils.RandomInt(ref tileFlameData.flameSeed, tileFlameData.flameRangeYMin, tileFlameData.flameRangeYMax) * tileFlameData.flameRangeMultY;
						Main.spriteBatch.Draw(tileFlameData.flameTexture, vector6 + new Vector2(x, y), (Rectangle?)rectangle, tileFlameData.flameColor, rotation, vector5, 1f, tileSpriteEffect, 0f);
					}
				}
			}
		}
	}

	private void EmitAlchemyHerbParticles(int j, int i, int style)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		Color newColor = default(Color);
		if (style == 0 && _rand.Next(100) == 0)
		{
			Vector2 position = new Vector2((float)(i * 16), (float)(j * 16 - 4));
			newColor = default(Color);
			int num = Dust.NewDust(position, 16, 16, 19, 0f, 0f, 160, newColor, 0.1f);
			_dust[num].velocity.X /= 2f;
			_dust[num].velocity.Y /= 2f;
			_dust[num].noGravity = true;
			_dust[num].fadeIn = 1f;
		}
		if (style == 1 && _rand.Next(100) == 0)
		{
			Vector2 position2 = new Vector2((float)(i * 16), (float)(j * 16));
			newColor = default(Color);
			Dust.NewDust(position2, 16, 16, 41, 0f, 0f, 250, newColor, 0.8f);
		}
		if (style == 3)
		{
			if (_rand.Next(200) == 0)
			{
				Vector2 position3 = new Vector2((float)(i * 16), (float)(j * 16));
				newColor = default(Color);
				int num2 = Dust.NewDust(position3, 16, 16, 14, 0f, 0f, 100, newColor, 0.2f);
				_dust[num2].fadeIn = 1.2f;
			}
			if (_rand.Next(75) == 0)
			{
				Vector2 position4 = new Vector2((float)(i * 16), (float)(j * 16));
				newColor = default(Color);
				int num3 = Dust.NewDust(position4, 16, 16, 27, 0f, 0f, 100, newColor);
				_dust[num3].velocity.X /= 2f;
				_dust[num3].velocity.Y /= 2f;
			}
		}
		if (style == 4 && _rand.Next(150) == 0)
		{
			Vector2 position5 = new Vector2((float)(i * 16), (float)(j * 16));
			newColor = default(Color);
			int num4 = Dust.NewDust(position5, 16, 8, 16, 0f, 0f, 0, newColor);
			_dust[num4].velocity.X /= 3f;
			_dust[num4].velocity.Y /= 3f;
			_dust[num4].velocity.Y -= 0.7f;
			_dust[num4].alpha = 50;
			_dust[num4].scale *= 0.1f;
			_dust[num4].fadeIn = 0.9f;
			_dust[num4].noGravity = true;
		}
		if (style == 5 && _rand.Next(40) == 0)
		{
			Vector2 position6 = new Vector2((float)(i * 16), (float)(j * 16 - 6));
			newColor = default(Color);
			int num5 = Dust.NewDust(position6, 16, 16, 6, 0f, 0f, 0, newColor, 1.5f);
			_dust[num5].velocity.Y -= 2f;
			_dust[num5].noGravity = true;
		}
		if (style == 6 && _rand.Next(30) == 0)
		{
			((Color)(ref newColor))._002Ector(50, 255, 255, 255);
			int num6 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 43, 0f, 0f, 254, newColor, 0.5f);
			Dust obj = _dust[num6];
			obj.velocity *= 0f;
		}
	}

	private bool IsAlchemyPlantHarvestable(int style)
	{
		if (style == 0 && Main.dayTime)
		{
			return true;
		}
		if (style == 1 && !Main.dayTime)
		{
			return true;
		}
		if (style == 3 && !Main.dayTime && (Main.bloodMoon || Main.moonPhase == 0))
		{
			return true;
		}
		if (style == 4 && (Main.raining || Main.cloudAlpha > 0f))
		{
			return true;
		}
		if (style == 5 && !Main.raining && Main.time > 40500.0)
		{
			return true;
		}
		return false;
	}

	/// <summary>
	/// Checks if a tile at the given coordinates counts towards tile coloring from the Dangersense buff.
	/// <br />Vanilla only uses Main.LocalPlayer for <paramref name="player" />
	/// </summary>
	public static bool IsTileDangerous(int tileX, int tileY, Player player)
	{
		Tile tile = Main.tile[tileX, tileY];
		return IsTileDangerous(tileX, tileY, player, tile, tile.type);
	}
}
