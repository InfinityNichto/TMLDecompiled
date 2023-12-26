using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UICharacterNameButton : UIElement
{
	private readonly Asset<Texture2D> _BasePanelTexture;

	private readonly Asset<Texture2D> _selectedBorderTexture;

	private readonly Asset<Texture2D> _hoveredBorderTexture;

	private bool _hovered;

	private bool _soundedHover;

	private readonly LocalizedText _textToShowWhenEmpty;

	private string actualContents;

	private UIText _text;

	private UIText _title;

	public readonly LocalizedText Description;

	public float DistanceFromTitleToOption = 20f;

	public UICharacterNameButton(LocalizedText titleText, LocalizedText emptyContentText, LocalizedText description = null)
	{
		Width = StyleDimension.FromPixels(400f);
		Height = StyleDimension.FromPixels(40f);
		Description = description;
		_BasePanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanel");
		_selectedBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight");
		_hoveredBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder");
		_textToShowWhenEmpty = emptyContentText;
		float textScale = 1f;
		UIText uIText = new UIText(titleText, textScale)
		{
			HAlign = 0f,
			VAlign = 0.5f,
			Left = StyleDimension.FromPixels(10f)
		};
		Append(uIText);
		_title = uIText;
		UIText uIText2 = new UIText(Language.GetText("UI.PlayerNameSlot"), textScale)
		{
			HAlign = 0f,
			VAlign = 0.5f,
			Left = StyleDimension.FromPixels(150f)
		};
		Append(uIText2);
		_text = uIText2;
		SetContents(null);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		if (_hovered)
		{
			if (!_soundedHover)
			{
				SoundEngine.PlaySound(12);
			}
			_soundedHover = true;
		}
		else
		{
			_soundedHover = false;
		}
		CalculatedStyle dimensions = GetDimensions();
		Utils.DrawSplicedPanel(spriteBatch, _BasePanelTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 10, 10, 10, 10, Color.White * 0.5f);
		if (_hovered)
		{
			Utils.DrawSplicedPanel(spriteBatch, _hoveredBorderTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 10, 10, 10, 10, Color.White);
		}
	}

	public void SetContents(string name)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		actualContents = name;
		if (string.IsNullOrEmpty(actualContents))
		{
			_text.TextColor = Color.Gray;
			_text.SetText(_textToShowWhenEmpty);
		}
		else
		{
			_text.TextColor = Color.White;
			_text.SetText(actualContents);
		}
		_text.Left = StyleDimension.FromPixels(_title.GetInnerDimensions().Width + DistanceFromTitleToOption);
	}

	public void TrimDisplayIfOverElementDimensions(int padding)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = GetDimensions();
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
		((Point)(ref point4))._002Ector(point3.X + (int)dimensions2.Width, point3.Y + (int)dimensions2.Height);
		Rectangle rectangle2 = default(Rectangle);
		((Rectangle)(ref rectangle2))._002Ector(point3.X, point3.Y, point4.X - point3.X, point4.Y - point3.Y);
		int num = 0;
		while (((Rectangle)(ref rectangle2)).Right > ((Rectangle)(ref rectangle)).Right - padding)
		{
			_text.SetText(_text.Text.Substring(0, _text.Text.Length - 1));
			num++;
			RecalculateChildren();
			dimensions2 = _text.GetDimensions();
			((Point)(ref point3))._002Ector((int)dimensions2.X, (int)dimensions2.Y);
			((Point)(ref point4))._002Ector(point3.X + (int)dimensions2.Width, point3.Y + (int)dimensions2.Height);
			((Rectangle)(ref rectangle2))._002Ector(point3.X, point3.Y, point4.X - point3.X, point4.Y - point3.Y);
		}
		if (num > 0)
		{
			_text.SetText(_text.Text.Substring(0, _text.Text.Length - 1) + "…");
		}
	}

	public override void LeftMouseDown(UIMouseEvent evt)
	{
		base.LeftMouseDown(evt);
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		base.MouseOver(evt);
		_hovered = true;
	}

	public override void MouseOut(UIMouseEvent evt)
	{
		base.MouseOut(evt);
		_hovered = false;
	}
}
