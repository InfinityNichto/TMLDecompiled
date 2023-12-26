using System.Collections.Generic;

namespace Terraria.ModLoader;

public abstract class AbstractNPCShop
{
	public interface Entry
	{
		Item Item { get; }

		IEnumerable<Condition> Conditions { get; }
	}

	public int NpcType { get; private init; }

	public string Name { get; private init; }

	public string FullName => NPCShopDatabase.GetShopName(NpcType, Name);

	public abstract IEnumerable<Entry> ActiveEntries { get; }

	public AbstractNPCShop(int npcType, string name = "Shop")
	{
		NpcType = npcType;
		Name = name;
	}

	public void Register()
	{
		NPCShopDatabase.AddShop(this);
	}

	/// <summary>
	/// Unbounded variant of <see cref="M:Terraria.ModLoader.AbstractNPCShop.FillShop(Terraria.Item[],Terraria.NPC,System.Boolean@)" />, for future forwards compatibility with tabbed or scrolling shops.
	/// </summary>
	/// <param name="items">The collection to be filled</param>
	/// <param name="npc">The NPC the player is talking to</param>
	public abstract void FillShop(ICollection<Item> items, NPC npc);

	/// <summary>
	/// Fills a shop array with the contents of this shop, evaluating conditions and running callbacks.
	/// </summary>
	/// <param name="items">Array to be filled.</param>
	/// <param name="npc">The NPC the player is talking to</param>
	/// <param name="overflow">True if some items were unable to fit in the provided array</param>
	public abstract void FillShop(Item[] items, NPC npc, out bool overflow);

	public virtual void FinishSetup()
	{
	}
}
