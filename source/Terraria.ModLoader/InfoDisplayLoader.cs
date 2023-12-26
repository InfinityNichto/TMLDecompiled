using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.ModLoader;

public static class InfoDisplayLoader
{
	internal static readonly List<InfoDisplay> InfoDisplays;

	internal static readonly int DefaultDisplayCount;

	public static int InfoDisplayPage;

	internal static readonly IList<GlobalInfoDisplay> globalInfoDisplays;

	public static int InfoDisplayCount => InfoDisplays.Count;

	static InfoDisplayLoader()
	{
		InfoDisplays = new List<InfoDisplay>
		{
			InfoDisplay.Watches,
			InfoDisplay.WeatherRadio,
			InfoDisplay.Sextant,
			InfoDisplay.FishFinder,
			InfoDisplay.MetalDetector,
			InfoDisplay.LifeformAnalyzer,
			InfoDisplay.Radar,
			InfoDisplay.TallyCounter,
			InfoDisplay.Dummy,
			InfoDisplay.DPSMeter,
			InfoDisplay.Stopwatch,
			InfoDisplay.Compass,
			InfoDisplay.DepthMeter
		};
		DefaultDisplayCount = InfoDisplays.Count;
		InfoDisplayPage = 0;
		globalInfoDisplays = new List<GlobalInfoDisplay>();
		RegisterDefaultDisplays();
	}

	internal static int Add(InfoDisplay infoDisplay)
	{
		InfoDisplays.Add(infoDisplay);
		return InfoDisplays.Count - 1;
	}

	internal static void Unload()
	{
		InfoDisplays.RemoveRange(DefaultDisplayCount, InfoDisplays.Count - DefaultDisplayCount);
		globalInfoDisplays.Clear();
	}

	internal static void RegisterDefaultDisplays()
	{
		int i = 0;
		foreach (InfoDisplay infoDisplay in InfoDisplays)
		{
			infoDisplay.Type = i++;
			ContentInstance.Register(infoDisplay);
			ModTypeLookup<InfoDisplay>.Register(infoDisplay);
		}
	}

	public static int ActiveDisplays()
	{
		int activeDisplays = 0;
		for (int i = 0; i < InfoDisplays.Count; i++)
		{
			if (InfoDisplays[i].Active())
			{
				activeDisplays++;
			}
		}
		return activeDisplays;
	}

	public static void AddGlobalInfoDisplay(GlobalInfoDisplay globalInfoDisplay)
	{
		globalInfoDisplays.Add(globalInfoDisplay);
		ModTypeLookup<GlobalInfoDisplay>.Register(globalInfoDisplay);
	}

	public static int ActivePages()
	{
		int activePages = 1;
		for (int activeDisplays = ActiveDisplays(); activeDisplays > 12; activeDisplays -= 12)
		{
			activePages++;
		}
		return activePages;
	}

	public static bool Active(InfoDisplay info)
	{
		bool active = info.Active();
		foreach (GlobalInfoDisplay globalInfoDisplay in globalInfoDisplays)
		{
			bool? val = globalInfoDisplay.Active(info);
			if (val.HasValue)
			{
				active &= val.Value;
			}
		}
		return active;
	}

	[Obsolete("Use ModifyDisplayParameters instead")]
	public static void ModifyDisplayName(InfoDisplay info, ref string displayName)
	{
		foreach (GlobalInfoDisplay globalInfoDisplay in globalInfoDisplays)
		{
			globalInfoDisplay.ModifyDisplayName(info, ref displayName);
		}
	}

	[Obsolete("Use ModifyDisplayParameters instead")]
	public static void ModifyDisplayValue(InfoDisplay info, ref string displayName)
	{
		foreach (GlobalInfoDisplay globalInfoDisplay in globalInfoDisplays)
		{
			globalInfoDisplay.ModifyDisplayValue(info, ref displayName);
		}
	}

	[Obsolete("Use ModifyDisplayParameters instead")]
	public static void ModifyDisplayColor(InfoDisplay info, ref Color displayColor, ref Color displayShadowColor)
	{
		foreach (GlobalInfoDisplay globalInfoDisplay in globalInfoDisplays)
		{
			globalInfoDisplay.ModifyDisplayColor(info, ref displayColor, ref displayShadowColor);
		}
	}

	public static void ModifyDisplayParameters(InfoDisplay info, ref string displayValue, ref string displayName, ref Color displayColor, ref Color displayShadowColor)
	{
		foreach (GlobalInfoDisplay globalInfoDisplay in globalInfoDisplays)
		{
			globalInfoDisplay.ModifyDisplayParameters(info, ref displayValue, ref displayName, ref displayColor, ref displayShadowColor);
		}
	}
}
