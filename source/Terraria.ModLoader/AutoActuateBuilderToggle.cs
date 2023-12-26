using Terraria.Localization;

namespace Terraria.ModLoader;

public class AutoActuateBuilderToggle : VanillaBuilderToggle
{
	public override bool Active()
	{
		return Main.player[Main.myPlayer].autoActuator;
	}

	public override string DisplayValue()
	{
		string text = "";
		switch (base.CurrentState)
		{
		case 0:
			text = Language.GetTextValue("GameUI.ActuationDeviceOn");
			break;
		case 1:
			text = Language.GetTextValue("GameUI.ActuationDeviceOff");
			break;
		}
		return text;
	}
}
