using System;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Physics;

namespace Terraria.GameContent.Golf;

public static class GolfHelper
{
	public struct ClubProperties
	{
		public readonly Vector2 MinimumStrength;

		public readonly Vector2 MaximumStrength;

		public readonly float RoughLandResistance;

		public ClubProperties(Vector2 minimumStrength, Vector2 maximumStrength, float roughLandResistance)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			MinimumStrength = minimumStrength;
			MaximumStrength = maximumStrength;
			RoughLandResistance = roughLandResistance;
		}
	}

	public struct ShotStrength
	{
		public readonly float AbsoluteStrength;

		public readonly float RelativeStrength;

		public readonly float RoughLandResistance;

		public ShotStrength(float absoluteStrength, float relativeStrength, float roughLandResistance)
		{
			AbsoluteStrength = absoluteStrength;
			RelativeStrength = relativeStrength;
			RoughLandResistance = roughLandResistance;
		}
	}

	public class ContactListener : IBallContactListener
	{
		public void OnCollision(PhysicsProperties properties, ref Vector2 position, ref Vector2 velocity, ref BallCollisionEvent collision)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			TileMaterial byTileId = TileMaterials.GetByTileId(collision.Tile.type);
			Vector2 vector = velocity * byTileId.GolfPhysics.SideImpactDampening;
			Vector2 vector2 = collision.Normal * Vector2.Dot(velocity, collision.Normal) * (byTileId.GolfPhysics.DirectImpactDampening - byTileId.GolfPhysics.SideImpactDampening);
			velocity = vector + vector2;
			Projectile projectile = collision.Entity as Projectile;
			switch (collision.Tile.type)
			{
			case 421:
			case 422:
			{
				float num2 = 2.5f * collision.TimeScale;
				Vector2 vector3 = default(Vector2);
				((Vector2)(ref vector3))._002Ector(0f - collision.Normal.Y, collision.Normal.X);
				if (collision.Tile.type == 422)
				{
					vector3 = -vector3;
				}
				float num3 = Vector2.Dot(velocity, vector3);
				if (num3 < num2)
				{
					velocity += vector3 * MathHelper.Clamp(num2 - num3, 0f, num2 * 0.5f);
				}
				break;
			}
			case 476:
			{
				float num = ((Vector2)(ref velocity)).Length() / collision.TimeScale;
				if (!(collision.Normal.Y > -0.01f) && !(num > 100f))
				{
					velocity *= 0f;
					if (projectile != null && projectile.active)
					{
						PutBallInCup(projectile, collision);
					}
				}
				break;
			}
			}
			if (projectile != null && velocity.Y < -0.3f && velocity.Y > -2f && ((Vector2)(ref velocity)).Length() > 1f)
			{
				Dust dust = Dust.NewDustPerfect(collision.Entity.Center, 31, collision.Normal, 127);
				dust.scale = 0.7f;
				dust.fadeIn = 1f;
				dust.velocity = dust.velocity * 0.5f + Main.rand.NextVector2CircularEdge(0.5f, 0.4f);
			}
		}

		public void PutBallInCup(Projectile proj, BallCollisionEvent collision)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			if (proj.owner == Main.myPlayer && Main.LocalGolfState.ShouldScoreHole)
			{
				Point hitLocation = (collision.ImpactPoint - collision.Normal * 0.5f).ToTileCoordinates();
				int owner = proj.owner;
				int num = (int)proj.ai[1];
				int type = proj.type;
				if (num > 1)
				{
					Main.LocalGolfState.SetScoreTime();
				}
				Main.LocalGolfState.RecordBallInfo(proj);
				Main.LocalGolfState.LandBall(proj);
				int golfBallScore = Main.LocalGolfState.GetGolfBallScore(proj);
				if (num > 0)
				{
					Main.player[owner].AccumulateGolfingScore(golfBallScore);
				}
				PutBallInCup_TextAndEffects(hitLocation, owner, num, type);
				Main.LocalGolfState.ResetScoreTime();
				Wiring.HitSwitch(hitLocation.X, hitLocation.Y);
				NetMessage.SendData(59, -1, -1, null, hitLocation.X, hitLocation.Y);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(128, -1, -1, null, owner, num, type, 0f, hitLocation.X, hitLocation.Y);
				}
			}
			proj.Kill();
		}

		public static void PutBallInCup_TextAndEffects(Point hitLocation, int plr, int numberOfHits, int projid)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			if (numberOfHits != 0)
			{
				EmitGolfballExplosion(hitLocation.ToWorldCoordinates(8f, 0f));
				string key = "Game.BallBounceResultGolf_Single";
				NetworkText networkText;
				if (numberOfHits != 1)
				{
					key = "Game.BallBounceResultGolf_Plural";
					networkText = NetworkText.FromKey(key, Main.player[plr].name, NetworkText.FromKey(Lang.GetProjectileName(projid).Key), numberOfHits);
				}
				else
				{
					networkText = NetworkText.FromKey(key, Main.player[plr].name, NetworkText.FromKey(Lang.GetProjectileName(projid).Key));
				}
				if (Main.netMode == 0 || Main.netMode == 1)
				{
					Main.NewText(networkText.ToString(), byte.MaxValue, 240, 20);
				}
				else if (Main.netMode == 2)
				{
					ChatHelper.BroadcastChatMessage(networkText, new Color(255, 240, 20));
				}
			}
		}

		public void OnPassThrough(PhysicsProperties properties, ref Vector2 position, ref Vector2 velocity, ref float angularVelocity, ref BallPassThroughEvent collision)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			switch (collision.Type)
			{
			case BallPassThroughType.Water:
				velocity *= 0.91f;
				angularVelocity *= 0.91f;
				break;
			case BallPassThroughType.Honey:
				velocity *= 0.8f;
				angularVelocity *= 0.8f;
				break;
			case BallPassThroughType.Tile:
			{
				TileMaterial byTileId = TileMaterials.GetByTileId(collision.Tile.type);
				velocity *= byTileId.GolfPhysics.PassThroughDampening;
				angularVelocity *= byTileId.GolfPhysics.PassThroughDampening;
				break;
			}
			case BallPassThroughType.Lava:
				break;
			}
		}

		public static void EmitGolfballExplosion_Old(Vector2 Center)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			EmitGolfballExplosion(Center);
		}

		public static void EmitGolfballExplosion(Vector2 Center)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_0366: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_039d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0471: Unknown result type (might be due to invalid IL or missing references)
			//IL_0478: Unknown result type (might be due to invalid IL or missing references)
			//IL_0482: Unknown result type (might be due to invalid IL or missing references)
			//IL_0487: Unknown result type (might be due to invalid IL or missing references)
			//IL_048c: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
			SoundEngine.PlaySound(in SoundID.Item129, Center);
			for (float num = 0f; num < 1f; num += 0.085f)
			{
				Dust dust5 = Dust.NewDustPerfect(Center, 278, (num * ((float)Math.PI * 2f)).ToRotationVector2() * new Vector2(2f, 0.5f));
				dust5.fadeIn = 1.2f;
				dust5.noGravity = true;
				dust5.velocity.X *= 0.7f;
				dust5.velocity.Y -= 1.5f;
				dust5.position.Y += 8f;
				dust5.velocity.X *= 2f;
				dust5.color = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f);
			}
			float num2 = Main.rand.NextFloat();
			float num3 = Main.rand.Next(5, 10);
			for (int i = 0; (float)i < num3; i++)
			{
				int num4 = Main.rand.Next(5, 22);
				Vector2 value = (((float)i - num3 / 2f) * ((float)Math.PI * 2f) / 256f - (float)Math.PI / 2f).ToRotationVector2() * new Vector2(5f, 1f) * (0.25f + Main.rand.NextFloat() * 0.05f);
				Color color = Main.hslToRgb((num2 + (float)i / num3) % 1f, 0.7f, 0.7f);
				((Color)(ref color)).A = 127;
				for (int j = 0; j < num4; j++)
				{
					Dust dust6 = Dust.NewDustPerfect(Center + new Vector2((float)i - num3 / 2f, 0f) * 2f, 278, value);
					dust6.fadeIn = 0.7f;
					dust6.scale = 0.7f;
					dust6.noGravity = true;
					dust6.position.Y += -1f;
					dust6.velocity *= (float)j;
					dust6.scale += 0.2f - (float)j * 0.03f;
					dust6.velocity += Main.rand.NextVector2Circular(0.05f, 0.05f);
					dust6.color = color;
				}
			}
			for (float num5 = 0f; num5 < 1f; num5 += 0.2f)
			{
				Dust dust7 = Dust.NewDustPerfect(Center, 278, (num5 * ((float)Math.PI * 2f)).ToRotationVector2() * new Vector2(1f, 0.5f));
				dust7.fadeIn = 1.2f;
				dust7.noGravity = true;
				dust7.velocity.X *= 0.7f;
				dust7.velocity.Y -= 0.5f;
				dust7.position.Y += 8f;
				dust7.velocity.X *= 2f;
				dust7.color = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.3f);
			}
			float num6 = Main.rand.NextFloatDirection();
			for (float num7 = 0f; num7 < 1f; num7 += 0.15f)
			{
				Dust dust4 = Dust.NewDustPerfect(Center, 278, (num6 + num7 * ((float)Math.PI * 2f)).ToRotationVector2() * 4f);
				dust4.fadeIn = 1.5f;
				dust4.velocity *= 0.5f + num7 * 0.8f;
				dust4.noGravity = true;
				dust4.velocity.X *= 0.35f;
				dust4.velocity.Y *= 2f;
				dust4.velocity.Y -= 1f;
				dust4.velocity.Y = 0f - Math.Abs(dust4.velocity.Y);
				dust4.position += dust4.velocity * 3f;
				dust4.color = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.6f + Main.rand.NextFloat() * 0.2f);
			}
		}

		public static void EmitGolfballExplosion_v1(Vector2 Center)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			for (float num = 0f; num < 1f; num += 0.085f)
			{
				Dust dust4 = Dust.NewDustPerfect(Center, 278, (num * ((float)Math.PI * 2f)).ToRotationVector2() * new Vector2(2f, 0.5f));
				dust4.fadeIn = 1.2f;
				dust4.noGravity = true;
				dust4.velocity.X *= 0.7f;
				dust4.velocity.Y -= 1.5f;
				dust4.position.Y += 8f;
				dust4.color = Color.Lerp(Color.Silver, Color.White, 0.5f);
			}
			for (float num2 = 0f; num2 < 1f; num2 += 0.2f)
			{
				Dust dust5 = Dust.NewDustPerfect(Center, 278, (num2 * ((float)Math.PI * 2f)).ToRotationVector2() * new Vector2(1f, 0.5f));
				dust5.fadeIn = 1.2f;
				dust5.noGravity = true;
				dust5.velocity.X *= 0.7f;
				dust5.velocity.Y -= 0.5f;
				dust5.position.Y += 8f;
				dust5.color = Color.Lerp(Color.Silver, Color.White, 0.5f);
			}
			float num3 = Main.rand.NextFloatDirection();
			for (float num4 = 0f; num4 < 1f; num4 += 0.15f)
			{
				Dust dust3 = Dust.NewDustPerfect(Center, 278, (num3 + num4 * ((float)Math.PI * 2f)).ToRotationVector2() * 4f);
				dust3.fadeIn = 1.5f;
				dust3.velocity *= 0.5f + num4 * 0.8f;
				dust3.noGravity = true;
				dust3.velocity.X *= 0.35f;
				dust3.velocity.Y *= 2f;
				dust3.velocity.Y -= 1f;
				dust3.velocity.Y = 0f - Math.Abs(dust3.velocity.Y);
				dust3.position += dust3.velocity * 3f;
				dust3.color = Color.Lerp(Color.Silver, Color.White, 0.5f);
			}
		}
	}

	public const int PointsNeededForLevel1 = 500;

	public const int PointsNeededForLevel2 = 1000;

	public const int PointsNeededForLevel3 = 2000;

	public static readonly PhysicsProperties PhysicsProperties = new PhysicsProperties(0.3f, 0.99f);

	public static readonly ContactListener Listener = new ContactListener();

	public static FancyGolfPredictionLine PredictionLine;

	public static BallStepResult StepGolfBall(Entity entity, ref float angularVelocity)
	{
		return BallCollision.Step(PhysicsProperties, entity, ref angularVelocity, Listener);
	}

	public static Vector2 FindVectorOnOval(Vector2 vector, Vector2 radius)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		if (Math.Abs(radius.X) < 0.0001f || Math.Abs(radius.Y) < 0.0001f)
		{
			return Vector2.Zero;
		}
		return Vector2.Normalize(vector / radius) * radius;
	}

	public static ShotStrength CalculateShotStrength(Vector2 shotVector, ClubProperties clubProperties)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		Vector2.Normalize(shotVector);
		float num3 = ((Vector2)(ref shotVector)).Length();
		Vector2 val = FindVectorOnOval(shotVector, clubProperties.MaximumStrength);
		float num = ((Vector2)(ref val)).Length();
		val = FindVectorOnOval(shotVector, clubProperties.MinimumStrength);
		float num2 = ((Vector2)(ref val)).Length();
		float num4 = MathHelper.Clamp(num3, num2, num);
		float relativeStrength = Math.Max((num4 - num2) / (num - num2), 0.001f);
		return new ShotStrength(num4 * 32f, relativeStrength, clubProperties.RoughLandResistance);
	}

	public static bool IsPlayerHoldingClub(Player player)
	{
		if (player == null || player.HeldItem == null)
		{
			return false;
		}
		int type = player.HeldItem.type;
		if (type == 4039 || (uint)(type - 4092) <= 2u || (uint)(type - 4587) <= 11u)
		{
			return true;
		}
		return false;
	}

	public static ShotStrength CalculateShotStrength(Projectile golfHelper, Entity golfBall)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		int num = Main.screenWidth;
		if (num > Main.screenHeight)
		{
			num = Main.screenHeight;
		}
		int num2 = 150;
		num -= num2;
		num /= 2;
		if (num < 200)
		{
			num = 200;
		}
		float num3 = num;
		num3 = 300f;
		if (golfHelper.ai[0] != 0f)
		{
			return default(ShotStrength);
		}
		Vector2 shotVector = (golfHelper.Center - golfBall.Center) / num3;
		ClubProperties clubPropertiesFromGolfHelper = GetClubPropertiesFromGolfHelper(golfHelper);
		return CalculateShotStrength(shotVector, clubPropertiesFromGolfHelper);
	}

	public static ClubProperties GetClubPropertiesFromGolfHelper(Projectile golfHelper)
	{
		return GetClubProperties((short)Main.player[golfHelper.owner].HeldItem.type);
	}

	public static ClubProperties GetClubProperties(short itemId)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector(0.25f, 0.25f);
		return itemId switch
		{
			4039 => new ClubProperties(vector, Vector2.One, 0f), 
			4092 => new ClubProperties(Vector2.Zero, vector, 0f), 
			4093 => new ClubProperties(vector, new Vector2(0.65f, 1.5f), 1f), 
			4094 => new ClubProperties(vector, new Vector2(1.5f, 0.65f), 0f), 
			4587 => new ClubProperties(vector, Vector2.One, 0f), 
			4588 => new ClubProperties(Vector2.Zero, vector, 0f), 
			4589 => new ClubProperties(vector, new Vector2(0.65f, 1.5f), 1f), 
			4590 => new ClubProperties(vector, new Vector2(1.5f, 0.65f), 0f), 
			4591 => new ClubProperties(vector, Vector2.One, 0f), 
			4592 => new ClubProperties(Vector2.Zero, vector, 0f), 
			4593 => new ClubProperties(vector, new Vector2(0.65f, 1.5f), 1f), 
			4594 => new ClubProperties(vector, new Vector2(1.5f, 0.65f), 0f), 
			4595 => new ClubProperties(vector, Vector2.One, 0f), 
			4596 => new ClubProperties(Vector2.Zero, vector, 0f), 
			4597 => new ClubProperties(vector, new Vector2(0.65f, 1.5f), 1f), 
			4598 => new ClubProperties(vector, new Vector2(1.5f, 0.65f), 0f), 
			_ => default(ClubProperties), 
		};
	}

	public static Projectile FindHelperFromGolfBall(Projectile golfBall)
	{
		for (int i = 0; i < 1000; i++)
		{
			Projectile projectile = Main.projectile[i];
			if (projectile.active && projectile.type == 722 && projectile.owner == golfBall.owner)
			{
				return Main.projectile[i];
			}
		}
		return null;
	}

	public static Projectile FindGolfBallForHelper(Projectile golfHelper)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 1000; i++)
		{
			Projectile projectile = Main.projectile[i];
			Vector2 shotVector = golfHelper.Center - projectile.Center;
			if (projectile.active && ProjectileID.Sets.IsAGolfBall[projectile.type] && projectile.owner == golfHelper.owner && ValidateShot(projectile, Main.player[golfHelper.owner], ref shotVector))
			{
				return Main.projectile[i];
			}
		}
		return null;
	}

	public static bool IsGolfBallResting(Projectile golfBall)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		if ((int)golfBall.localAI[1] != 0)
		{
			return Vector2.Distance(golfBall.position, golfBall.oldPos[golfBall.oldPos.Length - 1]) < 1f;
		}
		return true;
	}

	public static bool IsGolfShotValid(Entity golfBall, Player player)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = golfBall.Center - player.Bottom;
		if (player.direction == -1)
		{
			vector.X *= -1f;
		}
		if (vector.X >= -16f && vector.X <= 32f && vector.Y <= 16f)
		{
			return vector.Y >= -16f;
		}
		return false;
	}

	public static bool ValidateShot(Entity golfBall, Player player, ref Vector2 shotVector)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = golfBall.Center - player.Bottom;
		if (player.direction == -1)
		{
			vector.X *= -1f;
			shotVector.X *= -1f;
		}
		float num = shotVector.ToRotation();
		if (num > 0f)
		{
			shotVector = ((Vector2)(ref shotVector)).Length() * new Vector2((float)Math.Cos(0.0), (float)Math.Sin(0.0));
		}
		else if (num < -1.5207964f)
		{
			shotVector = ((Vector2)(ref shotVector)).Length() * new Vector2((float)Math.Cos(-1.5207964181900024), (float)Math.Sin(-1.5207964181900024));
		}
		if (player.direction == -1)
		{
			shotVector.X *= -1f;
		}
		if (vector.X >= -16f && vector.X <= 32f && vector.Y <= 16f)
		{
			return vector.Y >= -16f;
		}
		return false;
	}

	public static void HitGolfBall(Entity entity, Vector2 velocity, float roughLandResistance)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		Vector2 bottom = entity.Bottom;
		bottom.Y += 1f;
		Point point = bottom.ToTileCoordinates();
		Tile tile = Main.tile[point.X, point.Y];
		if (tile != null && tile.active())
		{
			TileMaterial byTileId = TileMaterials.GetByTileId(tile.type);
			velocity = Vector2.Lerp(velocity * byTileId.GolfPhysics.ClubImpactDampening, velocity, byTileId.GolfPhysics.ImpactDampeningResistanceEfficiency * roughLandResistance);
		}
		entity.velocity = velocity;
		if (entity is Projectile projectile)
		{
			projectile.timeLeft = 18000;
			if (projectile.ai[1] < 0f)
			{
				projectile.ai[1] = 0f;
			}
			projectile.ai[1] += 1f;
			projectile.localAI[1] = 1f;
			Main.LocalGolfState.RecordSwing(projectile);
		}
	}

	public static void DrawPredictionLine(Entity golfBall, Vector2 impactVelocity, float chargeProgress, float roughLandResistance)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		if (PredictionLine == null)
		{
			PredictionLine = new FancyGolfPredictionLine(20);
		}
		PredictionLine.Update(golfBall, impactVelocity, roughLandResistance);
		PredictionLine.Draw(Main.Camera, Main.spriteBatch, chargeProgress);
	}
}
