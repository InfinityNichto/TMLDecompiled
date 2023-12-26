using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Terraria.ModLoader.Core;

public static class GlobalTypeLookups<TGlobal> where TGlobal : GlobalType<TGlobal>
{
	private class CachedArrayComparer : IComparer<Memory<TGlobal>>
	{
		public int Compare(Memory<TGlobal> m1, Memory<TGlobal> m2)
		{
			int c = m1.Length.CompareTo(m2.Length);
			if (c != 0)
			{
				return c;
			}
			Span<TGlobal> s1 = m1.Span;
			Span<TGlobal> s2 = m2.Span;
			for (int i = 0; i < s1.Length; i++)
			{
				TGlobal g1 = s1[i];
				TGlobal g2 = s2[i];
				int c2 = g1.StaticIndex.CompareTo(g2.StaticIndex);
				if (c2 != 0)
				{
					return c2;
				}
				if (g1 != g2)
				{
					throw new Exception($"Two globals with the same static index in the cache! Is one of them instanced? ({g1},{g2})");
				}
			}
			return 0;
		}
	}

	public struct AppliesToTypeSet
	{
		private struct BitSet
		{
			private long[] arr;

			public bool this[int i]
			{
				get
				{
					if (arr != null)
					{
						return (arr[i >> 6] & (1L << i)) != 0;
					}
					return false;
				}
			}

			public bool IsEmpty => arr == null;

			public void Set(int i)
			{
				if (arr == null)
				{
					arr = new long[GlobalTypeLookups<TGlobal>.EntityTypeCount + 63 >> 6];
				}
				arr[i >> 6] |= 1L << i;
			}
		}

		private BitSet _bitset;

		public int SingleType { get; private set; }

		public bool this[int type]
		{
			get
			{
				if (type != SingleType)
				{
					return _bitset[type];
				}
				return true;
			}
		}

		public void Add(int type)
		{
			if (_bitset.IsEmpty && SingleType == 0)
			{
				SingleType = type;
				return;
			}
			if (SingleType > 0)
			{
				_bitset.Set(SingleType);
				SingleType = 0;
			}
			_bitset.Set(type);
		}
	}

	private static TGlobal[][] _globalsForType;

	private static AppliesToTypeSet[] _appliesToType;

	private static SortedDictionary<Memory<TGlobal>, TGlobal[]> _cache;

	public static bool Initialized { get; private set; }

	private static int EntityTypeCount => GlobalList<TGlobal>.EntityTypeCount;

	static GlobalTypeLookups()
	{
		Initialized = false;
		_globalsForType = null;
		_appliesToType = null;
		_cache = new SortedDictionary<Memory<TGlobal>, TGlobal[]>(new CachedArrayComparer());
		TypeCaching.ResetStaticMembersOnClear(typeof(GlobalTypeLookups<TGlobal>));
	}

	public static void Init(TGlobal[][] globalsForType, AppliesToTypeSet[] appliesToTypeCache)
	{
		if (Initialized)
		{
			throw new Exception("Init already called");
		}
		Initialized = true;
		_globalsForType = globalsForType;
		_appliesToType = appliesToTypeCache;
	}

	public static TGlobal[] GetGlobalsForType(int type)
	{
		if (type == 0)
		{
			return Array.Empty<TGlobal>();
		}
		if (_globalsForType == null)
		{
			throw new Exception("Cannot lookup globals by type until after PostSetupContent, consider moving the calling code to [Post]AddRecipes or later instead");
		}
		return _globalsForType[type];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool AppliesToType(TGlobal global, int type)
	{
		if (type > 0)
		{
			if (global.ConditionallyAppliesToEntities)
			{
				return AppliesToTypeCacheLookup(global.StaticIndex, type);
			}
			return true;
		}
		return false;
	}

	private static bool AppliesToTypeCacheLookup(int index, int type)
	{
		if (_appliesToType == null)
		{
			throw new Exception("Cannot access conditional globals on an entity until after PostSetupContent, consider moving the calling code to [Post]AddRecipes or later, or accessing the global definition directly");
		}
		return _appliesToType[index][type];
	}

	public static TGlobal[] CachedFilter(TGlobal[] arr, Predicate<TGlobal> filter)
	{
		TGlobal[] buf = ArrayPool<TGlobal>.Shared.Rent(arr.Length);
		TGlobal[] array;
		try
		{
			int i = 0;
			array = arr;
			foreach (TGlobal g in array)
			{
				if (filter(g))
				{
					buf[i++] = g;
				}
			}
			if (i == arr.Length)
			{
				array = arr;
			}
			else
			{
				lock (_cache)
				{
					Memory<TGlobal> filtered = buf.AsMemory().Slice(0, i);
					if (_cache.TryGetValue(filtered, out var cached))
					{
						array = cached;
					}
					else
					{
						TGlobal[] result = filtered.ToArray();
						_cache.Add(result, result);
						array = result;
					}
				}
			}
		}
		finally
		{
			ArrayPool<TGlobal>.Shared.Return(buf, clearArray: true);
		}
		return array;
	}

	public static TGlobal[][] BuildPerTypeGlobalLists(TGlobal[] arr)
	{
		Dictionary<TGlobal[], TGlobal[]> dict = new Dictionary<TGlobal[], TGlobal[]>();
		TGlobal[][] lookup = new TGlobal[EntityTypeCount][];
		int type;
		for (type = 0; type < lookup.Length; type++)
		{
			if (!dict.TryGetValue(GetGlobalsForType(type), out var v))
			{
				v = CachedFilter(arr, (TGlobal g) => AppliesToType(g, type));
			}
			lookup[type] = v;
		}
		return lookup;
	}

	public static void LogStats()
	{
		TGlobal[] globals = GlobalList<TGlobal>.Globals;
		int instanced = globals.Count((TGlobal g) => g.InstancePerEntity);
		int conditionalWithSlot = globals.Count((TGlobal g) => g.ConditionallyAppliesToEntities && g.SlotPerEntity);
		int conditionalByType = globals.Count((TGlobal g) => g.ConditionallyAppliesToEntities && !g.SlotPerEntity);
		int appliesToSingleType = _appliesToType.Count((AppliesToTypeSet s) => s.SingleType > 0);
		int cacheEntries = _cache.Count;
		int cacheSize = _cache.Values.Sum((TGlobal[] e) => e.Length * 8 + 16);
		Logging.tML.Debug((object)$"{typeof(TGlobal).Name} registration stats. Count: {globals.Length}, Slots per Entity: {GlobalList<TGlobal>.SlotsPerEntity}\n\tInstanced: {instanced}, Conditional with slot: {conditionalWithSlot}, Conditional by type: {conditionalByType}, Applies to single type: {appliesToSingleType}\n\tList Permutations: {cacheEntries}, Est Memory Consumption: {cacheSize} bytes");
	}
}
