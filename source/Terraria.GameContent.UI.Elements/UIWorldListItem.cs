using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.OS;
using Terraria.Audio;
using Terraria.GameContent.UI.States;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.Engine;
using Terraria.ModLoader.UI;
using Terraria.Social;
using Terraria.UI;
using Terraria.Utilities;

namespace Terraria.GameContent.UI.Elements;

public class UIWorldListItem : AWorldListItem
{
	private Asset<Texture2D> _dividerTexture;

	private Asset<Texture2D> _innerPanelTexture;

	private UIElement _worldIcon;

	private UIText _buttonLabel;

	private UIText _deleteButtonLabel;

	private Asset<Texture2D> _buttonCloudActiveTexture;

	private Asset<Texture2D> _buttonCloudInactiveTexture;

	private Asset<Texture2D> _buttonFavoriteActiveTexture;

	private Asset<Texture2D> _buttonFavoriteInactiveTexture;

	private Asset<Texture2D> _buttonPlayTexture;

	private Asset<Texture2D> _buttonSeedTexture;

	private Asset<Texture2D> _buttonRenameTexture;

	private Asset<Texture2D> _buttonDeleteTexture;

	private UIImageButton _deleteButton;

	private int _orderInList;

	private bool _canBePlayed;

	private ulong _fileSize;

	private Asset<Texture2D> _configTexture;

	public bool IsFavorite => _data.IsFavorite;

	public UIWorldListItem(WorldFileData data, int orderInList, bool canBePlayed)
	{
		_orderInList = orderInList;
		_data = data;
		_canBePlayed = canBePlayed;
		InitializeTmlFields(data);
		LoadTextures();
		InitializeAppearance();
		_worldIcon = GetIconElement();
		_worldIcon.OnLeftDoubleClick += PlayGame;
		Append(_worldIcon);
		if (_data.DefeatedMoonlord)
		{
			UIImage element = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/IconCompletion"))
			{
				HAlign = 0.5f,
				VAlign = 0.5f,
				Top = new StyleDimension(-10f, 0f),
				Left = new StyleDimension(-3f, 0f),
				IgnoresMouseInteraction = true
			};
			_worldIcon.Append(element);
		}
		float num = 4f;
		UIImageButton uIImageButton = new UIImageButton(_buttonPlayTexture);
		uIImageButton.VAlign = 1f;
		uIImageButton.Left.Set(num, 0f);
		uIImageButton.OnLeftClick += PlayGame;
		base.OnLeftDoubleClick += PlayGame;
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
			uIImageButton3.SetSnapPoint("Cloud", orderInList);
			Append(uIImageButton3);
			num += 24f;
		}
		if (_data.WorldGeneratorVersion != 0L)
		{
			UIImageButton uIImageButton4 = new UIImageButton(_buttonSeedTexture);
			uIImageButton4.VAlign = 1f;
			uIImageButton4.Left.Set(num, 0f);
			uIImageButton4.OnLeftClick += SeedButtonClick;
			uIImageButton4.OnMouseOver += SeedMouseOver;
			uIImageButton4.OnMouseOut += ButtonMouseOut;
			uIImageButton4.SetSnapPoint("Seed", orderInList);
			Append(uIImageButton4);
			num += 24f;
		}
		AddTmlElements(data, ref num);
		UIImageButton uIImageButton5 = new UIImageButton(_buttonRenameTexture);
		uIImageButton5.VAlign = 1f;
		uIImageButton5.Left.Set(num, 0f);
		uIImageButton5.OnLeftClick += RenameButtonClick;
		uIImageButton5.OnMouseOver += RenameMouseOver;
		uIImageButton5.OnMouseOut += ButtonMouseOut;
		uIImageButton5.SetSnapPoint("Rename", orderInList);
		Append(uIImageButton5);
		num += 24f;
		UIImageButton uIImageButton6 = new UIImageButton(_buttonDeleteTexture)
		{
			VAlign = 1f,
			HAlign = 1f
		};
		if (!_data.IsFavorite)
		{
			uIImageButton6.OnLeftClick += DeleteButtonClick;
		}
		uIImageButton6.OnMouseOver += DeleteMouseOver;
		uIImageButton6.OnMouseOut += DeleteMouseOut;
		_deleteButton = uIImageButton6;
		Append(uIImageButton6);
		num += 4f;
		_buttonLabel = new UIText("");
		_buttonLabel.VAlign = 1f;
		_buttonLabel.Left.Set(num, 0f);
		_buttonLabel.Top.Set(-3f, 0f);
		Append(_buttonLabel);
		_deleteButtonLabel = new UIText("");
		_deleteButtonLabel.VAlign = 1f;
		_deleteButtonLabel.HAlign = 1f;
		_deleteButtonLabel.Left.Set(-30f, 0f);
		_deleteButtonLabel.Top.Set(-3f, 0f);
		Append(_deleteButtonLabel);
		uIImageButton.SetSnapPoint("Play", orderInList);
		uIImageButton2.SetSnapPoint("Favorite", orderInList);
		uIImageButton5.SetSnapPoint("Rename", orderInList);
		uIImageButton6.SetSnapPoint("Delete", orderInList);
	}

	private void LoadTextures()
	{
		_dividerTexture = Main.Assets.Request<Texture2D>("Images/UI/Divider");
		_innerPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground");
		_buttonCloudActiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonCloudActive");
		_buttonCloudInactiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonCloudInactive");
		_buttonFavoriteActiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonFavoriteActive");
		_buttonFavoriteInactiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonFavoriteInactive");
		_buttonPlayTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay");
		_buttonSeedTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonSeed");
		_buttonRenameTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonRename");
		_buttonDeleteTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete");
		LoadTmlTextures();
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
		if (!_canBePlayed)
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
		if (!_canBePlayed)
		{
			BorderColor = new Color(127, 127, 127) * 0.7f;
			BackgroundColor = Color.Lerp(new Color(63, 82, 151), new Color(80, 80, 80), 0.5f) * 0.7f;
		}
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

	private void SeedMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		_buttonLabel.SetText(Language.GetTextValue("UI.CopySeed", _data.GetFullSeedText(allowCropping: true)));
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
		for (int i = 0; i < Main.WorldList.Count; i++)
		{
			if (Main.WorldList[i] == _data)
			{
				SoundEngine.PlaySound(10);
				Main.selectedWorld = i;
				Main.menuMode = 9;
				break;
			}
		}
	}

	private void PlayGame(UIMouseEvent evt, UIElement listeningElement)
	{
		if (!_data.IsValid || listeningElement != evt.Target || TryMovingToRejectionMenuIfNeeded(_data.GameMode))
		{
			return;
		}
		_data.SetAsActive();
		SoundEngine.PlaySound(10);
		Main.clrInput();
		Main.GetInputText("");
		if (Main.menuMultiplayer && SocialAPI.Network != null)
		{
			Main.menuMode = 889;
		}
		else if (Main.menuMultiplayer)
		{
			Main.menuMode = 30;
		}
		else
		{
			Main.menuMode = 10;
		}
		if (!Main.menuMultiplayer)
		{
			ConfigManager.LoadAll();
			if (ConfigManager.AnyModNeedsReload())
			{
				Terraria.ModLoader.ModLoader.OnSuccessfulLoad = (Action)Delegate.Combine(Terraria.ModLoader.ModLoader.OnSuccessfulLoad, PlayReload());
				Terraria.ModLoader.ModLoader.Reload();
			}
			else
			{
				ConfigManager.OnChangedAll();
				WorldGen.playWorld();
			}
		}
	}

	private bool TryMovingToRejectionMenuIfNeeded(int worldGameMode)
	{
		if (!Main.RegisteredGameModes.TryGetValue(worldGameMode, out var value))
		{
			SoundEngine.PlaySound(10);
			Main.statusText = Language.GetTextValue("UI.WorldCannotBeLoadedBecauseItHasAnInvalidGameMode");
			Main.menuMode = 1000000;
			return true;
		}
		if (_canBePlayed)
		{
			return false;
		}
		bool flag = Main.ActivePlayerFileData.Player.difficulty == 3;
		bool isJourneyMode = value.IsJourneyMode;
		if (flag && !isJourneyMode)
		{
			SoundEngine.PlaySound(10);
			Main.statusText = Language.GetTextValue("UI.PlayerIsCreativeAndWorldIsNotCreative");
			Main.menuMode = 1000000;
			return true;
		}
		if (!flag && isJourneyMode)
		{
			SoundEngine.PlaySound(10);
			Main.statusText = Language.GetTextValue("UI.PlayerIsNotCreativeAndWorldIsCreative");
			Main.menuMode = 1000000;
			return true;
		}
		if (!SystemLoader.CanWorldBePlayed(Main.ActivePlayerFileData, _data, out var rejector))
		{
			SoundEngine.PlaySound(10);
			Main.statusText = rejector.WorldCanBePlayedRejectionMessage(Main.ActivePlayerFileData, _data);
			Main.menuMode = 1000000;
			return true;
		}
		return false;
	}

	private void RenameButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		Main.clrInput();
		UIVirtualKeyboard uIVirtualKeyboard = new UIVirtualKeyboard(Lang.menu[48].Value, "", OnFinishedSettingName, GoBackHere, 0, allowEmpty: true);
		uIVirtualKeyboard.SetMaxInputLength(27);
		Main.MenuUI.SetState(uIVirtualKeyboard);
		if (base.Parent.Parent is UIList uIList)
		{
			uIList.UpdateOrder();
		}
	}

	private void OnFinishedSettingName(string name)
	{
		string newDisplayName = name.Trim();
		Main.menuMode = 10;
		_data.Rename(newDisplayName);
	}

	private void GoBackHere()
	{
		Main.GoToWorldSelect();
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

	private void SeedButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		Platform.Get<IClipboard>().Value = _data.GetFullSeedText();
		_buttonLabel.SetText(Language.GetTextValue("UI.SeedCopied"));
	}

	public override int CompareTo(object obj)
	{
		if (obj is UIWorldListItem uIWorldListItem)
		{
			return _orderInList.CompareTo(uIWorldListItem._orderInList);
		}
		if (obj is UIPanel)
		{
			return -1;
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
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		CalculatedStyle innerDimensions = GetInnerDimensions();
		CalculatedStyle dimensions = _worldIcon.GetDimensions();
		float num = dimensions.X + dimensions.Width;
		Color color = (_data.IsValid ? Color.White : Color.Gray);
		string worldName = _data.GetWorldName(allowCropping: true);
		Utils.DrawBorderString(spriteBatch, worldName, new Vector2(num + 6f, dimensions.Y - 2f), color);
		spriteBatch.Draw(_dividerTexture.Value, new Vector2(num, innerDimensions.Y + 21f), (Rectangle?)null, Color.White, 0f, Vector2.Zero, new Vector2((GetDimensions().X + GetDimensions().Width - num) / 8f, 1f), (SpriteEffects)0, 0f);
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector(num + 6f, innerDimensions.Y + 29f);
		float num2 = 100f;
		DrawPanel(spriteBatch, vector, num2);
		GetDifficulty(out var expertText, out var gameModeColor);
		float x = FontAssets.MouseText.Value.MeasureString(expertText).X;
		float x2 = num2 * 0.5f - x * 0.5f;
		Utils.DrawBorderString(spriteBatch, expertText, vector + new Vector2(x2, 3f), gameModeColor);
		vector.X += num2 + 5f;
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
		float num4 = innerDimensions.X + innerDimensions.Width - vector.X;
		DrawPanel(spriteBatch, vector, num4);
		string arg = ((!GameCulture.FromCultureName(GameCulture.CultureName.English).IsActive) ? _data.CreationTime.ToShortDateString() : _data.CreationTime.ToString("d MMMM yyyy"));
		string textValue2 = Language.GetTextValue("UI.WorldCreatedFormat", arg);
		float x5 = FontAssets.MouseText.Value.MeasureString(textValue2).X;
		float x6 = num4 * 0.5f - x5 * 0.5f;
		Utils.DrawBorderString(spriteBatch, textValue2, vector + new Vector2(x6, 3f), Color.White);
		vector.X += num4 + 5f;
	}

	private void InitializeTmlFields(WorldFileData data)
	{
		_fileSize = (ulong)FileUtilities.GetFileSize(data.Path, data.IsCloudSave);
	}

	private void LoadTmlTextures()
	{
		_configTexture = UICommon.ButtonConfigTexture;
	}

	private void AddTmlElements(WorldFileData data, ref float offset)
	{
		if (data.usedMods == null)
		{
			return;
		}
		string[] currentModNames = Terraria.ModLoader.ModLoader.Mods.Select((Mod m) => m.Name).ToArray();
		List<string> missingMods = data.usedMods.Except(currentModNames).ToList();
		List<string> newMods = currentModNames.Except(new string[1] { "ModLoader" }).Except(data.usedMods).ToList();
		bool checkModPack = Path.GetFileNameWithoutExtension(ModOrganizer.ModPackActive) != data.modPack;
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
		UIImageButton modListWarning = new UIImageButton(UICommon.ButtonErrorTexture)
		{
			VAlign = 0f,
			HAlign = 1f
		};
		modListWarning.Top.Set(-2f, 0f);
		StringBuilder fullSB = new StringBuilder(Language.GetTextValue("tModLoader.ModsDifferentSinceLastPlay"));
		StringBuilder shortSB = new StringBuilder();
		if (checkModPack)
		{
			string pack = data.modPack;
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
			Interface.infoMessage.Show(fullWarning, 888, Main._worldSelectMenu);
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

	internal static Action PlayReload()
	{
		string path = Main.ActivePlayerFileData.Path;
		bool isCloudSave = Main.ActivePlayerFileData.IsCloudSave;
		return delegate
		{
			Player.GetFileData(path, isCloudSave).SetAsActive();
			WorldGen.playWorld();
		};
	}

	private void ConfigMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		_buttonLabel.SetText("Edit World Config");
	}

	private void ConfigButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
	}
}
