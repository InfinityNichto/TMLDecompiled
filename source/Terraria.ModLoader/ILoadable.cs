namespace Terraria.ModLoader;

/// <summary>
/// Allows for implementing types to be loaded and unloaded.
/// </summary>
public interface ILoadable
{
	/// <summary>
	/// Called when loading the type.
	/// </summary>
	/// <param name="mod">The mod instance associated with this type.</param>
	void Load(Mod mod);

	/// <summary>
	/// Whether or not this type should be loaded when it's told to. Returning false disables <see cref="M:Terraria.ModLoader.Mod.AddContent(Terraria.ModLoader.ILoadable)" /> from actually loading this type.
	/// </summary>
	/// <param name="mod">The mod instance trying to add this content</param>
	bool IsLoadingEnabled(Mod mod)
	{
		return true;
	}

	/// <summary>
	/// Called during unloading when needed.
	/// </summary>
	void Unload();
}
