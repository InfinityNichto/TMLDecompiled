using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.ModLoader;

public struct PosData<T>
{
	/// <summary>
	/// Efficient builder for <see cref="T:Terraria.ModLoader.PosData`1" />[] lookups covering the whole world.
	/// Must add elements in ascending pos order.
	/// </summary>
	public class OrderedSparseLookupBuilder
	{
		private readonly List<PosData<T>> list;

		private readonly bool compressEqualValues;

		private readonly bool insertDefaultEntries;

		private PosData<T> last;

		/// <summary>
		/// Use <paramref name="compressEqualValues" /> to produce a smaller lookup which won't work with <see cref="M:Terraria.ModLoader.PosData.LookupExact``1(Terraria.ModLoader.PosData{``0}[],System.Int32,System.Int32,``0@)" />
		/// When using <paramref name="compressEqualValues" /> without <paramref name="insertDefaultEntries" />,
		/// unspecified positions will default to the value of the previous specified position
		/// </summary>
		/// <param name="capacity">Defaults to 1M entries to reduce reallocations. Final built collection will be smaller. </param>
		/// <param name="compressEqualValues">Reduces the size of the map, but gives unspecified positions a value.</param>
		/// <param name="insertDefaultEntries">Ensures unspecified positions are assigned a default value when used with <paramref name="compressEqualValues" /></param>
		public OrderedSparseLookupBuilder(int capacity = 1048576, bool compressEqualValues = true, bool insertDefaultEntries = false)
		{
			list = new List<PosData<T>>(capacity);
			last = PosData<T>.nullPosData;
			this.compressEqualValues = compressEqualValues;
			this.insertDefaultEntries = insertDefaultEntries;
		}

		public void Add(int x, int y, T value)
		{
			Add(PosData.CoordsToPos(x, y), value);
		}

		public void Add(int pos, T value)
		{
			if (pos <= last.pos)
			{
				throw new ArgumentException($"Must build in ascending index order. Prev: {last.pos}, pos: {pos}");
			}
			if (compressEqualValues)
			{
				if (insertDefaultEntries && pos >= last.pos + 2)
				{
					Add(last.pos + 1, default(T));
				}
				if (EqualityComparer<T>.Default.Equals(value, last.value))
				{
					return;
				}
			}
			list.Add(last = new PosData<T>(pos, value));
		}

		public PosData<T>[] Build()
		{
			return list.ToArray();
		}
	}

	public class OrderedSparseLookupReader
	{
		private readonly PosData<T>[] data;

		private readonly bool hasEqualValueCompression;

		private PosData<T> current;

		private int nextIdx;

		public OrderedSparseLookupReader(PosData<T>[] data, bool hasEqualValueCompression = true)
		{
			this.data = data;
			this.hasEqualValueCompression = hasEqualValueCompression;
			current = PosData<T>.nullPosData;
			nextIdx = 0;
		}

		public T Get(int x, int y)
		{
			return Get(PosData.CoordsToPos(x, y));
		}

		public T Get(int pos)
		{
			if (pos <= current.pos)
			{
				throw new ArgumentException($"Must read in ascending index order. Prev: {current.pos}, pos: {pos}");
			}
			while (nextIdx < data.Length && data[nextIdx].pos <= pos)
			{
				current = data[nextIdx++];
			}
			if (!hasEqualValueCompression && current.pos != pos)
			{
				throw new KeyNotFoundException($"Position does not exist in map. {pos} (X: {current.X}, Y: {current.Y})");
			}
			return current.value;
		}
	}

	public readonly int pos;

	public T value;

	public static PosData<T> nullPosData = new PosData<T>(-1, default(T));

	public int X => pos / Main.maxTilesY;

	public int Y => pos % Main.maxTilesY;

	public PosData(int pos, T value)
	{
		this.pos = pos;
		this.value = value;
	}

	public PosData(int x, int y, T value)
		: this(PosData.CoordsToPos(x, y), value)
	{
	}
}
public static class PosData
{
	/// <summary>
	/// Gets a Position ID based on the x,y position. If using in an order sensitive case, see NextLocation.
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public static int CoordsToPos(int x, int y)
	{
		return x * Main.maxTilesY + y;
	}

	public static int FindIndex<T>(this PosData<T>[] posMap, int x, int y)
	{
		return posMap.FindIndex(CoordsToPos(x, y));
	}

	/// <summary>
	/// Searches for the value i for which <code>posMap[i].pos &lt; pos &lt; posMap[i + 1].pos</code>
	/// </summary>
	/// <returns>The index of the nearest entry with <see cref="F:Terraria.ModLoader.PosData`1.pos" /> &lt;= <paramref name="pos" /> or -1 if <paramref name="pos" /> &lt; <paramref name="posMap" />[0].pos</returns>
	public static int FindIndex<T>(this PosData<T>[] posMap, int pos)
	{
		int minimum = -1;
		int maximum = posMap.Length;
		while (maximum - minimum > 1)
		{
			int split = (minimum + maximum) / 2;
			if (posMap[split].pos <= pos)
			{
				minimum = split;
			}
			else
			{
				maximum = split;
			}
		}
		return minimum;
	}

	/// <summary>
	/// Raw lookup function. Always returns the raw entry in the position map. Use if default values returned are a concern, as negative position returned are ~'null'
	/// </summary>
	public static PosData<T> Find<T>(this PosData<T>[] posMap, int pos)
	{
		int i = posMap.FindIndex(pos);
		if (i >= 0)
		{
			return posMap[i];
		}
		return PosData<T>.nullPosData;
	}

	/// <summary>
	/// General purpose lookup function. Always returns a value (even if that value is `default`).
	/// See <see cref="M:Terraria.ModLoader.PosData`1.OrderedSparseLookupBuilder.#ctor(System.Int32,System.Boolean,System.Boolean)" />for more info
	/// </summary>
	public static T Lookup<T>(this PosData<T>[] posMap, int x, int y)
	{
		return posMap.Lookup(CoordsToPos(x, y));
	}

	/// <summary>
	/// General purpose lookup function. Always returns a value (even if that value is `default`).
	/// See <see cref="M:Terraria.ModLoader.PosData`1.OrderedSparseLookupBuilder.#ctor(System.Int32,System.Boolean,System.Boolean)" />for more info
	/// </summary>
	public static T Lookup<T>(this PosData<T>[] posMap, int pos)
	{
		return posMap.Find(pos).value;
	}

	/// <summary>
	/// For use with uncompressed sparse data lookups. Checks that the exact position exists in the lookup table.
	/// </summary>
	public static bool LookupExact<T>(this PosData<T>[] posMap, int x, int y, out T data)
	{
		return posMap.LookupExact(CoordsToPos(x, y), out data);
	}

	/// <summary>
	/// For use with uncompressed sparse data lookups. Checks that the exact position exists in the lookup table.
	/// </summary>
	public static bool LookupExact<T>(this PosData<T>[] posMap, int pos, out T data)
	{
		PosData<T> posData = posMap.Find(pos);
		if (posData.pos != pos)
		{
			data = default(T);
			return false;
		}
		data = posData.value;
		return true;
	}

	/// <summary>
	/// Searches around the provided point to check for the nearest entry in the map for OrdereredSparse data
	/// Doesn't work with 'compressed' lookups from <see cref="T:Terraria.ModLoader.PosData`1.OrderedSparseLookupBuilder" />
	/// </summary>
	/// <param name="posMap"></param>
	/// <param name="pt"></param>
	/// <param name="distance"> The distance between the provided Point and nearby entry </param>
	/// <param name="entry"></param>
	/// <returns> True if successfully found an entry nearby </returns>
	public static bool NearbySearchOrderedPosMap<T>(PosData<T>[] posMap, Point pt, int distance, out PosData<T> entry)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		entry = new PosData<T>(-1, default(T));
		if (posMap.Length == 0)
		{
			return false;
		}
		int minPos = CoordsToPos(Math.Max(pt.X - distance, 0), Math.Max(pt.Y - distance, 0));
		int maxPos = CoordsToPos(Math.Min(pt.X + distance, Main.maxTilesX - 1), Math.Min(pt.Y + distance, Main.maxTilesY - 1));
		if (posMap[0].pos > maxPos || posMap[^1].pos < minPos)
		{
			return false;
		}
		int num = Math.Max(posMap.FindIndex(minPos), 0);
		int maximum = Math.Max(posMap.FindIndex(maxPos), 0);
		int bestSqDist = distance * distance + 1;
		for (int i = num; i < maximum; i++)
		{
			PosData<T> posData = posMap[i];
			int dy = posData.Y - pt.Y;
			if (dy >= -distance && dy <= distance)
			{
				int num2 = posData.X - pt.X;
				int sqDist = num2 * num2 + dy * dy;
				if (sqDist < bestSqDist)
				{
					bestSqDist = sqDist;
					entry = posData;
				}
			}
		}
		return entry.pos >= 0;
	}
}
