using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;

namespace Terraria.ModLoader;

public sealed class FartInAJarJump : VanillaExtraJump
{
	public override float GetDurationMultiplier(Player player)
	{
		return 2f;
	}

	public override void OnStarted(Player player, ref bool playSound)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		int num7 = player.height;
		if (player.gravDir == -1f)
		{
			num7 = 0;
		}
		playSound = false;
		SoundEngine.PlaySound(in SoundID.Item16, player.position);
		for (int i = 0; i < 10; i++)
		{
			int num8 = Dust.NewDust(new Vector2(player.position.X - 34f, player.position.Y + (float)num7 - 16f), 102, 32, 188, (0f - player.velocity.X) * 0.5f, player.velocity.Y * 0.5f, 100, default(Color), 1.5f);
			Main.dust[num8].velocity.X = Main.dust[num8].velocity.X * 0.5f - player.velocity.X * 0.1f;
			Main.dust[num8].velocity.Y = Main.dust[num8].velocity.Y * 0.5f - player.velocity.Y * 0.3f;
		}
		int num9 = Gore.NewGore(new Vector2(player.position.X + (float)(player.width / 2) - 16f, player.position.Y + (float)num7 - 16f), new Vector2(0f - player.velocity.X, 0f - player.velocity.Y), Main.rand.Next(435, 438));
		Main.gore[num9].velocity.X = Main.gore[num9].velocity.X * 0.1f - player.velocity.X * 0.1f;
		Main.gore[num9].velocity.Y = Main.gore[num9].velocity.Y * 0.1f - player.velocity.Y * 0.05f;
		num9 = Gore.NewGore(new Vector2(player.position.X - 36f, player.position.Y + (float)num7 - 16f), new Vector2(0f - player.velocity.X, 0f - player.velocity.Y), Main.rand.Next(435, 438));
		Main.gore[num9].velocity.X = Main.gore[num9].velocity.X * 0.1f - player.velocity.X * 0.1f;
		Main.gore[num9].velocity.Y = Main.gore[num9].velocity.Y * 0.1f - player.velocity.Y * 0.05f;
		num9 = Gore.NewGore(new Vector2(player.position.X + (float)player.width + 4f, player.position.Y + (float)num7 - 16f), new Vector2(0f - player.velocity.X, 0f - player.velocity.Y), Main.rand.Next(435, 438));
		Main.gore[num9].velocity.X = Main.gore[num9].velocity.X * 0.1f - player.velocity.X * 0.1f;
		Main.gore[num9].velocity.Y = Main.gore[num9].velocity.Y * 0.1f - player.velocity.Y * 0.05f;
	}

	public override void ShowVisuals(Player player)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		int num7 = player.height;
		if (player.gravDir == -1f)
		{
			num7 = -6;
		}
		int num8 = Dust.NewDust(new Vector2(player.position.X - 4f, player.position.Y + (float)num7), player.width + 8, 4, 188, (0f - player.velocity.X) * 0.5f, player.velocity.Y * 0.5f, 100, default(Color), 1.5f);
		Main.dust[num8].velocity.X = Main.dust[num8].velocity.X * 0.5f - player.velocity.X * 0.1f;
		Main.dust[num8].velocity.Y = Main.dust[num8].velocity.Y * 0.5f - player.velocity.Y * 0.3f;
		Dust obj = Main.dust[num8];
		obj.velocity *= 0.5f;
	}

	public override void UpdateHorizontalSpeeds(Player player)
	{
		player.runAcceleration *= 3f;
		player.maxRunSpeed *= 1.75f;
	}
}
