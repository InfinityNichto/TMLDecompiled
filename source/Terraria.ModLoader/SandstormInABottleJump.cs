using Microsoft.Xna.Framework;

namespace Terraria.ModLoader;

public sealed class SandstormInABottleJump : VanillaExtraJump
{
	public override float GetDurationMultiplier(Player player)
	{
		return 3f;
	}

	public override void ShowVisuals(Player player)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		int num3 = player.height;
		if (player.gravDir == -1f)
		{
			num3 = -6;
		}
		float num4 = ((float)player.jump / 75f + 1f) / 2f;
		for (int i = 0; i < 3; i++)
		{
			int num5 = Dust.NewDust(new Vector2(player.position.X, player.position.Y + (float)(num3 / 2)), player.width, 32, 124, player.velocity.X * 0.3f, player.velocity.Y * 0.3f, 150, default(Color), 1f * num4);
			Dust obj = Main.dust[num5];
			obj.velocity *= 0.5f * num4;
			Main.dust[num5].fadeIn = 1.5f * num4;
		}
		player.sandStorm = true;
		if (player.miscCounter % 3 == 0)
		{
			int num6 = Gore.NewGore(new Vector2(player.position.X + (float)(player.width / 2) - 18f, player.position.Y + (float)(num3 / 2)), new Vector2(0f - player.velocity.X, 0f - player.velocity.Y), Main.rand.Next(220, 223), num4);
			Main.gore[num6].velocity = player.velocity * 0.3f * num4;
			Main.gore[num6].alpha = 100;
		}
	}

	public override void UpdateHorizontalSpeeds(Player player)
	{
		player.runAcceleration *= 1.5f;
		player.maxRunSpeed *= 2f;
	}
}
