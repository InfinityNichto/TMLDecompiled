using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.GameContent.Liquid;

namespace Terraria.ModLoader;

/// <summary>
/// Represents a style of water that gets drawn, based on factors such as the background. This is used to determine the color of the water, as well as other things as determined by the hooks below.
/// </summary>
[Autoload(true, Side = ModSide.Client)]
public abstract class ModWaterStyle : ModTexturedType
{
	/// <summary>
	/// The ID of the water style.
	/// </summary>
	public int Slot { get; internal set; }

	public virtual string BlockTexture => Texture + "_Block";

	public virtual string SlopeTexture => Texture + "_Slope";

	protected sealed override void Register()
	{
		Slot = LoaderManager.Get<WaterStylesLoader>().Register(this);
	}

	public sealed override void SetupContent()
	{
		LiquidRenderer.Instance._liquidTextures[Slot] = ModContent.Request<Texture2D>(Texture);
		SetStaticDefaults();
		TextureAssets.Liquid[Slot] = ModContent.Request<Texture2D>(BlockTexture);
		if (base.Mod.TModLoaderVersion < new Version(2023, 6, 24))
		{
			TextureAssets.LiquidSlope[Slot] = ModContent.Request<Texture2D>(BlockTexture);
		}
		else
		{
			TextureAssets.LiquidSlope[Slot] = ModContent.Request<Texture2D>(SlopeTexture);
		}
	}

	/// <summary>
	/// The ID of the waterfall style the game should use when this water style is in use.
	/// </summary>
	public abstract int ChooseWaterfallStyle();

	/// <summary>
	/// The ID of the dust that is created when anything splashes in water.
	/// </summary>
	public abstract int GetSplashDust();

	/// <summary>
	/// The ID of the gore that represents droplets of water falling down from a block.
	/// </summary>
	public abstract int GetDropletGore();

	/// <summary>
	/// Allows you to modify the light levels of the tiles behind the water. The light color components will be multiplied by the parameters.
	/// </summary>
	public virtual void LightColorMultiplier(ref float r, ref float g, ref float b)
	{
		r = 0.88f;
		g = 0.96f;
		b = 1.015f;
	}

	/// <summary>
	/// Allows you to change the hair color resulting from the biome hair dye when this water style is in use.
	/// </summary>
	public virtual Color BiomeHairColor()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		return new Color(28, 216, 94);
	}

	/// <summary>
	/// Returns the texture to be used when drawing rain of this water type.
	/// <br />Default uses the vanilla rain texture.
	/// </summary>
	public virtual Asset<Texture2D> GetRainTexture()
	{
		return TextureAssets.Rain;
	}

	/// <summary>
	/// Return the variant of rain used. Equal to the offset in the rain texture divided by four.
	/// <br />Vanilla rain has three variants per biome, and so vanilla variants range from 0 to 3 * Main.maxLiquidTextures.
	/// <br />Default is a random number from 0 to 2, which creates normal vanilla forest biome rain.
	/// </summary>
	public virtual byte GetRainVariant()
	{
		return (byte)Main.rand.Next(3);
	}
}
