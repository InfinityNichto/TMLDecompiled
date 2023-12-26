using Microsoft.Xna.Framework;

namespace Terraria.ModLoader;

public sealed class TsunamiInABottleJump : VanillaExtraJump
{
	public override float GetDurationMultiplier(Player player)
	{
		return 1.25f;
	}

	public override void OnStarted(Player player, ref bool playSound)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		int num5 = player.height;
		if (player.gravDir == -1f)
		{
			num5 = 0;
		}
		Vector2 vector = default(Vector2);
		for (int i = 0; i < 30; i++)
		{
			int num6 = Dust.NewDust(new Vector2(player.position.X, player.position.Y + (float)num5), player.width, 12, 253, player.velocity.X * 0.3f, player.velocity.Y * 0.3f, 100, default(Color), 1.5f);
			if (i % 2 == 0)
			{
				Main.dust[num6].velocity.X += (float)Main.rand.Next(30, 71) * 0.1f;
			}
			else
			{
				Main.dust[num6].velocity.X -= (float)Main.rand.Next(30, 71) * 0.1f;
			}
			Main.dust[num6].velocity.Y += (float)Main.rand.Next(-10, 31) * 0.1f;
			Main.dust[num6].noGravity = true;
			Main.dust[num6].scale += (float)Main.rand.Next(-10, 41) * 0.01f;
			Dust obj = Main.dust[num6];
			obj.velocity *= Main.dust[num6].scale * 0.7f;
			((Vector2)(ref vector))._002Ector((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
			((Vector2)(ref vector)).Normalize();
			vector *= (float)Main.rand.Next(81) * 0.1f;
		}
	}

	public override void ShowVisuals(Player player)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		int num12 = 1;
		if (player.jump > 0)
		{
			num12 = 2;
		}
		int num10 = player.height - 6;
		if (player.gravDir == -1f)
		{
			num10 = 6;
		}
		Vector2 vector = default(Vector2);
		for (int i = 0; i < num12; i++)
		{
			int num11 = Dust.NewDust(new Vector2(player.position.X, player.position.Y + (float)num10), player.width, 12, 253, player.velocity.X * 0.3f, player.velocity.Y * 0.3f, 100, default(Color), 1.5f);
			Main.dust[num11].scale += (float)Main.rand.Next(-5, 3) * 0.1f;
			if (player.jump <= 0)
			{
				Main.dust[num11].scale *= 0.8f;
			}
			else
			{
				Dust obj = Main.dust[num11];
				obj.velocity -= player.velocity / 5f;
			}
			Main.dust[num11].noGravity = true;
			((Vector2)(ref vector))._002Ector((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
			((Vector2)(ref vector)).Normalize();
			vector *= (float)Main.rand.Next(81) * 0.1f;
		}
	}

	public override void UpdateHorizontalSpeeds(Player player)
	{
		player.runAcceleration *= 1.5f;
		player.maxRunSpeed *= 1.25f;
	}
}
