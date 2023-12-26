using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.ModLoader.Default;

/// <summary>
/// The Journey's End theme converted into a ModMenu, so that it better fits with the new system.
/// </summary>
[Autoload(false)]
internal class MenuJourneysEnd : ModMenu
{
	public override string DisplayName => "Journey's End";

	public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
	{
		return false;
	}
}
