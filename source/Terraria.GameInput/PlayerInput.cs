using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria.Audio;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Chat;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Social;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameInput;

public class PlayerInput
{
	public class MiscSettingsTEMP
	{
		public static bool HotbarRadialShouldBeUsed = true;
	}

	public static class SettingsForUI
	{
		public static CursorMode CurrentCursorMode { get; private set; }

		public static bool ShowGamepadHints
		{
			get
			{
				if (!UsingGamepad)
				{
					return SteamDeckIsUsed;
				}
				return true;
			}
		}

		public static bool AllowSecondaryGamepadAim
		{
			get
			{
				if (CurrentCursorMode != CursorMode.Gamepad)
				{
					return !SteamDeckIsUsed;
				}
				return true;
			}
		}

		public static bool PushEquipmentAreaUp
		{
			get
			{
				if (UsingGamepad)
				{
					return !SteamDeckIsUsed;
				}
				return false;
			}
		}

		public static bool ShowGamepadCursor
		{
			get
			{
				if (!SteamDeckIsUsed)
				{
					return UsingGamepad;
				}
				return CurrentCursorMode == CursorMode.Gamepad;
			}
		}

		public static bool HighlightThingsForMouse
		{
			get
			{
				if (UsingGamepadUI)
				{
					return CurrentCursorMode == CursorMode.Mouse;
				}
				return true;
			}
		}

		public static int FramesSinceLastTimeInMouseMode { get; private set; }

		public static bool PreventHighlightsForGamepad => FramesSinceLastTimeInMouseMode == 0;

		public static void SetCursorMode(CursorMode cursorMode)
		{
			CurrentCursorMode = cursorMode;
			if (CurrentCursorMode == CursorMode.Mouse)
			{
				FramesSinceLastTimeInMouseMode = 0;
			}
		}

		public static void UpdateCounters()
		{
			if (CurrentCursorMode != 0)
			{
				FramesSinceLastTimeInMouseMode++;
			}
		}

		public static void TryRevertingToMouseMode()
		{
			if (FramesSinceLastTimeInMouseMode <= 0)
			{
				SetCursorMode(CursorMode.Mouse);
				CurrentInputMode = InputMode.Mouse;
				Triggers.Current.UsedMovementKey = false;
				NavigatorUnCachePosition();
			}
		}
	}

	private struct FastUseItemMemory
	{
		private int _slot;

		private int _itemType;

		private bool _shouldFastUse;

		private bool _isMouseItem;

		private Player _player;

		public bool TryStartForItemSlot(Player player, int itemSlot)
		{
			if (itemSlot < 0 || itemSlot >= 50)
			{
				Clear();
				return false;
			}
			_player = player;
			_slot = itemSlot;
			_itemType = _player.inventory[itemSlot].type;
			_shouldFastUse = true;
			_isMouseItem = false;
			ItemSlot.PickupItemIntoMouse(player.inventory, 0, itemSlot, player);
			return true;
		}

		public bool TryStartForMouse(Player player)
		{
			_player = player;
			_slot = -1;
			_itemType = Main.mouseItem.type;
			_shouldFastUse = true;
			_isMouseItem = true;
			return true;
		}

		public void Clear()
		{
			_shouldFastUse = false;
		}

		public bool CanFastUse()
		{
			if (!_shouldFastUse)
			{
				return false;
			}
			if (_isMouseItem)
			{
				return Main.mouseItem.type == _itemType;
			}
			return _player.inventory[_slot].type == _itemType;
		}

		public void EndFastUse()
		{
			if (_shouldFastUse)
			{
				if (!_isMouseItem && _player.inventory[_slot].IsAir)
				{
					Utils.Swap(ref Main.mouseItem, ref _player.inventory[_slot]);
				}
				Clear();
			}
		}
	}

	public static Vector2 RawMouseScale;

	public static TriggersPack Triggers;

	public static List<string> KnownTriggers;

	private static bool _canReleaseRebindingLock;

	private static int _memoOfLastPoint;

	public static int NavigatorRebindingLock;

	public static string BlockedKey;

	private static string _listeningTrigger;

	private static InputMode _listeningInputMode;

	public static Dictionary<string, PlayerInputProfile> Profiles;

	public static Dictionary<string, PlayerInputProfile> OriginalProfiles;

	private static string _selectedProfile;

	private static PlayerInputProfile _currentProfile;

	public static InputMode CurrentInputMode;

	private static Buttons[] ButtonsGamepad;

	public static bool GrappleAndInteractAreShared;

	public static SmartSelectGamepadPointer smartSelectPointer;

	public static bool UseSteamDeckIfPossible;

	private static string _invalidatorCheck;

	private static bool _lastActivityState;

	public static MouseState MouseInfo;

	public static MouseState MouseInfoOld;

	public static int MouseX;

	public static int MouseY;

	public static bool LockGamepadTileUseButton;

	public static List<string> MouseKeys;

	public static int PreUIX;

	public static int PreUIY;

	public static int PreLockOnX;

	public static int PreLockOnY;

	public static int ScrollWheelValue;

	public static int ScrollWheelValueOld;

	public static int ScrollWheelDelta;

	public static int ScrollWheelDeltaForUI;

	public static bool GamepadAllowScrolling;

	public static int GamepadScrollValue;

	public static Vector2 GamepadThumbstickLeft;

	public static Vector2 GamepadThumbstickRight;

	private static FastUseItemMemory _fastUseMemory;

	private static bool _InBuildingMode;

	private static int _UIPointForBuildingMode;

	public static bool WritingText;

	private static int _originalMouseX;

	private static int _originalMouseY;

	private static int _originalLastMouseX;

	private static int _originalLastMouseY;

	private static int _originalScreenWidth;

	private static int _originalScreenHeight;

	private static ZoomContext _currentWantedZoom;

	private static List<string> _buttonsLocked;

	public static bool PreventCursorModeSwappingToGamepad;

	public static bool PreventFirstMousePositionGrab;

	private static int[] DpadSnapCooldown;

	public static bool AllowExecutionOfGamepadInstructions;

	internal static bool reinitialize;

	internal static List<string> MouseInModdedUI;

	public static string ListeningTrigger => _listeningTrigger;

	public static bool CurrentlyRebinding => _listeningTrigger != null;

	public static bool InvisibleGamepadInMenus
	{
		get
		{
			if ((!Main.gameMenu && !Main.ingameOptionsWindow && !Main.playerInventory && Main.player[Main.myPlayer].talkNPC == -1 && Main.player[Main.myPlayer].sign == -1 && Main.InGameUI.CurrentState == null) || _InBuildingMode || !Main.InvisibleCursorForGamepad)
			{
				if (CursorIsBusy)
				{
					return !_InBuildingMode;
				}
				return false;
			}
			return true;
		}
	}

	public static PlayerInputProfile CurrentProfile => _currentProfile;

	public static KeyConfiguration ProfileGamepadUI => CurrentProfile.InputModes[InputMode.XBoxGamepadUI];

	public static bool UsingGamepad
	{
		get
		{
			if (CurrentInputMode != InputMode.XBoxGamepad)
			{
				return CurrentInputMode == InputMode.XBoxGamepadUI;
			}
			return true;
		}
	}

	public static bool UsingGamepadUI => CurrentInputMode == InputMode.XBoxGamepadUI;

	public static bool IgnoreMouseInterface
	{
		get
		{
			if (Main.LocalPlayer.itemAnimation > 0 && !UsingGamepad)
			{
				return true;
			}
			bool flag = UsingGamepad && !UILinkPointNavigator.Available;
			if (flag && SteamDeckIsUsed && SettingsForUI.CurrentCursorMode == CursorMode.Mouse && !Main.mouseRight)
			{
				return false;
			}
			return flag;
		}
	}

	public static bool SteamDeckIsUsed => UseSteamDeckIfPossible;

	public static bool ShouldFastUseItem => _fastUseMemory.CanFastUse();

	public static bool InBuildingMode => _InBuildingMode;

	public static int RealScreenWidth => _originalScreenWidth;

	public static int RealScreenHeight => _originalScreenHeight;

	public static bool CursorIsBusy
	{
		get
		{
			if (!(ItemSlot.CircularRadialOpacity > 0f))
			{
				return ItemSlot.QuicksRadialOpacity > 0f;
			}
			return true;
		}
	}

	public static Vector2 OriginalScreenSize => new Vector2((float)_originalScreenWidth, (float)_originalScreenHeight);

	public static event Action OnBindingChange;

	public static event Action OnActionableInput;

	public static void ListenFor(string triggerName, InputMode inputmode)
	{
		_listeningTrigger = triggerName;
		_listeningInputMode = inputmode;
	}

	private static bool InvalidateKeyboardSwap()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		if (_invalidatorCheck.Length == 0)
		{
			return false;
		}
		string text = "";
		List<Keys> pressedKeys = GetPressedKeys();
		for (int i = 0; i < pressedKeys.Count; i++)
		{
			text = string.Concat(text, pressedKeys[i], ", ");
		}
		if (text == _invalidatorCheck)
		{
			return true;
		}
		_invalidatorCheck = "";
		return false;
	}

	public static void ResetInputsOnActiveStateChange()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		bool isActive = ((Game)Main.instance).IsActive;
		if (_lastActivityState != isActive)
		{
			MouseInfo = default(MouseState);
			MouseInfoOld = default(MouseState);
			Main.keyState = Keyboard.GetState();
			Main.inputText = Keyboard.GetState();
			Main.oldInputText = Keyboard.GetState();
			Main.keyCount = 0;
			Triggers.Reset();
			Triggers.Reset();
			string text = "";
			List<Keys> pressedKeys = GetPressedKeys();
			for (int i = 0; i < pressedKeys.Count; i++)
			{
				text = string.Concat(text, pressedKeys[i], ", ");
			}
			_invalidatorCheck = text;
		}
		_lastActivityState = isActive;
	}

	public static List<Keys> GetPressedKeys()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Invalid comparison between Unknown and I4
		List<Keys> list = ((KeyboardState)(ref Main.keyState)).GetPressedKeys().ToList();
		for (int num = list.Count - 1; num >= 0; num--)
		{
			if ((int)list[num] == 0 || (int)list[num] == 25)
			{
				list.RemoveAt(num);
			}
		}
		return list;
	}

	public static void TryEnteringFastUseModeForInventorySlot(int inventorySlot)
	{
		_fastUseMemory.TryStartForItemSlot(Main.LocalPlayer, inventorySlot);
	}

	public static void TryEnteringFastUseModeForMouseItem()
	{
		_fastUseMemory.TryStartForMouse(Main.LocalPlayer);
	}

	public static void TryEndingFastUse()
	{
		_fastUseMemory.EndFastUse();
	}

	public static void EnterBuildingMode()
	{
		SoundEngine.PlaySound(10);
		_InBuildingMode = true;
		_UIPointForBuildingMode = UILinkPointNavigator.CurrentPoint;
		if (Main.mouseItem.stack <= 0)
		{
			int uIPointForBuildingMode = _UIPointForBuildingMode;
			if (uIPointForBuildingMode < 50 && uIPointForBuildingMode >= 0 && Main.player[Main.myPlayer].inventory[uIPointForBuildingMode].stack > 0)
			{
				Utils.Swap(ref Main.mouseItem, ref Main.player[Main.myPlayer].inventory[uIPointForBuildingMode]);
			}
		}
	}

	public static void ExitBuildingMode()
	{
		SoundEngine.PlaySound(11);
		_InBuildingMode = false;
		UILinkPointNavigator.ChangePoint(_UIPointForBuildingMode);
		if (Main.mouseItem.stack > 0 && Main.player[Main.myPlayer].itemAnimation == 0)
		{
			int uIPointForBuildingMode = _UIPointForBuildingMode;
			if (uIPointForBuildingMode < 50 && uIPointForBuildingMode >= 0 && Main.player[Main.myPlayer].inventory[uIPointForBuildingMode].stack <= 0)
			{
				Utils.Swap(ref Main.mouseItem, ref Main.player[Main.myPlayer].inventory[uIPointForBuildingMode]);
			}
		}
		_UIPointForBuildingMode = -1;
	}

	public static void VerifyBuildingMode()
	{
		if (_InBuildingMode)
		{
			Player obj = Main.player[Main.myPlayer];
			bool flag = false;
			if (Main.mouseItem.stack <= 0)
			{
				flag = true;
			}
			if (obj.dead)
			{
				flag = true;
			}
			if (flag)
			{
				ExitBuildingMode();
			}
		}
	}

	public static void SetSelectedProfile(string name)
	{
		if (Profiles.ContainsKey(name))
		{
			_selectedProfile = name;
			_currentProfile = Profiles[_selectedProfile];
		}
	}

	private static void ReInitialize()
	{
		Profiles.Clear();
		OriginalProfiles.Clear();
		Initialize_Inner();
		Load();
		reinitialize = false;
	}

	public static void Initialize()
	{
		Main.InputProfiles.OnProcessText += PrettyPrintProfiles;
		Player.Hooks.OnEnterWorld += Hook_OnEnterWorld;
		Initialize_Inner();
	}

	private static void Initialize_Inner()
	{
		PlayerInputProfile playerInputProfile = new PlayerInputProfile("Redigit's Pick");
		playerInputProfile.Initialize(PresetProfiles.Redigit);
		Profiles.Add(playerInputProfile.Name, playerInputProfile);
		playerInputProfile = new PlayerInputProfile("Yoraiz0r's Pick");
		playerInputProfile.Initialize(PresetProfiles.Yoraiz0r);
		Profiles.Add(playerInputProfile.Name, playerInputProfile);
		playerInputProfile = new PlayerInputProfile("Console (Playstation)");
		playerInputProfile.Initialize(PresetProfiles.ConsolePS);
		Profiles.Add(playerInputProfile.Name, playerInputProfile);
		playerInputProfile = new PlayerInputProfile("Console (Xbox)");
		playerInputProfile.Initialize(PresetProfiles.ConsoleXBox);
		Profiles.Add(playerInputProfile.Name, playerInputProfile);
		playerInputProfile = new PlayerInputProfile("Custom");
		playerInputProfile.Initialize(PresetProfiles.Redigit);
		Profiles.Add(playerInputProfile.Name, playerInputProfile);
		playerInputProfile = new PlayerInputProfile("Redigit's Pick");
		playerInputProfile.Initialize(PresetProfiles.Redigit);
		OriginalProfiles.Add(playerInputProfile.Name, playerInputProfile);
		playerInputProfile = new PlayerInputProfile("Yoraiz0r's Pick");
		playerInputProfile.Initialize(PresetProfiles.Yoraiz0r);
		OriginalProfiles.Add(playerInputProfile.Name, playerInputProfile);
		playerInputProfile = new PlayerInputProfile("Console (Playstation)");
		playerInputProfile.Initialize(PresetProfiles.ConsolePS);
		OriginalProfiles.Add(playerInputProfile.Name, playerInputProfile);
		playerInputProfile = new PlayerInputProfile("Console (Xbox)");
		playerInputProfile.Initialize(PresetProfiles.ConsoleXBox);
		OriginalProfiles.Add(playerInputProfile.Name, playerInputProfile);
		SetSelectedProfile("Custom");
		Triggers.Initialize();
	}

	public static void Hook_OnEnterWorld(Player player)
	{
		if (player.whoAmI == Main.myPlayer)
		{
			Main.SmartCursorWanted_GamePad = true;
		}
	}

	public static bool Save()
	{
		Main.InputProfiles.Clear();
		Main.InputProfiles.Put("Selected Profile", _selectedProfile);
		foreach (KeyValuePair<string, PlayerInputProfile> profile in Profiles)
		{
			Main.InputProfiles.Put(profile.Value.Name, profile.Value.Save());
		}
		return Main.InputProfiles.Save();
	}

	public static void Load()
	{
		if (!Main.InputProfiles.Load())
		{
			return;
		}
		Dictionary<string, PlayerInputProfile> dictionary = new Dictionary<string, PlayerInputProfile>();
		string currentValue = null;
		Main.InputProfiles.Get("Selected Profile", ref currentValue);
		List<string> allKeys = Main.InputProfiles.GetAllKeys();
		for (int i = 0; i < allKeys.Count; i++)
		{
			string text = allKeys[i];
			if (text == "Selected Profile" || string.IsNullOrEmpty(text))
			{
				continue;
			}
			Dictionary<string, object> currentValue2 = new Dictionary<string, object>();
			Main.InputProfiles.Get(text, ref currentValue2);
			if (currentValue2.Count > 0)
			{
				PlayerInputProfile playerInputProfile = new PlayerInputProfile(text);
				playerInputProfile.Initialize(PresetProfiles.None);
				if (playerInputProfile.Load(currentValue2))
				{
					dictionary.Add(text, playerInputProfile);
				}
			}
		}
		if (dictionary.Count > 0)
		{
			Profiles = dictionary;
			if (!string.IsNullOrEmpty(currentValue) && Profiles.ContainsKey(currentValue))
			{
				SetSelectedProfile(currentValue);
			}
			else
			{
				SetSelectedProfile(Profiles.Keys.First());
			}
		}
	}

	public static void ManageVersion_1_3()
	{
		PlayerInputProfile playerInputProfile = Profiles["Custom"];
		string[,] array = new string[20, 2]
		{
			{ "KeyUp", "Up" },
			{ "KeyDown", "Down" },
			{ "KeyLeft", "Left" },
			{ "KeyRight", "Right" },
			{ "KeyJump", "Jump" },
			{ "KeyThrowItem", "Throw" },
			{ "KeyInventory", "Inventory" },
			{ "KeyQuickHeal", "QuickHeal" },
			{ "KeyQuickMana", "QuickMana" },
			{ "KeyQuickBuff", "QuickBuff" },
			{ "KeyUseHook", "Grapple" },
			{ "KeyAutoSelect", "SmartSelect" },
			{ "KeySmartCursor", "SmartCursor" },
			{ "KeyMount", "QuickMount" },
			{ "KeyMapStyle", "MapStyle" },
			{ "KeyFullscreenMap", "MapFull" },
			{ "KeyMapZoomIn", "MapZoomIn" },
			{ "KeyMapZoomOut", "MapZoomOut" },
			{ "KeyMapAlphaUp", "MapAlphaUp" },
			{ "KeyMapAlphaDown", "MapAlphaDown" }
		};
		for (int i = 0; i < array.GetLength(0); i++)
		{
			string currentValue = null;
			Main.Configuration.Get(array[i, 0], ref currentValue);
			if (currentValue != null)
			{
				playerInputProfile.InputModes[InputMode.Keyboard].KeyStatus[array[i, 1]] = new List<string> { currentValue };
				playerInputProfile.InputModes[InputMode.KeyboardUI].KeyStatus[array[i, 1]] = new List<string> { currentValue };
			}
		}
	}

	public static void LockGamepadButtons(string TriggerName)
	{
		List<string> value = null;
		KeyConfiguration value2 = null;
		if (CurrentProfile.InputModes.TryGetValue(CurrentInputMode, out value2) && value2.KeyStatus.TryGetValue(TriggerName, out value))
		{
			_buttonsLocked.AddRange(value);
		}
	}

	public static bool IsGamepadButtonLockedFromUse(string keyName)
	{
		return _buttonsLocked.Contains(keyName);
	}

	public static void UpdateInput()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		if (reinitialize)
		{
			ReInitialize();
		}
		SettingsForUI.UpdateCounters();
		Triggers.Reset();
		ScrollWheelValueOld = ScrollWheelValue;
		ScrollWheelValue = 0;
		GamepadThumbstickLeft = Vector2.Zero;
		GamepadThumbstickRight = Vector2.Zero;
		GrappleAndInteractAreShared = (UsingGamepad || SteamDeckIsUsed) && CurrentProfile.InputModes[InputMode.XBoxGamepad].DoGrappleAndInteractShareTheSameKey;
		if (InBuildingMode && !UsingGamepad)
		{
			ExitBuildingMode();
		}
		if (_canReleaseRebindingLock && NavigatorRebindingLock > 0)
		{
			NavigatorRebindingLock--;
			Triggers.Current.UsedMovementKey = false;
			if (NavigatorRebindingLock == 0 && _memoOfLastPoint != -1)
			{
				UIManageControls.ForceMoveTo = _memoOfLastPoint;
				_memoOfLastPoint = -1;
			}
		}
		_canReleaseRebindingLock = true;
		VerifyBuildingMode();
		MouseInput();
		int num = 0 | (KeyboardInput() ? 1 : 0) | (GamePadInput() ? 1 : 0);
		Triggers.Update();
		PostInput();
		ScrollWheelDelta = ScrollWheelValue - ScrollWheelValueOld;
		ScrollWheelDeltaForUI = ScrollWheelDelta;
		WritingText = false;
		UpdateMainMouse();
		Main.mouseLeft = Triggers.Current.MouseLeft;
		Main.mouseRight = Triggers.Current.MouseRight;
		Main.mouseMiddle = Triggers.Current.MouseMiddle;
		Main.mouseXButton1 = Triggers.Current.MouseXButton1;
		Main.mouseXButton2 = Triggers.Current.MouseXButton2;
		CacheZoomableValues();
		if (num != 0 && PlayerInput.OnActionableInput != null)
		{
			PlayerInput.OnActionableInput();
		}
	}

	public static void UpdateMainMouse()
	{
		Main.lastMouseX = Main.mouseX;
		Main.lastMouseY = Main.mouseY;
		Main.mouseX = MouseX;
		Main.mouseY = MouseY;
	}

	public static void CacheZoomableValues()
	{
		CacheOriginalInput();
		CacheOriginalScreenDimensions();
	}

	public static void CacheMousePositionForZoom()
	{
		float num = 1f;
		_originalMouseX = (int)((float)Main.mouseX * num);
		_originalMouseY = (int)((float)Main.mouseY * num);
	}

	private static void CacheOriginalInput()
	{
		_originalMouseX = Main.mouseX;
		_originalMouseY = Main.mouseY;
		_originalLastMouseX = Main.lastMouseX;
		_originalLastMouseY = Main.lastMouseY;
	}

	public static void CacheOriginalScreenDimensions()
	{
		_originalScreenWidth = Main.screenWidth;
		_originalScreenHeight = Main.screenHeight;
	}

	private static bool GamePadInput()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_049d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_0565: Unknown result type (might be due to invalid IL or missing references)
		//IL_056a: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05de: Unknown result type (might be due to invalid IL or missing references)
		//IL_057a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0524: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0644: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0598: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_069e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0876: Unknown result type (might be due to invalid IL or missing references)
		//IL_087b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0880: Unknown result type (might be due to invalid IL or missing references)
		//IL_088a: Unknown result type (might be due to invalid IL or missing references)
		//IL_088f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Unknown result type (might be due to invalid IL or missing references)
		//IL_0920: Unknown result type (might be due to invalid IL or missing references)
		//IL_095a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0964: Unknown result type (might be due to invalid IL or missing references)
		//IL_0966: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bcf: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		ScrollWheelValue += GamepadScrollValue;
		GamePadState gamePadState = default(GamePadState);
		bool flag2 = false;
		for (int i = 0; i < 4; i++)
		{
			GamePadState state = GamePad.GetState((PlayerIndex)i);
			if (((GamePadState)(ref state)).IsConnected)
			{
				flag2 = true;
				gamePadState = state;
				break;
			}
		}
		if (Main.SettingBlockGamepadsEntirely)
		{
			return false;
		}
		if (!flag2)
		{
			return false;
		}
		if (!((Game)Main.instance).IsActive && !Main.AllowUnfocusedInputOnGamepad)
		{
			return false;
		}
		Player player = Main.player[Main.myPlayer];
		bool flag3 = UILinkPointNavigator.Available && !InBuildingMode;
		InputMode inputMode = InputMode.XBoxGamepad;
		if (Main.gameMenu || flag3 || player.talkNPC != -1 || player.sign != -1 || IngameFancyUI.CanCover())
		{
			inputMode = InputMode.XBoxGamepadUI;
		}
		if (!Main.gameMenu && InBuildingMode)
		{
			inputMode = InputMode.XBoxGamepad;
		}
		if (CurrentInputMode == InputMode.XBoxGamepad && inputMode == InputMode.XBoxGamepadUI)
		{
			flag = true;
		}
		if (CurrentInputMode == InputMode.XBoxGamepadUI && inputMode == InputMode.XBoxGamepad)
		{
			flag = true;
		}
		if (flag)
		{
			CurrentInputMode = inputMode;
		}
		KeyConfiguration keyConfiguration = CurrentProfile.InputModes[inputMode];
		int num = 2145386496;
		for (int j = 0; j < ButtonsGamepad.Length; j++)
		{
			if ((int)((uint)num & (uint)ButtonsGamepad[j]) > 0)
			{
				continue;
			}
			string text = ((object)(Buttons)(ref ButtonsGamepad[j])).ToString();
			bool flag4 = _buttonsLocked.Contains(text);
			if (((GamePadState)(ref gamePadState)).IsButtonDown(ButtonsGamepad[j]))
			{
				if (!flag4)
				{
					if (CheckRebindingProcessGamepad(text))
					{
						return false;
					}
					keyConfiguration.Processkey(Triggers.Current, text, inputMode);
					flag = true;
				}
			}
			else
			{
				_buttonsLocked.Remove(text);
			}
		}
		GamePadThumbSticks thumbSticks = ((GamePadState)(ref gamePadState)).ThumbSticks;
		GamepadThumbstickLeft = ((GamePadThumbSticks)(ref thumbSticks)).Left * new Vector2(1f, -1f) * new Vector2((float)(CurrentProfile.LeftThumbstickInvertX.ToDirectionInt() * -1), (float)(CurrentProfile.LeftThumbstickInvertY.ToDirectionInt() * -1));
		thumbSticks = ((GamePadState)(ref gamePadState)).ThumbSticks;
		GamepadThumbstickRight = ((GamePadThumbSticks)(ref thumbSticks)).Right * new Vector2(1f, -1f) * new Vector2((float)(CurrentProfile.RightThumbstickInvertX.ToDirectionInt() * -1), (float)(CurrentProfile.RightThumbstickInvertY.ToDirectionInt() * -1));
		Vector2 gamepadThumbstickRight = GamepadThumbstickRight;
		Vector2 gamepadThumbstickLeft = GamepadThumbstickLeft;
		Vector2 vector = gamepadThumbstickRight;
		if (vector != Vector2.Zero)
		{
			((Vector2)(ref vector)).Normalize();
		}
		Vector2 vector2 = gamepadThumbstickLeft;
		if (vector2 != Vector2.Zero)
		{
			((Vector2)(ref vector2)).Normalize();
		}
		float num10 = 0.6f;
		float triggersDeadzone = CurrentProfile.TriggersDeadzone;
		if (inputMode == InputMode.XBoxGamepadUI)
		{
			num10 = 0.4f;
			if (GamepadAllowScrolling)
			{
				GamepadScrollValue -= (int)(gamepadThumbstickRight.Y * 16f);
			}
			GamepadAllowScrolling = false;
		}
		Buttons val;
		if (Vector2.Dot(-Vector2.UnitX, vector2) >= num10 && gamepadThumbstickLeft.X < 0f - CurrentProfile.LeftThumbstickDeadzoneX)
		{
			val = (Buttons)2097152;
			if (CheckRebindingProcessGamepad(((object)(Buttons)(ref val)).ToString()))
			{
				return false;
			}
			TriggersSet current = Triggers.Current;
			val = (Buttons)2097152;
			keyConfiguration.Processkey(current, ((object)(Buttons)(ref val)).ToString(), inputMode);
			flag = true;
		}
		if (Vector2.Dot(Vector2.UnitX, vector2) >= num10 && gamepadThumbstickLeft.X > CurrentProfile.LeftThumbstickDeadzoneX)
		{
			val = (Buttons)1073741824;
			if (CheckRebindingProcessGamepad(((object)(Buttons)(ref val)).ToString()))
			{
				return false;
			}
			TriggersSet current2 = Triggers.Current;
			val = (Buttons)1073741824;
			keyConfiguration.Processkey(current2, ((object)(Buttons)(ref val)).ToString(), inputMode);
			flag = true;
		}
		if (Vector2.Dot(-Vector2.UnitY, vector2) >= num10 && gamepadThumbstickLeft.Y < 0f - CurrentProfile.LeftThumbstickDeadzoneY)
		{
			val = (Buttons)268435456;
			if (CheckRebindingProcessGamepad(((object)(Buttons)(ref val)).ToString()))
			{
				return false;
			}
			TriggersSet current3 = Triggers.Current;
			val = (Buttons)268435456;
			keyConfiguration.Processkey(current3, ((object)(Buttons)(ref val)).ToString(), inputMode);
			flag = true;
		}
		if (Vector2.Dot(Vector2.UnitY, vector2) >= num10 && gamepadThumbstickLeft.Y > CurrentProfile.LeftThumbstickDeadzoneY)
		{
			val = (Buttons)536870912;
			if (CheckRebindingProcessGamepad(((object)(Buttons)(ref val)).ToString()))
			{
				return false;
			}
			TriggersSet current4 = Triggers.Current;
			val = (Buttons)536870912;
			keyConfiguration.Processkey(current4, ((object)(Buttons)(ref val)).ToString(), inputMode);
			flag = true;
		}
		if (Vector2.Dot(-Vector2.UnitX, vector) >= num10 && gamepadThumbstickRight.X < 0f - CurrentProfile.RightThumbstickDeadzoneX)
		{
			val = (Buttons)134217728;
			if (CheckRebindingProcessGamepad(((object)(Buttons)(ref val)).ToString()))
			{
				return false;
			}
			TriggersSet current5 = Triggers.Current;
			val = (Buttons)134217728;
			keyConfiguration.Processkey(current5, ((object)(Buttons)(ref val)).ToString(), inputMode);
			flag = true;
		}
		if (Vector2.Dot(Vector2.UnitX, vector) >= num10 && gamepadThumbstickRight.X > CurrentProfile.RightThumbstickDeadzoneX)
		{
			val = (Buttons)67108864;
			if (CheckRebindingProcessGamepad(((object)(Buttons)(ref val)).ToString()))
			{
				return false;
			}
			TriggersSet current6 = Triggers.Current;
			val = (Buttons)67108864;
			keyConfiguration.Processkey(current6, ((object)(Buttons)(ref val)).ToString(), inputMode);
			flag = true;
		}
		if (Vector2.Dot(-Vector2.UnitY, vector) >= num10 && gamepadThumbstickRight.Y < 0f - CurrentProfile.RightThumbstickDeadzoneY)
		{
			val = (Buttons)16777216;
			if (CheckRebindingProcessGamepad(((object)(Buttons)(ref val)).ToString()))
			{
				return false;
			}
			TriggersSet current7 = Triggers.Current;
			val = (Buttons)16777216;
			keyConfiguration.Processkey(current7, ((object)(Buttons)(ref val)).ToString(), inputMode);
			flag = true;
		}
		if (Vector2.Dot(Vector2.UnitY, vector) >= num10 && gamepadThumbstickRight.Y > CurrentProfile.RightThumbstickDeadzoneY)
		{
			val = (Buttons)33554432;
			if (CheckRebindingProcessGamepad(((object)(Buttons)(ref val)).ToString()))
			{
				return false;
			}
			TriggersSet current8 = Triggers.Current;
			val = (Buttons)33554432;
			keyConfiguration.Processkey(current8, ((object)(Buttons)(ref val)).ToString(), inputMode);
			flag = true;
		}
		GamePadTriggers triggers = ((GamePadState)(ref gamePadState)).Triggers;
		if (((GamePadTriggers)(ref triggers)).Left > triggersDeadzone)
		{
			val = (Buttons)8388608;
			if (CheckRebindingProcessGamepad(((object)(Buttons)(ref val)).ToString()))
			{
				return false;
			}
			TriggersSet current9 = Triggers.Current;
			val = (Buttons)8388608;
			keyConfiguration.Processkey(current9, ((object)(Buttons)(ref val)).ToString(), inputMode);
			flag = true;
		}
		triggers = ((GamePadState)(ref gamePadState)).Triggers;
		if (((GamePadTriggers)(ref triggers)).Right > triggersDeadzone)
		{
			val = (Buttons)4194304;
			string newKey = ((object)(Buttons)(ref val)).ToString();
			if (CheckRebindingProcessGamepad(newKey))
			{
				return false;
			}
			if (inputMode == InputMode.XBoxGamepadUI && SteamDeckIsUsed && SettingsForUI.CurrentCursorMode == CursorMode.Mouse)
			{
				Triggers.Current.MouseLeft = true;
			}
			else
			{
				keyConfiguration.Processkey(Triggers.Current, newKey, inputMode);
				flag = true;
			}
		}
		if (player.HeldItem.type >= ItemID.Sets.GamepadWholeScreenUseRange.Length)
		{
			return false;
		}
		bool flag5 = ItemID.Sets.GamepadWholeScreenUseRange[player.inventory[player.selectedItem].type] || player.scope;
		Item item = player.inventory[player.selectedItem];
		int num11 = item.tileBoost + ItemID.Sets.GamepadExtraRange[item.type];
		if (player.yoyoString && ItemID.Sets.Yoyo[item.type])
		{
			num11 += 5;
		}
		else if (item.createTile < 0 && item.createWall <= 0 && item.shoot > 0)
		{
			num11 += 10;
		}
		else if (player.controlTorch)
		{
			num11++;
		}
		if (item.createWall > 0 || item.createTile > 0 || item.tileWand > 0)
		{
			num11 += player.blockRange;
		}
		if (flag5)
		{
			num11 += 30;
		}
		if (player.mount.Active && player.mount.Type == 8)
		{
			num11 = 10;
		}
		bool flag6 = false;
		bool flag7 = !Main.gameMenu && !flag3 && Main.SmartCursorWanted;
		if (!CursorIsBusy)
		{
			bool flag8 = Main.mapFullscreen || (!Main.gameMenu && !flag3);
			int num12 = Main.screenWidth / 2;
			int num13 = Main.screenHeight / 2;
			if (!Main.mapFullscreen && flag8 && !flag5)
			{
				Point val2 = Main.ReverseGravitySupport(player.Center - Main.screenPosition).ToPoint();
				num12 = val2.X;
				num13 = val2.Y;
			}
			if (player.velocity == Vector2.Zero && gamepadThumbstickLeft == Vector2.Zero && gamepadThumbstickRight == Vector2.Zero && flag7)
			{
				num12 += player.direction * 10;
			}
			float k = Main.GameViewMatrix.ZoomMatrix.M11;
			smartSelectPointer.UpdateSize(new Vector2((float)(Player.tileRangeX * 16 + num11 * 16), (float)(Player.tileRangeY * 16 + num11 * 16)) * k);
			if (flag5)
			{
				smartSelectPointer.UpdateSize(new Vector2((float)(Math.Max(Main.screenWidth, Main.screenHeight) / 2)));
			}
			smartSelectPointer.UpdateCenter(new Vector2((float)num12, (float)num13));
			if (gamepadThumbstickRight != Vector2.Zero && flag8)
			{
				Vector2 vector3 = default(Vector2);
				((Vector2)(ref vector3))._002Ector(8f);
				if (!Main.gameMenu && Main.mapFullscreen)
				{
					((Vector2)(ref vector3))._002Ector(16f);
				}
				if (flag7)
				{
					((Vector2)(ref vector3))._002Ector((float)(Player.tileRangeX * 16), (float)(Player.tileRangeY * 16));
					if (num11 != 0)
					{
						vector3 += new Vector2((float)(num11 * 16), (float)(num11 * 16));
					}
					if (flag5)
					{
						((Vector2)(ref vector3))._002Ector((float)(Math.Max(Main.screenWidth, Main.screenHeight) / 2));
					}
				}
				else if (!Main.mapFullscreen)
				{
					vector3 = ((!player.inventory[player.selectedItem].mech) ? (vector3 + new Vector2((float)num11) / 4f) : (vector3 + Vector2.Zero));
				}
				float m2 = Main.GameViewMatrix.ZoomMatrix.M11;
				Vector2 vector4 = gamepadThumbstickRight * vector3 * m2;
				int num14 = MouseX - num12;
				int num15 = MouseY - num13;
				if (flag7)
				{
					num14 = 0;
					num15 = 0;
				}
				num14 += (int)vector4.X;
				num15 += (int)vector4.Y;
				MouseX = num14 + num12;
				MouseY = num15 + num13;
				flag = true;
				flag6 = true;
				SettingsForUI.SetCursorMode(CursorMode.Gamepad);
			}
			bool allowSecondaryGamepadAim = SettingsForUI.AllowSecondaryGamepadAim;
			if (gamepadThumbstickLeft != Vector2.Zero && flag8)
			{
				float num16 = 8f;
				if (!Main.gameMenu && Main.mapFullscreen)
				{
					num16 = 3f;
				}
				if (Main.mapFullscreen)
				{
					Vector2 vector5 = gamepadThumbstickLeft * num16;
					Main.mapFullscreenPos += vector5 * num16 * (1f / Main.mapFullscreenScale);
					flag = true;
				}
				else if (!flag6 && Main.SmartCursorWanted && allowSecondaryGamepadAim)
				{
					float m3 = Main.GameViewMatrix.ZoomMatrix.M11;
					Vector2 vector6 = gamepadThumbstickLeft * new Vector2((float)(Player.tileRangeX * 16), (float)(Player.tileRangeY * 16)) * m3;
					if (num11 != 0)
					{
						vector6 = gamepadThumbstickLeft * new Vector2((float)((Player.tileRangeX + num11) * 16), (float)((Player.tileRangeY + num11) * 16)) * m3;
					}
					if (flag5)
					{
						vector6 = new Vector2((float)(Math.Max(Main.screenWidth, Main.screenHeight) / 2)) * gamepadThumbstickLeft;
					}
					int num17 = (int)vector6.X;
					int num18 = (int)vector6.Y;
					MouseX = num17 + num12;
					MouseY = num18 + num13;
					flag6 = true;
				}
				flag = true;
			}
			if (CurrentInputMode == InputMode.XBoxGamepad)
			{
				HandleDpadSnap();
				if (SettingsForUI.AllowSecondaryGamepadAim)
				{
					int num2 = MouseX - num12;
					int num3 = MouseY - num13;
					if (!Main.gameMenu && !flag3)
					{
						if (flag5 && !Main.mapFullscreen)
						{
							float num4 = 1f;
							int num5 = Main.screenWidth / 2;
							int num6 = Main.screenHeight / 2;
							num2 = (int)Utils.Clamp(num2, (float)(-num5) * num4, (float)num5 * num4);
							num3 = (int)Utils.Clamp(num3, (float)(-num6) * num4, (float)num6 * num4);
						}
						else
						{
							float num7 = 0f;
							if (player.HeldItem.createTile >= 0 || player.HeldItem.createWall > 0 || player.HeldItem.tileWand >= 0)
							{
								num7 = 0.5f;
							}
							float m4 = Main.GameViewMatrix.ZoomMatrix.M11;
							float num8 = (0f - ((float)(Player.tileRangeY + num11) - num7)) * 16f * m4;
							float max = ((float)(Player.tileRangeY + num11) - num7) * 16f * m4;
							num8 -= (float)(player.height / 16 / 2 * 16);
							num2 = (int)Utils.Clamp(num2, (0f - ((float)(Player.tileRangeX + num11) - num7)) * 16f * m4, ((float)(Player.tileRangeX + num11) - num7) * 16f * m4);
							num3 = (int)Utils.Clamp(num3, num8, max);
						}
						if (flag7 && (!flag || flag5))
						{
							float num9 = 0.81f;
							if (flag5)
							{
								num9 = 0.95f;
							}
							num2 = (int)((float)num2 * num9);
							num3 = (int)((float)num3 * num9);
						}
					}
					else
					{
						num2 = Utils.Clamp(num2, -num12 + 10, num12 - 10);
						num3 = Utils.Clamp(num3, -num13 + 10, num13 - 10);
					}
					MouseX = num2 + num12;
					MouseY = num3 + num13;
				}
			}
		}
		if (flag)
		{
			CurrentInputMode = inputMode;
		}
		if (CurrentInputMode == InputMode.XBoxGamepad)
		{
			Main.SetCameraGamepadLerp(0.1f);
		}
		if (CurrentInputMode != InputMode.XBoxGamepadUI && flag)
		{
			PreventCursorModeSwappingToGamepad = true;
		}
		if (!flag)
		{
			PreventCursorModeSwappingToGamepad = false;
		}
		if (CurrentInputMode == InputMode.XBoxGamepadUI && flag && !PreventCursorModeSwappingToGamepad)
		{
			SettingsForUI.SetCursorMode(CursorMode.Gamepad);
		}
		return flag;
	}

	private static void MouseInput()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Invalid comparison between Unknown and I4
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Invalid comparison between Unknown and I4
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Invalid comparison between Unknown and I4
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Invalid comparison between Unknown and I4
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Invalid comparison between Unknown and I4
		bool flag = false;
		MouseInfoOld = MouseInfo;
		MouseInfo = Mouse.GetState();
		ScrollWheelValue += ((MouseState)(ref MouseInfo)).ScrollWheelValue;
		if (((MouseState)(ref MouseInfo)).X != ((MouseState)(ref MouseInfoOld)).X || ((MouseState)(ref MouseInfo)).Y != ((MouseState)(ref MouseInfoOld)).Y || ((MouseState)(ref MouseInfo)).ScrollWheelValue != ((MouseState)(ref MouseInfoOld)).ScrollWheelValue)
		{
			MouseX = (int)((float)((MouseState)(ref MouseInfo)).X * RawMouseScale.X);
			MouseY = (int)((float)((MouseState)(ref MouseInfo)).Y * RawMouseScale.Y);
			if (!PreventFirstMousePositionGrab)
			{
				flag = true;
				SettingsForUI.SetCursorMode(CursorMode.Mouse);
			}
			PreventFirstMousePositionGrab = false;
		}
		MouseKeys.Clear();
		if (((Game)Main.instance).IsActive)
		{
			if ((int)((MouseState)(ref MouseInfo)).LeftButton == 1)
			{
				MouseKeys.Add("Mouse1");
				flag = true;
			}
			if ((int)((MouseState)(ref MouseInfo)).RightButton == 1)
			{
				MouseKeys.Add("Mouse2");
				flag = true;
			}
			if ((int)((MouseState)(ref MouseInfo)).MiddleButton == 1)
			{
				MouseKeys.Add("Mouse3");
				flag = true;
			}
			if ((int)((MouseState)(ref MouseInfo)).XButton1 == 1)
			{
				MouseKeys.Add("Mouse4");
				flag = true;
			}
			if ((int)((MouseState)(ref MouseInfo)).XButton2 == 1)
			{
				MouseKeys.Add("Mouse5");
				flag = true;
			}
		}
		if (flag)
		{
			CurrentInputMode = InputMode.Mouse;
			Triggers.Current.UsedMovementKey = false;
		}
	}

	private static bool KeyboardInput()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Invalid comparison between Unknown and I4
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Invalid comparison between Unknown and I4
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Invalid comparison between Unknown and I4
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		bool flag2 = false;
		List<Keys> pressedKeys = GetPressedKeys();
		DebugKeys(pressedKeys);
		Keys val;
		if (pressedKeys.Count == 0 && MouseKeys.Count == 0)
		{
			val = (Keys)0;
			Main.blockKey = ((object)(Keys)(ref val)).ToString();
			return false;
		}
		for (int i = 0; i < pressedKeys.Count; i++)
		{
			if ((int)pressedKeys[i] == 160 || (int)pressedKeys[i] == 161)
			{
				flag = true;
			}
			else if ((int)pressedKeys[i] == 164 || (int)pressedKeys[i] == 165)
			{
				flag2 = true;
			}
			Main.ChromaPainter.PressKey(pressedKeys[i]);
		}
		string blockKey = Main.blockKey;
		val = (Keys)0;
		if (blockKey != ((object)(Keys)(ref val)).ToString())
		{
			bool flag3 = false;
			for (int j = 0; j < pressedKeys.Count; j++)
			{
				val = pressedKeys[j];
				if (((object)(Keys)(ref val)).ToString() == Main.blockKey)
				{
					pressedKeys[j] = (Keys)0;
					flag3 = true;
				}
			}
			if (!flag3)
			{
				val = (Keys)0;
				Main.blockKey = ((object)(Keys)(ref val)).ToString();
			}
		}
		KeyConfiguration keyConfiguration = CurrentProfile.InputModes[InputMode.Keyboard];
		if (Main.gameMenu && !WritingText)
		{
			keyConfiguration = CurrentProfile.InputModes[InputMode.KeyboardUI];
		}
		List<string> list = new List<string>(pressedKeys.Count);
		for (int k = 0; k < pressedKeys.Count; k++)
		{
			val = pressedKeys[k];
			list.Add(((object)(Keys)(ref val)).ToString());
		}
		if (WritingText)
		{
			list.Clear();
		}
		int count = list.Count;
		list.AddRange(MouseKeys);
		bool flag4 = false;
		for (int l = 0; l < list.Count; l++)
		{
			if (l < count && (int)pressedKeys[l] == 0)
			{
				continue;
			}
			string newKey = list[l];
			string text = list[l];
			val = (Keys)9;
			if (!(text == ((object)(Keys)(ref val)).ToString()) || !((flag && SocialAPI.Mode == SocialMode.Steam) || flag2))
			{
				if (CheckRebindingProcessKeyboard(newKey))
				{
					return false;
				}
				_ = Main.oldKeyState;
				if (l >= count || !((KeyboardState)(ref Main.oldKeyState)).IsKeyDown(pressedKeys[l]))
				{
					keyConfiguration.Processkey(Triggers.Current, newKey, InputMode.Keyboard);
				}
				else
				{
					keyConfiguration.CopyKeyState(Triggers.Old, Triggers.Current, newKey);
				}
				if (l >= count || (int)pressedKeys[l] != 0)
				{
					flag4 = true;
				}
			}
		}
		if (flag4)
		{
			CurrentInputMode = InputMode.Keyboard;
		}
		return flag4;
	}

	private static void DebugKeys(List<Keys> keys)
	{
	}

	private static void FixDerpedRebinds()
	{
		List<string> list = new List<string> { "MouseLeft", "MouseRight", "Inventory" };
		foreach (InputMode value in Enum.GetValues(typeof(InputMode)))
		{
			if (value == InputMode.Mouse)
			{
				continue;
			}
			FixKeysConflict(value, list);
			foreach (string item in list)
			{
				if (CurrentProfile.InputModes[value].KeyStatus[item].Count < 1)
				{
					ResetKeyBinding(value, item);
				}
			}
		}
	}

	private static void FixKeysConflict(InputMode inputMode, List<string> triggers)
	{
		for (int i = 0; i < triggers.Count; i++)
		{
			for (int j = i + 1; j < triggers.Count; j++)
			{
				List<string> list = CurrentProfile.InputModes[inputMode].KeyStatus[triggers[i]];
				List<string> list2 = CurrentProfile.InputModes[inputMode].KeyStatus[triggers[j]];
				foreach (string item in list.Intersect(list2).ToList())
				{
					list.Remove(item);
					list2.Remove(item);
				}
			}
		}
	}

	private static void ResetKeyBinding(InputMode inputMode, string trigger)
	{
		string key = "Redigit's Pick";
		if (OriginalProfiles.ContainsKey(_selectedProfile))
		{
			key = _selectedProfile;
		}
		CurrentProfile.InputModes[inputMode].KeyStatus[trigger].Clear();
		CurrentProfile.InputModes[inputMode].KeyStatus[trigger].AddRange(OriginalProfiles[key].InputModes[inputMode].KeyStatus[trigger]);
	}

	private static bool CheckRebindingProcessGamepad(string newKey)
	{
		_canReleaseRebindingLock = false;
		if (CurrentlyRebinding && _listeningInputMode == InputMode.XBoxGamepad)
		{
			NavigatorRebindingLock = 3;
			_memoOfLastPoint = UILinkPointNavigator.CurrentPoint;
			SoundEngine.PlaySound(12);
			if (CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus[ListeningTrigger].Contains(newKey))
			{
				CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus[ListeningTrigger].Remove(newKey);
			}
			else
			{
				CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus[ListeningTrigger] = new List<string> { newKey };
			}
			ListenFor(null, InputMode.XBoxGamepad);
		}
		if (CurrentlyRebinding && _listeningInputMode == InputMode.XBoxGamepadUI)
		{
			NavigatorRebindingLock = 3;
			_memoOfLastPoint = UILinkPointNavigator.CurrentPoint;
			SoundEngine.PlaySound(12);
			if (CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus[ListeningTrigger].Contains(newKey))
			{
				CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus[ListeningTrigger].Remove(newKey);
			}
			else
			{
				CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus[ListeningTrigger] = new List<string> { newKey };
			}
			ListenFor(null, InputMode.XBoxGamepadUI);
		}
		FixDerpedRebinds();
		if (PlayerInput.OnBindingChange != null)
		{
			PlayerInput.OnBindingChange();
		}
		return NavigatorRebindingLock > 0;
	}

	private static bool CheckRebindingProcessKeyboard(string newKey)
	{
		_canReleaseRebindingLock = false;
		if (CurrentlyRebinding && _listeningInputMode == InputMode.Keyboard)
		{
			NavigatorRebindingLock = 3;
			_memoOfLastPoint = UILinkPointNavigator.CurrentPoint;
			SoundEngine.PlaySound(12);
			if (CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus[ListeningTrigger].Contains(newKey))
			{
				CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus[ListeningTrigger].Remove(newKey);
			}
			else
			{
				CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus[ListeningTrigger] = new List<string> { newKey };
			}
			ListenFor(null, InputMode.Keyboard);
			Main.blockKey = newKey;
			Main.blockInput = false;
			Main.ChromaPainter.CollectBoundKeys();
		}
		if (CurrentlyRebinding && _listeningInputMode == InputMode.KeyboardUI)
		{
			NavigatorRebindingLock = 3;
			_memoOfLastPoint = UILinkPointNavigator.CurrentPoint;
			SoundEngine.PlaySound(12);
			if (CurrentProfile.InputModes[InputMode.KeyboardUI].KeyStatus[ListeningTrigger].Contains(newKey))
			{
				CurrentProfile.InputModes[InputMode.KeyboardUI].KeyStatus[ListeningTrigger].Remove(newKey);
			}
			else
			{
				CurrentProfile.InputModes[InputMode.KeyboardUI].KeyStatus[ListeningTrigger] = new List<string> { newKey };
			}
			ListenFor(null, InputMode.KeyboardUI);
			Main.blockKey = newKey;
			Main.blockInput = false;
			Main.ChromaPainter.CollectBoundKeys();
		}
		FixDerpedRebinds();
		if (PlayerInput.OnBindingChange != null)
		{
			PlayerInput.OnBindingChange();
		}
		return NavigatorRebindingLock > 0;
	}

	private static void PostInput()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		Main.GamepadCursorAlpha = MathHelper.Clamp(Main.GamepadCursorAlpha + ((Main.SmartCursorIsUsed && !UILinkPointNavigator.Available && GamepadThumbstickLeft == Vector2.Zero && GamepadThumbstickRight == Vector2.Zero) ? (-0.05f) : 0.05f), 0f, 1f);
		if (CurrentProfile.HotbarAllowsRadial)
		{
			int num = Triggers.Current.HotbarPlus.ToInt() - Triggers.Current.HotbarMinus.ToInt();
			if (MiscSettingsTEMP.HotbarRadialShouldBeUsed)
			{
				switch (num)
				{
				case 1:
					Triggers.Current.RadialHotbar = true;
					Triggers.JustReleased.RadialHotbar = false;
					break;
				case -1:
					Triggers.Current.RadialQuickbar = true;
					Triggers.JustReleased.RadialQuickbar = false;
					break;
				}
			}
		}
		MiscSettingsTEMP.HotbarRadialShouldBeUsed = false;
	}

	private static void HandleDpadSnap()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		Vector2 zero = Vector2.Zero;
		Player player = Main.player[Main.myPlayer];
		for (int i = 0; i < 4; i++)
		{
			bool flag = false;
			Vector2 vector = Vector2.Zero;
			if (Main.gameMenu || (UILinkPointNavigator.Available && !InBuildingMode))
			{
				return;
			}
			switch (i)
			{
			case 0:
				flag = Triggers.Current.DpadMouseSnap1;
				vector = -Vector2.UnitY;
				break;
			case 1:
				flag = Triggers.Current.DpadMouseSnap2;
				vector = Vector2.UnitX;
				break;
			case 2:
				flag = Triggers.Current.DpadMouseSnap3;
				vector = Vector2.UnitY;
				break;
			case 3:
				flag = Triggers.Current.DpadMouseSnap4;
				vector = -Vector2.UnitX;
				break;
			}
			if (DpadSnapCooldown[i] > 0)
			{
				DpadSnapCooldown[i]--;
			}
			if (flag)
			{
				if (DpadSnapCooldown[i] == 0)
				{
					int num = 6;
					if (ItemSlot.IsABuildingItem(player.inventory[player.selectedItem]))
					{
						num = CombinedHooks.TotalUseTime(player.inventory[player.selectedItem].useTime, player, player.inventory[player.selectedItem]);
					}
					DpadSnapCooldown[i] = num;
					zero += vector;
				}
			}
			else
			{
				DpadSnapCooldown[i] = 0;
			}
		}
		if (zero != Vector2.Zero)
		{
			Main.SmartCursorWanted_GamePad = false;
			Matrix zoomMatrix = Main.GameViewMatrix.ZoomMatrix;
			Matrix matrix = Matrix.Invert(zoomMatrix);
			Vector2 mouseScreen = Main.MouseScreen;
			Vector2.Transform(Main.screenPosition, matrix);
			Vector2 val = Vector2.Transform((Vector2.Transform(mouseScreen, matrix) + zero * new Vector2(16f) + Main.screenPosition).ToTileCoordinates().ToWorldCoordinates() - Main.screenPosition, zoomMatrix);
			MouseX = (int)val.X;
			MouseY = (int)val.Y;
			SettingsForUI.SetCursorMode(CursorMode.Gamepad);
		}
	}

	private static bool ShouldShowInstructionsForGamepad()
	{
		if (!UsingGamepad)
		{
			return SteamDeckIsUsed;
		}
		return true;
	}

	public static string ComposeInstructionsForGamepad()
	{
		string empty = string.Empty;
		InputMode inputMode = InputMode.XBoxGamepad;
		if (Main.gameMenu || UILinkPointNavigator.Available)
		{
			inputMode = InputMode.XBoxGamepadUI;
		}
		if (InBuildingMode && !Main.gameMenu)
		{
			inputMode = InputMode.XBoxGamepad;
		}
		KeyConfiguration keyConfiguration = CurrentProfile.InputModes[inputMode];
		if (Main.mapFullscreen && !Main.gameMenu)
		{
			empty += "          ";
			empty += BuildCommand(Lang.misc[56].Value, false, ProfileGamepadUI.KeyStatus["Inventory"]);
			empty += BuildCommand(Lang.inter[118].Value, false, ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			empty += BuildCommand(Lang.inter[119].Value, false, ProfileGamepadUI.KeyStatus["HotbarMinus"]);
			if (Main.netMode == 1 && Main.player[Main.myPlayer].HasItem(2997))
			{
				empty += BuildCommand(Lang.inter[120].Value, false, ProfileGamepadUI.KeyStatus["MouseRight"]);
			}
		}
		else if (inputMode == InputMode.XBoxGamepadUI && !InBuildingMode)
		{
			empty = UILinkPointNavigator.GetInstructions();
		}
		else
		{
			empty += BuildCommand(Lang.misc[58].Value, false, keyConfiguration.KeyStatus["Jump"]);
			empty += BuildCommand(Lang.misc[59].Value, false, keyConfiguration.KeyStatus["HotbarMinus"], keyConfiguration.KeyStatus["HotbarPlus"]);
			if (InBuildingMode)
			{
				empty += BuildCommand(Lang.menu[6].Value, false, keyConfiguration.KeyStatus["Inventory"], keyConfiguration.KeyStatus["MouseRight"]);
			}
			if (WiresUI.Open)
			{
				empty += BuildCommand(Lang.misc[53].Value, false, keyConfiguration.KeyStatus["MouseLeft"]);
				empty += BuildCommand(Lang.misc[56].Value, false, keyConfiguration.KeyStatus["MouseRight"]);
			}
			else
			{
				Item item = Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem];
				empty = ((item.damage > 0 && item.ammo == 0) ? (empty + BuildCommand(Lang.misc[60].Value, false, keyConfiguration.KeyStatus["MouseLeft"])) : ((item.createTile < 0 && item.createWall <= 0) ? (empty + BuildCommand(Lang.misc[63].Value, false, keyConfiguration.KeyStatus["MouseLeft"])) : (empty + BuildCommand(Lang.misc[61].Value, false, keyConfiguration.KeyStatus["MouseLeft"]))));
				bool flag = true;
				bool flag2 = Main.SmartInteractProj != -1 || Main.HasInteractibleObjectThatIsNotATile;
				bool flag3 = !Main.SmartInteractShowingGenuine && Main.SmartInteractShowingFake;
				if (Main.SmartInteractShowingGenuine || Main.SmartInteractShowingFake || flag2)
				{
					if (Main.SmartInteractNPC != -1)
					{
						if (flag3)
						{
							flag = false;
						}
						empty += BuildCommand(Lang.misc[80].Value, false, keyConfiguration.KeyStatus["MouseRight"]);
					}
					else if (flag2)
					{
						if (flag3)
						{
							flag = false;
						}
						empty += BuildCommand(Lang.misc[79].Value, false, keyConfiguration.KeyStatus["MouseRight"]);
					}
					else if (Main.SmartInteractX != -1 && Main.SmartInteractY != -1)
					{
						if (flag3)
						{
							flag = false;
						}
						Tile tile = Main.tile[Main.SmartInteractX, Main.SmartInteractY];
						empty = ((!TileID.Sets.TileInteractRead[tile.type]) ? (empty + BuildCommand(Lang.misc[79].Value, false, keyConfiguration.KeyStatus["MouseRight"])) : (empty + BuildCommand(Lang.misc[81].Value, false, keyConfiguration.KeyStatus["MouseRight"])));
					}
				}
				else if (WiresUI.Settings.DrawToolModeUI)
				{
					empty += BuildCommand(Lang.misc[89].Value, false, keyConfiguration.KeyStatus["MouseRight"]);
				}
				if ((!GrappleAndInteractAreShared || (!WiresUI.Settings.DrawToolModeUI && (!Main.SmartInteractShowingGenuine || !Main.HasSmartInteractTarget) && (!Main.SmartInteractShowingFake || flag))) && Main.LocalPlayer.QuickGrapple_GetItemToUse() != null)
				{
					empty += BuildCommand(Lang.misc[57].Value, false, keyConfiguration.KeyStatus["Grapple"]);
				}
			}
		}
		return empty;
	}

	public static string BuildCommand(string CommandText, bool Last, params List<string>[] Bindings)
	{
		string text = "";
		if (Bindings.Length == 0)
		{
			return text;
		}
		text += GenerateGlyphList(Bindings[0]);
		for (int i = 1; i < Bindings.Length; i++)
		{
			string text2 = GenerateGlyphList(Bindings[i]);
			if (text2.Length > 0)
			{
				text = text + "/" + text2;
			}
		}
		if (text.Length > 0)
		{
			text = text + ": " + CommandText;
			if (!Last)
			{
				text += "   ";
			}
		}
		return text;
	}

	public static string GenerateInputTag_ForCurrentGamemode_WithHacks(bool tagForGameplay, string triggerName)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		InputMode inputMode = CurrentInputMode;
		if (inputMode == InputMode.Mouse || inputMode == InputMode.KeyboardUI)
		{
			inputMode = InputMode.Keyboard;
		}
		Keys val;
		if (!(triggerName == "SmartSelect"))
		{
			if (triggerName == "SmartCursor" && inputMode == InputMode.Keyboard)
			{
				List<string> list = new List<string>();
				val = (Keys)164;
				list.Add(((object)(Keys)(ref val)).ToString());
				return GenerateRawInputList(list);
			}
		}
		else if (inputMode == InputMode.Keyboard)
		{
			List<string> list2 = new List<string>();
			val = (Keys)162;
			list2.Add(((object)(Keys)(ref val)).ToString());
			return GenerateRawInputList(list2);
		}
		return GenerateInputTag_ForCurrentGamemode(tagForGameplay, triggerName);
	}

	public static string GenerateInputTag_ForCurrentGamemode(bool tagForGameplay, string triggerName)
	{
		InputMode inputMode = CurrentInputMode;
		if (inputMode == InputMode.Mouse || inputMode == InputMode.KeyboardUI)
		{
			inputMode = InputMode.Keyboard;
		}
		if (tagForGameplay)
		{
			if ((uint)(inputMode - 3) > 1u)
			{
				return GenerateRawInputList(CurrentProfile.InputModes[inputMode].KeyStatus[triggerName]);
			}
			return GenerateGlyphList(CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus[triggerName]);
		}
		if ((uint)(inputMode - 3) > 1u)
		{
			return GenerateRawInputList(CurrentProfile.InputModes[inputMode].KeyStatus[triggerName]);
		}
		return GenerateGlyphList(CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus[triggerName]);
	}

	public static string GenerateInputTags_GamepadUI(string triggerName)
	{
		return GenerateGlyphList(CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus[triggerName]);
	}

	public static string GenerateInputTags_Gamepad(string triggerName)
	{
		return GenerateGlyphList(CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus[triggerName]);
	}

	private static string GenerateGlyphList(List<string> list)
	{
		if (list.Count == 0)
		{
			return "";
		}
		string text = GlyphTagHandler.GenerateTag(list[0]);
		for (int i = 1; i < list.Count; i++)
		{
			text = text + "/" + GlyphTagHandler.GenerateTag(list[i]);
		}
		return text;
	}

	private static string GenerateRawInputList(List<string> list)
	{
		if (list.Count == 0)
		{
			return "";
		}
		string text = list[0];
		for (int i = 1; i < list.Count; i++)
		{
			text = text + "/" + list[i];
		}
		return text;
	}

	public static void NavigatorCachePosition()
	{
		PreUIX = MouseX;
		PreUIY = MouseY;
	}

	public static void NavigatorUnCachePosition()
	{
		MouseX = PreUIX;
		MouseY = PreUIY;
	}

	public static void LockOnCachePosition()
	{
		PreLockOnX = MouseX;
		PreLockOnY = MouseY;
	}

	public static void LockOnUnCachePosition()
	{
		MouseX = PreLockOnX;
		MouseY = PreLockOnY;
	}

	public static void PrettyPrintProfiles(ref string text)
	{
		string[] array = text.Split(new string[1] { "\r\n" }, StringSplitOptions.None);
		foreach (string text2 in array)
		{
			if (text2.Contains(": {"))
			{
				string text3 = text2.Substring(0, text2.IndexOf('"'));
				string text4 = text2 + "\r\n  ";
				string newValue = text4.Replace(": {\r\n  ", ": \r\n" + text3 + "{\r\n  ");
				text = text.Replace(text4, newValue);
			}
		}
		text = text.Replace("[\r\n        ", "[");
		text = text.Replace("[\r\n      ", "[");
		text = text.Replace("\"\r\n      ", "\"");
		text = text.Replace("\",\r\n        ", "\", ");
		text = text.Replace("\",\r\n      ", "\", ");
		text = text.Replace("\r\n    ]", "]");
	}

	public static void PrettyPrintProfilesOld(ref string text)
	{
		text = text.Replace(": {\r\n  ", ": \r\n  {\r\n  ");
		text = text.Replace("[\r\n      ", "[");
		text = text.Replace("\"\r\n      ", "\"");
		text = text.Replace("\",\r\n      ", "\", ");
		text = text.Replace("\r\n    ]", "]");
	}

	public static void Reset(KeyConfiguration c, PresetProfiles style, InputMode mode)
	{
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d02: Unknown result type (might be due to invalid IL or missing references)
		//IL_288d: Unknown result type (might be due to invalid IL or missing references)
		OnReset(c, mode);
		Keys val;
		switch (style)
		{
		case PresetProfiles.Redigit:
			switch (mode)
			{
			case InputMode.Keyboard:
				c.KeyStatus["MouseLeft"].Add("Mouse1");
				c.KeyStatus["MouseRight"].Add("Mouse2");
				c.KeyStatus["Up"].Add("W");
				c.KeyStatus["Down"].Add("S");
				c.KeyStatus["Left"].Add("A");
				c.KeyStatus["Right"].Add("D");
				c.KeyStatus["Jump"].Add("Space");
				c.KeyStatus["Inventory"].Add("Escape");
				c.KeyStatus["Grapple"].Add("E");
				c.KeyStatus["SmartSelect"].Add("LeftShift");
				c.KeyStatus["SmartCursor"].Add("LeftControl");
				c.KeyStatus["QuickMount"].Add("R");
				c.KeyStatus["QuickHeal"].Add("H");
				c.KeyStatus["QuickMana"].Add("J");
				c.KeyStatus["QuickBuff"].Add("B");
				c.KeyStatus["MapStyle"].Add("Tab");
				c.KeyStatus["MapFull"].Add("M");
				c.KeyStatus["MapZoomIn"].Add("Add");
				c.KeyStatus["MapZoomOut"].Add("Subtract");
				c.KeyStatus["MapAlphaUp"].Add("PageUp");
				c.KeyStatus["MapAlphaDown"].Add("PageDown");
				c.KeyStatus["Hotbar1"].Add("D1");
				c.KeyStatus["Hotbar2"].Add("D2");
				c.KeyStatus["Hotbar3"].Add("D3");
				c.KeyStatus["Hotbar4"].Add("D4");
				c.KeyStatus["Hotbar5"].Add("D5");
				c.KeyStatus["Hotbar6"].Add("D6");
				c.KeyStatus["Hotbar7"].Add("D7");
				c.KeyStatus["Hotbar8"].Add("D8");
				c.KeyStatus["Hotbar9"].Add("D9");
				c.KeyStatus["Hotbar10"].Add("D0");
				c.KeyStatus["ViewZoomOut"].Add("OemMinus");
				c.KeyStatus["ViewZoomIn"].Add("OemPlus");
				c.KeyStatus["ToggleCreativeMenu"].Add("C");
				c.KeyStatus["Loadout1"].Add("F1");
				c.KeyStatus["Loadout2"].Add("F2");
				c.KeyStatus["Loadout3"].Add("F3");
				c.KeyStatus["ToggleCameraMode"].Add("F4");
				break;
			case InputMode.KeyboardUI:
			{
				c.KeyStatus["MouseLeft"].Add("Mouse1");
				c.KeyStatus["MouseLeft"].Add("Space");
				c.KeyStatus["MouseRight"].Add("Mouse2");
				c.KeyStatus["Up"].Add("W");
				c.KeyStatus["Up"].Add("Up");
				c.KeyStatus["Down"].Add("S");
				c.KeyStatus["Down"].Add("Down");
				c.KeyStatus["Left"].Add("A");
				c.KeyStatus["Left"].Add("Left");
				c.KeyStatus["Right"].Add("D");
				c.KeyStatus["Right"].Add("Right");
				List<string> list4 = c.KeyStatus["Inventory"];
				val = (Keys)27;
				list4.Add(((object)(Keys)(ref val)).ToString());
				c.KeyStatus["MenuUp"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["MenuDown"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["MenuLeft"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["MenuRight"].Add(string.Concat((object)(Buttons)8));
				break;
			}
			case InputMode.XBoxGamepad:
				c.KeyStatus["MouseLeft"].Add(string.Concat((object)(Buttons)4194304));
				c.KeyStatus["MouseRight"].Add(string.Concat((object)(Buttons)8192));
				c.KeyStatus["Up"].Add(string.Concat((object)(Buttons)268435456));
				c.KeyStatus["Down"].Add(string.Concat((object)(Buttons)536870912));
				c.KeyStatus["Left"].Add(string.Concat((object)(Buttons)2097152));
				c.KeyStatus["Right"].Add(string.Concat((object)(Buttons)1073741824));
				c.KeyStatus["Jump"].Add(string.Concat((object)(Buttons)8388608));
				c.KeyStatus["Inventory"].Add(string.Concat((object)(Buttons)32768));
				c.KeyStatus["Grapple"].Add(string.Concat((object)(Buttons)8192));
				c.KeyStatus["LockOn"].Add(string.Concat((object)(Buttons)16384));
				c.KeyStatus["QuickMount"].Add(string.Concat((object)(Buttons)4096));
				c.KeyStatus["SmartSelect"].Add(string.Concat((object)(Buttons)128));
				c.KeyStatus["SmartCursor"].Add(string.Concat((object)(Buttons)64));
				c.KeyStatus["HotbarMinus"].Add(string.Concat((object)(Buttons)256));
				c.KeyStatus["HotbarPlus"].Add(string.Concat((object)(Buttons)512));
				c.KeyStatus["MapFull"].Add(string.Concat((object)(Buttons)16));
				c.KeyStatus["DpadSnap1"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["DpadSnap3"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["DpadSnap4"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["DpadSnap2"].Add(string.Concat((object)(Buttons)8));
				c.KeyStatus["MapStyle"].Add(string.Concat((object)(Buttons)32));
				break;
			case InputMode.XBoxGamepadUI:
				c.KeyStatus["MouseLeft"].Add(string.Concat((object)(Buttons)4096));
				c.KeyStatus["MouseRight"].Add(string.Concat((object)(Buttons)256));
				c.KeyStatus["SmartCursor"].Add(string.Concat((object)(Buttons)512));
				c.KeyStatus["Up"].Add(string.Concat((object)(Buttons)268435456));
				c.KeyStatus["Down"].Add(string.Concat((object)(Buttons)536870912));
				c.KeyStatus["Left"].Add(string.Concat((object)(Buttons)2097152));
				c.KeyStatus["Right"].Add(string.Concat((object)(Buttons)1073741824));
				c.KeyStatus["Inventory"].Add(string.Concat((object)(Buttons)8192));
				c.KeyStatus["Inventory"].Add(string.Concat((object)(Buttons)32768));
				c.KeyStatus["HotbarMinus"].Add(string.Concat((object)(Buttons)8388608));
				c.KeyStatus["HotbarPlus"].Add(string.Concat((object)(Buttons)4194304));
				c.KeyStatus["Grapple"].Add(string.Concat((object)(Buttons)16384));
				c.KeyStatus["MapFull"].Add(string.Concat((object)(Buttons)16));
				c.KeyStatus["SmartSelect"].Add(string.Concat((object)(Buttons)32));
				c.KeyStatus["QuickMount"].Add(string.Concat((object)(Buttons)128));
				c.KeyStatus["DpadSnap1"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["DpadSnap3"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["DpadSnap4"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["DpadSnap2"].Add(string.Concat((object)(Buttons)8));
				c.KeyStatus["MenuUp"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["MenuDown"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["MenuLeft"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["MenuRight"].Add(string.Concat((object)(Buttons)8));
				break;
			case InputMode.Mouse:
				break;
			}
			break;
		case PresetProfiles.Yoraiz0r:
			switch (mode)
			{
			case InputMode.Keyboard:
				c.KeyStatus["MouseLeft"].Add("Mouse1");
				c.KeyStatus["MouseRight"].Add("Mouse2");
				c.KeyStatus["Up"].Add("W");
				c.KeyStatus["Down"].Add("S");
				c.KeyStatus["Left"].Add("A");
				c.KeyStatus["Right"].Add("D");
				c.KeyStatus["Jump"].Add("Space");
				c.KeyStatus["Inventory"].Add("Escape");
				c.KeyStatus["Grapple"].Add("E");
				c.KeyStatus["SmartSelect"].Add("LeftShift");
				c.KeyStatus["SmartCursor"].Add("LeftControl");
				c.KeyStatus["QuickMount"].Add("R");
				c.KeyStatus["QuickHeal"].Add("H");
				c.KeyStatus["QuickMana"].Add("J");
				c.KeyStatus["QuickBuff"].Add("B");
				c.KeyStatus["MapStyle"].Add("Tab");
				c.KeyStatus["MapFull"].Add("M");
				c.KeyStatus["MapZoomIn"].Add("Add");
				c.KeyStatus["MapZoomOut"].Add("Subtract");
				c.KeyStatus["MapAlphaUp"].Add("PageUp");
				c.KeyStatus["MapAlphaDown"].Add("PageDown");
				c.KeyStatus["Hotbar1"].Add("D1");
				c.KeyStatus["Hotbar2"].Add("D2");
				c.KeyStatus["Hotbar3"].Add("D3");
				c.KeyStatus["Hotbar4"].Add("D4");
				c.KeyStatus["Hotbar5"].Add("D5");
				c.KeyStatus["Hotbar6"].Add("D6");
				c.KeyStatus["Hotbar7"].Add("D7");
				c.KeyStatus["Hotbar8"].Add("D8");
				c.KeyStatus["Hotbar9"].Add("D9");
				c.KeyStatus["Hotbar10"].Add("D0");
				c.KeyStatus["ViewZoomOut"].Add("OemMinus");
				c.KeyStatus["ViewZoomIn"].Add("OemPlus");
				c.KeyStatus["ToggleCreativeMenu"].Add("C");
				c.KeyStatus["Loadout1"].Add("F1");
				c.KeyStatus["Loadout2"].Add("F2");
				c.KeyStatus["Loadout3"].Add("F3");
				c.KeyStatus["ToggleCameraMode"].Add("F4");
				break;
			case InputMode.KeyboardUI:
			{
				c.KeyStatus["MouseLeft"].Add("Mouse1");
				c.KeyStatus["MouseLeft"].Add("Space");
				c.KeyStatus["MouseRight"].Add("Mouse2");
				c.KeyStatus["Up"].Add("W");
				c.KeyStatus["Up"].Add("Up");
				c.KeyStatus["Down"].Add("S");
				c.KeyStatus["Down"].Add("Down");
				c.KeyStatus["Left"].Add("A");
				c.KeyStatus["Left"].Add("Left");
				c.KeyStatus["Right"].Add("D");
				c.KeyStatus["Right"].Add("Right");
				List<string> list3 = c.KeyStatus["Inventory"];
				val = (Keys)27;
				list3.Add(((object)(Keys)(ref val)).ToString());
				c.KeyStatus["MenuUp"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["MenuDown"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["MenuLeft"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["MenuRight"].Add(string.Concat((object)(Buttons)8));
				break;
			}
			case InputMode.XBoxGamepad:
				c.KeyStatus["MouseLeft"].Add(string.Concat((object)(Buttons)4194304));
				c.KeyStatus["MouseRight"].Add(string.Concat((object)(Buttons)8192));
				c.KeyStatus["Up"].Add(string.Concat((object)(Buttons)268435456));
				c.KeyStatus["Down"].Add(string.Concat((object)(Buttons)536870912));
				c.KeyStatus["Left"].Add(string.Concat((object)(Buttons)2097152));
				c.KeyStatus["Right"].Add(string.Concat((object)(Buttons)1073741824));
				c.KeyStatus["Jump"].Add(string.Concat((object)(Buttons)8388608));
				c.KeyStatus["Inventory"].Add(string.Concat((object)(Buttons)32768));
				c.KeyStatus["Grapple"].Add(string.Concat((object)(Buttons)256));
				c.KeyStatus["SmartSelect"].Add(string.Concat((object)(Buttons)64));
				c.KeyStatus["SmartCursor"].Add(string.Concat((object)(Buttons)128));
				c.KeyStatus["QuickMount"].Add(string.Concat((object)(Buttons)16384));
				c.KeyStatus["QuickHeal"].Add(string.Concat((object)(Buttons)4096));
				c.KeyStatus["RadialHotbar"].Add(string.Concat((object)(Buttons)512));
				c.KeyStatus["MapFull"].Add(string.Concat((object)(Buttons)16));
				c.KeyStatus["DpadSnap1"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["DpadSnap3"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["DpadSnap4"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["DpadSnap2"].Add(string.Concat((object)(Buttons)8));
				c.KeyStatus["MapStyle"].Add(string.Concat((object)(Buttons)32));
				break;
			case InputMode.XBoxGamepadUI:
				c.KeyStatus["MouseLeft"].Add(string.Concat((object)(Buttons)4096));
				c.KeyStatus["MouseRight"].Add(string.Concat((object)(Buttons)256));
				c.KeyStatus["SmartCursor"].Add(string.Concat((object)(Buttons)512));
				c.KeyStatus["Up"].Add(string.Concat((object)(Buttons)268435456));
				c.KeyStatus["Down"].Add(string.Concat((object)(Buttons)536870912));
				c.KeyStatus["Left"].Add(string.Concat((object)(Buttons)2097152));
				c.KeyStatus["Right"].Add(string.Concat((object)(Buttons)1073741824));
				c.KeyStatus["LockOn"].Add(string.Concat((object)(Buttons)8192));
				c.KeyStatus["Inventory"].Add(string.Concat((object)(Buttons)32768));
				c.KeyStatus["HotbarMinus"].Add(string.Concat((object)(Buttons)8388608));
				c.KeyStatus["HotbarPlus"].Add(string.Concat((object)(Buttons)4194304));
				c.KeyStatus["Grapple"].Add(string.Concat((object)(Buttons)16384));
				c.KeyStatus["MapFull"].Add(string.Concat((object)(Buttons)16));
				c.KeyStatus["SmartSelect"].Add(string.Concat((object)(Buttons)32));
				c.KeyStatus["QuickMount"].Add(string.Concat((object)(Buttons)128));
				c.KeyStatus["DpadSnap1"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["DpadSnap3"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["DpadSnap4"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["DpadSnap2"].Add(string.Concat((object)(Buttons)8));
				c.KeyStatus["MenuUp"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["MenuDown"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["MenuLeft"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["MenuRight"].Add(string.Concat((object)(Buttons)8));
				break;
			case InputMode.Mouse:
				break;
			}
			break;
		case PresetProfiles.ConsolePS:
			switch (mode)
			{
			case InputMode.Keyboard:
				c.KeyStatus["MouseLeft"].Add("Mouse1");
				c.KeyStatus["MouseRight"].Add("Mouse2");
				c.KeyStatus["Up"].Add("W");
				c.KeyStatus["Down"].Add("S");
				c.KeyStatus["Left"].Add("A");
				c.KeyStatus["Right"].Add("D");
				c.KeyStatus["Jump"].Add("Space");
				c.KeyStatus["Inventory"].Add("Escape");
				c.KeyStatus["Grapple"].Add("E");
				c.KeyStatus["SmartSelect"].Add("LeftShift");
				c.KeyStatus["SmartCursor"].Add("LeftControl");
				c.KeyStatus["QuickMount"].Add("R");
				c.KeyStatus["QuickHeal"].Add("H");
				c.KeyStatus["QuickMana"].Add("J");
				c.KeyStatus["QuickBuff"].Add("B");
				c.KeyStatus["MapStyle"].Add("Tab");
				c.KeyStatus["MapFull"].Add("M");
				c.KeyStatus["MapZoomIn"].Add("Add");
				c.KeyStatus["MapZoomOut"].Add("Subtract");
				c.KeyStatus["MapAlphaUp"].Add("PageUp");
				c.KeyStatus["MapAlphaDown"].Add("PageDown");
				c.KeyStatus["Hotbar1"].Add("D1");
				c.KeyStatus["Hotbar2"].Add("D2");
				c.KeyStatus["Hotbar3"].Add("D3");
				c.KeyStatus["Hotbar4"].Add("D4");
				c.KeyStatus["Hotbar5"].Add("D5");
				c.KeyStatus["Hotbar6"].Add("D6");
				c.KeyStatus["Hotbar7"].Add("D7");
				c.KeyStatus["Hotbar8"].Add("D8");
				c.KeyStatus["Hotbar9"].Add("D9");
				c.KeyStatus["Hotbar10"].Add("D0");
				c.KeyStatus["ViewZoomOut"].Add("OemMinus");
				c.KeyStatus["ViewZoomIn"].Add("OemPlus");
				c.KeyStatus["ToggleCreativeMenu"].Add("C");
				c.KeyStatus["Loadout1"].Add("F1");
				c.KeyStatus["Loadout2"].Add("F2");
				c.KeyStatus["Loadout3"].Add("F3");
				c.KeyStatus["ToggleCameraMode"].Add("F4");
				break;
			case InputMode.KeyboardUI:
			{
				c.KeyStatus["MouseLeft"].Add("Mouse1");
				c.KeyStatus["MouseLeft"].Add("Space");
				c.KeyStatus["MouseRight"].Add("Mouse2");
				c.KeyStatus["Up"].Add("W");
				c.KeyStatus["Up"].Add("Up");
				c.KeyStatus["Down"].Add("S");
				c.KeyStatus["Down"].Add("Down");
				c.KeyStatus["Left"].Add("A");
				c.KeyStatus["Left"].Add("Left");
				c.KeyStatus["Right"].Add("D");
				c.KeyStatus["Right"].Add("Right");
				c.KeyStatus["MenuUp"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["MenuDown"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["MenuLeft"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["MenuRight"].Add(string.Concat((object)(Buttons)8));
				List<string> list2 = c.KeyStatus["Inventory"];
				val = (Keys)27;
				list2.Add(((object)(Keys)(ref val)).ToString());
				break;
			}
			case InputMode.XBoxGamepad:
				c.KeyStatus["MouseLeft"].Add(string.Concat((object)(Buttons)512));
				c.KeyStatus["MouseRight"].Add(string.Concat((object)(Buttons)8192));
				c.KeyStatus["Up"].Add(string.Concat((object)(Buttons)268435456));
				c.KeyStatus["Down"].Add(string.Concat((object)(Buttons)536870912));
				c.KeyStatus["Left"].Add(string.Concat((object)(Buttons)2097152));
				c.KeyStatus["Right"].Add(string.Concat((object)(Buttons)1073741824));
				c.KeyStatus["Jump"].Add(string.Concat((object)(Buttons)4096));
				c.KeyStatus["LockOn"].Add(string.Concat((object)(Buttons)16384));
				c.KeyStatus["Inventory"].Add(string.Concat((object)(Buttons)32768));
				c.KeyStatus["Grapple"].Add(string.Concat((object)(Buttons)256));
				c.KeyStatus["SmartSelect"].Add(string.Concat((object)(Buttons)64));
				c.KeyStatus["SmartCursor"].Add(string.Concat((object)(Buttons)128));
				c.KeyStatus["HotbarMinus"].Add(string.Concat((object)(Buttons)8388608));
				c.KeyStatus["HotbarPlus"].Add(string.Concat((object)(Buttons)4194304));
				c.KeyStatus["MapFull"].Add(string.Concat((object)(Buttons)16));
				c.KeyStatus["DpadRadial1"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["DpadRadial3"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["DpadRadial4"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["DpadRadial2"].Add(string.Concat((object)(Buttons)8));
				c.KeyStatus["QuickMount"].Add(string.Concat((object)(Buttons)32));
				break;
			case InputMode.XBoxGamepadUI:
				c.KeyStatus["MouseLeft"].Add(string.Concat((object)(Buttons)4096));
				c.KeyStatus["MouseRight"].Add(string.Concat((object)(Buttons)256));
				c.KeyStatus["SmartCursor"].Add(string.Concat((object)(Buttons)512));
				c.KeyStatus["Up"].Add(string.Concat((object)(Buttons)268435456));
				c.KeyStatus["Down"].Add(string.Concat((object)(Buttons)536870912));
				c.KeyStatus["Left"].Add(string.Concat((object)(Buttons)2097152));
				c.KeyStatus["Right"].Add(string.Concat((object)(Buttons)1073741824));
				c.KeyStatus["Inventory"].Add(string.Concat((object)(Buttons)8192));
				c.KeyStatus["Inventory"].Add(string.Concat((object)(Buttons)32768));
				c.KeyStatus["HotbarMinus"].Add(string.Concat((object)(Buttons)8388608));
				c.KeyStatus["HotbarPlus"].Add(string.Concat((object)(Buttons)4194304));
				c.KeyStatus["Grapple"].Add(string.Concat((object)(Buttons)16384));
				c.KeyStatus["MapFull"].Add(string.Concat((object)(Buttons)16));
				c.KeyStatus["SmartSelect"].Add(string.Concat((object)(Buttons)32));
				c.KeyStatus["QuickMount"].Add(string.Concat((object)(Buttons)128));
				c.KeyStatus["DpadRadial1"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["DpadRadial3"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["DpadRadial4"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["DpadRadial2"].Add(string.Concat((object)(Buttons)8));
				c.KeyStatus["MenuUp"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["MenuDown"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["MenuLeft"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["MenuRight"].Add(string.Concat((object)(Buttons)8));
				break;
			case InputMode.Mouse:
				break;
			}
			break;
		case PresetProfiles.ConsoleXBox:
			switch (mode)
			{
			case InputMode.Keyboard:
				c.KeyStatus["MouseLeft"].Add("Mouse1");
				c.KeyStatus["MouseRight"].Add("Mouse2");
				c.KeyStatus["Up"].Add("W");
				c.KeyStatus["Down"].Add("S");
				c.KeyStatus["Left"].Add("A");
				c.KeyStatus["Right"].Add("D");
				c.KeyStatus["Jump"].Add("Space");
				c.KeyStatus["Inventory"].Add("Escape");
				c.KeyStatus["Grapple"].Add("E");
				c.KeyStatus["SmartSelect"].Add("LeftShift");
				c.KeyStatus["SmartCursor"].Add("LeftControl");
				c.KeyStatus["QuickMount"].Add("R");
				c.KeyStatus["QuickHeal"].Add("H");
				c.KeyStatus["QuickMana"].Add("J");
				c.KeyStatus["QuickBuff"].Add("B");
				c.KeyStatus["MapStyle"].Add("Tab");
				c.KeyStatus["MapFull"].Add("M");
				c.KeyStatus["MapZoomIn"].Add("Add");
				c.KeyStatus["MapZoomOut"].Add("Subtract");
				c.KeyStatus["MapAlphaUp"].Add("PageUp");
				c.KeyStatus["MapAlphaDown"].Add("PageDown");
				c.KeyStatus["Hotbar1"].Add("D1");
				c.KeyStatus["Hotbar2"].Add("D2");
				c.KeyStatus["Hotbar3"].Add("D3");
				c.KeyStatus["Hotbar4"].Add("D4");
				c.KeyStatus["Hotbar5"].Add("D5");
				c.KeyStatus["Hotbar6"].Add("D6");
				c.KeyStatus["Hotbar7"].Add("D7");
				c.KeyStatus["Hotbar8"].Add("D8");
				c.KeyStatus["Hotbar9"].Add("D9");
				c.KeyStatus["Hotbar10"].Add("D0");
				c.KeyStatus["ViewZoomOut"].Add("OemMinus");
				c.KeyStatus["ViewZoomIn"].Add("OemPlus");
				c.KeyStatus["ToggleCreativeMenu"].Add("C");
				c.KeyStatus["Loadout1"].Add("F1");
				c.KeyStatus["Loadout2"].Add("F2");
				c.KeyStatus["Loadout3"].Add("F3");
				c.KeyStatus["ToggleCameraMode"].Add("F4");
				break;
			case InputMode.KeyboardUI:
			{
				c.KeyStatus["MouseLeft"].Add("Mouse1");
				c.KeyStatus["MouseLeft"].Add("Space");
				c.KeyStatus["MouseRight"].Add("Mouse2");
				c.KeyStatus["Up"].Add("W");
				c.KeyStatus["Up"].Add("Up");
				c.KeyStatus["Down"].Add("S");
				c.KeyStatus["Down"].Add("Down");
				c.KeyStatus["Left"].Add("A");
				c.KeyStatus["Left"].Add("Left");
				c.KeyStatus["Right"].Add("D");
				c.KeyStatus["Right"].Add("Right");
				c.KeyStatus["MenuUp"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["MenuDown"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["MenuLeft"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["MenuRight"].Add(string.Concat((object)(Buttons)8));
				List<string> list = c.KeyStatus["Inventory"];
				val = (Keys)27;
				list.Add(((object)(Keys)(ref val)).ToString());
				break;
			}
			case InputMode.XBoxGamepad:
				c.KeyStatus["MouseLeft"].Add(string.Concat((object)(Buttons)4194304));
				c.KeyStatus["MouseRight"].Add(string.Concat((object)(Buttons)8192));
				c.KeyStatus["Up"].Add(string.Concat((object)(Buttons)268435456));
				c.KeyStatus["Down"].Add(string.Concat((object)(Buttons)536870912));
				c.KeyStatus["Left"].Add(string.Concat((object)(Buttons)2097152));
				c.KeyStatus["Right"].Add(string.Concat((object)(Buttons)1073741824));
				c.KeyStatus["Jump"].Add(string.Concat((object)(Buttons)4096));
				c.KeyStatus["LockOn"].Add(string.Concat((object)(Buttons)16384));
				c.KeyStatus["Inventory"].Add(string.Concat((object)(Buttons)32768));
				c.KeyStatus["Grapple"].Add(string.Concat((object)(Buttons)8388608));
				c.KeyStatus["SmartSelect"].Add(string.Concat((object)(Buttons)64));
				c.KeyStatus["SmartCursor"].Add(string.Concat((object)(Buttons)128));
				c.KeyStatus["HotbarMinus"].Add(string.Concat((object)(Buttons)256));
				c.KeyStatus["HotbarPlus"].Add(string.Concat((object)(Buttons)512));
				c.KeyStatus["MapFull"].Add(string.Concat((object)(Buttons)16));
				c.KeyStatus["DpadRadial1"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["DpadRadial3"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["DpadRadial4"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["DpadRadial2"].Add(string.Concat((object)(Buttons)8));
				c.KeyStatus["QuickMount"].Add(string.Concat((object)(Buttons)32));
				break;
			case InputMode.XBoxGamepadUI:
				c.KeyStatus["MouseLeft"].Add(string.Concat((object)(Buttons)4096));
				c.KeyStatus["MouseRight"].Add(string.Concat((object)(Buttons)256));
				c.KeyStatus["SmartCursor"].Add(string.Concat((object)(Buttons)512));
				c.KeyStatus["Up"].Add(string.Concat((object)(Buttons)268435456));
				c.KeyStatus["Down"].Add(string.Concat((object)(Buttons)536870912));
				c.KeyStatus["Left"].Add(string.Concat((object)(Buttons)2097152));
				c.KeyStatus["Right"].Add(string.Concat((object)(Buttons)1073741824));
				c.KeyStatus["Inventory"].Add(string.Concat((object)(Buttons)8192));
				c.KeyStatus["Inventory"].Add(string.Concat((object)(Buttons)32768));
				c.KeyStatus["HotbarMinus"].Add(string.Concat((object)(Buttons)8388608));
				c.KeyStatus["HotbarPlus"].Add(string.Concat((object)(Buttons)4194304));
				c.KeyStatus["Grapple"].Add(string.Concat((object)(Buttons)16384));
				c.KeyStatus["MapFull"].Add(string.Concat((object)(Buttons)16));
				c.KeyStatus["SmartSelect"].Add(string.Concat((object)(Buttons)32));
				c.KeyStatus["QuickMount"].Add(string.Concat((object)(Buttons)128));
				c.KeyStatus["DpadRadial1"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["DpadRadial3"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["DpadRadial4"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["DpadRadial2"].Add(string.Concat((object)(Buttons)8));
				c.KeyStatus["MenuUp"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["MenuDown"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["MenuLeft"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["MenuRight"].Add(string.Concat((object)(Buttons)8));
				break;
			case InputMode.Mouse:
				break;
			}
			break;
		}
	}

	public static void SetZoom_UI()
	{
		float uIScale = Main.UIScale;
		SetZoom_Scaled(1f / uIScale);
	}

	public static void SetZoom_World()
	{
		SetZoom_Scaled(1f);
		SetZoom_MouseInWorld();
	}

	public static void SetZoom_Unscaled()
	{
		Main.lastMouseX = _originalLastMouseX;
		Main.lastMouseY = _originalLastMouseY;
		Main.mouseX = _originalMouseX;
		Main.mouseY = _originalMouseY;
		Main.screenWidth = _originalScreenWidth;
		Main.screenHeight = _originalScreenHeight;
	}

	public static void SetZoom_Test()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = Main.screenPosition + new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / 2f;
		Vector2 vector3 = Main.screenPosition + new Vector2((float)_originalMouseX, (float)_originalMouseY);
		Vector2 vector4 = Main.screenPosition + new Vector2((float)_originalLastMouseX, (float)_originalLastMouseY);
		Vector2 vector5 = Main.screenPosition + new Vector2(0f, 0f);
		Vector2 val = Main.screenPosition + new Vector2((float)Main.screenWidth, (float)Main.screenHeight);
		Vector2 vector6 = vector3 - vector;
		Vector2 vector7 = vector4 - vector;
		Vector2 vector8 = vector5 - vector;
		_ = val - vector;
		float num = 1f / Main.GameViewMatrix.Zoom.X;
		float num2 = 1f;
		Vector2 val2 = vector - Main.screenPosition + vector6 * num;
		Vector2 vector2 = vector - Main.screenPosition + vector7 * num;
		Vector2 screenPosition = vector + vector8 * num2;
		Main.mouseX = (int)val2.X;
		Main.mouseY = (int)val2.Y;
		Main.lastMouseX = (int)vector2.X;
		Main.lastMouseY = (int)vector2.Y;
		Main.screenPosition = screenPosition;
		Main.screenWidth = (int)((float)_originalScreenWidth * num2);
		Main.screenHeight = (int)((float)_originalScreenHeight * num2);
	}

	public static void SetZoom_MouseInWorld()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = Main.screenPosition + new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / 2f;
		Vector2 vector2 = Main.screenPosition + new Vector2((float)_originalMouseX, (float)_originalMouseY);
		Vector2 val = Main.screenPosition + new Vector2((float)_originalLastMouseX, (float)_originalLastMouseY);
		Vector2 vector3 = vector2 - vector;
		Vector2 vector4 = val - vector;
		float num = 1f / Main.GameViewMatrix.Zoom.X;
		Vector2 val2 = vector - Main.screenPosition + vector3 * num;
		Main.mouseX = (int)val2.X;
		Main.mouseY = (int)val2.Y;
		Vector2 val3 = vector - Main.screenPosition + vector4 * num;
		Main.lastMouseX = (int)val3.X;
		Main.lastMouseY = (int)val3.Y;
	}

	public static void SetDesiredZoomContext(ZoomContext context)
	{
		_currentWantedZoom = context;
	}

	public static void SetZoom_Context()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		switch (_currentWantedZoom)
		{
		case ZoomContext.Unscaled:
			SetZoom_Unscaled();
			Main.SetRecommendedZoomContext(Matrix.Identity);
			break;
		case ZoomContext.Unscaled_MouseInWorld:
			SetZoom_Unscaled();
			SetZoom_MouseInWorld();
			Main.SetRecommendedZoomContext(Main.GameViewMatrix.ZoomMatrix);
			break;
		case ZoomContext.UI:
			SetZoom_UI();
			Main.SetRecommendedZoomContext(Main.UIScaleMatrix);
			break;
		case ZoomContext.World:
			SetZoom_World();
			Main.SetRecommendedZoomContext(Main.GameViewMatrix.ZoomMatrix);
			break;
		}
	}

	private static void SetZoom_Scaled(float scale)
	{
		Main.lastMouseX = (int)((float)_originalLastMouseX * scale);
		Main.lastMouseY = (int)((float)_originalLastMouseY * scale);
		Main.mouseX = (int)((float)_originalMouseX * scale);
		Main.mouseY = (int)((float)_originalMouseY * scale);
		Main.screenWidth = (int)((float)_originalScreenWidth * scale);
		Main.screenHeight = (int)((float)_originalScreenHeight * scale);
	}

	static PlayerInput()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		RawMouseScale = Vector2.One;
		Triggers = new TriggersPack();
		KnownTriggers = new List<string>
		{
			"MouseLeft", "MouseRight", "Up", "Down", "Left", "Right", "Jump", "Throw", "Inventory", "Grapple",
			"SmartSelect", "SmartCursor", "QuickMount", "QuickHeal", "QuickMana", "QuickBuff", "MapZoomIn", "MapZoomOut", "MapAlphaUp", "MapAlphaDown",
			"MapFull", "MapStyle", "Hotbar1", "Hotbar2", "Hotbar3", "Hotbar4", "Hotbar5", "Hotbar6", "Hotbar7", "Hotbar8",
			"Hotbar9", "Hotbar10", "HotbarMinus", "HotbarPlus", "DpadRadial1", "DpadRadial2", "DpadRadial3", "DpadRadial4", "RadialHotbar", "RadialQuickbar",
			"DpadSnap1", "DpadSnap2", "DpadSnap3", "DpadSnap4", "MenuUp", "MenuDown", "MenuLeft", "MenuRight", "LockOn", "ViewZoomIn",
			"ViewZoomOut", "Loadout1", "Loadout2", "Loadout3", "ToggleCameraMode", "ToggleCreativeMenu"
		};
		_canReleaseRebindingLock = true;
		_memoOfLastPoint = -1;
		BlockedKey = "";
		Profiles = new Dictionary<string, PlayerInputProfile>();
		OriginalProfiles = new Dictionary<string, PlayerInputProfile>();
		CurrentInputMode = InputMode.Keyboard;
		ButtonsGamepad = (Buttons[])Enum.GetValues(typeof(Buttons));
		smartSelectPointer = new SmartSelectGamepadPointer();
		_invalidatorCheck = "";
		LockGamepadTileUseButton = false;
		MouseKeys = new List<string>();
		GamepadThumbstickLeft = Vector2.Zero;
		GamepadThumbstickRight = Vector2.Zero;
		_UIPointForBuildingMode = -1;
		_buttonsLocked = new List<string>();
		PreventCursorModeSwappingToGamepad = false;
		PreventFirstMousePositionGrab = false;
		DpadSnapCooldown = new int[4];
		AllowExecutionOfGamepadInstructions = true;
		MouseInModdedUI = new List<string>();
		InsertExtraMouseButtonsIntoTriggerList(KnownTriggers);
	}

	/// <summary>
	/// Locks the vanilla scrollbar for the upcoming cycle when called. Autoclears in Player.
	/// Takes a string to denote that your UI has registered to lock vanilla scrolling. String does not need to be unique.
	/// </summary>
	public static void LockVanillaMouseScroll(string myUI)
	{
		if (!MouseInModdedUI.Contains(myUI))
		{
			MouseInModdedUI.Add(myUI);
		}
	}

	internal static void InsertExtraMouseButtonsIntoTriggerList(List<string> list)
	{
		int insertionIndex = list.FindLastIndex((string s) => s.Contains("Mouse")) + 1;
		list.InsertRange(insertionIndex, new string[3] { "MouseMiddle", "MouseXButton1", "MouseXButton2" });
	}

	private static void OnReset(KeyConfiguration c, InputMode mode)
	{
		if (mode == InputMode.Keyboard || mode == InputMode.KeyboardUI)
		{
			c.KeyStatus["MouseMiddle"].Add("Mouse3");
			c.KeyStatus["MouseXButton1"].Add("Mouse4");
			c.KeyStatus["MouseXButton2"].Add("Mouse5");
		}
	}
}
