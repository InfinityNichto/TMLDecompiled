using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.Social;
using Terraria.Social.Steam;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria;

public static class IngameOptions
{
	public const int width = 670;

	public const int height = 480;

	public static float[] leftScale = new float[11]
	{
		0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f,
		0.7f
	};

	public static float[] rightScale = new float[17]
	{
		0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f,
		0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f
	};

	private static Dictionary<int, int> _leftSideCategoryMapping = new Dictionary<int, int>
	{
		{ 0, 0 },
		{ 1, 1 },
		{ 2, 2 },
		{ 3, 3 }
	};

	public static bool[] skipRightSlot = new bool[20];

	public static int leftHover = -1;

	public static int rightHover = -1;

	public static int oldLeftHover = -1;

	public static int oldRightHover = -1;

	public static int rightLock = -1;

	public static bool inBar;

	public static bool notBar;

	public static bool noSound;

	private static Rectangle _GUIHover;

	public static int category;

	public static Vector2 valuePosition = Vector2.Zero;

	private static string _mouseOverText;

	private static bool _canConsumeHover;

	public static void Open()
	{
		Main.ClosePlayerChat();
		Main.chatText = "";
		Main.playerInventory = false;
		Main.editChest = false;
		Main.npcChatText = "";
		SoundEngine.PlaySound(10);
		Main.ingameOptionsWindow = true;
		category = 0;
		for (int i = 0; i < leftScale.Length; i++)
		{
			leftScale[i] = 0f;
		}
		for (int j = 0; j < rightScale.Length; j++)
		{
			rightScale[j] = 0f;
		}
		leftHover = -1;
		rightHover = -1;
		oldLeftHover = -1;
		oldRightHover = -1;
		rightLock = -1;
		inBar = false;
		notBar = false;
		noSound = false;
	}

	public static void Close()
	{
		if (Main.setKey == -1)
		{
			Main.ingameOptionsWindow = false;
			SoundEngine.PlaySound(11);
			Recipe.FindRecipes();
			Main.playerInventory = true;
			Main.SaveSettings();
		}
	}

	public static void Draw(Main mainInstance, SpriteBatch sb)
	{
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06da: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Unknown result type (might be due to invalid IL or missing references)
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_077c: Unknown result type (might be due to invalid IL or missing references)
		//IL_077e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_084e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0850: Unknown result type (might be due to invalid IL or missing references)
		//IL_0898: Unknown result type (might be due to invalid IL or missing references)
		//IL_089a: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ada: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c85: Unknown result type (might be due to invalid IL or missing references)
		//IL_1911: Unknown result type (might be due to invalid IL or missing references)
		//IL_1913: Unknown result type (might be due to invalid IL or missing references)
		//IL_1930: Unknown result type (might be due to invalid IL or missing references)
		//IL_1936: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d04: Unknown result type (might be due to invalid IL or missing references)
		//IL_274e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2753: Unknown result type (might be due to invalid IL or missing references)
		//IL_276f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2771: Unknown result type (might be due to invalid IL or missing references)
		//IL_2782: Unknown result type (might be due to invalid IL or missing references)
		//IL_2788: Unknown result type (might be due to invalid IL or missing references)
		//IL_27b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_27b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_27d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_27d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ffa: Unknown result type (might be due to invalid IL or missing references)
		//IL_2000: Unknown result type (might be due to invalid IL or missing references)
		//IL_27fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2803: Unknown result type (might be due to invalid IL or missing references)
		//IL_283f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2841: Unknown result type (might be due to invalid IL or missing references)
		//IL_2849: Unknown result type (might be due to invalid IL or missing references)
		//IL_2067: Unknown result type (might be due to invalid IL or missing references)
		//IL_2069: Unknown result type (might be due to invalid IL or missing references)
		//IL_2086: Unknown result type (might be due to invalid IL or missing references)
		//IL_208c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dee: Unknown result type (might be due to invalid IL or missing references)
		//IL_369c: Unknown result type (might be due to invalid IL or missing references)
		//IL_369e: Unknown result type (might be due to invalid IL or missing references)
		//IL_36a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_36aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_36c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_36c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_216c: Unknown result type (might be due to invalid IL or missing references)
		//IL_216e: Unknown result type (might be due to invalid IL or missing references)
		//IL_218b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2191: Unknown result type (might be due to invalid IL or missing references)
		//IL_1991: Unknown result type (might be due to invalid IL or missing references)
		//IL_1993: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e18: Unknown result type (might be due to invalid IL or missing references)
		//IL_28a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_28ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_21ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_21c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_374c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2949: Unknown result type (might be due to invalid IL or missing references)
		//IL_294b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2968: Unknown result type (might be due to invalid IL or missing references)
		//IL_296e: Unknown result type (might be due to invalid IL or missing references)
		//IL_225e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2265: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a16: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a18: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a35: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_36ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_36f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_36f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_36f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_36ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_3704: Unknown result type (might be due to invalid IL or missing references)
		//IL_2991: Unknown result type (might be due to invalid IL or missing references)
		//IL_2998: Unknown result type (might be due to invalid IL or missing references)
		//IL_29d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_29d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_29de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f87: Unknown result type (might be due to invalid IL or missing references)
		//IL_2318: Unknown result type (might be due to invalid IL or missing references)
		//IL_231a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2337: Unknown result type (might be due to invalid IL or missing references)
		//IL_233d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0faa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a43: Unknown result type (might be due to invalid IL or missing references)
		//IL_2382: Unknown result type (might be due to invalid IL or missing references)
		//IL_2384: Unknown result type (might be due to invalid IL or missing references)
		//IL_23a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_23a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a88: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aad: Unknown result type (might be due to invalid IL or missing references)
		//IL_23dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_23de: Unknown result type (might be due to invalid IL or missing references)
		//IL_23fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2401: Unknown result type (might be due to invalid IL or missing references)
		//IL_102f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1036: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ade: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ae0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2afd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b03: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b26: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b69: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b00: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b02: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b25: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_10da: Unknown result type (might be due to invalid IL or missing references)
		//IL_10eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1115: Unknown result type (might be due to invalid IL or missing references)
		//IL_1117: Unknown result type (might be due to invalid IL or missing references)
		//IL_1128: Unknown result type (might be due to invalid IL or missing references)
		//IL_112e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1173: Unknown result type (might be due to invalid IL or missing references)
		//IL_245b: Unknown result type (might be due to invalid IL or missing references)
		//IL_245d: Unknown result type (might be due to invalid IL or missing references)
		//IL_247a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2480: Unknown result type (might be due to invalid IL or missing references)
		//IL_11bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_11bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c04: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_120b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1212: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b78: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b97: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_24d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_24d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_24f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_24f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bf7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1296: Unknown result type (might be due to invalid IL or missing references)
		//IL_129d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cce: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ce8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cea: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d01: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d22: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c42: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c44: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c61: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c67: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d50: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d52: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d63: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d69: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d95: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d97: Unknown result type (might be due to invalid IL or missing references)
		//IL_2db4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dba: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ddd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2de4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e20: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e22: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2549: Unknown result type (might be due to invalid IL or missing references)
		//IL_254b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2568: Unknown result type (might be due to invalid IL or missing references)
		//IL_256e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ce3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ce5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d08: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d31: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d38: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e88: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_25bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_25c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_25de: Unknown result type (might be due to invalid IL or missing references)
		//IL_25e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f52: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f75: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fba: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_14df: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_266e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2670: Unknown result type (might be due to invalid IL or missing references)
		//IL_268d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2693: Unknown result type (might be due to invalid IL or missing references)
		//IL_3020: Unknown result type (might be due to invalid IL or missing references)
		//IL_3027: Unknown result type (might be due to invalid IL or missing references)
		//IL_1589: Unknown result type (might be due to invalid IL or missing references)
		//IL_158b: Unknown result type (might be due to invalid IL or missing references)
		//IL_159c: Unknown result type (might be due to invalid IL or missing references)
		//IL_15a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_15df: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f11: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f17: Unknown result type (might be due to invalid IL or missing references)
		//IL_30c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_30c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_30e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_30ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_26e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_26e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_26ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_2705: Unknown result type (might be due to invalid IL or missing references)
		//IL_161c: Unknown result type (might be due to invalid IL or missing references)
		//IL_161e: Unknown result type (might be due to invalid IL or missing references)
		//IL_163b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1641: Unknown result type (might be due to invalid IL or missing references)
		//IL_310d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3114: Unknown result type (might be due to invalid IL or missing references)
		//IL_3150: Unknown result type (might be due to invalid IL or missing references)
		//IL_3152: Unknown result type (might be due to invalid IL or missing references)
		//IL_315a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f56: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f58: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f75: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_31b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_31bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_168e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1690: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_325d: Unknown result type (might be due to invalid IL or missing references)
		//IL_325f: Unknown result type (might be due to invalid IL or missing references)
		//IL_327c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3282: Unknown result type (might be due to invalid IL or missing references)
		//IL_32a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_32ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_32e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_32ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_1706: Unknown result type (might be due to invalid IL or missing references)
		//IL_1708: Unknown result type (might be due to invalid IL or missing references)
		//IL_1725: Unknown result type (might be due to invalid IL or missing references)
		//IL_172b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3352: Unknown result type (might be due to invalid IL or missing references)
		//IL_3359: Unknown result type (might be due to invalid IL or missing references)
		//IL_1790: Unknown result type (might be due to invalid IL or missing references)
		//IL_1792: Unknown result type (might be due to invalid IL or missing references)
		//IL_17af: Unknown result type (might be due to invalid IL or missing references)
		//IL_17b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_33f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_340c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3411: Unknown result type (might be due to invalid IL or missing references)
		//IL_342b: Unknown result type (might be due to invalid IL or missing references)
		//IL_342d: Unknown result type (might be due to invalid IL or missing references)
		//IL_343e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3444: Unknown result type (might be due to invalid IL or missing references)
		//IL_34bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_34bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_34e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_34e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_17fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_17fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1819: Unknown result type (might be due to invalid IL or missing references)
		//IL_181f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3533: Unknown result type (might be due to invalid IL or missing references)
		//IL_3535: Unknown result type (might be due to invalid IL or missing references)
		//IL_3558: Unknown result type (might be due to invalid IL or missing references)
		//IL_355e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1859: Unknown result type (might be due to invalid IL or missing references)
		//IL_185b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1878: Unknown result type (might be due to invalid IL or missing references)
		//IL_187e: Unknown result type (might be due to invalid IL or missing references)
		//IL_18ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_18af: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_35ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_35ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_35ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_35d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3623: Unknown result type (might be due to invalid IL or missing references)
		//IL_3625: Unknown result type (might be due to invalid IL or missing references)
		//IL_3648: Unknown result type (might be due to invalid IL or missing references)
		//IL_364e: Unknown result type (might be due to invalid IL or missing references)
		_canConsumeHover = true;
		for (int i = 0; i < skipRightSlot.Length; i++)
		{
			skipRightSlot[i] = false;
		}
		bool flag = GameCulture.FromCultureName(GameCulture.CultureName.Russian).IsActive || GameCulture.FromCultureName(GameCulture.CultureName.Portuguese).IsActive || GameCulture.FromCultureName(GameCulture.CultureName.Polish).IsActive || GameCulture.FromCultureName(GameCulture.CultureName.French).IsActive;
		bool isActive = GameCulture.FromCultureName(GameCulture.CultureName.Polish).IsActive;
		bool isActive2 = GameCulture.FromCultureName(GameCulture.CultureName.German).IsActive;
		bool flag2 = GameCulture.FromCultureName(GameCulture.CultureName.Italian).IsActive || GameCulture.FromCultureName(GameCulture.CultureName.Spanish).IsActive;
		bool flag3 = false;
		int num = 70;
		float scale = 0.75f;
		float num12 = 60f;
		float maxWidth = 300f;
		if (flag)
		{
			flag3 = true;
		}
		if (isActive)
		{
			maxWidth = 200f;
		}
		new Vector2((float)Main.mouseX, (float)Main.mouseY);
		bool flag4 = Main.mouseLeft && Main.mouseLeftRelease;
		Vector2 val = new Vector2((float)Main.screenWidth, (float)Main.screenHeight);
		Vector2 vector2 = default(Vector2);
		((Vector2)(ref vector2))._002Ector(670f, 480f);
		Vector2 vector3 = val / 2f - vector2 / 2f;
		int num18 = 20;
		_GUIHover = new Rectangle((int)(vector3.X - (float)num18), (int)(vector3.Y - (float)num18), (int)(vector2.X + (float)(num18 * 2)), (int)(vector2.Y + (float)(num18 * 2)));
		Utils.DrawInvBG(sb, vector3.X - (float)num18, vector3.Y - (float)num18, vector2.X + (float)(num18 * 2), vector2.Y + (float)(num18 * 2), new Color(33, 15, 91, 255) * 0.685f);
		Rectangle val2 = new Rectangle((int)vector3.X - num18, (int)vector3.Y - num18, (int)vector2.X + num18 * 2, (int)vector2.Y + num18 * 2);
		if (((Rectangle)(ref val2)).Contains(new Point(Main.mouseX, Main.mouseY)))
		{
			Main.player[Main.myPlayer].mouseInterface = true;
		}
		Utils.DrawBorderString(sb, Language.GetTextValue("GameUI.SettingsMenu"), vector3 + vector2 * new Vector2(0.5f, 0f), Color.White, 1f, 0.5f);
		if (flag)
		{
			Utils.DrawInvBG(sb, vector3.X + (float)(num18 / 2), vector3.Y + (float)(num18 * 5 / 2), vector2.X / 3f - (float)num18, vector2.Y - (float)(num18 * 3));
			Utils.DrawInvBG(sb, vector3.X + vector2.X / 3f + (float)num18, vector3.Y + (float)(num18 * 5 / 2), vector2.X * 2f / 3f - (float)(num18 * 3 / 2), vector2.Y - (float)(num18 * 3));
		}
		else
		{
			Utils.DrawInvBG(sb, vector3.X + (float)(num18 / 2), vector3.Y + (float)(num18 * 5 / 2), vector2.X / 2f - (float)num18, vector2.Y - (float)(num18 * 3));
			Utils.DrawInvBG(sb, vector3.X + vector2.X / 2f + (float)num18, vector3.Y + (float)(num18 * 5 / 2), vector2.X / 2f - (float)(num18 * 3 / 2), vector2.Y - (float)(num18 * 3));
		}
		float num19 = 0.7f;
		float num20 = 0.8f;
		float num21 = 0.01f;
		if (flag)
		{
			num19 = 0.4f;
			num20 = 0.44f;
		}
		if (isActive2)
		{
			num19 = 0.55f;
			num20 = 0.6f;
		}
		if (oldLeftHover != leftHover && leftHover != -1)
		{
			SoundEngine.PlaySound(12);
		}
		if (oldRightHover != rightHover && rightHover != -1)
		{
			SoundEngine.PlaySound(12);
		}
		if (flag4 && rightHover != -1 && !noSound)
		{
			SoundEngine.PlaySound(12);
		}
		oldLeftHover = leftHover;
		oldRightHover = rightHover;
		noSound = false;
		bool flag5 = SocialAPI.Network != null && SocialAPI.Network.CanInvite();
		int num22 = (flag5 ? 1 : 0);
		int num23 = 5 + num22 + 2;
		num23++;
		Vector2 vector4 = default(Vector2);
		((Vector2)(ref vector4))._002Ector(vector3.X + vector2.X / 4f, vector3.Y + (float)(num18 * 5 / 2));
		Vector2 vector5 = new Vector2(0f, vector2.Y - (float)(num18 * 5)) / (float)(num23 + 1);
		if (flag)
		{
			vector4.X -= 55f;
		}
		UILinkPointNavigator.Shortcuts.INGAMEOPTIONS_BUTTONS_LEFT = num23 + 1;
		for (int j = 0; j <= num23; j++)
		{
			bool flag6 = false;
			if (_leftSideCategoryMapping.TryGetValue(j, out var value))
			{
				flag6 = category == value;
			}
			if (leftHover == j || flag6)
			{
				leftScale[j] += num21;
			}
			else
			{
				leftScale[j] -= num21;
			}
			if (leftScale[j] < num19)
			{
				leftScale[j] = num19;
			}
			if (leftScale[j] > num20)
			{
				leftScale[j] = num20;
			}
		}
		leftHover = -1;
		int num24 = category;
		int num2 = 0;
		if (DrawLeftSide(sb, Lang.menu[114].Value, num2, vector4, vector5, leftScale))
		{
			leftHover = num2;
			if (flag4)
			{
				category = 0;
				SoundEngine.PlaySound(10);
			}
		}
		num2++;
		if (DrawLeftSide(sb, Lang.menu[210].Value, num2, vector4, vector5, leftScale))
		{
			leftHover = num2;
			if (flag4)
			{
				category = 1;
				SoundEngine.PlaySound(10);
			}
		}
		num2++;
		if (DrawLeftSide(sb, Lang.menu[63].Value, num2, vector4, vector5, leftScale))
		{
			leftHover = num2;
			if (flag4)
			{
				category = 2;
				SoundEngine.PlaySound(10);
			}
		}
		num2++;
		if (DrawLeftSide(sb, Lang.menu[218].Value, num2, vector4, vector5, leftScale))
		{
			leftHover = num2;
			if (flag4)
			{
				category = 3;
				SoundEngine.PlaySound(10);
			}
		}
		num2++;
		if (DrawLeftSide(sb, Lang.menu[66].Value, num2, vector4, vector5, leftScale))
		{
			leftHover = num2;
			if (flag4)
			{
				Close();
				IngameFancyUI.OpenKeybinds();
			}
		}
		num2++;
		if (DrawLeftSide(sb, Language.GetTextValue("tModLoader.ModConfiguration"), num2, vector4, vector5, leftScale))
		{
			leftHover = num2;
			if (flag4)
			{
				Close();
				IngameFancyUI.CoverNextFrame();
				Main.playerInventory = false;
				Main.editChest = false;
				Main.npcChatText = "";
				Main.inFancyUI = true;
				Main.InGameUI.SetState(Interface.modConfigList);
			}
		}
		num2++;
		if (flag5 && DrawLeftSide(sb, Lang.menu[147].Value, num2, vector4, vector5, leftScale))
		{
			leftHover = num2;
			if (flag4)
			{
				Close();
				SocialAPI.Network.OpenInviteInterface();
			}
		}
		if (flag5)
		{
			num2++;
		}
		if (DrawLeftSide(sb, Lang.menu[131].Value, num2, vector4, vector5, leftScale))
		{
			leftHover = num2;
			if (flag4)
			{
				Close();
				IngameFancyUI.OpenAchievements();
			}
		}
		num2++;
		if (DrawLeftSide(sb, Lang.menu[118].Value, num2, vector4, vector5, leftScale))
		{
			leftHover = num2;
			if (flag4)
			{
				Close();
			}
		}
		num2++;
		if (DrawLeftSide(sb, Lang.inter[35].Value, num2, vector4, vector5, leftScale))
		{
			leftHover = num2;
			if (flag4)
			{
				SteamedWraps.StopPlaytimeTracking();
				SystemLoader.PreSaveAndQuit();
				Close();
				Main.menuMode = 10;
				Main.gameMenu = true;
				WorldGen.SaveAndQuit();
			}
		}
		num2++;
		if (num24 != category)
		{
			for (int k = 0; k < rightScale.Length; k++)
			{
				rightScale[k] = 0f;
			}
		}
		int num3 = 0;
		int num4 = 0;
		switch (category)
		{
		case 0:
			num4 = 16;
			num19 = 1f;
			num20 = 1.001f;
			num21 = 0.001f;
			break;
		case 1:
			num4 = 11;
			num4++;
			num19 = 1f;
			num20 = 1.001f;
			num21 = 0.001f;
			break;
		case 2:
			num4 = 12;
			num19 = 1f;
			num20 = 1.001f;
			num21 = 0.001f;
			break;
		case 3:
			num4 = 15;
			num19 = 1f;
			num20 = 1.001f;
			num21 = 0.001f;
			break;
		}
		if (flag)
		{
			num19 -= 0.1f;
			num20 -= 0.1f;
		}
		if (isActive2 && category == 3)
		{
			num19 -= 0.15f;
			num20 -= 0.15f;
		}
		if (flag2 && (category == 0 || category == 3))
		{
			num19 -= 0.2f;
			num20 -= 0.2f;
		}
		UILinkPointNavigator.Shortcuts.INGAMEOPTIONS_BUTTONS_RIGHT = num4;
		Vector2 vector6 = default(Vector2);
		((Vector2)(ref vector6))._002Ector(vector3.X + vector2.X * 3f / 4f, vector3.Y + (float)(num18 * 5 / 2));
		Vector2 vector7 = new Vector2(0f, vector2.Y - (float)(num18 * 3)) / (float)(num4 + 1);
		if (category == 2)
		{
			vector7.Y -= 2f;
		}
		new Vector2(8f, 0f);
		if (flag)
		{
			vector6.X = vector3.X + vector2.X * 2f / 3f;
		}
		for (int l = 0; l < rightScale.Length; l++)
		{
			if (rightLock == l || (rightHover == l && rightLock == -1))
			{
				rightScale[l] += num21;
			}
			else
			{
				rightScale[l] -= num21;
			}
			if (rightScale[l] < num19)
			{
				rightScale[l] = num19;
			}
			if (rightScale[l] > num20)
			{
				rightScale[l] = num20;
			}
		}
		inBar = false;
		rightHover = -1;
		if (!Main.mouseLeft)
		{
			rightLock = -1;
		}
		if (rightLock == -1)
		{
			notBar = false;
		}
		if (category == 0)
		{
			int num5 = 0;
			DrawRightSide(sb, Lang.menu[65].Value, num5, vector6, vector7, rightScale[num5], 1f);
			skipRightSlot[num5] = true;
			num5++;
			vector6.X -= num;
			if (DrawRightSide(sb, Lang.menu[99].Value + " " + Math.Round(Main.musicVolume * 100f) + "%", num5, vector6, vector7, rightScale[num5], (rightScale[num5] - num19) / (num20 - num19)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				noSound = true;
				rightHover = num5;
			}
			valuePosition.X = vector3.X + vector2.X - (float)(num18 / 2) - 20f;
			valuePosition.Y -= 3f;
			float musicVolume = DrawValueBar(sb, scale, Main.musicVolume);
			if ((inBar || rightLock == num5) && !notBar)
			{
				rightHover = num5;
				if (Main.mouseLeft && rightLock == num5)
				{
					Main.musicVolume = musicVolume;
				}
			}
			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num18 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num5;
			}
			if (rightHover == num5)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 2;
			}
			num5++;
			if (DrawRightSide(sb, Lang.menu[98].Value + " " + Math.Round(Main.soundVolume * 100f) + "%", num5, vector6, vector7, rightScale[num5], (rightScale[num5] - num19) / (num20 - num19)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num5;
			}
			valuePosition.X = vector3.X + vector2.X - (float)(num18 / 2) - 20f;
			valuePosition.Y -= 3f;
			float soundVolume = DrawValueBar(sb, scale, Main.soundVolume);
			if ((inBar || rightLock == num5) && !notBar)
			{
				rightHover = num5;
				if (Main.mouseLeft && rightLock == num5)
				{
					Main.soundVolume = soundVolume;
					noSound = true;
				}
			}
			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num18 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num5;
			}
			if (rightHover == num5)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 3;
			}
			num5++;
			if (DrawRightSide(sb, Lang.menu[119].Value + " " + Math.Round(Main.ambientVolume * 100f) + "%", num5, vector6, vector7, rightScale[num5], (rightScale[num5] - num19) / (num20 - num19)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num5;
			}
			valuePosition.X = vector3.X + vector2.X - (float)(num18 / 2) - 20f;
			valuePosition.Y -= 3f;
			float ambientVolume = DrawValueBar(sb, scale, Main.ambientVolume);
			if ((inBar || rightLock == num5) && !notBar)
			{
				rightHover = num5;
				if (Main.mouseLeft && rightLock == num5)
				{
					Main.ambientVolume = ambientVolume;
					noSound = true;
				}
			}
			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num18 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num5;
			}
			if (rightHover == num5)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 4;
			}
			num5++;
			vector6.X += num;
			DrawRightSide(sb, "", num5, vector6, vector7, rightScale[num5], 1f);
			skipRightSlot[num5] = true;
			num5++;
			DrawRightSide(sb, Language.GetTextValue("GameUI.ZoomCategory"), num5, vector6, vector7, rightScale[num5], 1f);
			skipRightSlot[num5] = true;
			num5++;
			vector6.X -= num;
			string text = Language.GetTextValue("GameUI.GameZoom", Math.Round(Main.GameZoomTarget * 100f), Math.Round(Main.GameViewMatrix.Zoom.X * 100f));
			if (flag3)
			{
				text = FontAssets.ItemStack.Value.CreateWrappedText(text, maxWidth, Language.ActiveCulture.CultureInfo);
			}
			if (DrawRightSide(sb, text, num5, vector6, vector7, rightScale[num5] * 0.85f, (rightScale[num5] - num19) / (num20 - num19)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num5;
			}
			valuePosition.X = vector3.X + vector2.X - (float)(num18 / 2) - 20f;
			valuePosition.Y -= 3f;
			float num6 = DrawValueBar(sb, scale, Main.GameZoomTarget - 1f);
			if ((inBar || rightLock == num5) && !notBar)
			{
				rightHover = num5;
				if (Main.mouseLeft && rightLock == num5)
				{
					Main.GameZoomTarget = num6 + 1f;
				}
			}
			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num18 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num5;
			}
			if (rightHover == num5)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 10;
			}
			num5++;
			bool flag7 = false;
			if (Main.temporaryGUIScaleSlider == -1f)
			{
				Main.temporaryGUIScaleSlider = Main.UIScaleWanted;
			}
			string text3 = Language.GetTextValue("GameUI.UIScale", Math.Round(Main.temporaryGUIScaleSlider * 100f), Math.Round(Main.UIScale * 100f));
			if (flag3)
			{
				text3 = FontAssets.ItemStack.Value.CreateWrappedText(text3, maxWidth, Language.ActiveCulture.CultureInfo);
			}
			if (DrawRightSide(sb, text3, num5, vector6, vector7, rightScale[num5] * 0.75f, (rightScale[num5] - num19) / (num20 - num19)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num5;
			}
			valuePosition.X = vector3.X + vector2.X - (float)(num18 / 2) - 20f;
			valuePosition.Y -= 3f;
			float num7 = DrawValueBar(sb, scale, MathHelper.Clamp((Main.temporaryGUIScaleSlider - 0.5f) / 1.5f, 0f, 1f));
			if ((inBar || rightLock == num5) && !notBar)
			{
				rightHover = num5;
				if (Main.mouseLeft && rightLock == num5)
				{
					Main.temporaryGUIScaleSlider = num7 * 1.5f + 0.5f;
					Main.temporaryGUIScaleSlider = (float)(int)(Main.temporaryGUIScaleSlider * 100f) / 100f;
					Main.temporaryGUIScaleSliderUpdate = true;
					flag7 = true;
				}
			}
			if (!flag7 && Main.temporaryGUIScaleSliderUpdate && Main.temporaryGUIScaleSlider != -1f)
			{
				Main.UIScale = Main.temporaryGUIScaleSlider;
				Main.temporaryGUIScaleSliderUpdate = false;
			}
			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num18 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num5;
			}
			if (rightHover == num5)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 11;
			}
			num5++;
			vector6.X += num;
			DrawRightSide(sb, "", num5, vector6, vector7, rightScale[num5], 1f);
			skipRightSlot[num5] = true;
			num5++;
			DrawRightSide(sb, Language.GetTextValue("GameUI.Gameplay"), num5, vector6, vector7, rightScale[num5], 1f);
			skipRightSlot[num5] = true;
			num5++;
			if (DrawRightSide(sb, Main.autoSave ? Lang.menu[67].Value : Lang.menu[68].Value, num5, vector6, vector7, rightScale[num5], (rightScale[num5] - num19) / (num20 - num19)))
			{
				rightHover = num5;
				if (flag4)
				{
					Main.autoSave = !Main.autoSave;
				}
			}
			num5++;
			if (DrawRightSide(sb, Main.autoPause ? Lang.menu[69].Value : Lang.menu[70].Value, num5, vector6, vector7, rightScale[num5], (rightScale[num5] - num19) / (num20 - num19)))
			{
				rightHover = num5;
				if (flag4)
				{
					Main.autoPause = !Main.autoPause;
				}
			}
			num5++;
			if (DrawRightSide(sb, Main.ReversedUpDownArmorSetBonuses ? Lang.menu[220].Value : Lang.menu[221].Value, num5, vector6, vector7, rightScale[num5], (rightScale[num5] - num19) / (num20 - num19)))
			{
				rightHover = num5;
				if (flag4)
				{
					Main.ReversedUpDownArmorSetBonuses = !Main.ReversedUpDownArmorSetBonuses;
				}
			}
			num5++;
			if (DrawRightSide(sb, DoorOpeningHelper.PreferenceSettings switch
			{
				DoorOpeningHelper.DoorAutoOpeningPreference.EnabledForEverything => Language.GetTextValue("UI.SmartDoorsEnabled"), 
				DoorOpeningHelper.DoorAutoOpeningPreference.EnabledForGamepadOnly => Language.GetTextValue("UI.SmartDoorsGamepad"), 
				_ => Language.GetTextValue("UI.SmartDoorsDisabled"), 
			}, num5, vector6, vector7, rightScale[num5], (rightScale[num5] - num19) / (num20 - num19)))
			{
				rightHover = num5;
				if (flag4)
				{
					DoorOpeningHelper.CyclePreferences();
				}
			}
			num5++;
			string textValue2 = ((Player.Settings.HoverControl == Player.Settings.HoverControlMode.Hold) ? Language.GetTextValue("UI.HoverControlSettingIsHold") : Language.GetTextValue("UI.HoverControlSettingIsClick"));
			if (DrawRightSide(sb, textValue2, num5, vector6, vector7, rightScale[num5], (rightScale[num5] - num19) / (num20 - num19)))
			{
				rightHover = num5;
				if (flag4)
				{
					Player.Settings.CycleHoverControl();
				}
			}
			num5++;
			if (DrawRightSide(sb, Language.GetTextValue(Main.SettingsEnabled_AutoReuseAllItems ? "UI.AutoReuseAllOn" : "UI.AutoReuseAllOff"), num5, vector6, vector7, rightScale[num5], (rightScale[num5] - num19) / (num20 - num19)))
			{
				rightHover = num5;
				if (flag4)
				{
					Main.SettingsEnabled_AutoReuseAllItems = !Main.SettingsEnabled_AutoReuseAllItems;
				}
			}
			num5++;
			DrawRightSide(sb, "", num5, vector6, vector7, rightScale[num5], 1f);
			skipRightSlot[num5] = true;
			num5++;
		}
		if (category == 1)
		{
			int num8 = 0;
			if (DrawRightSide(sb, Main.showItemText ? Lang.menu[71].Value : Lang.menu[72].Value, num8, vector6, vector7, rightScale[num8], (rightScale[num8] - num19) / (num20 - num19)))
			{
				rightHover = num8;
				if (flag4)
				{
					Main.showItemText = !Main.showItemText;
				}
			}
			num8++;
			if (DrawRightSide(sb, Lang.menu[123].Value + " " + Lang.menu[124 + Main.invasionProgressMode], num8, vector6, vector7, rightScale[num8], (rightScale[num8] - num19) / (num20 - num19)))
			{
				rightHover = num8;
				if (flag4)
				{
					Main.invasionProgressMode++;
					if (Main.invasionProgressMode >= 3)
					{
						Main.invasionProgressMode = 0;
					}
				}
			}
			num8++;
			if (DrawRightSide(sb, Main.placementPreview ? Lang.menu[128].Value : Lang.menu[129].Value, num8, vector6, vector7, rightScale[num8], (rightScale[num8] - num19) / (num20 - num19)))
			{
				rightHover = num8;
				if (flag4)
				{
					Main.placementPreview = !Main.placementPreview;
				}
			}
			num8++;
			if (DrawRightSide(sb, ItemSlot.Options.HighlightNewItems ? Lang.inter[117].Value : Lang.inter[116].Value, num8, vector6, vector7, rightScale[num8], (rightScale[num8] - num19) / (num20 - num19)))
			{
				rightHover = num8;
				if (flag4)
				{
					ItemSlot.Options.HighlightNewItems = !ItemSlot.Options.HighlightNewItems;
				}
			}
			num8++;
			if (DrawRightSide(sb, Main.MouseShowBuildingGrid ? Lang.menu[229].Value : Lang.menu[230].Value, num8, vector6, vector7, rightScale[num8], (rightScale[num8] - num19) / (num20 - num19)))
			{
				rightHover = num8;
				if (flag4)
				{
					Main.MouseShowBuildingGrid = !Main.MouseShowBuildingGrid;
				}
			}
			num8++;
			if (DrawRightSide(sb, Main.GamepadDisableInstructionsDisplay ? Lang.menu[241].Value : Lang.menu[242].Value, num8, vector6, vector7, rightScale[num8], (rightScale[num8] - num19) / (num20 - num19)))
			{
				rightHover = num8;
				if (flag4)
				{
					Main.GamepadDisableInstructionsDisplay = !Main.GamepadDisableInstructionsDisplay;
				}
			}
			num8++;
			Action onClick;
			string text2 = BossBarLoader.InsertMenu(out onClick);
			if (DrawRightSide(sb, text2, num8, vector6, vector7, rightScale[num8], (rightScale[num8] - num19) / (num20 - num19)))
			{
				rightHover = num8;
				if (flag4)
				{
					onClick();
				}
			}
			num8++;
			string textValue3 = Language.GetTextValue("UI.MinimapFrame_" + Main.MinimapFrameManagerInstance.ActiveSelectionKeyName);
			if (DrawRightSide(sb, Language.GetTextValue("UI.SelectMapBorder", textValue3), num8, vector6, vector7, rightScale[num8], (rightScale[num8] - num19) / (num20 - num19)))
			{
				rightHover = num8;
				if (flag4)
				{
					Main.MinimapFrameManagerInstance.CycleSelection();
				}
			}
			num8++;
			vector6.X -= num;
			string text4 = Language.GetTextValue("GameUI.MapScale", Math.Round(Main.MapScale * 100f));
			if (flag3)
			{
				text4 = FontAssets.ItemStack.Value.CreateWrappedText(text4, maxWidth, Language.ActiveCulture.CultureInfo);
			}
			if (DrawRightSide(sb, text4, num8, vector6, vector7, rightScale[num8] * 0.85f, (rightScale[num8] - num19) / (num20 - num19)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num8;
			}
			valuePosition.X = vector3.X + vector2.X - (float)(num18 / 2) - 20f;
			valuePosition.Y -= 3f;
			float num9 = DrawValueBar(sb, scale, (Main.MapScale - 0.5f) / 0.5f);
			if ((inBar || rightLock == num8) && !notBar)
			{
				rightHover = num8;
				if (Main.mouseLeft && rightLock == num8)
				{
					Main.MapScale = num9 * 0.5f + 0.5f;
				}
			}
			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num18 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num8;
			}
			if (rightHover == num8)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 12;
			}
			num8++;
			vector6.X += num;
			string textValue4 = Main.ResourceSetsManager.ActiveSet.DisplayedName;
			if (DrawRightSide(sb, Language.GetTextValue("UI.SelectHealthStyle", textValue4), num8, vector6, vector7, rightScale[num8], (rightScale[num8] - num19) / (num20 - num19)))
			{
				rightHover = num8;
				if (flag4)
				{
					Main.ResourceSetsManager.CycleResourceSet();
				}
			}
			num8++;
			string textValue5 = Language.GetTextValue(BigProgressBarSystem.ShowText ? "UI.ShowBossLifeTextOn" : "UI.ShowBossLifeTextOff");
			if (DrawRightSide(sb, textValue5, num8, vector6, vector7, rightScale[num8], (rightScale[num8] - num19) / (num20 - num19)))
			{
				rightHover = num8;
				if (flag4)
				{
					BigProgressBarSystem.ToggleShowText();
				}
			}
			num8++;
			if (DrawRightSide(sb, Main.SettingsEnabled_OpaqueBoxBehindTooltips ? Language.GetTextValue("GameUI.HoverTextBoxesOn") : Language.GetTextValue("GameUI.HoverTextBoxesOff"), num8, vector6, vector7, rightScale[num8], (rightScale[num8] - num19) / (num20 - num19)))
			{
				rightHover = num8;
				if (flag4)
				{
					Main.SettingsEnabled_OpaqueBoxBehindTooltips = !Main.SettingsEnabled_OpaqueBoxBehindTooltips;
				}
			}
			num8++;
		}
		if (category == 2)
		{
			int num10 = 0;
			if (DrawRightSide(sb, Main.graphics.IsFullScreen ? Lang.menu[49].Value : Lang.menu[50].Value, num10, vector6, vector7, rightScale[num10], (rightScale[num10] - num19) / (num20 - num19)))
			{
				rightHover = num10;
				if (flag4)
				{
					Main.ToggleFullScreen();
				}
			}
			num10++;
			if (DrawRightSide(sb, Lang.menu[51].Value + ": " + Main.PendingResolutionWidth + "x" + Main.PendingResolutionHeight, num10, vector6, vector7, rightScale[num10], (rightScale[num10] - num19) / (num20 - num19)))
			{
				rightHover = num10;
				if (flag4 || (Main.mouseRight && Main.mouseRightRelease))
				{
					int num11 = 0;
					for (int m = 0; m < Main.numDisplayModes; m++)
					{
						if (Main.displayWidth[m] == Main.PendingResolutionWidth && Main.displayHeight[m] == Main.PendingResolutionHeight)
						{
							num11 = m;
							break;
						}
					}
					num11 = Utils.Repeat(num11 + (flag4 ? 1 : (-1)), Main.numDisplayModes);
					Main.PendingResolutionWidth = Main.displayWidth[num11];
					Main.PendingResolutionHeight = Main.displayHeight[num11];
					Main.SetResolution(Main.PendingResolutionWidth, Main.PendingResolutionHeight);
				}
			}
			num10++;
			vector6.X -= num;
			if (DrawRightSide(sb, Lang.menu[52].Value + ": " + Main.bgScroll + "%", num10, vector6, vector7, rightScale[num10], (rightScale[num10] - num19) / (num20 - num19)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				noSound = true;
				rightHover = num10;
			}
			valuePosition.X = vector3.X + vector2.X - (float)(num18 / 2) - 20f;
			valuePosition.Y -= 3f;
			float num13 = DrawValueBar(sb, scale, (float)Main.bgScroll / 100f);
			if ((inBar || rightLock == num10) && !notBar)
			{
				rightHover = num10;
				if (Main.mouseLeft && rightLock == num10)
				{
					Main.bgScroll = (int)(num13 * 100f);
					Main.caveParallax = 1f - (float)Main.bgScroll / 500f;
				}
			}
			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num18 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num10;
			}
			if (rightHover == num10)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 1;
			}
			num10++;
			vector6.X += num;
			if (DrawRightSide(sb, Lang.menu[(int)(247 + Main.FrameSkipMode)].Value, num10, vector6, vector7, rightScale[num10], (rightScale[num10] - num19) / (num20 - num19)))
			{
				rightHover = num10;
				if (flag4)
				{
					Main.CycleFrameSkipMode();
				}
			}
			num10++;
			if (DrawRightSide(sb, Language.GetTextValue("UI.LightMode_" + Lighting.Mode), num10, vector6, vector7, rightScale[num10], (rightScale[num10] - num19) / (num20 - num19)))
			{
				rightHover = num10;
				if (flag4)
				{
					Lighting.NextLightMode();
				}
			}
			num10++;
			if (DrawRightSide(sb, Lang.menu[59 + Main.qaStyle].Value, num10, vector6, vector7, rightScale[num10], (rightScale[num10] - num19) / (num20 - num19)))
			{
				rightHover = num10;
				if (flag4)
				{
					Main.qaStyle++;
					if (Main.qaStyle > 3)
					{
						Main.qaStyle = 0;
					}
				}
			}
			num10++;
			if (DrawRightSide(sb, Main.BackgroundEnabled ? Lang.menu[100].Value : Lang.menu[101].Value, num10, vector6, vector7, rightScale[num10], (rightScale[num10] - num19) / (num20 - num19)))
			{
				rightHover = num10;
				if (flag4)
				{
					Main.BackgroundEnabled = !Main.BackgroundEnabled;
				}
			}
			num10++;
			if (DrawRightSide(sb, ChildSafety.Disabled ? Lang.menu[132].Value : Lang.menu[133].Value, num10, vector6, vector7, rightScale[num10], (rightScale[num10] - num19) / (num20 - num19)))
			{
				rightHover = num10;
				if (flag4)
				{
					ChildSafety.Disabled = !ChildSafety.Disabled;
				}
			}
			num10++;
			if (DrawRightSide(sb, Language.GetTextValue("GameUI.HeatDistortion", Main.UseHeatDistortion ? Language.GetTextValue("GameUI.Enabled") : Language.GetTextValue("GameUI.Disabled")), num10, vector6, vector7, rightScale[num10], (rightScale[num10] - num19) / (num20 - num19)))
			{
				rightHover = num10;
				if (flag4)
				{
					Main.UseHeatDistortion = !Main.UseHeatDistortion;
				}
			}
			num10++;
			if (DrawRightSide(sb, Language.GetTextValue("GameUI.StormEffects", Main.UseStormEffects ? Language.GetTextValue("GameUI.Enabled") : Language.GetTextValue("GameUI.Disabled")), num10, vector6, vector7, rightScale[num10], (rightScale[num10] - num19) / (num20 - num19)))
			{
				rightHover = num10;
				if (flag4)
				{
					Main.UseStormEffects = !Main.UseStormEffects;
				}
			}
			num10++;
			if (DrawRightSide(sb, Language.GetTextValue("GameUI.WaveQuality", Main.WaveQuality switch
			{
				1 => Language.GetTextValue("GameUI.QualityLow"), 
				2 => Language.GetTextValue("GameUI.QualityMedium"), 
				3 => Language.GetTextValue("GameUI.QualityHigh"), 
				_ => Language.GetTextValue("GameUI.QualityOff"), 
			}), num10, vector6, vector7, rightScale[num10], (rightScale[num10] - num19) / (num20 - num19)))
			{
				rightHover = num10;
				if (flag4)
				{
					Main.WaveQuality = (Main.WaveQuality + 1) % 4;
				}
			}
			num10++;
			if (DrawRightSide(sb, Language.GetTextValue("UI.TilesSwayInWind" + (Main.SettingsEnabled_TilesSwayInWind ? "On" : "Off")), num10, vector6, vector7, rightScale[num10], (rightScale[num10] - num19) / (num20 - num19)))
			{
				rightHover = num10;
				if (flag4)
				{
					Main.SettingsEnabled_TilesSwayInWind = !Main.SettingsEnabled_TilesSwayInWind;
				}
			}
			num10++;
		}
		if (category == 3)
		{
			int num14 = 0;
			float num15 = num;
			if (flag)
			{
				num12 = 126f;
			}
			Vector3 hSLVector = Main.mouseColorSlider.GetHSLVector();
			Main.mouseColorSlider.ApplyToMainLegacyBars();
			DrawRightSide(sb, Lang.menu[64].Value, num14, vector6, vector7, rightScale[num14], 1f);
			skipRightSlot[num14] = true;
			num14++;
			vector6.X -= num15;
			if (DrawRightSide(sb, "", num14, vector6, vector7, rightScale[num14], (rightScale[num14] - num19) / (num20 - num19)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num14;
			}
			valuePosition.X = vector3.X + vector2.X - (float)(num18 / 2) - 20f;
			valuePosition.Y -= 3f;
			valuePosition.X -= num12;
			DelegateMethods.v3_1 = hSLVector;
			float x = DrawValueBar(sb, scale, hSLVector.X, 0, DelegateMethods.ColorLerp_HSL_H);
			if ((inBar || rightLock == num14) && !notBar)
			{
				rightHover = num14;
				if (Main.mouseLeft && rightLock == num14)
				{
					hSLVector.X = x;
					noSound = true;
				}
			}
			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num18 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num14;
			}
			if (rightHover == num14)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 5;
				Main.menuMode = 25;
			}
			num14++;
			if (DrawRightSide(sb, "", num14, vector6, vector7, rightScale[num14], (rightScale[num14] - num19) / (num20 - num19)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num14;
			}
			valuePosition.X = vector3.X + vector2.X - (float)(num18 / 2) - 20f;
			valuePosition.Y -= 3f;
			valuePosition.X -= num12;
			DelegateMethods.v3_1 = hSLVector;
			x = DrawValueBar(sb, scale, hSLVector.Y, 0, DelegateMethods.ColorLerp_HSL_S);
			if ((inBar || rightLock == num14) && !notBar)
			{
				rightHover = num14;
				if (Main.mouseLeft && rightLock == num14)
				{
					hSLVector.Y = x;
					noSound = true;
				}
			}
			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num18 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num14;
			}
			if (rightHover == num14)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 6;
				Main.menuMode = 25;
			}
			num14++;
			if (DrawRightSide(sb, "", num14, vector6, vector7, rightScale[num14], (rightScale[num14] - num19) / (num20 - num19)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num14;
			}
			valuePosition.X = vector3.X + vector2.X - (float)(num18 / 2) - 20f;
			valuePosition.Y -= 3f;
			valuePosition.X -= num12;
			DelegateMethods.v3_1 = hSLVector;
			DelegateMethods.v3_1.Z = Utils.GetLerpValue(0.15f, 1f, DelegateMethods.v3_1.Z, clamped: true);
			x = DrawValueBar(sb, scale, DelegateMethods.v3_1.Z, 0, DelegateMethods.ColorLerp_HSL_L);
			if ((inBar || rightLock == num14) && !notBar)
			{
				rightHover = num14;
				if (Main.mouseLeft && rightLock == num14)
				{
					hSLVector.Z = x * 0.85f + 0.15f;
					noSound = true;
				}
			}
			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num18 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num14;
			}
			if (rightHover == num14)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 7;
				Main.menuMode = 25;
			}
			num14++;
			if (hSLVector.Z < 0.15f)
			{
				hSLVector.Z = 0.15f;
			}
			Main.mouseColorSlider.SetHSL(hSLVector);
			Main.mouseColor = Main.mouseColorSlider.GetColor();
			vector6.X += num15;
			DrawRightSide(sb, "", num14, vector6, vector7, rightScale[num14], 1f);
			skipRightSlot[num14] = true;
			num14++;
			hSLVector = Main.mouseBorderColorSlider.GetHSLVector();
			if (PlayerInput.UsingGamepad && rightHover == -1)
			{
				Main.mouseBorderColorSlider.ApplyToMainLegacyBars();
			}
			DrawRightSide(sb, Lang.menu[217].Value, num14, vector6, vector7, rightScale[num14], 1f);
			skipRightSlot[num14] = true;
			num14++;
			vector6.X -= num15;
			if (DrawRightSide(sb, "", num14, vector6, vector7, rightScale[num14], (rightScale[num14] - num19) / (num20 - num19)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num14;
			}
			valuePosition.X = vector3.X + vector2.X - (float)(num18 / 2) - 20f;
			valuePosition.Y -= 3f;
			valuePosition.X -= num12;
			DelegateMethods.v3_1 = hSLVector;
			x = DrawValueBar(sb, scale, hSLVector.X, 0, DelegateMethods.ColorLerp_HSL_H);
			if ((inBar || rightLock == num14) && !notBar)
			{
				rightHover = num14;
				if (Main.mouseLeft && rightLock == num14)
				{
					hSLVector.X = x;
					noSound = true;
				}
			}
			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num18 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num14;
			}
			if (rightHover == num14)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 5;
				Main.menuMode = 252;
			}
			num14++;
			if (DrawRightSide(sb, "", num14, vector6, vector7, rightScale[num14], (rightScale[num14] - num19) / (num20 - num19)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num14;
			}
			valuePosition.X = vector3.X + vector2.X - (float)(num18 / 2) - 20f;
			valuePosition.Y -= 3f;
			valuePosition.X -= num12;
			DelegateMethods.v3_1 = hSLVector;
			x = DrawValueBar(sb, scale, hSLVector.Y, 0, DelegateMethods.ColorLerp_HSL_S);
			if ((inBar || rightLock == num14) && !notBar)
			{
				rightHover = num14;
				if (Main.mouseLeft && rightLock == num14)
				{
					hSLVector.Y = x;
					noSound = true;
				}
			}
			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num18 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num14;
			}
			if (rightHover == num14)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 6;
				Main.menuMode = 252;
			}
			num14++;
			if (DrawRightSide(sb, "", num14, vector6, vector7, rightScale[num14], (rightScale[num14] - num19) / (num20 - num19)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num14;
			}
			valuePosition.X = vector3.X + vector2.X - (float)(num18 / 2) - 20f;
			valuePosition.Y -= 3f;
			valuePosition.X -= num12;
			DelegateMethods.v3_1 = hSLVector;
			x = DrawValueBar(sb, scale, hSLVector.Z, 0, DelegateMethods.ColorLerp_HSL_L);
			if ((inBar || rightLock == num14) && !notBar)
			{
				rightHover = num14;
				if (Main.mouseLeft && rightLock == num14)
				{
					hSLVector.Z = x;
					noSound = true;
				}
			}
			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num18 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num14;
			}
			if (rightHover == num14)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 7;
				Main.menuMode = 252;
			}
			num14++;
			if (DrawRightSide(sb, "", num14, vector6, vector7, rightScale[num14], (rightScale[num14] - num19) / (num20 - num19)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num14;
			}
			valuePosition.X = vector3.X + vector2.X - (float)(num18 / 2) - 20f;
			valuePosition.Y -= 3f;
			valuePosition.X -= num12;
			DelegateMethods.v3_1 = hSLVector;
			float num16 = Main.mouseBorderColorSlider.Alpha;
			x = DrawValueBar(sb, scale, num16, 0, DelegateMethods.ColorLerp_HSL_O);
			if ((inBar || rightLock == num14) && !notBar)
			{
				rightHover = num14;
				if (Main.mouseLeft && rightLock == num14)
				{
					num16 = x;
					noSound = true;
				}
			}
			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num18 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num14;
			}
			if (rightHover == num14)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 8;
				Main.menuMode = 252;
			}
			num14++;
			Main.mouseBorderColorSlider.SetHSL(hSLVector);
			Main.mouseBorderColorSlider.Alpha = num16;
			Main.MouseBorderColor = Main.mouseBorderColorSlider.GetColor();
			vector6.X += num15;
			DrawRightSide(sb, "", num14, vector6, vector7, rightScale[num14], 1f);
			skipRightSlot[num14] = true;
			num14++;
			string txt = "";
			switch (LockOnHelper.UseMode)
			{
			case LockOnHelper.LockOnMode.FocusTarget:
				txt = Lang.menu[232].Value;
				break;
			case LockOnHelper.LockOnMode.TargetClosest:
				txt = Lang.menu[233].Value;
				break;
			case LockOnHelper.LockOnMode.ThreeDS:
				txt = Lang.menu[234].Value;
				break;
			}
			if (DrawRightSide(sb, txt, num14, vector6, vector7, rightScale[num14] * 0.9f, (rightScale[num14] - num19) / (num20 - num19)))
			{
				rightHover = num14;
				if (flag4)
				{
					LockOnHelper.CycleUseModes();
				}
			}
			num14++;
			if (DrawRightSide(sb, Player.SmartCursorSettings.SmartBlocksEnabled ? Lang.menu[215].Value : Lang.menu[216].Value, num14, vector6, vector7, rightScale[num14] * 0.9f, (rightScale[num14] - num19) / (num20 - num19)))
			{
				rightHover = num14;
				if (flag4)
				{
					Player.SmartCursorSettings.SmartBlocksEnabled = !Player.SmartCursorSettings.SmartBlocksEnabled;
				}
			}
			num14++;
			if (DrawRightSide(sb, Main.cSmartCursorModeIsToggleAndNotHold ? Lang.menu[121].Value : Lang.menu[122].Value, num14, vector6, vector7, rightScale[num14], (rightScale[num14] - num19) / (num20 - num19)))
			{
				rightHover = num14;
				if (flag4)
				{
					Main.cSmartCursorModeIsToggleAndNotHold = !Main.cSmartCursorModeIsToggleAndNotHold;
				}
			}
			num14++;
			if (DrawRightSide(sb, Player.SmartCursorSettings.SmartAxeAfterPickaxe ? Lang.menu[214].Value : Lang.menu[213].Value, num14, vector6, vector7, rightScale[num14] * 0.9f, (rightScale[num14] - num19) / (num20 - num19)))
			{
				rightHover = num14;
				if (flag4)
				{
					Player.SmartCursorSettings.SmartAxeAfterPickaxe = !Player.SmartCursorSettings.SmartAxeAfterPickaxe;
				}
			}
			num14++;
		}
		if (rightHover != -1 && rightLock == -1)
		{
			rightLock = rightHover;
		}
		for (int n = 0; n < num23 + 1; n++)
		{
			UILinkPointNavigator.SetPosition(2900 + n, vector4 + vector5 * (float)(n + 1));
		}
		Vector2 zero = Vector2.Zero;
		if (flag)
		{
			zero.X = -40f;
		}
		for (int num17 = 0; num17 < num4; num17++)
		{
			if (!skipRightSlot[num17])
			{
				UILinkPointNavigator.SetPosition(2930 + num3, vector6 + zero + vector7 * (float)(num17 + 1));
				num3++;
			}
		}
		UILinkPointNavigator.Shortcuts.INGAMEOPTIONS_BUTTONS_RIGHT = num3;
		Main.DrawInterface_29_SettingsButton();
		Main.DrawGamepadInstructions();
		Main.mouseText = false;
		Main.instance.GUIBarsDraw();
		Main.instance.DrawMouseOver();
		Main.DrawCursor(Main.DrawThickCursor());
	}

	public static void MouseOver()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		if (Main.ingameOptionsWindow)
		{
			if (((Rectangle)(ref _GUIHover)).Contains(Main.MouseScreen.ToPoint()))
			{
				Main.mouseText = true;
			}
			if (_mouseOverText != null)
			{
				Main.instance.MouseText(_mouseOverText, 0, 0);
			}
			_mouseOverText = null;
		}
	}

	public static bool DrawLeftSide(SpriteBatch sb, string txt, int i, Vector2 anchor, Vector2 offset, float[] scales, float minscale = 0.7f, float maxscale = 0.8f, float scalespeed = 0.01f)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		if (_leftSideCategoryMapping.TryGetValue(i, out var value))
		{
			flag = category == value;
		}
		Color color = Color.Lerp(Color.Gray, Color.White, (scales[i] - minscale) / (maxscale - minscale));
		if (flag)
		{
			color = Color.Gold;
		}
		Vector2 vector = Utils.DrawBorderStringBig(sb, txt, anchor + offset * (float)(1 + i), color, scales[i], 0.5f, 0.5f);
		Rectangle val = new Rectangle((int)anchor.X - (int)vector.X / 2, (int)anchor.Y + (int)(offset.Y * (float)(1 + i)) - (int)vector.Y / 2, (int)vector.X, (int)vector.Y);
		bool flag2 = ((Rectangle)(ref val)).Contains(new Point(Main.mouseX, Main.mouseY));
		if (!_canConsumeHover)
		{
			return false;
		}
		if (flag2)
		{
			_canConsumeHover = false;
			return true;
		}
		return false;
	}

	public static bool DrawRightSide(SpriteBatch sb, string txt, int i, Vector2 anchor, Vector2 offset, float scale, float colorScale, Color over = default(Color))
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		Color color = Color.Lerp(Color.Gray, Color.White, colorScale);
		if (over != default(Color))
		{
			color = over;
		}
		Vector2 vector = Utils.DrawBorderString(sb, txt, anchor + offset * (float)(1 + i), color, scale, 0.5f, 0.5f);
		valuePosition = anchor + offset * (float)(1 + i) + vector * new Vector2(0.5f, 0f);
		Rectangle val = new Rectangle((int)anchor.X - (int)vector.X / 2, (int)anchor.Y + (int)(offset.Y * (float)(1 + i)) - (int)vector.Y / 2, (int)vector.X, (int)vector.Y);
		bool flag = ((Rectangle)(ref val)).Contains(new Point(Main.mouseX, Main.mouseY));
		if (!_canConsumeHover)
		{
			return false;
		}
		if (flag)
		{
			_canConsumeHover = false;
			return true;
		}
		return false;
	}

	public static Rectangle GetExpectedRectangleForNotification(int itemIndex, Vector2 anchor, Vector2 offset, int areaWidth)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		return Utils.CenteredRectangle(anchor + offset * (float)(1 + itemIndex), new Vector2((float)areaWidth, offset.Y - 4f));
	}

	public static bool DrawValue(SpriteBatch sb, string txt, int i, float scale, Color over = default(Color))
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		Color color = Color.Gray;
		Vector2 vector = FontAssets.MouseText.Value.MeasureString(txt) * scale;
		Rectangle val = new Rectangle((int)valuePosition.X, (int)valuePosition.Y - (int)vector.Y / 2, (int)vector.X, (int)vector.Y);
		bool flag = ((Rectangle)(ref val)).Contains(new Point(Main.mouseX, Main.mouseY));
		if (flag)
		{
			color = Color.White;
		}
		if (over != default(Color))
		{
			color = over;
		}
		Utils.DrawBorderString(sb, txt, valuePosition, color, scale, 0f, 0.5f);
		valuePosition.X += vector.X;
		if (!_canConsumeHover)
		{
			return false;
		}
		if (flag)
		{
			_canConsumeHover = false;
			return true;
		}
		return false;
	}

	public static float DrawValueBar(SpriteBatch sb, float scale, float perc, int lockState = 0, Utils.ColorLerpMethod colorMethod = null)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		if (colorMethod == null)
		{
			colorMethod = Utils.ColorLerp_BlackToWhite;
		}
		Texture2D value = TextureAssets.ColorBar.Value;
		Vector2 vector = new Vector2((float)value.Width, (float)value.Height) * scale;
		valuePosition.X -= (int)vector.X;
		Rectangle rectangle = default(Rectangle);
		((Rectangle)(ref rectangle))._002Ector((int)valuePosition.X, (int)valuePosition.Y - (int)vector.Y / 2, (int)vector.X, (int)vector.Y);
		Rectangle destinationRectangle = rectangle;
		sb.Draw(value, rectangle, Color.White);
		int num = 167;
		float num2 = (float)rectangle.X + 5f * scale;
		float num3 = (float)rectangle.Y + 4f * scale;
		for (float num4 = 0f; num4 < (float)num; num4 += 1f)
		{
			float percent = num4 / (float)num;
			sb.Draw(TextureAssets.ColorBlip.Value, new Vector2(num2 + num4 * scale, num3), (Rectangle?)null, colorMethod(percent), 0f, Vector2.Zero, scale, (SpriteEffects)0, 0f);
		}
		((Rectangle)(ref rectangle)).Inflate((int)(-5f * scale), 0);
		bool flag = ((Rectangle)(ref rectangle)).Contains(new Point(Main.mouseX, Main.mouseY));
		if (lockState == 2)
		{
			flag = false;
		}
		if (flag || lockState == 1)
		{
			sb.Draw(TextureAssets.ColorHighlight.Value, destinationRectangle, Main.OurFavoriteColor);
		}
		sb.Draw(TextureAssets.ColorSlider.Value, new Vector2(num2 + 167f * scale * perc, num3 + 4f * scale), (Rectangle?)null, Color.White, 0f, new Vector2(0.5f * (float)TextureAssets.ColorSlider.Width(), 0.5f * (float)TextureAssets.ColorSlider.Height()), scale, (SpriteEffects)0, 0f);
		if (Main.mouseX >= rectangle.X && Main.mouseX <= rectangle.X + rectangle.Width)
		{
			inBar = flag;
			return (float)(Main.mouseX - rectangle.X) / (float)rectangle.Width;
		}
		inBar = false;
		if (rectangle.X >= Main.mouseX)
		{
			return 0f;
		}
		return 1f;
	}
}
