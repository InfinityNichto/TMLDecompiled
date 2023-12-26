using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.ModLoader.Default;

/// <summary>
/// This is the default modmenu - the one that tML uses and the default one upon entering the game for the first time.
/// </summary>
[Autoload(false)]
internal class MenutML : ModMenu
{
	public override string DisplayName => "tModLoader";

	public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
	{
		logoScale *= 0.84f;
		return true;
	}
}
