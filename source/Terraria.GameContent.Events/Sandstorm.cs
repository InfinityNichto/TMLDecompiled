using System;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Events;

public class Sandstorm
{
	private const int SANDSTORM_DURATION_MINIMUM = 28800;

	private const int SANDSTORM_DURATION_MAXIMUM = 86400;

	public static bool Happening;

	public static double TimeLeft;

	public static float Severity;

	public static float IntendedSeverity;

	private static bool _effectsUp;

	private static bool HasSufficientWind()
	{
		return Math.Abs(Main.windSpeedCurrent) >= 0.6f;
	}

	public static void WorldClear()
	{
		Happening = false;
	}

	public static void UpdateTime()
	{
		if (Main.netMode != 1)
		{
			if (Happening)
			{
				if (TimeLeft > 86400.0)
				{
					TimeLeft = 0.0;
				}
				TimeLeft -= Main.desiredWorldEventsUpdateRate;
				if (!HasSufficientWind())
				{
					TimeLeft -= 15.0 * Main.desiredWorldEventsUpdateRate;
				}
				if (Main.windSpeedCurrent == 0f)
				{
					TimeLeft = 0.0;
				}
				if (TimeLeft <= 0.0)
				{
					StopSandstorm();
				}
			}
			else
			{
				int num = 21600;
				num = ((!Main.hardMode) ? (num * 3) : (num * 2));
				if (HasSufficientWind())
				{
					for (int i = 0; i < Main.worldEventUpdates; i++)
					{
						if (Main.rand.Next(num) == 0)
						{
							StartSandstorm();
						}
					}
				}
			}
			if (Main.rand.Next(18000) == 0)
			{
				ChangeSeverityIntentions();
			}
		}
		UpdateSeverity();
	}

	private static void ChangeSeverityIntentions()
	{
		if (Happening)
		{
			IntendedSeverity = 0.4f + Main.rand.NextFloat();
		}
		else if (Main.rand.Next(3) == 0)
		{
			IntendedSeverity = 0f;
		}
		else
		{
			IntendedSeverity = Main.rand.NextFloat() * 0.3f;
		}
		if (Main.netMode != 1)
		{
			NetMessage.SendData(7);
		}
	}

	private static void UpdateSeverity()
	{
		if (float.IsNaN(Severity))
		{
			Severity = 0f;
		}
		if (float.IsNaN(IntendedSeverity))
		{
			IntendedSeverity = 0f;
		}
		int num = Math.Sign(IntendedSeverity - Severity);
		Severity = MathHelper.Clamp(Severity + 0.003f * (float)num, 0f, 1f);
		int num2 = Math.Sign(IntendedSeverity - Severity);
		if (num != num2)
		{
			Severity = IntendedSeverity;
		}
	}

	/// <summary>
	/// Starts sandstorm for a random amount of time. Should be called on the server (netMode != client) - the method syncs it automatically.
	/// </summary>
	public static void StartSandstorm()
	{
		Happening = true;
		TimeLeft = Main.rand.Next(28800, 86401);
		ChangeSeverityIntentions();
	}

	/// <summary>
	/// Stops sandstorm. Should be called on the server (netMode != client) - the method syncs it automatically.
	/// </summary>
	public static void StopSandstorm()
	{
		Happening = false;
		TimeLeft = 0.0;
		ChangeSeverityIntentions();
	}

	public static void HandleEffectAndSky(bool toState)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		if (toState != _effectsUp)
		{
			_effectsUp = toState;
			Vector2 center = Main.player[Main.myPlayer].Center;
			if (_effectsUp)
			{
				SkyManager.Instance.Activate("Sandstorm", center);
				Filters.Scene.Activate("Sandstorm", center);
				Overlays.Scene.Activate("Sandstorm", center);
			}
			else
			{
				SkyManager.Instance.Deactivate("Sandstorm");
				Filters.Scene.Deactivate("Sandstorm");
				Overlays.Scene.Deactivate("Sandstorm");
			}
		}
	}

	public static bool ShouldSandstormDustPersist()
	{
		if (Happening && Main.player[Main.myPlayer].ZoneSandstorm && (Main.bgStyle == 2 || Main.bgStyle == 5))
		{
			return Main.bgDelay < 50;
		}
		return false;
	}

	public static void EmitDust()
	{
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0522: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Unknown result type (might be due to invalid IL or missing references)
		//IL_0550: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gamePaused)
		{
			return;
		}
		int sandTileCount = Main.SceneMetrics.SandTileCount;
		Player player = Main.player[Main.myPlayer];
		bool flag = ShouldSandstormDustPersist();
		HandleEffectAndSky(flag && Main.UseStormEffects);
		if (sandTileCount < 100 || (double)player.position.Y > Main.worldSurface * 16.0 || player.ZoneBeach)
		{
			return;
		}
		int maxValue = 1;
		if (!flag || Main.rand.Next(maxValue) != 0)
		{
			return;
		}
		int num = Math.Sign(Main.windSpeedCurrent);
		float num8 = Math.Abs(Main.windSpeedCurrent);
		if (num8 < 0.01f)
		{
			return;
		}
		float num9 = (float)num * MathHelper.Lerp(0.9f, 1f, num8);
		float num10 = 2000f / (float)sandTileCount;
		float value = 3f / num10;
		value = MathHelper.Clamp(value, 0.77f, 1f);
		int num11 = (int)num10;
		float num12 = (float)Main.screenWidth / (float)Main.maxScreenW;
		int num13 = (int)(1000f * num12);
		float num14 = 20f * Severity;
		float num15 = (float)num13 * (Main.gfxQuality * 0.5f + 0.5f) + (float)num13 * 0.1f - (float)Dust.SandStormCount;
		if (num15 <= 0f)
		{
			return;
		}
		float num2 = (float)Main.screenWidth + 1000f;
		float num3 = Main.screenHeight;
		Vector2 vector = Main.screenPosition + player.velocity;
		WeightedRandom<Color> weightedRandom = new WeightedRandom<Color>();
		weightedRandom.Add(new Color(200, 160, 20, 180), Main.SceneMetrics.GetTileCount(53) + Main.SceneMetrics.GetTileCount(396) + Main.SceneMetrics.GetTileCount(397));
		weightedRandom.Add(new Color(103, 98, 122, 180), Main.SceneMetrics.GetTileCount(112) + Main.SceneMetrics.GetTileCount(400) + Main.SceneMetrics.GetTileCount(398));
		weightedRandom.Add(new Color(135, 43, 34, 180), Main.SceneMetrics.GetTileCount(234) + Main.SceneMetrics.GetTileCount(401) + Main.SceneMetrics.GetTileCount(399));
		weightedRandom.Add(new Color(213, 196, 197, 180), Main.SceneMetrics.GetTileCount(116) + Main.SceneMetrics.GetTileCount(403) + Main.SceneMetrics.GetTileCount(402));
		float num4 = MathHelper.Lerp(0.2f, 0.35f, Severity);
		float num5 = MathHelper.Lerp(0.5f, 0.7f, Severity);
		float amount = (value - 0.77f) / 0.23000002f;
		int maxValue2 = (int)MathHelper.Lerp(1f, 10f, amount);
		Vector2 position = default(Vector2);
		for (int i = 0; (float)i < num14; i++)
		{
			if (Main.rand.Next(num11 / 4) != 0)
			{
				continue;
			}
			((Vector2)(ref position))._002Ector(Main.rand.NextFloat() * num2 - 500f, Main.rand.NextFloat() * -50f);
			if (Main.rand.Next(3) == 0 && num == 1)
			{
				position.X = Main.rand.Next(500) - 500;
			}
			else if (Main.rand.Next(3) == 0 && num == -1)
			{
				position.X = Main.rand.Next(500) + Main.screenWidth;
			}
			if (position.X < 0f || position.X > (float)Main.screenWidth)
			{
				position.Y += Main.rand.NextFloat() * num3 * 0.9f;
			}
			position += vector;
			int num6 = (int)position.X / 16;
			int num7 = (int)position.Y / 16;
			if (!WorldGen.InWorld(num6, num7, 10) || Main.tile[num6, num7] == null)
			{
				continue;
			}
			Tile tile = Main.tile[num6, num7];
			if (tile.wall != 0)
			{
				continue;
			}
			for (int j = 0; j < 1; j++)
			{
				Dust dust = Main.dust[Dust.NewDust(position, 10, 10, 268)];
				dust.velocity.Y = 2f + Main.rand.NextFloat() * 0.2f;
				dust.velocity.Y *= dust.scale;
				dust.velocity.Y *= 0.35f;
				dust.velocity.X = num9 * 5f + Main.rand.NextFloat() * 1f;
				dust.velocity.X += num9 * num5 * 20f;
				dust.fadeIn += num5 * 0.2f;
				dust.velocity *= 1f + num4 * 0.5f;
				dust.color = weightedRandom;
				dust.velocity *= 1f + num4;
				dust.velocity *= value;
				dust.scale = 0.9f;
				num15 -= 1f;
				if (num15 <= 0f)
				{
					break;
				}
				if (Main.rand.Next(maxValue2) != 0)
				{
					j--;
					position += Utils.RandomVector2(Main.rand, -10f, 10f) + dust.velocity * -1.1f;
					num6 = (int)position.X / 16;
					num7 = (int)position.Y / 16;
					if (WorldGen.InWorld(num6, num7, 10) && Main.tile[num6, num7] != null)
					{
						tile = Main.tile[num6, num7];
						_ = ref tile.wall;
					}
				}
			}
			if (num15 <= 0f)
			{
				break;
			}
		}
	}
}
