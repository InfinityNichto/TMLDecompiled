using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.ModLoader.Default.Patreon;

internal class SaetharEyeGlowDrawLayer : PlayerDrawLayer
{
	private Asset<Texture2D>? textureAsset;

	public override Position GetDefaultPosition()
	{
		return new AfterParent(PlayerDrawLayers.Head);
	}

	public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
	{
		if (drawInfo.drawPlayer.TryGetModPlayer<SaetharSetEffectPlayer>(out var modPlayer))
		{
			return modPlayer.IsActive;
		}
		return false;
	}

	protected override void Draw(ref PlayerDrawSet drawInfo)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		if (drawInfo.shadow != 0f)
		{
			return;
		}
		if (textureAsset == null)
		{
			textureAsset = ModContent.Request<Texture2D>("ModLoader/Patreon.Saethar_Head_Glow");
		}
		Asset<Texture2D> asset = textureAsset;
		if (asset != null && asset.IsLoaded)
		{
			Texture2D texture = asset.Value;
			if (texture != null)
			{
				Vector2 headOrigin = drawInfo.headVect;
				Player player = drawInfo.drawPlayer;
				Vector2 position = player.headPosition + drawInfo.headVect + new Vector2((float)(int)(drawInfo.Position.X + (float)player.width / 2f - (float)player.bodyFrame.Width / 2f - Main.screenPosition.X), (float)(int)(drawInfo.Position.Y + (float)player.height - (float)player.bodyFrame.Height + 4f - Main.screenPosition.Y));
				drawInfo.DrawDataCache.Add(new DrawData(texture, position, player.bodyFrame, Color.White, player.headRotation, headOrigin, 1f, drawInfo.playerEffect));
			}
		}
	}
}
