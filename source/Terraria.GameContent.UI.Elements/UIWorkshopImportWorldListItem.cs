using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.States;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Social;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIWorkshopImportWorldListItem : AWorldListItem
{
	private Asset<Texture2D> _dividerTexture;

	private Asset<Texture2D> _workshopIconTexture;

	private Asset<Texture2D> _innerPanelTexture;

	private UIElement _worldIcon;

	private UIText _buttonLabel;

	private Asset<Texture2D> _buttonImportTexture;

	private int _orderInList;

	public UIState _ownerState;

	public UIWorkshopImportWorldListItem(UIState ownerState, WorldFileData data, int orderInList)
	{
		_ownerState = ownerState;
		_orderInList = orderInList;
		_data = data;
		LoadTextures();
		InitializeAppearance();
		_worldIcon = GetIconElement();
		_worldIcon.Left.Set(4f, 0f);
		_worldIcon.OnLeftDoubleClick += ImportButtonClick_ImportWorldToLocalFiles;
		Append(_worldIcon);
		float num = 4f;
		UIImageButton uIImageButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay"));
		uIImageButton.VAlign = 1f;
		uIImageButton.Left.Set(num, 0f);
		uIImageButton.OnLeftClick += ImportButtonClick_ImportWorldToLocalFiles;
		base.OnLeftDoubleClick += ImportButtonClick_ImportWorldToLocalFiles;
		uIImageButton.OnMouseOver += PlayMouseOver;
		uIImageButton.OnMouseOut += ButtonMouseOut;
		Append(uIImageButton);
		num += 24f;
		_buttonLabel = new UIText("");
		_buttonLabel.VAlign = 1f;
		_buttonLabel.Left.Set(num, 0f);
		_buttonLabel.Top.Set(-3f, 0f);
		Append(_buttonLabel);
		uIImageButton.SetSnapPoint("Import", orderInList);
	}

	private void LoadTextures()
	{
		_dividerTexture = Main.Assets.Request<Texture2D>("Images/UI/Divider");
		_innerPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground");
		_workshopIconTexture = TextureAssets.Extra[243];
	}

	private void InitializeAppearance()
	{
		Height.Set(96f, 0f);
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

	private void PlayMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		_buttonLabel.SetText(Language.GetTextValue("UI.Import"));
	}

	private void ButtonMouseOut(UIMouseEvent evt, UIElement listeningElement)
	{
		_buttonLabel.SetText("");
	}

	private void ImportButtonClick_ImportWorldToLocalFiles(UIMouseEvent evt, UIElement listeningElement)
	{
		if (listeningElement == evt.Target)
		{
			SoundEngine.PlaySound(10);
			Main.clrInput();
			UIVirtualKeyboard uIVirtualKeyboard = new UIVirtualKeyboard(Language.GetTextValue("Workshop.EnterNewNameForImportedWorld"), _data.Name, OnFinishedSettingName, GoToMainMenu, 0, allowEmpty: true);
			uIVirtualKeyboard.SetMaxInputLength(27);
			uIVirtualKeyboard.Text = _data.Name;
			Main.MenuUI.SetState(uIVirtualKeyboard);
		}
	}

	private void OnFinishedSettingName(string name)
	{
		string newDisplayName = name.Trim();
		if (SocialAPI.Workshop != null)
		{
			SocialAPI.Workshop.ImportDownloadedWorldToLocalSaves(_data, null, newDisplayName);
		}
	}

	private void GoToMainMenu()
	{
		SoundEngine.PlaySound(11);
		Main.menuMode = 0;
	}

	public override int CompareTo(object obj)
	{
		if (obj is UIWorkshopImportWorldListItem uIWorkshopImportWorldListItem)
		{
			return _orderInList.CompareTo(uIWorkshopImportWorldListItem._orderInList);
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

	private void DrawPanel(SpriteBatch spriteBatch, Vector2 position, float width)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		spriteBatch.Draw(_innerPanelTexture.Value, position, (Rectangle?)new Rectangle(0, 0, 8, _innerPanelTexture.Height()), Color.White);
		spriteBatch.Draw(_innerPanelTexture.Value, new Vector2(position.X + 8f, position.Y), (Rectangle?)new Rectangle(8, 0, 8, _innerPanelTexture.Height()), Color.White, 0f, Vector2.Zero, new Vector2((width - 16f) / 8f, 1f), (SpriteEffects)0, 0f);
		spriteBatch.Draw(_innerPanelTexture.Value, new Vector2(position.X + width - 8f, position.Y), (Rectangle?)new Rectangle(16, 0, 8, _innerPanelTexture.Height()), Color.White);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		CalculatedStyle innerDimensions = GetInnerDimensions();
		CalculatedStyle dimensions = _worldIcon.GetDimensions();
		float num = dimensions.X + dimensions.Width;
		Color color = (_data.IsValid ? Color.White : Color.Gray);
		string worldName = _data.GetWorldName(allowCropping: true);
		if (worldName != null)
		{
			Utils.DrawBorderString(spriteBatch, worldName, new Vector2(num + 6f, dimensions.Y - 2f), color);
		}
		spriteBatch.Draw(_workshopIconTexture.Value, new Vector2(GetDimensions().X + GetDimensions().Width - (float)_workshopIconTexture.Width() - 3f, GetDimensions().Y + 2f), (Rectangle?)_workshopIconTexture.Frame(), Color.White);
		spriteBatch.Draw(_dividerTexture.Value, new Vector2(num, innerDimensions.Y + 21f), (Rectangle?)null, Color.White, 0f, Vector2.Zero, new Vector2((GetDimensions().X + GetDimensions().Width - num) / 8f, 1f), (SpriteEffects)0, 0f);
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector(num + 6f, innerDimensions.Y + 29f);
		float num2 = 100f;
		DrawPanel(spriteBatch, vector, num2);
		string expertText = "";
		Color gameModeColor = Color.White;
		GetDifficulty(out expertText, out gameModeColor);
		float x = FontAssets.MouseText.Value.MeasureString(expertText).X;
		float x2 = num2 * 0.5f - x * 0.5f;
		Utils.DrawBorderString(spriteBatch, expertText, vector + new Vector2(x2, 3f), gameModeColor);
		vector.X += num2 + 5f;
		if (_data._worldSizeName != null)
		{
			float num3 = 150f;
			if (!GameCulture.FromCultureName(GameCulture.CultureName.English).IsActive)
			{
				num3 += 40f;
			}
			DrawPanel(spriteBatch, vector, num3);
			string textValue = Language.GetTextValue("UI.WorldSizeFormat", _data.WorldSizeName);
			float x3 = FontAssets.MouseText.Value.MeasureString(textValue).X;
			float x4 = num3 * 0.5f - x3 * 0.5f;
			Utils.DrawBorderString(spriteBatch, textValue, vector + new Vector2(x4, 3f), Color.White);
			vector.X += num3 + 5f;
		}
		float num4 = innerDimensions.X + innerDimensions.Width - vector.X;
		DrawPanel(spriteBatch, vector, num4);
		string arg = ((!GameCulture.FromCultureName(GameCulture.CultureName.English).IsActive) ? _data.CreationTime.ToShortDateString() : _data.CreationTime.ToString("d MMMM yyyy"));
		string textValue2 = Language.GetTextValue("UI.WorldCreatedFormat", arg);
		float x5 = FontAssets.MouseText.Value.MeasureString(textValue2).X;
		float x6 = num4 * 0.5f - x5 * 0.5f;
		Utils.DrawBorderString(spriteBatch, textValue2, vector + new Vector2(x6, 3f), Color.White);
		vector.X += num4 + 5f;
	}
}
