using System;
using System.Collections.Generic;
using System.Diagnostics;
using Terraria.Localization;
using Terraria.Utilities;

namespace Terraria.WorldBuilding;

public class WorldGenerator
{
	internal readonly List<GenPass> _passes = new List<GenPass>();

	internal double _totalLoadWeight;

	internal readonly int _seed;

	private readonly WorldGenConfiguration _configuration;

	public static GenerationProgress CurrentGenerationProgress;

	public WorldGenerator(int seed, WorldGenConfiguration configuration)
	{
		_seed = seed;
		_configuration = configuration;
	}

	public void Append(GenPass pass)
	{
		_passes.Add(pass);
		_totalLoadWeight += pass.Weight;
	}

	public void GenerateWorld(GenerationProgress progress = null)
	{
		Stopwatch stopwatch = new Stopwatch();
		double num = 0.0;
		foreach (GenPass pass in _passes)
		{
			num += pass.Weight;
		}
		if (progress == null)
		{
			progress = new GenerationProgress();
		}
		CurrentGenerationProgress = progress;
		progress.TotalWeight = num;
		foreach (GenPass pass2 in _passes)
		{
			WorldGen._genRand = new UnifiedRandom(_seed);
			Main.rand = new UnifiedRandom(_seed);
			stopwatch.Start();
			progress.Start(pass2.Weight);
			try
			{
				pass2.Apply(progress, _configuration.GetPassConfiguration(pass2.Name));
			}
			catch (Exception e)
			{
				Utils.ShowFancyErrorMessage(string.Join("\n", Language.GetTextValue("tModLoader.WorldGenError"), pass2.Name, e), 0);
				throw;
			}
			progress.End();
			stopwatch.Reset();
		}
		CurrentGenerationProgress = null;
	}
}
