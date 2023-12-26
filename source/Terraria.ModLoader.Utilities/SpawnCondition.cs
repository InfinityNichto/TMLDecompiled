using System;
using System.Collections.Generic;
using Terraria.GameContent.Events;
using Terraria.ID;

namespace Terraria.ModLoader.Utilities;

/// <summary>
/// This serves as a central class to help modders spawn their NPCs. It's basically the vanilla spawn code if-else chains condensed into objects. See ExampleMod for usages.
/// </summary>
public class SpawnCondition
{
	private Func<NPCSpawnInfo, bool> condition;

	private List<SpawnCondition> children;

	private float blockWeight;

	internal Func<float> WeightFunc;

	private float chance;

	private bool active;

	public static readonly SpawnCondition NebulaTower;

	public static readonly SpawnCondition VortexTower;

	public static readonly SpawnCondition StardustTower;

	public static readonly SpawnCondition SolarTower;

	public static readonly SpawnCondition Sky;

	public static readonly SpawnCondition Invasion;

	public static readonly SpawnCondition GoblinArmy;

	public static readonly SpawnCondition FrostLegion;

	public static readonly SpawnCondition Pirates;

	public static readonly SpawnCondition MartianMadness;

	public static readonly SpawnCondition Bartender;

	public static readonly SpawnCondition SpiderCave;

	public static readonly SpawnCondition DesertCave;

	public static readonly SpawnCondition HardmodeJungleWater;

	public static readonly SpawnCondition HardmodeCrimsonWater;

	public static readonly SpawnCondition Ocean;

	public static readonly SpawnCondition OceanAngler;

	public static readonly SpawnCondition OceanMonster;

	public static readonly SpawnCondition BeachAngler;

	public static readonly SpawnCondition JungleWater;

	public static readonly SpawnCondition CavePiranha;

	public static readonly SpawnCondition CaveJellyfish;

	public static readonly SpawnCondition WaterCritter;

	public static readonly SpawnCondition CorruptWaterCritter;

	public static readonly SpawnCondition OverworldWaterCritter;

	public static readonly SpawnCondition OverworldWaterSurfaceCritter;

	public static readonly SpawnCondition OverworldUnderwaterCritter;

	public static readonly SpawnCondition DefaultWaterCritter;

	public static readonly SpawnCondition BoundCaveNPC;

	public static readonly SpawnCondition TownCritter;

	public static readonly SpawnCondition TownWaterCritter;

	public static readonly SpawnCondition TownOverworldWaterCritter;

	public static readonly SpawnCondition TownOverworldWaterSurfaceCritter;

	public static readonly SpawnCondition TownOverworldUnderwaterCritter;

	public static readonly SpawnCondition TownDefaultWaterCritter;

	public static readonly SpawnCondition TownSnowCritter;

	public static readonly SpawnCondition TownJungleCritter;

	public static readonly SpawnCondition TownGeneralCritter;

	public static readonly SpawnCondition Dungeon;

	public static readonly SpawnCondition DungeonGuardian;

	public static readonly SpawnCondition DungeonNormal;

	public static readonly SpawnCondition Meteor;

	public static readonly SpawnCondition OldOnesArmy;

	public static readonly SpawnCondition FrostMoon;

	public static readonly SpawnCondition PumpkinMoon;

	public static readonly SpawnCondition SolarEclipse;

	public static readonly SpawnCondition HardmodeMushroomWater;

	public static readonly SpawnCondition OverworldMushroom;

	public static readonly SpawnCondition UndergroundMushroom;

	public static readonly SpawnCondition CorruptWorm;

	public static readonly SpawnCondition UndergroundMimic;

	public static readonly SpawnCondition OverworldMimic;

	public static readonly SpawnCondition Wraith;

	public static readonly SpawnCondition HoppinJack;

	public static readonly SpawnCondition DoctorBones;

	public static readonly SpawnCondition LacBeetle;

	public static readonly SpawnCondition WormCritter;

	public static readonly SpawnCondition MouseCritter;

	public static readonly SpawnCondition SnailCritter;

	public static readonly SpawnCondition FrogCritter;

	public static readonly SpawnCondition HardmodeJungle;

	public static readonly SpawnCondition JungleTemple;

	public static readonly SpawnCondition UndergroundJungle;

	public static readonly SpawnCondition SurfaceJungle;

	public static readonly SpawnCondition SandstormEvent;

	public static readonly SpawnCondition Mummy;

	public static readonly SpawnCondition DarkMummy;

	public static readonly SpawnCondition LightMummy;

	public static readonly SpawnCondition OverworldHallow;

	public static readonly SpawnCondition EnchantedSword;

	public static readonly SpawnCondition Crimson;

	public static readonly SpawnCondition Corruption;

	public static readonly SpawnCondition Overworld;

	public static readonly SpawnCondition IceGolem;

	public static readonly SpawnCondition RainbowSlime;

	public static readonly SpawnCondition AngryNimbus;

	public static readonly SpawnCondition MartianProbe;

	public static readonly SpawnCondition OverworldDay;

	public static readonly SpawnCondition OverworldDaySnowCritter;

	public static readonly SpawnCondition OverworldDayGrassCritter;

	public static readonly SpawnCondition OverworldDaySandCritter;

	public static readonly SpawnCondition OverworldMorningBirdCritter;

	public static readonly SpawnCondition OverworldDayBirdCritter;

	public static readonly SpawnCondition KingSlime;

	public static readonly SpawnCondition OverworldDayDesert;

	public static readonly SpawnCondition GoblinScout;

	public static readonly SpawnCondition OverworldDayRain;

	public static readonly SpawnCondition OverworldDaySlime;

	public static readonly SpawnCondition OverworldNight;

	public static readonly SpawnCondition OverworldFirefly;

	public static readonly SpawnCondition OverworldNightMonster;

	public static readonly SpawnCondition Underground;

	public static readonly SpawnCondition Underworld;

	public static readonly SpawnCondition Cavern;

	internal IEnumerable<SpawnCondition> Children => children;

	internal float BlockWeight => blockWeight;

	public float Chance => chance;

	public bool Active => active;

	internal SpawnCondition(Func<NPCSpawnInfo, bool> condition, float blockWeight = 1f)
	{
		this.condition = condition;
		children = new List<SpawnCondition>();
		this.blockWeight = blockWeight;
		NPCSpawnHelper.conditions.Add(this);
	}

	internal SpawnCondition(SpawnCondition parent, Func<NPCSpawnInfo, bool> condition, float blockWeight = 1f)
	{
		this.condition = condition;
		children = new List<SpawnCondition>();
		this.blockWeight = blockWeight;
		parent.children.Add(this);
	}

	internal void Reset()
	{
		chance = 0f;
		active = false;
		foreach (SpawnCondition child in children)
		{
			child.Reset();
		}
	}

	internal void Check(NPCSpawnInfo info, ref float remainingWeight)
	{
		if (WeightFunc != null)
		{
			blockWeight = WeightFunc();
		}
		active = true;
		if (!condition(info))
		{
			return;
		}
		chance = remainingWeight * blockWeight;
		float childWeight = chance;
		foreach (SpawnCondition child in children)
		{
			child.Check(info, ref childWeight);
			if ((double)Math.Abs(childWeight) < 5E-06)
			{
				break;
			}
		}
		remainingWeight -= chance;
	}

	static SpawnCondition()
	{
		NebulaTower = new SpawnCondition((NPCSpawnInfo info) => info.Player.ZoneTowerNebula);
		VortexTower = new SpawnCondition((NPCSpawnInfo info) => info.Player.ZoneTowerVortex);
		StardustTower = new SpawnCondition((NPCSpawnInfo info) => info.Player.ZoneTowerStardust);
		SolarTower = new SpawnCondition((NPCSpawnInfo info) => info.Player.ZoneTowerSolar);
		Sky = new SpawnCondition((NPCSpawnInfo info) => info.Sky);
		Invasion = new SpawnCondition((NPCSpawnInfo info) => info.Invasion);
		GoblinArmy = new SpawnCondition(Invasion, (NPCSpawnInfo info) => Main.invasionType == 1);
		FrostLegion = new SpawnCondition(Invasion, (NPCSpawnInfo info) => Main.invasionType == 2);
		Pirates = new SpawnCondition(Invasion, (NPCSpawnInfo info) => Main.invasionType == 3);
		MartianMadness = new SpawnCondition(Invasion, (NPCSpawnInfo info) => Main.invasionType == 4);
		Bartender = new SpawnCondition((NPCSpawnInfo info) => !NPC.savedBartender && DD2Event.ReadyToFindBartender && !NPC.AnyNPCs(579) && !info.Water, 0.0125f);
		SpiderCave = new SpawnCondition((NPCSpawnInfo info) => GetTile(info).wall == 62 || info.SpiderCave);
		DesertCave = new SpawnCondition((NPCSpawnInfo info) => (WallID.Sets.Conversion.HardenedSand[GetTile(info).wall] || WallID.Sets.Conversion.Sandstone[GetTile(info).wall] || info.DesertCave) && WorldGen.checkUnderground(info.SpawnTileX, info.SpawnTileY));
		HardmodeJungleWater = new SpawnCondition((NPCSpawnInfo info) => Main.hardMode && info.Water && info.Player.ZoneJungle, 2f / 3f);
		HardmodeCrimsonWater = new SpawnCondition((NPCSpawnInfo info) => Main.hardMode && info.Water && info.Player.ZoneCrimson, 8f / 9f);
		Ocean = new SpawnCondition((NPCSpawnInfo info) => info.Water && (info.SpawnTileX < 250 || info.SpawnTileX > Main.maxTilesX - 250) && Main.tileSand[info.SpawnTileType] && (double)info.SpawnTileY < Main.rockLayer);
		OceanAngler = new SpawnCondition(Ocean, (NPCSpawnInfo info) => !NPC.savedAngler && !NPC.AnyNPCs(376) && WaterSurface(info));
		OceanMonster = new SpawnCondition(Ocean, (NPCSpawnInfo info) => true);
		BeachAngler = new SpawnCondition((NPCSpawnInfo info) => !info.Water && !NPC.savedAngler && !NPC.AnyNPCs(376) && (info.SpawnTileX < 340 || info.SpawnTileX > Main.maxTilesX - 340) && Main.tileSand[info.SpawnTileType] && (double)info.SpawnTileY < Main.worldSurface);
		JungleWater = new SpawnCondition((NPCSpawnInfo info) => info.Water && info.SpawnTileType == 60);
		CavePiranha = new SpawnCondition((NPCSpawnInfo info) => info.Water && (double)info.SpawnTileY > Main.rockLayer, 0.5f);
		CaveJellyfish = new SpawnCondition((NPCSpawnInfo info) => info.Water && (double)info.SpawnTileY > Main.worldSurface, 1f / 3f);
		WaterCritter = new SpawnCondition((NPCSpawnInfo info) => info.Water, 0.25f);
		CorruptWaterCritter = new SpawnCondition(WaterCritter, (NPCSpawnInfo info) => info.Player.ZoneCorrupt);
		OverworldWaterCritter = new SpawnCondition(WaterCritter, (NPCSpawnInfo info) => (double)info.SpawnTileY < Main.worldSurface && info.SpawnTileY > 50 && Main.dayTime, 2f / 3f);
		OverworldWaterSurfaceCritter = new SpawnCondition(OverworldWaterCritter, WaterSurface);
		OverworldUnderwaterCritter = new SpawnCondition(OverworldWaterCritter, (NPCSpawnInfo info) => true);
		DefaultWaterCritter = new SpawnCondition(WaterCritter, (NPCSpawnInfo info) => true);
		BoundCaveNPC = new SpawnCondition((NPCSpawnInfo info) => !info.Water && (double)info.SpawnTileY >= Main.rockLayer && info.SpawnTileY < Main.maxTilesY - 210, 0.05f);
		TownCritter = new SpawnCondition((NPCSpawnInfo info) => info.PlayerInTown);
		TownWaterCritter = new SpawnCondition(TownCritter, (NPCSpawnInfo info) => info.Water);
		TownOverworldWaterCritter = new SpawnCondition(TownWaterCritter, (NPCSpawnInfo info) => (double)info.SpawnTileY < Main.worldSurface && info.SpawnTileY > 50 && Main.dayTime, 2f / 3f);
		TownOverworldWaterSurfaceCritter = new SpawnCondition(TownOverworldWaterCritter, WaterSurface);
		TownOverworldUnderwaterCritter = new SpawnCondition(TownOverworldWaterCritter, (NPCSpawnInfo info) => true);
		TownDefaultWaterCritter = new SpawnCondition(TownWaterCritter, (NPCSpawnInfo info) => true);
		TownSnowCritter = new SpawnCondition(TownCritter, (NPCSpawnInfo info) => info.SpawnTileType == 147 || info.SpawnTileType == 161);
		TownJungleCritter = new SpawnCondition(TownCritter, (NPCSpawnInfo info) => info.SpawnTileType == 60);
		TownGeneralCritter = new SpawnCondition(TownCritter, (NPCSpawnInfo info) => info.SpawnTileType == 2 || info.SpawnTileType == 109 || (double)info.SpawnTileY > Main.worldSurface);
		Dungeon = new SpawnCondition((NPCSpawnInfo info) => info.Player.ZoneDungeon);
		DungeonGuardian = new SpawnCondition(Dungeon, (NPCSpawnInfo info) => !NPC.downedBoss3);
		DungeonNormal = new SpawnCondition(Dungeon, (NPCSpawnInfo info) => true);
		Meteor = new SpawnCondition((NPCSpawnInfo info) => info.Player.ZoneMeteor);
		OldOnesArmy = new SpawnCondition((NPCSpawnInfo info) => DD2Event.Ongoing && info.Player.ZoneOldOneArmy);
		FrostMoon = new SpawnCondition((NPCSpawnInfo info) => (double)info.SpawnTileY <= Main.worldSurface && !Main.dayTime && Main.snowMoon);
		PumpkinMoon = new SpawnCondition((NPCSpawnInfo info) => (double)info.SpawnTileY <= Main.worldSurface && !Main.dayTime && Main.pumpkinMoon);
		SolarEclipse = new SpawnCondition((NPCSpawnInfo info) => (double)info.SpawnTileY <= Main.worldSurface && Main.dayTime && Main.eclipse);
		HardmodeMushroomWater = new SpawnCondition((NPCSpawnInfo info) => Main.hardMode && info.SpawnTileType == 70 && info.Water);
		OverworldMushroom = new SpawnCondition((NPCSpawnInfo info) => info.SpawnTileType == 70 && (double)info.SpawnTileY <= Main.worldSurface, 2f / 3f);
		UndergroundMushroom = new SpawnCondition((NPCSpawnInfo info) => info.SpawnTileType == 70 && Main.hardMode && (double)info.SpawnTileY >= Main.worldSurface, 2f / 3f);
		CorruptWorm = new SpawnCondition((NPCSpawnInfo info) => info.Player.ZoneCorrupt && !info.PlayerSafe, 1f / 65f);
		UndergroundMimic = new SpawnCondition((NPCSpawnInfo info) => Main.hardMode && (double)info.SpawnTileY > Main.worldSurface, 1f / 70f);
		OverworldMimic = new SpawnCondition((NPCSpawnInfo info) => Main.hardMode && GetTile(info).wall == 2, 0.05f);
		Wraith = new SpawnCondition((NPCSpawnInfo info) => Main.hardMode && (double)info.SpawnTileY <= Main.worldSurface && !Main.dayTime, 0.05f);
		Wraith.WeightFunc = delegate
		{
			float num3 = 0.95f;
			if (Main.moonPhase == 4)
			{
				num3 *= 0.8f;
			}
			return 1f - num3;
		};
		HoppinJack = new SpawnCondition((NPCSpawnInfo info) => Main.hardMode && Main.halloween && (double)info.SpawnTileY <= Main.worldSurface && !Main.dayTime, 0.1f);
		DoctorBones = new SpawnCondition((NPCSpawnInfo info) => info.SpawnTileType == 60 && !Main.dayTime, 0.002f);
		LacBeetle = new SpawnCondition((NPCSpawnInfo info) => info.SpawnTileType == 60 && (double)info.SpawnTileY > Main.worldSurface, 1f / 60f);
		WormCritter = new SpawnCondition((NPCSpawnInfo info) => (double)info.SpawnTileY > Main.worldSurface && info.SpawnTileY < Main.maxTilesY - 210 && !info.Player.ZoneSnow && !info.Player.ZoneCrimson && !info.Player.ZoneCorrupt && !info.Player.ZoneJungle && !info.Player.ZoneHallow, 0.125f);
		MouseCritter = new SpawnCondition((NPCSpawnInfo info) => (double)info.SpawnTileY > Main.worldSurface && info.SpawnTileY < Main.maxTilesY - 210 && !info.Player.ZoneSnow && !info.Player.ZoneCrimson && !info.Player.ZoneCorrupt && !info.Player.ZoneJungle && !info.Player.ZoneHallow, 1f / 13f);
		SnailCritter = new SpawnCondition((NPCSpawnInfo info) => (double)info.SpawnTileY > Main.worldSurface && (double)info.SpawnTileY < (Main.rockLayer + (double)Main.maxTilesY) / 2.0 && !info.Player.ZoneSnow && !info.Player.ZoneCrimson && !info.Player.ZoneCorrupt && !info.Player.ZoneHallow, 1f / 13f);
		FrogCritter = new SpawnCondition((NPCSpawnInfo info) => (double)info.SpawnTileY < Main.worldSurface && info.Player.ZoneJungle, 1f / 9f);
		HardmodeJungle = new SpawnCondition((NPCSpawnInfo info) => info.SpawnTileType == 60 && Main.hardMode, 2f / 3f);
		JungleTemple = new SpawnCondition((NPCSpawnInfo info) => info.SpawnTileType == 226 && info.Lihzahrd);
		UndergroundJungle = new SpawnCondition((NPCSpawnInfo info) => info.SpawnTileType == 60 && (double)info.SpawnTileY > (Main.worldSurface + Main.rockLayer) / 2.0);
		SurfaceJungle = new SpawnCondition((NPCSpawnInfo info) => info.SpawnTileType == 60, 11f / 32f);
		SandstormEvent = new SpawnCondition((NPCSpawnInfo info) => Sandstorm.Happening && info.Player.ZoneSandstorm && TileID.Sets.Conversion.Sand[info.SpawnTileType] && NPC.Spawning_SandstoneCheck(info.SpawnTileX, info.SpawnTileY));
		Mummy = new SpawnCondition((NPCSpawnInfo info) => Main.hardMode && info.SpawnTileType == 53, 1f / 3f);
		DarkMummy = new SpawnCondition((NPCSpawnInfo info) => Main.hardMode && (info.SpawnTileType == 112 || info.SpawnTileType == 234), 0.5f);
		LightMummy = new SpawnCondition((NPCSpawnInfo info) => Main.hardMode && info.SpawnTileType == 116, 0.5f);
		OverworldHallow = new SpawnCondition((NPCSpawnInfo info) => Main.hardMode && !info.Water && (double)info.SpawnTileY < Main.rockLayer && (info.SpawnTileType == 116 || info.SpawnTileType == 117 || info.SpawnTileType == 109 || info.SpawnTileType == 164));
		EnchantedSword = new SpawnCondition((NPCSpawnInfo info) => !info.PlayerSafe && Main.hardMode && !info.Water && (double)info.SpawnTileY >= Main.rockLayer && (info.SpawnTileType == 116 || info.SpawnTileType == 117 || info.SpawnTileType == 109 || info.SpawnTileType == 164), 0.02f);
		Crimson = new SpawnCondition((NPCSpawnInfo info) => (info.SpawnTileType == 204 && info.Player.ZoneCrimson) || info.SpawnTileType == 199 || info.SpawnTileType == 200 || info.SpawnTileType == 203 || info.SpawnTileType == 234);
		Corruption = new SpawnCondition((NPCSpawnInfo info) => (info.SpawnTileType == 22 && info.Player.ZoneCorrupt) || info.SpawnTileType == 23 || info.SpawnTileType == 25 || info.SpawnTileType == 112 || info.SpawnTileType == 163);
		Overworld = new SpawnCondition((NPCSpawnInfo info) => (double)info.SpawnTileY <= Main.worldSurface);
		IceGolem = new SpawnCondition(Overworld, (NPCSpawnInfo info) => info.Player.ZoneSnow && Main.hardMode && Main.cloudAlpha > 0f && !NPC.AnyNPCs(243), 0.05f);
		RainbowSlime = new SpawnCondition(Overworld, (NPCSpawnInfo info) => info.Player.ZoneHallow && Main.hardMode && Main.cloudAlpha > 0f && !NPC.AnyNPCs(244), 0.05f);
		AngryNimbus = new SpawnCondition(Overworld, (NPCSpawnInfo info) => !info.Player.ZoneSnow && Main.hardMode && Main.cloudAlpha > 0f && NPC.CountNPCS(250) < 2, 0.1f);
		MartianProbe = new SpawnCondition(Overworld, (NPCSpawnInfo info) => MartianProbeHelper(info) && Main.hardMode && NPC.downedGolemBoss && !NPC.AnyNPCs(399), 0.0025f);
		MartianProbe.WeightFunc = delegate
		{
			float num2 = 0.9975f;
			if (!NPC.downedMartians)
			{
				num2 *= 0.99f;
			}
			return 1f - num2;
		};
		OverworldDay = new SpawnCondition(Overworld, (NPCSpawnInfo info) => Main.dayTime);
		OverworldDaySnowCritter = new SpawnCondition(OverworldDay, (NPCSpawnInfo info) => InnerThird(info) && (GetTile(info).type == 147 || GetTile(info).type == 161), 1f / 15f);
		OverworldDayGrassCritter = new SpawnCondition(OverworldDay, (NPCSpawnInfo info) => InnerThird(info) && (GetTile(info).type == 2 || GetTile(info).type == 109), 1f / 15f);
		OverworldDaySandCritter = new SpawnCondition(OverworldDay, (NPCSpawnInfo info) => InnerThird(info) && GetTile(info).type == 53, 1f / 15f);
		OverworldMorningBirdCritter = new SpawnCondition(OverworldDay, (NPCSpawnInfo info) => InnerThird(info) && Main.time < 18000.0 && (GetTile(info).type == 2 || GetTile(info).type == 109), 0.25f);
		OverworldDayBirdCritter = new SpawnCondition(OverworldDay, (NPCSpawnInfo info) => InnerThird(info) && (GetTile(info).type == 2 || GetTile(info).type == 109 || GetTile(info).type == 147), 1f / 15f);
		KingSlime = new SpawnCondition(OverworldDay, (NPCSpawnInfo info) => OuterThird(info) && GetTile(info).type == 2 && !NPC.AnyNPCs(50), 0.0033333334f);
		OverworldDayDesert = new SpawnCondition(OverworldDay, (NPCSpawnInfo info) => GetTile(info).type == 53 && !info.Water, 0.2f);
		GoblinScout = new SpawnCondition(OverworldDay, (NPCSpawnInfo info) => OuterThird(info), 1f / 15f);
		GoblinScout.WeightFunc = delegate
		{
			float num = 14f / 15f;
			return (!NPC.downedGoblins && WorldGen.shadowOrbSmashed) ? (num *= 0.85714287f) : (1f - num);
		};
		OverworldDayRain = new SpawnCondition(OverworldDay, (NPCSpawnInfo info) => Main.raining, 2f / 3f);
		OverworldDaySlime = new SpawnCondition(OverworldDay, (NPCSpawnInfo info) => true);
		OverworldNight = new SpawnCondition(Overworld, (NPCSpawnInfo info) => true);
		OverworldFirefly = new SpawnCondition(OverworldNight, (NPCSpawnInfo info) => GetTile(info).type == 2 || GetTile(info).type == 109, 0.1f);
		OverworldFirefly.WeightFunc = () => 1f / (float)NPC.fireFlyChance;
		OverworldNightMonster = new SpawnCondition(OverworldNight, (NPCSpawnInfo info) => true);
		Underground = new SpawnCondition((NPCSpawnInfo info) => (double)info.SpawnTileY <= Main.rockLayer);
		Underworld = new SpawnCondition((NPCSpawnInfo info) => info.SpawnTileY > Main.maxTilesY - 190);
		Cavern = new SpawnCondition((NPCSpawnInfo info) => true);
	}

	private static Tile GetTile(NPCSpawnInfo info)
	{
		return Main.tile[info.SpawnTileX, info.SpawnTileY];
	}

	private static bool WaterSurface(NPCSpawnInfo info)
	{
		if (info.SafeRangeX)
		{
			return false;
		}
		for (int i = info.SpawnTileY - 1; i > info.SpawnTileY - 50; i--)
		{
			if (Main.tile[info.SpawnTileX, i].liquid == 0 && !WorldGen.SolidTile(info.SpawnTileX, i) && !WorldGen.SolidTile(info.SpawnTileX, i + 1) && !WorldGen.SolidTile(info.SpawnTileX, i + 2))
			{
				return true;
			}
		}
		return false;
	}

	private static bool MartianProbeHelper(NPCSpawnInfo info)
	{
		if ((float)Math.Abs(info.SpawnTileX - Main.maxTilesX / 2) / (float)(Main.maxTilesX / 2) > 0.33f)
		{
			return !NPC.AnyDanger();
		}
		return false;
	}

	private static bool InnerThird(NPCSpawnInfo info)
	{
		return Math.Abs(info.SpawnTileX - Main.spawnTileX) < Main.maxTilesX / 3;
	}

	private static bool OuterThird(NPCSpawnInfo info)
	{
		return Math.Abs(info.SpawnTileX - Main.spawnTileX) > Main.maxTilesX / 3;
	}
}
