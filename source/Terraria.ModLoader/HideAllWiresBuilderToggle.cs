using Terraria.Localization;

namespace Terraria.ModLoader;

public class HideAllWiresBuilderToggle : WireVisibilityBuilderToggle
{
	public override int NumberOfStates => 2;

	public override string DisplayValue()
	{
		string text = "";
		switch (base.CurrentState)
		{
		case 0:
			text = Language.GetTextValue("GameUI.WireModeForced");
			break;
		case 1:
			text = Language.GetTextValue("GameUI.WireModeNormal");
			break;
		}
		return text;
	}
}
