using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using ReLogic.Peripherals.RGB;
using ReLogic.Peripherals.RGB.Corsair;
using ReLogic.Peripherals.RGB.Logitech;
using ReLogic.Peripherals.RGB.Razer;
using ReLogic.Peripherals.RGB.SteelSeries;
using SteelSeries.GameSense;
using SteelSeries.GameSense.DeviceZone;
using Terraria.GameContent.RGB;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.IO;

namespace Terraria.Initializers;

public static class ChromaInitializer
{
	public struct EventLocalization
	{
		public string DefaultDisplayName;

		public Dictionary<string, string> LocalizedNames;
	}

	private static ChromaEngine _engine;

	private const string GAME_NAME_ID = "TERRARIA";

	private static float _rgbUpdateRate;

	private static bool _useRazer;

	private static bool _useCorsair;

	private static bool _useLogitech;

	private static bool _useSteelSeries;

	private static VendorColorProfile _razerColorProfile;

	private static VendorColorProfile _corsairColorProfile;

	private static VendorColorProfile _logitechColorProfile;

	private static VendorColorProfile _steelSeriesColorProfile;

	private static Dictionary<string, EventLocalization> _localizedEvents = new Dictionary<string, EventLocalization>
	{
		{
			"KEY_MOUSELEFT",
			new EventLocalization
			{
				DefaultDisplayName = "Left Mouse Button"
			}
		},
		{
			"KEY_MOUSERIGHT",
			new EventLocalization
			{
				DefaultDisplayName = "Right Mouse Button"
			}
		},
		{
			"KEY_UP",
			new EventLocalization
			{
				DefaultDisplayName = "Up"
			}
		},
		{
			"KEY_DOWN",
			new EventLocalization
			{
				DefaultDisplayName = "Down"
			}
		},
		{
			"KEY_LEFT",
			new EventLocalization
			{
				DefaultDisplayName = "Left"
			}
		},
		{
			"KEY_RIGHT",
			new EventLocalization
			{
				DefaultDisplayName = "Right"
			}
		},
		{
			"KEY_JUMP",
			new EventLocalization
			{
				DefaultDisplayName = "Jump"
			}
		},
		{
			"KEY_THROW",
			new EventLocalization
			{
				DefaultDisplayName = "Throw"
			}
		},
		{
			"KEY_INVENTORY",
			new EventLocalization
			{
				DefaultDisplayName = "Inventory"
			}
		},
		{
			"KEY_GRAPPLE",
			new EventLocalization
			{
				DefaultDisplayName = "Grapple"
			}
		},
		{
			"KEY_SMARTSELECT",
			new EventLocalization
			{
				DefaultDisplayName = "Smart Select"
			}
		},
		{
			"KEY_SMARTCURSOR",
			new EventLocalization
			{
				DefaultDisplayName = "Smart Cursor"
			}
		},
		{
			"KEY_QUICKMOUNT",
			new EventLocalization
			{
				DefaultDisplayName = "Quick Mount"
			}
		},
		{
			"KEY_QUICKHEAL",
			new EventLocalization
			{
				DefaultDisplayName = "Quick Heal"
			}
		},
		{
			"KEY_QUICKMANA",
			new EventLocalization
			{
				DefaultDisplayName = "Quick Mana"
			}
		},
		{
			"KEY_QUICKBUFF",
			new EventLocalization
			{
				DefaultDisplayName = "Quick Buff"
			}
		},
		{
			"KEY_MAPZOOMIN",
			new EventLocalization
			{
				DefaultDisplayName = "Map Zoom In"
			}
		},
		{
			"KEY_MAPZOOMOUT",
			new EventLocalization
			{
				DefaultDisplayName = "Map Zoom Out"
			}
		},
		{
			"KEY_MAPALPHAUP",
			new EventLocalization
			{
				DefaultDisplayName = "Map Transparency Up"
			}
		},
		{
			"KEY_MAPALPHADOWN",
			new EventLocalization
			{
				DefaultDisplayName = "Map Transparency Down"
			}
		},
		{
			"KEY_MAPFULL",
			new EventLocalization
			{
				DefaultDisplayName = "Map Full"
			}
		},
		{
			"KEY_MAPSTYLE",
			new EventLocalization
			{
				DefaultDisplayName = "Map Style"
			}
		},
		{
			"KEY_HOTBAR1",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar 1"
			}
		},
		{
			"KEY_HOTBAR2",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar 2"
			}
		},
		{
			"KEY_HOTBAR3",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar 3"
			}
		},
		{
			"KEY_HOTBAR4",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar 4"
			}
		},
		{
			"KEY_HOTBAR5",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar 5"
			}
		},
		{
			"KEY_HOTBAR6",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar 6"
			}
		},
		{
			"KEY_HOTBAR7",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar 7"
			}
		},
		{
			"KEY_HOTBAR8",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar 8"
			}
		},
		{
			"KEY_HOTBAR9",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar 9"
			}
		},
		{
			"KEY_HOTBAR10",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar 10"
			}
		},
		{
			"KEY_HOTBARMINUS",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar Minus"
			}
		},
		{
			"KEY_HOTBARPLUS",
			new EventLocalization
			{
				DefaultDisplayName = "Hotbar Plus"
			}
		},
		{
			"KEY_DPADRADIAL1",
			new EventLocalization
			{
				DefaultDisplayName = "Dpad Radial 1"
			}
		},
		{
			"KEY_DPADRADIAL2",
			new EventLocalization
			{
				DefaultDisplayName = "Dpad Radial 2"
			}
		},
		{
			"KEY_DPADRADIAL3",
			new EventLocalization
			{
				DefaultDisplayName = "Dpad Radial 3"
			}
		},
		{
			"KEY_DPADRADIAL4",
			new EventLocalization
			{
				DefaultDisplayName = "Dpad Radial 4"
			}
		},
		{
			"KEY_RADIALHOTBAR",
			new EventLocalization
			{
				DefaultDisplayName = "Radial Hotbar"
			}
		},
		{
			"KEY_RADIALQUICKBAR",
			new EventLocalization
			{
				DefaultDisplayName = "Radial Quickbar"
			}
		},
		{
			"KEY_DPADSNAP1",
			new EventLocalization
			{
				DefaultDisplayName = "Dpad Snap 1"
			}
		},
		{
			"KEY_DPADSNAP2",
			new EventLocalization
			{
				DefaultDisplayName = "Dpad Snap 2"
			}
		},
		{
			"KEY_DPADSNAP3",
			new EventLocalization
			{
				DefaultDisplayName = "Dpad Snap 3"
			}
		},
		{
			"KEY_DPADSNAP4",
			new EventLocalization
			{
				DefaultDisplayName = "Dpad Snap 4"
			}
		},
		{
			"KEY_MENUUP",
			new EventLocalization
			{
				DefaultDisplayName = "Menu Up"
			}
		},
		{
			"KEY_MENUDOWN",
			new EventLocalization
			{
				DefaultDisplayName = "Menu Down"
			}
		},
		{
			"KEY_MENULEFT",
			new EventLocalization
			{
				DefaultDisplayName = "Menu Left"
			}
		},
		{
			"KEY_MENURIGHT",
			new EventLocalization
			{
				DefaultDisplayName = "Menu Right"
			}
		},
		{
			"KEY_LOCKON",
			new EventLocalization
			{
				DefaultDisplayName = "Lock On"
			}
		},
		{
			"KEY_VIEWZOOMIN",
			new EventLocalization
			{
				DefaultDisplayName = "Zoom In"
			}
		},
		{
			"KEY_VIEWZOOMOUT",
			new EventLocalization
			{
				DefaultDisplayName = "Zoom Out"
			}
		},
		{
			"KEY_TOGGLECREATIVEMENU",
			new EventLocalization
			{
				DefaultDisplayName = "Toggle Creative Menu"
			}
		},
		{
			"DO_RAINBOWS",
			new EventLocalization
			{
				DefaultDisplayName = "Theme"
			}
		},
		{
			"ZONE1",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 1"
			}
		},
		{
			"ZONE2",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 2"
			}
		},
		{
			"ZONE3",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 3"
			}
		},
		{
			"ZONE4",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 4"
			}
		},
		{
			"ZONE5",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 5"
			}
		},
		{
			"ZONE6",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 6"
			}
		},
		{
			"ZONE7",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 7"
			}
		},
		{
			"ZONE8",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 8"
			}
		},
		{
			"ZONE9",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 9"
			}
		},
		{
			"ZONE10",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 10"
			}
		},
		{
			"ZONE11",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 11"
			}
		},
		{
			"ZONE12",
			new EventLocalization
			{
				DefaultDisplayName = "Zone 12"
			}
		},
		{
			"LIFE",
			new EventLocalization
			{
				DefaultDisplayName = "Life Percent"
			}
		},
		{
			"MANA",
			new EventLocalization
			{
				DefaultDisplayName = "Mana Percent"
			}
		},
		{
			"BREATH",
			new EventLocalization
			{
				DefaultDisplayName = "Breath Percent"
			}
		}
	};

	public static IntRgbGameValueTracker Event_LifePercent;

	public static IntRgbGameValueTracker Event_ManaPercent;

	public static IntRgbGameValueTracker Event_BreathPercent;

	public static void BindTo(Preferences preferences)
	{
		preferences.OnSave += Configuration_OnSave;
		preferences.OnLoad += Configuration_OnLoad;
	}

	private static void Configuration_OnLoad(Preferences obj)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		_useRazer = obj.Get("UseRazerRGB", defaultValue: true);
		_useCorsair = obj.Get("UseCorsairRGB", defaultValue: true);
		_useLogitech = obj.Get("UseLogitechRGB", defaultValue: true);
		_useSteelSeries = obj.Get("UseSteelSeriesRGB", defaultValue: true);
		_razerColorProfile = obj.Get("RazerColorProfile", new VendorColorProfile(new Vector3(1f, 0.765f, 0.568f)));
		_corsairColorProfile = obj.Get("CorsairColorProfile", new VendorColorProfile());
		_logitechColorProfile = obj.Get("LogitechColorProfile", new VendorColorProfile());
		_steelSeriesColorProfile = obj.Get("SteelSeriesColorProfile", new VendorColorProfile());
		_rgbUpdateRate = obj.Get("RGBUpdatesPerSecond", 45f);
		if (_rgbUpdateRate <= 1E-07f)
		{
			_rgbUpdateRate = 45f;
		}
	}

	private static void Configuration_OnSave(Preferences preferences)
	{
		preferences.Put("RGBUpdatesPerSecond", _rgbUpdateRate);
		preferences.Put("UseRazerRGB", _useRazer);
		preferences.Put("RazerColorProfile", _razerColorProfile);
		preferences.Put("UseCorsairRGB", _useCorsair);
		preferences.Put("CorsairColorProfile", _corsairColorProfile);
		preferences.Put("UseLogitechRGB", _useLogitech);
		preferences.Put("LogitechColorProfile", _logitechColorProfile);
		preferences.Put("UseSteelSeriesRGB", _useSteelSeries);
		preferences.Put("SteelSeriesColorProfile", _steelSeriesColorProfile);
	}

	private static void AddDevices()
	{
		_engine.AddDeviceGroup("Razer", new RazerDeviceGroup(_razerColorProfile));
		_engine.AddDeviceGroup("Corsair", new CorsairDeviceGroup(_corsairColorProfile));
		_engine.AddDeviceGroup("Logitech", new LogitechDeviceGroup(_logitechColorProfile));
		_engine.AddDeviceGroup("SteelSeries", new SteelSeriesDeviceGroup(_steelSeriesColorProfile, "TERRARIA", "Terraria", (IconColor)3));
		_engine.FrameTimeInSeconds = 1f / _rgbUpdateRate;
		if (_useRazer)
		{
			_engine.EnableDeviceGroup("Razer");
		}
		if (_useCorsair)
		{
			_engine.EnableDeviceGroup("Corsair");
		}
		if (_useLogitech)
		{
			_engine.EnableDeviceGroup("Logitech");
		}
		if (_useSteelSeries)
		{
			_engine.EnableDeviceGroup("SteelSeries");
		}
		LoadSpecialRulesForDevices();
		AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
	}

	private static void LoadSpecialRulesForDevices()
	{
		Event_LifePercent = new IntRgbGameValueTracker
		{
			EventName = "LIFE"
		};
		Event_ManaPercent = new IntRgbGameValueTracker
		{
			EventName = "MANA"
		};
		Event_BreathPercent = new IntRgbGameValueTracker
		{
			EventName = "BREATH"
		};
		LoadSpecialRulesFor_GameSense();
	}

	public static void UpdateEvents()
	{
		if (Main.gameMenu)
		{
			Event_LifePercent.Update(0, isVisible: false);
			Event_ManaPercent.Update(0, isVisible: false);
			Event_BreathPercent.Update(0, isVisible: false);
			return;
		}
		Player localPlayer = Main.LocalPlayer;
		int value = (int)Utils.Clamp((float)localPlayer.statLife / (float)localPlayer.statLifeMax2 * 100f, 0f, 100f);
		Event_LifePercent.Update(value, isVisible: true);
		int value2 = (int)Utils.Clamp((float)localPlayer.statMana / (float)localPlayer.statManaMax2 * 100f, 0f, 100f);
		Event_ManaPercent.Update(value2, isVisible: true);
		int value3 = (int)Utils.Clamp((float)localPlayer.breath / (float)localPlayer.breathMax * 100f, 0f, 100f);
		Event_BreathPercent.Update(value3, isVisible: true);
	}

	private static void LoadSpecialRulesFor_GameSense()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Expected O, but got Unknown
		GameSenseSpecificInfo gameSenseSpecificInfo = new GameSenseSpecificInfo();
		List<Bind_Event> eventsToBind = (gameSenseSpecificInfo.EventsToBind = new List<Bind_Event>());
		LoadSpecialRulesFor_GameSense_Keyboard(eventsToBind);
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE1", "zone1", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("one"));
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE2", "zone2", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("two"));
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE3", "zone3", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("three"));
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE4", "zone4", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("four"));
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE5", "zone5", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("five"));
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE6", "zone6", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("six"));
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE7", "zone7", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("seven"));
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE8", "zone8", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("eight"));
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE9", "zone9", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("nine"));
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE10", "zone10", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("ten"));
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE11", "zone11", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("eleven"));
		LoadSpecialRulesFor_SecondaryDevice(eventsToBind, "ZONE12", "zone12", (AbstractIlluminationDevice_Zone)new RGBZonedDevice("twelve"));
		AddGameplayEvents(eventsToBind);
		gameSenseSpecificInfo.MiscEvents = new List<ARgbGameValueTracker> { Event_LifePercent, Event_ManaPercent, Event_BreathPercent };
		foreach (Bind_Event item in gameSenseSpecificInfo.EventsToBind)
		{
			if (_localizedEvents.TryGetValue(item.eventName, out var value))
			{
				item.defaultDisplayName = value.DefaultDisplayName;
				item.localizedDisplayNames = value.LocalizedNames;
			}
		}
		_engine.LoadSpecialRules(gameSenseSpecificInfo);
	}

	private static void AddGameplayEvents(List<Bind_Event> eventsToBind)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		eventsToBind.Add(new Bind_Event("TERRARIA", Event_LifePercent.EventName, 0, 100, (EventIconId)1, (AbstractHandler[])(object)new AbstractHandler[0]));
		eventsToBind.Add(new Bind_Event("TERRARIA", Event_ManaPercent.EventName, 0, 100, (EventIconId)14, (AbstractHandler[])(object)new AbstractHandler[0]));
		eventsToBind.Add(new Bind_Event("TERRARIA", Event_BreathPercent.EventName, 0, 100, (EventIconId)11, (AbstractHandler[])(object)new AbstractHandler[0]));
	}

	private static void LoadSpecialRulesFor_SecondaryDevice(List<Bind_Event> eventsToBind, string eventName, string contextFrameKey, AbstractIlluminationDevice_Zone zone)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		Bind_Event item = new Bind_Event("TERRARIA", eventName, 0, 10, (EventIconId)0, (AbstractHandler[])(object)new AbstractHandler[1] { (AbstractHandler)new ContextColorEventHandlerType
		{
			ContextFrameKey = contextFrameKey,
			DeviceZone = zone
		} });
		eventsToBind.Add(item);
	}

	private static void LoadSpecialRulesFor_GameSense_Keyboard(List<Bind_Event> eventsToBind)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Expected O, but got Unknown
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		Dictionary<string, byte> xnaKeyNamesToSteelSeriesKeyIndex = HIDCodes.XnaKeyNamesToSteelSeriesKeyIndex;
		Color white = Color.White;
		foreach (KeyValuePair<string, List<string>> item3 in PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus)
		{
			string key = item3.Key;
			List<string> value3 = item3.Value;
			List<byte> list = new List<byte>();
			foreach (string item4 in value3)
			{
				if (xnaKeyNamesToSteelSeriesKeyIndex.TryGetValue(item4, out var value2))
				{
					list.Add(value2);
				}
			}
			RGBPerkeyZoneCustom deviceZone = new RGBPerkeyZoneCustom(list.ToArray());
			new ColorStatic
			{
				red = ((Color)(ref white)).R,
				green = ((Color)(ref white)).G,
				blue = ((Color)(ref white)).B
			};
			Bind_Event item = new Bind_Event("TERRARIA", "KEY_" + key.ToUpper(), 0, 10, (EventIconId)0, (AbstractHandler[])(object)new AbstractHandler[1] { (AbstractHandler)new ContextColorEventHandlerType
			{
				ContextFrameKey = key,
				DeviceZone = (AbstractIlluminationDevice_Zone)(object)deviceZone
			} });
			eventsToBind.Add(item);
		}
		Bind_Event item2 = new Bind_Event("TERRARIA", "DO_RAINBOWS", 0, 10, (EventIconId)0, (AbstractHandler[])(object)new AbstractHandler[1] { (AbstractHandler)new PartialBitmapEventHandlerType
		{
			EventsToExclude = eventsToBind.Select((Bind_Event x) => x.eventName).ToArray()
		} });
		eventsToBind.Add(item2);
	}

	public static void DisableAllDeviceGroups()
	{
		if (_engine != null)
		{
			_engine.DisableAllDeviceGroups();
		}
	}

	private static void OnProcessExit(object sender, EventArgs e)
	{
		DisableAllDeviceGroups();
	}

	public static void Load()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0517: Unknown result type (might be due to invalid IL or missing references)
		//IL_0522: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_0664: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_0691: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0713: Unknown result type (might be due to invalid IL or missing references)
		//IL_0731: Unknown result type (might be due to invalid IL or missing references)
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_073b: Unknown result type (might be due to invalid IL or missing references)
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_076a: Unknown result type (might be due to invalid IL or missing references)
		//IL_077e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0818: Unknown result type (might be due to invalid IL or missing references)
		//IL_081d: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_084c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0866: Unknown result type (might be due to invalid IL or missing references)
		//IL_0874: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Unknown result type (might be due to invalid IL or missing references)
		//IL_089f: Unknown result type (might be due to invalid IL or missing references)
		//IL_08af: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0900: Unknown result type (might be due to invalid IL or missing references)
		//IL_090a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0924: Unknown result type (might be due to invalid IL or missing references)
		//IL_0929: Unknown result type (might be due to invalid IL or missing references)
		//IL_094c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0951: Unknown result type (might be due to invalid IL or missing references)
		//IL_0970: Unknown result type (might be due to invalid IL or missing references)
		//IL_097b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0986: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0caf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1b: Unknown result type (might be due to invalid IL or missing references)
		_engine = Main.Chroma;
		AddDevices();
		Color color = default(Color);
		((Color)(ref color))._002Ector(46, 23, 12);
		RegisterShader("Base", new SurfaceBiomeShader(Color.Green, color), CommonConditions.InMenu, ShaderLayer.Menu);
		RegisterShader("Surface Mushroom", new SurfaceBiomeShader(Color.DarkBlue, new Color(33, 31, 27)), CommonConditions.DrunkMenu, ShaderLayer.Menu);
		RegisterShader("Sky", new SkyShader(new Color(34, 51, 128), new Color(5, 5, 5)), CommonConditions.Depth.Sky, ShaderLayer.Background);
		RegisterShader("Surface", new SurfaceBiomeShader(Color.Green, color), CommonConditions.Depth.Surface, ShaderLayer.Background);
		RegisterShader("Vines", new VineShader(), CommonConditions.Depth.Vines, ShaderLayer.Background);
		RegisterShader("Underground", new CavernShader(new Color(122, 62, 32), new Color(25, 13, 7), 0.5f), CommonConditions.Depth.Underground, ShaderLayer.Background);
		RegisterShader("Caverns", new CavernShader(color, new Color(25, 25, 25), 0.5f), CommonConditions.Depth.Caverns, ShaderLayer.Background);
		RegisterShader("Magma", new CavernShader(new Color(181, 17, 0), new Color(25, 25, 25), 0.5f), CommonConditions.Depth.Magma, ShaderLayer.Background);
		RegisterShader("Underworld", new UnderworldShader(Color.Red, new Color(1f, 0.5f, 0f), 1f), CommonConditions.Depth.Underworld, ShaderLayer.Background);
		RegisterShader("Surface Desert", new SurfaceBiomeShader(new Color(84, 49, 0), new Color(245, 225, 33)), CommonConditions.SurfaceBiome.Desert, ShaderLayer.Biome);
		RegisterShader("Surface Jungle", new SurfaceBiomeShader(Color.Green, Color.Teal), CommonConditions.SurfaceBiome.Jungle, ShaderLayer.Biome);
		RegisterShader("Surface Ocean", new SurfaceBiomeShader(Color.SkyBlue, Color.Blue), CommonConditions.SurfaceBiome.Ocean, ShaderLayer.Biome);
		RegisterShader("Surface Snow", new SurfaceBiomeShader(new Color(0, 10, 50), new Color(0.5f, 0.75f, 1f)), CommonConditions.SurfaceBiome.Snow, ShaderLayer.Biome);
		RegisterShader("Surface Mushroom", new SurfaceBiomeShader(Color.DarkBlue, new Color(33, 31, 27)), CommonConditions.SurfaceBiome.Mushroom, ShaderLayer.Biome);
		RegisterShader("Surface Hallow", new HallowSurfaceShader(), CommonConditions.SurfaceBiome.Hallow, ShaderLayer.BiomeModifier);
		RegisterShader("Surface Crimson", new CorruptSurfaceShader(Color.Red, new Color(25, 25, 40)), CommonConditions.SurfaceBiome.Crimson, ShaderLayer.BiomeModifier);
		RegisterShader("Surface Corruption", new CorruptSurfaceShader(new Color(73, 0, 255), new Color(15, 15, 27)), CommonConditions.SurfaceBiome.Corruption, ShaderLayer.BiomeModifier);
		RegisterShader("Hive", new DrippingShader(new Color(0.05f, 0.01f, 0f), new Color(255, 150, 0), 0.5f), CommonConditions.UndergroundBiome.Hive, ShaderLayer.BiomeModifier);
		RegisterShader("Underground Mushroom", new UndergroundMushroomShader(), CommonConditions.UndergroundBiome.Mushroom, ShaderLayer.Biome);
		RegisterShader("Underground Corrutpion", new UndergroundCorruptionShader(), CommonConditions.UndergroundBiome.Corrupt, ShaderLayer.Biome);
		RegisterShader("Underground Crimson", new DrippingShader(new Color(0.05f, 0f, 0f), new Color(255, 0, 0)), CommonConditions.UndergroundBiome.Crimson, ShaderLayer.Biome);
		RegisterShader("Underground Hallow", new UndergroundHallowShader(), CommonConditions.UndergroundBiome.Hallow, ShaderLayer.Biome);
		RegisterShader("Meteorite", new MeteoriteShader(), CommonConditions.MiscBiome.Meteorite, ShaderLayer.BiomeModifier);
		RegisterShader("Temple", new TempleShader(), CommonConditions.UndergroundBiome.Temple, ShaderLayer.BiomeModifier);
		RegisterShader("Dungeon", new DungeonShader(), CommonConditions.UndergroundBiome.Dungeon, ShaderLayer.BiomeModifier);
		RegisterShader("Granite", new CavernShader(new Color(14, 19, 46), new Color(5, 0, 30), 0.5f), CommonConditions.UndergroundBiome.Granite, ShaderLayer.BiomeModifier);
		RegisterShader("Marble", new CavernShader(new Color(100, 100, 100), new Color(20, 20, 20), 0.5f), CommonConditions.UndergroundBiome.Marble, ShaderLayer.BiomeModifier);
		Color primaryColor = color;
		Color secondaryColor = new Color(25, 25, 25);
		Vector4[] array2 = new Vector4[7];
		Color val = Color.White;
		array2[0] = ((Color)(ref val)).ToVector4();
		val = Color.Yellow;
		array2[1] = ((Color)(ref val)).ToVector4();
		val = Color.Orange;
		array2[2] = ((Color)(ref val)).ToVector4();
		val = Color.Red;
		array2[3] = ((Color)(ref val)).ToVector4();
		val = Color.Green;
		array2[4] = ((Color)(ref val)).ToVector4();
		val = Color.Blue;
		array2[5] = ((Color)(ref val)).ToVector4();
		val = Color.Purple;
		array2[6] = ((Color)(ref val)).ToVector4();
		RegisterShader("Gem Cave", new GemCaveShader(primaryColor, secondaryColor, (Vector4[])(object)array2)
		{
			CycleTime = 100f,
			ColorRarity = 20f,
			TimeRate = 0.25f
		}, CommonConditions.UndergroundBiome.GemCave, ShaderLayer.BiomeModifier);
		Vector4[] array = (Vector4[])(object)new Vector4[12];
		for (int i = 0; i < array.Length; i++)
		{
			int num = i;
			val = Main.hslToRgb((float)i / (float)array.Length, 1f, 0.5f);
			array[num] = ((Color)(ref val)).ToVector4();
		}
		RegisterShader("Shimmer", new GemCaveShader(Color.Silver * 0.5f, new Color(125, 55, 125), array)
		{
			CycleTime = 2f,
			ColorRarity = 4f,
			TimeRate = 0.5f
		}, CommonConditions.UndergroundBiome.Shimmer, ShaderLayer.BiomeModifier);
		RegisterShader("Underground Jungle", new JungleShader(), CommonConditions.UndergroundBiome.Jungle, ShaderLayer.Biome);
		RegisterShader("Underground Ice", new IceShader(new Color(0, 10, 50), new Color(0.5f, 0.75f, 1f)), CommonConditions.UndergroundBiome.Ice, ShaderLayer.Biome);
		RegisterShader("Corrupt Ice", new IceShader(new Color(5, 0, 25), new Color(152, 102, 255)), CommonConditions.UndergroundBiome.CorruptIce, ShaderLayer.BiomeModifier);
		RegisterShader("Crimson Ice", new IceShader(new Color(0.1f, 0f, 0f), new Color(1f, 0.45f, 0.4f)), CommonConditions.UndergroundBiome.CrimsonIce, ShaderLayer.BiomeModifier);
		RegisterShader("Hallow Ice", new IceShader(new Color(0.2f, 0f, 0.1f), new Color(1f, 0.7f, 0.7f)), CommonConditions.UndergroundBiome.HallowIce, ShaderLayer.BiomeModifier);
		RegisterShader("Underground Desert", new DesertShader(new Color(60, 10, 0), new Color(255, 165, 0)), CommonConditions.UndergroundBiome.Desert, ShaderLayer.Biome);
		RegisterShader("Corrupt Desert", new DesertShader(new Color(15, 0, 15), new Color(116, 103, 255)), CommonConditions.UndergroundBiome.CorruptDesert, ShaderLayer.BiomeModifier);
		RegisterShader("Crimson Desert", new DesertShader(new Color(20, 10, 0), new Color(195, 0, 0)), CommonConditions.UndergroundBiome.CrimsonDesert, ShaderLayer.BiomeModifier);
		RegisterShader("Hallow Desert", new DesertShader(new Color(29, 0, 56), new Color(255, 221, 255)), CommonConditions.UndergroundBiome.HallowDesert, ShaderLayer.BiomeModifier);
		RegisterShader("Pumpkin Moon", new MoonShader(new Color(13, 0, 26), Color.Orange), CommonConditions.Events.PumpkinMoon, ShaderLayer.Event);
		RegisterShader("Blood Moon", new MoonShader(new Color(10, 0, 0), Color.Red, Color.Red, new Color(255, 150, 125)), CommonConditions.Events.BloodMoon, ShaderLayer.Event);
		RegisterShader("Frost Moon", new MoonShader(new Color(0, 4, 13), new Color(255, 255, 255)), CommonConditions.Events.FrostMoon, ShaderLayer.Event);
		RegisterShader("Solar Eclipse", new MoonShader(new Color(0.02f, 0.02f, 0.02f), Color.Orange, Color.Black), CommonConditions.Events.SolarEclipse, ShaderLayer.Event);
		RegisterShader("Pirate Invasion", new PirateInvasionShader(new Color(173, 173, 173), new Color(101, 101, 255), Color.Blue, Color.Black), CommonConditions.Events.PirateInvasion, ShaderLayer.Event);
		RegisterShader("DD2 Event", new DD2Shader(new Color(222, 94, 245), Color.White), CommonConditions.Events.DD2Event, ShaderLayer.Event);
		RegisterShader("Goblin Army", new GoblinArmyShader(new Color(14, 0, 79), new Color(176, 0, 144)), CommonConditions.Events.GoblinArmy, ShaderLayer.Event);
		RegisterShader("Frost Legion", new FrostLegionShader(Color.White, new Color(27, 80, 201)), CommonConditions.Events.FrostLegion, ShaderLayer.Event);
		RegisterShader("Martian Madness", new MartianMadnessShader(new Color(64, 64, 64), new Color(64, 113, 122), new Color(255, 255, 0), new Color(3, 3, 18)), CommonConditions.Events.MartianMadness, ShaderLayer.Event);
		RegisterShader("Solar Pillar", new PillarShader(Color.Red, Color.Orange), CommonConditions.Events.SolarPillar, ShaderLayer.Event);
		RegisterShader("Nebula Pillar", new PillarShader(new Color(255, 144, 209), new Color(100, 0, 76)), CommonConditions.Events.NebulaPillar, ShaderLayer.Event);
		RegisterShader("Vortex Pillar", new PillarShader(Color.Green, Color.Black), CommonConditions.Events.VortexPillar, ShaderLayer.Event);
		RegisterShader("Stardust Pillar", new PillarShader(new Color(46, 63, 255), Color.White), CommonConditions.Events.StardustPillar, ShaderLayer.Event);
		RegisterShader("Eater of Worlds", new WormShader(new Color(14, 0, 15), new Color(47, 51, 59), new Color(20, 25, 11)), CommonConditions.Boss.EaterOfWorlds, ShaderLayer.Boss);
		RegisterShader("Eye of Cthulhu", new EyeOfCthulhuShader(new Color(145, 145, 126), new Color(138, 0, 0), new Color(3, 3, 18)), CommonConditions.Boss.EyeOfCthulhu, ShaderLayer.Boss);
		RegisterShader("Skeletron", new SkullShader(new Color(110, 92, 47), new Color(36, 32, 51), new Color(0, 0, 0)), CommonConditions.Boss.Skeletron, ShaderLayer.Boss);
		RegisterShader("Brain Of Cthulhu", new BrainShader(new Color(54, 0, 0), new Color(186, 137, 139)), CommonConditions.Boss.BrainOfCthulhu, ShaderLayer.Boss);
		RegisterShader("Empress of Light", new EmpressShader(), CommonConditions.Boss.Empress, ShaderLayer.Boss);
		RegisterShader("Queen Slime", new QueenSlimeShader(new Color(72, 41, 130), new Color(126, 220, 255)), CommonConditions.Boss.QueenSlime, ShaderLayer.Boss);
		RegisterShader("King Slime", new KingSlimeShader(new Color(41, 70, 130), Color.White), CommonConditions.Boss.KingSlime, ShaderLayer.Boss);
		RegisterShader("Queen Bee", new QueenBeeShader(new Color(5, 5, 0), new Color(255, 235, 0)), CommonConditions.Boss.QueenBee, ShaderLayer.Boss);
		RegisterShader("Wall of Flesh", new WallOfFleshShader(new Color(112, 48, 60), new Color(5, 0, 0)), CommonConditions.Boss.WallOfFlesh, ShaderLayer.Boss);
		RegisterShader("Destroyer", new WormShader(new Color(25, 25, 25), new Color(192, 0, 0), new Color(10, 0, 0)), CommonConditions.Boss.Destroyer, ShaderLayer.Boss);
		RegisterShader("Skeletron Prime", new SkullShader(new Color(110, 92, 47), new Color(79, 0, 0), new Color(255, 29, 0)), CommonConditions.Boss.SkeletronPrime, ShaderLayer.Boss);
		RegisterShader("The Twins", new TwinsShader(new Color(145, 145, 126), new Color(138, 0, 0), new Color(138, 0, 0), new Color(20, 20, 20), new Color(65, 140, 0), new Color(3, 3, 18)), CommonConditions.Boss.TheTwins, ShaderLayer.Boss);
		RegisterShader("Duke Fishron", new DukeFishronShader(new Color(0, 0, 122), new Color(100, 254, 194)), CommonConditions.Boss.DukeFishron, ShaderLayer.Boss);
		RegisterShader("Deerclops", new BlizzardShader(new Vector4(1f, 1f, 1f, 1f), new Vector4(0.15f, 0.1f, 0.4f, 1f), -0.1f, 0.4f), CommonConditions.Boss.Deerclops, ShaderLayer.Boss);
		RegisterShader("Plantera", new PlanteraShader(new Color(255, 0, 220), new Color(0, 255, 0), new Color(12, 4, 0)), CommonConditions.Boss.Plantera, ShaderLayer.Boss);
		RegisterShader("Golem", new GolemShader(new Color(255, 144, 0), new Color(255, 198, 0), new Color(10, 10, 0)), CommonConditions.Boss.Golem, ShaderLayer.Boss);
		RegisterShader("Cultist", new CultistShader(), CommonConditions.Boss.Cultist, ShaderLayer.Boss);
		RegisterShader("Moon Lord", new EyeballShader(isSpawning: false), CommonConditions.Boss.MoonLord, ShaderLayer.Boss);
		RegisterShader("Rain", new RainShader(), CommonConditions.Weather.Rain, ShaderLayer.Weather);
		RegisterShader("Snowstorm", new BlizzardShader(new Vector4(1f, 1f, 1f, 1f), new Vector4(0.1f, 0.1f, 0.3f, 1f), 0.35f, -0.35f), CommonConditions.Weather.Blizzard, ShaderLayer.Weather);
		RegisterShader("Sandstorm", new SandstormShader(), CommonConditions.Weather.Sandstorm, ShaderLayer.Weather);
		RegisterShader("Slime Rain", new SlimeRainShader(), CommonConditions.Weather.SlimeRain, ShaderLayer.Weather);
		RegisterShader("Drowning", new DrowningShader(), CommonConditions.Alert.Drowning, ShaderLayer.Alert);
		RegisterShader("Keybinds", new KeybindsMenuShader(), CommonConditions.Alert.Keybinds, ShaderLayer.Alert);
		RegisterShader("Lava Indicator", new LavaIndicatorShader(Color.Black, Color.Red, new Color(255, 188, 0)), CommonConditions.Alert.LavaIndicator, ShaderLayer.Alert);
		RegisterShader("Moon Lord Spawn", new EyeballShader(isSpawning: true), CommonConditions.Alert.MoonlordComing, ShaderLayer.Alert);
		RegisterShader("Low Life", new LowLifeShader(), CommonConditions.CriticalAlert.LowLife, ShaderLayer.CriticalAlert);
		RegisterShader("Death", new DeathShader(new Color(36, 0, 10), new Color(158, 28, 53)), CommonConditions.CriticalAlert.Death, ShaderLayer.CriticalAlert);
	}

	private static void RegisterShader(string name, ChromaShader shader, ChromaCondition condition, ShaderLayer layer)
	{
		_engine.RegisterShader(shader, condition, layer);
	}

	[Conditional("DEBUG")]
	private static void AddDebugDraw()
	{
		new BasicDebugDrawer(((Game)Main.instance).GraphicsDevice);
		Filters.Scene.OnPostDraw += delegate
		{
		};
	}
}
