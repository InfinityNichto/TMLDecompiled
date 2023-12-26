using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.UI.States;
using Terraria.IO;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIWorkshopPublishResourcePackListItem : UIPanel
{
	private ResourcePack _data;

	private Asset<Texture2D> _dividerTexture;

	private Asset<Texture2D> _workshopIconTexture;

	private Asset<Texture2D> _iconBorderTexture;

	private Asset<Texture2D> _innerPanelTexture;

	private UIElement _iconArea;

	private UIElement _publishButton;

	private int _orderInList;

	private UIState _ownerState;

	private const int ICON_SIZE = 64;

	private const int ICON_BORDER_PADDING = 4;

	private const int HEIGHT_FLUFF = 10;

	private bool _canPublish;

	public UIWorkshopPublishResourcePackListItem(UIState ownerState, ResourcePack data, int orderInList, bool canBePublished)
	{
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		_ownerState = ownerState;
		_orderInList = orderInList;
		_data = data;
		_canPublish = canBePublished;
		LoadTextures();
		InitializeAppearance();
		UIElement uIElement = new UIElement
		{
			Width = new StyleDimension(72f, 0f),
			Height = new StyleDimension(72f, 0f),
			Left = new StyleDimension(4f, 0f),
			VAlign = 0.5f
		};
		uIElement.OnLeftDoubleClick += PublishButtonClick_ImportResourcePackToLocalFiles;
		UIImage element = new UIImage(data.Icon)
		{
			Width = new StyleDimension(-6f, 1f),
			Height = new StyleDimension(-6f, 1f),
			HAlign = 0.5f,
			VAlign = 0.5f,
			ScaleToFit = true,
			AllowResizingDimensions = false,
			IgnoresMouseInteraction = true
		};
		UIImage element2 = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders"))
		{
			HAlign = 0.5f,
			VAlign = 0.5f,
			IgnoresMouseInteraction = true
		};
		uIElement.Append(element);
		uIElement.Append(element2);
		Append(uIElement);
		_iconArea = uIElement;
		_publishButton = new UIIconTextButton(Language.GetText("Workshop.Publish"), Color.White, "Images/UI/Workshop/Publish");
		_publishButton.HAlign = 1f;
		_publishButton.VAlign = 1f;
		_publishButton.OnLeftClick += PublishButtonClick_ImportResourcePackToLocalFiles;
		base.OnLeftDoubleClick += PublishButtonClick_ImportResourcePackToLocalFiles;
		Append(_publishButton);
		_publishButton.SetSnapPoint("Publish", orderInList);
	}

	private void LoadTextures()
	{
		_dividerTexture = Main.Assets.Request<Texture2D>("Images/UI/Divider");
		_innerPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground");
		_iconBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders");
		_workshopIconTexture = TextureAssets.Extra[243];
	}

	private void InitializeAppearance()
	{
		Height.Set(82f, 0f);
		Width.Set(0f, 1f);
		SetPadding(6f);
		SetColorsToNotHovered();
	}

	private void SetColorsToHovered()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		BackgroundColor = new Color(73, 94, 171);
		BorderColor = new Color(89, 116, 213);
		if (!_canPublish)
		{
			BorderColor = new Color(150, 150, 150) * 1f;
			BackgroundColor = Color.Lerp(BackgroundColor, new Color(120, 120, 120), 0.5f) * 1f;
		}
	}

	private void SetColorsToNotHovered()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		BackgroundColor = new Color(63, 82, 151) * 0.7f;
		BorderColor = new Color(89, 116, 213) * 0.7f;
		if (!_canPublish)
		{
			BorderColor = new Color(127, 127, 127) * 0.7f;
			BackgroundColor = Color.Lerp(new Color(63, 82, 151), new Color(80, 80, 80), 0.5f) * 0.7f;
		}
	}

	private void PublishButtonClick_ImportResourcePackToLocalFiles(UIMouseEvent evt, UIElement listeningElement)
	{
		if (listeningElement == evt.Target && !TryMovingToRejectionMenuIfNeeded())
		{
			Main.MenuUI.SetState(new WorkshopPublishInfoStateForResourcePack(_ownerState, _data));
		}
	}

	private bool TryMovingToRejectionMenuIfNeeded()
	{
		if (!_canPublish)
		{
			SoundEngine.PlaySound(10);
			Main.instance.RejectionMenuInfo = new RejectionMenuInfo
			{
				TextToShow = Language.GetTextValue("Workshop.ReportIssue_CannotPublishZips"),
				ExitAction = RejectionMenuExitAction
			};
			Main.menuMode = 1000001;
			return true;
		}
		return false;
	}

	private void RejectionMenuExitAction()
	{
		SoundEngine.PlaySound(11);
		if (_ownerState == null)
		{
			Main.menuMode = 0;
			return;
		}
		Main.menuMode = 888;
		Main.MenuUI.SetState(_ownerState);
	}

	public override int CompareTo(object obj)
	{
		if (obj is UIWorkshopPublishResourcePackListItem uIWorkshopPublishResourcePackListItem)
		{
			return _orderInList.CompareTo(uIWorkshopPublishResourcePackListItem._orderInList);
		}
		return base.CompareTo(obj);
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		base.MouseOver(evt);
		SetColorsToHovered();
	}

	public override void MouseOut(UIMouseEvent evt)
	{
		base.MouseOut(evt);
		SetColorsToNotHovered();
	}

	private void DrawPanel(SpriteBatch spriteBatch, Vector2 position, float width, float height)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		Utils.DrawSplicedPanel(spriteBatch, _innerPanelTexture.Value, (int)position.X, (int)position.Y, (int)width, (int)height, 10, 10, 10, 10, Color.White);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		CalculatedStyle innerDimensions = GetInnerDimensions();
		CalculatedStyle dimensions = _iconArea.GetDimensions();
		float num = dimensions.X + dimensions.Width;
		Color white = Color.White;
		Utils.DrawBorderString(spriteBatch, _data.Name, new Vector2(num + 8f, innerDimensions.Y + 3f), white);
		float num6 = (innerDimensions.Width - 22f - dimensions.Width - _publishButton.GetDimensions().Width) / 2f;
		float height = _publishButton.GetDimensions().Height;
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector(num + 8f, innerDimensions.Y + innerDimensions.Height - height);
		float num2 = num6;
		DrawPanel(spriteBatch, vector, num2, height);
		string textValue = Language.GetTextValue("UI.Author", _data.Author);
		Color white2 = Color.White;
		Vector2 val = FontAssets.MouseText.Value.MeasureString(textValue);
		float x = val.X;
		float y = val.Y;
		float x2 = num2 * 0.5f - x * 0.5f;
		float num3 = height * 0.5f - y * 0.5f;
		Utils.DrawBorderString(spriteBatch, textValue, vector + new Vector2(x2, num3 + 3f), white2);
		vector.X += num2 + 5f;
		float num4 = num6;
		DrawPanel(spriteBatch, vector, num4, height);
		string textValue2 = Language.GetTextValue("UI.Version", _data.Version.GetFormattedVersion());
		Color white3 = Color.White;
		Vector2 val2 = FontAssets.MouseText.Value.MeasureString(textValue2);
		float x3 = val2.X;
		float y2 = val2.Y;
		float x4 = num4 * 0.5f - x3 * 0.5f;
		float num5 = height * 0.5f - y2 * 0.5f;
		Utils.DrawBorderString(spriteBatch, textValue2, vector + new Vector2(x4, num5 + 3f), white3);
		vector.X += num4 + 5f;
	}
}
