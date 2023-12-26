using System;
using System.Collections.Generic;

namespace Terraria.ModLoader;

internal static class GlobalBackgroundStyleLoader
{
	internal delegate void DelegateChooseUndergroundBackgroundStyle(ref int style);

	internal delegate void DelegateChooseSurfaceBackgroundStyle(ref int style);

	internal static readonly IList<GlobalBackgroundStyle> globalBackgroundStyles = new List<GlobalBackgroundStyle>();

	internal static bool loaded = false;

	internal static DelegateChooseUndergroundBackgroundStyle[] HookChooseUndergroundBackgroundStyle;

	internal static DelegateChooseSurfaceBackgroundStyle[] HookChooseSurfaceBackgroundStyle;

	internal static Action<int, int[]>[] HookFillUndergroundTextureArray;

	internal static Action<int, float[], float>[] HookModifyFarSurfaceFades;

	internal static void ResizeAndFillArrays(bool unloading = false)
	{
		ModLoader.BuildGlobalHook<GlobalBackgroundStyle, DelegateChooseUndergroundBackgroundStyle>(ref HookChooseUndergroundBackgroundStyle, globalBackgroundStyles, (GlobalBackgroundStyle g) => g.ChooseUndergroundBackgroundStyle);
		ModLoader.BuildGlobalHook<GlobalBackgroundStyle, DelegateChooseSurfaceBackgroundStyle>(ref HookChooseSurfaceBackgroundStyle, globalBackgroundStyles, (GlobalBackgroundStyle g) => g.ChooseSurfaceBackgroundStyle);
		ModLoader.BuildGlobalHook<GlobalBackgroundStyle, Action<int, int[]>>(ref HookFillUndergroundTextureArray, globalBackgroundStyles, (GlobalBackgroundStyle g) => g.FillUndergroundTextureArray);
		ModLoader.BuildGlobalHook<GlobalBackgroundStyle, Action<int, float[], float>>(ref HookModifyFarSurfaceFades, globalBackgroundStyles, (GlobalBackgroundStyle g) => g.ModifyFarSurfaceFades);
		if (!unloading)
		{
			loaded = true;
		}
	}

	internal static void Unload()
	{
		loaded = false;
		globalBackgroundStyles.Clear();
	}
}
