using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.ModLoader.Default.Developer.Jofairden;

internal abstract class JofairdenArmorGlowLayer : JofairdenArmorDrawLayer
{
	protected override void Draw(ref PlayerDrawSet drawInfo)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		DrawDataInfo drawDataInfo = GetData(drawInfo);
		Player drawPlayer = drawInfo.drawPlayer;
		JofairdenArmorEffectPlayer modPlayer = drawPlayer.GetModPlayer<JofairdenArmorEffectPlayer>();
		SpriteEffects effects = (SpriteEffects)0;
		if (drawPlayer.direction == -1)
		{
			effects = (SpriteEffects)(effects | 1);
		}
		if (drawPlayer.gravDir == -1f)
		{
			effects = (SpriteEffects)(effects | 2);
		}
		DrawData data = new DrawData(drawDataInfo.Texture, drawDataInfo.Position, drawDataInfo.Frame, Color.White * Main.essScale * modPlayer.LayerStrength, drawDataInfo.Rotation, drawDataInfo.Origin, 1f, effects);
		if (modPlayer.HasAura)
		{
			int valueOrDefault = JofairdenArmorDrawLayer.ShaderId.GetValueOrDefault();
			if (!JofairdenArmorDrawLayer.ShaderId.HasValue)
			{
				valueOrDefault = GameShaders.Armor.GetShaderIdFromItemId(2870);
				JofairdenArmorDrawLayer.ShaderId = valueOrDefault;
			}
			data.shader = JofairdenArmorDrawLayer.ShaderId.Value;
		}
		drawInfo.DrawDataCache.Add(data);
	}
}
