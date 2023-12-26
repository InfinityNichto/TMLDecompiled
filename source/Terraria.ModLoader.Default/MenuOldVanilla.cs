using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.ModLoader.Default;

/// <summary>
/// The Terraria 1.3.5.3 theme converted into a ModMenu, so that it better fits with the new system.
/// </summary>
[Autoload(false)]
internal class MenuOldVanilla : ModMenu
{
	public override bool IsAvailable => Main.instance.playOldTile;

	public override string DisplayName => "Terraria 1.3.5.3";

	public override int Music => 6;

	public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
	{
		return false;
	}
}
