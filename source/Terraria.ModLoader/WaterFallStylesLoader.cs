using System;

namespace Terraria.ModLoader;

[Autoload(true, Side = ModSide.Client)]
public class WaterFallStylesLoader : SceneEffectLoader<ModWaterfallStyle>
{
	public WaterFallStylesLoader()
	{
		Initialize(26);
	}

	internal override void ResizeArrays()
	{
		Array.Resize(ref Main.instance.waterfallManager.waterfallTexture, base.TotalCount);
	}
}
