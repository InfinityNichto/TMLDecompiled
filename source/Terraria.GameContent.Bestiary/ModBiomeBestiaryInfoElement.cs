using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary;

public class ModBiomeBestiaryInfoElement : ModBestiaryInfoElement, IBestiaryBackgroundImagePathAndColorProvider
{
	public ModBiomeBestiaryInfoElement(Mod mod, string displayName, string iconPath, string backgroundPath, Color? backgroundColor)
	{
		_mod = mod;
		_displayName = displayName;
		_iconPath = iconPath;
		_backgroundPath = backgroundPath;
		_backgroundColor = backgroundColor;
	}

	public override UIElement GetFilterImage()
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		if (_iconPath != null && ModContent.RequestIfExists(_iconPath, out Asset<Texture2D> asset, AssetRequestMode.ImmediateLoad))
		{
			if (asset.Size() == new Vector2(30f))
			{
				return new UIImage(asset)
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
			_mod.Logger.Info((object)(_iconPath + " needs to be 30x30 pixels."));
		}
		asset = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Tags_Shadow");
		return new UIImageFramed(asset, asset.Frame(16, 5, 0, 4))
		{
			HAlign = 0.5f,
			VAlign = 0.5f
		};
	}

	public Asset<Texture2D> GetBackgroundImage()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		if (_backgroundPath == null || !ModContent.RequestIfExists(_backgroundPath, out Asset<Texture2D> asset, AssetRequestMode.ImmediateLoad))
		{
			return null;
		}
		if (asset.Size() == new Vector2(115f, 65f))
		{
			return asset;
		}
		_mod.Logger.Info((object)(_backgroundPath + " needs to be 115x65 pixels."));
		return null;
	}

	public Color? GetBackgroundColor()
	{
		return _backgroundColor;
	}
}
