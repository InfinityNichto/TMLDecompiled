using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader.UI;

namespace Terraria.ModLoader.Core;

internal static class MemoryTracking
{
	internal static Dictionary<string, ModMemoryUsage> modMemoryUsageEstimates = new Dictionary<string, ModMemoryUsage>();

	private static long previousMemory;

	internal static void Clear()
	{
		modMemoryUsageEstimates.Clear();
	}

	internal static ModMemoryUsage Update(string modName)
	{
		if (!modMemoryUsageEstimates.TryGetValue(modName, out var usage))
		{
			usage = (modMemoryUsageEstimates[modName] = new ModMemoryUsage());
		}
		if (ModLoader.showMemoryEstimates)
		{
			long newMemory = GC.GetTotalMemory(forceFullCollection: true);
			usage.managed += Math.Max(0L, newMemory - previousMemory);
			previousMemory = newMemory;
		}
		return usage;
	}

	internal static void Checkpoint()
	{
		if (ModLoader.showMemoryEstimates)
		{
			previousMemory = GC.GetTotalMemory(forceFullCollection: true);
		}
	}

	internal static void Finish()
	{
		Mod[] mods = ModLoader.Mods;
		foreach (Mod mod in mods)
		{
			ModMemoryUsage modMemoryUsage = modMemoryUsageEstimates[mod.Name];
			modMemoryUsage.textures = (from asset in mod.Assets.GetLoadedAssets().OfType<Asset<Texture2D>>()
				select asset.Value into val
				where val != null
				select val).Sum((Texture2D tex) => tex.Width * tex.Height * 4);
			modMemoryUsage.sounds = (from asset in mod.Assets.GetLoadedAssets().OfType<Asset<SoundEffect>>()
				select asset.Value into val
				where val != null
				select val).Sum((SoundEffect sound) => (long)sound.Duration.TotalSeconds * 44100 * 2 * 2);
		}
		long totalRamUsage = -1L;
		try
		{
			totalRamUsage = Process.GetProcesses().Sum((Process x) => x.WorkingSet64);
		}
		catch
		{
		}
		Logging.tML.Info((object)$"RAM: tModLoader usage: {UIMemoryBar.SizeSuffix(Process.GetCurrentProcess().WorkingSet64)}, All processes usage: {((totalRamUsage == -1) ? "Unknown" : UIMemoryBar.SizeSuffix(totalRamUsage))}, Available: {UIMemoryBar.SizeSuffix(UIMemoryBar.GetTotalMemory() - totalRamUsage)}, Total Installed: {UIMemoryBar.SizeSuffix(UIMemoryBar.GetTotalMemory())}");
	}
}
