using System.Collections.Generic;
using System.Linq;
using Terraria.ID;

namespace Terraria.ModLoader;

public class TravellingMerchantShop : AbstractNPCShop
{
	private new record Entry(Item Item, IEnumerable<Condition> Conditions) : AbstractNPCShop.Entry;

	private List<Entry> _entries = new List<Entry>();

	public override IEnumerable<AbstractNPCShop.Entry> ActiveEntries => _entries;

	public TravellingMerchantShop(int npcType)
		: base(npcType)
	{
	}

	public TravellingMerchantShop AddInfoEntry(Item item, params Condition[] conditions)
	{
		_entries.Add(new Entry(item, conditions.ToList()));
		return this;
	}

	public TravellingMerchantShop AddInfoEntry(int item, params Condition[] conditions)
	{
		return AddInfoEntry(ContentSamples.ItemsByType[item], conditions);
	}

	public override void FillShop(ICollection<Item> items, NPC npc)
	{
		int[] travelShop = Main.travelShop;
		foreach (int itemId in travelShop)
		{
			if (itemId != 0)
			{
				items.Add(new Item(itemId));
			}
		}
	}

	public override void FillShop(Item[] items, NPC npc, out bool overflow)
	{
		overflow = false;
		int i = 0;
		int[] travelShop = Main.travelShop;
		foreach (int itemId in travelShop)
		{
			if (itemId != 0)
			{
				items[i++] = new Item(itemId);
			}
		}
	}
}
