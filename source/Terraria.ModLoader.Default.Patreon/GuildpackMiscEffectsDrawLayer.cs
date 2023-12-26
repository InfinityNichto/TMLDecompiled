using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.ModLoader.Default.Patreon;

internal class GuildpackMiscEffectsDrawLayer : PlayerDrawLayer
{
	private Asset<Texture2D>? textureAsset;

	public override Position GetDefaultPosition()
	{
		return new BeforeParent(PlayerDrawLayers.JimsCloak);
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
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		if (drawInfo.shadow != 0f)
		{
			return;
		}
		if (textureAsset == null)
		{
			textureAsset = ModContent.Request<Texture2D>("ModLoader/Patreon.Guildpack_Aura");
		}
		if (textureAsset.IsLoaded)
		{
			Texture2D texture = textureAsset.Value;
			if (texture != null)
			{
				Player player = drawInfo.drawPlayer;
				int frameSize = texture.Height / 3;
				int frame = DateTime.Now.Millisecond / 167 % 3;
				Vector2 position = (drawInfo.Position + player.Size * 0.5f - Main.screenPosition).ToPoint().ToVector2();
				Rectangle srcRect = default(Rectangle);
				((Rectangle)(ref srcRect))._002Ector(0, frameSize * frame, texture.Width, frameSize);
				drawInfo.DrawDataCache.Add(new DrawData(texture, position, srcRect, Color.White, 0f, new Vector2((float)texture.Width, (float)frameSize) * 0.5f, 1f, drawInfo.playerEffect));
			}
		}
	}
}
