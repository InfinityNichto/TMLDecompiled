namespace Terraria.ModLoader.Default;

public abstract class ModLoaderModItem : ModItem
{
	public override string Texture => "ModLoader/" + base.Texture.Substring("Terraria.ModLoader.Default.".Length).Replace('/', '.');
}
