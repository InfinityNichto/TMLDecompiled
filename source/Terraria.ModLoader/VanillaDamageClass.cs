using Terraria.Localization;

namespace Terraria.ModLoader;

[Autoload(false)]
public abstract class VanillaDamageClass : DamageClass
{
	public override LocalizedText DisplayName => Language.GetText(LangKey);

	protected abstract string LangKey { get; }
}
