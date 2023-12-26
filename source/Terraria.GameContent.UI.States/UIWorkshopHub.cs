using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States;

public class UIWorkshopHub : UIState, IHaveBackButtonCommand
{
	private UIState _previousUIState;

	private UIText _descriptionText;

	private UIElement _buttonUseResourcePacks;

	private UIElement _buttonPublishResourcePacks;

	private UIElement _buttonImportWorlds;

	private UIElement _buttonPublishWorlds;

	private UIElement _buttonBack;

	private UIElement _buttonLogs;

	private UIGamepadHelper _helper;

	private UIElement _buttonMods;

	private UIElement _buttonModSources;

	private UIElement _buttonModBrowser;

	private UIElement _buttonModPack;

	public UIState PreviousUIState { get; set; }

	public static event Action OnWorkshopHubMenuOpened;

	public UIWorkshopHub(UIState stateToGoBackTo)
	{
		_previousUIState = stateToGoBackTo;
	}

	public void EnterHub()
	{
		UIWorkshopHub.OnWorkshopHubMenuOpened();
	}

	public override void OnInitialize()
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		base.OnInitialize();
		int num = 20;
		int num2 = 250;
		int num3 = 50 + num * 2;
		int num4 = Main.minScreenH;
		int num5 = num4 - num2 - num3;
		UIElement uIElement = new UIElement();
		uIElement.Width.Set(600f, 0f);
		uIElement.Top.Set(num2, 0f);
		uIElement.Height.Set(num4 - num2, 0f);
		uIElement.HAlign = 0.5f;
		int num6 = 284;
		UIPanel uIPanel = new UIPanel();
		uIPanel.Width.Set(0f, 1f);
		uIPanel.Height.Set(num5, 0f);
		uIPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
		UIElement uIElement2 = new UIElement();
		uIElement2.Width.Set(0f, 1f);
		uIElement2.Height.Set(num6, 0f);
		uIElement2.SetPadding(0f);
		UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.WorkshopHub"), 0.8f, large: true);
		uITextPanel.HAlign = 0.5f;
		uITextPanel.Top.Set(-46f, 0f);
		uITextPanel.SetPadding(15f);
		uITextPanel.BackgroundColor = new Color(73, 94, 171);
		UITextPanel<LocalizedText> uITextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, large: true);
		uITextPanel2.Width.Set(-10f, 0.5f);
		uITextPanel2.Height.Set(50f, 0f);
		uITextPanel2.VAlign = 1f;
		uITextPanel2.HAlign = 0f;
		uITextPanel2.Top.Set(-num, 0f);
		uITextPanel2.OnMouseOver += FadedMouseOver;
		uITextPanel2.OnMouseOut += FadedMouseOut;
		uITextPanel2.OnLeftClick += GoBackClick;
		uITextPanel2.SetSnapPoint("Back", 0);
		uIElement.Append(uITextPanel2);
		_buttonBack = uITextPanel2;
		UITextPanel<LocalizedText> uITextPanel3 = new UITextPanel<LocalizedText>(Language.GetText("Workshop.ReportLogsButton"), 0.7f, large: true);
		uITextPanel3.Width.Set(-10f, 0.5f);
		uITextPanel3.Height.Set(50f, 0f);
		uITextPanel3.VAlign = 1f;
		uITextPanel3.HAlign = 1f;
		uITextPanel3.Top.Set(-num, 0f);
		uITextPanel3.OnMouseOver += FadedMouseOver;
		uITextPanel3.OnMouseOut += FadedMouseOut;
		uITextPanel3.OnLeftClick += GoLogsClick;
		uITextPanel3.SetSnapPoint("Logs", 0);
		uIElement.Append(uITextPanel3);
		_buttonLogs = uITextPanel3;
		UIElement uIElement3 = MakeButton_OpenWorkshopWorldsImportMenu();
		uIElement3.HAlign = 0f;
		uIElement3.VAlign = 1f;
		uIElement2.Append(uIElement3);
		uIElement3 = MakeButton_OpenUseResourcePacksMenu();
		uIElement3.HAlign = 1f;
		uIElement3.VAlign = 1f;
		uIElement2.Append(uIElement3);
		AppendTmlElements(uIElement2);
		AddHorizontalSeparator(uIPanel, num6 + 6 + 6);
		AddDescriptionPanel(uIPanel, num6 + 8 + 6 + 6, num5 - num6 - 12 - 12 - 8, "desc");
		uIPanel.Append(uIElement2);
		uIElement.Append(uIPanel);
		uIElement.Append(uITextPanel);
		Append(uIElement);
	}

	private UIElement MakeButton_OpenUseResourcePacksMenu()
	{
		UIElement uIElement = MakeFancyButton("Images/UI/Workshop/HubResourcepacks", "Workshop.HubResourcePacks");
		uIElement.OnLeftClick += Click_OpenResourcePacksMenu;
		_buttonUseResourcePacks = uIElement;
		return uIElement;
	}

	private void Click_OpenResourcePacksMenu(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		Main.OpenResourcePacksMenu(this);
	}

	private UIElement MakeButton_OpenWorkshopWorldsImportMenu()
	{
		UIElement uIElement = MakeFancyButton("Images/UI/Workshop/HubWorlds", "Workshop.HubWorlds");
		uIElement.OnLeftClick += Click_OpenWorldImportMenu;
		_buttonImportWorlds = uIElement;
		return uIElement;
	}

	private void Click_OpenWorldImportMenu(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		Main.MenuUI.SetState(new UIWorkshopWorldImport(this));
	}

	private UIElement MakeButton_OpenPublishResourcePacksMenu()
	{
		UIElement uIElement = MakeFancyButton("Images/UI/Workshop/HubPublishResourcepacks", "Workshop.HubPublishResourcePacks");
		uIElement.OnLeftClick += Click_OpenResourcePackPublishMenu;
		_buttonPublishResourcePacks = uIElement;
		return uIElement;
	}

	private void Click_OpenResourcePackPublishMenu(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		Main.MenuUI.SetState(new UIWorkshopSelectResourcePackToPublish(this));
	}

	private UIElement MakeButton_OpenPublishWorldsMenu()
	{
		UIElement uIElement = MakeFancyButton("Images/UI/Workshop/HubPublishWorlds", "Workshop.HubPublishWorlds");
		uIElement.OnLeftClick += Click_OpenWorldPublishMenu;
		_buttonPublishWorlds = uIElement;
		return uIElement;
	}

	private void Click_OpenWorldPublishMenu(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		Main.MenuUI.SetState(new UIWorkshopSelectWorldToPublish(this));
	}

	private static void AddHorizontalSeparator(UIElement Container, float accumualtedHeight)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		UIHorizontalSeparator element = new UIHorizontalSeparator
		{
			Width = StyleDimension.FromPercent(1f),
			Top = StyleDimension.FromPixels(accumualtedHeight - 8f),
			Color = Color.Lerp(Color.White, new Color(63, 65, 151, 255), 0.85f) * 0.9f
		};
		Container.Append(element);
	}

	public void ShowOptionDescription(UIMouseEvent evt, UIElement listeningElement)
	{
		LocalizedText localizedText = null;
		OnChooseOptionDescription(listeningElement, ref localizedText);
		if (listeningElement == _buttonUseResourcePacks)
		{
			localizedText = Language.GetText("Workshop.HubDescriptionUseResourcePacks");
		}
		if (listeningElement == _buttonPublishResourcePacks)
		{
			localizedText = Language.GetText("Workshop.HubDescriptionPublishResourcePacks");
		}
		if (listeningElement == _buttonImportWorlds)
		{
			localizedText = Language.GetText("Workshop.HubDescriptionImportWorlds");
		}
		if (listeningElement == _buttonPublishWorlds)
		{
			localizedText = Language.GetText("Workshop.HubDescriptionPublishWorlds");
		}
		if (localizedText != null)
		{
			_descriptionText.SetText(localizedText);
		}
	}

	public void ClearOptionDescription(UIMouseEvent evt, UIElement listeningElement)
	{
		_descriptionText.SetText(Language.GetText("Workshop.HubDescriptionDefault"));
	}

	private void AddDescriptionPanel(UIElement container, float accumulatedHeight, float height, string tagGroup)
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		UISlicedImage uISlicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight"))
		{
			HAlign = 0.5f,
			VAlign = 1f,
			Width = StyleDimension.FromPixelsAndPercent((0f - num) * 2f, 1f),
			Left = StyleDimension.FromPixels(0f - num),
			Height = StyleDimension.FromPixelsAndPercent(height, 0f),
			Top = StyleDimension.FromPixels(2f)
		};
		uISlicedImage.SetSliceDepths(10);
		uISlicedImage.Color = Color.LightGray * 0.7f;
		container.Append(uISlicedImage);
		UIText uIText = new UIText(Language.GetText("Workshop.HubDescriptionDefault"))
		{
			HAlign = 0f,
			VAlign = 0f,
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Top = StyleDimension.FromPixelsAndPercent(5f, 0f)
		};
		uIText.PaddingLeft = 20f;
		uIText.PaddingRight = 20f;
		uIText.PaddingTop = 6f;
		uIText.IsWrapped = true;
		uISlicedImage.Append(uIText);
		_descriptionText = uIText;
	}

	private UIElement MakeFancyButton(string iconImagePath, string textKey)
	{
		return MakeFancyButton_Inner(Main.Assets.Request<Texture2D>(iconImagePath), textKey);
	}

	private UIElement MakeFancyButton_Inner(Asset<Texture2D> iconImage, string textKey)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		UIPanel uIPanel = new UIPanel();
		int num = -3;
		int num2 = -3;
		uIPanel.Width = StyleDimension.FromPixelsAndPercent(num, 0.5f);
		uIPanel.Height = StyleDimension.FromPixelsAndPercent(num2, 0.33f);
		uIPanel.OnMouseOver += SetColorsToHovered;
		uIPanel.OnMouseOut += SetColorsToNotHovered;
		uIPanel.BackgroundColor = new Color(63, 82, 151) * 0.7f;
		uIPanel.BorderColor = new Color(89, 116, 213) * 0.7f;
		uIPanel.SetPadding(6f);
		UIImage uIImage = new UIImage(iconImage)
		{
			IgnoresMouseInteraction = true,
			VAlign = 0.5f
		};
		uIImage.Left.Set(2f, 0f);
		uIPanel.Append(uIImage);
		uIPanel.OnMouseOver += ShowOptionDescription;
		uIPanel.OnMouseOut += ClearOptionDescription;
		UIText uIText = new UIText(Language.GetText(textKey), 0.45f, large: true)
		{
			HAlign = 0f,
			VAlign = 0.5f,
			Width = StyleDimension.FromPixelsAndPercent(-80f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Top = StyleDimension.FromPixelsAndPercent(5f, 0f),
			Left = StyleDimension.FromPixels(80f),
			IgnoresMouseInteraction = true,
			TextOriginX = 0f,
			TextOriginY = 0f
		};
		uIText.PaddingLeft = 0f;
		uIText.PaddingRight = 20f;
		uIText.PaddingTop = 10f;
		uIText.IsWrapped = true;
		uIPanel.Append(uIText);
		uIPanel.SetSnapPoint("Button", 0);
		return uIPanel;
	}

	private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
	{
		HandleBackButtonUsage();
		SoundEngine.PlaySound(11);
	}

	private void GoLogsClick(UIMouseEvent evt, UIElement listeningElement)
	{
		Main.IssueReporterIndicator.Hide();
		Main.OpenReportsMenu();
		SoundEngine.PlaySound(10);
	}

	private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(12);
		((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
		((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
	}

	private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
		((UIPanel)evt.Target).BorderColor = Color.Black;
	}

	private void SetColorsToHovered(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		UIPanel obj = (UIPanel)evt.Target;
		obj.BackgroundColor = new Color(73, 94, 171);
		obj.BorderColor = new Color(89, 116, 213);
	}

	private void SetColorsToNotHovered(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		UIPanel obj = (UIPanel)evt.Target;
		obj.BackgroundColor = new Color(63, 82, 151) * 0.7f;
		obj.BorderColor = new Color(89, 116, 213) * 0.7f;
	}

	public void HandleBackButtonUsage()
	{
		if (PreviousUIState == null)
		{
			Main.menuMode = 0;
			return;
		}
		Main.menuMode = 888;
		Main.MenuUI.SetState(PreviousUIState);
		SoundEngine.PlaySound(11);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
		SetupGamepadPoints(spriteBatch);
	}

	private void SetupGamepadPoints(SpriteBatch spriteBatch)
	{
		UILinkPointNavigator.Shortcuts.BackButtonCommand = 7;
	}

	static UIWorkshopHub()
	{
		UIWorkshopHub.OnWorkshopHubMenuOpened = delegate
		{
		};
	}

	private void AppendTmlElements(UIElement uiElement)
	{
		UIElement modsMenu = MakeButton_OpenModsMenu();
		modsMenu.HAlign = 0f;
		modsMenu.VAlign = 0f;
		uiElement.Append(modsMenu);
		UIElement modSources = MakeButton_OpenModSourcesMenu();
		modSources.HAlign = 1f;
		modSources.VAlign = 0f;
		uiElement.Append(modSources);
		UIElement modBrowser = MakeButton_OpenModBrowserMenu();
		modBrowser.HAlign = 0f;
		modBrowser.VAlign = 0.5f;
		uiElement.Append(modBrowser);
		UIElement tbd = MakeButton_ModPackMenu();
		tbd.HAlign = 1f;
		tbd.VAlign = 0.5f;
		uiElement.Append(tbd);
	}

	private void OnChooseOptionDescription(UIElement listeningElement, ref LocalizedText localizedText)
	{
		if (listeningElement == _buttonMods)
		{
			localizedText = Language.GetText("tModLoader.MenuManageModsDescription");
		}
		if (listeningElement == _buttonModSources)
		{
			localizedText = Language.GetText("tModLoader.MenuDevelopModsDescription");
		}
		if (listeningElement == _buttonModBrowser)
		{
			localizedText = Language.GetText("tModLoader.MenuDownloadModsDescription");
		}
		if (listeningElement == _buttonModPack)
		{
			localizedText = Language.GetText("tModLoader.MenuModPackDescription");
		}
	}

	private UIElement MakeButton_OpenModsMenu()
	{
		UIElement uIElement = MakeFancyButtonMod("Terraria.GameContent.UI.States.HubManageMods", "tModLoader.MenuManageMods");
		uIElement.OnLeftClick += Click_OpenModsMenu;
		_buttonMods = uIElement;
		return uIElement;
	}

	private void Click_OpenModsMenu(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		Interface.modsMenu.PreviousUIState = this;
		Main.MenuUI.SetState(Interface.modsMenu);
	}

	private UIElement MakeButton_OpenModSourcesMenu()
	{
		UIElement uIElement = MakeFancyButtonMod("Terraria.GameContent.UI.States.HubDevelopMods", "tModLoader.MenuDevelopMods");
		uIElement.OnLeftClick += Click_OpenModSourcesMenu;
		_buttonModSources = uIElement;
		return uIElement;
	}

	private void Click_OpenModSourcesMenu(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		Interface.modSources.PreviousUIState = this;
		Main.MenuUI.SetState(Interface.modSources);
	}

	private UIElement MakeButton_OpenModBrowserMenu()
	{
		UIElement uIElement = MakeFancyButtonMod("Terraria.GameContent.UI.States.HubDownloadMods", "tModLoader.MenuDownloadMods");
		uIElement.OnLeftClick += Click_OpenModBrowserMenu;
		_buttonModBrowser = uIElement;
		return uIElement;
	}

	private void Click_OpenModBrowserMenu(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		Interface.modBrowser.PreviousUIState = this;
		Main.MenuUI.SetState(Interface.modBrowser);
	}

	private UIElement MakeButton_ModPackMenu()
	{
		UIElement uIElement = MakeFancyButtonMod("Terraria.GameContent.UI.States.HubModPacks", "tModLoader.ModsModPacks");
		uIElement.OnLeftClick += Click_OpenModPackMenu;
		_buttonModPack = uIElement;
		return uIElement;
	}

	private void Click_OpenModPackMenu(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		Interface.modPacksMenu.PreviousUIState = this;
		Main.MenuUI.SetState(Interface.modPacksMenu);
	}

	private UIElement MakeFancyButtonMod(string path, string textKey)
	{
		return MakeFancyButton_Inner(Terraria.ModLoader.ModLoader.ManifestAssets.Request<Texture2D>(path), textKey);
	}
}
