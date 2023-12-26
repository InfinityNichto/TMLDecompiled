using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.ModLoader;

public class BlockSwapBuilderToggle : VanillaBuilderToggle
{
	public override string Texture => "Terraria/Images/UI/BlockReplace_0";

	public override bool Active()
	{
		return true;
	}

	public override string DisplayValue()
	{
		string text = "";
		switch (base.CurrentState)
		{
		case 0:
			text = Language.GetTextValue("GameUI.BlockReplacerOn");
			break;
		case 1:
			text = Language.GetTextValue("GameUI.BlockReplacerOff");
			break;
		}
		return text;
	}

	public override Color DisplayColorTexture()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		return Color.White;
	}
}
