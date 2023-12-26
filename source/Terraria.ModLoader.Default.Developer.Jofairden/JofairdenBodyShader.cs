using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.ModLoader.Default.Developer.Jofairden;

internal class JofairdenBodyShader : JofairdenArmorShaderLayer
{
	private Asset<Texture2D> _shaderTexture;

	public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
	{
		if (drawInfo.drawPlayer.body == ModContent.GetInstance<Jofairden_Body>().Item.bodySlot)
		{
			return base.GetDefaultVisibility(drawInfo);
		}
		return false;
	}

	public override DrawDataInfo GetData(PlayerDrawSet info)
	{
		if (_shaderTexture == null)
		{
			_shaderTexture = ModContent.Request<Texture2D>("ModLoader/Developer.Jofairden.Jofairden_Body_Body_Shader");
		}
		return JofairdenArmorDrawLayer.GetBodyDrawDataInfo(info, _shaderTexture.Value);
	}

	public override Position GetDefaultPosition()
	{
		return new BeforeParent(PlayerDrawLayers.Torso);
	}
}
