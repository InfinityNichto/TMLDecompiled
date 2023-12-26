using System;
using System.Runtime.CompilerServices;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.Localization;

namespace Terraria;

public sealed record Condition(LocalizedText Description, Func<bool> Predicate)
{
	public static readonly Condition NearWater = new Condition("Conditions.NearWater", () => Main.LocalPlayer.adjWater || Main.LocalPlayer.adjTile[172]);

	public static readonly Condition NearLava = new Condition("Conditions.NearLava", () => Main.LocalPlayer.adjLava);

	public static readonly Condition NearHoney = new Condition("Conditions.NearHoney", () => Main.LocalPlayer.adjHoney);

	public static readonly Condition NearShimmer = new Condition("Conditions.NearShimmer", () => Main.LocalPlayer.adjShimmer);

	public static readonly Condition TimeDay = new Condition("Conditions.TimeDay", () => Main.dayTime);

	public static readonly Condition TimeNight = new Condition("Conditions.TimeNight", () => !Main.dayTime);

	public static readonly Condition InDungeon = new Condition("Conditions.InDungeon", () => Main.LocalPlayer.ZoneDungeon);

	public static readonly Condition InCorrupt = new Condition("Conditions.InCorrupt", () => Main.LocalPlayer.ZoneCorrupt);

	public static readonly Condition InHallow = new Condition("Conditions.InHallow", () => Main.LocalPlayer.ZoneHallow);

	public static readonly Condition InMeteor = new Condition("Conditions.InMeteor", () => Main.LocalPlayer.ZoneMeteor);

	public static readonly Condition InJungle = new Condition("Conditions.InJungle", () => Main.LocalPlayer.ZoneJungle);

	public static readonly Condition InSnow = new Condition("Conditions.InSnow", () => Main.LocalPlayer.ZoneSnow);

	public static readonly Condition InCrimson = new Condition("Conditions.InCrimson", () => Main.LocalPlayer.ZoneCrimson);

	public static readonly Condition InWaterCandle = new Condition("Conditions.InWaterCandle", () => Main.LocalPlayer.ZoneWaterCandle);

	public static readonly Condition InPeaceCandle = new Condition("Conditions.InPeaceCandle", () => Main.LocalPlayer.ZonePeaceCandle);

	public static readonly Condition InTowerSolar = new Condition("Conditions.InTowerSolar", () => Main.LocalPlayer.ZoneTowerSolar);

	public static readonly Condition InTowerVortex = new Condition("Conditions.InTowerVortex", () => Main.LocalPlayer.ZoneTowerVortex);

	public static readonly Condition InTowerNebula = new Condition("Conditions.InTowerNebula", () => Main.LocalPlayer.ZoneTowerNebula);

	public static readonly Condition InTowerStardust = new Condition("Conditions.InTowerStardust", () => Main.LocalPlayer.ZoneTowerStardust);

	public static readonly Condition InDesert = new Condition("Conditions.InDesert", () => Main.LocalPlayer.ZoneDesert);

	public static readonly Condition InGlowshroom = new Condition("Conditions.InGlowshroom", () => Main.LocalPlayer.ZoneGlowshroom);

	public static readonly Condition InUndergroundDesert = new Condition("Conditions.InUndergroundDesert", () => Main.LocalPlayer.ZoneUndergroundDesert);

	public static readonly Condition InSkyHeight = new Condition("Conditions.InSkyHeight", () => Main.LocalPlayer.ZoneSkyHeight);

	public static readonly Condition InSpace = InSkyHeight;

	public static readonly Condition InOverworldHeight = new Condition("Conditions.InOverworldHeight", () => Main.LocalPlayer.ZoneOverworldHeight);

	public static readonly Condition InDirtLayerHeight = new Condition("Conditions.InDirtLayerHeight", () => Main.LocalPlayer.ZoneDirtLayerHeight);

	public static readonly Condition InRockLayerHeight = new Condition("Conditions.InRockLayerHeight", () => Main.LocalPlayer.ZoneRockLayerHeight);

	public static readonly Condition InUnderworldHeight = new Condition("Conditions.InUnderworldHeight", () => Main.LocalPlayer.ZoneUnderworldHeight);

	public static readonly Condition InUnderworld = InUnderworldHeight;

	public static readonly Condition InBeach = new Condition("Conditions.InBeach", () => Main.LocalPlayer.ZoneBeach);

	public static readonly Condition InRain = new Condition("Conditions.InRain", () => Main.LocalPlayer.ZoneRain);

	public static readonly Condition InSandstorm = new Condition("Conditions.InSandstorm", () => Main.LocalPlayer.ZoneSandstorm);

	public static readonly Condition InOldOneArmy = new Condition("Conditions.InOldOneArmy", () => Main.LocalPlayer.ZoneOldOneArmy);

	public static readonly Condition InGranite = new Condition("Conditions.InGranite", () => Main.LocalPlayer.ZoneGranite);

	public static readonly Condition InMarble = new Condition("Conditions.InMarble", () => Main.LocalPlayer.ZoneMarble);

	public static readonly Condition InHive = new Condition("Conditions.InHive", () => Main.LocalPlayer.ZoneHive);

	public static readonly Condition InGemCave = new Condition("Conditions.InGemCave", () => Main.LocalPlayer.ZoneGemCave);

	public static readonly Condition InLihzhardTemple = new Condition("Conditions.InLihzardTemple", () => Main.LocalPlayer.ZoneLihzhardTemple);

	public static readonly Condition InGraveyard = new Condition("Conditions.InGraveyard", () => Main.LocalPlayer.ZoneGraveyard);

	public static readonly Condition InAether = new Condition("Conditions.InAether", () => Main.LocalPlayer.ZoneShimmer);

	public static readonly Condition InShoppingZoneForest = new Condition("Conditions.InShoppingForest", () => Main.LocalPlayer.ShoppingZone_Forest);

	public static readonly Condition InBelowSurface = new Condition("Conditions.InBelowSurface", () => Main.LocalPlayer.ShoppingZone_BelowSurface);

	public static readonly Condition InEvilBiome = new Condition("Conditions.InEvilBiome", () => Main.LocalPlayer.ZoneCrimson || Main.LocalPlayer.ZoneCorrupt);

	public static readonly Condition NotInEvilBiome = new Condition("Conditions.NotInEvilBiome", () => !Main.LocalPlayer.ZoneCrimson && !Main.LocalPlayer.ZoneCorrupt);

	public static readonly Condition NotInHallow = new Condition("Conditions.NotInHallow", () => !Main.LocalPlayer.ZoneHallow);

	public static readonly Condition NotInGraveyard = new Condition("Conditions.NotInGraveyard", () => !Main.LocalPlayer.ZoneGraveyard);

	public static readonly Condition NotInUnderworld = new Condition("Conditions.NotInUnderworld", () => !Main.LocalPlayer.ZoneUnderworldHeight);

	public static readonly Condition InClassicMode = new Condition("Conditions.InClassicMode", () => !Main.expertMode);

	public static readonly Condition InExpertMode = new Condition("Conditions.InExpertMode", () => Main.expertMode);

	public static readonly Condition InMasterMode = new Condition("Conditions.InMasterMode", () => Main.masterMode);

	public static readonly Condition InJourneyMode = new Condition("Conditions.InJourneyMode", () => Main.GameModeInfo.IsJourneyMode);

	public static readonly Condition Hardmode = new Condition("Conditions.InHardmode", () => Main.hardMode);

	public static readonly Condition PreHardmode = new Condition("Conditions.PreHardmode", () => !Main.hardMode);

	public static readonly Condition SmashedShadowOrb = new Condition("Conditions.SmashedShadowOrb", () => WorldGen.shadowOrbSmashed);

	public static readonly Condition CrimsonWorld = new Condition("Conditions.WorldCrimson", () => WorldGen.crimson);

	public static readonly Condition CorruptWorld = new Condition("Conditions.WorldCorrupt", () => !WorldGen.crimson);

	public static readonly Condition DrunkWorld = new Condition("Conditions.WorldDrunk", () => Main.drunkWorld);

	public static readonly Condition RemixWorld = new Condition("Conditions.WorldRemix", () => Main.remixWorld);

	public static readonly Condition NotTheBeesWorld = new Condition("Conditions.WorldNotTheBees", () => Main.notTheBeesWorld);

	public static readonly Condition ForTheWorthyWorld = new Condition("Conditions.WorldForTheWorthy", () => Main.getGoodWorld);

	public static readonly Condition TenthAnniversaryWorld = new Condition("Conditions.WorldAnniversary", () => Main.tenthAnniversaryWorld);

	public static readonly Condition DontStarveWorld = new Condition("Conditions.WorldDontStarve", () => Main.dontStarveWorld);

	public static readonly Condition NoTrapsWorld = new Condition("Conditions.WorldNoTraps", () => Main.noTrapsWorld);

	public static readonly Condition ZenithWorld = new Condition("Conditions.WorldZenith", () => Main.remixWorld && Main.getGoodWorld);

	public static readonly Condition NotDrunkWorld = new Condition("Conditions.WorldNotDrunk", () => !Main.drunkWorld);

	public static readonly Condition NotRemixWorld = new Condition("Conditions.WorldNotRemix", () => !Main.remixWorld);

	public static readonly Condition NotNotTheBeesWorld = new Condition("Conditions.WorldNotNotTheBees", () => !Main.notTheBeesWorld);

	public static readonly Condition NotForTheWorthy = new Condition("Conditions.WorldNotForTheWorthy", () => !Main.getGoodWorld);

	public static readonly Condition NotTenthAnniversaryWorld = new Condition("Conditions.WorldNotAnniversary", () => !Main.tenthAnniversaryWorld);

	public static readonly Condition NotDontStarveWorld = new Condition("Conditions.WorldNotDontStarve", () => !Main.dontStarveWorld);

	public static readonly Condition NotNoTrapsWorld = new Condition("Conditions.WorldNotNoTraps", () => !Main.noTrapsWorld);

	public static readonly Condition NotZenithWorld = new Condition("Conditions.WorldNotZenith", () => !ZenithWorld.IsMet());

	public static readonly Condition Christmas = new Condition("Conditions.Christmas", () => Main.xMas);

	public static readonly Condition Halloween = new Condition("Conditions.Halloween", () => Main.halloween);

	public static readonly Condition BloodMoon = new Condition("Conditions.BloodMoon", () => Main.bloodMoon);

	public static readonly Condition NotBloodMoon = new Condition("Conditions.NotBloodMoon", () => !Main.bloodMoon);

	public static readonly Condition Eclipse = new Condition("Conditions.SolarEclipse", () => Main.eclipse);

	public static readonly Condition NotEclipse = new Condition("Conditions.NotSolarEclipse", () => !Main.eclipse);

	public static readonly Condition EclipseOrBloodMoon = new Condition("Conditions.BloodOrSun", () => Main.bloodMoon || Main.eclipse);

	public static readonly Condition NotEclipseAndNotBloodMoon = new Condition("Conditions.NotBloodOrSun", () => !Main.bloodMoon && !Main.eclipse);

	public static readonly Condition Thunderstorm = new Condition("Conditions.Thunderstorm", () => Main.IsItStorming);

	public static readonly Condition BirthdayParty = new Condition("Conditions.BirthdayParty", () => Terraria.GameContent.Events.BirthdayParty.PartyIsUp);

	public static readonly Condition LanternNight = new Condition("Conditions.NightLanterns", () => Terraria.GameContent.Events.LanternNight.LanternsUp);

	public static readonly Condition HappyWindyDay = new Condition("Conditions.HappyWindyDay", () => Main.IsItAHappyWindyDay);

	public static readonly Condition DownedKingSlime = new Condition("Conditions.DownedKingSlime", () => NPC.downedSlimeKing);

	public static readonly Condition DownedEyeOfCthulhu = new Condition("Conditions.DownedEyeOfCthulhu", () => NPC.downedBoss1);

	public static readonly Condition DownedEowOrBoc = new Condition("Conditions.DownedBoss2", () => NPC.downedBoss2);

	public static readonly Condition DownedEaterOfWorlds = new Condition("Conditions.DownedEaterOfWorlds", () => NPC.downedBoss2 && !WorldGen.crimson);

	public static readonly Condition DownedBrainOfCthulhu = new Condition("Conditions.DownedBrainOfCthulhu", () => NPC.downedBoss2 && WorldGen.crimson);

	public static readonly Condition DownedQueenBee = new Condition("Conditions.DownedQueenBee", () => NPC.downedQueenBee);

	public static readonly Condition DownedSkeletron = new Condition("Conditions.DownedSkeletron", () => NPC.downedBoss3);

	public static readonly Condition DownedDeerclops = new Condition("Conditions.DownedDeerclops", () => NPC.downedDeerclops);

	public static readonly Condition DownedQueenSlime = new Condition("Conditions.DownedQueenSlime", () => NPC.downedQueenSlime);

	public static readonly Condition DownedEarlygameBoss = new Condition("Conditions.DownedEarlygameBoss", () => NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3 || NPC.downedQueenBee || Main.hardMode);

	public static readonly Condition DownedMechBossAny = new Condition("Conditions.DownedMechBossAny", () => NPC.downedMechBossAny);

	public static readonly Condition DownedTwins = new Condition("Conditions.DownedTwins", () => NPC.downedMechBoss2);

	public static readonly Condition DownedDestroyer = new Condition("Conditions.DownedDestroyer", () => NPC.downedMechBoss1);

	public static readonly Condition DownedSkeletronPrime = new Condition("Conditions.DownedSkeletronPrime", () => NPC.downedMechBoss3);

	public static readonly Condition DownedMechBossAll = new Condition("Conditions.DownedMechBossAll", () => NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3);

	public static readonly Condition DownedPlantera = new Condition("Conditions.DownedPlantera", () => NPC.downedPlantBoss);

	public static readonly Condition DownedEmpressOfLight = new Condition("Conditions.DownedEmpressOfLight", () => NPC.downedEmpressOfLight);

	public static readonly Condition DownedDukeFishron = new Condition("Conditions.DownedDukeFishron", () => NPC.downedFishron);

	public static readonly Condition DownedGolem = new Condition("Conditions.DownedGolem", () => NPC.downedGolemBoss);

	public static readonly Condition DownedMourningWood = new Condition("Conditions.DownedMourningWood", () => NPC.downedHalloweenTree);

	public static readonly Condition DownedPumpking = new Condition("Conditions.DownedPumpking", () => NPC.downedHalloweenKing);

	public static readonly Condition DownedEverscream = new Condition("Conditions.DownedEverscream", () => NPC.downedChristmasTree);

	public static readonly Condition DownedSantaNK1 = new Condition("Conditions.DownedSantaNK1", () => NPC.downedChristmasSantank);

	public static readonly Condition DownedIceQueen = new Condition("Conditions.DownedIceQueen", () => NPC.downedChristmasIceQueen);

	public static readonly Condition DownedCultist = new Condition("Conditions.DownedLunaticCultist", () => NPC.downedAncientCultist);

	public static readonly Condition DownedMoonLord = new Condition("Conditions.DownedMoonLord", () => NPC.downedMoonlord);

	public static readonly Condition DownedClown = new Condition("Conditions.DownedClown", () => NPC.downedClown);

	public static readonly Condition DownedGoblinArmy = new Condition("Conditions.DownedGoblinArmy", () => NPC.downedGoblins);

	public static readonly Condition DownedPirates = new Condition("Conditions.DownedPirates", () => NPC.downedPirates);

	public static readonly Condition DownedMartians = new Condition("Conditions.DownedMartians", () => NPC.downedMartians);

	public static readonly Condition DownedFrostLegion = new Condition("Conditions.DownedFrostLegion", () => NPC.downedFrost);

	public static readonly Condition DownedSolarPillar = new Condition("Conditions.DownedSolarPillar", () => NPC.downedTowerSolar);

	public static readonly Condition DownedVortexPillar = new Condition("Conditions.DownedVortexPillar", () => NPC.downedTowerVortex);

	public static readonly Condition DownedNebulaPillar = new Condition("Conditions.DownedNebulaPillar", () => NPC.downedTowerNebula);

	public static readonly Condition DownedStardustPillar = new Condition("Conditions.DownedStardustPillar", () => NPC.downedTowerStardust);

	public static readonly Condition DownedOldOnesArmyAny = new Condition("Conditions.DownedOldOnesArmyAny", () => DD2Event.DownedInvasionAnyDifficulty);

	public static readonly Condition DownedOldOnesArmyT1 = new Condition("Conditions.DownedOldOnesArmyT1", () => DD2Event.DownedInvasionT1);

	public static readonly Condition DownedOldOnesArmyT2 = new Condition("Conditions.DownedOldOnesArmyT2", () => DD2Event.DownedInvasionT2);

	public static readonly Condition DownedOldOnesArmyT3 = new Condition("Conditions.DownedOldOnesArmyT3", () => DD2Event.DownedInvasionT3);

	public static readonly Condition NotDownedKingSlime = new Condition("Conditions.NotDownedKingSlime", () => !NPC.downedSlimeKing);

	public static readonly Condition NotDownedEyeOfCthulhu = new Condition("Conditions.NotDownedEyeOfCthulhu", () => !NPC.downedBoss1);

	public static readonly Condition NotDownedEowOrBoc = new Condition("Conditions.NotDownedBoss2", () => !NPC.downedBoss2);

	public static readonly Condition NotDownedEaterOfWorlds = new Condition("Conditions.NotDownedEaterOfWorlds", () => !NPC.downedBoss2 && !WorldGen.crimson);

	public static readonly Condition NotDownedBrainOfCthulhu = new Condition("Conditions.NotDownedBrainOfCthulhu", () => !NPC.downedBoss2 && WorldGen.crimson);

	public static readonly Condition NotDownedQueenBee = new Condition("Conditions.NotDownedQueenBee", () => !NPC.downedQueenBee);

	public static readonly Condition NotDownedSkeletron = new Condition("Conditions.NotDownedSkeletron", () => !NPC.downedBoss3);

	public static readonly Condition NotDownedDeerclops = new Condition("Conditions.NotDownedDeerclops", () => !NPC.downedDeerclops);

	public static readonly Condition NotDownedQueenSlime = new Condition("Conditions.NotDownedQueenSlime", () => !NPC.downedQueenSlime);

	public static readonly Condition NotDownedMechBossAny = new Condition("Conditions.NotDownedMechBossAny", () => !NPC.downedMechBossAny);

	public static readonly Condition NotDownedTwins = new Condition("Conditions.NotDownedTwins", () => !NPC.downedMechBoss2);

	public static readonly Condition NotDownedDestroyer = new Condition("Conditions.NotDownedDestroyer", () => !NPC.downedMechBoss1);

	public static readonly Condition NotDownedSkeletronPrime = new Condition("Conditions.NotDownedSkeletronPrime", () => !NPC.downedMechBoss3);

	public static readonly Condition NotDownedPlantera = new Condition("Conditions.NotDownedPlantera", () => !NPC.downedPlantBoss);

	public static readonly Condition NotDownedEmpressOfLight = new Condition("Conditions.NotDownedEmpressOfLight", () => !NPC.downedEmpressOfLight);

	public static readonly Condition NotDownedDukeFishron = new Condition("Conditions.NotDownedDukeFishron", () => !NPC.downedFishron);

	public static readonly Condition NotDownedGolem = new Condition("Conditions.NotDownedGolem", () => !NPC.downedGolemBoss);

	public static readonly Condition NotDownedMourningWood = new Condition("Conditions.NotDownedMourningWood", () => !NPC.downedHalloweenTree);

	public static readonly Condition NotDownedPumpking = new Condition("Conditions.NotDownedPumpking", () => !NPC.downedHalloweenKing);

	public static readonly Condition NotDownedEverscream = new Condition("Conditions.NotDownedEverscream", () => !NPC.downedChristmasTree);

	public static readonly Condition NotDownedSantaNK1 = new Condition("Conditions.NotDownedSantaNK1", () => !NPC.downedChristmasSantank);

	public static readonly Condition NotDownedIceQueen = new Condition("Conditions.NotDownedIceQueen", () => !NPC.downedChristmasIceQueen);

	public static readonly Condition NotDownedCultist = new Condition("Conditions.NotDownedLunaticCultist", () => !NPC.downedAncientCultist);

	public static readonly Condition NotDownedMoonLord = new Condition("Conditions.NotDownedMoonLord", () => !NPC.downedMoonlord);

	public static readonly Condition NotDownedClown = new Condition("Conditions.NotDownedClown", () => !NPC.downedClown);

	public static readonly Condition NotDownedGoblinArmy = new Condition("Conditions.NotDownedGoblinArmy", () => !NPC.downedGoblins);

	public static readonly Condition NotDownedPirates = new Condition("Conditions.NotDownedPirates", () => !NPC.downedPirates);

	public static readonly Condition NotDownedMartians = new Condition("Conditions.NotDownedMartians", () => !NPC.downedMartians);

	public static readonly Condition NotDownedFrostLegin = new Condition("Conditions.NotDownedFrostLegion", () => !NPC.downedFrost);

	public static readonly Condition NotDownedSolarPillar = new Condition("Conditions.NotDownedSolarPillar", () => !NPC.downedTowerSolar);

	public static readonly Condition NotDownedVortexPillar = new Condition("Conditions.NotDownedVortexPillar", () => !NPC.downedTowerVortex);

	public static readonly Condition NotDownedNebulaPillar = new Condition("Conditions.NotDownedNebulaPillar", () => !NPC.downedTowerNebula);

	public static readonly Condition NotDownedStardustPillar = new Condition("Conditions.NotDownedStardustPillar", () => !NPC.downedTowerStardust);

	public static readonly Condition NotDownedOldOnesArmyAny = new Condition("Conditions.NotDownedOldOnesArmyAny", () => !DD2Event.DownedInvasionAnyDifficulty);

	public static readonly Condition NotDownedOldOnesArmyT1 = new Condition("Conditions.NotDownedOldOnesArmyT1", () => !DD2Event.DownedInvasionT1);

	public static readonly Condition NotDownedOldOnesArmyT2 = new Condition("Conditions.NotDownedOldOnesArmyT2", () => !DD2Event.DownedInvasionT2);

	public static readonly Condition NotDownedOldOnesArmyT3 = new Condition("Conditions.NotDownedOldOnesArmyT3", () => !DD2Event.DownedInvasionT3);

	public static readonly Condition BloodMoonOrHardmode = new Condition("Conditions.BloodMoonOrHardmode", () => Main.bloodMoon || Main.hardMode);

	public static readonly Condition NightOrEclipse = new Condition("Conditions.NightOrEclipse", () => !Main.dayTime || Main.eclipse);

	public static readonly Condition Multiplayer = new Condition("Conditions.InMultiplayer", () => Main.netMode != 0);

	public static readonly Condition HappyEnough = new Condition("Conditions.HappyEnough", () => Main.LocalPlayer.currentShoppingSettings.PriceAdjustment <= 0.9);

	public static readonly Condition HappyEnoughToSellPylons = new Condition("Conditions.HappyEnoughForPylons", () => Main.remixWorld || HappyEnough.IsMet());

	public static readonly Condition AnotherTownNPCNearby = new Condition("Conditions.AnotherTownNPCNearby", () => TeleportPylonsSystem.DoesPositionHaveEnoughNPCs(2, Main.LocalPlayer.Center.ToTileCoordinates16()));

	public static readonly Condition IsNpcShimmered = new Condition("Conditions.IsNpcShimmered", () => Main.LocalPlayer.TalkNPC?.IsShimmerVariant ?? false);

	public static readonly Condition MoonPhaseFull = new Condition("Conditions.FullMoon", () => Main.GetMoonPhase() == MoonPhase.Full);

	public static readonly Condition MoonPhaseWaningGibbous = new Condition("Conditions.WaningGibbousMoon", () => Main.GetMoonPhase() == MoonPhase.ThreeQuartersAtLeft);

	public static readonly Condition MoonPhaseThirdQuarter = new Condition("Conditions.ThirdQuarterMoon", () => Main.GetMoonPhase() == MoonPhase.HalfAtLeft);

	public static readonly Condition MoonPhaseWaningCrescent = new Condition("Conditions.WaningCrescentMoon", () => Main.GetMoonPhase() == MoonPhase.QuarterAtLeft);

	public static readonly Condition MoonPhaseNew = new Condition("Conditions.NewMoon", () => Main.GetMoonPhase() == MoonPhase.Empty);

	public static readonly Condition MoonPhaseWaxingCrescent = new Condition("Conditions.WaxingCrescentMoon", () => Main.GetMoonPhase() == MoonPhase.QuarterAtRight);

	public static readonly Condition MoonPhaseFirstQuarter = new Condition("Conditions.FirstQuarterMoon", () => Main.GetMoonPhase() == MoonPhase.HalfAtRight);

	public static readonly Condition MoonPhaseWaxingGibbous = new Condition("Conditions.WaxingGibbousMoon", () => Main.GetMoonPhase() == MoonPhase.ThreeQuartersAtRight);

	public static readonly Condition MoonPhasesQuarter0 = new Condition("Conditions.MoonPhasesQuarter0", () => Main.moonPhase / 2 == 0);

	public static readonly Condition MoonPhasesQuarter1 = new Condition("Conditions.MoonPhasesQuarter1", () => Main.moonPhase / 2 == 1);

	public static readonly Condition MoonPhasesQuarter2 = new Condition("Conditions.MoonPhasesQuarter2", () => Main.moonPhase / 2 == 2);

	public static readonly Condition MoonPhasesQuarter3 = new Condition("Conditions.MoonPhasesQuarter3", () => Main.moonPhase / 2 == 3);

	public static readonly Condition MoonPhasesHalf0 = new Condition("Conditions.MoonPhasesHalf0", () => Main.moonPhase / 4 == 0);

	public static readonly Condition MoonPhasesHalf1 = new Condition("Conditions.MoonPhasesHalf1", () => Main.moonPhase / 4 == 1);

	public static readonly Condition MoonPhasesEven = new Condition("Conditions.MoonPhasesEven", () => Main.moonPhase % 2 == 0);

	public static readonly Condition MoonPhasesOdd = new Condition("Conditions.MoonPhasesOdd", () => Main.moonPhase % 2 == 1);

	public static readonly Condition MoonPhasesNearNew = new Condition("Conditions.MoonPhasesNearNew", () => Main.moonPhase >= 3 && Main.moonPhase <= 5);

	public static readonly Condition MoonPhasesEvenQuarters = new Condition("Conditions.MoonPhasesEvenQuarters", () => Main.moonPhase / 2 % 2 == 0);

	public static readonly Condition MoonPhasesOddQuarters = new Condition("Conditions.MoonPhasesOddQuarters", () => Main.moonPhase / 2 % 2 == 1);

	public static readonly Condition MoonPhases04 = new Condition("Conditions.MoonPhases04", () => Main.moonPhase % 4 == 0);

	public static readonly Condition MoonPhases15 = new Condition("Conditions.MoonPhases15", () => Main.moonPhase % 4 == 1);

	public static readonly Condition MoonPhases26 = new Condition("Conditions.MoonPhases26", () => Main.moonPhase % 4 == 2);

	public static readonly Condition MoonPhases37 = new Condition("Conditions.MoonPhases37", () => Main.moonPhase % 4 == 3);

	public Condition(string LocalizationKey, Func<bool> Predicate)
		: this(Language.GetOrRegister(LocalizationKey), Predicate)
	{
	}

	public bool IsMet()
	{
		return Predicate();
	}

	public static Condition PlayerCarriesItem(int itemId)
	{
		return new Condition(Language.GetText("Conditions.PlayerCarriesItem").WithFormatArgs(Lang.GetItemName(itemId)), () => Main.LocalPlayer.HasItem(itemId));
	}

	public static Condition GolfScoreOver(int score)
	{
		return new Condition(Language.GetText("Conditions.GolfScoreOver").WithFormatArgs(score), () => Main.LocalPlayer.golferScoreAccumulated >= score);
	}

	public static Condition NpcIsPresent(int npcId)
	{
		return new Condition(Language.GetText("Conditions.NpcIsPresent").WithFormatArgs(Lang.GetNPCName(npcId)), () => NPC.AnyNPCs(npcId));
	}

	public static Condition AnglerQuestsFinishedOver(int quests)
	{
		return new Condition(Language.GetText("Conditions.AnglerQuestsFinishedOver").WithFormatArgs(quests), () => Main.LocalPlayer.anglerQuestsFinished >= quests);
	}

	[CompilerGenerated]
	private Condition(Condition original)
	{
		Description = original.Description;
		Predicate = original.Predicate;
	}
}
