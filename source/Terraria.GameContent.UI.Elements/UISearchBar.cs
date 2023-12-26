using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UISearchBar : UIElement
{
	private readonly LocalizedText _textToShowWhenEmpty;

	private UITextBox _text;

	private string actualContents;

	private float _textScale;

	private bool isWritingText;

	public bool HasContents => !string.IsNullOrWhiteSpace(actualContents);

	public bool IsWritingText => isWritingText;

	public event Action<string> OnContentsChanged;

	public event Action OnStartTakingInput;

	public event Action OnEndTakingInput;

	public event Action OnCanceledTakingInput;

	public event Action OnNeedingVirtualKeyboard;

	public UISearchBar(LocalizedText emptyContentText, float scale)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		_textToShowWhenEmpty = emptyContentText;
		_textScale = scale;
		UITextBox uITextBox = new UITextBox("", scale)
		{
			HAlign = 0f,
			VAlign = 0.5f,
			BackgroundColor = Color.Transparent,
			BorderColor = Color.Transparent,
			Width = new StyleDimension(0f, 1f),
			Height = new StyleDimension(0f, 1f),
			TextHAlign = 0f,
			ShowInputTicker = false
		};
		uITextBox.SetTextMaxLength(50);
		Append(uITextBox);
		_text = uITextBox;
	}

	public void SetContents(string contents, bool forced = false)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		if (!(actualContents == contents) || forced)
		{
			actualContents = contents;
			if (string.IsNullOrEmpty(actualContents))
			{
				_text.TextColor = Color.Gray;
				_text.SetText(_textToShowWhenEmpty.Value, _textScale, large: false);
			}
			else
			{
				_text.TextColor = Color.White;
				_text.SetText(actualContents);
				actualContents = _text.Text;
			}
			TrimDisplayIfOverElementDimensions(0);
			if (this.OnContentsChanged != null)
			{
				this.OnContentsChanged(contents);
			}
		}
	}

	public void TrimDisplayIfOverElementDimensions(int padding)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = GetDimensions();
		if (dimensions.Width != 0f || dimensions.Height != 0f)
		{
			Point point = default(Point);
			((Point)(ref point))._002Ector((int)dimensions.X, (int)dimensions.Y);
			Point point2 = default(Point);
			((Point)(ref point2))._002Ector(point.X + (int)dimensions.Width, point.Y + (int)dimensions.Height);
			Rectangle rectangle = default(Rectangle);
			((Rectangle)(ref rectangle))._002Ector(point.X, point.Y, point2.X - point.X, point2.Y - point.Y);
			CalculatedStyle dimensions2 = _text.GetDimensions();
			Point point3 = default(Point);
			((Point)(ref point3))._002Ector((int)dimensions2.X, (int)dimensions2.Y);
			Point point4 = default(Point);
			((Point)(ref point4))._002Ector(point3.X + (int)_text.MinWidth.Pixels, point3.Y + (int)_text.MinHeight.Pixels);
			Rectangle rectangle2 = default(Rectangle);
			((Rectangle)(ref rectangle2))._002Ector(point3.X, point3.Y, point4.X - point3.X, point4.Y - point3.Y);
			int num = 0;
			while (((Rectangle)(ref rectangle2)).Right > ((Rectangle)(ref rectangle)).Right - padding && _text.Text.Length > 0)
			{
				_text.SetText(_text.Text.Substring(0, _text.Text.Length - 1));
				num++;
				RecalculateChildren();
				dimensions2 = _text.GetDimensions();
				((Point)(ref point3))._002Ector((int)dimensions2.X, (int)dimensions2.Y);
				((Point)(ref point4))._002Ector(point3.X + (int)_text.MinWidth.Pixels, point3.Y + (int)_text.MinHeight.Pixels);
				((Rectangle)(ref rectangle2))._002Ector(point3.X, point3.Y, point4.X - point3.X, point4.Y - point3.Y);
				actualContents = _text.Text;
			}
		}
	}

	public override void LeftMouseDown(UIMouseEvent evt)
	{
		base.LeftMouseDown(evt);
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		base.MouseOver(evt);
		SoundEngine.PlaySound(12);
	}

	public override void Update(GameTime gameTime)
	{
		if (isWritingText)
		{
			if (NeedsVirtualkeyboard())
			{
				if (this.OnNeedingVirtualKeyboard != null)
				{
					this.OnNeedingVirtualKeyboard();
				}
				return;
			}
			PlayerInput.WritingText = true;
			Main.CurrentInputTextTakerOverride = this;
		}
		base.Update(gameTime);
	}

	private bool NeedsVirtualkeyboard()
	{
		return PlayerInput.SettingsForUI.ShowGamepadHints;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		if (!isWritingText)
		{
			return;
		}
		PlayerInput.WritingText = true;
		Main.instance.HandleIME();
		float num = Main.screenWidth / 2;
		Rectangle val = _text.GetDimensions().ToRectangle();
		Vector2 position = default(Vector2);
		((Vector2)(ref position))._002Ector(num, (float)(((Rectangle)(ref val)).Bottom + 32));
		Main.instance.DrawWindowsIMEPanel(position, 0.5f);
		string inputText = Main.GetInputText(actualContents);
		if (Main.inputTextEnter)
		{
			ToggleTakingText();
		}
		else if (Main.inputTextEscape)
		{
			ToggleTakingText();
			if (this.OnCanceledTakingInput != null)
			{
				this.OnCanceledTakingInput();
			}
		}
		SetContents(inputText);
		float num2 = Main.screenWidth / 2;
		val = _text.GetDimensions().ToRectangle();
		((Vector2)(ref position))._002Ector(num2, (float)(((Rectangle)(ref val)).Bottom + 32));
		Main.instance.DrawWindowsIMEPanel(position, 0.5f);
	}

	public void ToggleTakingText()
	{
		isWritingText = !isWritingText;
		_text.ShowInputTicker = isWritingText;
		Main.clrInput();
		if (isWritingText)
		{
			if (this.OnStartTakingInput != null)
			{
				this.OnStartTakingInput();
			}
		}
		else if (this.OnEndTakingInput != null)
		{
			this.OnEndTakingInput();
		}
	}
}
