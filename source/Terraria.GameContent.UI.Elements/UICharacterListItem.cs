using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.Engine;
using Terraria.ModLoader.UI;
using Terraria.Social;
using Terraria.UI;
using Terraria.Utilities;

namespace Terraria.GameContent.UI.Elements;

public class UICharacterListItem : UIPanel
{
	private PlayerFileData _data;

	private Asset<Texture2D> _dividerTexture;

	private Asset<Texture2D> _innerPanelTexture;

	private UICharacter _playerPanel;

	private UIText _buttonLabel;

	private UIText _deleteButtonLabel;

	private Asset<Texture2D> _buttonCloudActiveTexture;

	private Asset<Texture2D> _buttonCloudInactiveTexture;

	private Asset<Texture2D> _buttonFavoriteActiveTexture;

	private Asset<Texture2D> _buttonFavoriteInactiveTexture;

	private Asset<Texture2D> _buttonPlayTexture;

	private Asset<Texture2D> _buttonRenameTexture;

	private Asset<Texture2D> _buttonDeleteTexture;

	private UIImageButton _deleteButton;

	private Asset<Texture2D> _errorTexture;

	private Asset<Texture2D> _configTexture;

	private ulong _fileSize;

	public PlayerFileData Data => _data;

	public bool IsFavorite => _data.IsFavorite;

	public UICharacterListItem(PlayerFileData data, int snapPointIndex)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		BorderColor = new Color(89, 116, 213) * 0.7f;
		_dividerTexture = Main.Assets.Request<Texture2D>("Images/UI/Divider");
		_innerPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground");
		_buttonCloudActiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonCloudActive");
		_buttonCloudInactiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonCloudInactive");
		_buttonFavoriteActiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonFavoriteActive");
		_buttonFavoriteInactiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonFavoriteInactive");
		_buttonPlayTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay");
		_buttonRenameTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonRename");
		_buttonDeleteTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete");
		InitializeTmlFields(data);
		Height.Set(96f, 0f);
		Width.Set(0f, 1f);
		SetPadding(6f);
		_data = data;
		_playerPanel = new UICharacter(data.Player, animated: false, hasBackPanel: true, 1f, useAClone: true);
		_playerPanel.Left.Set(4f, 0f);
		_playerPanel.OnLeftDoubleClick += PlayGame;
		base.OnLeftDoubleClick += PlayGame;
		Append(_playerPanel);
		float num = 4f;
		UIImageButton uIImageButton = new UIImageButton(_buttonPlayTexture);
		uIImageButton.VAlign = 1f;
		uIImageButton.Left.Set(num, 0f);
		uIImageButton.OnLeftClick += PlayGame;
		uIImageButton.OnMouseOver += PlayMouseOver;
		uIImageButton.OnMouseOut += ButtonMouseOut;
		Append(uIImageButton);
		num += 24f;
		UIImageButton uIImageButton2 = new UIImageButton(_data.IsFavorite ? _buttonFavoriteActiveTexture : _buttonFavoriteInactiveTexture);
		uIImageButton2.VAlign = 1f;
		uIImageButton2.Left.Set(num, 0f);
		uIImageButton2.OnLeftClick += FavoriteButtonClick;
		uIImageButton2.OnMouseOver += FavoriteMouseOver;
		uIImageButton2.OnMouseOut += ButtonMouseOut;
		uIImageButton2.SetVisibility(1f, _data.IsFavorite ? 0.8f : 0.4f);
		Append(uIImageButton2);
		num += 24f;
		if (SocialAPI.Cloud != null)
		{
			UIImageButton uIImageButton3 = new UIImageButton(_data.IsCloudSave ? _buttonCloudActiveTexture : _buttonCloudInactiveTexture);
			uIImageButton3.VAlign = 1f;
			uIImageButton3.Left.Set(num, 0f);
			uIImageButton3.OnLeftClick += CloudButtonClick;
			uIImageButton3.OnMouseOver += CloudMouseOver;
			uIImageButton3.OnMouseOut += ButtonMouseOut;
			Append(uIImageButton3);
			uIImageButton3.SetSnapPoint("Cloud", snapPointIndex);
			num += 24f;
		}
		UIImageButton uIImageButton4 = new UIImageButton(_buttonRenameTexture);
		uIImageButton4.VAlign = 1f;
		uIImageButton4.Left.Set(num, 0f);
		uIImageButton4.OnLeftClick += RenameButtonClick;
		uIImageButton4.OnMouseOver += RenameMouseOver;
		uIImageButton4.OnMouseOut += ButtonMouseOut;
		Append(uIImageButton4);
		num += 24f;
		UIImageButton uIImageButton5 = new UIImageButton(_buttonDeleteTexture)
		{
			VAlign = 1f,
			HAlign = 1f
		};
		if (!_data.IsFavorite)
		{
			uIImageButton5.OnLeftClick += DeleteButtonClick;
		}
		uIImageButton5.OnMouseOver += DeleteMouseOver;
		uIImageButton5.OnMouseOut += DeleteMouseOut;
		_deleteButton = uIImageButton5;
		Append(uIImageButton5);
		num += 4f;
		AddTmlElements(data);
		_buttonLabel = new UIText("");
		_buttonLabel.VAlign = 1f;
		_buttonLabel.Left.Set(num, 0f);
		_buttonLabel.Top.Set(-3f, 0f);
		Append(_buttonLabel);
		_deleteButtonLabel = new UIText("");
		_deleteButtonLabel.VAlign = 1f;
		_deleteButtonLabel.HAlign = 1f;
		_deleteButtonLabel.Left.Set((data.customDataFail == null) ? (-30f) : (-54f), 0f);
		_deleteButtonLabel.Top.Set(-3f, 0f);
		Append(_deleteButtonLabel);
		uIImageButton.SetSnapPoint("Play", snapPointIndex);
		uIImageButton2.SetSnapPoint("Favorite", snapPointIndex);
		uIImageButton4.SetSnapPoint("Rename", snapPointIndex);
		uIImageButton5.SetSnapPoint("Delete", snapPointIndex);
	}

	private void RenameMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		_buttonLabel.SetText(Language.GetTextValue("UI.Rename"));
	}

	private void FavoriteMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		if (_data.IsFavorite)
		{
			_buttonLabel.SetText(Language.GetTextValue("UI.Unfavorite"));
		}
		else
		{
			_buttonLabel.SetText(Language.GetTextValue("UI.Favorite"));
		}
	}

	private void CloudMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		if (_data.IsCloudSave)
		{
			_buttonLabel.SetText(Language.GetTextValue("UI.MoveOffCloud"));
		}
		else if (!Steam.CheckSteamCloudStorageSufficient(_fileSize))
		{
			_buttonLabel.SetText(Language.GetTextValue("tModLoader.CloudWarning"));
		}
		else
		{
			_buttonLabel.SetText(Language.GetTextValue("UI.MoveToCloud"));
		}
	}

	private void PlayMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		_buttonLabel.SetText(Language.GetTextValue("UI.Play"));
	}

	private void DeleteMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		if (_data.IsFavorite)
		{
			_deleteButtonLabel.SetText(Language.GetTextValue("UI.CannotDeleteFavorited"));
		}
		else
		{
			_deleteButtonLabel.SetText(Language.GetTextValue("UI.Delete"));
		}
	}

	private void DeleteMouseOut(UIMouseEvent evt, UIElement listeningElement)
	{
		_deleteButtonLabel.SetText("");
	}

	private void ButtonMouseOut(UIMouseEvent evt, UIElement listeningElement)
	{
		_buttonLabel.SetText("");
	}

	private void RenameButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		Main.clrInput();
		UIVirtualKeyboard uIVirtualKeyboard = new UIVirtualKeyboard(Lang.menu[45].Value, "", OnFinishedSettingName, GoBackHere, 0, allowEmpty: true);
		uIVirtualKeyboard.SetMaxInputLength(20);
		Main.MenuUI.SetState(uIVirtualKeyboard);
		if (base.Parent.Parent is UIList uIList)
		{
			uIList.UpdateOrder();
		}
	}

	private void OnFinishedSettingName(string name)
	{
		string newName = name.Trim();
		Main.menuMode = 10;
		_data.Rename(newName);
		Main.OpenCharacterSelectUI();
	}

	private void GoBackHere()
	{
		Main.OpenCharacterSelectUI();
	}

	private void CloudButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		if (_data.IsCloudSave)
		{
			_data.MoveToLocal();
		}
		else
		{
			Steam.RecalculateAvailableSteamCloudStorage();
			if (!Steam.CheckSteamCloudStorageSufficient(_fileSize))
			{
				return;
			}
			_data.MoveToCloud();
		}
		((UIImageButton)evt.Target).SetImage(_data.IsCloudSave ? _buttonCloudActiveTexture : _buttonCloudInactiveTexture);
		if (_data.IsCloudSave)
		{
			_buttonLabel.SetText(Language.GetTextValue("UI.MoveOffCloud"));
		}
		else
		{
			_buttonLabel.SetText(Language.GetTextValue("UI.MoveToCloud"));
		}
	}

	private void DeleteButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		for (int i = 0; i < Main.PlayerList.Count; i++)
		{
			if (Main.PlayerList[i] == _data)
			{
				SoundEngine.PlaySound(10);
				Main.selectedPlayer = i;
				Main.menuMode = 5;
				break;
			}
		}
	}

	private void PlayGame(UIMouseEvent evt, UIElement listeningElement)
	{
		if (listeningElement == evt.Target && _data.Player.loadStatus == 0)
		{
			Main.SelectPlayer(_data);
		}
	}

	private void FavoriteButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		_data.ToggleFavorite();
		((UIImageButton)evt.Target).SetImage(_data.IsFavorite ? _buttonFavoriteActiveTexture : _buttonFavoriteInactiveTexture);
		((UIImageButton)evt.Target).SetVisibility(1f, _data.IsFavorite ? 0.8f : 0.4f);
		if (_data.IsFavorite)
		{
			_buttonLabel.SetText(Language.GetTextValue("UI.Unfavorite"));
			_deleteButton.OnLeftClick -= DeleteButtonClick;
		}
		else
		{
			_buttonLabel.SetText(Language.GetTextValue("UI.Favorite"));
			_deleteButton.OnLeftClick += DeleteButtonClick;
		}
		if (base.Parent.Parent is UIList uIList)
		{
			uIList.UpdateOrder();
		}
	}

	public override int CompareTo(object obj)
	{
		if (obj is UICharacterListItem uICharacterListItem)
		{
			if (IsFavorite && !uICharacterListItem.IsFavorite)
			{
				return -1;
			}
			if (!IsFavorite && uICharacterListItem.IsFavorite)
			{
				return 1;
			}
			if (_data.Name.CompareTo(uICharacterListItem._data.Name) != 0)
			{
				return _data.Name.CompareTo(uICharacterListItem._data.Name);
			}
			return _data.GetFileName().CompareTo(uICharacterListItem._data.GetFileName());
		}
		return base.CompareTo(obj);
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		base.MouseOver(evt);
		BackgroundColor = new Color(73, 94, 171);
		BorderColor = new Color(89, 116, 213);
		_playerPanel.SetAnimated(animated: true);
	}

	public override void MouseOut(UIMouseEvent evt)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		base.MouseOut(evt);
		BackgroundColor = new Color(63, 82, 151) * 0.7f;
		BorderColor = new Color(89, 116, 213) * 0.7f;
		_playerPanel.SetAnimated(animated: false);
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
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		CalculatedStyle innerDimensions = GetInnerDimensions();
		CalculatedStyle dimensions = _playerPanel.GetDimensions();
		float num = dimensions.X + dimensions.Width;
		Color color = Color.White;
		string text = _data.Name;
		if (_data.Player.loadStatus != 0)
		{
			color = Color.Gray;
			string name = StatusID.Search.GetName(_data.Player.loadStatus);
			text = "(" + name + ") " + text;
		}
		Utils.DrawBorderString(spriteBatch, text, new Vector2(num + 6f, dimensions.Y - 2f), color);
		spriteBatch.Draw(_dividerTexture.Value, new Vector2(num, innerDimensions.Y + 21f), (Rectangle?)null, Color.White, 0f, Vector2.Zero, new Vector2((GetDimensions().X + GetDimensions().Width - num) / 8f, 1f), (SpriteEffects)0, 0f);
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector(num + 6f, innerDimensions.Y + 29f);
		float num2 = 200f;
		Vector2 vector2 = vector;
		DrawPanel(spriteBatch, vector2, num2);
		spriteBatch.Draw(TextureAssets.Heart.Value, vector2 + new Vector2(5f, 2f), Color.White);
		vector2.X += 10f + (float)TextureAssets.Heart.Width();
		Utils.DrawBorderString(spriteBatch, _data.Player.statLifeMax2 + Language.GetTextValue("GameUI.PlayerLifeMax"), vector2 + new Vector2(0f, 3f), Color.White);
		vector2.X += 65f;
		spriteBatch.Draw(TextureAssets.Mana.Value, vector2 + new Vector2(5f, 2f), Color.White);
		vector2.X += 10f + (float)TextureAssets.Mana.Width();
		Utils.DrawBorderString(spriteBatch, _data.Player.statManaMax2 + Language.GetTextValue("GameUI.PlayerManaMax"), vector2 + new Vector2(0f, 3f), Color.White);
		vector.X += num2 + 5f;
		Vector2 vector3 = vector;
		float num3 = 140f;
		if (GameCulture.FromCultureName(GameCulture.CultureName.Russian).IsActive)
		{
			num3 = 180f;
		}
		DrawPanel(spriteBatch, vector3, num3);
		string text2 = "";
		Color color2 = Color.White;
		switch (_data.Player.difficulty)
		{
		case 0:
			text2 = Language.GetTextValue("UI.Softcore");
			break;
		case 1:
			text2 = Language.GetTextValue("UI.Mediumcore");
			color2 = Main.mcColor;
			break;
		case 2:
			text2 = Language.GetTextValue("UI.Hardcore");
			color2 = Main.hcColor;
			break;
		case 3:
			text2 = Language.GetTextValue("UI.Creative");
			color2 = Main.creativeModeColor;
			break;
		}
		vector3 += new Vector2(num3 * 0.5f - FontAssets.MouseText.Value.MeasureString(text2).X * 0.5f, 3f);
		Utils.DrawBorderString(spriteBatch, text2, vector3, color2);
		vector.X += num3 + 5f;
		Vector2 vector4 = vector;
		float num4 = innerDimensions.X + innerDimensions.Width - vector4.X;
		DrawPanel(spriteBatch, vector4, num4);
		TimeSpan playTime = _data.GetPlayTime();
		int num5 = playTime.Days * 24 + playTime.Hours;
		string text3 = ((num5 < 10) ? "0" : "") + num5 + playTime.ToString("\\:mm\\:ss");
		vector4 += new Vector2(num4 * 0.5f - FontAssets.MouseText.Value.MeasureString(text3).X * 0.5f, 3f);
		Utils.DrawBorderString(spriteBatch, text3, vector4, Color.White);
	}

	private void InitializeTmlFields(PlayerFileData data)
	{
		_errorTexture = UICommon.ButtonErrorTexture;
		_configTexture = UICommon.ButtonConfigTexture;
		_fileSize = (ulong)FileUtilities.GetFileSize(data.Path, data.IsCloudSave);
	}

	private void AddTmlElements(PlayerFileData data)
	{
		if (data.customDataFail != null)
		{
			UIImageButton errorButton = new UIImageButton(_errorTexture)
			{
				VAlign = 1f,
				HAlign = 1f
			};
			errorButton.Left.Set(-24f, 0f);
			errorButton.OnLeftClick += ErrorButtonClick;
			errorButton.OnMouseOver += ErrorMouseOver;
			errorButton.OnMouseOut += DeleteMouseOut;
			Append(errorButton);
		}
		if (data.Player.usedMods == null)
		{
			return;
		}
		string[] currentModNames = Terraria.ModLoader.ModLoader.Mods.Select((Mod m) => m.Name).ToArray();
		List<string> missingMods = data.Player.usedMods.Except(currentModNames).ToList();
		List<string> newMods = currentModNames.Except(new string[1] { "ModLoader" }).Except(data.Player.usedMods).ToList();
		bool checkModPack = Path.GetFileNameWithoutExtension(ModOrganizer.ModPackActive) != data.Player.modPack;
		if (!checkModPack && missingMods.Count <= 0 && newMods.Count <= 0)
		{
			return;
		}
		UIText warningLabel = new UIText("")
		{
			VAlign = 0f,
			HAlign = 1f
		};
		warningLabel.Left.Set(-30f, 0f);
		warningLabel.Top.Set(3f, 0f);
		Append(warningLabel);
		UIImageButton modListWarning = new UIImageButton(_errorTexture)
		{
			VAlign = 0f,
			HAlign = 1f
		};
		modListWarning.Top.Set(-2f, 0f);
		StringBuilder fullSB = new StringBuilder(Language.GetTextValue("tModLoader.ModsDifferentSinceLastPlay"));
		StringBuilder shortSB = new StringBuilder();
		if (checkModPack)
		{
			string pack = data.Player.modPack;
			if (string.IsNullOrEmpty(pack))
			{
				pack = "None";
			}
			shortSB.Append(Separator() + Language.GetTextValue("tModLoader.ModPackMismatch", pack));
			fullSB.Append("\n" + Language.GetTextValue("tModLoader.ModPackMismatch", pack));
		}
		if (missingMods.Count > 0)
		{
			shortSB.Append(Separator() + ((missingMods.Count > 1) ? Language.GetTextValue("tModLoader.MissingXMods", missingMods.Count) : Language.GetTextValue("tModLoader.Missing1Mod")));
			fullSB.Append("\n" + Language.GetTextValue("tModLoader.MissingModsListing", string.Join("\n", missingMods.Select((string x) => "- " + x))));
		}
		if (newMods.Count > 0)
		{
			shortSB.Append(Separator() + ((newMods.Count > 1) ? Language.GetTextValue("tModLoader.NewXMods", newMods.Count) : Language.GetTextValue("tModLoader.New1Mod")));
			fullSB.Append("\n" + Language.GetTextValue("tModLoader.NewModsListing", string.Join("\n", newMods.Select((string x) => "- " + x))));
		}
		if (shortSB.Length != 0)
		{
			shortSB.Append('.');
		}
		string warning = shortSB.ToString();
		string fullWarning = fullSB.ToString();
		modListWarning.OnMouseOver += delegate
		{
			warningLabel.SetText(warning);
		};
		modListWarning.OnMouseOut += delegate
		{
			warningLabel.SetText("");
		};
		modListWarning.OnLeftClick += delegate
		{
			Interface.infoMessage.Show(fullWarning, 888, Main._characterSelectMenu);
		};
		Append(modListWarning);
		string Separator()
		{
			if (shortSB.Length == 0)
			{
				return null;
			}
			return "; ";
		}
	}

	private void ErrorMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		_deleteButtonLabel.SetText(_data.customDataFail.modName + " Error");
	}

	private void ConfigMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		_buttonLabel.SetText("Edit Player Config");
	}

	private void ErrorButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		Logging.Terraria.Error((object)Language.GetTextValue("tModLoader.PlayerCustomDataFail"), _data.customDataFail.InnerException);
	}

	private void ConfigButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
	}
}
