using System;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameContent.UI.States;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.Social;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.Initializers;

public class UILinksInitializer
{
	public class SomeVarsForUILinkers
	{
		public static Recipe SequencedCraftingCurrent;

		public static int HairMoveCD;
	}

	public static bool NothingMoreImportantThanNPCChat()
	{
		if (!Main.hairWindow && Main.npcShop == 0)
		{
			return Main.player[Main.myPlayer].chest == -1;
		}
		return false;
	}

	public static float HandleSliderHorizontalInput(float currentValue, float min, float max, float deadZone = 0.2f, float sensitivity = 0.5f)
	{
		float x = PlayerInput.GamepadThumbstickLeft.X;
		x = ((!(x < 0f - deadZone) && !(x > deadZone)) ? 0f : (MathHelper.Lerp(0f, sensitivity / 60f, (Math.Abs(x) - deadZone) / (1f - deadZone)) * (float)Math.Sign(x)));
		return MathHelper.Clamp((currentValue - min) / (max - min) + x, 0f, 1f) * (max - min) + min;
	}

	public static float HandleSliderVerticalInput(float currentValue, float min, float max, float deadZone = 0.2f, float sensitivity = 0.5f)
	{
		float num = 0f - PlayerInput.GamepadThumbstickLeft.Y;
		num = ((!(num < 0f - deadZone) && !(num > deadZone)) ? 0f : (MathHelper.Lerp(0f, sensitivity / 60f, (Math.Abs(num) - deadZone) / (1f - deadZone)) * (float)Math.Sign(num)));
		return MathHelper.Clamp((currentValue - min) / (max - min) + num, 0f, 1f) * (max - min) + min;
	}

	public static bool CanExecuteInputCommand()
	{
		return PlayerInput.AllowExecutionOfGamepadInstructions;
	}

	public static void Load()
	{
		Func<string> value = () => PlayerInput.BuildCommand(Lang.misc[53].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
		UILinkPage uILinkPage = new UILinkPage();
		uILinkPage.UpdateEvent += delegate
		{
			PlayerInput.GamepadAllowScrolling = true;
		};
		for (int i = 0; i < 20; i++)
		{
			uILinkPage.LinkMap.Add(2000 + i, new UILinkPoint(2000 + i, enabled: true, -3, -4, -1, -2));
		}
		uILinkPage.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[53].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]) + PlayerInput.BuildCommand(Lang.misc[82].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]);
		uILinkPage.UpdateEvent += delegate
		{
			bool flag11 = PlayerInput.Triggers.JustPressed.Inventory;
			if (Main.inputTextEscape)
			{
				Main.inputTextEscape = false;
				flag11 = true;
			}
			if (CanExecuteInputCommand() && flag11)
			{
				FancyExit();
			}
			UILinkPointNavigator.Shortcuts.BackButtonInUse = flag11;
			HandleOptionsSpecials();
		};
		uILinkPage.IsValidEvent += () => Main.gameMenu && !Main.MenuUI.IsVisible;
		uILinkPage.CanEnterEvent += () => Main.gameMenu && !Main.MenuUI.IsVisible;
		UILinkPointNavigator.RegisterPage(uILinkPage, 1000);
		UILinkPage cp13 = new UILinkPage();
		cp13.LinkMap.Add(2500, new UILinkPoint(2500, enabled: true, -3, 2501, -1, -2));
		cp13.LinkMap.Add(2501, new UILinkPoint(2501, enabled: true, 2500, 2502, -1, -2));
		cp13.LinkMap.Add(2502, new UILinkPoint(2502, enabled: true, 2501, 2503, -1, -2));
		cp13.LinkMap.Add(2503, new UILinkPoint(2503, enabled: true, 2502, -4, -1, -2));
		cp13.UpdateEvent += delegate
		{
			cp13.LinkMap[2501].Right = (UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsRight ? 2502 : (-4));
			if (cp13.LinkMap[2501].Right == -4 && UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsRight2)
			{
				cp13.LinkMap[2501].Right = 2503;
			}
			cp13.LinkMap[2502].Right = (UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsRight2 ? 2503 : (-4));
			cp13.LinkMap[2503].Left = (UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsRight ? 2502 : 2501);
		};
		cp13.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[53].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]) + PlayerInput.BuildCommand(Lang.misc[56].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]);
		cp13.IsValidEvent += () => (Main.player[Main.myPlayer].talkNPC != -1 || Main.player[Main.myPlayer].sign != -1) && NothingMoreImportantThanNPCChat();
		cp13.CanEnterEvent += () => (Main.player[Main.myPlayer].talkNPC != -1 || Main.player[Main.myPlayer].sign != -1) && NothingMoreImportantThanNPCChat();
		cp13.EnterEvent += delegate
		{
			Main.player[Main.myPlayer].releaseInventory = false;
		};
		cp13.LeaveEvent += delegate
		{
			Main.npcChatRelease = false;
			Main.player[Main.myPlayer].LockGamepadTileInteractions();
		};
		UILinkPointNavigator.RegisterPage(cp13, 1003);
		UILinkPage cp11 = new UILinkPage();
		cp11.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		Func<string> value12 = delegate
		{
			int currentPoint5 = UILinkPointNavigator.CurrentPoint;
			return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].inventory, 0, currentPoint5);
		};
		Func<string> value15 = () => ItemSlot.GetGamepadInstructions(ref Main.player[Main.myPlayer].trashItem, 6);
		for (int j = 0; j <= 49; j++)
		{
			UILinkPoint uILinkPoint = new UILinkPoint(j, enabled: true, j - 1, j + 1, j - 10, j + 10);
			uILinkPoint.OnSpecialInteracts += value12;
			int num37 = j;
			if (num37 < 10)
			{
				uILinkPoint.Up = -1;
			}
			if (num37 >= 40)
			{
				uILinkPoint.Down = -2;
			}
			if (num37 % 10 == 9)
			{
				uILinkPoint.Right = -4;
			}
			if (num37 % 10 == 0)
			{
				uILinkPoint.Left = -3;
			}
			cp11.LinkMap.Add(j, uILinkPoint);
		}
		cp11.LinkMap[9].Right = 0;
		cp11.LinkMap[19].Right = 50;
		cp11.LinkMap[29].Right = 51;
		cp11.LinkMap[39].Right = 52;
		cp11.LinkMap[49].Right = 53;
		cp11.LinkMap[0].Left = 9;
		cp11.LinkMap[10].Left = 54;
		cp11.LinkMap[20].Left = 55;
		cp11.LinkMap[30].Left = 56;
		cp11.LinkMap[40].Left = 57;
		cp11.LinkMap.Add(300, new UILinkPoint(300, enabled: true, 309, 310, 49, -2));
		cp11.LinkMap.Add(309, new UILinkPoint(309, enabled: true, 310, 300, 302, 54));
		cp11.LinkMap.Add(310, new UILinkPoint(310, enabled: true, 300, 309, 301, 50));
		cp11.LinkMap.Add(301, new UILinkPoint(301, enabled: true, 300, 302, 53, 50));
		cp11.LinkMap.Add(302, new UILinkPoint(302, enabled: true, 301, 310, 57, 54));
		cp11.LinkMap.Add(311, new UILinkPoint(311, enabled: true, -3, -4, 40, -2));
		cp11.LinkMap[301].OnSpecialInteracts += value;
		cp11.LinkMap[302].OnSpecialInteracts += value;
		cp11.LinkMap[309].OnSpecialInteracts += value;
		cp11.LinkMap[310].OnSpecialInteracts += value;
		cp11.LinkMap[300].OnSpecialInteracts += value15;
		cp11.UpdateEvent += delegate
		{
			bool inReforgeMenu = Main.InReforgeMenu;
			bool flag7 = Main.player[Main.myPlayer].chest != -1;
			bool flag8 = Main.npcShop != 0;
			TileEntity tileEntity = Main.LocalPlayer.tileEntityAnchor.GetTileEntity();
			bool flag9 = tileEntity is TEHatRack;
			bool flag10 = tileEntity is TEDisplayDoll;
			for (int num74 = 40; num74 <= 49; num74++)
			{
				if (inReforgeMenu)
				{
					cp11.LinkMap[num74].Down = ((num74 < 45) ? 303 : 304);
				}
				else if (flag7)
				{
					cp11.LinkMap[num74].Down = 400 + num74 - 40;
				}
				else if (flag8)
				{
					cp11.LinkMap[num74].Down = 2700 + num74 - 40;
				}
				else if (num74 == 40)
				{
					cp11.LinkMap[num74].Down = 311;
				}
				else
				{
					cp11.LinkMap[num74].Down = -2;
				}
			}
			if (flag10)
			{
				for (int num75 = 40; num75 <= 47; num75++)
				{
					cp11.LinkMap[num75].Down = 5100 + num75 - 40;
				}
			}
			if (flag9)
			{
				for (int num76 = 44; num76 <= 45; num76++)
				{
					cp11.LinkMap[num76].Down = 5000 + num76 - 44;
				}
			}
			if (flag7)
			{
				cp11.LinkMap[300].Up = 439;
				cp11.LinkMap[300].Right = -4;
				cp11.LinkMap[300].Left = 309;
				cp11.LinkMap[309].Up = 438;
				cp11.LinkMap[309].Right = 300;
				cp11.LinkMap[309].Left = 310;
				cp11.LinkMap[310].Up = 437;
				cp11.LinkMap[310].Right = 309;
				cp11.LinkMap[310].Left = -3;
			}
			else if (flag8)
			{
				cp11.LinkMap[300].Up = 2739;
				cp11.LinkMap[300].Right = -4;
				cp11.LinkMap[300].Left = 309;
				cp11.LinkMap[309].Up = 2738;
				cp11.LinkMap[309].Right = 300;
				cp11.LinkMap[309].Left = 310;
				cp11.LinkMap[310].Up = 2737;
				cp11.LinkMap[310].Right = 309;
				cp11.LinkMap[310].Left = -3;
			}
			else
			{
				cp11.LinkMap[49].Down = 300;
				cp11.LinkMap[48].Down = 309;
				cp11.LinkMap[47].Down = 310;
				cp11.LinkMap[300].Up = 49;
				cp11.LinkMap[300].Right = 301;
				cp11.LinkMap[300].Left = 309;
				cp11.LinkMap[309].Up = 48;
				cp11.LinkMap[309].Right = 300;
				cp11.LinkMap[309].Left = 310;
				cp11.LinkMap[310].Up = 47;
				cp11.LinkMap[310].Right = 309;
				cp11.LinkMap[310].Left = 302;
			}
			cp11.LinkMap[0].Left = 9;
			cp11.LinkMap[10].Left = 54;
			cp11.LinkMap[20].Left = 55;
			cp11.LinkMap[30].Left = 56;
			cp11.LinkMap[40].Left = 57;
			if (UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 0)
			{
				cp11.LinkMap[0].Left = 6000;
			}
			if (UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 2)
			{
				cp11.LinkMap[10].Left = 6002;
			}
			if (UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 4)
			{
				cp11.LinkMap[20].Left = 6004;
			}
			if (UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 6)
			{
				cp11.LinkMap[30].Left = 6006;
			}
			if (UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 8)
			{
				cp11.LinkMap[40].Left = 6008;
			}
			cp11.PageOnLeft = 9;
			if (Main.CreativeMenu.Enabled)
			{
				cp11.PageOnLeft = 1005;
			}
			if (Main.InReforgeMenu)
			{
				cp11.PageOnLeft = 5;
			}
		};
		cp11.IsValidEvent += () => Main.playerInventory;
		cp11.PageOnLeft = 9;
		cp11.PageOnRight = 2;
		UILinkPointNavigator.RegisterPage(cp11, 0);
		UILinkPage cp10 = new UILinkPage();
		cp10.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		Func<string> value16 = delegate
		{
			int currentPoint4 = UILinkPointNavigator.CurrentPoint;
			return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].inventory, 1, currentPoint4);
		};
		for (int k = 50; k <= 53; k++)
		{
			UILinkPoint uILinkPoint9 = new UILinkPoint(k, enabled: true, -3, -4, k - 1, k + 1);
			uILinkPoint9.OnSpecialInteracts += value16;
			cp10.LinkMap.Add(k, uILinkPoint9);
		}
		cp10.LinkMap[50].Left = 19;
		cp10.LinkMap[51].Left = 29;
		cp10.LinkMap[52].Left = 39;
		cp10.LinkMap[53].Left = 49;
		cp10.LinkMap[50].Right = 54;
		cp10.LinkMap[51].Right = 55;
		cp10.LinkMap[52].Right = 56;
		cp10.LinkMap[53].Right = 57;
		cp10.LinkMap[50].Up = -1;
		cp10.LinkMap[53].Down = -2;
		cp10.UpdateEvent += delegate
		{
			if (Main.player[Main.myPlayer].chest == -1 && Main.npcShop == 0)
			{
				cp10.LinkMap[50].Up = 301;
				cp10.LinkMap[53].Down = 301;
			}
			else
			{
				cp10.LinkMap[50].Up = 504;
				cp10.LinkMap[53].Down = 500;
			}
		};
		cp10.IsValidEvent += () => Main.playerInventory;
		cp10.PageOnLeft = 0;
		cp10.PageOnRight = 2;
		UILinkPointNavigator.RegisterPage(cp10, 1);
		UILinkPage cp9 = new UILinkPage();
		cp9.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		Func<string> value17 = delegate
		{
			int currentPoint3 = UILinkPointNavigator.CurrentPoint;
			return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].inventory, 2, currentPoint3);
		};
		for (int l = 54; l <= 57; l++)
		{
			UILinkPoint uILinkPoint10 = new UILinkPoint(l, enabled: true, -3, -4, l - 1, l + 1);
			uILinkPoint10.OnSpecialInteracts += value17;
			cp9.LinkMap.Add(l, uILinkPoint10);
		}
		cp9.LinkMap[54].Left = 50;
		cp9.LinkMap[55].Left = 51;
		cp9.LinkMap[56].Left = 52;
		cp9.LinkMap[57].Left = 53;
		cp9.LinkMap[54].Right = 10;
		cp9.LinkMap[55].Right = 20;
		cp9.LinkMap[56].Right = 30;
		cp9.LinkMap[57].Right = 40;
		cp9.LinkMap[54].Up = -1;
		cp9.LinkMap[57].Down = -2;
		cp9.UpdateEvent += delegate
		{
			if (Main.player[Main.myPlayer].chest == -1 && Main.npcShop == 0)
			{
				cp9.LinkMap[54].Up = 302;
				cp9.LinkMap[57].Down = 302;
			}
			else
			{
				cp9.LinkMap[54].Up = 504;
				cp9.LinkMap[57].Down = 500;
			}
		};
		cp9.PageOnLeft = 0;
		cp9.PageOnRight = 8;
		UILinkPointNavigator.RegisterPage(cp9, 2);
		UILinkPage cp8 = new UILinkPage();
		cp8.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		Func<string> value18 = delegate
		{
			int num73 = UILinkPointNavigator.CurrentPoint - 100;
			return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].armor, (num73 < 10) ? 8 : 9, num73);
		};
		Func<string> value19 = delegate
		{
			int slot11 = UILinkPointNavigator.CurrentPoint - 120;
			return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].dye, 12, slot11);
		};
		for (int m = 100; m <= 119; m++)
		{
			UILinkPoint uILinkPoint13 = new UILinkPoint(m, enabled: true, m + 10, m - 10, m - 1, m + 1);
			uILinkPoint13.OnSpecialInteracts += value18;
			int num18 = m - 100;
			if (num18 == 0)
			{
				uILinkPoint13.Up = 305;
			}
			if (num18 == 10)
			{
				uILinkPoint13.Up = 306;
			}
			if (num18 == 9 || num18 == 19)
			{
				uILinkPoint13.Down = -2;
			}
			if (num18 >= 10)
			{
				uILinkPoint13.Left = 120 + num18 % 10;
			}
			else if (num18 >= 3)
			{
				uILinkPoint13.Right = -4;
			}
			else
			{
				uILinkPoint13.Right = 312 + num18;
			}
			cp8.LinkMap.Add(m, uILinkPoint13);
		}
		for (int n = 120; n <= 129; n++)
		{
			UILinkPoint uILinkPoint12 = new UILinkPoint(n, enabled: true, -3, -10 + n, n - 1, n + 1);
			uILinkPoint12.OnSpecialInteracts += value19;
			int num38 = n - 120;
			if (num38 == 0)
			{
				uILinkPoint12.Up = 307;
			}
			if (num38 == 9)
			{
				uILinkPoint12.Down = 308;
				uILinkPoint12.Left = 1557;
			}
			if (num38 == 8)
			{
				uILinkPoint12.Left = 1570;
			}
			cp8.LinkMap.Add(n, uILinkPoint12);
		}
		for (int num31 = 312; num31 <= 314; num31++)
		{
			int num32 = num31 - 312;
			UILinkPoint uILinkPoint11 = new UILinkPoint(num31, enabled: true, 100 + num32, -4, num31 - 1, num31 + 1);
			if (num32 == 0)
			{
				uILinkPoint11.Up = -1;
			}
			if (num32 == 2)
			{
				uILinkPoint11.Down = -2;
			}
			uILinkPoint11.OnSpecialInteracts += value;
			cp8.LinkMap.Add(num31, uILinkPoint11);
		}
		cp8.IsValidEvent += () => Main.playerInventory && Main.EquipPage == 0;
		cp8.UpdateEvent += delegate
		{
			int num69 = 107;
			int amountOfExtraAccessorySlotsToShow = Main.player[Main.myPlayer].GetAmountOfExtraAccessorySlotsToShow();
			for (int num70 = 0; num70 < amountOfExtraAccessorySlotsToShow; num70++)
			{
				cp8.LinkMap[num69 + num70].Down = num69 + num70 + 1;
				cp8.LinkMap[num69 - 100 + 120 + num70].Down = num69 - 100 + 120 + num70 + 1;
				cp8.LinkMap[num69 + 10 + num70].Down = num69 + 10 + num70 + 1;
			}
			cp8.LinkMap[num69 + amountOfExtraAccessorySlotsToShow].Down = 308;
			cp8.LinkMap[num69 + 10 + amountOfExtraAccessorySlotsToShow].Down = 308;
			cp8.LinkMap[num69 - 100 + 120 + amountOfExtraAccessorySlotsToShow].Down = 308;
			bool shouldPVPDraw = Main.ShouldPVPDraw;
			for (int num71 = 120; num71 <= 129; num71++)
			{
				UILinkPoint uILinkPoint20 = cp8.LinkMap[num71];
				int num72 = num71 - 120;
				uILinkPoint20.Left = -3;
				if (num72 == 0)
				{
					uILinkPoint20.Left = (shouldPVPDraw ? 1550 : (-3));
				}
				if (num72 == 1)
				{
					uILinkPoint20.Left = (shouldPVPDraw ? 1552 : (-3));
				}
				if (num72 == 2)
				{
					uILinkPoint20.Left = (shouldPVPDraw ? 1556 : (-3));
				}
				if (num72 == 3)
				{
					uILinkPoint20.Left = ((UILinkPointNavigator.Shortcuts.INFOACCCOUNT >= 1) ? 1558 : (-3));
				}
				if (num72 == 4)
				{
					uILinkPoint20.Left = ((UILinkPointNavigator.Shortcuts.INFOACCCOUNT >= 5) ? 1562 : (-3));
				}
				if (num72 == 5)
				{
					uILinkPoint20.Left = ((UILinkPointNavigator.Shortcuts.INFOACCCOUNT >= 9) ? 1566 : (-3));
				}
			}
			cp8.LinkMap[num69 - 100 + 120 + amountOfExtraAccessorySlotsToShow].Left = 1557;
			cp8.LinkMap[num69 - 100 + 120 + amountOfExtraAccessorySlotsToShow - 1].Left = 1570;
		};
		cp8.PageOnLeft = 8;
		cp8.PageOnRight = 8;
		UILinkPointNavigator.RegisterPage(cp8, 3);
		UILinkPage uILinkPage2 = new UILinkPage();
		uILinkPage2.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		Func<string> value20 = delegate
		{
			int slot10 = UILinkPointNavigator.CurrentPoint - 400;
			int context = 4;
			Item[] item = Main.player[Main.myPlayer].bank.item;
			switch (Main.player[Main.myPlayer].chest)
			{
			case -1:
				return "";
			case -3:
				item = Main.player[Main.myPlayer].bank2.item;
				break;
			case -4:
				item = Main.player[Main.myPlayer].bank3.item;
				break;
			case -5:
				item = Main.player[Main.myPlayer].bank4.item;
				context = 32;
				break;
			default:
				item = Main.chest[Main.player[Main.myPlayer].chest].item;
				context = 3;
				break;
			case -2:
				break;
			}
			return ItemSlot.GetGamepadInstructions(item, context, slot10);
		};
		for (int num33 = 400; num33 <= 439; num33++)
		{
			UILinkPoint uILinkPoint14 = new UILinkPoint(num33, enabled: true, num33 - 1, num33 + 1, num33 - 10, num33 + 10);
			uILinkPoint14.OnSpecialInteracts += value20;
			int num34 = num33 - 400;
			if (num34 < 10)
			{
				uILinkPoint14.Up = 40 + num34;
			}
			if (num34 >= 30)
			{
				uILinkPoint14.Down = -2;
			}
			if (num34 % 10 == 9)
			{
				uILinkPoint14.Right = -4;
			}
			if (num34 % 10 == 0)
			{
				uILinkPoint14.Left = -3;
			}
			uILinkPage2.LinkMap.Add(num33, uILinkPoint14);
		}
		uILinkPage2.LinkMap.Add(500, new UILinkPoint(500, enabled: true, 409, -4, 53, 501));
		uILinkPage2.LinkMap.Add(501, new UILinkPoint(501, enabled: true, 419, -4, 500, 502));
		uILinkPage2.LinkMap.Add(502, new UILinkPoint(502, enabled: true, 429, -4, 501, 503));
		uILinkPage2.LinkMap.Add(503, new UILinkPoint(503, enabled: true, 439, -4, 502, 505));
		uILinkPage2.LinkMap.Add(505, new UILinkPoint(505, enabled: true, 439, -4, 503, 504));
		uILinkPage2.LinkMap.Add(504, new UILinkPoint(504, enabled: true, 439, -4, 505, 50));
		uILinkPage2.LinkMap.Add(506, new UILinkPoint(506, enabled: true, 439, -4, 505, 50));
		uILinkPage2.LinkMap[500].OnSpecialInteracts += value;
		uILinkPage2.LinkMap[501].OnSpecialInteracts += value;
		uILinkPage2.LinkMap[502].OnSpecialInteracts += value;
		uILinkPage2.LinkMap[503].OnSpecialInteracts += value;
		uILinkPage2.LinkMap[504].OnSpecialInteracts += value;
		uILinkPage2.LinkMap[505].OnSpecialInteracts += value;
		uILinkPage2.LinkMap[506].OnSpecialInteracts += value;
		uILinkPage2.LinkMap[409].Right = 500;
		uILinkPage2.LinkMap[419].Right = 501;
		uILinkPage2.LinkMap[429].Right = 502;
		uILinkPage2.LinkMap[439].Right = 503;
		uILinkPage2.LinkMap[439].Down = 300;
		uILinkPage2.LinkMap[438].Down = 309;
		uILinkPage2.LinkMap[437].Down = 310;
		uILinkPage2.PageOnLeft = 0;
		uILinkPage2.PageOnRight = 0;
		uILinkPage2.DefaultPoint = 400;
		UILinkPointNavigator.RegisterPage(uILinkPage2, 4, automatedDefault: false);
		uILinkPage2.IsValidEvent += () => Main.playerInventory && Main.player[Main.myPlayer].chest != -1;
		UILinkPage uILinkPage3 = new UILinkPage();
		uILinkPage3.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		Func<string> value21 = delegate
		{
			int slot9 = UILinkPointNavigator.CurrentPoint - 5100;
			return (Main.LocalPlayer.tileEntityAnchor.GetTileEntity() is TEDisplayDoll tEDisplayDoll) ? tEDisplayDoll.GetItemGamepadInstructions(slot9) : "";
		};
		for (int num35 = 5100; num35 <= 5115; num35++)
		{
			UILinkPoint uILinkPoint15 = new UILinkPoint(num35, enabled: true, num35 - 1, num35 + 1, num35 - 8, num35 + 8);
			uILinkPoint15.OnSpecialInteracts += value21;
			int num36 = num35 - 5100;
			if (num36 < 8)
			{
				uILinkPoint15.Up = 40 + num36;
			}
			if (num36 >= 8)
			{
				uILinkPoint15.Down = -2;
			}
			if (num36 % 8 == 7)
			{
				uILinkPoint15.Right = -4;
			}
			if (num36 % 8 == 0)
			{
				uILinkPoint15.Left = -3;
			}
			uILinkPage3.LinkMap.Add(num35, uILinkPoint15);
		}
		uILinkPage3.PageOnLeft = 0;
		uILinkPage3.PageOnRight = 0;
		uILinkPage3.DefaultPoint = 5100;
		UILinkPointNavigator.RegisterPage(uILinkPage3, 20, automatedDefault: false);
		uILinkPage3.IsValidEvent += () => Main.playerInventory && Main.LocalPlayer.tileEntityAnchor.GetTileEntity() is TEDisplayDoll;
		UILinkPage uILinkPage4 = new UILinkPage();
		uILinkPage4.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		Func<string> value2 = delegate
		{
			int slot8 = UILinkPointNavigator.CurrentPoint - 5000;
			return (Main.LocalPlayer.tileEntityAnchor.GetTileEntity() is TEHatRack tEHatRack) ? tEHatRack.GetItemGamepadInstructions(slot8) : "";
		};
		for (int num10 = 5000; num10 <= 5003; num10++)
		{
			UILinkPoint uILinkPoint16 = new UILinkPoint(num10, enabled: true, num10 - 1, num10 + 1, num10 - 2, num10 + 2);
			uILinkPoint16.OnSpecialInteracts += value2;
			int num11 = num10 - 5000;
			if (num11 < 2)
			{
				uILinkPoint16.Up = 44 + num11;
			}
			if (num11 >= 2)
			{
				uILinkPoint16.Down = -2;
			}
			if (num11 % 2 == 1)
			{
				uILinkPoint16.Right = -4;
			}
			if (num11 % 2 == 0)
			{
				uILinkPoint16.Left = -3;
			}
			uILinkPage4.LinkMap.Add(num10, uILinkPoint16);
		}
		uILinkPage4.PageOnLeft = 0;
		uILinkPage4.PageOnRight = 0;
		uILinkPage4.DefaultPoint = 5000;
		UILinkPointNavigator.RegisterPage(uILinkPage4, 21, automatedDefault: false);
		uILinkPage4.IsValidEvent += () => Main.playerInventory && Main.LocalPlayer.tileEntityAnchor.GetTileEntity() is TEHatRack;
		UILinkPage uILinkPage5 = new UILinkPage();
		uILinkPage5.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		Func<string> value3 = delegate
		{
			if (Main.npcShop == 0)
			{
				return "";
			}
			int slot7 = UILinkPointNavigator.CurrentPoint - 2700;
			return ItemSlot.GetGamepadInstructions(Main.instance.shop[Main.npcShop].item, 15, slot7);
		};
		for (int num12 = 2700; num12 <= 2739; num12++)
		{
			UILinkPoint uILinkPoint17 = new UILinkPoint(num12, enabled: true, num12 - 1, num12 + 1, num12 - 10, num12 + 10);
			uILinkPoint17.OnSpecialInteracts += value3;
			int num13 = num12 - 2700;
			if (num13 < 10)
			{
				uILinkPoint17.Up = 40 + num13;
			}
			if (num13 >= 30)
			{
				uILinkPoint17.Down = -2;
			}
			if (num13 % 10 == 9)
			{
				uILinkPoint17.Right = -4;
			}
			if (num13 % 10 == 0)
			{
				uILinkPoint17.Left = -3;
			}
			uILinkPage5.LinkMap.Add(num12, uILinkPoint17);
		}
		uILinkPage5.LinkMap[2739].Down = 300;
		uILinkPage5.LinkMap[2738].Down = 309;
		uILinkPage5.LinkMap[2737].Down = 310;
		uILinkPage5.PageOnLeft = 0;
		uILinkPage5.PageOnRight = 0;
		UILinkPointNavigator.RegisterPage(uILinkPage5, 13);
		uILinkPage5.IsValidEvent += () => Main.playerInventory && Main.npcShop != 0;
		UILinkPage cp7 = new UILinkPage();
		cp7.LinkMap.Add(303, new UILinkPoint(303, enabled: true, 304, 304, 40, -2));
		cp7.LinkMap.Add(304, new UILinkPoint(304, enabled: true, 303, 303, 40, -2));
		cp7.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		Func<string> value4 = () => ItemSlot.GetGamepadInstructions(ref Main.reforgeItem, 5);
		cp7.LinkMap[303].OnSpecialInteracts += value4;
		cp7.LinkMap[304].OnSpecialInteracts += () => Lang.misc[53].Value;
		cp7.UpdateEvent += delegate
		{
			if (Main.reforgeItem.type > 0)
			{
				cp7.LinkMap[303].Left = (cp7.LinkMap[303].Right = 304);
			}
			else
			{
				if (UILinkPointNavigator.OverridePoint == -1 && cp7.CurrentPoint == 304)
				{
					UILinkPointNavigator.ChangePoint(303);
				}
				cp7.LinkMap[303].Left = -3;
				cp7.LinkMap[303].Right = -4;
			}
		};
		cp7.IsValidEvent += () => Main.playerInventory && Main.InReforgeMenu;
		cp7.PageOnLeft = 0;
		cp7.PageOnRight = 0;
		cp7.EnterEvent += delegate
		{
			PlayerInput.LockGamepadButtons("MouseLeft");
		};
		UILinkPointNavigator.RegisterPage(cp7, 5);
		UILinkPage cp6 = new UILinkPage();
		cp6.OnSpecialInteracts += delegate
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			bool flag5 = UILinkPointNavigator.CurrentPoint == 600;
			bool flag6 = !flag5 && WorldGen.IsNPCEvictable(UILinkPointNavigator.Shortcuts.NPCS_LastHovered);
			if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.Grapple)
			{
				Point val3 = Main.player[Main.myPlayer].Center.ToTileCoordinates();
				if (flag5)
				{
					if (WorldGen.MoveTownNPC(val3.X, val3.Y, -1))
					{
						Main.NewText(Lang.inter[39].Value, byte.MaxValue, 240, 20);
					}
					SoundEngine.PlaySound(12);
				}
				else if (WorldGen.MoveTownNPC(val3.X, val3.Y, UILinkPointNavigator.Shortcuts.NPCS_LastHovered))
				{
					WorldGen.moveRoom(val3.X, val3.Y, UILinkPointNavigator.Shortcuts.NPCS_LastHovered);
					SoundEngine.PlaySound(12);
				}
				PlayerInput.LockGamepadButtons("Grapple");
				PlayerInput.SettingsForUI.TryRevertingToMouseMode();
			}
			if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.SmartSelect)
			{
				UILinkPointNavigator.Shortcuts.NPCS_IconsDisplay = !UILinkPointNavigator.Shortcuts.NPCS_IconsDisplay;
				PlayerInput.LockGamepadButtons("SmartSelect");
				PlayerInput.SettingsForUI.TryRevertingToMouseMode();
			}
			if (flag6 && CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.MouseRight)
			{
				WorldGen.kickOut(UILinkPointNavigator.Shortcuts.NPCS_LastHovered);
			}
			return PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]) + PlayerInput.BuildCommand(Lang.misc[70].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]) + PlayerInput.BuildCommand(Lang.misc[69].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["SmartSelect"]) + (flag6 ? PlayerInput.BuildCommand("Evict", false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]) : "");
		};
		for (int num14 = 600; num14 <= 698; num14++)
		{
			UILinkPoint value5 = new UILinkPoint(num14, enabled: true, num14 + 10, num14 - 10, num14 - 1, num14 + 1);
			cp6.LinkMap.Add(num14, value5);
		}
		cp6.UpdateEvent += delegate
		{
			int num67 = UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn;
			if (num67 == 0)
			{
				num67 = 100;
			}
			for (int num68 = 0; num68 < 98; num68++)
			{
				cp6.LinkMap[600 + num68].Up = ((num68 % num67 == 0) ? (-1) : (600 + num68 - 1));
				if (cp6.LinkMap[600 + num68].Up == -1)
				{
					if (num68 >= num67 * 2)
					{
						cp6.LinkMap[600 + num68].Up = 307;
					}
					else if (num68 >= num67)
					{
						cp6.LinkMap[600 + num68].Up = 306;
					}
					else
					{
						cp6.LinkMap[600 + num68].Up = 305;
					}
				}
				cp6.LinkMap[600 + num68].Down = (((num68 + 1) % num67 == 0 || num68 == UILinkPointNavigator.Shortcuts.NPCS_IconsTotal - 1) ? 308 : (600 + num68 + 1));
				cp6.LinkMap[600 + num68].Left = ((num68 < UILinkPointNavigator.Shortcuts.NPCS_IconsTotal - num67) ? (600 + num68 + num67) : (-3));
				cp6.LinkMap[600 + num68].Right = ((num68 < num67) ? (-4) : (600 + num68 - num67));
			}
		};
		cp6.IsValidEvent += () => Main.playerInventory && Main.EquipPage == 1;
		cp6.PageOnLeft = 8;
		cp6.PageOnRight = 8;
		UILinkPointNavigator.RegisterPage(cp6, 6);
		UILinkPage cp5 = new UILinkPage();
		cp5.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		Func<string> value6 = delegate
		{
			int slot6 = UILinkPointNavigator.CurrentPoint - 180;
			return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscEquips, 20, slot6);
		};
		Func<string> value7 = delegate
		{
			int slot5 = UILinkPointNavigator.CurrentPoint - 180;
			return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscEquips, 19, slot5);
		};
		Func<string> value8 = delegate
		{
			int slot4 = UILinkPointNavigator.CurrentPoint - 180;
			return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscEquips, 18, slot4);
		};
		Func<string> value9 = delegate
		{
			int slot3 = UILinkPointNavigator.CurrentPoint - 180;
			return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscEquips, 17, slot3);
		};
		Func<string> value10 = delegate
		{
			int slot2 = UILinkPointNavigator.CurrentPoint - 180;
			return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscEquips, 16, slot2);
		};
		Func<string> value11 = delegate
		{
			int slot = UILinkPointNavigator.CurrentPoint - 185;
			return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscDyes, 33, slot);
		};
		for (int num15 = 180; num15 <= 184; num15++)
		{
			UILinkPoint uILinkPoint19 = new UILinkPoint(num15, enabled: true, 185 + num15 - 180, -4, num15 - 1, num15 + 1);
			int num39 = num15 - 180;
			if (num39 == 0)
			{
				uILinkPoint19.Up = 305;
			}
			if (num39 == 4)
			{
				uILinkPoint19.Down = 308;
			}
			cp5.LinkMap.Add(num15, uILinkPoint19);
			switch (num15)
			{
			case 180:
				uILinkPoint19.OnSpecialInteracts += value7;
				break;
			case 181:
				uILinkPoint19.OnSpecialInteracts += value6;
				break;
			case 182:
				uILinkPoint19.OnSpecialInteracts += value8;
				break;
			case 183:
				uILinkPoint19.OnSpecialInteracts += value9;
				break;
			case 184:
				uILinkPoint19.OnSpecialInteracts += value10;
				break;
			}
		}
		for (int num16 = 185; num16 <= 189; num16++)
		{
			UILinkPoint uILinkPoint18 = new UILinkPoint(num16, enabled: true, -3, -5 + num16, num16 - 1, num16 + 1);
			uILinkPoint18.OnSpecialInteracts += value11;
			int num40 = num16 - 185;
			if (num40 == 0)
			{
				uILinkPoint18.Up = 306;
			}
			if (num40 == 4)
			{
				uILinkPoint18.Down = 308;
			}
			cp5.LinkMap.Add(num16, uILinkPoint18);
		}
		cp5.UpdateEvent += delegate
		{
			cp5.LinkMap[184].Down = ((UILinkPointNavigator.Shortcuts.BUFFS_DRAWN > 0) ? 9000 : 308);
			cp5.LinkMap[189].Down = ((UILinkPointNavigator.Shortcuts.BUFFS_DRAWN > 0) ? 9000 : 308);
		};
		cp5.IsValidEvent += () => Main.playerInventory && Main.EquipPage == 2;
		cp5.PageOnLeft = 8;
		cp5.PageOnRight = 8;
		UILinkPointNavigator.RegisterPage(cp5, 7);
		UILinkPage cp4 = new UILinkPage();
		cp4.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		cp4.LinkMap.Add(305, new UILinkPoint(305, enabled: true, 306, -4, 308, -2));
		cp4.LinkMap.Add(306, new UILinkPoint(306, enabled: true, 307, 305, 308, -2));
		cp4.LinkMap.Add(307, new UILinkPoint(307, enabled: true, -3, 306, 308, -2));
		cp4.LinkMap.Add(308, new UILinkPoint(308, enabled: true, -3, -4, -1, 305));
		cp4.LinkMap[305].OnSpecialInteracts += value;
		cp4.LinkMap[306].OnSpecialInteracts += value;
		cp4.LinkMap[307].OnSpecialInteracts += value;
		cp4.LinkMap[308].OnSpecialInteracts += value;
		cp4.UpdateEvent += delegate
		{
			switch (Main.EquipPage)
			{
			case 0:
				cp4.LinkMap[305].Down = 100;
				cp4.LinkMap[306].Down = 110;
				cp4.LinkMap[307].Down = 120;
				cp4.LinkMap[308].Up = 108 + Main.player[Main.myPlayer].GetAmountOfExtraAccessorySlotsToShow() - 1;
				break;
			case 1:
			{
				cp4.LinkMap[305].Down = 600;
				cp4.LinkMap[306].Down = ((UILinkPointNavigator.Shortcuts.NPCS_IconsTotal / UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn > 0) ? (600 + UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn) : (-2));
				cp4.LinkMap[307].Down = ((UILinkPointNavigator.Shortcuts.NPCS_IconsTotal / UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn > 1) ? (600 + UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn * 2) : (-2));
				int num66 = UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn;
				if (num66 == 0)
				{
					num66 = 100;
				}
				if (num66 == 100)
				{
					num66 = UILinkPointNavigator.Shortcuts.NPCS_IconsTotal;
				}
				cp4.LinkMap[308].Up = 600 + num66 - 1;
				break;
			}
			case 2:
				cp4.LinkMap[305].Down = 180;
				cp4.LinkMap[306].Down = 185;
				cp4.LinkMap[307].Down = -2;
				cp4.LinkMap[308].Up = ((UILinkPointNavigator.Shortcuts.BUFFS_DRAWN > 0) ? 9000 : 184);
				break;
			}
			cp4.PageOnRight = GetCornerWrapPageIdFromRightToLeft();
		};
		cp4.IsValidEvent += () => Main.playerInventory;
		cp4.PageOnLeft = 0;
		cp4.PageOnRight = 0;
		UILinkPointNavigator.RegisterPage(cp4, 8);
		UILinkPage cp3 = new UILinkPage();
		cp3.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		Func<string> value13 = () => ItemSlot.GetGamepadInstructions(ref Main.guideItem, 7);
		Func<string> HandleItem2 = () => (Main.mouseItem.type >= 1) ? ItemSlot.GetGamepadInstructions(ref Main.mouseItem, 22) : "";
		for (int num17 = 1500; num17 < 1550; num17++)
		{
			UILinkPoint uILinkPoint2 = new UILinkPoint(num17, enabled: true, num17, num17, -1, -2);
			if (num17 != 1500)
			{
				uILinkPoint2.OnSpecialInteracts += HandleItem2;
			}
			cp3.LinkMap.Add(num17, uILinkPoint2);
		}
		cp3.LinkMap[1500].OnSpecialInteracts += value13;
		cp3.UpdateEvent += delegate
		{
			int num63 = UILinkPointNavigator.Shortcuts.CRAFT_CurrentIngredientsCount;
			int num64 = num63;
			if (Main.numAvailableRecipes > 0)
			{
				num64 += 2;
			}
			if (num63 < num64)
			{
				num63 = num64;
			}
			if (UILinkPointNavigator.OverridePoint == -1 && cp3.CurrentPoint > 1500 + num63)
			{
				UILinkPointNavigator.ChangePoint(1500);
			}
			if (UILinkPointNavigator.OverridePoint == -1 && cp3.CurrentPoint == 1500 && !Main.InGuideCraftMenu)
			{
				UILinkPointNavigator.ChangePoint(1501);
			}
			for (int num65 = 1; num65 < num63; num65++)
			{
				cp3.LinkMap[1500 + num65].Left = 1500 + num65 - 1;
				cp3.LinkMap[1500 + num65].Right = ((num65 == num63 - 2) ? (-4) : (1500 + num65 + 1));
			}
			cp3.LinkMap[1501].Left = -3;
			if (num63 > 0)
			{
				cp3.LinkMap[1500 + num63 - 1].Right = -4;
			}
			cp3.LinkMap[1500].Down = ((num63 >= 2) ? 1502 : (-2));
			cp3.LinkMap[1500].Left = ((num63 >= 1) ? 1501 : (-3));
			cp3.LinkMap[1502].Up = (Main.InGuideCraftMenu ? 1500 : (-1));
		};
		cp3.LinkMap[1501].OnSpecialInteracts += delegate
		{
			if (Main.InGuideCraftMenu)
			{
				return "";
			}
			string text2 = "";
			Player player2 = Main.player[Main.myPlayer];
			bool flag3 = false;
			Item createItem = Main.recipe[Main.availableRecipe[Main.focusRecipe]].createItem;
			if (Main.mouseItem.type == 0 && createItem.maxStack > 1 && player2.ItemSpace(createItem).CanTakeItemToPersonalInventory && !player2.HasLockedInventory())
			{
				flag3 = true;
				if (CanExecuteInputCommand() && PlayerInput.Triggers.Current.Grapple && Main.stackSplit <= 1)
				{
					if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.Grapple)
					{
						SomeVarsForUILinkers.SequencedCraftingCurrent = Main.recipe[Main.availableRecipe[Main.focusRecipe]];
					}
					ItemSlot.RefreshStackSplitCooldown();
					Main.preventStackSplitReset = true;
					if (SomeVarsForUILinkers.SequencedCraftingCurrent == Main.recipe[Main.availableRecipe[Main.focusRecipe]])
					{
						Main.CraftItem(Main.recipe[Main.availableRecipe[Main.focusRecipe]]);
						Main.mouseItem = player2.GetItem(player2.whoAmI, Main.mouseItem, new GetItemSettings(LongText: false, NoText: true));
					}
				}
			}
			else if (Main.mouseItem.type > 0 && Main.mouseItem.maxStack == 1 && ItemSlot.Equippable(ref Main.mouseItem))
			{
				text2 += PlayerInput.BuildCommand(Lang.misc[67].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
				if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.Grapple)
				{
					ItemSlot.SwapEquip(ref Main.mouseItem);
					if (Main.player[Main.myPlayer].ItemSpace(Main.mouseItem).CanTakeItemToPersonalInventory)
					{
						Main.mouseItem = player2.GetItem(player2.whoAmI, Main.mouseItem, GetItemSettings.InventoryUIToInventorySettings);
					}
					PlayerInput.LockGamepadButtons("Grapple");
					PlayerInput.SettingsForUI.TryRevertingToMouseMode();
				}
			}
			bool flag4 = Main.mouseItem.stack <= 0;
			if (flag4 || (Main.mouseItem.type == createItem.type && Main.mouseItem.stack < Main.mouseItem.maxStack && ItemLoader.CanStack(Main.mouseItem, createItem)))
			{
				text2 = ((!flag4) ? (text2 + PlayerInput.BuildCommand(Lang.misc[72].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"])) : (text2 + PlayerInput.BuildCommand(Lang.misc[72].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"], PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"])));
			}
			if (!flag4 && Main.mouseItem.type == createItem.type && Main.mouseItem.stack < Main.mouseItem.maxStack && ItemLoader.CanStack(Main.mouseItem, createItem))
			{
				text2 += PlayerInput.BuildCommand(Lang.misc[93].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
			}
			if (flag3)
			{
				text2 += PlayerInput.BuildCommand(Lang.misc[71].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
			}
			return text2 + HandleItem2();
		};
		cp3.ReachEndEvent += delegate(int current, int next)
		{
			switch (current)
			{
			case 1501:
				switch (next)
				{
				case -1:
					if (Main.focusRecipe > 0)
					{
						Main.focusRecipe--;
					}
					break;
				case -2:
					if (Main.focusRecipe < Main.numAvailableRecipes - 1)
					{
						Main.focusRecipe++;
					}
					break;
				}
				break;
			default:
				switch (next)
				{
				case -1:
					if (Main.focusRecipe > 0)
					{
						UILinkPointNavigator.ChangePoint(1501);
						Main.focusRecipe--;
					}
					break;
				case -2:
					if (Main.focusRecipe < Main.numAvailableRecipes - 1)
					{
						UILinkPointNavigator.ChangePoint(1501);
						Main.focusRecipe++;
					}
					break;
				}
				break;
			case 1500:
				break;
			}
		};
		cp3.EnterEvent += delegate
		{
			Main.recBigList = false;
			PlayerInput.LockGamepadButtons("MouseLeft");
		};
		cp3.CanEnterEvent += () => Main.playerInventory && (Main.numAvailableRecipes > 0 || Main.InGuideCraftMenu);
		cp3.IsValidEvent += () => Main.playerInventory && (Main.numAvailableRecipes > 0 || Main.InGuideCraftMenu);
		cp3.PageOnLeft = 10;
		cp3.PageOnRight = 0;
		UILinkPointNavigator.RegisterPage(cp3, 9);
		UILinkPage cp2 = new UILinkPage();
		cp2.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		for (int num19 = 700; num19 < 1500; num19++)
		{
			UILinkPoint uILinkPoint3 = new UILinkPoint(num19, enabled: true, num19, num19, num19, num19);
			int IHateLambda = num19;
			uILinkPoint3.OnSpecialInteracts += delegate
			{
				string text = "";
				bool flag2 = false;
				Player player = Main.player[Main.myPlayer];
				if (IHateLambda + Main.recStart < Main.numAvailableRecipes)
				{
					int num62 = Main.recStart + IHateLambda - 700;
					if (Main.mouseItem.type == 0 && Main.recipe[Main.availableRecipe[num62]].createItem.maxStack > 1 && player.ItemSpace(Main.recipe[Main.availableRecipe[num62]].createItem).CanTakeItemToPersonalInventory && !player.HasLockedInventory())
					{
						flag2 = true;
						if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.Grapple)
						{
							SomeVarsForUILinkers.SequencedCraftingCurrent = Main.recipe[Main.availableRecipe[num62]];
						}
						if (CanExecuteInputCommand() && PlayerInput.Triggers.Current.Grapple && Main.stackSplit <= 1)
						{
							ItemSlot.RefreshStackSplitCooldown();
							if (SomeVarsForUILinkers.SequencedCraftingCurrent == Main.recipe[Main.availableRecipe[num62]])
							{
								Main.CraftItem(Main.recipe[Main.availableRecipe[num62]]);
								Main.mouseItem = player.GetItem(player.whoAmI, Main.mouseItem, GetItemSettings.InventoryUIToInventorySettings);
							}
						}
					}
				}
				text += PlayerInput.BuildCommand(Lang.misc[73].Value, !flag2, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
				if (flag2)
				{
					text += PlayerInput.BuildCommand(Lang.misc[71].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
				}
				return text;
			};
			cp2.LinkMap.Add(num19, uILinkPoint3);
		}
		cp2.UpdateEvent += delegate
		{
			int num59 = UILinkPointNavigator.Shortcuts.CRAFT_IconsPerRow;
			int cRAFT_IconsPerColumn = UILinkPointNavigator.Shortcuts.CRAFT_IconsPerColumn;
			if (num59 == 0)
			{
				num59 = 100;
			}
			int num60 = num59 * cRAFT_IconsPerColumn;
			if (num60 > 800)
			{
				num60 = 800;
			}
			if (num60 > Main.numAvailableRecipes)
			{
				num60 = Main.numAvailableRecipes;
			}
			for (int num61 = 0; num61 < num60; num61++)
			{
				cp2.LinkMap[700 + num61].Left = ((num61 % num59 == 0) ? (-3) : (700 + num61 - 1));
				cp2.LinkMap[700 + num61].Right = (((num61 + 1) % num59 == 0 || num61 == Main.numAvailableRecipes - 1) ? (-4) : (700 + num61 + 1));
				cp2.LinkMap[700 + num61].Down = ((num61 < num60 - num59) ? (700 + num61 + num59) : (-2));
				cp2.LinkMap[700 + num61].Up = ((num61 < num59) ? (-1) : (700 + num61 - num59));
			}
			cp2.PageOnLeft = GetCornerWrapPageIdFromLeftToRight();
		};
		cp2.ReachEndEvent += delegate(int current, int next)
		{
			int cRAFT_IconsPerRow = UILinkPointNavigator.Shortcuts.CRAFT_IconsPerRow;
			switch (next)
			{
			case -1:
				Main.recStart -= cRAFT_IconsPerRow;
				if (Main.recStart < 0)
				{
					Main.recStart = 0;
				}
				break;
			case -2:
				Main.recStart += cRAFT_IconsPerRow;
				SoundEngine.PlaySound(12);
				if (Main.recStart > Main.numAvailableRecipes - cRAFT_IconsPerRow)
				{
					Main.recStart = Main.numAvailableRecipes - cRAFT_IconsPerRow;
				}
				break;
			}
		};
		cp2.EnterEvent += delegate
		{
			Main.recBigList = true;
		};
		cp2.LeaveEvent += delegate
		{
			Main.recBigList = false;
		};
		cp2.CanEnterEvent += () => Main.playerInventory && Main.numAvailableRecipes > 0;
		cp2.IsValidEvent += () => Main.playerInventory && Main.recBigList && Main.numAvailableRecipes > 0;
		cp2.PageOnLeft = 0;
		cp2.PageOnRight = 9;
		UILinkPointNavigator.RegisterPage(cp2, 10);
		UILinkPage cp20 = new UILinkPage();
		cp20.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		for (int num20 = 2605; num20 < 2620; num20++)
		{
			UILinkPoint uILinkPoint4 = new UILinkPoint(num20, enabled: true, num20, num20, num20, num20);
			uILinkPoint4.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[73].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
			cp20.LinkMap.Add(num20, uILinkPoint4);
		}
		cp20.UpdateEvent += delegate
		{
			int num55 = 5;
			int num56 = 3;
			int num57 = num55 * num56;
			int count = Main.Hairstyles.AvailableHairstyles.Count;
			for (int num58 = 0; num58 < num57; num58++)
			{
				cp20.LinkMap[2605 + num58].Left = ((num58 % num55 == 0) ? (-3) : (2605 + num58 - 1));
				cp20.LinkMap[2605 + num58].Right = (((num58 + 1) % num55 == 0 || num58 == count - 1) ? (-4) : (2605 + num58 + 1));
				cp20.LinkMap[2605 + num58].Down = ((num58 < num57 - num55) ? (2605 + num58 + num55) : (-2));
				cp20.LinkMap[2605 + num58].Up = ((num58 < num55) ? (-1) : (2605 + num58 - num55));
			}
		};
		cp20.ReachEndEvent += delegate(int current, int next)
		{
			int num54 = 5;
			switch (next)
			{
			case -1:
				Main.hairStart -= num54;
				SoundEngine.PlaySound(12);
				break;
			case -2:
				Main.hairStart += num54;
				SoundEngine.PlaySound(12);
				break;
			}
		};
		cp20.CanEnterEvent += () => Main.hairWindow;
		cp20.IsValidEvent += () => Main.hairWindow;
		cp20.PageOnLeft = 12;
		cp20.PageOnRight = 12;
		UILinkPointNavigator.RegisterPage(cp20, 11);
		UILinkPage uILinkPage6 = new UILinkPage();
		uILinkPage6.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		uILinkPage6.LinkMap.Add(2600, new UILinkPoint(2600, enabled: true, -3, -4, -1, 2601));
		uILinkPage6.LinkMap.Add(2601, new UILinkPoint(2601, enabled: true, -3, -4, 2600, 2602));
		uILinkPage6.LinkMap.Add(2602, new UILinkPoint(2602, enabled: true, -3, -4, 2601, 2603));
		uILinkPage6.LinkMap.Add(2603, new UILinkPoint(2603, enabled: true, -3, 2604, 2602, -2));
		uILinkPage6.LinkMap.Add(2604, new UILinkPoint(2604, enabled: true, 2603, -4, 2602, -2));
		uILinkPage6.UpdateEvent += delegate
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val2 = Main.rgbToHsl(Main.selColor);
			float interfaceDeadzoneX2 = PlayerInput.CurrentProfile.InterfaceDeadzoneX;
			float x2 = PlayerInput.GamepadThumbstickLeft.X;
			x2 = ((!(x2 < 0f - interfaceDeadzoneX2) && !(x2 > interfaceDeadzoneX2)) ? 0f : (MathHelper.Lerp(0f, 1f / 120f, (Math.Abs(x2) - interfaceDeadzoneX2) / (1f - interfaceDeadzoneX2)) * (float)Math.Sign(x2)));
			int currentPoint2 = UILinkPointNavigator.CurrentPoint;
			if (currentPoint2 == 2600)
			{
				Main.hBar = MathHelper.Clamp(Main.hBar + x2, 0f, 1f);
			}
			if (currentPoint2 == 2601)
			{
				Main.sBar = MathHelper.Clamp(Main.sBar + x2, 0f, 1f);
			}
			if (currentPoint2 == 2602)
			{
				Main.lBar = MathHelper.Clamp(Main.lBar + x2, 0.15f, 1f);
			}
			Vector3.Clamp(val2, Vector3.Zero, Vector3.One);
			if (x2 != 0f)
			{
				if (Main.hairWindow)
				{
					Main.player[Main.myPlayer].hairColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
				}
				SoundEngine.PlaySound(12);
			}
		};
		uILinkPage6.CanEnterEvent += () => Main.hairWindow;
		uILinkPage6.IsValidEvent += () => Main.hairWindow;
		uILinkPage6.PageOnLeft = 11;
		uILinkPage6.PageOnRight = 11;
		UILinkPointNavigator.RegisterPage(uILinkPage6, 12);
		UILinkPage cp19 = new UILinkPage();
		for (int num21 = 0; num21 < 30; num21++)
		{
			cp19.LinkMap.Add(2900 + num21, new UILinkPoint(2900 + num21, enabled: true, -3, -4, -1, -2));
			cp19.LinkMap[2900 + num21].OnSpecialInteracts += value;
		}
		cp19.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		cp19.TravelEvent += delegate
		{
			if (UILinkPointNavigator.CurrentPage == cp19.ID)
			{
				int num53 = cp19.CurrentPoint - 2900;
				if (num53 < 5)
				{
					IngameOptions.category = num53;
				}
			}
		};
		cp19.UpdateEvent += delegate
		{
			int num49 = UILinkPointNavigator.Shortcuts.INGAMEOPTIONS_BUTTONS_LEFT;
			if (num49 == 0)
			{
				num49 = 5;
			}
			if (UILinkPointNavigator.OverridePoint == -1 && cp19.CurrentPoint < 2930 && cp19.CurrentPoint > 2900 + num49 - 1)
			{
				UILinkPointNavigator.ChangePoint(2900);
			}
			for (int num50 = 2900; num50 < 2900 + num49; num50++)
			{
				cp19.LinkMap[num50].Up = num50 - 1;
				cp19.LinkMap[num50].Down = num50 + 1;
			}
			cp19.LinkMap[2900].Up = 2900 + num49 - 1;
			cp19.LinkMap[2900 + num49 - 1].Down = 2900;
			int num51 = cp19.CurrentPoint - 2900;
			if (num51 < 4 && CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.MouseLeft)
			{
				IngameOptions.category = num51;
				UILinkPointNavigator.ChangePage(1002);
			}
			int num52 = ((SocialAPI.Network != null && SocialAPI.Network.CanInvite()) ? 1 : 0);
			if (num51 == 4 + num52 && CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.MouseLeft)
			{
				UILinkPointNavigator.ChangePage(1004);
			}
		};
		cp19.EnterEvent += delegate
		{
			cp19.CurrentPoint = 2900 + IngameOptions.category;
		};
		cp19.PageOnLeft = (cp19.PageOnRight = 1002);
		cp19.IsValidEvent += () => Main.ingameOptionsWindow && !Main.InGameUI.IsVisible;
		cp19.CanEnterEvent += () => Main.ingameOptionsWindow && !Main.InGameUI.IsVisible;
		UILinkPointNavigator.RegisterPage(cp19, 1001);
		UILinkPage cp18 = new UILinkPage();
		for (int num22 = 0; num22 < 30; num22++)
		{
			cp18.LinkMap.Add(2930 + num22, new UILinkPoint(2930 + num22, enabled: true, -3, -4, -1, -2));
			cp18.LinkMap[2930 + num22].OnSpecialInteracts += value;
		}
		cp18.EnterEvent += delegate
		{
			Main.mouseLeftRelease = false;
		};
		cp18.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		cp18.UpdateEvent += delegate
		{
			int num47 = UILinkPointNavigator.Shortcuts.INGAMEOPTIONS_BUTTONS_RIGHT;
			if (num47 == 0)
			{
				num47 = 5;
			}
			if (UILinkPointNavigator.OverridePoint == -1 && cp18.CurrentPoint >= 2930 && cp18.CurrentPoint > 2930 + num47 - 1)
			{
				UILinkPointNavigator.ChangePoint(2930);
			}
			for (int num48 = 2930; num48 < 2930 + num47; num48++)
			{
				cp18.LinkMap[num48].Up = num48 - 1;
				cp18.LinkMap[num48].Down = num48 + 1;
			}
			cp18.LinkMap[2930].Up = -1;
			cp18.LinkMap[2930 + num47 - 1].Down = -2;
			HandleOptionsSpecials();
		};
		cp18.PageOnLeft = (cp18.PageOnRight = 1001);
		cp18.IsValidEvent += () => Main.ingameOptionsWindow;
		cp18.CanEnterEvent += () => Main.ingameOptionsWindow;
		UILinkPointNavigator.RegisterPage(cp18, 1002);
		UILinkPage cp17 = new UILinkPage();
		cp17.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		for (int num23 = 1550; num23 < 1558; num23++)
		{
			UILinkPoint uILinkPoint5 = new UILinkPoint(num23, enabled: true, -3, -4, -1, -2);
			switch (num23)
			{
			case 1551:
			case 1553:
			case 1555:
				uILinkPoint5.Up = uILinkPoint5.ID - 2;
				uILinkPoint5.Down = uILinkPoint5.ID + 2;
				uILinkPoint5.Right = uILinkPoint5.ID + 1;
				break;
			case 1552:
			case 1554:
			case 1556:
				uILinkPoint5.Up = uILinkPoint5.ID - 2;
				uILinkPoint5.Down = uILinkPoint5.ID + 2;
				uILinkPoint5.Left = uILinkPoint5.ID - 1;
				break;
			}
			cp17.LinkMap.Add(num23, uILinkPoint5);
		}
		cp17.LinkMap[1550].Down = 1551;
		cp17.LinkMap[1550].Right = 120;
		cp17.LinkMap[1550].Up = 307;
		cp17.LinkMap[1551].Up = 1550;
		cp17.LinkMap[1552].Up = 1550;
		cp17.LinkMap[1552].Right = 121;
		cp17.LinkMap[1554].Right = 121;
		cp17.LinkMap[1555].Down = 1570;
		cp17.LinkMap[1556].Down = 1570;
		cp17.LinkMap[1556].Right = 122;
		cp17.LinkMap[1557].Up = 1570;
		cp17.LinkMap[1557].Down = 308;
		cp17.LinkMap[1557].Right = 127;
		cp17.LinkMap.Add(1570, new UILinkPoint(1570, enabled: true, -3, -4, -1, -2));
		cp17.LinkMap[1570].Up = 1555;
		cp17.LinkMap[1570].Down = 1557;
		cp17.LinkMap[1570].Right = 126;
		for (int num24 = 0; num24 < 7; num24++)
		{
			cp17.LinkMap[1550 + num24].OnSpecialInteracts += value;
		}
		cp17.UpdateEvent += delegate
		{
			if (!Main.ShouldPVPDraw)
			{
				if (UILinkPointNavigator.OverridePoint == -1 && cp17.CurrentPoint != 1557 && cp17.CurrentPoint != 1570)
				{
					UILinkPointNavigator.ChangePoint(1557);
				}
				cp17.LinkMap[1570].Up = -1;
				cp17.LinkMap[1557].Down = 308;
				cp17.LinkMap[1557].Right = 127;
			}
			else
			{
				cp17.LinkMap[1570].Up = 1555;
				cp17.LinkMap[1557].Down = 308;
				cp17.LinkMap[1557].Right = 127;
			}
			int iNFOACCCOUNT2 = UILinkPointNavigator.Shortcuts.INFOACCCOUNT;
			if (iNFOACCCOUNT2 > 0)
			{
				cp17.LinkMap[1570].Up = 1558 + (iNFOACCCOUNT2 - 1) / 2 * 2;
			}
			if (Main.ShouldPVPDraw)
			{
				if (iNFOACCCOUNT2 >= 1)
				{
					cp17.LinkMap[1555].Down = 1558;
					cp17.LinkMap[1556].Down = 1558;
				}
				else
				{
					cp17.LinkMap[1555].Down = 1570;
					cp17.LinkMap[1556].Down = 1570;
				}
				if (iNFOACCCOUNT2 >= 2)
				{
					cp17.LinkMap[1556].Down = 1559;
				}
				else
				{
					cp17.LinkMap[1556].Down = 1570;
				}
			}
		};
		cp17.IsValidEvent += () => Main.playerInventory;
		cp17.PageOnLeft = 8;
		cp17.PageOnRight = 8;
		UILinkPointNavigator.RegisterPage(cp17, 16);
		UILinkPage cp16 = new UILinkPage();
		cp16.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		for (int num25 = 1558; num25 < 1570; num25++)
		{
			UILinkPoint uILinkPoint6 = new UILinkPoint(num25, enabled: true, -3, -4, -1, -2);
			uILinkPoint6.OnSpecialInteracts += value;
			switch (num25)
			{
			case 1559:
			case 1561:
			case 1563:
				uILinkPoint6.Up = uILinkPoint6.ID - 2;
				uILinkPoint6.Down = uILinkPoint6.ID + 2;
				uILinkPoint6.Right = uILinkPoint6.ID + 1;
				break;
			case 1560:
			case 1562:
			case 1564:
				uILinkPoint6.Up = uILinkPoint6.ID - 2;
				uILinkPoint6.Down = uILinkPoint6.ID + 2;
				uILinkPoint6.Left = uILinkPoint6.ID - 1;
				break;
			}
			cp16.LinkMap.Add(num25, uILinkPoint6);
		}
		cp16.UpdateEvent += delegate
		{
			int iNFOACCCOUNT = UILinkPointNavigator.Shortcuts.INFOACCCOUNT;
			if (UILinkPointNavigator.OverridePoint == -1 && cp16.CurrentPoint - 1558 >= iNFOACCCOUNT)
			{
				UILinkPointNavigator.ChangePoint(1558 + iNFOACCCOUNT - 1);
			}
			for (int num45 = 0; num45 < iNFOACCCOUNT; num45++)
			{
				bool flag = num45 % 2 == 0;
				int num46 = num45 + 1558;
				cp16.LinkMap[num46].Down = ((num45 < iNFOACCCOUNT - 2) ? (num46 + 2) : 1570);
				cp16.LinkMap[num46].Up = ((num45 > 1) ? (num46 - 2) : (Main.ShouldPVPDraw ? (flag ? 1555 : 1556) : (-1)));
				cp16.LinkMap[num46].Right = ((flag && num45 + 1 < iNFOACCCOUNT) ? (num46 + 1) : (123 + num45 / 4));
				cp16.LinkMap[num46].Left = (flag ? (-3) : (num46 - 1));
			}
		};
		cp16.IsValidEvent += () => Main.playerInventory && UILinkPointNavigator.Shortcuts.INFOACCCOUNT > 0;
		cp16.PageOnLeft = 8;
		cp16.PageOnRight = 8;
		UILinkPointNavigator.RegisterPage(cp16, 17);
		UILinkPage cp15 = new UILinkPage();
		cp15.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		for (int num26 = 6000; num26 < 6012; num26++)
		{
			UILinkPoint uILinkPoint7 = new UILinkPoint(num26, enabled: true, -3, -4, -1, -2);
			switch (num26)
			{
			case 6000:
				uILinkPoint7.Right = 0;
				break;
			case 6001:
			case 6002:
				uILinkPoint7.Right = 10;
				break;
			case 6003:
			case 6004:
				uILinkPoint7.Right = 20;
				break;
			case 6005:
			case 6006:
				uILinkPoint7.Right = 30;
				break;
			default:
				uILinkPoint7.Right = 40;
				break;
			}
			cp15.LinkMap.Add(num26, uILinkPoint7);
		}
		cp15.UpdateEvent += delegate
		{
			int bUILDERACCCOUNT = UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT;
			if (UILinkPointNavigator.OverridePoint == -1 && cp15.CurrentPoint - 6000 >= bUILDERACCCOUNT)
			{
				UILinkPointNavigator.ChangePoint(6000 + bUILDERACCCOUNT - 1);
			}
			for (int num43 = 0; num43 < bUILDERACCCOUNT; num43++)
			{
				_ = num43 % 2;
				int num44 = num43 + 6000;
				cp15.LinkMap[num44].Down = ((num43 < bUILDERACCCOUNT - 1) ? (num44 + 1) : (-2));
				cp15.LinkMap[num44].Up = ((num43 > 0) ? (num44 - 1) : (-1));
			}
		};
		cp15.IsValidEvent += () => Main.playerInventory && UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 0;
		cp15.PageOnLeft = 8;
		cp15.PageOnRight = 8;
		UILinkPointNavigator.RegisterPage(cp15, 18);
		UILinkPage uILinkPage7 = new UILinkPage();
		uILinkPage7.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		uILinkPage7.LinkMap.Add(2806, new UILinkPoint(2806, enabled: true, 2805, 2807, -1, 2808));
		uILinkPage7.LinkMap.Add(2807, new UILinkPoint(2807, enabled: true, 2806, 2810, -1, 2809));
		uILinkPage7.LinkMap.Add(2808, new UILinkPoint(2808, enabled: true, 2805, 2809, 2806, -2));
		uILinkPage7.LinkMap.Add(2809, new UILinkPoint(2809, enabled: true, 2808, 2811, 2807, -2));
		uILinkPage7.LinkMap.Add(2810, new UILinkPoint(2810, enabled: true, 2807, -4, -1, 2811));
		uILinkPage7.LinkMap.Add(2811, new UILinkPoint(2811, enabled: true, 2809, -4, 2810, -2));
		uILinkPage7.LinkMap.Add(2805, new UILinkPoint(2805, enabled: true, -3, 2806, -1, -2));
		uILinkPage7.LinkMap[2806].OnSpecialInteracts += value;
		uILinkPage7.LinkMap[2807].OnSpecialInteracts += value;
		uILinkPage7.LinkMap[2808].OnSpecialInteracts += value;
		uILinkPage7.LinkMap[2809].OnSpecialInteracts += value;
		uILinkPage7.LinkMap[2805].OnSpecialInteracts += value;
		uILinkPage7.CanEnterEvent += () => Main.clothesWindow;
		uILinkPage7.IsValidEvent += () => Main.clothesWindow;
		uILinkPage7.EnterEvent += delegate
		{
			Main.player[Main.myPlayer].releaseInventory = false;
		};
		uILinkPage7.LeaveEvent += delegate
		{
			Main.player[Main.myPlayer].LockGamepadTileInteractions();
		};
		uILinkPage7.PageOnLeft = 15;
		uILinkPage7.PageOnRight = 15;
		UILinkPointNavigator.RegisterPage(uILinkPage7, 14);
		UILinkPage uILinkPage8 = new UILinkPage();
		uILinkPage8.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		uILinkPage8.LinkMap.Add(2800, new UILinkPoint(2800, enabled: true, -3, -4, -1, 2801));
		uILinkPage8.LinkMap.Add(2801, new UILinkPoint(2801, enabled: true, -3, -4, 2800, 2802));
		uILinkPage8.LinkMap.Add(2802, new UILinkPoint(2802, enabled: true, -3, -4, 2801, 2803));
		uILinkPage8.LinkMap.Add(2803, new UILinkPoint(2803, enabled: true, -3, 2804, 2802, -2));
		uILinkPage8.LinkMap.Add(2804, new UILinkPoint(2804, enabled: true, 2803, -4, 2802, -2));
		uILinkPage8.LinkMap[2800].OnSpecialInteracts += value;
		uILinkPage8.LinkMap[2801].OnSpecialInteracts += value;
		uILinkPage8.LinkMap[2802].OnSpecialInteracts += value;
		uILinkPage8.LinkMap[2803].OnSpecialInteracts += value;
		uILinkPage8.LinkMap[2804].OnSpecialInteracts += value;
		uILinkPage8.UpdateEvent += delegate
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = Main.rgbToHsl(Main.selColor);
			float interfaceDeadzoneX = PlayerInput.CurrentProfile.InterfaceDeadzoneX;
			float x = PlayerInput.GamepadThumbstickLeft.X;
			x = ((!(x < 0f - interfaceDeadzoneX) && !(x > interfaceDeadzoneX)) ? 0f : (MathHelper.Lerp(0f, 1f / 120f, (Math.Abs(x) - interfaceDeadzoneX) / (1f - interfaceDeadzoneX)) * (float)Math.Sign(x)));
			int currentPoint = UILinkPointNavigator.CurrentPoint;
			if (currentPoint == 2800)
			{
				Main.hBar = MathHelper.Clamp(Main.hBar + x, 0f, 1f);
			}
			if (currentPoint == 2801)
			{
				Main.sBar = MathHelper.Clamp(Main.sBar + x, 0f, 1f);
			}
			if (currentPoint == 2802)
			{
				Main.lBar = MathHelper.Clamp(Main.lBar + x, 0.15f, 1f);
			}
			Vector3.Clamp(val, Vector3.Zero, Vector3.One);
			if (x != 0f)
			{
				if (Main.clothesWindow)
				{
					Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar);
					switch (Main.selClothes)
					{
					case 0:
						Main.player[Main.myPlayer].shirtColor = Main.selColor;
						break;
					case 1:
						Main.player[Main.myPlayer].underShirtColor = Main.selColor;
						break;
					case 2:
						Main.player[Main.myPlayer].pantsColor = Main.selColor;
						break;
					case 3:
						Main.player[Main.myPlayer].shoeColor = Main.selColor;
						break;
					}
				}
				SoundEngine.PlaySound(12);
			}
		};
		uILinkPage8.CanEnterEvent += () => Main.clothesWindow;
		uILinkPage8.IsValidEvent += () => Main.clothesWindow;
		uILinkPage8.EnterEvent += delegate
		{
			Main.player[Main.myPlayer].releaseInventory = false;
		};
		uILinkPage8.LeaveEvent += delegate
		{
			Main.player[Main.myPlayer].LockGamepadTileInteractions();
		};
		uILinkPage8.PageOnLeft = 14;
		uILinkPage8.PageOnRight = 14;
		UILinkPointNavigator.RegisterPage(uILinkPage8, 15);
		UILinkPage cp14 = new UILinkPage();
		cp14.UpdateEvent += delegate
		{
			PlayerInput.GamepadAllowScrolling = true;
		};
		for (int num27 = 3000; num27 <= 4999; num27++)
		{
			cp14.LinkMap.Add(num27, new UILinkPoint(num27, enabled: true, -3, -4, -1, -2));
		}
		cp14.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[53].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]) + PlayerInput.BuildCommand(Lang.misc[82].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + FancyUISpecialInstructions();
		cp14.UpdateEvent += delegate
		{
			if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.Inventory)
			{
				FancyExit();
			}
			UILinkPointNavigator.Shortcuts.BackButtonInUse = false;
		};
		cp14.EnterEvent += delegate
		{
			cp14.CurrentPoint = 3002;
		};
		cp14.CanEnterEvent += () => Main.MenuUI.IsVisible || Main.InGameUI.IsVisible;
		cp14.IsValidEvent += () => Main.MenuUI.IsVisible || Main.InGameUI.IsVisible;
		cp14.OnPageMoveAttempt += OnFancyUIPageMoveAttempt;
		UILinkPointNavigator.RegisterPage(cp14, 1004);
		UILinkPage cp12 = new UILinkPage();
		cp12.UpdateEvent += delegate
		{
			PlayerInput.GamepadAllowScrolling = true;
		};
		for (int num28 = 10000; num28 <= 11000; num28++)
		{
			cp12.LinkMap.Add(num28, new UILinkPoint(num28, enabled: true, -3, -4, -1, -2));
		}
		for (int num29 = 15000; num29 <= 15000; num29++)
		{
			cp12.LinkMap.Add(num29, new UILinkPoint(num29, enabled: true, -3, -4, -1, -2));
		}
		cp12.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[53].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]) + PlayerInput.BuildCommand(Lang.misc[82].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + FancyUISpecialInstructions();
		cp12.UpdateEvent += delegate
		{
			if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.Inventory)
			{
				FancyExit();
			}
			UILinkPointNavigator.Shortcuts.BackButtonInUse = false;
		};
		cp12.EnterEvent += delegate
		{
			cp12.CurrentPoint = 10000;
		};
		cp12.CanEnterEvent += CanEnterCreativeMenu;
		cp12.IsValidEvent += CanEnterCreativeMenu;
		cp12.OnPageMoveAttempt += OnFancyUIPageMoveAttempt;
		cp12.PageOnLeft = 8;
		cp12.PageOnRight = 0;
		UILinkPointNavigator.RegisterPage(cp12, 1005);
		UILinkPage cp = new UILinkPage();
		cp.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		Func<string> value14 = () => PlayerInput.BuildCommand(Lang.misc[94].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
		for (int num30 = 9000; num30 <= 9050; num30++)
		{
			UILinkPoint uILinkPoint8 = new UILinkPoint(num30, enabled: true, num30 + 10, num30 - 10, num30 - 1, num30 + 1);
			cp.LinkMap.Add(num30, uILinkPoint8);
			uILinkPoint8.OnSpecialInteracts += value14;
		}
		cp.UpdateEvent += delegate
		{
			int num41 = UILinkPointNavigator.Shortcuts.BUFFS_PER_COLUMN;
			if (num41 == 0)
			{
				num41 = 100;
			}
			for (int num42 = 0; num42 < 50; num42++)
			{
				cp.LinkMap[9000 + num42].Up = ((num42 % num41 == 0) ? (-1) : (9000 + num42 - 1));
				if (cp.LinkMap[9000 + num42].Up == -1)
				{
					if (num42 >= num41)
					{
						cp.LinkMap[9000 + num42].Up = 184;
					}
					else
					{
						cp.LinkMap[9000 + num42].Up = 189;
					}
				}
				cp.LinkMap[9000 + num42].Down = (((num42 + 1) % num41 == 0 || num42 == UILinkPointNavigator.Shortcuts.BUFFS_DRAWN - 1) ? 308 : (9000 + num42 + 1));
				cp.LinkMap[9000 + num42].Left = ((num42 < UILinkPointNavigator.Shortcuts.BUFFS_DRAWN - num41) ? (9000 + num42 + num41) : (-3));
				cp.LinkMap[9000 + num42].Right = ((num42 < num41) ? (-4) : (9000 + num42 - num41));
			}
		};
		cp.IsValidEvent += () => Main.playerInventory && Main.EquipPage == 2 && UILinkPointNavigator.Shortcuts.BUFFS_DRAWN > 0;
		cp.PageOnLeft = 8;
		cp.PageOnRight = 8;
		UILinkPointNavigator.RegisterPage(cp, 19);
		UILinkPage uILinkPage9 = UILinkPointNavigator.Pages[UILinkPointNavigator.CurrentPage];
		uILinkPage9.CurrentPoint = uILinkPage9.DefaultPoint;
		uILinkPage9.Enter();
	}

	private static bool CanEnterCreativeMenu()
	{
		if (Main.LocalPlayer.chest != -1)
		{
			return false;
		}
		if (Main.LocalPlayer.talkNPC != -1)
		{
			return false;
		}
		if (Main.playerInventory)
		{
			return Main.CreativeMenu.Enabled;
		}
		return false;
	}

	private static int GetCornerWrapPageIdFromLeftToRight()
	{
		return 8;
	}

	private static int GetCornerWrapPageIdFromRightToLeft()
	{
		if (Main.CreativeMenu.Enabled)
		{
			return 1005;
		}
		return 10;
	}

	private static void OnFancyUIPageMoveAttempt(int direction)
	{
		if (Main.MenuUI.CurrentState is UICharacterCreation uICharacterCreation)
		{
			uICharacterCreation.TryMovingCategory(direction);
		}
		if (UserInterface.ActiveInstance.CurrentState is UIBestiaryTest uIBestiaryTest)
		{
			uIBestiaryTest.TryMovingPages(direction);
		}
	}

	public static void FancyExit()
	{
		switch (UILinkPointNavigator.Shortcuts.BackButtonCommand)
		{
		case 1:
			SoundEngine.PlaySound(11);
			Main.menuMode = 0;
			break;
		case 2:
			SoundEngine.PlaySound(11);
			Main.menuMode = ((!Main.menuMultiplayer) ? 1 : 12);
			break;
		case 3:
			Main.menuMode = 0;
			IngameFancyUI.Close();
			break;
		case 4:
			SoundEngine.PlaySound(11);
			Main.menuMode = 11;
			break;
		case 5:
			SoundEngine.PlaySound(11);
			Main.menuMode = 11;
			break;
		case 6:
			UIVirtualKeyboard.Cancel();
			break;
		case 7:
			if (Main.MenuUI.CurrentState is IHaveBackButtonCommand haveBackButtonCommand)
			{
				haveBackButtonCommand.HandleBackButtonUsage();
			}
			break;
		case 100:
			SoundEngine.PlaySound(11);
			Main.menuMode = UILinkPointNavigator.Shortcuts.BackButtonGoto;
			break;
		}
	}

	public static string FancyUISpecialInstructions()
	{
		string text = "";
		if (UILinkPointNavigator.Shortcuts.FANCYUI_SPECIAL_INSTRUCTIONS == 1)
		{
			if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.HotbarMinus)
			{
				UIVirtualKeyboard.CycleSymbols();
				PlayerInput.LockGamepadButtons("HotbarMinus");
				PlayerInput.SettingsForUI.TryRevertingToMouseMode();
			}
			text += PlayerInput.BuildCommand(Lang.menu[235].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"]);
			if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.MouseRight)
			{
				UIVirtualKeyboard.BackSpace();
				PlayerInput.LockGamepadButtons("MouseRight");
				PlayerInput.SettingsForUI.TryRevertingToMouseMode();
			}
			text += PlayerInput.BuildCommand(Lang.menu[236].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
			if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.SmartCursor)
			{
				UIVirtualKeyboard.Write(" ");
				PlayerInput.LockGamepadButtons("SmartCursor");
				PlayerInput.SettingsForUI.TryRevertingToMouseMode();
			}
			text += PlayerInput.BuildCommand(Lang.menu[238].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["SmartCursor"]);
			if (UIVirtualKeyboard.CanSubmit)
			{
				if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.HotbarPlus)
				{
					UIVirtualKeyboard.Submit();
					PlayerInput.LockGamepadButtons("HotbarPlus");
					PlayerInput.SettingsForUI.TryRevertingToMouseMode();
				}
				text += PlayerInput.BuildCommand(Lang.menu[237].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			}
		}
		return text;
	}

	public static void HandleOptionsSpecials()
	{
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_061e: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0624: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0652: Unknown result type (might be due to invalid IL or missing references)
		//IL_0653: Unknown result type (might be due to invalid IL or missing references)
		//IL_0658: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_0686: Unknown result type (might be due to invalid IL or missing references)
		//IL_0687: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		switch (UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE)
		{
		case 1:
			Main.bgScroll = (int)HandleSliderHorizontalInput(Main.bgScroll, 0f, 100f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 1f);
			Main.caveParallax = 1f - (float)Main.bgScroll / 500f;
			break;
		case 2:
			Main.musicVolume = HandleSliderHorizontalInput(Main.musicVolume, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			break;
		case 3:
			Main.soundVolume = HandleSliderHorizontalInput(Main.soundVolume, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			break;
		case 4:
			Main.ambientVolume = HandleSliderHorizontalInput(Main.ambientVolume, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			break;
		case 5:
		{
			float hBar = Main.hBar;
			float num3 = (Main.hBar = HandleSliderHorizontalInput(hBar, 0f, 1f));
			if (hBar != num3)
			{
				switch (Main.menuMode)
				{
				case 17:
					Main.player[Main.myPlayer].hairColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 18:
					Main.player[Main.myPlayer].eyeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 19:
					Main.player[Main.myPlayer].skinColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 21:
					Main.player[Main.myPlayer].shirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 22:
					Main.player[Main.myPlayer].underShirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 23:
					Main.player[Main.myPlayer].pantsColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 24:
					Main.player[Main.myPlayer].shoeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 25:
					Main.mouseColorSlider.Hue = num3;
					break;
				case 252:
					Main.mouseBorderColorSlider.Hue = num3;
					break;
				}
				SoundEngine.PlaySound(12);
			}
			break;
		}
		case 6:
		{
			float sBar = Main.sBar;
			float num2 = (Main.sBar = HandleSliderHorizontalInput(sBar, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX));
			if (sBar != num2)
			{
				switch (Main.menuMode)
				{
				case 17:
					Main.player[Main.myPlayer].hairColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 18:
					Main.player[Main.myPlayer].eyeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 19:
					Main.player[Main.myPlayer].skinColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 21:
					Main.player[Main.myPlayer].shirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 22:
					Main.player[Main.myPlayer].underShirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 23:
					Main.player[Main.myPlayer].pantsColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 24:
					Main.player[Main.myPlayer].shoeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 25:
					Main.mouseColorSlider.Saturation = num2;
					break;
				case 252:
					Main.mouseBorderColorSlider.Saturation = num2;
					break;
				}
				SoundEngine.PlaySound(12);
			}
			break;
		}
		case 7:
		{
			float lBar3 = Main.lBar;
			float min = 0.15f;
			if (Main.menuMode == 252)
			{
				min = 0f;
			}
			Main.lBar = HandleSliderHorizontalInput(lBar3, min, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX);
			float lBar2 = Main.lBar;
			if (lBar3 != lBar2)
			{
				switch (Main.menuMode)
				{
				case 17:
					Main.player[Main.myPlayer].hairColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 18:
					Main.player[Main.myPlayer].eyeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 19:
					Main.player[Main.myPlayer].skinColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 21:
					Main.player[Main.myPlayer].shirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 22:
					Main.player[Main.myPlayer].underShirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 23:
					Main.player[Main.myPlayer].pantsColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 24:
					Main.player[Main.myPlayer].shoeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 25:
					Main.mouseColorSlider.Luminance = lBar2;
					break;
				case 252:
					Main.mouseBorderColorSlider.Luminance = lBar2;
					break;
				}
				SoundEngine.PlaySound(12);
			}
			break;
		}
		case 8:
		{
			float aBar = Main.aBar;
			float num4 = (Main.aBar = HandleSliderHorizontalInput(aBar, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX));
			if (aBar != num4)
			{
				if (Main.menuMode == 252)
				{
					Main.mouseBorderColorSlider.Alpha = num4;
				}
				SoundEngine.PlaySound(12);
			}
			break;
		}
		case 9:
		{
			bool left = PlayerInput.Triggers.Current.Left;
			bool right = PlayerInput.Triggers.Current.Right;
			if (PlayerInput.Triggers.JustPressed.Left || PlayerInput.Triggers.JustPressed.Right)
			{
				SomeVarsForUILinkers.HairMoveCD = 0;
			}
			else if (SomeVarsForUILinkers.HairMoveCD > 0)
			{
				SomeVarsForUILinkers.HairMoveCD--;
			}
			if (SomeVarsForUILinkers.HairMoveCD == 0 && (left || right))
			{
				if (left)
				{
					Main.PendingPlayer.hair--;
				}
				if (right)
				{
					Main.PendingPlayer.hair++;
				}
				SomeVarsForUILinkers.HairMoveCD = 12;
			}
			int num = 51;
			if (Main.PendingPlayer.hair >= num)
			{
				Main.PendingPlayer.hair = 0;
			}
			if (Main.PendingPlayer.hair < 0)
			{
				Main.PendingPlayer.hair = num - 1;
			}
			break;
		}
		case 10:
			Main.GameZoomTarget = HandleSliderHorizontalInput(Main.GameZoomTarget, 1f, 2f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			break;
		case 11:
			Main.UIScale = HandleSliderHorizontalInput(Main.UIScaleWanted, 0.5f, 2f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			Main.temporaryGUIScaleSlider = Main.UIScaleWanted;
			break;
		case 12:
			Main.MapScale = HandleSliderHorizontalInput(Main.MapScale, 0.5f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.7f);
			break;
		}
	}
}
