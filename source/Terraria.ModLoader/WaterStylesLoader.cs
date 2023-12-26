using System;
using Terraria.GameContent;
using Terraria.GameContent.Liquid;

namespace Terraria.ModLoader;

[Autoload(true, Side = ModSide.Client)]
public class WaterStylesLoader : SceneEffectLoader<ModWaterStyle>
{
	public WaterStylesLoader()
	{
		Initialize(15);
	}

	internal override void ResizeArrays()
	{
		Array.Resize(ref TextureAssets.Liquid, base.TotalCount);
		Array.Resize(ref TextureAssets.LiquidSlope, base.TotalCount);
		Array.Resize(ref LiquidRenderer.Instance._liquidTextures, base.TotalCount);
		Array.Resize(ref Main.liquidAlpha, base.TotalCount);
	}

	public override void ChooseStyle(out int style, out SceneEffectPriority priority)
	{
		int tst = Main.LocalPlayer.CurrentSceneEffect.waterStyle.value;
		style = -1;
		priority = SceneEffectPriority.None;
		if (tst >= base.VanillaCount)
		{
			style = tst;
			priority = Main.LocalPlayer.CurrentSceneEffect.waterStyle.priority;
		}
	}

	public void UpdateLiquidAlphas()
	{
		if (Main.waterStyle >= base.VanillaCount)
		{
			for (int i = 0; i < base.VanillaCount; i++)
			{
				if (i != 1 && i != 11)
				{
					Main.liquidAlpha[i] -= 0.2f;
					if (Main.liquidAlpha[i] < 0f)
					{
						Main.liquidAlpha[i] = 0f;
					}
				}
			}
		}
		foreach (ModWaterStyle item in list)
		{
			int type = item.Slot;
			if (Main.waterStyle == type)
			{
				Main.liquidAlpha[type] += 0.2f;
				if (Main.liquidAlpha[type] > 1f)
				{
					Main.liquidAlpha[type] = 1f;
				}
			}
			else
			{
				Main.liquidAlpha[type] -= 0.2f;
				if (Main.liquidAlpha[type] < 0f)
				{
					Main.liquidAlpha[type] = 0f;
				}
			}
		}
	}

	public void DrawWaterfall(WaterfallManager waterfallManager)
	{
		foreach (ModWaterStyle waterStyle in list)
		{
			if (Main.liquidAlpha[waterStyle.Slot] > 0f)
			{
				waterfallManager.DrawWaterfall(waterStyle.ChooseWaterfallStyle(), Main.liquidAlpha[waterStyle.Slot]);
			}
		}
	}

	public void LightColorMultiplier(int style, float factor, ref float r, ref float g, ref float b)
	{
		ModWaterStyle waterStyle = Get(style);
		if (waterStyle != null)
		{
			waterStyle.LightColorMultiplier(ref r, ref g, ref b);
			r *= factor;
			g *= factor;
			b *= factor;
		}
	}
}
