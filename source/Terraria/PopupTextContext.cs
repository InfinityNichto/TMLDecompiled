namespace Terraria;

/// <summary>
/// A context for a generated <see cref="T:Terraria.PopupText" />.
/// </summary>
public enum PopupTextContext
{
	/// <summary>
	/// Used when a player picks up an <see cref="T:Terraria.Item" /> and it goes into their inventory.
	/// </summary>
	RegularItemPickup,
	/// <summary>
	/// Used when a player picks up an <see cref="T:Terraria.Item" /> and it goes into their Void Bag.
	/// </summary>
	ItemPickupToVoidContainer,
	/// <summary>
	/// Used for fishing alerts for Sonar Potions.
	/// </summary>
	SonarAlert,
	/// <summary>
	/// Used when a player reforges an <see cref="T:Terraria.Item" />.
	/// </summary>
	ItemReforge,
	/// <summary>
	/// Used when a player crafts an <see cref="T:Terraria.Item" />.
	/// </summary>
	ItemCraft,
	/// <summary>
	/// Used for all other <see cref="T:Terraria.PopupText" />s.
	/// </summary>
	Advanced
}
