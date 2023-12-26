using System;
using System.Collections.Generic;

namespace Terraria.ModLoader.Utilities;

internal static class NPCSpawnHelper
{
	internal static List<SpawnCondition> conditions = new List<SpawnCondition>();

	internal static void Reset()
	{
		foreach (SpawnCondition condition in conditions)
		{
			condition.Reset();
		}
	}

	internal static void DoChecks(NPCSpawnInfo info)
	{
		float weight = 1f;
		foreach (SpawnCondition condition in conditions)
		{
			condition.Check(info, ref weight);
			if ((double)Math.Abs(weight) < 5E-06)
			{
				break;
			}
		}
	}
}
