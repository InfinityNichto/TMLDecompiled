using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent.Prefixes;
using Terraria.ID;
using Terraria.ModLoader.Core;
using Terraria.Utilities;

namespace Terraria.ModLoader;

public static class PrefixLoader
{
	internal static readonly IList<ModPrefix> prefixes;

	internal static readonly IDictionary<PrefixCategory, List<ModPrefix>> categoryPrefixes;

	public static int PrefixCount { get; private set; }

	static PrefixLoader()
	{
		prefixes = new List<ModPrefix>();
		PrefixCount = PrefixID.Count;
		categoryPrefixes = new Dictionary<PrefixCategory, List<ModPrefix>>();
		foreach (PrefixCategory category in Enum.GetValues(typeof(PrefixCategory)))
		{
			categoryPrefixes[category] = new List<ModPrefix>();
		}
	}

	internal static void RegisterPrefix(ModPrefix prefix)
	{
		prefixes.Add(prefix);
		categoryPrefixes[prefix.Category].Add(prefix);
	}

	internal static int ReservePrefixID()
	{
		if (ModNet.AllowVanillaClients)
		{
			throw new Exception("Adding items breaks vanilla client compatibility");
		}
		return PrefixCount++;
	}

	/// <summary>
	/// Returns the ModPrefix associated with specified type
	/// If not a ModPrefix, returns null.
	/// </summary>
	public static ModPrefix GetPrefix(int type)
	{
		if (type < PrefixID.Count || type >= PrefixCount)
		{
			return null;
		}
		return prefixes[type - PrefixID.Count];
	}

	/// <summary>
	/// Returns a list of all modded prefixes of a certain category.
	/// </summary>
	public static IReadOnlyList<ModPrefix> GetPrefixesInCategory(PrefixCategory category)
	{
		return categoryPrefixes[category];
	}

	internal static void ResizeArrays()
	{
		LoaderUtils.ResetStaticMembers(typeof(PrefixID));
		Array.Resize(ref Lang.prefix, PrefixCount);
	}

	internal static void FinishSetup()
	{
		foreach (ModPrefix prefix in prefixes)
		{
			Lang.prefix[prefix.Type] = prefix.DisplayName;
		}
	}

	internal static void Unload()
	{
		prefixes.Clear();
		PrefixCount = PrefixID.Count;
		foreach (PrefixCategory category in Enum.GetValues(typeof(PrefixCategory)))
		{
			categoryPrefixes[category].Clear();
		}
	}

	public static bool CanRoll(Item item, int prefix)
	{
		if (!ItemLoader.AllowPrefix(item, prefix))
		{
			return false;
		}
		ModPrefix modPrefix = GetPrefix(prefix);
		PrefixCategory? prefixCategory;
		if (modPrefix != null)
		{
			if (!modPrefix.CanRoll(item))
			{
				return false;
			}
			if (modPrefix.Category == PrefixCategory.Custom)
			{
				return true;
			}
			prefixCategory = item.GetPrefixCategory();
			if (prefixCategory.HasValue)
			{
				PrefixCategory itemCategory = prefixCategory.GetValueOrDefault();
				if (modPrefix.Category != itemCategory)
				{
					if (modPrefix.Category == PrefixCategory.AnyWeapon)
					{
						return IsWeaponSubCategory(itemCategory);
					}
					return false;
				}
				return true;
			}
			return false;
		}
		prefixCategory = item.GetPrefixCategory();
		if (prefixCategory.HasValue)
		{
			PrefixCategory category = prefixCategory.GetValueOrDefault();
			if (Item.GetVanillaPrefixes(category).Contains(prefix))
			{
				return true;
			}
		}
		if (PrefixLegacy.ItemSets.ItemsThatCanHaveLegendary2[item.type] && prefix == 84)
		{
			return true;
		}
		return false;
	}

	public static bool Roll(Item item, UnifiedRandom unifiedRandom, out int prefix, bool justCheck)
	{
		int forcedPrefix = ItemLoader.ChoosePrefix(item, unifiedRandom);
		if (forcedPrefix > 0 && CanRoll(item, forcedPrefix))
		{
			prefix = forcedPrefix;
			return true;
		}
		prefix = 0;
		WeightedRandom<int> wr = new WeightedRandom<int>(unifiedRandom);
		PrefixCategory? prefixCategory = item.GetPrefixCategory();
		if (prefixCategory.HasValue)
		{
			PrefixCategory category2 = prefixCategory.GetValueOrDefault();
			if (justCheck)
			{
				return true;
			}
			int[] vanillaPrefixes = Item.GetVanillaPrefixes(category2);
			foreach (int pre in vanillaPrefixes)
			{
				wr.Add(pre);
			}
			if (PrefixLegacy.ItemSets.ItemsThatCanHaveLegendary2[item.type])
			{
				wr.Add(84);
			}
			AddCategory(category2);
			if (IsWeaponSubCategory(category2))
			{
				AddCategory(PrefixCategory.AnyWeapon);
			}
			for (int i = 0; i < 50; i++)
			{
				prefix = wr.Get();
				if (ItemLoader.AllowPrefix(item, prefix))
				{
					return true;
				}
			}
			return false;
		}
		return false;
		void AddCategory(PrefixCategory category)
		{
			foreach (ModPrefix modPrefix in categoryPrefixes[category].Where((ModPrefix x) => x.CanRoll(item)))
			{
				wr.Add(modPrefix.Type, modPrefix.RollChance(item));
			}
		}
	}

	public static bool IsWeaponSubCategory(PrefixCategory category)
	{
		if (category != 0 && category != PrefixCategory.Ranged)
		{
			return category == PrefixCategory.Magic;
		}
		return true;
	}

	public static void ApplyAccessoryEffects(Player player, Item item)
	{
		GetPrefix(item.prefix)?.ApplyAccessoryEffects(player);
	}
}
