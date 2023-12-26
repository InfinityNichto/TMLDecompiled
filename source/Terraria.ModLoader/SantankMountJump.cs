using Microsoft.Xna.Framework;

namespace Terraria.ModLoader;

public sealed class SantankMountJump : VanillaExtraJump
{
	public override float GetDurationMultiplier(Player player)
	{
		return 2f;
	}

	public override void OnStarted(Player player, ref bool playSound)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		int num17 = player.height;
		if (player.gravDir == -1f)
		{
			num17 = 0;
		}
		for (int num18 = 0; num18 < 15; num18++)
		{
			int num19 = Dust.NewDust(new Vector2(player.position.X - 34f, player.position.Y + (float)num17 - 16f), 102, 32, 4, (0f - player.velocity.X) * 0.5f, player.velocity.Y * 0.5f, 100, new Color(250, 230, 230, 150), 1.5f);
			Main.dust[num19].velocity.X = Main.dust[num19].velocity.X * 0.5f - player.velocity.X * 0.1f;
			Main.dust[num19].velocity.Y = Main.dust[num19].velocity.Y * 0.5f - player.velocity.Y * 0.3f;
			Main.dust[num19].noGravity = true;
			num19 = Dust.NewDust(new Vector2(player.position.X - 34f, player.position.Y + (float)num17 - 16f), 102, 32, 6, (0f - player.velocity.X) * 0.5f, player.velocity.Y * 0.5f, 20, default(Color), 1.5f);
			Main.dust[num19].velocity.Y -= 1f;
			if (num18 % 2 == 0)
			{
				Main.dust[num19].fadeIn = Main.rand.NextFloat() * 2f;
			}
		}
		float y = player.Bottom.Y - 22f;
		for (int num20 = 0; num20 < 3; num20++)
		{
			Vector2 vector8 = player.Center;
			switch (num20)
			{
			case 0:
				((Vector2)(ref vector8))._002Ector(player.Center.X - 16f, y);
				break;
			case 1:
				((Vector2)(ref vector8))._002Ector(player.position.X - 36f, y);
				break;
			case 2:
				((Vector2)(ref vector8))._002Ector(player.Right.X + 4f, y);
				break;
			}
			int num21 = Gore.NewGore(vector8, new Vector2(0f - player.velocity.X, 0f - player.velocity.Y), Main.rand.Next(61, 63));
			Gore obj = Main.gore[num21];
			obj.velocity *= 0.1f;
			Main.gore[num21].velocity.X -= player.velocity.X * 0.1f;
			Main.gore[num21].velocity.Y -= player.velocity.Y * 0.05f;
			Gore obj2 = Main.gore[num21];
			obj2.velocity += Main.rand.NextVector2Circular(1f, 1f) * 0.5f;
		}
	}

	public override void UpdateHorizontalSpeeds(Player player)
	{
		player.runAcceleration *= 3f;
		player.maxRunSpeed *= 1.5f;
	}
}
