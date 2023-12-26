using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Elements;

public class UIKeybindingSimpleListItem : UIElement
{
	private Color _color;

	private Func<string> _GetTextFunction;

	public UIKeybindingSimpleListItem(Func<string> getText, Color color)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		_color = color;
		_GetTextFunction = ((getText != null) ? getText : ((Func<string>)(() => "???")));
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		float num = 6f;
		base.DrawSelf(spriteBatch);
		CalculatedStyle dimensions = GetDimensions();
		float num2 = dimensions.Width + 1f;
		Vector2 val = new Vector2(dimensions.X, dimensions.Y);
		Vector2 baseScale = default(Vector2);
		((Vector2)(ref baseScale))._002Ector(0.8f);
		Color value = (base.IsMouseHovering ? Color.White : Color.Silver);
		value = Color.Lerp(value, Color.White, base.IsMouseHovering ? 0.5f : 0f);
		Color color = (base.IsMouseHovering ? _color : _color.MultiplyRGBA(new Color(180, 180, 180)));
		Vector2 position = val;
		Utils.DrawSettings2Panel(spriteBatch, position, num2, color);
		position.X += 8f;
		position.Y += 2f + num;
		string text = _GetTextFunction();
		Vector2 stringSize = ChatManager.GetStringSize(FontAssets.ItemStack.Value, text, baseScale);
		if (stringSize.X > dimensions.Width)
		{
			float scaleToFit = dimensions.Width / stringSize.X;
			stringSize.X *= scaleToFit;
			baseScale.X *= scaleToFit;
		}
		position.X = dimensions.X + dimensions.Width / 2f - stringSize.X / 2f;
		ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, text, position, value, 0f, Vector2.Zero, baseScale, num2);
	}
}
