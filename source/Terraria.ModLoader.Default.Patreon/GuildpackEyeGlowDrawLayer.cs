using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.ModLoader.Default.Patreon;

internal class GuildpackEyeGlowDrawLayer : PlayerDrawLayer
{
	private Asset<Texture2D>? textureAsset;

	public override Position GetDefaultPosition()
	{
		return new AfterParent(PlayerDrawLayers.Head);
	}

	public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
	{
		if (drawInfo.drawPlayer.TryGetModPlayer<GuildpackSetEffectPlayer>(out var modPlayer))
		{
			return modPlayer.IsActive;
		}
		return false;
	}

	protected override void Draw(ref PlayerDrawSet drawInfo)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		if (textureAsset == null)
		{
			textureAsset = ModContent.Request<Texture2D>("ModLoader/Patreon.Guildpack_Head_Glow");
		}
		if (textureAsset.IsLoaded)
		{
			Texture2D texture = textureAsset.Value;
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
