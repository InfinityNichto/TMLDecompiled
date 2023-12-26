using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.ModLoader.Default.Patreon;

internal class xAqultWingLayer : PlayerDrawLayer
{
	public override Position GetDefaultPosition()
	{
		return new AfterParent(PlayerDrawLayers.Wings);
	}

	public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
	{
		return drawInfo.drawPlayer.wings == EquipLoader.GetEquipSlot(base.Mod, "xAqult_Wings", EquipType.Wings);
	}

	protected override void Draw(ref PlayerDrawSet drawInfo)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		if (drawInfo.drawPlayer.dead)
		{
			return;
		}
		DrawData? wingData = null;
		foreach (DrawData data in drawInfo.DrawDataCache)
		{
			if (data.texture == ModContent.Request<Texture2D>("ModLoader/Patreon.xAqult_Wings_Wings", AssetRequestMode.ImmediateLoad).Value)
			{
				wingData = data;
			}
		}
		if (wingData.HasValue)
		{
			DrawData glow = new DrawData(ModContent.Request<Texture2D>("ModLoader/Patreon.xAqult_Wings_Wings_Glow", AssetRequestMode.ImmediateLoad).Value, color: Color.White * drawInfo.stealth * (1f - drawInfo.shadow), position: wingData.Value.position, sourceRect: wingData.Value.sourceRect, rotation: wingData.Value.rotation, origin: wingData.Value.origin, scale: wingData.Value.scale, effect: wingData.Value.effect);
			glow.shader = wingData.Value.shader;
			drawInfo.DrawDataCache.Add(glow);
		}
	}
}
