namespace Terraria.ModLoader;

/// <summary>
/// Each background style determines in its own way how exactly the background is drawn. This class serves as a collection of functions for underground backgrounds.
/// </summary>
public abstract class ModUndergroundBackgroundStyle : ModBackgroundStyle
{
	protected sealed override void Register()
	{
		base.Slot = LoaderManager.Get<UndergroundBackgroundStylesLoader>().Register(this);
	}

	public sealed override void SetupContent()
	{
		SetStaticDefaults();
	}

	/// <summary>
	/// Allows you to determine which textures make up the background by assigning their background slots/IDs to the given array. BackgroundTextureLoader.GetBackgroundSlot may be useful here. Index 0 is the texture on the border of the ground and sky layers. Index 1 is the texture drawn between rock and ground layers. Index 2 is the texture on the border of ground and rock layers. Index 3 is the texture drawn in the rock layer. The border images are 160x16 pixels, and the others are 160x96, but it seems like the right 32 pixels of each is a duplicate of the far left 32 pixels.
	/// </summary>
	public abstract void FillTextureArray(int[] textureSlots);
}
