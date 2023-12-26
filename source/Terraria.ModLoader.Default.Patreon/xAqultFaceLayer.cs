using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace Terraria.ModLoader.Default.Patreon;

internal class xAqultFaceLayer : PlayerDrawLayer
{
	public override Position GetDefaultPosition()
	{
		return new AfterParent(PlayerDrawLayers.Head);
	}

	public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
	{
		if (drawInfo.drawPlayer.face != EquipLoader.GetEquipSlot(base.Mod, "xAqult_Lens", EquipType.Face))
		{
			return drawInfo.drawPlayer.face == EquipLoader.GetEquipSlot(base.Mod, "xAqult_Lens_Blue", EquipType.Face);
		}
		return true;
	}

	protected override void Draw(ref PlayerDrawSet drawInfo)
	{
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		int insertIndex = -1;
		for (int j = 0; j < drawInfo.DrawDataCache.Count; j++)
		{
			if (drawInfo.DrawDataCache[j].texture == TextureAssets.Players[drawInfo.skinVar, 2].Value)
			{
				insertIndex = j + 1;
				break;
			}
		}
		if (insertIndex < 0)
		{
			return;
		}
		for (int i = insertIndex; i < drawInfo.DrawDataCache.Count; i++)
		{
			DrawData data = drawInfo.DrawDataCache[i];
			if (data.texture == ModContent.Request<Texture2D>("ModLoader/Patreon.xAqult_Lens_Face", AssetRequestMode.ImmediateLoad).Value)
			{
				drawInfo.DrawDataCache.RemoveAt(i);
				drawInfo.DrawDataCache.Insert(insertIndex, data);
				data.texture = ModContent.Request<Texture2D>("ModLoader/Patreon.xAqult_Lens_Face_Glow", AssetRequestMode.ImmediateLoad).Value;
				data.color = drawInfo.drawPlayer.GetImmuneAlphaPure(Color.White, drawInfo.shadow);
				drawInfo.DrawDataCache.Insert(insertIndex + 1, data);
				break;
			}
			if (data.texture == ModContent.Request<Texture2D>("ModLoader/Patreon.xAqult_Lens_Blue_Face", AssetRequestMode.ImmediateLoad).Value)
			{
				drawInfo.DrawDataCache.RemoveAt(i);
				drawInfo.DrawDataCache.Insert(insertIndex, data);
				data.texture = ModContent.Request<Texture2D>("ModLoader/Patreon.xAqult_Lens_Blue_Face_Glow", AssetRequestMode.ImmediateLoad).Value;
				data.color = drawInfo.drawPlayer.GetImmuneAlphaPure(Color.White, drawInfo.shadow);
				drawInfo.DrawDataCache.Insert(insertIndex + 1, data);
				break;
			}
		}
	}
}
