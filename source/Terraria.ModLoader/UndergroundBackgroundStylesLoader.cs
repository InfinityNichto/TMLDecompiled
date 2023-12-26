using System;

namespace Terraria.ModLoader;

/// <summary>
/// This serves as the central class from which ModUndergroundBackgroundStyle functions are supported and carried out.
/// </summary>
[Autoload(true, Side = ModSide.Client)]
public class UndergroundBackgroundStylesLoader : SceneEffectLoader<ModUndergroundBackgroundStyle>
{
	public const int VanillaUndergroundBackgroundStylesCount = 22;

	public UndergroundBackgroundStylesLoader()
	{
		Initialize(22);
	}

	public override void ChooseStyle(out int style, out SceneEffectPriority priority)
	{
		priority = SceneEffectPriority.None;
		style = -1;
		if (GlobalBackgroundStyleLoader.loaded)
		{
			int playerUndergroundBackground = Main.LocalPlayer.CurrentSceneEffect.undergroundBackground.value;
			if (playerUndergroundBackground >= base.VanillaCount)
			{
				style = playerUndergroundBackground;
				priority = Main.LocalPlayer.CurrentSceneEffect.undergroundBackground.priority;
			}
		}
	}

	public void FillTextureArray(int style, int[] textureSlots)
	{
		if (GlobalBackgroundStyleLoader.loaded)
		{
			Get(style)?.FillTextureArray(textureSlots);
			Action<int, int[]>[] hookFillUndergroundTextureArray = GlobalBackgroundStyleLoader.HookFillUndergroundTextureArray;
			for (int i = 0; i < hookFillUndergroundTextureArray.Length; i++)
			{
				hookFillUndergroundTextureArray[i](style, textureSlots);
			}
		}
	}
}
