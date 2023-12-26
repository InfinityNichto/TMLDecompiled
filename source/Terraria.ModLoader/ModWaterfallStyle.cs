using Microsoft.Xna.Framework.Graphics;

namespace Terraria.ModLoader;

/// <summary>
/// Represents a style of waterfalls that gets drawn. This is mostly used to determine the color of the waterfall.
/// </summary>
[Autoload(true, Side = ModSide.Client)]
public abstract class ModWaterfallStyle : ModTexturedType
{
	/// <summary>
	/// The ID of this waterfall style.
	/// </summary>
	public int Slot { get; internal set; }

	protected sealed override void Register()
	{
		Slot = LoaderManager.Get<WaterFallStylesLoader>().Register(this);
	}

	public sealed override void SetupContent()
	{
		Main.instance.waterfallManager.waterfallTexture[Slot] = ModContent.Request<Texture2D>(Texture);
		SetStaticDefaults();
	}

	/// <summary>
	/// Allows you to create light at a tile occupied by a waterfall of this style.
	/// </summary>
	public virtual void AddLight(int i, int j)
	{
	}

	/// <summary>
	/// Allows you to determine the color multiplier acting on waterfalls of this style. Useful for waterfalls whose colors change over time.
	/// </summary>
	public virtual void ColorMultiplier(ref float r, ref float g, ref float b, float a)
	{
	}
}
