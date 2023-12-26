using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.ModLoader;

[Autoload(false)]
public abstract class WireVisibilityBuilderToggle : VanillaBuilderToggle
{
	public override int NumberOfStates => 3;

	public override bool Active()
	{
		return Main.player[Main.myPlayer].InfoAccMechShowWires;
	}

	public override string DisplayValue()
	{
		string text = "";
		switch (base.Type)
		{
		case 4:
			text = Language.GetTextValue("Game.RedWires");
			break;
		case 5:
			text = Language.GetTextValue("Game.BlueWires");
			break;
		case 6:
			text = Language.GetTextValue("Game.GreenWires");
			break;
		case 7:
			text = Language.GetTextValue("Game.YellowWires");
			break;
		case 9:
			text = Language.GetTextValue("Game.Actuators");
			break;
		}
		string text2 = "";
		switch (base.CurrentState)
		{
		case 0:
			text2 = Language.GetTextValue("GameUI.Bright");
			break;
		case 1:
			text2 = Language.GetTextValue("GameUI.Normal");
			break;
		case 2:
			text2 = Language.GetTextValue("GameUI.Faded");
			break;
		case 3:
			Language.GetTextValue("GameUI.Hidden");
			break;
		}
		return text + ": " + text2;
	}

	public override Color DisplayColorTexture()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		Color color = default(Color);
		switch (base.CurrentState)
		{
		case 0:
			color = Color.White;
			return color;
		case 1:
			((Color)(ref color))._002Ector(127, 127, 127);
			break;
		case 2:
			color = Utils.MultiplyRGBA(new Color(127, 127, 127), new Color(0.66f, 0.66f, 0.66f));
			return color;
		case 3:
			color = Utils.MultiplyRGBA(new Color(127, 127, 127), new Color(0.33f, 0.33f, 0.33f));
			return color;
		}
		return color;
	}
}
