using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Default;
using Terraria.UI.Chat;

namespace Terraria.ModLoader;

public static class MenuLoader
{
	internal static readonly MenutML MenutML = new MenutML();

	internal static readonly MenuJourneysEnd MenuJourneysEnd = new MenuJourneysEnd();

	internal static readonly MenuOldVanilla MenuOldVanilla = new MenuOldVanilla();

	private static readonly List<ModMenu> menus = new List<ModMenu> { MenutML, MenuJourneysEnd, MenuOldVanilla };

	private static readonly int DefaultMenuCount = menus.Count;

	private static ModMenu currentMenu = MenutML;

	private static ModMenu switchToMenu = null;

	internal static bool loading = true;

	internal static string LastSelectedModMenu = MenutML.FullName;

	internal static string KnownMenuSaveString = string.Join(",", menus.Select((ModMenu m) => m.FullName));

	public static ModMenu CurrentMenu => currentMenu;

	private static string[] KnownMenus => KnownMenuSaveString.Split(',');

	private static void AddKnownMenu(string name)
	{
		string newSaveString = string.Join(",", KnownMenus.Concat(new string[1] { name }).Distinct());
		if (newSaveString != KnownMenuSaveString)
		{
			KnownMenuSaveString = newSaveString;
			Main.SaveSettings();
		}
	}

	internal static void Add(ModMenu modMenu)
	{
		lock (menus)
		{
			menus.Add(modMenu);
			ModTypeLookup<ModMenu>.Register(modMenu);
		}
	}

	private static void OffsetModMenu(int offset)
	{
		lock (menus)
		{
			switchToMenu = currentMenu;
			do
			{
				switchToMenu = menus[Utils.Repeat(menus.IndexOf(switchToMenu) + offset, menus.Count)];
			}
			while (!switchToMenu.IsAvailable);
		}
	}

	internal static void GotoSavedModMenu()
	{
		if (LastSelectedModMenu == MenuOldVanilla.FullName)
		{
			Main.instance.playOldTile = true;
		}
		switchToMenu = MenutML;
		if (ModContent.TryFind<ModMenu>(LastSelectedModMenu, out var value) && value.IsAvailable)
		{
			switchToMenu = value;
		}
		loading = false;
	}

	public static void ActivateOldVanillaMenu()
	{
		switchToMenu = MenuOldVanilla;
	}

	internal static void UpdateAndDrawModMenu(SpriteBatch spriteBatch, GameTime gameTime, Color color, float logoRotation, float logoScale)
	{
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		if (switchToMenu != null && switchToMenu != currentMenu)
		{
			currentMenu.OnDeselected();
			currentMenu = switchToMenu;
			currentMenu.OnSelected();
			if (currentMenu.IsNew)
			{
				currentMenu.IsNew = false;
				AddKnownMenu(currentMenu.FullName);
			}
		}
		switchToMenu = null;
		if (!loading && currentMenu.FullName != LastSelectedModMenu)
		{
			LastSelectedModMenu = currentMenu.FullName;
			Main.SaveSettings();
		}
		currentMenu.UserInterface.Update(gameTime);
		currentMenu.UserInterface.Draw(spriteBatch, gameTime);
		currentMenu.Update(Main.menuMode == 0);
		Texture2D logo = currentMenu.Logo.Value;
		Vector2 logoDrawPos = default(Vector2);
		((Vector2)(ref logoDrawPos))._002Ector((float)(Main.screenWidth / 2), 100f);
		float scale = logoScale;
		if (currentMenu.PreDrawLogo(spriteBatch, ref logoDrawPos, ref logoRotation, ref scale, ref color))
		{
			spriteBatch.Draw(logo, logoDrawPos, (Rectangle?)new Rectangle(0, 0, logo.Width, logo.Height), color, logoRotation, new Vector2((float)logo.Width * 0.5f, (float)logo.Height * 0.5f), scale, (SpriteEffects)0, 0f);
		}
		currentMenu.PostDrawLogo(spriteBatch, logoDrawPos, logoRotation, scale, color);
		int newMenus;
		lock (menus)
		{
			string[] knownMenus = KnownMenus;
			foreach (ModMenu menu in menus)
			{
				menu.IsNew = menu.IsAvailable && !knownMenus.Contains(menu.FullName);
			}
			newMenus = menus.Count((ModMenu m) => m.IsNew);
		}
		string text = Language.GetTextValue("tModLoader.ModMenuSwap") + ": " + currentMenu.DisplayName + ((newMenus == 0) ? "" : (ModLoader.notifyNewMainMenuThemes ? $" ({newMenus} New)" : ""));
		Vector2 size = ChatManager.GetStringSize(FontAssets.MouseText.Value, ChatManager.ParseMessage(text, color).ToArray(), Vector2.One);
		Rectangle switchTextRect = (Rectangle)((Main.menuMode == 0) ? new Rectangle((int)((float)(Main.screenWidth / 2) - size.X / 2f), (int)((float)(Main.screenHeight - 2) - size.Y), (int)size.X, (int)size.Y) : Rectangle.Empty);
		if (((Rectangle)(ref switchTextRect)).Contains(Main.mouseX, Main.mouseY) && !Main.alreadyGrabbingSunOrMoon)
		{
			if (Main.mouseLeftRelease && Main.mouseLeft)
			{
				SoundEngine.PlaySound(in SoundID.MenuTick);
				OffsetModMenu(1);
			}
			else if (Main.mouseRightRelease && Main.mouseRight)
			{
				SoundEngine.PlaySound(in SoundID.MenuTick);
				OffsetModMenu(-1);
			}
		}
		if (Main.menuMode == 0)
		{
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, text, new Vector2((float)switchTextRect.X, (float)switchTextRect.Y), (Color)(((Rectangle)(ref switchTextRect)).Contains(Main.mouseX, Main.mouseY) ? Main.OurFavoriteColor : new Color(120, 120, 120, 76)), 0f, Vector2.Zero, Vector2.One);
		}
	}

	internal static void Unload()
	{
		loading = true;
		if (menus.IndexOf(currentMenu, 0, DefaultMenuCount) == -1)
		{
			switchToMenu = MenutML;
			while (currentMenu != MenutML)
			{
				Thread.Yield();
			}
		}
		lock (menus)
		{
			menus.RemoveRange(DefaultMenuCount, menus.Count - DefaultMenuCount);
		}
	}
}
