using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.Localization;
using Terraria.ModLoader.Default;

namespace Terraria.ModLoader;

public static class BossBarLoader
{
	/// <summary>
	/// Set to the current info that is being drawn just before any registered bar draws through the vanilla system (modded and vanilla), reset in the method used to draw it.
	/// <para>Allows tML to short-circuit the draw method and make ModBossBar and GlobalBossBar modify the draw parameters. Is null if a ModBossBarStyle skips drawing</para>
	/// </summary>
	internal static BigProgressBarInfo? drawingInfo = null;

	private static Asset<Texture2D> vanillaBossBarTexture;

	/// <summary>
	/// Used to prevent switching to the default style while the mods are loading (The code responsible for it runs in the main menu too)
	/// </summary>
	private static bool styleLoading = true;

	internal static readonly ModBossBarStyle vanillaStyle = new DefaultBossBarStyle();

	private static ModBossBarStyle switchToStyle = null;

	/// <summary>
	/// The string that is saved in the config
	/// </summary>
	internal static string lastSelectedStyle = CurrentStyle.FullName;

	internal static readonly IList<ModBossBar> bossBars = new List<ModBossBar>();

	internal static readonly IList<GlobalBossBar> globalBossBars = new List<GlobalBossBar>();

	internal static readonly List<ModBossBarStyle> bossBarStyles = new List<ModBossBarStyle> { vanillaStyle };

	internal static readonly int defaultStyleCount = bossBarStyles.Count;

	/// <summary>
	/// Only contains textures that exist.
	/// </summary>
	internal static readonly Dictionary<int, Asset<Texture2D>> bossBarTextures = new Dictionary<int, Asset<Texture2D>>();

	public static Asset<Texture2D> VanillaBossBarTexture => vanillaBossBarTexture ?? (vanillaBossBarTexture = Main.Assets.Request<Texture2D>("Images/UI/UI_BossBar"));

	public static ModBossBarStyle CurrentStyle { get; private set; } = vanillaStyle;


	internal static void Unload()
	{
		drawingInfo = null;
		vanillaBossBarTexture = null;
		styleLoading = true;
		bossBars.Clear();
		globalBossBars.Clear();
		lock (bossBarStyles)
		{
			bossBarStyles.RemoveRange(defaultStyleCount, bossBarStyles.Count - defaultStyleCount);
		}
		bossBarTextures.Clear();
	}

	internal static void AddBossBar(ModBossBar bossBar)
	{
		bossBar.index = bossBars.Count;
		bossBars.Add(bossBar);
		ModTypeLookup<ModBossBar>.Register(bossBar);
		if (ModContent.RequestIfExists(bossBar.Texture, out Asset<Texture2D> bossBarTexture, AssetRequestMode.AsyncLoad))
		{
			bossBarTextures[bossBar.index] = bossBarTexture;
		}
	}

	internal static void AddGlobalBossBar(GlobalBossBar globalBossBar)
	{
		globalBossBars.Add(globalBossBar);
		ModTypeLookup<GlobalBossBar>.Register(globalBossBar);
	}

	internal static void AddBossBarStyle(ModBossBarStyle bossBarStyle)
	{
		lock (bossBarStyles)
		{
			bossBarStyles.Add(bossBarStyle);
			ModTypeLookup<ModBossBarStyle>.Register(bossBarStyle);
		}
	}

	/// <summary>
	/// Sets the pending style that should be switched to
	/// </summary>
	/// <param name="bossBarStyle">Pending boss bar style</param>
	internal static void SwitchBossBarStyle(ModBossBarStyle bossBarStyle)
	{
		switchToStyle = bossBarStyle;
	}

	/// <summary>
	/// Sets the saved style that should be switched to, handles possibly unloaded/invalid ones and defaults to the vanilla style
	/// </summary>
	internal static void GotoSavedStyle()
	{
		switchToStyle = vanillaStyle;
		if (ModContent.TryFind<ModBossBarStyle>(lastSelectedStyle, out var value))
		{
			switchToStyle = value;
		}
		styleLoading = false;
	}

	/// <summary>
	/// Checks if the style was changed and applies it, saves the config if required
	/// </summary>
	internal static void HandleStyle()
	{
		if (switchToStyle != null && switchToStyle != CurrentStyle)
		{
			CurrentStyle.OnDeselected();
			CurrentStyle = switchToStyle;
			CurrentStyle.OnSelected();
		}
		switchToStyle = null;
		if (!styleLoading && CurrentStyle.FullName != lastSelectedStyle)
		{
			lastSelectedStyle = CurrentStyle.FullName;
			Main.SaveSettings();
		}
	}

	/// <summary>
	/// Returns the texture that the given bar is using. If it does not have a custom one, it returns the vanilla texture
	/// </summary>
	/// <param name="bossBar">The ModBossBar</param>
	/// <returns>Its texture, or the vanilla texture</returns>
	public static Asset<Texture2D> GetTexture(ModBossBar bossBar)
	{
		if (!bossBarTextures.TryGetValue(bossBar.index, out var texture))
		{
			return VanillaBossBarTexture;
		}
		return texture;
	}

	/// <summary>
	/// Gets the ModBossBar associated with this NPC
	/// </summary>
	/// <param name="npc">The NPC</param>
	/// <param name="value">When this method returns, contains the ModBossBar associated with the specified NPC</param>
	/// <returns><see langword="true" /> if a ModBossBar is assigned to it; otherwise, <see langword="false" />.</returns>
	public static bool NpcToBossBar(NPC npc, out ModBossBar value)
	{
		value = null;
		if (npc.BossBar is ModBossBar bossBar)
		{
			value = bossBar;
		}
		return value != null;
	}

	/// <summary>
	/// Inserts the boss bar style select option into the main and in-game menu under the "Interface" category
	/// </summary>
	internal static string InsertMenu(out Action onClick)
	{
		string styleText = null;
		ModBossBarStyle pendingBossBarStyle = null;
		foreach (ModBossBarStyle bossBarStyle in bossBarStyles)
		{
			if (bossBarStyle == CurrentStyle)
			{
				styleText = bossBarStyle.DisplayName;
				break;
			}
			pendingBossBarStyle = bossBarStyle;
		}
		if (pendingBossBarStyle == null)
		{
			pendingBossBarStyle = bossBarStyles.Last();
		}
		if (styleText == null || bossBarStyles.Count == 1)
		{
			styleText = Language.GetTextValue("tModLoader.BossBarStyleNoOptions");
		}
		onClick = delegate
		{
			SwitchBossBarStyle(pendingBossBarStyle);
		};
		return Language.GetTextValue("tModLoader.BossBarStyle", styleText);
	}

	public static bool PreDraw(SpriteBatch spriteBatch, BigProgressBarInfo info, ref BossBarDrawParams drawParams)
	{
		int index = info.npcIndexToAimAt;
		if (index < 0 || index > Main.maxNPCs)
		{
			return false;
		}
		NPC npc = Main.npc[index];
		ModBossBar bossBar;
		bool isModded = NpcToBossBar(npc, out bossBar);
		if (isModded)
		{
			drawParams.BarTexture = GetTexture(bossBar).Value;
		}
		bool modify = true;
		foreach (GlobalBossBar globalBossBar in globalBossBars)
		{
			modify &= globalBossBar.PreDraw(spriteBatch, npc, ref drawParams);
		}
		if (modify && isModded)
		{
			modify = bossBar.PreDraw(spriteBatch, npc, ref drawParams);
		}
		return modify;
	}

	public static void PostDraw(SpriteBatch spriteBatch, BigProgressBarInfo info, BossBarDrawParams drawParams)
	{
		int index = info.npcIndexToAimAt;
		if (index < 0 || index > Main.maxNPCs)
		{
			return;
		}
		NPC npc = Main.npc[index];
		if (NpcToBossBar(npc, out var bossBar))
		{
			bossBar.PostDraw(spriteBatch, npc, drawParams);
		}
		foreach (GlobalBossBar globalBossBar in globalBossBars)
		{
			globalBossBar.PostDraw(spriteBatch, npc, drawParams);
		}
	}

	/// <summary>
	/// Draws a healthbar with fixed barTexture dimensions (516x348) where the effective bar top left starts at 32x24, and is 456x22 big
	/// <para>The icon top left starts at 4x20, and is 26x28 big</para>
	/// <para>Frame 0 contains the frame (outline)</para>
	/// <para>Frame 1 contains the 2 pixel wide strip for the tip of the bar itself</para>
	/// <para>Frame 2 contains the 2 pixel wide strip for the bar itself, stretches out</para>
	/// <para>Frame 3 contains the background</para>
	/// <para>Frame 4 contains the 2 pixel wide strip for the tip of the bar itself (optional shield)</para>
	/// <para>Frame 5 contains the 2 pixel wide strip for the bar itself, stretches out (optional shield)</para>
	/// <para>Supply your own textures if you need a different shape/color, otherwise you can make your own method to draw it</para>
	/// </summary>
	/// <param name="spriteBatch">The spriteBatch that is drawn on</param>
	/// <param name="drawParams">The draw parameters for the boss bar</param>
	public static void DrawFancyBar_TML(SpriteBatch spriteBatch, BossBarDrawParams drawParams)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		BossBarDrawParams bossBarDrawParams = drawParams;
		var (barTexture, center, iconTexture, iconFrame, iconColor, life, lifeMax, shield, shieldMax, iconScale, showText, textOffset) = (BossBarDrawParams)(ref bossBarDrawParams);
		Point barSize = default(Point);
		((Point)(ref barSize))._002Ector(456, 22);
		Point topLeftOffset = default(Point);
		((Point)(ref topLeftOffset))._002Ector(32, 24);
		int frameCount = 6;
		Rectangle bgFrame = barTexture.Frame(1, frameCount, 0, 3);
		Color bgColor = Color.White * 0.2f;
		int scale = (int)((float)barSize.X * life / lifeMax);
		scale -= scale % 2;
		Rectangle barFrame = barTexture.Frame(1, frameCount, 0, 2);
		barFrame.X += topLeftOffset.X;
		barFrame.Y += topLeftOffset.Y;
		barFrame.Width = 2;
		barFrame.Height = barSize.Y;
		Rectangle tipFrame = barTexture.Frame(1, frameCount, 0, 1);
		tipFrame.X += topLeftOffset.X;
		tipFrame.Y += topLeftOffset.Y;
		tipFrame.Width = 2;
		tipFrame.Height = barSize.Y;
		int shieldScale = (int)((float)barSize.X * shield / shieldMax);
		shieldScale -= shieldScale % 2;
		Rectangle barShieldFrame = barTexture.Frame(1, frameCount, 0, 5);
		barShieldFrame.X += topLeftOffset.X;
		barShieldFrame.Y += topLeftOffset.Y;
		barShieldFrame.Width = 2;
		barShieldFrame.Height = barSize.Y;
		Rectangle tipShieldFrame = barTexture.Frame(1, frameCount, 0, 4);
		tipShieldFrame.X += topLeftOffset.X;
		tipShieldFrame.Y += topLeftOffset.Y;
		tipShieldFrame.Width = 2;
		tipShieldFrame.Height = barSize.Y;
		Rectangle barPosition = Utils.CenteredRectangle(center, barSize.ToVector2());
		Vector2 barTopLeft = barPosition.TopLeft();
		Vector2 topLeft = barTopLeft - topLeftOffset.ToVector2();
		spriteBatch.Draw(barTexture, topLeft, (Rectangle?)bgFrame, bgColor, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
		Vector2 stretchScale = default(Vector2);
		((Vector2)(ref stretchScale))._002Ector((float)(scale / barFrame.Width), 1f);
		Color barColor = Color.White;
		spriteBatch.Draw(barTexture, barTopLeft, (Rectangle?)barFrame, barColor, 0f, Vector2.Zero, stretchScale, (SpriteEffects)0, 0f);
		spriteBatch.Draw(barTexture, barTopLeft + new Vector2((float)(scale - 2), 0f), (Rectangle?)tipFrame, barColor, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
		if (shield > 0f)
		{
			((Vector2)(ref stretchScale))._002Ector((float)(shieldScale / barFrame.Width), 1f);
			spriteBatch.Draw(barTexture, barTopLeft, (Rectangle?)barShieldFrame, barColor, 0f, Vector2.Zero, stretchScale, (SpriteEffects)0, 0f);
			spriteBatch.Draw(barTexture, barTopLeft + new Vector2((float)(shieldScale - 2), 0f), (Rectangle?)tipShieldFrame, barColor, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
		}
		Rectangle frameFrame = barTexture.Frame(1, frameCount);
		spriteBatch.Draw(barTexture, topLeft, (Rectangle?)frameFrame, Color.White, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
		Vector2 val7 = new Vector2(4f, 20f);
		Vector2 iconSize = default(Vector2);
		((Vector2)(ref iconSize))._002Ector(26f, 28f);
		Vector2 iconPos = val7 + iconSize / 2f;
		spriteBatch.Draw(iconTexture, topLeft + iconPos, (Rectangle?)iconFrame, iconColor, 0f, iconFrame.Size() / 2f, iconScale, (SpriteEffects)0, 0f);
		if (BigProgressBarSystem.ShowText && showText)
		{
			if (shield > 0f)
			{
				BigProgressBarHelper.DrawHealthText(spriteBatch, barPosition, textOffset, shield, shieldMax);
			}
			else
			{
				BigProgressBarHelper.DrawHealthText(spriteBatch, barPosition, textOffset, life, lifeMax);
			}
		}
	}
}
