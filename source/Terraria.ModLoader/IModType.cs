namespace Terraria.ModLoader;

public interface IModType
{
	/// <summary>
	///  The mod this belongs to.
	///  </summary>
	Mod Mod { get; }

	/// <summary>
	/// The internal name of this instance.
	/// </summary>
	string Name { get; }

	/// <summary>
	/// =&gt; $"{Mod.Name}/{Name}"
	/// </summary>
	string FullName { get; }
}
