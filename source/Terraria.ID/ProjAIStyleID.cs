using ReLogic.Reflection;

namespace Terraria.ID;

public class ProjAIStyleID
{
	public static readonly IdDictionary Search = IdDictionary.Create<ProjAIStyleID, short>();

	/// <summary>
	/// Behavior: Includes Bullets and Lasers<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.WoodenArrowFriendly" />, <see cref="F:Terraria.ID.ProjectileID.FireArrow" />, <see cref="F:Terraria.ID.ProjectileID.UnholyArrow" />, <see cref="F:Terraria.ID.ProjectileID.JestersArrow" />, <see cref="F:Terraria.ID.ProjectileID.Bullet" />, <see cref="F:Terraria.ID.ProjectileID.GreenLaser" />, <see cref="F:Terraria.ID.ProjectileID.MeteorShot" />, <see cref="F:Terraria.ID.ProjectileID.HarpyFeather" />, <see cref="F:Terraria.ID.ProjectileID.HellfireArrow" />, <see cref="F:Terraria.ID.ProjectileID.Seed" />, <see cref="F:Terraria.ID.ProjectileID.Stinger" />, <see cref="F:Terraria.ID.ProjectileID.WoodenArrowHostile" />, <see cref="F:Terraria.ID.ProjectileID.FlamingArrow" />, <see cref="F:Terraria.ID.ProjectileID.EyeLaser" />, <see cref="F:Terraria.ID.ProjectileID.PinkLaser" />, <see cref="F:Terraria.ID.ProjectileID.PurpleLaser" />, <see cref="F:Terraria.ID.ProjectileID.CrystalBullet" />, <see cref="F:Terraria.ID.ProjectileID.HolyArrow" />, <see cref="F:Terraria.ID.ProjectileID.PoisonDart" />, <see cref="F:Terraria.ID.ProjectileID.DeathLaser" />, <see cref="F:Terraria.ID.ProjectileID.CursedArrow" />, <see cref="F:Terraria.ID.ProjectileID.CursedBullet" />, <see cref="F:Terraria.ID.ProjectileID.BulletSnowman" />, <see cref="F:Terraria.ID.ProjectileID.BoneArrow" />, <see cref="F:Terraria.ID.ProjectileID.FrostArrow" />, <see cref="F:Terraria.ID.ProjectileID.CopperCoin" />, <see cref="F:Terraria.ID.ProjectileID.SilverCoin" />, <see cref="F:Terraria.ID.ProjectileID.GoldCoin" />, <see cref="F:Terraria.ID.ProjectileID.PlatinumCoin" />, <see cref="F:Terraria.ID.ProjectileID.FrostburnArrow" />, <see cref="F:Terraria.ID.ProjectileID.IceSpike" />, <see cref="F:Terraria.ID.ProjectileID.JungleSpike" />, <see cref="F:Terraria.ID.ProjectileID.ConfettiGun" />, <see cref="F:Terraria.ID.ProjectileID.BulletDeadeye" />, <see cref="F:Terraria.ID.ProjectileID.PoisonDartTrap" />, <see cref="F:Terraria.ID.ProjectileID.PygmySpear" />, <see cref="F:Terraria.ID.ProjectileID.ChlorophyteBullet" />, <see cref="F:Terraria.ID.ProjectileID.ChlorophyteArrow" />, <see cref="F:Terraria.ID.ProjectileID.BulletHighVelocity" />, <see cref="F:Terraria.ID.ProjectileID.Stynger" />, <see cref="F:Terraria.ID.ProjectileID.FlowerPowPetal" />, <see cref="F:Terraria.ID.ProjectileID.FrostBeam" />, <see cref="F:Terraria.ID.ProjectileID.EyeBeam" />, <see cref="F:Terraria.ID.ProjectileID.PoisonFang" />, <see cref="F:Terraria.ID.ProjectileID.PoisonDartBlowgun" />, <see cref="F:Terraria.ID.ProjectileID.Skull" />, <see cref="F:Terraria.ID.ProjectileID.SeedPlantera" />, <see cref="F:Terraria.ID.ProjectileID.PoisonSeedPlantera" />, <see cref="F:Terraria.ID.ProjectileID.IchorArrow" />, <see cref="F:Terraria.ID.ProjectileID.IchorBullet" />, <see cref="F:Terraria.ID.ProjectileID.VenomArrow" />, <see cref="F:Terraria.ID.ProjectileID.VenomBullet" />, <see cref="F:Terraria.ID.ProjectileID.PartyBullet" />, <see cref="F:Terraria.ID.ProjectileID.NanoBullet" />, <see cref="F:Terraria.ID.ProjectileID.ExplosiveBullet" />, <see cref="F:Terraria.ID.ProjectileID.GoldenBullet" />, <see cref="F:Terraria.ID.ProjectileID.ConfettiMelee" />, <see cref="F:Terraria.ID.ProjectileID.Shadowflames" />, <see cref="F:Terraria.ID.ProjectileID.SniperBullet" />, <see cref="F:Terraria.ID.ProjectileID.CandyCorn" />, <see cref="F:Terraria.ID.ProjectileID.JackOLantern" />, <see cref="F:Terraria.ID.ProjectileID.Stake" />, <see cref="F:Terraria.ID.ProjectileID.FlamingWood" />, <see cref="F:Terraria.ID.ProjectileID.PineNeedleFriendly" />, <see cref="F:Terraria.ID.ProjectileID.Blizzard" />, <see cref="F:Terraria.ID.ProjectileID.NorthPoleSnowflake" />, <see cref="F:Terraria.ID.ProjectileID.PineNeedleHostile" />, <see cref="F:Terraria.ID.ProjectileID.FrostWave" />, <see cref="F:Terraria.ID.ProjectileID.FrostShard" />, <see cref="F:Terraria.ID.ProjectileID.Missile" />, <see cref="F:Terraria.ID.ProjectileID.VenomFang" />, <see cref="F:Terraria.ID.ProjectileID.PulseBolt" />, <see cref="F:Terraria.ID.ProjectileID.HornetStinger" />, <see cref="F:Terraria.ID.ProjectileID.ImpFireball" />, <see cref="F:Terraria.ID.ProjectileID.MiniRetinaLaser" />, <see cref="F:Terraria.ID.ProjectileID.MiniSharkron" />, <see cref="F:Terraria.ID.ProjectileID.Meteor1" />, <see cref="F:Terraria.ID.ProjectileID.Meteor2" />, <see cref="F:Terraria.ID.ProjectileID.Meteor3" />, <see cref="F:Terraria.ID.ProjectileID.MartianTurretBolt" />, <see cref="F:Terraria.ID.ProjectileID.BrainScramblerBolt" />, <see cref="F:Terraria.ID.ProjectileID.GigaZapperSpear" />, <see cref="F:Terraria.ID.ProjectileID.RayGunnerLaser" />, <see cref="F:Terraria.ID.ProjectileID.LaserMachinegunLaser" />, <see cref="F:Terraria.ID.ProjectileID.ElectrosphereMissile" />, <see cref="F:Terraria.ID.ProjectileID.SaucerLaser" />, <see cref="F:Terraria.ID.ProjectileID.ChargedBlasterOrb" />, <see cref="F:Terraria.ID.ProjectileID.PhantasmalBolt" />, <see cref="F:Terraria.ID.ProjectileID.CultistBossFireBall" />, <see cref="F:Terraria.ID.ProjectileID.CultistBossFireBallClone" />, <see cref="F:Terraria.ID.ProjectileID.BeeArrow" />, <see cref="F:Terraria.ID.ProjectileID.WebSpit" />, <see cref="F:Terraria.ID.ProjectileID.BoneArrowFromMerchant" />, <see cref="F:Terraria.ID.ProjectileID.CrystalDart" />, <see cref="F:Terraria.ID.ProjectileID.CursedDart" />, <see cref="F:Terraria.ID.ProjectileID.IchorDart" />, <see cref="F:Terraria.ID.ProjectileID.SeedlerThorn" />, <see cref="F:Terraria.ID.ProjectileID.Hellwing" />, <see cref="F:Terraria.ID.ProjectileID.ShadowFlameArrow" />, <see cref="F:Terraria.ID.ProjectileID.Nail" />, <see cref="F:Terraria.ID.ProjectileID.JavelinFriendly" />, <see cref="F:Terraria.ID.ProjectileID.JavelinHostile" />, <see cref="F:Terraria.ID.ProjectileID.BoneGloveProj" />, <see cref="F:Terraria.ID.ProjectileID.SalamanderSpit" />, <see cref="F:Terraria.ID.ProjectileID.NebulaLaser" />, <see cref="F:Terraria.ID.ProjectileID.VortexLaser" />, <see cref="F:Terraria.ID.ProjectileID.VortexAcid" />, <see cref="F:Terraria.ID.ProjectileID.ClothiersCurse" />, <see cref="F:Terraria.ID.ProjectileID.PainterPaintball" />, <see cref="F:Terraria.ID.ProjectileID.MartianWalkerLaser" />, <see cref="F:Terraria.ID.ProjectileID.AncientDoomProjectile" />, <see cref="F:Terraria.ID.ProjectileID.BlowupSmoke" />, <see cref="F:Terraria.ID.ProjectileID.PortalGunBolt" />, <see cref="F:Terraria.ID.ProjectileID.SpikedSlimeSpike" />, <see cref="F:Terraria.ID.ProjectileID.ScutlixLaser" />, <see cref="F:Terraria.ID.ProjectileID.VortexBeaterRocket" />, <see cref="F:Terraria.ID.ProjectileID.BlowupSmokeMoonlord" />, <see cref="F:Terraria.ID.ProjectileID.NebulaBlaze1" />, <see cref="F:Terraria.ID.ProjectileID.NebulaBlaze2" />, <see cref="F:Terraria.ID.ProjectileID.MoonlordBullet" />, <see cref="F:Terraria.ID.ProjectileID.MoonlordArrow" />, <see cref="F:Terraria.ID.ProjectileID.MoonlordArrowTrail" />, <see cref="F:Terraria.ID.ProjectileID.LunarFlare" />, <see cref="F:Terraria.ID.ProjectileID.SkyFracture" />, <see cref="F:Terraria.ID.ProjectileID.BlackBolt" />, <see cref="F:Terraria.ID.ProjectileID.DD2JavelinHostile" />, <see cref="F:Terraria.ID.ProjectileID.DD2DrakinShot" />, <see cref="F:Terraria.ID.ProjectileID.DD2DarkMageBolt" />, <see cref="F:Terraria.ID.ProjectileID.DD2OgreSpit" />, <see cref="F:Terraria.ID.ProjectileID.DD2BallistraProj" />, <see cref="F:Terraria.ID.ProjectileID.DD2LightningBugZap" />, <see cref="F:Terraria.ID.ProjectileID.DD2SquireSonicBoom" />, <see cref="F:Terraria.ID.ProjectileID.DD2JavelinHostileT3" />, <see cref="F:Terraria.ID.ProjectileID.DD2BetsyFireball" />, <see cref="F:Terraria.ID.ProjectileID.DD2PhoenixBowShot" />, <see cref="F:Terraria.ID.ProjectileID.MonkStaffT3_AltShot" />, <see cref="F:Terraria.ID.ProjectileID.DD2BetsyArrow" />, <see cref="F:Terraria.ID.ProjectileID.ApprenticeStaffT3Shot" />, <see cref="F:Terraria.ID.ProjectileID.BookStaffShot" />, <see cref="F:Terraria.ID.ProjectileID.QueenBeeStinger" />, <see cref="F:Terraria.ID.ProjectileID.RollingCactusSpike" />, <see cref="F:Terraria.ID.ProjectileID.Geode" />, <see cref="F:Terraria.ID.ProjectileID.BloodShot" />, <see cref="F:Terraria.ID.ProjectileID.BloodNautilusShot" />, <see cref="F:Terraria.ID.ProjectileID.BloodArrow" />, <see cref="F:Terraria.ID.ProjectileID.BookOfSkullsSkull" />, <see cref="F:Terraria.ID.ProjectileID.ZapinatorLaser" />, <see cref="F:Terraria.ID.ProjectileID.QueenSlimeMinionBlueSpike" />, <see cref="F:Terraria.ID.ProjectileID.QueenSlimeMinionPinkBall" />, <see cref="F:Terraria.ID.ProjectileID.QueenSlimeGelAttack" />, <see cref="F:Terraria.ID.ProjectileID.VolatileGelatinBall" />
	/// </summary>
	public const short Arrow = 1;

	/// <summary>
	/// Behavior: Includes Shurikens, Bones, and Knives<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Shuriken" />, <see cref="F:Terraria.ID.ProjectileID.Bone" />, <see cref="F:Terraria.ID.ProjectileID.ThrowingKnife" />, <see cref="F:Terraria.ID.ProjectileID.PoisonedKnife" />, <see cref="F:Terraria.ID.ProjectileID.HolyWater" />, <see cref="F:Terraria.ID.ProjectileID.UnholyWater" />, <see cref="F:Terraria.ID.ProjectileID.MagicDagger" />, <see cref="F:Terraria.ID.ProjectileID.CannonballFriendly" />, <see cref="F:Terraria.ID.ProjectileID.SnowBallFriendly" />, <see cref="F:Terraria.ID.ProjectileID.CannonballHostile" />, <see cref="F:Terraria.ID.ProjectileID.StyngerShrapnel" />, <see cref="F:Terraria.ID.ProjectileID.PaladinsHammerHostile" />, <see cref="F:Terraria.ID.ProjectileID.VampireKnife" />, <see cref="F:Terraria.ID.ProjectileID.EatersBite" />, <see cref="F:Terraria.ID.ProjectileID.RottenEgg" />, <see cref="F:Terraria.ID.ProjectileID.StarAnise" />, <see cref="F:Terraria.ID.ProjectileID.OrnamentHostileShrapnel" />, <see cref="F:Terraria.ID.ProjectileID.LovePotion" />, <see cref="F:Terraria.ID.ProjectileID.FoulPotion" />, <see cref="F:Terraria.ID.ProjectileID.SkeletonBone" />, <see cref="F:Terraria.ID.ProjectileID.ShadowFlameKnife" />, <see cref="F:Terraria.ID.ProjectileID.DrManFlyFlask" />, <see cref="F:Terraria.ID.ProjectileID.Spark" />, <see cref="F:Terraria.ID.ProjectileID.ToxicFlask" />, <see cref="F:Terraria.ID.ProjectileID.FrostDaggerfish" />, <see cref="F:Terraria.ID.ProjectileID.NurseSyringeHurt" />, <see cref="F:Terraria.ID.ProjectileID.SantaBombs" />, <see cref="F:Terraria.ID.ProjectileID.BoneDagger" />, <see cref="F:Terraria.ID.ProjectileID.BloodWater" />, <see cref="F:Terraria.ID.ProjectileID.Football" />, <see cref="F:Terraria.ID.ProjectileID.TreeGlobe" />, <see cref="F:Terraria.ID.ProjectileID.WorldGlobe" />, <see cref="F:Terraria.ID.ProjectileID.RockGolemRock" />, <see cref="F:Terraria.ID.ProjectileID.GelBalloon" />, <see cref="F:Terraria.ID.ProjectileID.WandOfSparkingSpark" />
	/// </summary>
	public const short ThrownProjectile = 2;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.EnchantedBoomerang" />, <see cref="F:Terraria.ID.ProjectileID.Flamarang" />, <see cref="F:Terraria.ID.ProjectileID.ThornChakram" />, <see cref="F:Terraria.ID.ProjectileID.WoodenBoomerang" />, <see cref="F:Terraria.ID.ProjectileID.LightDisc" />, <see cref="F:Terraria.ID.ProjectileID.IceBoomerang" />, <see cref="F:Terraria.ID.ProjectileID.PossessedHatchet" />, <see cref="F:Terraria.ID.ProjectileID.Bananarang" />, <see cref="F:Terraria.ID.ProjectileID.PaladinsHammerFriendly" />, <see cref="F:Terraria.ID.ProjectileID.BloodyMachete" />, <see cref="F:Terraria.ID.ProjectileID.FruitcakeChakram" />, <see cref="F:Terraria.ID.ProjectileID.Anchor" />, <see cref="F:Terraria.ID.ProjectileID.BouncingShield" />, <see cref="F:Terraria.ID.ProjectileID.Shroomerang" />, <see cref="F:Terraria.ID.ProjectileID.CombatWrench" />
	/// </summary>
	public const short Boomerang = 3;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.VilethornBase" />, <see cref="F:Terraria.ID.ProjectileID.VilethornTip" />, <see cref="F:Terraria.ID.ProjectileID.NettleBurstRight" />, <see cref="F:Terraria.ID.ProjectileID.NettleBurstLeft" />, <see cref="F:Terraria.ID.ProjectileID.NettleBurstEnd" />, <see cref="F:Terraria.ID.ProjectileID.CrystalVileShardHead" />, <see cref="F:Terraria.ID.ProjectileID.CrystalVileShardShaft" />
	/// </summary>
	public const short Vilethorn = 4;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Starfury" />, <see cref="F:Terraria.ID.ProjectileID.FallingStar" />, <see cref="F:Terraria.ID.ProjectileID.HallowStar" />, <see cref="F:Terraria.ID.ProjectileID.StarWrath" />, <see cref="F:Terraria.ID.ProjectileID.ManaCloakStar" />, <see cref="F:Terraria.ID.ProjectileID.BeeCloakStar" />, <see cref="F:Terraria.ID.ProjectileID.StarVeilStar" />, <see cref="F:Terraria.ID.ProjectileID.StarCloakStar" />, <see cref="F:Terraria.ID.ProjectileID.StarCannonStar" />
	/// </summary>
	public const short FallingStar = 5;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.PurificationPowder" />, <see cref="F:Terraria.ID.ProjectileID.VilePowder" />, <see cref="F:Terraria.ID.ProjectileID.ViciousPowder" />
	/// </summary>
	public const short Powder = 6;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Hook" />, <see cref="F:Terraria.ID.ProjectileID.IvyWhip" />, <see cref="F:Terraria.ID.ProjectileID.DualHookBlue" />, <see cref="F:Terraria.ID.ProjectileID.DualHookRed" />, <see cref="F:Terraria.ID.ProjectileID.Web" />, <see cref="F:Terraria.ID.ProjectileID.GemHookAmethyst" />, <see cref="F:Terraria.ID.ProjectileID.GemHookTopaz" />, <see cref="F:Terraria.ID.ProjectileID.GemHookSapphire" />, <see cref="F:Terraria.ID.ProjectileID.GemHookEmerald" />, <see cref="F:Terraria.ID.ProjectileID.GemHookRuby" />, <see cref="F:Terraria.ID.ProjectileID.GemHookDiamond" />, <see cref="F:Terraria.ID.ProjectileID.SkeletronHand" />, <see cref="F:Terraria.ID.ProjectileID.BatHook" />, <see cref="F:Terraria.ID.ProjectileID.WoodHook" />, <see cref="F:Terraria.ID.ProjectileID.CandyCaneHook" />, <see cref="F:Terraria.ID.ProjectileID.ChristmasHook" />, <see cref="F:Terraria.ID.ProjectileID.FishHook" />, <see cref="F:Terraria.ID.ProjectileID.SlimeHook" />, <see cref="F:Terraria.ID.ProjectileID.TrackHook" />, <see cref="F:Terraria.ID.ProjectileID.AntiGravityHook" />, <see cref="F:Terraria.ID.ProjectileID.TendonHook" />, <see cref="F:Terraria.ID.ProjectileID.ThornHook" />, <see cref="F:Terraria.ID.ProjectileID.IlluminantHook" />, <see cref="F:Terraria.ID.ProjectileID.WormHook" />, <see cref="F:Terraria.ID.ProjectileID.LunarHookSolar" />, <see cref="F:Terraria.ID.ProjectileID.LunarHookVortex" />, <see cref="F:Terraria.ID.ProjectileID.LunarHookNebula" />, <see cref="F:Terraria.ID.ProjectileID.LunarHookStardust" />, <see cref="F:Terraria.ID.ProjectileID.StaticHook" />, <see cref="F:Terraria.ID.ProjectileID.AmberHook" />, <see cref="F:Terraria.ID.ProjectileID.SquirrelHook" />, <see cref="F:Terraria.ID.ProjectileID.QueenSlimeHook" />
	/// </summary>
	public const short Hook = 7;

	/// <summary>
	/// Behavior: Includes the Flower of Fire, Waterbolt, Cursed Flame, and Meowmere projectiles<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.BallofFire" />, <see cref="F:Terraria.ID.ProjectileID.WaterBolt" />, <see cref="F:Terraria.ID.ProjectileID.CursedFlameFriendly" />, <see cref="F:Terraria.ID.ProjectileID.CursedFlameHostile" />, <see cref="F:Terraria.ID.ProjectileID.BallofFrost" />, <see cref="F:Terraria.ID.ProjectileID.Fireball" />, <see cref="F:Terraria.ID.ProjectileID.Meowmere" />
	/// </summary>
	public const short Bounce = 8;

	/// <summary>
	/// Behavior: Includes Flame Lash and Magic Missile<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.MagicMissile" />, <see cref="F:Terraria.ID.ProjectileID.Flamelash" />, <see cref="F:Terraria.ID.ProjectileID.RainbowRodBullet" />, <see cref="F:Terraria.ID.ProjectileID.FlyingKnife" />
	/// </summary>
	public const short MagicMissile = 9;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.DirtBall" />, <see cref="F:Terraria.ID.ProjectileID.SandBallFalling" />, <see cref="F:Terraria.ID.ProjectileID.MudBall" />, <see cref="F:Terraria.ID.ProjectileID.AshBallFalling" />, <see cref="F:Terraria.ID.ProjectileID.SandBallGun" />, <see cref="F:Terraria.ID.ProjectileID.EbonsandBallFalling" />, <see cref="F:Terraria.ID.ProjectileID.EbonsandBallGun" />, <see cref="F:Terraria.ID.ProjectileID.PearlSandBallFalling" />, <see cref="F:Terraria.ID.ProjectileID.PearlSandBallGun" />, <see cref="F:Terraria.ID.ProjectileID.SiltBall" />, <see cref="F:Terraria.ID.ProjectileID.SnowBallHostile" />, <see cref="F:Terraria.ID.ProjectileID.SlushBall" />, <see cref="F:Terraria.ID.ProjectileID.CrimsandBallFalling" />, <see cref="F:Terraria.ID.ProjectileID.CrimsandBallGun" />, <see cref="F:Terraria.ID.ProjectileID.CopperCoinsFalling" />, <see cref="F:Terraria.ID.ProjectileID.SilverCoinsFalling" />, <see cref="F:Terraria.ID.ProjectileID.GoldCoinsFalling" />, <see cref="F:Terraria.ID.ProjectileID.PlatinumCoinsFalling" />, <see cref="F:Terraria.ID.ProjectileID.BlueDungeonDebris" />, <see cref="F:Terraria.ID.ProjectileID.GreenDungeonDebris" />, <see cref="F:Terraria.ID.ProjectileID.PinkDungeonDebris" />, <see cref="F:Terraria.ID.ProjectileID.ShellPileFalling" />
	/// </summary>
	public const short FallingTile = 10;

	/// <summary>
	/// Behavior: Includes Shadow Orb and Fairy pets<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.ShadowOrb" />, <see cref="F:Terraria.ID.ProjectileID.BlueFairy" />, <see cref="F:Terraria.ID.ProjectileID.PinkFairy" />, <see cref="F:Terraria.ID.ProjectileID.GreenFairy" />
	/// </summary>
	public const short FloatingFollow = 11;

	/// <summary>
	/// Behavior: Includes Aqua Scepter and Golden Shower projectiles<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.WaterStream" />, <see cref="F:Terraria.ID.ProjectileID.GoldenShowerFriendly" />, <see cref="F:Terraria.ID.ProjectileID.GoldenShowerHostile" />
	/// </summary>
	public const short Stream = 12;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Harpoon" />, <see cref="F:Terraria.ID.ProjectileID.GolemFist" />, <see cref="F:Terraria.ID.ProjectileID.BoxingGlove" />, <see cref="F:Terraria.ID.ProjectileID.ChainKnife" />, <see cref="F:Terraria.ID.ProjectileID.ChainGuillotine" />
	/// </summary>
	public const short Harpoon = 13;

	/// <summary>
	/// Behavior: Includes most non-destructive Explosive, Glowstick, and Spike Ball projectiles<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.SpikyBall" />, <see cref="F:Terraria.ID.ProjectileID.Glowstick" />, <see cref="F:Terraria.ID.ProjectileID.StickyGlowstick" />, <see cref="F:Terraria.ID.ProjectileID.Beenade" />, <see cref="F:Terraria.ID.ProjectileID.SpikyBallTrap" />, <see cref="F:Terraria.ID.ProjectileID.SmokeBomb" />, <see cref="F:Terraria.ID.ProjectileID.BoulderStaffOfEarth" />, <see cref="F:Terraria.ID.ProjectileID.ThornBall" />, <see cref="F:Terraria.ID.ProjectileID.GreekFire1" />, <see cref="F:Terraria.ID.ProjectileID.GreekFire2" />, <see cref="F:Terraria.ID.ProjectileID.GreekFire3" />, <see cref="F:Terraria.ID.ProjectileID.OrnamentHostile" />, <see cref="F:Terraria.ID.ProjectileID.Spike" />, <see cref="F:Terraria.ID.ProjectileID.SpiderEgg" />, <see cref="F:Terraria.ID.ProjectileID.MolotovFire" />, <see cref="F:Terraria.ID.ProjectileID.MolotovFire2" />, <see cref="F:Terraria.ID.ProjectileID.MolotovFire3" />, <see cref="F:Terraria.ID.ProjectileID.SaucerScrap" />, <see cref="F:Terraria.ID.ProjectileID.SpelunkerGlowstick" />, <see cref="F:Terraria.ID.ProjectileID.CursedDartFlame" />, <see cref="F:Terraria.ID.ProjectileID.SeedlerNut" />, <see cref="F:Terraria.ID.ProjectileID.BouncyGlowstick" />, <see cref="F:Terraria.ID.ProjectileID.Twinkle" />, <see cref="F:Terraria.ID.ProjectileID.FairyGlowstick" />, <see cref="F:Terraria.ID.ProjectileID.DripplerFlailExtraBall" />
	/// </summary>
	public const short GroundProjectile = 14;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.BallOHurt" />, <see cref="F:Terraria.ID.ProjectileID.BlueMoon" />, <see cref="F:Terraria.ID.ProjectileID.Sunfury" />, <see cref="F:Terraria.ID.ProjectileID.TheDaoofPow" />, <see cref="F:Terraria.ID.ProjectileID.TheMeatball" />, <see cref="F:Terraria.ID.ProjectileID.FlowerPow" />, <see cref="F:Terraria.ID.ProjectileID.DripplerFlail" />, <see cref="F:Terraria.ID.ProjectileID.Mace" />, <see cref="F:Terraria.ID.ProjectileID.FlamingMace" />
	/// </summary>
	public const short Flail = 15;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Bomb" />, <see cref="F:Terraria.ID.ProjectileID.Dynamite" />, <see cref="F:Terraria.ID.ProjectileID.Grenade" />, <see cref="F:Terraria.ID.ProjectileID.StickyBomb" />, <see cref="F:Terraria.ID.ProjectileID.HappyBomb" />, <see cref="F:Terraria.ID.ProjectileID.BombSkeletronPrime" />, <see cref="F:Terraria.ID.ProjectileID.Explosives" />, <see cref="F:Terraria.ID.ProjectileID.GrenadeI" />, <see cref="F:Terraria.ID.ProjectileID.RocketI" />, <see cref="F:Terraria.ID.ProjectileID.ProximityMineI" />, <see cref="F:Terraria.ID.ProjectileID.GrenadeII" />, <see cref="F:Terraria.ID.ProjectileID.RocketII" />, <see cref="F:Terraria.ID.ProjectileID.ProximityMineII" />, <see cref="F:Terraria.ID.ProjectileID.GrenadeIII" />, <see cref="F:Terraria.ID.ProjectileID.RocketIII" />, <see cref="F:Terraria.ID.ProjectileID.ProximityMineIII" />, <see cref="F:Terraria.ID.ProjectileID.GrenadeIV" />, <see cref="F:Terraria.ID.ProjectileID.RocketIV" />, <see cref="F:Terraria.ID.ProjectileID.ProximityMineIV" />, <see cref="F:Terraria.ID.ProjectileID.Landmine" />, <see cref="F:Terraria.ID.ProjectileID.RocketSkeleton" />, <see cref="F:Terraria.ID.ProjectileID.RocketSnowmanI" />, <see cref="F:Terraria.ID.ProjectileID.RocketSnowmanII" />, <see cref="F:Terraria.ID.ProjectileID.RocketSnowmanIII" />, <see cref="F:Terraria.ID.ProjectileID.RocketSnowmanIV" />, <see cref="F:Terraria.ID.ProjectileID.StickyGrenade" />, <see cref="F:Terraria.ID.ProjectileID.StickyDynamite" />, <see cref="F:Terraria.ID.ProjectileID.BouncyBomb" />, <see cref="F:Terraria.ID.ProjectileID.BouncyGrenade" />, <see cref="F:Terraria.ID.ProjectileID.BombFish" />, <see cref="F:Terraria.ID.ProjectileID.PartyGirlGrenade" />, <see cref="F:Terraria.ID.ProjectileID.BouncyDynamite" />, <see cref="F:Terraria.ID.ProjectileID.DD2GoblinBomb" />, <see cref="F:Terraria.ID.ProjectileID.ScarabBomb" />, <see cref="F:Terraria.ID.ProjectileID.ClusterRocketI" />, <see cref="F:Terraria.ID.ProjectileID.ClusterGrenadeI" />, <see cref="F:Terraria.ID.ProjectileID.ClusterMineI" />, <see cref="F:Terraria.ID.ProjectileID.ClusterFragmentsI" />, <see cref="F:Terraria.ID.ProjectileID.ClusterRocketII" />, <see cref="F:Terraria.ID.ProjectileID.ClusterGrenadeII" />, <see cref="F:Terraria.ID.ProjectileID.ClusterMineII" />, <see cref="F:Terraria.ID.ProjectileID.ClusterFragmentsII" />, <see cref="F:Terraria.ID.ProjectileID.WetRocket" />, <see cref="F:Terraria.ID.ProjectileID.WetGrenade" />, <see cref="F:Terraria.ID.ProjectileID.WetMine" />, <see cref="F:Terraria.ID.ProjectileID.LavaRocket" />, <see cref="F:Terraria.ID.ProjectileID.LavaGrenade" />, <see cref="F:Terraria.ID.ProjectileID.LavaMine" />, <see cref="F:Terraria.ID.ProjectileID.HoneyRocket" />, <see cref="F:Terraria.ID.ProjectileID.HoneyGrenade" />, <see cref="F:Terraria.ID.ProjectileID.HoneyMine" />, <see cref="F:Terraria.ID.ProjectileID.MiniNukeRocketI" />, <see cref="F:Terraria.ID.ProjectileID.MiniNukeGrenadeI" />, <see cref="F:Terraria.ID.ProjectileID.MiniNukeMineI" />, <see cref="F:Terraria.ID.ProjectileID.MiniNukeRocketII" />, <see cref="F:Terraria.ID.ProjectileID.MiniNukeGrenadeII" />, <see cref="F:Terraria.ID.ProjectileID.MiniNukeMineII" />, <see cref="F:Terraria.ID.ProjectileID.DryRocket" />, <see cref="F:Terraria.ID.ProjectileID.DryGrenade" />, <see cref="F:Terraria.ID.ProjectileID.DryMine" />, <see cref="F:Terraria.ID.ProjectileID.ClusterSnowmanRocketI" />, <see cref="F:Terraria.ID.ProjectileID.ClusterSnowmanRocketII" />, <see cref="F:Terraria.ID.ProjectileID.WetSnowmanRocket" />, <see cref="F:Terraria.ID.ProjectileID.LavaSnowmanRocket" />, <see cref="F:Terraria.ID.ProjectileID.HoneySnowmanRocket" />, <see cref="F:Terraria.ID.ProjectileID.MiniNukeSnowmanRocketI" />, <see cref="F:Terraria.ID.ProjectileID.MiniNukeSnowmanRocketII" />, <see cref="F:Terraria.ID.ProjectileID.DrySnowmanRocket" />, <see cref="F:Terraria.ID.ProjectileID.ClusterSnowmanFragmentsI" />, <see cref="F:Terraria.ID.ProjectileID.ClusterSnowmanFragmentsII" />, <see cref="F:Terraria.ID.ProjectileID.WetBomb" />, <see cref="F:Terraria.ID.ProjectileID.LavaBomb" />, <see cref="F:Terraria.ID.ProjectileID.HoneyBomb" />, <see cref="F:Terraria.ID.ProjectileID.DryBomb" />, <see cref="F:Terraria.ID.ProjectileID.DirtBomb" />, <see cref="F:Terraria.ID.ProjectileID.DirtStickyBomb" />, <see cref="F:Terraria.ID.ProjectileID.SantankMountRocket" />
	/// </summary>
	public const short Explosive = 16;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Tombstone" />, <see cref="F:Terraria.ID.ProjectileID.GraveMarker" />, <see cref="F:Terraria.ID.ProjectileID.CrossGraveMarker" />, <see cref="F:Terraria.ID.ProjectileID.Headstone" />, <see cref="F:Terraria.ID.ProjectileID.Gravestone" />, <see cref="F:Terraria.ID.ProjectileID.Obelisk" />, <see cref="F:Terraria.ID.ProjectileID.RichGravestone1" />, <see cref="F:Terraria.ID.ProjectileID.RichGravestone2" />, <see cref="F:Terraria.ID.ProjectileID.RichGravestone3" />, <see cref="F:Terraria.ID.ProjectileID.RichGravestone4" />, <see cref="F:Terraria.ID.ProjectileID.RichGravestone5" />
	/// </summary>
	public const short GraveMarker = 17;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.DemonSickle" />, <see cref="F:Terraria.ID.ProjectileID.DemonScythe" />, <see cref="F:Terraria.ID.ProjectileID.IceSickle" />, <see cref="F:Terraria.ID.ProjectileID.DeathSickle" />
	/// </summary>
	public const short Sickle = 18;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.DarkLance" />, <see cref="F:Terraria.ID.ProjectileID.Trident" />, <see cref="F:Terraria.ID.ProjectileID.Spear" />, <see cref="F:Terraria.ID.ProjectileID.MythrilHalberd" />, <see cref="F:Terraria.ID.ProjectileID.AdamantiteGlaive" />, <see cref="F:Terraria.ID.ProjectileID.CobaltNaginata" />, <see cref="F:Terraria.ID.ProjectileID.Gungnir" />, <see cref="F:Terraria.ID.ProjectileID.MushroomSpear" />, <see cref="F:Terraria.ID.ProjectileID.TheRottedFork" />, <see cref="F:Terraria.ID.ProjectileID.PalladiumPike" />, <see cref="F:Terraria.ID.ProjectileID.OrichalcumHalberd" />, <see cref="F:Terraria.ID.ProjectileID.TitaniumTrident" />, <see cref="F:Terraria.ID.ProjectileID.ChlorophytePartisan" />, <see cref="F:Terraria.ID.ProjectileID.NorthPoleWeapon" />, <see cref="F:Terraria.ID.ProjectileID.ObsidianSwordfish" />, <see cref="F:Terraria.ID.ProjectileID.Swordfish" />, <see cref="F:Terraria.ID.ProjectileID.ThunderSpear" />, <see cref="F:Terraria.ID.ProjectileID.JoustingLance" />, <see cref="F:Terraria.ID.ProjectileID.ShadowJoustingLance" />, <see cref="F:Terraria.ID.ProjectileID.HallowJoustingLance" />
	/// </summary>
	public const short Spear = 19;

	/// <summary>
	/// Behavior: Includes Saws<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.CobaltChainsaw" />, <see cref="F:Terraria.ID.ProjectileID.MythrilChainsaw" />, <see cref="F:Terraria.ID.ProjectileID.CobaltDrill" />, <see cref="F:Terraria.ID.ProjectileID.MythrilDrill" />, <see cref="F:Terraria.ID.ProjectileID.AdamantiteChainsaw" />, <see cref="F:Terraria.ID.ProjectileID.AdamantiteDrill" />, <see cref="F:Terraria.ID.ProjectileID.Hamdrax" />, <see cref="F:Terraria.ID.ProjectileID.PalladiumDrill" />, <see cref="F:Terraria.ID.ProjectileID.PalladiumChainsaw" />, <see cref="F:Terraria.ID.ProjectileID.OrichalcumDrill" />, <see cref="F:Terraria.ID.ProjectileID.OrichalcumChainsaw" />, <see cref="F:Terraria.ID.ProjectileID.TitaniumDrill" />, <see cref="F:Terraria.ID.ProjectileID.TitaniumChainsaw" />, <see cref="F:Terraria.ID.ProjectileID.ChlorophyteDrill" />, <see cref="F:Terraria.ID.ProjectileID.ChlorophyteChainsaw" />, <see cref="F:Terraria.ID.ProjectileID.ChlorophyteJackhammer" />, <see cref="F:Terraria.ID.ProjectileID.SawtoothShark" />, <see cref="F:Terraria.ID.ProjectileID.VortexChainsaw" />, <see cref="F:Terraria.ID.ProjectileID.VortexDrill" />, <see cref="F:Terraria.ID.ProjectileID.NebulaChainsaw" />, <see cref="F:Terraria.ID.ProjectileID.NebulaDrill" />, <see cref="F:Terraria.ID.ProjectileID.SolarFlareChainsaw" />, <see cref="F:Terraria.ID.ProjectileID.SolarFlareDrill" />, <see cref="F:Terraria.ID.ProjectileID.ButchersChainsaw" />, <see cref="F:Terraria.ID.ProjectileID.StardustDrill" />, <see cref="F:Terraria.ID.ProjectileID.StardustChainsaw" />
	/// </summary>
	public const short Drill = 20;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.QuarterNote" />, <see cref="F:Terraria.ID.ProjectileID.EighthNote" />, <see cref="F:Terraria.ID.ProjectileID.TiedEighthNote" />
	/// </summary>
	public const short MusicNote = 21;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.IceBlock" />
	/// </summary>
	public const short IceRod = 22;

	/// <summary>
	/// Behavior: Includes Flamethrower Flames, Cursed Flames, and Eye Fire<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Flames" />, <see cref="F:Terraria.ID.ProjectileID.EyeFire" />, <see cref="F:Terraria.ID.ProjectileID.FlamesTrap" />
	/// </summary>
	public const short Flames = 23;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.CrystalShard" />, <see cref="F:Terraria.ID.ProjectileID.CrystalStorm" />
	/// </summary>
	public const short CrystalShard = 24;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Boulder" />, <see cref="F:Terraria.ID.ProjectileID.BeeHive" />, <see cref="F:Terraria.ID.ProjectileID.RollingCactus" />
	/// </summary>
	public const short Boulder = 25;

	/// <summary>
	/// Behavior: Includes some minions with simple AI, such as the Baby Slime<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Bunny" />, <see cref="F:Terraria.ID.ProjectileID.Penguin" />, <see cref="F:Terraria.ID.ProjectileID.Turtle" />, <see cref="F:Terraria.ID.ProjectileID.BabyEater" />, <see cref="F:Terraria.ID.ProjectileID.Pygmy" />, <see cref="F:Terraria.ID.ProjectileID.Pygmy2" />, <see cref="F:Terraria.ID.ProjectileID.Pygmy3" />, <see cref="F:Terraria.ID.ProjectileID.Pygmy4" />, <see cref="F:Terraria.ID.ProjectileID.BabySkeletronHead" />, <see cref="F:Terraria.ID.ProjectileID.BabyHornet" />, <see cref="F:Terraria.ID.ProjectileID.TikiSpirit" />, <see cref="F:Terraria.ID.ProjectileID.PetLizard" />, <see cref="F:Terraria.ID.ProjectileID.Parrot" />, <see cref="F:Terraria.ID.ProjectileID.Truffle" />, <see cref="F:Terraria.ID.ProjectileID.Sapling" />, <see cref="F:Terraria.ID.ProjectileID.Wisp" />, <see cref="F:Terraria.ID.ProjectileID.BabyDino" />, <see cref="F:Terraria.ID.ProjectileID.BabySlime" />, <see cref="F:Terraria.ID.ProjectileID.EyeSpring" />, <see cref="F:Terraria.ID.ProjectileID.BabySnowman" />, <see cref="F:Terraria.ID.ProjectileID.Spider" />, <see cref="F:Terraria.ID.ProjectileID.Squashling" />, <see cref="F:Terraria.ID.ProjectileID.BlackCat" />, <see cref="F:Terraria.ID.ProjectileID.CursedSapling" />, <see cref="F:Terraria.ID.ProjectileID.Puppy" />, <see cref="F:Terraria.ID.ProjectileID.BabyGrinch" />, <see cref="F:Terraria.ID.ProjectileID.ZephyrFish" />, <see cref="F:Terraria.ID.ProjectileID.VenomSpider" />, <see cref="F:Terraria.ID.ProjectileID.JumperSpider" />, <see cref="F:Terraria.ID.ProjectileID.DangerousSpider" />, <see cref="F:Terraria.ID.ProjectileID.MiniMinotaur" />, <see cref="F:Terraria.ID.ProjectileID.BabyFaceMonster" />, <see cref="F:Terraria.ID.ProjectileID.SugarGlider" />, <see cref="F:Terraria.ID.ProjectileID.SharkPup" />, <see cref="F:Terraria.ID.ProjectileID.LilHarpy" />, <see cref="F:Terraria.ID.ProjectileID.FennecFox" />, <see cref="F:Terraria.ID.ProjectileID.GlitteryButterfly" />, <see cref="F:Terraria.ID.ProjectileID.BabyImp" />, <see cref="F:Terraria.ID.ProjectileID.BabyRedPanda" />, <see cref="F:Terraria.ID.ProjectileID.Plantero" />, <see cref="F:Terraria.ID.ProjectileID.DynamiteKitten" />, <see cref="F:Terraria.ID.ProjectileID.BabyWerewolf" />, <see cref="F:Terraria.ID.ProjectileID.ShadowMimic" />, <see cref="F:Terraria.ID.ProjectileID.VoltBunny" />, <see cref="F:Terraria.ID.ProjectileID.KingSlimePet" />, <see cref="F:Terraria.ID.ProjectileID.BrainOfCthulhuPet" />, <see cref="F:Terraria.ID.ProjectileID.SkeletronPet" />, <see cref="F:Terraria.ID.ProjectileID.QueenBeePet" />, <see cref="F:Terraria.ID.ProjectileID.SkeletronPrimePet" />, <see cref="F:Terraria.ID.ProjectileID.PlanteraPet" />, <see cref="F:Terraria.ID.ProjectileID.GolemPet" />, <see cref="F:Terraria.ID.ProjectileID.DukeFishronPet" />, <see cref="F:Terraria.ID.ProjectileID.MoonLordPet" />, <see cref="F:Terraria.ID.ProjectileID.EverscreamPet" />, <see cref="F:Terraria.ID.ProjectileID.MartianPet" />, <see cref="F:Terraria.ID.ProjectileID.DD2OgrePet" />, <see cref="F:Terraria.ID.ProjectileID.DD2BetsyPet" />, <see cref="F:Terraria.ID.ProjectileID.QueenSlimePet" />
	/// </summary>
	public const short Pet = 26;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.UnholyTridentFriendly" />, <see cref="F:Terraria.ID.ProjectileID.UnholyTridentHostile" />, <see cref="F:Terraria.ID.ProjectileID.SwordBeam" />, <see cref="F:Terraria.ID.ProjectileID.TerraBeam" />, <see cref="F:Terraria.ID.ProjectileID.LightBeam" />, <see cref="F:Terraria.ID.ProjectileID.NightBeam" />, <see cref="F:Terraria.ID.ProjectileID.EnchantedBeam" />
	/// </summary>
	public const short Beam = 27;

	/// <summary>
	/// Behavior: Includes Ice Sword, Frost Hydra, Frost Bolt, and Icy Spit projectiles<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.IceBolt" />, <see cref="F:Terraria.ID.ProjectileID.FrostBoltSword" />, <see cref="F:Terraria.ID.ProjectileID.FrostBlastHostile" />, <see cref="F:Terraria.ID.ProjectileID.RuneBlast" />, <see cref="F:Terraria.ID.ProjectileID.IcewaterSpit" />, <see cref="F:Terraria.ID.ProjectileID.FrostBlastFriendly" />, <see cref="F:Terraria.ID.ProjectileID.FrostBoltStaff" />
	/// </summary>
	public const short ColdBolt = 28;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.AmethystBolt" />, <see cref="F:Terraria.ID.ProjectileID.TopazBolt" />, <see cref="F:Terraria.ID.ProjectileID.SapphireBolt" />, <see cref="F:Terraria.ID.ProjectileID.EmeraldBolt" />, <see cref="F:Terraria.ID.ProjectileID.RubyBolt" />, <see cref="F:Terraria.ID.ProjectileID.DiamondBolt" />, <see cref="F:Terraria.ID.ProjectileID.CrystalPulse" />, <see cref="F:Terraria.ID.ProjectileID.CrystalPulse2" />, <see cref="F:Terraria.ID.ProjectileID.AmberBolt" />, <see cref="F:Terraria.ID.ProjectileID.NebulaArcanumExplosionShot" />, <see cref="F:Terraria.ID.ProjectileID.NebulaArcanumExplosionShotShard" />, <see cref="F:Terraria.ID.ProjectileID.ThunderStaffShot" />
	/// </summary>
	public const short GemStaffBolt = 29;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Mushroom" />, <see cref="F:Terraria.ID.ProjectileID.OrnamentFriendly" />, <see cref="F:Terraria.ID.ProjectileID.OrnamentStar" />
	/// </summary>
	public const short Mushroom = 30;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.PureSpray" />, <see cref="F:Terraria.ID.ProjectileID.HallowSpray" />, <see cref="F:Terraria.ID.ProjectileID.CorruptSpray" />, <see cref="F:Terraria.ID.ProjectileID.MushroomSpray" />, <see cref="F:Terraria.ID.ProjectileID.CrimsonSpray" />
	/// </summary>
	public const short Spray = 31;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.BeachBall" />
	/// </summary>
	public const short BeachBall = 32;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Flare" />, <see cref="F:Terraria.ID.ProjectileID.BlueFlare" />
	/// </summary>
	public const short Flare = 33;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.RocketFireworkRed" />, <see cref="F:Terraria.ID.ProjectileID.RocketFireworkGreen" />, <see cref="F:Terraria.ID.ProjectileID.RocketFireworkBlue" />, <see cref="F:Terraria.ID.ProjectileID.RocketFireworkYellow" />, <see cref="F:Terraria.ID.ProjectileID.RocketFireworksBoxRed" />, <see cref="F:Terraria.ID.ProjectileID.RocketFireworksBoxGreen" />, <see cref="F:Terraria.ID.ProjectileID.RocketFireworksBoxBlue" />, <see cref="F:Terraria.ID.ProjectileID.RocketFireworksBoxYellow" />
	/// </summary>
	public const short FireWork = 34;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.RopeCoil" />, <see cref="F:Terraria.ID.ProjectileID.VineRopeCoil" />, <see cref="F:Terraria.ID.ProjectileID.SilkRopeCoil" />, <see cref="F:Terraria.ID.ProjectileID.WebRopeCoil" />
	/// </summary>
	public const short RopeCoil = 35;

	/// <summary>
	/// Behavior: Includes Bee, Wasp, Tiny Eater, and Bat projectiles<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Bee" />, <see cref="F:Terraria.ID.ProjectileID.Wasp" />, <see cref="F:Terraria.ID.ProjectileID.TinyEater" />, <see cref="F:Terraria.ID.ProjectileID.Bat" />, <see cref="F:Terraria.ID.ProjectileID.GiantBee" />
	/// </summary>
	public const short SmallFlying = 36;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.SpearTrap" />
	/// </summary>
	public const short SpearTrap = 37;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.FlamethrowerTrap" />
	/// </summary>
	public const short FlameThrower = 38;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.MechanicalPiranha" />
	/// </summary>
	public const short MechanicalPiranha = 39;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Leaf" />
	/// </summary>
	public const short Leaf = 40;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.FlowerPetal" />
	/// </summary>
	public const short FlowerPetal = 41;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.CrystalLeaf" />
	/// </summary>
	public const short CrystalLeaf = 42;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.CrystalLeafShot" />
	/// </summary>
	public const short CrystalLeafShot = 43;

	/// <summary>
	/// Behavior: Moves a short distance and then stops, includes Spore Cloud, Chlorophyte Orb, and Storm Spear Shot projectiles<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.SporeCloud" />, <see cref="F:Terraria.ID.ProjectileID.ChlorophyteOrb" />, <see cref="F:Terraria.ID.ProjectileID.ThunderSpearShot" />
	/// </summary>
	public const short MoveShort = 44;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.RainCloudMoving" />, <see cref="F:Terraria.ID.ProjectileID.RainCloudRaining" />, <see cref="F:Terraria.ID.ProjectileID.RainFriendly" />, <see cref="F:Terraria.ID.ProjectileID.BloodCloudMoving" />, <see cref="F:Terraria.ID.ProjectileID.BloodCloudRaining" />, <see cref="F:Terraria.ID.ProjectileID.BloodRain" />, <see cref="F:Terraria.ID.ProjectileID.RainNimbus" />
	/// </summary>
	public const short RainCloud = 45;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.RainbowFront" />, <see cref="F:Terraria.ID.ProjectileID.RainbowBack" />
	/// </summary>
	public const short Rainbow = 46;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.MagnetSphereBall" />
	/// </summary>
	public const short MagnetSphere = 47;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.MagnetSphereBolt" />, <see cref="F:Terraria.ID.ProjectileID.HeatRay" />, <see cref="F:Terraria.ID.ProjectileID.ShadowBeamHostile" />, <see cref="F:Terraria.ID.ProjectileID.ShadowBeamFriendly" />, <see cref="F:Terraria.ID.ProjectileID.UFOLaser" />
	/// </summary>
	public const short Ray = 48;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.ExplosiveBunny" />
	/// </summary>
	public const short ExplosiveBunny = 49;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.InfernoHostileBolt" />, <see cref="F:Terraria.ID.ProjectileID.InfernoHostileBlast" />, <see cref="F:Terraria.ID.ProjectileID.InfernoFriendlyBolt" />, <see cref="F:Terraria.ID.ProjectileID.InfernoFriendlyBlast" />
	/// </summary>
	public const short Inferno = 50;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.LostSoulHostile" />, <see cref="F:Terraria.ID.ProjectileID.LostSoulFriendly" />
	/// </summary>
	public const short LostSoul = 51;

	/// <summary>
	/// Behavior: Includes Spirit Heal from the Spectre Hood and Vampire Heal from the Vampire Knives<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.SpiritHeal" />, <see cref="F:Terraria.ID.ProjectileID.VampireHeal" />
	/// </summary>
	public const short Heal = 52;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.FrostHydra" />, <see cref="F:Terraria.ID.ProjectileID.SpiderHiver" />
	/// </summary>
	public const short FrostHydra = 53;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Raven" />
	/// </summary>
	public const short Raven = 54;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.FlamingJack" />
	/// </summary>
	public const short FlamingJack = 55;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.FlamingScythe" />
	/// </summary>
	public const short FlamingScythe = 56;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.NorthPoleSpear" />
	/// </summary>
	public const short NorthPoleSpear = 57;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Present" />
	/// </summary>
	public const short Present = 58;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.SpectreWrath" />
	/// </summary>
	public const short SpectreWrath = 59;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.WaterGun" />, <see cref="F:Terraria.ID.ProjectileID.SlimeGun" />
	/// </summary>
	public const short WaterJet = 60;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.BobberWooden" />, <see cref="F:Terraria.ID.ProjectileID.BobberReinforced" />, <see cref="F:Terraria.ID.ProjectileID.BobberFiberglass" />, <see cref="F:Terraria.ID.ProjectileID.BobberFisherOfSouls" />, <see cref="F:Terraria.ID.ProjectileID.BobberGolden" />, <see cref="F:Terraria.ID.ProjectileID.BobberMechanics" />, <see cref="F:Terraria.ID.ProjectileID.BobbersittingDuck" />, <see cref="F:Terraria.ID.ProjectileID.BobberFleshcatcher" />, <see cref="F:Terraria.ID.ProjectileID.BobberHotline" />, <see cref="F:Terraria.ID.ProjectileID.BobberBloody" />, <see cref="F:Terraria.ID.ProjectileID.BobberScarab" />
	/// </summary>
	public const short Bobber = 61;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Hornet" />, <see cref="F:Terraria.ID.ProjectileID.FlyingImp" />, <see cref="F:Terraria.ID.ProjectileID.Tempest" />, <see cref="F:Terraria.ID.ProjectileID.UFOMinion" />, <see cref="F:Terraria.ID.ProjectileID.StardustCellMinion" />
	/// </summary>
	public const short Hornet = 62;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.BabySpider" />
	/// </summary>
	public const short BabySpider = 63;

	/// <summary>
	/// Behavior: Includes Sharknado and Cthulunado projectiles<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Sharknado" />, <see cref="F:Terraria.ID.ProjectileID.Cthulunado" />
	/// </summary>
	public const short Nado = 64;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.SharknadoBolt" />
	/// </summary>
	public const short SharknadoBolt = 65;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Retanimini" />, <see cref="F:Terraria.ID.ProjectileID.Spazmamini" />, <see cref="F:Terraria.ID.ProjectileID.DeadlySphere" />
	/// </summary>
	public const short MiniTwins = 66;

	/// <summary>
	/// Behavior: Includes Mini Pirate, Crimson Heart, Companion Cube, Vampire Frog, and Desert Tiger projectiles<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.OneEyedPirate" />, <see cref="F:Terraria.ID.ProjectileID.SoulscourgePirate" />, <see cref="F:Terraria.ID.ProjectileID.PirateCaptain" />, <see cref="F:Terraria.ID.ProjectileID.CrimsonHeart" />, <see cref="F:Terraria.ID.ProjectileID.CompanionCube" />, <see cref="F:Terraria.ID.ProjectileID.VampireFrog" />, <see cref="F:Terraria.ID.ProjectileID.StormTigerTier1" />, <see cref="F:Terraria.ID.ProjectileID.StormTigerTier2" />, <see cref="F:Terraria.ID.ProjectileID.StormTigerTier3" />, <see cref="F:Terraria.ID.ProjectileID.FlinxMinion" />
	/// </summary>
	public const short CommonFollow = 67;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.MolotovCocktail" />, <see cref="F:Terraria.ID.ProjectileID.Ale" />
	/// </summary>
	public const short MolotovCocktail = 68;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Flairon" />
	/// </summary>
	public const short Flairon = 69;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.FlaironBubble" />
	/// </summary>
	public const short FlaironBubble = 70;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Typhoon" />
	/// </summary>
	public const short Typhoon = 71;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Bubble" />
	/// </summary>
	public const short Bubble = 72;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.FireworkFountainYellow" />, <see cref="F:Terraria.ID.ProjectileID.FireworkFountainRed" />, <see cref="F:Terraria.ID.ProjectileID.FireworkFountainBlue" />, <see cref="F:Terraria.ID.ProjectileID.FireworkFountainRainbow" />
	/// </summary>
	public const short FireWorkFountain = 73;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.ScutlixLaserFriendly" />
	/// </summary>
	public const short ScutlixLaser = 74;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.LaserMachinegun" />, <see cref="F:Terraria.ID.ProjectileID.LaserDrill" />, <see cref="F:Terraria.ID.ProjectileID.ChargedBlasterCannon" />, <see cref="F:Terraria.ID.ProjectileID.Arkhalis" />, <see cref="F:Terraria.ID.ProjectileID.PortalGun" />, <see cref="F:Terraria.ID.ProjectileID.SolarWhipSword" />, <see cref="F:Terraria.ID.ProjectileID.VortexBeater" />, <see cref="F:Terraria.ID.ProjectileID.Phantasm" />, <see cref="F:Terraria.ID.ProjectileID.LastPrism" />, <see cref="F:Terraria.ID.ProjectileID.DD2PhoenixBow" />, <see cref="F:Terraria.ID.ProjectileID.Celeb2Weapon" />, <see cref="F:Terraria.ID.ProjectileID.Terragrim" />, <see cref="F:Terraria.ID.ProjectileID.PiercingStarlight" />
	/// </summary>
	public const short HeldProjectile = 75;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.ScutlixLaserCrosshair" />, <see cref="F:Terraria.ID.ProjectileID.DrillMountCrosshair" />
	/// </summary>
	public const short Crosshair = 76;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Electrosphere" />
	/// </summary>
	public const short Electrosphere = 77;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Xenopopper" />
	/// </summary>
	public const short Xenopopper = 78;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.SaucerDeathray" />
	/// </summary>
	public const short MartianDeathRay = 79;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.SaucerMissile" />
	/// </summary>
	public const short MartianRocket = 80;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.InfluxWaver" />
	/// </summary>
	public const short InfluxWaver = 81;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.PhantasmalEye" />
	/// </summary>
	public const short PhantasmalEye = 82;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.PhantasmalSphere" />
	/// </summary>
	public const short PhantasmalSphere = 83;

	/// <summary>
	/// Behavior: Includes Charged Laser Blaster, Stardust Laser, Last Prism, and Lunar Portal Laser projectiles<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.PhantasmalDeathray" />, <see cref="F:Terraria.ID.ProjectileID.ChargedBlasterLaser" />, <see cref="F:Terraria.ID.ProjectileID.StardustSoldierLaser" />, <see cref="F:Terraria.ID.ProjectileID.LastPrismLaser" />, <see cref="F:Terraria.ID.ProjectileID.MoonlordTurretLaser" />
	/// </summary>
	public const short ThickLaser = 84;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.MoonLeech" />
	/// </summary>
	public const short MoonLeech = 85;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.CultistBossIceMist" />
	/// </summary>
	public const short IceMist = 86;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.ClingerStaff" />
	/// </summary>
	public const short CursedFlameWall = 87;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.CultistBossLightningOrb" />, <see cref="F:Terraria.ID.ProjectileID.CultistBossLightningOrbArc" />, <see cref="F:Terraria.ID.ProjectileID.VortexLightning" />
	/// </summary>
	public const short LightningOrb = 88;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.CultistRitual" />
	/// </summary>
	public const short LightningRitual = 89;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.MagicLantern" />
	/// </summary>
	public const short MagicLantern = 90;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.ShadowFlame" />
	/// </summary>
	public const short ShadowFlame = 91;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.ToxicCloud" />, <see cref="F:Terraria.ID.ProjectileID.ToxicCloud2" />, <see cref="F:Terraria.ID.ProjectileID.ToxicCloud3" />
	/// </summary>
	public const short ToxicCloud = 92;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.NailFriendly" />
	/// </summary>
	public const short Nail = 93;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.CoinPortal" />
	/// </summary>
	public const short CoinPortal = 94;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.ToxicBubble" />
	/// </summary>
	public const short ToxicBubble = 95;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.IchorSplash" />
	/// </summary>
	public const short IchorSplash = 96;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.FlyingPiggyBank" />
	/// </summary>
	public const short FlyingPiggyBank = 97;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.CultistBossParticle" />
	/// </summary>
	public const short MysteriousTablet = 98;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Code1" />, <see cref="F:Terraria.ID.ProjectileID.WoodYoyo" />, <see cref="F:Terraria.ID.ProjectileID.CorruptYoyo" />, <see cref="F:Terraria.ID.ProjectileID.CrimsonYoyo" />, <see cref="F:Terraria.ID.ProjectileID.JungleYoyo" />, <see cref="F:Terraria.ID.ProjectileID.Cascade" />, <see cref="F:Terraria.ID.ProjectileID.Chik" />, <see cref="F:Terraria.ID.ProjectileID.Code2" />, <see cref="F:Terraria.ID.ProjectileID.Rally" />, <see cref="F:Terraria.ID.ProjectileID.Yelets" />, <see cref="F:Terraria.ID.ProjectileID.RedsYoyo" />, <see cref="F:Terraria.ID.ProjectileID.ValkyrieYoyo" />, <see cref="F:Terraria.ID.ProjectileID.Amarok" />, <see cref="F:Terraria.ID.ProjectileID.HelFire" />, <see cref="F:Terraria.ID.ProjectileID.Kraken" />, <see cref="F:Terraria.ID.ProjectileID.TheEyeOfCthulhu" />, <see cref="F:Terraria.ID.ProjectileID.BlackCounterweight" />, <see cref="F:Terraria.ID.ProjectileID.BlueCounterweight" />, <see cref="F:Terraria.ID.ProjectileID.GreenCounterweight" />, <see cref="F:Terraria.ID.ProjectileID.PurpleCounterweight" />, <see cref="F:Terraria.ID.ProjectileID.RedCounterweight" />, <see cref="F:Terraria.ID.ProjectileID.YellowCounterweight" />, <see cref="F:Terraria.ID.ProjectileID.FormatC" />, <see cref="F:Terraria.ID.ProjectileID.Gradient" />, <see cref="F:Terraria.ID.ProjectileID.Valor" />, <see cref="F:Terraria.ID.ProjectileID.Terrarian" />
	/// </summary>
	public const short Yoyo = 99;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.MedusaHead" />
	/// </summary>
	public const short MedusaRay = 100;

	/// <summary>
	/// Behavior: Includes Medusa Head Ray and Mechanical Cart Laser projectiles<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.MedusaHeadRay" />, <see cref="F:Terraria.ID.ProjectileID.MinecartMechLaser" />
	/// </summary>
	public const short HorizontalRay = 101;

	/// <summary>
	/// Behavior: Includes Flow Invader, Nebular Piercer, and Nebula Eye projectiles<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.StardustJellyfishSmall" />, <see cref="F:Terraria.ID.ProjectileID.NebulaBolt" />, <see cref="F:Terraria.ID.ProjectileID.NebulaEye" />
	/// </summary>
	public const short LunarProjectile = 102;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.StardustTowerMark" />
	/// </summary>
	public const short Starmark = 103;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.BrainOfConfusion" />
	/// </summary>
	public const short BrainofConfusion = 104;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.SporeTrap" />, <see cref="F:Terraria.ID.ProjectileID.SporeTrap2" />
	/// </summary>
	public const short SporeTrap = 105;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.SporeGas" />, <see cref="F:Terraria.ID.ProjectileID.SporeGas2" />, <see cref="F:Terraria.ID.ProjectileID.SporeGas3" />
	/// </summary>
	public const short SporeGas = 106;

	/// <summary>
	/// Behavior: Includes Desert Sprit's Curse<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.NebulaSphere" />, <see cref="F:Terraria.ID.ProjectileID.DesertDjinnCurse" />
	/// </summary>
	public const short NebulaSphere = 107;

	/// <summary>
	/// Behavior: Includes Blood Tears<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.VortexVortexLightning" />, <see cref="F:Terraria.ID.ProjectileID.VortexVortexPortal" />, <see cref="F:Terraria.ID.ProjectileID.BloodNautilusTears" />
	/// </summary>
	public const short Vortex = 108;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.MechanicWrench" />
	/// </summary>
	public const short MechanicWrench = 109;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.NurseSyringeHeal" />
	/// </summary>
	public const short NurseSyringe = 110;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.DryadsWardCircle" />
	/// </summary>
	public const short DryadWard = 111;

	/// <summary>
	/// Behavior: Includes Truffle Spore, Rainbow Crystal Explosion, and Dandelion Seed projectiles<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.TruffleSpore" />, <see cref="F:Terraria.ID.ProjectileID.RainbowCrystalExplosion" />, <see cref="F:Terraria.ID.ProjectileID.DandelionSeed" />
	/// </summary>
	public const short SmallProximityExplosion = 112;

	/// <summary>
	/// Behavior: Includes Bone Javelin, Stardust Cell Shot, and Daybreak projectiles<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.BoneJavelin" />, <see cref="F:Terraria.ID.ProjectileID.StardustCellMinionShot" />, <see cref="F:Terraria.ID.ProjectileID.Daybreak" />
	/// </summary>
	public const short StickProjectile = 113;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.PortalGunGate" />
	/// </summary>
	public const short PortalGate = 114;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.TerrarianBeam" />
	/// </summary>
	public const short TerrarianBeam = 115;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.SolarFlareRay" />
	/// </summary>
	public const short DrakomiteFlare = 116;

	/// <summary>
	/// Behavior: Includes Solar Radience and Solar Eruption Explosion projectiles<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.SolarCounter" />, <see cref="F:Terraria.ID.ProjectileID.SolarWhipSwordExplosion" />, <see cref="F:Terraria.ID.ProjectileID.StardustGuardianExplosion" />, <see cref="F:Terraria.ID.ProjectileID.DaybreakExplosion" />
	/// </summary>
	public const short SolarEffect = 117;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.NebulaArcanum" />
	/// </summary>
	public const short NebulaArcanum = 118;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.NebulaArcanumSubshot" />
	/// </summary>
	public const short ArcanumSubShot = 119;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.StardustGuardian" />
	/// </summary>
	public const short StardustGuardian = 120;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.StardustDragon1" />, <see cref="F:Terraria.ID.ProjectileID.StardustDragon2" />, <see cref="F:Terraria.ID.ProjectileID.StardustDragon3" />, <see cref="F:Terraria.ID.ProjectileID.StardustDragon4" />
	/// </summary>
	public const short StardustDragon = 121;

	/// <summary>
	/// Behavior: The effect displayed when killing a Lunar Event enemy while it's respective Celestial Pillar is alive, also used by Phantasm Arrow<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.TowerDamageBolt" />, <see cref="F:Terraria.ID.ProjectileID.PhantasmArrow" />
	/// </summary>
	public const short ReleasedEnergy = 122;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.MoonlordTurret" />, <see cref="F:Terraria.ID.ProjectileID.RainbowCrystal" />
	/// </summary>
	public const short LunarSentry = 123;

	/// <summary>
	/// Behavior: Includes Suspicious Looking Tentacle, Suspicious Eye, Rez and Spaz, Fairy Princess, Jack 'O Lantern, and Ice Queen pets<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.SuspiciousTentacle" />, <see cref="F:Terraria.ID.ProjectileID.EyeOfCthulhuPet" />, <see cref="F:Terraria.ID.ProjectileID.TwinsPet" />, <see cref="F:Terraria.ID.ProjectileID.FairyQueenPet" />, <see cref="F:Terraria.ID.ProjectileID.PumpkingPet" />, <see cref="F:Terraria.ID.ProjectileID.IceQueenPet" />
	/// </summary>
	public const short FloatInFrontPet = 124;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.WireKite" />
	/// </summary>
	public const short WireKite = 125;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.GeyserTrap" />, <see cref="F:Terraria.ID.ProjectileID.DD2OgreStomp" />
	/// </summary>
	public const short Geyser = 126;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.SandnadoFriendly" />, <see cref="F:Terraria.ID.ProjectileID.SandnadoHostile" />
	/// </summary>
	public const short AncientStorm = 127;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.SandnadoHostileMark" />
	/// </summary>
	public const short AncientStormMark = 128;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.SpiritFlame" />
	/// </summary>
	public const short SpiritFlame = 129;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.DD2FlameBurstTowerT1" />, <see cref="F:Terraria.ID.ProjectileID.DD2FlameBurstTowerT2" />, <see cref="F:Terraria.ID.ProjectileID.DD2FlameBurstTowerT3" />
	/// </summary>
	public const short DD2FlameBurst = 130;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.DD2FlameBurstTowerT1Shot" />, <see cref="F:Terraria.ID.ProjectileID.DD2FlameBurstTowerT2Shot" />, <see cref="F:Terraria.ID.ProjectileID.DD2FlameBurstTowerT3Shot" />
	/// </summary>
	public const short DD2FlameBurstShot = 131;

	/// <summary>
	/// Behavior: Eternia Crystal destroyed<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.DD2ElderWins" />
	/// </summary>
	public const short DD2GrimEnd = 132;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.DD2DarkMageRaise" />, <see cref="F:Terraria.ID.ProjectileID.DD2DarkMageHeal" />
	/// </summary>
	public const short DD2DarkSigil = 133;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.DD2BallistraTowerT1" />, <see cref="F:Terraria.ID.ProjectileID.DD2BallistraTowerT2" />, <see cref="F:Terraria.ID.ProjectileID.DD2BallistraTowerT3" />
	/// </summary>
	public const short DD2Ballista = 134;

	/// <summary>
	/// Behavior: Includes Ogre's Stomp and Geyser projectiles<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.DD2OgreSmash" />, <see cref="F:Terraria.ID.ProjectileID.QueenSlimeSmash" />
	/// </summary>
	public const short UpwardExpand = 135;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.DD2BetsyFlameBreath" />
	/// </summary>
	public const short DD2BetsysBreath = 136;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.DD2LightningAuraT1" />, <see cref="F:Terraria.ID.ProjectileID.DD2LightningAuraT2" />, <see cref="F:Terraria.ID.ProjectileID.DD2LightningAuraT3" />
	/// </summary>
	public const short DD2LightningAura = 137;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.DD2ExplosiveTrapT1" />, <see cref="F:Terraria.ID.ProjectileID.DD2ExplosiveTrapT2" />, <see cref="F:Terraria.ID.ProjectileID.DD2ExplosiveTrapT3" />
	/// </summary>
	public const short DD2ExplosiveTrap = 138;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.DD2ExplosiveTrapT1Explosion" />, <see cref="F:Terraria.ID.ProjectileID.DD2ExplosiveTrapT2Explosion" />, <see cref="F:Terraria.ID.ProjectileID.DD2ExplosiveTrapT3Explosion" />
	/// </summary>
	public const short DD2ExplosiveTrapExplosion = 139;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.MonkStaffT1" />, <see cref="F:Terraria.ID.ProjectileID.MonkStaffT3" />
	/// </summary>
	public const short SleepyOctopod = 140;

	/// <summary>
	/// Behavior: The effect created on use of the Sleepy Octopod<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.MonkStaffT1Explosion" />
	/// </summary>
	public const short PoleSmash = 141;

	/// <summary>
	/// Behavior: Use style of the Ghastly Glaive and Sky Dragon's Fury alt1<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.MonkStaffT2" />, <see cref="F:Terraria.ID.ProjectileID.MonkStaffT3_Alt" />
	/// </summary>
	public const short ForwardStab = 142;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.MonkStaffT2Ghast" />
	/// </summary>
	public const short Ghast = 143;

	/// <summary>
	/// Behavior: Includes the Hoardragon, Flickerwick, Estee, and Propeller Gato<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.DD2PetDragon" />, <see cref="F:Terraria.ID.ProjectileID.DD2PetGhost" />, <see cref="F:Terraria.ID.ProjectileID.DD2PetGato" />, <see cref="F:Terraria.ID.ProjectileID.UpbeatStar" />
	/// </summary>
	public const short FloatBehindPet = 144;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.DD2ApprenticeStorm" />
	/// </summary>
	public const short WisdomWhirlwind = 145;

	/// <summary>
	/// Behavior: Old One's Army defeated<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.DD2Win" />
	/// </summary>
	public const short DD2Victory = 146;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Celeb2Rocket" />, <see cref="F:Terraria.ID.ProjectileID.Celeb2RocketExplosive" />, <see cref="F:Terraria.ID.ProjectileID.Celeb2RocketLarge" />, <see cref="F:Terraria.ID.ProjectileID.Celeb2RocketExplosiveLarge" />
	/// </summary>
	public const short CelebrationMk2Shots = 147;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.FallingStarSpawner" />
	/// </summary>
	public const short FallingStarAnimation = 148;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.DirtGolfBall" />, <see cref="F:Terraria.ID.ProjectileID.GolfBallDyedBlack" />, <see cref="F:Terraria.ID.ProjectileID.GolfBallDyedBlue" />, <see cref="F:Terraria.ID.ProjectileID.GolfBallDyedBrown" />, <see cref="F:Terraria.ID.ProjectileID.GolfBallDyedCyan" />, <see cref="F:Terraria.ID.ProjectileID.GolfBallDyedGreen" />, <see cref="F:Terraria.ID.ProjectileID.GolfBallDyedLimeGreen" />, <see cref="F:Terraria.ID.ProjectileID.GolfBallDyedOrange" />, <see cref="F:Terraria.ID.ProjectileID.GolfBallDyedPink" />, <see cref="F:Terraria.ID.ProjectileID.GolfBallDyedPurple" />, <see cref="F:Terraria.ID.ProjectileID.GolfBallDyedRed" />, <see cref="F:Terraria.ID.ProjectileID.GolfBallDyedSkyBlue" />, <see cref="F:Terraria.ID.ProjectileID.GolfBallDyedTeal" />, <see cref="F:Terraria.ID.ProjectileID.GolfBallDyedViolet" />, <see cref="F:Terraria.ID.ProjectileID.GolfBallDyedYellow" />
	/// </summary>
	public const short GolfBall = 149;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.GolfClubHelper" />
	/// </summary>
	public const short GolfClub = 150;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.SuperStar" />
	/// </summary>
	public const short SuperStar = 151;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.SuperStarSlash" />
	/// </summary>
	public const short SuperStarBeam = 152;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.ToiletEffect" />
	/// </summary>
	public const short ToiletEffect = 153;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.VoidLens" />
	/// </summary>
	public const short VoidBag = 154;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.MysticSnakeCoil" />
	/// </summary>
	public const short SnakeCoil = 155;

	/// <summary>
	/// Behavior: Includes the Sanguine Bat<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.BatOfLight" />, <see cref="F:Terraria.ID.ProjectileID.EmpressBlade" />
	/// </summary>
	public const short Terraprisma = 156;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.SharpTears" />
	/// </summary>
	public const short BloodThorn = 157;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.BabyBird" />
	/// </summary>
	public const short Finch = 158;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.PaperAirplaneA" />, <see cref="F:Terraria.ID.ProjectileID.PaperAirplaneB" />
	/// </summary>
	public const short PaperPlane = 159;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.KiteBlue" />, <see cref="F:Terraria.ID.ProjectileID.KiteBlueAndYellow" />, <see cref="F:Terraria.ID.ProjectileID.KiteRed" />, <see cref="F:Terraria.ID.ProjectileID.KiteRedAndYellow" />, <see cref="F:Terraria.ID.ProjectileID.KiteYellow" />, <see cref="F:Terraria.ID.ProjectileID.KiteWyvern" />, <see cref="F:Terraria.ID.ProjectileID.KiteBoneSerpent" />, <see cref="F:Terraria.ID.ProjectileID.KiteWorldFeeder" />, <see cref="F:Terraria.ID.ProjectileID.KiteBunny" />, <see cref="F:Terraria.ID.ProjectileID.KitePigron" />, <see cref="F:Terraria.ID.ProjectileID.KiteManEater" />, <see cref="F:Terraria.ID.ProjectileID.KiteJellyfishBlue" />, <see cref="F:Terraria.ID.ProjectileID.KiteJellyfishPink" />, <see cref="F:Terraria.ID.ProjectileID.KiteShark" />, <see cref="F:Terraria.ID.ProjectileID.KiteSandShark" />, <see cref="F:Terraria.ID.ProjectileID.KiteBunnyCorrupt" />, <see cref="F:Terraria.ID.ProjectileID.KiteBunnyCrimson" />, <see cref="F:Terraria.ID.ProjectileID.KiteGoldfish" />, <see cref="F:Terraria.ID.ProjectileID.KiteAngryTrapper" />, <see cref="F:Terraria.ID.ProjectileID.KiteKoi" />, <see cref="F:Terraria.ID.ProjectileID.KiteCrawltipede" />, <see cref="F:Terraria.ID.ProjectileID.KiteSpectrum" />, <see cref="F:Terraria.ID.ProjectileID.KiteWanderingEye" />, <see cref="F:Terraria.ID.ProjectileID.KiteUnicorn" />
	/// </summary>
	public const short Kite = 160;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.GladiusStab" />, <see cref="F:Terraria.ID.ProjectileID.RulerStab" />, <see cref="F:Terraria.ID.ProjectileID.CopperShortswordStab" />, <see cref="F:Terraria.ID.ProjectileID.TinShortswordStab" />, <see cref="F:Terraria.ID.ProjectileID.IronShortswordStab" />, <see cref="F:Terraria.ID.ProjectileID.LeadShortswordStab" />, <see cref="F:Terraria.ID.ProjectileID.SilverShortswordStab" />, <see cref="F:Terraria.ID.ProjectileID.TungstenShortswordStab" />, <see cref="F:Terraria.ID.ProjectileID.GoldShortswordStab" />, <see cref="F:Terraria.ID.ProjectileID.PlatinumShortswordStab" />
	/// </summary>
	public const short ShortSword = 161;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.WhiteTigerPounce" />
	/// </summary>
	public const short DesertTiger = 162;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.ChumBucket" />
	/// </summary>
	public const short Chum = 163;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.StormTigerGem" />
	/// </summary>
	public const short DesertTigerBall = 164;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.BlandWhip" />, <see cref="F:Terraria.ID.ProjectileID.SwordWhip" />, <see cref="F:Terraria.ID.ProjectileID.MaceWhip" />, <see cref="F:Terraria.ID.ProjectileID.ScytheWhip" />, <see cref="F:Terraria.ID.ProjectileID.CoolWhip" />, <see cref="F:Terraria.ID.ProjectileID.FireWhip" />, <see cref="F:Terraria.ID.ProjectileID.ThornWhip" />, <see cref="F:Terraria.ID.ProjectileID.RainbowWhip" />, <see cref="F:Terraria.ID.ProjectileID.BoneWhip" />
	/// </summary>
	public const short Whip = 165;

	/// <summary>
	/// Behavior: Includes Dove and Lantern projectiles<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.ReleaseDoves" />, <see cref="F:Terraria.ID.ProjectileID.ReleaseLantern" />
	/// </summary>
	public const short ReleasedProjectile = 166;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.SparkleGuitar" />
	/// </summary>
	public const short StellarTune = 167;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.FirstFractal" />
	/// </summary>
	public const short FirstFractal = 168;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Smolstar" />
	/// </summary>
	public const short EnchantedDagger = 169;

	/// <summary>
	/// Behavior: Only used when the Fairy GlowSticks's ai[1] is greater than 0<br />
	/// Used by: None
	/// </summary>
	public const short FairyGlowStick = 170;

	/// <summary>
	/// Behavior: Includes the Prismatic Bolt and Nightglow projectiles, these float for a second and then fly toward their target<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.HallowBossRainbowStreak" />, <see cref="F:Terraria.ID.ProjectileID.FairyQueenMagicItemShot" />
	/// </summary>
	public const short FloatAndFly = 171;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.HallowBossSplitShotCore" />
	/// </summary>
	public const short SplitShotCore = 172;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.HallowBossLastingRainbow" />
	/// </summary>
	public const short EverlastingRainbow = 173;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.EaterOfWorldsPet" />, <see cref="F:Terraria.ID.ProjectileID.DestroyerPet" />, <see cref="F:Terraria.ID.ProjectileID.LunaticCultistPet" />
	/// </summary>
	public const short WormPet = 174;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.TitaniumStormShard" />
	/// </summary>
	public const short TitaniumShard = 175;

	/// <summary>
	/// Behavior: The effect displayed when an enemy is hit with the Dark Harvest whip<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.ScytheWhipProj" />
	/// </summary>
	public const short Reaping = 176;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.CoolWhipProj" />
	/// </summary>
	public const short CoolFlake = 177;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.FireWhipProj" />
	/// </summary>
	public const short FireCracker = 178;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.FairyQueenLance" />
	/// </summary>
	public const short EtherealLance = 179;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.FairyQueenSunDance" />
	/// </summary>
	public const short SunDance = 180;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.FairyQueenRangedItemShot" />
	/// </summary>
	public const short TwilightLance = 181;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.FinalFractal" />
	/// </summary>
	public const short Zenith = 182;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.ZoologistStrikeGreen" />, <see cref="F:Terraria.ID.ProjectileID.ZoologistStrikeRed" />
	/// </summary>
	public const short ZoologistStike = 183;

	/// <summary>
	/// Behavior: The Torch God event, not the projectiles fired out of the torches<br />
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.TorchGod" />
	/// </summary>
	public const short TorchGod = 184;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.SoulDrain" />
	/// </summary>
	public const short LifeDrain = 185;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.PrincessWeapon" />
	/// </summary>
	public const short PrincessWeapon = 186;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.InsanityShadowFriendly" />, <see cref="F:Terraria.ID.ProjectileID.InsanityShadowHostile" />
	/// </summary>
	public const short ShadowHand = 187;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.LightsBane" />
	/// </summary>
	public const short LightsBane = 188;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Volcano" />
	/// </summary>
	public const short Volcano = 189;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.NightsEdge" />,<see cref="F:Terraria.ID.ProjectileID.Excalibur" />,<see cref="F:Terraria.ID.ProjectileID.TrueExcalibur" />,<see cref="F:Terraria.ID.ProjectileID.TerraBlade2" />,<see cref="F:Terraria.ID.ProjectileID.TheHorsemansBlade" />
	/// </summary>
	public const short NightsEdge = 190;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.TrueNightsEdge" />, <see cref="F:Terraria.ID.ProjectileID.TerraBlade2Shot" />
	/// </summary>
	public const short TrueNightsEdge = 191;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.JuminoStardropAnimation" />
	/// </summary>
	public const short JuminoAnimation = 192;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.Flames" />
	/// </summary>
	public const short Flamethrower = 193;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.HorsemanPumpkin" />
	/// </summary>
	public const short HorsemanPumpkin = 194;

	/// <summary>
	/// Used by: <see cref="F:Terraria.ID.ProjectileID.JimsDrone" />
	/// </summary>
	public const short JimsDrone = 195;
}
