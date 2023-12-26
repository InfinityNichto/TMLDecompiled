namespace Terraria.ModLoader.Core;

internal class ModMemoryUsage
{
	internal long managed;

	internal long sounds;

	internal long textures;

	internal long code;

	internal long total => managed + code + sounds + textures;
}
