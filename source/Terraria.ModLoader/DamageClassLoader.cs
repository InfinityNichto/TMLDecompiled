using System.Collections.Generic;

namespace Terraria.ModLoader;

public static class DamageClassLoader
{
	internal static bool[,] effectInheritanceCache;

	internal static readonly List<DamageClass> DamageClasses;

	private static readonly int DefaultClassCount;

	public static int DamageClassCount => DamageClasses.Count;

	static DamageClassLoader()
	{
		DamageClasses = new List<DamageClass>
		{
			DamageClass.Default,
			DamageClass.Generic,
			DamageClass.Melee,
			DamageClass.MeleeNoSpeed,
			DamageClass.Ranged,
			DamageClass.Magic,
			DamageClass.Summon,
			DamageClass.SummonMeleeSpeed,
			DamageClass.MagicSummonHybrid,
			DamageClass.Throwing
		};
		DefaultClassCount = DamageClasses.Count;
		RegisterDefaultClasses();
		ResizeArrays();
	}

	internal static int Add(DamageClass damageClass)
	{
		DamageClasses.Add(damageClass);
		return DamageClasses.Count - 1;
	}

	internal static void ResizeArrays()
	{
		RebuildEffectInheritanceCache();
	}

	internal static void Unload()
	{
		DamageClasses.RemoveRange(DefaultClassCount, DamageClasses.Count - DefaultClassCount);
	}

	private static void RebuildEffectInheritanceCache()
	{
		effectInheritanceCache = new bool[DamageClassCount, DamageClassCount];
		for (int i = 0; i < DamageClasses.Count; i++)
		{
			for (int j = 0; j < DamageClasses.Count; j++)
			{
				DamageClass damageClass = DamageClasses[i];
				if (damageClass == DamageClasses[j] || damageClass.GetEffectInheritance(DamageClasses[j]))
				{
					effectInheritanceCache[i, j] = true;
				}
			}
		}
	}

	internal static void RegisterDefaultClasses()
	{
		int i = 0;
		foreach (DamageClass damageClass in DamageClasses)
		{
			damageClass.Type = i++;
			ContentInstance.Register(damageClass);
			ModTypeLookup<DamageClass>.Register(damageClass);
		}
	}

	/// <summary>
	/// Gets the DamageClass instance corresponding to the specified type
	/// </summary>
	/// <param name="type">The <see cref="P:Terraria.ModLoader.DamageClass.Type" /> of the damage class</param>
	/// <returns>The DamageClass instance, null if not found.</returns>
	public static DamageClass GetDamageClass(int type)
	{
		if (type >= DamageClasses.Count)
		{
			return null;
		}
		return DamageClasses[type];
	}
}
