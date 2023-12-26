using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.ModLoader.Default.Developer.Jofairden;

internal class JofairdenHeadGlow : JofairdenArmorGlowLayer
{
	private Asset<Texture2D> _glowTexture;

	public override bool IsHeadLayer => true;

	public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
	{
		if (drawInfo.drawPlayer.head == ModContent.GetInstance<Jofairden_Head>().Item.headSlot)
		{
			return base.GetDefaultVisibility(drawInfo);
		}
		return false;
	}

	public override DrawDataInfo GetData(PlayerDrawSet info)
	{
		if (_glowTexture == null)
		{
			_glowTexture = ModContent.Request<Texture2D>("ModLoader/Developer.Jofairden.Jofairden_Head_Head_Glow");
		}
		return JofairdenArmorDrawLayer.GetHeadDrawDataInfo(info, _glowTexture.Value);
	}

	public override Position GetDefaultPosition()
	{
		return new AfterParent(PlayerDrawLayers.Head);
	}
}
