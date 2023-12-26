using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;

namespace Terraria.ModLoader.Config.UI;

internal class StringInputElement : ConfigElement<string>
{
	public override void OnBind()
	{
		base.OnBind();
		UIPanel textBoxBackground = new UIPanel();
		textBoxBackground.SetPadding(0f);
		UIFocusInputTextField uIInputTextField = new UIFocusInputTextField("Type here");
		textBoxBackground.Top.Set(0f, 0f);
		textBoxBackground.Left.Set(-190f, 1f);
		textBoxBackground.Width.Set(180f, 0f);
		textBoxBackground.Height.Set(30f, 0f);
		Append(textBoxBackground);
		uIInputTextField.SetText(Value);
		uIInputTextField.Top.Set(5f, 0f);
		uIInputTextField.Left.Set(10f, 0f);
		uIInputTextField.Width.Set(-20f, 1f);
		uIInputTextField.Height.Set(20f, 0f);
		uIInputTextField.OnTextChange += delegate
		{
			Value = uIInputTextField.CurrentString;
		};
		textBoxBackground.Append(uIInputTextField);
	}
}
