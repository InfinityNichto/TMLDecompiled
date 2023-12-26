using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Terraria.ModLoader.Default.Patreon;

internal class OrianSetEffectPlayer : ModPlayer
{
	public bool IsActive;

	public override void ResetEffects()
	{
		IsActive = false;
	}

	public override void PostUpdate()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		if (!IsActive)
		{
			return;
		}
		Player player = base.Player;
		Vector2 playerCenter = player.Center;
		Main.npc.Any((NPC x) => x != Main.npc[Main.maxNPCs] && x.active && !x.friendly && !NPCID.Sets.TownCritter[x.type] && x.type != 488 && x.WithinRange(player.position, 300f));
		float maxIntensity = 0f;
		for (int i = 0; i < Main.maxNPCs; i++)
		{
			NPC npc = Main.npc[i];
			if (npc.active && npc.damage > 0 && !npc.friendly && !NPCID.Sets.TownCritter[npc.type] && npc.type != 488)
			{
				float distanceSquared = npc.DistanceSQ(playerCenter);
				float intensity = 1f - MathF.Min(1f, distanceSquared / 262144f);
				intensity *= intensity;
				maxIntensity = MathF.Max(maxIntensity, intensity);
			}
		}
		if (maxIntensity > 0f)
		{
			float pulse = MathF.Sin((float)Main.GameUpdateCount / 17f) * 0.25f + 0.75f;
			Color deepSkyBlue = Color.DeepSkyBlue;
			Lighting.AddLight(playerCenter, ((Color)(ref deepSkyBlue)).ToVector3() * maxIntensity * pulse * 1.5f);
		}
	}
}
