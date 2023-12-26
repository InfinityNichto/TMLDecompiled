using Microsoft.Xna.Framework;

namespace Terraria.ModLoader;

public sealed class BlizzardInABottleJump : VanillaExtraJump
{
	public override float GetDurationMultiplier(Player player)
	{
		return 1.5f;
	}

	public override void ShowVisuals(Player player)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		int num12 = player.height - 6;
		if (player.gravDir == -1f)
		{
			num12 = 6;
		}
		for (int i = 0; i < 2; i++)
		{
			int num13 = Dust.NewDust(new Vector2(player.position.X, player.position.Y + (float)num12), player.width, 12, 76, player.velocity.X * 0.3f, player.velocity.Y * 0.3f);
			Dust obj = Main.dust[num13];
			obj.velocity *= 0.1f;
			if (i == 0)
			{
				Dust obj2 = Main.dust[num13];
				obj2.velocity += player.velocity * 0.03f;
			}
			else
			{
				Dust obj3 = Main.dust[num13];
				obj3.velocity -= player.velocity * 0.03f;
			}
			Dust obj4 = Main.dust[num13];
			obj4.velocity -= player.velocity * 0.1f;
			Main.dust[num13].noGravity = true;
			Main.dust[num13].noLight = true;
		}
		for (int j = 0; j < 3; j++)
		{
			int num14 = Dust.NewDust(new Vector2(player.position.X, player.position.Y + (float)num12), player.width, 12, 76, player.velocity.X * 0.3f, player.velocity.Y * 0.3f);
			Main.dust[num14].fadeIn = 1.5f;
			Dust obj5 = Main.dust[num14];
			obj5.velocity *= 0.6f;
			Dust obj6 = Main.dust[num14];
			obj6.velocity += player.velocity * 0.8f;
			Main.dust[num14].noGravity = true;
			Main.dust[num14].noLight = true;
		}
		for (int k = 0; k < 3; k++)
		{
			int num15 = Dust.NewDust(new Vector2(player.position.X, player.position.Y + (float)num12), player.width, 12, 76, player.velocity.X * 0.3f, player.velocity.Y * 0.3f);
			Main.dust[num15].fadeIn = 1.5f;
			Dust obj7 = Main.dust[num15];
			obj7.velocity *= 0.6f;
			Dust obj8 = Main.dust[num15];
			obj8.velocity -= player.velocity * 0.8f;
			Main.dust[num15].noGravity = true;
			Main.dust[num15].noLight = true;
		}
	}

	public override void UpdateHorizontalSpeeds(Player player)
	{
		player.runAcceleration *= 3f;
		player.maxRunSpeed *= 1.5f;
	}
}
