using System.Collections.Generic;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.ModLoader;

public abstract class ModPrefix : ModType, ILocalizedModType, IModType
{
	public int Type { get; internal set; }

	public virtual string LocalizationCategory => "Prefixes";

	public virtual LocalizedText DisplayName => this.GetLocalization("DisplayName", base.PrettyPrintName);

	/// <summary>
	/// The category your prefix belongs to, PrefixCategory.Custom by default
	/// </summary>
	public virtual PrefixCategory Category => PrefixCategory.Custom;

	protected sealed override void Register()
	{
		ModTypeLookup<ModPrefix>.Register(this);
		Type = PrefixLoader.ReservePrefixID();
		PrefixLoader.RegisterPrefix(this);
	}

	public sealed override void SetupContent()
	{
		SetStaticDefaults();
		PrefixID.Search.Add(base.FullName, Type);
	}

	/// <summary>
	/// The roll chance of your prefix relative to a vanilla prefix, 1f by default.
	/// </summary>
	public virtual float RollChance(Item item)
	{
		return 1f;
	}

	/// <summary>
	/// Returns if your ModPrefix can roll on the given item
	/// By default returns RollChance(item) &gt; 0
	/// </summary>
	public virtual bool CanRoll(Item item)
	{
		return RollChance(item) > 0f;
	}

	/// <summary>
	/// Sets the stat changes for this prefix. If data is not already pre-stored, it is best to store custom data changes to some static variables.
	/// </summary>
	public virtual void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
	{
	}

	/// <summary>
	/// Use this to check whether modifiers to custom stats would be too small to have an effect after rounding, and prevent the prefix from being applied to the given item if there would be no change.
	/// <para />Vanilla stat changes (<seealso cref="M:Terraria.ModLoader.ModPrefix.SetStats(System.Single@,System.Single@,System.Single@,System.Single@,System.Single@,System.Single@,System.Int32@)" />) are checked automatically, so there is no need to override this method to check them
	/// </summary>
	/// <returns>false to prevent the prefix from being applied</returns>
	public virtual bool AllStatChangesHaveEffectOn(Item item)
	{
		return true;
	}

	/// <summary>
	/// Applies the custom data stats set in SetStats to the given item.
	/// </summary>
	public virtual void Apply(Item item)
	{
	}

	/// <summary>
	/// Allows you to modify the sell price of the item based on the prefix or changes in custom data stats. This also influences the item's rarity.
	/// </summary>
	public virtual void ModifyValue(ref float valueMult)
	{
	}

	/// <summary>
	/// Use this to modify player stats (or any other applicable data) based on this ModPrefix.
	/// </summary>
	/// <param name="player"> The player gaining the benefits of this accessory. </param>
	public virtual void ApplyAccessoryEffects(Player player)
	{
	}

	/// <summary>
	/// Use this to add tooltips to any item with this prefix applied. Note that the stat bonuses applied via <see cref="M:Terraria.ModLoader.ModPrefix.SetStats(System.Single@,System.Single@,System.Single@,System.Single@,System.Single@,System.Single@,System.Int32@)" /> will automatically generate tooltips. (such as damage, use speed, crit chance, mana cost, scale, shoot speed, and knockback)<br />
	/// </summary>
	public virtual IEnumerable<TooltipLine> GetTooltipLines(Item item)
	{
		return null;
	}
}
