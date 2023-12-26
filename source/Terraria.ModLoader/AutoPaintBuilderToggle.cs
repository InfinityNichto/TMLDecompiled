using Terraria.Localization;

namespace Terraria.ModLoader;

public class AutoPaintBuilderToggle : VanillaBuilderToggle
{
	public override bool Active()
	{
		return Main.player[Main.myPlayer].autoPaint;
	}

	public override string DisplayValue()
	{
		string text = "";
		switch (base.CurrentState)
		{
		case 0:
			text = Language.GetTextValue("GameUI.PaintSprayerOn");
			break;
		case 1:
			text = Language.GetTextValue("GameUI.PaintSprayerOff");
			break;
		}
		return text;
	}
}
