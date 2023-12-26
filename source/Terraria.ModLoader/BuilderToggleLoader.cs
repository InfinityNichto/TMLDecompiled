using System.Collections.Generic;

namespace Terraria.ModLoader;

public static class BuilderToggleLoader
{
	internal static readonly List<BuilderToggle> BuilderToggles;

	internal static readonly int DefaultDisplayCount;

	public static int BuilderTogglePage;

	public static int BuilderToggleCount => BuilderToggles.Count;

	static BuilderToggleLoader()
	{
		BuilderToggles = new List<BuilderToggle>
		{
			BuilderToggle.BlockSwap,
			BuilderToggle.TorchBiome,
			BuilderToggle.HideAllWires,
			BuilderToggle.ActuatorsVisibility,
			BuilderToggle.RulerLine,
			BuilderToggle.RulerGrid,
			BuilderToggle.AutoActuate,
			BuilderToggle.AutoPaint,
			BuilderToggle.RedWireVisibility,
			BuilderToggle.BlueWireVisibility,
			BuilderToggle.GreenWireVisibility,
			BuilderToggle.YellowWireVisibility
		};
		DefaultDisplayCount = BuilderToggles.Count;
		BuilderTogglePage = 0;
		RegisterDefaultToggles();
	}

	internal static int Add(BuilderToggle builderToggle)
	{
		BuilderToggles.Add(builderToggle);
		return BuilderToggles.Count - 1;
	}

	internal static void Unload()
	{
		BuilderToggles.RemoveRange(DefaultDisplayCount, BuilderToggles.Count - DefaultDisplayCount);
	}

	internal static void RegisterDefaultToggles()
	{
		int[] defaultTogglesShowOrder = new int[12]
		{
			10, 11, 8, 9, 0, 1, 2, 3, 4, 5,
			6, 7
		};
		int i = 0;
		foreach (BuilderToggle builderToggle in BuilderToggles)
		{
			builderToggle.Type = defaultTogglesShowOrder[i++];
			ContentInstance.Register(builderToggle);
			ModTypeLookup<BuilderToggle>.Register(builderToggle);
		}
	}

	internal static List<BuilderToggle> ActiveBuilderTogglesList()
	{
		List<BuilderToggle> activeToggles = new List<BuilderToggle>(BuilderToggleCount);
		for (int i = 0; i < BuilderToggles.Count; i++)
		{
			if (BuilderToggles[i].Active())
			{
				activeToggles.Add(BuilderToggles[i]);
			}
		}
		return activeToggles;
	}

	public static int ActiveBuilderToggles()
	{
		return ActiveBuilderTogglesList().Count;
	}

	public static bool Active(BuilderToggle builderToggle)
	{
		return builderToggle.Active();
	}
}
