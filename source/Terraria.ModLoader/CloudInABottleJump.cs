using Microsoft.Xna.Framework;

namespace Terraria.ModLoader;

public sealed class CloudInABottleJump : VanillaExtraJump
{
	public override float GetDurationMultiplier(Player player)
	{
		return 0.75f;
	}

	public override void OnStarted(Player player, ref bool playSound)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		int num22 = player.height;
		if (player.gravDir == -1f)
		{
			num22 = 0;
		}
		for (int num23 = 0; num23 < 10; num23++)
		{
			int num24 = Dust.NewDust(new Vector2(player.position.X - 34f, player.position.Y + (float)num22 - 16f), 102, 32, 16, (0f - player.velocity.X) * 0.5f, player.velocity.Y * 0.5f, 100, default(Color), 1.5f);
			Main.dust[num24].velocity.X = Main.dust[num24].velocity.X * 0.5f - player.velocity.X * 0.1f;
			Main.dust[num24].velocity.Y = Main.dust[num24].velocity.Y * 0.5f - player.velocity.Y * 0.3f;
		}
		int num25 = Gore.NewGore(new Vector2(player.position.X + (float)(player.width / 2) - 16f, player.position.Y + (float)num22 - 16f), new Vector2(0f - player.velocity.X, 0f - player.velocity.Y), Main.rand.Next(11, 14));
		Main.gore[num25].velocity.X = Main.gore[num25].velocity.X * 0.1f - player.velocity.X * 0.1f;
		Main.gore[num25].velocity.Y = Main.gore[num25].velocity.Y * 0.1f - player.velocity.Y * 0.05f;
		num25 = Gore.NewGore(new Vector2(player.position.X - 36f, player.position.Y + (float)num22 - 16f), new Vector2(0f - player.velocity.X, 0f - player.velocity.Y), Main.rand.Next(11, 14));
		Main.gore[num25].velocity.X = Main.gore[num25].velocity.X * 0.1f - player.velocity.X * 0.1f;
		Main.gore[num25].velocity.Y = Main.gore[num25].velocity.Y * 0.1f - player.velocity.Y * 0.05f;
		num25 = Gore.NewGore(new Vector2(player.position.X + (float)player.width + 4f, player.position.Y + (float)num22 - 16f), new Vector2(0f - player.velocity.X, 0f - player.velocity.Y), Main.rand.Next(11, 14));
		Main.gore[num25].velocity.X = Main.gore[num25].velocity.X * 0.1f - player.velocity.X * 0.1f;
		Main.gore[num25].velocity.Y = Main.gore[num25].velocity.Y * 0.1f - player.velocity.Y * 0.05f;
	}

	public override void ShowVisuals(Player player)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		int num = player.height;
		if (player.gravDir == -1f)
		{
			num = -6;
		}
		int num2 = Dust.NewDust(new Vector2(player.position.X - 4f, player.position.Y + (float)num), player.width + 8, 4, 16, (0f - player.velocity.X) * 0.5f, player.velocity.Y * 0.5f, 100, default(Color), 1.5f);
		Main.dust[num2].velocity.X = Main.dust[num2].velocity.X * 0.5f - player.velocity.X * 0.1f;
		Main.dust[num2].velocity.Y = Main.dust[num2].velocity.Y * 0.5f - player.velocity.Y * 0.3f;
	}
}
