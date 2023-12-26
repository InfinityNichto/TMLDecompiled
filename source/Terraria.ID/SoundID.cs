using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Terraria.Audio;

namespace Terraria.ID;

public class SoundID
{
	internal struct SoundStyleDefaults
	{
		public readonly float PitchVariance;

		public readonly float Volume;

		public readonly SoundType Type;

		public static readonly SoundStyleDefaults ItemDefaults = new SoundStyleDefaults(1f, 0.12f);

		public static readonly SoundStyleDefaults NPCHitDefaults = new SoundStyleDefaults(1f, 0.2f);

		public static readonly SoundStyleDefaults NPCDeathDefaults = new SoundStyleDefaults(1f, 0.2f);

		public static readonly SoundStyleDefaults ZombieDefaults = new SoundStyleDefaults(1f, 0.2f);

		public SoundStyleDefaults(float volume, float pitchVariance, SoundType type = SoundType.Sound)
		{
			PitchVariance = pitchVariance;
			Volume = volume;
			Type = type;
		}

		public void Apply(ref SoundStyle style)
		{
			style.PitchVariance = PitchVariance;
			style.Volume = Volume;
			style.Type = Type;
		}
	}

	public static int ItemSoundCount;

	public static readonly SoundStyle DD2_GoblinBomb;

	public static readonly SoundStyle AchievementComplete;

	public static readonly SoundStyle BlizzardInsideBuildingLoop;

	public static readonly SoundStyle BlizzardStrongLoop;

	public static readonly SoundStyle LiquidsHoneyWater;

	public static readonly SoundStyle LiquidsHoneyLava;

	public static readonly SoundStyle LiquidsWaterLava;

	public static readonly SoundStyle DD2_BallistaTowerShot;

	public static readonly SoundStyle DD2_ExplosiveTrapExplode;

	public static readonly SoundStyle DD2_FlameburstTowerShot;

	public static readonly SoundStyle DD2_LightningAuraZap;

	public static readonly SoundStyle DD2_DefenseTowerSpawn;

	public static readonly SoundStyle DD2_BetsyDeath;

	public static readonly SoundStyle DD2_BetsyFireballShot;

	public static readonly SoundStyle DD2_BetsyFireballImpact;

	public static readonly SoundStyle DD2_BetsyFlameBreath;

	public static readonly SoundStyle DD2_BetsyFlyingCircleAttack;

	public static readonly SoundStyle DD2_BetsyHurt;

	public static readonly SoundStyle DD2_BetsyScream;

	public static readonly SoundStyle DD2_BetsySummon;

	public static readonly SoundStyle DD2_BetsyWindAttack;

	public static readonly SoundStyle DD2_DarkMageAttack;

	public static readonly SoundStyle DD2_DarkMageCastHeal;

	public static readonly SoundStyle DD2_DarkMageDeath;

	public static readonly SoundStyle DD2_DarkMageHealImpact;

	public static readonly SoundStyle DD2_DarkMageHurt;

	public static readonly SoundStyle DD2_DarkMageSummonSkeleton;

	public static readonly SoundStyle DD2_DrakinBreathIn;

	public static readonly SoundStyle DD2_DrakinDeath;

	public static readonly SoundStyle DD2_DrakinHurt;

	public static readonly SoundStyle DD2_DrakinShot;

	public static readonly SoundStyle DD2_GoblinDeath;

	public static readonly SoundStyle DD2_GoblinHurt;

	public static readonly SoundStyle DD2_GoblinScream;

	public static readonly SoundStyle DD2_GoblinBomberDeath;

	public static readonly SoundStyle DD2_GoblinBomberHurt;

	public static readonly SoundStyle DD2_GoblinBomberScream;

	public static readonly SoundStyle DD2_GoblinBomberThrow;

	public static readonly SoundStyle DD2_JavelinThrowersAttack;

	public static readonly SoundStyle DD2_JavelinThrowersDeath;

	public static readonly SoundStyle DD2_JavelinThrowersHurt;

	public static readonly SoundStyle DD2_JavelinThrowersTaunt;

	public static readonly SoundStyle DD2_KoboldDeath;

	public static readonly SoundStyle DD2_KoboldExplosion;

	public static readonly SoundStyle DD2_KoboldHurt;

	public static readonly SoundStyle DD2_KoboldIgnite;

	public static readonly SoundStyle DD2_KoboldIgniteLoop;

	public static readonly SoundStyle DD2_KoboldScreamChargeLoop;

	public static readonly SoundStyle DD2_KoboldFlyerChargeScream;

	public static readonly SoundStyle DD2_KoboldFlyerDeath;

	public static readonly SoundStyle DD2_KoboldFlyerHurt;

	public static readonly SoundStyle DD2_LightningBugDeath;

	public static readonly SoundStyle DD2_LightningBugHurt;

	public static readonly SoundStyle DD2_LightningBugZap;

	public static readonly SoundStyle DD2_OgreAttack;

	public static readonly SoundStyle DD2_OgreDeath;

	public static readonly SoundStyle DD2_OgreGroundPound;

	public static readonly SoundStyle DD2_OgreHurt;

	public static readonly SoundStyle DD2_OgreRoar;

	public static readonly SoundStyle DD2_OgreSpit;

	public static readonly SoundStyle DD2_SkeletonDeath;

	public static readonly SoundStyle DD2_SkeletonHurt;

	public static readonly SoundStyle DD2_SkeletonSummoned;

	public static readonly SoundStyle DD2_WitherBeastAuraPulse;

	public static readonly SoundStyle DD2_WitherBeastCrystalImpact;

	public static readonly SoundStyle DD2_WitherBeastDeath;

	public static readonly SoundStyle DD2_WitherBeastHurt;

	public static readonly SoundStyle DD2_WyvernDeath;

	public static readonly SoundStyle DD2_WyvernHurt;

	public static readonly SoundStyle DD2_WyvernScream;

	public static readonly SoundStyle DD2_WyvernDiveDown;

	public static readonly SoundStyle DD2_EtherianPortalDryadTouch;

	public static readonly SoundStyle DD2_EtherianPortalIdleLoop;

	public static readonly SoundStyle DD2_EtherianPortalOpen;

	public static readonly SoundStyle DD2_EtherianPortalSpawnEnemy;

	public static readonly SoundStyle DD2_CrystalCartImpact;

	public static readonly SoundStyle DD2_DefeatScene;

	public static readonly SoundStyle DD2_WinScene;

	public static readonly SoundStyle DD2_BetsysWrathShot;

	public static readonly SoundStyle DD2_BetsysWrathImpact;

	public static readonly SoundStyle DD2_BookStaffCast;

	public static readonly SoundStyle DD2_BookStaffTwisterLoop;

	public static readonly SoundStyle DD2_GhastlyGlaiveImpactGhost;

	public static readonly SoundStyle DD2_GhastlyGlaivePierce;

	public static readonly SoundStyle DD2_MonkStaffGroundImpact;

	public static readonly SoundStyle DD2_MonkStaffGroundMiss;

	public static readonly SoundStyle DD2_MonkStaffSwing;

	public static readonly SoundStyle DD2_PhantomPhoenixShot;

	public static readonly SoundStyle DD2_SonicBoomBladeSlash;

	public static readonly SoundStyle DD2_SkyDragonsFuryCircle;

	public static readonly SoundStyle DD2_SkyDragonsFuryShot;

	public static readonly SoundStyle DD2_SkyDragonsFurySwing;

	public static readonly SoundStyle LucyTheAxeTalk;

	public static readonly SoundStyle DeerclopsHit;

	public static readonly SoundStyle DeerclopsDeath;

	public static readonly SoundStyle DeerclopsScream;

	public static readonly SoundStyle DeerclopsIceAttack;

	public static readonly SoundStyle DeerclopsRubbleAttack;

	public static readonly SoundStyle DeerclopsStep;

	public static readonly SoundStyle ChesterOpen;

	public static readonly SoundStyle ChesterClose;

	public static readonly SoundStyle AbigailSummon;

	public static readonly SoundStyle AbigailCry;

	public static readonly SoundStyle AbigailAttack;

	public static readonly SoundStyle AbigailUpgrade;

	public static readonly SoundStyle GlommerBounce;

	public static readonly SoundStyle DSTMaleHurt;

	public static readonly SoundStyle DSTFemaleHurt;

	public static readonly SoundStyle JimsDrone;

	private static List<string> _trackableLegacySoundPathList;

	public static Dictionary<string, SoundStyle> SoundByName;

	public static Dictionary<string, ushort> IndexByName;

	public static Dictionary<ushort, SoundStyle> SoundByIndex;

	private const string Prefix = "Terraria/Sounds/";

	private const string PrefixCustom = "Terraria/Sounds/Custom/";

	public static readonly SoundStyle Dig;

	public static readonly SoundStyle PlayerHit;

	public static readonly SoundStyle Item;

	public static readonly SoundStyle PlayerKilled;

	public static readonly SoundStyle Grass;

	public static readonly SoundStyle Grab;

	public static readonly SoundStyle DoorOpen;

	public static readonly SoundStyle DoorClosed;

	public static readonly SoundStyle MenuOpen;

	public static readonly SoundStyle MenuClose;

	public static readonly SoundStyle MenuTick;

	public static readonly SoundStyle Shatter;

	public static readonly SoundStyle ZombieMoan;

	public static readonly SoundStyle SandShark;

	public static readonly SoundStyle BloodZombie;

	public static readonly SoundStyle Roar;

	public static readonly SoundStyle WormDig;

	public static readonly SoundStyle WormDigQuiet;

	public static readonly SoundStyle ScaryScream;

	public static readonly SoundStyle DoubleJump;

	public static readonly SoundStyle Run;

	public static readonly SoundStyle Coins;

	public static readonly SoundStyle Splash;

	public static readonly SoundStyle SplashWeak;

	public static readonly SoundStyle Shimmer1;

	public static readonly SoundStyle Shimmer2;

	public static readonly SoundStyle ShimmerWeak1;

	public static readonly SoundStyle ShimmerWeak2;

	public static readonly SoundStyle FemaleHit;

	public static readonly SoundStyle Tink;

	public static readonly SoundStyle Unlock;

	public static readonly SoundStyle Drown;

	public static readonly SoundStyle Chat;

	public static readonly SoundStyle MaxMana;

	public static readonly SoundStyle Mummy;

	public static readonly SoundStyle Pixie;

	public static readonly SoundStyle Mech;

	public static readonly SoundStyle Duck;

	public static readonly SoundStyle Frog;

	public static readonly SoundStyle Bird;

	public static readonly SoundStyle Critter;

	public static readonly SoundStyle Waterfall;

	public static readonly SoundStyle Lavafall;

	public static readonly SoundStyle ForceRoar;

	public static readonly SoundStyle ForceRoarPitched;

	public static readonly SoundStyle Meowmere;

	public static readonly SoundStyle CoinPickup;

	public static readonly SoundStyle Drip;

	public static readonly SoundStyle Camera;

	public static readonly SoundStyle MoonLord;

	public static readonly SoundStyle Thunder;

	public static readonly SoundStyle Seagull;

	public static readonly SoundStyle Dolphin;

	public static readonly SoundStyle Owl;

	public static readonly SoundStyle GuitarC;

	public static readonly SoundStyle GuitarD;

	public static readonly SoundStyle GuitarEm;

	public static readonly SoundStyle GuitarG;

	public static readonly SoundStyle GuitarBm;

	public static readonly SoundStyle GuitarAm;

	public static readonly SoundStyle DrumHiHat;

	public static readonly SoundStyle DrumTomHigh;

	public static readonly SoundStyle DrumTomLow;

	public static readonly SoundStyle DrumTomMid;

	public static readonly SoundStyle DrumClosedHiHat;

	public static readonly SoundStyle DrumCymbal1;

	public static readonly SoundStyle DrumCymbal2;

	public static readonly SoundStyle DrumKick;

	public static readonly SoundStyle DrumTamaSnare;

	public static readonly SoundStyle DrumFloorTom;

	public static readonly SoundStyle Research;

	public static readonly SoundStyle ResearchComplete;

	public static readonly SoundStyle QueenSlime;

	public static readonly SoundStyle Clown;

	public static readonly SoundStyle Cockatiel;

	public static readonly SoundStyle Macaw;

	public static readonly SoundStyle Toucan;

	public static readonly SoundStyle NPCHit1;

	public static readonly SoundStyle NPCHit2;

	public static readonly SoundStyle NPCHit3;

	public static readonly SoundStyle NPCHit4;

	public static readonly SoundStyle NPCHit5;

	public static readonly SoundStyle NPCHit6;

	public static readonly SoundStyle NPCHit7;

	public static readonly SoundStyle NPCHit8;

	public static readonly SoundStyle NPCHit9;

	public static readonly SoundStyle NPCHit10;

	public static readonly SoundStyle NPCHit11;

	public static readonly SoundStyle NPCHit12;

	public static readonly SoundStyle NPCHit13;

	public static readonly SoundStyle NPCHit14;

	public static readonly SoundStyle NPCHit15;

	public static readonly SoundStyle NPCHit16;

	public static readonly SoundStyle NPCHit17;

	public static readonly SoundStyle NPCHit18;

	public static readonly SoundStyle NPCHit19;

	public static readonly SoundStyle NPCHit20;

	public static readonly SoundStyle NPCHit21;

	public static readonly SoundStyle NPCHit22;

	public static readonly SoundStyle NPCHit23;

	public static readonly SoundStyle NPCHit24;

	public static readonly SoundStyle NPCHit25;

	public static readonly SoundStyle NPCHit26;

	public static readonly SoundStyle NPCHit27;

	public static readonly SoundStyle NPCHit28;

	public static readonly SoundStyle NPCHit29;

	public static readonly SoundStyle NPCHit30;

	public static readonly SoundStyle NPCHit31;

	public static readonly SoundStyle NPCHit32;

	public static readonly SoundStyle NPCHit33;

	public static readonly SoundStyle NPCHit34;

	public static readonly SoundStyle NPCHit35;

	public static readonly SoundStyle NPCHit36;

	public static readonly SoundStyle NPCHit37;

	public static readonly SoundStyle NPCHit38;

	public static readonly SoundStyle NPCHit39;

	public static readonly SoundStyle NPCHit40;

	public static readonly SoundStyle NPCHit41;

	public static readonly SoundStyle NPCHit42;

	public static readonly SoundStyle NPCHit43;

	public static readonly SoundStyle NPCHit44;

	public static readonly SoundStyle NPCHit45;

	public static readonly SoundStyle NPCHit46;

	public static readonly SoundStyle NPCHit47;

	public static readonly SoundStyle NPCHit48;

	public static readonly SoundStyle NPCHit49;

	public static readonly SoundStyle NPCHit50;

	public static readonly SoundStyle NPCHit51;

	public static readonly SoundStyle NPCHit52;

	public static readonly SoundStyle NPCHit53;

	public static readonly SoundStyle NPCHit54;

	public static readonly SoundStyle NPCHit55;

	public static readonly SoundStyle NPCHit56;

	public static readonly SoundStyle NPCHit57;

	public static int NPCHitCount;

	public static readonly SoundStyle NPCDeath1;

	public static readonly SoundStyle NPCDeath2;

	public static readonly SoundStyle NPCDeath3;

	public static readonly SoundStyle NPCDeath4;

	public static readonly SoundStyle NPCDeath5;

	public static readonly SoundStyle NPCDeath6;

	public static readonly SoundStyle NPCDeath7;

	public static readonly SoundStyle NPCDeath8;

	public static readonly SoundStyle NPCDeath9;

	public static readonly SoundStyle NPCDeath10;

	public static readonly SoundStyle NPCDeath11;

	public static readonly SoundStyle NPCDeath12;

	public static readonly SoundStyle NPCDeath13;

	public static readonly SoundStyle NPCDeath14;

	public static readonly SoundStyle NPCDeath15;

	public static readonly SoundStyle NPCDeath16;

	public static readonly SoundStyle NPCDeath17;

	public static readonly SoundStyle NPCDeath18;

	public static readonly SoundStyle NPCDeath19;

	public static readonly SoundStyle NPCDeath20;

	public static readonly SoundStyle NPCDeath21;

	public static readonly SoundStyle NPCDeath22;

	public static readonly SoundStyle NPCDeath23;

	public static readonly SoundStyle NPCDeath24;

	public static readonly SoundStyle NPCDeath25;

	public static readonly SoundStyle NPCDeath26;

	public static readonly SoundStyle NPCDeath27;

	public static readonly SoundStyle NPCDeath28;

	public static readonly SoundStyle NPCDeath29;

	public static readonly SoundStyle NPCDeath30;

	public static readonly SoundStyle NPCDeath31;

	public static readonly SoundStyle NPCDeath32;

	public static readonly SoundStyle NPCDeath33;

	public static readonly SoundStyle NPCDeath34;

	public static readonly SoundStyle NPCDeath35;

	public static readonly SoundStyle NPCDeath36;

	public static readonly SoundStyle NPCDeath37;

	public static readonly SoundStyle NPCDeath38;

	public static readonly SoundStyle NPCDeath39;

	public static readonly SoundStyle NPCDeath40;

	public static readonly SoundStyle NPCDeath41;

	public static readonly SoundStyle NPCDeath42;

	public static readonly SoundStyle NPCDeath43;

	public static readonly SoundStyle NPCDeath44;

	public static readonly SoundStyle NPCDeath45;

	public static readonly SoundStyle NPCDeath46;

	public static readonly SoundStyle NPCDeath47;

	public static readonly SoundStyle NPCDeath48;

	public static readonly SoundStyle NPCDeath49;

	public static readonly SoundStyle NPCDeath50;

	public static readonly SoundStyle NPCDeath51;

	public static readonly SoundStyle NPCDeath52;

	public static readonly SoundStyle NPCDeath53;

	public static readonly SoundStyle NPCDeath54;

	public static readonly SoundStyle NPCDeath55;

	public static readonly SoundStyle NPCDeath56;

	public static readonly SoundStyle NPCDeath57;

	public static readonly SoundStyle NPCDeath58;

	public static readonly SoundStyle NPCDeath59;

	public static readonly SoundStyle NPCDeath60;

	public static readonly SoundStyle NPCDeath61;

	public static readonly SoundStyle NPCDeath62;

	public static readonly SoundStyle NPCDeath63;

	public static readonly SoundStyle NPCDeath64;

	public static readonly SoundStyle NPCDeath65;

	public static readonly SoundStyle NPCDeath66;

	public static int NPCDeathCount;

	public static readonly SoundStyle Item1;

	public static readonly SoundStyle Item2;

	public static readonly SoundStyle Item3;

	public static readonly SoundStyle Item4;

	public static readonly SoundStyle Item5;

	public static readonly SoundStyle Item6;

	public static readonly SoundStyle Item7;

	public static readonly SoundStyle Item8;

	public static readonly SoundStyle Item9;

	public static readonly SoundStyle Item10;

	public static readonly SoundStyle Item11;

	public static readonly SoundStyle Item12;

	public static readonly SoundStyle Item13;

	public static readonly SoundStyle Item14;

	public static readonly SoundStyle Item15;

	public static readonly SoundStyle Item16;

	public static readonly SoundStyle Item17;

	public static readonly SoundStyle Item18;

	public static readonly SoundStyle Item19;

	public static readonly SoundStyle Item20;

	public static readonly SoundStyle Item21;

	public static readonly SoundStyle Item22;

	public static readonly SoundStyle Item23;

	public static readonly SoundStyle Item24;

	public static readonly SoundStyle Item25;

	public static readonly SoundStyle Item26;

	public static readonly SoundStyle Item27;

	public static readonly SoundStyle Item28;

	public static readonly SoundStyle Item29;

	public static readonly SoundStyle Item30;

	public static readonly SoundStyle Item31;

	public static readonly SoundStyle Item32;

	public static readonly SoundStyle Item33;

	public static readonly SoundStyle Item34;

	public static readonly SoundStyle Item35;

	public static readonly SoundStyle Item36;

	public static readonly SoundStyle Item37;

	public static readonly SoundStyle Item38;

	public static readonly SoundStyle Item39;

	public static readonly SoundStyle Item40;

	public static readonly SoundStyle Item41;

	public static readonly SoundStyle Item42;

	public static readonly SoundStyle Item43;

	public static readonly SoundStyle Item44;

	public static readonly SoundStyle Item45;

	public static readonly SoundStyle Item46;

	public static readonly SoundStyle Item47;

	public static readonly SoundStyle Item48;

	public static readonly SoundStyle Item49;

	public static readonly SoundStyle Item50;

	public static readonly SoundStyle Item51;

	public static readonly SoundStyle Item52;

	public static readonly SoundStyle Item53;

	public static readonly SoundStyle Item54;

	public static readonly SoundStyle Item55;

	public static readonly SoundStyle Item56;

	public static readonly SoundStyle Item57;

	public static readonly SoundStyle Item58;

	public static readonly SoundStyle Item59;

	public static readonly SoundStyle Item60;

	public static readonly SoundStyle Item61;

	public static readonly SoundStyle Item62;

	public static readonly SoundStyle Item63;

	public static readonly SoundStyle Item64;

	public static readonly SoundStyle Item65;

	public static readonly SoundStyle Item66;

	public static readonly SoundStyle Item67;

	public static readonly SoundStyle Item68;

	public static readonly SoundStyle Item69;

	public static readonly SoundStyle Item70;

	public static readonly SoundStyle Item71;

	public static readonly SoundStyle Item72;

	public static readonly SoundStyle Item73;

	public static readonly SoundStyle Item74;

	public static readonly SoundStyle Item75;

	public static readonly SoundStyle Item76;

	public static readonly SoundStyle Item77;

	public static readonly SoundStyle Item78;

	public static readonly SoundStyle Item79;

	public static readonly SoundStyle Item80;

	public static readonly SoundStyle Item81;

	public static readonly SoundStyle Item82;

	public static readonly SoundStyle Item83;

	public static readonly SoundStyle Item84;

	public static readonly SoundStyle Item85;

	public static readonly SoundStyle Item86;

	public static readonly SoundStyle Item87;

	public static readonly SoundStyle Item88;

	public static readonly SoundStyle Item89;

	public static readonly SoundStyle Item90;

	public static readonly SoundStyle Item91;

	public static readonly SoundStyle Item92;

	public static readonly SoundStyle Item93;

	public static readonly SoundStyle Item94;

	public static readonly SoundStyle Item95;

	public static readonly SoundStyle Item96;

	public static readonly SoundStyle Item97;

	public static readonly SoundStyle Item98;

	public static readonly SoundStyle Item99;

	public static readonly SoundStyle Item100;

	public static readonly SoundStyle Item101;

	public static readonly SoundStyle Item102;

	public static readonly SoundStyle Item103;

	public static readonly SoundStyle Item104;

	public static readonly SoundStyle Item105;

	public static readonly SoundStyle Item106;

	public static readonly SoundStyle Item107;

	public static readonly SoundStyle Item108;

	public static readonly SoundStyle Item109;

	public static readonly SoundStyle Item110;

	public static readonly SoundStyle Item111;

	public static readonly SoundStyle Item112;

	public static readonly SoundStyle Item113;

	public static readonly SoundStyle Item114;

	public static readonly SoundStyle Item115;

	public static readonly SoundStyle Item116;

	public static readonly SoundStyle Item117;

	public static readonly SoundStyle Item118;

	public static readonly SoundStyle Item119;

	public static readonly SoundStyle Item120;

	public static readonly SoundStyle Item121;

	public static readonly SoundStyle Item122;

	public static readonly SoundStyle Item123;

	public static readonly SoundStyle Item124;

	public static readonly SoundStyle Item125;

	public static readonly SoundStyle Item126;

	public static readonly SoundStyle Item127;

	public static readonly SoundStyle Item128;

	public static readonly SoundStyle Item129;

	public static readonly SoundStyle Item130;

	public static readonly SoundStyle Item131;

	public static readonly SoundStyle Item132;

	public static readonly SoundStyle Item133;

	public static readonly SoundStyle Item134;

	public static readonly SoundStyle Item135;

	public static readonly SoundStyle Item136;

	public static readonly SoundStyle Item137;

	public static readonly SoundStyle Item138;

	public static readonly SoundStyle Item139;

	public static readonly SoundStyle Item140;

	public static readonly SoundStyle Item141;

	public static readonly SoundStyle Item142;

	public static readonly SoundStyle Item143;

	public static readonly SoundStyle Item144;

	public static readonly SoundStyle Item145;

	public static readonly SoundStyle Item146;

	public static readonly SoundStyle Item147;

	public static readonly SoundStyle Item148;

	public static readonly SoundStyle Item149;

	public static readonly SoundStyle Item150;

	public static readonly SoundStyle Item151;

	public static readonly SoundStyle Item152;

	public static readonly SoundStyle Item153;

	public static readonly SoundStyle Item154;

	public static readonly SoundStyle Item155;

	public static readonly SoundStyle Item156;

	public static readonly SoundStyle Item157;

	public static readonly SoundStyle Item158;

	public static readonly SoundStyle Item159;

	public static readonly SoundStyle Item160;

	public static readonly SoundStyle Item161;

	public static readonly SoundStyle Item162;

	public static readonly SoundStyle Item163;

	public static readonly SoundStyle Item164;

	public static readonly SoundStyle Item165;

	public static readonly SoundStyle Item166;

	public static readonly SoundStyle Item167;

	public static readonly SoundStyle Item168;

	public static readonly SoundStyle Item169;

	public static readonly SoundStyle Item170;

	public static readonly SoundStyle Item171;

	public static readonly SoundStyle Item172;

	public static readonly SoundStyle Item173;

	public static readonly SoundStyle Item174;

	public static readonly SoundStyle Item175;

	public static readonly SoundStyle Item176;

	public static readonly SoundStyle Item177;

	public static readonly SoundStyle Item178;

	public static readonly SoundStyle Zombie1;

	public static readonly SoundStyle Zombie2;

	public static readonly SoundStyle Zombie3;

	public static readonly SoundStyle Zombie4;

	public static readonly SoundStyle Zombie5;

	public static readonly SoundStyle Zombie6;

	public static readonly SoundStyle Zombie7;

	public static readonly SoundStyle Zombie8;

	public static readonly SoundStyle Zombie9;

	public static readonly SoundStyle Zombie10;

	public static readonly SoundStyle Zombie11;

	public static readonly SoundStyle Zombie12;

	public static readonly SoundStyle Zombie13;

	public static readonly SoundStyle Zombie14;

	public static readonly SoundStyle Zombie15;

	public static readonly SoundStyle Zombie16;

	public static readonly SoundStyle Zombie17;

	public static readonly SoundStyle Zombie18;

	public static readonly SoundStyle Zombie19;

	public static readonly SoundStyle Zombie20;

	public static readonly SoundStyle Zombie21;

	public static readonly SoundStyle Zombie22;

	public static readonly SoundStyle Zombie23;

	public static readonly SoundStyle Zombie24;

	public static readonly SoundStyle Zombie25;

	public static readonly SoundStyle Zombie26;

	public static readonly SoundStyle Zombie27;

	public static readonly SoundStyle Zombie28;

	public static readonly SoundStyle Zombie29;

	public static readonly SoundStyle Zombie30;

	public static readonly SoundStyle Zombie31;

	public static readonly SoundStyle Zombie32;

	public static readonly SoundStyle Zombie33;

	public static readonly SoundStyle Zombie34;

	public static readonly SoundStyle Zombie35;

	public static readonly SoundStyle Zombie36;

	public static readonly SoundStyle Zombie37;

	public static readonly SoundStyle Zombie38;

	public static readonly SoundStyle Zombie39;

	public static readonly SoundStyle Zombie40;

	public static readonly SoundStyle Zombie41;

	public static readonly SoundStyle Zombie42;

	public static readonly SoundStyle Zombie43;

	public static readonly SoundStyle Zombie44;

	public static readonly SoundStyle Zombie45;

	public static readonly SoundStyle Zombie46;

	public static readonly SoundStyle Zombie47;

	public static readonly SoundStyle Zombie48;

	public static readonly SoundStyle Zombie49;

	public static readonly SoundStyle Zombie50;

	public static readonly SoundStyle Zombie51;

	public static readonly SoundStyle Zombie52;

	public static readonly SoundStyle Zombie53;

	public static readonly SoundStyle Zombie54;

	public static readonly SoundStyle Zombie55;

	public static readonly SoundStyle Zombie56;

	public static readonly SoundStyle Zombie57;

	public static readonly SoundStyle Zombie58;

	public static readonly SoundStyle Zombie59;

	public static readonly SoundStyle Zombie60;

	public static readonly SoundStyle Zombie61;

	public static readonly SoundStyle Zombie62;

	public static readonly SoundStyle Zombie63;

	public static readonly SoundStyle Zombie64;

	public static readonly SoundStyle Zombie65;

	public static readonly SoundStyle Zombie66;

	public static readonly SoundStyle Zombie67;

	public static readonly SoundStyle Zombie68;

	public static readonly SoundStyle Zombie69;

	public static readonly SoundStyle Zombie70;

	public static readonly SoundStyle Zombie71;

	public static readonly SoundStyle Zombie72;

	public static readonly SoundStyle Zombie73;

	public static readonly SoundStyle Zombie74;

	public static readonly SoundStyle Zombie75;

	public static readonly SoundStyle Zombie76;

	public static readonly SoundStyle Zombie77;

	public static readonly SoundStyle Zombie78;

	public static readonly SoundStyle Zombie79;

	public static readonly SoundStyle Zombie80;

	public static readonly SoundStyle Zombie81;

	public static readonly SoundStyle Zombie82;

	public static readonly SoundStyle Zombie83;

	public static readonly SoundStyle Zombie84;

	public static readonly SoundStyle Zombie85;

	public static readonly SoundStyle Zombie86;

	public static readonly SoundStyle Zombie87;

	public static readonly SoundStyle Zombie88;

	public static readonly SoundStyle Zombie89;

	public static readonly SoundStyle Zombie90;

	public static readonly SoundStyle Zombie91;

	public static readonly SoundStyle Zombie92;

	public static readonly SoundStyle Zombie93;

	public static readonly SoundStyle Zombie94;

	public static readonly SoundStyle Zombie95;

	public static readonly SoundStyle Zombie96;

	public static readonly SoundStyle Zombie97;

	public static readonly SoundStyle Zombie98;

	public static readonly SoundStyle Zombie99;

	public static readonly SoundStyle Zombie100;

	public static readonly SoundStyle Zombie101;

	public static readonly SoundStyle Zombie102;

	public static readonly SoundStyle Zombie103;

	public static readonly SoundStyle Zombie104;

	public static readonly SoundStyle Zombie105;

	public static readonly SoundStyle Zombie106;

	public static readonly SoundStyle Zombie107;

	public static readonly SoundStyle Zombie108;

	public static readonly SoundStyle Zombie109;

	public static readonly SoundStyle Zombie110;

	public static readonly SoundStyle Zombie111;

	public static readonly SoundStyle Zombie112;

	public static readonly SoundStyle Zombie113;

	public static readonly SoundStyle Zombie114;

	public static readonly SoundStyle Zombie115;

	public static readonly SoundStyle Zombie116;

	public static readonly SoundStyle Zombie117;

	public static readonly SoundStyle Zombie118;

	public static readonly SoundStyle Zombie119;

	public static readonly SoundStyle Zombie120;

	public static readonly SoundStyle Zombie121;

	public static readonly SoundStyle Zombie122;

	public static readonly SoundStyle Zombie123;

	public static readonly SoundStyle Zombie124;

	public static readonly SoundStyle Zombie125;

	public static readonly SoundStyle Zombie126;

	public static readonly SoundStyle Zombie127;

	public static readonly SoundStyle Zombie128;

	public static readonly SoundStyle Zombie129;

	public static readonly SoundStyle Zombie130;

	private static SoundStyle[][] legacyArrayedStylesMapping;

	internal static int TrackableLegacySoundCount => _trackableLegacySoundPathList.Count;

	internal static string GetTrackableLegacySoundPath(int id)
	{
		return _trackableLegacySoundPathList[id];
	}

	private static SoundStyle CreateTrackable(string name, SoundStyleDefaults defaults)
	{
		return CreateTrackable(name, 1, defaults.Type).WithPitchVariance(defaults.PitchVariance).WithVolume(defaults.Volume);
	}

	private static SoundStyle CreateTrackable(string name, int variations, SoundStyleDefaults defaults)
	{
		return CreateTrackable(name, variations, defaults.Type).WithPitchVariance(defaults.PitchVariance).WithVolume(defaults.Volume);
	}

	private static SoundStyle CreateTrackable(string name, SoundType type = SoundType.Sound)
	{
		return CreateTrackable(name, 1, type);
	}

	private static SoundStyle CreateTrackable(string name, int variations, SoundType type = SoundType.Sound)
	{
		if (_trackableLegacySoundPathList == null)
		{
			_trackableLegacySoundPathList = new List<string>();
		}
		_ = _trackableLegacySoundPathList.Count;
		if (variations == 1)
		{
			_trackableLegacySoundPathList.Add(name);
		}
		else
		{
			for (int i = 0; i < variations; i++)
			{
				_trackableLegacySoundPathList.Add(name + "_" + i);
			}
		}
		return new SoundStyle("Terraria/Sounds/Custom/" + name + ((variations > 1) ? "_" : null), 0, variations, type);
	}

	public static void FillAccessMap()
	{
		Dictionary<string, SoundStyle> ret = new Dictionary<string, SoundStyle>();
		Dictionary<string, ushort> ret2 = new Dictionary<string, ushort>();
		Dictionary<ushort, SoundStyle> ret3 = new Dictionary<ushort, SoundStyle>();
		ushort nextIndex = 0;
		List<FieldInfo> list = (from f in typeof(SoundID).GetFields(BindingFlags.Static | BindingFlags.Public)
			where f.FieldType == typeof(SoundStyle)
			select f).ToList();
		list.Sort((FieldInfo a, FieldInfo b) => string.Compare(a.Name, b.Name));
		list.ForEach(delegate(FieldInfo field)
		{
			ret[field.Name] = (SoundStyle)field.GetValue(null);
			ret2[field.Name] = nextIndex;
			ret3[nextIndex] = (SoundStyle)field.GetValue(null);
			nextIndex++;
		});
		SoundByName = ret;
		IndexByName = ret2;
		SoundByIndex = ret3;
	}

	static SoundID()
	{
		ItemSoundCount = 179;
		DD2_GoblinBomb = new SoundStyle("Terraria/Sounds/NPC_Killed_14")
		{
			Volume = 0.5f
		};
		AchievementComplete = CreateTrackable("achievement_complete");
		BlizzardInsideBuildingLoop = CreateTrackable("blizzard_inside_building_loop", SoundType.Ambient);
		BlizzardStrongLoop = CreateTrackable("blizzard_strong_loop", SoundType.Ambient).WithVolume(0.5f);
		LiquidsHoneyWater = CreateTrackable("liquids_honey_water", 3, SoundType.Ambient);
		LiquidsHoneyLava = CreateTrackable("liquids_honey_lava", 3, SoundType.Ambient);
		LiquidsWaterLava = CreateTrackable("liquids_water_lava", 3, SoundType.Ambient);
		DD2_BallistaTowerShot = CreateTrackable("dd2_ballista_tower_shot", 3);
		DD2_ExplosiveTrapExplode = CreateTrackable("dd2_explosive_trap_explode", 3);
		DD2_FlameburstTowerShot = CreateTrackable("dd2_flameburst_tower_shot", 3);
		DD2_LightningAuraZap = CreateTrackable("dd2_lightning_aura_zap", 4);
		DD2_DefenseTowerSpawn = CreateTrackable("dd2_defense_tower_spawn");
		DD2_BetsyDeath = CreateTrackable("dd2_betsy_death", 3);
		DD2_BetsyFireballShot = CreateTrackable("dd2_betsy_fireball_shot", 3);
		DD2_BetsyFireballImpact = CreateTrackable("dd2_betsy_fireball_impact", 3);
		DD2_BetsyFlameBreath = CreateTrackable("dd2_betsy_flame_breath");
		DD2_BetsyFlyingCircleAttack = CreateTrackable("dd2_betsy_flying_circle_attack");
		DD2_BetsyHurt = CreateTrackable("dd2_betsy_hurt", 3);
		DD2_BetsyScream = CreateTrackable("dd2_betsy_scream");
		DD2_BetsySummon = CreateTrackable("dd2_betsy_summon", 3);
		DD2_BetsyWindAttack = CreateTrackable("dd2_betsy_wind_attack", 3);
		DD2_DarkMageAttack = CreateTrackable("dd2_dark_mage_attack", 3);
		DD2_DarkMageCastHeal = CreateTrackable("dd2_dark_mage_cast_heal", 3);
		DD2_DarkMageDeath = CreateTrackable("dd2_dark_mage_death", 3);
		DD2_DarkMageHealImpact = CreateTrackable("dd2_dark_mage_heal_impact", 3);
		DD2_DarkMageHurt = CreateTrackable("dd2_dark_mage_hurt", 3);
		DD2_DarkMageSummonSkeleton = CreateTrackable("dd2_dark_mage_summon_skeleton", 3);
		DD2_DrakinBreathIn = CreateTrackable("dd2_drakin_breath_in", 3);
		DD2_DrakinDeath = CreateTrackable("dd2_drakin_death", 3);
		DD2_DrakinHurt = CreateTrackable("dd2_drakin_hurt", 3);
		DD2_DrakinShot = CreateTrackable("dd2_drakin_shot", 3);
		DD2_GoblinDeath = CreateTrackable("dd2_goblin_death", 3);
		DD2_GoblinHurt = CreateTrackable("dd2_goblin_hurt", 6);
		DD2_GoblinScream = CreateTrackable("dd2_goblin_scream", 3);
		DD2_GoblinBomberDeath = CreateTrackable("dd2_goblin_bomber_death", 3);
		DD2_GoblinBomberHurt = CreateTrackable("dd2_goblin_bomber_hurt", 3);
		DD2_GoblinBomberScream = CreateTrackable("dd2_goblin_bomber_scream", 3);
		DD2_GoblinBomberThrow = CreateTrackable("dd2_goblin_bomber_throw", 3);
		DD2_JavelinThrowersAttack = CreateTrackable("dd2_javelin_throwers_attack", 3);
		DD2_JavelinThrowersDeath = CreateTrackable("dd2_javelin_throwers_death", 3);
		DD2_JavelinThrowersHurt = CreateTrackable("dd2_javelin_throwers_hurt", 3);
		DD2_JavelinThrowersTaunt = CreateTrackable("dd2_javelin_throwers_taunt", 3);
		DD2_KoboldDeath = CreateTrackable("dd2_kobold_death", 3);
		DD2_KoboldExplosion = CreateTrackable("dd2_kobold_explosion", 3);
		DD2_KoboldHurt = CreateTrackable("dd2_kobold_hurt", 3);
		DD2_KoboldIgnite = CreateTrackable("dd2_kobold_ignite");
		DD2_KoboldIgniteLoop = CreateTrackable("dd2_kobold_ignite_loop");
		DD2_KoboldScreamChargeLoop = CreateTrackable("dd2_kobold_scream_charge_loop");
		DD2_KoboldFlyerChargeScream = CreateTrackable("dd2_kobold_flyer_charge_scream", 3);
		DD2_KoboldFlyerDeath = CreateTrackable("dd2_kobold_flyer_death", 3);
		DD2_KoboldFlyerHurt = CreateTrackable("dd2_kobold_flyer_hurt", 3);
		DD2_LightningBugDeath = CreateTrackable("dd2_lightning_bug_death", 3);
		DD2_LightningBugHurt = CreateTrackable("dd2_lightning_bug_hurt", 3);
		DD2_LightningBugZap = CreateTrackable("dd2_lightning_bug_zap", 3);
		DD2_OgreAttack = CreateTrackable("dd2_ogre_attack", 3);
		DD2_OgreDeath = CreateTrackable("dd2_ogre_death", 3);
		DD2_OgreGroundPound = CreateTrackable("dd2_ogre_ground_pound");
		DD2_OgreHurt = CreateTrackable("dd2_ogre_hurt", 3);
		DD2_OgreRoar = CreateTrackable("dd2_ogre_roar", 3);
		DD2_OgreSpit = CreateTrackable("dd2_ogre_spit");
		DD2_SkeletonDeath = CreateTrackable("dd2_skeleton_death", 3);
		DD2_SkeletonHurt = CreateTrackable("dd2_skeleton_hurt", 3);
		DD2_SkeletonSummoned = CreateTrackable("dd2_skeleton_summoned");
		DD2_WitherBeastAuraPulse = CreateTrackable("dd2_wither_beast_aura_pulse", 2);
		DD2_WitherBeastCrystalImpact = CreateTrackable("dd2_wither_beast_crystal_impact", 3);
		DD2_WitherBeastDeath = CreateTrackable("dd2_wither_beast_death", 3);
		DD2_WitherBeastHurt = CreateTrackable("dd2_wither_beast_hurt", 3);
		DD2_WyvernDeath = CreateTrackable("dd2_wyvern_death", 3);
		DD2_WyvernHurt = CreateTrackable("dd2_wyvern_hurt", 3);
		DD2_WyvernScream = CreateTrackable("dd2_wyvern_scream", 3);
		DD2_WyvernDiveDown = CreateTrackable("dd2_wyvern_dive_down", 3);
		DD2_EtherianPortalDryadTouch = CreateTrackable("dd2_etherian_portal_dryad_touch");
		DD2_EtherianPortalIdleLoop = CreateTrackable("dd2_etherian_portal_idle_loop");
		DD2_EtherianPortalOpen = CreateTrackable("dd2_etherian_portal_open");
		DD2_EtherianPortalSpawnEnemy = CreateTrackable("dd2_etherian_portal_spawn_enemy", 3);
		DD2_CrystalCartImpact = CreateTrackable("dd2_crystal_cart_impact", 3);
		DD2_DefeatScene = CreateTrackable("dd2_defeat_scene");
		DD2_WinScene = CreateTrackable("dd2_win_scene");
		DD2_BetsysWrathShot = DD2_BetsyFireballShot.WithVolume(0.4f);
		DD2_BetsysWrathImpact = DD2_BetsyFireballImpact.WithVolume(0.4f);
		DD2_BookStaffCast = CreateTrackable("dd2_book_staff_cast", 3);
		DD2_BookStaffTwisterLoop = CreateTrackable("dd2_book_staff_twister_loop");
		DD2_GhastlyGlaiveImpactGhost = CreateTrackable("dd2_ghastly_glaive_impact_ghost", 3);
		DD2_GhastlyGlaivePierce = CreateTrackable("dd2_ghastly_glaive_pierce", 3);
		DD2_MonkStaffGroundImpact = CreateTrackable("dd2_monk_staff_ground_impact", 3);
		DD2_MonkStaffGroundMiss = CreateTrackable("dd2_monk_staff_ground_miss", 3);
		DD2_MonkStaffSwing = CreateTrackable("dd2_monk_staff_swing", 4);
		DD2_PhantomPhoenixShot = CreateTrackable("dd2_phantom_phoenix_shot", 3);
		DD2_SonicBoomBladeSlash = CreateTrackable("dd2_sonic_boom_blade_slash", 3, SoundStyleDefaults.ItemDefaults).WithVolume(0.5f);
		DD2_SkyDragonsFuryCircle = CreateTrackable("dd2_sky_dragons_fury_circle", 3);
		DD2_SkyDragonsFuryShot = CreateTrackable("dd2_sky_dragons_fury_shot", 3);
		DD2_SkyDragonsFurySwing = CreateTrackable("dd2_sky_dragons_fury_swing", 4);
		LucyTheAxeTalk = CreateTrackable("lucyaxe_talk", 5).WithVolume(0.4f).WithPitchVariance(0.1f);
		DeerclopsHit = CreateTrackable("deerclops_hit", 3).WithVolume(0.3f);
		DeerclopsDeath = CreateTrackable("deerclops_death");
		DeerclopsScream = CreateTrackable("deerclops_scream", 3);
		DeerclopsIceAttack = CreateTrackable("deerclops_ice_attack", 3).WithVolume(0.1f);
		DeerclopsRubbleAttack = CreateTrackable("deerclops_rubble_attack").WithVolume(0.5f);
		DeerclopsStep = CreateTrackable("deerclops_step").WithVolume(0.2f);
		ChesterOpen = CreateTrackable("chester_open", 2);
		ChesterClose = CreateTrackable("chester_close", 2);
		AbigailSummon = CreateTrackable("abigail_summon");
		AbigailCry = CreateTrackable("abigail_cry", 3).WithVolume(0.4f);
		AbigailAttack = CreateTrackable("abigail_attack").WithVolume(0.35f);
		AbigailUpgrade = CreateTrackable("abigail_upgrade", 3).WithVolume(0.5f);
		GlommerBounce = CreateTrackable("glommer_bounce", 2).WithVolume(0.5f);
		DSTMaleHurt = CreateTrackable("dst_male_hit", 3).WithVolume(0.1f);
		DSTFemaleHurt = CreateTrackable("dst_female_hit", 3).WithVolume(0.1f);
		JimsDrone = CreateTrackable("Drone").WithVolume(0.1f);
		SoundByName = null;
		IndexByName = null;
		SoundByIndex = null;
		Dig = new SoundStyle("Terraria/Sounds/Dig_", 0, 3)
		{
			PitchVariance = 0.2f
		};
		PlayerHit = new SoundStyle("Terraria/Sounds/Player_Hit_", 0, 3)
		{
			PitchVariance = 0.2f
		};
		Item = new SoundStyle("Terraria/Sounds/Item_");
		PlayerKilled = new SoundStyle("Terraria/Sounds/Player_Killed");
		Grass = new SoundStyle("Terraria/Sounds/Grass")
		{
			PitchVariance = 0.6f
		};
		Grab = new SoundStyle("Terraria/Sounds/Grab")
		{
			PitchVariance = 0.2f
		};
		DoorOpen = new SoundStyle("Terraria/Sounds/Door_Opened")
		{
			PitchVariance = 0.4f
		};
		DoorClosed = new SoundStyle("Terraria/Sounds/Door_Closed")
		{
			PitchVariance = 0.4f
		};
		MenuOpen = new SoundStyle("Terraria/Sounds/Menu_Open");
		MenuClose = new SoundStyle("Terraria/Sounds/Menu_Close");
		MenuTick = new SoundStyle("Terraria/Sounds/Menu_Tick")
		{
			PlayOnlyIfFocused = true
		};
		Shatter = new SoundStyle("Terraria/Sounds/Shatter");
		ZombieMoan = new SoundStyle("Terraria/Sounds/Zombie_", 0, 3)
		{
			Volume = 0.4f
		};
		SandShark = new SoundStyle("Terraria/Sounds/Zombie_7")
		{
			Volume = 0.4f
		};
		BloodZombie = new SoundStyle("Terraria/Sounds/Zombie_", 21, 3)
		{
			Volume = 0.4f
		};
		Roar = new SoundStyle("Terraria/Sounds/Roar_0")
		{
			Identifier = "Terraria/Roar",
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
		WormDig = new SoundStyle("Terraria/Sounds/Roar_1")
		{
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
		WormDigQuiet = WormDig with
		{
			Volume = 0.25f
		};
		ScaryScream = new SoundStyle("Terraria/Sounds/Roar_2")
		{
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
		DoubleJump = new SoundStyle("Terraria/Sounds/Double_Jump")
		{
			PitchVariance = 0.2f
		};
		Run = new SoundStyle("Terraria/Sounds/Run")
		{
			PitchVariance = 0.2f
		};
		Coins = new SoundStyle("Terraria/Sounds/Coins");
		Splash = new SoundStyle("Terraria/Sounds/Splash_0")
		{
			PitchVariance = 0.2f,
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
		SplashWeak = new SoundStyle("Terraria/Sounds/Splash_1")
		{
			PitchVariance = 0.2f,
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
		Shimmer1 = new SoundStyle("Terraria/Sounds/Splash_2")
		{
			Volume = 0.75f,
			PitchVariance = 0.2f,
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
		Shimmer2 = new SoundStyle("Terraria/Sounds/Splash_3")
		{
			Volume = 0.75f,
			PitchVariance = 0.2f,
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
		ShimmerWeak1 = new SoundStyle("Terraria/Sounds/Splash_4")
		{
			Volume = 0.75f,
			Pitch = -0.1f,
			PitchVariance = 0.2f,
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
		ShimmerWeak2 = new SoundStyle("Terraria/Sounds/Splash_5")
		{
			Volume = 0.75f,
			Pitch = -0.1f,
			PitchVariance = 0.2f,
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
		FemaleHit = new SoundStyle("Terraria/Sounds/Female_Hit_", 0, 3);
		Tink = new SoundStyle("Terraria/Sounds/Tink_", 0, 3);
		Unlock = new SoundStyle("Terraria/Sounds/Unlock");
		Drown = new SoundStyle("Terraria/Sounds/Drown");
		Chat = new SoundStyle("Terraria/Sounds/Chat");
		MaxMana = new SoundStyle("Terraria/Sounds/MaxMana");
		Mummy = new SoundStyle("Terraria/Sounds/Zombie_", 3, 2)
		{
			Volume = 0.9f,
			PitchVariance = 0.2f
		};
		Pixie = new SoundStyle("Terraria/Sounds/Pixie")
		{
			PitchVariance = 0.2f,
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
		Mech = new SoundStyle("Terraria/Sounds/Mech_0")
		{
			PitchVariance = 0.2f,
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
		Span<(int, float)> span = stackalloc(int, float)[3]
		{
			(10, 300f),
			(11, 300f),
			(12, 1f)
		};
		Duck = new SoundStyle("Terraria/Sounds/Zombie_", span, SoundType.Ambient)
		{
			Volume = 0.75f,
			PitchRange = (minPitch: -0.7f, maxPitch: 0f)
		};
		Frog = new SoundStyle("Terraria/Sounds/Zombie_13", SoundType.Ambient)
		{
			Volume = 0.35f,
			PitchRange = (minPitch: -0.4f, maxPitch: 0.2f)
		};
		Bird = new SoundStyle("Terraria/Sounds/Zombie_", 14, 5, SoundType.Ambient)
		{
			Volume = 0.15f,
			PitchRange = (minPitch: -0.7f, maxPitch: 0.26f),
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
		Critter = new SoundStyle("Terraria/Sounds/Zombie_15", SoundType.Ambient)
		{
			Volume = 0.2f,
			PitchRange = (minPitch: -0.1f, maxPitch: 0.3f),
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
		Waterfall = new SoundStyle("Terraria/Sounds/Liquid_0", SoundType.Ambient)
		{
			Volume = 0.2f,
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
		Lavafall = new SoundStyle("Terraria/Sounds/Liquid_1", SoundType.Ambient)
		{
			Volume = 0.65f,
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
		ForceRoar = new SoundStyle("Terraria/Sounds/Roar_0")
		{
			Identifier = "Terraria/Roar"
		};
		ForceRoarPitched = new SoundStyle("Terraria/Sounds/Roar_0")
		{
			Pitch = 0.6f,
			Identifier = "Terraria/Roar"
		};
		Meowmere = new SoundStyle("Terraria/Sounds/Item_", 57, 2)
		{
			PitchVariance = 0.8f
		};
		CoinPickup = new SoundStyle("Terraria/Sounds/Coin_", 0, 5)
		{
			PitchVariance = 0.16f
		};
		Drip = new SoundStyle("Terraria/Sounds/Drip_", 0, 3, SoundType.Ambient)
		{
			Volume = 0.5f,
			PitchVariance = 0.6f
		};
		Camera = new SoundStyle("Terraria/Sounds/Camera");
		MoonLord = new SoundStyle("Terraria/Sounds/NPC_Killed_10")
		{
			PitchVariance = 0.2f
		};
		Thunder = new SoundStyle("Terraria/Sounds/Thunder_", 0, 7, SoundType.Ambient)
		{
			MaxInstances = 7,
			PitchVariance = 0.2f
		};
		Seagull = new SoundStyle("Terraria/Sounds/Zombie_", 106, 3)
		{
			Volume = 0.2f,
			PitchRange = (minPitch: -0.7f, maxPitch: 0f)
		};
		Dolphin = new SoundStyle("Terraria/Sounds/Zombie_109")
		{
			Volume = 0.3f,
			PitchVariance = 0.2f,
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
		span = stackalloc(int, float)[5]
		{
			(110, 300f),
			(111, 300f),
			(112, 1f),
			(113, 1f),
			(114, 1f)
		};
		Owl = new SoundStyle("Terraria/Sounds/Zombie_", span)
		{
			PitchVariance = 0.2f
		};
		GuitarC = new SoundStyle("Terraria/Sounds/Item_133")
		{
			Volume = 0.45f,
			Identifier = "Terraria/Guitar"
		};
		GuitarD = new SoundStyle("Terraria/Sounds/Item_134")
		{
			Volume = 0.45f,
			Identifier = "Terraria/Guitar"
		};
		GuitarEm = new SoundStyle("Terraria/Sounds/Item_135")
		{
			Volume = 0.45f,
			Identifier = "Terraria/Guitar"
		};
		GuitarG = new SoundStyle("Terraria/Sounds/Item_136")
		{
			Volume = 0.45f,
			Identifier = "Terraria/Guitar"
		};
		GuitarBm = new SoundStyle("Terraria/Sounds/Item_137")
		{
			Volume = 0.45f,
			Identifier = "Terraria/Guitar"
		};
		GuitarAm = new SoundStyle("Terraria/Sounds/Item_138")
		{
			Volume = 0.45f,
			Identifier = "Terraria/Guitar"
		};
		DrumHiHat = new SoundStyle("Terraria/Sounds/Item_139")
		{
			Volume = 0.7f,
			Identifier = "Terraria/Drums"
		};
		DrumTomHigh = new SoundStyle("Terraria/Sounds/Item_140")
		{
			Volume = 0.7f,
			Identifier = "Terraria/Drums"
		};
		DrumTomLow = new SoundStyle("Terraria/Sounds/Item_141")
		{
			Volume = 0.7f,
			Identifier = "Terraria/Drums"
		};
		DrumTomMid = new SoundStyle("Terraria/Sounds/Item_142")
		{
			Volume = 0.7f,
			Identifier = "Terraria/Drums"
		};
		DrumClosedHiHat = new SoundStyle("Terraria/Sounds/Item_143")
		{
			Volume = 0.7f,
			Identifier = "Terraria/Drums"
		};
		DrumCymbal1 = new SoundStyle("Terraria/Sounds/Item_144")
		{
			Volume = 0.7f,
			Identifier = "Terraria/Drums"
		};
		DrumCymbal2 = new SoundStyle("Terraria/Sounds/Item_145")
		{
			Volume = 0.7f,
			Identifier = "Terraria/Drums"
		};
		DrumKick = new SoundStyle("Terraria/Sounds/Item_146")
		{
			Volume = 0.7f,
			Identifier = "Terraria/Drums"
		};
		DrumTamaSnare = new SoundStyle("Terraria/Sounds/Item_147")
		{
			Volume = 0.7f,
			Identifier = "Terraria/Drums"
		};
		DrumFloorTom = new SoundStyle("Terraria/Sounds/Item_148")
		{
			Volume = 0.7f,
			Identifier = "Terraria/Drums"
		};
		Research = new SoundStyle("Terraria/Sounds/Research_", 1, 3);
		ResearchComplete = new SoundStyle("Terraria/Sounds/Research_0");
		QueenSlime = new SoundStyle("Terraria/Sounds/Zombie_", 115, 3)
		{
			Volume = 0.5f,
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
		Clown = new SoundStyle("Terraria/Sounds/Zombie_", 121, 3)
		{
			Volume = 0.45f,
			PitchVariance = 0.15f,
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
		Cockatiel = new SoundStyle("Terraria/Sounds/Zombie_", 118, 3)
		{
			Volume = 0.3f,
			PitchVariance = 0.05f
		};
		Macaw = new SoundStyle("Terraria/Sounds/Zombie_", 126, 3)
		{
			Volume = 0.22f,
			PitchVariance = 0.05f
		};
		Toucan = new SoundStyle("Terraria/Sounds/Zombie_", 129, 3)
		{
			Volume = 0.2f,
			PitchVariance = 0.05f
		};
		NPCHit1 = NPCHitSound(1);
		NPCHit2 = NPCHitSound(2);
		NPCHit3 = NPCHitSound(3);
		NPCHit4 = NPCHitSound(4);
		NPCHit5 = NPCHitSound(5);
		NPCHit6 = NPCHitSound(6);
		NPCHit7 = NPCHitSound(7);
		NPCHit8 = NPCHitSound(8);
		NPCHit9 = NPCHitSound(9);
		NPCHit10 = NPCHitSound(10);
		NPCHit11 = NPCHitSound(11);
		NPCHit12 = NPCHitSound(12);
		NPCHit13 = NPCHitSound(13);
		NPCHit14 = NPCHitSound(14);
		NPCHit15 = NPCHitSound(15);
		NPCHit16 = NPCHitSound(16);
		NPCHit17 = NPCHitSound(17);
		NPCHit18 = NPCHitSound(18);
		NPCHit19 = NPCHitSound(19);
		NPCHit20 = NPCHitSound(20)with
		{
			Volume = 0.5f
		};
		NPCHit21 = NPCHitSound(21)with
		{
			Volume = 0.5f
		};
		NPCHit22 = NPCHitSound(22)with
		{
			Volume = 0.5f
		};
		NPCHit23 = NPCHitSound(23)with
		{
			Volume = 0.5f
		};
		NPCHit24 = NPCHitSound(24)with
		{
			Volume = 0.5f
		};
		NPCHit25 = NPCHitSound(25)with
		{
			Volume = 0.5f
		};
		NPCHit26 = NPCHitSound(26)with
		{
			Volume = 0.5f
		};
		NPCHit27 = NPCHitSound(27)with
		{
			Volume = 0.5f
		};
		NPCHit28 = NPCHitSound(28)with
		{
			Volume = 0.5f
		};
		NPCHit29 = NPCHitSound(29)with
		{
			Volume = 0.5f
		};
		NPCHit30 = NPCHitSound(30)with
		{
			Volume = 0.5f
		};
		NPCHit31 = NPCHitSound(31)with
		{
			Volume = 0.5f
		};
		NPCHit32 = NPCHitSound(32)with
		{
			Volume = 0.5f
		};
		NPCHit33 = NPCHitSound(33)with
		{
			Volume = 0.5f
		};
		NPCHit34 = NPCHitSound(34)with
		{
			Volume = 0.5f
		};
		NPCHit35 = NPCHitSound(35)with
		{
			Volume = 0.5f
		};
		NPCHit36 = NPCHitSound(36)with
		{
			Volume = 0.5f
		};
		NPCHit37 = NPCHitSound(37)with
		{
			Volume = 0.5f
		};
		NPCHit38 = NPCHitSound(38)with
		{
			Volume = 0.5f
		};
		NPCHit39 = NPCHitSound(39)with
		{
			Volume = 0.5f
		};
		NPCHit40 = NPCHitSound(40)with
		{
			Volume = 0.5f
		};
		NPCHit41 = NPCHitSound(41)with
		{
			Volume = 0.5f
		};
		NPCHit42 = NPCHitSound(42)with
		{
			Volume = 0.5f
		};
		NPCHit43 = NPCHitSound(43)with
		{
			Volume = 0.5f
		};
		NPCHit44 = NPCHitSound(44)with
		{
			Volume = 0.5f
		};
		NPCHit45 = NPCHitSound(45)with
		{
			Volume = 0.5f
		};
		NPCHit46 = NPCHitSound(46)with
		{
			Volume = 0.5f
		};
		NPCHit47 = NPCHitSound(47)with
		{
			Volume = 0.5f
		};
		NPCHit48 = NPCHitSound(48)with
		{
			Volume = 0.5f
		};
		NPCHit49 = NPCHitSound(49)with
		{
			Volume = 0.5f
		};
		NPCHit50 = NPCHitSound(50)with
		{
			Volume = 0.5f
		};
		NPCHit51 = NPCHitSound(51)with
		{
			Volume = 0.5f
		};
		NPCHit52 = NPCHitSound(52)with
		{
			Volume = 0.5f
		};
		NPCHit53 = NPCHitSound(53)with
		{
			Volume = 0.5f
		};
		NPCHit54 = NPCHitSound(54)with
		{
			Volume = 0.5f
		};
		NPCHit55 = NPCHitSound(55)with
		{
			Volume = 0.5f
		};
		NPCHit56 = NPCHitSound(56)with
		{
			Volume = 0.5f
		};
		NPCHit57 = NPCHitSound(57)with
		{
			Volume = 0.6f,
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
		NPCHitCount = 58;
		NPCDeath1 = NPCDeathSound(1);
		NPCDeath2 = NPCDeathSound(2);
		NPCDeath3 = NPCDeathSound(3);
		NPCDeath4 = NPCDeathSound(4);
		NPCDeath5 = NPCDeathSound(5);
		NPCDeath6 = NPCDeathSound(6);
		NPCDeath7 = NPCDeathSound(7);
		NPCDeath8 = NPCDeathSound(8);
		NPCDeath9 = NPCDeathSound(9);
		NPCDeath10 = NPCDeathSound(10)with
		{
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
		NPCDeath11 = NPCDeathSound(11);
		NPCDeath12 = NPCDeathSound(12);
		NPCDeath13 = NPCDeathSound(13);
		NPCDeath14 = NPCDeathSound(14);
		NPCDeath15 = NPCDeathSound(15);
		NPCDeath16 = NPCDeathSound(16);
		NPCDeath17 = NPCDeathSound(17);
		NPCDeath18 = NPCDeathSound(18);
		NPCDeath19 = NPCDeathSound(19);
		NPCDeath20 = NPCDeathSound(20);
		NPCDeath21 = NPCDeathSound(21);
		NPCDeath22 = NPCDeathSound(22);
		NPCDeath23 = NPCDeathSound(23)with
		{
			Volume = 0.5f
		};
		NPCDeath24 = NPCDeathSound(24)with
		{
			Volume = 0.5f
		};
		NPCDeath25 = NPCDeathSound(25)with
		{
			Volume = 0.5f
		};
		NPCDeath26 = NPCDeathSound(26)with
		{
			Volume = 0.5f
		};
		NPCDeath27 = NPCDeathSound(27)with
		{
			Volume = 0.5f
		};
		NPCDeath28 = NPCDeathSound(28)with
		{
			Volume = 0.5f
		};
		NPCDeath29 = NPCDeathSound(29)with
		{
			Volume = 0.5f
		};
		NPCDeath30 = NPCDeathSound(30)with
		{
			Volume = 0.5f
		};
		NPCDeath31 = NPCDeathSound(31)with
		{
			Volume = 0.5f
		};
		NPCDeath32 = NPCDeathSound(32)with
		{
			Volume = 0.5f
		};
		NPCDeath33 = NPCDeathSound(33)with
		{
			Volume = 0.5f
		};
		NPCDeath34 = NPCDeathSound(34)with
		{
			Volume = 0.5f
		};
		NPCDeath35 = NPCDeathSound(35)with
		{
			Volume = 0.5f
		};
		NPCDeath36 = NPCDeathSound(36)with
		{
			Volume = 0.5f
		};
		NPCDeath37 = NPCDeathSound(37)with
		{
			Volume = 0.5f
		};
		NPCDeath38 = NPCDeathSound(38)with
		{
			Volume = 0.5f
		};
		NPCDeath39 = NPCDeathSound(39)with
		{
			Volume = 0.5f
		};
		NPCDeath40 = NPCDeathSound(40)with
		{
			Volume = 0.5f
		};
		NPCDeath41 = NPCDeathSound(41)with
		{
			Volume = 0.5f
		};
		NPCDeath42 = NPCDeathSound(42)with
		{
			Volume = 0.5f
		};
		NPCDeath43 = NPCDeathSound(43)with
		{
			Volume = 0.5f
		};
		NPCDeath44 = NPCDeathSound(44)with
		{
			Volume = 0.5f
		};
		NPCDeath45 = NPCDeathSound(45)with
		{
			Volume = 0.5f
		};
		NPCDeath46 = NPCDeathSound(46)with
		{
			Volume = 0.5f
		};
		NPCDeath47 = NPCDeathSound(47)with
		{
			Volume = 0.5f
		};
		NPCDeath48 = NPCDeathSound(48)with
		{
			Volume = 0.5f
		};
		NPCDeath49 = NPCDeathSound(49)with
		{
			Volume = 0.5f
		};
		NPCDeath50 = NPCDeathSound(50)with
		{
			Volume = 0.5f
		};
		NPCDeath51 = NPCDeathSound(51)with
		{
			Volume = 0.5f
		};
		NPCDeath52 = NPCDeathSound(52)with
		{
			Volume = 0.5f
		};
		NPCDeath53 = NPCDeathSound(53)with
		{
			Volume = 0.5f
		};
		NPCDeath54 = NPCDeathSound(54)with
		{
			Volume = 0.5f
		};
		NPCDeath55 = NPCDeathSound(55)with
		{
			Volume = 0.5f
		};
		NPCDeath56 = NPCDeathSound(56)with
		{
			Volume = 0.5f
		};
		NPCDeath57 = NPCDeathSound(57)with
		{
			Volume = 0.5f
		};
		NPCDeath58 = NPCDeathSound(58);
		NPCDeath59 = NPCDeathSound(59);
		NPCDeath60 = NPCDeathSound(60);
		NPCDeath61 = NPCDeathSound(61)with
		{
			Volume = 0.6f
		};
		NPCDeath62 = NPCDeathSound(62)with
		{
			Volume = 0.6f
		};
		NPCDeath63 = NPCDeathSound(63);
		NPCDeath64 = NPCDeathSound(64);
		NPCDeath65 = NPCDeathSound(65);
		NPCDeath66 = NPCDeathSound(66);
		NPCDeathCount = 66;
		Item1 = ItemSound(stackalloc int[3] { 1, 18, 19 });
		Item2 = ItemSound(2);
		Item3 = ItemSound(3);
		Item4 = ItemSound(4);
		Item5 = ItemSound(5);
		Item6 = ItemSound(6);
		Item7 = ItemSound(7);
		Item8 = ItemSound(8);
		Item9 = ItemSound(9);
		Item10 = ItemSound(10);
		Item11 = ItemSound(11);
		Item12 = ItemSound(12);
		Item13 = ItemSound(13);
		Item14 = ItemSound(14);
		Item15 = ItemSound(15);
		Item16 = ItemSound(16);
		Item17 = ItemSound(17);
		Item18 = ItemSound(18);
		Item19 = ItemSound(19);
		Item20 = ItemSound(20);
		Item21 = ItemSound(21);
		Item22 = ItemSound(22);
		Item23 = ItemSound(23);
		Item24 = ItemSound(24);
		Item25 = ItemSound(25);
		Item26 = ItemSound(26)with
		{
			Volume = 0.75f,
			PitchVariance = 0f,
			UsesMusicPitch = true
		};
		Item27 = ItemSound(27);
		Item28 = ItemSound(28);
		Item29 = ItemSound(29);
		Item30 = ItemSound(30);
		Item31 = ItemSound(31);
		Item32 = ItemSound(32);
		Item33 = ItemSound(33);
		Item34 = ItemSound(34);
		Item35 = ItemSound(35)with
		{
			Volume = 0.75f,
			PitchVariance = 0f,
			UsesMusicPitch = true
		};
		Item36 = ItemSound(36);
		Item37 = ItemSound(37)with
		{
			Volume = 0.5f
		};
		Item38 = ItemSound(38);
		Item39 = ItemSound(39);
		Item40 = ItemSound(40);
		Item41 = ItemSound(41);
		Item42 = ItemSound(42);
		Item43 = ItemSound(43);
		Item44 = ItemSound(44);
		Item45 = ItemSound(45);
		Item46 = ItemSound(46);
		Item47 = ItemSound(47)with
		{
			Volume = 0.75f,
			PitchVariance = 0f,
			UsesMusicPitch = true
		};
		Item48 = ItemSound(48);
		Item49 = ItemSound(49);
		Item50 = ItemSound(50);
		Item51 = ItemSound(51);
		Item52 = ItemSound(52)with
		{
			Volume = 0.35f
		};
		Item53 = ItemSound(53)with
		{
			Volume = 0.75f,
			PitchRange = (minPitch: -0.4f, maxPitch: -0.2f),
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
		Item54 = ItemSound(54);
		Item55 = ItemSound(55)with
		{
			Volume = 0.5625f,
			PitchRange = (minPitch: -0.4f, maxPitch: -0.2f),
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
		Item56 = ItemSound(56);
		Item57 = ItemSound(57);
		Item58 = ItemSound(58);
		Item59 = ItemSound(59);
		Item60 = ItemSound(60);
		Item61 = ItemSound(61);
		Item62 = ItemSound(62);
		Item63 = ItemSound(63);
		Item64 = ItemSound(64);
		Item65 = ItemSound(65);
		Item66 = ItemSound(66);
		Item67 = ItemSound(67);
		Item68 = ItemSound(68);
		Item69 = ItemSound(69);
		Item70 = ItemSound(70);
		Item71 = ItemSound(71);
		Item72 = ItemSound(72);
		Item73 = ItemSound(73);
		Item74 = ItemSound(74);
		Item75 = ItemSound(75);
		Item76 = ItemSound(76);
		Item77 = ItemSound(77);
		Item78 = ItemSound(78);
		Item79 = ItemSound(79);
		Item80 = ItemSound(80);
		Item81 = ItemSound(81);
		Item82 = ItemSound(82);
		Item83 = ItemSound(83);
		Item84 = ItemSound(84);
		Item85 = ItemSound(85);
		Item86 = ItemSound(86);
		Item87 = ItemSound(87);
		Item88 = ItemSound(88);
		Item89 = ItemSound(89);
		Item90 = ItemSound(90);
		Item91 = ItemSound(91);
		Item92 = ItemSound(92);
		Item93 = ItemSound(93);
		Item94 = ItemSound(94);
		Item95 = ItemSound(95);
		Item96 = ItemSound(96);
		Item97 = ItemSound(97);
		Item98 = ItemSound(98);
		Item99 = ItemSound(99);
		Item100 = ItemSound(100);
		Item101 = ItemSound(101);
		Item102 = ItemSound(102);
		Item103 = ItemSound(103);
		Item104 = ItemSound(104);
		Item105 = ItemSound(105);
		Item106 = ItemSound(106);
		Item107 = ItemSound(107);
		Item108 = ItemSound(108);
		Item109 = ItemSound(109);
		Item110 = ItemSound(110);
		Item111 = ItemSound(111);
		Item112 = ItemSound(112);
		Item113 = ItemSound(113);
		Item114 = ItemSound(114);
		Item115 = ItemSound(115);
		Item116 = ItemSound(116)with
		{
			Volume = 0.5f
		};
		Item117 = ItemSound(117);
		Item118 = ItemSound(118);
		Item119 = ItemSound(119);
		Item120 = ItemSound(120);
		Item121 = ItemSound(121);
		Item122 = ItemSound(122);
		Item123 = ItemSound(123)with
		{
			Volume = 0.5f
		};
		Item124 = ItemSound(124)with
		{
			Volume = 0.65f
		};
		Item125 = ItemSound(125)with
		{
			Volume = 0.65f
		};
		Item126 = ItemSound(126);
		Item127 = ItemSound(127);
		Item128 = ItemSound(128);
		Item129 = ItemSound(129)with
		{
			Volume = 0.6f
		};
		Item130 = ItemSound(130);
		Item131 = ItemSound(131);
		Item132 = ItemSound(132)with
		{
			PitchVariance = 0.04f
		};
		Item133 = ItemSound(133);
		Item134 = ItemSound(134);
		Item135 = ItemSound(135);
		Item136 = ItemSound(136);
		Item137 = ItemSound(137);
		Item138 = ItemSound(138);
		Item139 = ItemSound(139);
		Item140 = ItemSound(140);
		Item141 = ItemSound(141);
		Item142 = ItemSound(142);
		Item143 = ItemSound(143);
		Item144 = ItemSound(144);
		Item145 = ItemSound(145);
		Item146 = ItemSound(146);
		Item147 = ItemSound(147);
		Item148 = ItemSound(148);
		Item149 = ItemSound(149);
		Item150 = ItemSound(150);
		Item151 = ItemSound(151);
		Item152 = ItemSound(152);
		Item153 = ItemSound(153)with
		{
			PitchVariance = 0.3f
		};
		Item154 = ItemSound(154);
		Item155 = ItemSound(155);
		Item156 = ItemSound(156)with
		{
			Volume = 0.6f,
			PitchVariance = 0.2f
		};
		Item157 = ItemSound(157)with
		{
			Volume = 0.7f
		};
		Item158 = ItemSound(158)with
		{
			Volume = 0.8f
		};
		Item159 = ItemSound(159)with
		{
			Volume = 0.75f,
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
		Item160 = ItemSound(160);
		Item161 = ItemSound(161);
		Item162 = ItemSound(162);
		Item163 = ItemSound(163);
		Item164 = ItemSound(164);
		Item165 = ItemSound(165);
		Item166 = ItemSound(166);
		Item167 = ItemSound(167);
		Item168 = ItemSound(168);
		Item169 = ItemSound(169)with
		{
			Pitch = -0.8f
		};
		Item170 = ItemSound(170);
		Item171 = ItemSound(171);
		Item172 = ItemSound(172);
		Item173 = ItemSound(173);
		Item174 = ItemSound(174);
		Item175 = ItemSound(175);
		Item176 = ItemSound(176);
		Item177 = ItemSound(177);
		Item178 = ItemSound(178);
		Zombie1 = ZombieSound(1);
		Zombie2 = ZombieSound(2);
		Zombie3 = ZombieSound(3);
		Zombie4 = ZombieSound(4);
		Zombie5 = ZombieSound(5);
		Zombie6 = ZombieSound(6);
		Zombie7 = ZombieSound(7);
		Zombie8 = ZombieSound(8);
		Zombie9 = ZombieSound(9);
		Zombie10 = ZombieSound(10);
		Zombie11 = ZombieSound(11);
		Zombie12 = ZombieSound(12);
		Zombie13 = ZombieSound(13);
		Zombie14 = ZombieSound(14);
		Zombie15 = ZombieSound(15);
		Zombie16 = ZombieSound(16);
		Zombie17 = ZombieSound(17);
		Zombie18 = ZombieSound(18);
		Zombie19 = ZombieSound(19);
		Zombie20 = ZombieSound(20);
		Zombie21 = ZombieSound(21);
		Zombie22 = ZombieSound(22);
		Zombie23 = ZombieSound(23);
		Zombie24 = ZombieSound(24)with
		{
			Volume = 0.5f
		};
		Zombie25 = ZombieSound(25)with
		{
			Volume = 0.5f
		};
		Zombie26 = ZombieSound(26)with
		{
			Volume = 0.5f
		};
		Zombie27 = ZombieSound(27)with
		{
			Volume = 0.5f
		};
		Zombie28 = ZombieSound(28)with
		{
			Volume = 0.5f
		};
		Zombie29 = ZombieSound(29)with
		{
			Volume = 0.5f
		};
		Zombie30 = ZombieSound(30)with
		{
			Volume = 0.5f
		};
		Zombie31 = ZombieSound(31)with
		{
			Volume = 0.5f
		};
		Zombie32 = ZombieSound(32)with
		{
			Volume = 0.5f
		};
		Zombie33 = ZombieSound(33)with
		{
			Volume = 0.5f
		};
		Zombie34 = ZombieSound(34)with
		{
			Volume = 0.5f
		};
		Zombie35 = ZombieSound(35)with
		{
			Volume = 0.5f
		};
		Zombie36 = ZombieSound(36)with
		{
			Volume = 0.5f
		};
		Zombie37 = ZombieSound(37)with
		{
			Volume = 0.5f
		};
		Zombie38 = ZombieSound(38)with
		{
			Volume = 0.5f
		};
		Zombie39 = ZombieSound(39)with
		{
			Volume = 0.5f
		};
		Zombie40 = ZombieSound(40)with
		{
			Volume = 0.5f
		};
		Zombie41 = ZombieSound(41)with
		{
			Volume = 0.5f
		};
		Zombie42 = ZombieSound(42)with
		{
			Volume = 0.5f
		};
		Zombie43 = ZombieSound(43)with
		{
			Volume = 0.5f
		};
		Zombie44 = ZombieSound(44)with
		{
			Volume = 0.5f
		};
		Zombie45 = ZombieSound(45)with
		{
			Volume = 0.5f
		};
		Zombie46 = ZombieSound(46)with
		{
			Volume = 0.5f
		};
		Zombie47 = ZombieSound(47)with
		{
			Volume = 0.5f
		};
		Zombie48 = ZombieSound(48)with
		{
			Volume = 0.5f
		};
		Zombie49 = ZombieSound(49)with
		{
			Volume = 0.5f
		};
		Zombie50 = ZombieSound(50)with
		{
			Volume = 0.5f
		};
		Zombie51 = ZombieSound(51)with
		{
			Volume = 0.5f
		};
		Zombie52 = ZombieSound(52)with
		{
			Volume = 0.5f
		};
		Zombie53 = ZombieSound(53)with
		{
			Volume = 0.5f
		};
		Zombie54 = ZombieSound(54)with
		{
			Volume = 0.5f
		};
		Zombie55 = ZombieSound(55)with
		{
			Volume = 0.5f
		};
		Zombie56 = ZombieSound(56)with
		{
			Volume = 0.5f
		};
		Zombie57 = ZombieSound(57)with
		{
			Volume = 0.5f
		};
		Zombie58 = ZombieSound(58)with
		{
			Volume = 0.5f
		};
		Zombie59 = ZombieSound(59)with
		{
			Volume = 0.5f
		};
		Zombie60 = ZombieSound(60)with
		{
			Volume = 0.5f
		};
		Zombie61 = ZombieSound(61)with
		{
			Volume = 0.5f
		};
		Zombie62 = ZombieSound(62)with
		{
			Volume = 0.5f
		};
		Zombie63 = ZombieSound(63)with
		{
			Volume = 0.5f
		};
		Zombie64 = ZombieSound(64)with
		{
			Volume = 0.5f
		};
		Zombie65 = ZombieSound(65)with
		{
			Volume = 0.5f
		};
		Zombie66 = ZombieSound(66)with
		{
			Volume = 0.5f
		};
		Zombie67 = ZombieSound(67)with
		{
			Volume = 0.5f
		};
		Zombie68 = ZombieSound(68)with
		{
			Volume = 0.5f
		};
		Zombie69 = ZombieSound(69)with
		{
			Volume = 0.5f
		};
		Zombie70 = ZombieSound(70)with
		{
			Volume = 0.5f
		};
		Zombie71 = ZombieSound(71)with
		{
			Volume = 0.5f
		};
		Zombie72 = ZombieSound(72)with
		{
			Volume = 0.5f
		};
		Zombie73 = ZombieSound(73)with
		{
			Volume = 0.5f
		};
		Zombie74 = ZombieSound(74)with
		{
			Volume = 0.5f
		};
		Zombie75 = ZombieSound(75)with
		{
			Volume = 0.5f
		};
		Zombie76 = ZombieSound(76)with
		{
			Volume = 0.5f
		};
		Zombie77 = ZombieSound(77)with
		{
			Volume = 0.5f
		};
		Zombie78 = ZombieSound(78)with
		{
			Volume = 0.5f
		};
		Zombie79 = ZombieSound(79)with
		{
			Volume = 0.5f
		};
		Zombie80 = ZombieSound(80)with
		{
			Volume = 0.5f
		};
		Zombie81 = ZombieSound(81)with
		{
			Volume = 0.5f
		};
		Zombie82 = ZombieSound(82)with
		{
			Volume = 0.5f
		};
		Zombie83 = ZombieSound(83)with
		{
			Volume = 0.5f
		};
		Zombie84 = ZombieSound(84)with
		{
			Volume = 0.5f
		};
		Zombie85 = ZombieSound(85)with
		{
			Volume = 0.5f
		};
		Zombie86 = ZombieSound(86)with
		{
			Volume = 0.5f
		};
		Zombie87 = ZombieSound(87)with
		{
			Volume = 0.5f
		};
		Zombie88 = ZombieSound(88)with
		{
			Volume = 0.7f
		};
		Zombie89 = ZombieSound(89)with
		{
			Volume = 0.7f
		};
		Zombie90 = ZombieSound(90)with
		{
			Volume = 0.7f
		};
		Zombie91 = ZombieSound(91)with
		{
			Volume = 0.7f
		};
		Zombie92 = ZombieSound(92)with
		{
			Volume = 0.5f
		};
		Zombie93 = ZombieSound(93)with
		{
			Volume = 0.4f
		};
		Zombie94 = ZombieSound(94)with
		{
			Volume = 0.4f
		};
		Zombie95 = ZombieSound(95)with
		{
			Volume = 0.4f
		};
		Zombie96 = ZombieSound(96)with
		{
			Volume = 0.4f
		};
		Zombie97 = ZombieSound(97)with
		{
			Volume = 0.4f
		};
		Zombie98 = ZombieSound(98)with
		{
			Volume = 0.4f
		};
		Zombie99 = ZombieSound(99)with
		{
			Volume = 0.4f
		};
		Zombie100 = ZombieSound(100)with
		{
			Volume = 0.25f
		};
		Zombie101 = ZombieSound(101)with
		{
			Volume = 0.25f
		};
		Zombie102 = ZombieSound(102)with
		{
			Volume = 0.4f
		};
		Zombie103 = ZombieSound(103)with
		{
			Volume = 0.4f
		};
		Zombie104 = ZombieSound(104)with
		{
			Volume = 0.55f
		};
		Zombie105 = ZombieSound(105);
		Zombie106 = ZombieSound(106);
		Zombie107 = ZombieSound(107);
		Zombie108 = ZombieSound(108);
		Zombie109 = ZombieSound(109);
		Zombie110 = ZombieSound(110);
		Zombie111 = ZombieSound(111);
		Zombie112 = ZombieSound(112);
		Zombie113 = ZombieSound(113);
		Zombie114 = ZombieSound(114);
		Zombie115 = ZombieSound(115);
		Zombie116 = ZombieSound(116);
		Zombie117 = ZombieSound(117);
		Zombie118 = ZombieSound(118);
		Zombie119 = ZombieSound(119);
		Zombie120 = ZombieSound(120);
		Zombie121 = ZombieSound(121);
		Zombie122 = ZombieSound(122);
		Zombie123 = ZombieSound(123);
		Zombie124 = ZombieSound(124);
		Zombie125 = ZombieSound(125);
		Zombie126 = ZombieSound(126);
		Zombie127 = ZombieSound(127);
		Zombie128 = ZombieSound(128);
		Zombie129 = ZombieSound(129);
		Zombie130 = ZombieSound(130);
		legacyArrayedStylesMapping = new SoundStyle[LegacySoundIDs.Count][];
		FillLegacyArrayedStylesMap();
	}

	internal static bool TryGetLegacyStyle(int type, int style, out SoundStyle result)
	{
		SoundStyle? tempResult = GetLegacyStyle(type, style);
		if (tempResult.HasValue)
		{
			result = tempResult.Value;
			return true;
		}
		result = default(SoundStyle);
		return false;
	}

	private static void FillLegacyArrayedStylesMap()
	{
		AddNumberedStyles(2, "Item", 0, 172);
		AddNumberedStyles(3, "NPCHit", 0, 65);
		AddNumberedStyles(4, "NPCDeath", 0, 57);
		AddNumberedStyles(29, "Zombie", 0, 118);
		static void AddNumberedStyles(int type, string baseName, int start, int numStyles)
		{
			SoundStyle[] array = (legacyArrayedStylesMapping[type] = new SoundStyle[start + numStyles]);
			for (int i = 0; i < numStyles; i++)
			{
				int ii = start + i;
				if (typeof(SoundID).GetField($"{baseName}{ii}", BindingFlags.Static | BindingFlags.Public)?.GetValue(null) is SoundStyle soundStyle)
				{
					array[ii] = soundStyle;
				}
			}
		}
	}

	private static SoundStyle SoundWithDefaults(SoundStyleDefaults defaults, SoundStyle style)
	{
		defaults.Apply(ref style);
		return style;
	}

	private static SoundStyle NPCHitSound(int soundStyle)
	{
		return SoundWithDefaults(SoundStyleDefaults.NPCHitDefaults, new SoundStyle($"{"Terraria/Sounds/"}NPC_Hit_{soundStyle}"));
	}

	private static SoundStyle NPCHitSound(ReadOnlySpan<int> soundStyles)
	{
		return SoundWithDefaults(SoundStyleDefaults.NPCHitDefaults, new SoundStyle("Terraria/Sounds/NPC_Hit_", soundStyles));
	}

	private static SoundStyle NPCDeathSound(int soundStyle)
	{
		return SoundWithDefaults(SoundStyleDefaults.NPCDeathDefaults, new SoundStyle($"{"Terraria/Sounds/"}NPC_Killed_{soundStyle}"));
	}

	private static SoundStyle NPCDeathSound(ReadOnlySpan<int> soundStyles)
	{
		return SoundWithDefaults(SoundStyleDefaults.NPCDeathDefaults, new SoundStyle("Terraria/Sounds/NPC_Killed_", soundStyles));
	}

	private static SoundStyle ItemSound(int soundStyle)
	{
		return SoundWithDefaults(SoundStyleDefaults.ItemDefaults, new SoundStyle($"{"Terraria/Sounds/"}Item_{soundStyle}"));
	}

	private static SoundStyle ItemSound(ReadOnlySpan<int> soundStyles)
	{
		return SoundWithDefaults(SoundStyleDefaults.ItemDefaults, new SoundStyle("Terraria/Sounds/Item_", soundStyles));
	}

	private static SoundStyle ZombieSound(int soundStyle)
	{
		SoundStyleDefaults zombieDefaults = SoundStyleDefaults.ZombieDefaults;
		DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 2);
		defaultInterpolatedStringHandler.AppendFormatted("Terraria/Sounds/");
		defaultInterpolatedStringHandler.AppendLiteral("Zombie_");
		defaultInterpolatedStringHandler.AppendFormatted(soundStyle);
		return SoundWithDefaults(zombieDefaults, new SoundStyle(defaultInterpolatedStringHandler.ToStringAndClear()))with
		{
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
	}

	private static SoundStyle ZombieSound(ReadOnlySpan<int> soundStyles)
	{
		return SoundWithDefaults(SoundStyleDefaults.ZombieDefaults, new SoundStyle("Terraria/Sounds/Zombie_", soundStyles))with
		{
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};
	}

	internal static SoundStyle? GetLegacyStyle(int type, int style)
	{
		switch (type)
		{
		case 2:
		case 3:
		case 4:
		case 29:
			return (style >= 1 && style < legacyArrayedStylesMapping[type].Length) ? new SoundStyle?(legacyArrayedStylesMapping[type][style]) : null;
		case 0:
			return Dig;
		case 1:
			return PlayerHit;
		case 5:
			return PlayerKilled;
		case 6:
			return Grass;
		case 7:
			return Grab;
		case 8:
			return DoorOpen;
		case 9:
			return DoorClosed;
		case 10:
			return MenuOpen;
		case 11:
			return MenuClose;
		case 12:
			return MenuTick;
		case 13:
			return Shatter;
		case 14:
			return ZombieMoan;
		case 15:
			return style switch
			{
				0 => Roar, 
				1 => WormDig, 
				2 => ScaryScream, 
				4 => WormDigQuiet, 
				_ => null, 
			};
		case 16:
			return DoubleJump;
		case 17:
			return Run;
		case 18:
			return Coins;
		case 19:
			return (style != 1) ? Splash : SplashWeak;
		case 20:
			return FemaleHit;
		case 21:
			return Tink;
		case 22:
			return Unlock;
		case 23:
			return Drown;
		case 24:
			return Chat;
		case 25:
			return MaxMana;
		case 26:
			return Mummy;
		case 27:
			return Pixie;
		case 28:
			return Mech;
		case 30:
			return Duck;
		case 31:
			return Frog;
		case 32:
			return Bird;
		case 33:
			return Critter;
		case 34:
			return Waterfall;
		case 35:
			return Lavafall;
		case 36:
			return (style != -1) ? ForceRoar : ForceRoarPitched;
		case 37:
			return Meowmere;
		case 38:
			return CoinPickup;
		case 39:
			return Drip;
		case 40:
			return Camera;
		case 41:
			return MoonLord;
		case 43:
			return Thunder;
		case 44:
			return Seagull;
		case 45:
			return Dolphin;
		case 46:
			return Owl;
		case 47:
			return GuitarC;
		case 48:
			return GuitarD;
		case 49:
			return GuitarEm;
		case 50:
			return GuitarG;
		case 51:
			return GuitarBm;
		case 52:
			return GuitarAm;
		case 53:
			return DrumHiHat;
		case 54:
			return DrumTomHigh;
		case 55:
			return DrumTomLow;
		case 56:
			return DrumTomMid;
		case 57:
			return DrumClosedHiHat;
		case 58:
			return DrumCymbal1;
		case 59:
			return DrumCymbal2;
		case 60:
			return DrumKick;
		case 61:
			return DrumTamaSnare;
		case 62:
			return DrumFloorTom;
		case 63:
			return Research;
		case 64:
			return ResearchComplete;
		case 65:
			return QueenSlime;
		default:
			return null;
		}
	}
}
