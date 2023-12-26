using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.ModLoader.Default.Patreon;

internal class HER0zeroGlowEffect : PlayerDrawLayer
{
	private Asset<Texture2D>? textureAsset;

	public override Position GetDefaultPosition()
	{
		return new BeforeParent(PlayerDrawLayers.JimsCloak);
	}

	public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
	{
		if (drawInfo.drawPlayer.TryGetModPlayer<HER0zeroPlayer>(out var modPlayer))
		{
			return modPlayer.glowEffect;
		}
		return false;
	}

	protected override void Draw(ref PlayerDrawSet drawInfo)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		if (drawInfo.shadow != 0f)
		{
			return;
		}
		if (textureAsset == null)
		{
			textureAsset = ModContent.Request<Texture2D>("ModLoader/Patreon.HER0zero_Effect");
		}
		if (textureAsset.IsLoaded)
		{
			Texture2D texture = textureAsset.Value;
			if (texture != null)
			{
				Player player = drawInfo.drawPlayer;
				int frameSize = texture.Height / 4;
				int frame = player.miscCounter % 40 / 10;
				float alpha = 0.5f;
				Vector2 position = (drawInfo.Position + player.Size * 0.5f - Main.screenPosition).ToPoint().ToVector2();
				Rectangle srcRect = default(Rectangle);
				((Rectangle)(ref srcRect))._002Ector(0, frameSize * frame, texture.Width, frameSize);
				drawInfo.DrawDataCache.Add(new DrawData(texture, position, srcRect, Color.White * alpha, 0f, new Vector2((float)texture.Width, (float)frameSize) * 0.5f, 1f, drawInfo.playerEffect));
			}
		}
	}
}
