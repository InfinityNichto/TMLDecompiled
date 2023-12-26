using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary;

public class ModSourceBestiaryInfoElement : ModBestiaryInfoElement
{
	public ModSourceBestiaryInfoElement(Mod mod, string displayName)
	{
		_mod = mod;
		_displayName = displayName;
	}

	public override UIElement GetFilterImage()
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		Asset<Texture2D> asset;
		if (_mod.HasAsset("icon_small"))
		{
			asset = _mod.Assets.Request<Texture2D>("icon_small");
			if (asset.Size() == new Vector2(30f))
			{
				return new UIImage(asset)
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
			_mod.Logger.Info((object)"icon_small needs to be 30x30 pixels.");
		}
		asset = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Tags_Shadow");
		return new UIImageFramed(asset, asset.Frame(16, 5, 0, 4))
		{
			HAlign = 0.5f,
			VAlign = 0.5f
		};
	}
}
