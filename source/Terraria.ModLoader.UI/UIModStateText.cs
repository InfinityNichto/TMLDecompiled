using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.ModLoader.UI;

internal class UIModStateText : UIElement
{
	private bool _enabled;

	private string DisplayText
	{
		get
		{
			if (!_enabled)
			{
				return Language.GetTextValue("GameUI.Disabled");
			}
			return Language.GetTextValue("GameUI.Enabled");
		}
	}

	private Color DisplayColor
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			if (!_enabled)
			{
				return Color.Red;
			}
			return Color.Green;
		}
	}

	public UIModStateText(bool enabled = true)
	{
		_enabled = enabled;
		PaddingLeft = (PaddingRight = 5f);
		PaddingBottom = (PaddingTop = 10f);
	}

	public override void OnInitialize()
	{
		base.OnLeftClick += delegate
		{
			if (_enabled)
			{
				SetDisabled();
			}
			else
			{
				SetEnabled();
			}
		};
	}

	public void SetEnabled()
	{
		_enabled = true;
		Recalculate();
	}

	public void SetDisabled()
	{
		_enabled = false;
		Recalculate();
	}

	public override void Recalculate()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		Vector2 textSize = default(Vector2);
		((Vector2)(ref textSize))._002Ector(FontAssets.MouseText.Value.MeasureString(DisplayText).X, 16f);
		Width.Set(textSize.X + PaddingLeft + PaddingRight, 0f);
		Height.Set(textSize.Y + PaddingTop + PaddingBottom, 0f);
		base.Recalculate();
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		base.DrawSelf(spriteBatch);
		DrawPanel(spriteBatch);
		DrawEnabledText(spriteBatch);
	}

	private void DrawPanel(SpriteBatch spriteBatch)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		Vector2 position = GetDimensions().Position();
		float width = Width.Pixels;
		spriteBatch.Draw(UICommon.InnerPanelTexture.Value, position, (Rectangle?)new Rectangle(0, 0, 8, UICommon.InnerPanelTexture.Height()), Color.White);
		spriteBatch.Draw(UICommon.InnerPanelTexture.Value, new Vector2(position.X + 8f, position.Y), (Rectangle?)new Rectangle(8, 0, 8, UICommon.InnerPanelTexture.Height()), Color.White, 0f, Vector2.Zero, new Vector2((width - 16f) / 8f, 1f), (SpriteEffects)0, 0f);
		spriteBatch.Draw(UICommon.InnerPanelTexture.Value, new Vector2(position.X + width - 8f, position.Y), (Rectangle?)new Rectangle(16, 0, 8, UICommon.InnerPanelTexture.Height()), Color.White);
	}

	private void DrawEnabledText(SpriteBatch spriteBatch)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		Vector2 pos = GetDimensions().Position() + new Vector2(PaddingLeft, PaddingTop * 0.5f);
		Utils.DrawBorderString(spriteBatch, DisplayText, pos, DisplayColor);
	}
}
