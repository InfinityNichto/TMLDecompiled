using System;
using Microsoft.Xna.Framework;

namespace Terraria.ModLoader.Default.Developer.Jofairden;

internal sealed class JofairdenArmorEffectPlayer : ModPlayer
{
	public bool HasSetBonus;

	public float LayerStrength;

	public float ShaderStrength;

	private int _auraTime;

	public bool HasAura => _auraTime > 0;

	public override void ResetEffects()
	{
		HasSetBonus = false;
	}

	public override void UpdateDead()
	{
		HasSetBonus = false;
		_auraTime = 0;
	}

	public override void PostUpdate()
	{
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		if (!HasAura)
		{
			if (ShaderStrength > 0f)
			{
				ShaderStrength -= 0.02f;
			}
		}
		else if (ShaderStrength <= 1f)
		{
			ShaderStrength += 0.02f;
		}
		else
		{
			_auraTime--;
		}
		if (!HasSetBonus)
		{
			if (LayerStrength > 0f)
			{
				LayerStrength -= 0.02f;
			}
		}
		else if (LayerStrength <= 1f)
		{
			LayerStrength += 0.02f;
		}
		if (ShaderStrength > 0f)
		{
			Vector2 center = base.Player.Center;
			Color discoColor = Main.DiscoColor;
			Lighting.AddLight(center, ((Color)(ref discoColor)).ToVector3() * LayerStrength * ((float)Main.time % 2f) * (float)Math.Abs(Math.Log10(Main.essScale * 0.75f)));
		}
	}

	public override void PostHurt(Player.HurtInfo info)
	{
		if ((float)info.Damage >= 0.1f * (float)base.Player.statLifeMax2)
		{
			_auraTime = 300 + info.Damage;
		}
	}
}
