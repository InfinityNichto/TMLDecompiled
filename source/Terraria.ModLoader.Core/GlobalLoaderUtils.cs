using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Terraria.ModLoader.Core;

public static class GlobalLoaderUtils<TGlobal, TEntity> where TGlobal : GlobalType<TEntity, TGlobal> where TEntity : IEntityWithGlobals<TGlobal>
{
	private enum InstantiationTime
	{
		NotApplied,
		Pass1,
		Pass2
	}

	private delegate void TUpdateGlobalTypeData(int type, ReadOnlySpan<InstantiationTime> data);

	private static TGlobal[][] SlotPerEntityGlobals;

	private static TGlobal[][] HookSetDefaultsEarly;

	private static TGlobal[][] HookSetDefaultsLate;

	[ThreadStatic]
	private static TUpdateGlobalTypeData UpdateGlobalTypeData;

	private static TGlobal[] Globals => GlobalList<TGlobal>.Globals;

	static GlobalLoaderUtils()
	{
		TypeCaching.ResetStaticMembersOnClear(typeof(GlobalTypeLookups<TGlobal>));
	}

	public static void SetDefaults(TEntity entity, ref TGlobal[] entityGlobals, Action<TEntity> setModEntityDefaults)
	{
		if (entity.Type == 0)
		{
			return;
		}
		int initialType = entity.Type;
		entityGlobals = new TGlobal[GlobalList<TGlobal>.SlotsPerEntity];
		if (!GlobalTypeLookups<TGlobal>.Initialized)
		{
			SetDefaultsBeforeLookupsAreBuilt(entity, entityGlobals, setModEntityDefaults);
			return;
		}
		TGlobal[] array = SlotPerEntityGlobals[entity.Type];
		foreach (TGlobal g in array)
		{
			short slot = g.PerEntityIndex;
			entityGlobals[slot] = (TGlobal)(g.InstancePerEntity ? ((GlobalType<TEntity, TGlobal>)g.NewInstance(entity)) : ((GlobalType<TEntity, TGlobal>)g));
		}
		setModEntityDefaults(entity);
		EntityGlobalsEnumerator<TGlobal> entityGlobalsEnumerator = new EntityGlobalsEnumerator<TGlobal>(HookSetDefaultsEarly[entity.Type], entity);
		EntityGlobalsEnumerator<TGlobal> enumerator = entityGlobalsEnumerator.GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.SetDefaults(entity);
		}
		entityGlobalsEnumerator = new EntityGlobalsEnumerator<TGlobal>(HookSetDefaultsLate[entity.Type], entity);
		enumerator = entityGlobalsEnumerator.GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.SetDefaults(entity);
		}
		if (entity.Type == initialType)
		{
			return;
		}
		throw new Exception($"A mod attempted to {typeof(TEntity).Name}.type from {initialType} to {entity.Type} during SetDefaults. {entity}");
	}

	private static void SetDefaultsBeforeLookupsAreBuilt(TEntity entity, TGlobal[] entityGlobals, Action<TEntity> setModEntityDefaults)
	{
		InstantiationTime[] instTimes = ArrayPool<InstantiationTime>.Shared.Rent(Globals.Length);
		try
		{
			SetDefaultsBeforeLookupsAreBuilt(entity, entityGlobals, setModEntityDefaults, ref instTimes);
			UpdateGlobalTypeData?.Invoke(entity.Type, instTimes.AsSpan().Slice(0, Globals.Length));
		}
		finally
		{
			ArrayPool<InstantiationTime>.Shared.Return(instTimes, clearArray: true);
		}
	}

	private static void SetDefaultsBeforeLookupsAreBuilt(TEntity entity, TGlobal[] entityGlobals, Action<TEntity> setModEntityDefaults, ref InstantiationTime[] instTimes)
	{
		TGlobal[] globals = Globals;
		foreach (TGlobal g3 in globals)
		{
			if (!g3.ConditionallyAppliesToEntities || g3.AppliesToEntity(entity, lateInstantiation: false))
			{
				if (g3.PerEntityIndex >= 0)
				{
					entityGlobals[g3.PerEntityIndex] = (TGlobal)(g3.InstancePerEntity ? ((GlobalType<TEntity, TGlobal>)g3.NewInstance(entity)) : ((GlobalType<TEntity, TGlobal>)g3));
				}
				instTimes[g3.StaticIndex] = InstantiationTime.Pass1;
			}
		}
		setModEntityDefaults(entity);
		globals = Globals;
		foreach (TGlobal g4 in globals)
		{
			if (instTimes[g4.StaticIndex] == InstantiationTime.Pass1)
			{
				((g4.PerEntityIndex >= 0) ? entityGlobals[g4.PerEntityIndex] : g4)?.SetDefaults(entity);
			}
		}
		globals = Globals;
		foreach (TGlobal g2 in globals)
		{
			if (instTimes[g2.StaticIndex] != InstantiationTime.Pass1 && (!g2.ConditionallyAppliesToEntities || g2.AppliesToEntity(entity, lateInstantiation: true)))
			{
				if (g2.PerEntityIndex >= 0)
				{
					entityGlobals[g2.PerEntityIndex] = (TGlobal)(g2.InstancePerEntity ? ((GlobalType<TEntity, TGlobal>)g2.NewInstance(entity)) : ((GlobalType<TEntity, TGlobal>)g2));
				}
				instTimes[g2.StaticIndex] = InstantiationTime.Pass2;
			}
		}
		globals = Globals;
		foreach (TGlobal g in globals)
		{
			if (instTimes[g.StaticIndex] == InstantiationTime.Pass2)
			{
				((g.PerEntityIndex >= 0) ? entityGlobals[g.PerEntityIndex] : g)?.SetDefaults(entity);
			}
		}
	}

	public static void BuildTypeLookups(Action<int> setDefaults)
	{
		try
		{
			TGlobal[] hookSetDefaults = ((IEnumerable<TGlobal>)Globals).WhereMethodIsOverridden((Expression<Func<TGlobal, Action<TEntity>>>)((TGlobal g) => g.SetDefaults)).ToArray();
			int typeCount = GlobalList<TGlobal>.EntityTypeCount;
			Array.Fill(HookSetDefaultsEarly = new TGlobal[typeCount][], Array.Empty<TGlobal>());
			Array.Fill(HookSetDefaultsLate = new TGlobal[typeCount][], Array.Empty<TGlobal>());
			TGlobal[][] globalsForType = new TGlobal[typeCount][];
			Array.Fill(globalsForType, Array.Empty<TGlobal>());
			GlobalTypeLookups<TGlobal>.AppliesToTypeSet[] appliesToTypeCache = new GlobalTypeLookups<TGlobal>.AppliesToTypeSet[Globals.Length];
			InstantiationTime[] instTimes = new InstantiationTime[Globals.Length];
			for (int setDefaultsType = 0; setDefaultsType < typeCount; setDefaultsType++)
			{
				int finalType = 0;
				UpdateGlobalTypeData = delegate(int type, ReadOnlySpan<InstantiationTime> data)
				{
					if (type != 0)
					{
						finalType = type;
						data.CopyTo(instTimes);
					}
				};
				setDefaults(setDefaultsType);
				if (finalType == 0)
				{
					continue;
				}
				globalsForType[finalType] = GlobalTypeLookups<TGlobal>.CachedFilter(Globals, (TGlobal g) => instTimes[g.StaticIndex] > InstantiationTime.NotApplied);
				HookSetDefaultsEarly[finalType] = GlobalTypeLookups<TGlobal>.CachedFilter(hookSetDefaults, (TGlobal g) => instTimes[g.StaticIndex] == InstantiationTime.Pass1);
				HookSetDefaultsLate[finalType] = GlobalTypeLookups<TGlobal>.CachedFilter(hookSetDefaults, (TGlobal g) => instTimes[g.StaticIndex] == InstantiationTime.Pass2);
				TGlobal[] globals = Globals;
				foreach (TGlobal g2 in globals)
				{
					if (g2.ConditionallyAppliesToEntities && instTimes[g2.StaticIndex] > InstantiationTime.NotApplied)
					{
						appliesToTypeCache[g2.StaticIndex].Add(finalType);
					}
				}
			}
			GlobalTypeLookups<TGlobal>.Init(globalsForType, appliesToTypeCache);
			SlotPerEntityGlobals = GlobalTypeLookups<TGlobal>.BuildPerTypeGlobalLists(Globals.Where((TGlobal g) => g.SlotPerEntity).ToArray());
		}
		finally
		{
			UpdateGlobalTypeData = null;
		}
	}
}
