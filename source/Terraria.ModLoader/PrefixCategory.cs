namespace Terraria.ModLoader;

public enum PrefixCategory
{
	/// <summary>
	/// Can modify the size of the weapon
	/// </summary>
	Melee,
	/// <summary>
	/// Can modify the shoot speed of the weapon
	/// </summary>
	Ranged,
	/// <summary>
	/// Can modify the mana usage of the weapon
	/// </summary>
	Magic,
	AnyWeapon,
	Accessory,
	/// <summary>
	/// Will not appear by default. Useful as prefixes for your own damage type.
	/// </summary>
	Custom
}
