using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.GameContent.Items;

/// <summary>
/// Handles conditional variants for <see cref="T:Terraria.Item" />s, commonly used for secret seeds.
/// </summary>
public static class ItemVariants
{
	public class VariantEntry
	{
		public readonly ItemVariant Variant;

		private readonly List<Condition> _conditions = new List<Condition>();

		public IEnumerable<Condition> Conditions => _conditions;

		public VariantEntry(ItemVariant variant)
		{
			Variant = variant;
		}

		internal void AddConditions(IEnumerable<Condition> conditions)
		{
			_conditions.AddRange(conditions);
		}

		public bool AnyConditionMet()
		{
			return Conditions.Any((Condition c) => c.IsMet());
		}
	}

	private static List<VariantEntry>[] _variants;

	/// <summary>
	/// Represents the stronger variant of items used on the Remix seed.
	/// </summary>
	public static ItemVariant StrongerVariant;

	/// <summary>
	/// Represents the weaker variant of items used on the Remix seed.
	/// </summary>
	public static ItemVariant WeakerVariant;

	/// <summary>
	/// Represents the rebalanced variant of items used on the "For the Worthy" seed.
	/// </summary>
	public static ItemVariant RebalancedVariant;

	/// <summary>
	/// Represents a variant of an item that is conditionally enabled.
	/// </summary>
	public static ItemVariant EnabledVariant;

	/// <summary>
	/// Represents a variant of a boss summoning item that is conditionally disabled.
	/// </summary>
	public static ItemVariant DisabledBossSummonVariant;

	private static Condition RemixWorld;

	private static Condition GetGoodWorld;

	private static Condition EverythingWorld;

	/// <summary>
	/// Gets all of the <see cref="T:Terraria.GameContent.Items.ItemVariant" />s associated with <paramref name="itemId" />.
	/// </summary>
	/// <param name="itemId">The <see cref="F:Terraria.Item.type" /> to get <see cref="T:Terraria.GameContent.Items.ItemVariant" />s for.</param>
	/// <returns>A list of all registered <see cref="T:Terraria.GameContent.Items.ItemVariant" />s for <paramref name="itemId" />.</returns>
	public static IEnumerable<VariantEntry> GetVariants(int itemId)
	{
		if (!_variants.IndexInRange(itemId))
		{
			return Enumerable.Empty<VariantEntry>();
		}
		IEnumerable<VariantEntry> enumerable = _variants[itemId];
		return enumerable ?? Enumerable.Empty<VariantEntry>();
	}

	private static VariantEntry GetEntry(int itemId, ItemVariant variant)
	{
		return GetVariants(itemId).SingleOrDefault((VariantEntry v) => v.Variant == variant);
	}

	/// <summary>
	/// Registers an <see cref="T:Terraria.GameContent.Items.ItemVariant" /> for the given <see cref="F:Terraria.Item.type" />.
	/// </summary>
	/// <param name="itemId">The <see cref="F:Terraria.Item.type" /> to register the <see cref="T:Terraria.GameContent.Items.ItemVariant" /> for.</param>
	/// <param name="variant">The <see cref="T:Terraria.GameContent.Items.ItemVariant" /> to register to <paramref name="itemId" />.</param>
	/// <param name="conditions">The conditions under which <see cref="T:Terraria.Item" />s of type <paramref name="itemId" /> will have <paramref name="variant" /> applied. (<see cref="M:Terraria.GameContent.Items.ItemVariants.SelectVariant(System.Int32)" />)</param>
	public static void AddVariant(int itemId, ItemVariant variant, params Condition[] conditions)
	{
		VariantEntry variantEntry = GetEntry(itemId, variant);
		if (variantEntry == null)
		{
			List<VariantEntry> list = _variants[itemId];
			if (list == null)
			{
				list = (_variants[itemId] = new List<VariantEntry>());
			}
			list.Add(variantEntry = new VariantEntry(variant));
		}
		variantEntry.AddConditions(conditions);
	}

	/// <summary>
	/// Determines if an <see cref="F:Terraria.Item.type" /> has a particular <see cref="T:Terraria.GameContent.Items.ItemVariant" />.
	/// </summary>
	/// <param name="itemId">The <see cref="F:Terraria.Item.type" /> to check.</param>
	/// <param name="variant">The <see cref="T:Terraria.GameContent.Items.ItemVariant" /> to check for.</param>
	/// <returns><see langword="true" /> if <paramref name="itemId" /> has a registered <see cref="T:Terraria.GameContent.Items.ItemVariant" /> of type <paramref name="variant" />, <see langword="false" /> otherwise.</returns>
	/// <remarks>This method only checks if the given <see cref="T:Terraria.GameContent.Items.ItemVariant" /> exists, not if it will be applied.</remarks>
	public static bool HasVariant(int itemId, ItemVariant variant)
	{
		return GetEntry(itemId, variant) != null;
	}

	/// <summary>
	/// Determines which <see cref="T:Terraria.GameContent.Items.ItemVariant" /> should be applied to an item of type <paramref name="itemId" />.
	/// </summary>
	/// <param name="itemId">The <see cref="F:Terraria.Item.type" /> to check.</param>
	/// <returns>The <see cref="T:Terraria.GameContent.Items.ItemVariant" /> to use under the current conditions, or <see langword="null" /> if no appropriate <see cref="T:Terraria.GameContent.Items.ItemVariant" /> exists.</returns>
	public static ItemVariant SelectVariant(int itemId)
	{
		if (!_variants.IndexInRange(itemId))
		{
			return null;
		}
		List<VariantEntry> list = _variants[itemId];
		if (list == null)
		{
			return null;
		}
		foreach (VariantEntry item in list)
		{
			if (item.AnyConditionMet())
			{
				return item.Variant;
			}
		}
		return null;
	}

	static ItemVariants()
	{
		_variants = new List<VariantEntry>[ItemID.Count];
		StrongerVariant = new ItemVariant(Language.GetText("ItemVariant.Stronger"));
		WeakerVariant = new ItemVariant(Language.GetText("ItemVariant.Weaker"));
		RebalancedVariant = new ItemVariant(Language.GetText("ItemVariant.Rebalanced"));
		EnabledVariant = new ItemVariant(Language.GetText("ItemVariant.Enabled"));
		DisabledBossSummonVariant = new ItemVariant(Language.GetText("ItemVariant.DisabledBossSummon"));
		RemixWorld = Condition.RemixWorld;
		GetGoodWorld = Condition.ForTheWorthyWorld;
		EverythingWorld = Condition.ZenithWorld;
		AddVariant(112, StrongerVariant, RemixWorld);
		AddVariant(157, StrongerVariant, RemixWorld);
		AddVariant(1319, StrongerVariant, RemixWorld);
		AddVariant(1325, StrongerVariant, RemixWorld);
		AddVariant(2273, StrongerVariant, RemixWorld);
		AddVariant(3069, StrongerVariant, RemixWorld);
		AddVariant(5147, StrongerVariant, RemixWorld);
		AddVariant(517, WeakerVariant, RemixWorld);
		AddVariant(671, WeakerVariant, RemixWorld);
		AddVariant(683, WeakerVariant, RemixWorld);
		AddVariant(725, WeakerVariant, RemixWorld);
		AddVariant(1314, WeakerVariant, RemixWorld);
		AddVariant(2623, WeakerVariant, RemixWorld);
		AddVariant(5279, WeakerVariant, RemixWorld);
		AddVariant(5280, WeakerVariant, RemixWorld);
		AddVariant(5281, WeakerVariant, RemixWorld);
		AddVariant(5282, WeakerVariant, RemixWorld);
		AddVariant(5283, WeakerVariant, RemixWorld);
		AddVariant(5284, WeakerVariant, RemixWorld);
		AddVariant(197, RebalancedVariant, GetGoodWorld);
		AddVariant(4060, RebalancedVariant, GetGoodWorld);
		AddVariant(556, DisabledBossSummonVariant, EverythingWorld);
		AddVariant(557, DisabledBossSummonVariant, EverythingWorld);
		AddVariant(544, DisabledBossSummonVariant, EverythingWorld);
		AddVariant(5334, EnabledVariant, EverythingWorld);
	}
}
