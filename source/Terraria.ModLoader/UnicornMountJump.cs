using System;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.ModLoader;

public sealed class UnicornMountJump : VanillaExtraJump
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
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		Vector2 center = player.Center;
		Vector2 vector2 = default(Vector2);
		((Vector2)(ref vector2))._002Ector(50f, 20f);
		float num10 = (float)Math.PI * 2f * Main.rand.NextFloat();
		for (int i = 0; i < 5; i++)
		{
			for (float num11 = 0f; num11 < 14f; num11 += 1f)
			{
				Dust obj = Main.dust[Dust.NewDust(center, 0, 0, Utils.SelectRandom<int>(Main.rand, 176, 177, 179))];
				Vector2 vector3 = Vector2.UnitY.RotatedBy(num11 * ((float)Math.PI * 2f) / 14f + num10);
				vector3 *= 0.2f * (float)i;
				obj.position = center + vector3 * vector2;
				obj.velocity = vector3 + new Vector2(0f, player.gravDir * 4f);
				obj.noGravity = true;
				obj.scale = 1f + Main.rand.NextFloat() * 0.8f;
				obj.fadeIn = Main.rand.NextFloat() * 2f;
				obj.shader = GameShaders.Armor.GetSecondaryShader(player.cMount, player);
			}
		}
	}

	public override void ShowVisuals(Player player)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		Dust obj = Main.dust[Dust.NewDust(player.position, player.width, player.height, Utils.SelectRandom<int>(Main.rand, 176, 177, 179))];
		obj.velocity = Vector2.Zero;
		obj.noGravity = true;
		obj.scale = 0.5f + Main.rand.NextFloat() * 0.8f;
		obj.fadeIn = 1f + Main.rand.NextFloat() * 2f;
		obj.shader = GameShaders.Armor.GetSecondaryShader(player.cMount, player);
	}

	public override void UpdateHorizontalSpeeds(Player player)
	{
		player.runAcceleration *= 3f;
		player.maxRunSpeed *= 1.5f;
	}
}
