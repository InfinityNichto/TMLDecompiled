using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Terraria;

public class SceneMetrics
{
	public static int ShimmerTileThreshold = 300;

	public static int CorruptionTileThreshold = 300;

	public static int CorruptionTileMax = 1000;

	public static int CrimsonTileThreshold = 300;

	public static int CrimsonTileMax = 1000;

	public static int HallowTileThreshold = 125;

	public static int HallowTileMax = 600;

	public static int JungleTileThreshold = 140;

	public static int JungleTileMax = 700;

	public static int SnowTileThreshold = 1500;

	public static int SnowTileMax = 6000;

	public static int DesertTileThreshold = 1500;

	public static int MushroomTileThreshold = 100;

	public static int MushroomTileMax = 160;

	public static int MeteorTileThreshold = 75;

	public static int GraveyardTileMax = 36;

	public static int GraveyardTileMin = 16;

	public static int GraveyardTileThreshold = 28;

	public bool CanPlayCreditsRoll;

	public bool[] NPCBannerBuff = new bool[290];

	public bool hasBanner;

	internal int[] _tileCounts = new int[TileLoader.TileCount];

	private readonly int[] _liquidCounts = new int[LiquidID.Count];

	private readonly List<Point> _oreFinderTileLocations = new List<Point>(512);

	public int bestOre;

	public Point? ClosestOrePosition { get; set; }

	public int ShimmerTileCount { get; set; }

	public int EvilTileCount { get; set; }

	public int HolyTileCount { get; set; }

	public int HoneyBlockCount { get; set; }

	public int ActiveMusicBox { get; set; }

	public int SandTileCount { get; set; }

	public int MushroomTileCount { get; set; }

	public int SnowTileCount { get; set; }

	public int WaterCandleCount { get; set; }

	public int PeaceCandleCount { get; set; }

	public int ShadowCandleCount { get; set; }

	public int PartyMonolithCount { get; set; }

	public int MeteorTileCount { get; set; }

	public int BloodTileCount { get; set; }

	public int JungleTileCount { get; set; }

	public int DungeonTileCount { get; set; }

	public bool HasSunflower { get; set; }

	public bool HasGardenGnome { get; set; }

	public bool HasClock { get; set; }

	public bool HasCampfire { get; set; }

	public bool HasStarInBottle { get; set; }

	public bool HasHeartLantern { get; set; }

	public int ActiveFountainColor { get; set; }

	public int ActiveMonolithType { get; set; }

	public bool BloodMoonMonolith { get; set; }

	public bool MoonLordMonolith { get; set; }

	public bool EchoMonolith { get; set; }

	public int ShimmerMonolithState { get; set; }

	public bool HasCatBast { get; set; }

	public int GraveyardTileCount { get; set; }

	public bool EnoughTilesForShimmer => ShimmerTileCount >= ShimmerTileThreshold;

	public bool EnoughTilesForJungle => JungleTileCount >= JungleTileThreshold;

	public bool EnoughTilesForHallow => HolyTileCount >= HallowTileThreshold;

	public bool EnoughTilesForSnow => SnowTileCount >= SnowTileThreshold;

	public bool EnoughTilesForGlowingMushroom => MushroomTileCount >= MushroomTileThreshold;

	public bool EnoughTilesForDesert => SandTileCount >= DesertTileThreshold;

	public bool EnoughTilesForCorruption => EvilTileCount >= CorruptionTileThreshold;

	public bool EnoughTilesForCrimson => BloodTileCount >= CrimsonTileThreshold;

	public bool EnoughTilesForMeteor => MeteorTileCount >= MeteorTileThreshold;

	public bool EnoughTilesForGraveyard => GraveyardTileCount >= GraveyardTileThreshold;

	public SceneMetrics()
	{
		Reset();
	}

	public void ScanAndExportToMain(SceneMetricsScanSettings settings)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		Reset();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		if (settings.ScanOreFinderData)
		{
			_oreFinderTileLocations.Clear();
		}
		SystemLoader.ResetNearbyTileEffects();
		if (settings.BiomeScanCenterPositionInWorld.HasValue)
		{
			Point point = settings.BiomeScanCenterPositionInWorld.Value.ToTileCoordinates();
			Rectangle tileRectangle = default(Rectangle);
			((Rectangle)(ref tileRectangle))._002Ector(point.X - Main.buffScanAreaWidth / 2, point.Y - Main.buffScanAreaHeight / 2, Main.buffScanAreaWidth, Main.buffScanAreaHeight);
			tileRectangle = WorldUtils.ClampToWorld(tileRectangle);
			for (int i = ((Rectangle)(ref tileRectangle)).Left; i < ((Rectangle)(ref tileRectangle)).Right; i++)
			{
				for (int j = ((Rectangle)(ref tileRectangle)).Top; j < ((Rectangle)(ref tileRectangle)).Bottom; j++)
				{
					if (!((Rectangle)(ref tileRectangle)).Contains(i, j))
					{
						continue;
					}
					Tile tile = Main.tile[i, j];
					if (tile == null)
					{
						continue;
					}
					if (!tile.active())
					{
						if (tile.liquid > 0)
						{
							_liquidCounts[tile.liquidType()]++;
						}
						continue;
					}
					((Rectangle)(ref tileRectangle)).Contains(i, j);
					if (!TileID.Sets.isDesertBiomeSand[tile.type] || !WorldGen.oceanDepths(i, j))
					{
						_tileCounts[tile.type]++;
					}
					if (tile.type == 215 && tile.frameY < 36)
					{
						HasCampfire = true;
					}
					if (tile.type == 49 && tile.frameX < 18)
					{
						num++;
					}
					if (tile.type == 372 && tile.frameX < 18)
					{
						num2++;
					}
					if (tile.type == 646 && tile.frameX < 18)
					{
						num3++;
					}
					if (tile.type == 405 && tile.frameX < 54)
					{
						HasCampfire = true;
					}
					if (tile.type == 506 && tile.frameX < 72)
					{
						HasCatBast = true;
					}
					if (tile.type == 42 && tile.frameY >= 324 && tile.frameY <= 358)
					{
						HasHeartLantern = true;
					}
					if (tile.type == 42 && tile.frameY >= 252 && tile.frameY <= 286)
					{
						HasStarInBottle = true;
					}
					if (tile.type == 91 && (tile.frameX >= 396 || tile.frameY >= 54))
					{
						int num4 = tile.frameX / 18 - 21;
						for (int num5 = tile.frameY; num5 >= 54; num5 -= 54)
						{
							num4 += 90;
							num4 += 21;
						}
						int num6 = Item.BannerToItem(num4);
						if (ItemID.Sets.BannerStrength.IndexInRange(num6) && ItemID.Sets.BannerStrength[num6].Enabled)
						{
							NPCBannerBuff[num4] = true;
							hasBanner = true;
						}
					}
					if (settings.ScanOreFinderData && Main.tileOreFinderPriority[tile.type] != 0)
					{
						_oreFinderTileLocations.Add(new Point(i, j));
					}
					TileLoader.NearbyEffects(i, j, tile.type, closer: false);
				}
			}
		}
		if (settings.VisualScanArea.HasValue)
		{
			Rectangle rectangle = WorldUtils.ClampToWorld(settings.VisualScanArea.Value);
			for (int k = ((Rectangle)(ref rectangle)).Left; k < ((Rectangle)(ref rectangle)).Right; k++)
			{
				for (int l = ((Rectangle)(ref rectangle)).Top; l < ((Rectangle)(ref rectangle)).Bottom; l++)
				{
					Tile tile2 = Main.tile[k, l];
					if (tile2 == null || !tile2.active())
					{
						continue;
					}
					if (TileID.Sets.Clock[tile2.type])
					{
						HasClock = true;
					}
					switch (tile2.type)
					{
					case 139:
						if (tile2.frameX >= 36)
						{
							ActiveMusicBox = tile2.frameY / 36;
						}
						break;
					case 207:
						if (tile2.frameY >= 72)
						{
							switch (tile2.frameX / 36)
							{
							case 0:
								ActiveFountainColor = 0;
								break;
							case 1:
								ActiveFountainColor = 12;
								break;
							case 2:
								ActiveFountainColor = 3;
								break;
							case 3:
								ActiveFountainColor = 5;
								break;
							case 4:
								ActiveFountainColor = 2;
								break;
							case 5:
								ActiveFountainColor = 10;
								break;
							case 6:
								ActiveFountainColor = 4;
								break;
							case 7:
								ActiveFountainColor = 9;
								break;
							case 8:
								ActiveFountainColor = 8;
								break;
							case 9:
								ActiveFountainColor = 6;
								break;
							default:
								ActiveFountainColor = -1;
								break;
							}
						}
						break;
					case 410:
						if (tile2.frameY >= 56)
						{
							int activeMonolithType = tile2.frameX / 36;
							ActiveMonolithType = activeMonolithType;
						}
						break;
					case 509:
						if (tile2.frameY >= 56)
						{
							ActiveMonolithType = 4;
						}
						break;
					case 480:
						if (tile2.frameY >= 54)
						{
							BloodMoonMonolith = true;
						}
						break;
					case 657:
						if (tile2.frameY >= 54)
						{
							EchoMonolith = true;
						}
						break;
					case 658:
					{
						int shimmerMonolithState = tile2.frameY / 54;
						ShimmerMonolithState = shimmerMonolithState;
						break;
					}
					}
					if (MusicLoader.tileToMusic.ContainsKey(tile2.type) && MusicLoader.tileToMusic[tile2.type].ContainsKey(tile2.frameY) && tile2.frameX == 36)
					{
						ActiveMusicBox = MusicLoader.tileToMusic[tile2.type][tile2.frameY];
					}
					TileLoader.NearbyEffects(k, l, tile2.type, closer: true);
				}
			}
		}
		WaterCandleCount = num;
		PeaceCandleCount = num2;
		ShadowCandleCount = num3;
		ExportTileCountsToMain();
		CanPlayCreditsRoll = ActiveMusicBox == 85;
		if (settings.ScanOreFinderData)
		{
			UpdateOreFinderData();
		}
		SystemLoader.TileCountsAvailable(_tileCounts);
	}

	private void ExportTileCountsToMain()
	{
		if (_tileCounts[27] > 0)
		{
			HasSunflower = true;
		}
		if (_tileCounts[567] > 0)
		{
			HasGardenGnome = true;
		}
		ShimmerTileCount = _liquidCounts[3];
		HoneyBlockCount = _tileCounts[229];
		MeteorTileCount = _tileCounts[37];
		PartyMonolithCount = _tileCounts[455];
		GraveyardTileCount = _tileCounts[85];
		GraveyardTileCount -= _tileCounts[27] / 2;
		TileLoader.RecountTiles(this);
		if (_tileCounts[27] > 0)
		{
			HasSunflower = true;
		}
		if (GraveyardTileCount > GraveyardTileMin)
		{
			HasSunflower = false;
		}
		if (GraveyardTileCount < 0)
		{
			GraveyardTileCount = 0;
		}
		if (HolyTileCount < 0)
		{
			HolyTileCount = 0;
		}
		if (EvilTileCount < 0)
		{
			EvilTileCount = 0;
		}
		if (BloodTileCount < 0)
		{
			BloodTileCount = 0;
		}
		int holyTileCount = HolyTileCount;
		HolyTileCount -= EvilTileCount;
		HolyTileCount -= BloodTileCount;
		EvilTileCount -= holyTileCount;
		BloodTileCount -= holyTileCount;
		if (HolyTileCount < 0)
		{
			HolyTileCount = 0;
		}
		if (EvilTileCount < 0)
		{
			EvilTileCount = 0;
		}
		if (BloodTileCount < 0)
		{
			BloodTileCount = 0;
		}
	}

	public int GetTileCount(ushort tileId)
	{
		return _tileCounts[tileId];
	}

	public int GetLiquidCount(short liquidType)
	{
		return _liquidCounts[liquidType];
	}

	public void Reset()
	{
		Array.Clear(_tileCounts, 0, _tileCounts.Length);
		Array.Clear(_liquidCounts, 0, _liquidCounts.Length);
		SandTileCount = 0;
		EvilTileCount = 0;
		BloodTileCount = 0;
		GraveyardTileCount = 0;
		MushroomTileCount = 0;
		SnowTileCount = 0;
		HolyTileCount = 0;
		MeteorTileCount = 0;
		JungleTileCount = 0;
		DungeonTileCount = 0;
		HasCampfire = false;
		HasSunflower = false;
		HasGardenGnome = false;
		HasStarInBottle = false;
		HasHeartLantern = false;
		HasClock = false;
		HasCatBast = false;
		ActiveMusicBox = -1;
		WaterCandleCount = 0;
		ActiveFountainColor = -1;
		ActiveMonolithType = -1;
		bestOre = -1;
		BloodMoonMonolith = false;
		MoonLordMonolith = false;
		EchoMonolith = false;
		ShimmerMonolithState = 0;
		Array.Clear(NPCBannerBuff, 0, NPCBannerBuff.Length);
		hasBanner = false;
		CanPlayCreditsRoll = false;
		if (NPCBannerBuff.Length < NPCLoader.NPCCount)
		{
			Array.Resize(ref NPCBannerBuff, NPCLoader.NPCCount);
		}
	}

	private void UpdateOreFinderData()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		int num = -1;
		foreach (Point oreFinderTileLocation in _oreFinderTileLocations)
		{
			Tile tile = Main.tile[oreFinderTileLocation.X, oreFinderTileLocation.Y];
			if (IsValidForOreFinder(tile) && (num < 0 || Main.tileOreFinderPriority[tile.type] > Main.tileOreFinderPriority[num]))
			{
				num = tile.type;
				ClosestOrePosition = oreFinderTileLocation;
			}
		}
		bestOre = num;
	}

	public static bool IsValidForOreFinder(Tile t)
	{
		if (t.type == 227 && (t.frameX < 272 || t.frameX > 374))
		{
			return false;
		}
		if (t.type == 129 && t.frameX < 324)
		{
			return false;
		}
		return Main.tileOreFinderPriority[t.type] > 0;
	}
}
