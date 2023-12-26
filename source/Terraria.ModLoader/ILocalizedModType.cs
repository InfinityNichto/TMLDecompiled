namespace Terraria.ModLoader;

public interface ILocalizedModType : IModType
{
	/// <summary>
	/// The category used by this modded content for use in localization keys. Localization keys follow the pattern of "Mods.{ModName}.{Category}.{ContentName}.{DataName}". The <see href="https://github.com/tModLoader/tModLoader/wiki/Localization#modtype-and-ilocalizedmodtype">Localization wiki page</see> explains how custom <see cref="T:Terraria.ModLoader.ModType" /> classes can utilize this.
	/// </summary>
	string LocalizationCategory { get; }
}
