using System;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameContent.NetModules;
using Terraria.Graphics.Renderers;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Net;

namespace Terraria.GameContent.Drawing;

public class ParticleOrchestrator
{
	private static ParticlePool<FadingParticle> _poolFading = new ParticlePool<FadingParticle>(200, GetNewFadingParticle);

	private static ParticlePool<LittleFlyingCritterParticle> _poolFlies = new ParticlePool<LittleFlyingCritterParticle>(200, GetNewPooFlyParticle);

	private static ParticlePool<ItemTransferParticle> _poolItemTransfer = new ParticlePool<ItemTransferParticle>(100, GetNewItemTransferParticle);

	private static ParticlePool<FlameParticle> _poolFlame = new ParticlePool<FlameParticle>(200, GetNewFlameParticle);

	private static ParticlePool<RandomizedFrameParticle> _poolRandomizedFrame = new ParticlePool<RandomizedFrameParticle>(200, GetNewRandomizedFrameParticle);

	private static ParticlePool<PrettySparkleParticle> _poolPrettySparkle = new ParticlePool<PrettySparkleParticle>(200, GetNewPrettySparkleParticle);

	private static ParticlePool<GasParticle> _poolGas = new ParticlePool<GasParticle>(200, GetNewGasParticle);

	public static void RequestParticleSpawn(bool clientOnly, ParticleOrchestraType type, ParticleOrchestraSettings settings, int? overrideInvokingPlayerIndex = null)
	{
		settings.IndexOfPlayerWhoInvokedThis = (byte)Main.myPlayer;
		if (overrideInvokingPlayerIndex.HasValue)
		{
			settings.IndexOfPlayerWhoInvokedThis = (byte)overrideInvokingPlayerIndex.Value;
		}
		if (clientOnly)
		{
			SpawnParticlesDirect(type, settings);
		}
		else
		{
			NetManager.Instance.SendToServerAndSelf(NetParticlesModule.Serialize(type, settings));
		}
	}

	public static void BroadcastParticleSpawn(ParticleOrchestraType type, ParticleOrchestraSettings settings)
	{
		settings.IndexOfPlayerWhoInvokedThis = (byte)Main.myPlayer;
		NetManager.Instance.BroadcastOrLoopback(NetParticlesModule.Serialize(type, settings));
	}

	public static void BroadcastOrRequestParticleSpawn(ParticleOrchestraType type, ParticleOrchestraSettings settings)
	{
		settings.IndexOfPlayerWhoInvokedThis = (byte)Main.myPlayer;
		if (Main.netMode == 1)
		{
			NetManager.Instance.SendToServerAndSelf(NetParticlesModule.Serialize(type, settings));
		}
		else
		{
			NetManager.Instance.BroadcastOrLoopback(NetParticlesModule.Serialize(type, settings));
		}
	}

	private static FadingParticle GetNewFadingParticle()
	{
		return new FadingParticle();
	}

	private static LittleFlyingCritterParticle GetNewPooFlyParticle()
	{
		return new LittleFlyingCritterParticle();
	}

	private static ItemTransferParticle GetNewItemTransferParticle()
	{
		return new ItemTransferParticle();
	}

	private static FlameParticle GetNewFlameParticle()
	{
		return new FlameParticle();
	}

	private static RandomizedFrameParticle GetNewRandomizedFrameParticle()
	{
		return new RandomizedFrameParticle();
	}

	private static PrettySparkleParticle GetNewPrettySparkleParticle()
	{
		return new PrettySparkleParticle();
	}

	private static GasParticle GetNewGasParticle()
	{
		return new GasParticle();
	}

	public static void SpawnParticlesDirect(ParticleOrchestraType type, ParticleOrchestraSettings settings)
	{
		if (Main.netMode != 2)
		{
			switch (type)
			{
			case ParticleOrchestraType.Keybrand:
				Spawn_Keybrand(settings);
				break;
			case ParticleOrchestraType.FlameWaders:
				Spawn_FlameWaders(settings);
				break;
			case ParticleOrchestraType.StellarTune:
				Spawn_StellarTune(settings);
				break;
			case ParticleOrchestraType.WallOfFleshGoatMountFlames:
				Spawn_WallOfFleshGoatMountFlames(settings);
				break;
			case ParticleOrchestraType.BlackLightningHit:
				Spawn_BlackLightningHit(settings);
				break;
			case ParticleOrchestraType.RainbowRodHit:
				Spawn_RainbowRodHit(settings);
				break;
			case ParticleOrchestraType.BlackLightningSmall:
				Spawn_BlackLightningSmall(settings);
				break;
			case ParticleOrchestraType.StardustPunch:
				Spawn_StardustPunch(settings);
				break;
			case ParticleOrchestraType.PrincessWeapon:
				Spawn_PrincessWeapon(settings);
				break;
			case ParticleOrchestraType.PaladinsHammer:
				Spawn_PaladinsHammer(settings);
				break;
			case ParticleOrchestraType.NightsEdge:
				Spawn_NightsEdge(settings);
				break;
			case ParticleOrchestraType.SilverBulletSparkle:
				Spawn_SilverBulletSparkle(settings);
				break;
			case ParticleOrchestraType.TrueNightsEdge:
				Spawn_TrueNightsEdge(settings);
				break;
			case ParticleOrchestraType.ChlorophyteLeafCrystalPassive:
				Spawn_LeafCrystalPassive(settings);
				break;
			case ParticleOrchestraType.ChlorophyteLeafCrystalShot:
				Spawn_LeafCrystalShot(settings);
				break;
			case ParticleOrchestraType.AshTreeShake:
				Spawn_AshTreeShake(settings);
				break;
			case ParticleOrchestraType.TerraBlade:
				Spawn_TerraBlade(settings);
				break;
			case ParticleOrchestraType.Excalibur:
				Spawn_Excalibur(settings);
				break;
			case ParticleOrchestraType.TrueExcalibur:
				Spawn_TrueExcalibur(settings);
				break;
			case ParticleOrchestraType.PetExchange:
				Spawn_PetExchange(settings);
				break;
			case ParticleOrchestraType.SlapHand:
				Spawn_SlapHand(settings);
				break;
			case ParticleOrchestraType.WaffleIron:
				Spawn_WaffleIron(settings);
				break;
			case ParticleOrchestraType.FlyMeal:
				Spawn_FlyMeal(settings);
				break;
			case ParticleOrchestraType.GasTrap:
				Spawn_GasTrap(settings);
				break;
			case ParticleOrchestraType.ItemTransfer:
				Spawn_ItemTransfer(settings);
				break;
			case ParticleOrchestraType.ShimmerArrow:
				Spawn_ShimmerArrow(settings);
				break;
			case ParticleOrchestraType.TownSlimeTransform:
				Spawn_TownSlimeTransform(settings);
				break;
			case ParticleOrchestraType.LoadoutChange:
				Spawn_LoadOutChange(settings);
				break;
			case ParticleOrchestraType.ShimmerBlock:
				Spawn_ShimmerBlock(settings);
				break;
			case ParticleOrchestraType.Digestion:
				Spawn_Digestion(settings);
				break;
			case ParticleOrchestraType.PooFly:
				Spawn_PooFly(settings);
				break;
			case ParticleOrchestraType.ShimmerTownNPC:
				Spawn_ShimmerTownNPC(settings);
				break;
			case ParticleOrchestraType.ShimmerTownNPCSend:
				Spawn_ShimmerTownNPCSend(settings);
				break;
			}
		}
	}

	private static void Spawn_ShimmerTownNPCSend(ParticleOrchestraSettings settings)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		Rectangle rect = Utils.CenteredRectangle(settings.PositionInWorld, new Vector2(30f, 60f));
		for (float num = 0f; num < 20f; num += 1f)
		{
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			int num2 = Main.rand.Next(20, 40);
			prettySparkleParticle.ColorTint = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f, 0);
			prettySparkleParticle.LocalPosition = Main.rand.NextVector2FromRectangle(rect);
			prettySparkleParticle.Rotation = (float)Math.PI / 2f;
			prettySparkleParticle.Scale = new Vector2(1f + Main.rand.NextFloat() * 2f, 0.7f + Main.rand.NextFloat() * 0.7f);
			prettySparkleParticle.Velocity = new Vector2(0f, -1f);
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.TimeToLive = num2;
			prettySparkleParticle.FadeOutEnd = num2;
			prettySparkleParticle.FadeInEnd = num2 / 2;
			prettySparkleParticle.FadeOutStart = num2 / 2;
			prettySparkleParticle.AdditiveAmount = 0.35f;
			prettySparkleParticle.DrawVerticalAxis = false;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			PrettySparkleParticle prettySparkleParticle2 = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle2.ColorTint = new Color(255, 255, 255, 0);
			prettySparkleParticle2.LocalPosition = Main.rand.NextVector2FromRectangle(rect);
			prettySparkleParticle2.Rotation = (float)Math.PI / 2f;
			prettySparkleParticle2.Scale = prettySparkleParticle.Scale * 0.5f;
			prettySparkleParticle2.Velocity = new Vector2(0f, -1f);
			prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle2.TimeToLive = num2;
			prettySparkleParticle2.FadeOutEnd = num2;
			prettySparkleParticle2.FadeInEnd = num2 / 2;
			prettySparkleParticle2.FadeOutStart = num2 / 2;
			prettySparkleParticle2.AdditiveAmount = 1f;
			prettySparkleParticle2.DrawVerticalAxis = false;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
		}
	}

	private static void Spawn_ShimmerTownNPC(ParticleOrchestraSettings settings)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		Rectangle rectangle = Utils.CenteredRectangle(settings.PositionInWorld, new Vector2(30f, 60f));
		for (float num = 0f; num < 20f; num += 1f)
		{
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			int num2 = Main.rand.Next(20, 40);
			prettySparkleParticle.ColorTint = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f, 0);
			prettySparkleParticle.LocalPosition = Main.rand.NextVector2FromRectangle(rectangle);
			prettySparkleParticle.Rotation = (float)Math.PI / 2f;
			prettySparkleParticle.Scale = new Vector2(1f + Main.rand.NextFloat() * 2f, 0.7f + Main.rand.NextFloat() * 0.7f);
			prettySparkleParticle.Velocity = new Vector2(0f, -1f);
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.TimeToLive = num2;
			prettySparkleParticle.FadeOutEnd = num2;
			prettySparkleParticle.FadeInEnd = num2 / 2;
			prettySparkleParticle.FadeOutStart = num2 / 2;
			prettySparkleParticle.AdditiveAmount = 0.35f;
			prettySparkleParticle.DrawVerticalAxis = false;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			PrettySparkleParticle prettySparkleParticle2 = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle2.ColorTint = new Color(255, 255, 255, 0);
			prettySparkleParticle2.LocalPosition = Main.rand.NextVector2FromRectangle(rectangle);
			prettySparkleParticle2.Rotation = (float)Math.PI / 2f;
			prettySparkleParticle2.Scale = prettySparkleParticle.Scale * 0.5f;
			prettySparkleParticle2.Velocity = new Vector2(0f, -1f);
			prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle2.TimeToLive = num2;
			prettySparkleParticle2.FadeOutEnd = num2;
			prettySparkleParticle2.FadeInEnd = num2 / 2;
			prettySparkleParticle2.FadeOutStart = num2 / 2;
			prettySparkleParticle2.AdditiveAmount = 1f;
			prettySparkleParticle2.DrawVerticalAxis = false;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
		}
		for (int i = 0; i < 20; i++)
		{
			int num3 = Dust.NewDust(rectangle.TopLeft(), rectangle.Width, rectangle.Height, 308);
			Main.dust[num3].velocity.Y -= 8f;
			Main.dust[num3].velocity.X *= 0.5f;
			Main.dust[num3].scale = 0.8f;
			Main.dust[num3].noGravity = true;
			switch (Main.rand.Next(6))
			{
			case 0:
				Main.dust[num3].color = new Color(255, 255, 210);
				break;
			case 1:
				Main.dust[num3].color = new Color(190, 245, 255);
				break;
			case 2:
				Main.dust[num3].color = new Color(255, 150, 255);
				break;
			default:
				Main.dust[num3].color = new Color(190, 175, 255);
				break;
			}
		}
		SoundEngine.PlaySound(in SoundID.Item29, settings.PositionInWorld);
	}

	private static void Spawn_PooFly(ParticleOrchestraSettings settings)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		int num = _poolFlies.CountParticlesInUse();
		if (num <= 50 || !(Main.rand.NextFloat() >= Utils.Remap(num, 50f, 400f, 0.5f, 0f)))
		{
			LittleFlyingCritterParticle littleFlyingCritterParticle = _poolFlies.RequestParticle();
			littleFlyingCritterParticle.Prepare(settings.PositionInWorld, 300);
			Main.ParticleSystem_World_OverPlayers.Add(littleFlyingCritterParticle);
		}
	}

	private static void Spawn_Digestion(ParticleOrchestraSettings settings)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		Vector2 positionInWorld = settings.PositionInWorld;
		int num = ((settings.MovementVector.X < 0f) ? 1 : (-1));
		int num2 = Main.rand.Next(4);
		for (int i = 0; i < 3 + num2; i++)
		{
			int num3 = Dust.NewDust(positionInWorld + Vector2.UnitX * (float)(-num) * 8f - Vector2.One * 5f + Vector2.UnitY * 8f, 3, 6, 216, -num, 1f);
			Dust obj = Main.dust[num3];
			obj.velocity /= 2f;
			Main.dust[num3].scale = 0.8f;
		}
		if (Main.rand.Next(30) == 0)
		{
			int num4 = Gore.NewGore(positionInWorld + Vector2.UnitX * (float)(-num) * 8f, Vector2.Zero, Main.rand.Next(580, 583));
			Gore obj2 = Main.gore[num4];
			obj2.velocity /= 2f;
			Main.gore[num4].velocity.Y = Math.Abs(Main.gore[num4].velocity.Y);
			Main.gore[num4].velocity.X = (0f - Math.Abs(Main.gore[num4].velocity.X)) * (float)num;
		}
		SoundEngine.PlaySound(in SoundID.Item16, settings.PositionInWorld);
	}

	private static void Spawn_ShimmerBlock(ParticleOrchestraSettings settings)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		FadingParticle fadingParticle = _poolFading.RequestParticle();
		fadingParticle.SetBasicInfo(TextureAssets.Star[0], null, settings.MovementVector, settings.PositionInWorld);
		float num = 45f;
		fadingParticle.SetTypeInfo(num);
		fadingParticle.AccelerationPerFrame = settings.MovementVector / num;
		fadingParticle.ColorTint = Main.hslToRgb(Main.rand.NextFloat(), 0.75f, 0.8f);
		((Color)(ref fadingParticle.ColorTint)).A = 30;
		fadingParticle.FadeInNormalizedTime = 0.5f;
		fadingParticle.FadeOutNormalizedTime = 0.5f;
		fadingParticle.Rotation = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		fadingParticle.Scale = Vector2.One * (0.5f + 0.5f * Main.rand.NextFloat());
		Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
	}

	private static void Spawn_LoadOutChange(ParticleOrchestraSettings settings)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		Player player = Main.player[settings.IndexOfPlayerWhoInvokedThis];
		if (player.active)
		{
			Rectangle hitbox = player.Hitbox;
			int num = 6;
			hitbox.Height -= num;
			if (player.gravDir == 1f)
			{
				hitbox.Y += num;
			}
			for (int i = 0; i < 40; i++)
			{
				Dust dust = Dust.NewDustPerfect(Main.rand.NextVector2FromRectangle(hitbox), 16, null, 120, default(Color), Main.rand.NextFloat() * 0.8f + 0.8f);
				dust.velocity = Utils.RotatedBy(new Vector2(0f, (float)(-hitbox.Height) * Main.rand.NextFloat() * 0.04f), Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.1f);
				dust.velocity += player.velocity * 2f * Main.rand.NextFloat();
				dust.noGravity = true;
				dust.noLight = (dust.noLightEmittence = true);
			}
			for (int j = 0; j < 5; j++)
			{
				Dust dust2 = Dust.NewDustPerfect(Main.rand.NextVector2FromRectangle(hitbox), 43, null, 254, Main.hslToRgb(Main.rand.NextFloat(), 0.3f, 0.8f), Main.rand.NextFloat() * 0.8f + 0.8f);
				dust2.velocity = Utils.RotatedBy(new Vector2(0f, (float)(-hitbox.Height) * Main.rand.NextFloat() * 0.04f), Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.1f);
				dust2.velocity += player.velocity * 2f * Main.rand.NextFloat();
				dust2.noGravity = true;
				dust2.noLight = (dust2.noLightEmittence = true);
			}
		}
	}

	private static void Spawn_TownSlimeTransform(ParticleOrchestraSettings settings)
	{
		switch (settings.UniqueInfoPiece)
		{
		case 0:
			NerdySlimeEffect(settings);
			break;
		case 1:
			CopperSlimeEffect(settings);
			break;
		case 2:
			ElderSlimeEffect(settings);
			break;
		}
	}

	private static void ElderSlimeEffect(ParticleOrchestraSettings settings)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 30; i++)
		{
			Dust dust = Dust.NewDustPerfect(settings.PositionInWorld + Main.rand.NextVector2Circular(20f, 20f), 43, (settings.MovementVector * 0.75f + Main.rand.NextVector2Circular(6f, 6f)) * Main.rand.NextFloat(), 26, Color.Lerp(Main.OurFavoriteColor, Color.White, Main.rand.NextFloat()), 1f + Main.rand.NextFloat() * 1.4f);
			dust.fadeIn = 1.5f;
			if (dust.velocity.Y > 0f && Main.rand.Next(2) == 0)
			{
				dust.velocity.Y *= -1f;
			}
			dust.noGravity = true;
		}
		for (int j = 0; j < 8; j++)
		{
			Gore gore = Gore.NewGoreDirect(settings.PositionInWorld + Utils.RandomVector2(Main.rand, -30f, 30f) * new Vector2(0.5f, 1f), Vector2.Zero, 61 + Main.rand.Next(3));
			gore.velocity *= 0.5f;
		}
	}

	private static void NerdySlimeEffect(ParticleOrchestraSettings settings)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		Color newColor = default(Color);
		((Color)(ref newColor))._002Ector(0, 80, 255, 100);
		for (int i = 0; i < 60; i++)
		{
			Dust.NewDustPerfect(settings.PositionInWorld, 4, (settings.MovementVector * 0.75f + Main.rand.NextVector2Circular(6f, 6f)) * Main.rand.NextFloat(), 175, newColor, 0.6f + Main.rand.NextFloat() * 1.4f);
		}
	}

	private static void CopperSlimeEffect(ParticleOrchestraSettings settings)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 40; i++)
		{
			Dust dust = Dust.NewDustPerfect(settings.PositionInWorld + Main.rand.NextVector2Circular(20f, 20f), 43, (settings.MovementVector * 0.75f + Main.rand.NextVector2Circular(6f, 6f)) * Main.rand.NextFloat(), 26, Color.Lerp(new Color(183, 88, 25), Color.White, Main.rand.NextFloat() * 0.5f), 1f + Main.rand.NextFloat() * 1.4f);
			dust.fadeIn = 1.5f;
			if (dust.velocity.Y > 0f && Main.rand.Next(2) == 0)
			{
				dust.velocity.Y *= -1f;
			}
			dust.noGravity = true;
		}
	}

	private static void Spawn_ShimmerArrow(ParticleOrchestraSettings settings)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		float num = 20f;
		for (int i = 0; i < 2; i++)
		{
			float num2 = (float)Math.PI * 2f * Main.rand.NextFloatDirection() * 0.05f;
			Color color = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f);
			((Color)(ref color)).A = (byte)(((Color)(ref color)).A / 2);
			Color value = color;
			((Color)(ref value)).A = byte.MaxValue;
			value = Color.Lerp(value, Color.White, 0.5f);
			for (float num3 = 0f; num3 < 4f; num3 += 1f)
			{
				PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
				Vector2 vector = ((float)Math.PI / 2f * num3 + num2).ToRotationVector2() * 4f;
				prettySparkleParticle.ColorTint = color;
				prettySparkleParticle.LocalPosition = settings.PositionInWorld;
				prettySparkleParticle.Rotation = vector.ToRotation();
				prettySparkleParticle.Scale = new Vector2((num3 % 2f == 0f) ? 2f : 4f, 0.5f) * 1.1f;
				prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle.TimeToLive = num;
				prettySparkleParticle.FadeOutEnd = num;
				prettySparkleParticle.FadeInEnd = num / 2f;
				prettySparkleParticle.FadeOutStart = num / 2f;
				prettySparkleParticle.AdditiveAmount = 0.35f;
				prettySparkleParticle.Velocity = -vector * 0.2f;
				prettySparkleParticle.DrawVerticalAxis = false;
				if (num3 % 2f == 1f)
				{
					prettySparkleParticle.Scale *= 0.9f;
					prettySparkleParticle.Velocity *= 0.9f;
				}
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			}
			for (float num4 = 0f; num4 < 4f; num4 += 1f)
			{
				PrettySparkleParticle prettySparkleParticle2 = _poolPrettySparkle.RequestParticle();
				Vector2 vector2 = ((float)Math.PI / 2f * num4 + num2).ToRotationVector2() * 4f;
				prettySparkleParticle2.ColorTint = value;
				prettySparkleParticle2.LocalPosition = settings.PositionInWorld;
				prettySparkleParticle2.Rotation = vector2.ToRotation();
				prettySparkleParticle2.Scale = new Vector2((num4 % 2f == 0f) ? 2f : 4f, 0.5f) * 0.7f;
				prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle2.TimeToLive = num;
				prettySparkleParticle2.FadeOutEnd = num;
				prettySparkleParticle2.FadeInEnd = num / 2f;
				prettySparkleParticle2.FadeOutStart = num / 2f;
				prettySparkleParticle2.Velocity = vector2 * 0.2f;
				prettySparkleParticle2.DrawVerticalAxis = false;
				if (num4 % 2f == 1f)
				{
					prettySparkleParticle2.Scale *= 1.2f;
					prettySparkleParticle2.Velocity *= 1.2f;
				}
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
				if (i == 0)
				{
					for (int j = 0; j < 1; j++)
					{
						Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, 306, vector2.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
						dust.noGravity = true;
						dust.scale = 1.4f;
						dust.fadeIn = 1.2f;
						dust.color = color;
						Dust dust2 = Dust.NewDustPerfect(settings.PositionInWorld, 306, -vector2.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
						dust2.noGravity = true;
						dust2.scale = 1.4f;
						dust2.fadeIn = 1.2f;
						dust2.color = color;
					}
				}
			}
		}
	}

	private static void Spawn_ItemTransfer(ParticleOrchestraSettings settings)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = settings.PositionInWorld + settings.MovementVector;
		Vector2 vector2 = Main.rand.NextVector2Circular(32f, 32f);
		Vector2 vector3 = settings.PositionInWorld + vector2;
		Vector2 vector4 = val - vector3;
		int uniqueInfoPiece = settings.UniqueInfoPiece;
		if (ContentSamples.ItemsByType.TryGetValue(uniqueInfoPiece, out var value) && !value.IsAir)
		{
			uniqueInfoPiece = value.type;
			int num = Main.rand.Next(60, 80);
			Chest.AskForChestToEatItem(vector3 + vector4 + new Vector2(-8f, -8f), num + 10);
			ItemTransferParticle itemTransferParticle = _poolItemTransfer.RequestParticle();
			itemTransferParticle.Prepare(uniqueInfoPiece, num, vector3, vector3 + vector4);
			Main.ParticleSystem_World_OverPlayers.Add(itemTransferParticle);
		}
	}

	private static void Spawn_PetExchange(ParticleOrchestraSettings settings)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		Vector2 positionInWorld = settings.PositionInWorld;
		for (int i = 0; i < 13; i++)
		{
			Gore gore = Gore.NewGoreDirect(positionInWorld + new Vector2(-20f, -20f) + Main.rand.NextVector2Circular(20f, 20f), Vector2.Zero, Main.rand.Next(61, 64), 1f + Main.rand.NextFloat() * 0.3f);
			gore.alpha = 100;
			gore.velocity = ((float)Math.PI * 2f * (float)Main.rand.Next()).ToRotationVector2() * Main.rand.NextFloat() + settings.MovementVector * 0.5f;
		}
	}

	private static void Spawn_TerraBlade(ParticleOrchestraSettings settings)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		float num = 30f;
		float num2 = settings.MovementVector.ToRotation() + (float)Math.PI / 2f;
		float x = 3f;
		for (float num3 = 0f; num3 < 4f; num3 += 1f)
		{
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			Vector2 vector = ((float)Math.PI / 2f * num3 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle.ColorTint = new Color(0.2f, 0.85f, 0.4f, 0.5f);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle.Rotation = vector.ToRotation();
			prettySparkleParticle.Scale = new Vector2(x, 0.5f) * 1.1f;
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.TimeToLive = num;
			prettySparkleParticle.FadeOutEnd = num;
			prettySparkleParticle.FadeInEnd = num / 2f;
			prettySparkleParticle.FadeOutStart = num / 2f;
			prettySparkleParticle.AdditiveAmount = 0.35f;
			prettySparkleParticle.Velocity = -vector * 0.2f;
			prettySparkleParticle.DrawVerticalAxis = false;
			if (num3 % 2f == 1f)
			{
				prettySparkleParticle.Scale *= 1.5f;
				prettySparkleParticle.Velocity *= 2f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		for (float num4 = -1f; num4 <= 1f; num4 += 2f)
		{
			PrettySparkleParticle prettySparkleParticle2 = _poolPrettySparkle.RequestParticle();
			_ = num2.ToRotationVector2() * 4f;
			Vector2 vector2 = ((float)Math.PI / 2f * num4 + num2).ToRotationVector2() * 2f;
			prettySparkleParticle2.ColorTint = new Color(0.4f, 1f, 0.4f, 0.5f);
			prettySparkleParticle2.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle2.Rotation = vector2.ToRotation();
			prettySparkleParticle2.Scale = new Vector2(x, 0.5f) * 1.1f;
			prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle2.TimeToLive = num;
			prettySparkleParticle2.FadeOutEnd = num;
			prettySparkleParticle2.FadeInEnd = num / 2f;
			prettySparkleParticle2.FadeOutStart = num / 2f;
			prettySparkleParticle2.AdditiveAmount = 0.35f;
			prettySparkleParticle2.Velocity = vector2.RotatedBy(1.5707963705062866) * 0.5f;
			prettySparkleParticle2.DrawVerticalAxis = false;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
		}
		for (float num5 = 0f; num5 < 4f; num5 += 1f)
		{
			PrettySparkleParticle prettySparkleParticle3 = _poolPrettySparkle.RequestParticle();
			Vector2 vector3 = ((float)Math.PI / 2f * num5 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle3.ColorTint = new Color(0.2f, 1f, 0.2f, 1f);
			prettySparkleParticle3.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle3.Rotation = vector3.ToRotation();
			prettySparkleParticle3.Scale = new Vector2(x, 0.5f) * 0.7f;
			prettySparkleParticle3.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle3.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle3.TimeToLive = num;
			prettySparkleParticle3.FadeOutEnd = num;
			prettySparkleParticle3.FadeInEnd = num / 2f;
			prettySparkleParticle3.FadeOutStart = num / 2f;
			prettySparkleParticle3.Velocity = vector3 * 0.2f;
			prettySparkleParticle3.DrawVerticalAxis = false;
			if (num5 % 2f == 1f)
			{
				prettySparkleParticle3.Scale *= 1.5f;
				prettySparkleParticle3.Velocity *= 2f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle3);
			for (int i = 0; i < 1; i++)
			{
				Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, 107, vector3.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
				dust.noGravity = true;
				dust.scale = 0.8f;
				Dust dust2 = Dust.NewDustPerfect(settings.PositionInWorld, 107, -vector3.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
				dust2.noGravity = true;
				dust2.scale = 1.4f;
			}
		}
	}

	private static void Spawn_Excalibur(ParticleOrchestraSettings settings)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		float num = 30f;
		float num2 = 0f;
		for (float num3 = 0f; num3 < 4f; num3 += 1f)
		{
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			Vector2 vector = ((float)Math.PI / 2f * num3 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle.ColorTint = new Color(0.9f, 0.85f, 0.4f, 0.5f);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle.Rotation = vector.ToRotation();
			prettySparkleParticle.Scale = new Vector2((num3 % 2f == 0f) ? 2f : 4f, 0.5f) * 1.1f;
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.TimeToLive = num;
			prettySparkleParticle.FadeOutEnd = num;
			prettySparkleParticle.FadeInEnd = num / 2f;
			prettySparkleParticle.FadeOutStart = num / 2f;
			prettySparkleParticle.AdditiveAmount = 0.35f;
			prettySparkleParticle.Velocity = -vector * 0.2f;
			prettySparkleParticle.DrawVerticalAxis = false;
			if (num3 % 2f == 1f)
			{
				prettySparkleParticle.Scale *= 1.5f;
				prettySparkleParticle.Velocity *= 1.5f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		for (float num4 = 0f; num4 < 4f; num4 += 1f)
		{
			PrettySparkleParticle prettySparkleParticle2 = _poolPrettySparkle.RequestParticle();
			Vector2 vector2 = ((float)Math.PI / 2f * num4 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle2.ColorTint = new Color(1f, 1f, 0.2f, 1f);
			prettySparkleParticle2.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle2.Rotation = vector2.ToRotation();
			prettySparkleParticle2.Scale = new Vector2((num4 % 2f == 0f) ? 2f : 4f, 0.5f) * 0.7f;
			prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle2.TimeToLive = num;
			prettySparkleParticle2.FadeOutEnd = num;
			prettySparkleParticle2.FadeInEnd = num / 2f;
			prettySparkleParticle2.FadeOutStart = num / 2f;
			prettySparkleParticle2.Velocity = vector2 * 0.2f;
			prettySparkleParticle2.DrawVerticalAxis = false;
			if (num4 % 2f == 1f)
			{
				prettySparkleParticle2.Scale *= 1.5f;
				prettySparkleParticle2.Velocity *= 1.5f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
			for (int i = 0; i < 1; i++)
			{
				Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, 169, vector2.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
				dust.noGravity = true;
				dust.scale = 1.4f;
				Dust dust2 = Dust.NewDustPerfect(settings.PositionInWorld, 169, -vector2.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
				dust2.noGravity = true;
				dust2.scale = 1.4f;
			}
		}
	}

	private static void Spawn_SlapHand(ParticleOrchestraSettings settings)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(in SoundID.Item175, settings.PositionInWorld);
	}

	private static void Spawn_WaffleIron(ParticleOrchestraSettings settings)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(in SoundID.Item178, settings.PositionInWorld);
	}

	private static void Spawn_FlyMeal(ParticleOrchestraSettings settings)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(in SoundID.Item16, settings.PositionInWorld);
	}

	private static void Spawn_GasTrap(ParticleOrchestraSettings settings)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(in SoundID.Item16, settings.PositionInWorld);
		Vector2 movementVector = settings.MovementVector;
		int num = 12;
		int num2 = 10;
		float num3 = 5f;
		float num4 = 2.5f;
		Color lightColorTint = default(Color);
		((Color)(ref lightColorTint))._002Ector(0.2f, 0.4f, 0.15f);
		Vector2 positionInWorld = settings.PositionInWorld;
		float num5 = (float)Math.PI / 20f;
		float num6 = (float)Math.PI / 15f;
		for (int i = 0; i < num; i++)
		{
			Vector2 spinninpoint = movementVector + Utils.RotatedBy(new Vector2(num3 + Main.rand.NextFloat() * 1f, 0f), (float)i / (float)num * ((float)Math.PI * 2f), Vector2.Zero);
			spinninpoint = spinninpoint.RotatedByRandom(num5);
			GasParticle gasParticle = _poolGas.RequestParticle();
			gasParticle.AccelerationPerFrame = Vector2.Zero;
			gasParticle.Velocity = spinninpoint;
			gasParticle.ColorTint = Color.White;
			gasParticle.LightColorTint = lightColorTint;
			gasParticle.LocalPosition = positionInWorld + spinninpoint;
			gasParticle.TimeToLive = 50 + Main.rand.Next(20);
			gasParticle.InitialScale = 1f + Main.rand.NextFloat() * 0.35f;
			Main.ParticleSystem_World_BehindPlayers.Add(gasParticle);
		}
		for (int j = 0; j < num2; j++)
		{
			Vector2 spinninpoint2 = Utils.RotatedBy(new Vector2(num4 + Main.rand.NextFloat() * 1.45f, 0f), (float)j / (float)num2 * ((float)Math.PI * 2f), Vector2.Zero);
			spinninpoint2 = spinninpoint2.RotatedByRandom(num6);
			if (j % 2 == 0)
			{
				spinninpoint2 *= 0.5f;
			}
			GasParticle gasParticle2 = _poolGas.RequestParticle();
			gasParticle2.AccelerationPerFrame = Vector2.Zero;
			gasParticle2.Velocity = spinninpoint2;
			gasParticle2.ColorTint = Color.White;
			gasParticle2.LightColorTint = lightColorTint;
			gasParticle2.LocalPosition = positionInWorld;
			gasParticle2.TimeToLive = 80 + Main.rand.Next(30);
			gasParticle2.InitialScale = 1f + Main.rand.NextFloat() * 0.5f;
			Main.ParticleSystem_World_BehindPlayers.Add(gasParticle2);
		}
	}

	private static void Spawn_TrueExcalibur(ParticleOrchestraSettings settings)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0511: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_056b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_0613: Unknown result type (might be due to invalid IL or missing references)
		//IL_0622: Unknown result type (might be due to invalid IL or missing references)
		//IL_062f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_0653: Unknown result type (might be due to invalid IL or missing references)
		//IL_065d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0680: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_0694: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
		float num = 36f;
		float num2 = (float)Math.PI / 4f;
		for (float num3 = 0f; num3 < 2f; num3 += 1f)
		{
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			Vector2 v = ((float)Math.PI / 2f * num3 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle.ColorTint = new Color(1f, 0f, 0.3f, 1f);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle.Rotation = v.ToRotation();
			prettySparkleParticle.Scale = new Vector2(5f, 0.5f) * 1.1f;
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.TimeToLive = num;
			prettySparkleParticle.FadeOutEnd = num;
			prettySparkleParticle.FadeInEnd = num / 2f;
			prettySparkleParticle.FadeOutStart = num / 2f;
			prettySparkleParticle.AdditiveAmount = 0.35f;
			prettySparkleParticle.DrawVerticalAxis = false;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		for (float num4 = 0f; num4 < 2f; num4 += 1f)
		{
			PrettySparkleParticle prettySparkleParticle2 = _poolPrettySparkle.RequestParticle();
			Vector2 vector = ((float)Math.PI / 2f * num4 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle2.ColorTint = new Color(1f, 0.5f, 0.8f, 1f);
			prettySparkleParticle2.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle2.Rotation = vector.ToRotation();
			prettySparkleParticle2.Scale = new Vector2(3f, 0.5f) * 0.7f;
			prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle2.TimeToLive = num;
			prettySparkleParticle2.FadeOutEnd = num;
			prettySparkleParticle2.FadeInEnd = num / 2f;
			prettySparkleParticle2.FadeOutStart = num / 2f;
			prettySparkleParticle2.DrawVerticalAxis = false;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
			for (int i = 0; i < 1; i++)
			{
				if (Main.rand.Next(2) != 0)
				{
					Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, 242, vector.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
					dust.noGravity = true;
					dust.scale = 1.4f;
					Dust dust2 = Dust.NewDustPerfect(settings.PositionInWorld, 242, -vector.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
					dust2.noGravity = true;
					dust2.scale = 1.4f;
				}
			}
		}
		num = 30f;
		num2 = 0f;
		for (float num5 = 0f; num5 < 4f; num5 += 1f)
		{
			PrettySparkleParticle prettySparkleParticle3 = _poolPrettySparkle.RequestParticle();
			Vector2 vector2 = ((float)Math.PI / 2f * num5 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle3.ColorTint = new Color(0.9f, 0.85f, 0.4f, 0.5f);
			prettySparkleParticle3.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle3.Rotation = vector2.ToRotation();
			prettySparkleParticle3.Scale = new Vector2((num5 % 2f == 0f) ? 2f : 4f, 0.5f) * 1.1f;
			prettySparkleParticle3.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle3.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle3.TimeToLive = num;
			prettySparkleParticle3.FadeOutEnd = num;
			prettySparkleParticle3.FadeInEnd = num / 2f;
			prettySparkleParticle3.FadeOutStart = num / 2f;
			prettySparkleParticle3.AdditiveAmount = 0.35f;
			prettySparkleParticle3.Velocity = -vector2 * 0.2f;
			prettySparkleParticle3.DrawVerticalAxis = false;
			if (num5 % 2f == 1f)
			{
				prettySparkleParticle3.Scale *= 1.5f;
				prettySparkleParticle3.Velocity *= 1.5f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle3);
		}
		for (float num6 = 0f; num6 < 4f; num6 += 1f)
		{
			PrettySparkleParticle prettySparkleParticle4 = _poolPrettySparkle.RequestParticle();
			Vector2 vector3 = ((float)Math.PI / 2f * num6 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle4.ColorTint = new Color(1f, 1f, 0.2f, 1f);
			prettySparkleParticle4.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle4.Rotation = vector3.ToRotation();
			prettySparkleParticle4.Scale = new Vector2((num6 % 2f == 0f) ? 2f : 4f, 0.5f) * 0.7f;
			prettySparkleParticle4.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle4.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle4.TimeToLive = num;
			prettySparkleParticle4.FadeOutEnd = num;
			prettySparkleParticle4.FadeInEnd = num / 2f;
			prettySparkleParticle4.FadeOutStart = num / 2f;
			prettySparkleParticle4.Velocity = vector3 * 0.2f;
			prettySparkleParticle4.DrawVerticalAxis = false;
			if (num6 % 2f == 1f)
			{
				prettySparkleParticle4.Scale *= 1.5f;
				prettySparkleParticle4.Velocity *= 1.5f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle4);
			for (int j = 0; j < 1; j++)
			{
				if (Main.rand.Next(2) != 0)
				{
					Dust dust3 = Dust.NewDustPerfect(settings.PositionInWorld, 169, vector3.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
					dust3.noGravity = true;
					dust3.scale = 1.4f;
					Dust dust4 = Dust.NewDustPerfect(settings.PositionInWorld, 169, -vector3.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
					dust4.noGravity = true;
					dust4.scale = 1.4f;
				}
			}
		}
	}

	private static void Spawn_AshTreeShake(ParticleOrchestraSettings settings)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		float num = 10f + 20f * Main.rand.NextFloat();
		float num2 = -(float)Math.PI / 4f;
		float num3 = 0.2f + 0.4f * Main.rand.NextFloat();
		Color colorTint = Main.hslToRgb(Main.rand.NextFloat() * 0.1f + 0.06f, 1f, 0.5f);
		((Color)(ref colorTint)).A = (byte)(((Color)(ref colorTint)).A / 2);
		colorTint *= Main.rand.NextFloat() * 0.3f + 0.7f;
		for (float num4 = 0f; num4 < 2f; num4 += 1f)
		{
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			Vector2 vector = ((float)Math.PI / 4f + (float)Math.PI * num4 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle.ColorTint = colorTint;
			prettySparkleParticle.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle.Rotation = vector.ToRotation();
			prettySparkleParticle.Scale = new Vector2(4f, 1f) * 1.1f * num3;
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.TimeToLive = num;
			prettySparkleParticle.FadeOutEnd = num;
			prettySparkleParticle.FadeInEnd = num / 2f;
			prettySparkleParticle.FadeOutStart = num / 2f;
			prettySparkleParticle.AdditiveAmount = 0.35f;
			prettySparkleParticle.LocalPosition -= vector * num * 0.25f;
			prettySparkleParticle.Velocity = vector * 0.05f;
			prettySparkleParticle.DrawVerticalAxis = false;
			if (num4 == 1f)
			{
				prettySparkleParticle.Scale *= 1.5f;
				prettySparkleParticle.Velocity *= 1.5f;
				prettySparkleParticle.LocalPosition -= prettySparkleParticle.Velocity * 4f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		for (float num5 = 0f; num5 < 2f; num5 += 1f)
		{
			PrettySparkleParticle prettySparkleParticle2 = _poolPrettySparkle.RequestParticle();
			Vector2 vector2 = ((float)Math.PI / 4f + (float)Math.PI * num5 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle2.ColorTint = new Color(1f, 0.4f, 0.2f, 1f);
			prettySparkleParticle2.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle2.Rotation = vector2.ToRotation();
			prettySparkleParticle2.Scale = new Vector2(4f, 1f) * 0.7f * num3;
			prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle2.TimeToLive = num;
			prettySparkleParticle2.FadeOutEnd = num;
			prettySparkleParticle2.FadeInEnd = num / 2f;
			prettySparkleParticle2.FadeOutStart = num / 2f;
			prettySparkleParticle2.LocalPosition -= vector2 * num * 0.25f;
			prettySparkleParticle2.Velocity = vector2 * 0.05f;
			prettySparkleParticle2.DrawVerticalAxis = false;
			if (num5 == 1f)
			{
				prettySparkleParticle2.Scale *= 1.5f;
				prettySparkleParticle2.Velocity *= 1.5f;
				prettySparkleParticle2.LocalPosition -= prettySparkleParticle2.Velocity * 4f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
			for (int i = 0; i < 1; i++)
			{
				Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, 6, vector2.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
				dust.noGravity = true;
				dust.scale = 1.4f;
				Dust dust2 = Dust.NewDustPerfect(settings.PositionInWorld, 6, -vector2.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
				dust2.noGravity = true;
				dust2.scale = 1.4f;
			}
		}
	}

	private static void Spawn_LeafCrystalPassive(ParticleOrchestraSettings settings)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		float num = 90f;
		float num2 = (float)Math.PI * 2f * Main.rand.NextFloat();
		float num3 = 3f;
		for (float num4 = 0f; num4 < num3; num4 += 1f)
		{
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			Vector2 v = ((float)Math.PI * 2f / num3 * num4 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle.ColorTint = new Color(0.3f, 0.6f, 0.3f, 0.5f);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle.Rotation = v.ToRotation();
			prettySparkleParticle.Scale = new Vector2(4f, 1f) * 0.4f;
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.TimeToLive = num;
			prettySparkleParticle.FadeOutEnd = num;
			prettySparkleParticle.FadeInEnd = 10f;
			prettySparkleParticle.FadeOutStart = 10f;
			prettySparkleParticle.AdditiveAmount = 0.5f;
			prettySparkleParticle.Velocity = Vector2.Zero;
			prettySparkleParticle.DrawVerticalAxis = false;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
	}

	private static void Spawn_LeafCrystalShot(ParticleOrchestraSettings settings)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		int num = 30;
		PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
		Vector2 movementVector = settings.MovementVector;
		Color value = Main.hslToRgb((float)settings.UniqueInfoPiece / 255f, 1f, 0.5f);
		value = (prettySparkleParticle.ColorTint = Color.Lerp(value, Color.Gold, (float)(int)((Color)(ref value)).R / 255f * 0.5f));
		prettySparkleParticle.LocalPosition = settings.PositionInWorld;
		prettySparkleParticle.Rotation = movementVector.ToRotation();
		prettySparkleParticle.Scale = new Vector2(4f, 1f) * 1f;
		prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
		prettySparkleParticle.FadeOutNormalizedTime = 1f;
		prettySparkleParticle.TimeToLive = num;
		prettySparkleParticle.FadeOutEnd = num;
		prettySparkleParticle.FadeInEnd = num / 2;
		prettySparkleParticle.FadeOutStart = num / 2;
		prettySparkleParticle.AdditiveAmount = 0.5f;
		prettySparkleParticle.Velocity = settings.MovementVector;
		prettySparkleParticle.LocalPosition -= prettySparkleParticle.Velocity * 4f;
		prettySparkleParticle.DrawVerticalAxis = false;
		Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
	}

	private static void Spawn_TrueNightsEdge(ParticleOrchestraSettings settings)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		float num = 30f;
		float num2 = 0f;
		for (float num3 = 0f; num3 < 3f; num3 += 2f)
		{
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			Vector2 vector = ((float)Math.PI / 4f + (float)Math.PI / 4f * num3 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle.ColorTint = new Color(0.3f, 0.6f, 0.3f, 0.5f);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle.Rotation = vector.ToRotation();
			prettySparkleParticle.Scale = new Vector2(4f, 1f) * 1.1f;
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.TimeToLive = num;
			prettySparkleParticle.FadeOutEnd = num;
			prettySparkleParticle.FadeInEnd = num / 2f;
			prettySparkleParticle.FadeOutStart = num / 2f;
			prettySparkleParticle.AdditiveAmount = 0.35f;
			prettySparkleParticle.LocalPosition -= vector * num * 0.25f;
			prettySparkleParticle.Velocity = vector;
			prettySparkleParticle.DrawVerticalAxis = false;
			if (num3 == 1f)
			{
				prettySparkleParticle.Scale *= 1.5f;
				prettySparkleParticle.Velocity *= 1.5f;
				prettySparkleParticle.LocalPosition -= prettySparkleParticle.Velocity * 4f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		for (float num4 = 0f; num4 < 3f; num4 += 2f)
		{
			PrettySparkleParticle prettySparkleParticle2 = _poolPrettySparkle.RequestParticle();
			Vector2 vector2 = ((float)Math.PI / 4f + (float)Math.PI / 4f * num4 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle2.ColorTint = new Color(0.6f, 1f, 0.2f, 1f);
			prettySparkleParticle2.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle2.Rotation = vector2.ToRotation();
			prettySparkleParticle2.Scale = new Vector2(4f, 1f) * 0.7f;
			prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle2.TimeToLive = num;
			prettySparkleParticle2.FadeOutEnd = num;
			prettySparkleParticle2.FadeInEnd = num / 2f;
			prettySparkleParticle2.FadeOutStart = num / 2f;
			prettySparkleParticle2.LocalPosition -= vector2 * num * 0.25f;
			prettySparkleParticle2.Velocity = vector2;
			prettySparkleParticle2.DrawVerticalAxis = false;
			if (num4 == 1f)
			{
				prettySparkleParticle2.Scale *= 1.5f;
				prettySparkleParticle2.Velocity *= 1.5f;
				prettySparkleParticle2.LocalPosition -= prettySparkleParticle2.Velocity * 4f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
			for (int i = 0; i < 2; i++)
			{
				Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, 75, vector2.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
				dust.noGravity = true;
				dust.scale = 1.4f;
				Dust dust2 = Dust.NewDustPerfect(settings.PositionInWorld, 75, -vector2.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
				dust2.noGravity = true;
				dust2.scale = 1.4f;
			}
		}
	}

	private static void Spawn_NightsEdge(ParticleOrchestraSettings settings)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		float num = 30f;
		float num2 = 0f;
		for (float num3 = 0f; num3 < 3f; num3 += 1f)
		{
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			Vector2 vector = ((float)Math.PI / 4f + (float)Math.PI / 4f * num3 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle.ColorTint = new Color(0.25f, 0.1f, 0.5f, 0.5f);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle.Rotation = vector.ToRotation();
			prettySparkleParticle.Scale = new Vector2(2f, 1f) * 1.1f;
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.TimeToLive = num;
			prettySparkleParticle.FadeOutEnd = num;
			prettySparkleParticle.FadeInEnd = num / 2f;
			prettySparkleParticle.FadeOutStart = num / 2f;
			prettySparkleParticle.AdditiveAmount = 0.35f;
			prettySparkleParticle.LocalPosition -= vector * num * 0.25f;
			prettySparkleParticle.Velocity = vector;
			prettySparkleParticle.DrawVerticalAxis = false;
			if (num3 == 1f)
			{
				prettySparkleParticle.Scale *= 1.5f;
				prettySparkleParticle.Velocity *= 1.5f;
				prettySparkleParticle.LocalPosition -= prettySparkleParticle.Velocity * 4f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		for (float num4 = 0f; num4 < 3f; num4 += 1f)
		{
			PrettySparkleParticle prettySparkleParticle2 = _poolPrettySparkle.RequestParticle();
			Vector2 vector2 = ((float)Math.PI / 4f + (float)Math.PI / 4f * num4 + num2).ToRotationVector2() * 4f;
			prettySparkleParticle2.ColorTint = new Color(0.5f, 0.25f, 1f, 1f);
			prettySparkleParticle2.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle2.Rotation = vector2.ToRotation();
			prettySparkleParticle2.Scale = new Vector2(2f, 1f) * 0.7f;
			prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle2.TimeToLive = num;
			prettySparkleParticle2.FadeOutEnd = num;
			prettySparkleParticle2.FadeInEnd = num / 2f;
			prettySparkleParticle2.FadeOutStart = num / 2f;
			prettySparkleParticle2.LocalPosition -= vector2 * num * 0.25f;
			prettySparkleParticle2.Velocity = vector2;
			prettySparkleParticle2.DrawVerticalAxis = false;
			if (num4 == 1f)
			{
				prettySparkleParticle2.Scale *= 1.5f;
				prettySparkleParticle2.Velocity *= 1.5f;
				prettySparkleParticle2.LocalPosition -= prettySparkleParticle2.Velocity * 4f;
			}
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
		}
	}

	private static void Spawn_SilverBulletSparkle(ParticleOrchestraSettings settings)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		Main.rand.NextFloat();
		Vector2 movementVector = settings.MovementVector;
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector(Main.rand.NextFloat() * 0.2f + 0.4f);
		Main.rand.NextFloat();
		float rotation = (float)Math.PI / 2f;
		Vector2 vector2 = Main.rand.NextVector2Circular(4f, 4f) * vector;
		PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
		prettySparkleParticle.AccelerationPerFrame = -movementVector * 1f / 30f;
		prettySparkleParticle.Velocity = movementVector;
		prettySparkleParticle.ColorTint = Color.White;
		prettySparkleParticle.LocalPosition = settings.PositionInWorld + vector2;
		prettySparkleParticle.Rotation = rotation;
		prettySparkleParticle.Scale = vector;
		prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
		prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
		prettySparkleParticle.FadeInEnd = 10f;
		prettySparkleParticle.FadeOutStart = 20f;
		prettySparkleParticle.FadeOutEnd = 30f;
		prettySparkleParticle.TimeToLive = 30f;
		Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
	}

	private static void Spawn_PaladinsHammer(ParticleOrchestraSettings settings)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		float num2 = 1f;
		Vector2 vector2 = default(Vector2);
		for (float num3 = 0f; num3 < 1f; num3 += 1f / num2)
		{
			float num4 = 0.6f + Main.rand.NextFloat() * 0.35f;
			Vector2 vector = settings.MovementVector * num4;
			((Vector2)(ref vector2))._002Ector(Main.rand.NextFloat() * 0.4f + 0.2f);
			float f = num + Main.rand.NextFloat() * ((float)Math.PI * 2f);
			float rotation = (float)Math.PI / 2f;
			_ = 0.1f * vector2;
			Vector2 vector3 = Main.rand.NextVector2Circular(12f, 12f) * vector2;
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.AccelerationPerFrame = -vector * 1f / 30f;
			prettySparkleParticle.Velocity = vector + f.ToRotationVector2() * 2f * num4;
			prettySparkleParticle.ColorTint = new Color(1f, 0.8f, 0.4f, 0f);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + vector3;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = vector2;
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.TimeToLive = 40f;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.AccelerationPerFrame = -vector * 1f / 30f;
			prettySparkleParticle.Velocity = vector * 0.8f + f.ToRotationVector2() * 2f;
			prettySparkleParticle.ColorTint = new Color(255, 255, 255, 0);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + vector3;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = vector2 * 0.6f;
			prettySparkleParticle.FadeInNormalizedTime = 0.1f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.9f;
			prettySparkleParticle.TimeToLive = 60f;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		Color newColor = default(Color);
		for (int i = 0; i < 2; i++)
		{
			((Color)(ref newColor))._002Ector(1f, 0.7f, 0.3f, 0f);
			int num5 = Dust.NewDust(settings.PositionInWorld, 0, 0, 267, 0f, 0f, 0, newColor);
			Main.dust[num5].velocity = Main.rand.NextVector2Circular(2f, 2f);
			Dust obj = Main.dust[num5];
			obj.velocity += settings.MovementVector * (0.5f + 0.5f * Main.rand.NextFloat()) * 1.4f;
			Main.dust[num5].noGravity = true;
			Main.dust[num5].scale = 0.1f;
			Dust obj2 = Main.dust[num5];
			obj2.position += Main.rand.NextVector2Circular(16f, 16f);
			Main.dust[num5].velocity = settings.MovementVector;
			if (num5 != 6000)
			{
				Dust dust = Dust.CloneDust(num5);
				dust.scale /= 2f;
				dust.fadeIn *= 0.75f;
				dust.color = new Color(255, 255, 255, 255);
			}
		}
	}

	private static void Spawn_PrincessWeapon(ParticleOrchestraSettings settings)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		float num2 = 1f;
		Vector2 vector2 = default(Vector2);
		for (float num3 = 0f; num3 < 1f; num3 += 1f / num2)
		{
			Vector2 vector = settings.MovementVector * (0.6f + Main.rand.NextFloat() * 0.35f);
			((Vector2)(ref vector2))._002Ector(Main.rand.NextFloat() * 0.4f + 0.2f);
			float f = num + Main.rand.NextFloat() * ((float)Math.PI * 2f);
			float rotation = (float)Math.PI / 2f;
			Vector2 vector3 = 0.1f * vector2;
			float num4 = 60f;
			Vector2 vector4 = Main.rand.NextVector2Circular(8f, 8f) * vector2;
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.Velocity = f.ToRotationVector2() * vector3 + vector;
			prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(vector3 / num4) - vector * 1f / 30f;
			prettySparkleParticle.AccelerationPerFrame = -vector * 1f / 60f;
			prettySparkleParticle.Velocity = vector * 0.66f;
			prettySparkleParticle.ColorTint = Main.hslToRgb((0.92f + Main.rand.NextFloat() * 0.02f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f);
			((Color)(ref prettySparkleParticle.ColorTint)).A = 0;
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + vector4;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = vector2;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.Velocity = f.ToRotationVector2() * vector3 + vector;
			prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(vector3 / num4) - vector * 1f / 15f;
			prettySparkleParticle.AccelerationPerFrame = -vector * 1f / 60f;
			prettySparkleParticle.Velocity = vector * 0.66f;
			prettySparkleParticle.ColorTint = new Color(255, 255, 255, 0);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + vector4;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = vector2 * 0.6f;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		for (int i = 0; i < 2; i++)
		{
			Color newColor = Main.hslToRgb((0.92f + Main.rand.NextFloat() * 0.02f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f);
			int num5 = Dust.NewDust(settings.PositionInWorld, 0, 0, 267, 0f, 0f, 0, newColor);
			Main.dust[num5].velocity = Main.rand.NextVector2Circular(2f, 2f);
			Dust obj = Main.dust[num5];
			obj.velocity += settings.MovementVector * (0.5f + 0.5f * Main.rand.NextFloat()) * 1.4f;
			Main.dust[num5].noGravity = true;
			Main.dust[num5].scale = 0.1f;
			Dust obj2 = Main.dust[num5];
			obj2.position += Main.rand.NextVector2Circular(16f, 16f);
			Main.dust[num5].velocity = settings.MovementVector;
			if (num5 != 6000)
			{
				Dust dust = Dust.CloneDust(num5);
				dust.scale /= 2f;
				dust.fadeIn *= 0.75f;
				dust.color = new Color(255, 255, 255, 255);
			}
		}
	}

	private static void Spawn_StardustPunch(ParticleOrchestraSettings settings)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		float num2 = 1f;
		Vector2 vector2 = default(Vector2);
		for (float num3 = 0f; num3 < 1f; num3 += 1f / num2)
		{
			Vector2 vector = settings.MovementVector * (0.3f + Main.rand.NextFloat() * 0.35f);
			((Vector2)(ref vector2))._002Ector(Main.rand.NextFloat() * 0.4f + 0.4f);
			float f = num + Main.rand.NextFloat() * ((float)Math.PI * 2f);
			float rotation = (float)Math.PI / 2f;
			Vector2 vector3 = 0.1f * vector2;
			float num4 = 60f;
			Vector2 vector4 = Main.rand.NextVector2Circular(8f, 8f) * vector2;
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.Velocity = f.ToRotationVector2() * vector3 + vector;
			prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(vector3 / num4) - vector * 1f / 60f;
			prettySparkleParticle.ColorTint = Main.hslToRgb((0.6f + Main.rand.NextFloat() * 0.05f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f);
			((Color)(ref prettySparkleParticle.ColorTint)).A = 0;
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + vector4;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = vector2;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.Velocity = f.ToRotationVector2() * vector3 + vector;
			prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(vector3 / num4) - vector * 1f / 30f;
			prettySparkleParticle.ColorTint = new Color(255, 255, 255, 0);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + vector4;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = vector2 * 0.6f;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		for (int i = 0; i < 2; i++)
		{
			Color newColor = Main.hslToRgb((0.59f + Main.rand.NextFloat() * 0.05f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f);
			int num5 = Dust.NewDust(settings.PositionInWorld, 0, 0, 267, 0f, 0f, 0, newColor);
			Main.dust[num5].velocity = Main.rand.NextVector2Circular(2f, 2f);
			Dust obj = Main.dust[num5];
			obj.velocity += settings.MovementVector * (0.5f + 0.5f * Main.rand.NextFloat()) * 1.4f;
			Main.dust[num5].noGravity = true;
			Main.dust[num5].scale = 0.6f + Main.rand.NextFloat() * 2f;
			Dust obj2 = Main.dust[num5];
			obj2.position += Main.rand.NextVector2Circular(16f, 16f);
			if (num5 != 6000)
			{
				Dust dust = Dust.CloneDust(num5);
				dust.scale /= 2f;
				dust.fadeIn *= 0.75f;
				dust.color = new Color(255, 255, 255, 255);
			}
		}
	}

	private static void Spawn_RainbowRodHit(ParticleOrchestraSettings settings)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		float num2 = 6f;
		float num3 = Main.rand.NextFloat();
		Vector2 vector2 = default(Vector2);
		for (float num4 = 0f; num4 < 1f; num4 += 1f / num2)
		{
			Vector2 vector = settings.MovementVector * Main.rand.NextFloatDirection() * 0.15f;
			((Vector2)(ref vector2))._002Ector(Main.rand.NextFloat() * 0.4f + 0.4f);
			float f = num + Main.rand.NextFloat() * ((float)Math.PI * 2f);
			float rotation = (float)Math.PI / 2f;
			Vector2 vector3 = 1.5f * vector2;
			float num5 = 60f;
			Vector2 vector4 = Main.rand.NextVector2Circular(8f, 8f) * vector2;
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.Velocity = f.ToRotationVector2() * vector3 + vector;
			prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(vector3 / num5) - vector * 1f / 60f;
			prettySparkleParticle.ColorTint = Main.hslToRgb((num3 + Main.rand.NextFloat() * 0.33f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f);
			((Color)(ref prettySparkleParticle.ColorTint)).A = 0;
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + vector4;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = vector2;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.Velocity = f.ToRotationVector2() * vector3 + vector;
			prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(vector3 / num5) - vector * 1f / 60f;
			prettySparkleParticle.ColorTint = new Color(255, 255, 255, 0);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + vector4;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = vector2 * 0.6f;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		for (int i = 0; i < 12; i++)
		{
			Color newColor = Main.hslToRgb((num3 + Main.rand.NextFloat() * 0.12f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f);
			int num6 = Dust.NewDust(settings.PositionInWorld, 0, 0, 267, 0f, 0f, 0, newColor);
			Main.dust[num6].velocity = Main.rand.NextVector2Circular(1f, 1f);
			Dust obj = Main.dust[num6];
			obj.velocity += settings.MovementVector * Main.rand.NextFloatDirection() * 0.5f;
			Main.dust[num6].noGravity = true;
			Main.dust[num6].scale = 0.6f + Main.rand.NextFloat() * 0.9f;
			Main.dust[num6].fadeIn = 0.7f + Main.rand.NextFloat() * 0.8f;
			if (num6 != 6000)
			{
				Dust dust = Dust.CloneDust(num6);
				dust.scale /= 2f;
				dust.fadeIn *= 0.75f;
				dust.color = new Color(255, 255, 255, 255);
			}
		}
	}

	private static void Spawn_BlackLightningSmall(ParticleOrchestraSettings settings)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		float num2 = Main.rand.Next(1, 3);
		float num3 = 0.7f;
		int num4 = 916;
		Main.instance.LoadProjectile(num4);
		Color value = default(Color);
		((Color)(ref value))._002Ector(255, 255, 255, 255);
		Color indigo = Color.Indigo;
		((Color)(ref indigo)).A = 0;
		Color colorTint = default(Color);
		for (float num5 = 0f; num5 < 1f; num5 += 1f / num2)
		{
			float f = (float)Math.PI * 2f * num5 + num + Main.rand.NextFloatDirection() * 0.25f;
			float num6 = Main.rand.NextFloat() * 4f + 0.1f;
			Vector2 vector = Main.rand.NextVector2Circular(12f, 12f) * num3;
			Color.Lerp(Color.Lerp(Color.Black, indigo, Main.rand.NextFloat() * 0.5f), value, Main.rand.NextFloat() * 0.6f);
			((Color)(ref colorTint))._002Ector(0, 0, 0, 255);
			int num7 = Main.rand.Next(4);
			if (num7 == 1)
			{
				colorTint = Color.Lerp(new Color(106, 90, 205, 127), Color.Black, 0.1f + 0.7f * Main.rand.NextFloat());
			}
			if (num7 == 2)
			{
				colorTint = Color.Lerp(new Color(106, 90, 205, 60), Color.Black, 0.1f + 0.8f * Main.rand.NextFloat());
			}
			RandomizedFrameParticle randomizedFrameParticle = _poolRandomizedFrame.RequestParticle();
			randomizedFrameParticle.SetBasicInfo(TextureAssets.Projectile[num4], null, Vector2.Zero, vector);
			randomizedFrameParticle.SetTypeInfo(Main.projFrames[num4], 2, 24f);
			randomizedFrameParticle.Velocity = f.ToRotationVector2() * num6 * new Vector2(1f, 0.5f) * 0.2f + settings.MovementVector;
			randomizedFrameParticle.ColorTint = colorTint;
			randomizedFrameParticle.LocalPosition = settings.PositionInWorld + vector;
			randomizedFrameParticle.Rotation = randomizedFrameParticle.Velocity.ToRotation();
			randomizedFrameParticle.Scale = Vector2.One * 0.5f;
			randomizedFrameParticle.FadeInNormalizedTime = 0.01f;
			randomizedFrameParticle.FadeOutNormalizedTime = 0.5f;
			randomizedFrameParticle.ScaleVelocity = new Vector2(0.025f);
			Main.ParticleSystem_World_OverPlayers.Add(randomizedFrameParticle);
		}
	}

	private static void Spawn_BlackLightningHit(ParticleOrchestraSettings settings)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		float num2 = 7f;
		float num3 = 0.7f;
		int num4 = 916;
		Main.instance.LoadProjectile(num4);
		Color value = default(Color);
		((Color)(ref value))._002Ector(255, 255, 255, 255);
		Color indigo = Color.Indigo;
		((Color)(ref indigo)).A = 0;
		Color colorTint = default(Color);
		for (float num5 = 0f; num5 < 1f; num5 += 1f / num2)
		{
			float num6 = (float)Math.PI * 2f * num5 + num + Main.rand.NextFloatDirection() * 0.25f;
			float num7 = Main.rand.NextFloat() * 4f + 0.1f;
			Vector2 vector = Main.rand.NextVector2Circular(12f, 12f) * num3;
			Color.Lerp(Color.Lerp(Color.Black, indigo, Main.rand.NextFloat() * 0.5f), value, Main.rand.NextFloat() * 0.6f);
			((Color)(ref colorTint))._002Ector(0, 0, 0, 255);
			int num8 = Main.rand.Next(4);
			if (num8 == 1)
			{
				colorTint = Color.Lerp(new Color(106, 90, 205, 127), Color.Black, 0.1f + 0.7f * Main.rand.NextFloat());
			}
			if (num8 == 2)
			{
				colorTint = Color.Lerp(new Color(106, 90, 205, 60), Color.Black, 0.1f + 0.8f * Main.rand.NextFloat());
			}
			RandomizedFrameParticle randomizedFrameParticle = _poolRandomizedFrame.RequestParticle();
			randomizedFrameParticle.SetBasicInfo(TextureAssets.Projectile[num4], null, Vector2.Zero, vector);
			randomizedFrameParticle.SetTypeInfo(Main.projFrames[num4], 2, 24f);
			randomizedFrameParticle.Velocity = num6.ToRotationVector2() * num7 * new Vector2(1f, 0.5f);
			randomizedFrameParticle.ColorTint = colorTint;
			randomizedFrameParticle.LocalPosition = settings.PositionInWorld + vector;
			randomizedFrameParticle.Rotation = num6;
			randomizedFrameParticle.Scale = Vector2.One;
			randomizedFrameParticle.FadeInNormalizedTime = 0.01f;
			randomizedFrameParticle.FadeOutNormalizedTime = 0.5f;
			randomizedFrameParticle.ScaleVelocity = new Vector2(0.05f);
			Main.ParticleSystem_World_OverPlayers.Add(randomizedFrameParticle);
		}
	}

	private static void Spawn_StellarTune(ParticleOrchestraSettings settings)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		float num2 = 5f;
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector(0.7f);
		for (float num3 = 0f; num3 < 1f; num3 += 1f / num2)
		{
			float num4 = (float)Math.PI * 2f * num3 + num + Main.rand.NextFloatDirection() * 0.25f;
			Vector2 vector2 = 1.5f * vector;
			float num5 = 60f;
			Vector2 vector3 = Main.rand.NextVector2Circular(12f, 12f) * vector;
			Color colorTint = Color.Lerp(Color.Gold, Color.HotPink, Main.rand.NextFloat());
			if (Main.rand.Next(2) == 0)
			{
				colorTint = Color.Lerp(Color.Violet, Color.HotPink, Main.rand.NextFloat());
			}
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.Velocity = num4.ToRotationVector2() * vector2;
			prettySparkleParticle.AccelerationPerFrame = num4.ToRotationVector2() * -(vector2 / num5);
			prettySparkleParticle.ColorTint = colorTint;
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + vector3;
			prettySparkleParticle.Rotation = num4;
			prettySparkleParticle.Scale = vector * (Main.rand.NextFloat() * 0.8f + 0.2f);
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		num2 = 1f;
	}

	private static void Spawn_Keybrand(ParticleOrchestraSettings settings)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		float num3 = 3f;
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector(0.7f);
		for (float num4 = 0f; num4 < 1f; num4 += 1f / num3)
		{
			float num5 = (float)Math.PI * 2f * num4 + num + Main.rand.NextFloatDirection() * 0.1f;
			Vector2 vector2 = 1.5f * vector;
			float num6 = 60f;
			Vector2 vector3 = Main.rand.NextVector2Circular(4f, 4f) * vector;
			PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
			prettySparkleParticle.Velocity = num5.ToRotationVector2() * vector2;
			prettySparkleParticle.AccelerationPerFrame = num5.ToRotationVector2() * -(vector2 / num6);
			prettySparkleParticle.ColorTint = Color.Lerp(Color.Gold, Color.OrangeRed, Main.rand.NextFloat());
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + vector3;
			prettySparkleParticle.Rotation = num5;
			prettySparkleParticle.Scale = vector * 0.8f;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}
		num += 1f / num3 / 2f * ((float)Math.PI * 2f);
		num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		for (float num7 = 0f; num7 < 1f; num7 += 1f / num3)
		{
			float num8 = (float)Math.PI * 2f * num7 + num + Main.rand.NextFloatDirection() * 0.1f;
			Vector2 vector4 = 1f * vector;
			float num9 = 30f;
			Color value = Color.Lerp(Color.Gold, Color.OrangeRed, Main.rand.NextFloat());
			value = Color.Lerp(Color.White, value, 0.5f);
			((Color)(ref value)).A = 0;
			Vector2 vector5 = Main.rand.NextVector2Circular(4f, 4f) * vector;
			FadingParticle fadingParticle = _poolFading.RequestParticle();
			fadingParticle.SetBasicInfo(TextureAssets.Extra[98], null, Vector2.Zero, Vector2.Zero);
			fadingParticle.SetTypeInfo(num9);
			fadingParticle.Velocity = num8.ToRotationVector2() * vector4;
			fadingParticle.AccelerationPerFrame = num8.ToRotationVector2() * -(vector4 / num9);
			fadingParticle.ColorTint = value;
			fadingParticle.LocalPosition = settings.PositionInWorld + num8.ToRotationVector2() * vector4 * vector * num9 * 0.2f + vector5;
			fadingParticle.Rotation = num8 + (float)Math.PI / 2f;
			fadingParticle.FadeInNormalizedTime = 0.3f;
			fadingParticle.FadeOutNormalizedTime = 0.4f;
			fadingParticle.Scale = new Vector2(0.5f, 1.2f) * 0.8f * vector;
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
		}
		num3 = 1f;
		num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		for (float num10 = 0f; num10 < 1f; num10 += 1f / num3)
		{
			float num2 = (float)Math.PI * 2f * num10 + num;
			float typeInfo = 30f;
			Color colorTint = Color.Lerp(Color.CornflowerBlue, Color.White, Main.rand.NextFloat());
			((Color)(ref colorTint)).A = 127;
			Vector2 vector6 = Main.rand.NextVector2Circular(4f, 4f) * vector;
			Vector2 vector7 = Main.rand.NextVector2Square(0.7f, 1.3f);
			FadingParticle fadingParticle2 = _poolFading.RequestParticle();
			fadingParticle2.SetBasicInfo(TextureAssets.Extra[174], null, Vector2.Zero, Vector2.Zero);
			fadingParticle2.SetTypeInfo(typeInfo);
			fadingParticle2.ColorTint = colorTint;
			fadingParticle2.LocalPosition = settings.PositionInWorld + vector6;
			fadingParticle2.Rotation = num2 + (float)Math.PI / 2f;
			fadingParticle2.FadeInNormalizedTime = 0.1f;
			fadingParticle2.FadeOutNormalizedTime = 0.4f;
			fadingParticle2.Scale = new Vector2(0.1f, 0.1f) * vector;
			fadingParticle2.ScaleVelocity = vector7 * 1f / 60f;
			fadingParticle2.ScaleAcceleration = vector7 * (-1f / 60f) / 60f;
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle2);
		}
	}

	private static void Spawn_FlameWaders(ParticleOrchestraSettings settings)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		float num = 60f;
		for (int i = -1; i <= 1; i++)
		{
			int num2 = Main.rand.NextFromList(new short[3] { 326, 327, 328 });
			Main.instance.LoadProjectile(num2);
			Player player = Main.player[settings.IndexOfPlayerWhoInvokedThis];
			float num3 = Main.rand.NextFloat() * 0.9f + 0.1f;
			Vector2 vector = settings.PositionInWorld + new Vector2((float)i * 5.3333335f, 0f);
			FlameParticle flameParticle = _poolFlame.RequestParticle();
			flameParticle.SetBasicInfo(TextureAssets.Projectile[num2], null, Vector2.Zero, vector);
			flameParticle.SetTypeInfo(num, settings.IndexOfPlayerWhoInvokedThis, player.cFlameWaker);
			flameParticle.FadeOutNormalizedTime = 0.4f;
			flameParticle.ScaleAcceleration = Vector2.One * num3 * (-1f / 60f) / num;
			flameParticle.Scale = Vector2.One * num3;
			Main.ParticleSystem_World_BehindPlayers.Add(flameParticle);
			if (Main.rand.Next(16) == 0)
			{
				Dust dust = Dust.NewDustDirect(vector, 4, 4, 6, 0f, 0f, 100);
				if (Main.rand.Next(2) == 0)
				{
					dust.noGravity = true;
					dust.fadeIn = 1.15f;
				}
				else
				{
					dust.scale = 0.6f;
				}
				dust.velocity *= 0.6f;
				dust.velocity.Y -= 1.2f;
				dust.noLight = true;
				dust.position.Y -= 4f;
				dust.shader = GameShaders.Armor.GetSecondaryShader(player.cFlameWaker, player);
			}
		}
	}

	private static void Spawn_WallOfFleshGoatMountFlames(ParticleOrchestraSettings settings)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		float num = 50f;
		for (int i = -1; i <= 1; i++)
		{
			int num2 = Main.rand.NextFromList(new short[3] { 326, 327, 328 });
			Main.instance.LoadProjectile(num2);
			Player player = Main.player[settings.IndexOfPlayerWhoInvokedThis];
			float num3 = Main.rand.NextFloat() * 0.9f + 0.1f;
			Vector2 vector = settings.PositionInWorld + new Vector2((float)i * 5.3333335f, 0f);
			FlameParticle flameParticle = _poolFlame.RequestParticle();
			flameParticle.SetBasicInfo(TextureAssets.Projectile[num2], null, Vector2.Zero, vector);
			flameParticle.SetTypeInfo(num, settings.IndexOfPlayerWhoInvokedThis, player.cMount);
			flameParticle.FadeOutNormalizedTime = 0.3f;
			flameParticle.ScaleAcceleration = Vector2.One * num3 * (-1f / 60f) / num;
			flameParticle.Scale = Vector2.One * num3;
			Main.ParticleSystem_World_BehindPlayers.Add(flameParticle);
			if (Main.rand.Next(8) == 0)
			{
				Dust dust = Dust.NewDustDirect(vector, 4, 4, 6, 0f, 0f, 100);
				if (Main.rand.Next(2) == 0)
				{
					dust.noGravity = true;
					dust.fadeIn = 1.15f;
				}
				else
				{
					dust.scale = 0.6f;
				}
				dust.velocity *= 0.6f;
				dust.velocity.Y -= 1.2f;
				dust.noLight = true;
				dust.position.Y -= 4f;
				dust.shader = GameShaders.Armor.GetSecondaryShader(player.cMount, player);
			}
		}
	}
}
