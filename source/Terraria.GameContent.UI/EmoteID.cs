using ReLogic.Reflection;

namespace Terraria.GameContent.UI;

public class EmoteID
{
	/// <summary>
	/// This class is added by TML for easily adding mod emotes to a specific category.
	/// </summary>
	public class Category
	{
		/// <summary>
		/// <b>General</b> Emotes
		/// </summary>
		public const int General = 0;

		/// <summary>
		/// <b>Rock, Paper, Scissors!</b> Emotes
		/// </summary>
		public const int Rps = 1;

		/// <summary>
		/// <b>Items</b> Emotes
		/// </summary>
		public const int Items = 2;

		/// <summary>
		/// <b>Nature &amp; Weather</b> Emotes
		/// </summary>
		public const int NatureAndWeather = 3;

		/// <summary>
		/// <b>Town</b> Emotes
		/// </summary>
		public const int Town = 4;

		/// <summary>
		/// <b>Critters &amp; Monsters</b> Emotes
		/// </summary>
		public const int CrittersAndMonsters = 5;

		/// <summary>
		/// <b>Dangers</b> Emotes (For bosses)
		/// </summary>
		public const int Dangers = 6;
	}

	public const int ItemDisplay = -1;

	public static readonly int Count = 151;

	public const int RPSWinScissors = 33;

	public const int RPSWinRock = 34;

	public const int RPSWinPaper = 35;

	public const int RPSScissors = 36;

	public const int RPSRock = 37;

	public const int RPSPaper = 38;

	public const int WeatherRain = 4;

	public const int WeatherLightning = 5;

	public const int WeatherRainbow = 6;

	public const int WeatherSunny = 95;

	public const int WeatherCloudy = 96;

	public const int WeatherStorming = 97;

	public const int WeatherSnowstorm = 98;

	public const int EventBloodmoon = 18;

	public const int EventEclipse = 19;

	public const int EventPumpkin = 20;

	public const int EventSnow = 21;

	public const int EventMeteor = 99;

	public const int ItemRing = 7;

	public const int ItemLifePotion = 73;

	public const int ItemManaPotion = 74;

	public const int ItemSoup = 75;

	public const int ItemCookedFish = 76;

	public const int ItemAle = 77;

	public const int ItemSword = 78;

	public const int ItemFishingRod = 79;

	public const int ItemBugNet = 80;

	public const int ItemDynamite = 81;

	public const int ItemMinishark = 82;

	public const int ItemCog = 83;

	public const int ItemTombstone = 84;

	public const int ItemGoldpile = 85;

	public const int ItemDiamondRing = 86;

	public const int ItemPickaxe = 90;

	public const int DebuffPoison = 8;

	public const int DebuffBurn = 9;

	public const int DebuffSilence = 10;

	public const int DebuffCurse = 11;

	public const int CritterBee = 12;

	public const int CritterSlime = 13;

	public const int CritterZombie = 61;

	public const int CritterBunny = 62;

	public const int CritterButterfly = 63;

	public const int CritterGoblin = 64;

	public const int CritterPirate = 65;

	public const int CritterSnowman = 66;

	public const int CritterSpider = 67;

	public const int CritterBird = 68;

	public const int CritterMouse = 69;

	public const int CritterGoldfish = 70;

	public const int CritterMartian = 71;

	public const int CritterSkeleton = 72;

	public const int BossEoC = 39;

	public const int BossEoW = 40;

	public const int BossBoC = 41;

	public const int BossKingSlime = 51;

	public const int BossQueenBee = 42;

	public const int BossSkeletron = 43;

	public const int BossWoF = 44;

	public const int BossDestroyer = 45;

	public const int BossSkeletronPrime = 46;

	public const int BossTwins = 47;

	public const int BossPlantera = 48;

	public const int BossGolem = 49;

	public const int BossFishron = 50;

	public const int BossCultist = 52;

	public const int BossMoonmoon = 53;

	public const int BossMourningWood = 54;

	public const int BossPumpking = 55;

	public const int BossEverscream = 56;

	public const int BossIceQueen = 57;

	public const int BossSantank = 58;

	public const int BossPirateship = 59;

	public const int BossMartianship = 60;

	public const int BossEmpressOfLight = 143;

	public const int BossQueenSlime = 144;

	public const int EmotionLove = 0;

	public const int EmotionAnger = 1;

	public const int EmotionCry = 2;

	public const int EmotionAlert = 3;

	public const int EmoteLaugh = 15;

	public const int EmoteFear = 16;

	public const int EmoteNote = 17;

	public const int EmoteConfused = 87;

	public const int EmoteKiss = 88;

	public const int EmoteSleep = 89;

	public const int EmoteRun = 91;

	public const int EmoteKick = 92;

	public const int EmoteFight = 93;

	public const int EmoteEating = 94;

	public const int EmoteSadness = 134;

	public const int EmoteAnger = 135;

	public const int EmoteHappiness = 136;

	public const int EmoteWink = 137;

	public const int EmoteScowl = 138;

	public const int EmoteSilly = 139;

	public const int MiscTree = 14;

	public const int MiscFire = 100;

	public const int BiomeSky = 22;

	public const int BiomeOtherworld = 23;

	public const int BiomeJungle = 24;

	public const int BiomeCrimson = 25;

	public const int BiomeCorruption = 26;

	public const int BiomeHallow = 27;

	public const int BiomeDesert = 28;

	public const int BiomeBeach = 29;

	public const int BiomeRocklayer = 30;

	public const int BiomeLavalayer = 31;

	public const int BiomeSnow = 32;

	public const int TownMerchant = 101;

	public const int TownNurse = 102;

	public const int TownArmsDealer = 103;

	public const int TownDryad = 104;

	public const int TownGuide = 105;

	public const int TownOldman = 106;

	public const int TownDemolitionist = 107;

	public const int TownClothier = 108;

	public const int TownGoblinTinkerer = 109;

	public const int TownWizard = 110;

	public const int TownMechanic = 111;

	public const int TownSanta = 112;

	public const int TownTruffle = 113;

	public const int TownSteampunker = 114;

	public const int TownDyeTrader = 115;

	public const int TownPartyGirl = 116;

	public const int TownCyborg = 117;

	public const int TownPainter = 118;

	public const int TownWitchDoctor = 119;

	public const int TownPirate = 120;

	public const int TownStylist = 121;

	public const int TownTravellingMerchant = 122;

	public const int TownAngler = 123;

	public const int TownSkeletonMerchant = 124;

	public const int TownTaxCollector = 125;

	public const int TownGolfer = 140;

	public const int TownBestiaryGirl = 141;

	public const int TownBestiaryGirlFox = 142;

	public const int TownPrincess = 145;

	public const int PartyPresent = 126;

	public const int PartyBalloons = 127;

	public const int PartyCake = 128;

	public const int PartyHats = 129;

	public const int TownBartender = 130;

	public const int ItemBeer = 131;

	public const int ItemDefenderMedal = 132;

	public const int EventOldOnesArmy = 133;

	public const int Peckish = 146;

	public const int Hungry = 147;

	public const int Starving = 148;

	public const int LucyTheAxe = 149;

	public const int BossDeerclops = 150;

	public static readonly IdDictionary Search = IdDictionary.Create<EmoteID, int>();
}
