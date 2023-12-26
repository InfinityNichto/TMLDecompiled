using Terraria.Localization;

namespace Terraria.ModLoader;

public class RulerLineBuilderToggle : VanillaBuilderToggle
{
	public override bool Active()
	{
		return Main.player[Main.myPlayer].rulerLine;
	}

	public override string DisplayValue()
	{
		string text = "";
		switch (base.CurrentState)
		{
		case 0:
			text = Language.GetTextValue("GameUI.RulerOn");
			break;
		case 1:
			text = Language.GetTextValue("GameUI.RulerOff");
			break;
		}
		return text;
	}
}
