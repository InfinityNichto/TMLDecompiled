using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Terraria.ModLoader.UI;

internal class UIProgressBar : UIPanel
{
	private string _cachedText = "";

	private UIAutoScaleTextTextPanel<string> _textPanel;

	public string DisplayText
	{
		get
		{
			return _textPanel?.Text ?? _cachedText;
		}
		set
		{
			if (_textPanel == null)
			{
				_cachedText = value;
			}
			else
			{
				_textPanel.SetText(value ?? _textPanel.Text);
			}
		}
	}

	public float Progress { get; private set; }

	public override void OnInitialize()
	{
		_textPanel = new UIAutoScaleTextTextPanel<string>(_cachedText ?? "", 1f, large: true)
		{
			Top = 
			{
				Pixels = 10f
			},
			HAlign = 0.5f,
			Width = 
			{
				Percent = 1f
			},
			Height = 
			{
				Pixels = 60f
			},
			DrawPanel = false
		};
		Append(_textPanel);
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		if (!string.IsNullOrEmpty(_cachedText) && _textPanel != null)
		{
			_textPanel.SetText(_cachedText);
			_cachedText = string.Empty;
		}
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		CalculatedStyle space = GetInnerDimensions();
		spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)space.X + 10, (int)space.Y + (int)space.Height / 2 + 20, (int)space.Width - 20, 10), (Rectangle?)new Rectangle(0, 0, 1, 1), new Color(0, 0, 70));
		spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)space.X + 10, (int)space.Y + (int)space.Height / 2 + 20, (int)((space.Width - 20f) * Progress), 10), (Rectangle?)new Rectangle(0, 0, 1, 1), new Color(200, 200, 70));
	}

	public void UpdateProgress(float value)
	{
		Progress = value;
	}
}
