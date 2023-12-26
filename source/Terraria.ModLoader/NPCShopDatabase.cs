using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.ModLoader;

public static class NPCShopDatabase
{
	private class ConditionTest
	{
		public IEnumerable<Condition> Conditions;

		public string Name;

		public bool Tested;

		public string Status => $"[{(Tested ? 35 : 32)}] {Name}";

		public ConditionTest(IEnumerable<Condition> conditions)
		{
			Conditions = conditions.OrderBy((Condition c) => c.Description.Value).ToList();
			Name = string.Join(" && ", conditions.Select((Condition c) => c.Description));
		}
	}

	private static readonly Dictionary<string, AbstractNPCShop> npcShopByName = new Dictionary<string, AbstractNPCShop>();

	public static readonly ISet<string> NoPylons = new HashSet<string>();

	private static string[] _vanillaShopNames = new string[26]
	{
		null,
		GetShopName(17),
		GetShopName(19),
		GetShopName(20),
		GetShopName(38),
		GetShopName(54),
		GetShopName(107),
		GetShopName(108),
		GetShopName(124),
		GetShopName(142),
		GetShopName(160),
		GetShopName(178),
		GetShopName(207),
		GetShopName(208),
		GetShopName(209),
		GetShopName(227),
		GetShopName(228),
		GetShopName(229),
		GetShopName(353),
		GetShopName(368),
		GetShopName(453),
		GetShopName(550),
		GetShopName(588),
		GetShopName(633),
		GetShopName(663),
		GetShopName(227, "Decor")
	};

	internal const bool TestingEnabled = false;

	private static List<ConditionTest> tests = new List<ConditionTest>();

	private static HashSet<string> mismatches = new HashSet<string>();

	private static readonly string TestFilePath = "TestedShopConditions.txt";

	public static IEnumerable<AbstractNPCShop> AllShops => npcShopByName.Values;

	internal static void AddShop(AbstractNPCShop shop)
	{
		npcShopByName.Add(shop.FullName, shop);
	}

	public static bool TryGetNPCShop(string fullName, out AbstractNPCShop shop)
	{
		return npcShopByName.TryGetValue(fullName, out shop);
	}

	/// <summary>
	/// Gets a shop name (identifier) in the format matching <see cref="P:Terraria.ModLoader.AbstractNPCShop.FullName" /> <br />
	/// Can be used with <see cref="M:Terraria.ModLoader.NPCShopDatabase.TryGetNPCShop(System.String,Terraria.ModLoader.AbstractNPCShop@)" /> and <see cref="M:Terraria.ModLoader.GlobalNPC.ModifyActiveShop(Terraria.NPC,System.String,Terraria.Item[])" />
	/// </summary>
	/// <param name="npcType"></param>
	/// <param name="shopName"></param>
	/// <returns></returns>
	public static string GetShopName(int npcType, string shopName = "Shop")
	{
		return ((npcType < NPCID.Count) ? ("Terraria/" + NPCID.Search.GetName(npcType)) : NPCLoader.GetNPC(npcType).FullName) + "/" + shopName;
	}

	public static string GetShopNameFromVanillaIndex(int index)
	{
		return _vanillaShopNames[index];
	}

	public static void Initialize()
	{
		npcShopByName.Clear();
		NoPylons.Clear();
		RegisterVanillaNPCShops();
		for (int i = 0; i < NPCLoader.NPCCount; i++)
		{
			NPCLoader.AddShops(i);
		}
		foreach (NPCShop item in AllShops.OfType<NPCShop>())
		{
			NPCLoader.ModifyShop(item);
		}
	}

	internal static void FinishSetup()
	{
		foreach (AbstractNPCShop allShop in AllShops)
		{
			allShop.FinishSetup();
			foreach (AbstractNPCShop.Entry entry in allShop.ActiveEntries)
			{
				entry.Item.material = ItemID.Sets.IsAMaterial[entry.Item.type];
			}
		}
		InitShopTestSystem();
	}

	private static void RegisterVanillaNPCShops()
	{
		NoPylons.Add(GetShopName(368));
		NoPylons.Add(GetShopName(453));
		NoPylons.Add(GetShopName(550));
		NoPylons.Add(GetShopName(142));
		RegisterMerchant();
		RegisterArmsDealer();
		RegisterDryad();
		RegisterBombGuy();
		RegisterClothier();
		RegisterGoblin();
		RegisterWizard();
		RegisterMechanic();
		RegisterSantaClaws();
		RegisterTruffle();
		RegisterSteampunker();
		RegisterDyeTrader();
		RegisterPartyGirl();
		RegisterCyborg();
		RegisterPainter();
		RegisterWitchDoctor();
		RegisterPirate();
		RegisterStylist();
		RegisterSkeletonMerchant();
		RegisterBartender();
		RegisterGolfer();
		RegisterZoologist();
		RegisterPrincess();
		RegisterTravellingMerchant();
	}

	public static IEnumerable<NPCShop.Entry> GetPylonEntries()
	{
		Condition forestPylonCondition = new Condition("Conditions.ForestPylon", delegate
		{
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			if (Main.LocalPlayer.ZoneSnow || Main.LocalPlayer.ZoneDesert || Main.LocalPlayer.ZoneBeach || Main.LocalPlayer.ZoneJungle || Main.LocalPlayer.ZoneHallow || Main.LocalPlayer.ZoneGlowshroom)
			{
				return false;
			}
			if (!Main.remixWorld)
			{
				return (double)Main.LocalPlayer.Center.Y / 16.0 < Main.worldSurface;
			}
			return (double)Main.LocalPlayer.Center.Y / 16.0 > Main.rockLayer && Main.LocalPlayer.Center.Y / 16f < (float)(Main.maxTilesY - 350);
		});
		Condition cavernPylonCondition = new Condition("Conditions.UndergroundPylon", () => !Main.LocalPlayer.ZoneSnow && !Main.LocalPlayer.ZoneDesert && !Main.LocalPlayer.ZoneBeach && !Main.LocalPlayer.ZoneJungle && !Main.LocalPlayer.ZoneHallow && (Main.remixWorld || !Main.LocalPlayer.ZoneGlowshroom) && (double)Main.LocalPlayer.Center.Y / 16.0 >= Main.worldSurface);
		Condition oceanPylonCondition = new Condition("Conditions.InBeach", delegate
		{
			bool flag = Main.LocalPlayer.ZoneBeach && (double)Main.LocalPlayer.position.Y < Main.worldSurface * 16.0;
			if (Main.remixWorld)
			{
				double num = (double)Main.LocalPlayer.position.X / 16.0;
				double num2 = (double)Main.LocalPlayer.position.Y / 16.0;
				flag |= (num < (double)Main.maxTilesX * 0.43 || num > (double)Main.maxTilesX * 0.57) && num2 > Main.rockLayer && num2 < (double)(Main.maxTilesY - 350);
			}
			return flag;
		});
		Condition mushroomPylonCondition = new Condition("Conditions.InGlowshroom", () => Main.LocalPlayer.ZoneGlowshroom && (!Main.remixWorld || !Main.LocalPlayer.ZoneUnderworldHeight));
		yield return new NPCShop.Entry(4876, Condition.HappyEnoughToSellPylons, Condition.AnotherTownNPCNearby, Condition.NotInEvilBiome, forestPylonCondition).OrderLast();
		yield return new NPCShop.Entry(4920, Condition.HappyEnoughToSellPylons, Condition.AnotherTownNPCNearby, Condition.NotInEvilBiome, Condition.InSnow).OrderLast();
		yield return new NPCShop.Entry(4919, Condition.HappyEnoughToSellPylons, Condition.AnotherTownNPCNearby, Condition.NotInEvilBiome, Condition.InDesert).OrderLast();
		yield return new NPCShop.Entry(4917, Condition.HappyEnoughToSellPylons, Condition.AnotherTownNPCNearby, Condition.NotInEvilBiome, cavernPylonCondition).OrderLast();
		yield return new NPCShop.Entry(4918, Condition.HappyEnoughToSellPylons, Condition.AnotherTownNPCNearby, Condition.NotInEvilBiome, oceanPylonCondition).OrderLast();
		yield return new NPCShop.Entry(4875, Condition.HappyEnoughToSellPylons, Condition.AnotherTownNPCNearby, Condition.NotInEvilBiome, Condition.InJungle).OrderLast();
		yield return new NPCShop.Entry(4916, Condition.HappyEnoughToSellPylons, Condition.AnotherTownNPCNearby, Condition.NotInEvilBiome, Condition.InHallow).OrderLast();
		yield return new NPCShop.Entry(4921, Condition.HappyEnoughToSellPylons, Condition.AnotherTownNPCNearby, Condition.NotInEvilBiome, mushroomPylonCondition).OrderLast();
		foreach (ModPylon modPylon in PylonLoader.modPylons)
		{
			NPCShop.Entry entry = modPylon.GetNPCShopEntry();
			if (entry != null)
			{
				yield return entry.OrderLast();
			}
		}
	}

	private static void RegisterMerchant()
	{
		Condition carriesFlareGun = Condition.PlayerCarriesItem(930);
		Condition drumSetCondition = new Condition("Conditions.DownedB2B3HM", () => NPC.downedBoss2 || NPC.downedBoss3 || Main.hardMode);
		new NPCShop(17).Add(88).Add(87).Add(35)
			.Add(1991)
			.Add(3509)
			.Add(3506)
			.Add(8)
			.Add(28)
			.Add(188, Condition.Hardmode)
			.Add(110)
			.Add(189, Condition.Hardmode)
			.Add(40)
			.Add(42)
			.Add(965)
			.Add(967, Condition.InSnow)
			.Add(33, Condition.InJungle)
			.Add(4074, Condition.TimeDay, Condition.HappyWindyDay)
			.Add(279, Condition.BloodMoon)
			.Add(282, Condition.TimeNight)
			.Add(346, Condition.DownedSkeletron)
			.Add(488, Condition.Hardmode)
			.Add(931, carriesFlareGun)
			.Add(1614, carriesFlareGun)
			.Add(1786)
			.Add(1348, Condition.Hardmode)
			.Add(3198, Condition.Hardmode)
			.Add(4063, drumSetCondition)
			.Add(4673, drumSetCondition)
			.Add(3108, Condition.PlayerCarriesItem(3107))
			.Register();
	}

	private static void RegisterArmsDealer()
	{
		new NPCShop(19).Add(97).Add(278, Condition.BloodMoonOrHardmode, new Condition("Conditions.WorldGenSilver", () => WorldGen.SavedOreTiers.Silver == 9)).Add(4915, Condition.BloodMoonOrHardmode, new Condition("Conditions.WorldGenTungsten", () => WorldGen.SavedOreTiers.Silver == 168))
			.Add(47, new Condition("Conditions.NightAfterEvilOrHardmode", () => (NPC.downedBoss2 && !Main.dayTime) || Main.hardMode))
			.Add(95)
			.Add(98)
			.Add(4703, Condition.InGraveyard, Condition.DownedSkeletron)
			.Add(324, Condition.TimeNight)
			.Add(534, Condition.Hardmode)
			.Add(1432, Condition.Hardmode)
			.Add(2177, Condition.Hardmode)
			.Add(1261, Condition.PlayerCarriesItem(1258))
			.Add(1836, Condition.PlayerCarriesItem(1835))
			.Add(3108, Condition.PlayerCarriesItem(3107))
			.Add(1783, Condition.PlayerCarriesItem(1782))
			.Add(1785, Condition.PlayerCarriesItem(1784))
			.Add(1736, Condition.Halloween)
			.Add(1737, Condition.Halloween)
			.Add(1738, Condition.Halloween)
			.Register();
	}

	private static void RegisterDryad()
	{
		new NPCShop(20).Add(2886, Condition.BloodMoon, Condition.CrimsonWorld, Condition.NotRemixWorld).Add(2171, Condition.BloodMoon, Condition.CrimsonWorld).Add(4508, Condition.BloodMoon, Condition.CrimsonWorld)
			.Add(67, Condition.BloodMoon, Condition.CorruptWorld, Condition.NotRemixWorld)
			.Add(59, Condition.BloodMoon, Condition.CorruptWorld)
			.Add(4504, Condition.BloodMoon, Condition.CorruptWorld)
			.Add(66, Condition.NotBloodMoon, Condition.NotRemixWorld)
			.Add(62, Condition.NotBloodMoon)
			.Add(63, Condition.NotBloodMoon)
			.Add(745, Condition.NotBloodMoon)
			.Add(2171, Condition.Hardmode, Condition.InGraveyard, Condition.CorruptWorld)
			.Add(59, Condition.Hardmode, Condition.InGraveyard, Condition.CrimsonWorld)
			.Add(27)
			.Add(5309)
			.Add(114)
			.Add(1828)
			.Add(747)
			.Add(746, Condition.Hardmode)
			.Add(369, Condition.Hardmode)
			.Add(4505, Condition.Hardmode)
			.Add(5214, Condition.InUnderworld)
			.Add(194, Condition.NotInUnderworld, Condition.InGlowshroom)
			.Add(1853, Condition.Halloween)
			.Add(1854, Condition.Halloween)
			.Add(3215, Condition.DownedKingSlime)
			.Add(3216, Condition.DownedQueenBee)
			.Add(3219, Condition.DownedEyeOfCthulhu)
			.Add(3217, Condition.DownedEaterOfWorlds)
			.Add(3218, Condition.DownedBrainOfCthulhu)
			.Add(3220, Condition.DownedSkeletron)
			.Add(3221, Condition.DownedSkeletron)
			.Add(3222, Condition.Hardmode)
			.Add(4047)
			.Add(4045)
			.Add(4044)
			.Add(4043)
			.Add(4042)
			.Add(4046)
			.Add(4041)
			.Add(4241)
			.Add(4048)
			.Add(4430, Condition.Hardmode, Condition.MoonPhasesQuarter0)
			.Add(4431, Condition.Hardmode, Condition.MoonPhasesQuarter0)
			.Add(4432, Condition.Hardmode, Condition.MoonPhasesQuarter0)
			.Add(4433, Condition.Hardmode, Condition.MoonPhasesQuarter1)
			.Add(4434, Condition.Hardmode, Condition.MoonPhasesQuarter1)
			.Add(4435, Condition.Hardmode, Condition.MoonPhasesQuarter1)
			.Add(4436, Condition.Hardmode, Condition.MoonPhasesQuarter2)
			.Add(4437, Condition.Hardmode, Condition.MoonPhasesQuarter2)
			.Add(4438, Condition.Hardmode, Condition.MoonPhasesQuarter2)
			.Add(4439, Condition.Hardmode, Condition.MoonPhasesQuarter3)
			.Add(4440, Condition.Hardmode, Condition.MoonPhasesQuarter3)
			.Add(4441, Condition.Hardmode, Condition.MoonPhasesQuarter3)
			.Register();
	}

	private static void RegisterBombGuy()
	{
		new NPCShop(38).Add(168).Add(166).Add(167)
			.Add(265, Condition.Hardmode)
			.Add(937, Condition.Hardmode, Condition.DownedPlantera, Condition.DownedPirates)
			.Add(1347, Condition.Hardmode)
			.Add(4827, Condition.PlayerCarriesItem(4827))
			.Add(4824, Condition.PlayerCarriesItem(4824))
			.Add(4825, Condition.PlayerCarriesItem(4825))
			.Add(4826, Condition.PlayerCarriesItem(4826))
			.Register();
	}

	private static void RegisterClothier()
	{
		Condition taxCollectorIsPresent = Condition.NpcIsPresent(441);
		new NPCShop(54).Add(254).Add(981).Add(242, Condition.TimeDay)
			.Add(245, Condition.MoonPhaseFull)
			.Add(246, Condition.MoonPhaseFull)
			.Add(1288, Condition.MoonPhaseFull, Condition.TimeNight)
			.Add(1289, Condition.MoonPhaseFull, Condition.TimeNight)
			.Add(325, Condition.MoonPhaseWaningGibbous)
			.Add(326, Condition.MoonPhaseWaningGibbous)
			.Add(269)
			.Add(270)
			.Add(271)
			.Add(503, Condition.DownedClown)
			.Add(504, Condition.DownedClown)
			.Add(505, Condition.DownedClown)
			.Add(322, Condition.BloodMoon)
			.Add(3362, Condition.BloodMoon)
			.Add(3363, Condition.BloodMoon)
			.Add(2856, Condition.TimeDay, Condition.DownedCultist)
			.Add(2858, Condition.TimeDay, Condition.DownedCultist)
			.Add(2857, Condition.TimeNight, Condition.DownedCultist)
			.Add(2859, Condition.TimeNight, Condition.DownedCultist)
			.Add(3242, taxCollectorIsPresent)
			.Add(3243, taxCollectorIsPresent)
			.Add(3244, taxCollectorIsPresent)
			.Add(4685, Condition.InGraveyard)
			.Add(4686, Condition.InGraveyard)
			.Add(4704, Condition.InGraveyard)
			.Add(4705, Condition.InGraveyard)
			.Add(4706, Condition.InGraveyard)
			.Add(4707, Condition.InGraveyard)
			.Add(4708, Condition.InGraveyard)
			.Add(4709, Condition.InGraveyard)
			.Add(1429, Condition.InSnow)
			.Add(1740, Condition.Halloween)
			.Add(869, Condition.Hardmode, Condition.MoonPhaseThirdQuarter)
			.Add(4994, Condition.Hardmode, Condition.MoonPhaseWaningCrescent)
			.Add(4997, Condition.Hardmode, Condition.MoonPhaseWaningCrescent)
			.Add(864, Condition.Hardmode, Condition.MoonPhaseNew)
			.Add(865, Condition.Hardmode, Condition.MoonPhaseNew)
			.Add(4995, Condition.Hardmode, Condition.MoonPhaseWaxingCrescent)
			.Add(4998, Condition.Hardmode, Condition.MoonPhaseWaxingCrescent)
			.Add(873, Condition.Hardmode, Condition.MoonPhaseFirstQuarter)
			.Add(874, Condition.Hardmode, Condition.MoonPhaseFirstQuarter)
			.Add(875, Condition.Hardmode, Condition.MoonPhaseFirstQuarter)
			.Add(4996, Condition.Hardmode, Condition.MoonPhaseWaxingGibbous)
			.Add(4999, Condition.Hardmode, Condition.MoonPhaseWaxingGibbous)
			.Add(1275, Condition.DownedFrostLegion, Condition.TimeDay)
			.Add(1276, Condition.DownedFrostLegion, Condition.TimeNight)
			.Add(3246, Condition.Halloween)
			.Add(3247, Condition.Halloween)
			.Add(3730, Condition.BirthdayParty)
			.Add(3731, Condition.BirthdayParty)
			.Add(3733, Condition.BirthdayParty)
			.Add(3734, Condition.BirthdayParty)
			.Add(3735, Condition.BirthdayParty)
			.Add(4744, Condition.GolfScoreOver(2000))
			.Add(5308)
			.Register();
	}

	private static void RegisterGoblin()
	{
		new NPCShop(107).Add(128).Add(486).Add(398)
			.Add(84)
			.Add(407)
			.Add(161)
			.Add(5324, Condition.Hardmode)
			.Register();
	}

	private static void RegisterWizard()
	{
		new NPCShop(108).Add(487).Add(496).Add(500)
			.Add(507)
			.Add(508)
			.Add(531)
			.Add(149)
			.Add(576)
			.Add(3186)
			.Add(1739, Condition.Halloween)
			.Register();
	}

	private static void RegisterMechanic()
	{
		new NPCShop(124).Add(509).Add(850).Add(851)
			.Add(3612)
			.Add(510)
			.Add(530)
			.Add(513)
			.Add(538)
			.Add(529)
			.Add(541)
			.Add(542)
			.Add(543)
			.Add(852)
			.Add(853)
			.Add(4261)
			.Add(3707)
			.Add(2739)
			.Add(849)
			.Add(1263)
			.Add(3616)
			.Add(3725)
			.Add(2799)
			.Add(3619)
			.Add(3627)
			.Add(3629)
			.Add(585)
			.Add(584)
			.Add(583)
			.Add(4484)
			.Add(4485)
			.Add(2295, Condition.NpcIsPresent(369), Condition.MoonPhasesOdd)
			.Register();
	}

	private static void RegisterSantaClaws()
	{
		NPCShop shop = new NPCShop(142).Add(588).Add(589).Add(590)
			.Add(597)
			.Add(598)
			.Add(596);
		for (int i = 1873; i <= 1905; i++)
		{
			shop.Add(i);
		}
		shop.Register();
	}

	private static void RegisterTruffle()
	{
		new NPCShop(160).Add(756, Condition.DownedMechBossAny).Add(787, Condition.DownedMechBossAny).Add(868)
			.Add(1551, Condition.DownedPlantera)
			.Add(1181)
			.Add(5231)
			.Add(783, Condition.NotRemixWorld)
			.Register();
	}

	private static void RegisterSteampunker()
	{
		Condition steampunkerOutfitCondition = new Condition("Conditions.MoonPhasesHalf0OrPreHardmode", () => Condition.PreHardmode.IsMet() || Condition.MoonPhasesHalf0.IsMet());
		new NPCShop(178).Add(779, Condition.NotRemixWorld).Add(839, steampunkerOutfitCondition).Add(840, steampunkerOutfitCondition)
			.Add(841, steampunkerOutfitCondition)
			.Add(748, Condition.Hardmode, Condition.MoonPhasesHalf1)
			.Add(948, Condition.DownedGolem)
			.Add(3623, Condition.Hardmode)
			.Add(3603)
			.Add(3604)
			.Add(3607)
			.Add(3605)
			.Add(3606)
			.Add(3608)
			.Add(3618)
			.Add(3602)
			.Add(3663)
			.Add(3609)
			.Add(3610)
			.Add(995, new Condition("Conditions.HardmodeFTW", () => Main.hardMode || !Main.getGoodWorld))
			.Add(2203, Condition.DownedEyeOfCthulhu, Condition.DownedEowOrBoc, Condition.DownedSkeletron)
			.Add(2193, Condition.CrimsonWorld)
			.Add(4142, Condition.CorruptWorld)
			.Add(2192, Condition.InGraveyard)
			.Add(2204, Condition.InJungle)
			.Add(2198, Condition.InSnow)
			.Add(2197, Condition.InSpace)
			.Add(2196, Condition.PlayerCarriesItem(832))
			.Add(784, Condition.NotRemixWorld, Condition.EclipseOrBloodMoon, Condition.CrimsonWorld)
			.Add(782, Condition.NotRemixWorld, Condition.EclipseOrBloodMoon, Condition.CorruptWorld)
			.Add(781, Condition.NotRemixWorld, Condition.NotEclipseAndNotBloodMoon, Condition.InHallow)
			.Add(780, Condition.NotRemixWorld, Condition.NotEclipseAndNotBloodMoon, Condition.NotInHallow)
			.Add(5392, Condition.NotRemixWorld, Condition.DownedMoonLord)
			.Add(5393, Condition.NotRemixWorld, Condition.DownedMoonLord)
			.Add(5394, Condition.NotRemixWorld, Condition.DownedMoonLord)
			.Add(1344, Condition.Hardmode)
			.Add(4472, Condition.Hardmode)
			.Add(1742, Condition.Halloween)
			.Register();
	}

	private static void RegisterDyeTrader()
	{
		new NPCShop(207).Add(1037).Add(2874).Add(1120)
			.Add(1969, Condition.Multiplayer)
			.Add(3248, Condition.Halloween)
			.Add(1741, Condition.Halloween)
			.Add(2871, Condition.MoonPhaseFull)
			.Add(2872, Condition.MoonPhaseFull)
			.Add(4663, Condition.BloodMoon)
			.Add(4662, Condition.InGraveyard)
			.Register();
	}

	private static void RegisterPartyGirl()
	{
		new NPCShop(208).Add(859).Add(4743, Condition.GolfScoreOver(500)).Add(1000)
			.Add(1168)
			.Add(1449, Condition.TimeDay)
			.Add(4552, Condition.TimeNight)
			.Add(1345)
			.Add(1450)
			.Add(3253)
			.Add(4553)
			.Add(2700)
			.Add(2738)
			.Add(4470)
			.Add(4681)
			.Add(4682, Condition.InGraveyard)
			.Add(4702, Condition.LanternNight)
			.Add(3548, Condition.PlayerCarriesItem(3548))
			.Add(3369, Condition.NpcIsPresent(229))
			.Add(3546, Condition.DownedGolem)
			.Add(3214, Condition.Hardmode)
			.Add(2868, Condition.Hardmode)
			.Add(970, Condition.Hardmode)
			.Add(971, Condition.Hardmode)
			.Add(972, Condition.Hardmode)
			.Add(973, Condition.Hardmode)
			.Add(4791)
			.Add(3747)
			.Add(3732)
			.Add(3742)
			.Add(3749, Condition.BirthdayParty)
			.Add(3746, Condition.BirthdayParty)
			.Add(3739, Condition.BirthdayParty)
			.Add(3740, Condition.BirthdayParty)
			.Add(3741, Condition.BirthdayParty)
			.Add(3737, Condition.BirthdayParty)
			.Add(3738, Condition.BirthdayParty)
			.Add(3736, Condition.BirthdayParty)
			.Add(3745, Condition.BirthdayParty)
			.Add(3744, Condition.BirthdayParty)
			.Add(3743, Condition.BirthdayParty)
			.Register();
	}

	private static void RegisterCyborg()
	{
		Condition portalGunStation = new Condition(Language.GetText("Conditions.PlayerCarriesItem2").WithFormatArgs(Lang.GetItemName(3384), Lang.GetItemName(3664)), () => Main.LocalPlayer.HasItem(3384) || Main.LocalPlayer.HasItem(3664));
		new NPCShop(209).Add(771).Add(772, Condition.BloodMoon).Add(773, Condition.NightOrEclipse)
			.Add(774, Condition.Eclipse)
			.Add(4445, Condition.DownedMartians)
			.Add(4446, Condition.DownedMartians, Condition.EclipseOrBloodMoon)
			.Add(4459, Condition.Hardmode)
			.Add(760, Condition.Hardmode)
			.Add(1346, Condition.Hardmode)
			.Add(5451, Condition.Hardmode)
			.Add(5452, Condition.Hardmode)
			.Add(4409, Condition.InGraveyard)
			.Add(4392, Condition.InGraveyard)
			.Add(1743, Condition.Halloween)
			.Add(1744, Condition.Halloween)
			.Add(1745, Condition.Halloween)
			.Add(2862, Condition.DownedMartians)
			.Add(3109, Condition.DownedMartians)
			.Add(3664, portalGunStation)
			.Register();
	}

	private static void RegisterPainter()
	{
		new NPCShop(227).Add(1071).Add(1072).Add(1100)
			.Add(1073)
			.Add(1074)
			.Add(1075)
			.Add(1076)
			.Add(1077)
			.Add(1078)
			.Add(1079)
			.Add(1080)
			.Add(1081)
			.Add(1082)
			.Add(1083)
			.Add(1084)
			.Add(1097)
			.Add(1099)
			.Add(1098)
			.Add(1966)
			.Add(1967, Condition.Hardmode)
			.Add(1968, Condition.Hardmode)
			.Add(4668, Condition.InGraveyard)
			.Add(5344, Condition.InGraveyard, Condition.DownedPlantera)
			.Register();
		new NPCShop(227, "Decor").Add(1948, Condition.Christmas).Add(1949, Condition.Christmas).Add(1950, Condition.Christmas)
			.Add(1951, Condition.Christmas)
			.Add(1952, Condition.Christmas)
			.Add(1953, Condition.Christmas)
			.Add(1954, Condition.Christmas)
			.Add(1955, Condition.Christmas)
			.Add(1956, Condition.Christmas)
			.Add(1957, Condition.Christmas)
			.Add(2158)
			.Add(2159)
			.Add(2160)
			.Add(2008)
			.Add(2009)
			.Add(2010)
			.Add(2011)
			.Add(2012)
			.Add(2013)
			.Add(2014)
			.Add(1490, Condition.NotInGraveyard)
			.Add(1481, Condition.NotInGraveyard, Condition.MoonPhasesQuarter0)
			.Add(1482, Condition.NotInGraveyard, Condition.MoonPhasesQuarter1)
			.Add(1483, Condition.NotInGraveyard, Condition.MoonPhasesQuarter2)
			.Add(1484, Condition.NotInGraveyard, Condition.MoonPhasesQuarter3)
			.Add(5245, Condition.InShoppingZoneForest)
			.Add(1492, Condition.InCrimson)
			.Add(1488, Condition.InCorrupt)
			.Add(1489, Condition.InHallow)
			.Add(1486, Condition.InJungle)
			.Add(1487, Condition.InSnow)
			.Add(1491, Condition.InDesert)
			.Add(1493, Condition.BloodMoon)
			.Add(1485, Condition.NotInGraveyard, Condition.InSpace)
			.Add(1494, Condition.NotInGraveyard, Condition.Hardmode, Condition.InSpace)
			.Add(5251, Condition.Thunderstorm)
			.Add(4723, Condition.InGraveyard)
			.Add(4724, Condition.InGraveyard)
			.Add(4725, Condition.InGraveyard)
			.Add(4726, Condition.InGraveyard)
			.Add(4727, Condition.InGraveyard)
			.Add(5257, Condition.InGraveyard)
			.Add(4728, Condition.InGraveyard)
			.Add(4729, Condition.InGraveyard)
			.Register();
	}

	private static void RegisterWitchDoctor()
	{
		new NPCShop(228).Add(1430).Add(986).Add(2999, Condition.NpcIsPresent(108))
			.Add(1158, Condition.TimeNight)
			.Add(1159, Condition.DownedPlantera)
			.Add(1160, Condition.DownedPlantera)
			.Add(1161, Condition.DownedPlantera)
			.Add(1167, Condition.DownedPlantera, Condition.InJungle)
			.Add(1339, Condition.DownedPlantera)
			.Add(1171, Condition.Hardmode, Condition.InJungle)
			.Add(1162, Condition.Hardmode, Condition.InJungle, Condition.TimeNight, Condition.DownedPlantera)
			.Add(909)
			.Add(910)
			.Add(940)
			.Add(941)
			.Add(942)
			.Add(943)
			.Add(944)
			.Add(945)
			.Add(4922)
			.Add(4417)
			.Add(1836, Condition.PlayerCarriesItem(1835))
			.Add(1261, Condition.PlayerCarriesItem(1258))
			.Add(1791, Condition.Halloween)
			.Register();
	}

	private static void RegisterPirate()
	{
		Condition beachCondition = new Condition("Conditions.InBeach", delegate
		{
			int num = (int)((Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16f);
			return (double)(Main.screenPosition.Y / 16f) < Main.worldSurface + 10.0 && (num < 380 || num > Main.maxTilesX - 380);
		});
		new NPCShop(229).Add(928).Add(929).Add(876)
			.Add(877)
			.Add(878)
			.Add(2434)
			.Add(1180, beachCondition)
			.Add(1337, Condition.NpcIsPresent(208), Condition.Hardmode, Condition.DownedMechBossAny)
			.Register();
	}

	private static void RegisterStylist()
	{
		Condition maxLife = new Condition(Language.GetText("Conditions.AtleastXHealth").WithFormatArgs(400), () => Main.LocalPlayer.ConsumedLifeCrystals == 15);
		Condition maxMana = new Condition(Language.GetText("Conditions.AtleastXMana").WithFormatArgs(200), () => Main.LocalPlayer.ConsumedManaCrystals == 9);
		Condition moneyHair = new Condition("Conditions.PlatinumCoin", delegate
		{
			long num = 0L;
			for (int i = 0; i < 54; i++)
			{
				if (Main.LocalPlayer.inventory[i].type == 71)
				{
					num += Main.LocalPlayer.inventory[i].stack;
				}
				else if (Main.LocalPlayer.inventory[i].type == 72)
				{
					num += Main.LocalPlayer.inventory[i].stack * 100;
				}
				else if (Main.LocalPlayer.inventory[i].type == 73)
				{
					num += Main.LocalPlayer.inventory[i].stack * 10000;
				}
				else if (Main.LocalPlayer.inventory[i].type == 74)
				{
					num += Main.LocalPlayer.inventory[i].stack * 1000000;
				}
				if (num >= 1000000)
				{
					return true;
				}
			}
			return false;
		});
		Condition timeHair = new Condition("Conditions.StyleMoon", () => Main.moonPhase % 2 == (!Main.dayTime).ToInt());
		Condition teamHair = new Condition("Conditions.OnTeam", () => Main.LocalPlayer.team != 0);
		new NPCShop(353).Add(1990).Add(1979).Add(1977, maxLife)
			.Add(1978, maxMana)
			.Add(1980, moneyHair)
			.Add(1981, timeHair)
			.Add(1982, teamHair)
			.Add(1983, Condition.Hardmode)
			.Add(1984, Condition.NpcIsPresent(208))
			.Add(1985, Condition.Hardmode, Condition.DownedTwins, Condition.DownedSkeletronPrime, Condition.DownedDestroyer)
			.Add(1986, Condition.Hardmode, Condition.DownedMechBossAny)
			.Add(2863, Condition.DownedMartians)
			.Add(3259, Condition.DownedMartians)
			.Add(5104)
			.Register();
	}

	private static void RegisterSkeletonMerchant()
	{
		Condition spelunkerGlowCondition = new Condition("Conditions.NightDayFullMoon", () => !Main.dayTime || Main.moonPhase == 0);
		Condition glowstickCondition = new Condition("Conditions.DaytimeNotFullMoon", () => Main.dayTime && Main.moonPhase != 0);
		Condition artisanCondition = new Condition("Conditions.NoAteLoaf", () => !Main.LocalPlayer.ateArtisanBread);
		Condition boneTorchCondition = new Condition("Conditions.Periodically", () => Main.time % 60.0 <= 30.0);
		Condition torchCondition = new Condition("Conditions.Periodically", () => Main.time % 60.0 > 30.0);
		new NPCShop(453).Add(284, Condition.MoonPhaseFull).Add(946, Condition.MoonPhaseWaningGibbous).Add(3069, Condition.MoonPhaseThirdQuarter, Condition.NotRemixWorld)
			.Add(517, Condition.MoonPhaseThirdQuarter, Condition.RemixWorld)
			.Add(4341, Condition.MoonPhaseWaningCrescent)
			.Add(285, Condition.MoonPhaseNew)
			.Add(953, Condition.MoonPhaseWaxingCrescent)
			.Add(3068, Condition.MoonPhaseFirstQuarter)
			.Add(3084, Condition.MoonPhaseWaxingGibbous)
			.Add(3001, Condition.MoonPhasesEven)
			.Add(28, Condition.MoonPhasesOdd)
			.Add(188, Condition.Hardmode, Condition.MoonPhasesOdd)
			.Add(3002, spelunkerGlowCondition)
			.Add(5377, spelunkerGlowCondition, Condition.PlayerCarriesItem(930))
			.Add(282, glowstickCondition)
			.Add(3004, boneTorchCondition)
			.Add(8, torchCondition)
			.Add(3003, Condition.MoonPhasesEvenQuarters)
			.Add(40, Condition.MoonPhasesOddQuarters)
			.Add(3310, Condition.MoonPhases04)
			.Add(3313, Condition.MoonPhases15)
			.Add(3312, Condition.MoonPhases26)
			.Add(3311, Condition.MoonPhases37)
			.Add(166)
			.Add(965)
			.Add(3316, Condition.Hardmode, Condition.MoonPhasesHalf0)
			.Add(3315, Condition.Hardmode, Condition.MoonPhasesHalf1)
			.Add(3334, Condition.Hardmode)
			.Add(3258, Condition.Hardmode, Condition.BloodMoon)
			.Add(3043, Condition.TimeNight, Condition.MoonPhaseFull)
			.Add(5326, artisanCondition, Condition.MoonPhasesNearNew)
			.Register();
	}

	private static void RegisterBartender()
	{
		NPCShop shop = new NPCShop(550).AllowFillingLastSlot();
		shop.Add(353);
		shop.Add(new NPCShop.Entry(3828).AddShopOpenedCallback(delegate(Item item, NPC npc)
		{
			if (NPC.downedGolemBoss)
			{
				item.shopCustomPrice = Item.buyPrice(0, 4);
			}
			else if (NPC.downedMechBossAny)
			{
				item.shopCustomPrice = Item.buyPrice(0, 1);
			}
			else
			{
				item.shopCustomPrice = Item.buyPrice(0, 0, 25);
			}
		}));
		shop.Add(3816);
		AddEntry(3813, 50, Array.Empty<Condition>());
		AddEntry(3800, 15, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedMechBossAny
		});
		AddEntry(3801, 15, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedMechBossAny
		});
		AddEntry(3802, 15, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedMechBossAny
		});
		AddEntry(3871, 50, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedGolem
		});
		AddEntry(3872, 50, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedGolem
		});
		AddEntry(3873, 50, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedGolem
		});
		AddEntry(3818, 5, Array.Empty<Condition>());
		AddEntry(3824, 5, Array.Empty<Condition>());
		AddEntry(3832, 5, Array.Empty<Condition>());
		AddEntry(3829, 5, Array.Empty<Condition>());
		AddEntry(3797, 15, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedMechBossAny
		});
		AddEntry(3798, 15, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedMechBossAny
		});
		AddEntry(3799, 15, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedMechBossAny
		});
		AddEntry(3874, 50, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedGolem
		});
		AddEntry(3875, 50, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedGolem
		});
		AddEntry(3876, 50, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedGolem
		});
		AddEntry(3819, 15, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedMechBossAny
		});
		AddEntry(3825, 15, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedMechBossAny
		});
		AddEntry(3833, 15, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedMechBossAny
		});
		AddEntry(3830, 15, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedMechBossAny
		});
		AddEntry(3803, 15, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedMechBossAny
		});
		AddEntry(3804, 15, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedMechBossAny
		});
		AddEntry(3805, 15, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedMechBossAny
		});
		AddEntry(3877, 50, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedGolem
		});
		AddEntry(3878, 50, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedGolem
		});
		AddEntry(3879, 50, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedGolem
		});
		AddEntry(3820, 60, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedGolem
		});
		AddEntry(3826, 60, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedGolem
		});
		AddEntry(3834, 60, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedGolem
		});
		AddEntry(3831, 60, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedGolem
		});
		AddEntry(3806, 15, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedMechBossAny
		});
		AddEntry(3807, 15, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedMechBossAny
		});
		AddEntry(3808, 15, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedMechBossAny
		});
		AddEntry(3880, 50, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedGolem
		});
		AddEntry(3881, 50, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedGolem
		});
		AddEntry(3882, 50, new Condition[2]
		{
			Condition.Hardmode,
			Condition.DownedGolem
		});
		shop.Register();
		void AddEntry(int id, int price, Condition[] conditions)
		{
			shop.Add(new NPCShop.Entry(new Item(id)
			{
				shopCustomPrice = price,
				shopSpecialCurrency = CustomCurrencyID.DefenderMedals
			}, conditions).ReserveSlot());
		}
	}

	private static void RegisterGolfer()
	{
		Condition scoreOver1002 = Condition.GolfScoreOver(500);
		Condition scoreOver1000 = Condition.GolfScoreOver(1000);
		Condition scoreOver1001 = Condition.GolfScoreOver(2000);
		new NPCShop(588).Add(4587).Add(4590).Add(4589)
			.Add(4588)
			.Add(4083)
			.Add(4084)
			.Add(4085)
			.Add(4086)
			.Add(4087)
			.Add(4088)
			.Add(4039, scoreOver1002)
			.Add(4094, scoreOver1002)
			.Add(4093, scoreOver1002)
			.Add(4092, scoreOver1002)
			.Add(4089)
			.Add(3989)
			.Add(4095)
			.Add(4040)
			.Add(4319)
			.Add(4320)
			.Add(4591, scoreOver1000)
			.Add(4594, scoreOver1000)
			.Add(4593, scoreOver1000)
			.Add(4592, scoreOver1000)
			.Add(4135)
			.Add(4138)
			.Add(4136)
			.Add(4137)
			.Add(4049)
			.Add(4265, scoreOver1002)
			.Add(4595, scoreOver1001)
			.Add(4598, scoreOver1001)
			.Add(4597, scoreOver1001)
			.Add(4596, scoreOver1001)
			.Add(4264, scoreOver1001, Condition.DownedSkeletron)
			.Add(4599, scoreOver1002)
			.Add(4600, scoreOver1000)
			.Add(4601, scoreOver1001)
			.Add(4658, scoreOver1001, Condition.MoonPhasesQuarter0)
			.Add(4659, scoreOver1001, Condition.MoonPhasesQuarter1)
			.Add(4660, scoreOver1001, Condition.MoonPhasesQuarter2)
			.Add(4661, scoreOver1001, Condition.MoonPhasesQuarter3)
			.Register();
	}

	private static void RegisterZoologist()
	{
		new NPCShop(633).Add(4776, new Condition("Conditions.BestiaryWinx", () => Chest.BestiaryGirl_IsFairyTorchAvailable())).Add(4767).Add(4759)
			.Add(5253, Condition.MoonPhaseFull, Condition.TimeNight)
			.Add(4672, BestiaryFilledPercent(10))
			.Add(4829)
			.Add(4830, BestiaryFilledPercent(25))
			.Add(4910, BestiaryFilledPercent(45))
			.Add(4871, BestiaryFilledPercent(30))
			.Add(4907, BestiaryFilledPercent(30))
			.Add(4677, Condition.DownedSolarPillar)
			.Add(4676, BestiaryFilledPercent(10))
			.Add(4762, BestiaryFilledPercent(30))
			.Add(4716, BestiaryFilledPercent(25))
			.Add(4785, BestiaryFilledPercent(30))
			.Add(4786, BestiaryFilledPercent(30))
			.Add(4787, BestiaryFilledPercent(30))
			.Add(4788, BestiaryFilledPercent(30), Condition.Hardmode)
			.Add(4763, BestiaryFilledPercent(35))
			.Add(4955, BestiaryFilledPercent(40))
			.Add(4736, Condition.Hardmode, Condition.BloodMoon)
			.Add(4701, Condition.DownedPlantera)
			.Add(4765, BestiaryFilledPercent(50))
			.Add(4766, BestiaryFilledPercent(50))
			.Add(5285, BestiaryFilledPercent(50))
			.Add(4777, BestiaryFilledPercent(50))
			.Add(4735, BestiaryFilledPercent(70))
			.Add(4951, new Condition("Conditions.BestiaryFull", () => Main.GetBestiaryProgressReport().CompletionPercent >= 1f))
			.Add(4768, Condition.MoonPhasesQuarter0)
			.Add(4769, Condition.MoonPhasesQuarter0)
			.Add(4770, Condition.MoonPhasesQuarter1)
			.Add(4771, Condition.MoonPhasesQuarter1)
			.Add(4772, Condition.MoonPhasesQuarter2)
			.Add(4773, Condition.MoonPhasesQuarter2)
			.Add(4560, Condition.MoonPhasesQuarter3)
			.Add(4775, Condition.MoonPhasesQuarter3)
			.Register();
		static Condition BestiaryFilledPercent(int percent)
		{
			return new Condition(Language.GetText("Conditions.BestiaryPercentage").WithFormatArgs(percent), () => Main.GetBestiaryProgressReport().CompletionPercent >= (float)percent / 100f);
		}
	}

	private static void RegisterPrincess()
	{
		NPCShop shop = new NPCShop(663).Add(5071).Add(5072).Add(5073);
		for (int i = 5076; i <= 5087; i++)
		{
			shop.Add(i);
		}
		shop.Add(5310).Add(5222).Add(5228)
			.Add(5266, Condition.DownedKingSlime, Condition.DownedQueenSlime)
			.Add(5044, Condition.Hardmode, Condition.DownedMoonLord)
			.Add(1309, Condition.TenthAnniversaryWorld)
			.Add(1859, Condition.TenthAnniversaryWorld)
			.Add(1358, Condition.TenthAnniversaryWorld)
			.Add(857, Condition.TenthAnniversaryWorld, Condition.InDesert)
			.Add(4144, Condition.TenthAnniversaryWorld, Condition.BloodMoon)
			.Add(2584, Condition.TenthAnniversaryWorld, Condition.Hardmode, Condition.DownedPirates, Condition.MoonPhasesQuarter0)
			.Add(854, Condition.TenthAnniversaryWorld, Condition.Hardmode, Condition.DownedPirates, Condition.MoonPhasesQuarter1)
			.Add(855, Condition.TenthAnniversaryWorld, Condition.Hardmode, Condition.DownedPirates, Condition.MoonPhasesQuarter2)
			.Add(905, Condition.TenthAnniversaryWorld, Condition.Hardmode, Condition.DownedPirates, Condition.MoonPhasesQuarter3)
			.Add(5088)
			.Register();
	}

	private static void RegisterTravellingMerchant()
	{
		new TravellingMerchantShop(368).AddInfoEntry(3309).AddInfoEntry(3314).AddInfoEntry(1987)
			.AddInfoEntry(2270, Condition.Hardmode)
			.AddInfoEntry(4760, Condition.Hardmode)
			.AddInfoEntry(2278)
			.AddInfoEntry(2271)
			.AddInfoEntry(2223, Condition.DownedDestroyer, Condition.DownedTwins, Condition.DownedSkeletronPrime)
			.AddInfoEntry(2272)
			.AddInfoEntry(2276)
			.AddInfoEntry(2284)
			.AddInfoEntry(2285)
			.AddInfoEntry(2286)
			.AddInfoEntry(2287)
			.AddInfoEntry(4744)
			.AddInfoEntry(2296, Condition.DownedSkeletron)
			.AddInfoEntry(3628)
			.AddInfoEntry(4091, Condition.Hardmode)
			.AddInfoEntry(4603)
			.AddInfoEntry(4604)
			.AddInfoEntry(5297)
			.AddInfoEntry(4605)
			.AddInfoEntry(4550)
			.AddInfoEntry(2268)
			.AddInfoEntry(2269, Condition.SmashedShadowOrb)
			.AddInfoEntry(1988)
			.AddInfoEntry(2275)
			.AddInfoEntry(2279)
			.AddInfoEntry(2277)
			.AddInfoEntry(4555)
			.AddInfoEntry(4321)
			.AddInfoEntry(4323)
			.AddInfoEntry(5390)
			.AddInfoEntry(4549)
			.AddInfoEntry(4561)
			.AddInfoEntry(4774)
			.AddInfoEntry(5136)
			.AddInfoEntry(5305)
			.AddInfoEntry(4562)
			.AddInfoEntry(4558)
			.AddInfoEntry(4559)
			.AddInfoEntry(4563)
			.AddInfoEntry(4666)
			.AddInfoEntry(4347, Condition.DownedEarlygameBoss)
			.AddInfoEntry(4348, Condition.Hardmode)
			.AddInfoEntry(3262, Condition.DownedEyeOfCthulhu)
			.AddInfoEntry(3284, Condition.DownedMechBossAny)
			.AddInfoEntry(2267)
			.AddInfoEntry(2214)
			.AddInfoEntry(2215)
			.AddInfoEntry(2216)
			.AddInfoEntry(2217)
			.AddInfoEntry(3624)
			.AddInfoEntry(671, Condition.RemixWorld)
			.AddInfoEntry(2273, Condition.NotRemixWorld)
			.AddInfoEntry(2274)
			.AddInfoEntry(2266)
			.AddInfoEntry(2281)
			.AddInfoEntry(2282)
			.AddInfoEntry(2283)
			.AddInfoEntry(2258)
			.AddInfoEntry(2242)
			.AddInfoEntry(2260)
			.AddInfoEntry(3637)
			.AddInfoEntry(4420)
			.AddInfoEntry(3119)
			.AddInfoEntry(3118)
			.AddInfoEntry(3099)
			.Register();
	}

	private static void InitShopTestSystem()
	{
	}

	internal static void Test()
	{
	}
}
