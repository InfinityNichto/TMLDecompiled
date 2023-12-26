using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.ModLoader.UI;

public static class UICommon
{
	public static Color DefaultUIBlue = new Color(73, 94, 171);

	public static Color DefaultUIBlueMouseOver = new Color(63, 82, 151) * 0.7f;

	public static Color DefaultUIBorder = Color.Black;

	public static Color DefaultUIBorderMouseOver = Colors.FancyUIFatButtonMouseOver;

	public static Color MainPanelBackground = new Color(33, 43, 79) * 0.8f;

	public static StyleDimension MaxPanelWidth = new StyleDimension(600f, 0f);

	public static Asset<Texture2D> ButtonErrorTexture { get; internal set; }

	public static Asset<Texture2D> ButtonConfigTexture { get; internal set; }

	public static Asset<Texture2D> ButtonPlusTexture { get; internal set; }

	public static Asset<Texture2D> ButtonUpDownTexture { get; internal set; }

	public static Asset<Texture2D> ButtonCollapsedTexture { get; internal set; }

	public static Asset<Texture2D> ButtonExpandedTexture { get; internal set; }

	public static Asset<Texture2D> ModBrowserIconsTexture { get; internal set; }

	public static Asset<Texture2D> ButtonExclamationTexture { get; internal set; }

	public static Asset<Texture2D> ButtonTranslationModTexture { get; internal set; }

	public static Asset<Texture2D> LoaderTexture { get; internal set; }

	public static Asset<Texture2D> LoaderBgTexture { get; internal set; }

	public static Asset<Texture2D> ButtonDownloadTexture { get; internal set; }

	public static Asset<Texture2D> ButtonDowngradeTexture { get; internal set; }

	public static Asset<Texture2D> ButtonDownloadMultipleTexture { get; internal set; }

	public static Asset<Texture2D> ButtonModInfoTexture { get; internal set; }

	public static Asset<Texture2D> ButtonModConfigTexture { get; internal set; }

	public static Asset<Texture2D> DividerTexture { get; internal set; }

	public static Asset<Texture2D> InnerPanelTexture { get; internal set; }

	public static Asset<Texture2D> InfoDisplayPageArrowTexture { get; internal set; }

	public static Asset<Texture2D> tModLoaderTitleLinkButtonsTexture { get; internal set; }

	public static Asset<Texture2D> CopyCodeButtonTexture { get; internal set; }

	public static T WithFadedMouseOver<T>(this T elem, Color overColor = default(Color), Color outColor = default(Color), Color overBorderColor = default(Color), Color outBorderColor = default(Color)) where T : UIPanel
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		if (overColor == default(Color))
		{
			overColor = DefaultUIBlue;
		}
		if (outColor == default(Color))
		{
			outColor = DefaultUIBlueMouseOver;
		}
		if (overBorderColor == default(Color))
		{
			overBorderColor = DefaultUIBorderMouseOver;
		}
		if (outBorderColor == default(Color))
		{
			outBorderColor = DefaultUIBorder;
		}
		elem.OnMouseOver += delegate
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			SoundEngine.PlaySound(in SoundID.MenuTick);
			elem.BackgroundColor = overColor;
			elem.BorderColor = overBorderColor;
		};
		elem.OnMouseOut += delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			elem.BackgroundColor = outColor;
			elem.BorderColor = outBorderColor;
		};
		return elem;
	}

	public static T WithPadding<T>(this T elem, float pixels) where T : UIElement
	{
		elem.SetPadding(pixels);
		return elem;
	}

	public static T WithPadding<T>(this T elem, string name, int id, Vector2? anchor = null, Vector2? offset = null) where T : UIElement
	{
		elem.SetSnapPoint(name, id, anchor, offset);
		return elem;
	}

	public static T WithView<T>(this T elem, float viewSize, float maxViewSize) where T : UIScrollbar
	{
		elem.SetView(viewSize, maxViewSize);
		return elem;
	}

	public static void AddOrRemoveChild(this UIElement elem, UIElement child, bool add)
	{
		if (!add)
		{
			elem.RemoveChild(child);
		}
		else if (!elem.HasChild(child))
		{
			elem.Append(child);
		}
	}

	public static void DrawHoverStringInBounds(SpriteBatch spriteBatch, string text, Rectangle? bounds = null)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		if (!bounds.HasValue)
		{
			bounds = new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);
		}
		Vector2 stringSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, text, Vector2.One);
		Vector2 vector = Main.MouseScreen + new Vector2(16f);
		float x = vector.X;
		Rectangle value = bounds.Value;
		vector.X = Math.Min(x, (float)((Rectangle)(ref value)).Right - stringSize.X - 16f);
		float y = vector.Y;
		value = bounds.Value;
		vector.Y = Math.Min(y, (float)((Rectangle)(ref value)).Bottom - stringSize.Y - 16f);
		Color color = default(Color);
		((Color)(ref color))._002Ector((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, 255);
		ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, text, vector, color, 0f, Vector2.Zero, Vector2.One);
	}

	/// <summary>
	/// Functions like Main.instance.MouseText, but adds the same background seen in tooltips to the text
	/// </summary>
	/// <param name="text"></param>
	public static void TooltipMouseText(string text)
	{
		if (Main.SettingsEnabled_OpaqueBoxBehindTooltips)
		{
			Item item = new Item();
			item.SetDefaults(0, noMatCheck: true);
			item.SetNameOverride(text);
			item.type = 1;
			item.scale = 0f;
			item.rare = 0;
			item.value = -1;
			Main.HoverItem = item;
			Main.instance.MouseText("", 0, 0);
			Main.mouseText = true;
		}
		else
		{
			Main.instance.MouseText(text, 0, 0);
		}
	}

	internal static void LoadTextures()
	{
		ButtonErrorTexture = LoadEmbeddedTexture("UI.ButtonError");
		ButtonPlusTexture = LoadEmbeddedTexture("Config.UI.ButtonPlus");
		ButtonUpDownTexture = LoadEmbeddedTexture("Config.UI.ButtonUpDown");
		ButtonCollapsedTexture = LoadEmbeddedTexture("Config.UI.ButtonCollapsed");
		ButtonExpandedTexture = LoadEmbeddedTexture("Config.UI.ButtonExpanded");
		ModBrowserIconsTexture = LoadEmbeddedTexture("UI.UIModBrowserIcons");
		ButtonExclamationTexture = LoadEmbeddedTexture("UI.ButtonExclamation");
		ButtonTranslationModTexture = LoadEmbeddedTexture("UI.ButtonTranslationMod");
		LoaderTexture = LoadEmbeddedTexture("UI.Loader");
		LoaderBgTexture = LoadEmbeddedTexture("UI.LoaderBG");
		ButtonDownloadTexture = LoadEmbeddedTexture("UI.ButtonDownload");
		ButtonDowngradeTexture = LoadEmbeddedTexture("UI.ButtonDowngrade");
		ButtonDownloadMultipleTexture = LoadEmbeddedTexture("UI.ButtonDownloadMultiple");
		ButtonModInfoTexture = LoadEmbeddedTexture("UI.ButtonModInfo");
		ButtonModConfigTexture = LoadEmbeddedTexture("UI.ButtonModConfig");
		DividerTexture = Main.Assets.Request<Texture2D>("Images/UI/Divider");
		InnerPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground");
		InfoDisplayPageArrowTexture = LoadEmbeddedTexture("UI.InfoDisplayPageArrow");
		tModLoaderTitleLinkButtonsTexture = LoadEmbeddedTexture("UI.tModLoaderTitleLinkButtons");
		CopyCodeButtonTexture = LoadEmbeddedTexture("UI.CopyCodeButton");
		static Asset<Texture2D> LoadEmbeddedTexture(string name)
		{
			return ModLoader.ManifestAssets.Request<Texture2D>("Terraria.ModLoader." + name);
		}
	}
}
