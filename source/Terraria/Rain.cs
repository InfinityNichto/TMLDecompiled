using System;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace Terraria;

public class Rain
{
	public Vector2 position;

	public Vector2 velocity;

	public float scale;

	public float rotation;

	public int alpha;

	public bool active;

	public byte type;

	public byte waterStyle;

	public static void ClearRain()
	{
		for (int i = 0; i < Main.maxRain; i++)
		{
			Main.rain[i].active = false;
		}
	}

	public static void MakeRain()
	{
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		if (Main.shimmerAlpha > 0f || Main.netMode == 2 || Main.gamePaused)
		{
			return;
		}
		if (Main.remixWorld)
		{
			if (!((double)(Main.player[Main.myPlayer].position.Y / 16f) > Main.rockLayer) || !(Main.player[Main.myPlayer].position.Y / 16f < (float)(Main.maxTilesY - 350)) || Main.player[Main.myPlayer].ZoneDungeon)
			{
				return;
			}
		}
		else if ((double)Main.screenPosition.Y > Main.worldSurface * 16.0)
		{
			return;
		}
		if (Main.gameMenu)
		{
			return;
		}
		float num = (float)Main.screenWidth / 1920f;
		num *= 25f;
		num *= 0.25f + 1f * Main.cloudAlpha;
		if (Filters.Scene["Sandstorm"].IsActive())
		{
			return;
		}
		Vector2 vector = default(Vector2);
		for (int i = 0; (float)i < num; i++)
		{
			int num2 = 600;
			if (Main.player[Main.myPlayer].velocity.Y < 0f)
			{
				num2 += (int)(Math.Abs(Main.player[Main.myPlayer].velocity.Y) * 30f);
			}
			vector.X = Main.rand.Next((int)Main.screenPosition.X - num2, (int)Main.screenPosition.X + Main.screenWidth + num2);
			vector.Y = Main.screenPosition.Y - (float)Main.rand.Next(20, 100);
			vector.X -= Main.windSpeedCurrent * 15f * 40f;
			vector.X += Main.player[Main.myPlayer].velocity.X * 40f;
			if (vector.X < 0f)
			{
				vector.X = 0f;
			}
			if (vector.X > (float)((Main.maxTilesX - 1) * 16))
			{
				vector.X = (Main.maxTilesX - 1) * 16;
			}
			int num3 = (int)vector.X / 16;
			int num4 = (int)vector.Y / 16;
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num3 > Main.maxTilesX - 1)
			{
				num3 = Main.maxTilesX - 1;
			}
			if (num4 < 0)
			{
				num4 = 0;
			}
			if (num4 > Main.maxTilesY - 1)
			{
				num4 = Main.maxTilesY - 1;
			}
			if (Main.remixWorld || Main.gameMenu || (!WorldGen.SolidTile(num3, num4) && Main.tile[num3, num4].wall <= 0))
			{
				Vector2 rainFallVelocity = GetRainFallVelocity();
				NewRain(vector, rainFallVelocity);
			}
		}
	}

	public static Vector2 GetRainFallVelocity()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(Main.windSpeedCurrent * 18f, 14f);
	}

	public void Update()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gamePaused)
		{
			return;
		}
		position += velocity;
		if (Main.gameMenu)
		{
			if (position.Y > Main.screenPosition.Y + (float)Main.screenHeight + 2000f)
			{
				active = false;
			}
		}
		else if (Main.remixWorld)
		{
			if (position.Y > Main.screenPosition.Y + (float)Main.screenHeight + 100f)
			{
				active = false;
			}
		}
		else if (Collision.SolidCollision(position, 2, 2) || position.Y > Main.screenPosition.Y + (float)Main.screenHeight + 100f || Collision.WetCollision(position, 2, 2))
		{
			active = false;
			if ((float)Main.rand.Next(100) < Main.gfxQuality * 100f)
			{
				int num = Dust.NewDust(position - velocity, 2, 2, Dust.dustWater());
				Main.dust[num].position.X -= 2f;
				Main.dust[num].position.Y += 2f;
				Main.dust[num].alpha = 38;
				Dust obj = Main.dust[num];
				obj.velocity *= 0.1f;
				Dust obj2 = Main.dust[num];
				obj2.velocity += -velocity * 0.025f;
				Main.dust[num].velocity.Y -= 2f;
				Main.dust[num].scale = 0.6f;
				Main.dust[num].noGravity = true;
			}
		}
	}

	public static int NewRainForced(Vector2 Position, Vector2 Velocity)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		int num = -1;
		int num2 = Main.maxRain;
		float num3 = (1f + Main.gfxQuality) / 2f;
		if (num3 < 0.9f)
		{
			num2 = (int)((float)num2 * num3);
		}
		for (int i = 0; i < num2; i++)
		{
			if (!Main.rain[i].active)
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			return Main.maxRain;
		}
		Rain rain = Main.rain[num];
		rain.active = true;
		rain.position = Position;
		rain.scale = 1f + (float)Main.rand.Next(-20, 21) * 0.01f;
		rain.velocity = Velocity * rain.scale;
		rain.rotation = (float)Math.Atan2(rain.velocity.X, 0f - rain.velocity.Y);
		rain.waterStyle = (byte)Main.waterStyle;
		if (Main.waterStyle >= 15)
		{
			rain.type = LoaderManager.Get<WaterStylesLoader>().Get(Main.waterStyle).GetRainVariant();
			return num;
		}
		rain.type = (byte)(Main.waterStyle * 3 + Main.rand.Next(3));
		return num;
	}

	private static int NewRain(Vector2 Position, Vector2 Velocity)
	{
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		int num = -1;
		int num2 = (int)((float)Main.maxRain * Main.cloudAlpha);
		if (num2 > Main.maxRain)
		{
			num2 = Main.maxRain;
		}
		float num3 = (float)Main.maxTilesX / 6400f;
		Math.Max(0f, Math.Min(1f, (Main.player[Main.myPlayer].position.Y / 16f - 85f * num3) / (60f * num3)));
		float num4 = (1f + Main.gfxQuality) / 2f;
		if ((double)num4 < 0.9)
		{
			num2 = (int)((float)num2 * num4);
		}
		float num5 = 800 - Main.SceneMetrics.SnowTileCount;
		if (num5 < 0f)
		{
			num5 = 0f;
		}
		num5 /= 800f;
		num2 = (int)((float)num2 * num5);
		num2 = (int)((double)num2 * Math.Pow(Main.atmo, 9.0));
		if ((double)Main.atmo < 0.4)
		{
			num2 = 0;
		}
		for (int i = 0; i < num2; i++)
		{
			if (!Main.rain[i].active)
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			return Main.maxRain;
		}
		Rain rain = Main.rain[num];
		rain.active = true;
		rain.position = Position;
		rain.scale = 1f + (float)Main.rand.Next(-20, 21) * 0.01f;
		rain.velocity = Velocity * rain.scale;
		rain.rotation = (float)Math.Atan2(rain.velocity.X, 0f - rain.velocity.Y);
		rain.waterStyle = (byte)Main.waterStyle;
		if (Main.waterStyle >= 15)
		{
			rain.type = LoaderManager.Get<WaterStylesLoader>().Get(Main.waterStyle).GetRainVariant();
			return num;
		}
		rain.type = (byte)(Main.waterStyle * 3 + Main.rand.Next(3));
		return num;
	}
}
