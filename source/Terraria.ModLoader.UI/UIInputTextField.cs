using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria.GameInput;
using Terraria.Initializers;
using Terraria.UI;

namespace Terraria.ModLoader.UI;

internal class UIInputTextField : UIElement
{
	public delegate void EventHandler(object sender, EventArgs e);

	private readonly string _hintText;

	private string _currentString = string.Empty;

	private int _textBlinkerCount;

	public string Text
	{
		get
		{
			return _currentString;
		}
		set
		{
			if (_currentString != value)
			{
				_currentString = value;
				this.OnTextChange?.Invoke(this, EventArgs.Empty);
			}
		}
	}

	public event EventHandler OnTextChange;

	public UIInputTextField(string hintText)
	{
		_hintText = hintText;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)27) && !((KeyboardState)(ref Main.oldKeyState)).IsKeyDown((Keys)27))
		{
			UILinksInitializer.FancyExit();
		}
		PlayerInput.WritingText = true;
		Main.instance.HandleIME();
		string newString = Main.GetInputText(_currentString);
		if (newString != _currentString)
		{
			_currentString = newString;
			this.OnTextChange?.Invoke(this, EventArgs.Empty);
		}
		string displayString = _currentString;
		if (++_textBlinkerCount / 20 % 2 == 0)
		{
			displayString += "|";
		}
		CalculatedStyle space = GetDimensions();
		if (_currentString.Length == 0)
		{
			Utils.DrawBorderString(spriteBatch, _hintText, new Vector2(space.X, space.Y), Color.Gray);
		}
		else
		{
			Utils.DrawBorderString(spriteBatch, displayString, new Vector2(space.X, space.Y), Color.White);
		}
	}
}
