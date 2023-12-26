using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Initializers;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States;

public class UIManageControls : UIState
{
	public static int ForceMoveTo;

	private const float PanelTextureHeight = 30f;

	private static List<string> _BindingsFullLine;

	private static List<string> _BindingsHalfSingleLine;

	private bool OnKeyboard = true;

	private bool OnGameplay = true;

	private List<UIElement> _bindsKeyboard = new List<UIElement>();

	private List<UIElement> _bindsGamepad = new List<UIElement>();

	private List<UIElement> _bindsKeyboardUI = new List<UIElement>();

	private List<UIElement> _bindsGamepadUI = new List<UIElement>();

	private UIElement _outerContainer;

	private UIList _uilist;

	private UIImageFramed _buttonKeyboard;

	private UIImageFramed _buttonGamepad;

	private UIImageFramed _buttonBorder1;

	private UIImageFramed _buttonBorder2;

	private UIKeybindingSimpleListItem _buttonProfile;

	private UIElement _buttonBack;

	private UIImageFramed _buttonVs1;

	private UIImageFramed _buttonVs2;

	private UIImageFramed _buttonBorderVs1;

	private UIImageFramed _buttonBorderVs2;

	private Asset<Texture2D> _KeyboardGamepadTexture;

	private Asset<Texture2D> _keyboardGamepadBorderTexture;

	private Asset<Texture2D> _GameplayVsUITexture;

	private Asset<Texture2D> _GameplayVsUIBorderTexture;

	private static int SnapPointIndex;

	private const string ResetModKeybinds = "ResetModKeybinds";

	private const string ClearModKeybinds = "ClearModKeybinds";

	private const int TmlBindingGroupId = 5;

	private static readonly string[] tmlBindings;

	private static readonly List<string> _ModBindings;

	private static readonly List<string> _ModNames;

	public override void OnInitialize()
	{
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_061b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06de: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f3: Unknown result type (might be due to invalid IL or missing references)
		_KeyboardGamepadTexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Inputs");
		_keyboardGamepadBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Inputs_Border");
		_GameplayVsUITexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Inputs_2");
		_GameplayVsUIBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Inputs_2_Border");
		UIElement uIElement = new UIElement();
		uIElement.Width.Set(0f, 0.8f);
		uIElement.MaxWidth.Set(600f, 0f);
		uIElement.Top.Set(220f, 0f);
		uIElement.Height.Set(-200f, 1f);
		uIElement.HAlign = 0.5f;
		_outerContainer = uIElement;
		UIPanel uIPanel = new UIPanel();
		uIPanel.Width.Set(0f, 1f);
		uIPanel.Height.Set(-110f, 1f);
		uIPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
		uIElement.Append(uIPanel);
		_buttonKeyboard = new UIImageFramed(_KeyboardGamepadTexture, _KeyboardGamepadTexture.Frame(2, 2));
		_buttonKeyboard.VAlign = 0f;
		_buttonKeyboard.HAlign = 0f;
		_buttonKeyboard.Left.Set(0f, 0f);
		_buttonKeyboard.Top.Set(8f, 0f);
		_buttonKeyboard.OnLeftClick += KeyboardButtonClick;
		_buttonKeyboard.OnMouseOver += ManageBorderKeyboardOn;
		_buttonKeyboard.OnMouseOut += ManageBorderKeyboardOff;
		uIPanel.Append(_buttonKeyboard);
		_buttonGamepad = new UIImageFramed(_KeyboardGamepadTexture, _KeyboardGamepadTexture.Frame(2, 2, 1, 1));
		_buttonGamepad.VAlign = 0f;
		_buttonGamepad.HAlign = 0f;
		_buttonGamepad.Left.Set(76f, 0f);
		_buttonGamepad.Top.Set(8f, 0f);
		_buttonGamepad.OnLeftClick += GamepadButtonClick;
		_buttonGamepad.OnMouseOver += ManageBorderGamepadOn;
		_buttonGamepad.OnMouseOut += ManageBorderGamepadOff;
		uIPanel.Append(_buttonGamepad);
		_buttonBorder1 = new UIImageFramed(_keyboardGamepadBorderTexture, _keyboardGamepadBorderTexture.Frame());
		_buttonBorder1.VAlign = 0f;
		_buttonBorder1.HAlign = 0f;
		_buttonBorder1.Left.Set(0f, 0f);
		_buttonBorder1.Top.Set(8f, 0f);
		_buttonBorder1.Color = Color.Silver;
		_buttonBorder1.IgnoresMouseInteraction = true;
		uIPanel.Append(_buttonBorder1);
		_buttonBorder2 = new UIImageFramed(_keyboardGamepadBorderTexture, _keyboardGamepadBorderTexture.Frame());
		_buttonBorder2.VAlign = 0f;
		_buttonBorder2.HAlign = 0f;
		_buttonBorder2.Left.Set(76f, 0f);
		_buttonBorder2.Top.Set(8f, 0f);
		_buttonBorder2.Color = Color.Transparent;
		_buttonBorder2.IgnoresMouseInteraction = true;
		uIPanel.Append(_buttonBorder2);
		_buttonVs1 = new UIImageFramed(_GameplayVsUITexture, _GameplayVsUITexture.Frame(2, 2));
		_buttonVs1.VAlign = 0f;
		_buttonVs1.HAlign = 0f;
		_buttonVs1.Left.Set(172f, 0f);
		_buttonVs1.Top.Set(8f, 0f);
		_buttonVs1.OnLeftClick += VsGameplayButtonClick;
		_buttonVs1.OnMouseOver += ManageBorderGameplayOn;
		_buttonVs1.OnMouseOut += ManageBorderGameplayOff;
		uIPanel.Append(_buttonVs1);
		_buttonVs2 = new UIImageFramed(_GameplayVsUITexture, _GameplayVsUITexture.Frame(2, 2, 1, 1));
		_buttonVs2.VAlign = 0f;
		_buttonVs2.HAlign = 0f;
		_buttonVs2.Left.Set(212f, 0f);
		_buttonVs2.Top.Set(8f, 0f);
		_buttonVs2.OnLeftClick += VsMenuButtonClick;
		_buttonVs2.OnMouseOver += ManageBorderMenuOn;
		_buttonVs2.OnMouseOut += ManageBorderMenuOff;
		uIPanel.Append(_buttonVs2);
		_buttonBorderVs1 = new UIImageFramed(_GameplayVsUIBorderTexture, _GameplayVsUIBorderTexture.Frame());
		_buttonBorderVs1.VAlign = 0f;
		_buttonBorderVs1.HAlign = 0f;
		_buttonBorderVs1.Left.Set(172f, 0f);
		_buttonBorderVs1.Top.Set(8f, 0f);
		_buttonBorderVs1.Color = Color.Silver;
		_buttonBorderVs1.IgnoresMouseInteraction = true;
		uIPanel.Append(_buttonBorderVs1);
		_buttonBorderVs2 = new UIImageFramed(_GameplayVsUIBorderTexture, _GameplayVsUIBorderTexture.Frame());
		_buttonBorderVs2.VAlign = 0f;
		_buttonBorderVs2.HAlign = 0f;
		_buttonBorderVs2.Left.Set(212f, 0f);
		_buttonBorderVs2.Top.Set(8f, 0f);
		_buttonBorderVs2.Color = Color.Transparent;
		_buttonBorderVs2.IgnoresMouseInteraction = true;
		uIPanel.Append(_buttonBorderVs2);
		_buttonProfile = new UIKeybindingSimpleListItem(() => PlayerInput.CurrentProfile.ShowName, new Color(73, 94, 171, 255) * 0.9f);
		_buttonProfile.VAlign = 0f;
		_buttonProfile.HAlign = 1f;
		_buttonProfile.Width.Set(180f, 0f);
		_buttonProfile.Height.Set(30f, 0f);
		_buttonProfile.MarginRight = 30f;
		_buttonProfile.Left.Set(0f, 0f);
		_buttonProfile.Top.Set(8f, 0f);
		_buttonProfile.OnLeftClick += profileButtonClick;
		uIPanel.Append(_buttonProfile);
		_uilist = new UIList();
		_uilist.Width.Set(-25f, 1f);
		_uilist.Height.Set(-50f, 1f);
		_uilist.VAlign = 1f;
		_uilist.PaddingBottom = 5f;
		_uilist.ListPadding = 20f;
		uIPanel.Append(_uilist);
		AssembleBindPanels();
		FillList();
		UIScrollbar uIScrollbar = new UIScrollbar();
		uIScrollbar.SetView(100f, 1000f);
		uIScrollbar.Height.Set(-67f, 1f);
		uIScrollbar.HAlign = 1f;
		uIScrollbar.VAlign = 1f;
		uIScrollbar.MarginBottom = 11f;
		uIPanel.Append(uIScrollbar);
		_uilist.SetScrollbar(uIScrollbar);
		UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Keybindings"), 0.7f, large: true);
		uITextPanel.HAlign = 0.5f;
		uITextPanel.Top.Set(-45f, 0f);
		uITextPanel.Left.Set(-10f, 0f);
		uITextPanel.SetPadding(15f);
		uITextPanel.BackgroundColor = new Color(73, 94, 171);
		uIElement.Append(uITextPanel);
		UITextPanel<LocalizedText> uITextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, large: true);
		uITextPanel2.Width.Set(-10f, 0.5f);
		uITextPanel2.Height.Set(50f, 0f);
		uITextPanel2.VAlign = 1f;
		uITextPanel2.HAlign = 0.5f;
		uITextPanel2.Top.Set(-45f, 0f);
		uITextPanel2.OnMouseOver += FadedMouseOver;
		uITextPanel2.OnMouseOut += FadedMouseOut;
		uITextPanel2.OnLeftClick += GoBackClick;
		uIElement.Append(uITextPanel2);
		_buttonBack = uITextPanel2;
		Append(uIElement);
	}

	private void AssembleBindPanels()
	{
		List<string> bindings = new List<string>
		{
			"MouseLeft", "MouseRight", "MouseMiddle", "MouseXButton1", "MouseXButton2", "Up", "Down", "Left", "Right", "Jump",
			"Grapple", "SmartSelect", "SmartCursor", "QuickMount", "QuickHeal", "QuickMana", "QuickBuff", "Throw", "Inventory", "ToggleCreativeMenu",
			"ViewZoomIn", "ViewZoomOut", "Loadout1", "Loadout2", "Loadout3", "ToggleCameraMode", "sp9"
		};
		List<string> bindings2 = new List<string>
		{
			"MouseLeft", "MouseRight", "MouseMiddle", "MouseXButton1", "MouseXButton2", "Up", "Down", "Left", "Right", "Jump",
			"Grapple", "SmartSelect", "SmartCursor", "QuickMount", "QuickHeal", "QuickMana", "QuickBuff", "LockOn", "Throw", "Inventory",
			"Loadout1", "Loadout2", "Loadout3", "ToggleCameraMode", "sp9"
		};
		List<string> bindings3 = new List<string>
		{
			"HotbarMinus", "HotbarPlus", "Hotbar1", "Hotbar2", "Hotbar3", "Hotbar4", "Hotbar5", "Hotbar6", "Hotbar7", "Hotbar8",
			"Hotbar9", "Hotbar10", "sp10"
		};
		List<string> bindings4 = new List<string> { "MapZoomIn", "MapZoomOut", "MapAlphaUp", "MapAlphaDown", "MapFull", "MapStyle", "sp11" };
		List<string> bindings5 = new List<string> { "sp1", "sp2", "RadialHotbar", "RadialQuickbar", "sp12" };
		List<string> bindings6 = new List<string>
		{
			"sp3", "sp4", "sp5", "sp6", "sp7", "sp8", "sp14", "sp15", "sp16", "sp17",
			"sp18", "sp19", "sp13"
		};
		OnAssembleBindPanels();
		InputMode currentInputMode = InputMode.Keyboard;
		_bindsKeyboard.Add(CreateBindingGroup(0, bindings, currentInputMode));
		_bindsKeyboard.Add(CreateBindingGroup(1, bindings4, currentInputMode));
		_bindsKeyboard.Add(CreateBindingGroup(2, bindings3, currentInputMode));
		currentInputMode = InputMode.XBoxGamepad;
		_bindsGamepad.Add(CreateBindingGroup(0, bindings2, currentInputMode));
		_bindsGamepad.Add(CreateBindingGroup(1, bindings4, currentInputMode));
		_bindsGamepad.Add(CreateBindingGroup(2, bindings3, currentInputMode));
		_bindsGamepad.Add(CreateBindingGroup(3, bindings5, currentInputMode));
		_bindsGamepad.Add(CreateBindingGroup(4, bindings6, currentInputMode));
		currentInputMode = InputMode.KeyboardUI;
		_bindsKeyboardUI.Add(CreateBindingGroup(0, bindings, currentInputMode));
		_bindsKeyboardUI.Add(CreateBindingGroup(1, bindings4, currentInputMode));
		_bindsKeyboardUI.Add(CreateBindingGroup(2, bindings3, currentInputMode));
		currentInputMode = InputMode.XBoxGamepadUI;
		_bindsGamepadUI.Add(CreateBindingGroup(0, bindings2, currentInputMode));
		_bindsGamepadUI.Add(CreateBindingGroup(1, bindings4, currentInputMode));
		_bindsGamepadUI.Add(CreateBindingGroup(2, bindings3, currentInputMode));
		_bindsGamepadUI.Add(CreateBindingGroup(3, bindings5, currentInputMode));
		_bindsGamepadUI.Add(CreateBindingGroup(4, bindings6, currentInputMode));
		AddModBindingGroups();
	}

	private UISortableElement CreateBindingGroup(int elementIndex, List<string> bindings, InputMode currentInputMode)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		UISortableElement uISortableElement = new UISortableElement(elementIndex);
		uISortableElement.HAlign = 0.5f;
		uISortableElement.Width.Set(0f, 1f);
		uISortableElement.Height.Set(2000f, 0f);
		UIPanel uIPanel = new UIPanel();
		uIPanel.Width.Set(0f, 1f);
		uIPanel.Height.Set(-16f, 1f);
		uIPanel.VAlign = 1f;
		uIPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
		uISortableElement.Append(uIPanel);
		UIList uIList = new UIList();
		uIList.OverflowHidden = false;
		uIList.Width.Set(0f, 1f);
		uIList.Height.Set(-8f, 1f);
		uIList.VAlign = 1f;
		uIList.ListPadding = 5f;
		uIPanel.Append(uIList);
		_ = uIPanel.BackgroundColor;
		switch (elementIndex)
		{
		case 0:
			uIPanel.BackgroundColor = Color.Lerp(uIPanel.BackgroundColor, Color.Green, 0.18f);
			break;
		case 1:
			uIPanel.BackgroundColor = Color.Lerp(uIPanel.BackgroundColor, Color.Goldenrod, 0.18f);
			break;
		case 2:
			uIPanel.BackgroundColor = Color.Lerp(uIPanel.BackgroundColor, Color.HotPink, 0.18f);
			break;
		case 3:
			uIPanel.BackgroundColor = Color.Lerp(uIPanel.BackgroundColor, Color.Indigo, 0.18f);
			break;
		case 4:
			uIPanel.BackgroundColor = Color.Lerp(uIPanel.BackgroundColor, Color.Turquoise, 0.18f);
			break;
		case 5:
			uIPanel.BackgroundColor = Color.Lerp(uIPanel.BackgroundColor, Color.Yellow, 0.18f);
			break;
		}
		CreateElementGroup(uIList, bindings, currentInputMode, uIPanel.BackgroundColor);
		uIPanel.BackgroundColor = uIPanel.BackgroundColor.MultiplyRGBA(new Color(111, 111, 111));
		LocalizedText text = LocalizedText.Empty;
		switch (elementIndex)
		{
		case 0:
			text = ((currentInputMode == InputMode.Keyboard || currentInputMode == InputMode.XBoxGamepad) ? Lang.menu[164] : Lang.menu[243]);
			break;
		case 1:
			text = Lang.menu[165];
			break;
		case 2:
			text = Lang.menu[166];
			break;
		case 3:
			text = Lang.menu[167];
			break;
		case 4:
			text = Lang.menu[198];
			break;
		case 5:
			text = Language.GetText("tModLoader.ModControls");
			break;
		}
		UITextPanel<LocalizedText> element = new UITextPanel<LocalizedText>(text, 0.7f)
		{
			VAlign = 0f,
			HAlign = 0.5f
		};
		uISortableElement.Append(element);
		uISortableElement.Recalculate();
		float totalHeight = uIList.GetTotalHeight();
		uISortableElement.Width.Set(0f, 1f);
		uISortableElement.Height.Set(totalHeight + 30f + 16f, 0f);
		return uISortableElement;
	}

	private void CreateElementGroup(UIList parent, List<string> bindings, InputMode currentInputMode, Color color)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < bindings.Count; i++)
		{
			_ = bindings[i];
			UISortableElement uISortableElement = new UISortableElement(i);
			uISortableElement.Width.Set(0f, 1f);
			uISortableElement.Height.Set(30f, 0f);
			uISortableElement.HAlign = 0.5f;
			parent.Add(uISortableElement);
			if (_BindingsHalfSingleLine.Contains(bindings[i]))
			{
				UIElement uIElement = CreatePanel(bindings[i], currentInputMode, color);
				uIElement.Width.Set(0f, 0.5f);
				uIElement.HAlign = 0.5f;
				uIElement.Height.Set(0f, 1f);
				uIElement.SetSnapPoint("Wide", SnapPointIndex++);
				uISortableElement.Append(uIElement);
				continue;
			}
			if (_BindingsFullLine.Contains(bindings[i]))
			{
				UIElement uIElement2 = CreatePanel(bindings[i], currentInputMode, color);
				uIElement2.Width.Set(0f, 1f);
				uIElement2.Height.Set(0f, 1f);
				uIElement2.SetSnapPoint("Wide", SnapPointIndex++);
				uISortableElement.Append(uIElement2);
				continue;
			}
			if (_ModNames.Contains(bindings[i]))
			{
				UIElement header = new HeaderElement(bindings[i]);
				header.Width.Set(0f, 1f);
				header.Height.Set(0f, 1f);
				header.SetSnapPoint("Wide", SnapPointIndex++);
				uISortableElement.Append(header);
				continue;
			}
			UIElement uIElement3 = CreatePanel(bindings[i], currentInputMode, color);
			uIElement3.Width.Set(-5f, 0.5f);
			uIElement3.Height.Set(0f, 1f);
			uIElement3.SetSnapPoint("Thin", SnapPointIndex++);
			uISortableElement.Append(uIElement3);
			i++;
			if (i < bindings.Count)
			{
				uIElement3 = CreatePanel(bindings[i], currentInputMode, color);
				uIElement3.Width.Set(-5f, 0.5f);
				uIElement3.Height.Set(0f, 1f);
				uIElement3.HAlign = 1f;
				uIElement3.SetSnapPoint("Thin", SnapPointIndex++);
				uISortableElement.Append(uIElement3);
			}
		}
	}

	public UIElement CreatePanel(string bind, InputMode currentInputMode, Color color)
	{
		//IL_077c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_070c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0949: Unknown result type (might be due to invalid IL or missing references)
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_08df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0744: Unknown result type (might be due to invalid IL or missing references)
		//IL_080b: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f6: Unknown result type (might be due to invalid IL or missing references)
		switch (bind)
		{
		case "sp1":
		{
			UIKeybindingToggleListItem uIKeybindingToggleListItem6 = new UIKeybindingToggleListItem(() => Lang.menu[196].Value, delegate
			{
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_0090: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
				//IL_0100: Unknown result type (might be due to invalid IL or missing references)
				//IL_0138: Unknown result type (might be due to invalid IL or missing references)
				//IL_016d: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
				List<string> list = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap1"];
				Buttons val = (Buttons)1;
				if (list.Contains(((object)(Buttons)(ref val)).ToString()))
				{
					List<string> list2 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap2"];
					val = (Buttons)8;
					if (list2.Contains(((object)(Buttons)(ref val)).ToString()))
					{
						List<string> list3 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap3"];
						val = (Buttons)2;
						if (list3.Contains(((object)(Buttons)(ref val)).ToString()))
						{
							List<string> list4 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap4"];
							val = (Buttons)4;
							if (list4.Contains(((object)(Buttons)(ref val)).ToString()))
							{
								List<string> list5 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap1"];
								val = (Buttons)1;
								if (list5.Contains(((object)(Buttons)(ref val)).ToString()))
								{
									List<string> list6 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap2"];
									val = (Buttons)8;
									if (list6.Contains(((object)(Buttons)(ref val)).ToString()))
									{
										List<string> list7 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap3"];
										val = (Buttons)2;
										if (list7.Contains(((object)(Buttons)(ref val)).ToString()))
										{
											List<string> list8 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap4"];
											val = (Buttons)4;
											return list8.Contains(((object)(Buttons)(ref val)).ToString());
										}
									}
								}
							}
						}
					}
				}
				return false;
			}, color);
			uIKeybindingToggleListItem6.OnLeftClick += SnapButtonClick;
			return uIKeybindingToggleListItem6;
		}
		case "sp2":
		{
			UIKeybindingToggleListItem uIKeybindingToggleListItem5 = new UIKeybindingToggleListItem(() => Lang.menu[197].Value, delegate
			{
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_0090: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
				//IL_0100: Unknown result type (might be due to invalid IL or missing references)
				//IL_0138: Unknown result type (might be due to invalid IL or missing references)
				//IL_016d: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
				List<string> list9 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial1"];
				Buttons val2 = (Buttons)1;
				if (list9.Contains(((object)(Buttons)(ref val2)).ToString()))
				{
					List<string> list10 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial2"];
					val2 = (Buttons)8;
					if (list10.Contains(((object)(Buttons)(ref val2)).ToString()))
					{
						List<string> list11 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial3"];
						val2 = (Buttons)2;
						if (list11.Contains(((object)(Buttons)(ref val2)).ToString()))
						{
							List<string> list12 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial4"];
							val2 = (Buttons)4;
							if (list12.Contains(((object)(Buttons)(ref val2)).ToString()))
							{
								List<string> list13 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial1"];
								val2 = (Buttons)1;
								if (list13.Contains(((object)(Buttons)(ref val2)).ToString()))
								{
									List<string> list14 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial2"];
									val2 = (Buttons)8;
									if (list14.Contains(((object)(Buttons)(ref val2)).ToString()))
									{
										List<string> list15 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial3"];
										val2 = (Buttons)2;
										if (list15.Contains(((object)(Buttons)(ref val2)).ToString()))
										{
											List<string> list16 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial4"];
											val2 = (Buttons)4;
											return list16.Contains(((object)(Buttons)(ref val2)).ToString());
										}
									}
								}
							}
						}
					}
				}
				return false;
			}, color);
			uIKeybindingToggleListItem5.OnLeftClick += RadialButtonClick;
			return uIKeybindingToggleListItem5;
		}
		case "sp3":
			return new UIKeybindingSliderItem(() => Lang.menu[199].Value + " (" + PlayerInput.CurrentProfile.TriggersDeadzone.ToString("P1") + ")", () => PlayerInput.CurrentProfile.TriggersDeadzone, delegate(float f)
			{
				PlayerInput.CurrentProfile.TriggersDeadzone = f;
			}, delegate
			{
				PlayerInput.CurrentProfile.TriggersDeadzone = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.TriggersDeadzone, 0f, 0.95f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			}, 1000, color);
		case "sp4":
			return new UIKeybindingSliderItem(() => Lang.menu[200].Value + " (" + PlayerInput.CurrentProfile.InterfaceDeadzoneX.ToString("P1") + ")", () => PlayerInput.CurrentProfile.InterfaceDeadzoneX, delegate(float f)
			{
				PlayerInput.CurrentProfile.InterfaceDeadzoneX = f;
			}, delegate
			{
				PlayerInput.CurrentProfile.InterfaceDeadzoneX = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0f, 0.95f, 0.35f, 0.35f);
			}, 1001, color);
		case "sp5":
			return new UIKeybindingSliderItem(() => Lang.menu[201].Value + " (" + PlayerInput.CurrentProfile.LeftThumbstickDeadzoneX.ToString("P1") + ")", () => PlayerInput.CurrentProfile.LeftThumbstickDeadzoneX, delegate(float f)
			{
				PlayerInput.CurrentProfile.LeftThumbstickDeadzoneX = f;
			}, delegate
			{
				PlayerInput.CurrentProfile.LeftThumbstickDeadzoneX = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.LeftThumbstickDeadzoneX, 0f, 0.95f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			}, 1002, color);
		case "sp6":
			return new UIKeybindingSliderItem(() => Lang.menu[202].Value + " (" + PlayerInput.CurrentProfile.LeftThumbstickDeadzoneY.ToString("P1") + ")", () => PlayerInput.CurrentProfile.LeftThumbstickDeadzoneY, delegate(float f)
			{
				PlayerInput.CurrentProfile.LeftThumbstickDeadzoneY = f;
			}, delegate
			{
				PlayerInput.CurrentProfile.LeftThumbstickDeadzoneY = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.LeftThumbstickDeadzoneY, 0f, 0.95f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			}, 1003, color);
		case "sp7":
			return new UIKeybindingSliderItem(() => Lang.menu[203].Value + " (" + PlayerInput.CurrentProfile.RightThumbstickDeadzoneX.ToString("P1") + ")", () => PlayerInput.CurrentProfile.RightThumbstickDeadzoneX, delegate(float f)
			{
				PlayerInput.CurrentProfile.RightThumbstickDeadzoneX = f;
			}, delegate
			{
				PlayerInput.CurrentProfile.RightThumbstickDeadzoneX = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.RightThumbstickDeadzoneX, 0f, 0.95f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			}, 1004, color);
		case "sp8":
			return new UIKeybindingSliderItem(() => Lang.menu[204].Value + " (" + PlayerInput.CurrentProfile.RightThumbstickDeadzoneY.ToString("P1") + ")", () => PlayerInput.CurrentProfile.RightThumbstickDeadzoneY, delegate(float f)
			{
				PlayerInput.CurrentProfile.RightThumbstickDeadzoneY = f;
			}, delegate
			{
				PlayerInput.CurrentProfile.RightThumbstickDeadzoneY = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.RightThumbstickDeadzoneY, 0f, 0.95f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			}, 1005, color);
		case "sp9":
		{
			UIKeybindingSimpleListItem uIKeybindingSimpleListItem3 = new UIKeybindingSimpleListItem(() => Lang.menu[86].Value, color);
			uIKeybindingSimpleListItem3.OnLeftClick += delegate
			{
				string copyableProfileName3 = GetCopyableProfileName();
				PlayerInput.CurrentProfile.CopyGameplaySettingsFrom(PlayerInput.OriginalProfiles[copyableProfileName3], currentInputMode);
			};
			return uIKeybindingSimpleListItem3;
		}
		case "sp10":
		{
			UIKeybindingSimpleListItem uIKeybindingSimpleListItem5 = new UIKeybindingSimpleListItem(() => Lang.menu[86].Value, color);
			uIKeybindingSimpleListItem5.OnLeftClick += delegate
			{
				string copyableProfileName = GetCopyableProfileName();
				PlayerInput.CurrentProfile.CopyHotbarSettingsFrom(PlayerInput.OriginalProfiles[copyableProfileName], currentInputMode);
			};
			return uIKeybindingSimpleListItem5;
		}
		case "sp11":
		{
			UIKeybindingSimpleListItem uIKeybindingSimpleListItem2 = new UIKeybindingSimpleListItem(() => Lang.menu[86].Value, color);
			uIKeybindingSimpleListItem2.OnLeftClick += delegate
			{
				string copyableProfileName4 = GetCopyableProfileName();
				PlayerInput.CurrentProfile.CopyMapSettingsFrom(PlayerInput.OriginalProfiles[copyableProfileName4], currentInputMode);
			};
			return uIKeybindingSimpleListItem2;
		}
		case "sp12":
		{
			UIKeybindingSimpleListItem uIKeybindingSimpleListItem = new UIKeybindingSimpleListItem(() => Lang.menu[86].Value, color);
			uIKeybindingSimpleListItem.OnLeftClick += delegate
			{
				string copyableProfileName5 = GetCopyableProfileName();
				PlayerInput.CurrentProfile.CopyGamepadSettingsFrom(PlayerInput.OriginalProfiles[copyableProfileName5], currentInputMode);
			};
			return uIKeybindingSimpleListItem;
		}
		case "sp13":
		{
			UIKeybindingSimpleListItem uIKeybindingSimpleListItem4 = new UIKeybindingSimpleListItem(() => Lang.menu[86].Value, color);
			uIKeybindingSimpleListItem4.OnLeftClick += delegate
			{
				string copyableProfileName2 = GetCopyableProfileName();
				PlayerInput.CurrentProfile.CopyGamepadAdvancedSettingsFrom(PlayerInput.OriginalProfiles[copyableProfileName2], currentInputMode);
			};
			return uIKeybindingSimpleListItem4;
		}
		case "sp14":
		{
			UIKeybindingToggleListItem uIKeybindingToggleListItem2 = new UIKeybindingToggleListItem(() => Lang.menu[205].Value, () => PlayerInput.CurrentProfile.LeftThumbstickInvertX, color);
			uIKeybindingToggleListItem2.OnLeftClick += delegate
			{
				if (PlayerInput.CurrentProfile.AllowEditting)
				{
					PlayerInput.CurrentProfile.LeftThumbstickInvertX = !PlayerInput.CurrentProfile.LeftThumbstickInvertX;
				}
			};
			return uIKeybindingToggleListItem2;
		}
		case "sp15":
		{
			UIKeybindingToggleListItem uIKeybindingToggleListItem4 = new UIKeybindingToggleListItem(() => Lang.menu[206].Value, () => PlayerInput.CurrentProfile.LeftThumbstickInvertY, color);
			uIKeybindingToggleListItem4.OnLeftClick += delegate
			{
				if (PlayerInput.CurrentProfile.AllowEditting)
				{
					PlayerInput.CurrentProfile.LeftThumbstickInvertY = !PlayerInput.CurrentProfile.LeftThumbstickInvertY;
				}
			};
			return uIKeybindingToggleListItem4;
		}
		case "sp16":
		{
			UIKeybindingToggleListItem uIKeybindingToggleListItem3 = new UIKeybindingToggleListItem(() => Lang.menu[207].Value, () => PlayerInput.CurrentProfile.RightThumbstickInvertX, color);
			uIKeybindingToggleListItem3.OnLeftClick += delegate
			{
				if (PlayerInput.CurrentProfile.AllowEditting)
				{
					PlayerInput.CurrentProfile.RightThumbstickInvertX = !PlayerInput.CurrentProfile.RightThumbstickInvertX;
				}
			};
			return uIKeybindingToggleListItem3;
		}
		case "sp17":
		{
			UIKeybindingToggleListItem uIKeybindingToggleListItem = new UIKeybindingToggleListItem(() => Lang.menu[208].Value, () => PlayerInput.CurrentProfile.RightThumbstickInvertY, color);
			uIKeybindingToggleListItem.OnLeftClick += delegate
			{
				if (PlayerInput.CurrentProfile.AllowEditting)
				{
					PlayerInput.CurrentProfile.RightThumbstickInvertY = !PlayerInput.CurrentProfile.RightThumbstickInvertY;
				}
			};
			return uIKeybindingToggleListItem;
		}
		case "sp18":
			return new UIKeybindingSliderItem(delegate
			{
				int hotbarRadialHoldTimeRequired = PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired;
				return (hotbarRadialHoldTimeRequired != -1) ? (Lang.menu[227].Value + " (" + ((float)hotbarRadialHoldTimeRequired / 60f).ToString("F2") + "s)") : Lang.menu[228].Value;
			}, () => (PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired != -1) ? ((float)PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired / 301f) : 1f, delegate(float f)
			{
				PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired = (int)(f * 301f);
				if ((float)PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired == 301f)
				{
					PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired = -1;
				}
			}, delegate
			{
				float currentValue = ((PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired == -1) ? 1f : ((float)PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired / 301f));
				currentValue = UILinksInitializer.HandleSliderHorizontalInput(currentValue, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX);
				PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired = (int)(currentValue * 301f);
				if ((float)PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired == 301f)
				{
					PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired = -1;
				}
			}, 1007, color);
		case "sp19":
			return new UIKeybindingSliderItem(delegate
			{
				int inventoryMoveCD = PlayerInput.CurrentProfile.InventoryMoveCD;
				return Lang.menu[252].Value + " (" + ((float)inventoryMoveCD / 60f).ToString("F2") + "s)";
			}, () => Utils.GetLerpValue(4f, 12f, PlayerInput.CurrentProfile.InventoryMoveCD, clamped: true), delegate(float f)
			{
				PlayerInput.CurrentProfile.InventoryMoveCD = (int)Math.Round(MathHelper.Lerp(4f, 12f, f));
			}, delegate
			{
				if (UILinkPointNavigator.Shortcuts.INV_MOVE_OPTION_CD > 0)
				{
					UILinkPointNavigator.Shortcuts.INV_MOVE_OPTION_CD--;
				}
				if (UILinkPointNavigator.Shortcuts.INV_MOVE_OPTION_CD == 0)
				{
					float lerpValue = Utils.GetLerpValue(4f, 12f, PlayerInput.CurrentProfile.InventoryMoveCD, clamped: true);
					float num = UILinksInitializer.HandleSliderHorizontalInput(lerpValue, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX);
					if (lerpValue != num)
					{
						UILinkPointNavigator.Shortcuts.INV_MOVE_OPTION_CD = 8;
						int num2 = Math.Sign(num - lerpValue);
						PlayerInput.CurrentProfile.InventoryMoveCD = (int)MathHelper.Clamp((float)(PlayerInput.CurrentProfile.InventoryMoveCD + num2), 4f, 12f);
					}
				}
			}, 1008, color);
		default:
		{
			UIElement tmlResult = HandlePanelCreation(bind, currentInputMode, color);
			if (tmlResult != null)
			{
				return tmlResult;
			}
			return new UIKeybindingListItem(bind, currentInputMode, color);
		}
		}
	}

	public override void OnActivate()
	{
		_bindsKeyboard.Clear();
		_bindsGamepad.Clear();
		_bindsKeyboardUI.Clear();
		_bindsGamepadUI.Clear();
		AssembleBindPanels();
		FillList();
		if (Main.gameMenu)
		{
			_outerContainer.Top.Set(220f, 0f);
			_outerContainer.Height.Set(-220f, 1f);
		}
		else
		{
			_outerContainer.Top.Set(120f, 0f);
			_outerContainer.Height.Set(-120f, 1f);
		}
		if (PlayerInput.UsingGamepadUI)
		{
			UILinkPointNavigator.ChangePoint(3002);
		}
	}

	private static string GetCopyableProfileName()
	{
		string result = "Redigit's Pick";
		if (PlayerInput.OriginalProfiles.ContainsKey(PlayerInput.CurrentProfile.Name))
		{
			result = PlayerInput.CurrentProfile.Name;
		}
		return result;
	}

	private void FillList()
	{
		List<UIElement> list = _bindsKeyboard;
		if (!OnKeyboard)
		{
			list = _bindsGamepad;
		}
		if (!OnGameplay)
		{
			list = (OnKeyboard ? _bindsKeyboardUI : _bindsGamepadUI);
		}
		_uilist.Clear();
		foreach (UIElement item in list)
		{
			_uilist.Add(item);
		}
	}

	private void SnapButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		if (!PlayerInput.CurrentProfile.AllowEditting)
		{
			return;
		}
		SoundEngine.PlaySound(12);
		List<string> list = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap1"];
		Buttons val = (Buttons)1;
		if (list.Contains(((object)(Buttons)(ref val)).ToString()))
		{
			List<string> list2 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap2"];
			val = (Buttons)8;
			if (list2.Contains(((object)(Buttons)(ref val)).ToString()))
			{
				List<string> list3 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap3"];
				val = (Buttons)2;
				if (list3.Contains(((object)(Buttons)(ref val)).ToString()))
				{
					List<string> list4 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap4"];
					val = (Buttons)4;
					if (list4.Contains(((object)(Buttons)(ref val)).ToString()))
					{
						List<string> list5 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap1"];
						val = (Buttons)1;
						if (list5.Contains(((object)(Buttons)(ref val)).ToString()))
						{
							List<string> list6 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap2"];
							val = (Buttons)8;
							if (list6.Contains(((object)(Buttons)(ref val)).ToString()))
							{
								List<string> list7 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap3"];
								val = (Buttons)2;
								if (list7.Contains(((object)(Buttons)(ref val)).ToString()))
								{
									List<string> list8 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap4"];
									val = (Buttons)4;
									if (list8.Contains(((object)(Buttons)(ref val)).ToString()))
									{
										PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap1"].Clear();
										PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap2"].Clear();
										PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap3"].Clear();
										PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap4"].Clear();
										PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap1"].Clear();
										PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap2"].Clear();
										PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap3"].Clear();
										PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap4"].Clear();
										return;
									}
								}
							}
						}
					}
				}
			}
		}
		PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial1"].Clear();
		PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial2"].Clear();
		PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial3"].Clear();
		PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial4"].Clear();
		PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial1"].Clear();
		PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial2"].Clear();
		PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial3"].Clear();
		PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial4"].Clear();
		Dictionary<string, List<string>> keyStatus = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus;
		List<string> list9 = new List<string>();
		val = (Buttons)1;
		list9.Add(((object)(Buttons)(ref val)).ToString());
		keyStatus["DpadSnap1"] = list9;
		Dictionary<string, List<string>> keyStatus2 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus;
		List<string> list10 = new List<string>();
		val = (Buttons)8;
		list10.Add(((object)(Buttons)(ref val)).ToString());
		keyStatus2["DpadSnap2"] = list10;
		Dictionary<string, List<string>> keyStatus3 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus;
		List<string> list11 = new List<string>();
		val = (Buttons)2;
		list11.Add(((object)(Buttons)(ref val)).ToString());
		keyStatus3["DpadSnap3"] = list11;
		Dictionary<string, List<string>> keyStatus4 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus;
		List<string> list12 = new List<string>();
		val = (Buttons)4;
		list12.Add(((object)(Buttons)(ref val)).ToString());
		keyStatus4["DpadSnap4"] = list12;
		Dictionary<string, List<string>> keyStatus5 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus;
		List<string> list13 = new List<string>();
		val = (Buttons)1;
		list13.Add(((object)(Buttons)(ref val)).ToString());
		keyStatus5["DpadSnap1"] = list13;
		Dictionary<string, List<string>> keyStatus6 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus;
		List<string> list14 = new List<string>();
		val = (Buttons)8;
		list14.Add(((object)(Buttons)(ref val)).ToString());
		keyStatus6["DpadSnap2"] = list14;
		Dictionary<string, List<string>> keyStatus7 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus;
		List<string> list15 = new List<string>();
		val = (Buttons)2;
		list15.Add(((object)(Buttons)(ref val)).ToString());
		keyStatus7["DpadSnap3"] = list15;
		Dictionary<string, List<string>> keyStatus8 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus;
		List<string> list16 = new List<string>();
		val = (Buttons)4;
		list16.Add(((object)(Buttons)(ref val)).ToString());
		keyStatus8["DpadSnap4"] = list16;
	}

	private void RadialButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		if (!PlayerInput.CurrentProfile.AllowEditting)
		{
			return;
		}
		SoundEngine.PlaySound(12);
		List<string> list = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial1"];
		Buttons val = (Buttons)1;
		if (list.Contains(((object)(Buttons)(ref val)).ToString()))
		{
			List<string> list2 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial2"];
			val = (Buttons)8;
			if (list2.Contains(((object)(Buttons)(ref val)).ToString()))
			{
				List<string> list3 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial3"];
				val = (Buttons)2;
				if (list3.Contains(((object)(Buttons)(ref val)).ToString()))
				{
					List<string> list4 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial4"];
					val = (Buttons)4;
					if (list4.Contains(((object)(Buttons)(ref val)).ToString()))
					{
						List<string> list5 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial1"];
						val = (Buttons)1;
						if (list5.Contains(((object)(Buttons)(ref val)).ToString()))
						{
							List<string> list6 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial2"];
							val = (Buttons)8;
							if (list6.Contains(((object)(Buttons)(ref val)).ToString()))
							{
								List<string> list7 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial3"];
								val = (Buttons)2;
								if (list7.Contains(((object)(Buttons)(ref val)).ToString()))
								{
									List<string> list8 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial4"];
									val = (Buttons)4;
									if (list8.Contains(((object)(Buttons)(ref val)).ToString()))
									{
										PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial1"].Clear();
										PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial2"].Clear();
										PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial3"].Clear();
										PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial4"].Clear();
										PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial1"].Clear();
										PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial2"].Clear();
										PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial3"].Clear();
										PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial4"].Clear();
										return;
									}
								}
							}
						}
					}
				}
			}
		}
		PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap1"].Clear();
		PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap2"].Clear();
		PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap3"].Clear();
		PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap4"].Clear();
		PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap1"].Clear();
		PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap2"].Clear();
		PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap3"].Clear();
		PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap4"].Clear();
		Dictionary<string, List<string>> keyStatus = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus;
		List<string> list9 = new List<string>();
		val = (Buttons)1;
		list9.Add(((object)(Buttons)(ref val)).ToString());
		keyStatus["DpadRadial1"] = list9;
		Dictionary<string, List<string>> keyStatus2 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus;
		List<string> list10 = new List<string>();
		val = (Buttons)8;
		list10.Add(((object)(Buttons)(ref val)).ToString());
		keyStatus2["DpadRadial2"] = list10;
		Dictionary<string, List<string>> keyStatus3 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus;
		List<string> list11 = new List<string>();
		val = (Buttons)2;
		list11.Add(((object)(Buttons)(ref val)).ToString());
		keyStatus3["DpadRadial3"] = list11;
		Dictionary<string, List<string>> keyStatus4 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus;
		List<string> list12 = new List<string>();
		val = (Buttons)4;
		list12.Add(((object)(Buttons)(ref val)).ToString());
		keyStatus4["DpadRadial4"] = list12;
		Dictionary<string, List<string>> keyStatus5 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus;
		List<string> list13 = new List<string>();
		val = (Buttons)1;
		list13.Add(((object)(Buttons)(ref val)).ToString());
		keyStatus5["DpadRadial1"] = list13;
		Dictionary<string, List<string>> keyStatus6 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus;
		List<string> list14 = new List<string>();
		val = (Buttons)8;
		list14.Add(((object)(Buttons)(ref val)).ToString());
		keyStatus6["DpadRadial2"] = list14;
		Dictionary<string, List<string>> keyStatus7 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus;
		List<string> list15 = new List<string>();
		val = (Buttons)2;
		list15.Add(((object)(Buttons)(ref val)).ToString());
		keyStatus7["DpadRadial3"] = list15;
		Dictionary<string, List<string>> keyStatus8 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus;
		List<string> list16 = new List<string>();
		val = (Buttons)4;
		list16.Add(((object)(Buttons)(ref val)).ToString());
		keyStatus8["DpadRadial4"] = list16;
	}

	private void KeyboardButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		_buttonKeyboard.SetFrame(_KeyboardGamepadTexture.Frame(2, 2));
		_buttonGamepad.SetFrame(_KeyboardGamepadTexture.Frame(2, 2, 1, 1));
		OnKeyboard = true;
		FillList();
	}

	private void GamepadButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		_buttonKeyboard.SetFrame(_KeyboardGamepadTexture.Frame(2, 2, 0, 1));
		_buttonGamepad.SetFrame(_KeyboardGamepadTexture.Frame(2, 2, 1));
		OnKeyboard = false;
		FillList();
	}

	private void ManageBorderKeyboardOn(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		_buttonBorder2.Color = ((!OnKeyboard) ? Color.Silver : Color.Black);
		_buttonBorder1.Color = Main.OurFavoriteColor;
	}

	private void ManageBorderKeyboardOff(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		_buttonBorder2.Color = ((!OnKeyboard) ? Color.Silver : Color.Black);
		_buttonBorder1.Color = (OnKeyboard ? Color.Silver : Color.Black);
	}

	private void ManageBorderGamepadOn(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		_buttonBorder1.Color = (OnKeyboard ? Color.Silver : Color.Black);
		_buttonBorder2.Color = Main.OurFavoriteColor;
	}

	private void ManageBorderGamepadOff(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		_buttonBorder1.Color = (OnKeyboard ? Color.Silver : Color.Black);
		_buttonBorder2.Color = ((!OnKeyboard) ? Color.Silver : Color.Black);
	}

	private void VsGameplayButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		_buttonVs1.SetFrame(_GameplayVsUITexture.Frame(2, 2));
		_buttonVs2.SetFrame(_GameplayVsUITexture.Frame(2, 2, 1, 1));
		OnGameplay = true;
		FillList();
	}

	private void VsMenuButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		_buttonVs1.SetFrame(_GameplayVsUITexture.Frame(2, 2, 0, 1));
		_buttonVs2.SetFrame(_GameplayVsUITexture.Frame(2, 2, 1));
		OnGameplay = false;
		FillList();
	}

	private void ManageBorderGameplayOn(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		_buttonBorderVs2.Color = ((!OnGameplay) ? Color.Silver : Color.Black);
		_buttonBorderVs1.Color = Main.OurFavoriteColor;
	}

	private void ManageBorderGameplayOff(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		_buttonBorderVs2.Color = ((!OnGameplay) ? Color.Silver : Color.Black);
		_buttonBorderVs1.Color = (OnGameplay ? Color.Silver : Color.Black);
	}

	private void ManageBorderMenuOn(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		_buttonBorderVs1.Color = (OnGameplay ? Color.Silver : Color.Black);
		_buttonBorderVs2.Color = Main.OurFavoriteColor;
	}

	private void ManageBorderMenuOff(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		_buttonBorderVs1.Color = (OnGameplay ? Color.Silver : Color.Black);
		_buttonBorderVs2.Color = ((!OnGameplay) ? Color.Silver : Color.Black);
	}

	private void profileButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		string name = PlayerInput.CurrentProfile.Name;
		List<string> list = PlayerInput.Profiles.Keys.ToList();
		int num = list.IndexOf(name);
		num++;
		if (num >= list.Count)
		{
			num -= list.Count;
		}
		PlayerInput.SetSelectedProfile(list[num]);
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
		((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.7f;
		((UIPanel)evt.Target).BorderColor = Color.Black;
	}

	private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
	{
		Main.menuMode = 1127;
		IngameFancyUI.Close();
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
		SetupGamepadPoints(spriteBatch);
	}

	private void SetupGamepadPoints(SpriteBatch spriteBatch)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		UILinkPointNavigator.Shortcuts.BackButtonCommand = 4;
		int num = 3000;
		Rectangle val = _buttonBack.GetInnerDimensions().ToRectangle();
		UILinkPointNavigator.SetPosition(num, ((Rectangle)(ref val)).Center.ToVector2());
		int iD = num + 1;
		val = _buttonKeyboard.GetInnerDimensions().ToRectangle();
		UILinkPointNavigator.SetPosition(iD, ((Rectangle)(ref val)).Center.ToVector2());
		int iD2 = num + 2;
		val = _buttonGamepad.GetInnerDimensions().ToRectangle();
		UILinkPointNavigator.SetPosition(iD2, ((Rectangle)(ref val)).Center.ToVector2());
		int iD3 = num + 3;
		val = _buttonProfile.GetInnerDimensions().ToRectangle();
		UILinkPointNavigator.SetPosition(iD3, ((Rectangle)(ref val)).Center.ToVector2());
		int iD4 = num + 4;
		val = _buttonVs1.GetInnerDimensions().ToRectangle();
		UILinkPointNavigator.SetPosition(iD4, ((Rectangle)(ref val)).Center.ToVector2());
		int iD5 = num + 5;
		val = _buttonVs2.GetInnerDimensions().ToRectangle();
		UILinkPointNavigator.SetPosition(iD5, ((Rectangle)(ref val)).Center.ToVector2());
		int key = num;
		UILinkPoint uILinkPoint = UILinkPointNavigator.Points[key];
		uILinkPoint.Unlink();
		uILinkPoint.Up = num + 6;
		key = num + 1;
		UILinkPoint uILinkPoint2 = UILinkPointNavigator.Points[key];
		uILinkPoint2.Unlink();
		uILinkPoint2.Right = num + 2;
		uILinkPoint2.Down = num + 6;
		key = num + 2;
		UILinkPoint uILinkPoint3 = UILinkPointNavigator.Points[key];
		uILinkPoint3.Unlink();
		uILinkPoint3.Left = num + 1;
		uILinkPoint3.Right = num + 4;
		uILinkPoint3.Down = num + 6;
		key = num + 4;
		UILinkPoint uILinkPoint4 = UILinkPointNavigator.Points[key];
		uILinkPoint4.Unlink();
		uILinkPoint4.Left = num + 2;
		uILinkPoint4.Right = num + 5;
		uILinkPoint4.Down = num + 6;
		key = num + 5;
		UILinkPoint uILinkPoint5 = UILinkPointNavigator.Points[key];
		uILinkPoint5.Unlink();
		uILinkPoint5.Left = num + 4;
		uILinkPoint5.Right = num + 3;
		uILinkPoint5.Down = num + 6;
		key = num + 3;
		UILinkPoint uILinkPoint6 = UILinkPointNavigator.Points[key];
		uILinkPoint6.Unlink();
		uILinkPoint6.Left = num + 5;
		uILinkPoint6.Down = num + 6;
		float num2 = 1f / Main.UIScale;
		Rectangle clippingRectangle = _uilist.GetClippingRectangle(spriteBatch);
		Vector2 minimum = clippingRectangle.TopLeft() * num2;
		Vector2 maximum = clippingRectangle.BottomRight() * num2;
		List<SnapPoint> snapPoints = _uilist.GetSnapPoints();
		for (int i = 0; i < snapPoints.Count; i++)
		{
			if (!snapPoints[i].Position.Between(minimum, maximum))
			{
				_ = snapPoints[i].Position;
				snapPoints.Remove(snapPoints[i]);
				i--;
			}
		}
		snapPoints.Sort((SnapPoint x, SnapPoint y) => x.Id.CompareTo(y.Id));
		for (int j = 0; j < snapPoints.Count; j++)
		{
			key = num + 6 + j;
			if (snapPoints[j].Name == "Thin")
			{
				UILinkPoint uILinkPoint7 = UILinkPointNavigator.Points[key];
				uILinkPoint7.Unlink();
				UILinkPointNavigator.SetPosition(key, snapPoints[j].Position);
				uILinkPoint7.Right = key + 1;
				uILinkPoint7.Down = ((j < snapPoints.Count - 2) ? (key + 2) : num);
				uILinkPoint7.Up = ((j < 2) ? (num + 1) : ((snapPoints[j - 1].Name == "Wide") ? (key - 1) : (key - 2)));
				UILinkPointNavigator.Points[num].Up = key;
				UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX = key;
				j++;
				if (j < snapPoints.Count)
				{
					key = num + 6 + j;
					UILinkPoint uILinkPoint8 = UILinkPointNavigator.Points[key];
					uILinkPoint8.Unlink();
					UILinkPointNavigator.SetPosition(key, snapPoints[j].Position);
					uILinkPoint8.Left = key - 1;
					uILinkPoint8.Down = ((j >= snapPoints.Count - 1) ? num : ((snapPoints[j + 1].Name == "Wide") ? (key + 1) : (key + 2)));
					uILinkPoint8.Up = ((j < 2) ? (num + 1) : (key - 2));
					UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX = key;
				}
			}
			else
			{
				UILinkPoint uILinkPoint9 = UILinkPointNavigator.Points[key];
				uILinkPoint9.Unlink();
				UILinkPointNavigator.SetPosition(key, snapPoints[j].Position);
				uILinkPoint9.Down = ((j < snapPoints.Count - 1) ? (key + 1) : num);
				uILinkPoint9.Up = ((j < 1) ? (num + 1) : ((snapPoints[j - 1].Name == "Wide") ? (key - 1) : (key - 2)));
				UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX = key;
				UILinkPointNavigator.Points[num].Up = key;
			}
		}
		if (ForceMoveTo != -1)
		{
			UILinkPointNavigator.ChangePoint((int)MathHelper.Clamp((float)ForceMoveTo, (float)num, (float)UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX));
			ForceMoveTo = -1;
		}
	}

	static UIManageControls()
	{
		ForceMoveTo = -1;
		_BindingsFullLine = new List<string>
		{
			"Throw", "Inventory", "RadialHotbar", "RadialQuickbar", "LockOn", "ToggleCreativeMenu", "Loadout1", "Loadout2", "Loadout3", "ToggleCameraMode",
			"sp3", "sp4", "sp5", "sp6", "sp7", "sp8", "sp18", "sp19", "sp9", "sp10",
			"sp11", "sp12", "sp13"
		};
		_BindingsHalfSingleLine = new List<string> { "sp9", "sp10", "sp11", "sp12", "sp13" };
		tmlBindings = new string[2] { "ResetModKeybinds", "ClearModKeybinds" };
		_ModBindings = new List<string>();
		_ModNames = new List<string>();
		_BindingsFullLine.AddRange(tmlBindings);
		_BindingsHalfSingleLine.AddRange(tmlBindings);
	}

	private void OnAssembleBindPanels()
	{
		_BindingsFullLine.RemoveAll((string x) => x.Contains('/'));
		_ModBindings.Clear();
		_ModNames.Clear();
		Mod currentMod = null;
		foreach (ModKeybind keybind in KeybindLoader.Keybinds)
		{
			if (currentMod != keybind.Mod)
			{
				currentMod = keybind.Mod;
				_ModBindings.Add(keybind.Mod.DisplayName);
				_ModNames.Add(keybind.Mod.DisplayName);
			}
			_ModBindings.Add(keybind.FullName);
			_BindingsFullLine.Add(keybind.FullName);
		}
		_ModBindings.AddRange(tmlBindings);
	}

	private void AddModBindingGroups()
	{
		_bindsKeyboard.Add(CreateBindingGroup(5, _ModBindings, InputMode.Keyboard));
		_bindsGamepad.Add(CreateBindingGroup(5, _ModBindings, InputMode.XBoxGamepad));
		_bindsKeyboardUI.Add(CreateBindingGroup(5, _ModBindings, InputMode.KeyboardUI));
		_bindsGamepadUI.Add(CreateBindingGroup(5, _ModBindings, InputMode.XBoxGamepadUI));
	}

	private UIElement? HandlePanelCreation(string bind, InputMode currentInputMode, Color color)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		string bind2 = bind;
		string text = bind2;
		if (!(text == "ResetModKeybinds"))
		{
			if (text == "ClearModKeybinds")
			{
				UIKeybindingSimpleListItem uIKeybindingSimpleListItem = new UIKeybindingSimpleListItem(() => "Clear", color);
				uIKeybindingSimpleListItem.OnLeftClick += delegate
				{
					foreach (ModKeybind current in KeybindLoader.Keybinds)
					{
						PlayerInput.CurrentProfile.InputModes[currentInputMode].KeyStatus[current.FullName].Clear();
					}
				};
				return uIKeybindingSimpleListItem;
			}
			if (!_ModBindings.Contains(bind2))
			{
				return null;
			}
			string defaultKey = KeybindLoader.modKeybinds[bind2].DefaultBinding;
			UIElement uIElement = new UIElement();
			uIElement.Append(new UIKeybindingListItem(bind2, currentInputMode, color)
			{
				Width = 
				{
					Precent = 0.58f
				},
				Height = 
				{
					Precent = 1f
				}
			});
			UIKeybindingSimpleListItem right = new UIKeybindingSimpleListItem(() => Lang.menu[86].Value + " (" + defaultKey + ")", color);
			right.OnLeftClick += delegate
			{
				string copyableProfileName2 = GetCopyableProfileName();
				PlayerInput.CurrentProfile.CopyIndividualModKeybindSettingsFrom(PlayerInput.OriginalProfiles[copyableProfileName2], currentInputMode, bind2);
			};
			right.Left.Precent = 0.6f;
			right.Width.Precent = 0.4f;
			right.Height.Precent = 1f;
			uIElement.Append(right);
			return uIElement;
		}
		UIKeybindingSimpleListItem uIKeybindingSimpleListItem2 = new UIKeybindingSimpleListItem(() => Lang.menu[86].Value, color);
		uIKeybindingSimpleListItem2.OnLeftClick += delegate
		{
			string copyableProfileName = GetCopyableProfileName();
			PlayerInput.CurrentProfile.CopyModKeybindSettingsFrom(PlayerInput.OriginalProfiles[copyableProfileName], currentInputMode);
		};
		return uIKeybindingSimpleListItem2;
	}
}
