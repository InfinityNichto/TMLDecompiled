using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.ModLoader.UI;

namespace Terraria.Initializers;

public class LinkButtonsInitializer
{
	public static void Load()
	{
		List<TitleLinkButton> titleLinks = Main.TitleLinks;
		titleLinks.Add(MakeSimpleButton("TitleLinks.Discord", "https://discord.gg/terraria", 0));
		titleLinks.Add(MakeSimpleButton("TitleLinks.Instagram", "https://www.instagram.com/terraria_logic/", 1));
		titleLinks.Add(MakeSimpleButton("TitleLinks.Reddit", "https://www.reddit.com/r/Terraria/", 2));
		titleLinks.Add(MakeSimpleButton("TitleLinks.Twitter", "https://twitter.com/Terraria_Logic", 3));
		titleLinks.Add(MakeSimpleButton("TitleLinks.Forums", "https://forums.terraria.org/index.php", 4));
		titleLinks.Add(MakeSimpleButton("TitleLinks.Merch", "https://terraria.org/store", 5));
		titleLinks.Add(MakeSimpleButton("TitleLinks.Wiki", "https://terraria.wiki.gg/", 6));
		List<TitleLinkButton> tModLoaderTitleLinks = Main.tModLoaderTitleLinks;
		tModLoaderTitleLinks.Add(MakeSimpleButton("TitleLinks.Discord", "https://tmodloader.net/discord", 0));
		tModLoaderTitleLinks.Add(MakeSimpleButton("TitleLinks.Twitter", "https://twitter.com/tModLoader", 3));
		tModLoaderTitleLinks.Add(MakeSimpleButton("TitleLinks.Wiki", "https://github.com/tModLoader/tModLoader/wiki", 6));
		tModLoaderTitleLinks.Add(MakeSimpleButton("TitleLinks.Patreon", "https://www.patreon.com/tmodloader", 7));
	}

	private static TitleLinkButton MakeSimpleButton(string textKey, string linkUrl, int horizontalFrameIndex)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		Asset<Texture2D> asset = UICommon.tModLoaderTitleLinkButtonsTexture;
		Rectangle value = asset.Frame(8, 2, horizontalFrameIndex);
		Rectangle value2 = asset.Frame(8, 2, horizontalFrameIndex, 1);
		value.Width--;
		value.Height--;
		value2.Width--;
		value2.Height--;
		return new TitleLinkButton
		{
			TooltipTextKey = textKey,
			LinkUrl = linkUrl,
			FrameWehnSelected = value2,
			FrameWhenNotSelected = value,
			Image = asset
		};
	}
}
