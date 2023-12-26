using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.ModLoader;

[Autoload(false)]
public abstract class VanillaInfoDisplay : InfoDisplay
{
	public override LocalizedText DisplayName => Language.GetText(LangKey);

	public override string HoverTexture => InfoDisplay.VanillaHoverTexture;

	protected abstract string LangKey { get; }

	public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
	{
		return "";
	}
}
