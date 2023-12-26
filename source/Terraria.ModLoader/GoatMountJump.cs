using System;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.ModLoader;

public sealed class GoatMountJump : VanillaExtraJump
{
	public override float GetDurationMultiplier(Player player)
	{
		return 2f;
	}

	public override void OnStarted(Player player, ref bool playSound)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		Vector2 center2 = player.Center;
		Vector2 vector4 = default(Vector2);
		((Vector2)(ref vector4))._002Ector(50f, 20f);
		float num12 = (float)Math.PI * 2f * Main.rand.NextFloat();
		for (int i = 0; i < 5; i++)
		{
			for (float num13 = 0f; num13 < 14f; num13 += 1f)
			{
				Dust obj = Main.dust[Dust.NewDust(center2, 0, 0, 6)];
				Vector2 vector5 = Vector2.UnitY.RotatedBy(num13 * ((float)Math.PI * 2f) / 14f + num12);
				vector5 *= 0.2f * (float)i;
				obj.position = center2 + vector5 * vector4;
				obj.velocity = vector5 + new Vector2(0f, player.gravDir * 4f);
				obj.noGravity = true;
				obj.scale = 1f + Main.rand.NextFloat() * 0.8f;
				obj.fadeIn = Main.rand.NextFloat() * 2f;
				obj.shader = GameShaders.Armor.GetSecondaryShader(player.cMount, player);
			}
		}
	}

	public override void UpdateHorizontalSpeeds(Player player)
	{
		player.runAcceleration *= 3f;
		player.maxRunSpeed *= 1.5f;
	}
}
