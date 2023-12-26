using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.States;
using Terraria.IO;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIWorkshopPublishWorldListItem : AWorldListItem
{
	private Asset<Texture2D> _workshopIconTexture;

	private Asset<Texture2D> _innerPanelTexture;

	private UIElement _worldIcon;

	private UIElement _publishButton;

	private int _orderInList;

	private UIState _ownerState;

	public UIWorkshopPublishWorldListItem(UIState ownerState, WorldFileData data, int orderInList)
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		_ownerState = ownerState;
		_orderInList = orderInList;
		_data = data;
		LoadTextures();
		InitializeAppearance();
		_worldIcon = GetIconElement();
		_worldIcon.Left.Set(4f, 0f);
		_worldIcon.VAlign = 0.5f;
		_worldIcon.OnLeftDoubleClick += PublishButtonClick_ImportWorldToLocalFiles;
		Append(_worldIcon);
		_publishButton = new UIIconTextButton(Language.GetText("Workshop.Publish"), Color.White, "Images/UI/Workshop/Publish");
		_publishButton.HAlign = 1f;
		_publishButton.VAlign = 1f;
		_publishButton.OnLeftClick += PublishButtonClick_ImportWorldToLocalFiles;
		base.OnLeftDoubleClick += PublishButtonClick_ImportWorldToLocalFiles;
		Append(_publishButton);
		_publishButton.SetSnapPoint("Publish", orderInList);
	}

	private void LoadTextures()
	{
		_innerPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground");
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
		BackgroundColor = new Color(73, 94, 171);
		BorderColor = new Color(89, 116, 213);
	}

	private void SetColorsToNotHovered()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		BackgroundColor = new Color(63, 82, 151) * 0.7f;
		BorderColor = new Color(89, 116, 213) * 0.7f;
	}

	private void PublishButtonClick_ImportWorldToLocalFiles(UIMouseEvent evt, UIElement listeningElement)
	{
		if (listeningElement == evt.Target)
		{
			Main.MenuUI.SetState(new WorkshopPublishInfoStateForWorld(_ownerState, _data));
		}
	}

	public override int CompareTo(object obj)
	{
		if (obj is UIWorkshopPublishWorldListItem uIWorkshopPublishWorldListItem)
		{
			return _orderInList.CompareTo(uIWorkshopPublishWorldListItem._orderInList);
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
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		CalculatedStyle innerDimensions = GetInnerDimensions();
		CalculatedStyle dimensions = _worldIcon.GetDimensions();
		float num = dimensions.X + dimensions.Width;
		Color color = (_data.IsValid ? Color.White : Color.Gray);
		string worldName = _data.GetWorldName(allowCropping: true);
		Utils.DrawBorderString(spriteBatch, worldName, new Vector2(num + 6f, innerDimensions.Y + 3f), color);
		float num6 = (innerDimensions.Width - 22f - dimensions.Width - _publishButton.GetDimensions().Width) / 2f;
		float height = _publishButton.GetDimensions().Height;
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector(num + 6f, innerDimensions.Y + innerDimensions.Height - height);
		float num2 = num6;
		DrawPanel(spriteBatch, vector, num2, height);
		string expertText = "";
		Color gameModeColor = Color.White;
		GetDifficulty(out expertText, out gameModeColor);
		Vector2 val = FontAssets.MouseText.Value.MeasureString(expertText);
		float x = val.X;
		float y = val.Y;
		float x2 = num2 * 0.5f - x * 0.5f;
		float num3 = height * 0.5f - y * 0.5f;
		Utils.DrawBorderString(spriteBatch, expertText, vector + new Vector2(x2, num3 + 3f), gameModeColor);
		vector.X += num2 + 5f;
		float num4 = num6;
		if (!GameCulture.FromCultureName(GameCulture.CultureName.English).IsActive)
		{
			num4 += 40f;
		}
		DrawPanel(spriteBatch, vector, num4, height);
		string textValue = Language.GetTextValue("UI.WorldSizeFormat", _data.WorldSizeName);
		Vector2 val2 = FontAssets.MouseText.Value.MeasureString(textValue);
		float x3 = val2.X;
		float y2 = val2.Y;
		float x4 = num4 * 0.5f - x3 * 0.5f;
		float num5 = height * 0.5f - y2 * 0.5f;
		Utils.DrawBorderString(spriteBatch, textValue, vector + new Vector2(x4, num5 + 3f), Color.White);
		vector.X += num4 + 5f;
	}
}
