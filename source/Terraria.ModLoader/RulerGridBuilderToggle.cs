using Terraria.Localization;

namespace Terraria.ModLoader;

public class RulerGridBuilderToggle : VanillaBuilderToggle
{
	public override bool Active()
	{
		return Main.player[Main.myPlayer].rulerGrid;
	}

	public override string DisplayValue()
	{
		string text = "";
		switch (base.CurrentState)
		{
		case 0:
			text = Language.GetTextValue("GameUI.MechanicalRulerOn");
			break;
		case 1:
			text = Language.GetTextValue("GameUI.MechanicalRulerOff");
			break;
		}
		return text;
	}
}
