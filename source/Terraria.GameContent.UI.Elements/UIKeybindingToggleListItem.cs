using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Elements;

public class UIKeybindingToggleListItem : UIElement
{
	private Color _color;

	private Func<string> _TextDisplayFunction;

	private Func<bool> _IsOnFunction;

	private Asset<Texture2D> _toggleTexture;

	public UIKeybindingToggleListItem(Func<string> getText, Func<bool> getStatus, Color color)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		_color = color;
		_toggleTexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Toggle");
		_TextDisplayFunction = ((getText != null) ? getText : ((Func<string>)(() => "???")));
		_IsOnFunction = ((getStatus != null) ? getStatus : ((Func<bool>)(() => false)));
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
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
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
		Utils.DrawSettingsPanel(spriteBatch, position, num2, color);
		position.X += 8f;
		position.Y += 2f + num;
		ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, _TextDisplayFunction(), position, value, 0f, Vector2.Zero, baseScale, num2);
		position.X -= 17f;
		Rectangle value2 = default(Rectangle);
		((Rectangle)(ref value2))._002Ector(_IsOnFunction() ? ((_toggleTexture.Width() - 2) / 2 + 2) : 0, 0, (_toggleTexture.Width() - 2) / 2, _toggleTexture.Height());
		Vector2 vector2 = default(Vector2);
		((Vector2)(ref vector2))._002Ector((float)value2.Width, 0f);
		Vector2 val2 = default(Vector2);
		((Vector2)(ref val2))._002Ector(dimensions.X + dimensions.Width - vector2.X - 10f, dimensions.Y + 2f + num);
		spriteBatch.Draw(_toggleTexture.Value, val2, (Rectangle?)value2, Color.White, 0f, Vector2.Zero, Vector2.One, (SpriteEffects)0, 0f);
	}
}
