using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;

namespace Terraria.ModLoader;

public sealed class NPCShop : AbstractNPCShop
{
	public new sealed class Entry : AbstractNPCShop.Entry
	{
		private readonly List<Condition> conditions;

		private Action<Item, NPC> shopOpenedHooks;

		public Item Item { get; }

		public IEnumerable<Condition> Conditions => conditions;

		internal (Entry target, bool after) Ordering { get; private set; } = (target: null, after: false);


		public bool Disabled { get; private set; }

		public bool OrdersLast { get; private set; }

		public bool SlotReserved { get; private set; }

		public Entry(int item, params Condition[] condition)
			: this(new Item(item), condition)
		{
		}

		public Entry(Item item, params Condition[] condition)
		{
			Disabled = false;
			Item = item;
			conditions = condition.ToList();
		}

		internal Entry SetOrdering(Entry entry, bool after)
		{
			ArgumentNullException.ThrowIfNull(entry, "entry");
			Ordering = (target: entry, after: after);
			Entry target = entry;
			do
			{
				if (target == this)
				{
					throw new Exception("Entry ordering loop!");
				}
				target = target.Ordering.target;
			}
			while (target != null);
			return this;
		}

		public Entry SortBefore(Entry target)
		{
			return SetOrdering(target, after: false);
		}

		public Entry SortAfter(Entry target)
		{
			return SetOrdering(target, after: true);
		}

		public Entry AddCondition(Condition condition)
		{
			ArgumentNullException.ThrowIfNull(condition, "condition");
			conditions.Add(condition);
			return this;
		}

		public Entry OrderLast()
		{
			OrdersLast = true;
			return this;
		}

		public Entry Disable()
		{
			Disabled = true;
			return this;
		}

		public Entry ReserveSlot()
		{
			SlotReserved = true;
			return this;
		}

		public Entry AddShopOpenedCallback(Action<Item, NPC> callback)
		{
			shopOpenedHooks = (Action<Item, NPC>)Delegate.Combine(shopOpenedHooks, callback);
			return this;
		}

		public void OnShopOpen(Item item, NPC npc)
		{
			shopOpenedHooks?.Invoke(item, npc);
		}

		public bool ConditionsMet()
		{
			for (int i = 0; i < conditions.Count; i++)
			{
				if (!conditions[i].IsMet())
				{
					return false;
				}
			}
			return true;
		}
	}

	private List<Entry> _entries;

	public IReadOnlyList<Entry> Entries => _entries;

	public bool FillLastSlot { get; private set; }

	public override IEnumerable<Entry> ActiveEntries => Entries.Where((Entry e) => !e.Disabled);

	public NPCShop(int npcType, string name = "Shop")
		: base(npcType, name)
	{
		_entries = new List<Entry>();
	}

	public Entry GetEntry(int item)
	{
		return _entries.First((Entry x) => x.Item.type == item);
	}

	public bool TryGetEntry(int item, out Entry entry)
	{
		int i = _entries.FindIndex((Entry x) => x.Item.type == item);
		if (i == -1)
		{
			entry = null;
			return false;
		}
		entry = _entries[i];
		return true;
	}

	public NPCShop AllowFillingLastSlot()
	{
		FillLastSlot = true;
		return this;
	}

	public NPCShop Add(params Entry[] entries)
	{
		_entries.AddRange(entries);
		return this;
	}

	public NPCShop Add(Item item, params Condition[] condition)
	{
		return Add(new Entry(item, condition));
	}

	public NPCShop Add(int item, params Condition[] condition)
	{
		return Add(ContentSamples.ItemsByType[item], condition);
	}

	public NPCShop Add<T>(params Condition[] condition) where T : ModItem
	{
		return Add(ModContent.ItemType<T>(), condition);
	}

	private NPCShop InsertAt(Entry targetEntry, bool after, Item item, params Condition[] condition)
	{
		return Add(new Entry(item, condition).SetOrdering(targetEntry, after));
	}

	private NPCShop InsertAt(int targetItem, bool after, Item item, params Condition[] condition)
	{
		return InsertAt(GetEntry(targetItem), after, item, condition);
	}

	private NPCShop InsertAt(int targetItem, bool after, int item, params Condition[] condition)
	{
		return InsertAt(targetItem, after, ContentSamples.ItemsByType[item], condition);
	}

	public NPCShop InsertBefore(Entry targetEntry, Item item, params Condition[] condition)
	{
		return InsertAt(targetEntry, after: false, item, condition);
	}

	public NPCShop InsertBefore(int targetItem, Item item, params Condition[] condition)
	{
		return InsertAt(targetItem, after: false, item, condition);
	}

	public NPCShop InsertBefore(int targetItem, int item, params Condition[] condition)
	{
		return InsertAt(targetItem, after: false, item, condition);
	}

	public NPCShop InsertAfter(Entry targetEntry, Item item, params Condition[] condition)
	{
		return InsertAt(targetEntry, after: true, item, condition);
	}

	public NPCShop InsertAfter(int targetItem, Item item, params Condition[] condition)
	{
		return InsertAt(targetItem, after: true, item, condition);
	}

	public NPCShop InsertAfter(int targetItem, int item, params Condition[] condition)
	{
		return InsertAt(targetItem, after: true, item, condition);
	}

	public override void FillShop(ICollection<Item> items, NPC npc)
	{
		foreach (Entry entry in _entries)
		{
			if (entry.Disabled)
			{
				continue;
			}
			Item item;
			if (entry.ConditionsMet())
			{
				item = entry.Item.Clone();
				entry.OnShopOpen(item, npc);
			}
			else
			{
				if (!entry.SlotReserved)
				{
					continue;
				}
				item = new Item(0);
			}
			items.Add(item);
		}
	}

	/// <summary>
	/// Fills a shop array with the contents of this shop, evaluating conditions and running callbacks. <br />
	/// Does not fill the entire array if there are insufficient entries. <br />
	/// The last slot will be kept empty (null) if <see cref="P:Terraria.ModLoader.NPCShop.FillLastSlot" /> is false
	/// </summary>
	/// <param name="items">Array to be filled.</param>
	/// <param name="npc">The NPC the player is talking to, for <see cref="M:Terraria.ModLoader.NPCShop.Entry.OnShopOpen(Terraria.Item,Terraria.NPC)" /> calls.</param>
	/// <param name="overflow">True if some items were unable to fit in the provided array.</param>
	public override void FillShop(Item[] items, NPC npc, out bool overflow)
	{
		overflow = false;
		int limit = (FillLastSlot ? items.Length : (items.Length - 1));
		int i = 0;
		foreach (Entry entry in _entries)
		{
			if (entry.Disabled)
			{
				continue;
			}
			bool conditionsMet = entry.ConditionsMet();
			if (conditionsMet || entry.SlotReserved)
			{
				if (i == limit)
				{
					overflow = true;
					break;
				}
				Item item;
				if (conditionsMet)
				{
					item = entry.Item.Clone();
					entry.OnShopOpen(item, npc);
				}
				else
				{
					item = new Item(0);
				}
				items[i++] = item;
			}
		}
	}

	public override void FinishSetup()
	{
		Sort();
	}

	private void Sort()
	{
		List<Entry> toBeLast = _entries.Where((Entry x) => x.OrdersLast).ToList();
		_entries.RemoveAll((Entry x) => x.OrdersLast);
		_entries.AddRange(toBeLast);
		_entries = SortBeforeAfter(_entries, (Entry r) => ((Entry, bool after))r.Ordering);
	}

	private static List<T> SortBeforeAfter<T>(IEnumerable<T> values, Func<T, (T, bool after)> func)
	{
		List<T> baseOrder = new List<T>();
		Dictionary<T, List<T>> sortBefore = new Dictionary<T, List<T>>();
		Dictionary<T, List<T>> sortAfter = new Dictionary<T, List<T>>();
		foreach (T r2 in values)
		{
			(T, bool) tuple = func(r2);
			var (target2, _) = tuple;
			if (target2 != null)
			{
				if (!tuple.Item2)
				{
					if (!sortBefore.TryGetValue(target2, out var before))
					{
						List<T> list2 = (sortBefore[target2] = new List<T>());
						before = list2;
					}
					before.Add(r2);
					continue;
				}
				T target = target2;
				if (!sortAfter.TryGetValue(target, out var after))
				{
					List<T> list2 = (sortAfter[target] = new List<T>());
					after = list2;
				}
				after.Add(r2);
			}
			else
			{
				baseOrder.Add(r2);
			}
		}
		if (sortBefore.Count + sortAfter.Count == 0)
		{
			return values.ToList();
		}
		List<T> sorted = new List<T>();
		foreach (T item in baseOrder)
		{
			Sort(item);
		}
		return sorted;
		void Sort(T r)
		{
			if (sortBefore.TryGetValue(r, out var before2))
			{
				foreach (T item2 in before2)
				{
					Sort(item2);
				}
			}
			sorted.Add(r);
			if (sortAfter.TryGetValue(r, out var after2))
			{
				foreach (T item3 in after2)
				{
					Sort(item3);
				}
			}
		}
	}
}
