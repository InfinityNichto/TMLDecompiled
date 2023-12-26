using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.ModLoader;

public class TorchBiomeBuilderToggle : VanillaBuilderToggle
{
	public override string Texture => "Terraria/Images/Extra_211";

	public override bool Active()
	{
		return Main.player[Main.myPlayer].unlockedBiomeTorches;
	}

	public override string DisplayValue()
	{
		string text = "";
		switch (base.CurrentState)
		{
		case 0:
			text = Language.GetTextValue("GameUI.TorchTypeSwapperOn");
			break;
		case 1:
			text = Language.GetTextValue("GameUI.TorchTypeSwapperOff");
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
