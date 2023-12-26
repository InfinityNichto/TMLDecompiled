using Microsoft.Xna.Framework.Graphics;

namespace Terraria.ModLoader;

/// <summary>
/// Each background style determines in its own way how exactly the background is drawn. This class serves as a collection of functions for above-ground backgrounds.
/// </summary>
public abstract class ModSurfaceBackgroundStyle : ModBackgroundStyle
{
	protected sealed override void Register()
	{
		base.Slot = LoaderManager.Get<SurfaceBackgroundStylesLoader>().Register(this);
	}

	public sealed override void SetupContent()
	{
		SetStaticDefaults();
	}

	/// <summary>
	/// Allows you to modify the transparency of all background styles that exist. In general, you should move the index equal to this style's slot closer to 1, and all other indexes closer to 0. The transitionSpeed parameter is what you should add/subtract to each element of the fades parameter. See the ExampleMod for an example.
	/// </summary>
	public abstract void ModifyFarFades(float[] fades, float transitionSpeed);

	/// <summary>
	/// Allows you to determine which texture is drawn in the very back of the background. BackgroundTextureLoader.GetBackgroundSlot may be useful here, as well as for the other texture-choosing hooks.
	/// </summary>
	public virtual int ChooseFarTexture()
	{
		return -1;
	}

	/// <summary>
	/// Allows you to determine which texture is drawn in the middle of the background.
	/// </summary>
	public virtual int ChooseMiddleTexture()
	{
		return -1;
	}

	/// <summary>
	/// Gives you complete freedom over how the closest part of the background is drawn. Return true for ChooseCloseTexture to have an effect; return false to disable tModLoader's own code for drawing the close background.
	/// </summary>
	public virtual bool PreDrawCloseBackground(SpriteBatch spriteBatch)
	{
		return true;
	}

	/// <summary>
	/// Allows you to determine which texture is drawn in the closest part of the background. This also lets you modify the scale and parallax (as well as two unfortunately-unknown parameters).
	/// </summary>
	/// <param name="scale">The scale.</param>
	/// <param name="parallax">The parallax value.</param>
	/// <param name="a">a?</param>
	/// <param name="b">b?</param>
	/// <returns></returns>
	public virtual int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
	{
		return -1;
	}
}
